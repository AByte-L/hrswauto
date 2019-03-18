using Gy.HrswAuto.PLCMold;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ClientMold
{

    public class CompletedEventArgs : EventArgs
    {
        private string _partID;

        public string PartID
        {
            get { return _partID; }
        }
        public CompletedEventArgs(string partID)
        {
            _partID = partID;
        }
    }

    public abstract class RobotAction
    {
        protected PlcClient _plcClient; // 用于PLC读写
        protected CmmClient _client; // 客户端
        protected bool _heartbeat; // 心跳信号
        public event EventHandler<CompletedEventArgs> CompletedEvent;
        public RobotAction(CmmClient client)
        {
            _client = client;
            _plcClient = PlcClient.Instance;
            _plcClient.DisconnectEvent += _plcClient_DisconnectEvent;
            _heartbeat = true;
        }

        private void _plcClient_DisconnectEvent(object sender, EventArgs e)
        {
            _heartbeat = false;
        }

        public virtual void Perform()
        {
            Task.Factory.StartNew(PerformAction);
        }

        protected virtual void PerformAction()
        {
            _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
        }

        protected void CompletedEventInvoker(object sender, CompletedEventArgs e)
        {
            CompletedEvent?.Invoke(sender, e);
        }

        public void ChangeAction(RobotAction state)
        {
            _client.ChangeState(state);
        }
    }
    /// <summary>
    /// 下料请求
    /// </summary>
    public class RobotGripRequest : RobotAction
    {
        private bool _isPass;
        public RobotGripRequest(CmmClient client, bool isPass) : base(client)
        {
            _isPass = isPass;
        }

        protected override void PerformAction()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (!_heartbeat)
                {
                    // 更新界面 PLC断开连接
                    break;
                }
                else if (sw.Elapsed > TimeSpan.FromMinutes(10))
                {
                    // 更新界面， PLC发送抓料响应失败
                    break;
                }
                else
                {
                    if (_plcClient.SendGripRequest(_client.CmmSvrConfig.ServerID, _isPass))
                    {
                        // 开始等待抓料响应
                        ChangeAction(new RobotGripResponse(_client));
                        _client.OnPerformPlcRequest();
                        break;
                    }
                }
                Thread.Sleep(1000);
            }
            base.PerformAction();
        }
    }
    /// <summary>
    /// 下料响应
    /// </summary>
    public class RobotGripResponse : RobotAction
    {
        public RobotGripResponse(CmmClient client) : base(client)
        {
            CompletedEvent += client.OnGripCompleted;
        }

        protected override void PerformAction()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (!_heartbeat)
                {
                    // 更新界面 PLC断开连接
                    break;
                }
                else if (sw.Elapsed > TimeSpan.FromMinutes(10))
                {
                    // 更新界面， PLC发送抓料响应失败
                    break;
                }
                else
                {
                    if (_plcClient.VerifyGripCompleted(_client.CmmSvrConfig.ServerID)) // 抓料完成，调用完成事件
                    {      
                        CompletedEventInvoker(this, null);
                        break;
                    }
                }
                Thread.Sleep(1000);
            }
            CompletedEvent -= _client.OnGripCompleted;
            base.PerformAction();
        }
    }
    /// <summary>
    /// 上料请求
    /// </summary>
    public class RobotPlaceRequest : RobotAction
    {
        public RobotPlaceRequest(CmmClient client) : base(client)
        {

        }
        protected override void PerformAction()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (!_heartbeat)
                {
                    // 更新界面 PLC断开连接
                    break;
                }
                else if (sw.Elapsed > TimeSpan.FromMinutes(10))
                {
                    // 更新界面， PLC发送上料请求失败
                    break;
                }
                else
                {
                    if (_plcClient.SendPlaceRequest(_client.CmmSvrConfig.ServerID))
                    {
                        // 开始等待抓料响应
                        ChangeAction(new RobotPlaceResponse(_client));
                        _client.OnPerformPlcRequest();
                        break;
                    }
                }
                Thread.Sleep(1000);
            }
            sw.Stop();
            base.PerformAction();
        }
    }
    /// <summary>
    /// 上料响应
    /// </summary>
    public class RobotPlaceResponse : RobotAction
    {

        public RobotPlaceResponse(CmmClient client) : base(client)
        {
            CompletedEvent += client.OnPlaceCompleted;
        }

        protected override void PerformAction()
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //while (true)
            //{
            //    if (!_heartbeat)
            //    {
            //        // 更新界面 PLC断开连接
            //        break;
            //    }
            //    else if (sw.Elapsed > TimeSpan.FromMinutes(10))
            //    {
            //        // 更新界面， PLC发送抓料响应失败
            //        break;
            //    }
            //    else
            //    {
            //        string partID = string.Empty;
            //        //if (_plcClient.VerifyPlaceCompleted(_client.CmmSvrConfig.ServerID, out partID)) // 抓料完成，调用完成事件
            //        //{
            //        //    CompletedEventArgs ce = new CompletedEventArgs(partID);
            //        //    CompletedEventInvoker(this, ce);
            //        //    // 关闭心跳事件
            //        //    break;
            //        //}
            //        if (_plcClient.VerifyDuringPlacement(_client.CmmSvrConfig.ServerID, out partID))
            //        {

            //        }
            //    }
            //    Thread.Sleep(1000);
            //}
            
            string partId = string.Empty;
            var rdtask = Task.Run(() => ReadPartID(out partId));
            if (rdtask.Result)
            {
                var vftask = Task.Run(() => VerifyCompleted());
                if (vftask.Result)
                {
                    CompletedEventArgs ce = new CompletedEventArgs(partId);
                    CompletedEventInvoker(this, ce);
                }
                else
                {
                    // todo 报告放置工作出错
                }
            }
            else
            {
                // todo 报告放置时读取RFID错误
            }
            CompletedEvent -= _client.OnGripCompleted;
            base.PerformAction();
        }

        private bool VerifyCompleted()
        {
            bool result = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (sw.Elapsed > TimeSpan.FromMinutes(10) || !_heartbeat)
                {
                    // 超时或中断连接跳出
                    result = false;
                    break;
                }
                result = _plcClient.VerifyPlaceCompleted(_client.CmmSvrConfig.ServerID);
                if (result)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            sw.Stop();
            return result;
        }

        private bool ReadPartID(out string rfid)
        {
            bool result = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (sw.Elapsed > TimeSpan.FromMinutes(1) || !_heartbeat) 
                {
                    // 超时或中断连接跳出
                    result = false;
                    rfid = string.Empty;
                    break; 
                }
                result = _plcClient.VerifyDuringPlacement(_client.CmmSvrConfig.ServerID, out rfid);
                if (result)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            sw.Stop();
            return result;
        }
    }

    /// <summary>
    /// 发送三坐标准备就绪信号
    /// </summary>
    public class CmmInitReady: RobotAction
    {
        public CmmInitReady(CmmClient client) : base(client)
        {
        }

        protected override void PerformAction()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (!_heartbeat)
                {
                    // 更新界面 PLC断开连接
                    break;
                }
                else if (sw.Elapsed > TimeSpan.FromMinutes(10))
                {
                    // 更新界面， PLC发送准备就绪信号失败
                    break;
                }
                else
                {
                    if (_plcClient.SendCmmReadySignal(_client.CmmSvrConfig.ServerID))
                    {
                        // 等待PLC准备就绪
                        Thread.Sleep(5000); 
                        // 开始等待抓料响应
                        ChangeAction(new RobotPlaceRequest(_client));
                        _client.OnPerformPlcRequest();
                        break;
                    }
                }
                Thread.Sleep(1000);
            }
            sw.Stop();
            base.PerformAction();
        }
    }
}
