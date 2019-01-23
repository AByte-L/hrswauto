using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProgramMonitor.Core;
using System.ServiceModel;

namespace ProgramMonitor.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, InstanceContextMode = InstanceContextMode.PerCall)]
    public class ListenService : IListenService
    {
        public void Start(ProgramInfo programInfo)
        {
            var listenCall = OperationContext.Current.GetCallbackChannel<IListenCall>();
            Common.SaveProgramStartInfo(programInfo, listenCall);
        }

        public void Stop(string programId)
        {
            Common.SaveProgramStopInfo(programId);
        }


        public void ReportRunning(ProgramInfo programInfo)
        {
            var listenCall = OperationContext.Current.GetCallbackChannel<IListenCall>();
            Common.SaveProgramRunningInfo(programInfo, listenCall);
        }
    }
}
