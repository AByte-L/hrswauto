using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gy.HrswAuto.BladeMold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Gy.HrswAuto.BladeMold.Tests
{
    [TestClass()]
    public class BladeMeasAssistTests
    {
        [TestMethod()]
        public void VerifyAnalysisResultTest()
        {
            BladeMeasAssist bma = new BladeMeasAssist();
            string path = @"E:\CSpItems\bladepart\blades\xx10_1\Results\xx10_1_CI8V82MKABUH101_181228_113446.CMM";
            bool outt = bma.VerifyAnalysisResult(path);
            Assert.IsTrue(outt);
        }
    }
}