namespace CommonAppClasses
{
  partial class FrmGetTwoDates
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetTwoDates));
      this.buttonOK = new System.Windows.Forms.Button();
      this.LabelDateText = new System.Windows.Forms.Label();
      this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
      this.labelStart = new System.Windows.Forms.Label();
      this.labelEnd = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // buttonOK
      // 
      this.buttonOK.Location = new System.Drawing.Point(294, 12);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(75, 23);
      this.buttonOK.TabIndex = 7;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // LabelDateText
      // 
      this.LabelDateText.AutoSize = true;
      this.LabelDateText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.LabelDateText.Location = new System.Drawing.Point(89, 42);
      this.LabelDateText.Name = "LabelDateText";
      this.LabelDateText.Size = new System.Drawing.Size(63, 13);
      this.LabelDateText.TabIndex = 6;
      this.LabelDateText.Text = "Date Text";
      // 
      // dateTimePickerStart
      // 
      this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dateTimePickerStart.Location = new System.Drawing.Point(172, 76);
      this.dateTimePickerStart.Name = "dateTimePickerStart";
      this.dateTimePickerStart.Size = new System.Drawing.Size(106, 20);
      this.dateTimePickerStart.TabIndex = 5;
      this.dateTimePickerStart.ValueChanged += new System.EventHandler(this.dateTimePickerStart_ValueChanged);
      // 
      // buttonCancel
      // 
      this.buttonCancel.Location = new System.Drawing.Point(213, 12);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(75, 23);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // dateTimePickerEnd
      // 
      this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
      this.dateTimePickerEnd.Location = new System.Drawing.Point(172, 126);
      this.dateTimePickerEnd.Name = "dateTimePickerEnd";
      this.dateTimePickerEnd.Size = new System.Drawing.Size(97, 20);
      this.dateTimePickerEnd.TabIndex = 8;
      this.dateTimePickerEnd.ValueChanged += new System.EventHandler(this.dateTimePickerEnd_ValueChanged);
      // 
      // labelStart
      // 
      this.labelStart.AutoSize = true;
      this.labelStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelStart.Location = new System.Drawing.Point(58, 83);
      this.labelStart.Name = "labelStart";
      this.labelStart.Size = new System.Drawing.Size(94, 13);
      this.labelStart.TabIndex = 9;
      this.labelStart.Text = "Beginning Date";
      // 
      // labelEnd
      // 
      this.labelEnd.AutoSize = true;
      this.labelEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelEnd.Location = new System.Drawing.Point(58, 132);
      this.labelEnd.Name = "labelEnd";
      this.labelEnd.Size = new System.Drawing.Size(77, 13);
      this.labelEnd.TabIndex = 10;
      this.labelEnd.Text = "Ending Date";
      // 
      // FrmGetTwoDates
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(428, 191);
      this.Controls.Add(this.labelEnd);
      this.Controls.Add(this.labelStart);
      this.Controls.Add(this.dateTimePickerEnd);
      this.Controls.Add(this.buttonOK);
      this.Controls.Add(this.LabelDateText);
      this.Controls.Add(this.dateTimePickerStart);
      this.Controls.Add(this.buttonCancel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FrmGetTwoDates";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Date Selector";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmGetTwoDates_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOK;
    public System.Windows.Forms.Label LabelDateText;
    private System.Windows.Forms.DateTimePicker dateTimePickerStart;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
    public System.Windows.Forms.Label labelStart;
    public System.Windows.Forms.Label labelEnd;
  }
}