namespace BusinessReports
{
    partial class FrmPickupListing
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPickupListing));
            this.labelDate = new System.Windows.Forms.Label();
            this.buttonDate = new System.Windows.Forms.Button();
            this.labelDriver = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.buttonDriver = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonMoneyCenter = new System.Windows.Forms.RadioButton();
            this.radioButtonCompany = new System.Windows.Forms.RadioButton();
            this.radioButtonManifest = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDate.Location = new System.Drawing.Point(145, 99);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(37, 13);
            this.labelDate.TabIndex = 19;
            this.labelDate.Text = "None";
            // 
            // buttonDate
            // 
            this.buttonDate.Location = new System.Drawing.Point(31, 95);
            this.buttonDate.Name = "buttonDate";
            this.buttonDate.Size = new System.Drawing.Size(75, 23);
            this.buttonDate.TabIndex = 18;
            this.buttonDate.Text = "Date";
            this.buttonDate.UseVisualStyleBackColor = true;
            this.buttonDate.Click += new System.EventHandler(this.buttonDate_Click);
            // 
            // labelDriver
            // 
            this.labelDriver.AutoSize = true;
            this.labelDriver.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDriver.Location = new System.Drawing.Point(143, 47);
            this.labelDriver.Name = "labelDriver";
            this.labelDriver.Size = new System.Drawing.Size(37, 13);
            this.labelDriver.TabIndex = 14;
            this.labelDriver.Text = "None";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(329, 17);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(227, 17);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 12;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // buttonDriver
            // 
            this.buttonDriver.Location = new System.Drawing.Point(31, 42);
            this.buttonDriver.Name = "buttonDriver";
            this.buttonDriver.Size = new System.Drawing.Size(75, 23);
            this.buttonDriver.TabIndex = 10;
            this.buttonDriver.Text = "Driver";
            this.buttonDriver.UseVisualStyleBackColor = true;
            this.buttonDriver.Click += new System.EventHandler(this.buttonDriver_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonManifest);
            this.groupBox1.Controls.Add(this.radioButtonMoneyCenter);
            this.groupBox1.Controls.Add(this.radioButtonCompany);
            this.groupBox1.Location = new System.Drawing.Point(31, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 85);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sequence";
            // 
            // radioButtonMoneyCenter
            // 
            this.radioButtonMoneyCenter.AutoSize = true;
            this.radioButtonMoneyCenter.Location = new System.Drawing.Point(26, 42);
            this.radioButtonMoneyCenter.Name = "radioButtonMoneyCenter";
            this.radioButtonMoneyCenter.Size = new System.Drawing.Size(91, 17);
            this.radioButtonMoneyCenter.TabIndex = 1;
            this.radioButtonMoneyCenter.TabStop = true;
            this.radioButtonMoneyCenter.Text = "Money Center";
            this.radioButtonMoneyCenter.UseVisualStyleBackColor = true;
            // 
            // radioButtonCompany
            // 
            this.radioButtonCompany.AutoSize = true;
            this.radioButtonCompany.Location = new System.Drawing.Point(26, 21);
            this.radioButtonCompany.Name = "radioButtonCompany";
            this.radioButtonCompany.Size = new System.Drawing.Size(69, 17);
            this.radioButtonCompany.TabIndex = 0;
            this.radioButtonCompany.TabStop = true;
            this.radioButtonCompany.Text = "Company";
            this.radioButtonCompany.UseVisualStyleBackColor = true;
            // 
            // radioButtonManifest
            // 
            this.radioButtonManifest.AutoSize = true;
            this.radioButtonManifest.Location = new System.Drawing.Point(26, 63);
            this.radioButtonManifest.Name = "radioButtonManifest";
            this.radioButtonManifest.Size = new System.Drawing.Size(65, 17);
            this.radioButtonManifest.TabIndex = 2;
            this.radioButtonManifest.TabStop = true;
            this.radioButtonManifest.Text = "Manifest";
            this.radioButtonManifest.UseVisualStyleBackColor = true;
            // 
            // FrmPickupListing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 273);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.buttonDate);
            this.Controls.Add(this.labelDriver);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.buttonDriver);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPickupListing";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pickup Listing";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label labelDate;
        public System.Windows.Forms.Button buttonDate;
        public System.Windows.Forms.Label labelDriver;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Button buttonProceed;
        public System.Windows.Forms.Button buttonDriver;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonMoneyCenter;
        private System.Windows.Forms.RadioButton radioButtonCompany;
        private System.Windows.Forms.RadioButton radioButtonManifest;
    }
}