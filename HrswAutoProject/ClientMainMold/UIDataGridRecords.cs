using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMainMold
{

    public class PartResultRecord
    {
        public string SlotID { get; set; }
        public string PartID { get; set; }
        //public string SlotState { get; set; }
        public string PcProgram { get; set; }
        public string IsPass { get; set; }
        public string ReportFileName { get; set; }
        public string ReportFilePath { get; set; }
        public string RptFileName { get; set; }
    }

    public class CmmDataRecord
    {
        Dictionary<ClientState, string> cmmStateInfo = new Dictionary<ClientState, string>()
        {
            {
                ClientState.CS_Idle, "三坐标空闲"
            },
            {
                ClientState.CS_Completed, "工件检测完成"
            },
            {
                ClientState.CS_Continue, "等待继续测量"
            },
            {
                ClientState.CS_Error, "三坐标出错状态"
            },
            {
                ClientState.CS_Busy, "三坐标忙碌状态"
            }
        };
        public CmmDataRecord(CmmServerConfig conf, bool active, ClientState state)
        {
            IsActived = active;
            ServerID = conf.ServerID;
            IPAddress = conf.HostIPAddress;
            IsFault = state == ClientState.CS_Error ? true : false;
            State = cmmStateInfo[state];
        }
        public bool IsActived { get; set; }
        public int ServerID { get; set; }
        public string IPAddress { get; set; }
        public string State { get; set; }
        public bool IsFault { get; set; } = false;
        public Image StateImage
        {
            get
            {
                Image gridImage;
                if (IsFault)
                {
                    gridImage = Properties.Resources.Error;
                }
                else
                {
                    gridImage = Properties.Resources.ok;
                }
                return gridImage;
            }
        }
    }
}
