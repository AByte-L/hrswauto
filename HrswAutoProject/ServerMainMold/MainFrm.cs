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
        MeasureServiceContext msc;
        ServiceHost ctrlHost;
        ServiceHost partServiceHost;

        public MainFrm()
        {
            InitializeComponent();
            msc = new MeasureServiceContext();
            ctrlHost = new ServiceHost(msc);
            partServiceHost = new ServiceHost(typeof(PartConfigService));
            ServerUILinker.syncContext = SynchronizationContext.Current;
            ServerUILinker.RefreshLog = RefreshLogListView;
            ServerUILinker.RefreshRemoteState = RefreshRemoteState;
        }

        private void RefreshRemoteState(bool obj)
        {
            throw new NotImplementedException();
        }

        private void RefreshLogListView(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
