namespace ServerMainMold
{
    partial class SetupFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.pathSettingButton = new System.Windows.Forms.Button();
            this.programsPathTextBox = new System.Windows.Forms.TextBox();
            this.bladesPathTextBox = new System.Windows.Forms.TextBox();
            this.rootPathTextBox = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.progTextBox = new System.Windows.Forms.TextBox();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.bladeTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "路径";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(9, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(605, 1);
            this.panel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Blade";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(19, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "定位程序";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "超时时长";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(8, 228);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(605, 1);
            this.panel2.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(18, 235);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 23);
            this.label6.TabIndex = 14;
            this.label6.Text = "PCDmis超时时长";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(18, 299);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 16);
            this.checkBox1.TabIndex = 16;
            this.checkBox1.Text = "最小化到托盘";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(445, 281);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(81, 37);
            this.okButton.TabIndex = 17;
            this.okButton.Text = "确定";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(532, 281);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(81, 37);
            this.cancelButton.TabIndex = 17;
            this.cancelButton.Text = "取消";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(264, 235);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 23);
            this.label7.TabIndex = 18;
            this.label7.Text = "Blade分析时长";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(8, 263);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(605, 1);
            this.panel3.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(188, 240);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "分钟";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(426, 240);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "分钟";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "日志文件";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(19, 110);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 23);
            this.label10.TabIndex = 22;
            this.label10.Text = "根目录";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(19, 137);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 23);
            this.label11.TabIndex = 24;
            this.label11.Text = "Blades目录";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(19, 164);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 23);
            this.label12.TabIndex = 26;
            this.label12.Text = "程序目录";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pathSettingButton
            // 
            this.pathSettingButton.Location = new System.Drawing.Point(533, 192);
            this.pathSettingButton.Name = "pathSettingButton";
            this.pathSettingButton.Size = new System.Drawing.Size(81, 29);
            this.pathSettingButton.TabIndex = 34;
            this.pathSettingButton.Text = "更改";
            this.pathSettingButton.UseVisualStyleBackColor = true;
            this.pathSettingButton.Click += new System.EventHandler(this.pathSettingButton_Click);
            // 
            // programsPathTextBox
            // 
            this.programsPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ServerMainMold.Properties.Settings.Default, "ProgramsPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.programsPathTextBox.Enabled = false;
            this.programsPathTextBox.Location = new System.Drawing.Point(105, 165);
            this.programsPathTextBox.Name = "programsPathTextBox";
            this.programsPathTextBox.Size = new System.Drawing.Size(509, 21);
            this.programsPathTextBox.TabIndex = 27;
            this.programsPathTextBox.Text = global::ServerMainMold.Properties.Settings.Default.ProgramsPath;
            // 
            // bladesPathTextBox
            // 
            this.bladesPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ServerMainMold.Properties.Settings.Default, "BladesPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bladesPathTextBox.Enabled = false;
            this.bladesPathTextBox.Location = new System.Drawing.Point(105, 138);
            this.bladesPathTextBox.Name = "bladesPathTextBox";
            this.bladesPathTextBox.Size = new System.Drawing.Size(509, 21);
            this.bladesPathTextBox.TabIndex = 25;
            this.bladesPathTextBox.Text = global::ServerMainMold.Properties.Settings.Default.BladesPath;
            // 
            // rootPathTextBox
            // 
            this.rootPathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ServerMainMold.Properties.Settings.Default, "RootPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rootPathTextBox.Enabled = false;
            this.rootPathTextBox.Location = new System.Drawing.Point(105, 111);
            this.rootPathTextBox.Name = "rootPathTextBox";
            this.rootPathTextBox.Size = new System.Drawing.Size(509, 21);
            this.rootPathTextBox.TabIndex = 23;
            this.rootPathTextBox.Text = global::ServerMainMold.Properties.Settings.Default.RootPath;
            // 
            // textBox5
            // 
            this.textBox5.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ServerMainMold.Properties.Settings.Default, "BladeTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox5.Location = new System.Drawing.Point(360, 236);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(60, 21);
            this.textBox5.TabIndex = 19;
            this.textBox5.Text = global::ServerMainMold.Properties.Settings.Default.BladeTimeout;
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox5.Validating += new System.ComponentModel.CancelEventHandler(this.textBox5_Validating);
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ServerMainMold.Properties.Settings.Default, "PCDmisTimeout", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox4.Location = new System.Drawing.Point(114, 236);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(67, 21);
            this.textBox4.TabIndex = 15;
            this.textBox4.Text = global::ServerMainMold.Properties.Settings.Default.PCDmisTimeout;
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox4.Validating += new System.ComponentModel.CancelEventHandler(this.textBox4_Validating);
            // 
            // progTextBox
            // 
            this.progTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ServerMainMold.Properties.Settings.Default, "GotoProg", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.progTextBox.Enabled = false;
            this.progTextBox.Location = new System.Drawing.Point(105, 57);
            this.progTextBox.Name = "progTextBox";
            this.progTextBox.Size = new System.Drawing.Size(509, 21);
            this.progTextBox.TabIndex = 11;
            this.progTextBox.Text = global::ServerMainMold.Properties.Settings.Default.GotoProg;
            // 
            // logTextBox
            // 
            this.logTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ServerMainMold.Properties.Settings.Default, "LogFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.logTextBox.Enabled = false;
            this.logTextBox.Location = new System.Drawing.Point(105, 84);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(509, 21);
            this.logTextBox.TabIndex = 8;
            this.logTextBox.Text = global::ServerMainMold.Properties.Settings.Default.LogFilePath;
            // 
            // bladeTextBox
            // 
            this.bladeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ServerMainMold.Properties.Settings.Default, "BladeExe", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bladeTextBox.Enabled = false;
            this.bladeTextBox.Location = new System.Drawing.Point(105, 30);
            this.bladeTextBox.Name = "bladeTextBox";
            this.bladeTextBox.Size = new System.Drawing.Size(509, 21);
            this.bladeTextBox.TabIndex = 5;
            this.bladeTextBox.Text = global::ServerMainMold.Properties.Settings.Default.BladeExe;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(445, 192);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 29);
            this.button1.TabIndex = 35;
            this.button1.Text = "默认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SetupFrm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(633, 336);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pathSettingButton);
            this.Controls.Add(this.programsPathTextBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.bladesPathTextBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.rootPathTextBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.progTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bladeTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupFrm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "设置";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox bladeTextBox;
        private System.Windows.Forms.TextBox progTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox programsPathTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox bladesPathTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox rootPathTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button pathSettingButton;
        private System.Windows.Forms.Button button1;
    }
}