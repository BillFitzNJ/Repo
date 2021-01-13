namespace BusinessTransactions
{
  partial class FrmExtractBankFiles
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExtractBankFiles));
      this.buttonReturn = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.dateTimePickerProcessDate = new System.Windows.Forms.DateTimePicker();
      this.buttonGetFiles = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonReturn
      // 
      this.buttonReturn.Location = new System.Drawing.Point(306, 12);
      this.buttonReturn.Name = "buttonReturn";
      this.buttonReturn.Size = new System.Drawing.Size(75, 23);
      this.buttonReturn.TabIndex = 0;
      this.buttonReturn.Text = "Return";
      this.buttonReturn.UseVisualStyleBackColor = true;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(12, 79);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(140, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Select Processing Date";
      // 
      // dateTimePickerProcessDate
      // 
      this.dateTimePickerProcessDate.Location = new System.Drawing.Point(158, 73);
      this.dateTimePickerProcessDate.Name = "dateTimePickerProcessDate";
      this.dateTimePickerProcessDate.Size = new System.Drawing.Size(200, 20);
      this.dateTimePickerProcessDate.TabIndex = 3;
      // 
      // buttonGetFiles
      // 
      this.buttonGetFiles.Location = new System.Drawing.Point(128, 110);
      this.buttonGetFiles.Name = "buttonGetFiles";
      this.buttonGetFiles.Size = new System.Drawing.Size(230, 23);
      this.buttonGetFiles.TabIndex = 4;
      this.buttonGetFiles.Text = "Select Destination and Copy Bank Files";
      this.buttonGetFiles.UseVisualStyleBackColor = true;
      // 
      // FrmExtractBankFiles
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(461, 258);
      this.Controls.Add(this.buttonGetFiles);
      this.Controls.Add(this.dateTimePickerProcessDate);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.buttonReturn);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FrmExtractBankFiles";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Extract Bank Files";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    public System.Windows.Forms.Button buttonReturn;
    public System.Windows.Forms.DateTimePicker dateTimePickerProcessDate;
    public System.Windows.Forms.Button buttonGetFiles;
  }
}