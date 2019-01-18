using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.CmmServer
{
    /// <summary>
    /// 监视PCDMIS进程，如果意外退出重新初始化PCDMIS
    /// 重新初始化之后，需要通知客户端继续
    /// </summary>
    public class PCDmisDaemon
    {
        public MeasureServiceContext Msc { get; set; }
        Task daeMonTask;

        public PCDmisDaemon(CancellationToken token) // 由测量上下文终止守护线程
        {
            daeMonTask = new Task(MonitorPCDmis, token, token, TaskCreationOptions.LongRunning);
        }

        public void Perform()
        {
            daeMonTask.Start();
        }

        private void MonitorPCDmis(object token)
        {
            throw new NotImplementedException();
        }
    }
}
