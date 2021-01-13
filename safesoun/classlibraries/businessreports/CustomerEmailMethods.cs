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
namespace BusinessReports
{
    public class CustomerEmailMethods : WSGDataAccess
    {
        public string ReportName = "";
        public string ReportTitle = "";
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocessDs = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        ssprocess sscloseds = new ssprocess();
        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        sysdata sysdatads = new sysdata();
        WSGUtilities wsgUtilities = new WSGUtilities("Reports");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public CustomerEmailMethods()
            : base("SQL", "SQLConnString")
        {
        }

        public void SendCustomerReportEmail(string compcode, string emailsubject, string emailbody, List<string> attachments)
        {
            string commandtext = "SELECT * FROM company WHERE comp_code = @compcode";
            ssprocessds.company.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode, "SQL");
            FillData(ssprocessds, "company", commandtext, CommandType.Text);
            if (ssprocessds.company.Rows.Count > 0)
            {
                if (ssprocessds.company[0].email)
                {
                    ssprocessds.companyemailaddress.Rows.Clear();
                    commandtext = "SELECT * FROM companyemailaddress WHERE comp_code = @compcode";
                    ClearParameters();
                    AddParms("@compcode", compcode, "SQL");
                    FillData(ssprocessds, "companyemailaddress", commandtext, CommandType.Text);

                    System.Net.Mail.MailMessage customermessage = new System.Net.Mail.MailMessage();
                    customermessage.To.Clear();
                    for (int i = 0; i <= ssprocessds.companyemailaddress.Rows.Count - 1; i++)
                    {
                        customermessage.To.Add(ssprocessds.companyemailaddress[i].emailaddr.TrimEnd());
                    }
                    if (ConfigurationManager.AppSettings["TestMode"] == "True")
                    {
                        customermessage.To.Clear();
                        customermessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                    }

                    // Send the proof sheet, activity report and total files
                    customermessage.Attachments.Clear();
                    foreach (string filename in attachments)
                    {
                        customermessage.Attachments.Add(new System.Net.Mail.Attachment(filename));
                    }
                    // Send the pdf's
                    appUtilities.SendEmail(customermessage, emailsubject, "See Attached");
                }
            }
        }
        public void SendRegionReportEmail(string compcode, string regioncode, string emailsubject, string emailbody, List<string> attachments)
        {

            mySQLDataMethods.FillCompanyRegionUsers(compcode, regioncode);
            if (mySQLDataMethods.mysqlsearchds.user.Rows.Count > 0)
            {
                System.Net.Mail.MailMessage customermessage = new System.Net.Mail.MailMessage();
                customermessage.To.Clear();
                for (int i = 0; i <= mySQLDataMethods.mysqlsearchds.user.Rows.Count - 1; i++)
                {
                    if (mySQLDataMethods.mysqlsearchds.user[i].emailaddress.TrimEnd() != "")
                    {
                        customermessage.To.Add(mySQLDataMethods.mysqlsearchds.user[i].emailaddress.TrimEnd());
                    }
                }

                if (customermessage.To.Count > 0)
                {
                    if (ConfigurationManager.AppSettings["TestMode"] == "True")
                    {
                        customermessage.To.Clear();
                        customermessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                    }

                    customermessage.Attachments.Clear();
                    foreach (string filename in attachments)
                    {
                        customermessage.Attachments.Add(new System.Net.Mail.Attachment(filename));
                    }
                    appUtilities.SendEmail(customermessage, emailsubject, "See Attached");
                }
            }
        }



    }
}