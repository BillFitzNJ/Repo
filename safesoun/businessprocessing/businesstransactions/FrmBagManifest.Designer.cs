namespace BusinessTransactions
{
  partial class FrmBagManifest
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBagManifest));
            this.buttonNewOneCustManifest = new System.Windows.Forms.Button();
            this.labelBagNumber = new System.Windows.Forms.Label();
            this.dataGridViewManifestDetails = new System.Windows.Forms.DataGridView();
            this.ColumnBagNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxBagNumber = new System.Windows.Forms.TextBox();
            this.buttonNewMixedCustManifest = new System.Windows.Forms.Button();
            this.buttonEditManifest = new System.Windows.Forms.Button();
            this.buttonPrintManifest = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.labelAmount = new System.Windows.Forms.Label();
            this.textBoxBagCount = new System.Windows.Forms.TextBox();
            this.labelBagCount = new System.Windows.Forms.Label();
            this.comboBoxCustomerSelector = new System.Windows.Forms.ComboBox();
            this.labelSelectCustomer = new System.Windows.Forms.Label();
            this.labelMoneyCenter = new System.Windows.Forms.Label();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBoxAmount = new System.Windows.Forms.TextBox();
            this.labelCustomerName = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelMoneyCenterSeal = new System.Windows.Forms.Label();
            this.textBoxMoneyCenterSeal = new System.Windows.Forms.TextBox();
            this.buttonCustomerInquiry = new System.Windows.Forms.Button();
            this.buttonManifestDetails = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewManifestDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonNewOneCustManifest
            // 
            this.buttonNewOneCustManifest.Location = new System.Drawing.Point(23, 14);
            this.buttonNewOneCustManifest.Name = "buttonNewOneCustManifest";
            this.buttonNewOneCustManifest.Size = new System.Drawing.Size(225, 23);
            this.buttonNewOneCustManifest.TabIndex = 0;
            this.buttonNewOneCustManifest.Text = "New Manifest For One Customer";
            this.buttonNewOneCustManifest.UseVisualStyleBackColor = true;
            // 
            // labelBagNumber
            // 
            this.labelBagNumber.AutoSize = true;
            this.labelBagNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBagNumber.Location = new System.Drawing.Point(23, 178);
            this.labelBagNumber.Name = "labelBagNumber";
            this.labelBagNumber.Size = new System.Drawing.Size(76, 13);
            this.labelBagNumber.TabIndex = 1;
            this.labelBagNumber.Text = "Bag Number";
            // 
            // dataGridViewManifestDetails
            // 
            this.dataGridViewManifestDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewManifestDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnBagNumber,
            this.ColumnAmount,
            this.ColumnCustomer});
            this.dataGridViewManifestDetails.Location = new System.Drawing.Point(26, 218);
            this.dataGridViewManifestDetails.Name = "dataGridViewManifestDetails";
            this.dataGridViewManifestDetails.RowHeadersVisible = false;
            this.dataGridViewManifestDetails.Size = new System.Drawing.Size(622, 339);
            this.dataGridViewManifestDetails.TabIndex = 2;
            // 
            // ColumnBagNumber
            // 
            this.ColumnBagNumber.DataPropertyName = "sealnumber";
            this.ColumnBagNumber.HeaderText = "Bag Number";
            this.ColumnBagNumber.Name = "ColumnBagNumber";
            this.ColumnBagNumber.Width = 250;
            // 
            // ColumnAmount
            // 
            this.ColumnAmount.DataPropertyName = "amount";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.ColumnAmount.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColumnAmount.HeaderText = "Amount";
            this.ColumnAmount.Name = "ColumnAmount";
            this.ColumnAmount.Width = 125;
            // 
            // ColumnCustomer
            // 
            this.ColumnCustomer.DataPropertyName = "customer";
            this.ColumnCustomer.HeaderText = "Customer";
            this.ColumnCustomer.Name = "ColumnCustomer";
            this.ColumnCustomer.Width = 250;
            // 
            // textBoxBagNumber
            // 
            this.textBoxBagNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxBagNumber.Location = new System.Drawing.Point(143, 175);
            this.textBoxBagNumber.MaxLength = 25;
            this.textBoxBagNumber.Name = "textBoxBagNumber";
            this.textBoxBagNumber.Size = new System.Drawing.Size(191, 20);
            this.textBoxBagNumber.TabIndex = 1;
            // 
            // buttonNewMixedCustManifest
            // 
            this.buttonNewMixedCustManifest.Location = new System.Drawing.Point(23, 41);
            this.buttonNewMixedCustManifest.Name = "buttonNewMixedCustManifest";
            this.buttonNewMixedCustManifest.Size = new System.Drawing.Size(226, 23);
            this.buttonNewMixedCustManifest.TabIndex = 4;
            this.buttonNewMixedCustManifest.Text = "New Manifest For Mixed Customers";
            this.buttonNewMixedCustManifest.UseVisualStyleBackColor = true;
            // 
            // buttonEditManifest
            // 
            this.buttonEditManifest.Location = new System.Drawing.Point(258, 14);
            this.buttonEditManifest.Name = "buttonEditManifest";
            this.buttonEditManifest.Size = new System.Drawing.Size(152, 23);
            this.buttonEditManifest.TabIndex = 5;
            this.buttonEditManifest.Text = "Edit Manifest ";
            this.buttonEditManifest.UseVisualStyleBackColor = true;
            // 
            // buttonPrintManifest
            // 
            this.buttonPrintManifest.Location = new System.Drawing.Point(258, 41);
            this.buttonPrintManifest.Name = "buttonPrintManifest";
            this.buttonPrintManifest.Size = new System.Drawing.Size(152, 23);
            this.buttonPrintManifest.TabIndex = 6;
            this.buttonPrintManifest.Text = "Print Manifest";
            this.buttonPrintManifest.UseVisualStyleBackColor = true;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(546, 14);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(76, 23);
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            // 
            // labelAmount
            // 
            this.labelAmount.AutoSize = true;
            this.labelAmount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmount.Location = new System.Drawing.Point(378, 177);
            this.labelAmount.Name = "labelAmount";
            this.labelAmount.Size = new System.Drawing.Size(49, 13);
            this.labelAmount.TabIndex = 8;
            this.labelAmount.Text = "Amount";
            // 
            // textBoxBagCount
            // 
            this.textBoxBagCount.Location = new System.Drawing.Point(544, 99);
            this.textBoxBagCount.Name = "textBoxBagCount";
            this.textBoxBagCount.ReadOnly = true;
            this.textBoxBagCount.Size = new System.Drawing.Size(60, 20);
            this.textBoxBagCount.TabIndex = 3;
            this.textBoxBagCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelBagCount
            // 
            this.labelBagCount.AutoSize = true;
            this.labelBagCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBagCount.Location = new System.Drawing.Point(462, 102);
            this.labelBagCount.Name = "labelBagCount";
            this.labelBagCount.Size = new System.Drawing.Size(66, 13);
            this.labelBagCount.TabIndex = 12;
            this.labelBagCount.Text = "Bag Count";
            // 
            // comboBoxCustomerSelector
            // 
            this.comboBoxCustomerSelector.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxCustomerSelector.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxCustomerSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCustomerSelector.FormattingEnabled = true;
            this.comboBoxCustomerSelector.Location = new System.Drawing.Point(143, 141);
            this.comboBoxCustomerSelector.Name = "comboBoxCustomerSelector";
            this.comboBoxCustomerSelector.Size = new System.Drawing.Size(211, 21);
            this.comboBoxCustomerSelector.TabIndex = 18;
            // 
            // labelSelectCustomer
            // 
            this.labelSelectCustomer.AutoSize = true;
            this.labelSelectCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectCustomer.Location = new System.Drawing.Point(23, 144);
            this.labelSelectCustomer.Name = "labelSelectCustomer";
            this.labelSelectCustomer.Size = new System.Drawing.Size(99, 13);
            this.labelSelectCustomer.TabIndex = 17;
            this.labelSelectCustomer.Text = "Select Customer";
            // 
            // labelMoneyCenter
            // 
            this.labelMoneyCenter.AutoSize = true;
            this.labelMoneyCenter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMoneyCenter.Location = new System.Drawing.Point(23, 78);
            this.labelMoneyCenter.Name = "labelMoneyCenter";
            this.labelMoneyCenter.Size = new System.Drawing.Size(85, 13);
            this.labelMoneyCenter.TabIndex = 19;
            this.labelMoneyCenter.Text = "Money Center";
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(416, 14);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(112, 23);
            this.buttonClear.TabIndex = 20;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            // 
            // textBoxAmount
            // 
            this.textBoxAmount.Location = new System.Drawing.Point(456, 175);
            this.textBoxAmount.Name = "textBoxAmount";
            this.textBoxAmount.Size = new System.Drawing.Size(154, 20);
            this.textBoxAmount.TabIndex = 2;
            this.textBoxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelCustomerName
            // 
            this.labelCustomerName.AutoSize = true;
            this.labelCustomerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerName.Location = new System.Drawing.Point(23, 122);
            this.labelCustomerName.Name = "labelCustomerName";
            this.labelCustomerName.Size = new System.Drawing.Size(95, 13);
            this.labelCustomerName.TabIndex = 21;
            this.labelCustomerName.Text = "Customer Name";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(544, 43);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(112, 23);
            this.buttonSave.TabIndex = 22;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // labelMoneyCenterSeal
            // 
            this.labelMoneyCenterSeal.AutoSize = true;
            this.labelMoneyCenterSeal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMoneyCenterSeal.Location = new System.Drawing.Point(23, 103);
            this.labelMoneyCenterSeal.Name = "labelMoneyCenterSeal";
            this.labelMoneyCenterSeal.Size = new System.Drawing.Size(114, 13);
            this.labelMoneyCenterSeal.TabIndex = 24;
            this.labelMoneyCenterSeal.Text = "Money Center Seal";
            // 
            // textBoxMoneyCenterSeal
            // 
            this.textBoxMoneyCenterSeal.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.textBoxMoneyCenterSeal.Location = new System.Drawing.Point(144, 100);
            this.textBoxMoneyCenterSeal.Name = "textBoxMoneyCenterSeal";
            this.textBoxMoneyCenterSeal.Size = new System.Drawing.Size(237, 20);
            this.textBoxMoneyCenterSeal.TabIndex = 25;
            // 
            // buttonCustomerInquiry
            // 
            this.buttonCustomerInquiry.Location = new System.Drawing.Point(258, 71);
            this.buttonCustomerInquiry.Name = "buttonCustomerInquiry";
            this.buttonCustomerInquiry.Size = new System.Drawing.Size(152, 23);
            this.buttonCustomerInquiry.TabIndex = 26;
            this.buttonCustomerInquiry.Text = "Customer Inquiry";
            this.buttonCustomerInquiry.UseVisualStyleBackColor = true;
            // 
            // buttonManifestDetails
            // 
            this.buttonManifestDetails.Location = new System.Drawing.Point(419, 42);
            this.buttonManifestDetails.Name = "buttonManifestDetails";
            this.buttonManifestDetails.Size = new System.Drawing.Size(110, 23);
            this.buttonManifestDetails.TabIndex = 27;
            this.buttonManifestDetails.Text = "Manifest Details";
            this.buttonManifestDetails.UseVisualStyleBackColor = true;
            // 
            // FrmBagManifest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 569);
            this.Controls.Add(this.buttonManifestDetails);
            this.Controls.Add(this.buttonCustomerInquiry);
            this.Controls.Add(this.textBoxMoneyCenterSeal);
            this.Controls.Add(this.labelMoneyCenterSeal);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelCustomerName);
            this.Controls.Add(this.textBoxAmount);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.labelMoneyCenter);
            this.Controls.Add(this.comboBoxCustomerSelector);
            this.Controls.Add(this.labelSelectCustomer);
            this.Controls.Add(this.textBoxBagCount);
            this.Controls.Add(this.labelBagCount);
            this.Controls.Add(this.labelAmount);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonPrintManifest);
            this.Controls.Add(this.buttonEditManifest);
            this.Controls.Add(this.buttonNewMixedCustManifest);
            this.Controls.Add(this.textBoxBagNumber);
            this.Controls.Add(this.dataGridViewManifestDetails);
            this.Controls.Add(this.labelBagNumber);
            this.Controls.Add(this.buttonNewOneCustManifest);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmBagManifest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manifest";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewManifestDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.Button buttonNewOneCustManifest;
    public System.Windows.Forms.TextBox textBoxBagNumber;
    public System.Windows.Forms.Button buttonNewMixedCustManifest;
    public System.Windows.Forms.Button buttonEditManifest;
    public System.Windows.Forms.Button buttonPrintManifest;
    public System.Windows.Forms.Button buttonClose;
    public System.Windows.Forms.TextBox textBoxBagCount;
    public System.Windows.Forms.ComboBox comboBoxCustomerSelector;
    public System.Windows.Forms.Label labelMoneyCenter;
    public System.Windows.Forms.Button buttonClear;
    public System.Windows.Forms.Label labelBagNumber;
    public System.Windows.Forms.Label labelAmount;
    public System.Windows.Forms.DataGridView dataGridViewManifestDetails;
    public System.Windows.Forms.TextBox textBoxAmount;
    public System.Windows.Forms.Label labelCustomerName;
    public System.Windows.Forms.Label labelSelectCustomer;
    public System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBagNumber;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAmount;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCustomer;
    public System.Windows.Forms.Label labelBagCount;
    public System.Windows.Forms.Label labelMoneyCenterSeal;
    public System.Windows.Forms.TextBox textBoxMoneyCenterSeal;
    public System.Windows.Forms.Button buttonCustomerInquiry;
    public System.Windows.Forms.Button buttonManifestDetails;
  }
}