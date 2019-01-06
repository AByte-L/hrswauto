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
    public class PartConfig
    {
        private string _partID;

        public string PartID
        {
            get { return _partID; }
            set { _partID = value; }
        }

        private string _partBladeName;
        /// <summary>
        /// Blade分析工件需要使用的文件名，包括Flv, Norm, Tol文件
        /// 这些文件名除扩展名不一样其他都一致
        /// </summary>
        public string PartBlName
        {
            get { return _partBladeName; }
            set { _partBladeName = value; }
        }

        private string _partPath;
        /// <summary>
        /// Blade分析所使用的文件集路径
        /// </summary>
        public string PartPath
        {
            get { return _partPath; }
            set { _partPath = value; }
        }

        private string _partProgFileName;
        /// <summary>
        /// 当前零件测量程序名
        /// </summary>
        public string PartProgFileName
        {
            get { return _partProgFileName; }
            set { _partProgFileName = value; }
        }

    }
}
