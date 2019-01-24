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
        public SetupFrm()
        {
            InitializeComponent();
            okButton.DialogResult = DialogResult.OK;
            cancelButton.DialogResult = DialogResult.Cancel;
        }

        private void bladeExeSelButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "Blade分析软件|Blade.exe";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                bladeTextBox.Text = dlg.FileName;
            }
        }

        private void gotoSafeProgButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "PCDmis定位程序|*.prg";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                progTextBox.Text = dlg.FileName;
            }
        }

        private void logFileSelButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            dlg.Filter = "Log文件|*.txt";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                logTextBox.Text = dlg.FileName;
            }
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
    }
}
