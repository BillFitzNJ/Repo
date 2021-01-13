using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonAppClasses
{
    public partial class FrmSelectInvoice : Form
    {
        CommonAppClasses.CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public string SelectedCompany = "";

        public FrmSelectInvoice()
        {
            InitializeComponent();
            RefreshCompanyLabel();
        }
      
        public bool cont = true;
        private void buttonProceed_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
           cont = false;
           this.Close();
        }

        private void buttonCompany_Click(object sender, EventArgs e)
        {
            SelectedCompany = commonAppDataMethods.SelectActiveCompany();
            RefreshCompanyLabel();
        }
        public void RefreshCompanyLabel()
        {
            if (SelectedCompany.TrimEnd() != "")
            {
                labelCompanyname.Text = commonAppDataMethods.GetCompanyName(SelectedCompany);
            }
            else
            {
                labelCompanyname.Text = "Company Not Specified";
            }
        }

        private void FrmSelectInvoice_Load(object sender, EventArgs e)
        {

        }
    }
}
