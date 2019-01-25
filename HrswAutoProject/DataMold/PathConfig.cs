using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    [DataContract]
    public class PathSettings
    {
        [DataMember]
        public string RootPath { get; set; }
        [DataMember]
        public string BladeFilePath { get; set; }
        [DataMember]
        public string ProgFilePath { get; set; }
        [DataMember]
        public string TempFilePath { get; set; }
        [DataMember]
        public string ReportFilePath { get; set; }
        [DataMember]
        public string SettingsSavePath { get; set; }
    }
}
