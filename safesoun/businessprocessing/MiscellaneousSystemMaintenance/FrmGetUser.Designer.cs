namespace MiscellaneousSystemMaintenance
{
   partial class FrmGetUser
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
        this.dataGridViewUserData = new System.Windows.Forms.DataGridView();
        this.ColumnCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.ColumnDescrip = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.ColumnUserrole = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.buttonCancel = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUserData)).BeginInit();
        this.SuspendLayout();
        // 
        // dataGridViewUserData
        // 
        this.dataGridViewUserData.AllowUserToOrderColumns = true;
        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.dataGridViewUserData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        this.dataGridViewUserData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dataGridViewUserData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCode,
            this.ColumnDescrip,
            this.ColumnUserrole});
        this.dataGridViewUserData.EnableHeadersVisualStyles = false;
        this.dataGridViewUserData.Location = new System.Drawing.Point(24, 72);
        this.dataGridViewUserData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.dataGridViewUserData.Name = "dataGridViewUserData";
        this.dataGridViewUserData.ReadOnly = true;
        this.dataGridViewUserData.RowHeadersVisible = false;
        this.dataGridViewUserData.RowTemplate.Height = 24;
        this.dataGridViewUserData.Size = new System.Drawing.Size(360, 195);
        this.dataGridViewUserData.TabIndex = 34;
        this.dataGridViewUserData.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewUserData_CellContentDoubleClick);
        this.dataGridViewUserData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewUserData_KeyDown);
        // 
        // ColumnCode
        // 
        this.ColumnCode.DataPropertyName = "userid";
        this.ColumnCode.HeaderText = "User ID";
        this.ColumnCode.Name = "ColumnCode";
        this.ColumnCode.ReadOnly = true;
        this.ColumnCode.Width = 75;
        // 
        // ColumnDescrip
        // 
        this.ColumnDescrip.DataPropertyName = "username";
        this.ColumnDescrip.FillWeight = 250F;
        this.ColumnDescrip.HeaderText = "User Name";
        this.ColumnDescrip.Name = "ColumnDescrip";
        this.ColumnDescrip.ReadOnly = true;
        this.ColumnDescrip.Width = 300;
        // 
        // ColumnUserrole
        // 
        this.ColumnUserrole.DataPropertyName = "userrole";
        this.ColumnUserrole.HeaderText = "User Role";
        this.ColumnUserrole.Name = "ColumnUserrole";
        this.ColumnUserrole.ReadOnly = true;
        // 
        // buttonCancel
        // 
        this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.buttonCancel.Location = new System.Drawing.Point(306, 13);
        this.buttonCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.buttonCancel.Name = "buttonCancel";
        this.buttonCancel.Size = new System.Drawing.Size(56, 26);
        this.buttonCancel.TabIndex = 35;
        this.buttonCancel.Text = "Cancel";
        this.buttonCancel.UseVisualStyleBackColor = true;
        this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
        // 
        // FrmGetUser
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.CancelButton = this.buttonCancel;
        this.ClientSize = new System.Drawing.Size(414, 322);
        this.Controls.Add(this.buttonCancel);
        this.Controls.Add(this.dataGridViewUserData);
        this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
        this.Name = "FrmGetUser";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Select User";
        ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUserData)).EndInit();
        this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button buttonCancel;
      private System.Windows.Forms.DataGridView dataGridViewUserData;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCode;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDescrip;
      private System.Windows.Forms.DataGridViewTextBoxColumn ColumnUserrole;

   }
}