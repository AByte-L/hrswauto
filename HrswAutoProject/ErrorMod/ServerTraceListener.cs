using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ErrorMod
{
    public class ServerTraceListener : TraceListener
    {
        // todo UILinker 界面连接器
        public string LogFileName { get; set; }

        public override void Write(string message)
        {
            File.AppendAllText(LogFileName, message + Environment.NewLine); 
            // todo 更新界面
        }

        public override void WriteLine(string message)
        {
            File.AppendAllText(LogFileName, message + Environment.NewLine);
        }
    }
}
