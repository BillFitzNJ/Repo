namespace CommonAppClasses
{
  partial class FrmGetDate
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetDate));
      this.buttonCancel = new System.Windows.Forms.Button();
      this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
      this.LabelDateText = new System.Windows.Forms.Label();
      this.buttonOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(55, 2);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 0;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // dateTimePicker1
      // 
      this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dateTimePicker1.Location = new System.Drawing.Point(32, 80);
      this.dateTimePicker1.Name = "dateTimePicker1";
      this.dateTimePicker1.Size = new System.Drawing.Size(108, 20);
      this.dateTimePicker1.TabIndex = 1;
      this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
      // 
      // LabelDateText
      // 
      this.LabelDateText.AutoSize = true;
      this.LabelDateText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LabelDateText.Location = new System.Drawing.Point(48, 51);
      this.LabelDateText.Name = "LabelDateText";
      this.LabelDateText.Size = new System.Drawing.Size(63, 13);
      this.LabelDateText.TabIndex = 2;
      this.LabelDateText.Text = "Date Text";
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(136, 2);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 3;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // FrmGetDate
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(264, 156);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.LabelDateText);
      this.Controls.Add(this.dateTimePicker1);
      this.Controls.Add(this.buttonCancel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FrmGetDate";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Date Selector";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmGetDate_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonOK;
    public System.Windows.Forms.Label LabelDateText;
    public System.Windows.Forms.DateTimePicker dateTimePicker1;
  }
}