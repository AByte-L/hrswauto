using Gy.HrswAuto.CmmServer;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.ErrorMod;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCDServer
{
    public partial class Form1 : Form
    {
        MeasureServiceContext _bladeMeasureContext;
        
        ServiceHost cmmCtrlHost;
        ServiceHost partServiceHost;
        private string _pcProg;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // PathConfig ptc = new PathConfig();
            PathManager.Instance.RootPath = @"D:\ServerPathRoot"/*Path.GetDirectoryName(Application.ExecutablePath)*/;
            PathManager.Instance.PartProgramsPath = "PartPrograms";
            PathManager.Instance.BladesPath = "blades";
            PathManager.Instance.ReportsPath = "Results";
            PathManager.Instance.TempPath = @"D:\ServerPathRoot";
            //PathManager.Instance.Configration = ptc;
            PartConfigManager.Instance.InitPartConfigManager(@"D:\ServerPathRoot\parts.xml");
            _bladeMeasureContext = new MeasureServiceContext(10,10);
            _bladeMeasureContext.Initialize();
            cmmCtrlHost = new ServiceHost(_bladeMeasureContext);
            cmmCtrlHost.Opened += Host_Opened;
            cmmCtrlHost.Open(TimeSpan.FromMinutes(0.5));
            partServiceHost = new ServiceHost(typeof(PartConfigService));
            partServiceHost.Open(TimeSpan.FromMinutes(0.5));
        }

        private void Host_Opened(object sender, EventArgs e)
        {
            label1.Text = "服务已开启";
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            partServiceHost.Close();
            cmmCtrlHost.Close();
            _bladeMeasureContext.Dispose();
            PartConfigManager.Instance.SavePartConfigToXml(@"D:\ServerPathRoot\parts.xml");
        }

        private void select_button_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult result = ofd.ShowDialog();
            if (result.HasFlag(DialogResult.OK))
                {
                _pcProg = ofd.FileName;
                label2.Text = _pcProg;
            }
        }

        private void RunProg_button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                _bladeMeasureContext.IsBladeMeasure = false;
                _bladeMeasureContext.MeasurePart("TestPart"); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _bladeMeasureContext.ReinitialPCDmist();
        }
    }
}
