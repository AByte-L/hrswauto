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
            this.resultPanel = new System.Windows.Forms.Panel();
            this.ResultView = new System.Windows.Forms.DataGridView();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column17 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column18 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.partPanel = new System.Windows.Forms.Panel();
            this.partView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.addPartToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.modifyToolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.delPartToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.writePartIDToPlcToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.cmmPanel = new System.Windows.Forms.Panel();
            this.CmmView = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewImageColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.cmmToolStrip = new System.Windows.Forms.ToolStrip();
            this.addCmmTsb = new System.Windows.Forms.ToolStripButton();
            this.deleteCmmTsb = new System.Windows.Forms.ToolStripButton();
            this.enableCmmTsb = new System.Windows.Forms.ToolStripButton();
            this.disableCmmTsb = new System.Windows.Forms.ToolStripButton();
            this.plcPanel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.mainToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.resultPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultView)).BeginInit();
            this.partPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.partView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.cmmPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmmView)).BeginInit();
            this.cmmToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
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
            this.splitContainer1.Panel2.Controls.Add(this.resultPanel);
            this.splitContainer1.Panel2.Controls.Add(this.partPanel);
            this.splitContainer1.Panel2.Controls.Add(this.cmmPanel);
            this.splitContainer1.Panel2.Controls.Add(this.plcPanel);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.splitContainer1.Size = new System.Drawing.Size(1069, 803);
            this.splitContainer1.SplitterDistance = 121;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // mainToolStrip
            // 
            this.mainToolStrip.BackColor = System.Drawing.Color.White;
            this.mainToolStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mainToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resultTtoolStripButton,
            this.cmmToolStripButton,
            this.partToolStripButton,
            this.plcToolStripButton});
            this.mainToolStrip.Location = new System.Drawing.Point(634, 0);
            this.mainToolStrip.Name = "mainToolStrip";
            this.mainToolStrip.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.mainToolStrip.Size = new System.Drawing.Size(435, 121);
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
            this.resultTtoolStripButton.Click += new System.EventHandler(this.resultTtoolStripButton_Click);
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
            this.plcToolStripButton.Click += new System.EventHandler(this.plcToolStripButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(10, 10, 10, 15);
            this.pictureBox1.Size = new System.Drawing.Size(634, 121);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(10, 10);
            this.splitter1.Margin = new System.Windows.Forms.Padding(2);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1049, 2);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // resultPanel
            // 
            this.resultPanel.Controls.Add(this.ResultView);
            this.resultPanel.Controls.Add(this.label3);
            this.resultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultPanel.Location = new System.Drawing.Point(10, 10);
            this.resultPanel.Margin = new System.Windows.Forms.Padding(2);
            this.resultPanel.Name = "resultPanel";
            this.resultPanel.Size = new System.Drawing.Size(1049, 669);
            this.resultPanel.TabIndex = 2;
            // 
            // ResultView
            // 
            this.ResultView.AllowUserToAddRows = false;
            this.ResultView.AllowUserToDeleteRows = false;
            this.ResultView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ResultView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column12,
            this.Column13,
            this.Column14,
            this.Column15,
            this.Column16,
            this.Column17,
            this.Column18});
            this.ResultView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultView.Location = new System.Drawing.Point(0, 10);
            this.ResultView.Name = "ResultView";
            this.ResultView.ReadOnly = true;
            this.ResultView.RowTemplate.Height = 23;
            this.ResultView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ResultView.Size = new System.Drawing.Size(1049, 659);
            this.ResultView.TabIndex = 3;
            // 
            // Column12
            // 
            this.Column12.FillWeight = 50F;
            this.Column12.HeaderText = "块架槽号";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            // 
            // Column13
            // 
            this.Column13.HeaderText = "工件标号";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            // 
            // Column14
            // 
            this.Column14.HeaderText = "检测程序";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            // 
            // Column15
            // 
            this.Column15.HeaderText = "工件槽状态";
            this.Column15.Name = "Column15";
            this.Column15.ReadOnly = true;
            // 
            // Column16
            // 
            this.Column16.HeaderText = "是否合格";
            this.Column16.Name = "Column16";
            this.Column16.ReadOnly = true;
            // 
            // Column17
            // 
            this.Column17.HeaderText = "查看报告";
            this.Column17.Name = "Column17";
            this.Column17.ReadOnly = true;
            // 
            // Column18
            // 
            this.Column18.HeaderText = "报告目录";
            this.Column18.Name = "Column18";
            this.Column18.ReadOnly = true;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(1049, 10);
            this.label3.TabIndex = 2;
            // 
            // partPanel
            // 
            this.partPanel.Controls.Add(this.partView);
            this.partPanel.Controls.Add(this.label1);
            this.partPanel.Controls.Add(this.toolStrip1);
            this.partPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partPanel.Location = new System.Drawing.Point(10, 10);
            this.partPanel.Margin = new System.Windows.Forms.Padding(2);
            this.partPanel.Name = "partPanel";
            this.partPanel.Size = new System.Drawing.Size(1049, 669);
            this.partPanel.TabIndex = 1;
            // 
            // partView
            // 
            this.partView.AllowUserToAddRows = false;
            this.partView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.partView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.partView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column7,
            this.Column9,
            this.Column10,
            this.Column11,
            this.Column8});
            this.partView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partView.Location = new System.Drawing.Point(0, 10);
            this.partView.Name = "partView";
            this.partView.RowTemplate.Height = 23;
            this.partView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.partView.Size = new System.Drawing.Size(1049, 634);
            this.partView.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "工件ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "检测程序";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "理论文件";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "算法文件";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "公差文件";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "工件说明";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1049, 10);
            this.label1.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPartToolStripButton,
            this.modifyToolStripButton1,
            this.delPartToolStripButton,
            this.toolStripSeparator1,
            this.writePartIDToPlcToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 644);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1049, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // addPartToolStripButton
            // 
            this.addPartToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.addPartToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("addPartToolStripButton.Image")));
            this.addPartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.addPartToolStripButton.Name = "addPartToolStripButton";
            this.addPartToolStripButton.Size = new System.Drawing.Size(24, 22);
            this.addPartToolStripButton.Text = "toolStripButton1";
            this.addPartToolStripButton.Click += new System.EventHandler(this.addPartToolStripButton_Click);
            // 
            // modifyToolStripButton1
            // 
            this.modifyToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.modifyToolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("modifyToolStripButton1.Image")));
            this.modifyToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.modifyToolStripButton1.Name = "modifyToolStripButton1";
            this.modifyToolStripButton1.Size = new System.Drawing.Size(24, 22);
            this.modifyToolStripButton1.Text = "toolStripButton1";
            this.modifyToolStripButton1.Click += new System.EventHandler(this.modifyToolStripButton1_Click);
            // 
            // delPartToolStripButton
            // 
            this.delPartToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.delPartToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("delPartToolStripButton.Image")));
            this.delPartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delPartToolStripButton.Name = "delPartToolStripButton";
            this.delPartToolStripButton.Size = new System.Drawing.Size(24, 22);
            this.delPartToolStripButton.Text = "toolStripButton2";
            this.delPartToolStripButton.Click += new System.EventHandler(this.delPartToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // writePartIDToPlcToolStripButton
            // 
            this.writePartIDToPlcToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.writePartIDToPlcToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("writePartIDToPlcToolStripButton.Image")));
            this.writePartIDToPlcToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.writePartIDToPlcToolStripButton.Name = "writePartIDToPlcToolStripButton";
            this.writePartIDToPlcToolStripButton.Size = new System.Drawing.Size(24, 22);
            this.writePartIDToPlcToolStripButton.Text = "toolStripButton1";
            this.writePartIDToPlcToolStripButton.Click += new System.EventHandler(this.writePartIDToPlcToolStripButton_Click);
            // 
            // cmmPanel
            // 
            this.cmmPanel.Controls.Add(this.CmmView);
            this.cmmPanel.Controls.Add(this.label2);
            this.cmmPanel.Controls.Add(this.cmmToolStrip);
            this.cmmPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmmPanel.Location = new System.Drawing.Point(10, 10);
            this.cmmPanel.Margin = new System.Windows.Forms.Padding(2);
            this.cmmPanel.Name = "cmmPanel";
            this.cmmPanel.Size = new System.Drawing.Size(1049, 669);
            this.cmmPanel.TabIndex = 4;
            // 
            // CmmView
            // 
            this.CmmView.AllowUserToAddRows = false;
            this.CmmView.AllowUserToDeleteRows = false;
            this.CmmView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CmmView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CmmView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.CmmView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CmmView.Location = new System.Drawing.Point(0, 10);
            this.CmmView.MultiSelect = false;
            this.CmmView.Name = "CmmView";
            this.CmmView.ReadOnly = true;
            this.CmmView.RowTemplate.Height = 50;
            this.CmmView.Size = new System.Drawing.Size(1049, 632);
            this.CmmView.TabIndex = 1;
            // 
            // Column6
            // 
            this.Column6.FillWeight = 20F;
            this.Column6.HeaderText = "激活";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
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
            this.Column5.FillWeight = 50F;
            this.Column5.HeaderText = "";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1049, 10);
            this.label2.TabIndex = 2;
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
            this.cmmToolStrip.Location = new System.Drawing.Point(0, 642);
            this.cmmToolStrip.Name = "cmmToolStrip";
            this.cmmToolStrip.Size = new System.Drawing.Size(1049, 27);
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
            this.deleteCmmTsb.Click += new System.EventHandler(this.deleteCmmTsb_Click);
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
            this.enableCmmTsb.Click += new System.EventHandler(this.enableCmmTsb_Click);
            // 
            // disableCmmTsb
            // 
            this.disableCmmTsb.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.disableCmmTsb.Image = ((System.Drawing.Image)(resources.GetObject("disableCmmTsb.Image")));
            this.disableCmmTsb.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.disableCmmTsb.Name = "disableCmmTsb";
            this.disableCmmTsb.Size = new System.Drawing.Size(24, 24);
            this.disableCmmTsb.Text = "toolStripButton1";
            this.disableCmmTsb.ToolTipText = "单机处理";
            this.disableCmmTsb.Click += new System.EventHandler(this.disableCmmTsb_Click);
            // 
            // plcPanel
            // 
            this.plcPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plcPanel.Location = new System.Drawing.Point(10, 10);
            this.plcPanel.Margin = new System.Windows.Forms.Padding(2);
            this.plcPanel.Name = "plcPanel";
            this.plcPanel.Size = new System.Drawing.Size(1049, 669);
            this.plcPanel.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Location = new System.Drawing.Point(0, 803);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 11, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1069, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1069, 825);
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
            this.resultPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultView)).EndInit();
            this.partPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.partView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.cmmPanel.ResumeLayout(false);
            this.cmmPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmmView)).EndInit();
            this.cmmToolStrip.ResumeLayout(false);
            this.cmmToolStrip.PerformLayout();
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
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton addCmmTsb;
        private System.Windows.Forms.ToolStripButton deleteCmmTsb;
        private System.Windows.Forms.ToolStripButton enableCmmTsb;
        private System.Windows.Forms.ToolStripButton disableCmmTsb;
        private System.Windows.Forms.DataGridView CmmView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewImageColumn Column5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView partView;
        private System.Windows.Forms.ToolStripButton addPartToolStripButton;
        private System.Windows.Forms.ToolStripButton delPartToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton writePartIDToPlcToolStripButton;
        private System.Windows.Forms.ToolStripButton modifyToolStripButton1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView ResultView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
        private System.Windows.Forms.DataGridViewButtonColumn Column17;
        private System.Windows.Forms.DataGridViewButtonColumn Column18;
    }
}

