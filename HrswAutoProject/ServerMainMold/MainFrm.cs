using Gy.HrswAuto.CmmServer;
using Gy.HrswAuto.UICommonTools;
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
        // todo 报告服务处理

        string _bladeExe;
        string _gotoProg;
        string _logFileName;
        double _PCDmisTimeout;
        double _BladeTimeout;
        double _ServiceOpenTimeout;
        private string errorInfo;

        public MainFrm()
        {
            InitializeComponent();
            SetConfigValue();
            _msc = new MeasureServiceContext(_PCDmisTimeout, _BladeTimeout);
            _ctrlHost = new ServiceHost(_msc);
            _partServiceHost = new ServiceHost(typeof(PartConfigService));
            ServerUILinker.syncContext = SynchronizationContext.Current;
            ServerUILinker.RefreshLog = RefreshLogListView;
            ServerUILinker.RefreshRemoteState = RefreshRemoteState;
        }

        private void SetConfigValue()
        {
            _bladeExe = Properties.Settings.Default.BladeExe;
            _gotoProg = Properties.Settings.Default.GotoProg;
            _logFileName = Properties.Settings.Default.LogFilePath;
            _PCDmisTimeout = double.Parse(Properties.Settings.Default.PCDmisTimeout);
            _BladeTimeout = double.Parse(Properties.Settings.Default.BladeTimeout);
            _ServiceOpenTimeout = double.Parse(Properties.Settings.Default.ServiceOpenTimeout);
        }

        private void RefreshRemoteState(bool obj)
        {

        }

        private void RefreshLogListView(string obj)
        {
            ServerUILinker.syncContext.Post(o =>
            {
                logListView.Items.Add(obj);
            }, null);
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
            Task.Run(() =>
            {
                InitForm initForm = new InitForm(arevt);
                initForm.ShowDialog();
            });
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
            logListView.Items.Add(errorInfo);
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            SetupFrm setupForm = new SetupFrm();
            DialogResult dr = setupForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                SetConfigValue();
                _msc.SetMeasureDuration(_PCDmisTimeout, _BladeTimeout);
                Properties.Settings.Default.Save();
            }
        }
    }
}
