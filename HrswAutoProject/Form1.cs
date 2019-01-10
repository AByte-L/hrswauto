using Snap7;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PLCItem1
{
    public partial class Form1 : Form
    {
        System.Timers.Timer timer { get; set; }
        S7Client s7Client = new S7Client();
        CancellationTokenSource ts ;
        CancellationToken token;
        
        public Form1()
        {
            InitializeComponent();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (s7Client.Connected())
            {
                byte[] buf = new byte[1];
                int result = s7Client.DBRead(1, 0, 1, buf);
                if (result != 0)
                {
                    toolStripStatusLabel1.Text = s7Client.ErrorText(result);
                    return;
                }
                else
                {
                    int flag = S7.GetByteAt(buf, 0);
                    toolStripStatusLabel1.Text = flag.ToString();
                }
                buf[0] = (byte)(buf[0] ^ 0x01);
                //S7.SetIntAt(buf, 0, 0x01);
                result = s7Client.DBWrite(1, 0, 1, buf);
                if (result != 0)
                {
                    toolStripStatusLabel1.Text = s7Client.ErrorText(result);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();
            ts = new CancellationTokenSource();
            token = ts.Token;
            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
            button7.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int result = s7Client.ConnectTo("192.168.100.1", 0, 0);
            if (result == 0)
            {
                toolStripStatusLabel1.Text = "PLC已连接";
                button2.Enabled = false;
                button3.Enabled = true;
                button1.Enabled = true;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                toolStripStatusLabel1.Text = s7Client.ErrorText(result);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int result = s7Client.Disconnect();
            if (result == 0)
            {
                toolStripStatusLabel1.Text = "PLC已断开";
                button2.Enabled = true;
                button3.Enabled = false;
                button1.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
                button7.Enabled = false;
            }
            else
            {
                toolStripStatusLabel1.Text = s7Client.ErrorText(result);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] wrb = new byte[256];
            string wrString = textBox1.Text;
            //bool hb = true;
            S7.SetStringAt(wrb, 0, 256, wrString);
            //S7.SetBitAt()
            int result = s7Client.DBWrite(1, 2, wrString.Length + 2, wrb);
            if (result == 0)
            {
                toolStripStatusLabel1.Text = "写入成功";
            }
            else
            {
                toolStripStatusLabel1.Text = s7Client.ErrorText(result);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] rdb = new byte[256];
            int result = s7Client.DBRead(1, 2, 256, rdb);
            if (result == 0)
            {
                string rds = S7.GetStringAt(rdb, 0);
                toolStripStatusLabel1.Text = "读取成功 : " + rds;
            }
            else
            {
                toolStripStatusLabel1.Text = s7Client.ErrorText(result);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            byte[] bob = new byte[2];
            int result = s7Client.DBRead(1, 0, 2, bob);
            if (result == 0)
            {
                bool bo = Convert.ToBoolean(S7.GetIntAt(bob, 0));
                toolStripStatusLabel1.Text = bo ? "True" : "False";
            }
            else
            {
                toolStripStatusLabel1.Text = s7Client.ErrorText(result);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Action<int> acWrite1 = (x) =>
            {
                while(!token.IsCancellationRequested)
                {
                    byte[] buf = new byte[2];
                    S7.SetIntAt(buf, 0, (short)x);
                    int result = s7Client.DBWrite(1, 258, 2, buf);
                    if (result != 0)
                    {
                        toolStripStatusLabel1.Text = s7Client.ErrorText(result);
                    }
                    Thread.Sleep(500);
                }
            };
            Action<int> acWrite2 = (x) =>
            {
                while (!token.IsCancellationRequested)
                {
                    byte[] buf = new byte[2];
                    S7.SetIntAt(buf, 0, (short)x);
                    int result = s7Client.DBWrite(1, 258, 2, buf);
                    if (result != 0)
                    {
                        toolStripStatusLabel1.Text = s7Client.ErrorText(result);
                    }
                    Thread.Sleep(500);
                }
            };
            IAsyncResult iar = acWrite1.BeginInvoke(53, new AsyncCallback(AcEndInvoke), null);
            IAsyncResult iar2 = acWrite2.BeginInvoke(17, new AsyncCallback(AcEndInvoke), null);
            button6.Enabled = false;
            button7.Enabled = true;
        }

        private void AcEndInvoke(IAsyncResult iar)
        {
            AsyncResult ar = (AsyncResult)iar;
            var ard = (Action<int>)ar.AsyncDelegate;
            ard.EndInvoke(iar);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ts.Cancel();
            button7.Enabled = false;
            button6.Enabled = true;
        }
    }
}
