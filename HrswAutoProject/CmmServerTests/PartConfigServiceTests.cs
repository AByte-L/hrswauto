using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gy.HrswAuto.CmmServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gy.HrswAuto.Utilities;
using Gy.HrswAuto.DataMold;
using System.IO;
using Gy.HrswAuto.ICmmServer;
using System.Windows.Forms;

namespace Gy.HrswAuto.CmmServer.Tests
{
    [TestClass()]
    public class PartConfigServiceTests
    {
        [TestMethod()]
        public void UpLoadFileTest()
        {
            PartConfigService pcnfService = new PartConfigService();
            PathConfig pc = new PathConfig();/*PathManager.Instance.Configration*/;
            PathManager.Instance.Configration = pc;
            pc.RootPath = @"D:\ServerPathRoot";
            string filePath = Path.Combine(pc.RootPath, @"TestPart");
            string[] files = Directory.GetFiles(filePath);
            filePath = Path.Combine(filePath, files[0]);
            using (Stream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                UpFile fileData = new UpFile();
                fileData.FileName = Path.GetFileName(filePath);
                fileData.FileSize = fs.Length;
                fileData.FileStream = fs;
                fileData.PartId = "Parts";
                UpFileResult ures = pcnfService.UpLoadFile(fileData);
                if (ures.IsSuccess)
                {
                    MessageBox.Show("传输完成");
                }
            }
        }
    }
}