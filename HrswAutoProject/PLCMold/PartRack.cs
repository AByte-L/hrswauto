using Gy.HrswAuto.DataMold;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.PLCMold
{
    public class PartRack
    {
        public int SlotNumber { get; set; }
        public ResultRecord CurPartResult { get; set; }

        public void RefreshSlots(ResultRecord partResult)
        {

        }
    }
}
