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
        public string SlotID { get; set; } // 槽号
        public string PartID { get; set; } // 零件标识
        //public string SlotState { get; set; }
        public string PartNumber { get; set; } // 零件号
        public string ServerID { get; set; } // 服务器ID
        public string PcProgram { get; set; } // 测量程序
        public string IsPass { get; set; } // 是否合格
        public string ReportFileName { get; set; } // cmm报告文件
        public string ReportFilePath { get; set; } // 报告文件路径
        public string RptFileName { get; set; } // rpt结果文件
        public string MeasDateTime { get; set; } // 日期-时间
      }

    public class CmmDataRecord
    {
        public static Dictionary<ClientState, string> cmmStateInfo = new Dictionary<ClientState, string>()
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
            },
            {
                ClientState.CS_InitError, "三坐标初始化错误"
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
