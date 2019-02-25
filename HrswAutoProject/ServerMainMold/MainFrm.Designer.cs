namespace ServerMainMold
{
    partial class MainFrm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.infoDataGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SetupToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.gotoToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.RestartToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ClearErrorToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.infoDataGridView);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(8, 2);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.groupBox1.Size = new System.Drawing.Size(607, 465);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // infoDataGridView
            // 
            this.infoDataGridView.AllowUserToAddRows = false;
            this.infoDataGridView.AllowUserToDeleteRows = false;
            this.infoDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.infoDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            this.infoDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoDataGridView.Location = new System.Drawing.Point(6, 20);
            this.infoDataGridView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.infoDataGridView.MultiSelect = false;
            this.infoDataGridView.Name = "infoDataGridView";
            this.infoDataGridView.ReadOnly = true;
            this.infoDataGridView.RowTemplate.Height = 27;
            this.infoDataGridView.Size = new System.Drawing.Size(595, 439);
            this.infoDataGridView.TabIndex = 0;
            // 
            // Column1
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.HeaderText = "时间";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 120;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column2.HeaderText = "信息";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "info.jpg");
            this.imageList1.Images.SetKeyName(1, "warn.jpg");
            this.imageList1.Images.SetKeyName(2, "error.jpg");
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetupToolStripButton,
            this.gotoToolStripButton,
            this.RestartToolStripButton,
            this.ClearErrorToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(8, 467);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(607, 47);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // SetupToolStripButton
            // 
            this.SetupToolStripButton.AutoSize = false;
            this.SetupToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SetupToolStripButton.Image")));
            this.SetupToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SetupToolStripButton.Name = "SetupToolStripButton";
            this.SetupToolStripButton.Size = new System.Drawing.Size(73, 44);
            this.SetupToolStripButton.Text = "设置";
            this.SetupToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.SetupToolStripButton.Click += new System.EventHandler(this.SetupToolStripButton_Click);
            // 
            // gotoToolStripButton
            // 
            this.gotoToolStripButton.AutoSize = false;
            this.gotoToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("gotoToolStripButton.Image")));
            this.gotoToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.gotoToolStripButton.Name = "gotoToolStripButton";
            this.gotoToolStripButton.Size = new System.Drawing.Size(73, 44);
            this.gotoToolStripButton.Text = "回安全位";
            this.gotoToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.gotoToolStripButton.Click += new System.EventHandler(this.gotoToolStripButton_Click);
            // 
            // RestartToolStripButton
            // 
            this.RestartToolStripButton.AutoSize = false;
            this.RestartToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("RestartToolStripButton.Image")));
            this.RestartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RestartToolStripButton.Name = "RestartToolStripButton";
            this.RestartToolStripButton.Size = new System.Drawing.Size(73, 44);
            this.RestartToolStripButton.Text = "重启PC";
            this.RestartToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.RestartToolStripButton.Click += new System.EventHandler(this.RestartToolStripButton_Click);
            // 
            // ClearErrorToolStripButton
            // 
            this.ClearErrorToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ClearErrorToolStripButton.Image")));
            this.ClearErrorToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearErrorToolStripButton.Name = "ClearErrorToolStripButton";
            this.ClearErrorToolStripButton.Size = new System.Drawing.Size(60, 44);
            this.ClearErrorToolStripButton.Text = "清除错误";
            this.ClearErrorToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ClearErrorToolStripButton.Click += new System.EventHandler(this.ClearErrorToolStripButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(8, 514);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 10, 0);
            this.statusStrip1.Size = new System.Drawing.Size(607, 13);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "服务器";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(623, 531);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.Padding = new System.Windows.Forms.Padding(8, 2, 8, 4);
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "MainFrm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.SizeChanged += new System.EventHandler(this.MainFrm_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.infoDataGridView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton SetupToolStripButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.DataGridView infoDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.ToolStripButton ClearErrorToolStripButton;
        private System.Windows.Forms.ToolStripButton gotoToolStripButton;
        private System.Windows.Forms.ToolStripButton RestartToolStripButton;
    }
}