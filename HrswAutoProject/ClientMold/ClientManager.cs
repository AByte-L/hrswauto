using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
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

        private Timer _dispatchTimer;
        public double DispatchInterval { get; set; } = 3; // 
        #region 客户端管理器初始化
        public void Initialize()
        {
            // 导入client
            string path = Path.Combine(PathManager.Instance.SettingsSavePath, SaveSettings.ClientConfigFileName);
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
                client.InitClient(); // 如果client初始化失败该怎么办？

                _cmmClients.Add(client);
            }
        }

        private void _dispatchTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (var client in _cmmClients)
            {
                switch (client.State)
                {
                    case ClientState.CS_Idle:
                        client.State = ClientState.CS_Busy;
                        client.StartWork();
                        break;
                    case ClientState.CS_Completed:
                        // 处理回传报告
                        client.State = ClientState.CS_Continue;
                        break;
                    case ClientState.CS_Busy:
                        // todo 刷新client状态显示
                        break;
                    case ClientState.CS_Error:
                        // todo 刷新client状态显示
                        break;
                    case ClientState.CS_Continue:
                        client.State = ClientState.CS_Busy;
                        client.Continue();
                        break;
                    default:
                        break;
                }
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
        #endregion

        #region 单件实现
        private static ClientManager _clientManager;
        private ClientManager()
        {
            _cmmClients = new List<CmmClient>();
            _dispatchTimer = new Timer((DispatchInterval>3? DispatchInterval:3) * 1000); // 刷新间隔不小于3s
            _dispatchTimer.Elapsed += _dispatchTimer_Elapsed;
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
        #endregion
    }
}
