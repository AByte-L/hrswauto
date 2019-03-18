namespace ClientMainMold
{
    partial class WritePartIDForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.wCancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 36);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "等待写入工件标识。。。。";
            // 
            // wCancelButton
            // 
            this.wCancelButton.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.wCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.wCancelButton.Location = new System.Drawing.Point(146, 82);
            this.wCancelButton.Name = "wCancelButton";
            this.wCancelButton.Size = new System.Drawing.Size(86, 26);
            this.wCancelButton.TabIndex = 1;
            this.wCancelButton.Text = "取消";
            this.wCancelButton.UseVisualStyleBackColor = true;
            this.wCancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // WritePartIDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 129);
            this.Controls.Add(this.wCancelButton);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WritePartIDForm";
            this.Text = "WritePartIDForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WritePartIDForm_FormClosed);
            this.Load += new System.EventHandler(this.WritePartIDForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button wCancelButton;
    }
}