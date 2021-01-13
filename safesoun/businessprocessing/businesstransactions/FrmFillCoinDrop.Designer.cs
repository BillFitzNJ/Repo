namespace BusinessTransactions
{
    partial class FrmFillCoinDrop
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFillCoinDrop));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.dataGridViewCoinDrops = new System.Windows.Forms.DataGridView();
            this.ColumnFilled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStorecode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStoreName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDropTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelFilledTotal = new System.Windows.Forms.Label();
            this.buttonSetAll = new System.Windows.Forms.Button();
            this.buttonDeselectAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCoinDrops)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(274, 24);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCoinDrops
            // 
            this.dataGridViewCoinDrops.AllowUserToAddRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCoinDrops.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCoinDrops.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCoinDrops.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnFilled,
            this.ColumnStorecode,
            this.ColumnStoreName,
            this.ColumnDropTotal});
            this.dataGridViewCoinDrops.EnableHeadersVisualStyles = false;
            this.dataGridViewCoinDrops.Location = new System.Drawing.Point(32, 179);
            this.dataGridViewCoinDrops.Name = "dataGridViewCoinDrops";
            this.dataGridViewCoinDrops.RowHeadersVisible = false;
            this.dataGridViewCoinDrops.Size = new System.Drawing.Size(479, 292);
            this.dataGridViewCoinDrops.TabIndex = 1;
            // 
            // ColumnFilled
            // 
            this.ColumnFilled.DataPropertyName = "filled";
            this.ColumnFilled.HeaderText = "Filled";
            this.ColumnFilled.Name = "ColumnFilled";
            this.ColumnFilled.Width = 50;
            // 
            // ColumnStorecode
            // 
            this.ColumnStorecode.DataPropertyName = "store";
            this.ColumnStorecode.HeaderText = "Store";
            this.ColumnStorecode.Name = "ColumnStorecode";
            // 
            // ColumnStoreName
            // 
            this.ColumnStoreName.DataPropertyName = "store_name";
            this.ColumnStoreName.HeaderText = "Store Name";
            this.ColumnStoreName.Name = "ColumnStoreName";
            this.ColumnStoreName.Width = 225;
            // 
            // ColumnDropTotal
            // 
            this.ColumnDropTotal.DataPropertyName = "droptotal";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.ColumnDropTotal.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColumnDropTotal.HeaderText = "Drop Total";
            this.ColumnDropTotal.Name = "ColumnDropTotal";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(70, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Double Click To Select/Deselect Filled Store";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(388, 24);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // labelFilledTotal
            // 
            this.labelFilledTotal.AutoSize = true;
            this.labelFilledTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFilledTotal.Location = new System.Drawing.Point(61, 489);
            this.labelFilledTotal.Name = "labelFilledTotal";
            this.labelFilledTotal.Size = new System.Drawing.Size(262, 13);
            this.labelFilledTotal.TabIndex = 4;
            this.labelFilledTotal.Text = "Double Click To Select/Deselect Filled Store";
            // 
            // buttonSetAll
            // 
            this.buttonSetAll.Location = new System.Drawing.Point(32, 54);
            this.buttonSetAll.Name = "buttonSetAll";
            this.buttonSetAll.Size = new System.Drawing.Size(226, 23);
            this.buttonSetAll.TabIndex = 5;
            this.buttonSetAll.Text = "Click to select all";
            this.buttonSetAll.UseVisualStyleBackColor = true;
            // 
            // buttonDeselectAll
            // 
            this.buttonDeselectAll.Location = new System.Drawing.Point(32, 93);
            this.buttonDeselectAll.Name = "buttonDeselectAll";
            this.buttonDeselectAll.Size = new System.Drawing.Size(226, 23);
            this.buttonDeselectAll.TabIndex = 6;
            this.buttonDeselectAll.Text = "Click to de-select all";
            this.buttonDeselectAll.UseVisualStyleBackColor = true;
            // 
            // FrmFillCoinDrop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 544);
            this.Controls.Add(this.buttonDeselectAll);
            this.Controls.Add(this.buttonSetAll);
            this.Controls.Add(this.labelFilledTotal);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewCoinDrops);
            this.Controls.Add(this.buttonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmFillCoinDrop";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fill Coin Drop";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCoinDrops)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.DataGridView dataGridViewCoinDrops;
        public System.Windows.Forms.Button buttonSave;
        public System.Windows.Forms.Label labelFilledTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnFilled;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStorecode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStoreName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDropTotal;
        public System.Windows.Forms.Button buttonSetAll;
        public System.Windows.Forms.Button buttonDeselectAll;
    }
}