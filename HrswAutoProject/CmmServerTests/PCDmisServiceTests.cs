using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gy.HrswAuto.CmmServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.CmmServer.Tests
{
    [TestClass()]
    public class PCDmisServiceTests
    {
        [TestMethod()]
        public void InitialPCDmisTest()
        {
            PCDmisService pcds = new PCDmisService();
            pcds.InitialPCDmis();
        }
    }
}