using Snap7;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Gy.HrswAuto.PLCMold
{
    public class PlcMonitor
    {
        public int ClientCount { get; set; } = 5;
        private Timer _monitorTimer;
        private PlcClient _plcClient;
        /// <summary>
        /// 抓取完成事件
        /// </summary>
        public event EventHandler<GripCompletedArgs> GripCompletedEvent;
        /// <summary>
        /// 放置完成事件
        /// </summary>
        public event EventHandler<PlaceCompletedArgs> PlaceCompletedEvent;

        // 开启循环监控PLC事件标志位
        public void Monitoring()
        {
            _monitorTimer = new Timer(2000);
            _monitorTimer.Elapsed += _monitorTimer_Elapsed;
            _monitorTimer.Start();
        }
        private void _monitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // 存储器号从1开始
            byte[] buf = new byte[1];
            _monitorTimer.Stop();
            for (int i = 1; i < ClientCount + 1; i++)
            {
                _plcClient.ReadData(i, 1, 1, buf); // 响应标志字节
                if (S7.GetBitAt(buf, 0, 1)) // 抓取完成
                {
                    GripCompletedArgs gcArgs = new GripCompletedArgs() { ClientID = i };
                    GripCompletedEvent?.Invoke(this, gcArgs);
                    _plcClient.ResetGripOkFlag(i);
                    break;
                }
                else if (S7.GetBitAt(buf, 0, 0)) // 放置完成
                {
                    byte[] partIdbyte = new byte[256];
                    _plcClient.ReadData(i, 2, 256, partIdbyte);
                    string partId = S7.GetStringAt(partIdbyte, 0);
                    PlaceCompletedArgs pcArgs = new PlaceCompletedArgs() { ClientID = i, PartID = partId };
                    PlaceCompletedEvent?.Invoke(this, pcArgs);
                    _plcClient.ResetPlaceOkFlag(i);
                    break;
                }
            }
            _monitorTimer.Start();
        }

        #region 单实例
        private PlcMonitor()
        {
            _plcClient = PlcClient.Instance;
        }

        private PlcMonitor _plcMonitor;
        public PlcMonitor Instance
        {
            get
            {
                if (_plcMonitor == null)
                {
                    _plcMonitor = new PlcMonitor();
                }
                return _plcMonitor;
            }
        } 
        #endregion
    }

    public class PlaceCompletedArgs : EventArgs
    {
        public int ClientID { get; set; }
        public string PartID { get; set; }
    }

    public class GripCompletedArgs : EventArgs
    {
        public int ClientID { get; set; }
    }
}
