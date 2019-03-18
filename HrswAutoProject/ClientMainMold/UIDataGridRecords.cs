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
        //private PartResultRecord partResultRecord;

        public PartResultRecord()
        {

        }
        public PartResultRecord(PartResultRecord partResultRecord)
        {
            SlotID = partResultRecord.SlotID;
            PartID = partResultRecord.PartID;
            PartNumber = partResultRecord.PartNumber;
            ServerID = partResultRecord.ServerID;
            PcProgram = partResultRecord.PcProgram;
            IsPass = partResultRecord.IsPass;
            ReportFileName = partResultRecord.ReportFileName;
            ReportFilePath = partResultRecord.ReportFilePath;
            RptFileName = partResultRecord.RptFileName;
            MeasDateTime = partResultRecord.MeasDateTime;
        }

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
        public DateTime MeasDateTime { get; set; } // 日期-时间
      }

    public class CmmDataRecord
    {
        public static Dictionary<ClientState, string> cmmStateInfo = new Dictionary<ClientState, string>()
        {
            {
                ClientState.CS_Idle, "三坐标空闲"
            },
            {
                ClientState.CS_MeasCompleted, "工件检测完成"
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
            },
            {
                ClientState.CS_ConnectError, "三坐标连接错误"
            },
            {
                ClientState.CS_None, ""
            }
        };

        private ClientState _state;
        public void SetClientState(ClientState state)
        {
            _state = state;
        }

        public CmmDataRecord(CmmServerConfig conf, bool active, ClientState state)
        {
            IsActived = active;
            ServerID = conf.ServerID;
            ServerName = "三坐标 " + ServerID.ToString();
            IPAddress = conf.HostIPAddress;
            _state = state;
            IsFault = (state == ClientState.CS_Error) ? true : false;
            //State = cmmStateInfo[state];
        }
        public bool IsActived { get; set; }
        public int ServerID { get; set; }
        public string IPAddress { get; set; }
        public string ServerName { get; set; }
        public string State {
            get
            {
                return cmmStateInfo[_state];
            }
        }
        private bool _isFault = false;
        public bool IsFault
        {
            get
            {
                return _isFault;
            }
            set
            {
                _isFault = value;
                //if (_state == ClientState.CS_None)
                //{
                //    return;
                //}
                if (IsFault)
                {
                    _stateImage = Properties.Resources.Error;
                }
                else if (_state != ClientState.CS_Busy)
                {
                    _stateImage = Properties.Resources.ok;
                }
                else
                {
                    _stateImage = Properties.Resources.busy;
                }
            }
        }
        private Image _stateImage = new Bitmap(200,50); 
                
        public Image StateImage
        {
            get
            {
                return _stateImage; ;
            }
        }
    }

    public class PlcLog
    {
        public DateTime DTime { get; set; }
        public string PlcAction { get; set; }
        public string LogString { get; set; }
    }
}
