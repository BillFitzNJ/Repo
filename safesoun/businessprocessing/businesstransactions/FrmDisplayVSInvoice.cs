using System;
using System.Collections.Generic;
using CommonAppClasses;
using WSGUtilitieslib;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessTransactions
{
    public partial class FrmDisplayVSInvoice : Form
    {
        public Form menuform { get; set; }
        public string CompCode = "";
        public string StoreCode = "";
        public int invmonth = 0;
        public int invyear = 0;
        BusinessReports.BusinessReportsMethods brmethods = new BusinessReports.BusinessReportsMethods();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        BillingDisplaydata billingdisplaydata = new BillingDisplaydata();

        public FrmDisplayVSInvoice()
        {
            InitializeComponent();
        }

        public void ShowParent()
        {
            this.MdiParent = menuform;
            this.Update();
            this.Show();
            if (DateTime.Now.Date.Month != 1)
            {
                billingdisplaydata.invoicenumber = String.Format("{0:yyyy}", DateTime.Now.Date) + "-      ";
            }
            else
            {
                billingdisplaydata.invoicenumber = String.Format("{0:yyyy}", DateTime.Now.Date.AddYears(-1)) + "-      ";
            }
            this.radioButtonView.Checked = true;
            if (DateTime.Now.Month == 1)
            {
                billingdisplaydata.billingmonth = 12;
                billingdisplaydata.billingyear = DateTime.Now.Year - 1;
            }
            else
            {
                billingdisplaydata.billingmonth = DateTime.Now.Month - 1;
                billingdisplaydata.billingyear = DateTime.Now.Year;
            }

            textBoxInvnumber.DataBindings.Clear();
            textBoxInvnumber.DataBindings.Add("Text", billingdisplaydata, "invoicenumber");
            textBoxMonth.DataBindings.Clear();
            textBoxMonth.DataBindings.Add("Text", billingdisplaydata, "billingmonth");
            textBoxYear.DataBindings.Clear();
            textBoxYear.DataBindings.Add("Text", billingdisplaydata, "billingyear");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCompany_Click(object sender, EventArgs e)
        {
            CompCode = commonAppDataMethods.SelectCompany();
            if (CompCode.TrimEnd() != "")
            {
                labelCompanyname.Text = commonAppDataMethods.GetCompanyName(CompCode);
            }
            else
            {
                labelCompanyname.Text = "Unspecified";
            }
        }

        private void buttonSelectstore_Click(object sender, EventArgs e)
        {
            StoreCode = commonAppDataMethods.SelectCompanyAndStore();
            if (StoreCode.TrimEnd() != "")
            {
                labelStorename.Text = commonAppDataMethods.GetStoreName(StoreCode);
            }
            else
            {
                labelStorename.Text = "Unspecified";
            }
        }



        private void buttonProceed_Click(object sender, EventArgs e)
        {
            // Develop invoice date from month and year
            int daysinbillingmonth = DateTime.DaysInMonth(billingdisplaydata.billingyear, billingdisplaydata.billingmonth);
            billingdisplaydata.invoicedate = new DateTime(billingdisplaydata.billingyear, billingdisplaydata.billingmonth, daysinbillingmonth);

            billingdisplaydata.outputmethod = "V";
            if (this.radioButtonPrint.Checked)
            {
                billingdisplaydata.outputmethod = "P";
            }
            BillDisplayMethods billDisplayMethods = new BillDisplayMethods();
            billDisplayMethods.ProcessInvoiceDisplay(billingdisplaydata);
        }

    }
    public class BillDisplayMethods : WSGDataAccess
    {
        WSGUtilities wsgUtilities = new WSGUtilities("Billing Generation");
        AppUtilities appUtilities = new AppUtilities();

        BusinessReports.BusinessReportsMethods brmethods = new BusinessReports.BusinessReportsMethods();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        string commandstring = "";
        ssprocess ssprocessds = new ssprocess();
        public BillDisplayMethods()
            : base("SQL", "SQLConnString")
        {
        }
        public void ProcessInvoiceDisplay(BillingDisplaydata billingdisplaydata)
        {
          
            // First priority - specfic invoice number
            if (billingdisplaydata.invoicenumber.Substring(5,1).TrimEnd() != "" )
            {
                ssprocessds.billing.Rows.Clear();
                ClearParameters();
                commandstring = "SELECT TOP 1 * FROM billing where inv_number = @invnumber";
                AddParms("@invnumber",billingdisplaydata.invoicenumber, "SQL");
                FillData(ssprocessds, "billing", commandstring, CommandType.Text);
                if(ssprocessds.billing.Rows.Count > 0)
                {
                    brmethods.DisplayVSInvoice(billingdisplaydata.invoicenumber, billingdisplaydata.outputmethod);

                }
                else
                {
                    wsgUtilities.wsgNotice("That invoice number cannot be found");
                }
            }
          
         
        }

    }
    public class BillingDisplaydata
    {
        public string invoicenumber { get; set; }
        public int billingmonth { get; set; }
        public int billingyear { get; set; }
        public DateTime invoicedate { get; set; }
        public string storecode { get; set; }
        public string compcode { get; set; }
        public string outputmethod { get; set; }

    }
}
