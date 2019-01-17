using Gy.HrswAuto.BladeMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.ICmmServer;
using Gy.HrswAuto.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.CmmServer
{
    /// <summary>
    /// 控制三坐标测量服务实现类
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false)]
    public class CmmControl : ICmmControl, IDisposable
    {
        #region 本地资源变量
        PCDLRN.Application _pcdApp; // pcdmis主引用
        PCDLRN.PartPrograms _partPrograms; // pcdmis程序集
        PCDLRN.PartProgram _partProgram; // 当前调用程序
        PCDLRN.ApplicationObjectEvents _pcdAppEvents; // APP事件
        string _currentPartId; // 当前测量工件标识 
        private bool _IsInitialed = false; // 是否初始化
        private bool _IsMeasured = false; // 是否测量完成
        public string BladeExePath { get; set; } // 全路径的Blade.exe文件路径
        public string GotoProgFilePath { get; set; } // 全路径的回安全位置程序
        private BladeMeasAssist _bladeMeasAssist;
        /// <summary>
        /// 事件回调代理接口
        /// </summary>
        IWorkflowNotify _eventNotify;
        private string _outputFileName;
        #endregion

        #region 服务接口方法实现

        public void ConnectWFEvents()
        {
            _eventNotify = OperationContext.Current.GetCallbackChannel<IWorkflowNotify>();
        }

        public void DisconnectWFEvents()
        {
            throw new NotImplementedException();
        }

        public void MeasurePart(string partId)
        {
            _currentPartId = partId;
            string prgFile = FindProgFile(partId); // 返回带扩展名的零件程序
            // 正常情况下不会返回空值
            Debug.Assert(string.IsNullOrEmpty(prgFile));

            _bladeMeasAssist = new BladeMeasAssist();
            _bladeMeasAssist.Part = PartConfigManager.Instance.GetPartConfig(partId);
            // 创建blade.txt文件
            _bladeMeasAssist.CreateBladeTxtFromNominal();

            try
            {
                _partPrograms.CloseAll();
                _pcdApp.Maximize();
                _pcdApp.SetActive();
                _partProgram = _partPrograms.Open(prgFile, _pcdApp.DefaultMachineName/*"CMM1"*/);
                _partProgram.OnExecuteDialogErrorMsg += _partProgram_OnExecuteDialogErrorMsg;
                //  获取程序中测尖直径
                PCDLRN.OldBasic ob = _partProgram.OldBasic;
                _bladeMeasAssist.ProbeDiam = 2 * ob.GetProbeRadius();
                _bladeMeasAssist.RtfFileName/*_outputFileName*/ = FindOutputFileName();
                 // 异步执行
                _partProgram.AsyncExecute();
            }
            catch (Exception ex)
            {

            }
            _IsMeasured = true; // 是否需要
        }

        public bool IsInitialed()
        {
            return _IsInitialed;
        }
        #endregion

        #region 本地功能方法
        /// <summary>
        /// 开启并初始化PCDMIS
        /// </summary>
        public void InitialPCDmis()
        {
            // 关闭先前的PCDMIS
            ClosePCDmis();
            try
            {
                Type t = Type.GetTypeFromProgID("PCDLRN.Application");
                //SetPCDmisOffline(t); //是否以离线方式启动PCDMIS
                _pcdApp = (PCDLRN.Application)Activator.CreateInstance(t);
                _pcdApp.UserExit = false;
                _pcdApp.Visible = true;
                if (_pcdApp.SetActive())
                {
                    _pcdApp.Maximize();
                }
                _partPrograms = _pcdApp.PartPrograms;
                _pcdAppEvents = _pcdApp.ApplicationEvents;
                _pcdAppEvents.OnCloseExecutionDialog += _pcdAppEvents_OnCloseExecutionDialog; ;
            }
            catch (Exception ex)
            {
                // 启动失败的处理方式
            }
            _IsInitialed = true;
        }
        /// <summary>
        /// 定位到CMM的安全位置
        /// </summary>
        /// <param name="movePrg">PCDMIS定位程序名</param>
        public void CmmMoveToSafePosition(string movePrg)
        {
            _pcdApp.SetActive();
            _pcdApp.Maximize();
            try
            {
                _partPrograms.CloseAll();
                _partProgram = _partPrograms.Open(movePrg, _pcdApp.DefaultMachineName/*"CMM1"*/);
                _partProgram?.AsyncExecute();
            }
            catch (Exception ex)
            {
                // 移动到安全位置 异常处理

            }
        } 
        /// <summary>
        /// 在工件配置列表中查找测量程序
        /// </summary>
        /// <param name="partId">工件标识</param>
        /// <returns></returns>
        private string FindProgFile(string partId)
        {
            // 从工件配置管理器中获得工件列表
            PartConfig pc = PartConfigManager.Instance.GetPartConfig(partId);
            string filePath = PathManager.Instance.GetPartProgramPath(pc);
            Debug.Assert(string.IsNullOrEmpty(filePath)); // 正常情况FilePath不是空字符串
            return filePath;
        }
        #endregion

        #region PCDmis响应事件
        /// <summary>
        /// 测量完成
        /// </summary>
        /// <param name="ExecutionWindow"></param>
        private void _pcdAppEvents_OnCloseExecutionDialog(PCDLRN.ExecutionWindow ExecutionWindow)
        {
            if (_partProgram.ExecutionWasCancelled)
            {
                // 执行被终止
                _partProgram.Close();
                return;
            }
            _partProgram.Close();
            //GotoSafePosition(); // 是否需要回安全位置

            // 结果转换及叶片分析
            if (_IsMeasured) 
            {
                //Func<bool> bladeInvoker = new Func<bool>(BladeAnalysis);
                //IAsyncResult iar = bladeInvoker.BeginInvoke(analysisFinish, null);
                _eventNotify?.WorkCompleted(true); // 通知测量完成
            }
        } 
        private void _partProgram_OnExecuteDialogErrorMsg(string ErrorMsg)
        {
            // 通知客户端测量出错
        }
        #endregion

        #region 辅助类函数
        private void SetPCDmisOffline(Type t)
        {
            string clsid = "{" + t.GUID.ToString() + "}";
            RegistryKey clsKey = Registry.ClassesRoot.OpenSubKey(@"CLSID\" + clsid);
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

        public void Dispose()
        {
            if (_pcdApp != null)
            {
                _pcdApp.Quit();
            }
        }

        /// <summary>
        /// 获取第一个输出文件名
        /// </summary>
        /// <returns>全路径文件名或者空字符串</returns>
        private string FindOutputFileName()
        {
            int count = 1;
            string result = "";
            while (true)
            {
                PCDLRN.Command cmd = _partProgram.Commands.Item(count++);
                if (cmd.Type == PCDLRN.OBTYPE.PRINT_REPORT)
                {
                    result = cmd.GetText(PCDLRN.ENUM_FIELD_TYPES.FILE_NAME, 1);
                    break;
                }
            }
            if (!result.Contains(".RTF"))
            {
                result = result + ".RTF";
            }
            return result;
        }
        #endregion
    }
}
