using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommonAppClasses;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessReports
{
    public partial class FrmReportDataSelector : Form
    {
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public bool cont = true;
        public string driver = "";
        public string compcode = "";
        public string storecode = "";
        public  DateTime startdate = Convert.ToDateTime("01/01/01");
        public   DateTime enddate = Convert.ToDateTime("01/01/2999");
          
        public FrmReportDataSelector()
        {
            InitializeComponent();
            labelDates.Text = "All";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cont = false;
            this.Close();
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonDriver_Click(object sender, EventArgs e)
        {
            driver = commonAppDataMethods.SelectDriver();
            if (driver.TrimEnd() != "")
            {
                labelDriver.Text = commonAppDataMethods.GetDriverName(driver);
            }
            else
            {
                labelDriver.Text = "All";
            }
        }

        private void buttonCompany_Click(object sender, EventArgs e)
        {
            compcode = commonAppDataMethods.SelectActiveCompany();
            if (compcode.TrimEnd() != "")
            {
                labelCompany.Text = commonAppDataMethods.GetCompanyName(compcode);
            }
            else
            {
                labelCompany.Text = "All";
            }

        }

        private void buttonStore_Click(object sender, EventArgs e)
        {
           storecode = commonAppDataMethods.SelectCompanyAndStore();
            if (storecode.TrimEnd() != "")
            {
              labelStore.Text = commonAppDataMethods.GetStoreName(storecode);
            }
            else
            {
                labelStore.Text = "All";
            }
        }

        private void buttonDates_Click(object sender, EventArgs e)
        {
            if (commonAppDataMethods.GetTwoDates("Delivery Dates"))
            { 
               startdate = commonAppDataMethods.SelectedStartDate;
               enddate = commonAppDataMethods.SelectedEndDate;
               labelDates.Text =               String.Format("{0:MM/dd/yy}", startdate) + " - " + String.Format("{0:MM/dd/yy}", enddate);
            }
            else
            {
                labelDates.Text = "All";
            }
        }
    }
}
