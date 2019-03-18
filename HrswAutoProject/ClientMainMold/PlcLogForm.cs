using Gy.HrswAuto.UICommonTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class PlcLogForm : Form
    {
        BindingList<PlcLog> _plcLog = new BindingList<PlcLog>();
        public PlcLogForm()
        {
            InitializeComponent();
            CloseBtn.DialogResult = DialogResult.OK;
            //PlcUILinker.syncContext = SynchronizationContext.Current;
            PlcUILinker.Logging = PlcLogging;
        }

        private void PlcLogging(DateTime arg1, string arg2, string arg3)
        {
            PlcLog plg = new PlcLog() { DTime = arg1, PlcAction = arg2, LogString = arg3 };
            _plcLog.Add(plg);
        }

        private void PlcLogForm_Load(object sender, EventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            PlcUILinker.Logging = null;
            Close();
        }
    }
}
