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
            Task.Factory.StartNew(WaitInitProcess);
        }

        private void WaitInitProcess()
        {
            SetInitInfo("正在连接三坐标服务器....");
            // 等待连接三坐标初始化5分钟
            cmmCompEvent.WaitOne((int)TimeSpan.FromMinutes(5).TotalMilliseconds);
            SetInitInfo("正在连接PLC...");
            // 等待连接PLC初始化1分钟
            plcCompEvent.WaitOne((int)TimeSpan.FromMinutes(1).TotalMilliseconds);
            Invoke(new MethodInvoker(CloseForm));
        }

        private void CloseForm()
        {
            Close();
        }

        private void InitForm_FormClosed(object sender, FormClosedEventArgs e)
        {

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
