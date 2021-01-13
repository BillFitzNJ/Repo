namespace BusinessTransactions
{
    partial class FrmSmartSafeOutgoingFED
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
            this.panelVerified = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelVerified
            // 
            this.panelVerified.Location = new System.Drawing.Point(25, 45);
            this.panelVerified.Name = "panelVerified";
            this.panelVerified.Size = new System.Drawing.Size(611, 204);
            this.panelVerified.TabIndex = 13;
            // 
            // FrmSmartSafeOutgoingFED
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 295);
            this.Controls.Add(this.panelVerified);
            this.Name = "FrmSmartSafeOutgoingFED";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Safe OutgoingFED";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelVerified;
    }
}