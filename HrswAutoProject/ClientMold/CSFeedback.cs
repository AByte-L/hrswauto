using Gy.HrswAuto.ICmmServer;
using Gy.HrswAuto.UICommonTools;
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
            // 刷新三坐标界面显示
            string str = "三坐标" + _cmmClient.CmmSvrConfig.ServerID.ToString() + "错误: " + message;
            ClientUICommon.RefreshCmmEventLog(str);
        }

        public void ServerWorkStatus(string message)
        {
            // 不更新三坐标状态
            // 刷新三坐标界面显示
            string str = "三坐标" + _cmmClient.CmmSvrConfig.ServerID.ToString() + ": " + message;
            ClientUICommon.RefreshCmmEventLog(str);
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
