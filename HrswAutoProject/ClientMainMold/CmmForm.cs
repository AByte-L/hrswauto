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
        public int ServerID { get; set; }
        public CmmForm()
        {
            InitializeComponent();
            okButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            IpAddress = ipInput.IpAddressString;
            ServerID = int.Parse(serverCmmID.Text);
        }
    }
}
