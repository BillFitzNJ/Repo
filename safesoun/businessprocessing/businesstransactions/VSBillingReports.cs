using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using BusinessTransactions;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net;
using System.Globalization;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using SBPGPKeys;
using SBPGP;

namespace BusinessTransactions
{
    public class VSBillingReports : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Safekeeping");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess ssprocessds = new ssprocess();
        BusinessReports.BusinessReportsMethods businessReportMethods = new BusinessReports.BusinessReportsMethods();
        // PGP objects
        TElPGPWriter pgprwriter = new TElPGPWriter();
        TElPGPReader pgpreader = new TElPGPReader();
        TElPGPKeyring keyring = new TElPGPKeyring();
        TElPGPKeyring encryptingkeyring = new TElPGPKeyring();
        TElPGPSecretKey secretkey = new TElPGPSecretKey();
        System.Net.Mail.MailMessage EmailMessage = new System.Net.Mail.MailMessage();
        string reportpath = ConfigurationManager.AppSettings["ReportPath"];
        ssprocess ssprocesssearchds = new ssprocess();
        public VSBillingReports()
            : base("SQL", "SQLConnString")
        {

        }
        public void GenerateVSBillingReports(string reporttype)
        {
        
            int[] monthandyear = commonAppDataMethods.GetMonthAndYear();
            if (monthandyear[0] != 0)
            {
                switch (reporttype)
                {
                    case "R":
                        {
                            BusinessReports.BusinessReportsMethods businessReportMethods = new BusinessReports.BusinessReportsMethods();
                            businessReportMethods.RevenueSummary(monthandyear);
                            break;
                        }
                    case "B":
                        {
                            BusinessReports.BusinessReportsMethods businessReportMethods = new BusinessReports.BusinessReportsMethods();
                            businessReportMethods.BillingSummary(monthandyear);
                            break;
                        }
                     
                    case "T":
                        {
                            BusinessReports.BusinessReportsMethods businessReportMethods = new BusinessReports.BusinessReportsMethods();
                            businessReportMethods.TaxSummary(monthandyear);
                            break;
                        }
                
                }
               
                
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }
    }
}