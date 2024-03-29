﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gy.HrswAuto.CmmServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gy.HrswAuto.Utilities;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.BladeMold;
using Gy.HrswAuto.ErrorMod;

namespace Gy.HrswAuto.CmmServer.Tests
{
    [TestClass()]
    public class MeasureServiceContextTests
    {
        [TestMethod()]
        public void _pcdmisCore_PCDmisMeasureEventTest()
        {
            //PathConfig ptcf = new PathConfig();
            PathManager.Instance.RootPath = @"D:\ServerPathRoot";
            PathManager.Instance.BladesPath = "blades";
            PathManager.Instance.ReportsPath = "Results";
            PathManager.Instance.TempPath = "Temp";
            //PathManager.Instance.Configration = ptcf;
            PartConfig part = new PartConfig();
            part.PartID = "TestPart";
            part.FlvFileName = "xx10_1.flv";
            part.NormFileName = "xx10_1.nom";
            part.TolFileName = "xx10_1.tol";

            LocalLogCollector.LogFilePath = @"d:\log.txt";
            ServerSettings.BladeExe = @"C:\Program Files (x86)\Hexagon\PC-DMIS Blade 5.0 (Release)\Blade.exe";
            BladeMeasAssist _bladeMeasAssist = new BladeMeasAssist();
            _bladeMeasAssist.RtfFileName = @"C:\BladeRunner\blade.RTF";
            _bladeMeasAssist.ProbeDiam = 2;
            _bladeMeasAssist.SectionNum = 3;
            List<string> sn = new List<string>() { "8-8", "5-5", "2-2" };
            _bladeMeasAssist.SectionNames = sn;
            _bladeMeasAssist.Part = part;

            MeasureServiceContext msc = new MeasureServiceContext(10,10);
            msc.SetBladeMeasAssist(_bladeMeasAssist);
            PCDmisEventArgs pca = new PCDmisEventArgs();
            pca.IsCompleted = true;
            msc._pcdmisCore_PCDmisMeasureEvent(null, pca);

            while (true) ;
        }
    }
}