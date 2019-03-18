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
        public string PlcIPAddress { get; set; } = "192.168.0.1";
        public int Rack { get; set; } = 0;
        public int Slot { get; set; } = 0;
        public bool IsConnected
        {
            get
            {
                //return _s7Client.Connected();
                return _isConnected;
            }
        }

        #region 发送请求标志
        /// <summary>
        /// 设置三坐标准备就绪信号
        /// </summary>
        /// <param name="serverID"></param>
        /// <returns></returns>
        public bool SendCmmReadySignal(int serverID)
        {
            int[] pos = new int[1] { 0 }; // 第0位
            bool result = WritePlcFlags(serverID, 0, true, pos);
            return result;
        }

        /// <summary>
        /// 发送下料请求
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="isPassed"></param>
        /// <returns></returns>
        public bool SendGripRequest(int clientID, bool isPass)
        {
            bool result = false;
            if (isPass) // 合格
            {
                result = WritePlcFlags(clientID, 0, true, new int[] { 2, 3 });
            }
            else
            {
                result = WritePlcFlags(clientID, 0, true, new int[] { 2, 4 });
            }
            return result;
        }

        /// <summary>
        /// 发送上料请求
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public bool SendPlaceRequest(int clientID)
        {
            bool result = WritePlcFlags(clientID, 0, true, new int[] { 1 });
            return result;
        } 
        #endregion

        #region 验证请求动作完成
        /// <summary>
        /// 验证下料完成标志
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public bool VerifyGripCompleted(int clientId)
        {
            bool result = false;
            byte[] buf = new byte[1];
            result = ReadData(clientId, 0, 1, buf);
            if (result)
            {
                result = S7.GetBitAt(buf, 0, 6);
                if (result)
                {
                    // 重置标志位
                    result = WritePlcFlags(clientId, 0, false, new int[] { 6 });
                }
            }
            return result;
        }

        /// <summary>
        /// 确认上料过程中，并读取RFID
        /// </summary>
        /// <param name="serverID"></param>
        /// <param name="partID"></param>
        /// <returns></returns>
        public bool VerifyDuringPlacement(int serverID, out string partID)
        {
            bool result = false;
            byte[] buf = new byte[1];
            partID = string.Empty;
            if (result = ReadData(serverID, 0, 1, buf)) 
            {
                if (result = S7.GetBitAt(buf, 0, 7)) // 确认上料过程中标志
                {
                    byte[] str = new byte[256];
                    result = ReadData(47, 0, 256, str);
                    if (result)
                    {
                        partID = S7.GetStringAt(str, 0);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 验证上料完成
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="partID"></param>
        /// <returns></returns>
        public bool VerifyPlaceCompleted(int clientId)
        {
            bool result = false;
            byte[] buf = new byte[1];
            if (result = ReadData(clientId, 0, 1, buf))
            {
                result = S7.GetBitAt(buf, 0, 5);
                //if (result = S7.GetBitAt(buf, 0, 5))
                //{
                //    if (result)
                //    {
                //        partID = S7.GetStringAt(buf, 2); // 获取partID;
                //        S7.SetBitAt(ref buf, 0, 5, false);
                //        S7.SetCharsAt(buf, 2, "");
                //        result = WriteData(clientId, 0, 258, buf);// 重置标志和PartID
                //    }
                //}
            }
            return result;
        }

        /// <summary>
        /// 确认槽标志及槽号
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        public bool ReadUnloadSlot(int Dbnmb, out int slot)
        {
            byte[] buf = new byte[4];
            bool result = ReadData(Dbnmb, 0, 4, buf);
            slot = -1;
            if (result)
            {
                result = S7.GetBitAt(buf, 0, 0);
                if (result)
                {
                    slot = S7.GetIntAt(buf, 2);
                }
            }
            return result;
        }

        #endregion

        #region PLC读写方法
        /// <summary>
        /// 写PLC标志
        /// </summary>
        /// <param name="serverID">存储器号</param>
        /// <param name="offset">标志位字节偏置</param>
        /// <param name="value">设置或者复位</param>
        /// <param name="pos">要设置的字节位位置</param>
        /// <returns></returns>
        private bool WritePlcFlags(int serverID, int offset, bool value, params int[] pos)
        {
            lock (syncObj)
            {
                byte[] buf = new byte[1];
                int result = _s7Client.DBRead(serverID, offset, 1, buf);
                PlcUILinker.Log($"WritePlcFlags DB{serverID} off{offset}-Read", _s7Client.ErrorText(result));
                Thread.Sleep(100);
                if (result != 0)
                {
                    return false;
                }
                foreach (int index in pos)
                {
                    S7.SetBitAt(ref buf, 0, index, value);
                }
                result = _s7Client.DBWrite(serverID, offset, 1, buf);
                PlcUILinker.Log($"WritePlcFlags DB{serverID} off{offset}-Write", _s7Client.ErrorText(result));
                Thread.Sleep(100);
                //Debug.Assert(result == 0);
                return result == 0;
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
            lock (syncObj)
            {
                if (_s7Client.Connected())
                {
                    int result = _s7Client.DBRead(DBNumber, start, size, buf);
                    PlcUILinker.Log($"ReadData DB{DBNumber} off{start}-Read", _s7Client.ErrorText(result));
                    Thread.Sleep(200); // 等待数据交换
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
                    PlcUILinker.Log($"WriteData DB{dbNb} off{start}-Write", _s7Client.ErrorText(result));
                    Thread.Sleep(200); // 等待数据交换
                    if (result != 0)
                    {
                        Trace.Write("PLC 写入错误");
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region 写工件ID
        public bool SetWriteIDFlag(int clientId)
        {
            // Byte 0.0
            return WritePlcFlags(clientId, 256, true, new int[] { 0 });
        }

        public bool SetPartID(int clientId, string partId)
        {
            int result = 1;
            byte[] buf = new byte[256];
            S7.SetStringAt(buf, 0, 256, partId);
            lock (syncObj)
            {
                if (_s7Client.Connected())
                {
                    result = _s7Client.DBWrite(clientId, 0, 256, buf);
                    PlcUILinker.Log($"SetPartID DB{clientId} off0-256DBWrite", _s7Client.ErrorText(result));
                    if (result != 0)
                    {
                        return false;
                    }
                    Thread.Sleep(200); // 等待数据交换
                }
            }
            return result == 0;
        }

        public bool VerifyWirteIDCompleted(int nmb)
        {
            byte[] buf = new byte[1];
            bool result = false;
            lock(syncObj)
            {
                if (_s7Client.Connected())
                {
                    result = ReadData(nmb, 256, 1, buf);
                    if (result)
                    {
                        result =S7.GetBitAt(buf, 0, 1);
                    }
                }
            }
            return result;
        }
        #endregion

        #region 初始化PLC连接
        //Timer _initTimer;
        AutoResetEvent _initEvent;

        public bool Initialize()
        {
            // 连接时效15s
            //_initTimer = new Timer(new TimerCallback(InitPlcConnect), null, 1000, 2000);
            _hbTimer.Stop();
            _initPlcConnect = false;
            CancellationTokenSource cs = new CancellationTokenSource();
            Task.Factory.StartNew(InitPlcConnect, cs.Token, cs.Token);

            bool result = _initEvent.WaitOne((int)TimeSpan.FromSeconds(15).TotalMilliseconds);

            if (!result)
            {
                ClientUICommon.RefreshPlcConnectState("PLC连接失败");
                _isConnected = false;
                return false;
            }
            _isConnected = true;
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
                    break;
                }
                if (result == 0/* && _s7Client.Connected()*/)
                {
                    _initEvent.Set();
                    ReconnectEvent?.Invoke(this, null);
                    _isConnected = true;
                    _initPlcConnect = true;
                    break;
                }
                Thread.Sleep(2000); // 等待2s延时连接
                result = _s7Client.Connect();
                Thread.Sleep(10);
            }
        } 

        public bool ReconnectPlc()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                // 等待5s的重连
                if (sw.Elapsed > TimeSpan.FromSeconds(5))
                {
                    break;
                }
                int result = _s7Client.ConnectTo(PlcIPAddress, Rack, Slot);
                Thread.Sleep(50);
                if (result == 0)
                {
                    ReconnectEvent?.Invoke(this, null);
                    _isConnected = true;
                    break;
                }
                Thread.Sleep(800);
            }
            sw.Stop();
            if (_isConnected)
            {
                _hbTimer.Start();
            }
            return _isConnected;
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
            int result = 1; // 不为0的正整数
            lock (syncObj)
            {
                result = _s7Client.DBRead(10, 0, 1, _hbbuf);
                // PLC CPU可达, 更新心跳时间
            }
            if (result == 0)
            {
                _hbNow = DateTime.Now;
            }

            if (_hbNow.AddSeconds(3) < DateTime.Now) //连接时长超过10s表示心跳信号中断
            {
                if (_isConnected) // 第一次连接中断
                {
                    DisconnectEvent?.Invoke(this, null); // 更新包括Mainform
                    _isConnected = false;
                    return;
                }
                //else // 试着重新连接
                //{
                //    if (_reConnect) // 自动进行5s的重连
                //    {
                //        Stopwatch sw = new Stopwatch();
                //        sw.Start();
                //        while (true)
                //        {
                //            if (sw.Elapsed > TimeSpan.FromSeconds(5))
                //            {
                //                _reConnect = false;
                //                break;
                //            }
                //            int connOk = _s7Client.Connect();
                //            // 连接成功
                //            if (connOk == 0)
                //            {
                //                ReconnectEvent?.Invoke(this, null);
                //                _isConnected = true;
                //            }
                //        }
                //        sw.Stop();
                //    }
                //}
            }
            if (_isConnected)
            {
                _hbTimer.Start(); 
            }
        }

        private static PlcClient _plcClient;
        private byte[] _hbbuf = new byte[1];
        private DateTime _hbNow;
        private bool _initPlcConnect;

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
