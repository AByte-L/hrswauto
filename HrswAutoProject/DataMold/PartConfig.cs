using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    /// <summary>
    /// 包含工件ID，测量程序及分析用文件的配置
    /// </summary>
    [DataContract]
    public class PartConfig
    {
        [DataMember]
        public string PartID { get; set; }
        [DataMember]
        public string NormFileName { get; set; }
        [DataMember]
        public string FlvFileName { get; set; }
        [DataMember]
        public string TolFileName { get; set; }
        [DataMember]
        public string ProgFileName { get; set; }
        //[DataMember]
        //public string MathFileName { get; set; }
        //[DataMember]
        //public string OpFileName { get; set; }

        //[DataMember]
        //public List<string> FileNames { get; set; } = new List<string>();
    }
}
