using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #region 客户端管理器初始化
        public void Initialize()
        {
            // 导入client
            string path = Path.Combine(PathManager.Instance.AutoPathConfig.SettingsSavePath, SaveSettings.ClientConfigFileName);
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
                client.InitClient(); // 如果client初始化失败该怎么办？
                //if (client.IsInitialed)

                // PLC消息响应事件
                //client.OnGripRequestEvent += Client_OnGripRequestEvent;
                //client.OnPlaceRequestEvent += Client_OnPlaceRequestEvent;
                //client.OnPlaceAndGripRequestEvent += Client_OnPlaceAndGripRequestEvent;

                _cmmClients.Add(client);
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

        //private void Client_OnPlaceAndGripRequestEvent(object sender, FeedRequestArg e)
        //{
        //    CmmClient client = (CmmClient)sender;
        //    PlaceAndGripFeedRequest frq = new PlaceAndGripFeedRequest();
        //    frq.ClientID = e.ClientID;
        //    frq.IsPass = e.IsPassed;
        //    frq.PlaceActionCompletedEvent += FeedRequest_PlaceActionCompletedEvent;
        //    frq.Perform();
        //}

        //private void Client_OnPlaceRequestEvent(object sender, FeedRequestArg e)
        //{
        //    CmmClient client = (CmmClient)sender;
        //    //FeedRequest feedRqst = new FeedRequest();
        //    //feedRqst.ReqType = RequestType.Request_Place;
        //    //feedRqst.ClientID = client.CmmSvrConfig.ServerID;
        //    //feedRqst.Client = client;
        //    ////feedRqst.IsCompleted = false;
        //    //TaskDispatcher._taskQueue.Enqueue(feedRqst);
            
        //    PlaceFeedRequest frq = new PlaceFeedRequest();
        //    frq.ClientID = e.ClientID;
        //    frq.PlaceActionCompletedEvent += FeedRequest_PlaceActionCompletedEvent;
        //    frq.Perform();
            
        //}

        //private void Client_OnGripRequestEvent(object sender, FeedRequestArg e)
        //{
        //    CmmClient client = (CmmClient)sender;
        //    //FeedRequest feedRqst = new FeedRequest();
        //    ////feedRqst.ReqType = RequestType.Request_Grip;
        //    //feedRqst.ClientID = client.CmmSvrConfig.ServerID;
        //    //feedRqst.Client = client;
        //    ////feedRqst.IsCompleted = false;
        //    //TaskDispatcher._taskQueue.Enqueue(feedRqst);
        //    GripFeedRequest frq = new GripFeedRequest();
        //    frq.ClientID = e.ClientID;
        //    frq.IsPassed = e.IsPassed;
        //    frq.GripActionCompletedEvent += Frq_GripActionCompletedEvent;
        //    frq.Perform();
        //}

        //private void FeedRequest_PlaceActionCompletedEvent(object sender, ResponsePlcEventArgs e)
        //{
        //    if (_cmmClients[e.ClientID].FindPart(e.PartID))
        //    {
        //        // 设置工件标识错误
        //        _cmmClients[e.ClientID].SendPartIDError(e.PartID);
        //    }
        //    else
        //    {
        //        // 开始测量工作流程
        //        _cmmClients[e.ClientID].StartMeasureWorkFlow(e.PartID);
        //    }
        //}

        //private void Frq_GripActionCompletedEvent(object sender, ResponsePlcEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        #region 单件实现
        ClientManager _clientManager;
        private ClientManager()
        {
            _cmmClients = new List<CmmClient>();
        }

        public ClientManager Instance
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
