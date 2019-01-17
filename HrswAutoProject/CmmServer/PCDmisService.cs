using Gy.HrswAuto.DataMold;
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
    public class PCDmisService : IDisposable
    {
        //private MeasureServiceContext _bladeMeasureContext; // 辅助测量上下文

        private PCDLRN.Application _pcdApplication;
        private PCDLRN.PartPrograms _pcdProgramManager;
        private PCDLRN.PartProgram _partProgram;
        private PCDLRN.ApplicationObjectEvents _pcdAppEvents;

        private bool _IsOpened = false;

        public bool _IsInitialed { get; private set; } = false;
        public string RtfFileName { get; private set; }
        public double ProbeDiam { get; private set; }
        public bool HasOutputFile { get; private set; } // 程序是否有输出文件

        public event EventHandler<PCDmisEventArgs> PCDmisMeasureEvent;

        public PCDmisService(/*MeasureServiceContext bmc*/)
        {
            //_bladeMeasureContext = bmc;
        }

        /// <summary>
        /// 初始化PCDmis应用，并设置APPEVENT事件
        /// </summary>
        public void InitialPCDmis()
        {
            ClosePCDmis();
            try
            {
                Type t = Type.GetTypeFromProgID("PCDLRN.Application");
                SetPCDmisOffline(t); //是否以离线方式启动PCDMIS
                _pcdApplication = (PCDLRN.Application)Activator.CreateInstance(t);
                //_pcdApplication.UserExit = false;
                Thread.Sleep(1000); // 等待PCDmils创建完成
                _pcdApplication.Visible = true;
                _IsInitialed = _pcdApplication.WaitUntilReady((int)TimeSpan.FromMinutes(1).TotalSeconds); // 等待初始化
                _pcdProgramManager = _pcdApplication.PartPrograms;
                _pcdAppEvents = _pcdApplication.ApplicationEvents;
                _pcdAppEvents.OnCloseExecutionDialog += _pcdAppEvents_OnCloseExecutionDialog;
                _pcdAppEvents.OnClosePartProgram += _pcdAppEvents_OnClosePartProgram;
                _pcdAppEvents.OnSavePartProgram += _pcdAppEvents_OnSavePartProgram;
                _pcdApplication.SetActive();
                _IsInitialed = true;
            }
            catch (Exception ex)
            {
                // 启动失败的处理方式
                throw ex;
            }
        }

        //public void RestartPCDmist()
        //{
        //    InitialPCDmis();
        //}

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
        /// PCDmis测量完成响应事件
        /// </summary>
        /// <param name="ExecutionWindow"></param>
        private void _pcdAppEvents_OnCloseExecutionDialog(PCDLRN.ExecutionWindow ExecutionWindow)
        {
            if (_partProgram.ExecutionWasCancelled)
            {
                Debug.WriteLine("执行被终止");
                return;
            }
            // 响应PCDMIS测量结束事件
            PCDmisEventArgs peArgs = new PCDmisEventArgs() { IsCompleted = true };
            PCDmisMeasureEvent?.Invoke(this, peArgs);
            Debug.WriteLine("测量结束");
        }

        /// <summary>
        /// 测量中出错
        /// </summary>
        /// <param name="ErrorMsg"></param>
        private void _partProgram_OnExecuteDialogErrorMsg(string ErrorMsg)
        {
            Debug.WriteLine(ErrorMsg);
            PCDmisEventArgs peArgs = new PCDmisEventArgs() { IsCompleted = false };
            PCDmisMeasureEvent?.Invoke(this, peArgs);
        }

        /// <summary>
        /// 打开PCDMIS测量程序
        /// </summary>
        /// <param name="prgFile">测量程序全路径名</param>
        public void  OpenPartProgram(string prgFile)
        {
            //string prgFile = FindProgFile(partId);
            //if (string.IsNullOrEmpty(prgFile))
            //{
            //    _IsOpened = false;
            //    return; 
            //}
            try
            {
                _pcdProgramManager.CloseAll();
                //_pcdProgramManager.Remove(1);
                //Thread.Sleep(10000);
                while (_pcdProgramManager.Count != 0) ; // 等待程序关闭
                _partProgram = null;
                //_pcdApplication.SetActive();
                //_pcdApplication.Maximize();
                _partProgram = _pcdProgramManager.Open(prgFile, _pcdApplication.DefaultMachineName/*"CMM1"*/);
                //_pcdApplication.WaitUntilReady(10);
                _IsOpened = true;
            }
            catch (Exception)
            {
                Debug.WriteLine("打开测量程序失败");
            }
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
            bool exeOk = false;
            if (_IsOpened)
            {
                // todo 设置程序事件响应函数
                _partProgram.OnExecuteDialogErrorMsg += _partProgram_OnExecuteDialogErrorMsg;
                // 执行程序
                exeOk = _partProgram.AsyncExecute();
            }
            return exeOk;
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
                    result = item.WaitForExit((int)TimeSpan.FromSeconds(3).TotalMilliseconds);
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

        public void Dispose()
        {
            if (_pcdApplication != null)
            {
                _pcdApplication.Quit();
            }
        }
    }

    public class PCDmisEventArgs : EventArgs
    {
        // 测量是否正常完成
        public bool IsCompleted { get; set; }
    }
}
