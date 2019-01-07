using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    public class PathConfig
    {
        public string RootPath { get; set; } = @"d:\CSItems";
        public string BladeFilePath { get; set; } = "Blades";
        public string ProgFilePath { get; set; } = "Programs";
        public string TempFilePath { get; set; } = "temp";
        public string ReportFilePath { get; set; } = "Reports";
        public string PartConfigSavePath { get; set; } = "PartConfigs";
    }
}
