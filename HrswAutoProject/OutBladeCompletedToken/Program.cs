using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OutBladeCompletedToken
{
    class Program
    {
            private static EventWaitHandle _bladeFinishEvent; // 分析结束事件，由外部程序重置
        static void Main(string[] args)
        {
            // blade软件完成事件

            bool cn = false;
            _bladeFinishEvent = new EventWaitHandle(false, EventResetMode.ManualReset, "Gy.HrswAuto.BladeFinishEvent", out cn);
            _bladeFinishEvent.Set();
            Thread.Sleep(1000);
            _bladeFinishEvent.Close();
        }
    }
}
