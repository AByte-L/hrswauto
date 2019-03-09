using Gy.HrswAuto.PLCMold;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Gy.HrswAuto.ClientMold
{

    public class ResponsePlcEventArgs
    {
        public int ClientID { get; internal set; }
        public string PartID { get; internal set; }
    }

    public class FeedRequest
    {
        public int ClientID { get; set; }
        protected bool _state; // false发送请求，true等待PLC响应请求
        //public CmmClient Client { get; set; }
        public event EventHandler<ResponsePlcEventArgs> PlcCompletedEvent;

        protected PlcClient _plcClient; // 用于PLC读写

        //protected Timer _timer;
        protected bool _plcHeartbeatTriggle;
        public FeedRequest()
        {
            _plcClient = PlcClient.Instance;
            _plcClient.DisconnectEvent += _plcClient_DisconnectEvent;
            _plcClient.ReconnectEvent += _plcClient_ReconnectEvent;
            //_timer = new Timer(2000); // 2s判断一次PLC请求是否完成
            //_timer.Elapsed += _timer_PlcStateMonitor;
            _plcHeartbeatTriggle = false;
            _state = false; // 发送请求
        }

        protected void _plcClient_ReconnectEvent(object sender, EventArgs e)
        {
            //_timer.Start();
            _plcHeartbeatTriggle = false;
            // 重新启动
            Perform();
        }

        protected void _plcClient_DisconnectEvent(object sender, EventArgs e)
        {
            // 关闭动作扫描
            //_timer.Stop(); 
            _plcHeartbeatTriggle = true;
        }

        public virtual void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual void Perform()
        {
            Task.Factory.StartNew(FeedRequestHandler);
        }

        protected virtual void FeedRequestHandler()
        {
            throw new NotImplementedException();
        }

        protected void PlcCompletedEventInvoker(object sender, ResponsePlcEventArgs e)
        {
            PlcCompletedEvent?.Invoke(sender, e);
        }
    }


    /// <summary>
    /// 只下料请求
    /// </summary>
    public class GripFeedRequest : FeedRequest
    {
        public bool IsPassed { get; set; } // 工件是否合格
        //public event EventHandler<ResponsePlcEventArgs> GripActionCompletedEvent;

        public GripFeedRequest() : base()
        {

        }

        //public override void Perform()
        //{
        //    //_timer.Start();
        //    //_plcClient.SendGripRequest(ClientID, IsPassed);
        //}

        //public override void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        //{
        //    // base._timer_PlcStateMonitor(sender, e);
        //    //_timer.Stop(); // 暂停检测, 避免重复触发
        //    if (!_state)
        //    {
        //        _state = _plcClient.SendGripRequest(ClientID, IsPassed);
        //    }
        //    else
        //    {
        //        if (_plcClient.VerifyGripCompleted(ClientID)) // 抓料完成，调用完成事件
        //        {
        //            ResponsePlcEventArgs args = new ResponsePlcEventArgs();
        //            args.ClientID = ClientID;
        //            //GripActionCompletedEvent?.Invoke(this, args);  //           
        //            PlcCompletedEventInvoker(this, args);
        //            // 关闭心跳事件
        //            _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
        //            _plcClient.ReconnectEvent -= _plcClient_ReconnectEvent;
        //            //_timer.Dispose();
        //            //_timer = null;
        //            _plcClient = null;
        //            return;
        //        }
        //    }
        //    //_timer.Start();
        //}

        protected override void FeedRequestHandler()
        {
            //base.FeedRequestHandler();
            while (true)
            {
                if (_plcHeartbeatTriggle)
                {
                    return;
                }
                if (!_state)
                {
                    _state = _plcClient.SendGripRequest(ClientID, IsPassed);
                }
                else
                {
                    if (_plcClient.VerifyGripCompleted(ClientID)) // 抓料完成，调用完成事件
                    {
                        ResponsePlcEventArgs args = new ResponsePlcEventArgs();
                        args.ClientID = ClientID;
                        PlcCompletedEventInvoker(this, args);
                        // 关闭心跳事件
                        _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
                        _plcClient.ReconnectEvent -= _plcClient_ReconnectEvent;
                        _plcClient = null; // 释放
                        return;
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }

    /// <summary>
    /// 只下料请求
    /// </summary>
    public class PlaceFeedRequest : FeedRequest
    {
        //public event EventHandler<ResponsePlcEventArgs> PlaceActionCompletedEvent;
        public PlaceFeedRequest() : base()
        {

        }
        //public override void Perform()
        //{
        //    //_timer.Start();
        //    //_plcClient.SendPlaceRequest(ClientID);
        //}

        //public override void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        //{
        //    // base._timer_PlcStateMonitor(sender, e);
        //    //_timer.Stop(); 
        //    if (!_state) // 循环写
        //    {
        //        _state = _plcClient.SendPlaceRequest(ClientID);
        //    }
        //    else
        //    {
        //        if (_plcClient.VerifyPlaceCompleted(ClientID)) // 放料完成，调用完成事件
        //        {
        //            string partId = _plcClient.ReadPartID(ClientID);
        //            ResponsePlcEventArgs args = new ResponsePlcEventArgs();
        //            args.ClientID = ClientID;
        //            args.PartID = partId;
        //            //PlaceActionCompletedEvent?.Invoke(this, args);  // 触发上料完成事件       
        //            PlcCompletedEventInvoker(this, args);
        //            _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
        //            _plcClient.ReconnectEvent -= _plcClient_ReconnectEvent;
        //            //_timer.Dispose();
        //            //_timer = null;
        //            _plcClient = null;
        //            return;
        //        } 
        //    }
        //    //_timer.Start();
        //}
        protected override void FeedRequestHandler()
        {
            //base.FeedRequestHandler();
            while (true)
            {
                if (_plcHeartbeatTriggle)
                {
                    return;
                }
                if (!_state) // 循环写
                {
                    _state = _plcClient.SendPlaceRequest(ClientID);
                }
                else
                {
                    string partID;
                    if (_plcClient.VerifyPlaceCompleted(ClientID, out partID)) // 放料完成，调用完成事件
                    {
                        string partId = _plcClient.ReadPartID(ClientID);
                        ResponsePlcEventArgs args = new ResponsePlcEventArgs();
                        args.ClientID = ClientID;
                        args.PartID = partId;
                        PlcCompletedEventInvoker(this, args);
                        _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
                        _plcClient.ReconnectEvent -= _plcClient_ReconnectEvent;
                        _plcClient = null;
                        return;
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }

    /// <summary>
    /// 先下后上请求
    /// </summary>
    public class PlaceAndGripFeedRequest : FeedRequest
    {
        public bool IsPass { get; set; }
        //public event EventHandler<ResponsePlcEventArgs> GripActionCompletedEvent;
        //public event EventHandler<ResponsePlcEventArgs> PlaceActionCompletedEvent;

        public PlaceAndGripFeedRequest() : base()
        {

        }
        //public override void Perform()
        //{
        //    //_timer.Start();
        //    //_plcClient.ResponsePlaceAndGripFeedRequest(ClientID, IsPass);
        //}

        //public override void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        //{
        //    // base._timer_PlcStateMonitor(sender, e);
        //    //_timer.Stop(); // 暂停检测
        //    if (!_state)
        //    {
        //        _state = _plcClient.ResponsePlaceAndGripFeedRequest(ClientID, IsPass);
        //    }
        //    else
        //    {
        //        if (_plcClient.VerifyPlaceCompleted(ClientID)) // 放料完成，调用完成事件
        //        {
        //            string partID = _plcClient.ReadPartID(ClientID); // 读取零件标识
        //            ResponsePlcEventArgs args = new ResponsePlcEventArgs();
        //            args.PartID = partID;
        //            args.ClientID = ClientID;
        //            //PlaceActionCompletedEvent?.Invoke(this, args);
        //            PlcCompletedEventInvoker(this, args);
        //            _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
        //            _plcClient.ReconnectEvent -= _plcClient_ReconnectEvent;
        //            //_timer.Dispose();
        //            //_timer = null;
        //            _plcClient = null;
        //            return;
        //        } 
        //    }
        //    //_timer.Start();
        //}
        protected override void FeedRequestHandler()
        {
            //base.FeedRequestHandler();
            while (true)
            {
                if (_plcHeartbeatTriggle)
                {
                    return;
                }
                if (!_state)
                {
                    _state = _plcClient.ResponsePlaceAndGripFeedRequest(ClientID, IsPass);
                }
                else
                {
                    string partID; 
                    if (_plcClient.VerifyPlaceCompleted(ClientID, out partID)) // 放料完成，调用完成事件
                    {
                        partID = _plcClient.ReadPartID(ClientID); // 读取零件标识
                        ResponsePlcEventArgs args = new ResponsePlcEventArgs();
                        args.PartID = partID;
                        args.ClientID = ClientID;
                        PlcCompletedEventInvoker(this, args);
                        _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
                        _plcClient.ReconnectEvent -= _plcClient_ReconnectEvent;
                        _plcClient = null;
                        return;
                    }
                    Thread.Sleep(1000);
                }
            }
        }
    }

    public class ErrorFeedBack : FeedRequest
    {
        public ErrorFeedBack() : base()
        {

        }
        public override void Perform()
        {
            //_timer.Start();
            
        }

        public override void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        {
            //_timer.Stop();
            if (_state)
            {
               _state = _plcClient.SetIDErrorFlag(ClientID);
            }
            else
            {
                _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
                _plcClient.ReconnectEvent -= _plcClient_ReconnectEvent;
                //_timer.Dispose();
                //_timer = null;
                return;
            }
            //_timer.Start();
        }

        protected override void FeedRequestHandler()
        {
            while (true)
            {
                if (_plcHeartbeatTriggle)
                {
                    return;
                }
                if (_state)
                {
                    _state = _plcClient.SetIDErrorFlag(ClientID);
                }
                else
                {
                    _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
                    _plcClient.ReconnectEvent -= _plcClient_ReconnectEvent;
                    return;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
