﻿using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.UICommonTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Gy.HrswAuto.PLCMold
{
    /// <summary>
    /// 槽类，用来获取工件在放置到槽上的位置，并对应上报告
    /// </summary>
    public class PartRack
    {
        //private Timer _timer;
        private PlcClient _plcClient;
        public bool IsReadOk { get; private set; } // 读取槽号是否成功
        private int _slotNumber = 0;

        public int SlotNumber
        {
            get { return _slotNumber; }
        }

        private ResultRecord CurPartResult;
        private bool _plcHbDis;
        private PartRack()
        {
            //_timer = new Timer(2000);
            //_timer.Elapsed += _timer_Elapsed;
            _plcHbDis = false;
            _plcClient = PlcClient.Instance;
            _plcClient.DisconnectEvent += _plcClient_DisconnectEvent;
            _plcClient.ReconnectEvent += _plcClient_ReconnectEvent;
        }

        private void _plcClient_ReconnectEvent(object sender, EventArgs e)
        {
            //_timer.Start();
        }

        private void _plcClient_DisconnectEvent(object sender, EventArgs e)
        {
            //_timer.Stop();
            _plcHbDis = true;
        }

        private void Perform()
        {
            Task.Factory.StartNew(UpdateRack);
        }

        private void UpdateRack()
        {
            while (true)
            {
                if (_plcHbDis)
                {
                    break;
                }
                if (_plcClient.ReadUnloadSlot(15, out _slotNumber))
                {
                    IsReadOk = true;
                    ClientUICommon.RefreshRackView(CurPartResult, _slotNumber);
                    break;
                }
                Thread.Sleep(1500);
            }
        }

        public void RefreshSlots(ResultRecord partResult)
        {
            //_timer.Start();
            Perform();
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
