namespace PCDServer
{
    partial class Form1
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
            this.RunProg_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.select_button = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RunProg_button
            // 
            this.RunProg_button.Location = new System.Drawing.Point(28, 105);
            this.RunProg_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RunProg_button.Name = "RunProg_button";
            this.RunProg_button.Size = new System.Drawing.Size(236, 74);
            this.RunProg_button.TabIndex = 0;
            this.RunProg_button.Text = "运行程序";
            this.RunProg_button.UseVisualStyleBackColor = true;
            this.RunProg_button.Click += new System.EventHandler(this.RunProg_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 58);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // select_button
            // 
            this.select_button.Location = new System.Drawing.Point(115, 11);
            this.select_button.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.select_button.Name = "select_button";
            this.select_button.Size = new System.Drawing.Size(222, 74);
            this.select_button.TabIndex = 3;
            this.select_button.Text = "选择程序";
            this.select_button.UseVisualStyleBackColor = true;
            this.select_button.Click += new System.EventHandler(this.select_button_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(305, 105);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(265, 74);
            this.button1.TabIndex = 4;
            this.button1.Text = "重新初始化";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 230);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.select_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RunProg_button);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RunProg_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button select_button;
        private System.Windows.Forms.Button button1;
    }
}

