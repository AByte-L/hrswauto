using Gy.HrswAuto.ICmmServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ClientMold
{
    /// <summary>
    /// 接受服务器通知
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class CSFeedback : IWorkflowNotify
    {
        private CmmClient _cmmClient;

        public CSFeedback(CmmClient cmmClient)
        {
            _cmmClient = cmmClient;
        }

        public void ServerInErrorStatus(string message)
        {
            _cmmClient.State = ClientState.CS_Error;
            _cmmClient.ErrorInfo = message;
            // todo 刷新三坐标界面显示
        }

        public void ServerWorkStatus(string message)
        {
            // 不更新三坐标状态
            // todo 刷新三坐标界面显示
        }

        public void WorkCompleted(bool isPass)
        {
            // 由ClientManager循环处理
            _cmmClient.IsPass = isPass; 
            _cmmClient.State = ClientState.CS_Completed;
            // 调试时使用
            //_cmmClient.WorkContinue();
        }
    }
}
