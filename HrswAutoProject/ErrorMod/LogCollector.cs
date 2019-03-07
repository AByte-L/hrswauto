using Gy.HrswAuto.ICmmServer;
using Gy.HrswAuto.UICommonTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ErrorMod
{
    public class LogCollector
    {
        private object _syncobj = new object(); // 同步对象
        private LogCollector() { }
        private static LogCollector _instance;
        public static LogCollector Instance
        {
            get
            {
                _instance = _instance ?? new LogCollector();

                return _instance;
            }
        }
        
        public IWorkflowNotify SvrNotify { get; set; }// 客户端通知接口
        //public string LogFileName { get; set; }// 本地Log文件名
        //public string Message { get; set; } // 日志消息有很多，取消本地属性
        public void PostSvrErrorMessage(string message) // 传递服务器错误消息, 同步方法
        {
            lock (_syncobj) 
            {
                try
                {
                    //LocalLogCollector.WriteMessage(message);
                    SvrNotify?.ServerInErrorStatus(message);
                    LocalLogCollector.WriteMessage(message);
                }
                catch (Exception ex)
                {
                    LocalLogCollector.WriteMessage(message + ", 通信异常:" + ex.Message); // 记录通信异常事件
                }
                //finally
                //{
                //    LocalLogCollector.WriteMessage(message); // 工作出错状态信息
                //    //ServerUILinker.WriteUILog(message);
                //}
            }
        }

        public void PostSvrWorkStatus(string message)
        {
            lock (_syncobj)
            {
                try
                {
                    SvrNotify?.ServerWorkStatus(message);
                    LocalLogCollector.WriteMessage(message);
                }
                catch (Exception ex)
                {
                    LocalLogCollector.WriteMessage(message + ", 通信异常:" + ex.Message); // 记录通信异常事件
                }
                //finally
                //{
                //    LocalLogCollector.WriteMessage(message); // 服务器工作状态
                //    //ServerUILinker.WriteUILog(message);
                //}
            }
        }  // 传递服务器工作状态信息，同步方法
    }
}
