using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.MasterMold;
using Gy.HrswAuto.UICommonTools;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class MainFrm : Form
    {
        //private ClientManager clientManager; 单件初始化
        public MainFrm()
        {
            InitializeComponent();
            // 设置Path
            SetAppPaths();
            ClientUICommon.syncContext = SynchronizationContext.Current;
            ClientUICommon.AddCmmToView = AddClientView;
        }

        private void AddClientView(CmmServerConfig arg1, bool arg2)
        {
            int index = CmmView.Rows.Add();
            DataGridViewRow row = CmmView.Rows[index];
            row.Cells[0].Value = true;
            row.Cells[1].Value = "机器1";
            row.Cells[2].Value = arg1.ServerID;
            row.Cells[3].Value = arg1.HostIPAddress;
            row.Cells[4].Value = arg2 ? "正常" : "错误";
            
        }

        private static void SetAppPaths()
        {
            PathManager.Instance.RootPath = @"D:\clientPathRoot";
            PathManager.Instance.BladesPath = "blades";
            PathManager.Instance.PartProgramsPath = "programs";
            PathManager.Instance.ReportsPath = "results";
            PathManager.Instance.SettingsPath = "settings";
            PathManager.Instance.SettingsSavePath = "settingssave";
            PathManager.Instance.TempPath = "temp";
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
            ShowPanel(SwPanel.cmmPanel);
            ClientManager.Instance.Initialize();
        }


        private void addCmmTsb_Click(object sender, EventArgs e)
        {
            CmmForm cf = new CmmForm();
            if (cf.ShowDialog() == DialogResult.OK)
            {
                CmmServerConfig csConf = new CmmServerConfig();
                csConf.HostIPAddress = cf.IpAddress;
                csConf.ServerID = cf.ServerID;
                if (!ClientManager.Instance.AddClient(csConf))
                {
                    MessageBox.Show("测量机已存在.");
                }
            }
        }
    }
}
