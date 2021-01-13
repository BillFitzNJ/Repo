namespace CommonAppClasses
{
  partial class FrmGetDateAndTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmGetDateAndTime));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonProceed = new System.Windows.Forms.Button();
            this.dateTimePickerDateTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(149, 12);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonProceed
            // 
            this.buttonProceed.Location = new System.Drawing.Point(246, 12);
            this.buttonProceed.Name = "buttonProceed";
            this.buttonProceed.Size = new System.Drawing.Size(75, 23);
            this.buttonProceed.TabIndex = 2;
            this.buttonProceed.Text = "Proceed";
            this.buttonProceed.UseVisualStyleBackColor = true;
            this.buttonProceed.Click += new System.EventHandler(this.buttonProceed_Click);
            // 
            // dateTimePickerDateTime
            // 
            this.dateTimePickerDateTime.CustomFormat = "MM/dd/yyyy:hh:mm tt";
            this.dateTimePickerDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerDateTime.Location = new System.Drawing.Point(79, 76);
            this.dateTimePickerDateTime.Name = "dateTimePickerDateTime";
            this.dateTimePickerDateTime.Size = new System.Drawing.Size(145, 20);
            this.dateTimePickerDateTime.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(115, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Date";
            // 
            // FrmGetDateAndTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(351, 156);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePickerDateTime);
            this.Controls.Add(this.buttonProceed);
            this.Controls.Add(this.buttonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmGetDateAndTime";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter date and time";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonProceed;
    private System.Windows.Forms.DateTimePicker dateTimePickerDateTime;
    private System.Windows.Forms.Label label1;
  }
}