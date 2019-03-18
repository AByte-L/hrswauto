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
        private PlcClient _plcClient;
        private Task task = null;
        private bool _plcHbDis;
        private bool _isCancel;

        //private AutoResetEvent arEvt = new AutoResetEvent(false);
        public WritePartIDForm()
        {
            InitializeComponent();
            writeok = false;
            _plcClient = PlcClient.Instance;
            _plcClient.DisconnectEvent += _plcClient_DisconnectEvent;
            _plcHbDis = false;
            _isCancel = false;
        }

        private void _plcClient_DisconnectEvent(object sender, EventArgs e)
        {
            _plcHbDis = true;
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
            task = Task.Factory.StartNew(WritePartIDFlow, 60);
        }

        private void WritePartIDFlow(object t)
        {
            int time = (int)t;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (sw.Elapsed > TimeSpan.FromSeconds(time) || _plcHbDis) // 超过30s跳出，写入失败
                {
                    MessageBox.Show("写入工件ID失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                if (_isCancel) // 取消
                {
                    // 主线程已取消, 不能在写入标签
                    //Invoke((Action)(() => label1.Text = "取消写入..."));
                    break;
                }
                if (!writeok)
                {
                    Invoke((Action)(() => label1.Text = "正在写入工件ID到PLC中..."));
                    writeok = WritePartID();
                }
                else
                {
                    Invoke((Action)(() => label1.Text = "等待写码器写码完成..."));
                    if (PlcClient.Instance.VerifyWirteIDCompleted(48))
                    {
                        // 
                        Invoke((Action)(() => label1.Text = "写入完成."));
                        System.Media.SystemSounds.Hand.Play();
                        break;
                    }
                }
                        Thread.Sleep(500);
            }
            sw.Stop();
            Invoke((Action)(() => Close()));
        }

        private bool WritePartID()
        {
            // todo 更改存储器号，这个是写入工件RFID的存储器号
            bool wresult = PlcClient.Instance.SetPartID(48, partId);
            if (wresult)
            {
                wresult = PlcClient.Instance.SetWriteIDFlag(48);
            }
            return wresult;
        }

        private void WritePartIDForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _plcClient.DisconnectEvent -= _plcClient_DisconnectEvent;
            _plcClient = null;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            _isCancel = true;
        }
    }
}
