using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.MasterMold;
using Gy.HrswAuto.UICommonTools;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class MainFrm : Form
    {
        //private ClientManager clientManager; 单件初始化
        private List<ResultViewRow> resultArray = new List<ResultViewRow>(60);
        public MainFrm()
        {
            InitializeComponent();
            // 设置Path
            SetAppPaths();
            ClientUICommon.syncContext = SynchronizationContext.Current;
            ClientUICommon.AddCmmToView = AddClientView;
            //ClientUICommon.AddPartToView = AddPartToView;
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
        private void resultTtoolStripButton_Click(object sender, EventArgs e)
        {
            ShowPanel(SwPanel.resultPanel);
        }
        private void plcToolStripButton_Click(object sender, EventArgs e)
        {
            ShowPanel(SwPanel.plcPanel);
        }
        private void MainFrm_Load(object sender, EventArgs e)
        {
            ShowPanel(SwPanel.cmmPanel);
            CreateResultView();
            //ClientManager.Instance.ClientConfigFileName = "clients.xml";
            //PartConfigManager.Instance.PartConfFile = "parts.xml";
            ClientManager.Instance.Initialize();
            PartConfigManager.Instance.InitPartConfigManager();
        }

        #region 测量机Panel
        private void AddClientView(CmmServerConfig arg1, ClientState arg2)
        {
            ClientUICommon.syncContext.Post(o =>
            {
                Image errorPic = Properties.Resources.Error;
                Image okPic = Properties.Resources.ok;
                int index = CmmView.Rows.Add();
                DataGridViewRow row = CmmView.Rows[index];
                row.Cells[0].Value = true;
                row.Cells[1].Value = arg1.ServerID;
                row.Cells[2].Value = arg1.HostIPAddress;
                string stateInfo = "";
                switch (arg2)
                {
                    case ClientState.CS_Idle:
                        stateInfo = "空闲";
                        break;
                    case ClientState.CS_Completed:
                        stateInfo = "完成";
                        break;
                    case ClientState.CS_Busy:
                        stateInfo = "忙碌";
                        break;
                    case ClientState.CS_Error:
                        stateInfo = "出错";
                        break;
                    case ClientState.CS_Continue:
                        stateInfo = "等待";
                        break;
                    default:
                        stateInfo = "未知";
                        break;
                }
                row.Cells[3].Value = stateInfo;
                if (arg2 == ClientState.CS_Error)
                {
                    row.Cells[4].Value = errorPic;
                }
                else
                {
                    row.Cells[4].Value = okPic;
                }
            }, null);

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

        private void deleteCmmTsb_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in CmmView.SelectedRows)
            {
                if (!r.IsNewRow)
                {
                    CmmView.Rows.Remove(r);
                    ClientManager.Instance.DeleteClient(r.Index);
                }
            }
        }

        private void enableCmmTsb_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in CmmView.SelectedRows)
            {
                if (!(bool)r.Cells[0].Value)
                {
                    r.Cells[0].Value = true;
                    ClientManager.Instance.EnableClient(r.Index);
                }
            }
        }

        private void disableCmmTsb_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in CmmView.SelectedRows)
            {
                if ((bool)r.Cells[0].Value)
                {
                    r.Cells[0].Value = false;
                    ClientManager.Instance.DisableClient(r.Index);
                }
            }
        }

        #endregion

        #region 工件Panel
        private void addPartToolStripButton_Click(object sender, EventArgs e)
        {
            PartConfForm pcfm = new PartConfForm();
            if (pcfm.ShowDialog() == DialogResult.OK)
            {
                // 查找blades文件
                string path = PathManager.Instance.GetBladesFullPath(pcfm.PartID);
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("blades目录不存在");
                    // todo 更新状态条

                    return;
                }
                else
                {
                    if (Directory.GetFiles(path, "*.nom", SearchOption.TopDirectoryOnly).Length != 1 ||
                       Directory.GetFiles(path, "*.flv", SearchOption.TopDirectoryOnly).Length != 1 ||
                       Directory.GetFiles(path, "*.tol", SearchOption.TopDirectoryOnly).Length != 1)
                    {
                        MessageBox.Show("blades文件缺失");
                        return;
                    }
                }
                // 添加工件配置
                PartConfig pc = new PartConfig();
                pc.FlvFileName = Path.GetFileName(Directory.GetFiles(path, "*.flv", SearchOption.TopDirectoryOnly)[0]);
                pc.NormFileName = Path.GetFileName(Directory.GetFiles(path, "*.nom", SearchOption.TopDirectoryOnly)[0]);
                pc.TolFileName = Path.GetFileName(Directory.GetFiles(path, "*.Tol", SearchOption.TopDirectoryOnly)[0]);
                pc.PartID = pcfm.PartID;
                pc.ProgFileName = Path.GetFileName(pcfm.PartProgram);
                if (!PartConfigManager.Instance.AddPartConfig(pc))
                {
                    MessageBox.Show("工件已存在");
                    return;
                }

                // 更新工件Panel
                int index = partView.Rows.Add();
                partView.Rows[index].Cells[0].Value = pcfm.PartID;
                partView.Rows[index].Cells[1].Value = pcfm.PartProgram;
                partView.Rows[index].Cells[2].Value = pc.NormFileName;
                partView.Rows[index].Cells[3].Value = pc.FlvFileName;
                partView.Rows[index].Cells[4].Value = pc.TolFileName;
                partView.Rows[index].Cells[5].Value = pcfm.PartDescription;
            }

        }

        //private void AddPartToView(PartConfig pconf, string description)
        //{
        //    ClientUICommon.syncContext.Post(o =>
        //   {
        //       int index = partView.Rows.Add();
        //       partView.Rows[index].Cells[0].Value = pconf.PartID;
        //       partView.Rows[index].Cells[1].Value = pconf.ProgFileName;
        //       partView.Rows[index].Cells[2].Value = pconf.NormFileName;
        //       partView.Rows[index].Cells[3].Value = pconf.FlvFileName;
        //       partView.Rows[index].Cells[4].Value = pconf.TolFileName;
        //       partView.Rows[index].Cells[5].Value = description;
        //   }, null);
        //}

        private void modifyToolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void delPartToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void writePartIDToPlcToolStripButton_Click(object sender, EventArgs e)
        {
            if (partView.SelectedRows.Count != 1)
            {
                MessageBox.Show("请选择一个工件");
                return;
            }
            WritePartIDForm wpform = new WritePartIDForm();
            wpform.PartId = partView.SelectedRows[0].Cells[0].Value.ToString();
            wpform.ShowDialog();
        }
        #endregion

        private void CreateResultView()
        {
            for (int i = 0; i < 60; i++)
            {
                int index = ResultView.Rows.Add();
                ResultView.Rows[index].Cells[0].Value = i+1;
            }
        }

    }
}
