using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCDClient
{
    public partial class Form1 : Form
    {
        CmmClient _cmmClient;
        CmmServerConfig serverCnf;
        public Form1()
        {
            InitializeComponent();
            serverCnf = new CmmServerConfig();
            serverCnf.HostIPAddress = "localhost";
            serverCnf.ControlPost = 6666;
            serverCnf.PartConfigPost = 7777;
            serverCnf.ServerID = 1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //PathConfig ptcnf = new PathConfig();
            PathManager.Instance.RootPath = @"D:\clientPathRoot";
            PathManager.Instance.PartProgramsPath = @"Progs";
            PathManager.Instance.BladesPath = @"blades";
            PathManager.Instance.ReportsPath = @"Results";
            //PathManager.Instance.Configration = ptcnf;

            PartConfig prcnf = new PartConfig();
            prcnf.PartID = "TestPart";
            prcnf.ProgFileName = "1.prg";
            prcnf.FlvFileName = "blade5.flv";
            prcnf.NormFileName = "blade5.nom";
            prcnf.TolFileName = "blade5.tol";
            PartConfigManager.Instance.InitPartConfigManager(@"d:\clientPathRoot\parts.xml");
            PartConfigManager.Instance.AddPartConfig(prcnf);
            PartConfigManager.Instance.SavePartConfigToXml(@"d:\clientPathRoot\parts.xml");
            _cmmClient = new CmmClient(serverCnf);
            _cmmClient.InitClient();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _cmmClient.StartMeasureWorkFlow("TestPart");
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = _cmmClient.runCount.ToString();
        }
    }
}
