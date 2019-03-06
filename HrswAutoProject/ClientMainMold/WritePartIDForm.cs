using Gy.HrswAuto.PLCMold;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class WritePartIDForm : Form
    {
        //private System.Timers.Timer timer;
        private string partId = "";
        bool writeok;

        public string PartId
        {
            get { return partId; }
            set { partId = value; }
        }

        public WritePartIDForm()
        {
            InitializeComponent();
            writeok = false;
        }

        private void WritePartIDForm_Load(object sender, System.EventArgs e)
        {
            if (partId.Trim().Length == 0)
            {
                MessageBox.Show("工件ID不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            //timer = new System.Timers.Timer(1000);
            //timer.Elapsed += Timer_Elapsed;
            //timer.Start();
            Task.Factory.StartNew(WritePartIDFlow, 30);
        }

        private void WritePartIDFlow(object t)
        {
            int time = (int)t;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (sw.Elapsed > TimeSpan.FromSeconds(time)) // 超过30s跳出，写入失败
                {
                    MessageBox.Show("写入工件ID失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    sw.Stop();
                    break;
                }
                if (!writeok)
                {
                    Invoke((Action)(() => label1.Text = "正在写入工件ID..."));
                    writeok = WritePartID();
                }
                else
                {
                    if (PlcClient.Instance.VerifyWirteIDCompleted())
                    {
                        // 
                        Invoke((Action)(() => label1.Text = "写入完成."));
                        Thread.Sleep(500);
                        Invoke(new MethodInvoker(Close));
                    }
                }
            }
            Invoke(new MethodInvoker(Close));
        }

        private bool WritePartID()
        {
            bool wresult = PlcClient.Instance.SetPartID(0, partId);
            if (wresult)
            {
                wresult = PlcClient.Instance.SetWriteIDFlag(0);
            }
            return wresult;
        }
    }
}
