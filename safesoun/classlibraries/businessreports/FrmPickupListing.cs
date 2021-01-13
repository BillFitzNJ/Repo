using System;
using System.Collections.Generic;
using Microsoft.Reporting.WinForms;
using CommonAppClasses;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net;
using System.Windows.Forms;
using System.Globalization;
using WSGUtilitieslib;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Reflection;
namespace BusinessReports
{

    public partial class FrmPickupListing : Form
    {
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public bool cont = true;
        public bool cancel = false;
        public string reportsequence = "";
        public DateTime reportdate = DateTime.Now.Date;
        public string reportdriver = "";
        WSGUtilities wsgUtilities = new WSGUtilities("Reports");
        AppUtilities appUtilities = new AppUtilities();
        public FrmPickupListing()
        {
            InitializeComponent();
            radioButtonMoneyCenter.Checked = true;
        }


        private void buttonCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            cont = true;
            if (reportdriver.TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("There must be a driver");
                cont = false;
            }

            if (cont)
            {
                if (radioButtonMoneyCenter.Checked)
                {
                    reportsequence = "Money Center";
                }
                else
                {
                    if (radioButtonCompany.Checked)
                    {
                        reportsequence = "Company";
                    }
                    else
                    {
                        reportsequence = "Manifest";
                    }
                }

                this.Close();
            }
        }

        private void buttonDriver_Click(object sender, EventArgs e)
        {
            reportdriver = commonAppDataMethods.SelectDriver();
            if (reportdriver.TrimEnd() != "")
            {
                labelDriver.Text = commonAppDataMethods.GetDriverName(reportdriver);
            }
            else
            {
                labelDriver.Text = "";
            }
        }

        private void buttonDate_Click(object sender, EventArgs e)
        {
            if (commonAppDataMethods.GetSingleDate("Enter Pickup Date", 10000, 10000))
            {
                reportdate = commonAppDataMethods.SelectedDate;
                labelDate.Text = reportdate.ToShortDateString();
            }
        }
    }
}
