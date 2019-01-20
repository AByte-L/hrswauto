using System.ServiceModel;

namespace Gy.HrswAuto.ICmmServer
{
    public interface IWorkflowNotify
    {
        [OperationContract(IsOneWay = true)]
        void WorkCompleted(bool isPass);
        [OperationContract(IsOneWay = true)]
        void ServerInErrorStatus(string message);
        [OperationContract(IsOneWay = true)]
        void ServerWorkStatus(string message);
    }
}