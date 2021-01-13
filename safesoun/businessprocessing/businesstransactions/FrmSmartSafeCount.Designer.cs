namespace BusinessTransactions
{
    partial class FrmSmartSafeCount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSmartSafeCount));
            this.panelCounts = new System.Windows.Forms.Panel();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSelectSmartSafe = new System.Windows.Forms.Button();
            this.labelSafeSerialNumber = new System.Windows.Forms.Label();
            this.textBoxDenominations = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // panelCounts
            // 
            this.panelCounts.Location = new System.Drawing.Point(16, 198);
            this.panelCounts.Name = "panelCounts";
            this.panelCounts.Size = new System.Drawing.Size(611, 204);
            this.panelCounts.TabIndex = 13;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(555, 10);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 15;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // buttonSelectSmartSafe
            // 
            this.buttonSelectSmartSafe.Location = new System.Drawing.Point(390, 10);
            this.buttonSelectSmartSafe.Name = "buttonSelectSmartSafe";
            this.buttonSelectSmartSafe.Size = new System.Drawing.Size(137, 23);
            this.buttonSelectSmartSafe.TabIndex = 14;
            this.buttonSelectSmartSafe.Text = "Select Smart Safe";
            this.buttonSelectSmartSafe.UseVisualStyleBackColor = true;
            // 
            // labelSafeSerialNumber
            // 
            this.labelSafeSerialNumber.AutoSize = true;
            this.labelSafeSerialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSafeSerialNumber.Location = new System.Drawing.Point(77, 68);
            this.labelSafeSerialNumber.Name = "labelSafeSerialNumber";
            this.labelSafeSerialNumber.Size = new System.Drawing.Size(0, 13);
            this.labelSafeSerialNumber.TabIndex = 16;
            // 
            // textBoxDenominations
            // 
            this.textBoxDenominations.Location = new System.Drawing.Point(222, 79);
            this.textBoxDenominations.Multiline = true;
            this.textBoxDenominations.Name = "textBoxDenominations";
            this.textBoxDenominations.Size = new System.Drawing.Size(160, 110);
            this.textBoxDenominations.TabIndex = 17;
            // 
            // FrmSmartSafeCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 420);
            this.Controls.Add(this.textBoxDenominations);
            this.Controls.Add(this.labelSafeSerialNumber);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSelectSmartSafe);
            this.Controls.Add(this.panelCounts);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSmartSafeCount";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Safe Counts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel panelCounts;
        public System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.Button buttonSelectSmartSafe;
        public System.Windows.Forms.Label labelSafeSerialNumber;
        public System.Windows.Forms.TextBox textBoxDenominations;
    }
}