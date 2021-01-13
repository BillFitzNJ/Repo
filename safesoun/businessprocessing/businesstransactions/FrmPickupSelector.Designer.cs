namespace BusinessTransactions
{
    partial class FrmPickupSelector
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPickupSelector));
            this.dataGridViewPickups = new System.Windows.Forms.DataGridView();
            this.buttonClose = new System.Windows.Forms.Button();
            this.ColumnFilled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStorecode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Columnidcol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPickups)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPickups
            // 
            this.dataGridViewPickups.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewPickups.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewPickups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPickups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnFilled,
            this.ColumnStorecode,
            this.Columnidcol});
            this.dataGridViewPickups.EnableHeadersVisualStyles = false;
            this.dataGridViewPickups.Location = new System.Drawing.Point(12, 89);
            this.dataGridViewPickups.Name = "dataGridViewPickups";
            this.dataGridViewPickups.RowHeadersVisible = false;
            this.dataGridViewPickups.Size = new System.Drawing.Size(255, 292);
            this.dataGridViewPickups.TabIndex = 2;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(149, 22);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ColumnFilled
            // 
            this.ColumnFilled.DataPropertyName = "sealnumber";
            this.ColumnFilled.HeaderText = "Bag Number";
            this.ColumnFilled.Name = "ColumnFilled";
            this.ColumnFilled.Width = 150;
            // 
            // ColumnStorecode
            // 
            this.ColumnStorecode.DataPropertyName = "amount";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.ColumnStorecode.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnStorecode.HeaderText = "Amount";
            this.ColumnStorecode.Name = "ColumnStorecode";
            // 
            // Columnidcol
            // 
            this.Columnidcol.DataPropertyName = "idcol";
            this.Columnidcol.HeaderText = " ";
            this.Columnidcol.Name = "Columnidcol";
            // 
            // FrmPickupSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 416);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.dataGridViewPickups);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmPickupSelector";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pickup Selector";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPickups)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataGridView dataGridViewPickups;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFilled;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStorecode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Columnidcol;
    }
}