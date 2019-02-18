namespace ClientMainMold
{
    partial class MainFrm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFrm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mainToolStrip = new System.Windows.Forms.ToolStrip();
            this.resultTtoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.cmmToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.partToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.plcToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.cmmPanel = new System.Windows.Forms.Panel();
            this.cmmToolStrip = new System.Windows.Forms.ToolStrip();
            this.addCmmTsb = new System.Windows.Forms.ToolStripButton();
            this.deleteCmmTsb = new System.Windows.Forms.ToolStripButton();
            this.enableCmmTsb = new System.Windows.Forms.ToolStripButton();
            this.disableCmmTsb = new System.Windows.Forms.ToolStripButton();
            this.partPanel = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.plcPanel = new System.Windows.Forms.Panel();
            this.resultPanel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.CmmView = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.cmmPanel.SuspendLayout();
            this.cmmToolStrip.SuspendLayout();
            this.partPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmmView)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mainToolStrip);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitter1);
            this.splitContainer1.Panel2.Controls.Add(this.cmmPanel);
            this.splitContainer1.Panel2.Controls.Add(this.partPanel);
            this.splitContainer1.Panel2.Controls.Add(this.plcPanel);
            this.splitContainer1.Panel2.Controls.Add(this.resultPanel);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.splitContainer1.Size = new System.Drawing.Size(843, 569);
            this.splitContainer1.SplitterDistance = 86;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resultTtoolStripButton,
            this.cmmToolStripButton,
            this.partToolStripButton,
            this.plcToolStripButton});
            this.mainToolStrip.Location = new System.Drawing.Point(324, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainToolStrip.Size = new System.Drawing.Size(519, 86);
            this.mainToolStrip.TabIndex = 1;
            this.mainToolStrip.Text = "toolStrip1";
            // 
            // resultTtoolStripButton
            // 
            this.resultTtoolStripButton.AutoSize = false;
            this.resultTtoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("resultTtoolStripButton.Image")));
            this.resultTtoolStripButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.resultTtoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.resultTtoolStripButton.Name = "resultTtoolStripButton";
            this.resultTtoolStripButton.Size = new System.Drawing.Size(100, 106);
            this.resultTtoolStripButton.Text = "检测结果";
            this.resultTtoolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.resultTtoolStripButton.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal;
            this.resultTtoolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // cmmToolStripButton
            // 
            this.cmmToolStripButton.AutoSize = false;
            this.cmmToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cmmToolStripButton.Image")));
            this.cmmToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmmToolStripButton.Name = "cmmToolStripButton";
            this.cmmToolStripButton.Size = new System.Drawing.Size(100, 106);
            this.cmmToolStripButton.Text = "测量机";
            this.cmmToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cmmToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.cmmToolStripButton.Click += new System.EventHandler(this.cmmToolStripButton_Click);
            // 
            // partToolStripButton
            // 
            this.partToolStripButton.AutoSize = false;
            this.partToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("partToolStripButton.Image")));
            this.partToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.partToolStripButton.Name = "partToolStripButton";
            this.partToolStripButton.RightToLeftAutoMirrorImage = true;
            this.partToolStripButton.Size = new System.Drawing.Size(100, 106);
            this.partToolStripButton.Text = "工件";
            this.partToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.partToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.partToolStripButton.Click += new System.EventHandler(this.partToolStripButton_Click);
            // 
            // plcToolStripButton
            // 
            this.plcToolStripButton.AutoSize = false;
            this.plcToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("plcToolStripButton.Image")));
            this.plcToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.plcToolStripButton.Name = "plcToolStripButton";
            this.plcToolStripButton.Size = new System.Drawing.Size(100, 106);
            this.plcToolStripButton.Text = "PLC";
            this.plcToolStripButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.plcToolStripButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(324, 86);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(10, 10);
            this.splitter1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(823, 2);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // cmmPanel
            // 
            this.cmmPanel.Controls.Add(this.CmmView);
            this.cmmPanel.Controls.Add(this.cmmToolStrip);
            this.cmmPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmmPanel.Location = new System.Drawing.Point(10, 10);
            this.cmmPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmmPanel.Name = "cmmPanel";
            this.cmmPanel.Size = new System.Drawing.Size(823, 470);
            this.cmmPanel.TabIndex = 4;
            // 
            // cmmToolStrip
            // 
            this.cmmToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmmToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.cmmToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmmToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCmmTsb,
            this.deleteCmmTsb,
            this.enableCmmTsb,
            this.disableCmmTsb});
            this.cmmToolStrip.Location = new System.Drawing.Point(0, 443);
            this.cmmToolStrip.Name = "cmmToolStrip";
            this.cmmToolStrip.Size = new System.Drawing.Size(823, 27);
            this.cmmToolStrip.TabIndex = 0;
            this.cmmToolStrip.Text = "toolStrip2";
            // 
            // addCmmTsb
            // 
            this.addCmmTsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addCmmTsb.Image = ((System.Drawing.Image)(resources.GetObject("addCmmTsb.Image")));
            this.addCmmTsb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addCmmTsb.Name = "addCmmTsb";
            this.addCmmTsb.Size = new System.Drawing.Size(24, 24);
            this.addCmmTsb.Text = "toolStripButton1";
            this.addCmmTsb.ToolTipText = "添加";
            this.addCmmTsb.Click += new System.EventHandler(this.addCmmTsb_Click);
            // 
            // deleteCmmTsb
            // 
            this.deleteCmmTsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteCmmTsb.Image = ((System.Drawing.Image)(resources.GetObject("deleteCmmTsb.Image")));
            this.deleteCmmTsb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.deleteCmmTsb.Name = "deleteCmmTsb";
            this.deleteCmmTsb.Size = new System.Drawing.Size(24, 24);
            this.deleteCmmTsb.Text = "toolStripButton1";
            this.deleteCmmTsb.ToolTipText = "删除";
            // 
            // enableCmmTsb
            // 
            this.enableCmmTsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.enableCmmTsb.Image = ((System.Drawing.Image)(resources.GetObject("enableCmmTsb.Image")));
            this.enableCmmTsb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.enableCmmTsb.Name = "enableCmmTsb";
            this.enableCmmTsb.Size = new System.Drawing.Size(24, 24);
            this.enableCmmTsb.Text = "toolStripButton1";
            this.enableCmmTsb.ToolTipText = "激活";
            // 
            // disableCmmTsb
            // 
            this.disableCmmTsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.disableCmmTsb.Image = ((System.Drawing.Image)(resources.GetObject("disableCmmTsb.Image")));
            this.disableCmmTsb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.disableCmmTsb.Name = "disableCmmTsb";
            this.disableCmmTsb.Size = new System.Drawing.Size(24, 24);
            this.disableCmmTsb.Text = "toolStripButton1";
            // 
            // partPanel
            // 
            this.partPanel.Controls.Add(this.dataGridView1);
            this.partPanel.Controls.Add(this.toolStrip1);
            this.partPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partPanel.Location = new System.Drawing.Point(10, 10);
            this.partPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.partPanel.Name = "partPanel";
            this.partPanel.Size = new System.Drawing.Size(823, 470);
            this.partPanel.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(823, 445);
            this.dataGridView1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(0, 445);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(823, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // plcPanel
            // 
            this.plcPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcPanel.Location = new System.Drawing.Point(10, 10);
            this.plcPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.plcPanel.Name = "plcPanel";
            this.plcPanel.Size = new System.Drawing.Size(823, 470);
            this.plcPanel.TabIndex = 3;
            // 
            // resultPanel
            // 
            this.resultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPanel.Location = new System.Drawing.Point(10, 10);
            this.resultPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(823, 470);
            this.resultPanel.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 569);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            this.statusStrip1.Size = new System.Drawing.Size(843, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // CmmView
            // 
            this.CmmView.AllowUserToAddRows = false;
            this.CmmView.AllowUserToDeleteRows = false;
            this.CmmView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CmmView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CmmView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.CmmView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CmmView.Location = new System.Drawing.Point(0, 0);
            this.CmmView.MultiSelect = false;
            this.CmmView.Name = "CmmView";
            this.CmmView.ReadOnly = true;
            this.CmmView.RowTemplate.Height = 50;
            this.CmmView.Size = new System.Drawing.Size(823, 443);
            this.CmmView.TabIndex = 1;
            // 
            // Column6
            // 
            this.Column6.FillWeight = 30F;
            this.Column6.HeaderText = "激活";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 89.54314F;
            this.Column1.HeaderText = "机器编号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column2
            // 
            this.Column2.FillWeight = 89.54314F;
            this.Column2.HeaderText = "服务号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.FillWeight = 89.54314F;
            this.Column3.HeaderText = "IP地址";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.FillWeight = 89.54314F;
            this.Column4.HeaderText = "状态";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.FillWeight = 89.54314F;
            this.Column5.HeaderText = "";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(843, 591);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainFrm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.mainToolStrip.ResumeLayout(false);
            this.mainToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.cmmPanel.ResumeLayout(false);
            this.cmmPanel.PerformLayout();
            this.cmmToolStrip.ResumeLayout(false);
            this.cmmToolStrip.PerformLayout();
            this.partPanel.ResumeLayout(false);
            this.partPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmmView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStrip mainToolStrip;
        private System.Windows.Forms.ToolStripButton cmmToolStripButton;
        private System.Windows.Forms.ToolStripButton partToolStripButton;
        private System.Windows.Forms.ToolStripButton plcToolStripButton;
        private System.Windows.Forms.ToolStripButton resultTtoolStripButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel cmmPanel;
        private System.Windows.Forms.Panel plcPanel;
        private System.Windows.Forms.Panel resultPanel;
        private System.Windows.Forms.Panel partPanel;
        private System.Windows.Forms.ToolStrip cmmToolStrip;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addCmmTsb;
        private System.Windows.Forms.ToolStripButton deleteCmmTsb;
        private System.Windows.Forms.ToolStripButton enableCmmTsb;
        private System.Windows.Forms.ToolStripButton disableCmmTsb;
        private System.Windows.Forms.DataGridView CmmView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewImageColumn Column5;
    }
}

