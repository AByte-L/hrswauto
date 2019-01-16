using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.BladeMold
{
    public class BladeContext
    {
        private bool CMMFileCreated;
        private Process _bladeProcess;

        public string CMMFileFullPath { get; private set; }

        private EventWaitHandle _bladeErrorEvent; // 分析出错事件
        private EventWaitHandle _bladeFinishEvent; // 分析结束事件，由外部程序重置
        private ManualResetEvent _cmmFileEvent; // Cmm文件创建完成事件

        public BladeContext()
        {
            // blade软件完成事件
            _bladeErrorEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "Gy.HrswAuto.BladeErrorEvent");
            _bladeFinishEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "Gy.HrswAuto.BladeFinishEvent");
            _cmmFileEvent = new ManualResetEvent(false);
            CMMFileCreated = false;
        }

        /// <summary>
        /// 查找关闭Blade进程，blade回调会占用网络端口
        /// </summary>
        /// <returns></returns>
        public bool CloseBlade()
        {
            Process[] blades = Process.GetProcessesByName("Blade");
            if (blades.Length > 0) // 已经启动
            {
                bool closeOk = false;
                foreach (var blade in blades)
                {
                    // 关闭消息对话框
                    //CloseBladeInfoDialog();
                    //Thread.Sleep(300); // 等待主界面显示
                    if (blade.MainWindowHandle != IntPtr.Zero)
                    {
                        WinApi.SetForegroundWindow(blade.MainWindowHandle);
                        WinApi.SendMessage(blade.MainWindowHandle, WinApi.WM_CLOSE, 0, 0);
                        closeOk = blade.WaitForExit((int)TimeSpan.FromSeconds(3).TotalMilliseconds);
                    }
                    if (!closeOk)
                    {
                        blade.Kill();
                        closeOk = blade.WaitForExit((int)TimeSpan.FromSeconds(10).TotalMilliseconds);
                        if (!closeOk)
                        {
                            Debug.WriteLine("Blade.exe无法关闭");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 启动blade进行分析
        /// 阻塞方法，需要时在单独线程中运行
        /// </summary>
        /// <param name="bladeExecutablePath">blade软件路径</param>
        /// <param name="rptFile">报告文件路径</param>
        /// <returns>是否正确执行了分析</returns>
        public bool StartBlade(string bladeExecutablePath, string rptFile)
        {
            // 监控CMM文件创建
            string reportPath = Path.GetDirectoryName(rptFile);
            FileSystemWatcher reportWatcher = new FileSystemWatcher(reportPath, "*.CMM");
            reportWatcher.Created += ReportFiles_Created;
            reportWatcher.EnableRaisingEvents = true;
            // 查找Blade应用，如果已经启动则关闭应用
            if (!CloseBlade()/*CloseBladeInfoDialog()*/)
            {
                // kill blade进程
                //if (!CloseBlade())
                //{
                Debug.WriteLine("blade 在错误状态");
                return false;
                //}
            }
            // 监控线程取消令牌
            CancellationTokenSource cts = new CancellationTokenSource(/*TimeSpan.FromMinutes(6)*/);
            var tk = cts.Token;
            // 启动监控线程
            var t = new Task<string>(BladeMonitor, tk);
            t.Start();

            // 启动分析
            _bladeProcess = new Process();
            _bladeProcess.StartInfo.FileName = bladeExecutablePath;
            _bladeProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(bladeExecutablePath);
            _bladeProcess.StartInfo.Arguments = rptFile;
            _bladeProcess.StartInfo.UseShellExecute = false;
            _bladeProcess.StartInfo.ErrorDialog = true;
            ////_bladeProcess.StartInfo.Verb = "runas"; // 是否以管理员权限启动
            _bladeProcess.Start();

            // 设置等待信号
            _bladeErrorEvent.Reset();
            _bladeFinishEvent.Reset();
            var waitHandles = new EventWaitHandle[2] { _bladeErrorEvent, _bladeFinishEvent };

            // 等待分析结束
            int index = WaitHandle.WaitAny(waitHandles, TimeSpan.FromMinutes(5));
            cts.Cancel(); // 取消监控线程
            if (index == WaitHandle.WaitTimeout) // 等待分析超时
            {
                // 错误反馈
                Debug.WriteLine("分析超时");
                return false;
            }
            else if (index == 1)
            {
                // 分析完成通知主程序
                Debug.WriteLine("Blade 分析完成");
                if(!_cmmFileEvent.WaitOne(TimeSpan.FromSeconds(30)))
                {
                    Debug.WriteLine("CMM文件创建失败");
                     reportWatcher.Dispose();
                    return false;
                }
                reportWatcher.Dispose();
                // 等待CMM创建完成
                // 阻塞
                if(!WaitForCreated(CMMFileFullPath))
                {
                    Debug.WriteLine("创建CMM文件超时");
                    return false;
                }
                return true;
            }
            else if (index == 0)
            {
                // blade分析出现错误
                Debug.WriteLine("Blade 分析出错");
                if (t.IsCompleted)
                {
                    string err = t.Result;
                    Debug.WriteLine(err);
                }
                //MessageBox.Show("Blade 分析出错, 请检查.");
                return false;
            }
            // 
            return false;
        }


        /// <summary>
        /// 等待Blade创建CMM完成
        /// </summary>
        /// <param name="cMMFileFullPath"></param>
        private bool WaitForCreated(string cMMFileFullPath)
        {
            bool inUse = true;
            FileStream fs = null;
            DateTime sdt = DateTime.Now;
            DateTime edt = sdt;
            // 这里可能产生死循环
            while (inUse)
            {
                if ((edt - sdt) > TimeSpan.FromMinutes(2))
                {
                    // CMM文件创建超时
                    return false;
                }
                try
                {
                    fs = new FileStream(cMMFileFullPath, FileMode.Open, FileAccess.Read, FileShare.None);
                    inUse = false;
                }
                catch (Exception)
                {
                    Debug.WriteLine("打印没有完成请等待");
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
                edt = DateTime.Now;
            }
            return true;
        }

        private void ReportFiles_Created(object sender, FileSystemEventArgs e)
        {
            // 等待文件创建完成
            CMMFileCreated = true;
            CMMFileFullPath = e.FullPath;
            _cmmFileEvent.Set();
        }

        /// <summary>
        /// 监控blade分析，判断是否Blade分析出错
        /// </summary>
        /// <param name="ctk"></param>
        private string BladeMonitor(object ctk)
        {
            string errorString = "";
            bool findError = false;
            CancellationToken ctkn = (CancellationToken)ctk;
            List<string> stcs = new List<string>();
            int pId = 0;
            while (!ctkn.IsCancellationRequested)
            {
                // blade 必须打开AutoPrinter设置，否则会产生Activation Error错误，可避免错误
                //IntPtr hWnd = WinApi.FindWindow("#32770", "Activation Error");
                IntPtr hWnd = WinApi.FindWindow("#32770", "blade");
                if (hWnd == IntPtr.Zero)
                {
                    continue;
                }
                WinApi.GetWindowThreadProcessId(hWnd, out pId);
                if (pId == _bladeProcess.Id) // 比较进程ID
                {
                    IntPtr shwnd = IntPtr.Zero;
                    var sb = new StringBuilder(200);
                    // 循环查找静态文本框
                    while (true)
                    {
                        shwnd = WinApi.FindWindowEx(hWnd, shwnd, "Static", null);
                        if (shwnd == IntPtr.Zero)
                        {
                            break;
                        }
                        int len = WinApi.GetWindowTextW(shwnd, sb, sb.Capacity);
                        if (sb.ToString() != string.Empty)
                        {
                            stcs.Add(sb.ToString());
                        }
                    }
                    if (stcs.Count > 0)
                    {
                        if (!stcs.Contains("Printing"))
                        {
                            errorString = stcs[0];
                            findError = true;
                        }
                    }
                }
                // 关闭错误对话框？

                if (findError)
                {
                    _bladeErrorEvent.Set();
                    break;
                }
            }
            return errorString;
        }
    }
}
