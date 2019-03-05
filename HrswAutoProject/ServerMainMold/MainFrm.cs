using Gy.HrswAuto.CmmServer;
using Gy.HrswAuto.ErrorMod;
using Gy.HrswAuto.UICommonTools;
using Gy.HrswAuto.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerMainMold
{
    public partial class MainFrm : Form
    {
        MeasureServiceContext _msc;
        ServiceHost _ctrlHost;
        ServiceHost _partServiceHost;

        double _PCDmisTimeout;
        double _BladeTimeout;
        double _ServiceOpenTimeout;
        private string errorInfo;
        private string _partFile;

        bool _notifyIcon = false;

        public MainFrm()
        {
            InitializeComponent();
            SetConfigValue();
            _msc = new MeasureServiceContext(_PCDmisTimeout, _BladeTimeout);
            _ctrlHost = new ServiceHost(_msc);
            _partServiceHost = new ServiceHost(typeof(PartConfigService));
            ServerUILinker.syncContext = SynchronizationContext.Current;
            ServerUILinker.RefreshLog = RefreshInfoView;
            notifyIcon1.Visible = false;
        }

        private void RefreshInfoView(string obj)
        {
            ServerUILinker.syncContext.Post(o =>
            {
                //DateTime infoTime = DateTime.Now;
                int index = infoDataGridView.Rows.Add();
                infoDataGridView.Rows[index].Cells[0].Value = DateTime.Now.ToLongTimeString();
                infoDataGridView.Rows[index].Cells[1].Value = obj;
            }, null);
        }

        private void SetConfigValue()
        {
            ServerSettings.BladeExe = Properties.Settings.Default.BladeExe;
            ServerSettings.GotoSafePrg = Properties.Settings.Default.GotoProg;
            LocalLogCollector.LogFilePath = Properties.Settings.Default.LogFilePath;
            _PCDmisTimeout = double.Parse(Properties.Settings.Default.PCDmisTimeout);
            _BladeTimeout = double.Parse(Properties.Settings.Default.BladeTimeout);
            _ServiceOpenTimeout = double.Parse(Properties.Settings.Default.ServiceOpenTimeout);
            PathManager.Instance.RootPath = Properties.Settings.Default.RootPath;
            PathManager.Instance.BladesPath = Properties.Settings.Default.BladesPath;
            PathManager.Instance.PartProgramsPath = Properties.Settings.Default.ProgramsPath;
            PathManager.Instance.ReportsPath = Properties.Settings.Default.ReportsPath;
            PathManager.Instance.SettingsPath = Properties.Settings.Default.SettingsPath;
            PathManager.Instance.TempPath = Properties.Settings.Default.TempPath;
        }

        private void MainFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _partServiceHost.Close();
            _ctrlHost.Close();
            _msc.Dispose();
            _partServiceHost = null;
            _ctrlHost = null;
            _msc = null;
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            AutoResetEvent arevt = new AutoResetEvent(false);
            errorInfo = "服务器启动成功";
            Task.Run(() =>
            {
                InitForm initForm = new InitForm(arevt);
                initForm.ShowDialog();
            });
            // 初始化工件配置管理器
            PartConfigManager.Instance.InitPartConfigManager(/*_partFile*/);
            if (!_msc.Initialize())
            {
                //初始化PCDMIS失败
                errorInfo = "未能初始化PCDMIS";
            }
            // 开启服务器 
            try
            {
                _ctrlHost.Open(TimeSpan.FromSeconds(_ServiceOpenTimeout));
                _partServiceHost.Open(TimeSpan.FromSeconds(_ServiceOpenTimeout));
            }
            catch (TimeoutException)
            {
                errorInfo = "服务器未正常启动";
            }
            finally
            {
                arevt.Set();
            }
            //logListView.Items.Add(errorInfo);
            RefreshInfoView(errorInfo);
        }

        private void MainFrm_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (_notifyIcon)
                {
                    ShowInTaskbar = false;
                    notifyIcon1.Visible = true;
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            ShowInTaskbar = true;
            Activate();
        }

        private void SetupToolStripButton_Click(object sender, EventArgs e)
        {
            SetupFrm setupForm = new SetupFrm();
            DialogResult dr = setupForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                SetConfigValue();
                _msc.SetMeasureDuration(_PCDmisTimeout, _BladeTimeout);
                _notifyIcon = setupForm.MinState;
                Properties.Settings.Default.Save();
            }
        }

        private void ClearErrorToolStripButton_Click(object sender, EventArgs e)
        {
            _msc.ClearServerError();
        }

        private void RestartToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void gotoToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}
