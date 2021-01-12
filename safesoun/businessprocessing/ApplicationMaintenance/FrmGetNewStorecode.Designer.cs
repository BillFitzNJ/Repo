namespace ApplicationMaintenance
{
    partial class FrmGetNewStorecode
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetNewStorecode));
            this.dataGridViewStoreData = new System.Windows.Forms.DataGridView();
            this.ColumnStoreCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStoreName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStoreAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxStorecode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.labelCompanyData = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStoreData)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewStoreData
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewStoreData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewStoreData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStoreData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnStoreCode,
            this.ColumnStoreName,
            this.ColumnStoreAddress});
            this.dataGridViewStoreData.EnableHeadersVisualStyles = false;
            this.dataGridViewStoreData.Location = new System.Drawing.Point(27, 82);
            this.dataGridViewStoreData.Name = "dataGridViewStoreData";
            this.dataGridViewStoreData.RowHeadersVisible = false;
            this.dataGridViewStoreData.Size = new System.Drawing.Size(755, 150);
            this.dataGridViewStoreData.TabIndex = 0;
            // 
            // ColumnStoreCode
            // 
            this.ColumnStoreCode.DataPropertyName = "storecode";
            this.ColumnStoreCode.HeaderText = "Code";
            this.ColumnStoreCode.Name = "ColumnStoreCode";
            this.ColumnStoreCode.Width = 150;
            // 
            // ColumnStoreName
            // 
            this.ColumnStoreName.DataPropertyName = "store_name";
            this.ColumnStoreName.HeaderText = "Store Name";
            this.ColumnStoreName.Name = "ColumnStoreName";
            this.ColumnStoreName.Width = 300;
            // 
            // ColumnStoreAddress
            // 
            this.ColumnStoreAddress.DataPropertyName = "f_address";
            this.ColumnStoreAddress.HeaderText = "Address";
            this.ColumnStoreAddress.Name = "ColumnStoreAddress";
            this.ColumnStoreAddress.Width = 300;
            // 
            // textBoxStorecode
            // 
            this.textBoxStorecode.Location = new System.Drawing.Point(340, 265);
            this.textBoxStorecode.MaxLength = 6;
            this.textBoxStorecode.Name = "textBoxStorecode";
            this.textBoxStorecode.Size = new System.Drawing.Size(50, 20);
            this.textBoxStorecode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 268);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(286, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter the last 6 characters of the new store code";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(484, 24);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(589, 24);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 4;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            // 
            // labelCompanyData
            // 
            this.labelCompanyData.AutoSize = true;
            this.labelCompanyData.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCompanyData.Location = new System.Drawing.Point(50, 34);
            this.labelCompanyData.Name = "labelCompanyData";
            this.labelCompanyData.Size = new System.Drawing.Size(89, 13);
            this.labelCompanyData.TabIndex = 5;
            this.labelCompanyData.Text = "Company Data";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(29, 291);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(295, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "The computer will add the company code and the -";
            // 
            // FrmGetNewStorecode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 323);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelCompanyData);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxStorecode);
            this.Controls.Add(this.dataGridViewStoreData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGetNewStorecode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter New Store Code";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStoreData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBoxStorecode;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Button buttonProceed;
        public System.Windows.Forms.Label labelCompanyData;
        public System.Windows.Forms.DataGridView dataGridViewStoreData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStoreCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStoreName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStoreAddress;
        public System.Windows.Forms.Label label2;
    }
}