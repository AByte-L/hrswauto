using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientMainMold
{
    public class ResultViewRow
    {
        public int ID { get; set; }
        public string PartID { get; set; } = "";
        public string Program { get; set; } = "";
        public SlotState State { get; set; } = SlotState.S_NoPart; // 工作槽状态，有（检测，未检测），无
        public bool IsPassed { get; set; } = false; // 是否超差
    }

    public enum SlotState
    {
        S_NoPart,
        S_NoCheck,
        S_Check
    }
}
