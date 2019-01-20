using Gy.HrswAuto.ICmmServer;
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
                    SvrNotify?.ServerInErrorStatus(message);
                }
                catch (Exception ex)
                {
                    WriteMessage(ex.Message); // 记录通信异常事件
                }
                finally
                {
                    WriteMessage(message); // 工作出错状态信息
                }
            }
        }

        public void PostSvrWorkStatus(string message)
        {
            lock (_syncobj)
            {
                try
                {
                    SvrNotify?.ServerWorkStatus(message);
                }
                catch (Exception ex)
                {
                    WriteMessage(ex.Message); // 记录通信异常事件
                }
                finally
                {
                    WriteMessage(message); // 服务器工作状态
                }
            }
        }  // 传递服务器工作状态信息，同步方法

        public void WriteMessage(string message) // 写本地事件
        {
            // 程序开始需要挂接TraceListener
            try
            {
                Trace.Write(message);
            }
            catch (Exception)
            {
                // 写日志异常，不做处理
            }
        }
    }
}
