namespace GenDs
{
  partial class FrmGenerateTypedDataSet
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
            this.buttonCreateATMDataset = new System.Windows.Forms.Button();
            this.buttonCreateSystemTablesDataset = new System.Windows.Forms.Button();
            this.buttonCreateSSProcesDataSet = new System.Windows.Forms.Button();
            this.buttonCreateSlipsReportDataset = new System.Windows.Forms.Button();
            this.buttonMyQLDataset = new System.Windows.Forms.Button();
            this.buttonCreateReportDataset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonCreateATMDataset
            // 
            this.buttonCreateATMDataset.Location = new System.Drawing.Point(50, 284);
            this.buttonCreateATMDataset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreateATMDataset.Name = "buttonCreateATMDataset";
            this.buttonCreateATMDataset.Size = new System.Drawing.Size(210, 65);
            this.buttonCreateATMDataset.TabIndex = 0;
            this.buttonCreateATMDataset.Text = "Create ATM Dataset";
            this.buttonCreateATMDataset.UseVisualStyleBackColor = true;
            this.buttonCreateATMDataset.Click += new System.EventHandler(this.buttonCreateATMDataset_Click);
            // 
            // buttonCreateSystemTablesDataset
            // 
            this.buttonCreateSystemTablesDataset.Location = new System.Drawing.Point(50, 116);
            this.buttonCreateSystemTablesDataset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreateSystemTablesDataset.Name = "buttonCreateSystemTablesDataset";
            this.buttonCreateSystemTablesDataset.Size = new System.Drawing.Size(210, 65);
            this.buttonCreateSystemTablesDataset.TabIndex = 1;
            this.buttonCreateSystemTablesDataset.Text = "Create System Tables Dataset";
            this.buttonCreateSystemTablesDataset.UseVisualStyleBackColor = true;
            this.buttonCreateSystemTablesDataset.Click += new System.EventHandler(this.buttonCreateSystemTablesDataset_Click);
            // 
            // buttonCreateSSProcesDataSet
            // 
            this.buttonCreateSSProcesDataSet.Location = new System.Drawing.Point(50, 32);
            this.buttonCreateSSProcesDataSet.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreateSSProcesDataSet.Name = "buttonCreateSSProcesDataSet";
            this.buttonCreateSSProcesDataSet.Size = new System.Drawing.Size(210, 65);
            this.buttonCreateSSProcesDataSet.TabIndex = 4;
            this.buttonCreateSSProcesDataSet.Text = "Create SS Process Dataset";
            this.buttonCreateSSProcesDataSet.UseVisualStyleBackColor = true;
            this.buttonCreateSSProcesDataSet.Click += new System.EventHandler(this.buttonCreateSSProcesDataSet_Click);
            // 
            // buttonCreateSlipsReportDataset
            // 
            this.buttonCreateSlipsReportDataset.Location = new System.Drawing.Point(50, 368);
            this.buttonCreateSlipsReportDataset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreateSlipsReportDataset.Name = "buttonCreateSlipsReportDataset";
            this.buttonCreateSlipsReportDataset.Size = new System.Drawing.Size(210, 65);
            this.buttonCreateSlipsReportDataset.TabIndex = 5;
            this.buttonCreateSlipsReportDataset.Text = "Create Slips Report Dataset";
            this.buttonCreateSlipsReportDataset.UseVisualStyleBackColor = true;
            this.buttonCreateSlipsReportDataset.Click += new System.EventHandler(this.buttonCreateSlipsReportDataset_Click);
            // 
            // buttonMyQLDataset
            // 
            this.buttonMyQLDataset.Location = new System.Drawing.Point(50, 200);
            this.buttonMyQLDataset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonMyQLDataset.Name = "buttonMyQLDataset";
            this.buttonMyQLDataset.Size = new System.Drawing.Size(210, 65);
            this.buttonMyQLDataset.TabIndex = 6;
            this.buttonMyQLDataset.Text = "Create MySQL Dataset";
            this.buttonMyQLDataset.UseVisualStyleBackColor = true;
            this.buttonMyQLDataset.Click += new System.EventHandler(this.buttonMySQLDataset_Click);
            // 
            // buttonCreateReportDataset
            // 
            this.buttonCreateReportDataset.Location = new System.Drawing.Point(50, 452);
            this.buttonCreateReportDataset.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCreateReportDataset.Name = "buttonCreateReportDataset";
            this.buttonCreateReportDataset.Size = new System.Drawing.Size(210, 65);
            this.buttonCreateReportDataset.TabIndex = 7;
            this.buttonCreateReportDataset.Text = "Create  Report Dataset";
            this.buttonCreateReportDataset.UseVisualStyleBackColor = true;
            this.buttonCreateReportDataset.Click += new System.EventHandler(this.buttonCreateReportDataset_Click);
            // 
            // FrmGenerateTypedDataSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 531);
            this.Controls.Add(this.buttonCreateReportDataset);
            this.Controls.Add(this.buttonMyQLDataset);
            this.Controls.Add(this.buttonCreateSlipsReportDataset);
            this.Controls.Add(this.buttonCreateSSProcesDataSet);
            this.Controls.Add(this.buttonCreateSystemTablesDataset);
            this.Controls.Add(this.buttonCreateATMDataset);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmGenerateTypedDataSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate Typed Datasets";
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button buttonCreateATMDataset;
    private System.Windows.Forms.Button buttonCreateSystemTablesDataset;
    private System.Windows.Forms.Button buttonCreateSSProcesDataSet;
    private System.Windows.Forms.Button buttonCreateSlipsReportDataset;
    private System.Windows.Forms.Button buttonMyQLDataset;
    private System.Windows.Forms.Button buttonCreateReportDataset;
  }
}

