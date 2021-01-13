namespace BusinessTransactions
{
    partial class FrmGenerateBills
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGenerateBills));
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonCompany = new System.Windows.Forms.Button();
            this.buttonSelectstore = new System.Windows.Forms.Button();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.textBoxMonth = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelCompanyname = new System.Windows.Forms.Label();
            this.labelStorename = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(570, 22);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(75, 23);
            this.buttonGenerate.TabIndex = 0;
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(665, 22);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonCompany
            // 
            this.buttonCompany.Location = new System.Drawing.Point(261, 139);
            this.buttonCompany.Name = "buttonCompany";
            this.buttonCompany.Size = new System.Drawing.Size(75, 23);
            this.buttonCompany.TabIndex = 2;
            this.buttonCompany.Text = "Company";
            this.buttonCompany.UseVisualStyleBackColor = true;
            this.buttonCompany.Click += new System.EventHandler(this.buttonCompany_Click);
            // 
            // buttonSelectstore
            // 
            this.buttonSelectstore.Location = new System.Drawing.Point(262, 177);
            this.buttonSelectstore.Name = "buttonSelectstore";
            this.buttonSelectstore.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectstore.TabIndex = 3;
            this.buttonSelectstore.Text = "Store";
            this.buttonSelectstore.UseVisualStyleBackColor = true;
            this.buttonSelectstore.Click += new System.EventHandler(this.buttonSelectstore_Click);
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(263, 90);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(57, 20);
            this.textBoxYear.TabIndex = 4;
            // 
            // textBoxMonth
            // 
            this.textBoxMonth.Location = new System.Drawing.Point(263, 64);
            this.textBoxMonth.Name = "textBoxMonth";
            this.textBoxMonth.Size = new System.Drawing.Size(31, 20);
            this.textBoxMonth.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(206, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Month";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(206, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Year";
            // 
            // labelCompanyname
            // 
            this.labelCompanyname.AutoSize = true;
            this.labelCompanyname.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCompanyname.Location = new System.Drawing.Point(380, 144);
            this.labelCompanyname.Name = "labelCompanyname";
            this.labelCompanyname.Size = new System.Drawing.Size(85, 13);
            this.labelCompanyname.TabIndex = 8;
            this.labelCompanyname.Text = "All companies";
            // 
            // labelStorename
            // 
            this.labelStorename.AutoSize = true;
            this.labelStorename.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStorename.Location = new System.Drawing.Point(381, 182);
            this.labelStorename.Name = "labelStorename";
            this.labelStorename.Size = new System.Drawing.Size(59, 13);
            this.labelStorename.TabIndex = 9;
            this.labelStorename.Text = "All stores";
            // 
            // FrmGenerateBills
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 278);
            this.Controls.Add(this.labelStorename);
            this.Controls.Add(this.labelCompanyname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMonth);
            this.Controls.Add(this.textBoxYear);
            this.Controls.Add(this.buttonSelectstore);
            this.Controls.Add(this.buttonCompany);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonGenerate);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGenerateBills";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Bills";
            this.Load += new System.EventHandler(this.FrmGenerateBills_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonCompany;
        private System.Windows.Forms.Button buttonSelectstore;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.TextBox textBoxMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelCompanyname;
        private System.Windows.Forms.Label labelStorename;
    }
}