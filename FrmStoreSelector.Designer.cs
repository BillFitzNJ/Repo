namespace CommonAppClasses
{
    partial class FrmStoreSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStoreSelector));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxIncludeinactive = new System.Windows.Forms.CheckBox();
            this.dataGridViewStoreSelector = new System.Windows.Forms.DataGridView();
            this.ColumnStorecode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStorename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnFaddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStoreSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(729, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeinactive
            // 
            this.checkBoxIncludeinactive.AutoSize = true;
            this.checkBoxIncludeinactive.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxIncludeinactive.Location = new System.Drawing.Point(24, 12);
            this.checkBoxIncludeinactive.Name = "checkBoxIncludeinactive";
            this.checkBoxIncludeinactive.Size = new System.Drawing.Size(118, 17);
            this.checkBoxIncludeinactive.TabIndex = 5;
            this.checkBoxIncludeinactive.Text = "Include Inactive";
            this.checkBoxIncludeinactive.UseVisualStyleBackColor = true;
            // 
            // dataGridViewStoreSelector
            // 
            this.dataGridViewStoreSelector.AllowUserToAddRows = false;
            this.dataGridViewStoreSelector.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewStoreSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewStoreSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStoreSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnStorecode,
            this.ColumnStorename,
            this.ColumnFaddress});
            this.dataGridViewStoreSelector.EnableHeadersVisualStyles = false;
            this.dataGridViewStoreSelector.Location = new System.Drawing.Point(33, 59);
            this.dataGridViewStoreSelector.Name = "dataGridViewStoreSelector";
            this.dataGridViewStoreSelector.ReadOnly = true;
            this.dataGridViewStoreSelector.RowHeadersVisible = false;
            this.dataGridViewStoreSelector.Size = new System.Drawing.Size(803, 430);
            this.dataGridViewStoreSelector.TabIndex = 0;
            // 
            // ColumnStorecode
            // 
            this.ColumnStorecode.DataPropertyName = "storecode";
            this.ColumnStorecode.HeaderText = "Code";
            this.ColumnStorecode.Name = "ColumnStorecode";
            this.ColumnStorecode.ReadOnly = true;
            // 
            // ColumnStorename
            // 
            this.ColumnStorename.DataPropertyName = "store_name";
            this.ColumnStorename.HeaderText = "Store Name";
            this.ColumnStorename.Name = "ColumnStorename";
            this.ColumnStorename.ReadOnly = true;
            this.ColumnStorename.Width = 350;
            // 
            // ColumnFaddress
            // 
            this.ColumnFaddress.DataPropertyName = "f_address";
            this.ColumnFaddress.HeaderText = "Street Address";
            this.ColumnFaddress.Name = "ColumnFaddress";
            this.ColumnFaddress.ReadOnly = true;
            this.ColumnFaddress.Width = 350;
            // 
            // FrmStoreSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 501);
            this.Controls.Add(this.dataGridViewStoreSelector);
            this.Controls.Add(this.checkBoxIncludeinactive);
            this.Controls.Add(this.buttonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmStoreSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Location";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStoreSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.CheckBox checkBoxIncludeinactive;
        public System.Windows.Forms.DataGridView dataGridViewStoreSelector;
        public System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStorecode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStorename;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFaddress;
    }
}