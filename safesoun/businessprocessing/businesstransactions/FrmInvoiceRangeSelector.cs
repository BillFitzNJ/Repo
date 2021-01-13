using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using CommonAppClasses;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessTransactions
{
    public partial class FrmInvoiceRangeSelector : Form
    {
        public bool cont = true;
        public String CompCode = "";
        public String StoreCode = "";
        public Int32 BeginMonth = 0;
        public Int32 BeginYear = 0;
        public Int32 EndMonth = 0;
        public Int32 EndYear = 0;
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public FrmInvoiceRangeSelector()
        {
            InitializeComponent();
            numericUpDownStartMonth.Value = DateTime.Now.AddMonths(-1).Month;
            numericUpDownStartYear.Value  = DateTime.Now.AddMonths(-1).Year;
            numericUpDownEndMonth.Value = DateTime.Now.AddMonths(-1).Month;
            numericUpDownEndYear.Value = DateTime.Now.AddMonths(-1).Year;
         
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.cont = false;
            this.Close();
        }

        private void buttonCompany_Click(object sender, EventArgs e)
        {
            CompCode = commonAppDataMethods.SelectCompany();
            if (CompCode.TrimEnd() != "")
            {
                labelCompany.Text = commonAppDataMethods.GetCompanyName(CompCode);
            }
            else
            {
                labelCompany.Text = "Unspecified";
            }
        }

        private void buttonStore_Click(object sender, EventArgs e)
        {

        }
    }
}
