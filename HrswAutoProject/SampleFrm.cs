using System;


using ProgramMonitor.Core;
using ProgramMonitor.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ProgramMonitor
{
    public partial class FrmMain : Form
    {
        private ServiceHost serviceHost = null;


        public FrmMain()
        {
            InitializeComponent();

            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(0, 1);


#if (DEBUG)
            btnTestSend.Visible = true;
#else
            btnTestSend.Visible = false;
#endif

            Common.SyncContext = SynchronizationContext.Current;
            Common.ProgramInfos = new ConcurrentDictionary<string, ProgramInfo>();
            Common.ListenCalls = new ConcurrentDictionary<string, IListenCall>();
            Common.ManualStopProgramIds = new ConcurrentBag<string>();

            Common.RefreshListView = RefreshListView;
            Common.RefreshTabControl = RefreshTabControl;
        }

        #region 自定义方法区域

        private void RefreshListView(ProgramInfo programInfo, bool needUpdateStatInfo)
        {
            Common.SyncContext.Post(o =>
            {
                string listViewItemKey = string.Format("lvItem_{0}", programInfo.Id);
                if (!listView1.Items.ContainsKey(listViewItemKey))
                {

                    var lstItem = listView1.Items.Add(listViewItemKey, programInfo.Name, 0);
                    lstItem.Name = listViewItemKey;
                    lstItem.Tag = programInfo.Id;
                    lstItem.SubItems.Add(programInfo.Version);
                    lstItem.SubItems.Add(programInfo.InstalledLocation);
                    lstItem.ToolTipText = programInfo.Description;

                    if (needUpdateStatInfo)
                    {
                        UpdateProgramListenStatInfo();
                    }
                }
                else
                {

                    if (!Common.ListenCalls.ContainsKey(programInfo.Id) && programInfo.RunState == -1 && Common.ClearInterval > 0
                        && programInfo.StopTime.AddMinutes(Common.ClearInterval) < DateTime.Now) //当属于正常关闭的程序在指定时间后从监控列表中移除
                    {
                        RemoveListenItem(programInfo.Id);
                    }
                }
            }, null);
        }

        private void RefreshTabControl(ProgramInfo programInfo, bool needUpdateStatInfo)
        {
            Common.SyncContext.Post(o =>
            {
                string tabPgName = string.Format("tabpg_{0}", programInfo.Id);
                string msgCtrlName = string.Format("{0}_MsgText", tabPgName);
                if (!tabControl1.TabPages.ContainsKey(tabPgName))
                {
                    RichTextBox rchTextBox = new RichTextBox();
                    rchTextBox.Name = msgCtrlName;
                    rchTextBox.Dock = DockStyle.Fill;
                    rchTextBox.ReadOnly = true;
                    AppendTextToRichTextBox(rchTextBox, programInfo);
                    var tabPg = new TabPage();
                    tabPg.Name = tabPgName;
                    tabPg.Controls.Add(rchTextBox);
                    tabControl1.TabPages.Add(tabPg);
                }
                else
                {
                    var tabPg = tabControl1.TabPages[tabPgName];
                    var rchTextBox = tabPg.Controls[msgCtrlName] as RichTextBox;
                    AppendTextToRichTextBox(rchTextBox, programInfo);
                }

                if (needUpdateStatInfo)
                {
                    UpdateProgramListenStatInfo();
                }
            }, null);
        }


        private void UpdateProgramListenStatInfo()
        {
            int runCount = Common.ProgramInfos.Count(p => p.Value.RunState >= 0);
            labRunCount.Text = string.Format("{0}个", runCount);
            labStopCount.Text = string.Format("{0}个", Common.ProgramInfos.Count - runCount);

            foreach (ListViewItem lstItem in listView1.Items)
            {
                string programId = lstItem.Tag.ToString();

                if (Common.ProgramInfos[programId].RunState == -1)
                {
                    lstItem.ForeColor = Color.Red;
                }
                else
                {
                    lstItem.ForeColor = Color.Black;
                }
            }
        }

        private void RemoveListenItem(string programInfoId)
        {
            ProgramInfo programInfo = Common.ProgramInfos[programInfoId];
            listView1.Items.RemoveByKey(string.Format("lvItem_{0}", programInfo.Id));
            tabControl1.TabPages.RemoveByKey(string.Format("tabpg_{0}", programInfo.Id));
            Common.ProgramInfos.TryRemove(programInfo.Id, out programInfo);
            IListenCall listenCall = null;
            Common.ListenCalls.TryRemove(programInfoId, out listenCall);

            UpdateProgramListenStatInfo();
        }

        private void AppendTextToRichTextBox(RichTextBox rchTextBox, Core.ProgramInfo programInfo)
        {
            Color msgColor = Color.Black;
            string lineMsg = string.Format("{0:yyyy-MM-dd HH:mm}\t{1}({2})\t{3} \n", DateTime.Now, programInfo.Name, programInfo.Version, GetRunStateString(programInfo.RunState, out msgColor));
            rchTextBox.SelectionColor = msgColor;
            rchTextBox.SelectionStart = rchTextBox.Text.Length;
            rchTextBox.AppendText(lineMsg);
            rchTextBox.SelectionLength = rchTextBox.Text.Length;

            if (rchTextBox.Lines.Length > 1000)
            {

            }
        }

        private string GetRunStateString(int runState, out Color msgColor)
        {
            if (runState < 0)
            {
                msgColor = Color.Red;
                return "程序已停止运行";
            }
            else if (runState == 0)
            {
                msgColor = Color.Blue;
                return "程序已启动运行";
            }
            else
            {
                msgColor = Color.Black;
                return "程序已在运行中";
            }
        }


        private void StartListenService()
        {
            if (serviceHost == null)
            {
                string serviceHostAddr = System.Configuration.ConfigurationManager.AppSettings["ServiceHostAddr"];
                string serviceMetaHostAddr = System.Configuration.ConfigurationManager.AppSettings["ServiceMetaHostAddr"];

                serviceHost = new ServiceHost(typeof(ListenService));

                NetTcpBinding binding = new NetTcpBinding();
                binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
                binding.SendTimeout = new TimeSpan(0, 5, 0);
                serviceHost.AddServiceEndpoint(typeof(IListenService), binding, string.Format("net.tcp://{0}/ListenService", serviceHostAddr));
                if (serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>() == null)
                {
                    ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
                    behavior.HttpGetEnabled = true;
                    behavior.HttpGetUrl = new Uri(string.Format("http://{0}/ListenService/metadata", serviceMetaHostAddr));
                    serviceHost.Description.Behaviors.Add(behavior);
                }
                serviceHost.Opened += (s, arg) =>
                {
                    SetUIStyle("S");
                    Common.Listening = true;
                    Common.AutoLoadProgramInfos();
                    Common.AutoListenPrograms();
                };
                serviceHost.Closed += (s, arg) =>
                {
                    SetUIStyle("C");
                    Common.loadTimer.Stop();
                    Common.listenTimer.Stop();
                    Common.Listening = false;
                };
            }

            serviceHost.Open();

        }

        private void StopListenService()
        {
            try
            {
                if (serviceHost != null && serviceHost.State != CommunicationState.Closed)
                {
                    serviceHost.Close();
                }
                serviceHost = null;
            }
            catch
            { }
        }

        private void SetUIStyle(string state)
        {
            if (state == "S")
            {
                labSericeState.BackColor = Color.Green;
                txtRefreshInterval.Enabled = false;
                txtListenInterval.Enabled = false;
                btnExec.Tag = "C";
                btnExec.Text = "停止监控";
                panel1.Enabled = false;
                panel2.Enabled = false;
            }
            else
            {
                labSericeState.BackColor = Color.Red;
                txtRefreshInterval.Enabled = true;
                txtListenInterval.Enabled = true;
                btnExec.Tag = "S";
                btnExec.Text = "开启监控";
                panel1.Enabled = true;
                panel2.Enabled = true;
            }

        }

        private void InitListViewStyle()
        {
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(32, 32);
            imgList.Images.Add(Properties.Resources.monitor);

            listView1.SmallImageList = imgList;
            listView1.LargeImageList = imgList;
            listView1.View = View.Details;
            listView1.GridLines = false;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("程序名称", -2);
            listView1.Columns.Add("版本");
            listView1.Columns.Add("运行路径");

            int avgWidth = listView1.Width / 3;

        }

        private void InitNoticeSetting()
        {

            if (chkSendSms.Checked)
            {
                bool dbConnected = false;
                Common.BuildConnectionString(txtServer.Text, txtDb.Text, txtUID.Text, txtPwd.Text);
                using (var da = new DataAccess(Common.DbConnString, Common.SqlProviderName))
                {
                    try
                    {
                        da.ExecuteScalar<DateTime>("select getdate()");
                        dbConnected = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("数据库测试连接失败，原因：" + ex.Message);
                    }
                }

                if (dbConnected)
                {
                    if (txtPhoneNos.Text.Trim().IndexOf(",") >= 0)
                    {
                        Common.NoticePhoneNos = txtPhoneNos.Text.Trim().Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        Common.NoticePhoneNos = new[] { txtPhoneNos.Text.Trim() };
                    }
                }
            }
            else
            {
                Common.NoticePhoneNos = null;
            }

            if (chkSendWx.Checked)
            {
                Common.NoticeWxUserIds = txtWxUIDs.Text.Trim();
            }
            else
            {
                Common.NoticeWxUserIds = null;
            }

        }

        private bool IsRightClickSelectedItem(Point point)
        {
            foreach (ListViewItem item in listView1.SelectedItems)
            {
                if (item.Bounds.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion


        private void btnExec_Click(object sender, EventArgs e)
        {
            string state = (btnExec.Tag ?? "S").ToString();
            if (state == "S")
            {
                InitNoticeSetting();
                Common.ClearInterval = int.Parse(txtRefreshInterval.Text);
                Common.ListenInterval = int.Parse(txtListenInterval.Text);
                StartListenService();
            }
            else
            {
                StopListenService();
            }

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0) return;

            string programId = listView1.SelectedItems[0].Tag.ToString();
            string tabPgName = string.Format("tabpg_{0}", programId);
            if (tabControl1.TabPages.ContainsKey(tabPgName))
            {
                tabControl1.SelectedTab = tabControl1.TabPages[tabPgName];
            }
            else
            {
                MessageBox.Show("未找到相应的程序监控记录！");
                listView1.SelectedItems[0].ForeColor = Color.Red;
            }
        }


        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitListViewStyle();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("您确定要退出吗？退出后将无法正常监控各程序的运行状况", "退出提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                e.Cancel = true;
                return;
            }

            StopListenService();
        }

        private void btnTestSend_Click(object sender, EventArgs e)
        {
            var wx = new WeChat();
            var msg = wx.SendMessage("kyezuo", "测试消息，有程序停止运行了，赶紧处理！");
            MessageBox.Show(msg["errmsg"].ToString());
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && IsRightClickSelectedItem(e.Location))
            {
                ctxMuPop.Show(listView1, e.Location);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0) return;

            string programId = listView1.SelectedItems[0].Tag.ToString();
            if (Common.ProgramInfos[programId].RunState != -1)
            {
                MessageBox.Show("只有被监控的程序处于已停止状态的监控项才能移除，除外情况请务必保持正常！");
                return;
            }

            RemoveListenItem(programId);

        }








    }
}