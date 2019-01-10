using Gy.HrswAuto.PLCMold;
using System;
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
        //public CmmClient Client { get; set; }

        protected PlcClient _plcClient; // 用于PLC读写

        protected Timer _timer;

        public FeedRequest()
        {
            _plcClient = PlcClient.Instance;
            _timer = new Timer(2000); // 2s判断一次PLC请求是否完成
            _timer.Elapsed += _timer_PlcStateMonitor;
        }

        public virtual void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual void Perform()
        {
            throw new NotImplementedException();
        }
     }

    
    /// <summary>
    /// 只下料请求
    /// </summary>
    public class GripFeedRequest : FeedRequest
    {
        public bool IsPassed { get; set; }
        /// <summary>
        /// PLC下料动作完成事件
        /// </summary>
        public event EventHandler<ResponsePlcEventArgs> GripActionCompletedEvent;

        public GripFeedRequest():base()
        {

        }

        public override void Perform()
        {
            _timer.Start();
            _plcClient.ResponseGripRequest(ClientID, IsPassed);
        }

        public override void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        {
            // base._timer_PlcStateMonitor(sender, e);
            _timer.Stop(); // 暂停检测
            if (_plcClient.VerifyGripCompleted(ClientID))
            {
                ResponsePlcEventArgs args = new ResponsePlcEventArgs();
                args.ClientID = ClientID;
                GripActionCompletedEvent?.Invoke(this, args);  // 确定client是否在continue状态，否则停止上料            
                _timer.Dispose();
                return;
            }
            _timer.Start();
        }
    }
    
    /// <summary>
    /// 只下料请求
    /// </summary>
    public class PlaceFeedRequest : FeedRequest
    {
        /// <summary>
        /// PLC上料完成事件
        /// </summary>
        public event EventHandler<ResponsePlcEventArgs> PlaceActionCompletedEvent;
        public PlaceFeedRequest():base()
        {

        }
        public override void Perform()
        {
            _timer.Start();
            _plcClient.ResponsePlaceRequest(ClientID);
        }

        public override void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        {
            // base._timer_PlcStateMonitor(sender, e);
            _timer.Stop(); // 暂停检测
            if (_plcClient.VerifyPlaceCompleted(ClientID))
            {
                string partId = _plcClient.ReadPartID(ClientID);
                ResponsePlcEventArgs args = new ResponsePlcEventArgs();
                args.ClientID = ClientID;
                args.PartID = partId;
                PlaceActionCompletedEvent?.Invoke(this, args);  // 触发上料完成事件       
                _timer.Dispose();
                return;
            }
            _timer.Start();
        }
    }

    /// <summary>
    /// 先下后上请求
    /// </summary>
    public class PlaceAndGripFeedRequest : FeedRequest
    {
        public bool IsPass { get; set; } = false;
        /// <summary>
        /// PLC下料动作完成事件
        /// </summary>
        //public event EventHandler<ResponsePlcEventArgs> GripActionCompletedEvent;
        /// <summary>
        /// PLC上料完成事件
        /// </summary>
        public event EventHandler<ResponsePlcEventArgs> PlaceActionCompletedEvent;

        public PlaceAndGripFeedRequest():base()
        {

        }
        public override void Perform()
        {
            _timer.Start();
            _plcClient.ResponsePlaceAndGripFeedRequest(ClientID, IsPass);
        }

        public override void _timer_PlcStateMonitor(object sender, ElapsedEventArgs e)
        {
            // base._timer_PlcStateMonitor(sender, e);
            _timer.Stop(); // 暂停检测
            if (_plcClient.VerifyPlaceCompleted(ClientID)) // 上料完成
            {
                string partID = _plcClient.ReadPartID(ClientID); // 读取零件标识
                ResponsePlcEventArgs args = new ResponsePlcEventArgs();
                args.PartID = partID;
                args.ClientID = ClientID;
                PlaceActionCompletedEvent?.Invoke(this, args);
                _timer.Dispose();
                return;
            }
            _timer.Start();
        }
    }
}
