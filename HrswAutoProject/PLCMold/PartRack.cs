using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.UICommonTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Gy.HrswAuto.PLCMold
{
    /// <summary>
    /// 槽类，用来获取工件在放置到槽上的位置，并对应上报告
    /// </summary>
    public class PartRack
    {
        private Timer _timer;
        private PlcClient _plcClient;
        public bool IsReadOk { get; private set; } // 读取槽号是否成功
        private int _slotNumber = 0;

        public int SlotNumber
        {
            get { return _slotNumber; }
        }

        private ResultRecord CurPartResult;

        private PartRack()
        {
            _timer = new Timer(2000);
            _timer.Elapsed += _timer_Elapsed;
            _plcClient = PlcClient.Instance;
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            if (_plcClient.ReadUnloadFlag())
            {
                // 卸料标志，3s内读取槽号
                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (true)
                {
                    if (sw.Elapsed > TimeSpan.FromSeconds(2.5))
                    {
                        // todo 读取失败，更新状态条
                        IsReadOk = false;
                        break;
                    }
                    if (_plcClient.ReadSlotNumber(out _slotNumber))
                    {
                        IsReadOk = true;
                        // 更新料架界面
                        ClientUICommon.RefreshRackView(CurPartResult, _slotNumber);
                        break;
                    }
                }
                sw.Stop();
                _timer.Dispose();
            }
            _timer.Start();
        }

        public void RefreshSlots(ResultRecord partResult)
        {
            _timer.Start();
            CurPartResult = partResult;
        }

        private static PartRack _partRack;

        public static PartRack Instance
        {
            get
            {
                if (_partRack == null)
                {
                    _partRack = new PartRack();
                }
                return _partRack;
            }
        }

    }
}
