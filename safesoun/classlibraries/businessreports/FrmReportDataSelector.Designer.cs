namespace BusinessReports
{
    partial class FrmReportDataSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmReportDataSelector));
            this.buttonDriver = new System.Windows.Forms.Button();
            this.buttonCompany = new System.Windows.Forms.Button();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelDriver = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.buttonStore = new System.Windows.Forms.Button();
            this.labelStore = new System.Windows.Forms.Label();
            this.buttonDates = new System.Windows.Forms.Button();
            this.labelDates = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonDriver
            // 
            this.buttonDriver.Location = new System.Drawing.Point(12, 50);
            this.buttonDriver.Name = "buttonDriver";
            this.buttonDriver.Size = new System.Drawing.Size(75, 23);
            this.buttonDriver.TabIndex = 0;
            this.buttonDriver.Text = "Driver";
            this.buttonDriver.UseVisualStyleBackColor = true;
            this.buttonDriver.Click += new System.EventHandler(this.buttonDriver_Click);
            // 
            // buttonCompany
            // 
            this.buttonCompany.Location = new System.Drawing.Point(12, 97);
            this.buttonCompany.Name = "buttonCompany";
            this.buttonCompany.Size = new System.Drawing.Size(75, 23);
            this.buttonCompany.TabIndex = 1;
            this.buttonCompany.Text = "Company";
            this.buttonCompany.UseVisualStyleBackColor = true;
            this.buttonCompany.Click += new System.EventHandler(this.buttonCompany_Click);
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(292, 32);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 2;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(394, 32);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // labelDriver
            // 
            this.labelDriver.AutoSize = true;
            this.labelDriver.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDriver.Location = new System.Drawing.Point(131, 55);
            this.labelDriver.Name = "labelDriver";
            this.labelDriver.Size = new System.Drawing.Size(21, 13);
            this.labelDriver.TabIndex = 4;
            this.labelDriver.Text = "All";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCompany.Location = new System.Drawing.Point(131, 102);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(21, 13);
            this.labelCompany.TabIndex = 5;
            this.labelCompany.Text = "All";
            // 
            // buttonStore
            // 
            this.buttonStore.Location = new System.Drawing.Point(12, 141);
            this.buttonStore.Name = "buttonStore";
            this.buttonStore.Size = new System.Drawing.Size(75, 23);
            this.buttonStore.TabIndex = 6;
            this.buttonStore.Text = "Store";
            this.buttonStore.UseVisualStyleBackColor = true;
            this.buttonStore.Click += new System.EventHandler(this.buttonStore_Click);
            // 
            // labelStore
            // 
            this.labelStore.AutoSize = true;
            this.labelStore.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStore.Location = new System.Drawing.Point(131, 151);
            this.labelStore.Name = "labelStore";
            this.labelStore.Size = new System.Drawing.Size(21, 13);
            this.labelStore.TabIndex = 7;
            this.labelStore.Text = "All";
            // 
            // buttonDates
            // 
            this.buttonDates.Location = new System.Drawing.Point(12, 188);
            this.buttonDates.Name = "buttonDates";
            this.buttonDates.Size = new System.Drawing.Size(75, 23);
            this.buttonDates.TabIndex = 8;
            this.buttonDates.Text = "Dates";
            this.buttonDates.UseVisualStyleBackColor = true;
            this.buttonDates.Click += new System.EventHandler(this.buttonDates_Click);
            // 
            // labelDates
            // 
            this.labelDates.AutoSize = true;
            this.labelDates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDates.Location = new System.Drawing.Point(133, 192);
            this.labelDates.Name = "labelDates";
            this.labelDates.Size = new System.Drawing.Size(21, 13);
            this.labelDates.TabIndex = 9;
            this.labelDates.Text = "All";
            // 
            // FrmReportDataSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 261);
            this.Controls.Add(this.labelDates);
            this.Controls.Add(this.buttonDates);
            this.Controls.Add(this.labelStore);
            this.Controls.Add(this.buttonStore);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.labelDriver);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.buttonCompany);
            this.Controls.Add(this.buttonDriver);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmReportDataSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Data Selector";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button buttonDriver;
        public System.Windows.Forms.Button buttonCompany;
        public System.Windows.Forms.Button buttonProceed;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Label labelDriver;
        public System.Windows.Forms.Label labelCompany;
        public System.Windows.Forms.Button buttonStore;
        public System.Windows.Forms.Button buttonDates;
        public System.Windows.Forms.Label labelDates;
        public System.Windows.Forms.Label labelStore;
    }
}