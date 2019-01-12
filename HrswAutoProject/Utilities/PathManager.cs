using Gy.HrswAuto.DataMold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.Utilities
{
    public class PathManager
    {
        // 通过应用程序设置配置目录
        public PathConfig Configration { get; set; }

        #region 单例实现
        private static PathManager _instance;

        public static PathManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PathManager();
                }
                return _instance;
            }
        } 
        #endregion
    }
}
