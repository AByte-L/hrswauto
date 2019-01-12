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
        public void OpenPartProgramTest()
        {
            string filename = @"D:\WorkItems\hrswauto\HrswAutoProject\PCDServer\bin\Debug\blades\TestPart\1.prg";
            PCDmisService pcd = new PCDmisService();
            pcd.InitialPCDmis();
            pcd.OpenPartProgram(filename);
            Thread.Sleep(5000);
            pcd.Dispose();
        }
    }
}