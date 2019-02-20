using Gy.HrswAuto.PLCMold;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class WritePartIDForm : Form
    {
        private System.Timers.Timer timer;
        private string partId = "";

        public string PartId
        {
            get { return partId; }
            set { partId = value; }
        }

        public WritePartIDForm()
        {
            InitializeComponent();
        }

        private void WritePartIDForm_Load(object sender, System.EventArgs e)
        {
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (PlcClient.Instance.VerifyWirteIDCompleted())
            {
                // 
                label1.Text = "写入完成.";
                timer.Close();

                Thread.Sleep(2000);
                Invoke( ()=>
                {
                    Close();
                    return null;
                });
            }
        }

        private void Invoke(Func<object> p)
        {
            throw new NotImplementedException();
        }

        private void WritePartIDForm_Shown(object sender, System.EventArgs e)
        {
            // 
            if (!WriePartID())
            {
                MessageBox.Show("写入标识出错");
                return;
            }
            label1.Text = "正在写入工件标识...";
        }

        private bool WriePartID()
        {
            if (partId.Trim().Length == 0)
            {
                MessageBox.Show("工件ID不能为空");
                return false;
            }
            bool wresult = PlcClient.Instance.SetPartID(0, partId);
            if (wresult)
            {
                wresult = PlcClient.Instance.SetWriteIDFlag(0);
            }
            return wresult;
        }
    }
}
