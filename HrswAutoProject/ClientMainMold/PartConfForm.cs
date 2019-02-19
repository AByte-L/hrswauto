using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClientMainMold
{
    public partial class PartConfForm : Form
    {
        public string PartID { get; set; }
        public string PartProgram { get; set; }
        public string PartDescription { get; set; }
        public PartConfForm()
        {
            InitializeComponent();
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (((Control)sender).Text.Trim().Length == 0)
            {
                error = "请设置检测程序";
                e.Cancel = true;
            }
            this.errorProvider1.SetError((Control)sender, error);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (((Control)sender).Text.Trim().Length == 0)
            {
                error = "请输入工件ID";
                e.Cancel = true;
            }
            this.errorProvider1.SetError((Control)sender, error);
        }

        public void prgSelecteButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Prg|*.prg";
            dlg.CheckFileExists = true;
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                PartProgram = dlg.FileName;
                textBox2.Text = Path.GetFileName(dlg.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PartID = textBox1.Text;
            PartDescription = textBox3.Text;
        }
    }
}
