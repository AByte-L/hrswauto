using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gy.HrswAuto.ErrorMod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.ErrorMod.Tests
{
    [TestClass()]
    public class LocalLogCollectorTests
    {
        [TestMethod()]
        public void WriteMessageTest()
        {
            LocalLogCollector.LogFilePath = @"G:\AutoMeasureItems\ServerPathRoot\log.txt";
            LocalLogCollector.WriteMessage("测试字符串\n");
            LocalLogCollector.WriteMessage("测试字符串1");
            LocalLogCollector.WriteMessage("测试字符串2");
        }
    }
}