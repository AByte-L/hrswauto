using Gy.HrswAuto.DataMold;
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
        /// <summary>
        /// 服务器端IP和端口配置
        /// </summary>
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
        //public event EventHandler<FeedRequestArg> OnPlaceAndGripRequestEvent;
        //public event EventHandler<FeedRequestArg> OnGripRequestEvent;
        //public event EventHandler<FeedRequestArg> OnPlaceRequestEvent;
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
                _IsInitialed = _cmmCtrl.IsInitialed(); // 返回服务器端是否初始化
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
            PathConfig pathConfig = PathManager.Instance.Configration;
            // etc root\Progs\1.prg 
            string filePath = Path.Combine(pathConfig.RootPath, pathConfig.ProgFilePath, partConfig.ProgFileName);
            // etc root\blades\partId
            string bladeFilePath = Path.Combine(pathConfig.RootPath, pathConfig.BladeFilePath, $"{partConfig.PartID}");
            if (!File.Exists(filePath)) return false;
            filePath = Path.Combine(bladeFilePath, partConfig.FlvFileName);
            if (!File.Exists(filePath)) return false;
            filePath = Path.Combine(bladeFilePath, partConfig.NormFileName);
            if (!File.Exists(filePath)) return false;
            filePath = Path.Combine(bladeFilePath, partConfig.TolFileName);
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
            //if (!FindPart(partId))
            //{
            //    // 提示未发现工件
            //    return;
            //}

            bool ok = SetServerPartConfig(partId);

            if (!ok)
            {
                Debug.WriteLine("文件部署失败，请检查");
                //State = State | ClientState.Error;
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
            //if (!VerifyLocalFiles(partId))
            //{
            //    Debug.WriteLine("文件缺失");
            //    return false;
            //}
            PartConfig partConfig = PartConfigManager.Instance.GetPartConfig(partId);
            PathConfig pathConfig = PathManager.Instance.Configration;
            // 上传程序文件 pcdmis programs目录中的文件，包括.prg .cad等 
            // 目录结构 root\programs\...
            string ProgFilesPath = Path.Combine(PathManager.Instance.Configration.RootPath, PathManager.Instance.Configration.ProgFilePath);
            if (!Directory.Exists(ProgFilesPath))
            {
                Debug.WriteLine("文件目录错误");
                return false;
            }
            string[] pfileNames = Directory.GetFiles(ProgFilesPath);
            string progFileWithoutExt = Path.GetFileNameWithoutExtension(partConfig.ProgFileName);
            bool IsSuccessUpLoad = false;
            // 程序文件和CAD文件集中在一个目录里，可以公用
            string filePath = PathManager.Instance.Configration.ProgFilePath;
            foreach (string item in pfileNames)
            {
                // 上传progs目录中的所有相关文件
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(item);
                if (string.Compare(progFileWithoutExt, fileNameWithoutExt,true) == 0)
                {
                    IsSuccessUpLoad = UpFileToServer1(partConfig.PartID, item, 0/*filePath*/);
                    if (!IsSuccessUpLoad)
                    {
                        return false;
                    }
                }
            }
            // 上传Blades文件 
            // root\blades\partid\...
            string bladeFilesPath = Path.Combine(PathManager.Instance.Configration.RootPath, PathManager.Instance.Configration.BladeFilePath, partConfig.PartID);
            if (!Directory.Exists(bladeFilesPath))
            {
                Debug.WriteLine("文件目录错误");
                return false;
            }
            string[] bfileNames = Directory.GetFiles(bladeFilesPath);
            //string bladeFileWithoutExt = Path.GetFileNameWithoutExtension(partConfig.ProgFileName);
            IsSuccessUpLoad = false;
            // blades文件进行工件标识分类
            filePath = Path.Combine(PathManager.Instance.Configration.BladeFilePath, partConfig.PartID);
            foreach (string item in bfileNames)
            { 
                // 上传progs目录中的所有相关文件
                IsSuccessUpLoad = UpFileToServer1(partConfig.PartID, item, 1/*filePath*/); // 放入服务器的blades文件目录中
                if (!IsSuccessUpLoad)
                {
                    return false;
                }
            }

            // 上传完毕添加工件配置
            _partConfigService.AddPartConfig(partConfig);
            return true;
            //string filePath = Path.Combine(pathConfig.RootPath, pathConfig.ProgFilePath, partConfig.ProgFileName);
            //bool ok = UpFileToServer(partId, filePath);
            //if (!ok) return false;
            //string bladeFilePath = Path.Combine(pathConfig.RootPath, pathConfig.BladeFilePath, $"{partConfig.PartID}");
            //// 上传算法Flv文件
            //filePath = Path.Combine(bladeFilePath, partConfig.FlvFileName);
            //ok = UpFileToServer(partId, filePath);
            //if (!ok) return false;
            //// 上传理论nom文件
            //filePath = Path.Combine(bladeFilePath, partConfig.NormFileName);
            //ok = UpFileToServer(partId, filePath);
            //if (!ok) return false;
            //// 上传公差文件
            //filePath = Path.Combine(bladeFilePath, partConfig.TolFileName);
            //ok = UpFileToServer(partId, filePath);
            //if (!ok) return false;
            //return true;
        }

        /// <summary>
        /// 上传单个文件到服务器
        /// </summary>
        /// <param name="partId">工件标识</param>
        /// <param name="filefullPath">文件全路径名</param>
        /// <param name="filePath">相对路径，baldes\partId或者programs</param>
        /// <returns></returns>
        private bool UpFileToServer(string partId, string filefullPath, string filePath)
        {
            using (Stream fs = new FileStream(filefullPath, FileMode.Open, FileAccess.Read))
            {
                UpFile fileData = new UpFile();
                fileData.FileName = Path.GetFileName(filefullPath);
                fileData.FileSize = fs.Length;
                fileData.FilePath = filePath;
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

        /// <summary>
        /// 上传单个文件到服务器
        /// </summary>
        /// <param name="partId">工件标识</param>
        /// <param name="filefullPath">文件全路径名</param>
        /// <param name="selPath">相对路径，baldes\partId或者programs</param>
        /// <returns></returns>
        private bool UpFileToServer1(string partId, string filefullPath, int selPath)
        {
            using (Stream fs = new FileStream(filefullPath, FileMode.Open, FileAccess.Read))
            {
                UpFile1 fileData = new UpFile1();
                fileData.FileName = Path.GetFileName(filefullPath);
                fileData.FileSize = fs.Length;
                fileData.selPath = selPath;
                fileData.FileStream = fs;
                fileData.PartId = partId;
                UpFileResult ures = _partConfigService.UpLoadFile1(fileData);
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
        /// 发送上料请求
        /// </summary>
        public void SendPlaceRequest()
        {
            //FeedRequestArg rarg = new FeedRequestArg();
            //rarg.ClientID = CmmSvrConfig.ServerID;
            //rarg.RqtType = RequestType.Request_Place;
            //OnPlaceRequestEvent?.Invoke(this, rarg);
            PlaceFeedRequest placeFeedRequest = new PlaceFeedRequest();
            placeFeedRequest.ClientID = CmmSvrConfig.ServerID;
            placeFeedRequest.PlcCompletedEvent += OnPlaceActionCompletedEvent;
            placeFeedRequest.Perform();
        }

        /// <summary>
        /// 上料完成事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPlaceActionCompletedEvent(object sender, ResponsePlcEventArgs e)
        {
            // 工件标识错误，发送工件未找到错误信号
            if (FindPart(e.PartID))
            {
                SendPartIDErrorSign(e.ClientID);
                Debug.WriteLine("报错处理");
                State = State | ClientState.Error; // 设置客户端为错误状态
                return;
            }

            StartMeasureWorkFlow(e.PartID);
        }

        /// <summary>
        /// 发送抓取下料请求
        /// </summary>
        /// <param name="isPassed">当前测量工件是否合格</param>
        public void SendGripRequest(bool isPassed)
        {
            //FeedRequestArg rarg = new FeedRequestArg();
            //rarg.ClientID = CmmSvrConfig.ServerID;
            //rarg.RqtType = RequestType.Request_Grip;
            //rarg.IsPassed = isPassed;
            //OnGripRequestEvent?.Invoke(this, rarg);
            GripFeedRequest gripFeedRequest = new GripFeedRequest();
            gripFeedRequest.ClientID = CmmSvrConfig.ServerID;
            gripFeedRequest.IsPassed = isPassed;
            gripFeedRequest.PlcCompletedEvent += OnGripActionCompletedEvent;
            gripFeedRequest.Perform(); // 执行请求
        }

        /// <summary>
        /// 下料抓取完成事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGripActionCompletedEvent(object sender, ResponsePlcEventArgs e)
        {
            if (State.HasFlag(ClientState.Continue)) // 如果客户状态是继续测量则发送上料信号
            {
                SendPlaceRequest();
            }
        }

        /// <summary>
        /// 发送下料并上料请求
        /// </summary>
        /// <param name="isPassed">当前工件是否合格</param>
        public void SendPlaceAndGripRequest(bool isPassed)
        {
            PlaceAndGripFeedRequest placeAndGripFeedRequest = new PlaceAndGripFeedRequest();
            placeAndGripFeedRequest.ClientID = CmmSvrConfig.ServerID;
            placeAndGripFeedRequest.IsPass = isPassed;
            placeAndGripFeedRequest.PlcCompletedEvent += OnPlaceActionCompletedEvent;
        }

        /// <summary>
        /// 发送工件标识错误信号
        /// </summary>
        /// <param name="partID"></param>
        public void SendPartIDErrorSign(int clientId)
        {
            //PlcPartIDErrorResponse(partID);
            ErrorFeedBack partIdErr = new ErrorFeedBack();
            partIdErr.ClientID = clientId;
            partIdErr.Perform();
        }
        #endregion

    }
}
