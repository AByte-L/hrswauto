﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace IPInputControl.Ctrl
{
    public partial class ControlText : TextBox
    {
        public ControlText()
        {
            InitializeComponent();
        }
        public void txt_TextChange(object sender, EventArgs e)
        {
            if (this.Text.Length == 3)
            {
                SendKeys.Send("{TAB}");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Tab)
            {
                if (Text.Length == 0)
                    return true;
                return false;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
