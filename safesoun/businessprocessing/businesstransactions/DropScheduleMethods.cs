using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using BusinessTransactions;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Data;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BusinessTransactions
{
    public class DropScheduleMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Event");
        ssprocess ssprocessds = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        BusinessReports.BusinessReportsMethods brMethods = new BusinessReports.BusinessReportsMethods();
        public DropScheduleMethods()
            : base("SQL", "SQLConnString")
        {
        }
        public void SendDriverEmails()
        {
            System.Net.Mail.MailMessage drivermessage = new System.Net.Mail.MailMessage();
            bool cont = true;
            string driver = "";
            string commandstring = "";
            string pdffilename = "";
            DateTime selectdate = DateTime.Now;
            if (commonAppDataMethods.GetSingleDate("Enter Drop Schedule Date", 200, 200))
            {
                ssprocessds.driver.Rows.Clear();
                ClearParameters();
                selectdate = commonAppDataMethods.SelectedDate.Date;
                if (wsgUtilities.wsgReply("All Drivers?"))
                {
                    commandstring = "SELECT * FROM driver WHERE emaddr <> ''";
                    FillData(ssprocessds, "driver", commandstring, CommandType.Text);
                    if (ssprocessds.driver.Rows.Count < 1)
                    {
                        wsgUtilities.wsgNotice("There are no drivers with email addresses");
                        cont = false;
                    }

                }
                else
                {
                    driver = commonAppDataMethods.SelectDriver();
                    if (driver.TrimEnd() != "")
                    {
                        AddParms("@driver", driver, "SQL");
                        commandstring = "SELECT * FROM driver WHERE number = @driver";
                        FillData(ssprocessds, "driver", commandstring, CommandType.Text);
                        if (ssprocessds.driver.Rows.Count > 0)
                        {
                            if (ssprocessds.driver[0].emaddr.TrimEnd() == "")
                            {
                                wsgUtilities.wsgNotice("This driver has no email address");
                                cont = false;
                            }
                        }
                    }
                    else
                    {
                        cont = false;
                    }

                }
            }
            else
            {
                cont = false;
            }
            if (cont)
            {
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                for (int r = 0; r < ssprocessds.driver.Rows.Count; r++)
                {
                    pdffilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "dropschedule" + ssprocessds.driver[r].number + String.Format("{0:MMddyy}", selectdate) + ".pdf";
                    brMethods.CreateDriverDropSchedulePDF(ssprocessds.driver[r].number, DateTime.Now, pdffilename);
                    if (File.Exists(pdffilename))
                    {
                        drivermessage.To.Clear();

                        if (ConfigurationManager.AppSettings["TestMode"] == "True")
                        {
                          drivermessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                        }
                        else
                        {
                            drivermessage.To.Add(ssprocessds.driver[r].emaddr);
                        }
                            drivermessage.Attachments.Clear();
                        drivermessage.Attachments.Add(new System.Net.Mail.Attachment(pdffilename));
                        // Send the pdf
                        appUtilities.SendEmail(drivermessage, "Drop Schedule " + String.Format("{0:MM/dd/yy}", selectdate), "See Attached");

                    }
                    else
                    {
                        wsgUtilities.wsgNotice("There has been an error. Get help");
                        cont = false;
                        break;
                    }
                }
                if (cont)
                {
                    wsgUtilities.wsgNotice("Mailing complete");
                }
                frmLoading.Close();
            }

        }
    }
}
