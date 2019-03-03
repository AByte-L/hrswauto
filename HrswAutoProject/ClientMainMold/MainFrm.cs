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
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class MainFrm : Form
    {
        // 料架上的工件结果
        BindingList<PartResultRecord> RackResultRecordList = new BindingList<PartResultRecord>();
        // 工件结果记录
        BindingList<PartResultRecord> resultRecordList = new BindingList<PartResultRecord>();
        BindingList<CmmDataRecord> cmmRecordList = new BindingList<CmmDataRecord>();
        BindingList<PartConfig> partConfList = new BindingList<PartConfig>();
        public MainFrm()
        {
            InitializeComponent();
            // 设置Path
            cmmDataRecordBindingSource.DataSource = cmmRecordList;
            resultRowBindingSource.DataSource = RackResultRecordList;
            partConfigBindingSource.DataSource = partConfList;
            SetAppPaths();
            ClientUICommon.syncContext = SynchronizationContext.Current;
            ClientUICommon.AddCmmToView = AddClientView;
            ClientUICommon.RefreshRackView = RefreshRackView;
            ClientUICommon.RefreshCmmViewState = RefreshCmmViewState;
            //ClientUICommon.AddPartResult = AddPartResult;
            //ClientUICommon.AddPartToView = AddPartToView;
        }

        //private void AddPartResult(ResultRecord resultRecord)
        //{
        //    PartResultRecord
        //}

        #region 主界面
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
            CmmView.AutoGenerateColumns = false;
            ResultView.AutoGenerateColumns = false;
            ResultView.DataSource = RackResultRecordList;
            InitResult();
            //ClientManager.Instance.ClientConfigFileName = "clients.xml";
            //PartConfigManager.Instance.PartConfFile = "parts.xml";
            ClientManager.Instance.Initialize();
            PartConfigManager.Instance.InitPartConfigManager();
            // 初始化工件界面
            InitPartConfView();
        }

        private void InitPartConfView()
        {
            PartConfigManager.Instance.PartConfList.ForEach(pc =>
            partConfList.Add(pc));
        }
        #endregion

        #region 测量机Panel
        private void AddClientView(CmmServerConfig arg1, ClientState arg2)
        {
            ClientUICommon.syncContext.Post(o =>
            {
                CmmDataRecord cmmRecord = new CmmDataRecord(arg1, true, arg2);
                cmmRecordList.Add(cmmRecord);
            }, null);
        }

        /// <summary>
        /// 刷新三坐标服务器状态
        /// </summary>
        /// <param name="ServerID">三坐标号</param>
        /// <param name="state">三坐标服务器状态</param>
        private void RefreshCmmViewState(int index, ClientState state)
        {
            ClientUICommon.syncContext.Post(o =>
            {
                //cmmRecordList[index].State = CmmDataRecord.cmmStateInfo[state];
                cmmRecordList[index].SetClientState(state);
                cmmRecordList[index].IsFault = (state == ClientState.CS_InitError ||
                state == ClientState.CS_Error);
                //cmmRecordList[index].StateImage = cmmRecordList[index].StateImage;
                CmmView.InvalidateRow(index);
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
                ClientManager.Instance.DeleteClient(r.Index);
                cmmRecordList.RemoveAt(r.Index);
            }
        }

        private void enableCmmTsb_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in CmmView.SelectedRows)
            {
                ClientManager.Instance.EnableClient(r.Index);
                cmmRecordList[r.Index].IsActived = true;
            }
            CmmView.Invalidate();
        }

        private void disableCmmTsb_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in CmmView.SelectedRows)
            {
                ClientManager.Instance.DisableClient(r.Index);
                cmmRecordList[r.Index].IsActived = false;
            }
            CmmView.Invalidate();
        }

        private void InitClientTsb_Click(object sender, EventArgs e)
        {
            ClientManager.Instance.InitClients();
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
                partConfList.Add(pc);
                //partView.Refresh();
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

        private void InitResult()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    PartResultRecord row = new PartResultRecord()
                    {
                        SlotID = string.Format($"第{i+1}排-{j}号槽"),
                        PartID = string.Empty,
                        PcProgram = string.Empty,
                        IsPass = string.Empty,
                        ServerID = string.Empty,
                    };
                    RackResultRecordList.Add(row); 
                }
            }
        }

        private void RefreshRackView(ResultRecord result, int slotNumber)
        {
            if (slotNumber <= 0)
            {
                return;
            }
            int pos = slotNumber - 1;
            RackResultRecordList[pos].PartID = result.PartID;
            RackResultRecordList[pos].IsPass = result.IsPass ? "合格" : "不合格";
            RackResultRecordList[pos].ServerID = result.ServerID.ToString();
            RackResultRecordList[pos].ReportFileName = result.CmmFileName;
            RackResultRecordList[pos].RptFileName = result.RptFileName;
            RackResultRecordList[pos].ReportFilePath = result.FilePath;
            RackResultRecordList[pos].PartNumber = result.PartNumber.ToString();
            RackResultRecordList[pos].MeasDateTime = result.MeasDateTime;
            ResultView.InvalidateRow(pos);
            resultRecordList.Add(new PartResultRecord(RackResultRecordList[pos]));
            dataGridView1.InvalidateRow(resultRecordList.Count - 1);
        }
    
        private void ResetResult()
        {
            for (int i = 0; i < 60; i++)
            {
                RackResultRecordList[i].PartID = string.Empty;
                RackResultRecordList[i].IsPass = string.Empty;
                RackResultRecordList[i].ServerID = string.Empty;
                RackResultRecordList[i].ReportFileName = string.Empty;
                RackResultRecordList[i].RptFileName = string.Empty;
                RackResultRecordList[i].ReportFilePath = string.Empty;
                RackResultRecordList[i].MeasDateTime = string.Empty;
                RackResultRecordList[i].PartNumber = string.Empty;
                ResultView.Invalidate();
            }
        }

        private void ClearLogInfoTsb_Click(object sender, EventArgs e)
        {
            cmmInfoListBox.Items.Clear();
        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
            PartConfigManager.Instance.SavePartConfig();
        }
    }
}
