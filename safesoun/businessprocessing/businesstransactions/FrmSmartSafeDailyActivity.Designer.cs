namespace BusinessTransactions
{
    partial class FrmSmartSafeDailyActivity
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSmartSafeDailyActivity));
            this.textBoxDailyAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewDailyActivity = new System.Windows.Forms.DataGridView();
            this.ColumnSaidToContain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPostingDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelStoreName = new System.Windows.Forms.Label();
            this.dateTimePickerPostingDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDailyActivity)).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxDailyAmount
            // 
            this.textBoxDailyAmount.Location = new System.Drawing.Point(106, 70);
            this.textBoxDailyAmount.Name = "textBoxDailyAmount";
            this.textBoxDailyAmount.Size = new System.Drawing.Size(100, 20);
            this.textBoxDailyAmount.TabIndex = 0;
            this.textBoxDailyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(17, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Daily Amount";
            // 
            // dataGridViewDailyActivity
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDailyActivity.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewDailyActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDailyActivity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSaidToContain,
            this.ColumnPostingDate});
            this.dataGridViewDailyActivity.EnableHeadersVisualStyles = false;
            this.dataGridViewDailyActivity.Location = new System.Drawing.Point(82, 136);
            this.dataGridViewDailyActivity.Name = "dataGridViewDailyActivity";
            this.dataGridViewDailyActivity.RowHeadersVisible = false;
            this.dataGridViewDailyActivity.Size = new System.Drawing.Size(303, 150);
            this.dataGridViewDailyActivity.TabIndex = 2;
            // 
            // ColumnSaidToContain
            // 
            this.ColumnSaidToContain.DataPropertyName = "saidtocontain";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.ColumnSaidToContain.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnSaidToContain.HeaderText = "Daily Amount";
            this.ColumnSaidToContain.Name = "ColumnSaidToContain";
            this.ColumnSaidToContain.ReadOnly = true;
            this.ColumnSaidToContain.Width = 150;
            // 
            // ColumnPostingDate
            // 
            this.ColumnPostingDate.DataPropertyName = "postingdate";
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            this.ColumnPostingDate.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColumnPostingDate.HeaderText = "Date";
            this.ColumnPostingDate.Name = "ColumnPostingDate";
            this.ColumnPostingDate.ReadOnly = true;
            this.ColumnPostingDate.Width = 150;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(254, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(351, 12);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // labelStoreName
            // 
            this.labelStoreName.AutoSize = true;
            this.labelStoreName.Location = new System.Drawing.Point(53, 23);
            this.labelStoreName.Name = "labelStoreName";
            this.labelStoreName.Size = new System.Drawing.Size(63, 13);
            this.labelStoreName.TabIndex = 5;
            this.labelStoreName.Text = "Store Name";
            // 
            // dateTimePickerPostingDate
            // 
            this.dateTimePickerPostingDate.Location = new System.Drawing.Point(221, 70);
            this.dateTimePickerPostingDate.Name = "dateTimePickerPostingDate";
            this.dateTimePickerPostingDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerPostingDate.TabIndex = 6;
            // 
            // FrmSmartSafeDailyActivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 359);
            this.Controls.Add(this.dateTimePickerPostingDate);
            this.Controls.Add(this.labelStoreName);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dataGridViewDailyActivity);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDailyAmount);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSmartSafeDailyActivity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smart Safe Daily Activity";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDailyActivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox textBoxDailyAmount;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGridView dataGridViewDailyActivity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSaidToContain;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPostingDate;
        public System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.Button buttonSave;
        public System.Windows.Forms.Label labelStoreName;
        public System.Windows.Forms.DateTimePicker dateTimePickerPostingDate;
    }
}