using Gy.HrswAuto.ClientMold;
using Gy.HrswAuto.DataMold;
using Gy.HrswAuto.MasterMold;
using Gy.HrswAuto.PLCMold;
using Gy.HrswAuto.UICommonTools;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class MainFrm : Form
    {
        // 料架上的工件结果
        BindingList<PartResultRecord> RackResultRecordList = new BindingList<PartResultRecord>();
        // 历史工件结果
        BindingList<PartResultRecord> resultRecordList = new BindingList<PartResultRecord>();
        BindingList<CmmDataRecord> cmmRecordList = new BindingList<CmmDataRecord>();
        BindingSource partConfBs = new BindingSource();
        string bladePath = @"C:\Program Files (x86)\Hexagon\PC-DMIS Blade 5.0 (Release)\Blade.exe";

        bool IsRunning = false; // 
        private bool _plcConnected;

        public MainFrm()
        {
            InitializeComponent();
            // 设置Path
            cmmDataRecordBindingSource.DataSource = cmmRecordList;
            resultRowBindingSource.DataSource = RackResultRecordList;
            ClientUICommon.syncContext = SynchronizationContext.Current;
            ClientUICommon.AddCmmToView = AddClientView;
            ClientUICommon.RefreshRackView = RefreshRackView;
            ClientUICommon.RefreshCmmViewState = RefreshCmmViewState;
            ClientUICommon.RefreshCmmEventLogAction = RefreshCmmEventLog;
            ClientUICommon.RefreshPlcConnectStateAction = RefreshPlcConnect;

        }

        private void RefreshPlcConnect(string obj)
        {
            toolStripStatusLabel1.Text = obj;
        }

        private void RefreshCmmEventLog(string obj)
        {
            ClientUICommon.syncContext.Post(o =>
            {
                string info = DateTime.Now.ToShortTimeString() + " " + obj;
                cmmInfoListBox.Items.Add(info);
                cmmListBox.Items.Add(info);
            }, null);
        }

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
            splitContainer5.Hide();
            ShowPanel(SwPanel.cmmPanel);
        }
        private void partToolStripButton_Click(object sender, EventArgs e)
        {
            splitContainer5.Show();
            ShowPanel(SwPanel.partPanel);
        }
        private void resultTtoolStripButton_Click(object sender, EventArgs e)
        {
            splitContainer5.Hide();
            ShowPanel(SwPanel.resultPanel);
        }
        private void plcToolStripButton_Click(object sender, EventArgs e)
        {
            splitContainer5.Show();
            ShowPanel(SwPanel.plcPanel);
        }
        private void MainFrm_Load(object sender, EventArgs e)
        {
            //
            AutoResetEvent cmmArEvt = new AutoResetEvent(false);
            AutoResetEvent plcArEvt = new AutoResetEvent(false);
            InitForm initForm = null;
            Task.Run(() =>
            {
                initForm = new InitForm(cmmArEvt, plcArEvt);
                initForm.ShowDialog();
            });
            Thread.Sleep(1000); //等待初始窗口启动
            SetAppPaths();
            ClientManager.Instance.Initialize();
            ClientManager.Instance.InitClients();
            cmmArEvt.Set();
            //initForm?.SetInitInfo("正在初始化工件管理器...");
            PartConfigManager.Instance.InitPartConfigManager();
            //Thread.Sleep(1000);
            //initForm?.SetInitInfo("正在连接PLC控制器...");
            _plcConnected = PlcClient.Instance.Initialize();
            //Thread.Sleep(1000);
            plcArEvt.Set();

            if (ClientManager.Instance.CmmConnected(0)) // 第一号三坐标连接成功
            {
                cmmConnButton.Enabled = false;
            }
            if (_plcConnected) // plc连接成功关闭button1
            {
                button1.Enabled = false;
            }
            splitContainer5.Show();
            ShowPanel(SwPanel.plcPanel);
            plcIPStatusLabel.Text = PlcClient.Instance.PlcIPAddress;
            ResultView.DataSource = RackResultRecordList;
            InitResult();
            partConfBs.DataSource = PartConfigManager.Instance.PartConfList;
            partConfigBindingSource.DataSource = partConfBs;
            WindowState = FormWindowState.Maximized;
            // 初始化工件界面
            //InitPartConfView();
        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClientManager.Instance.SaveCmmServer();
            PartConfigManager.Instance.SavePartConfig();
            PlcClient.Instance.DisconnectPLC();
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
                cmmRecordList[index].SetClientState(state);
                cmmRecordList[index].IsFault = (state == ClientState.CS_InitError ||
                state == ClientState.CS_Error || state == ClientState.CS_ConnectError);
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

        private async void InitClientTsb_Click(object sender, EventArgs e)
        {
            //ClientManager.Instance.InitClients();
            int index = CmmView.SelectedRows[0].Index;
            if (!ClientManager.Instance.CmmConnected(index))
            {
                deleteCmmTsb.Enabled = false;
                InitClientTsb.Enabled = false;
                cmmConnButton.Enabled = false;
                cmmConnButton.Text = "正在连接";
                bool result = await Task.Run(() =>
                {
                    ClientManager.Instance.CmmConnect(index);
                    return ClientManager.Instance.CmmConnected(index);
                });
                if (!result)
                {
                    cmmConnButton.Enabled = true;
                }
                cmmConnButton.Text = "连接";
                InitClientTsb.Enabled = true;
                deleteCmmTsb.Enabled = true;
            }

        }
        private void ClearLogInfoTsb_Click(object sender, EventArgs e)
        {
            cmmInfoListBox.Items.Clear();
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
                    MessageBox.Show("blades目录不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (Directory.GetFiles(path, "*.nom", SearchOption.TopDirectoryOnly).Length != 1 ||
                       Directory.GetFiles(path, "*.flv", SearchOption.TopDirectoryOnly).Length != 1 ||
                       Directory.GetFiles(path, "*.tol", SearchOption.TopDirectoryOnly).Length != 1)
                    {
                        MessageBox.Show("blades文件缺失", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                // 添加工件配置
                if (PartConfigManager.Instance./*AddPartConfig*/Exists(pcfm.PartID))
                {
                    MessageBox.Show("工件已存在", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                PartConfig pc = new PartConfig();
                pc.FlvFileName = Path.GetFileName(Directory.GetFiles(path, "*.flv", SearchOption.TopDirectoryOnly)[0]);
                pc.NormFileName = Path.GetFileName(Directory.GetFiles(path, "*.nom", SearchOption.TopDirectoryOnly)[0]);
                pc.TolFileName = Path.GetFileName(Directory.GetFiles(path, "*.Tol", SearchOption.TopDirectoryOnly)[0]);
                pc.PartID = pcfm.PartID;
                pc.ProgFileName = Path.GetFileName(pcfm.PartProgram);
                pc.Description = pcfm.PartDescription;

                partConfBs.Add(pc);

                // 更新工件Panel
                //partConfList.Add(pc);
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
            if (partView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个工件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            int index = partView.SelectedRows[0].Index;
            PartConfForm pcForm = new PartConfForm();
            pcForm.PartID = ((PartConfig)partConfBs[index]).PartID;
            pcForm.PartProgram = ((PartConfig)partConfBs[index]).ProgFileName;
            pcForm.PartDescription = ((PartConfig)partConfBs[index]).Description;
            if (pcForm.ShowDialog() == DialogResult.OK)
            {
                string path = PathManager.Instance.GetBladesFullPath(pcForm.PartID);
                if (!Directory.Exists(path))
                {
                    MessageBox.Show("blades目录不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    if (Directory.GetFiles(path, "*.nom", SearchOption.TopDirectoryOnly).Length != 1 ||
                       Directory.GetFiles(path, "*.flv", SearchOption.TopDirectoryOnly).Length != 1 ||
                       Directory.GetFiles(path, "*.tol", SearchOption.TopDirectoryOnly).Length != 1)
                    {
                        MessageBox.Show("blades文件缺失", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                //if (PartConfigManager.Instance./*AddPartConfig*/Exists(pcForm.PartID))
                //{
                //    MessageBox.Show("工件已存在", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                ((PartConfig)partConfBs[index]).FlvFileName = Path.GetFileName(Directory.GetFiles(path, "*.flv", SearchOption.TopDirectoryOnly)[0]);
                ((PartConfig)partConfBs[index]).NormFileName = Path.GetFileName(Directory.GetFiles(path, "*.nom", SearchOption.TopDirectoryOnly)[0]);
                ((PartConfig)partConfBs[index]).TolFileName = Path.GetFileName(Directory.GetFiles(path, "*.Tol", SearchOption.TopDirectoryOnly)[0]);
                ((PartConfig)partConfBs[index]).PartID = pcForm.PartID;
                ((PartConfig)partConfBs[index]).ProgFileName = Path.GetFileName(pcForm.PartProgram);
                ((PartConfig)partConfBs[index]).Description = pcForm.PartDescription;
            }
        }

        private void delPartToolStripButton_Click(object sender, EventArgs e)
        {
            if (partView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个工件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            int index = partView.SelectedRows[0].Index;
            partConfBs.RemoveAt(index);
        }

        private void writePartIDToPlcToolStripButton_Click(object sender, EventArgs e)
        {
            if (!_plcConnected)
            {
                MessageBox.Show("PLC控制器未连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (partView.SelectedRows.Count != 1)
            {
                MessageBox.Show("请选择一个工件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            WritePartIDForm wpform = new WritePartIDForm();
            wpform.PartId = ((PartConfig)partConfBs[partView.SelectedRows[0].Index]).PartID;
            wpform.ShowDialog();
        }
        private void lookupToolStripButton_Click(object sender, EventArgs e)
        {
            if (partView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个工件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            int index = partView.SelectedRows[0].Index;
            string path = PathManager.Instance.GetBladesFullPath(((PartConfig)partConfBs[index]).PartID);
            Process.Start("Explorer.exe", path);
        }
        #endregion

        #region 报告Panel
        private void InitResult()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    PartResultRecord row = new PartResultRecord()
                    {
                        SlotID = string.Format($"第{i + 1}排-{j}号槽"),
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
            // 刷新块架报告
            ClientUICommon.syncContext.Post(o =>
           {
               if (slotNumber <= 0)
               {
                   // todo 槽号不能小于0
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
           }, null);
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

        private void checkReportTtoolStripButton_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = ResultView.SelectedRows[0].Index;
            // 
            if (string.IsNullOrEmpty(RackResultRecordList[index].PartID))
            {
                MessageBox.Show("当前选择槽未放置零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string path = PathManager.Instance.GetReportFullPath(RackResultRecordList[index].PartID);

            // 用记事本查看CMM报告
            Process.Start("Notepad.exe", Path.Combine(path, RackResultRecordList[index].ReportFileName));
        }

        private void browseToolStripButton_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = ResultView.SelectedRows[0].Index;
            if (string.IsNullOrEmpty(RackResultRecordList[index].PartID))
            {
                MessageBox.Show("当前选择槽未放置零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string path = PathManager.Instance.GetReportFullPath(RackResultRecordList[index].PartID);

            Process.Start("Explorer.exe", path);
        }

        private void runBladeToolStripButton_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = ResultView.SelectedRows[0].Index;
            if (string.IsNullOrEmpty(RackResultRecordList[index].PartID))
            {
                MessageBox.Show("当前选择槽未放置零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string path = PathManager.Instance.GetReportFullPath(RackResultRecordList[index].PartID);

            Process.Start(bladePath, Path.Combine(path, RackResultRecordList[index].RptFileName));
        }

        private void ResetToolStripButton_Click(object sender, EventArgs e)
        {
            ResetResult();
        }

        private void wholeCheckToolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = dataGridView1.SelectedRows[0].Index;
            // 

            string path = PathManager.Instance.GetReportFullPath(resultRecordList[index].PartID);

            // 用记事本查看CMM报告
            Process.Start("Notepad.exe", Path.Combine(path, resultRecordList[index].ReportFileName));
        }

        private void wholeBrowseToolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = dataGridView1.SelectedRows[0].Index;
            // 

            string path = PathManager.Instance.GetReportFullPath(resultRecordList[index].PartID);

            // 用记事本查看CMM报告
            Process.Start("Explorer.exe", path);
        }

        private void wholeToolStripButton_Click(object sender, EventArgs e)
        {
            if (ResultView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = ResultView.SelectedRows[0].Index;
            if (string.IsNullOrEmpty(RackResultRecordList[index].PartID))
            {
                MessageBox.Show("当前选择槽未放置零件", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string path = PathManager.Instance.GetReportFullPath(RackResultRecordList[index].PartID);

            Process.Start(bladePath, Path.Combine(path, RackResultRecordList[index].RptFileName));
        }

        private void ClearErrorTsb_Click(object sender, EventArgs e)
        {

        }

        #endregion

        // 控件自绘制
        private void comboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();
            Font drawFont = e.Font;
            bool outFont = false;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                outFont = true;
                drawFont = new Font(drawFont, FontStyle.Bold); // 黑体显示
            }

            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                CmmDataRecord cd = (CmmDataRecord)comboBox1.Items[e.Index];
                string str = cd.ServerName;
                if (cd.IsFault)
                {
                    str = cd.ServerName + "   " + "错误";
                }
                e.Graphics.DrawString(str, drawFont, brush, e.Bounds);
                if (outFont) drawFont.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            e.DrawBackground();
            Font drawFont = e.Font;
            bool outFont = false;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                outFont = true;
                drawFont = new Font(drawFont, FontStyle.Bold); // 黑体显示
            }

            using (Brush brush = new SolidBrush(e.ForeColor))
            {
                PartConfig cd = (PartConfig)comboBox2.Items[e.Index];
                string str = cd.PartID + " " + cd.Description;

                e.Graphics.DrawString(str, drawFont, brush, e.Bounds);
                if (outFont) drawFont.Dispose();
            }

            e.DrawFocusRectangle();
        }

        #region 主按钮
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!_plcConnected)
            {
                MessageBox.Show("PLC控制器未连接", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < ClientManager.Instance.CmmCount; i++)
            {
                if (!ClientManager.Instance.CmmConnected(i))
                {
                    DialogResult dr = MessageBox.Show($"三坐标{i + 1}未连接，是否继续?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {

                    }
                }
            }
            if (!IsRunning)
            {
                ClientManager.Instance.RunDispatchTask();
                toolStripButton1.Text = "暂停";
                IsRunning = true;
            }
            else
            {
                ClientManager.Instance.PauseDispatchTask();
                toolStripButton1.Text = "启动";
                IsRunning = false;
            }
        }

        private void initConnectToolStripButton_Click(object sender, EventArgs e)
        {
            AutoResetEvent arEvt = new AutoResetEvent(false);
            ConnectWaitForm cwForm = null;
            Task.Run(() =>
            {
                cwForm = new ConnectWaitForm(arEvt);
                cwForm.ShowDialog();
            });
            Thread.Sleep(1000);
            cwForm?.SetInitInfo("连接PLC...");
            // 连接PLC
            string ipAddress = "192.168.0.1";

            PlcClient.Instance.SetConnectParam(ipAddress, 0, 0);
            PlcClient.Instance.Initialize();
            cwForm?.SetInitInfo("连接三坐标控制器...");
            // 连接CMM
            ClientManager.Instance.InitClients();
            arEvt.Set();
        }

        #endregion

        private async void button1_Click(object sender, EventArgs e)
        {
            //AutoResetEvent cmevt = new AutoResetEvent(false);
            if (!PlcClient.Instance.IsConnected)
            {
                button1.Enabled = false;
                toolStripStatusLabel1.Text = "正在连接...";
                button1.Text = "正在连接...";
                bool connected = await Task.Run(() => PlcClient.Instance.Initialize());
                if (connected)
                {
                    if (button1.InvokeRequired)
                    {
                        button1.Invoke(new MethodInvoker(() =>
                        {
                            button1.Text = "PLC连接正常";
                            toolStripStatusLabel1.Text = "PLC连接正常";
                            button1.Enabled = false;
                        }));
                    }
                    else
                    {
                        button1.Text = "PLC连接正常";
                        toolStripStatusLabel1.Text = "PLC连接正常";
                        button1.Enabled = false;
                    }
                }
                else
                {
                    if (button1.InvokeRequired)
                    {
                        button1.Invoke(new MethodInvoker(() =>
                        {
                            button1.Text = "连接";
                            toolStripStatusLabel1.Text = "PLC连接失败";
                            button1.Enabled = true;
                        }));
                    }
                    else
                    {
                        button1.Text = "连接";
                        toolStripStatusLabel1.Text = "PLC连接失败";
                        button1.Enabled = true;
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                return;
            }
            if (ClientManager.Instance.CmmConnected(comboBox1.SelectedIndex))
            {
                cmmConnButton.Enabled = false;
            }
            else
            {
                cmmConnButton.Enabled = true;
            }
            if (ClientManager.Instance.CmmActived(comboBox1.SelectedIndex))
            {
                enableButton.Text = "离线";
            }
            else
            {
                enableButton.Text = "联机";
            }
        }

        private async void cmmConnButton_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (!ClientManager.Instance.CmmConnected(index))
            {
                deleteCmmTsb.Enabled = false;
                InitClientTsb.Enabled = false;
                cmmConnButton.Enabled = false;
                cmmConnButton.Text = "正在连接";
                bool result = await Task.Run(() =>
                {
                    ClientManager.Instance.CmmConnect(index);
                    return ClientManager.Instance.CmmConnected(index);
                });
                if (!result)
                {
                    cmmConnButton.Enabled = true;
                }
                cmmConnButton.Text = "连接";
                InitClientTsb.Enabled = true;
                deleteCmmTsb.Enabled = true;
            }
        }

        private void enableButton_Click(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            if (index < 0)
            {
                return;
            }
            if (ClientManager.Instance.CmmActived(index))
            {
                enableButton.Text = "联机";
                ClientManager.Instance.DisableClient(index);
                cmmRecordList[index].IsActived = false;
            }
            else
            {
                enableButton.Text = "离线";
                ClientManager.Instance.EnableClient(index);
                cmmRecordList[index].IsActived = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cmmListBox.Items.Clear();
            cmmListBox.Invalidate();
        }
    }
}
