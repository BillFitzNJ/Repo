namespace BusinessReports
{
    partial class FrmVSReportViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmVSReportViewer));
            this.reportViewerVSReport = new Microsoft.Reporting.WinForms.ReportViewer();
            this.SuspendLayout();
            // 
            // reportViewerVSReport
            // 
            this.reportViewerVSReport.Location = new System.Drawing.Point(12, 12);
            this.reportViewerVSReport.Name = "reportViewerVSReport";
            this.reportViewerVSReport.Size = new System.Drawing.Size(1033, 562);
            this.reportViewerVSReport.TabIndex = 0;
            // 
            // FrmVSReportViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 586);
            this.Controls.Add(this.reportViewerVSReport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmVSReportViewer";
            this.Text = "Report Viewer";
            this.Load += new System.EventHandler(this.FrmVSReportViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public Microsoft.Reporting.WinForms.ReportViewer reportViewerVSReport;

    }
}