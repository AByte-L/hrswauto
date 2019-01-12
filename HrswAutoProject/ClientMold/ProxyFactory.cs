using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.ICmmServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ClientMold
{
    public class ProxyFactory
    {
        /// <summary>
        /// 通道工厂, 保持工厂以接受新的代理连接
        /// </summary>
        private DuplexChannelFactory<ICmmControl> CmmControlFactory;
        /// <summary>
        /// 文件操作通道工厂
        /// </summary>
        private ChannelFactory<IPartConfigService> PartConfigServiceFactory;

        public ProxyFactory(CmmClient cmmClient)
        {
            // 三坐标控制代理工厂
            NetTcpBinding cmmBinding = new NetTcpBinding(SecurityMode.None);
            CSFeedback events = new CSFeedback(cmmClient);
            CmmControlFactory = new DuplexChannelFactory<ICmmControl>(new InstanceContext(events), cmmBinding);
            CmmControlFactory.Open();
            // 文件传输代理工厂
            NetTcpBinding fileBinding = new NetTcpBinding(SecurityMode.None);
            fileBinding.TransferMode = TransferMode.Streamed;
            fileBinding.MaxBufferSize = 65536;
            fileBinding.MaxReceivedMessageSize = 0x7fffffff;
            PartConfigServiceFactory = new ChannelFactory<IPartConfigService>(fileBinding);
            PartConfigServiceFactory.Open();
        }
        /// <summary>
        /// 获取控制代理端口
        /// </summary>
        /// <param name="_machineId">机器号</param>
        /// <param name="_ipAddress">IP地址</param>
        /// <returns></returns>
        public ICmmControl GetCmmControl(CmmServerConfig csInfo)
        {
            //int servicePort = 8000 + clientInfo._machineId; // 端口
            string serviceUri = @"net.tcp://" + csInfo.HostIPAddress + ":" + csInfo.ControlPost.ToString() + @"/cmmcontrolservice";
            ICmmControl cmmControl = null;
            cmmControl = CmmControlFactory.CreateChannel(new EndpointAddress(serviceUri));
            //    连接回调事件
            cmmControl.ConnectWFEvents();
            return cmmControl;
        }

        public IPartConfigService GetPartConfigService(CmmServerConfig clientInfo)
        {
            //int servicePort = 9000 + clientInfo._machineId; // 端口
            string serviceUri = @"net.tcp://" + clientInfo.HostIPAddress + ":" + clientInfo.PartConfigPost.ToString() + @"/partconfigservice";
            IPartConfigService fileServiceProxy = null;
            fileServiceProxy = PartConfigServiceFactory.CreateChannel(new EndpointAddress(serviceUri));
            return fileServiceProxy;
        }
    }
}
