namespace BusinessTransactions
{
  partial class FrmBagSearchInput
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBagSearchInput));
      this.buttonSearch = new System.Windows.Forms.Button();
      this.buttonClear = new System.Windows.Forms.Button();
      this.buttonClose = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.textBoxBagNumber = new System.Windows.Forms.TextBox();
      this.radioButtonBeginsWith = new System.Windows.Forms.RadioButton();
      this.radioButtonEndswith = new System.Windows.Forms.RadioButton();
      this.radioButtonContains = new System.Windows.Forms.RadioButton();
      this.groupBoxTextSearchOptions = new System.Windows.Forms.GroupBox();
      this.label2 = new System.Windows.Forms.Label();
      this.dateTimePickerDate = new System.Windows.Forms.DateTimePicker();
      this.textBoxAmount = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.dataGridViewSearchResults = new System.Windows.Forms.DataGridView();
      this.labelSeachDate = new System.Windows.Forms.Label();
      this.textBoxSealNumber = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.ColumnBag = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnManifest = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnConsignee = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnCustomer = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnBankSealNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.groupBoxTextSearchOptions.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearchResults)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonSearch
      // 
      this.buttonSearch.Location = new System.Drawing.Point(670, 56);
      this.buttonSearch.Name = "buttonSearch";
      this.buttonSearch.Size = new System.Drawing.Size(75, 23);
      this.buttonSearch.TabIndex = 0;
      this.buttonSearch.Text = "Search";
      this.buttonSearch.UseVisualStyleBackColor = true;
      // 
      // buttonClear
      // 
      this.buttonClear.Location = new System.Drawing.Point(670, 26);
      this.buttonClear.Name = "buttonClear";
      this.buttonClear.Size = new System.Drawing.Size(75, 23);
      this.buttonClear.TabIndex = 1;
      this.buttonClear.Text = "Clear";
      this.buttonClear.UseVisualStyleBackColor = true;
      // 
      // buttonClose
      // 
      this.buttonClose.Location = new System.Drawing.Point(751, 26);
      this.buttonClose.Name = "buttonClose";
      this.buttonClose.Size = new System.Drawing.Size(75, 23);
      this.buttonClose.TabIndex = 2;
      this.buttonClose.Text = "Close";
      this.buttonClose.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(203, 40);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(76, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Bag Number";
      // 
      // textBoxBagNumber
      // 
      this.textBoxBagNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
      this.textBoxBagNumber.Location = new System.Drawing.Point(287, 37);
      this.textBoxBagNumber.Name = "textBoxBagNumber";
      this.textBoxBagNumber.Size = new System.Drawing.Size(178, 20);
      this.textBoxBagNumber.TabIndex = 4;
      // 
      // radioButtonBeginsWith
      // 
      this.radioButtonBeginsWith.AutoSize = true;
      this.radioButtonBeginsWith.Location = new System.Drawing.Point(32, 19);
      this.radioButtonBeginsWith.Name = "radioButtonBeginsWith";
      this.radioButtonBeginsWith.Size = new System.Drawing.Size(82, 17);
      this.radioButtonBeginsWith.TabIndex = 5;
      this.radioButtonBeginsWith.TabStop = true;
      this.radioButtonBeginsWith.Text = "Begins With";
      this.radioButtonBeginsWith.UseVisualStyleBackColor = true;
      // 
      // radioButtonEndswith
      // 
      this.radioButtonEndswith.AutoSize = true;
      this.radioButtonEndswith.Location = new System.Drawing.Point(32, 38);
      this.radioButtonEndswith.Name = "radioButtonEndswith";
      this.radioButtonEndswith.Size = new System.Drawing.Size(74, 17);
      this.radioButtonEndswith.TabIndex = 6;
      this.radioButtonEndswith.TabStop = true;
      this.radioButtonEndswith.Text = "Ends With";
      this.radioButtonEndswith.UseVisualStyleBackColor = true;
      // 
      // radioButtonContains
      // 
      this.radioButtonContains.AutoSize = true;
      this.radioButtonContains.Location = new System.Drawing.Point(32, 58);
      this.radioButtonContains.Name = "radioButtonContains";
      this.radioButtonContains.Size = new System.Drawing.Size(66, 17);
      this.radioButtonContains.TabIndex = 7;
      this.radioButtonContains.TabStop = true;
      this.radioButtonContains.Text = "Contains";
      this.radioButtonContains.UseVisualStyleBackColor = true;
      // 
      // groupBoxTextSearchOptions
      // 
      this.groupBoxTextSearchOptions.Controls.Add(this.radioButtonContains);
      this.groupBoxTextSearchOptions.Controls.Add(this.radioButtonEndswith);
      this.groupBoxTextSearchOptions.Controls.Add(this.radioButtonBeginsWith);
      this.groupBoxTextSearchOptions.Location = new System.Drawing.Point(16, 40);
      this.groupBoxTextSearchOptions.Name = "groupBoxTextSearchOptions";
      this.groupBoxTextSearchOptions.Size = new System.Drawing.Size(131, 91);
      this.groupBoxTextSearchOptions.TabIndex = 8;
      this.groupBoxTextSearchOptions.TabStop = false;
      this.groupBoxTextSearchOptions.Text = "Search Options";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(245, 85);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(34, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Date";
      // 
      // dateTimePickerDate
      // 
      this.dateTimePickerDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dateTimePickerDate.Location = new System.Drawing.Point(287, 85);
      this.dateTimePickerDate.Name = "dateTimePickerDate";
      this.dateTimePickerDate.Size = new System.Drawing.Size(168, 20);
      this.dateTimePickerDate.TabIndex = 10;
      // 
      // textBoxAmount
      // 
      this.textBoxAmount.Location = new System.Drawing.Point(287, 111);
      this.textBoxAmount.Name = "textBoxAmount";
      this.textBoxAmount.Size = new System.Drawing.Size(178, 20);
      this.textBoxAmount.TabIndex = 12;
      this.textBoxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(230, 114);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(49, 13);
      this.label3.TabIndex = 11;
      this.label3.Text = "Amount";
      // 
      // dataGridViewSearchResults
      // 
      dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
      dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
      dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Info;
      dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
      dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
      dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
      this.dataGridViewSearchResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
      this.dataGridViewSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewSearchResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnBag,
            this.ColumnManifest,
            this.ColumnConsignee,
            this.ColumnAmount,
            this.ColumnDate,
            this.ColumnCustomer,
            this.ColumnBankSealNumber});
      this.dataGridViewSearchResults.EnableHeadersVisualStyles = false;
      this.dataGridViewSearchResults.Location = new System.Drawing.Point(69, 153);
      this.dataGridViewSearchResults.Name = "dataGridViewSearchResults";
      this.dataGridViewSearchResults.ReadOnly = true;
      this.dataGridViewSearchResults.RowHeadersVisible = false;
      this.dataGridViewSearchResults.Size = new System.Drawing.Size(1004, 465);
      this.dataGridViewSearchResults.TabIndex = 13;
      // 
      // labelSeachDate
      // 
      this.labelSeachDate.AutoSize = true;
      this.labelSeachDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelSeachDate.Location = new System.Drawing.Point(481, 91);
      this.labelSeachDate.Name = "labelSeachDate";
      this.labelSeachDate.Size = new System.Drawing.Size(78, 13);
      this.labelSeachDate.TabIndex = 14;
      this.labelSeachDate.Text = "Search Date";
      // 
      // textBoxSealNumber
      // 
      this.textBoxSealNumber.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
      this.textBoxSealNumber.Location = new System.Drawing.Point(287, 63);
      this.textBoxSealNumber.Name = "textBoxSealNumber";
      this.textBoxSealNumber.Size = new System.Drawing.Size(178, 20);
      this.textBoxSealNumber.TabIndex = 16;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(165, 65);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(114, 13);
      this.label4.TabIndex = 15;
      this.label4.Text = "Money Center Seal";
      // 
      // ColumnBag
      // 
      this.ColumnBag.DataPropertyName = "sealnumber";
      this.ColumnBag.HeaderText = "Bag";
      this.ColumnBag.Name = "ColumnBag";
      this.ColumnBag.ReadOnly = true;
      this.ColumnBag.Width = 150;
      // 
      // ColumnManifest
      // 
      this.ColumnManifest.DataPropertyName = "idcol";
      this.ColumnManifest.HeaderText = "Manifest";
      this.ColumnManifest.Name = "ColumnManifest";
      this.ColumnManifest.ReadOnly = true;
      // 
      // ColumnConsignee
      // 
      this.ColumnConsignee.DataPropertyName = "centername";
      this.ColumnConsignee.HeaderText = "Consignee";
      this.ColumnConsignee.Name = "ColumnConsignee";
      this.ColumnConsignee.ReadOnly = true;
      this.ColumnConsignee.Width = 200;
      // 
      // ColumnAmount
      // 
      this.ColumnAmount.DataPropertyName = "amount";
      this.ColumnAmount.HeaderText = "Amount";
      this.ColumnAmount.Name = "ColumnAmount";
      this.ColumnAmount.ReadOnly = true;
      // 
      // ColumnDate
      // 
      this.ColumnDate.DataPropertyName = "postingdate";
      this.ColumnDate.HeaderText = "Date";
      this.ColumnDate.Name = "ColumnDate";
      this.ColumnDate.ReadOnly = true;
      // 
      // ColumnCustomer
      // 
      this.ColumnCustomer.DataPropertyName = "customer";
      this.ColumnCustomer.HeaderText = "Customer";
      this.ColumnCustomer.Name = "ColumnCustomer";
      this.ColumnCustomer.ReadOnly = true;
      this.ColumnCustomer.Width = 200;
      // 
      // ColumnBankSealNumber
      // 
      this.ColumnBankSealNumber.DataPropertyName = "banksealnumber";
      this.ColumnBankSealNumber.HeaderText = "Money Center Seal";
      this.ColumnBankSealNumber.Name = "ColumnBankSealNumber";
      this.ColumnBankSealNumber.ReadOnly = true;
      this.ColumnBankSealNumber.Width = 150;
      // 
      // FrmBagSearchInput
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1123, 656);
      this.Controls.Add(this.textBoxSealNumber);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.labelSeachDate);
      this.Controls.Add(this.dataGridViewSearchResults);
      this.Controls.Add(this.textBoxAmount);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.dateTimePickerDate);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.groupBoxTextSearchOptions);
      this.Controls.Add(this.textBoxBagNumber);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.buttonClose);
      this.Controls.Add(this.buttonClear);
      this.Controls.Add(this.buttonSearch);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FrmBagSearchInput";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Bag Search";
      this.groupBoxTextSearchOptions.ResumeLayout(false);
      this.groupBoxTextSearchOptions.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearchResults)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.Button buttonSearch;
    public System.Windows.Forms.Button buttonClear;
    public System.Windows.Forms.Button buttonClose;
    public System.Windows.Forms.Label label1;
    public System.Windows.Forms.TextBox textBoxBagNumber;
    public System.Windows.Forms.GroupBox groupBoxTextSearchOptions;
    public System.Windows.Forms.Label label2;
    public System.Windows.Forms.DateTimePicker dateTimePickerDate;
    public System.Windows.Forms.TextBox textBoxAmount;
    public System.Windows.Forms.Label label3;
    public System.Windows.Forms.RadioButton radioButtonBeginsWith;
    public System.Windows.Forms.RadioButton radioButtonEndswith;
    public System.Windows.Forms.RadioButton radioButtonContains;
    public System.Windows.Forms.DataGridView dataGridViewSearchResults;
    public System.Windows.Forms.Label labelSeachDate;
    public System.Windows.Forms.TextBox textBoxSealNumber;
    public System.Windows.Forms.Label label4;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBag;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnManifest;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnConsignee;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnAmount;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCustomer;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBankSealNumber;
  }
}