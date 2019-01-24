namespace ServerMainMold
{
    partial class InitForm
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.InitLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 64);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(267, 20);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            // 
            // InitLabel
            // 
            this.InitLabel.AutoSize = true;
            this.InitLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.InitLabel.Location = new System.Drawing.Point(0, 20);
            this.InitLabel.Margin = new System.Windows.Forms.Padding(10, 0, 3, 0);
            this.InitLabel.Name = "InitLabel";
            this.InitLabel.Size = new System.Drawing.Size(149, 12);
            this.InitLabel.TabIndex = 1;
            this.InitLabel.Text = "  正在初始化PCDMIS。。。";
            // 
            // InitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(267, 84);
            this.ControlBox = false;
            this.Controls.Add(this.InitLabel);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "InitForm";
            this.Padding = new System.Windows.Forms.Padding(0, 20, 0, 0);
            this.Text = "AutoMeasure";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InitForm_FormClosed);
            this.Load += new System.EventHandler(this.InitForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label InitLabel;
    }
}