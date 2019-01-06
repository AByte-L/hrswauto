using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.CmmServerInterfaces
{
    [ServiceContract(CallbackContract = typeof(IWorkflowNotify))]
    public interface ICmmControl
    {
        /// <summary>
        /// 启动工件测量，分析过程包含在后续的工作流程中，不需要远端控制器控制
        /// </summary>
        /// <param name="partId">工件标识，从PLC中读取</param>
        [OperationContract(IsOneWay = true)]
        void MeasurePart(string partId);
        [OperationContract(IsOneWay = true)]
        void ConnectWFEvents();
        [OperationContract(IsOneWay = true)]
        void DisconnectWFEvents();
    }
}
