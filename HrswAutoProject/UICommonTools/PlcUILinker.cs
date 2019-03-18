using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.UICommonTools
{
    public class PlcUILinker
    {
        public static SynchronizationContext syncContext; // 同步上下文

        public static Action<DateTime, string, string> Logging;

        public static void Log(string v1, string v2)
        {
            Logging?.Invoke(DateTime.Now, v1, v2);
        }
    }
}

