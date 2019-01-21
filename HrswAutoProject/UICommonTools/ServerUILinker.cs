using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.UICommonTools
{
    public class ServerUILinker
    {
        //public static string message; 
        public static SynchronizationContext syncContext; // 同步上下文

        public static Action<string> RefreshLog; // 刷新log字符串
        public static Action<bool> RefreshRemoteState; // 刷新远端状态，心跳信号

        public static void WriteUILog(string message)
        {
            RefreshLog(message);
        }

        public static void UpdateRemoteState(bool state)
        {
            RefreshRemoteState(state);
        }
    }
}
