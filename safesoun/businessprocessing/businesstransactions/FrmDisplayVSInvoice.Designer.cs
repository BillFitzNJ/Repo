namespace BusinessTransactions
{
    partial class FrmDisplayVSInvoice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDisplayVSInvoice));
            this.labelStorename = new System.Windows.Forms.Label();
            this.labelCompanyname = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMonth = new System.Windows.Forms.TextBox();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.buttonSelectstore = new System.Windows.Forms.Button();
            this.buttonCompany = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxInvnumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonPrint = new System.Windows.Forms.RadioButton();
            this.radioButtonView = new System.Windows.Forms.RadioButton();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelStorename
            // 
            this.labelStorename.AutoSize = true;
            this.labelStorename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStorename.Location = new System.Drawing.Point(220, 196);
            this.labelStorename.Name = "labelStorename";
            this.labelStorename.Size = new System.Drawing.Size(59, 13);
            this.labelStorename.TabIndex = 19;
            this.labelStorename.Text = "All stores";
            // 
            // labelCompanyname
            // 
            this.labelCompanyname.AutoSize = true;
            this.labelCompanyname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCompanyname.Location = new System.Drawing.Point(219, 158);
            this.labelCompanyname.Name = "labelCompanyname";
            this.labelCompanyname.Size = new System.Drawing.Size(85, 13);
            this.labelCompanyname.TabIndex = 18;
            this.labelCompanyname.Text = "All companies";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(68, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Year";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(68, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Month";
            // 
            // textBoxMonth
            // 
            this.textBoxMonth.Location = new System.Drawing.Point(118, 78);
            this.textBoxMonth.Name = "textBoxMonth";
            this.textBoxMonth.Size = new System.Drawing.Size(31, 20);
            this.textBoxMonth.TabIndex = 15;
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(118, 104);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(57, 20);
            this.textBoxYear.TabIndex = 14;
            // 
            // buttonSelectstore
            // 
            this.buttonSelectstore.Location = new System.Drawing.Point(117, 191);
            this.buttonSelectstore.Name = "buttonSelectstore";
            this.buttonSelectstore.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectstore.TabIndex = 13;
            this.buttonSelectstore.Text = "Store";
            this.buttonSelectstore.UseVisualStyleBackColor = true;
            this.buttonSelectstore.Click += new System.EventHandler(this.buttonSelectstore_Click);
            // 
            // buttonCompany
            // 
            this.buttonCompany.Location = new System.Drawing.Point(116, 153);
            this.buttonCompany.Name = "buttonCompany";
            this.buttonCompany.Size = new System.Drawing.Size(75, 23);
            this.buttonCompany.TabIndex = 12;
            this.buttonCompany.Text = "Company";
            this.buttonCompany.UseVisualStyleBackColor = true;
            this.buttonCompany.Click += new System.EventHandler(this.buttonCompany_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(352, 47);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 11;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxInvnumber
            // 
            this.textBoxInvnumber.Location = new System.Drawing.Point(116, 51);
            this.textBoxInvnumber.Name = "textBoxInvnumber";
            this.textBoxInvnumber.Size = new System.Drawing.Size(113, 20);
            this.textBoxInvnumber.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Invoice Number";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonPrint);
            this.groupBox1.Controls.Add(this.radioButtonView);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(117, 227);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(189, 50);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Output Method";
            // 
            // radioButtonPrint
            // 
            this.radioButtonPrint.AutoSize = true;
            this.radioButtonPrint.Location = new System.Drawing.Point(14, 31);
            this.radioButtonPrint.Name = "radioButtonPrint";
            this.radioButtonPrint.Size = new System.Drawing.Size(46, 17);
            this.radioButtonPrint.TabIndex = 1;
            this.radioButtonPrint.TabStop = true;
            this.radioButtonPrint.Text = "Print";
            this.radioButtonPrint.UseVisualStyleBackColor = true;
            // 
            // radioButtonView
            // 
            this.radioButtonView.AutoSize = true;
            this.radioButtonView.Location = new System.Drawing.Point(15, 14);
            this.radioButtonView.Name = "radioButtonView";
            this.radioButtonView.Size = new System.Drawing.Size(48, 17);
            this.radioButtonView.TabIndex = 0;
            this.radioButtonView.TabStop = true;
            this.radioButtonView.Text = "View";
            this.radioButtonView.UseVisualStyleBackColor = true;
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(271, 46);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(72, 23);
            this.buttonProceed.TabIndex = 23;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // FrmDisplayVSInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 318);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxInvnumber);
            this.Controls.Add(this.labelStorename);
            this.Controls.Add(this.labelCompanyname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMonth);
            this.Controls.Add(this.textBoxYear);
            this.Controls.Add(this.buttonSelectstore);
            this.Controls.Add(this.buttonCompany);
            this.Controls.Add(this.buttonClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmDisplayVSInvoice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Display Invoice";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelStorename;
        private System.Windows.Forms.Label labelCompanyname;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMonth;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.Button buttonSelectstore;
        private System.Windows.Forms.Button buttonCompany;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxInvnumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonPrint;
        private System.Windows.Forms.RadioButton radioButtonView;
        private System.Windows.Forms.Button buttonProceed;
    }
}