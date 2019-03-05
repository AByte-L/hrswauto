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
    public partial class InitForm : Form
    {
        private System.Timers.Timer timer;
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
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            IsInitCompleted = compEvent.WaitOne((int)TimeSpan.FromSeconds(30).TotalMilliseconds); // 等待30s超时
            this.Invoke(new MethodInvoker(CloseForm));
        }

        private void CloseForm()
        {
            Close();
        }

        private void InitForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Dispose();
        }
        public void SetInitInfo(string info)
        {
            if (InvokeRequired)
            {
                this.Invoke((Action)delegate
                {
                    label2.Text = info;
                });
            }
        }
    }
}
