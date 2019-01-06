using Gy.HrswAuto.ICmmServer;
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
        #endregion

        #region 服务接口方法实现
        /// <summary>
        /// 事件回调代理接口
        /// </summary>
        IWorkflowNotify _eventNotify;

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
            string prgFile = FindProgFile(partId);
            if (string.IsNullOrEmpty(prgFile))
            {
                // 错误处理 
                return;
            }
            // 创建blade.txt文件
            //PCDmisUtility.CreateBladeTxtFromNominal(PartManager.GetConfig(partID));
            try
            {
                _partPrograms.CloseAll();
                _partProgram = _partPrograms.Open(prgFile, _pcdApp.DefaultMachineName/*"CMM1"*/);
                _pcdApp.Maximize();
                //  获取程序中测尖直径
                PCDLRN.OldBasic ob = _partProgram.OldBasic;
                //PCDmisUtility._probeDiam = 2 * ob.GetProbeRadius();
                // 异步执行
                _partProgram.AsyncExecute();

            }
            catch (Exception ex)
            {

            }
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
                // _pcdAppEvents.OnCloseExecutionDialog += 
            }
            catch (Exception ex)
            {
                // 启动失败的处理方式
            }
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
            //PartConfig partConfig = PartManager.GetConfig(partID);
            //string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\parts\" + partID;
            //if (!Directory.Exists(path))
            //{
            //    return string.Empty;
            //}
            //if (partConfig == null)
            //{
            //    return string.Empty;
            //}
            //else if (!partConfig.config.ContainsKey(".prg"))
            //{
            //    return string.Empty;
            //}
            //return Path.Combine(path, partConfig.config[".prg"]);
            return @"d:\1.prg";
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
        #endregion
    }
}
