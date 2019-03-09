using Snap7;
using System.Diagnostics;
using System.Threading;
using System;
using System.Collections.Generic;
using Gy.HrswAuto.UICommonTools;
using System.Threading.Tasks;

namespace Gy.HrswAuto.PLCMold
{
    public class PlcClient
    {
        private object syncObj = new object();
        private System.Timers.Timer _hbTimer;
        private bool _isConnected;
        private bool _reConnect;

        // 心跳中断事件
        public event EventHandler DisconnectEvent;
        public event EventHandler ReconnectEvent;
        // S7 
        private S7Client _s7Client;
        public string PlcIPAddress { get; set; } = "192.168.100.1";
        public int Rack { get; set; } = 0;
        public int Slot { get; set; } = 0;
        public bool IsConnected
        {
            get
            {
                return _s7Client.Connected();
            }
        }

        public bool SendGripRequest(int clientID, bool isPassed)
        {
            return SetGripFlag(clientID, isPassed);
        }


        public bool SendPlaceRequest(int clientID)
        {
            return SetPlaceFlag(clientID);
        }

        public bool ResponsePlaceAndGripFeedRequest(int clientID, bool isPassed)
        {
            return SetAllFlag(clientID, isPassed);
        }

        public bool ReadUnloadFlag()
        {
            lock (syncObj)
            {
                byte[] buf = new byte[1];
                int result = _s7Client.DBRead(15, 0, 1, buf);
                Thread.Sleep(100);
                if (result == 0)
                {
                    return S7.GetBitAt(buf, 0, 0);
                }
                else
                {
                    return false;
                }
            }
        }

        public bool VerifyWirteIDCompleted()
        {
            lock (syncObj)
            {
                byte[] buf = new byte[1];
                int result = _s7Client.MBRead(0, 1, buf);
                Thread.Sleep(100);
                if (result == 0)
                {
                    return S7.GetBitAt(buf, 0, 0);
                }
                else
                {
                    // todo 更新界面
                    return false;
                }
            }
        }

