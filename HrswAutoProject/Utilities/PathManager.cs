using Gy.HrswAuto.DataMold;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.Utilities
{
    public class PathManager
    {
        // 通过应用程序设置配置目录
        //public PathConfig Configration { get; set; }

        public string SettingsSavePath { get; set; }
        public string RootPath { get; set; }
        public string PartProgramsPath { get; set; }
        public string BladesPath { get; set; }
        public string TempPath { get; set; }
        public string ReportsPath { get; set; }

        public string GetProgsFullPath()
        {
            return Path.Combine(RootPath, PartProgramsPath);
        }

        // 实际partId可能不是零件标识
        public string GetBladesFullPath(string partId)
        {
            return Path.Combine(RootPath, BladesPath, partId);
        }

        public string GetReportFullPath(string partId)
        {
            return Path.Combine(RootPath, BladesPath, partId, ReportsPath);
        }
        
        public string GetTempFullPath()
        {
            return Path.Combine(RootPath, TempPath);
        }

        public string GetPartNomPath(PartConfig partCnf)
        {
            return Path.Combine(GetBladesFullPath(partCnf.PartID), partCnf.NormFileName);
        }
        public string GetPartFlvPath(PartConfig partCnf)
        {
            return Path.Combine(GetBladesFullPath(partCnf.PartID), partCnf.FlvFileName);
        }

        public string GetPartTolPath(PartConfig partCnf)
        {
            return Path.Combine(GetBladesFullPath(partCnf.PartID), partCnf.TolFileName);
        }

        public string GetPartProgramPath(PartConfig partCnf)
        {
            return Path.Combine(GetProgsFullPath(), partCnf.ProgFileName);
        }

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
