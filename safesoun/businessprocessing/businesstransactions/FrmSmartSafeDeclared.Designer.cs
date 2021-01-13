namespace BusinessTransactions
{
    partial class FrmSmartSafeDeclared
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSmartSafeDeclared));
            this.dataGridViewPriorDeclared = new System.Windows.Forms.DataGridView();
            this.buttonSelectSmartSafe = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBoxSaidtocontain = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelSmartSafeInformation = new System.Windows.Forms.Label();
            this.ColumnPostingDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSaidtocontain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPriorDeclared)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPriorDeclared
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPriorDeclared.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewPriorDeclared.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPriorDeclared.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnPostingDate,
            this.ColumnSaidtocontain});
            this.dataGridViewPriorDeclared.EnableHeadersVisualStyles = false;
            this.dataGridViewPriorDeclared.Location = new System.Drawing.Point(63, 73);
            this.dataGridViewPriorDeclared.Name = "dataGridViewPriorDeclared";
            this.dataGridViewPriorDeclared.ReadOnly = true;
            this.dataGridViewPriorDeclared.RowHeadersVisible = false;
            this.dataGridViewPriorDeclared.Size = new System.Drawing.Size(256, 150);
            this.dataGridViewPriorDeclared.TabIndex = 0;
            // 
            // buttonSelectSmartSafe
            // 
            this.buttonSelectSmartSafe.Location = new System.Drawing.Point(63, 23);
            this.buttonSelectSmartSafe.Name = "buttonSelectSmartSafe";
            this.buttonSelectSmartSafe.Size = new System.Drawing.Size(137, 23);
            this.buttonSelectSmartSafe.TabIndex = 1;
            this.buttonSelectSmartSafe.Text = "Select Smart Safe";
            this.buttonSelectSmartSafe.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(148, 285);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // textBoxSaidtocontain
            // 
            this.textBoxSaidtocontain.Location = new System.Drawing.Point(183, 327);
            this.textBoxSaidtocontain.Name = "textBoxSaidtocontain";
            this.textBoxSaidtocontain.Size = new System.Drawing.Size(132, 20);
            this.textBoxSaidtocontain.TabIndex = 3;
            this.textBoxSaidtocontain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Current amount declared";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(244, 285);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(244, 23);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 6;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // labelSmartSafeInformation
            // 
            this.labelSmartSafeInformation.AutoSize = true;
            this.labelSmartSafeInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSmartSafeInformation.Location = new System.Drawing.Point(60, 240);
            this.labelSmartSafeInformation.Name = "labelSmartSafeInformation";
            this.labelSmartSafeInformation.Size = new System.Drawing.Size(100, 13);
            this.labelSmartSafeInformation.TabIndex = 7;
            this.labelSmartSafeInformation.Text = "Safe Information";
            // 
            // ColumnPostingDate
            // 
            this.ColumnPostingDate.DataPropertyName = "postingdate";
            dataGridViewCellStyle2.Format = "d";
            dataGridViewCellStyle2.NullValue = null;
            this.ColumnPostingDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnPostingDate.HeaderText = "Posting Date";
            this.ColumnPostingDate.Name = "ColumnPostingDate";
            this.ColumnPostingDate.ReadOnly = true;
            // 
            // ColumnSaidtocontain
            // 
            this.ColumnSaidtocontain.DataPropertyName = "saidtocontain";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            this.ColumnSaidtocontain.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnSaidtocontain.HeaderText = "Declared";
            this.ColumnSaidtocontain.Name = "ColumnSaidtocontain";
            this.ColumnSaidtocontain.ReadOnly = true;
            this.ColumnSaidtocontain.Width = 150;
            // 
            // FrmSmartSafeDeclared
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 410);
            this.Controls.Add(this.labelSmartSafeInformation);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSaidtocontain);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonSelectSmartSafe);
            this.Controls.Add(this.dataGridViewPriorDeclared);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSmartSafeDeclared";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SmartSafe Declared";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPriorDeclared)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView dataGridViewPriorDeclared;
        public System.Windows.Forms.Button buttonSelectSmartSafe;
        public System.Windows.Forms.Button buttonSave;
        public System.Windows.Forms.TextBox textBoxSaidtocontain;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.Label labelSmartSafeInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPostingDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSaidtocontain;
    }
}