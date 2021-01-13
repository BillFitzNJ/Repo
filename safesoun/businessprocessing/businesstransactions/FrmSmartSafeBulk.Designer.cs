namespace BusinessTransactions
{
    partial class FrmSmartSafeBulk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSmartSafeBulk));
            this.panelVerified = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelVerified
            // 
            this.panelVerified.Location = new System.Drawing.Point(76, 45);
            this.panelVerified.Name = "panelVerified";
            this.panelVerified.Size = new System.Drawing.Size(627, 232);
            this.panelVerified.TabIndex = 14;
            // 
            // FrmSmartSafeBulk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 338);
            this.Controls.Add(this.panelVerified);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSmartSafeBulk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmSmartSafeBulk";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelVerified;
    }
}