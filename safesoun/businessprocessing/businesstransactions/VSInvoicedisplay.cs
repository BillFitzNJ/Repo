using System;
using CommonAppClasses;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSGUtilitieslib;
namespace BusinessTransactions
{
    public class VSInvoicedisplay : WSGDataAccess
    {

        WSGUtilities wsgUtilities = new WSGUtilities("Invoice Display");
        AppUtilities appUtilities = new AppUtilities();
        ssprocess ssprocessds = new ssprocess();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        BusinessReports.BusinessReportsMethods brmethods = new BusinessReports.BusinessReportsMethods();
        BusinessReports.BusinessReportsMethods brMethods = new BusinessReports.BusinessReportsMethods();
        public VSInvoicedisplay()
            : base("SQL", "SQLConnString")
        {

        }
        public void ShowVSInvoice()
        {

            brmethods.ShowVsInvoice("2020-33689");
         
        }

    }
    public class Billingdisplay
    {
        public int billingmonth { get; set; }
        public int billingyear { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
        public string storecode { get; set; }
        public string compcode { get; set; }

        public string invnumber { get; set; }
    }

}


