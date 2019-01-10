using Gy.HrswAuto.ICmmServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ClientMold
{
    /// <summary>
    /// 接受服务器通知
    /// </summary>
    //[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class CSFeedback : IWorkflowNotify
    {
        private CmmClient _cmmClient;

        public CSFeedback(CmmClient cmmClient)
        {
            _cmmClient = cmmClient;
        }

        public void WorkCompleted(bool isPass)
        {
            // cmmClient发送上下料请求
            _cmmClient.SendPlaceAndGripRequest(isPass);
        }
    }
}
