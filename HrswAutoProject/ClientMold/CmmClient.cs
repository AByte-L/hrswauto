﻿using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.ICmmServer;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ClientMold
{
    public class CmmClient
    {
        #region 数据成员属性
        public CmmServerConfig CmmSvrConfig { get; set; }
        // 当前检测ID
        public string CurPartId { get; set; } 
        public ClientState State { get; set; }
        private ProxyFactory _proxyFactory; // 
        // 代理通道
        ICmmControl _cmmCtrl;
        IPartConfigService _partConfigService;
        private bool _IsInitialed = false;

        public bool IsInitialed
        {
            get { return _IsInitialed; }
            private set { _IsInitialed = value; }
        }

        public event EventHandler<FeedRequestArg> OnGripRequestEvent;
        public event EventHandler<FeedRequestArg> OnPlaceRequestEvent;
        #endregion

        #region 初始化
        /// <summary>
        /// </summary>
        /// <param name="cmmSvrConfig">服务器配置，包括IP地址，端口号等</param>
        public CmmClient(CmmServerConfig cmmSvrConfig)
        {
            CmmSvrConfig = cmmSvrConfig;
        }

        public void InitClient()
        {
            try
            {
                _proxyFactory = new ProxyFactory(this);
                _cmmCtrl = _proxyFactory.GetCmmControl(CmmSvrConfig);
                _partConfigService = _proxyFactory.GetPartConfigService(CmmSvrConfig);
                _IsInitialed = true;
            }
            catch (Exception)
            {
                _IsInitialed = false; // 初始化不成功
            }
        }
        #endregion

        #region 本地方法
        /// <summary>
        /// 确定是否存在工件ID，如果不存在工件
        /// 则向PLC设置未找到工件标示位
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public bool FindPart(string partId)
        {
            return PartConfigManager.Instance.Exists(partId);
        }

        /// <summary>
        /// 确认本地文件是否完整
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        private bool VerifyLocalFiles(string partId)
        {
            PartConfig partConfig = PartConfigManager.Instance.GetPartConfig(partId);
            PathConfig pathConfig = PathManager.Instance.AutoPathConfig;
            string filePath = Path.Combine(pathConfig.ProgFilePath, partConfig.ProgFileName);
            if (!File.Exists(filePath)) return false;
            filePath = Path.Combine(pathConfig.ProgFilePath, partConfig.FlvFileName);
            if (!File.Exists(filePath)) return false;
            filePath = Path.Combine(pathConfig.ProgFilePath, partConfig.NormFileName);
            if (!File.Exists(filePath)) return false;
            filePath = Path.Combine(pathConfig.ProgFilePath, partConfig.TolFileName);
            if (!File.Exists(filePath)) return false;
            return true;
        }

        #endregion

        #region Remote方法
        /// <summary>
        /// 启动cmm测量工作流
        /// </summary>
        /// <param name="partId"></param>
        public void StartMeasureWorkFlow(string partId)
        {
            CurPartId = partId;
            if (!FindPart(partId))
            {
                // 提示未发现工件
                return;
            }

            bool ok = SetServerPartConfig(partId);

            if (!ok)
            {
                Debug.WriteLine("文件部署失败，请查证");
                return;
            }

            if (!_cmmCtrl.IsInitialed())
            {
                Debug.WriteLine("PCDMIS未初始化");
                return;
            }

            // 启动测量
            _cmmCtrl.MeasurePart(partId);
        }

        /// <summary>
        /// 设置服务器工件配置
        /// </summary>
        /// <param name="partId">工件标识</param>
        /// <returns></returns>
        private bool SetServerPartConfig(string partId)
        {
            // 发现远端存在工件配置
            if (_partConfigService.FindPart(partId))
            {
                return true;
            }
            // 
            if (!VerifyLocalFiles(partId))
            {
                return false;
            }
            // 添加工件配置
            PartConfig partConfig = PartConfigManager.Instance.GetPartConfig(partId);
            _partConfigService.AddPartConfig(partConfig);
            PathConfig pathConfig = PathManager.Instance.AutoPathConfig;
            // 上传程序文件
            string filePath = Path.Combine(pathConfig.ProgFilePath, partConfig.ProgFileName);
            bool ok = UpFileToServer(partId, filePath);
            if (!ok) return false;
            // 上传算法Flv文件
            filePath = Path.Combine(pathConfig.ProgFilePath, partConfig.FlvFileName);
            ok = UpFileToServer(partId, filePath);
            if (!ok) return false;
            // 上传理论nom文件
            filePath = Path.Combine(pathConfig.ProgFilePath, partConfig.NormFileName);
            ok = UpFileToServer(partId, filePath);
            if (!ok) return false;
            // 上传公差文件
            filePath = Path.Combine(pathConfig.ProgFilePath, partConfig.TolFileName);
            ok = UpFileToServer(partId, filePath);
            if (!ok) return false;
            return true;
        }

        /// <summary>
        /// 上传单个文件到服务器
        /// </summary>
        /// <param name="partId">工件标示</param>
        /// <param name="filePath">上传文件路径</param>
        /// <returns></returns>
        private bool UpFileToServer(string partId, string filePath)
        {
            using (Stream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                UpFile fileData = new UpFile();
                fileData.FileName = Path.GetFileName(filePath);
                fileData.FileSize = fs.Length;
                fileData.FileStream = fs;
                fileData.PartId = partId;
                UpFileResult ures = _partConfigService.UpLoadFile(fileData);
                //if (ures.IsSuccess)
                //{
                //    Debug.WriteLine("File up ok");
                //}
                return ures.IsSuccess;
            }
        }
        #endregion

        #region PLC方法
        /// <summary>
        /// 设置上料请求
        /// </summary>
        public void SendPlaceRequest()
        {
            FeedRequestArg rarg = new FeedRequestArg();
            rarg.ClientID = CmmSvrConfig.ServerID;
            rarg.RqtType = RequestType.Request_Place;
            OnPlaceRequestEvent?.Invoke(this, rarg);
        }
        /// <summary>
        /// 设置下料请求
        /// </summary>
        public void SendGripRequest(bool isPassed)
        {
            FeedRequestArg rarg = new FeedRequestArg();
            rarg.ClientID = CmmSvrConfig.ServerID;
            rarg.RqtType = RequestType.Request_Grip;
            rarg.IsPassed = isPassed;
            OnGripRequestEvent?.Invoke(this, rarg);
        } 
        #endregion

    }
}
