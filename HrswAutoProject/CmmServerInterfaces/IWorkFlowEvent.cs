using System.ServiceModel;

namespace Gy.HrswAuto.CmmServerInterfaces
{
    public interface IWorkflowNotify
    {
        [OperationContract(IsOneWay = true)]
        void WorkCompleted();
    }
}