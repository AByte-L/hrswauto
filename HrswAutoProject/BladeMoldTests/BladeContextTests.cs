using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gy.HrswAuto.BladeMold;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gy.HrswAuto.BladeMold.Tests
{
    [TestClass()]
    public class BladeContextTests
    {
        [TestMethod()]
        public void StartBladeTest()
        {
            Task<bool> t = RunBlade();
            while (t.Result) ;
        }

        private async Task<bool> RunBlade()
        {
            bool ok = await Task.Run(() =>
            {
                string be = @"C:\Program Files (x86)\Hexagon\PC-DMIS Blade 5.0 (Release)\Blade.exe";
                string rpt = @"D:\ServerPathRoot\blades\TestPart\Results\blade190121033341.rpt";
                BladeContext bc = new BladeContext(10);
                return bc.StartBlade(be, rpt);
            });
            return ok;
        }
    }
}