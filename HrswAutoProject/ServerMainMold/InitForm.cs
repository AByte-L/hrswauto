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

namespace ServerMainMold
{
    public partial class InitForm : Form
    {
        public AutoResetEvent compEvent { get; set; }
        public bool IsInitCompleted { get; set; }
        public InitForm(AutoResetEvent arEvent)
        {
            InitializeComponent();
            IsInitCompleted = false;
            ShowInTaskbar = false;
            compEvent = arEvent;
        }

        private void InitForm_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(WaitServerInit);
        }

        private void WaitServerInit()
        {
            IsInitCompleted = compEvent.WaitOne((int)TimeSpan.FromSeconds(60).TotalMilliseconds); // 等待60s超时
            Invoke((Action)(() => Close()));
        }

        private void InitForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
    }
}
