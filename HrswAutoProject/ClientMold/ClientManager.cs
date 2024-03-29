﻿using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.PLCMold;
using Gy.HrswAuto.UICommonTools;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Serialization;

namespace Gy.HrswAuto.MasterMold
{
    public class ClientManager
    {
        List<CmmClient> _cmmClients;
        private List<CmmServerConfig> _cmmSvrConfigs;
        public List<CmmServerConfig> CmmSvrConfigs
        {
            get { return _cmmSvrConfigs; }
            set { _cmmSvrConfigs = value; }
        }
        public int CmmCount
        {
            get
            {
                return _cmmClients.Count;
            }
        }
        public string ClientConfigFileName { get; set; } = "clients.xml";

        private Timer _dispatchTimer; // 任务分派
        private Timer _heartbeatTimer; // 心跳信号

        public double DispatchInterval { get; set; } = 3; // 
        #region 客户端管理器初始化
        public void Initialize()
        {
            // 导入client
            string path = Path.Combine(PathManager.Instance.GetSettingsPath(), ClientConfigFileName);
            if (File.Exists(path))
            {
                LoadClientFromXmlFile(path);
            }
            else
            {
                _cmmSvrConfigs = new List<CmmServerConfig>();
            }
            // 循环初始化client
            foreach (var config in CmmSvrConfigs)
            {
                CmmClient client = new CmmClient(config);
                client.IsActived = true;
                //client.InitClient(); // 
                // 更新UI界面
                ClientUICommon.AddCmmToView(config, client.State);

                _cmmClients.Add(client);
            }
            // 设置心跳信号
            PlcClient.Instance.DisconnectEvent += Plc_DisconnectEvent;
            PlcClient.Instance.ReconnectEvent += Plc_ReconnectEvent;
            _heartbeatTimer.Start();
        }

        private void Plc_ReconnectEvent(object sender, EventArgs e)
        {
            _plcHeartbeat = true;
        }

        private void Plc_DisconnectEvent(object sender, EventArgs e)
        {
            _plcHeartbeat = false;
        }

        /// <summary>
        /// 添加客户端
        /// </summary>
        /// <param name="csConf"></param>
        /// <returns>返回false表示客户端已存在，返回true添加成功</returns>
        public bool AddClient(CmmServerConfig csConf)
        {
            // 判断机器是否已经存在
            if (_cmmSvrConfigs.Where(cl => cl.HostIPAddress.Equals(csConf.HostIPAddress)).Count() == 0 && _cmmSvrConfigs.Where(cl => cl.ServerID == csConf.ServerID).Count() == 0)
            {
                _cmmSvrConfigs.Add(csConf); //  添加机器配置信息
                CmmClient client = new CmmClient(csConf);
                client.IsActived = true;
                //client.InitClient(); // 添加client时不做初始化
                _cmmClients.Add(client);
                //
                ClientUICommon.AddCmmToView(csConf, client.State);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 连接三坐标服务器
        /// </summary>
        public void InitClients()
        {
            //foreach (CmmClient client in _cmmClients)
            for (int i = 0; i < _cmmClients.Count; i++)
            {
                if (_cmmClients[i].Connected)
                {
                    continue;
                }
                _cmmClients[i].InitClient();
                ClientUICommon.RefreshCmmViewState(i, _cmmClients[i].State); // 初始化是否成功
            }
        }

        public void CmmConnect(int index)
        {
            if (_cmmClients[index].Connected)
            {
                return;
            }
            _cmmClients[index].InitClient();
            ClientUICommon.RefreshCmmViewState(index, _cmmClients[index].State);
        }

        public void RunDispatchTask()
        {
            _dispatchTaskEnable = true;
            _dispatchTimer.Start();
        }

        public void PauseDispatchTask()
        {
            _dispatchTaskEnable = false;
            _dispatchTimer.Stop();
        }

        private void _dispatchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _dispatchTimer.Stop();
            foreach (var client in _cmmClients)
            {
                if (!client.IsActived) // 未激活不响应自动事件
                    break;
                switch (client.State)
                {
                    case ClientState.CS_Idle: // 三坐标空闲，可以上料
                        if (_plcHeartbeat)
                        {
                            client.State = ClientState.CS_Busy;
                            client.StartWorkFlow();
                        }
                        break;
                    case ClientState.CS_MeasCompleted: // 三坐标测量完成
                        client.State = ClientState.CS_Busy;
                        client.PullReport(); // 下载报告文件
                        break;
                    case ClientState.CS_Continue: // 调试使用
                        if (_plcHeartbeat)
                        {
                            client.State = ClientState.CS_Busy;
                            client.SendGripRequest(); 
                        }
                        break;
                    case ClientState.CS_GripCompleted: // 抓取完成
                        if (_plcHeartbeat)
                        {
                            client.State = ClientState.CS_Busy;
                            client.SendPlaceRequest(); 
                        }
                        break;
                    case ClientState.CS_PlaceCompleted: // 放置完成
                        if (_plcHeartbeat)
                        {
                            client.State = ClientState.CS_Busy;
                            client.StartMeasureWork();
                        }
                        break;
                    case ClientState.CS_Busy: // 忙碌状态
                        // todo 更新进度条？
                        break;
                    case ClientState.CS_Place: // 放置请求
                        if (_plcHeartbeat)
                        {
                            client.State = ClientState.CS_Busy;
                            client.SendPlaceRequest();
                        }
                        break;
                    case ClientState.CS_Grip: // 抓取请求
                        if (_plcHeartbeat)
                        {
                            client.State = ClientState.CS_Busy;
                            client.SendGripRequest();
                        }
                        break;
                    case ClientState.CS_Error:
                        break;
                    case ClientState.CS_InitError:
                        break;
                    case ClientState.CS_ConnectError:
                        break;
                    case ClientState.CS_RobotGripError:
                        break;
                    case ClientState.CS_RobotPlaceError:
                        break;
                    case ClientState.CS_None:
                        break;
                    case ClientState.CS_PlcConnectError:
                        break;
                    default:
                        break;
                }
            }
            if (_dispatchTaskEnable)
            {
                _dispatchTimer.Start();
            }
        }

        public void LoadClientFromXmlFile(string path)
        {
            using (XmlReader reader = new XmlTextReader(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CmmServerConfig>));
                _cmmSvrConfigs = serializer.Deserialize(reader) as List<CmmServerConfig>;
            }
        }

