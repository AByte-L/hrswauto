//
// 当前逻辑算法暂时没有用到任务调度器类
//
//
using Gy.HrswAuto.ClientMold;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace Gy.HrswAuto.MasterMold
{
    /// <summary>
    /// 循环处理上料请求
    /// </summary>
    public class TaskDispatcher
    {
        #region 数据域
        /// <summary>
        /// 请求队列
        /// </summary>
        public static ConcurrentQueue<FeedRequest> _taskQueue = new ConcurrentQueue<FeedRequest>();
        
        private FeedRequest _currentFeedRequest;

        /// <summary>
        /// 轮询定时器
        /// </summary>
        private System.Timers.Timer _timer;
        #endregion

        public TaskDispatcher()
        {
            _timer = new System.Timers.Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);
            _timer.Elapsed += _timer_Elapsed;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_taskQueue.TryDequeue(out _currentFeedRequest))
            {
                _currentFeedRequest.Perform();
                _timer.Stop();
            }
            else
            {
                Debug.WriteLine($"等待上料请求... {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}
