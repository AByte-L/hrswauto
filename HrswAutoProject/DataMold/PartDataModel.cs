using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.DataMold
{
    /// <summary>
    ///  表示检测完成的零件数据模型
    /// </summary>
    public class PartDataModel
    {
        private string _reportRptFileName;
        /// <summary>
        /// 检测报告文件
        /// </summary>
        public string RptFileName
        {
            get { return _reportRptFileName; }
            set { _reportRptFileName = value; }
        }

        /// <summary>
        /// 检测结果是否合格
        /// </summary>
        public bool IsPass { get; set; }
    }
}
