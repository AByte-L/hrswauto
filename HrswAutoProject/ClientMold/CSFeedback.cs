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
            // todo 刷新界面显示
        }

        public void ServerWorkStatus(string message)
        {
            // todo 刷新界面显示
        }

        public void WorkCompleted(bool isPass) // todo 完成之前，需要与服务器协调报告处理方式
        {
            // 由ClientManager循环处理
            _cmmClient.CurPartIsPass = isPass;
            _cmmClient.State = ClientState.CS_Continue;
            // 调试时使用
            //_cmmClient.WorkContinue();
        }
    }
}
