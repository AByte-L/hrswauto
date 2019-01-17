using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gy.HrswAuto.CmmServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Gy.HrswAuto.CmmServer.Tests
{
    [TestClass()]
    public class PCDmisServiceTests
    {
        [TestMethod()]
        public void ExecutePartProgramTest()
        {
            PCDmisService pcdService = new PCDmisService();
            pcdService.InitialPCDmis();
            pcdService.OpenPartProgram(@"D:\ServerPathRoot\PartPrograms\1.prg");
            pcdService.ExecutePartProgram();
            Thread.Sleep(10000);
            pcdService.OpenPartProgram(@"D:\clientPathRoot\programs\1.prg");
            while (true)
            {

            }
        }
    }
}