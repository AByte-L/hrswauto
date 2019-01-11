using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gy.HrswAuto.BladeMold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Gy.HrswAuto.BladeMold.Tests
{
    [TestClass()]
    public class BladeMeasAssistTests
    {
        [TestMethod()]
        public void PCDmisRtfToBladeRptTest()
        {
            string path = "c:\\test\\debug\\b.exe";
            string fn = Path.GetFileNameWithoutExtension(path);
            string fn1 = Path.GetFileName(path);
        }
    }
}