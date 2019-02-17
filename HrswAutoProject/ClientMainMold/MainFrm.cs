using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.MasterMold;
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

namespace ClientMainMold
{
    public partial class MainFrm : Form
    {
        //private ClientManager clientManager; 单件初始化
        public MainFrm()
        {
            InitializeComponent();
            //ShowPanel(SwPanel.partPanel);
        }

        private void ShowPanel(SwPanel panel)
        {
            switch (panel)
            {
                case SwPanel.cmmPanel:
                    partPanel.Hide();
                    plcPanel.Hide();
                    resultPanel.Hide();
                    cmmPanel.Show();
                    break;
                case SwPanel.partPanel:
                    cmmPanel.Hide();
                    plcPanel.Hide();
                    resultPanel.Hide();
                    partPanel.Show();
                    break;
                case SwPanel.plcPanel:
                    cmmPanel.Hide();
                    partPanel.Hide();
                    resultPanel.Hide();
                    plcPanel.Show();
                    break;
                case SwPanel.resultPanel:
                    cmmPanel.Hide();
                    partPanel.Hide();
                    plcPanel.Hide();
                    resultPanel.Show();
                    break;
                case SwPanel.swNone:
                    cmmPanel.Hide();
                    partPanel.Hide();
                    plcPanel.Hide();
                    resultPanel.Hide();
                    break;
                default:
                    cmmPanel.Hide();
                    plcPanel.Hide();
                    resultPanel.Hide();
                    partPanel.Show();
                    break;
            }
        }

        private void cmmToolStripButton_Click(object sender, EventArgs e)
        {
            ShowPanel(SwPanel.cmmPanel);
        }
        private void partToolStripButton_Click(object sender, EventArgs e)
        {
            ShowPanel(SwPanel.partPanel);
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            // todo 设置PathManager      
            ClientManager.Instance.Initialize();
            ShowPanel(SwPanel.partPanel);
        }


        private void addCmmTsb_Click(object sender, EventArgs e)
        {
            CmmForm cf = new CmmForm();
            if (cf.ShowDialog() == DialogResult.OK)
            {
                CmmServerConfig csConf = new CmmServerConfig();
                csConf.HostIPAddress = cf.IpAddress;
                csConf.ServerID = cf.ServerID;
                csConf.ControlPost = 6666;
                csConf.PartConfigPost = 7777;
            }
        }
    }
}
