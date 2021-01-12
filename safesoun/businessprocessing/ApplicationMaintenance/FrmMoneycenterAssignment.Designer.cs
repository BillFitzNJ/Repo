namespace ApplicationMaintenance
{
  partial class FrmMoneycenterAssignment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMoneycenterAssignment));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxUnassigned = new System.Windows.Forms.ListBox();
            this.listBoxAssigned = new System.Windows.Forms.ListBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelDefaultMoneyCenter = new System.Windows.Forms.Label();
            this.labelDefaultInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(346, 7);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(95, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Unassigned";
            // 
            // listBoxUnassigned
            // 
            this.listBoxUnassigned.FormattingEnabled = true;
            this.listBoxUnassigned.Location = new System.Drawing.Point(32, 138);
            this.listBoxUnassigned.Name = "listBoxUnassigned";
            this.listBoxUnassigned.Size = new System.Drawing.Size(218, 264);
            this.listBoxUnassigned.TabIndex = 2;
            // 
            // listBoxAssigned
            // 
            this.listBoxAssigned.FormattingEnabled = true;
            this.listBoxAssigned.Location = new System.Drawing.Point(294, 138);
            this.listBoxAssigned.Name = "listBoxAssigned";
            this.listBoxAssigned.Size = new System.Drawing.Size(224, 264);
            this.listBoxAssigned.TabIndex = 3;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(427, 7);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(372, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Assigned";
            // 
            // labelDefaultMoneyCenter
            // 
            this.labelDefaultMoneyCenter.AutoSize = true;
            this.labelDefaultMoneyCenter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultMoneyCenter.Location = new System.Drawing.Point(187, 52);
            this.labelDefaultMoneyCenter.Name = "labelDefaultMoneyCenter";
            this.labelDefaultMoneyCenter.Size = new System.Drawing.Size(130, 13);
            this.labelDefaultMoneyCenter.TabIndex = 6;
            this.labelDefaultMoneyCenter.Text = "Default Money Center";
            // 
            // labelDefaultInstructions
            // 
            this.labelDefaultInstructions.AutoSize = true;
            this.labelDefaultInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDefaultInstructions.Location = new System.Drawing.Point(133, 417);
            this.labelDefaultInstructions.Name = "labelDefaultInstructions";
            this.labelDefaultInstructions.Size = new System.Drawing.Size(321, 13);
            this.labelDefaultInstructions.TabIndex = 7;
            this.labelDefaultInstructions.Text = "To select a default money center, point and Right Click";
            // 
            // FrmMoneycenterAssignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 439);
            this.Controls.Add(this.labelDefaultInstructions);
            this.Controls.Add(this.labelDefaultMoneyCenter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.listBoxAssigned);
            this.Controls.Add(this.listBoxUnassigned);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMoneycenterAssignment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Moneycenter Assignment";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    public System.Windows.Forms.Button buttonCancel;
    public System.Windows.Forms.ListBox listBoxUnassigned;
    public System.Windows.Forms.ListBox listBoxAssigned;
    public System.Windows.Forms.Button buttonSave;
    public System.Windows.Forms.Label labelDefaultMoneyCenter;
    public System.Windows.Forms.Label labelDefaultInstructions;
  }
}