using Snap7;
using System.Diagnostics;
using System.Threading;
using System;
using System.Collections.Generic;

namespace Gy.HrswAuto.PLCMold
{
    public class PlcClient
    {
        private object syncObj = new object();
        // S7 
        private S7Client _s7Client;
        public string PlcIPAddress { get; set; } = "192.168.100.1";
        public int Rack { get; set; } = 0;
        public int Slot { get; set; } = 0;
        public bool IsConnected { get; set; } = false;

        Timer _initTimer;
        AutoResetEvent _initEvent;

        /// <summary>
        /// 读取大块存储器
        /// </summary>
        /// <param name="DBNumber"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool ReadData(int DBNumber, int start, int size, byte[] buf)
        {
            //byte[] buf = new byte[512];
            lock (syncObj)
            {
                if (_s7Client.Connected())
                {
                    int result = _s7Client.DBRead(DBNumber, start, size, buf);
                    Thread.Sleep(100); // 等待数据交换
                    if (result != 0)
                    {
                        Debug.WriteLine("PLC 读取错误");
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 写入大块存储器
        /// </summary>
        /// <param name="dbNb"></param>
        /// <param name="start"></param>
        /// <param name="size"></param>
        /// <param name="buf"></param>
        /// <returns></returns>
        public bool WriteData(int dbNb, int start, int size, byte[] buf)
        {
            lock (syncObj)
            {
                if (_s7Client.Connected())
                {
                    int result = _s7Client.DBWrite(dbNb, start, size, buf);
                    Thread.Sleep(100); // 等待数据交换
                    if (result != 0)
                    {
                        Debug.WriteLine("PLC 写入错误");
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 清除上料完成标志
        /// </summary>
        /// <param name="clientId"></param>
        public void ResetPlaceOkFlag(int clientId)
        {
            int[] bn = { 1 };
            SetPlcFlags(clientId, 0, false, bn);
        }
        /// <summary>
        /// 清除抓取完成标志
        /// </summary>
        /// <param name="clientId"></param>
        public void ResetGripOkFlag(int clientId)
        {
            int[] bn = { 3, 4, 5 };
            SetPlcFlags(clientId, 0, false, bn);
        }
        /// <summary>
        /// 设置上料请求标志
        /// </summary>
        /// <param name="clientId"></param>
        public void SetPlaceFlag(int clientId)
        {
            int[] bn = { 0 };
            SetPlcFlags(clientId, 0, true, bn);
        }
        /// <summary>
        /// 设置下料请求标志
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="Pass"></param>
        public void SetGripFlag(int clientId, bool Pass)
        {
            int[] bn = new int[1];
            bn[0] = Pass ? 4 : 5;
            SetPlcFlags(clientId, 0, true, bn); // 设置合格位
            int[] bn1 = { 3 };
            SetPlcFlags(clientId, 0, true, bn1); // 设置抓取位
        }

        public void SetWriteIDFlag(int clientId)
        {
            int[] bn = { 6 };
            SetPlcFlags(clientId, 0, true, bn);
        }

        public void SetIDErrorFlag(int clientId)
        {
            int[] bn = { 7 };
            SetPlcFlags(clientId, 0, true, bn);
        }

        public void SetPartID(int clientId, string partId)
        {
            byte[] buf = new byte[256];
            S7.SetStringAt(buf, 0, 256, partId);
            lock(syncObj)
            {
                int result = _s7Client.DBWrite(clientId, 2, 256, buf);
                Debug.Assert(result == 0);
            }
        }

        public string ReadPartID(int clientId)
        {
            byte[] buf = new byte[256];
            lock (syncObj)
            {
                int result = _s7Client.DBWrite(clientId, 2, 256, buf);
                Debug.Assert(result == 0);
            }
            return S7.GetStringAt(buf, 0);
        }

        /// <summary>
        /// 设置PLC存储器字节位
        /// </summary>
        /// <param name="clientId">客户号或存储器号</param>
        /// <param name="byteOffset">字节偏置</param>
        /// <param name="value">1or0</param>
        /// <param name="bitNbs">要设置的位数组</param>
        private void SetPlcFlags(int clientId, int byteOffset /* 字节偏置 */, bool value, params int[] bitNbs)
        {
            lock (syncObj)
            {
                byte[] buf = new byte[1];
                int result = _s7Client.DBRead(clientId, byteOffset, 1, buf);
                Thread.Sleep(100);
                Debug.Assert(result == 0);
                // 
                foreach (var bn in bitNbs)
                {
                    S7.SetBitAt(ref buf, 0, bn, false);
                }
                result = _s7Client.DBWrite(clientId, byteOffset, 1, buf);
                Thread.Sleep(100);
                Debug.Assert(result == 0);
            }
        }

        #region 初始化PLC连接
        public bool Initialize()
        {
            // 连接时效30s
            _initTimer = new Timer(new TimerCallback(InitPlcConnect), null, 1000, 2000);
            bool result = _initEvent.WaitOne((int)TimeSpan.FromSeconds(30).TotalMilliseconds);
            if (!result)
            {
                Debug.WriteLine("PLC连接失败");
                return false;
            }
            return true;
        }

        private void InitPlcConnect(object state)
        {
            int result = _s7Client.ConnectTo(PlcIPAddress, Rack, Slot);
            Thread.Sleep(10); // 很小的复位时间
            if (result == 0 && _s7Client.Connected())
            {
                _initEvent.Set();
            }
        } 
        #endregion

        #region 单例实现
        private PlcClient()
        {
            _initEvent = new AutoResetEvent(false);
            _s7Client = new S7Client();
        }

        private static PlcClient _plcClient;

        public static PlcClient Instance
        {
            get
            {
                if (_plcClient == null)
                {
                    _plcClient = new PlcClient();
                }
                return _plcClient;
            }
        } 
        #endregion
    }
}
