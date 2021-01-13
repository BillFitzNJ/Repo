namespace CommonAppClasses
{
    partial class FrmSelectInvoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelectInvoice));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.textBoxInvoiceNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownYear = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMonth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonCompany = new System.Windows.Forms.Button();
            this.labelCompanyname = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(152, 23);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(252, 23);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 2;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // textBoxInvoiceNumber
            // 
            this.textBoxInvoiceNumber.Location = new System.Drawing.Point(152, 139);
            this.textBoxInvoiceNumber.MaxLength = 13;
            this.textBoxInvoiceNumber.Name = "textBoxInvoiceNumber";
            this.textBoxInvoiceNumber.Size = new System.Drawing.Size(112, 20);
            this.textBoxInvoiceNumber.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Invoice Number";
            // 
            // numericUpDownYear
            // 
            this.numericUpDownYear.Location = new System.Drawing.Point(150, 81);
            this.numericUpDownYear.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownYear.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownYear.Name = "numericUpDownYear";
            this.numericUpDownYear.Size = new System.Drawing.Size(62, 20);
            this.numericUpDownYear.TabIndex = 8;
            this.numericUpDownYear.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // numericUpDownMonth
            // 
            this.numericUpDownMonth.Location = new System.Drawing.Point(151, 54);
            this.numericUpDownMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numericUpDownMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownMonth.Name = "numericUpDownMonth";
            this.numericUpDownMonth.Size = new System.Drawing.Size(44, 20);
            this.numericUpDownMonth.TabIndex = 7;
            this.numericUpDownMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(68, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Month";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(69, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Year";
            // 
            // buttonCompany
            // 
            this.buttonCompany.Location = new System.Drawing.Point(152, 109);
            this.buttonCompany.Name = "buttonCompany";
            this.buttonCompany.Size = new System.Drawing.Size(75, 23);
            this.buttonCompany.TabIndex = 13;
            this.buttonCompany.Text = "Company";
            this.buttonCompany.UseVisualStyleBackColor = true;
            this.buttonCompany.Click += new System.EventHandler(this.buttonCompany_Click);
            // 
            // labelCompanyname
            // 
            this.labelCompanyname.AutoSize = true;
            this.labelCompanyname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCompanyname.Location = new System.Drawing.Point(249, 114);
            this.labelCompanyname.Name = "labelCompanyname";
            this.labelCompanyname.Size = new System.Drawing.Size(33, 13);
            this.labelCompanyname.TabIndex = 14;
            this.labelCompanyname.Text = "Year";
            // 
            // FrmSelectInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 216);
            this.Controls.Add(this.labelCompanyname);
            this.Controls.Add(this.buttonCompany);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDownYear);
            this.Controls.Add(this.numericUpDownMonth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxInvoiceNumber);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.buttonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSelectInvoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice Number";
            this.Load += new System.EventHandler(this.FrmSelectInvoice_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBoxInvoiceNumber;
        public System.Windows.Forms.NumericUpDown numericUpDownYear;
        public System.Windows.Forms.NumericUpDown numericUpDownMonth;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button buttonCompany;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Button buttonProceed;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label labelCompanyname;
    }
}