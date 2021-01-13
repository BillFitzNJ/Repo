namespace BusinessTransactions
{
    partial class FrmInvoiceRangeSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInvoiceRangeSelector));
            this.buttonProceed = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonCompany = new System.Windows.Forms.Button();
            this.buttonStore = new System.Windows.Forms.Button();
            this.labelCompany = new System.Windows.Forms.Label();
            this.labelStore = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownStartMonth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownStartYear = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownEndYear = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownEndMonth = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(275, 34);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 0;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(163, 34);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonCompany
            // 
            this.buttonCompany.Location = new System.Drawing.Point(113, 118);
            this.buttonCompany.Name = "buttonCompany";
            this.buttonCompany.Size = new System.Drawing.Size(75, 23);
            this.buttonCompany.TabIndex = 2;
            this.buttonCompany.Text = "Company";
            this.buttonCompany.UseVisualStyleBackColor = true;
            this.buttonCompany.Click += new System.EventHandler(this.buttonCompany_Click);
            // 
            // buttonStore
            // 
            this.buttonStore.Location = new System.Drawing.Point(113, 159);
            this.buttonStore.Name = "buttonStore";
            this.buttonStore.Size = new System.Drawing.Size(75, 23);
            this.buttonStore.TabIndex = 3;
            this.buttonStore.Text = "Store";
            this.buttonStore.UseVisualStyleBackColor = true;
            this.buttonStore.Click += new System.EventHandler(this.buttonStore_Click);
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(207, 124);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(51, 13);
            this.labelCompany.TabIndex = 4;
            this.labelCompany.Text = "Company";
            // 
            // labelStore
            // 
            this.labelStore.AutoSize = true;
            this.labelStore.Location = new System.Drawing.Point(207, 170);
            this.labelStore.Name = "labelStore";
            this.labelStore.Size = new System.Drawing.Size(32, 13);
            this.labelStore.TabIndex = 5;
            this.labelStore.Text = "Store";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(92, 219);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Beginning Month/Year";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(92, 251);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Ending Month/Year";
            // 
            // numericUpDownStartMonth
            // 
            this.numericUpDownStartMonth.Location = new System.Drawing.Point(228, 217);
            this.numericUpDownStartMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDownStartMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownStartMonth.Name = "numericUpDownStartMonth";
            this.numericUpDownStartMonth.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownStartMonth.TabIndex = 8;
            this.numericUpDownStartMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numericUpDownStartYear
            // 
            this.numericUpDownStartYear.Location = new System.Drawing.Point(286, 217);
            this.numericUpDownStartYear.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownStartYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownStartYear.Name = "numericUpDownStartYear";
            this.numericUpDownStartYear.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownStartYear.TabIndex = 9;
            this.numericUpDownStartYear.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // numericUpDownEndYear
            // 
            this.numericUpDownEndYear.Location = new System.Drawing.Point(287, 247);
            this.numericUpDownEndYear.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownEndYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownEndYear.Name = "numericUpDownEndYear";
            this.numericUpDownEndYear.Size = new System.Drawing.Size(61, 20);
            this.numericUpDownEndYear.TabIndex = 11;
            this.numericUpDownEndYear.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // numericUpDownEndMonth
            // 
            this.numericUpDownEndMonth.Location = new System.Drawing.Point(229, 247);
            this.numericUpDownEndMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDownEndMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownEndMonth.Name = "numericUpDownEndMonth";
            this.numericUpDownEndMonth.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownEndMonth.TabIndex = 10;
            this.numericUpDownEndMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // FrmInvoiceRangeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 344);
            this.Controls.Add(this.numericUpDownEndYear);
            this.Controls.Add(this.numericUpDownEndMonth);
            this.Controls.Add(this.numericUpDownStartYear);
            this.Controls.Add(this.numericUpDownStartMonth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelStore);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.buttonStore);
            this.Controls.Add(this.buttonCompany);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonProceed);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmInvoiceRangeSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice Range Selector";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStartYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownEndMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button buttonProceed;
        public System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonCompany;
        private System.Windows.Forms.Button buttonStore;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Label labelStore;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown numericUpDownStartMonth;
        public System.Windows.Forms.NumericUpDown numericUpDownStartYear;
        public System.Windows.Forms.NumericUpDown numericUpDownEndYear;
        public System.Windows.Forms.NumericUpDown numericUpDownEndMonth;
    }
}