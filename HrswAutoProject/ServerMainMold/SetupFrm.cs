using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerMainMold
{
    public partial class SetupFrm : Form
    {
        public bool MinState
        {
            get
            {
                return checkBox1.Checked;
            }
        }
        public SetupFrm()
        {
            InitializeComponent();
            okButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            const string pattern = @"^\d+(\.\d+)?$";
            string text = ((Control)sender).Text;
            if (!Regex.IsMatch(text, pattern))
            {
                error = "请输入一个正数";
                e.Cancel = true;
            }
            errorProvider1.SetError((Control)sender, error);
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            const string pattern = @"^\d+(\.\d+)?$";
            string text = ((Control)sender).Text;
            if (!Regex.IsMatch(text, pattern))
            {
                error = "请输入一个正数";
                e.Cancel = true;
            }
            errorProvider1.SetError((Control)sender, error);
        }

        private void pathSettingButton_Click(object sender, EventArgs e)
        {
            PathSettingForm psfrm = new PathSettingForm();
            psfrm.BladeExe = bladeTextBox.Text;
            psfrm.GotoProg = progTextBox.Text;
            psfrm.LogFilePath = logTextBox.Text;
            psfrm.RootPath = rootPathTextBox.Text;
            if (psfrm.ShowDialog() == DialogResult.OK)
            {
                 bladeTextBox.Text=psfrm.BladeExe;
                 progTextBox.Text= psfrm.GotoProg;
                 logTextBox.Text= psfrm.LogFilePath;
                 rootPathTextBox.Text= psfrm.RootPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
        }
    }
}
