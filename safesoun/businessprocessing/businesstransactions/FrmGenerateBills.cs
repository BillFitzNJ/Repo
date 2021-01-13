using System;
using CommonAppClasses;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSGUtilitieslib;
namespace BusinessTransactions
{
    public partial class FrmGenerateBills : Form
    {
        public Form menuform = new Form();
        ProcessBillGeneration processBillGeneration = new ProcessBillGeneration();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        WSGUtilities wsgUtilities = new WSGUtilities("Billing Generation");
        AppUtilities appUtilities = new AppUtilities();
        string selectedstore = "";
        string selectedcompany = "";
        bool cont = false;
        public FrmGenerateBills()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmGenerateBills_Load(object sender, EventArgs e)
        {
            if (DateTime.Now.Month == 1)
            {
                processBillGeneration.billingappdata.billingmonth = 12;
                processBillGeneration.billingappdata.billingyear = DateTime.Now.Year - 1;

            }
            else
            {
                processBillGeneration.billingappdata.billingmonth = DateTime.Now.Month - 1;
                processBillGeneration.billingappdata.billingyear = DateTime.Now.Year;
            }

            textBoxMonth.DataBindings.Clear();
            textBoxMonth.DataBindings.Add("Text", processBillGeneration.billingappdata, "billingmonth");
            textBoxYear.DataBindings.Clear();
            textBoxYear.DataBindings.Add("Text", processBillGeneration.billingappdata, "billingyear");
        }


        private void buttonGenerate_Click(object sender, EventArgs e)
        {

            int daysinbillingmonth = DateTime.DaysInMonth(processBillGeneration.billingappdata.billingyear, processBillGeneration.billingappdata.billingmonth);
            DateTime invdate = new DateTime(processBillGeneration.billingappdata.billingyear, processBillGeneration.billingappdata.billingmonth, daysinbillingmonth);

            if (processBillGeneration.ValidateInvoiceDate(invdate))
            {
                cont = true;
            }
            else
            {
                wsgUtilities.wsgNotice("There are posted bills for the date you have selected. Billing generation cancelled");
                cont = false;
            }
            if (cont)
            {
                if (selectedcompany.TrimEnd() != "" && selectedstore.TrimEnd() != "")
                {
                    wsgUtilities.wsgNotice("Please select a company or a store, not both");
                    cont = false;
                }
            }
            if (cont)
            {
                cont = wsgUtilities.wsgReply("Do you want to generate bills?");
            }

            if (cont)
            {
                processBillGeneration.billingappdata.storecode = selectedstore;
                processBillGeneration.billingappdata.compcode = selectedcompany;
                processBillGeneration.generatebills();
                this.Close();
            }
        }

        public void RefreshStoreName()
        {
            labelStorename.Text = commonAppDataMethods.GetStoreName(selectedstore);
            if (labelStorename.Text.TrimEnd() == "")
            {
                labelStorename.Text = "All Stores";
            }
        }

        private void buttonSelectstore_Click(object sender, EventArgs e)
        {
            selectedstore = commonAppDataMethods.SelectCompanyAndStore();
            RefreshStoreName();
        }

        private void buttonCompany_Click(object sender, EventArgs e)
        {
            selectedcompany = commonAppDataMethods.SelectCompany();
            labelCompanyname.Text = commonAppDataMethods.GetCompanyName(selectedcompany);
            if (labelCompanyname.Text.TrimEnd() == "")
            {
                labelCompanyname.Text = "All Companies";
            }

        }


    }



}
