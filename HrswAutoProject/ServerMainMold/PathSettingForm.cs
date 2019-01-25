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

namespace ServerMainMold
{
    public partial class PathSettingForm : Form
    {
 
        public string BladeExe
        {
            get { return listView1.Items[0].SubItems[1].Text; }
            set { listView1.Items[0].SubItems[1].Text = value; }
        }

        public string GotoProg
        {
            get { return listView1.Items[1].SubItems[1].Text; }
            set { listView1.Items[1].SubItems[1].Text = value; }
        }

        public string LogFilePath
        {
            get { return listView1.Items[2].SubItems[1].Text; }
            set { listView1.Items[2].SubItems[1].Text = value; }
        }

        public string RootPath
        {
            get { return listView1.Items[3].SubItems[1].Text; }
            set { listView1.Items[3].SubItems[1].Text = value; }
        }

        public PathSettingForm()
        {
            InitializeComponent();
            button1.DialogResult = DialogResult.OK;
            button2.DialogResult = DialogResult.Cancel;
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems.Count < 0)
            {
                return;
            }
            switch (listView1.SelectedItems[0].Index)
            {
                case 0:
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Filter = "blade|blade.exe";
                    if (dlg.ShowDialog() == DialogResult.OK)
                    {
                        listView1.Items[0].SubItems[1].Text = dlg.FileName;
                    }
                    break;
                case 1:
                    OpenFileDialog dlg1 = new OpenFileDialog();
                    dlg1.Filter = "GotoSafe|*.prg";
                    if (dlg1.ShowDialog() == DialogResult.OK)
                    {
                        listView1.Items[1].SubItems[1].Text = dlg1.FileName;
                    }
                    break;
                case 2:
                    OpenFileDialog dlg2 = new OpenFileDialog();
                    dlg2.Filter = "Log文件|*.txt";
                    if (dlg2.ShowDialog() == DialogResult.OK)
                    {
                        listView1.Items[2].SubItems[1].Text = dlg2.FileName;
                    }
                    break;
                case 3:
                    FolderBrowserDialog dialog = new FolderBrowserDialog();
                    dialog.Description = "请选择文件路径";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        listView1.Items[3].SubItems[1].Text = dialog.SelectedPath;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
