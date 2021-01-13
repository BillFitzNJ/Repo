namespace CommonAppClasses
{
  partial class FrmSelectCoinFunder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSelectCoinFunder));
            this.buttonSafeAndSound = new System.Windows.Forms.Button();
            this.buttonSignature = new System.Windows.Forms.Button();
            this.buttonRapid = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonFEGS = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSafeAndSound
            // 
            this.buttonSafeAndSound.Location = new System.Drawing.Point(46, 27);
            this.buttonSafeAndSound.Name = "buttonSafeAndSound";
            this.buttonSafeAndSound.Size = new System.Drawing.Size(119, 23);
            this.buttonSafeAndSound.TabIndex = 0;
            this.buttonSafeAndSound.Text = "Safe And Sound";
            this.buttonSafeAndSound.UseVisualStyleBackColor = true;
            this.buttonSafeAndSound.Click += new System.EventHandler(this.buttonSafeAndSound_Click);
            // 
            // buttonSignature
            // 
            this.buttonSignature.Location = new System.Drawing.Point(46, 63);
            this.buttonSignature.Name = "buttonSignature";
            this.buttonSignature.Size = new System.Drawing.Size(119, 23);
            this.buttonSignature.TabIndex = 1;
            this.buttonSignature.Text = "Signature";
            this.buttonSignature.UseVisualStyleBackColor = true;
            this.buttonSignature.Click += new System.EventHandler(this.buttonSignature_Click);
            // 
            // buttonRapid
            // 
            this.buttonRapid.Location = new System.Drawing.Point(46, 98);
            this.buttonRapid.Name = "buttonRapid";
            this.buttonRapid.Size = new System.Drawing.Size(119, 23);
            this.buttonRapid.TabIndex = 2;
            this.buttonRapid.Text = "Rapid";
            this.buttonRapid.UseVisualStyleBackColor = true;
            this.buttonRapid.Click += new System.EventHandler(this.buttonRapid_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(47, 157);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(119, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonFEGS
            // 
            this.buttonFEGS.Location = new System.Drawing.Point(47, 128);
            this.buttonFEGS.Name = "buttonFEGS";
            this.buttonFEGS.Size = new System.Drawing.Size(119, 23);
            this.buttonFEGS.TabIndex = 4;
            this.buttonFEGS.Text = "FEGS";
            this.buttonFEGS.UseVisualStyleBackColor = true;
            this.buttonFEGS.Click += new System.EventHandler(this.buttonFEGS_Click);
            // 
            // FrmSelectCoinFunder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 233);
            this.Controls.Add(this.buttonFEGS);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonRapid);
            this.Controls.Add(this.buttonSignature);
            this.Controls.Add(this.buttonSafeAndSound);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmSelectCoinFunder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select CoinFunder";
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonSafeAndSound;
    private System.Windows.Forms.Button buttonSignature;
    private System.Windows.Forms.Button buttonRapid;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Button buttonFEGS;
  }
}