        public void SaveClientToXmlFile(string path)
        {
            using (XmlWriter writer = new XmlTextWriter(path, Encoding.UTF8))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<CmmServerConfig>));
                serializer.Serialize(writer, _cmmSvrConfigs);
            }
        }

        public void SaveCmmServer()
        {
            string path = PathManager.Instance.GetSettingsPath();
            path = Path.Combine(path, ClientConfigFileName);
            SaveClientToXmlFile(path);

        }
        #endregion

        #region CreateMethod
        private static ClientManager _clientManager;

        public void CloseHeartbeat()
        {
            _heartbeatTimer.Close();
        }

        private bool _dispatchTaskEnable;
        private bool _plcHeartbeat;

        private ClientManager()
        {
            _cmmClients = new List<CmmClient>();
            _dispatchTimer = new Timer((DispatchInterval > 3 ? DispatchInterval : 3) * 1000); // 刷新间隔不小于3s
            _dispatchTimer.Elapsed += _dispatchTimer_Elapsed;

            _heartbeatTimer = new Timer(3000);
            _heartbeatTimer.Elapsed += _heartbeatTimer_Elapsed;
            _dispatchTaskEnable = true;
            _plcHeartbeat = true;
        }

        /// <summary>
        /// 三坐标连接心跳信号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _heartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _heartbeatTimer.Stop();
            // 没有三坐标
            //if (_cmmClients.Count == 0)
            //{
            //    return;
            //}
            try
            {
                for (int i = 0; i < _cmmClients.Count; i++)
                {
                    bool oldConnected = _cmmClients[i].Connected;
                    _cmmClients[i].QueryCmmServer();
                    if (!_cmmClients[i].Connected)
                    {
                        _cmmClients[i].State = ClientState.CS_ConnectError;
                        //ClientUICommon.RefreshCmmStatus(i);
                        if (oldConnected)
                        {
                            ClientUICommon.RefreshCmmViewState(i, _cmmClients[i].State);
                            ClientUICommon.RefreshCmmEventLog($"三坐标{_cmmClients[i].CmmSvrConfig.ServerID} 连接错误。");
                            oldConnected = false;
                        }
                        // todo 这里有调试错误
                    }
                }
            }
            catch (Exception)
            {
                // todo 主界面退出后可能还会启动几轮心跳信号 
            }

            //_cmmClients.ForEach(cmm =>
            //{
            //});
            _heartbeatTimer.Start();
        }

        public bool CmmConnected(int index)
        {
            if (index > (_cmmClients.Count - 1))
            {
                return true;
            }
            return _cmmClients[index].Connected;
        }

        public static ClientManager Instance
        {
            get
            {
                if (_clientManager == null)
                {
                    _clientManager = new ClientManager();
                }
                return _clientManager;
            }
        }

        public bool PlcConnected
        {
            set
            {
                _plcHeartbeat = value;
            }
        }



        #endregion
        public void EnableClient(int index)
        {
            _cmmClients[index].IsActived = true;
        }

        public void DisableClient(int index)
        {
            _cmmClients[index].IsActived = false;
        }

        public void DeleteClient(int index)
        {
            if (index < 0)
            {
                return;
            }
            _cmmClients.RemoveAt(index);
            CmmSvrConfigs.RemoveAt(index);
        }

        public bool CmmActived(int index)
        {
            return _cmmClients[index].IsActived;
        }

        public ClientState CmmState(int index)
        {
            return _cmmClients[index].State;
        }

        public void ClearCmmError(int index)
        {
            if (_cmmClients[index].State == ClientState.CS_InitError)
            {
                if (_cmmClients[index].CmmIsInitialed())
                {
                    _cmmClients[index].State = ClientState.CS_Idle;
                    string str = "三坐标" + _cmmClients[index].CmmSvrConfig.ServerID.ToString() + "错误恢复";
                    ClientUICommon.RefreshCmmEventLog(str);
                }
            }
            if (_cmmClients[index].State == ClientState.CS_Error)
            {
                _cmmClients[index].State = ClientState.CS_Idle;
                string str = "三坐标" + _cmmClients[index].CmmSvrConfig.ServerID.ToString() + "错误恢复";
                ClientUICommon.RefreshCmmEventLog(str);
            }
            // PLC连接错误
            //if (!_plcHeartbeat)
            //{
            //    _cmmClients.ForEach(client =>
            //    {
            //        client.State = ClientState.CS_Idle;
            //        string str = "三坐标服务器" + "复位";
            //        ClientUICommon.RefreshCmmEventLog(str);
            //    });
            //}
        }
    }
}
