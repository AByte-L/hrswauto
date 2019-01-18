using Gy.HrswAuto.BladeMold;
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
        public bool IsBladeMeasure { get; set; }
        private PartConfig _part;
        public PartConfig CurPart
        {
            get { return _part; }
            private set { _part = value; }
        }

        //Timer _pcdmisMonitorTimer;

        #region 本地功能方法
        public MeasureServiceContext()
        {
            _pcdmisCore = new PCDmisService(10/* 测量超时时间 */); // 
            _bladeMeasAssist = new BladeMeasAssist();
            _bladeContext = new BladeContext();
            //_pcdmisMonitorTimer = new Timer(3000); // PCDMIS监控定时器
            //_pcdmisMonitorTimer.Elapsed += _pcdmisMonitorTimer_Elapsed;
        }

        //private void _pcdmisMonitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    // 寻找PCDMIS进程，如果没有则重新初始化
        //    Process[] pcs = Process.GetProcessesByName("PCDLRN");
        //    if (pcs.Length > 0)
        //    {
        //        Debug.WriteLine("PCDmis正常运行");
        //        return;
        //    }
        //    // todo PCDmis异常, 通知客户端然后重新启动初始化PCDMIS
            
        //    ReinitialPCDmist();
        //}

        public bool Initialize()
        {
            //_pcdmisMonitorTimer.Start();
            try
            {
                _pcdmisCore.InitialPCDmis();
                //_pcdmisCore.PCDmisMeasureEvent += _pcdmisCore_PCDmisMeasureEvent;
                _pcdmisCore.PCDmisMeasureEvent += _pcdmisCore_PCDmisMeasureEvent1;
            }
            catch (Exception)
            {
                Debug.WriteLine("初始化PCDmis失败");
            }
            return _pcdmisCore._IsInitialed;
        }

        #region 调试用方法
        // 
        private void _pcdmisCore_PCDmisMeasureEvent1(object sender, PCDmisEventArgs e)
        {
            if (!e.IsCompleted)
            {
                Debug.WriteLine("PCDMIS没有完成执行或执行出错");
                if (e.FaultType == PCDmisFaultType.FT_FatalError)
                {
                    ReinitialPCDmist();
                    // 
                }
                return;
            }
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
                Debug.WriteLine("PCDMIS没有完成执行或执行出错");
                if (e.FaultType == PCDmisFaultType.FT_FatalError)
                {
                    // 异常错误需要重新初始化PCDMIS
                    ReinitialPCDmist();
                    // todo 通知客户端
                }
                return;
            }
            // 开启Blade异步分析
            string bladeExe = @"C:\Program Files (x86)\Hexagon\PC-DMIS Blade 5.0 (Release)\Blade.exe";
            bool ok = await Task.Run(() =>
            {
                _bladeMeasAssist.PCDmisRtfToBladeRpt(); // 转换rtf到rpt文件
                return _bladeContext.StartBlade(bladeExe, _bladeMeasAssist.RptFileName);
            });
            if (ok)
            {
                // todo 执行结果分析, 分析CMM文件
                bool measResult = _bladeMeasAssist.VerifyAnalysisResult(_bladeContext.CMMFileFullPath);
                _eventNotify?.WorkCompleted(measResult); // 通知客户端测量结果是否合格
            }
        }

        public void ReinitialPCDmist()
        {
                Initialize();
        }
        #endregion

        #region 通信接口实现
        public void ConnectWFEvents()
        {
            _eventNotify = OperationContext.Current.GetCallbackChannel<IWorkflowNotify>();
        }

        public void DisconnectWFEvents()
        {
            throw new NotImplementedException();
        }

        public bool IsInitialed()
        {
            return _pcdmisCore._IsInitialed;
        }

        public void MeasurePart(string partId)
        {
            _part = PartConfigManager.Instance.GetPartConfig(partId);
            Debug.Assert(_part != null);
            string partProgFileName = PathManager.Instance.GetPartProgramPath(_part);
            if (!File.Exists(partProgFileName))
            {
                Debug.WriteLine("程序文件不存在");
                return;
            }
            try
            {
                _pcdmisCore.OpenPartProgram(partProgFileName);
                _pcdmisCore.GetProgramCommandParameters(); // 获得测尖直径和输出文件
                if (_pcdmisCore.HasOutputFile) // 如果程序找到输出文件，则设置blade测量辅助
                {
                    _bladeMeasAssist.Part = _part;
                    _bladeMeasAssist.ProbeDiam = _pcdmisCore.ProbeDiam;
                    _bladeMeasAssist.RtfFileName = _pcdmisCore.RtfFileName;
                    // 创建Blade.txt文件
                    if (IsBladeMeasure) // 
                    {
                        _bladeMeasAssist.CreateBladeTxtFromNominal();
                    }
                }
                if (!_pcdmisCore.ExecutePartProgram()) // 执行程序
                {
                    Debug.WriteLine("pcdmis没有启动执行");
                    return;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("PCDmis出错, 重启PCDmis");
                //处理PCDMIS的CrashSender1402.exe窗口
                bool result = CloseCrashSender();
                throw;
            }
        }

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
        #endregion
    }
}
