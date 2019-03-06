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
        private AutoResetEvent cmmCompEvent;
        private AutoResetEvent plcCompEvent;
        public bool IsInitCompleted { get; set; }
        
        public InitForm(AutoResetEvent cmmArEvent, AutoResetEvent plcArEvent)
        {
            InitializeComponent();
            IsInitCompleted = false;
            ShowInTaskbar = false;
            cmmCompEvent = cmmArEvent;
            plcCompEvent = plcArEvent;
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
            SetInitInfo("正在连接三坐标服务器....");
            IsInitCompleted = cmmCompEvent.WaitOne((int)TimeSpan.FromSeconds(30).TotalMilliseconds); // 等待30s超时
            SetInitInfo("正在连接PLC...");
            IsInitCompleted = plcCompEvent.WaitOne((int)TimeSpan.FromSeconds(15).TotalMilliseconds);
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
