using Gy.HrswAuto.BladeMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.ErrorMod;
using Gy.HrswAuto.ICmmServer;
using Gy.HrswAuto.UICommonTools;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Gy.HrswAuto.CmmServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class MeasureServiceContext : ICmmControl, IDisposable
    {
        /// <summary>
        /// 事件回调代理接口
        /// </summary>
        IWorkflowNotify _eventNotify;

        private PCDmisService _pcdmisCore; // PCDmis测量核心
        private BladeMeasAssist _bladeMeasAssist; // 工件测量辅助
        private BladeContext _bladeContext; // blade分析
        public bool IsBladeMeasure { get; set; } = true;
        private PartConfig _part;
        private bool _serverInError;
        private bool _isConnected;

        private DateTime _dateTime;
        private System.Timers.Timer _timer;

        public bool ServerInError
        {
            get { return _serverInError; }
            set { _serverInError = value; }
        }

        public PartConfig CurPart
        {
            get { return _part; }
            private set { _part = value; }
        }


        #region 本地功能方法
        public MeasureServiceContext(double pcTimeout, double bdTimeout)
        {
            _pcdmisCore = new PCDmisService(pcTimeout/* 测量超时时间 */); // 
            _bladeMeasAssist = new BladeMeasAssist();
            _bladeContext = new BladeContext(bdTimeout);
            _serverInError = false;

            // 心跳信号
            _isConnected = false;
            _dateTime = DateTime.Now;
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += ListenHeartbeat;
            _timer.Start();
            //_pcdmisMonitorTimer = new Timer(3000); // PCDMIS监控定时器
            //_pcdmisMonitorTimer.Elapsed += _pcdmisMonitorTimer_Elapsed;
        }

        public void SetMeasureDuration(double pcTimeout, double bdTimeout)
        {
            _pcdmisCore.SetTimeout(pcTimeout);
            _bladeContext.SetTimeout(bdTimeout);
        }

        public bool Initialize()
        {
            try
            {
                _pcdmisCore.InitialPCDmis();
                //_pcdmisCore.PCDmisMeasureEvent += _pcdmisCore_PCDmisMeasureEvent;
                _pcdmisCore.PCDmisMeasureEvent += _pcdmisCore_PCDmisMeasureEvent;
            }
            catch (Exception)
            {
                LogCollector.Instance.PostSvrErrorMessage("PCDmis未能初始化");
                _serverInError = true;
            }
            return _pcdmisCore._IsInitialed;
        }

        #region 调试方法
        // 
        private void _pcdmisCore_PCDmisMeasureEvent1(object sender, PCDmisEventArgs e)
        {
            if (!e.IsCompleted)
            {
                LogCollector.Instance.PostSvrErrorMessage("PCDMIS没有完成执行或执行出错");
                //ServerUILinker.WriteUILog(e.PCDmisRunInfo);
                if (e.FaultType == PCDmisFaultType.FT_FatalError)
                {
                    ReinitialPCDmist();
                }
                _serverInError = true;
                return;
            }
            LocalLogCollector.WriteMessage("PCDMIS测量完成");
            _eventNotify?.WorkCompleted(true);
        }

        public void SetBladeMeasAssist(BladeMeasAssist bma)
        {
            _bladeMeasAssist = bma;
        }
        #endregion

        /// <summary>
        /// 通过工件标识获得零件检测程序
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        private string FindProgFile(string partId)
        {
            // 从工件配置管理器中获得工件列表
            PartConfig pc = PartConfigManager.Instance.GetPartConfig(partId);
            if (pc == null)
            {
                return string.Empty;
            }
            string filePath = PathManager.Instance.GetPartProgramPath(pc);
            //Debug.Assert(string.IsNullOrEmpty(filePath)); // 正常情况FilePath不是空字符串
            return filePath;
        }


        /// <summary>
        /// PCDMIS测量完成事件, 在这里异步执行PCDMIS分析过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void _pcdmisCore_PCDmisMeasureEvent(object sender, PCDmisEventArgs e)
        {
            if (!e.IsCompleted)
            {
                LogCollector.Instance.PostSvrErrorMessage("PCDMIS没有完成执行或执行出错");
                if (e.FaultType == PCDmisFaultType.FT_FatalError)
                {
                    ReinitialPCDmist();
                }
                _serverInError = true;
                return;
            }
            LogCollector.Instance.PostSvrWorkStatus("PCDMIS执行完成");
            if (!IsBladeMeasure)
            {
                return;
            }
            // 开启Blade异步分析
            string bladeExe = ServerSettings.BladeExe;
            bool ok = await Task.Run(() =>
            {
                bool result = _bladeMeasAssist.PCDmisRtfToBladeRpt(); // 转换rtf到rpt文件
                if (!result)
                {
                    _serverInError = true;
                    return result;
                }
                PathManager.Instance.RptFilePath = _bladeMeasAssist.RptFileName; // rptfilename 全路径
                return _bladeContext.StartBlade(bladeExe, _bladeMeasAssist.RptFileName);
            });
            if (ok)
            {
                LogCollector.Instance.PostSvrWorkStatus("Blade分析完成");
                // 执行结果分析, 分析CMM文件
                PathManager.Instance.ReportsPath = _bladeContext.CMMFileFullPath;
                bool measResult = _bladeMeasAssist.VerifyAnalysisResult(_bladeContext.CMMFileFullPath);
                // 如果客户端断开，跳出异常
                try
                {
                    _eventNotify?.WorkCompleted(measResult); // 通知客户端测量结果是否合格
                    LogCollector.Instance.PostSvrWorkStatus("检测流完成");
                }
                catch (Exception)
                {
                    LocalLogCollector.WriteMessage("与客户端连接异常");
                }
            }
            else
            {
                LogCollector.Instance.PostSvrErrorMessage("Blade分析失败");
                _serverInError = true;
            }
        }

        public void ClearServerError()
        {
            try
            {
                _serverInError = false;
                _eventNotify?.ClearCmmServerError();
                LogCollector.Instance.PostSvrWorkStatus("三坐标服务器重置");
                //ServerUILinker.WriteUILog("清空错误");
            }
            catch (Exception)
            {
                LocalLogCollector.WriteMessage("与客户端连接异常");
            }
        }

        public void ReinitialPCDmist()
        {
            Initialize();
        }

        public void BackToSafePosition(string gotoPrg)
        {
            IsBladeMeasure = false;
            try
            {
                if (_pcdmisCore._IsInitialed)
                {
                    _pcdmisCore.OpenPartProgram(ServerSettings.GotoSafePrg);
                    if (!_pcdmisCore.ExecutePartProgram())
                    {
                        LogCollector.Instance.PostSvrErrorMessage("pcdmis没有启动执行回安全位置程序");
                        return;
                    }
                    else
                    {
                        LogCollector.Instance.PostSvrWorkStatus("PCDMIS执行回安全位置程序");
                    }
                }
            }
            catch (Exception ex)
            {
                LogCollector.Instance.PostSvrErrorMessage("pcdmis出现异常 " + ex.Message);
            }
            //finally
            //{
            //    IsBladeMeasure = true;
            //}
        }
        #endregion

        #region 通信接口实现
        public void ConnectWFEvents()
        {
            try
            {
                _eventNotify = OperationContext.Current.GetCallbackChannel<IWorkflowNotify>();
                LogCollector.Instance.SvrNotify = _eventNotify;
                LocalLogCollector.WriteMessage("控制中心已连接");
                // 心跳信号
                _isConnected = true;
                _dateTime = DateTime.Now;

            }
            catch (Exception ex)
            {
                LocalLogCollector.WriteMessage("注册回调事件异常 " + ex.Message);
            }
        }

        public void DisconnectWFEvents()
        {
            throw new NotImplementedException();
        }

        public bool IsInitialed()
        {
            return _pcdmisCore._IsInitialed;
        }

        public void MeasurePart1(string partId)
        {
            IsBladeMeasure = true;
            _part = PartConfigManager.Instance.GetPartConfig(partId);
            Debug.Assert(_part != null);
            string partProgFileName = PathManager.Instance.GetPartProgramPath(_part);
            if (!File.Exists(partProgFileName))
            {
                LogCollector.Instance.PostSvrErrorMessage("程序文件不存在");
                return;
            }
            LocalLogCollector.WriteMessage($"测量工件: {partId}, 程序:{partProgFileName}");
            try
            {
                _pcdmisCore.OpenPartProgram(partProgFileName);
                Thread.Sleep(100); // 等待开启测量程序
                                   //if (IsBladeMeasure)
                                   //{
                _pcdmisCore.GetProgramCommandParameters(); // 获得测尖直径和输出文件
                if (!_pcdmisCore.HasOutputFile)
                {
                    //ServerUILinker.WriteUILog("测量程序无输出，无法进行叶片计算");
                    LogCollector.Instance.PostSvrErrorMessage("测量程序无输出，无法进行叶片计算");
                    return;
                }
                // 
                _bladeMeasAssist.Part = _part;
                _bladeMeasAssist.ProbeDiam = _pcdmisCore.ProbeDiam;
                _bladeMeasAssist.RtfFileName = _pcdmisCore.RtfFileName;
                // 创建Blade.txt文件
                if (!_bladeMeasAssist.CreateBladeTxtFromNominal())
                {
                    LogCollector.Instance.PostSvrErrorMessage("辅助文件创建失败");
                    return;
                }
                //}
                //if (_pcdmisCore.HasOutputFile) // 如果程序找到输出文件，则设置blade测量辅助
                //{
                //    _bladeMeasAssist.Part = _part;
                //    _bladeMeasAssist.ProbeDiam = _pcdmisCore.ProbeDiam;
                //    _bladeMeasAssist.RtfFileName = _pcdmisCore.RtfFileName;
                //    if (IsBladeMeasure) // 
                //    {
                //        _bladeMeasAssist.CreateBladeTxtFromNominal();
                //    }
                //}
                if (!_pcdmisCore.ExecutePartProgram()) // 执行程序
                {
                    LogCollector.Instance.PostSvrErrorMessage("pcdmis没有启动执行");
                    return;
                }
                else
                {
                    LogCollector.Instance.PostSvrWorkStatus("pcdmis启动执行 " + _part.ProgFileName);
                }
            }
            catch (Exception)
            {
                LogCollector.Instance.PostSvrErrorMessage("PCDmis出错, 重启PCDmis");
                //ServerUILinker.WriteUILog("PCDmis出错, 重启PCDmis");
                //处理PCDMIS的CrashSender1402.exe窗口
                bool result = CloseCrashSender();
                ReinitialPCDmist();
                //throw;
            }
        }

        #region 测试方法 
        public void MeasurePart(string partId) // todo MeasureServiceContext 测试方法
        {
            _part = PartConfigManager.Instance.GetPartConfig(partId);
            Debug.Assert(_part != null);
            string partProgFileName = PathManager.Instance.GetPartProgramPath(_part);
            if (!File.Exists(partProgFileName))
            {
                LogCollector.Instance.PostSvrErrorMessage("程序文件不存在");
                return;
            }
            LocalLogCollector.WriteMessage($"测量工件: {partId}, 程序:{partProgFileName}");
            BackToSafePosition(partProgFileName);
        } 
        #endregion

        // 关闭PCDmis的错误对话框
        private bool CloseCrashSender()
        {
            bool result = false;
            Process[] processes = Process.GetProcessesByName("CrashSender1402");
            foreach (var item in processes)
            {
                result = item.CloseMainWindow();
                if (!result)
                {
                    item.Kill();
                    result = item.WaitForExit((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
                }
            }
            return result;
        }

        public void Dispose()
        {
            //停止守护线程
            //_pcdmisMonitorTimer.Dispose();
            _pcdmisCore.Dispose();
        }
        private void ListenHeartbeat(object sender, ElapsedEventArgs e)
        {
            if (_dateTime.AddSeconds(15) < DateTime.Now)
            {
                if (_isConnected)
                {
                    LocalLogCollector.WriteMessage("控制中心已断开");
                    _isConnected = false;
                }
            }
        }

        public bool QueryCmmServer()
        {
            _dateTime = DateTime.Now;
            return true;
        }

        #endregion
    }
}
