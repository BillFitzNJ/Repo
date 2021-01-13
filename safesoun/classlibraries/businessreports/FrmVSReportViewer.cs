using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessReports
{
    public partial class FrmVSReportViewer : Form
    {
        public FrmVSReportViewer()
        {
            InitializeComponent();
        }

        private void FrmVSReportViewer_Load(object sender, EventArgs e)
        {

            this.reportViewerVSReport.RefreshReport();
        }
    }
}
