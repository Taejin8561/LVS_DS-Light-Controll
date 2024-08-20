namespace Light
{
    partial class fMonitor
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
            this.label2 = new System.Windows.Forms.Label();
            this.rtxtTX = new System.Windows.Forms.RichTextBox();
            this.rtxtRX = new System.Windows.Forms.RichTextBox();
            this.btnClearTX = new System.Windows.Forms.Button();
            this.btnClearRX = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "TX";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 19);
            this.label2.TabIndex = 14;
            this.label2.Text = "RX";
            // 
            // rtxtTX
            // 
            this.rtxtTX.BackColor = System.Drawing.SystemColors.Window;
            this.rtxtTX.Location = new System.Drawing.Point(12, 31);
            this.rtxtTX.Name = "rtxtTX";
            this.rtxtTX.ReadOnly = true;
            this.rtxtTX.Size = new System.Drawing.Size(627, 123);
            this.rtxtTX.TabIndex = 15;
            this.rtxtTX.Text = "";
            // 
            // rtxtRX
            // 
            this.rtxtRX.BackColor = System.Drawing.SystemColors.Window;
            this.rtxtRX.Location = new System.Drawing.Point(12, 207);
            this.rtxtRX.Name = "rtxtRX";
            this.rtxtRX.ReadOnly = true;
            this.rtxtRX.Size = new System.Drawing.Size(627, 123);
            this.rtxtRX.TabIndex = 16;
            this.rtxtRX.Text = "";
            // 
            // btnClearTX
            // 
            this.btnClearTX.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnClearTX.Location = new System.Drawing.Point(588, 160);
            this.btnClearTX.Name = "btnClearTX";
            this.btnClearTX.Size = new System.Drawing.Size(51, 23);
            this.btnClearTX.TabIndex = 124;
            this.btnClearTX.Text = "CLR";
            this.btnClearTX.UseVisualStyleBackColor = true;
            this.btnClearTX.Click += new System.EventHandler(this.btnClearTX_Click);
            // 
            // btnClearRX
            // 
            this.btnClearRX.Font = new System.Drawing.Font("Consolas", 9F);
            this.btnClearRX.Location = new System.Drawing.Point(588, 336);
            this.btnClearRX.Name = "btnClearRX";
            this.btnClearRX.Size = new System.Drawing.Size(51, 23);
            this.btnClearRX.TabIndex = 125;
            this.btnClearRX.Text = "CLR";
            this.btnClearRX.UseVisualStyleBackColor = true;
            this.btnClearRX.Click += new System.EventHandler(this.btnClearRX_Click);
            // 
            // fMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 369);
            this.Controls.Add(this.btnClearRX);
            this.Controls.Add(this.btnClearTX);
            this.Controls.Add(this.rtxtRX);
            this.Controls.Add(this.rtxtTX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fMonitor";
            this.ShowInTaskbar = false;
            this.Text = "fMonitor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.fMonitor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnClearTX;
        private System.Windows.Forms.Button btnClearRX;
        public System.Windows.Forms.RichTextBox rtxtTX;
        public System.Windows.Forms.RichTextBox rtxtRX;
    }
}