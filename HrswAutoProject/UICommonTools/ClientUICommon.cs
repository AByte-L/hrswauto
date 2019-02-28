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
    }
}
