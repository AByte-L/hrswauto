namespace ServerMainMold
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logListView = new System.Windows.Forms.ListView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.setupButton = new System.Windows.Forms.Button();
            this.partButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.reinitPCDmisButton = new System.Windows.Forms.Button();
            this.gotoSafePositionButton = new System.Windows.Forms.Button();
            this.clearErrorButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.curPartIDTextBox = new System.Windows.Forms.TextBox();
            this.pcProgTextBox = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 616);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(688, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(15, 8, 15, 0);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.85526F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 61.67763F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.63636F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(688, 616);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.logListView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(19, 161);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(8);
            this.panel1.Size = new System.Drawing.Size(650, 366);
            this.panel1.TabIndex = 0;
            // 
            // logListView
            // 
            this.logListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logListView.Location = new System.Drawing.Point(8, 8);
            this.logListView.Name = "logListView";
            this.logListView.Size = new System.Drawing.Size(634, 350);
            this.logListView.TabIndex = 0;
            this.logListView.UseCompatibleStateImageBehavior = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.0566F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.9434F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 217F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 243F));
            this.tableLayoutPanel2.Controls.Add(this.setupButton, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.partButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label1, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(19, 12);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(650, 76);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // setupButton
            // 
            this.setupButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupButton.Location = new System.Drawing.Point(6, 6);
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(84, 64);
            this.setupButton.TabIndex = 0;
            this.setupButton.Text = "设置";
            this.setupButton.UseVisualStyleBackColor = true;
            this.setupButton.Click += new System.EventHandler(this.setupButton_Click);
            // 
            // partButton
            // 
            this.partButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.partButton.Location = new System.Drawing.Point(96, 6);
            this.partButton.Name = "partButton";
            this.partButton.Size = new System.Drawing.Size(87, 64);
            this.partButton.TabIndex = 1;
            this.partButton.Text = "工件列表";
            this.partButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(406, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(238, 70);
            this.label1.TabIndex = 2;
            this.label1.Text = "放置图标或服务器文本";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel3.Controls.Add(this.reinitPCDmisButton, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.gotoSafePositionButton, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.clearErrorButton, 3, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(19, 534);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(650, 78);
            this.tableLayoutPanel3.TabIndex = 2;
            // 
            // reinitPCDmisButton
            // 
            this.reinitPCDmisButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reinitPCDmisButton.Location = new System.Drawing.Point(136, 8);
            this.reinitPCDmisButton.Name = "reinitPCDmisButton";
            this.reinitPCDmisButton.Size = new System.Drawing.Size(122, 62);
            this.reinitPCDmisButton.TabIndex = 0;
            this.reinitPCDmisButton.Text = "重新初始化";
            this.reinitPCDmisButton.UseVisualStyleBackColor = true;
            // 
            // gotoSafePositionButton
            // 
            this.gotoSafePositionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gotoSafePositionButton.Location = new System.Drawing.Point(264, 8);
            this.gotoSafePositionButton.Name = "gotoSafePositionButton";
            this.gotoSafePositionButton.Size = new System.Drawing.Size(122, 62);
            this.gotoSafePositionButton.TabIndex = 1;
            this.gotoSafePositionButton.Text = "回安全位置";
            this.gotoSafePositionButton.UseVisualStyleBackColor = true;
            // 
            // clearErrorButton
            // 
            this.clearErrorButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clearErrorButton.Location = new System.Drawing.Point(392, 8);
            this.clearErrorButton.Name = "clearErrorButton";
            this.clearErrorButton.Size = new System.Drawing.Size(122, 62);
            this.clearErrorButton.TabIndex = 2;
            this.clearErrorButton.Text = "清除错误";
            this.clearErrorButton.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.64174F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.35825F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.curPartIDTextBox, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.pcProgTextBox, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(19, 95);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(650, 59);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(8, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 24);
            this.label2.TabIndex = 0;
            this.label2.Text = "工件标识：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(8, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "检测程序：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // curPartIDTextBox
            // 
            this.curPartIDTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.curPartIDTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.curPartIDTextBox.Enabled = false;
            this.curPartIDTextBox.Location = new System.Drawing.Point(101, 8);
            this.curPartIDTextBox.Name = "curPartIDTextBox";
            this.curPartIDTextBox.Size = new System.Drawing.Size(541, 14);
            this.curPartIDTextBox.TabIndex = 2;
            // 
            // pcProgTextBox
            // 
            this.pcProgTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pcProgTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pcProgTextBox.Enabled = false;
            this.pcProgTextBox.Location = new System.Drawing.Point(101, 32);
            this.pcProgTextBox.Name = "pcProgTextBox";
            this.pcProgTextBox.Size = new System.Drawing.Size(541, 14);
            this.pcProgTextBox.TabIndex = 3;
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(688, 638);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainFrm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFrm_FormClosed);
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView logListView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button setupButton;
        private System.Windows.Forms.Button partButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button reinitPCDmisButton;
        private System.Windows.Forms.Button gotoSafePositionButton;
        private System.Windows.Forms.Button clearErrorButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox curPartIDTextBox;
        private System.Windows.Forms.TextBox pcProgTextBox;
    }
}

