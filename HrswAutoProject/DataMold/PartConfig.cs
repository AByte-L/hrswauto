using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    /// <summary>
    /// 包含工件ID，测量程序及分析用文件的配置
    /// </summary>
    [Serializable]
    public class PartConfig
    {
        public string PartID { get; set; }
        public string NormFileName { get; set; }
        public string FlvFileName { get; set; }
        public string TolFileName { get; set; }
        public string ProgFileName { get; set; }
        public string MathFileName { get; set; }
        public string OpFileName { get; set; }
    }
}
