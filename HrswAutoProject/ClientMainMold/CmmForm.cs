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
    public partial class CmmForm : Form
    {
        public string IpAddress { get; private set; }
        private int _serverId;

        public int ServerID
        {
            get { return _serverId; }
            set { _serverId = value; }
        }
        public CmmForm()
        {
            InitializeComponent();
            okButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;
            AutoValidate = AutoValidate.EnableAllowFocusChange;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (!this.Validate())
            {
                DialogResult = DialogResult.None;
            }
            if (!this.ValidateChildren(ValidationConstraints.ImmediateChildren))
            {
                DialogResult = DialogResult.None;
            }
            IpAddress = ipInput.IpAddressString;
            //ServerID = int.Parse(serverCmmID.Text);
        }

        private void ipInput_Validating(object sender, CancelEventArgs e)
        {
            string error = "";
            if (ipInput.IpAddressString.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length < 4)
            {
                error = "IP地址输入错误";
                e.Cancel = true;
            }
            errorProvider1.SetError(ipInput, error);
        }

        private void serverCmmID_Validating(object sender, CancelEventArgs e)
        {
            string error = "";
            if (!int.TryParse(serverCmmID.Text, out _serverId))
            {
                error = "服务器编号输入错误";
                e.Cancel = true;
            }
            errorProvider1.SetError(serverCmmID, error);
        }
    }
}
