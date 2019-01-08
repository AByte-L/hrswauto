using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    /// <summary>
    ///  表示检测完成的零件数据模型
    /// </summary>
    [DataContract]
    public class PartDataModel
    {
        [DataMember]
        public string RptFileName { get; set; }
        [DataMember]
        public string CmmFileName { get; set; }
        /// <summary>
        /// 检测结果是否合格
        /// </summary>
        [DataMember]
        public bool IsPass { get; set; }
    }
}
