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
            this.cmmListView = new System.Windows.Forms.ListView();
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
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
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
            this.splitContainer1.Size = new System.Drawing.Size(1054, 717);
            this.splitContainer1.SplitterDistance = 109;
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
            this.mainToolStrip.Location = new System.Drawing.Point(405, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainToolStrip.Size = new System.Drawing.Size(649, 109);
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
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(405, 109);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1054, 3);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // cmmPanel
            // 
            this.cmmPanel.Controls.Add(this.cmmListView);
            this.cmmPanel.Controls.Add(this.cmmToolStrip);
            this.cmmPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmmPanel.Location = new System.Drawing.Point(0, 0);
            this.cmmPanel.Name = "cmmPanel";
            this.cmmPanel.Size = new System.Drawing.Size(1054, 604);
            this.cmmPanel.TabIndex = 4;
            // 
            // cmmListView
            // 
            this.cmmListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmmListView.Location = new System.Drawing.Point(0, 0);
            this.cmmListView.Name = "cmmListView";
            this.cmmListView.Size = new System.Drawing.Size(1054, 577);
            this.cmmListView.TabIndex = 1;
            this.cmmListView.UseCompatibleStateImageBehavior = false;
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
            this.cmmToolStrip.Location = new System.Drawing.Point(0, 577);
            this.cmmToolStrip.Name = "cmmToolStrip";
            this.cmmToolStrip.Size = new System.Drawing.Size(1054, 27);
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
            this.partPanel.Location = new System.Drawing.Point(0, 0);
            this.partPanel.Name = "partPanel";
            this.partPanel.Size = new System.Drawing.Size(1054, 604);
            this.partPanel.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(1054, 579);
            this.dataGridView1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Location = new System.Drawing.Point(0, 579);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1054, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // plcPanel
            // 
            this.plcPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcPanel.Location = new System.Drawing.Point(0, 0);
            this.plcPanel.Name = "plcPanel";
            this.plcPanel.Size = new System.Drawing.Size(1054, 604);
            this.plcPanel.TabIndex = 3;
            // 
            // resultPanel
            // 
            this.resultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPanel.Location = new System.Drawing.Point(0, 0);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(1054, 604);
            this.resultPanel.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 717);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1054, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1054, 739);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Margin = new System.Windows.Forms.Padding(4);
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
        private System.Windows.Forms.ListView cmmListView;
        private System.Windows.Forms.ToolStrip cmmToolStrip;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addCmmTsb;
        private System.Windows.Forms.ToolStripButton deleteCmmTsb;
        private System.Windows.Forms.ToolStripButton enableCmmTsb;
        private System.Windows.Forms.ToolStripButton disableCmmTsb;
    }
}

