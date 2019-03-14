using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    public class ResultRecord
    {
        public string CmmFileName { get; set; }
        public string RptFileName { get; set; }
        public string FilePath { get; set; }
        public bool IsPass { get; set; }
        public string PartID { get; set; }
        public int PartNumber { get; set; }
        public int ServerID { get; set; }
        public DateTime MeasDateTime { get; set; }
    }
}