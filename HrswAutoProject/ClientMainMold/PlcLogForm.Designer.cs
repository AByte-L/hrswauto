namespace ClientMainMold
{
    partial class PlcLogForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plcActionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logStringDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.plcLogBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plcLogBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CloseBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 526);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5, 8, 10, 8);
            this.panel1.Size = new System.Drawing.Size(692, 58);
            this.panel1.TabIndex = 1;
            // 
            // CloseBtn
            // 
            this.CloseBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.CloseBtn.Location = new System.Drawing.Point(552, 8);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(130, 42);
            this.CloseBtn.TabIndex = 0;
            this.CloseBtn.Text = "关闭";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dTimeDataGridViewTextBoxColumn,
            this.plcActionDataGridViewTextBoxColumn,
            this.logStringDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.plcLogBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Location = new System.Drawing.Point(5, 5);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(692, 521);
            this.dataGridView1.TabIndex = 1;
            // 
            // dTimeDataGridViewTextBoxColumn
            // 
            this.dTimeDataGridViewTextBoxColumn.DataPropertyName = "DTime";
            dataGridViewCellStyle1.Format = "T";
            dataGridViewCellStyle1.NullValue = null;
            this.dTimeDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.dTimeDataGridViewTextBoxColumn.FillWeight = 15F;
            this.dTimeDataGridViewTextBoxColumn.HeaderText = "时间";
            this.dTimeDataGridViewTextBoxColumn.Name = "dTimeDataGridViewTextBoxColumn";
            this.dTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // plcActionDataGridViewTextBoxColumn
            // 
            this.plcActionDataGridViewTextBoxColumn.DataPropertyName = "PlcAction";
            this.plcActionDataGridViewTextBoxColumn.FillWeight = 15F;
            this.plcActionDataGridViewTextBoxColumn.HeaderText = "PLC动作";
            this.plcActionDataGridViewTextBoxColumn.Name = "plcActionDataGridViewTextBoxColumn";
            this.plcActionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // logStringDataGridViewTextBoxColumn
            // 
            this.logStringDataGridViewTextBoxColumn.DataPropertyName = "LogString";
            this.logStringDataGridViewTextBoxColumn.FillWeight = 70F;
            this.logStringDataGridViewTextBoxColumn.HeaderText = "日志";
            this.logStringDataGridViewTextBoxColumn.Name = "logStringDataGridViewTextBoxColumn";
            this.logStringDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // plcLogBindingSource
            // 
            this.plcLogBindingSource.DataSource = typeof(ClientMainMold.PlcLog);
            // 
            // PlcLogForm
            // 
            this.AcceptButton = this.CloseBtn;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 589);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlcLogForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "PlcLogForm";
            this.Load += new System.EventHandler(this.PlcLogForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plcLogBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn plcActionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn logStringDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource plcLogBindingSource;
    }
}