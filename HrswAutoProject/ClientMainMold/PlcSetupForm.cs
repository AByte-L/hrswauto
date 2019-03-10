using Gy.HrswAuto.PLCMold;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class PlcSetupForm : Form
    {
        private string _ipAddress;

        public string IpAddress
        {
            get { return _ipAddress; }
        }

        public PlcSetupForm()
        {
            InitializeComponent();
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!this.Validate())
            {
                this.DialogResult = DialogResult.None;
            }
            if (!this.ValidateChildren())
            {
                this.DialogResult = DialogResult.None;
            }
            _ipAddress = ipInput1.IpAddressString;
        }

        private void ipInput1_Validating(object sender, CancelEventArgs e)
        {
            string error = "";
            if (ipInput1.IpAddressString.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length < 4)
            {
                error = "IP地址输入错误";
                e.Cancel = true;
            }
            errorProvider1.SetError(ipInput1, error);
        }

        private void PlcSetupForm_Load(object sender, EventArgs e)
        {
            if (PlcClient.Instance.IsConnected)
            {
                ipInput1.Enabled = false;
            }
        }
    }
}
