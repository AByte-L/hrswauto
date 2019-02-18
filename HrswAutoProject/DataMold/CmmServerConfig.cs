using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    [Serializable]
    public class CmmServerConfig
    {
        public int ServerID { get; set; }
        public int ControlPost { get; set; } = 6666;
        public int PartConfigPost { get; set; } = 7777;
        public string HostIPAddress { get; set; }
    }
}
