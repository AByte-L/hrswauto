using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.ErrorMod;
using Gy.HrswAuto.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.CmmServer
{
    /// <summary>
    /// 处理PCDmis事务，内部方法会抛出异常，需要外部类接收异常并处理
    /// </summary>
    public class PCDmisService : IDisposable
    {
        //private MeasureServiceContext _bladeMeasureContext; // 辅助测量上下文

        private PCDLRN.Application _pcdApplication;
        private PCDLRN.PartPrograms _pcdProgramManager;
        private PCDLRN.PartProgram _partProgram;
        private PCDLRN.ApplicationObjectEvents _pcdAppEvents;

        private bool _IsOpened = false;
        private bool _ExeOK = false;
        private System.Timers.Timer _monitorTimer;
        private DateTime _timerStart;
        private TimeSpan _timeout;

        public bool _IsInitialed { get; private set; } = false;
        public string RtfFileName { get; private set; }
        public double ProbeDiam { get; private set; }
        public bool HasOutputFile { get; private set; } // 程序是否有输出文件

        public event EventHandler<PCDmisEventArgs> PCDmisMeasureEvent;

        public PCDmisService(/*MeasureServiceContext bmc*/double timeout = 3)
        {
            //_bladeMeasureContext = bmc;
            _monitorTimer = new System.Timers.Timer(5000);
            _monitorTimer.Elapsed += _monitorTimer_Elapsed;
            _timeout = timeout > 3 ? TimeSpan.FromMinutes(timeout) : TimeSpan.FromMinutes(3); // 
        }

        /// <summary>
        /// 初始化PCDmis应用，并设置APPEVENT事件
        /// </summary>
        public void InitialPCDmis()
        {
            ClosePCDmis();
            //try
            //{
            _pcdApplication = null;
            _pcdProgramManager = null;
            _pcdAppEvents = null;
            Type t = Type.GetTypeFromProgID("PCDLRN.Application");
            SetPCDmisOffline(t); //是否以离线方式启动PCDMIS
            _pcdApplication = (PCDLRN.Application)Activator.CreateInstance(t);
            _pcdApplication.UserExit = true; // 用户无法手动退出PCDMIS
            _IsInitialed = _pcdApplication.WaitUntilReady((int)_timeout.TotalSeconds); // 等待初始化完成
            //Thread.Sleep(5000); // 等待PCDmils创建完成
            _pcdApplication.Visible = true;
            _pcdProgramManager = _pcdApplication.PartPrograms;
            _pcdAppEvents = _pcdApplication.ApplicationEvents;
            _pcdAppEvents.OnCloseExecutionDialog += _pcdAppEvents_OnCloseExecutionDialog;
            _pcdAppEvents.OnClosePartProgram += _pcdAppEvents_OnClosePartProgram;
            _pcdAppEvents.OnSavePartProgram += _pcdAppEvents_OnSavePartProgram;
            _pcdApplication.SetActive();
            _IsInitialed = true;
            //}
            //catch (Exception ex)
            //{
            //    // 启动失败的处理方式
            //    throw ex;
            //}
        }

        public void SetTimeout(double pcTimeout)
        {
            _timeout = pcTimeout > 3 ? TimeSpan.FromMinutes(pcTimeout) : TimeSpan.FromMinutes(3);
        }

        #region 测试PCDmis应用事件
        private void _pcdAppEvents_OnSavePartProgram(PCDLRN.PartProgram PartProg)
        {
            Debug.WriteLine("保存程序");
        }

        private void _pcdAppEvents_OnClosePartProgram(PCDLRN.PartProgram PartProg)
        {
            Debug.WriteLine("关闭程序");
        }
        #endregion


        /// <summary>
        /// 打开PCDMIS测量程序
        /// </summary>
        /// <param name="prgFile">测量程序全路径名</param>
        public void OpenPartProgram(string prgFile)
        {
            _pcdProgramManager.CloseAll();
            while (_pcdProgramManager.Count != 0) ; // 等待程序关闭
            _partProgram = null;
            _ExeOK = false;
            //_pcdApplication.SetActive();
            //_pcdApplication.Maximize();
            _partProgram = _pcdProgramManager.Open(prgFile, _pcdApplication.DefaultMachineName/*"CMM1"*/);
            _IsOpened = true;
        }

        /// <summary>
        /// 获取程序中Blade软件需要的测尖和输出rtf文件
        /// </summary>
        public void GetProgramCommandParameters()
        {
            if (_IsOpened)
            {
                //  获取程序中测尖直径
                PCDLRN.OldBasic ob = _partProgram.OldBasic;
                ProbeDiam = 2 * ob.GetProbeRadius();
                // 获取第一个输出文件名
                HasOutputFile = FindOutputFileName();
            }
        }

        /// <summary>
        /// 执行零件检测程序
        /// </summary>
        public bool ExecutePartProgram()
        {
            if (_IsOpened)
            {
                //设置程序事件响应函数
                _partProgram.OnExecuteDialogErrorMsg += _partProgram_OnExecuteDialogErrorMsg;
                // 开启监控线程，判断PCDMIS是否异常退出
                _monitorTimer.Start();
                _timerStart = DateTime.Now;
                // 异步执行程序
                _ExeOK = _partProgram.AsyncExecute();
            }
            return _ExeOK;
        }

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
        /// 设置以离线方式启动PCDMIS，调试时使用
        /// </summary>
        /// <param name="t"></param>
        private void SetPCDmisOffline(Type t)
        {
            string clsid = "{" + t.GUID.ToString() + "}";
            RegistryKey clsKey = Registry.ClassesRoot.OpenSubKey(@"CLSID\" + clsid);
            if (clsKey == null) // 处理64位PCDMIS
            {
                RegistryKey classRoot = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                clsKey = classRoot.OpenSubKey(@"CLSID\" + clsid);
                classRoot.Close();
            }
            string[] valueNames = clsKey.GetSubKeyNames();
            List<string> nalist = new List<string>(valueNames);
            string path = "";
            nalist.ForEach(exe =>
            {
                if (string.Compare(exe, "localserver32", true) == 0)
                {
                    RegistryKey pathKey = clsKey.OpenSubKey(exe);
                    path = (string)pathKey.GetValue(pathKey.GetValueNames()[0]);
                    path = path.TrimStart('\"').TrimEnd('\"');
                    path = Path.GetDirectoryName(path);
                }
            });
            if (path != "")
            {
                string startFile = Path.Combine(path, "AutomationStartupOptions.txt");
                File.WriteAllText(startFile, "/f");
            }
        }

        /// <summary>
        /// 关闭PCDmis进程
        /// </summary>
        /// <returns></returns>
        private bool ClosePCDmis()
        {
            bool result = false;
            Process[] processes = Process.GetProcessesByName("PCDLRN");
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

        /// <summary>
        /// 获取第一个输出文件名
        /// </summary>
        /// <returns>是否找到RTF文件</returns>
        private bool FindOutputFileName()
        {
            int count = 1;
            bool result = false;

            for (int i = 1; i <= _partProgram.Commands.Count; i++)
            {
                PCDLRN.Command cmd = _partProgram.Commands.Item(count++);
                if (cmd.Type == PCDLRN.OBTYPE.PRINT_REPORT)
                {
                    RtfFileName = cmd.GetText(PCDLRN.ENUM_FIELD_TYPES.FILE_NAME, 1);
                    if (!RtfFileName.Contains(".RTF"))
                    {
                        RtfFileName = RtfFileName + ".RTF";
                    }
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// PCDmis执行监控事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _monitorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _monitorTimer.Stop();
            Process[] pcs = Process.GetProcessesByName("PCDLRN");
            TimeSpan exeTime = e.SignalTime - _timerStart;
            if (pcs.Length > 0)
            {
                if (exeTime > _timeout)
                {
                    PCDmisEventArgs pce = new PCDmisEventArgs();
                    pce.IsCompleted = false;
                    pce.FaultType = PCDmisFaultType.FT_Timeout;
                    pce.PCDmisRunInfo = "PCDmis执行超时";
                    PCDmisMeasureEvent?.Invoke(this, pce);
                    //LogCollector.Instance.PostSvrErrorMessage("PCDmis执行超时");
                    _monitorTimer.Close();
                }
            }
            else if (_ExeOK) // 已经开始异步执行
            {
                PCDmisEventArgs pce = new PCDmisEventArgs();
                pce.IsCompleted = false;
                pce.FaultType = PCDmisFaultType.FT_FatalError;
                pce.PCDmisRunInfo = "PCDmis在执行时异常退出";
                PCDmisMeasureEvent?.Invoke(this, pce);
                //LogCollector.Instance.PostSvrErrorMessage("PCDMIS异常跳出");
                _monitorTimer.Close();
            }
            else
            {
                // 外部异常捕获
            }
            _monitorTimer.Start();
        }

        /// <summary>
        /// PCDmis测量完成响应事件
        /// </summary>
        /// <param name="ExecutionWindow"></param>
        private void _pcdAppEvents_OnCloseExecutionDialog(PCDLRN.ExecutionWindow ExecutionWindow)
        {
            _ExeOK = false;
            PCDmisEventArgs pce;
            _monitorTimer.Stop();
            if (_partProgram.ExecutionWasCancelled)
            {
                LogCollector.Instance.PostSvrErrorMessage("执行被终止");
                pce = new PCDmisEventArgs() { IsCompleted = false };
                pce.PCDmisRunInfo = "执行被终止";
                pce.FaultType = PCDmisFaultType.FT_CancelMeasure;
                PCDmisMeasureEvent?.Invoke(this, pce);
                return;
            }
            // 响应PCDMIS测量结束事件
            pce = new PCDmisEventArgs() { IsCompleted = true };
            pce.PCDmisRunInfo = "测量完成";
            pce.FaultType = PCDmisFaultType.FT_None;
            PCDmisMeasureEvent?.Invoke(this, pce);
            LogCollector.Instance.PostSvrWorkStatus("测量结束");
        }

        /// <summary>
        /// 测量中出错
        /// </summary>
        /// <param name="ErrorMsg"></param>
        private void _partProgram_OnExecuteDialogErrorMsg(string ErrorMsg)
        {
            //_monitorTimer.Stop();
            LogCollector.Instance.PostSvrErrorMessage(ErrorMsg);
            PCDmisEventArgs peArgs = new PCDmisEventArgs() { IsCompleted = false };
            peArgs.PCDmisRunInfo = ErrorMsg; // 出错信息
            peArgs.FaultType = PCDmisFaultType.FT_MeasureError;
            PCDmisMeasureEvent?.Invoke(this, peArgs);
        }

        public void Dispose()
        {
            try
            {
                if (_pcdApplication != null)
                {
                    _pcdApplication.Quit(); // PCDMIS异常退出
                }
            }
            catch(Exception)
            {
                // 不做处理
            }
            finally
            {
                _pcdApplication = null;
                _monitorTimer.Dispose();
            }
        }
    }

    public class PCDmisEventArgs : EventArgs
    {
        // 测量是否正常完成
        public bool IsCompleted { get; set; }
        public string PCDmisRunInfo { get; set; }
        public PCDmisFaultType FaultType { get; set; }
    }

    public enum PCDmisFaultType
    {
        FT_FatalError, // 异常跳出，需要重新初始化
        FT_MeasureError, // 测量过程中产生的错误
        FT_Timeout, // 测量超时
        FT_CancelMeasure, // 人为取消测量
        FT_None
    }
}
