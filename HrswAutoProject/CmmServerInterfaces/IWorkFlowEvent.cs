using System.ServiceModel;

namespace Gy.HrswAuto.ICmmServer
{
    public interface IWorkflowNotify
    {
        [OperationContract(IsOneWay = true)]
        void WorkCompleted(bool isPass);
    }
}