        internal bool ReadSlotNumber(out int slotNumber)
        {
            bool ok = true;
            lock (syncObj)
            {
                byte[] buf = new byte[2];
                int result = _s7Client.DBRead(15, 2, 2, buf);
                Thread.Sleep(100);
                if (ok = (result == 0))
                {
                    slotNumber = S7.GetIntAt(buf, 0);
                }
                else
                {
                    slotNumber = 0;
                 }
                return ok;
            }
        }

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
                        Trace.Write("PLC 读取错误");
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
                        Trace.Write("PLC 写入错误");
                        return false;
                    }
                }
            }
            return true;
        }

        public bool SetAllFlag(int clientID, bool isPassed)
        {
            int[] bn = new int[3];
            bn[0] = 0;
            bn[1] = 1;
            bn[2] = isPassed ? 2 : 3;
            return SetPlcFlags(clientID, 0, true, bn);
        }

        public bool ResetAllOkFlag(int clientId)
        {
            int[] bn = { 0, 1 };
            return SetPlcFlags(clientId, 1, false, bn);
        }
        

        /// <summary>
        /// 清除上料完成标志
        /// </summary>
        /// <param name="clientId"></param>
        public bool ResetPlaceOkFlag(int clientId)
        {
            int[] bn = { 0 };
            return SetPlcFlags(clientId, 1, false, bn);
        }
        /// <summary>
        /// 清除抓取完成标志
        /// </summary>
        /// <param name="clientId"></param>
        public bool ResetGripOkFlag(int clientId)
        {
            int[] bn = { 1 };
            return SetPlcFlags(clientId, 1, false, bn);
        }
        /// <summary>
        /// 设置上料请求标志
        /// </summary>
        /// <param name="clientId"></param>
        public bool SetPlaceFlag(int clientId)
        {
            int[] bn = { 0 };
            return SetPlcFlags(clientId, 0, true, bn);
        }
        /// <summary>
        /// 设置下料请求标志
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="Pass"></param>
        public bool SetGripFlag(int clientId, bool Pass)
        {
            int[] bn = new int[2];
            bn[0] = 1;
            bn[1] = Pass ? 2 : 3;
            return SetPlcFlags(clientId, 0, true, bn); // 设置合格位
            //int[] bn1 = { 3 };
            //SetPlcFlags(clientId, 0, true, bn1); // 设置抓取位
        }

        public bool SetWriteIDFlag(int clientId)
        {
            int[] bn = { 4 };
            return SetPlcFlags(clientId, 0, true, bn);
        }

        public bool SetIDErrorFlag(int clientId)
        {
            int[] bn = { 5 };
            return SetPlcFlags(clientId, 0, true, bn);
        }

        public bool SetPartID(int clientId, string partId)
        {
            int result = 1;
            byte[] buf = new byte[256];
            S7.SetStringAt(buf, 0, 256, partId);
            lock(syncObj)
            {
                if (_s7Client.Connected())
                {
                    result = _s7Client.DBWrite(clientId, 2, 256, buf);
                    Thread.Sleep(100); // 等待数据交换
                                       // todo 更新状态条
                                       //Debug.Assert(result == 0); 
                }
            }
            return result == 0;
        }


        public string ReadPartID(int clientId)
        {
            byte[] buf = new byte[256];
            lock (syncObj)
            {
                int result = _s7Client.DBRead(clientId, 2, 256, buf);
                Thread.Sleep(100); // 等待数据交换
                Debug.Assert(result == 0);
            }
            return S7.GetStringAt(buf, 0);
        }

        /// <summary>
        /// 验证下料完成标志
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool VerifyGripCompleted(int clientId)
        {
            byte[] buf = new byte[1];
            if (ReadData(clientId, 1, 1, buf))
            {
                if (S7.GetBitAt(buf, 0, 1))
                {
                    ResetGripOkFlag(clientId); // 重置标志
                    return true;
                }
            }
            return false;
        }

        public bool VerifyPlaceCompleted(int clientId, out string partID)
        {
            byte[] buf = new byte[1];
            partID = string.Empty;
            if (ReadData(clientId, 1, 1, buf))
            {
                if (S7.GetBitAt(buf, 0, 0))
                {
                    partID = S7.GetStringAt(buf, 2); // 获取partID;
                    ResetGripOkFlag(clientId); // 重置标志
                    Thread.Sleep(100);
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 设置PLC存储器字节位
        /// </summary>
        /// <param name="clientId">存储器号</param>
        /// <param name="byteOffset">字节偏置</param>
        /// <param name="value">1or0</param>
        /// <param name="bitNbs">要设置的位数组</param>
        private bool SetPlcFlags(int clientId, int byteOffset, bool value, params int[] bitNbs)
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
                //Debug.Assert(result == 0);
                return result == 0;
            }
        }

        #region 初始化PLC连接
        //Timer _initTimer;
        AutoResetEvent _initEvent;

        public bool Initialize()
        {
            // 连接时效15s
            //_initTimer = new Timer(new TimerCallback(InitPlcConnect), null, 1000, 2000);
            CancellationTokenSource cs = new CancellationTokenSource();
            Task.Factory.StartNew(InitPlcConnect, cs.Token, cs.Token);

            bool result = _initEvent.WaitOne((int)TimeSpan.FromSeconds(15).TotalMilliseconds);

            if (!result)
            {
                ClientUICommon.RefreshPlcConnectState("PLC连接失败");
                return false;
            }
            cs.Cancel();
            ClientUICommon.RefreshPlcConnectState("PLC连接成功");
            //连接成功开启心跳信号
            _hbTimer.Start();
            return result;
        }

        private void InitPlcConnect(object state) 
        {
            CancellationToken token = (CancellationToken)state;
            int result = _s7Client.ConnectTo(PlcIPAddress, Rack, Slot);
            Thread.Sleep(10); // 很小的复位时间
            while(true)
            {
                if (token.IsCancellationRequested)
                {
                    return ;
                }
                if (result == 0 && _s7Client.Connected())
                {
                    _initEvent.Set();
                    break;
                }
                Thread.Sleep(2000); // 等待2s延时连接
                result = _s7Client.Connect();
                Thread.Sleep(10);
            }
        } 
        #endregion

        #region 创建方法
        private PlcClient()
        {
            _initEvent = new AutoResetEvent(false);
            _s7Client = new S7Client();
            // 心跳信号实现
            _isConnected = false;
            _reConnect = true;
            _hbTimer = new System.Timers.Timer(1000);
            _hbTimer.Elapsed += _hbTimer_Elapsed;

        }
        /// <summary>
        /// PLC心跳事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _hbTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _hbTimer.Stop();
            if (!_s7Client.Connected())
            {
                // 如果是刚刚断开，触发事件
                if (_isConnected) 
                {
                    DisconnectEvent?.Invoke(this, null); // 包括更新Mainform
                    _isConnected = false;
                }
                else
                {
                    // 试着进行的重新连接
                    if (_reConnect)
                    {
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        while (true)
                        {
                            if (sw.Elapsed > TimeSpan.FromSeconds(5))
                            {
                                _reConnect = false;
                                break;
                            }
                            int result = _s7Client.Connect();
                            // 连接成功
                            if ((result == 0) && _s7Client.Connected())
                            {
                                ReconnectEvent?.Invoke(this, null);
                            }
                        }
                        sw.Stop(); 
                    }
                }
            }
            _hbTimer.Start();
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
        public void SetConnectParam(string ipAddress, int v1, int v2)
        {
            PlcIPAddress = ipAddress;
            Rack = v1;
            Slot = v2;
        }

        public void DisconnectPLC()
        {
            if (_s7Client.Connected())
            {
                // 不判断断开情况
                int result = _s7Client.Disconnect();
                Thread.Sleep(100);
            }
            _hbTimer.Dispose();
            _hbTimer = null;
        }
    }
}
