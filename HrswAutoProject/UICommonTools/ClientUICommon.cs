using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gy.HrswAuto.UICommonTools
{
    public class ClientUICommon
    {
        public static SynchronizationContext syncContext; // 同步上下文

        public static Action<CmmServerConfig, ClientState> AddCmmToView;

        public static Action<PartConfig, string> AddPartToView;

        public static Action<ResultRecord, int> RefreshRackView;

        public static Action<int, ClientState> RefreshCmmViewState; // 三坐标号、状态

        public static Action<ResultRecord> AddPartResult;

        public static Action<string> RefreshStatusInfomation; // 刷新状态条

        public static Action<string> RefreshCmmEventLogAction;

        public static Action<string> RefreshPlcConnectStateAction;

        public static Action<ResultRecord> AddCommonReportAction;

        public static void RefreshCmmEventLog(string cmmError)
        {
            RefreshCmmEventLogAction(cmmError);
        }

        public static void RefreshPlcConnectState(string v)
        {
            RefreshPlcConnectStateAction(v);
        }

        public static void AddCommonReport(ResultRecord _resultRecord)
        {
            AddCommonReportAction(_resultRecord);
        }
    }
}
