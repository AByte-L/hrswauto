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
        private Timer _monitorTimer;
        private PlcClient _plcClient;
        // 开启循环监控PLC事件标志位
        public void Monitoring()
        {
            _monitorTimer = new Timer(2000);
            _monitorTimer.Elapsed += _monitorTimer_Elapsed;
            _monitorTimer.Start();
        }
        private void _monitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _monitorTimer.Stop();

            _monitorTimer.Start();
        }

        #region 实例创建
        private PlcMonitor()
        {
            _plcClient = PlcClient.Instance;
        }

        private PlcMonitor _plcMonitor;
        public PlcMonitor Instance
        {
            get
            {
                _plcMonitor = _plcMonitor ?? new PlcMonitor();
                return _plcMonitor;
            }
        }
        #endregion
    }
}
