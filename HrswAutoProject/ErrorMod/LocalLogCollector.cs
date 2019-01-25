using Gy.HrswAuto.UICommonTools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gy.HrswAuto.ErrorMod
{
    public class LocalLogCollector
    {
        private static object _syncObj = new object();
        protected LocalLogCollector() { }
        private static LocalLogCollector _instance;
        public static LocalLogCollector Instance
        {
            get
            {
                _instance = _instance ?? new LocalLogCollector();
                return _instance;
            }
        }

        public static string LogFilePath { get; set; }

        public static void WriteMessage(string message)
        {
            lock (_syncObj) // 同步锁
            {
                try
                {
                    // UILinker.RefreshEventLog 刷新列表显示
                    string logText = message + Environment.NewLine;
                    File.AppendAllText(LogFilePath, logText);
                    //ServerUILinker.WriteUILog(message);
                }
                catch (Exception)
                {
                    MessageBox.Show("写本地LOG出错");
                }
            }
        }
    }
}
