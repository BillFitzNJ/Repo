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
    public class BusinessReportsMethods : WSGDataAccess
    {
        public string ReportName = "";
        public string ReportTitle = "";
        atm atmds = new atm();
        ssprocess ssprocessds = new ssprocess();
        reportsds reportsDs = new reportsds();
        ssprocess ssprocessDs = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        mysqldata mysqldatads = new mysqldata();
        mysqldata mysqlrptds = new mysqldata();
        ssprocess sscloseds = new ssprocess();
        CustomerEmailMethods customerEmailMethods = new CustomerEmailMethods();
        sysdata sysdatads = new sysdata();
        WSGUtilities wsgUtilities = new WSGUtilities("Reports");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        public string reportpath = "";

        public BusinessReportsMethods()
            : base("SQL", "SQLConnString")
        {
            reportpath = ConfigurationManager.AppSettings["ReportPath"];

        }

        public void GetCountingTimeofEntry()
        {
            bool cont = true;
            string selectedstorecode = "";
            DateTime postingdate = new DateTime();
            if (commonAppDataMethods.GetSingleDate("Enter Counting Date", 10000, 10000))
            {
                postingdate = commonAppDataMethods.SelectedDate;

                selectedstorecode = commonAppDataMethods.SelectCompanyAndStore().TrimEnd();
                if (selectedstorecode.TrimEnd() == "")
                {
                    cont = false;
                }
            }
            else
            {
                cont = false;
            }
            if (cont)
            {

                ssprocessds.depdetail.Rows.Clear();
                string commandstring = "SELECT * FROM depdetail  WHERE store = @store and postingdate = @postingdate  ";
                ClearParameters();
                AddParms("@store", selectedstorecode, "SQL");
                AddParms("@postingdate", postingdate, "SQL");

                FillData(ssprocessds, "depdetail", commandstring, CommandType.Text);
                if (ssprocessds.depdetail.Rows.Count > 0)
                {
                    ShowVsReport(reportpath + "VSCountingTime.rdlc", "ssprocessDs", ssprocessds.depdetail, "Counting Time for Store " + selectedstorecode.TrimEnd() + "- Posting date " + string.Format("{0:MM/dd/yy}", postingdate));
                }
                else
                {
                    wsgUtilities.wsgNotice("No matching records");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }


        public void CoinOrderListing()
        {
            string commandtext = "SELECT * from view_expandedcoindrop where dropdate > '2014-03-05' order by drivername, store_name  ";
            ClearParameters();
            ssprocessDs.view_expandedcoindrop.Rows.Clear();
            FillData(ssprocessDs, "view_expandedcoindrop", commandtext, CommandType.Text);
            ShowVsReport(reportpath + "CoinOrderList.rdlc", "ssprocessDs", ssprocessDs.view_expandedcoindrop, "Coin Drop Listing");
        }

        public void SendPickupReportsManually()
        {


            DateTime reportdate = DateTime.Now.Date;
            if (wsgUtilities.wsgReply("Do you want to send Pickup Reports?"))
            {
                bool cont = true;
                if (commonAppDataMethods.GetSingleDate("Report Date", 100, 100))
                {
                    reportdate = commonAppDataMethods.SelectedDate;
                }
                else
                {
                    cont = false;
                }
                if (cont)
                {
                    FrmLoading frmLoading = new FrmLoading();
                    frmLoading.Show();
                    SendPickupReports(reportdate, true);
                    frmLoading.Close();
                    wsgUtilities.wsgNotice("Operation Complete");
                }
                else
                {
                    wsgUtilities.wsgNotice("Operation Cancelled");
                }

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }

        public void BillingChargeCounts()
        {
            string InvoiceNumber = commonAppDataMethods.SelectInvoiceNumber();
            if (InvoiceNumber != "")
            {

                ssprocessds.view_billingchargecount.Rows.Clear();

                string commandstring = "SELECT * FROM view_billingchargecount WHERE inv_number = @inv_number";
                ClearParameters();
                AddParms("@inv_number", InvoiceNumber, "SQL");
                FillData(ssprocessds, "view_billingchargecount", commandstring, CommandType.Text);
                ShowVsReport(reportpath + "VSChargeCounts.rdlc", "ssprocessDs", ssprocessds.view_billingchargecount, "Charge Counts for Invoice " + InvoiceNumber);

            }
            else
            {

                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }

        public void SendPickupReports(DateTime reportdate, bool promptuser)
        {
            // Recipients and contents hard coded until patterns emerge
            List<string> EmailAttachments = new List<string>();
            string pdffilename = "";
            string xlsfilename = "";
            string compcode = "";
            string regioncode = "";
            bool sendreport = true;
            appUtilities.logEvent("Send Activity " + string.Format("{0:MM/dd/yy}", reportdate), "PUActivity", "OK", false);

            if (promptuser)
            {
                sendreport = wsgUtilities.wsgReply("Send Driver Notes?");
            }



            if (sendreport)
            {

                // Driver Notes - Daily
                // Send to Safe and Sound email addresses
                EmailAttachments.Clear();
                pdffilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "drivernotes" + String.Format("{0:MMddyy}", reportdate.Date) + ".pdf";
                CreateActivityList("", "", "", reportdate, reportdate, "Notes");
                if (ssprocessds.view_pickupactivity.Rows.Count > 0)
                {
                    EmailAttachments.Add(pdffilename);
                    MakeVSReportPDF(reportpath + "VSDriverActivity.rdlc", "ssprocessDs", ssprocessds.view_pickupactivity, "Driver Notes For " + string.Format("{0:MM/dd/yy}", reportdate), pdffilename);
                    commonAppDataMethods.SendCompanyEmail(EmailAttachments, "9926", "Driver Notes " + string.Format("{0:MM/dd/yy}", reportdate), "See Attached");

                }
                else
                {
                    commonAppDataMethods.SendCompanyEmail(EmailAttachments, "9926", "Driver Notes " + string.Format("{0:MM/dd/yy}", reportdate), "No activity");

                }

            }

            sendreport = true;
            if (promptuser)
            {
                sendreport = wsgUtilities.wsgReply("Send Daily Pickup Reports?");
            }

            if (sendreport)
            {
                ssprocessds.company.Rows.Clear();
                ClearParameters();
                string commandtext = "SELECT * FROM company where  dailypickupreports = 1";
                FillData(ssprocessds, "company", commandtext, CommandType.Text);
                if (ssprocessds.company.Rows.Count > 0)
                {
                    for (int r = 0; r < ssprocessds.company.Rows.Count; r++)
                    {

                        compcode = ssprocessds.company[r].comp_code;
                        pdffilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "pickupdata" + compcode + String.Format("{0:MMddyy}", reportdate) + ".pdf";
                        CreateActivityList("", "", compcode, reportdate, reportdate, "All");
                        EmailAttachments.Clear();

                        if (ssprocessds.view_pickupactivity.Rows.Count > 0)
                        {
                            EmailAttachments.Add(pdffilename);
                            MakeVSReportPDF(reportpath + "VSDriverActivity.rdlc", "ssprocessDs", ssprocessds.view_pickupactivity, "Deposit Activity For " + string.Format("{0:MM/dd/yy}", reportdate), pdffilename);
                            commonAppDataMethods.SendCompanyEmail(EmailAttachments, compcode, "Deposit Activity " + string.Format("{0:MM/dd/yy}", reportdate), "See Attached");
                        }
                        else
                        {
                            commonAppDataMethods.SendCompanyEmail(EmailAttachments, compcode, "Deposit Activity " + string.Format("{0:MM/dd/yy}", reportdate), "No activity");
                        }
                    }
                }
            }

            // Send Weekly Reports if report date is Saturday
            if (reportdate.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                sendreport = true;

                if (sendreport)
                {

                    ssprocessds.company.Rows.Clear();
                    ClearParameters();
                    string commandtext = "SELECT * FROM company where  weeklypickupreports = 1";
                    FillData(ssprocessds, "company", commandtext, CommandType.Text);
                    if (ssprocessds.company.Rows.Count > 0)
                    {
                        for (int c = 0; c < ssprocessds.company.Rows.Count; c++)
                        {

                            if (promptuser)
                            {
                                if (!wsgUtilities.wsgReply("Send " + ssprocessds.company[c].name.TrimEnd() + "?"))
                                {
                                    continue;
                                }
                            }


                            compcode = ssprocessds.company[c].comp_code;
                            mySQLDataMethods.FillRegion(compcode);
                            ssprocessds.view_pickupactivity.Rows.Clear();
                            if (mySQLDataMethods.mysqlselectords.region.Rows.Count > 0)
                            {
                                for (int r = 0; r < mySQLDataMethods.mysqlselectords.region.Rows.Count; r++)
                                {

                                    regioncode = mySQLDataMethods.mysqlselectords.region[r].regioncode;

                                    xlsfilename = ConfigurationManager.AppSettings["XLSFilesPath"] + "pickupdata" + regioncode + String.Format("{0:MMddyy}", reportdate) + ".xls";

                                    ssprocessds.view_pickupactivity.Rows.Clear();
                                    CreateRegionActivityList(regioncode, compcode, reportdate.AddDays(-6), reportdate);
                                    EmailAttachments.Clear();
                                    if (ssprocessds.view_pickupactivity.Rows.Count > 0)
                                    {
                                        EmailAttachments.Add(xlsfilename);
                                        MakeVSReportXLS(reportpath + "VSDriverActivity.rdlc", "ssprocessDs", ssprocessds.view_pickupactivity, "Deposit Activity Seven Days Ending " + string.Format("{0:MM/dd/yy}", reportdate), xlsfilename);
                                        customerEmailMethods.SendRegionReportEmail(compcode, regioncode, "Deposit Activity " + string.Format("{0:MM/dd/yy}", reportdate), "See Attached", EmailAttachments);

                                    }
                                    else
                                    {
                                        customerEmailMethods.SendRegionReportEmail(compcode, regioncode, "Deposit Activity " + string.Format("{0:MM/dd/yy}", reportdate), "No activity", EmailAttachments);

                                    }
                                }
                            }
                            else
                            {
                                // Send company report if there are no regions
                                xlsfilename = ConfigurationManager.AppSettings["XLSFilesPath"] + "pickupdata" + compcode + String.Format("{0:MMddyy}", reportdate) + ".xls";
                                ssprocessds.view_pickupactivity.Rows.Clear();
                                CreateCompanyActivityList(compcode, reportdate.AddDays(-6), reportdate);
                                EmailAttachments.Clear();
                                if (ssprocessds.view_pickupactivity.Rows.Count > 0)
                                {
                                    EmailAttachments.Add(xlsfilename);
                                    MakeVSReportXLS(reportpath + "VSDriverActivity.rdlc", "ssprocessDs", ssprocessds.view_pickupactivity, "Deposit Activity Seven Days Ending " + string.Format("{0:MM/dd/yy}", reportdate), xlsfilename);
                                    customerEmailMethods.SendCustomerReportEmail(compcode, "Deposit Activity " + string.Format("{0:MM/dd/yy}", reportdate), "See Attached", EmailAttachments);

                                }
                                else
                                {
                                    customerEmailMethods.SendCustomerReportEmail(compcode, "Deposit Activity " + string.Format("{0:MM/dd/yy}", reportdate), "No activity", EmailAttachments);

                                }

                            }
                        }
                    }
                }
            } // Weekly reports

        }


        public void CompanyDropSchedule(string ReportLevel)
        {

            string compcode = commonAppDataMethods.SelectCompany();
            string commandtext = "";
            if (compcode.TrimEnd() != "")
            {
                ssprocessds.view_expandeddropschedule.Rows.Clear();
                if (ReportLevel == "Detail")
                {
                    commandtext = "SELECT * from view_expandeddropschedule where active = 1 AND LEFT(storecode,4) = @compcode order by storecode  ";
                }
                else
                {
                    commandtext = "select distinct storecode, storelocation,drivername, space(15) AS decrip from view_expandeddropschedule where active = 1 AND LEFT(storecode,4) = @compcode order by storecode ";
                }
                ClearParameters();
                AddParms("@compcode", compcode, "SQL");
                FillData(ssprocessds, "view_expandeddropschedule", commandtext, CommandType.Text);
                if (ssprocessds.view_expandeddropschedule.Rows.Count > 0)
                {
                    ShowVsReport(reportpath + "VSCompanyDropSchedule.rdlc", "ssprocessDs", ssprocessds.view_expandeddropschedule, "Company Drop Schedule for " + commonAppDataMethods.GetCompanyName(compcode));
                }
                else
                {
                    wsgUtilities.wsgNotice("No matching records");
                }


            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }


        public void DriverActivityList(string scope)
        {

            FrmReportDataSelector frmDriverActivitySelector = new FrmReportDataSelector();
            FrmLoading frmLoading = new FrmLoading();
            frmDriverActivitySelector.Text = "Pickup Activity -";
            frmDriverActivitySelector.ShowDialog();
            ReportTitle = "Safe and Sound Armed Courier Activity Analysis ";
            if (frmDriverActivitySelector.cont)
            {
                if (frmDriverActivitySelector.driver.TrimEnd() != "")
                {
                    ReportTitle += " - " + commonAppDataMethods.GetDriverName(frmDriverActivitySelector.driver).TrimEnd();
                }

                if (frmDriverActivitySelector.storecode.TrimEnd() != "")
                {
                    ReportTitle += " - " + commonAppDataMethods.GetStoreName(frmDriverActivitySelector.storecode).TrimEnd();
                }
                else
                {
                    if (frmDriverActivitySelector.compcode.TrimEnd() != "")
                    {
                        ReportTitle += " - " + commonAppDataMethods.GetCompanyName(frmDriverActivitySelector.compcode).TrimEnd();
                    }

                }


                ReportTitle += " - " + String.Format("{0:MM/dd/yy}", frmDriverActivitySelector.startdate) + " thru " + String.Format("{0:MM/dd/yy}", frmDriverActivitySelector.enddate);

                frmLoading.Show();
                CreateActivityList(frmDriverActivitySelector.driver, frmDriverActivitySelector.storecode, frmDriverActivitySelector.compcode, frmDriverActivitySelector.startdate, frmDriverActivitySelector.enddate, scope);
                frmLoading.Hide();

                if (ssprocessds.view_pickupactivity.Rows.Count > 0)
                {
                    ShowVsReport(reportpath + "VSDriverActivity.rdlc", "ssprocessDs", ssprocessds.view_pickupactivity, ReportTitle);

                }
                else
                {
                    wsgUtilities.wsgNotice("No matching records");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }


        public void CreateCompanyActivityList(string compcode, DateTime startdate, DateTime enddate)
        {
            ssprocessds.view_pickupactivity.Rows.Clear();

            ClearParameters();
            string commandtext = "SELECT * FROM view_pickupactivity WHERE pickupdate >= @startdate AND pickupdate <= @enddate  AND LEFT(storecode,4) = @compcode ORDER BY storenameandaddress, pickupdate";
            AddParms("@startdate", startdate, "SQL");
            AddParms("@enddate", enddate, "SQL");
            AddParms("@compcode", compcode, "SQL");
            FillData(ssprocessds, "view_pickupactivity", commandtext, CommandType.Text);
        }

        public void CreateRegionActivityList(string regioncode, string compcode, DateTime startdate, DateTime enddate)
        {
            mySQLDataMethods.FillStoreRegion(compcode, regioncode);
            ssprocessds.view_pickupactivity.Rows.Clear();

            for (int r = 0; r < mySQLDataMethods.mysqlselectords.storeregion.Rows.Count; r++)
            {
                ClearParameters();
                string commandtext = "SELECT * FROM view_pickupactivity WHERE pickupdate >= @startdate AND pickupdate <= @enddate  AND storecode = @storecode ORDER BY storenameandaddress, pickupdate";
                AddParms("@startdate", startdate, "SQL");
                AddParms("@enddate", enddate, "SQL");
                AddParms("@storecode", mySQLDataMethods.mysqlselectords.storeregion[r].storecode, "SQL");
                FillData(ssprocessds, "view_pickupactivity", commandtext, CommandType.Text);
            }
        }





        public void CreateActivityList(string driver, string storecode, string compcode, DateTime startdate, DateTime enddate, string scope)
        {
            ssprocessds.view_pickupactivity.Rows.Clear();
            ClearParameters();
            string commandtext = "SELECT * FROM view_pickupactivity WHERE pickupdate >= @startdate AND pickupdate <= @enddate ";
            AddParms("@startdate", startdate, "SQL");
            AddParms("@enddate", enddate, "SQL");

            if (scope != "All")
            {
                commandtext += " AND RTRIM(notes) <> ''";
            }


            if (driver.TrimEnd() != "")
            {
                AddParms("@driver", driver, "SQL");
                commandtext += " AND driver = @driver";

            }

            if (storecode.TrimEnd() != "")
            {
                AddParms("@storecode", storecode, "SQL");
                commandtext += " AND storecode = @storecode";
            }

            else
            {
                if (compcode.TrimEnd() != "")
                {
                    AddParms("@compcode", compcode, "SQL");
                    commandtext += " AND LEFT(storecode,4) = @compcode";

                }
            }
            commandtext += " ORDER BY storenameandaddress, pickupdate";
            ssprocessds.view_pickupactivity.Rows.Clear();
            FillData(ssprocessds, "view_pickupactivity", commandtext, CommandType.Text);
        }


        public void CurrencyDropsListing()
        {
            DateTime startdate = Convert.ToDateTime("01/01/01");
            DateTime enddate = Convert.ToDateTime("01/01/2999");

            FrmReportDataSelector frmCurrencyDropSelector = new FrmReportDataSelector();
            frmCurrencyDropSelector.Text = "Currency Drop Selector";
            frmCurrencyDropSelector.ShowDialog();
            if (frmCurrencyDropSelector.labelDates.Text != "All")
            {
                startdate = frmCurrencyDropSelector.startdate;
                enddate = frmCurrencyDropSelector.enddate;
            }

            if (frmCurrencyDropSelector.cont)
            {
                string pdffilename = "";
                DateTime scheduledate = DateTime.Now;
                pdffilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "dropschedule" + String.Format("{0:MMddyy}", scheduledate) + ".pdf";
                CreateCurrencyDropListing(frmCurrencyDropSelector.driver, frmCurrencyDropSelector.compcode, frmCurrencyDropSelector.storecode, startdate, enddate);

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }

        public void DriverSlipList()
        {
            FrmReportDataSelector frmDropScheduleSelector = new FrmReportDataSelector();
            frmDropScheduleSelector.Text = "Slip List Selector";
            frmDropScheduleSelector.buttonCompany.Visible = false;
            frmDropScheduleSelector.buttonStore.Visible = false;
            frmDropScheduleSelector.ShowDialog();
            if (frmDropScheduleSelector.cont)
            {
                string pdffilename = "";
                DateTime scheduledate = DateTime.Now;
                pdffilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "dropschedule" + String.Format("{0:MMddyy}", scheduledate) + ".pdf";
                CreateDriverSlipsList(frmDropScheduleSelector.driver, frmDropScheduleSelector.enddate);

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }


        public void DriverDropSchedule()
        {
            FrmReportDataSelector frmDropScheduleSelector = new FrmReportDataSelector();
            frmDropScheduleSelector.Text = "Drop Schedule Selector";
            frmDropScheduleSelector.buttonDates.Visible = false;
            frmDropScheduleSelector.labelDates.Visible = false;
            frmDropScheduleSelector.ShowDialog();
            if (frmDropScheduleSelector.cont)
            {
                string pdffilename = "";
                DateTime scheduledate = DateTime.Now;
                pdffilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "dropschedule" + String.Format("{0:MMddyy}", scheduledate) + ".pdf";
                CreateDropScheduleReport(frmDropScheduleSelector.driver, frmDropScheduleSelector.compcode, frmDropScheduleSelector.storecode);

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }
        public void CompanyRateData()
        {
            string CommandString = "";
            if (wsgUtilities.wsgReply("Include ATM Owners"))
            {
                CommandString = "SELECT * FROM company WHERE atmowner = 1 ORDER BY comp_code";
            }
            else
            {
                CommandString = "SELECT * FROM company WHERE atmowner = 0 ORDER BY comp_code";

            }

            FrmLoading frmLoading = new FrmLoading();
            frmLoading.Show();
            ReportTitle = "Company Rate Data";
            ssprocessds.company.Rows.Clear();
            ClearParameters();
            FillData(ssprocessds, "company", CommandString, CommandType.Text);
            frmLoading.Hide();
            ShowVsReport(reportpath + "VSCompanyRateData.rdlc", "ssprocessDs", ssprocessds.company, ReportTitle);


        }
        public void CreateDriverDropSchedulePDF(string driver, DateTime selectdate, string filename)
        {
            ssprocessds.view_expandeddropschedule.Rows.Clear();
            string commandtext = "SELECT * from view_expandeddropschedule WHERE driver_1 = @driver AND  active = 1 AND DATEPART(dw,@selectdate) = daynumber ORDER by storelocation ";
            ClearParameters();
            AddParms("@driver", driver, "SQL");
            AddParms("@selectdate", selectdate, "SQL");

            FillData(ssprocessds, "view_expandeddropschedule", commandtext, CommandType.Text);
            MakeVSReportPDF(reportpath + "VSDropList.rdlc", "ssprocessDs", ssprocessds.view_expandeddropschedule, "Drop Schedule For " + commonAppDataMethods.GetDriverName(driver).TrimEnd() + " - " + selectdate.DayOfWeek + ", " + String.Format("{0:MM/dd/yy}", selectdate), filename);


        }

        public string CreateBillingSpreadsheet(string Inv_Number)
        {
            string outputfilename = "";
            ssprocessds.view_ExpandedBillingStoreSummary.Clear();
            string commandtext = "SELECT * from view_ExpandedBillingStoreSummary WHERE inv_number = @inv_number ORDER BY storecode, bill_type ";
            string logofilename = Environment.CurrentDirectory + "\\" + "SNSLOGO.bmp";
            string couriername = "Safe And Sound Armed Courier - PO Box 1463   Bayville, NY 11709-0463";
            ClearParameters();
            AddParms("@inv_number", Inv_Number, "SQL");
            FillData(ssprocessds, "view_ExpandedBillingStoreSummary", commandtext, CommandType.Text);
            if (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count > 0)
            {
                outputfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "Invoice " + ssprocessds.view_ExpandedBillingStoreSummary[0].inv_number + String.Format("{0:MMddyy}", ssprocessds.view_ExpandedBillingStoreSummary[0].inv_date).Substring(0, 2);
                outputfilename += String.Format("{0:MMddyy}", ssprocessds.view_ExpandedBillingStoreSummary[0].inv_date).Substring(4, 2) + ".xlsx";

                string sheettitle = commonAppDataMethods.GetCompanyName(ssprocessds.view_ExpandedBillingStoreSummary[0].storecode.Substring(0, 4)) + " Invoice # " + ssprocessds.view_ExpandedBillingStoreSummary[0].inv_number + "  " + String.Format("{0:MMddyy}", ssprocessds.view_ExpandedBillingStoreSummary[0].inv_date);
                object misValue = System.Reflection.Missing.Value;
                Excel.Application objApp = new Excel.Application();
                Excel.Workbooks objBooks = objApp.Workbooks;
                Excel.Workbook objBook = objBooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = objBook.Worksheets.get_Item(1);
                xlWorkSheet.Name = "Invoice";
                xlWorkSheet.Cells[5, 1] = "Code";
                xlWorkSheet.Cells[5, 2] = "Name";
                xlWorkSheet.Cells[5, 3] = "Address";
                xlWorkSheet.Cells[5, 4] = "Service";
                xlWorkSheet.Cells[5, 5] = "Volume";
                xlWorkSheet.Cells[5, 6] = "Rate";
                xlWorkSheet.Cells[5, 7] = "Extension";
                xlWorkSheet.Range["A5:G5"].HorizontalAlignment = HorizontalAlignment.Center;
                xlWorkSheet.Range["A5:G5"].Borders.Weight = BorderStyle.FixedSingle;

                object[,] arr = new object[ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count, 7];

                for (int r = 0; r < ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count; r++)
                {
                    arr[r, 0] = ssprocessds.view_ExpandedBillingStoreSummary[r].storecode;
                    arr[r, 1] = ssprocessds.view_ExpandedBillingStoreSummary[r].store_name;
                    arr[r, 2] = ssprocessds.view_ExpandedBillingStoreSummary[r].f_address;
                    arr[r, 3] = ssprocessds.view_ExpandedBillingStoreSummary[r].bill_type;
                    arr[r, 4] = ssprocessds.view_ExpandedBillingStoreSummary[r].volume;
                    arr[r, 5] = ssprocessds.view_ExpandedBillingStoreSummary[r].rate;
                    arr[r, 6] = ssprocessds.view_ExpandedBillingStoreSummary[r].charged;

                }
                Excel.Range c1 = (Excel.Range)xlWorkSheet.Cells[6, 1];
                Excel.Range c2 = (Excel.Range)xlWorkSheet.Cells[ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count + 5, 7];
                Excel.Range range = xlWorkSheet.get_Range(c1, c2);
                range.Columns.AutoFit();
                range.Value = arr;

                xlWorkSheet.Range["A" + (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Value = "Total";
                xlWorkSheet.Range["A" + (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Font.Bold = true;
                xlWorkSheet.Range["G" + (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Formula =
                    "=SUM(G6:G" + (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count + 5).ToString().TrimStart().TrimEnd() + ")";
                xlWorkSheet.Range["G" + (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Font.Bold = true;
                xlWorkSheet.Range["G" + (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count + 9).ToString().TrimStart().TrimEnd()].NumberFormat = "###,###,###.00";
                xlWorkSheet.Shapes.AddPicture(logofilename, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, 400, 5, 242, 80);
                xlWorkSheet.Cells[1, 1] = couriername;
                xlWorkSheet.Cells[3, 1] = sheettitle;
                xlWorkSheet.Range["A1:A3"].Font.Bold = true;
                xlWorkSheet.Range["A1:A3"].Font.Size = 14;

                if (File.Exists(outputfilename))
                {
                    File.Delete(outputfilename);
                }
                objBook.SaveAs(outputfilename, Excel.XlFileFormat.xlOpenXMLWorkbook,
                misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                objBook.Close(true, misValue, misValue);
                objApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(objBook);
                releaseObject(objApp);
            }
            return outputfilename;
        }

        public string CreateStarbucksBillingSpreadsheet(string Inv_Number, int filenumber)
        {
            string textfilename = "";
            ssprocessds.view_BillingStoreBillTypeSummary.Clear();
            string commandtext = "SELECT * from view_BillingStoreBillTypeSummary WHERE inv_number = @inv_number ORDER BY storecode, bill_type ";
            string logofilename = Environment.CurrentDirectory + "\\" + "SNSLOGO.bmp";
            string couriername = "Safe And Sound Armed Courier - PO Box 1463   Bayville, NY 11709-0463";
            ClearParameters();
            AddParms("@inv_number", Inv_Number, "SQL");
            FillData(ssprocessds, "view_BillingStoreBillTypeSummary", commandtext, CommandType.Text);
            if (ssprocessds.view_BillingStoreBillTypeSummary.Rows.Count > 0)
            {



                decimal invoiceamount = 0;
                // Develop the invoice total
                for (int i = 0; i <= ssprocessds.view_BillingStoreBillTypeSummary.Rows.Count - 1; i++)
                {
                    if (filenumber == 1 && (ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) == "054913" || ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) == "057611"))
                    {
                        continue;
                    }
                    if (filenumber == 2 && ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) != "054913" && ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) != "057611")
                    {
                        continue;
                    }


                    invoiceamount += ssprocessds.view_BillingStoreBillTypeSummary[i].charged;
                }
                // Start the text file
                string servicedescription = "ARMOURED CAR";

                if (filenumber == 1)
                {
                    textfilename = ConfigurationManager.AppSettings["FTPFilepath"] +
                    "SAFE" + ssprocessds.view_BillingStoreBillTypeSummary[0].inv_date.ToString("MMddyy") + "u.txt";
                }
                else
                {
                    textfilename = ConfigurationManager.AppSettings["FTPFilepath"] +
          "SAFE" + ssprocessds.view_BillingStoreBillTypeSummary[0].inv_date.ToString("MMddyy") + "SR.txt";

                }
                TextWriter Textout = new StreamWriter(textfilename);
                string legalentity = "";
                string textline = "";

                if (filenumber == 1)
                {
                    legalentity = "100";
                }
                else
                {
                    legalentity = "160";
                }
                // Header
                // Constant H
                textline = "H^";
                // Supplier number
                textline += "932703";
                // Pipe Character
                textline += "|";
                // Legal Entity
                textline += legalentity;
                // Caret
                textline += "^";
                // Supplier Site Code
                textline += "BAYVILLE01";
                // Caret
                textline += "^";
                // Invoice Number
                textline += ssprocessds.view_BillingStoreBillTypeSummary[0].inv_number.TrimEnd();
                // Caret
                textline += "^";
                //Invoice Date
                textline += String.Format("{0:yyyyMMdd}", ssprocessds.view_BillingStoreBillTypeSummary[0].inv_date);
                // Caret
                textline += "^";
                // Invoice Amount
                textline += invoiceamount.ToString("F2");
                // Caret
                textline += "^";
                textline += servicedescription;
                // Caret
                textline += "^";
                Textout.WriteLine(textline);
                // Detail lines
                for (int i = 0; i <= ssprocessds.view_BillingStoreBillTypeSummary.Rows.Count - 1; i++)
                {
                    if (filenumber == 1 && (ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) == "054913" || ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) == "057611"))
                    {
                        continue;
                    }
                    if (filenumber == 2 && ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) != "054913" && ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) != "057611")
                    {
                        continue;
                    }


                    if (ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) == "017505" | ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) == "022596")
                    {
                        legalentity = "142";
                    }
                    else
                    {
                        legalentity = "100";

                    }

                    if (ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) == "054913" || ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) == "057611")
                    {
                        legalentity = "160";

                    }

                    // Constant D
                    textline = "D^";
                    // Supplier number
                    textline += "932703";
                    // Caret
                    textline += "^";
                    // Supplier Site Code
                    textline += "BAYVILLE01";
                    // Caret
                    textline += "^";
                    // Invoice Number
                    textline += ssprocessds.view_BillingStoreBillTypeSummary[i].inv_number.TrimEnd();
                    // Caret
                    textline += "^";
                    //Invoice Date
                    textline += String.Format("{0:yyyyMMdd}", ssprocessds.view_BillingStoreBillTypeSummary[i].inv_date);
                    // Caret
                    textline += "^";
                    // Account - Legal Entity + right 6 postions of store code + 661095.000.0000.
                    textline += legalentity + "." + ssprocessds.view_BillingStoreBillTypeSummary[i].storecode.Substring(5, 6) + ".661095.000.0000";
                    // Caret
                    textline += "^";
                    // Amount
                    textline += ssprocessds.view_BillingStoreBillTypeSummary[i].charged.ToString("F2");
                    // Caret
                    textline += "^";
                    textline += ssprocessds.view_BillingStoreBillTypeSummary[i].bill_type.TrimEnd();
                    // Caret
                    textline += "^";
                    if (ssprocessds.view_BillingStoreBillTypeSummary[i].bill_type.TrimEnd() == "TAX")
                    {
                        textline += "VDR S-TAX";
                    }
                    // Caret
                    textline += "^";
                    // Service 
                    textline += "ARMOURED CAR";
                    // Caret
                    textline += "^";
                    Textout.WriteLine(textline);
                }
                // Batch Record
                // Constant B
                textline = "B";
                // Caret
                textline += "^";
                // Constant 1
                textline += "1";
                // Caret
                textline += "^";
                // Number of detail lines
                textline += ssprocessds.view_BillingStoreBillTypeSummary.Rows.Count.ToString("F0");
                // Caret
                textline += "^";
                // Invoice Amount
                textline += invoiceamount.ToString("F2");
                Textout.WriteLine(textline);
                Textout.Close();
            }
            else
            {

            }
            return textfilename;
        }

        public string CreateFamilyDollarBillingSpreadSheet(int month, int year, int x)
        {
            string commandtext = "SELECT * FROM view_familydollarbillingsummary WHERE  MONTH(inv_date) = @rptmonth AND YEAR(inv_date) = @rptyear";
            string textfilename = "";
            string servicedescription = "";
            if (x == 1)
            {
                commandtext += " AND bill_group = '1' ";
            }
            else
            {
                commandtext += " AND bill_group =  '3' ";

            }
            ssprocessds.view_familydollarbillingsummary.Rows.Clear();
            commandtext += " ORDER BY storecode  ";
            ClearParameters();
            AddParms("@rptmonth", month, "SQL");
            AddParms("@rptyear", year, "SQL");
            FillData(ssprocessds, "view_familydollarbillingsummary", commandtext, CommandType.Text);

            decimal invoiceamount = 0;
            if (ssprocessds.view_familydollarbillingsummary.Rows.Count > 1)
            {
                for (int i = 0; i <= ssprocessds.view_familydollarbillingsummary.Rows.Count - 1; i++)
                {
                    invoiceamount += ssprocessds.view_familydollarbillingsummary[i].charged + ssprocessds.view_familydollarbillingsummary[i].tax;
                }
            }

            // Start the text file
            if (x == 1)
            {
                textfilename = ConfigurationManager.AppSettings["FTPFilepath"] +
               "SAFE AND SOUND " + ssprocessds.view_familydollarbillingsummary[0].inv_date.ToString("MMMM") + " Service " + ssprocessds.view_familydollarbillingsummary[0].inv_number + ".csv";
            }
            else
            {
                textfilename = ConfigurationManager.AppSettings["FTPFilepath"] +
               "SAFE AND SOUND " + ssprocessds.view_familydollarbillingsummary[0].inv_date.ToString("MMMM") + " EE " + ssprocessds.view_familydollarbillingsummary[0].inv_number + ".csv";
            }
            TextWriter Textout = new StreamWriter(textfilename);
            string distributiondatestring = ssprocessds.view_familydollarbillingsummary[0].inv_date.Month.ToString() + "/15/" + ssprocessds.view_familydollarbillingsummary[0].inv_date.Year.ToString();
            DateTime DistributionDate = DateTime.Parse(distributiondatestring);

            string textline = "Co,Vendor ,Summary Invoice Number,Invoice Date, Due Date,Distribution Date,Service Type Description,Total Amount,Distribution Company, Acct Unit, Account,Sub Acct, ";
            textline += "Service Total, Tax, Activity, Category Code, Commodity, Purchasing Category, Source";
            Textout.WriteLine(textline);

            // Detail lines
            for (int i = 0; i <= ssprocessds.view_familydollarbillingsummary.Rows.Count - 1; i++)
            {
                if (x == 1)
                {
                    servicedescription = "Safe and Sound Service " + ssprocessds.view_familydollarbillingsummary[i].inv_date.ToString("MMMM");
                }
                else
                {
                    servicedescription = "Safe and Sound EE " + ssprocessds.view_familydollarbillingsummary[i].inv_date.ToString("MMMM");
                }
                // Constant 212 and 76804
                textline = "212,";
                // Supplier number
                textline += "76084,";
                // Invoice Number
                textline += ssprocessds.view_familydollarbillingsummary[i].inv_number.TrimEnd().TrimStart();
                // Comma
                textline += ",";
                //Invoice Date
                textline += String.Format("{0:MM/dd/yyyy}", ssprocessds.view_familydollarbillingsummary[i].inv_date).TrimStart();
                // Comma
                textline += ",";
                // Due Date 
                textline += String.Format("{0:MM/dd/yyyy}", ssprocessds.view_familydollarbillingsummary[i].inv_date.AddMonths(1)).TrimStart();
                // Comma
                textline += ",";
                // Distribution Date
                textline += String.Format("{0:MM/dd/yyyy}", DistributionDate);
                // Comma
                textline += ",";
                // Service Type
                textline += servicedescription;
                // Comma
                textline += ",";
                // Invoice Amount
                textline += invoiceamount.ToString("F2");
                // Comma
                textline += ",";
                // Distribution company- constant 263 for NY
                textline += "263";
                // Comma
                textline += ",";
                // Accounting Unit - storecode + 20,000     
                textline += (Convert.ToInt32(ssprocessds.view_familydollarbillingsummary[i].storecode.Substring(5, 6)) + 20000).ToString();
                // Comma
                textline += ",";
                //  Account - constant 6750
                textline += "6750";
                // Comma
                textline += ",";
                // Sub Account - constant 6
                textline += "6";
                // Comma
                textline += ",";
                // Pretax  Amount
                textline += ssprocessds.view_familydollarbillingsummary[i].charged.ToString("F2");
                // Comma
                textline += ",";
                // Tax  Amount
                textline += ssprocessds.view_familydollarbillingsummary[i].tax.ToString("F2");
                // Comma
                textline += ",";
                //Unused Columns
                textline += ", , , , ";

                Textout.WriteLine(textline);
            }
            Textout.Close();

            return textfilename;

        }

        public string CreateTMobileBillingSpreadsheet(string Inv_Number)
        {
            string outputfilename = "";
            ssprocessds.view_tmobilebilling.Clear();
            string commandtext = "SELECT * from view_tmobilebilling WHERE inv_number = @inv_number ORDER BY storecode ";
            string logofilename = Environment.CurrentDirectory + "\\" + "SNSLOGO.bmp";
            string couriername = "Safe And Sound Armed Courier - PO Box 1463   Bayville, NY 11709-0463";
            ClearParameters();
            AddParms("@inv_number", Inv_Number, "SQL");
            FillData(ssprocessds, "view_TMobileBilling", commandtext, CommandType.Text);
            if (ssprocessds.view_tmobilebilling.Rows.Count > 0)
            {
                outputfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "Invoice " + ssprocessds.view_tmobilebilling[0].inv_number + String.Format("{0:MMddyy}", ssprocessds.view_tmobilebilling[0].inv_date).Substring(0, 2);
                outputfilename += String.Format("{0:MMddyy}", ssprocessds.view_tmobilebilling[0].inv_date).Substring(4, 2) + ".xlsx";

                string sheettitle = commonAppDataMethods.GetCompanyName(ssprocessds.view_tmobilebilling[0].storecode.Substring(0, 4)) + " Invoice # " + ssprocessds.view_tmobilebilling[0].inv_number + "  " + String.Format("{0:MMddyy}", ssprocessds.view_tmobilebilling[0].inv_date);
                object misValue = System.Reflection.Missing.Value;
                Excel.Application objApp = new Excel.Application();
                Excel.Workbooks objBooks = objApp.Workbooks;
                Excel.Workbook objBook = objBooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = objBook.Worksheets.get_Item(1);
                xlWorkSheet.Name = "Invoice";
                xlWorkSheet.Cells[5, 1] = "Name";
                xlWorkSheet.Cells[5, 2] = "Code";
                xlWorkSheet.Cells[5, 3] = "Address";
                xlWorkSheet.Cells[5, 4] = "Charged";
                xlWorkSheet.Cells[5, 5] = "Tax";
                xlWorkSheet.Cells[5, 6] = "Total";
                xlWorkSheet.Cells[5, 7] = "Su";
                xlWorkSheet.Cells[5, 8] = "M";
                xlWorkSheet.Cells[5, 9] = "Tu";
                xlWorkSheet.Cells[5, 10] = "W";
                xlWorkSheet.Cells[5, 11] = "Th";
                xlWorkSheet.Cells[5, 12] = "F";
                xlWorkSheet.Cells[5, 13] = "Sa";


                xlWorkSheet.Range["A5:L5"].HorizontalAlignment = HorizontalAlignment.Center;
                xlWorkSheet.Range["A5:L5"].Borders.Weight = BorderStyle.FixedSingle;

                object[,] arr = new object[ssprocessds.view_tmobilebilling.Rows.Count, 14];

                for (int r = 0; r < ssprocessds.view_tmobilebilling.Rows.Count; r++)
                {
                    arr[r, 0] = ssprocessds.view_tmobilebilling[r].store_name;
                    arr[r, 1] = ssprocessds.view_tmobilebilling[r].code;
                    arr[r, 2] = ssprocessds.view_tmobilebilling[r].f_address;
                    arr[r, 3] = ssprocessds.view_tmobilebilling[r].charged;
                    arr[r, 4] = ssprocessds.view_tmobilebilling[r].tax;
                    arr[r, 5] = ssprocessds.view_tmobilebilling[r].charged + ssprocessds.view_tmobilebilling[r].tax;
                    arr[r, 6] = ssprocessds.view_tmobilebilling[r].su;
                    arr[r, 7] = ssprocessds.view_tmobilebilling[r].m;
                    arr[r, 8] = ssprocessds.view_tmobilebilling[r].tu;
                    arr[r, 9] = ssprocessds.view_tmobilebilling[r].w;
                    arr[r, 10] = ssprocessds.view_tmobilebilling[r].th;
                    arr[r, 11] = ssprocessds.view_tmobilebilling[r].f;
                    arr[r, 12] = ssprocessds.view_tmobilebilling[r].sa;

                }
                Excel.Range c1 = (Excel.Range)xlWorkSheet.Cells[6, 1];
                Excel.Range c2 = (Excel.Range)xlWorkSheet.Cells[ssprocessds.view_tmobilebilling.Rows.Count + 5, 13];
                Excel.Range range = xlWorkSheet.get_Range(c1, c2);
                range.Columns.AutoFit();
                range.Value = arr;

                xlWorkSheet.Range["A" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Value = "Total";
                xlWorkSheet.Range["A" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Font.Bold = true;

                // Total charge line
                xlWorkSheet.Range["D" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Formula =
                    "=SUM(D6:D" + (ssprocessds.view_tmobilebilling.Rows.Count + 5).ToString().TrimStart().TrimEnd() + ")";
                xlWorkSheet.Range["D" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Font.Bold = true;

                //Code column formatting
                Excel.Range code = (Excel.Range)xlWorkSheet.Cells[6, 2];
                Excel.Range code2 = (Excel.Range)xlWorkSheet.Cells[ssprocessds.view_tmobilebilling.Rows.Count + 5, 4];
                Excel.Range coderange = xlWorkSheet.get_Range(code, code2);
                coderange.NumberFormat = "####";


                // Amount column formatting
                xlWorkSheet.Range["D" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].NumberFormat = "###,###,###.00";
                Excel.Range charge = (Excel.Range)xlWorkSheet.Cells[6, 4];
                Excel.Range charge2 = (Excel.Range)xlWorkSheet.Cells[ssprocessds.view_tmobilebilling.Rows.Count + 5, 4];
                Excel.Range chargerange = xlWorkSheet.get_Range(charge, charge2);
                chargerange.NumberFormat = "###,###,###.00";


                //Total Tax line
                xlWorkSheet.Range["E" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Formula =
               "=SUM(E6:E" + (ssprocessds.view_tmobilebilling.Rows.Count + 5).ToString().TrimStart().TrimEnd() + ")";
                xlWorkSheet.Range["E" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Font.Bold = true;
                xlWorkSheet.Range["E" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].NumberFormat = "###,###,###.00";
                //Total  Total line
                xlWorkSheet.Range["F" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Formula =
              "=SUM(F6:F" + (ssprocessds.view_tmobilebilling.Rows.Count + 5).ToString().TrimStart().TrimEnd() + ")";
                xlWorkSheet.Range["F" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].Font.Bold = true;
                xlWorkSheet.Range["F" + (ssprocessds.view_tmobilebilling.Rows.Count + 9).ToString().TrimStart().TrimEnd()].NumberFormat = "###,###,###.00";



                xlWorkSheet.Shapes.AddPicture(logofilename, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, 650, 5, 242, 80);
                xlWorkSheet.Cells[1, 1] = couriername;
                xlWorkSheet.Cells[3, 1] = sheettitle;
                xlWorkSheet.Range["A1:A3"].Font.Bold = true;
                xlWorkSheet.Range["A1:A3"].Font.Size = 14;

                if (File.Exists(outputfilename))
                {
                    File.Delete(outputfilename);
                }
                objBook.SaveAs(outputfilename, Excel.XlFileFormat.xlOpenXMLWorkbook,
                misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                objBook.Close(true, misValue, misValue);
                objApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(objBook);
                releaseObject(objApp);
            }
            return outputfilename;
        }



        public void CreateSignatureBillingSpreadsheet(ssprocess.view_SignatureBillSummaryDataTable billingdatatable, string FileName, DateTime billingdate)
        {
            string effectivedate = String.Format("{0:MMddyyyy}", billingdate).Substring(0, 2) + "01" + String.Format("{0:MMddyyyy}", billingdate).Substring(4, 4);
            object misValue = System.Reflection.Missing.Value;
            Excel.Application objApp = new Excel.Application();
            Excel.Workbooks objBooks = objApp.Workbooks;
            Excel.Workbook objBook = objBooks.Add(misValue);
            Excel.Worksheet xlWorkSheet = objBook.Worksheets.get_Item(1);
            xlWorkSheet.Cells[1, 1] = "fld_Bank_Number";
            xlWorkSheet.Cells[1, 2] = "fld_Account_Number";
            xlWorkSheet.Cells[1, 3] = "fld_Effective_Date";
            xlWorkSheet.Cells[1, 4] = "fld_Source";
            xlWorkSheet.Cells[1, 5] = "fld_Service_Subtype";
            xlWorkSheet.Cells[1, 6] = "fld_Service_Code";
            xlWorkSheet.Cells[1, 7] = "fld_Volume";
            xlWorkSheet.Cells[1, 8] = "fld_Addendum";
            xlWorkSheet.Cells[1, 9] = "fld_Total_Fee";
            xlWorkSheet.Cells[1, 10] = "fld_Total_Cost";
            xlWorkSheet.Cells[1, 11] = "fld_Fee_Disposition";
            xlWorkSheet.Cells[1, 12] = "fld_Description";
            object[,] arr = new object[billingdatatable.Rows.Count, 12];
            for (int r = 0; r < billingdatatable.Rows.Count; r++)
            {
                arr[r, 0] = "199";
                arr[r, 1] = billingdatatable[r].account.TrimStart().TrimEnd();
                arr[r, 2] = effectivedate;
                arr[r, 3] = "004";
                if (billingdatatable[r].servicecode.TrimEnd() == "10011")
                {
                    arr[r, 4] = "";
                    arr[r, 8] = 0;
                }
                else
                {
                    arr[r, 4] = "P";
                    arr[r, 8] = billingdatatable[r].charged;
                }
                arr[r, 5] = billingdatatable[r].servicecode;
                arr[r, 6] = billingdatatable[r].chargecount;
                arr[r, 7] = "";
                arr[r, 9] = "";
                arr[r, 10] = "A";
                arr[r, 11] = "";
            }
            // Import the array
            Excel.Range c1 = (Excel.Range)xlWorkSheet.Cells[2, 1];
            Excel.Range c2 = (Excel.Range)xlWorkSheet.Cells[billingdatatable.Count + 1, 12];
            Excel.Range range = xlWorkSheet.get_Range(c1, c2);
            range.Columns.AutoFit();
            Excel.Range chargedrange = (Excel.Range)xlWorkSheet.get_Range("I2", "I" + billingdatatable.Count + 1.ToString().TrimEnd().TrimStart());
            chargedrange.NumberFormat = "#########.00";
            Excel.Range daterange = (Excel.Range)xlWorkSheet.get_Range("C2", "C" + billingdatatable.Count + 1.ToString().TrimEnd().TrimStart());
            daterange.NumberFormat = "00000000";
            Excel.Range sourcerange = (Excel.Range)xlWorkSheet.get_Range("D2", "D" + billingdatatable.Count + 1.ToString().TrimEnd().TrimStart());
            sourcerange.NumberFormat = "0000";
            range.Value = arr;
            range.Columns.AutoFit();

            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
            objBook.SaveAs(FileName, Excel.XlFileFormat.xlOpenXMLWorkbook,
            misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            objBook.Close(true, misValue, misValue);
            objApp.Quit();
            releaseObject(xlWorkSheet);
            releaseObject(objBook);
            releaseObject(objApp);

        }




        public void CreateDriverSlipsList(string driver, DateTime EndDate)
        {

            ssprocessds.view_scheduledays.Rows.Clear();

            for (int d = 1; d <= 7; d++)
            {
                ssprocessds.view_distinctstoreslipday.Rows.Clear();
                string commandtext = "SELECT distinct driver_1, drivername, storecode, storenameandaddress, daynumber  from view_expandedslip where driver_1 = @driver and daynumber = @daynumber and DATEADD( d, 10, slip_date) >= @enddate  and  slip_date <= @enddate ";
                ClearParameters();
                AddParms("@driver", driver, "SQL");
                AddParms("@daynumber", d, "SQL");
                AddParms("@enddate", EndDate, "SQL");

                FillData(ssprocessds, "view_distinctstoreslipday", commandtext, CommandType.Text);


                for (int r = 0; r < ssprocessds.view_distinctstoreslipday.Rows.Count; r++)
                {
                    if (ssprocessds.view_scheduledays.Rows.Count < r + 1)
                    {
                        ssprocessds.view_scheduledays.Rows.Add();
                        ssprocessds.view_scheduledays[r].sunday = "";
                        ssprocessds.view_scheduledays[r].monday = "";
                        ssprocessds.view_scheduledays[r].tuesday = "";
                        ssprocessds.view_scheduledays[r].wednesday = "";
                        ssprocessds.view_scheduledays[r].thursday = "";
                        ssprocessds.view_scheduledays[r].friday = "";
                        ssprocessds.view_scheduledays[r].saturday = "";

                    }
                    switch (ssprocessds.view_distinctstoreslipday[r].daynumber.ToString().PadLeft(3, '0'))
                    {
                        case "001":
                            {
                                ssprocessds.view_scheduledays[r].sunday = ssprocessds.view_distinctstoreslipday[r].storenameandaddress;
                                break;
                            }
                        case "002":
                            {
                                ssprocessds.view_scheduledays[r].monday = ssprocessds.view_distinctstoreslipday[r].storenameandaddress;
                                break;
                            }
                        case "003":
                            {
                                ssprocessds.view_scheduledays[r].tuesday = ssprocessds.view_distinctstoreslipday[r].storenameandaddress;
                                break;
                            }
                        case "004":
                            {
                                ssprocessds.view_scheduledays[r].wednesday = ssprocessds.view_distinctstoreslipday[r].storenameandaddress;
                                break;
                            }
                        case "005":
                            {
                                ssprocessds.view_scheduledays[r].thursday = ssprocessds.view_distinctstoreslipday[r].storenameandaddress;
                                break;
                            }
                        case "006":
                            {
                                ssprocessds.view_scheduledays[r].friday = ssprocessds.view_distinctstoreslipday[r].storenameandaddress;
                                break;
                            }
                        case "007":
                            {
                                ssprocessds.view_scheduledays[r].saturday = ssprocessds.view_distinctstoreslipday[r].storenameandaddress;
                                break;
                            }

                    }
                    ssprocessds.view_scheduledays.AcceptChanges();
                }
            }


            ShowVsReport(reportpath + "VSDropList.rdlc", "ssprocessDs", ssprocessds.view_scheduledays, "Driver Schedule For " + commonAppDataMethods.GetDriverName(driver).TrimEnd() + "- Week Ending " + String.Format("{0:MM/dd/yy}", EndDate));


        }

        public void CreateDropScheduleReport(string driver, string compcode, string storecode)
        {
            FrmLoading frmLoading = new FrmLoading();
            frmLoading.Show();


            string reporttitle = "Drop Schedule ";

            // Build the query based on the paramater values
            string commandtext = "SELECT * from view_expandeddropschedule WHERE  daynumber = @daynumber  AND active = 1 ";

            if (driver.TrimEnd() != "")
            {
                reporttitle += commonAppDataMethods.GetDriverName(driver).TrimEnd() + " ";


                commandtext += " AND driver_1 = @driver";
            }

            if (storecode.TrimEnd() != "")
            {
                reporttitle += commonAppDataMethods.GetStoreName(storecode).TrimEnd();
                commandtext += " AND storecode = @storecode";
            }
            else
            {
                if (compcode.TrimEnd() != "")
                {
                    reporttitle += commonAppDataMethods.GetCompanyName(compcode).TrimEnd();
                    commandtext += " AND LEFT(storecode,4) = @compcode";
                }



            }


            commandtext += " ORDER by storelocation ";

            ssprocessds.view_scheduledays.Rows.Clear();
            for (int d = 1; d <= 7; d++)
            {
                ClearParameters();
                AddParms("@daynumber", d, "SQL");
                if (driver.TrimEnd() != "")
                {
                    AddParms("@driver", driver, "SQL");
                }

                if (storecode.TrimEnd() != "")
                {
                    AddParms("@storecode", storecode, "SQL");
                }
                else
                {
                    if (compcode.TrimEnd() != "")
                    {
                        AddParms("@compcode", compcode, "SQL");
                    }
                }


                ssprocessds.view_expandeddropschedule.Rows.Clear();

                FillData(ssprocessds, "view_expandeddropschedule", commandtext, CommandType.Text);


                for (int r = 0; r < ssprocessds.view_expandeddropschedule.Rows.Count; r++)
                {
                    if (ssprocessds.view_scheduledays.Rows.Count < r + 1)
                    {
                        ssprocessds.view_scheduledays.Rows.Add();
                        ssprocessds.view_scheduledays[r].sunday = "";
                        ssprocessds.view_scheduledays[r].monday = "";
                        ssprocessds.view_scheduledays[r].tuesday = "";
                        ssprocessds.view_scheduledays[r].wednesday = "";
                        ssprocessds.view_scheduledays[r].thursday = "";
                        ssprocessds.view_scheduledays[r].friday = "";
                        ssprocessds.view_scheduledays[r].saturday = "";

                    }
                    switch (ssprocessds.view_expandeddropschedule[r].daynumber.ToString().PadLeft(3, '0'))
                    {
                        case "001":
                            {
                                ssprocessds.view_scheduledays[r].sunday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "002":
                            {
                                ssprocessds.view_scheduledays[r].monday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "003":
                            {
                                ssprocessds.view_scheduledays[r].tuesday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "004":
                            {
                                ssprocessds.view_scheduledays[r].wednesday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "005":
                            {
                                ssprocessds.view_scheduledays[r].thursday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "006":
                            {
                                ssprocessds.view_scheduledays[r].friday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "007":
                            {
                                ssprocessds.view_scheduledays[r].saturday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }

                    }
                    ssprocessds.view_scheduledays.AcceptChanges();
                }
            }

            frmLoading.Close();
            ShowVsReport(reportpath + "VSDropList.rdlc", "ssprocessDs", ssprocessds.view_scheduledays, reporttitle);


        }


        private void CreateSlipsReport(string driver, DateTime startdate, string compcode, string storecode)
        {
            FrmLoading frmLoading = new FrmLoading();
            frmLoading.Show();


            string reporttitle = "Slips Listing ";

            // Build the query based on the paramater values
            string commandtext = "SELECT * from  view_distinctstoreslipday WHERE  daynumber = @daynumber ";

            if (driver.TrimEnd() != "")
            {
                reporttitle += commonAppDataMethods.GetDriverName(driver).TrimEnd() + " ";


                commandtext += " AND driver_1 = @driver";
            }

            if (storecode.TrimEnd() != "")
            {
                reporttitle += commonAppDataMethods.GetStoreName(storecode).TrimEnd();
                commandtext += " AND storecode = @storecode";
            }
            else
            {
                if (compcode.TrimEnd() != "")
                {
                    reporttitle += commonAppDataMethods.GetCompanyName(compcode).TrimEnd();
                    commandtext += " AND LEFT(storecode,4) = @compcode";
                }



            }


            commandtext += " ORDER by storelocation ";

            ssprocessds.view_scheduledays.Rows.Clear();
            for (int d = 1; d <= 7; d++)
            {
                ClearParameters();
                AddParms("@daynumber", d, "SQL");
                if (driver.TrimEnd() != "")
                {
                    AddParms("@driver", driver, "SQL");
                }

                if (storecode.TrimEnd() != "")
                {
                    AddParms("@storecode", storecode, "SQL");
                }
                else
                {
                    if (compcode.TrimEnd() != "")
                    {
                        AddParms("@compcode", compcode, "SQL");
                    }
                }


                ssprocessds.view_expandeddropschedule.Rows.Clear();

                FillData(ssprocessds, "view_expandeddropschedule", commandtext, CommandType.Text);


                for (int r = 0; r < ssprocessds.view_expandeddropschedule.Rows.Count; r++)
                {
                    if (ssprocessds.view_scheduledays.Rows.Count < r + 1)
                    {
                        ssprocessds.view_scheduledays.Rows.Add();
                        ssprocessds.view_scheduledays[r].sunday = "";
                        ssprocessds.view_scheduledays[r].monday = "";
                        ssprocessds.view_scheduledays[r].tuesday = "";
                        ssprocessds.view_scheduledays[r].wednesday = "";
                        ssprocessds.view_scheduledays[r].thursday = "";
                        ssprocessds.view_scheduledays[r].friday = "";
                        ssprocessds.view_scheduledays[r].saturday = "";

                    }
                    switch (ssprocessds.view_expandeddropschedule[r].daynumber.ToString().PadLeft(3, '0'))
                    {
                        case "001":
                            {
                                ssprocessds.view_scheduledays[r].sunday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "002":
                            {
                                ssprocessds.view_scheduledays[r].monday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "003":
                            {
                                ssprocessds.view_scheduledays[r].tuesday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "004":
                            {
                                ssprocessds.view_scheduledays[r].wednesday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "005":
                            {
                                ssprocessds.view_scheduledays[r].thursday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "006":
                            {
                                ssprocessds.view_scheduledays[r].friday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }
                        case "007":
                            {
                                ssprocessds.view_scheduledays[r].saturday = ssprocessds.view_expandeddropschedule[r].storeaddress;
                                break;
                            }

                    }
                    ssprocessds.view_scheduledays.AcceptChanges();
                }
            }

            frmLoading.Close();
            ShowVsReport(reportpath + "VSDropList.rdlc", "ssprocessDs", ssprocessds.view_scheduledays, reporttitle);

        }

        public void CreateCurrencyDropListing(string driver, string compcode, string storecode, DateTime startdate, DateTime enddate)
        {
            FrmLoading frmLoading = new FrmLoading();
            frmLoading.Show();


            string reporttitle = "Currency Drop Listing ";

            // Build the query based on the paramater values
            string commandtext = "SELECT * from view_expandedcurrencydrop  WHERE active = 1 AND deliverydate >= @startdate AND deliverydate <= @enddate ";
            ClearParameters();
            AddParms("@startdate", startdate, "SQL");
            AddParms("@enddate", enddate, "SQL");

            if (driver.TrimEnd() != "")
            {
                reporttitle += commonAppDataMethods.GetDriverName(driver).TrimEnd() + " ";
                AddParms("@driver", driver, "SQL");
                commandtext += " AND driver = @driver";
            }

            if (storecode.TrimEnd() != "")
            {
                reporttitle += commonAppDataMethods.GetStoreName(storecode).TrimEnd();
                AddParms("@storecode", storecode, "SQL");
                commandtext += " AND storecode = @storecode";
            }
            else
            {
                if (compcode.TrimEnd() != "")
                {
                    AddParms("@compcode", compcode, "SQL");
                    reporttitle += commonAppDataMethods.GetCompanyName(compcode).TrimEnd();
                    commandtext += " AND LEFT(storecode,4) = @compcode";
                }
            }
            commandtext += " ORDER by drivername ";
            ssprocessds.view_expandedcurrencydrop.Rows.Clear();

            FillData(ssprocessds, "view_expandedcurrencydrop", commandtext, CommandType.Text);



            frmLoading.Close();
            ShowVsReport(reportpath + "VSCurrencydropList.rdlc", "ssprocessDs", ssprocessds.view_expandedcurrencydrop, reporttitle);


        }


        public void DriverCoinRouteList()
        {
            string driver = "";

            driver = commonAppDataMethods.SelectDriver();
            if (driver.TrimEnd() != "")
            {

                ssprocessds.view_coinroute.Rows.Clear();
                string commandtext = "SELECT * from view_coinroute where driver_1 = @driver order by store  ";
                ClearParameters();
                AddParms("@driver", driver, "SQL");
                FillData(ssprocessds, "view_coinroute", commandtext, CommandType.Text);
                ShowVsReport(reportpath + "DriverCoinRoute.rdlc", "ssprocessDs", ssprocessds.view_coinroute, "Driver Coin Route for " + commonAppDataMethods.GetDriverName(driver));



            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }

        public void InventoriedCoinDrops()
        {

            int selectedbankid = commonAppDataMethods.SelectBank();
            string trantype = "";
            string trandescrip = "";
            string bankfedid = "";
            string bankname = "";
            DateTime InventoryDate = DateTime.Now.Date;
            // If a row has been selected fill the data and process
            if (selectedbankid > 0)
            {
                ClearParameters();
                AddParms("@idcol", selectedbankid, "SQL");
                FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
                bankfedid = ssprocessds.bank[0].bankfedid;
                bankname = ssprocessds.bank[0].name;
                if (commonAppDataMethods.GetSingleDate("Enter Inventory Date", 10000, 10000))
                {
                    InventoryDate = commonAppDataMethods.SelectedDate.Date;
                    FrmLoading frmLoading = new FrmLoading();
                    frmLoading.Show();
                    ClearParameters();
                    AddParms("@inventorydate", InventoryDate, "SQL");
                    AddParms("@bankfedid", bankfedid, "SQL");
                    string commandtext = "SELECT  * from view_inventoriedcoindrop where bankfedid = @bankfedid AND inventorydate = @inventorydate";
                    FillData(ssprocessds, "view_inventoriedcoindrop", commandtext, CommandType.Text);
                    ShowVsReport(reportpath + "InventoriedCoinDrops.rdlc", "ssprocessDs", ssprocessds.view_inventoriedcoindrop, "Inventoried Coin Drops " + bankname.TrimEnd() + " Inventory Date: " + String.Format("{0:MM/dd/yy}", InventoryDate.Date));

                }


            }

        }

        public void StartSmartSafeCustomerReports()
        {
            List<string> EmailAttachments = new List<string>();
            EmailAttachments.Clear();
            string reportCompany = "";
            DateTime reportPostingDate = DateTime.Now;
            while (1 == 1)
            {
                reportCompany = commonAppDataMethods.SelectCompany();
                if (reportCompany.Length > 0)
                {
                    if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 10000, 10000))
                    {
                        reportPostingDate = commonAppDataMethods.SelectedDate.Date;
                        string commandtext = "";
                        FrmLoading frmLoading = new FrmLoading();

                        string ReportTitle = "Smart Safe Activity Customer: " + commonAppDataMethods.GetCompanyName(reportCompany).TrimEnd() + " for " + String.Format("{0:MM/dd/yyyy}", reportPostingDate.Date);
                        // Declared
                        commandtext = "SELECT * FROM view_expandedsmartsafetrans WHERE LEFT(store,4) = @compcode AND postingdate = @postingdate";
                        rptprocessds.view_expandedsmartsafetrans.Rows.Clear();
                        ClearParameters();
                        AddParms("@compcode", reportCompany, "SQL");
                        AddParms("@postingdate", reportPostingDate.Date, "SQL");
                        FillData(rptprocessds, "view_expandedsmartsafetrans", commandtext, CommandType.Text);
                        ShowVsReport(reportpath + "VSSmartSafeCustomerReport.rdlc", "ssprocessDs", rptprocessds.view_expandedsmartsafetrans, ReportTitle);
                        MakeVSReportPDF(reportpath + "VSSmartSafeCustomerReport.rdlc", "ssprocessDs", rptprocessds.view_expandedsmartsafetrans, ReportTitle,
                          ConfigurationManager.AppSettings["PDFFilesPath"] + reportCompany + String.Format("{0:MM-dd-yy}", reportPostingDate) + ".pdf");
                        EmailAttachments.Add(ConfigurationManager.AppSettings["PDFFilesPath"] + reportCompany + String.Format("{0:MM-dd-yy}", reportPostingDate) + ".pdf");
                        MakeVSReportXLS(reportpath + "VSSmartSafeCustomerReport.rdlc", "ssprocessDs", rptprocessds.view_expandedsmartsafetrans, ReportTitle,
                          ConfigurationManager.AppSettings["XLSFilesPath"] + reportCompany + String.Format("{0:MM-dd-yy}", reportPostingDate) + ".xls");
                        EmailAttachments.Add(ConfigurationManager.AppSettings["XLSFilesPath"] + reportCompany + String.Format("{0:MM-dd-yy}", reportPostingDate) + ".xls");

                        if (wsgUtilities.wsgReply("Send Email?"))
                        {
                            customerEmailMethods.SendCustomerReportEmail(reportCompany, "SmartSafe Activity " + String.Format("{0:MM-dd-yy}", reportPostingDate), "See Attached", EmailAttachments);
                            appUtilities.logEvent(reportCompany + "-" + String.Format("{0:MM-dd-yy}", reportPostingDate) + " file Sent", "Cust File", "Success", false);
                            wsgUtilities.wsgNotice("Email Sent");
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("Email Not Sent");
                        }
                    }



                }
                else
                {
                    break;
                }
            }

        }


        public void ATMDropImportList()
        {
            DateTime reportdate = new DateTime();
            string commandtext = "";
            if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 10000, 10000))
            {
                reportdate = commonAppDataMethods.SelectedDate.Date;
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                string ReportTitle = "ATM Drop Import Listing: " + String.Format("{0:MM/dd/yyyy}", reportdate.Date);
                commandtext = "SELECT * FROM view_ExpandedATMDropImport  WHERE postingdate = @postingdate ORDER BY owner, dropdate";
                atmds.view_ExpandedATMDropImport.Rows.Clear();
                ClearParameters();
                AddParms("@postingdate", reportdate, "SQL");
                FillData(atmds, "view_ExpandedATMDropImport", commandtext, CommandType.Text);
                frmLoading.Close();
                if (atmds.view_ExpandedATMDropImport.Rows.Count > 0)
                {
                    ShowVsReport(reportpath + "VSATMDropImportList.rdlc", "ssprocessDs", atmds.view_ExpandedATMDropImport, ReportTitle);
                }
                else
                {
                    wsgUtilities.wsgNotice("No matching records");
                }

            }


        }
        public bool RunATMDropsStatus(int bankidcol, DateTime startdate, DateTime enddate, bool showreport, bool inventoried, bool makepdf)
        {
            string commandtext = "";
            FrmLoading frmLoading = new FrmLoading();
            string ReportTitle = "";
            if (showreport)
            {
                frmLoading.Show();
            }

            // Locate the bank record
            ssprocessds.bank.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", bankidcol, "SQL");
            FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
            string bankfedid = ssprocessds.bank[0].bankfedid;
            this.ClearParameters();
            this.AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@startdate", startdate, "SQL");
            AddParms("@enddate", enddate, "SQL");
            ssprocessds.view_ATMDropInvStatus.Rows.Clear();
            if (inventoried)
            {
                commandtext = "  SELECT *  FROM view_ATMDropInvStatus WHERE bankfedid = @bankfedid AND trandate IS NOT NULL AND trandate BETWEEN @startdate AND @enddate";
                ReportTitle = "Drops and CRVs Included in Inventory - ";
            }
            else
            {
                commandtext = "  SELECT * FROM view_ATMDropInvStatus WHERE trantype = 'D' AND bankfedid = @bankfedid AND dropdate BETWEEN @startdate AND @enddate";

                ReportTitle = "ATM Drops Not Included in Inventory - ";
                commandtext += " AND trandate IS null ";
            }
            ReportTitle += ssprocessds.bank[0].name.TrimEnd() + " - " + String.Format("{0:MM/dd/yyyy}", startdate) +
            " thru " + String.Format("{0:MM/dd/yyyy}", enddate);
            commandtext += " ORDER BY dropdate, drivername, store_name ";

            FillData(ssprocessds, "view_ATMDropInvStatus", commandtext, CommandType.Text);
            frmLoading.Close();
            if (ssprocessds.view_ATMDropInvStatus.Count > 0)
            {
                if (showreport)
                {
                    ShowVsReport(reportpath + "VSATMDropStatus.rdlc", "ssprocessDS", ssprocessds.view_ATMDropInvStatus, ReportTitle);
                }
            }
            else
            {
                wsgUtilities.wsgNotice("No matching records");
            }


            if (makepdf)
            {
                MakeVSReportPDF(reportpath + "VSATMDropStatus.rdlc", "ssprocessDS", ssprocessds.view_ATMDropInvStatus, ReportTitle, ConfigurationManager.AppSettings["PDFFilesPath"] + bankfedid.TrimEnd() + "ATMDropStatus" + String.Format("{0:MM-dd-yy}", enddate) + ".pdf");

            }

            return true;
        }
        public bool RunATMDepositedJournalHistory(int bankidcol, DateTime startdate, DateTime enddate, bool showreport, bool makepdf)
        {

            string bankfedid = "";
            string RptTitle = "ATM Residual History From " + String.Format("{0:MM/dd/yyyy}", startdate) + " thru " + String.Format("{0:MM/dd/yyyy}", enddate);
            FrmLoading frmLoading = new FrmLoading();
            if (showreport)
            {
                frmLoading.Show();
            }
            // Locate the bank record
            ssprocessds.bank.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", bankidcol, "SQL");
            FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
            bankfedid = ssprocessds.bank[0].bankfedid;
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@startdate", startdate, "SQL");
            AddParms("@enddate", enddate, "SQL");
            string commandtext = " SELECT a.* FROM view_ATMJournalHistory a INNER JOIN  invtrans b ON a.atmjournalid = b.atmtransid ";
            commandtext += " WHERE b.trandescrip = 'ATM Jour' AND a.bankfedid = @bankfedid AND b.trandate BETWEEN @startdate and  @enddate ";
            FillData(rptprocessds, "view_ATMJournalHistory", commandtext, CommandType.Text);
            frmLoading.Close();
            if (showreport)
            {
                ShowVsReport(reportpath + "VSATMDepositedJournalHistory.rdlc", "ssprocessDS", rptprocessds.view_ATMJournalHistory, RptTitle);
            }
            if (makepdf)
            {
                MakeVSReportPDF(reportpath + "VSATMDepositedJournalHistory.rdlc", "ssprocessDS", rptprocessds.view_ATMJournalHistory, RptTitle, ConfigurationManager.AppSettings["PDFFilesPath"] + bankfedid.TrimEnd() + "ATMResiduals" + String.Format("{0:MM-dd-yy}", enddate) + ".pdf");

            }
            return true;


        }
        public bool RunProofSheet(int bankidcol, DateTime proofsheetclosedate, bool showreport, bool makepdf)
        {
            DateTime ProofSheetOpenDate = proofsheetclosedate.AddDays(-1);
            string SheetTitle = "";
            string bankfedid = "";
            string CloseMessage = "";
            FrmLoading frmLoading = new FrmLoading();
            if (showreport)
            {
                frmLoading.Show();
            }
            // Locate the bank record
            ssprocessds.bank.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", bankidcol, "SQL");
            FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
            bankfedid = ssprocessds.bank[0].bankfedid;

            SheetTitle = ssprocessds.bank[0].name.TrimEnd() + " Proof Sheet dated " + String.Format("{0:MM/dd/yyyy}", proofsheetclosedate.Date);
            string commandtext = "SELECT TOP 1 *  FROM balance  WHERE bankfedid = @bankfedid AND postdate < @balancedate ORDER BY postdate DESC";
            rptprocessds.balance.Rows.Clear();
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@balancedate", proofsheetclosedate, "SQL");
            FillData(rptprocessds, "balance", commandtext, CommandType.Text);
            if (rptprocessds.balance.Rows.Count > 0)
            {
                ProofSheetOpenDate = rptprocessds.balance[0].postdate;
            }
            rptprocessds.view_proofsheet.Rows.Clear();
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@balancedate", proofsheetclosedate, "SQL");
            FillData(rptprocessds, "view_proofsheet", "wsgsp_proofsheetopeningbalance", CommandType.StoredProcedure);
            ClearParameters();
            AddParms("@opendate", ProofSheetOpenDate, "SQL");
            AddParms("@closedate", proofsheetclosedate, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(rptprocessds, "view_proofsheet", "wsgsp_proofsheetreceipts", CommandType.StoredProcedure);
            ClearParameters();
            AddParms("@opendate", ProofSheetOpenDate, "SQL");
            AddParms("@closedate", proofsheetclosedate, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(rptprocessds, "view_proofsheet", "wsgsp_proofsheetissues", CommandType.StoredProcedure);
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@balancedate", proofsheetclosedate, "SQL");
            FillData(rptprocessds, "view_proofsheet", "wsgsp_proofsheetclosingbalance", CommandType.StoredProcedure);

            // Check for closing balance
            string selectexpression = "rectype = 'D'";
            DataRow[] foundRows;
            foundRows = rptprocessds.view_proofsheet.Select(selectexpression);
            if (foundRows.Length == 0)
            {
                CloseMessage = "Proof Sheet has not been closed";
            }
            // Check for missing rows
            CheckProofSheetRow("A", "Opening Balance");
            CheckProofSheetRow("B", "Receipts");
            CheckProofSheetRow("C", "Issues");
            CheckProofSheetRow("D", "Closing Balance");
            if (showreport)
            {
                frmLoading.Close();
                ShowVsReport(reportpath + "VSProofSheet.rdlc", "ssprocessDS", rptprocessds.view_proofsheet, SheetTitle + Environment.NewLine + CloseMessage);
            }
            if (makepdf)
            {
                MakeVSReportPDF(reportpath + "VSProofSheet.rdlc", "ssprocessDS", rptprocessds.view_proofsheet, SheetTitle + Environment.NewLine + CloseMessage, ConfigurationManager.AppSettings["PDFFilesPath"] + bankfedid.TrimEnd() + "ps" + String.Format("{0:MM-dd-yy}", proofsheetclosedate) + ".pdf");
            }
            return true;

        }

        public void RunSafekeepingUnverifiedDeclared()
        {

            string bankfedid = commonAppDataMethods.SelectSmartsafeBankFedid();

            if (bankfedid != "")
            {
                DateTime reportdate = new DateTime();
                if (commonAppDataMethods.GetSingleDate("Enter Inventory Date", 10000, 10000))
                {
                    reportdate = commonAppDataMethods.SelectedDate.Date;
                    string ReportTitle = "Smart Safe Unverified Declared As Of " + String.Format("{0:MM/dd/yyyy}", reportdate);
                    string commandtext = " select *, dbo.GetNextPickupdate(@selectdate,store) AS nextpickupdate from view_expandedsmartsafetrans where bankfedid = @bankfedid AND eventcode = 'DECL' AND postingdate <= @selectdate  and (verifyid = 0  OR ";
                    commandtext += " (verifyid <> 0 AND  verifyid IN  (SELECT idcol FROM smartsafetrans WHERE bankfedid = @bankfedid AND eventcode =  'VER' AND postingdate > @selectdate )))";
                    rptprocessds.view_smartsafeverified.Rows.Clear();
                    ClearParameters();
                    AddParms("@selectdate", reportdate, "SQL");
                    AddParms("@bankfedid", bankfedid, "SQL");
                    FrmLoading frmLoading = new FrmLoading();
                    frmLoading.Show();
                    FillData(rptprocessds, "view_smartsafeverified", commandtext, CommandType.Text);
                    if (rptprocessds.view_smartsafeverified.Rows.Count < 1)
                    {
                        EstablishBlankDataTableRow(rptprocessds.view_smartsafeverified);
                    }
                    frmLoading.Close();
                    ShowVsReport(reportpath + "VSSafekeepingUnverifiedDeclared.rdlc", "ssprocessDS", rptprocessds.view_smartsafeverified, ReportTitle);
                    MakeVSReportPDF(reportpath + "VSSafekeepingUnverifiedDeclared.rdlc", "ssprocessDS", rptprocessds.view_smartsafeverified, ReportTitle, ConfigurationManager.AppSettings["PDFFilesPath"] + bankfedid.TrimEnd().ToLower() + "unverified" + String.Format("{0:MM-dd-yy}", reportdate.Date) + ".pdf");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Report Cancelled");
            }

        }
        public bool RunSafekeepingActivityAnalysis(DateTime reportdate, bool showreport, string bankfedid, bool makepdf)
        {
            string commandtext = "";
            FrmLoading frmLoading = new FrmLoading();

            string ReportTitle = "Smart Safe Activity Analysis " + String.Format("{0:MM/dd/yyyy}", reportdate.Date);
            // Declared
            commandtext = "SELECT 0 AS verified, 0 AS transferred, saidtocontain AS declared, *  FROM view_expandedsmartsafetrans  WHERE bankfedid = @bankfedid AND  postingdate = @postingdate AND eventcode <> 'VER'";
            rptprocessds.view_smartsafeactivityanalysis.Rows.Clear();
            ClearParameters();
            AddParms("@postingdate", reportdate.Date, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(rptprocessds, "view_smartsafeactivityanalysis", commandtext, CommandType.Text);
            for (int r = 0; r < rptprocessds.view_smartsafeactivityanalysis.Rows.Count; r++)
            {
                switch (rptprocessds.view_smartsafeactivityanalysis[r].eventcode.TrimEnd())
                {

                    case "FED":
                        {
                            rptprocessds.view_smartsafeactivityanalysis[r].storename = "Fed Transfer";
                            rptprocessds.view_smartsafeactivityanalysis[r].hundreds = rptprocessds.view_smartsafeactivityanalysis[r].hundreds * -1;
                            rptprocessds.view_smartsafeactivityanalysis[r].fiftys = rptprocessds.view_smartsafeactivityanalysis[r].fiftys * -1;
                            rptprocessds.view_smartsafeactivityanalysis[r].twentys = rptprocessds.view_smartsafeactivityanalysis[r].twentys * -1;
                            rptprocessds.view_smartsafeactivityanalysis[r].tens = rptprocessds.view_smartsafeactivityanalysis[r].tens * -1;
                            rptprocessds.view_smartsafeactivityanalysis[r].fives = rptprocessds.view_smartsafeactivityanalysis[r].fives * -1;
                            rptprocessds.view_smartsafeactivityanalysis[r].twos = rptprocessds.view_smartsafeactivityanalysis[r].twos * -1;
                            rptprocessds.view_smartsafeactivityanalysis[r].ones = rptprocessds.view_smartsafeactivityanalysis[r].ones * -1;
                            rptprocessds.view_smartsafeactivityanalysis[r].mixedcoin = rptprocessds.view_smartsafeactivityanalysis[r].mixedcoin * -1;
                            rptprocessds.view_smartsafeactivityanalysis[r].totaldeposit = rptprocessds.view_smartsafeactivityanalysis[r].totaldeposit * -1;
                            break;
                        }
                    case "BULK":
                        {
                            rptprocessds.view_smartsafeactivityanalysis[r].storename = "Incoming Bulk";
                            break;
                        }

                    default:
                        {
                            rptprocessds.view_smartsafeactivityanalysis[r].totaldeposit = 0;
                            break;
                        }
                }
            }

            // Verified transactions
            commandtext = "SELECT 0 AS transferred, totaldeposit AS verified, 0 AS declared,  *  FROM view_smartsafeverified WHERE postingdate = @postingdate AND eventcode = 'VER'";
            ssprocessds.view_smartsafeactivityanalysis.Rows.Clear();
            ClearParameters();
            AddParms("@postingdate", reportdate.Date, "SQL");
            FillData(ssprocessds, "view_smartsafeactivityanalysis", commandtext, CommandType.Text);

            // Merge the tables
            for (int r = 0; r < ssprocessds.view_smartsafeactivityanalysis.Rows.Count; r++)
            {
                rptprocessds.view_smartsafeactivityanalysis.ImportRow(ssprocessds.view_smartsafeactivityanalysis[r]);

            }

            // Declared amounts related to verified transactions

            commandtext = "SELECT saidtocontain as transferred, 'TR' AS eventcode,* from smartsafetrans WHERE eventcode = 'DECL'  ";

            commandtext += " AND verifyid IN (SELECT idcol FROM view_expandedsmartsafetrans WHERE eventcode = 'VER' AND postingdate = @postingdate)";
            ssprocessds.view_smartsafeactivityanalysis.Rows.Clear();
            ClearParameters();
            AddParms("@postingdate", reportdate.Date, "SQL");
            FillData(ssprocessds, "view_smartsafeactivityanalysis", commandtext, CommandType.Text);
            // Merge the tables
            for (int r = 0; r < ssprocessds.view_smartsafeactivityanalysis.Rows.Count; r++)
            {
                rptprocessds.view_smartsafeactivityanalysis.ImportRow(ssprocessds.view_smartsafeactivityanalysis[r]);

            }

            // Force one row if needed
            if (rptprocessds.view_smartsafeactivityanalysis.Rows.Count < 1)
            {
                EstablishBlankDataTableRow(rptprocessds.view_smartsafeactivityanalysis);
            }

            ShowVsReport(reportpath + "VSSafekeepingActivityAnalysis.rdlc", "ssprocessDS", rptprocessds.view_smartsafeactivityanalysis, ReportTitle);

            MakeVSReportPDF(reportpath + "VSSafekeepingActivityAnalysis.rdlc", "ssprocessDS", rptprocessds.view_smartsafeactivityanalysis, ReportTitle, ConfigurationManager.AppSettings["PDFFilesPath"] + bankfedid.TrimEnd().ToLower() + "activity" + String.Format("{0:MM-dd-yy}", reportdate.Date) + ".pdf");
            return true;
        }

        public bool RunSafekeepingProofSheet(DateTime proofsheetclosedate, bool showreport, string bankfedid, bool makepdf)
        {
            string commandtext = "";
            DateTime ProofSheetOpenDate = proofsheetclosedate.AddDays(-1);
            string SheetTitle = "";
            string CloseMessage = "";
            FrmLoading frmLoading = new FrmLoading();
            SheetTitle = "Smart Safe Proof Sheet dated " + String.Format("{0:MM/dd/yyyy}", proofsheetclosedate.Date);
            commandtext = "SELECT TOP 1 *  FROM balance  WHERE bankfedid = @bankfedid AND postdate < @balancedate ORDER BY postdate DESC";
            rptprocessds.balance.Rows.Clear();
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@balancedate", proofsheetclosedate, "SQL");
            FillData(rptprocessds, "balance", commandtext, CommandType.Text);
            if (rptprocessds.balance.Rows.Count > 0)
            {
                ProofSheetOpenDate = rptprocessds.balance[0].postdate;
            }
            rptprocessds.view_safekeepingproofsheet.Rows.Clear();

            // Opening Balance
            ClearParameters();
            commandtext = "SELECT TOP 1 'A' AS rectype, 'Opening Balance  ' AS descrip, saidtocontain AS unverified, hundreds, fiftys, twentys, tens, fives, ones, twos, mixedcoin, foodstamps FROM balance ";
            commandtext += "  WHERE bankfedid = @bankfedid AND postdate < @balancedate ORDER BY postdate DESC";
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@balancedate", proofsheetclosedate, "SQL");
            FillData(rptprocessds, "view_safekeepingproofsheet", commandtext, CommandType.Text);
            // Transfer from processing
            ClearParameters();
            AddParms("@closedate", proofsheetclosedate, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");
            commandtext = "IF EXISTS(SELECT * FROM smartsafetrans WHERE bankfedid = @bankfedid AND postingdate = @closedate AND eventcode = 'VER') ";
            commandtext += "  SELECT SUM( mixedcoin) AS mixedcoin, 'B' AS rectype, 'Transfer From Processing' AS descrip, COALESCE(SUM(-d.unverified),0) as unverified, SUM(hundreds) AS hundreds, ";
            commandtext += " SUM(fiftys) AS fiftys, SUM(twentys) AS twentys, SUM(tens) AS tens, SUM(fives) AS fives, SUM(twos) AS twos, SUM(ones) AS ones, SUM(foodstamps) AS foodstamps  ";
            commandtext += " FROM ((SELECT idcol, hundreds,fiftys,twentys,tens,fives,ones, twos,mixedcoin,foodstamps FROM smartsafetrans WHERE bankfedid = @bankfedid AND postingdate = @closedate AND eventcode = 'VER') v ";
            commandtext += " INNER JOIN (SELECT verifyid,  SUM(saidtocontain) AS unverified FROM smartsafetrans WHERE bankfedid = @bankfedid GROUP BY verifyid) D ON d.verifyid = v.idcol )";
            ssprocessds.view_safekeepingproofsheet.Rows.Clear();
            FillData(ssprocessds, "view_safekeepingproofsheet", commandtext, CommandType.Text);
            if (ssprocessds.view_safekeepingproofsheet.Rows.Count > 0)
            {
                rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);

            }
            else
            {
                ssprocessds.view_safekeepingproofsheet.Rows.Clear();
                ssprocessds.view_safekeepingproofsheet.Rows.Add();
                InitializeDataRow(ssprocessds.view_safekeepingproofsheet[0]);
                ssprocessds.view_safekeepingproofsheet[0].rectype = "B";
                ssprocessds.view_safekeepingproofsheet[0].descrip = "Transfer From Processing";
                rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);
            }

            // Unverified incoming from customer           
            ClearParameters();
            AddParms("@closedate", proofsheetclosedate, "SQL");
            commandtext = " SELECT SUM(0) AS mixedcoin , 'C' AS rectype,  'Unverified Inc. From Cust' AS descrip, SUM(saidtocontain) as unverified,sum(0) AS hundreds, sum(0) AS fiftys, ";
            commandtext += " SUM(0) as twentys, SUM(0) AS tens, SUM(0) AS fives, SUM(0) AS  twos, SUM(0) AS  ones, SUM(0) AS foodstamps from smartsafetrans ";
            commandtext += " WHERE  postingdate = @closedate AND eventcode = 'DECL' GROUP BY eventcode";
            ssprocessds.view_safekeepingproofsheet.Rows.Clear();
            FillData(ssprocessds, "view_safekeepingproofsheet", commandtext, CommandType.Text);
            if (ssprocessds.view_safekeepingproofsheet.Rows.Count > 0)
            {
                rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);

            }
            else
            {
                ssprocessds.view_safekeepingproofsheet.Rows.Add();
                InitializeDataRow(ssprocessds.view_safekeepingproofsheet[0]);
                ssprocessds.view_safekeepingproofsheet[0].rectype = "C";
                ssprocessds.view_safekeepingproofsheet[0].descrip = "Unverified Inc. From Cust";
                rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);
            }


            // Outgoing Customer - add blank row
            rptprocessds.view_safekeepingproofsheet.Rows.Add();
            InitializeDataRow(rptprocessds.view_safekeepingproofsheet[rptprocessds.view_safekeepingproofsheet.Rows.Count - 1]);
            rptprocessds.view_safekeepingproofsheet[rptprocessds.view_safekeepingproofsheet.Rows.Count - 1].rectype = "D";
            rptprocessds.view_safekeepingproofsheet[rptprocessds.view_safekeepingproofsheet.Rows.Count - 1].descrip = "Outgoing-Customer";

            // Outgoing Fed
            ssprocessds.view_safekeepingproofsheet.Rows.Clear();
            commandtext = "IF EXISTS(SELECT * FROM smartsafetrans WHERE  postingdate = @closedate AND eventcode = 'FED') ";
            commandtext = "SELECT  SUM(-mixedcoin) AS mixedcoin, 'E' AS rectype, 'Outgoing - FED' AS descrip, 0 AS unverified, SUM(-hundreds) AS hundreds, SUM(-fiftys) as fiftys, SUM(-twentys) AS twentys, SUM(-tens) AS tens, ";
            commandtext += "  SUM(-fives) AS fives, SUM(-twos) AS twos, SUM(-ones) AS ones,  SUM(-foodstamps) AS foodstamps FROM smartsafetrans ";
            commandtext += " WHERE  postingdate = @closedate AND eventcode = 'FED'  GROUP BY eventcode";
            ClearParameters();
            AddParms("@closedate", proofsheetclosedate, "SQL");
            FillData(ssprocessds, "view_safekeepingproofsheet", commandtext, CommandType.Text);

            if (ssprocessds.view_safekeepingproofsheet.Rows.Count > 0)
            {
                rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);

            }
            else
            {
                ssprocessds.view_safekeepingproofsheet.Rows.Add();
                InitializeDataRow(ssprocessds.view_safekeepingproofsheet[0]);
                ssprocessds.view_safekeepingproofsheet[0].rectype = "E";
                ssprocessds.view_safekeepingproofsheet[0].descrip = "Outgoing - Fed";
                rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);
            }

            // Outgoing Fed
            ssprocessds.view_safekeepingproofsheet.Rows.Clear();
            commandtext = "IF EXISTS(SELECT * FROM smartsafetrans WHERE  postingdate = @closedate AND eventcode = 'BULK') ";
            commandtext = "SELECT  SUM(mixedcoin) AS mixedcoin, 'F' AS rectype, 'Incoming Bulk' AS descrip, 0 AS unverified, SUM(hundreds) AS hundreds, SUM(fiftys) as fiftys, SUM(twentys) AS twentys, SUM(tens) AS tens, ";
            commandtext += "  SUM(fives) AS fives, SUM(twos) AS twos, SUM(ones) AS ones,  SUM(foodstamps) AS foodstamps FROM smartsafetrans ";
            commandtext += " WHERE  postingdate = @closedate AND eventcode = 'BULK'  GROUP BY eventcode";
            ClearParameters();
            AddParms("@closedate", proofsheetclosedate, "SQL");
            FillData(ssprocessds, "view_safekeepingproofsheet", commandtext, CommandType.Text);

            if (ssprocessds.view_safekeepingproofsheet.Rows.Count > 0)
            {
                rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);

            }
            else
            {

                // Incoming Bulk - add blank row
                rptprocessds.view_safekeepingproofsheet.Rows.Add();
                InitializeDataRow(rptprocessds.view_safekeepingproofsheet[rptprocessds.view_safekeepingproofsheet.Rows.Count - 1]);
                rptprocessds.view_safekeepingproofsheet[rptprocessds.view_safekeepingproofsheet.Rows.Count - 1].rectype = "F";
                rptprocessds.view_safekeepingproofsheet[rptprocessds.view_safekeepingproofsheet.Rows.Count - 1].descrip = "Incoming Bulk";
            }
            // Compute totals
            ssprocessds.view_safekeepingproofsheet.Rows.Clear();
            ssprocessds.view_safekeepingproofsheet.Rows.Add();
            InitializeDataRow(ssprocessds.view_safekeepingproofsheet[0]);

            for (int r = 0; r < rptprocessds.view_safekeepingproofsheet.Rows.Count; r++)
            {

                ssprocessds.view_safekeepingproofsheet[0].unverified += rptprocessds.view_safekeepingproofsheet[r].unverified;
                ssprocessds.view_safekeepingproofsheet[0].hundreds += rptprocessds.view_safekeepingproofsheet[r].hundreds;
                ssprocessds.view_safekeepingproofsheet[0].fiftys += rptprocessds.view_safekeepingproofsheet[r].fiftys;
                ssprocessds.view_safekeepingproofsheet[0].twentys += rptprocessds.view_safekeepingproofsheet[r].twentys;
                ssprocessds.view_safekeepingproofsheet[0].tens += rptprocessds.view_safekeepingproofsheet[r].tens;
                ssprocessds.view_safekeepingproofsheet[0].fives += rptprocessds.view_safekeepingproofsheet[r].fives;
                ssprocessds.view_safekeepingproofsheet[0].twos += rptprocessds.view_safekeepingproofsheet[r].twos;
                ssprocessds.view_safekeepingproofsheet[0].ones += rptprocessds.view_safekeepingproofsheet[r].ones;
                ssprocessds.view_safekeepingproofsheet[0].mixedcoin += rptprocessds.view_safekeepingproofsheet[r].mixedcoin;
                ssprocessds.view_safekeepingproofsheet[0].foodstamps += rptprocessds.view_safekeepingproofsheet[r].foodstamps;
            }
            rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);
            rptprocessds.view_safekeepingproofsheet[rptprocessds.view_safekeepingproofsheet.Rows.Count - 1].rectype = "H";
            rptprocessds.view_safekeepingproofsheet[rptprocessds.view_safekeepingproofsheet.Rows.Count - 1].descrip = "Recalc Closing Balance";

            // Closing Balance
            ClearParameters();
            commandtext = "SELECT 'G' AS rectype, 'Closing Balance  ' AS descrip, saidtocontain AS unverified, hundreds, fiftys, twentys, tens, fives, ones, twos, mixedcoin, foodstamps FROM balance ";
            commandtext += "  WHERE bankfedid = @bankfedid AND postdate = @closedate";
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@closedate", proofsheetclosedate, "SQL");
            sscloseds.view_safekeepingproofsheet.Rows.Clear();
            FillData(sscloseds, "view_safekeepingproofsheet", commandtext, CommandType.Text);
            if (sscloseds.view_safekeepingproofsheet.Rows.Count > 0)
            {

                rptprocessds.view_safekeepingproofsheet.ImportRow(sscloseds.view_safekeepingproofsheet.Rows[0]);
                SheetTitle = "Final Smart Safe Proof Sheet dated " + String.Format("{0:MM/dd/yyyy}", proofsheetclosedate.Date);

            }
            else
            {
                SheetTitle = "Preliminary Smart Safe Proof Sheet dated " + String.Format("{0:MM/dd/yyyy}", proofsheetclosedate.Date);
                ssprocessds.view_safekeepingproofsheet.Rows.Clear();
                ssprocessds.view_safekeepingproofsheet.Rows.Add();
                InitializeDataRow(ssprocessds.view_safekeepingproofsheet[0]);
                ssprocessds.view_safekeepingproofsheet[0].rectype = "H";
                ssprocessds.view_safekeepingproofsheet[0].descrip = "Sheet Not closed";
                rptprocessds.view_safekeepingproofsheet.ImportRow(ssprocessds.view_safekeepingproofsheet.Rows[0]);
            }


            if (showreport)
            {
                frmLoading.Close();
                ShowVsReport(reportpath + "VSSafekeepingProof.rdlc", "ssprocessDS", rptprocessds.view_safekeepingproofsheet, SheetTitle + Environment.NewLine + CloseMessage);
                MakeVSReportPDF(reportpath + "VSSafekeepingProof.rdlc", "ssprocessDS", rptprocessds.view_safekeepingproofsheet, SheetTitle, ConfigurationManager.AppSettings["PDFFilesPath"] + bankfedid.TrimEnd().ToLower() + "proofsheet" + String.Format("{0:MM-dd-yy}", proofsheetclosedate.Date) + ".pdf");
            }
            if (makepdf)
            {
                //             MakeVSReportPDF(reportpath + "VSProofSheet.rdlc", "ssprocessDS", rptprocessds.view_proofsheet, SheetTitle + Environment.NewLine + CloseMessage, ConfigurationManager.AppSettings["PDFFilesPath"] + bankfedid.TrimEnd() + "ps" + String.Format("{0:MM-dd-yy}", proofsheetclosedate) + ".pdf");
            }
            return true;

        }



        public void CheckProofSheetRow(string rectype, string descrip)
        {
            // Check for missing rows
            string selectexpression = "rectype = '" + rectype + "'";
            DataRow[] foundRows;
            foundRows = rptprocessds.view_proofsheet.Select(selectexpression);
            if (foundRows.Length == 0)
            {

                rptprocessds.view_proofsheet.Rows.Add();
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].rectype = rectype;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].descrip = descrip;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].hundreds = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].fiftys = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].twentys = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].tens = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].fives = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].twos = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].ones = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].sba = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].halves = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].quarters = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].dimes = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].nickels = 0;
                rptprocessds.view_proofsheet[rptprocessds.view_proofsheet.Rows.Count - 1].pennies = 0;
            }
        }

        public void StartProofSheet()
        {
            int BankId = commonAppDataMethods.SelectBank();
            if (BankId > 0)
            {
                if (commonAppDataMethods.GetSingleDate("Enter Proof Sheet Date", 1000, 10))
                {
                    DateTime SheetDate = commonAppDataMethods.SelectedDate.Date;
                    string ProofSheetFileName = ConfigurationManager.AppSettings["PDFFilesPath"] + commonAppDataMethods.GetBankFedIdFromBankId(BankId).TrimEnd() + "ps" + String.Format("{0:MM-dd-yy}", SheetDate) + ".pdf";
                    FrmLoading frmLoading = new FrmLoading();
                    frmLoading.Show();
                    string BankFedid = commonAppDataMethods.GetBankFedIdFromBankId(BankId);
                    if (BankFedid.TrimEnd() != "")
                    {
                        RunProofSheet(BankId, SheetDate, false, true);
                        if (File.Exists(ProofSheetFileName))
                        {
                            List<string> AttachmentNames = new List<string>();
                            AttachmentNames.Add(ProofSheetFileName);
                            if (BankFedid.TrimEnd() == "RA111")
                            {
                                string CoinSheetName = CreateCoinSpreadSheet("4600", SheetDate, SheetDate);
                                if (File.Exists(CoinSheetName))
                                {
                                    AttachmentNames.Add(CoinSheetName);
                                }
                            }
                            commonAppDataMethods.SendBankEmail(AttachmentNames, BankFedid, "Proof Sheet " + commonAppDataMethods.GetBankNameFromBankId(BankId).TrimEnd() + " " + String.Format("{0:MM-dd-yy}", SheetDate) + ".pdf", "See attached");
                            frmLoading.Close();
                            wsgUtilities.wsgNotice("Email Sent");
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("Proof creation failed. Email not sent");

                        }
                    }
                    else
                    {
                        wsgUtilities.wsgNotice("Report Cancelled");
                    }
                }
                else
                {
                    wsgUtilities.wsgNotice("Report Cancelled");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Report Cancelled");
            }
        }

        public void StartSafekeepingProofSheet()
        {

            string bankfedid = commonAppDataMethods.SelectSmartsafeBankFedid();

            if (bankfedid != "")
            {
                if (commonAppDataMethods.GetSingleDate("Activity Date Date", 1000, 10))
                {


                    DateTime ReportDate = commonAppDataMethods.SelectedDate.Date;
                    RunSafekeepingProofSheet(ReportDate, true, bankfedid, true);
                }
                else
                {
                    wsgUtilities.wsgNotice("Report Cancelled");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Report Cancelled");
            }
        }

        public void StartSafekeepingActivityAnalysis()
        {
            string bankfedid = commonAppDataMethods.SelectSmartsafeBankFedid();

            if (bankfedid != "")
            {
                if (commonAppDataMethods.GetSingleDate("Activity Date Date", 1000, 10))
                {


                    DateTime ReportDate = commonAppDataMethods.SelectedDate.Date;
                    RunSafekeepingActivityAnalysis(ReportDate, true, bankfedid, true);
                }
                else
                {
                    wsgUtilities.wsgNotice("Report Cancelled");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Report Cancelled");
            }

        }


        public string CreateCoinSpreadSheet(string CoinCompany, DateTime ReportStartDate, DateTime ReportEndDate)
        {
            string FileName = "";

            string commandtext = "SELECT * FROM view_expandedcoindrop WHERE  ";
            commandtext += "  dropdate >= @startdate AND dropdate <= @enddate AND LEFT(store,4) = @coincompany ORDER By drivername, dropdate, store_name";
            rptprocessds.view_expandedcoindrop.Rows.Clear();
            ClearParameters();
            AddParms("@startdate", ReportStartDate.Date, "SQL");
            AddParms("@enddate", ReportEndDate.Date, "SQL");
            AddParms("@coincompany", CoinCompany, "SQL");
            FillData(rptprocessds, "view_expandedcoindrop", commandtext, CommandType.Text);

            if (rptprocessds.view_expandedcoindrop.Rows.Count > 0)
            {
                object misValue = System.Reflection.Missing.Value;
                Excel.Application objApp = new Excel.Application();
                Excel.Workbooks objBooks = objApp.Workbooks;
                Excel.Workbook objBook = objBooks.Add(misValue);
                Excel.Worksheet xlWorkSheet = objBook.Worksheets.get_Item(1);
                xlWorkSheet.Cells[1, 1] = "Safe and Sound Armed Courier";
                xlWorkSheet.Cells[1, 6] = "Coin Drop Listing " + String.Format("{0:MM/dd/yyyy}", ReportStartDate) +
                " thru " + String.Format("{0:MM/dd/yyyy}", ReportEndDate); ;
                xlWorkSheet.Cells[3, 1] = "Location";
                xlWorkSheet.Cells[3, 2] = "Date";
                xlWorkSheet.Cells[3, 3] = "Hundreds";
                xlWorkSheet.Cells[3, 4] = "Fiftys";
                xlWorkSheet.Cells[3, 5] = "Twentys";
                xlWorkSheet.Cells[3, 6] = "Tens";
                xlWorkSheet.Cells[3, 7] = "Fives";
                xlWorkSheet.Cells[3, 8] = "Ones";
                xlWorkSheet.Cells[3, 9] = "Quarters";
                xlWorkSheet.Cells[3, 10] = "Dimes";
                xlWorkSheet.Cells[3, 11] = "Nickels";
                xlWorkSheet.Cells[3, 12] = "Pennies";
                xlWorkSheet.Cells[3, 13] = "Total";
                object[,] arr = new object[rptprocessds.view_expandedcoindrop.Rows.Count, 13];
                for (int r = 0; r < rptprocessds.view_expandedcoindrop.Count; r++)
                {
                    arr[r, 0] = rptprocessds.view_expandedcoindrop[r].store_name;
                    arr[r, 1] = " " + String.Format("{0:MM/dd/yy}", rptprocessds.view_expandedcoindrop[r].dropdate.Date);
                    arr[r, 2] = rptprocessds.view_expandedcoindrop[r].hundreds;
                    arr[r, 3] = rptprocessds.view_expandedcoindrop[r].fiftys;
                    arr[r, 4] = rptprocessds.view_expandedcoindrop[r].twentys;
                    arr[r, 5] = rptprocessds.view_expandedcoindrop[r].tens;
                    arr[r, 6] = rptprocessds.view_expandedcoindrop[r].fives;
                    arr[r, 7] = rptprocessds.view_expandedcoindrop[r].ones;
                    arr[r, 8] = rptprocessds.view_expandedcoindrop[r].quarters;
                    arr[r, 9] = rptprocessds.view_expandedcoindrop[r].dimes;
                    arr[r, 10] = rptprocessds.view_expandedcoindrop[r].nickels;
                    arr[r, 11] = rptprocessds.view_expandedcoindrop[r].pennies;
                    arr[r, 12] = rptprocessds.view_expandedcoindrop[r].hundreds +
                      rptprocessds.view_expandedcoindrop[r].fiftys +
                      rptprocessds.view_expandedcoindrop[r].twentys +
                      rptprocessds.view_expandedcoindrop[r].tens +
                      rptprocessds.view_expandedcoindrop[r].fives +
                      rptprocessds.view_expandedcoindrop[r].ones +
                      rptprocessds.view_expandedcoindrop[r].quarters +
                      rptprocessds.view_expandedcoindrop[r].dimes +
                      rptprocessds.view_expandedcoindrop[r].nickels +
                      rptprocessds.view_expandedcoindrop[r].pennies;
                }
                // Import the array
                Excel.Range c1 = (Excel.Range)xlWorkSheet.Cells[4, 1];
                Excel.Range c2 = (Excel.Range)xlWorkSheet.Cells[rptprocessds.view_expandedcoindrop.Rows.Count + 3, 13];
                Excel.Range range = xlWorkSheet.get_Range(c1, c2);
                range.Value = arr;

                xlWorkSheet.Cells[rptprocessds.view_expandedcoindrop.Rows.Count + 6, 1] = "Totals";

                // Develop total formulas
                //100
                Excel.Range rng100 = xlWorkSheet.Range["C" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rng100.Formula = "=SUM(C4:C" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rng100.Font.Bold = true;
                //50
                Excel.Range rng50 = xlWorkSheet.Range["D" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rng50.Formula = "=SUM(D4:D" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rng50.Font.Bold = true;
                //20
                Excel.Range rng20 = xlWorkSheet.Range["E" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rng20.Formula = "=SUM(E4:E" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rng20.Font.Bold = true;
                //10
                Excel.Range rng10 = xlWorkSheet.Range["F" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rng10.Formula = "=SUM(F4:F" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rng10.Font.Bold = true;
                //5
                Excel.Range rng5 = xlWorkSheet.Range["G" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rng5.Formula = "=SUM(G4:G" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rng5.Font.Bold = true;
                //1
                Excel.Range rng1 = xlWorkSheet.Range["H" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rng1.Formula = "=SUM(H4:H" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rng1.Font.Bold = true;
                //Quarters
                Excel.Range rngQuarters = xlWorkSheet.Range["I" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rngQuarters.Formula = "=SUM(I4:I" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rngQuarters.Font.Bold = true;
                //Dimes
                Excel.Range rngDimes = xlWorkSheet.Range["J" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rngDimes.Formula = "=SUM(J4:J" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rngDimes.Font.Bold = true;
                //Nickels
                Excel.Range rngNickels = xlWorkSheet.Range["K" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rngNickels.Formula = "=SUM(K4:K" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rngNickels.Font.Bold = true;
                //Pennies
                Excel.Range rngPennies = xlWorkSheet.Range["L" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rngPennies.Formula = "=SUM(L4:L" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                rngPennies.Font.Bold = true;
                //Total
                Excel.Range rngTotal = xlWorkSheet.Range["M" + (rptprocessds.view_expandedcoindrop.Rows.Count + 6).ToString().TrimStart().TrimEnd()];
                rngTotal.Formula = "=SUM(M4:M" + (rptprocessds.view_expandedcoindrop.Rows.Count + 3).ToString().TrimStart().TrimEnd() + ")";
                // Format the numbers
                rngTotal.Font.Bold = true;
                Excel.Range dollarrange = (Excel.Range)xlWorkSheet.get_Range("B4", "H2000");
                dollarrange.NumberFormat = "###,###,###";
                // Format the numbers
                Excel.Range dollarsandcentsange = (Excel.Range)xlWorkSheet.get_Range("I4", "M2000");
                dollarsandcentsange.NumberFormat = "###,###,###.00";

                Excel.Range selectedRange = (Excel.Range)objApp.Selection;
                selectedRange.Columns.AutoFit();
                selectedRange.HorizontalAlignment = HorizontalAlignment.Right;
                // Excel.Range TitleRange = (Excel.Range)xlWorkSheet.get_Range("A3", "M3");
                // TitleRange.Style.HorizontalAlignment = HorizontalAlignment.Center;
                FileName = ConfigurationManager.AppSettings["FTPFilePath"] + CoinCompany + "-" + "CoinList" + String.Format("{0:MM-dd-yy}", ReportStartDate) + ".xls";
                if (File.Exists(FileName))
                {
                    File.Delete(FileName);
                }
                objBook.SaveAs(FileName, Excel.XlFileFormat.xlExcel5,
                misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                objBook.Close(true, misValue, misValue);
                objApp.Quit();
                releaseObject(xlWorkSheet);
                releaseObject(objBook);
                releaseObject(objApp);
            }
            return FileName;
        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }

        public void StoreServiceDays()
        {
            string compcode = commonAppDataMethods.SelectCompany();
            string commandtext = "";
            if (compcode.TrimEnd() != "")
            {

                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                string ReportTitle = "Safe and Sound Armed Courier - Service Days - " + commonAppDataMethods.GetCompanyName(compcode).TrimEnd();
                commandtext = "SELECT * FROM view_storeservicedays WHERE LEFT(storecode,4) = @compcode";
                commandtext += " order by storecode ";

                ssprocessds.view_storeservicedays.Clear();
                AddParms("@compcode", compcode, "SQL");
                FillData(ssprocessds, "view_storeservicedays", commandtext, CommandType.Text);
                ShowVsReport(reportpath + "StoreServiceDays.rdlc", "ssprocessDs", ssprocessds.view_storeservicedays, ReportTitle);
                frmLoading.Close();

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }
        public void PickupRegister()
        {
            string compcode = commonAppDataMethods.SelectCompany();
            string commandtext = "";
            if (compcode.TrimEnd() != "")
            {
                if (commonAppDataMethods.GetTwoDates("Enter Pickup Dates"))
                {
                    FrmLoading frmLoading = new FrmLoading();
                    frmLoading.Show();
                    DateTime ReportStartDate = commonAppDataMethods.SelectedStartDate;
                    DateTime ReportEndDate = commonAppDataMethods.SelectedEndDate;
                    string ReportTitle = "Pickup Register - " + commonAppDataMethods.GetCompanyName(compcode).TrimEnd() + " - " + String.Format("{0:MM/dd/yy}", ReportStartDate) + " - " + String.Format("{0:MM/dd/yy}", ReportEndDate);
                    commandtext = "SELECT * FROM view_expandedhhpickup WHERE LEFT(storecode,4)  = @compcode AND pickupdate > dateadd(d, -1, @startdate)  AND pickupdate < DATEADD(d,1,@enddate)";
                    commandtext += " order by storecode, pickupdate ";

                    ssprocessds.view_expandedhhpickup.Clear();
                    AddParms("@startdate", ReportStartDate.Date, "SQL");
                    AddParms("@enddate", ReportEndDate.Date, "SQL");
                    AddParms("@compcode", compcode, "SQL");
                    FillData(ssprocessds, "view_expandedhhpickup", commandtext, CommandType.Text);
                    ShowVsReport(reportpath + "PickupRegister.rdlc", "ssprocessDs", ssprocessds.view_expandedhhpickup, ReportTitle);
                    frmLoading.Close();
                }
                else
                {
                    wsgUtilities.wsgNotice("Operation Cancelled");
                }

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }


        public void DropSchedulePerformanceDriver()
        {
            string driver = commonAppDataMethods.SelectDriver();
            string commandtext = "";
            if (driver.TrimEnd() != "")
            {
                if (commonAppDataMethods.GetTwoDates("Enter Pickup Dates"))
                {

                    FrmLoading frmLoading = new FrmLoading();
                    frmLoading.Show();

                    DateTime ReportStartDate = commonAppDataMethods.SelectedStartDate;
                    DateTime ReportEndDate = commonAppDataMethods.SelectedEndDate;
                    string ReportTitle = "Drop Schedule Performance - " + commonAppDataMethods.GetDriverName(driver).TrimEnd() + " - " + String.Format("{0:MM/dd/yy}", ReportStartDate) + " - " + String.Format("{0:MM/dd/yy}", ReportEndDate);
                    commandtext = "SELECT * FROM view_expandedslip s WHERE driver_1  = @driver AND s.slip_date  > @startdate ";
                    commandtext += "AND s.slip_date < @enddate  AND NOT EXISTS ";
                    commandtext += " (SELECT distinct storecode, pickupdate FROM   hhpickup p WHERE  p.storecode = s.storecode  AND  p.pickupdate > dateadd(d, -1, s.slip_date)  AND pickupdate < DATEADD(d,1, s.slip_date))";
                    commandtext += " order by s.slip_date, s.storecode ";

                    ssprocessds.view_expandedslip.Rows.Clear();
                    AddParms("@startdate", ReportStartDate.Date, "SQL");
                    AddParms("@enddate", ReportEndDate.Date, "SQL");
                    AddParms("@driver", driver, "SQL");
                    FillData(ssprocessds, "view_expandedslip", commandtext, CommandType.Text);
                    frmLoading.Close();
                    ShowVsReport(reportpath + "DropSchedulePerformance.rdlc", "ssprocessDs", ssprocessds.view_expandedslip, ReportTitle);

                }
                else
                {
                    wsgUtilities.wsgNotice("Operation Cancelled");
                }

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }


        public void DropSchedulePerformanceCustomer()
        {
            string compcode = commonAppDataMethods.SelectCompany();
            string commandtext = "";
            if (compcode.TrimEnd() != "")
            {
                if (commonAppDataMethods.GetTwoDates("Enter Pickup Dates"))
                {

                    FrmLoading frmLoading = new FrmLoading();
                    frmLoading.Show();

                    DateTime ReportStartDate = commonAppDataMethods.SelectedStartDate;
                    DateTime ReportEndDate = commonAppDataMethods.SelectedEndDate;
                    string ReportTitle = "Drop Schedule Performance - " + commonAppDataMethods.GetCompanyName(compcode).TrimEnd() + " - " + String.Format("{0:MM/dd/yy}", ReportStartDate) + " - " + String.Format("{0:MM/dd/yy}", ReportEndDate);
                    commandtext = "SELECT * FROM view_expandedslip s WHERE LEFT(s.storecode,4)  = @compcode AND s.slip_date  > @startdate ";
                    commandtext += "AND s.slip_date < @enddate  AND NOT EXISTS ";
                    commandtext += " (SELECT distinct storecode, pickupdate FROM   hhpickup p WHERE  p.storecode = s.storecode  AND  p.pickupdate > dateadd(d, -1, s.slip_date)  AND pickupdate < DATEADD(d,1, s.slip_date))";
                    commandtext += " order by s.slip_date, s.storecode ";

                    ssprocessds.view_expandedslip.Rows.Clear();
                    AddParms("@startdate", ReportStartDate.Date, "SQL");
                    AddParms("@enddate", ReportEndDate.Date, "SQL");
                    AddParms("@compcode", compcode, "SQL");
                    FillData(ssprocessds, "view_expandedslip", commandtext, CommandType.Text);
                    frmLoading.Close();
                    ShowVsReport(reportpath + "DropSchedulePerformance.rdlc", "ssprocessDs", ssprocessds.view_expandedslip, ReportTitle);

                }
                else
                {
                    wsgUtilities.wsgNotice("Operation Cancelled");
                }

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }

        public void CoinOrderBillingRegister()
        {
            string compcode = commonAppDataMethods.SelectCompany();
            string commandtext = "";
            if (compcode.TrimEnd() != "")
            {

                FrmLoading frmLoading = new FrmLoading();
                int[] monthandyear = commonAppDataMethods.GetMonthAndYear();
                if (monthandyear[0] != 0)
                {
                    frmLoading.Show();
                    DateTime ReportDate = commonAppDataMethods.SelectedDate.Date;
                    ReportTitle = "Coin Order Register " + commonAppDataMethods.GetCompanyName(compcode).TrimEnd() + " - " + monthandyear[0].ToString() + "/" + monthandyear[1].ToString();
                    string CommandString = "SELECT * FROM  view_expandedcoindrop WHERE LEFT(store,4) = @company AND MONTH(dropdate) = @rptmonth AND YEAR(dropdate) = @rptyear ORDER BY store,dropdate";
                    frmLoading.Show();
                    ssprocessds.view_expandedcoindrop.Clear();
                    ClearParameters();
                    AddParms("@company", compcode, "SQL");
                    AddParms("@rptmonth", monthandyear[0], "SQL");
                    AddParms("@rptyear", monthandyear[1], "SQL");
                    FillData(ssprocessds, "view_expandedcoindrop", CommandString, CommandType.Text);
                    frmLoading.Hide();
                    if (ssprocessds.view_expandedcoindrop.Rows.Count > 0)
                    {
                        ShowVsReport(reportpath + "CoinOrderBillingList.rdlc", "ssprocessDs", ssprocessds.view_expandedcoindrop, ReportTitle);

                    }
                    else
                    {
                        wsgUtilities.wsgNotice("Operation Cancelled");
                    }

                }
                else
                {
                    wsgUtilities.wsgNotice("Operation Cancelled");
                }
            }
        }
        public void SignatureBillingRegister()
        {
            FrmLoading frmLoading = new FrmLoading();
            int[] monthandyear = commonAppDataMethods.GetMonthAndYear();
            if (monthandyear[0] != 0)
            {

                DateTime ReportDate = commonAppDataMethods.GetLastDateOfMonth(Convert.ToDateTime(monthandyear[0].ToString() + "/28/" + monthandyear[1].ToString()));

                ReportTitle = "Signature Billing Register " + String.Format("{0:MM/dd/yyyy}", ReportDate);
                string CommandString = "SELECT * FROM   view_signaturebillsummary WHERE MONTH(inv_date) = @reportmonth AND YEAR(inv_date) = @reportyear ORDER BY inv_number, servicecode";
                frmLoading.Show();
                ssprocessds.view_SignatureBillSummary.Rows.Clear();
                ClearParameters();
                AddParms("@reportmonth", monthandyear[0], "SQL");
                AddParms("@reportyear", monthandyear[1], "SQL");

                FillData(ssprocessds, "view_signaturebillsummary", CommandString, CommandType.Text);
                frmLoading.Hide();
                if (ssprocessds.view_SignatureBillSummary.Rows.Count > 0)
                {

                    ShowVsReport(reportpath + "VSSignatureBillingRegister.rdlc", "ssprocessDs", ssprocessds.view_SignatureBillSummary, ReportTitle);
                }
                else
                {
                    wsgUtilities.wsgNotice("No Matching Records");
                }
            }
        }

        public void CreateSignatureBillingRegistePDF(DateTime ReportDate, string pdfname)
        {

            ReportTitle = "Signature Billing Register " + String.Format("{0:MM/dd/yyyy}", ReportDate);
            string CommandString = "SELECT * FROM   view_signaturebillsummary WHERE CONVERT(date, inv_date) = @reportdate ORDER BY inv_number, servicecode";
            ssprocessds.view_SignatureBillSummary.Rows.Clear();
            ClearParameters();
            AddParms("@reportdate", ReportDate, "SQL");
            FillData(ssprocessds, "view_signaturebillsummary", CommandString, CommandType.Text);
            if (ssprocessds.view_SignatureBillSummary.Rows.Count > 0)
            {
                MakeVSReportPDF(reportpath + "VSSignatureBillingRegister.rdlc", "ssprocessDs", ssprocessds.view_SignatureBillSummary, ReportTitle, pdfname);
            }
        }




        public void ATMFillRegister()
        {
            FrmLoading frmLoading = new FrmLoading();
            if (commonAppDataMethods.GetTwoDates("Enter Report Dates"))
            {
                frmLoading.Show();
                DateTime ReportStartDate = commonAppDataMethods.SelectedStartDate;
                DateTime ReportEndDate = commonAppDataMethods.SelectedEndDate;

                ReportTitle = "ATM Fill Register " + String.Format("{0:MM/dd/yyyy}", ReportStartDate) +
                " thru " + String.Format("{0:MM/dd/yyyy}", ReportEndDate);
                string CommandString = "SELECT * FROM  view_atmdropnetwithstore WHERE CONVERT(date, dropdate) between @startdate AND @enddate ORDER BY dropdate, atmid";
                frmLoading.Show();
                ssprocessds.view_ATMDropNetWithStore.Rows.Clear();
                ClearParameters();
                AddParms("@startdate", ReportStartDate, "SQL");
                AddParms("@enddate", ReportEndDate, "SQL");
                FillData(ssprocessds, "view_ATMDropNetWithStore", CommandString, CommandType.Text);
                ShowVsReport(reportpath + "VSATMFillRegister.rdlc", "ssprocessDs", ssprocessds.view_ATMDropNetWithStore, ReportTitle);
                frmLoading.Hide();


            }
        }

        public void BagPickupList()
        {

            ReportName = "VSPickupListByCompany.rdlc";
            ReportTitle = "Pickup List";

            while (true)
            {

                FrmPickupListing frmPickupListing = new FrmPickupListing();
                frmPickupListing.ShowDialog();
                if (frmPickupListing.cancel)
                {
                    break;
                }

                if (frmPickupListing.cont)
                {
                    FrmLoading frmLoading = new FrmLoading();
                    frmLoading.Show();
                    ReportTitle = ReportTitle + " " + commonAppDataMethods.GetDriverName(frmPickupListing.reportdriver).TrimEnd() + " " + String.Format("{0:MM/dd/yyyy}", frmPickupListing.reportdate);


                    string commandtext = "SELECT * FROM view_expandedhhpickup WHERE CAST(pickupdate AS DATE) = @pickupdate ";
                    commandtext += "AND driver = @driver ";

                    switch (frmPickupListing.reportsequence)
                    {
                        case "Company":
                            {
                                ReportName = "VSPickupListByCompany.rdlc";
                                ReportTitle = "Pickup List";
                                commandtext += " ORDER BY  companyname, moneycentername";
                                break;
                            }
                        case "Money Center":
                            {
                                ReportTitle = "Money Center Pickup List";

                                ReportName = "VSPickupListByMoneyCenter.rdlc";
                                commandtext += " ORDER BY  moneycentername, companyname";
                                break;
                            }
                        default:
                            {
                                ReportTitle = "Pickup Manifest";
                                ReportName = "VSPickupManifest.rdlc";
                                commandtext += " ORDER BY  driver, store_name";
                                break;
                            }
                    }
                    ReportTitle = ReportTitle + " " + commonAppDataMethods.GetDriverName(frmPickupListing.reportdriver).TrimEnd() + " " + String.Format("{0:MM/dd/yyyy}", frmPickupListing.reportdate);
                    rptprocessds.view_expandedhhpickup.Rows.Clear();
                    ClearParameters();
                    AddParms("@pickupdate", frmPickupListing.reportdate, "SQL");
                    AddParms("@driver", frmPickupListing.reportdriver, "SQL");
                    FillData(rptprocessds, "view_expandedhhpickup", commandtext, CommandType.Text);

                    if (rptprocessds.view_expandedhhpickup.Rows.Count > 0)
                    {
                        frmLoading.Close();
                        ShowVsReport(reportpath + ReportName, "ssprocessDs", rptprocessds.view_expandedhhpickup, ReportTitle);
                    }
                    else
                    {
                        frmLoading.Close();
                        wsgUtilities.wsgNotice("There are no matching records");

                    }
                }
                else
                {
                    wsgUtilities.wsgNotice("Operation Cancelled");
                }
            }
        }



        public void PrintVSStoreMoneyCenters()
        {
            ClearParameters();
            reportsDs.view_expandedstoremoneycenter.Rows.Clear();

            string CommandString = "SELECT * FROM .view_expandedstoremoneycenter";

            FillData(reportsDs, "view_expandedstoremoneycenter", CommandString, CommandType.Text);

            if (reportsDs.view_expandedstoremoneycenter.Rows.Count > 0)
            {
                string ReportTitle = "Store Money Centers";
                ShowVsReport(reportpath + "VSStoreMoneyCenters.rdlc", "reportsDs", reportsDs.view_expandedstoremoneycenter, ReportTitle);

            }
            else
            {
                wsgUtilities.wsgNotice("There are no matching records");

            }
        }

        public void PrintVSPickupMoneyCenters()
        {
            ClearParameters();
            reportsDs.view_expandedhhpickup.Rows.Clear();

            string CommandString = "SELECT * FROM  view_expandedhhpickup WHERE manid =0";

            FillData(reportsDs, "view_expandedhhpickup", CommandString, CommandType.Text);

            if (reportsDs.view_expandedhhpickup.Rows.Count > 0)
            {
                string ReportTitle = "PickupMoney Centers";
                ShowVsReport(reportpath + "VSPickupMoneyCenters.rdlc", "reportsDs", reportsDs.view_expandedhhpickup, ReportTitle);

            }
            else
            {
                wsgUtilities.wsgNotice("There are no matching records");

            }
        }

        public void PrintVSCompanyMoneyCenters()
        {
            ClearParameters();
            reportsDs.view_expandedcompmoneycenter.Rows.Clear();

            string CommandString = "SELECT * FROM view_expandedcompmoneycenter";

            FillData(reportsDs, "view_expandedcompmoneycenter", CommandString, CommandType.Text);

            if (reportsDs.view_expandedcompmoneycenter.Rows.Count > 0)
            {
                string ReportTitle = "Company Money Centers";
                ShowVsReport(reportpath + "VSCompanyMoneyCenters.rdlc", "reportsDs", reportsDs.view_expandedcompmoneycenter, ReportTitle);

            }
            else
            {
                wsgUtilities.wsgNotice("There are no matching records");

            }

        }

        public void PrintVSCustomerInquiry(string Customer, string Centername)
        {
            ClearParameters();
            ssprocessds.view_manifestbagsearch.Rows.Clear();
            AddParms("@customer", Customer.TrimEnd(), "SQL");
            AddParms("@centername", Centername.TrimEnd(), "SQL");

            string CommandString = "SELECT * FROM view_manifestbagsearch WHERE postingdate > GETDATE() - 730 AND rtrim(customer) = @customer AND rtrim(centername) = @centername";

            FillData(ssprocessds, "view_manifestbagsearch", CommandString, CommandType.Text);

            if (ssprocessds.view_manifestbagsearch.Rows.Count > 0)
            {
                string ReportTitle = "Customer Inquiry " + Customer.TrimEnd() + " - " + Centername.TrimEnd();
                ShowVsReport(reportpath + "VSCustomerManifestList.rdlc", "ssprocessDs", ssprocessds.view_manifestbagsearch, ReportTitle);

            }
        }

        public void PrintVSManifest(int CurrentManifestid, string printmode)
        {
            ClearParameters();
            rptprocessds.view_manifestbagsearch.Rows.Clear();
            AddParms("@currentmanifestid", CurrentManifestid, "SQL");
            string commandtext = "SELECT * FROM view_manifestbagsearch where idcol = @currentmanifestid ORDER BY  lineid";
            FillData(rptprocessds, "view_manifestbagsearch", commandtext, CommandType.Text);

            if (rptprocessds.view_manifestbagsearch.Rows.Count > 0)
            {
                string ReportTitle = "Bag Manifest";
                List<ReportParameter> reportparameters = new List<ReportParameter>();
                string citystatezip = ConfigurationManager.AppSettings["ManifestCourierCity"].TrimEnd() + ", " + ConfigurationManager.AppSettings["ManifestCourierState"] + " " + ConfigurationManager.AppSettings["ManifestCourierZip"];
                string phonefax = "Phone " + ConfigurationManager.AppSettings["ManifestCourierPhone"] + " Fax" + ConfigurationManager.AppSettings["ManifestCourierFax"];
                reportparameters.Add(new ReportParameter("RptTitle", ReportTitle));
                reportparameters.Add(new ReportParameter("CourierName", ConfigurationManager.AppSettings["ManifestCourierName"]));
                reportparameters.Add(new ReportParameter("CourierStreet", ConfigurationManager.AppSettings["ManifestCourierStreet"]));
                reportparameters.Add(new ReportParameter("CourierCityStateZip", citystatezip));
                reportparameters.Add(new ReportParameter("CourierPhoneFax", phonefax));

                FrmVSReportViewer frmVSReportViewer = new FrmVSReportViewer();
                frmVSReportViewer.Text = ReportTitle;
                frmVSReportViewer.reportViewerVSReport.ProcessingMode = ProcessingMode.Local;
                frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Clear();
                if (rptprocessds.view_manifestbagsearch[0].client.Substring(0, 3).ToUpper() == "MIX")
                {
                    frmVSReportViewer.reportViewerVSReport.LocalReport.ReportPath = reportpath + "VSManifestMixedCustomers.rdlc";

                }
                else
                {
                    frmVSReportViewer.reportViewerVSReport.LocalReport.ReportPath = reportpath + "VSManifestSingleCustomer.rdlc";

                }

                // Note: The report must have a parameter with the name RptTitle
                // Use that parameter in the title textbox
                frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(reportparameters);
                ReportDataSource rds = new ReportDataSource();
                rds.Name = "ssprocessDs";
                rds.Value = rptprocessds.view_manifestbagsearch;
                frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Add(rds);
                frmVSReportViewer.reportViewerVSReport.RefreshReport();
                if (printmode == "V")
                {
                    frmVSReportViewer.ShowDialog();
                }
                else
                {
                    ReportPrintDocument reportPrintDocument = new ReportPrintDocument(frmVSReportViewer.reportViewerVSReport.LocalReport);

                }
            }
            else
            {
                wsgUtilities.wsgNotice("No matching records");
            }
        }

        public void RunDriverCoinAnalysis()
        {
            DateTime ReportStartDate;
            DateTime ReportEndDate;
            String ReportTitle;
            FrmLoading frmLoading = new FrmLoading();

            if (commonAppDataMethods.GetTwoDates("Enter Report Dates"))
            {
                frmLoading.Show();
                ReportStartDate = commonAppDataMethods.SelectedStartDate;
                ReportEndDate = commonAppDataMethods.SelectedEndDate;

                ReportTitle = "Driver Coin Analysis " + String.Format("{0:MM/dd/yyyy}", ReportStartDate) +
                " thru " + String.Format("{0:MM/dd/yyyy}", ReportEndDate);
                string CommandString = "SELECT * FROM  view_totalinvtrans  where RTRIM(bankfedid) = '10000000' AND driver <> '  ' AND CONVERT(date, trandate) between @startdate AND @enddate ORDER BY trandate, trandescrip";
                frmLoading.Show();
                ssprocessds.view_totalinvtrans.Rows.Clear();
                ClearParameters();
                AddParms("@startdate", ReportStartDate, "SQL");
                AddParms("@enddate", ReportEndDate, "SQL");
                FillData(ssprocessds, "view_totalinvtrans", CommandString, CommandType.Text);
                ShowVsReport(reportpath + "VSDriverCoinAnalysis.rdlc", "ssprocessDs", ssprocessds.view_totalinvtrans, ReportTitle);
                frmLoading.Hide();

            }
        }
        public void DriverStopNotes()
        {
            DateTime ReportStartDate;
            DateTime ReportEndDate;
            String ReportTitle;
            FrmLoading frmLoading = new FrmLoading();

            if (commonAppDataMethods.GetTwoDates("Enter Report Dates"))
            {
                frmLoading.Show();
                ReportStartDate = commonAppDataMethods.SelectedStartDate;
                ReportEndDate = commonAppDataMethods.SelectedEndDate;

                ReportTitle = "Driver Stop Notes - " + String.Format("{0:MM/dd/yyyy}", ReportStartDate) +
                " thru " + String.Format("{0:MM/dd/yyyy}", ReportEndDate);
                string CommandString = "SELECT * FROM view_expandedschedulenote where  CONVERT(date, slipdate) between @startdate AND @enddate ORDER BY drivername, storecode";
                frmLoading.Show();
                ssprocessds.view_expandedschedulenote.Rows.Clear();
                ClearParameters();
                AddParms("@startdate", ReportStartDate, "SQL");
                AddParms("@enddate", ReportEndDate, "SQL");
                FillData(ssprocessds, "view_expandedschedulenote", CommandString, CommandType.Text);
                ShowVsReport(reportpath + "DriverStopNotes.rdlc", "ssprocessDs", ssprocessds.view_expandedschedulenote, ReportTitle);

                frmLoading.Hide();

            }
            else
            {
                wsgUtilities.wsgNotice("Action cancelled");
            }
        }
        public void RunCashReconciliation()
        {
            DateTime ReportStartDate;
            DateTime ReportEndDate;
            String ReportTitle;
            FrmLoading frmLoading = new FrmLoading();

            if (commonAppDataMethods.GetTwoDates("Enter Report Dates"))
            {
                frmLoading.Show();
                ReportStartDate = commonAppDataMethods.SelectedStartDate;
                ReportEndDate = commonAppDataMethods.SelectedEndDate;

                ReportTitle = "Cash Reconciliation - " + String.Format("{0:MM/dd/yyyy}", ReportStartDate) +
                " thru " + String.Format("{0:MM/dd/yyyy}", ReportEndDate);
                string CommandString = "SELECT * FROM  view_totalinvtrans  where RTRIM(bankfedid) = '10000000' AND  CONVERT(date, trandate) between @startdate AND @enddate ORDER BY trandate, trandescrip";
                frmLoading.Show();
                ssprocessds.view_totalinvtrans.Rows.Clear();
                ClearParameters();
                AddParms("@startdate", ReportStartDate, "SQL");
                AddParms("@enddate", ReportEndDate, "SQL");
                FillData(ssprocessds, "view_totalinvtrans", CommandString, CommandType.Text);
                for (int r = 0; r < ssprocessds.view_totalinvtrans.Rows.Count; r++)
                {
                    if (ssprocessds.view_totalinvtrans[r].trantype == "I")
                    {
                        ssprocessds.view_totalinvtrans[r].transtotal = ssprocessds.view_totalinvtrans[r].transtotal * -1;
                    }
                }
                // Get balance rows

                CommandString = "SELECT * FROM balance   where RTRIM(bankfedid) = '10000000' AND  CONVERT(date, postdate) between @startdate AND @enddate";
                ssprocessds.balance.Rows.Clear();
                ClearParameters();
                AddParms("@startdate", ReportStartDate, "SQL");
                AddParms("@enddate", ReportEndDate, "SQL");
                FillData(ssprocessds, "balance", CommandString, CommandType.Text);
                for (int r = 0; r < ssprocessds.balance.Rows.Count; r++)
                {
                    ssprocessds.view_totalinvtrans.Rows.Add();
                    ssprocessds.view_totalinvtrans[ssprocessds.view_totalinvtrans.Rows.Count - 1].trandescrip = " Closing Balance";
                    ssprocessds.view_totalinvtrans[ssprocessds.view_totalinvtrans.Rows.Count - 1].trandate = ssprocessds.balance[r].postdate;
                    ssprocessds.view_totalinvtrans[ssprocessds.view_totalinvtrans.Rows.Count - 1].transtotal = ssprocessds.balance[r].hundreds + ssprocessds.balance[r].fiftys + ssprocessds.balance[r].twentys + ssprocessds.balance[r].tens;
                    ssprocessds.view_totalinvtrans[ssprocessds.view_totalinvtrans.Rows.Count - 1].transtotal += ssprocessds.balance[r].fives + ssprocessds.balance[r].twos + ssprocessds.balance[r].ones + ssprocessds.balance[r].halves;
                    ssprocessds.view_totalinvtrans[ssprocessds.view_totalinvtrans.Rows.Count - 1].transtotal += ssprocessds.balance[r].quarters + ssprocessds.balance[r].dimes + ssprocessds.balance[r].nickels + ssprocessds.balance[r].pennies;

                }


                ShowVsReport(reportpath + "VSCashReconciliation.rdlc", "ssprocessDs", ssprocessds.view_totalinvtrans, ReportTitle);
                frmLoading.Hide();


            }
        }

        public void DriverListing()
        {
            string commandtext = "SELECT * from view_expandeddriver  order by number";
            ssprocessDs.view_expandeddriver.Clear();
            FillData(ssprocessDs, "view_expandeddriver", commandtext, CommandType.Text);
            ShowVsReport(reportpath + "VSDriverListing.rdlc", "ssprocessDs", ssprocessDs.view_expandeddriver, "Driver Listing");
        }

        public bool RunEventLog()
        {
            DateTime ReportStartDate;
            DateTime ReportEndDate;
            String ReportTitle;
            FrmLoading frmLoading = new FrmLoading();

            if (commonAppDataMethods.GetTwoDates("Enter Report Dates"))
            {
                frmLoading.Show();
                ReportStartDate = commonAppDataMethods.SelectedStartDate;
                ReportEndDate = commonAppDataMethods.SelectedEndDate;

                ReportTitle = "Event Log Listing - " + String.Format("{0:MM/dd/yyyy}", ReportStartDate) +
                " thru " + String.Format("{0:MM/dd/yyyy}", ReportEndDate);
                string CommandString = "SELECT * FROM eventlog  where CONVERT(date, eventtime) between @startdate AND @enddate ORDER BY eventcode, eventtime";
                frmLoading.Show();
                // Locate the eventlog records
                sysdatads.eventlog.Rows.Clear();
                ClearParameters();
                AddParms("@startdate", ReportStartDate, "SQL");
                AddParms("@enddate", ReportEndDate, "SQL");
                FillData(sysdatads, "eventlog", CommandString, CommandType.Text);
                ShowVsReport(reportpath + "VSEventLogAnalysis.rdlc", "SysdataDs", sysdatads.eventlog, ReportTitle);
                frmLoading.Hide();

            }

            return true;
        }

        public void MakeVSReportPDF(string ReportPath, string DataSetName, DataTable dt, string RptTitle, string FilePath)
        {
            Microsoft.Reporting.WinForms.Warning[] warnings;

            string[] streamids;

            string mimeType;

            string encoding;

            string extension;
            FrmVSReportViewer frmVSReportViewer = new FrmVSReportViewer();
            frmVSReportViewer.Text = RptTitle;
            frmVSReportViewer.reportViewerVSReport.ProcessingMode = ProcessingMode.Local;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Clear();
            frmVSReportViewer.reportViewerVSReport.LocalReport.ReportPath = ReportPath;
            // Note: The report must have a parameter with the name RptTitle
            // Use that parameter in the title textbox
            ReportParameter p = new ReportParameter("RptTitle", RptTitle);
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { p });
            ReportDataSource rds = new ReportDataSource();
            rds.Name = DataSetName;
            rds.Value = dt;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Add(rds);
            frmVSReportViewer.reportViewerVSReport.RefreshReport();

            byte[] bytes = frmVSReportViewer.reportViewerVSReport.LocalReport.Render(
                     "PDF", null, out mimeType, out encoding,
                      out extension,
                     out streamids, out warnings);
            FileStream fs = new FileStream(FilePath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        public void SendVSReportToPrint(string ReportPath, string DataSetName, DataTable dt, string RptTitle, string FilePath)
        {
            Microsoft.Reporting.WinForms.Warning[] warnings;

            string[] streamids;

            string mimeType;

            string encoding;

            string extension;
            FrmVSReportViewer frmVSReportViewer = new FrmVSReportViewer();
            frmVSReportViewer.Text = RptTitle;
            frmVSReportViewer.reportViewerVSReport.ProcessingMode = ProcessingMode.Local;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Clear();
            frmVSReportViewer.reportViewerVSReport.LocalReport.ReportPath = ReportPath;
            // Note: The report must have a parameter with the name RptTitle
            // Use that parameter in the title textbox
            ReportParameter p = new ReportParameter("RptTitle", RptTitle);
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { p });
            ReportDataSource rds = new ReportDataSource();
            rds.Name = DataSetName;
            rds.Value = dt;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Add(rds);
            frmVSReportViewer.reportViewerVSReport.RefreshReport();
            ReportPrintDocument rptPrintDocument = new ReportPrintDocument(frmVSReportViewer.reportViewerVSReport.LocalReport);
            rptPrintDocument.Print();
        }
        public void MakeVSReportXLS(string ReportPath, string DataSetName, DataTable dt, string RptTitle, string FilePath)
        {
            Microsoft.Reporting.WinForms.Warning[] warnings;

            string[] streamids;

            string mimeType;

            string encoding;

            string extension;
            FrmVSReportViewer frmVSReportViewer = new FrmVSReportViewer();
            frmVSReportViewer.Text = RptTitle;
            frmVSReportViewer.reportViewerVSReport.ProcessingMode = ProcessingMode.Local;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Clear();
            frmVSReportViewer.reportViewerVSReport.LocalReport.ReportPath = ReportPath;
            // Note: The report must have a parameter with the name RptTitle
            // Use that parameter in the title textbox
            ReportParameter p = new
            ReportParameter("RptTitle", RptTitle);
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { p });
            ReportDataSource rds = new ReportDataSource();
            rds.Name = DataSetName;
            rds.Value = dt;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Add(rds);
            frmVSReportViewer.reportViewerVSReport.RefreshReport();

            byte[] bytes = frmVSReportViewer.reportViewerVSReport.LocalReport.Render(
                     "EXCEL", null, out mimeType, out encoding,
                      out extension,
                     out streamids, out warnings);
            FileStream fs = new FileStream(FilePath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }

        public void SpecialChargeListing()
        {
            if (commonAppDataMethods.GetTwoDates("Enter Service Dates"))
            {
                string commandtext = "";
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                DateTime ReportStartDate = commonAppDataMethods.SelectedStartDate;
                DateTime ReportEndDate = commonAppDataMethods.SelectedEndDate;
                string ReportTitle = "Special Charge Listing - " + String.Format("{0:MM/dd/yy}", ReportStartDate) + " - " + String.Format("{0:MM/dd/yy}", ReportEndDate);
                commandtext = "SELECT * FROM view_expandedspecialcharge WHERE chgdate > dateadd(d, -1, @startdate)  AND chgdate < DATEADD(d,1,@enddate)";
                commandtext += " order by storecode, chgdate ";

                ssprocessds.view_expandedspecialcharge.Rows.Clear();
                AddParms("@startdate", ReportStartDate.Date, "SQL");
                AddParms("@enddate", ReportEndDate.Date, "SQL");
                FillData(ssprocessds, "view_expandedspecialcharge", commandtext, CommandType.Text);
                ShowVsReport(reportpath + "VSSpecialChargeListing.rdlc", "ssprocessDs", ssprocessds.view_expandedspecialcharge, ReportTitle);
                frmLoading.Close();
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }
        public void SafeandSoundSmartSafeActivity()
        {
            DateTime reportdate = DateTime.Now.Date;
            bool cont = true;
            if (commonAppDataMethods.GetSingleDate("Report Date", 100, 100))
            {
                reportdate = commonAppDataMethods.SelectedDate;
            }
            else
            {
                cont = false;
            }
            if (cont)
            {
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();

                ReportTitle = "Safe and Sound Smart Safe Activity - " + String.Format("{0:MM/dd/yyyy}", reportdate);
                mySQLDataMethods.FillSmartsafetrans(reportdate);
                frmLoading.Close();
                mysqldatads.safetrans.Rows.Clear();
                for (int r = 0; r < mySQLDataMethods.mysqlds.safetrans.Rows.Count; r++)
                {
                    if (mySQLDataMethods.mysqlds.safetrans[r].eventcode.Contains("Cash In") || mySQLDataMethods.mysqlds.safetrans[r].eventcode.Contains("Stacker removed"))
                    {
                        mysqldatads.safetrans.ImportRow(mySQLDataMethods.mysqlds.safetrans[r]);
                    }
                }
                if (mysqldatads.safetrans.Rows.Count > 0)
                {
                    ShowVsReport(reportpath + "SafeActivity.rdlc", "mysqldataDs", mysqldatads.safetrans, ReportTitle);
                }
                else
                {
                    wsgUtilities.wsgNotice("No Matching Records");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }

        public void ManifestDetails(int manifestid)
        {
            string ReportPath = reportpath + "DriverBagCount.rdlc";
            string ReportTitle = " Manifest Details " + manifestid.ToString().TrimStart().TrimEnd();

            string commandtext = "  SELECT * from view_expandedhhpickup where manid = @manid ORDER BY 1 ";
            sscloseds.view_expandedhhpickup.Rows.Clear();
            ClearParameters();
            AddParms("@manid", manifestid, "SQL");
            FillData(ssprocessds, "view_expandedhhpickup", commandtext, CommandType.Text);
            ShowVsReport(ReportPath , "DataSet1", ssprocessds.view_expandedhhpickup, ReportTitle);

        }
        public void BagCount()
        {
            DateTime reportdate = new DateTime();
            bool cont = true;
            int[] monthandyear = commonAppDataMethods.GetMonthAndYear();
            if (monthandyear[0] == 0)
            {
                cont = false;
            }

            int postingmonth = monthandyear[0];
            int postingyear = monthandyear[1];
            string postingmonthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(postingmonth);
            if (cont)
            {
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                string ReportPath = reportpath + "BagCount.rdl";
                string RptTitle = "Bag Count - " + postingmonthname.TrimEnd() + "  " + postingyear.ToString();
                string query = " select company.name AS company,  count(*) as bagcount from depdetail  ";
                query += " inner join company on left(store, 4) = company.comp_code ";
                query += "where (eventcode = 'C' or eventcode = 'D') ";
                query += " and month(postingdate) = @postingmonth and year(postingdate) = @postingyear group by company.name order by 2 desc ";

                // Populate parameters
                SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["SQLConnString"]);
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.Add(new SqlParameter("postingyear", postingyear));
                cmd.Parameters.Add(new SqlParameter("postingmonth", postingmonth));
                DataTable rptdt = CreateDataTableFromQuery(cmd);
                frmLoading.Close();
                ShowVsReport(ReportPath, "ScriptDataSet", rptdt, RptTitle);
            }

            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }

        public DataTable CreateDataTableFromQuery(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }



        public void CustomerCoinOrderReport()
        {
            string compcode = commonAppDataMethods.SelectCompany();
            string commandtext = "";
            if (compcode.TrimEnd() != "")
            {

                FrmLoading frmLoading = new FrmLoading();
                int[] monthandyear = commonAppDataMethods.GetMonthAndYear();
                if (monthandyear[0] != 0)
                {
                    frmLoading.Show();
                    DateTime ReportDate = commonAppDataMethods.SelectedDate.Date;
                    ReportTitle = "Coin Order Register " + commonAppDataMethods.GetCompanyName(compcode).TrimEnd() + " - " + monthandyear[0].ToString() + "/" + monthandyear[1].ToString();
                    string CommandString = "SELECT * FROM  view_coinorderreporting WHERE LEFT(store,4) = @company AND MONTH(dropdate) = @rptmonth AND YEAR(dropdate) = @rptyear ORDER BY store,dropdate";
                    frmLoading.Show();
                    ssprocessds.view_coinorderreporting.Clear();
                    ClearParameters();
                    AddParms("@company", compcode, "SQL");
                    AddParms("@rptmonth", monthandyear[0], "SQL");
                    AddParms("@rptyear", monthandyear[1], "SQL");
                    FillData(ssprocessds, "view_coinorderreporting", CommandString, CommandType.Text);
                    frmLoading.Hide();
                    if (ssprocessds.view_coinorderreporting.Rows.Count > 0)
                    {
                        ShowVsReport(reportpath + "VSCustomercoinorderreport.rdlc", "ssprocessDs", ssprocessds.view_coinorderreporting, ReportTitle);

                    }
                    else
                    {
                        wsgUtilities.wsgNotice("No matching records");
                    }

                }
                else
                {
                    wsgUtilities.wsgNotice("Operation Cancelled");
                }
            }
        }


        public void StartAMSECDepositAnalysis()
        {
            if (commonAppDataMethods.GetTwoDates("Enter Deposit Dates"))
            {
                string commandtext = "";
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                DateTime ReportStartDate = commonAppDataMethods.SelectedStartDate;
                DateTime ReportEndDate = commonAppDataMethods.SelectedEndDate;
                string ReportTitle = "AMSEC Deposit Listing - " + String.Format("{0:MM/dd/yy}", ReportStartDate) + " - " + String.Format("{0:MM/dd/yy}", ReportEndDate);
                commandtext = "SELECT * FROM view_smartsafedeposits WHERE depositdate >=  @startdate and depositdate <= @enddate";
                commandtext += " order by serialnumber, depositdate";
                ssprocessds.view_smartsafedeposits.Rows.Clear();
                AddParms("@startdate", ReportStartDate.Date, "SQL");
                AddParms("@enddate", ReportEndDate.Date, "SQL");
                FillData(ssprocessds, "view_smartsafedeposits", commandtext, CommandType.Text);
                frmLoading.Close();
                ShowVsReport(reportpath + "AMSECDeposits.rdlc", "ssprocessDs", ssprocessds.view_smartsafedeposits, ReportTitle);

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }


        public void BillingSummary(int[] monthandyear)
        {
            DateTime startdate = Convert.ToDateTime(monthandyear[0].ToString() + "/01/ " + monthandyear[1].ToString());
            DateTime enddate = Convert.ToDateTime(monthandyear[0].ToString() + "/" + DateTime.DaysInMonth(monthandyear[1], monthandyear[0]).ToString());
            ClearParameters();
            string commandtext = "";
            FrmLoading frmLoading = new FrmLoading();
            frmLoading.Show();
            string ReportTitle = "Billing Summary - " + monthandyear[0].ToString() + "/" + DateTime.DaysInMonth(monthandyear[1], monthandyear[0]).ToString() + "/" + monthandyear[1].ToString();
            commandtext = @"select iif(right(bill_store,6) = SPACE(6), name, store_name) as  billingname, bill_store, inv_number, charged,  tax, tax + charged as totalcharge from
(select bill_store, inv_number, sum(charged) as charged, sum(storetax) as tax
FROM
(
 SELECT inv_number, inv_date,  bill_type, bill_store,  charged, storetax 
  FROM billing    where inv_date =  @enddate and charged <> 0  
 UNION ALL SELECT invno AS inv_number, invdte AS invdate,  SPACE(23) as bill_type, RTRIM(custno) + '       ' AS bill_store ,  invamt - tax AS charged, 
 tax as storetax   
 from arinvoice  where  ( left(invno,1) = SPACE(1) OR left(invno,4) = '3000'  )  and   invdte BETWEEN @startdate AND @enddate) B
 GROUP BY bill_store, inv_number) i
 LEFT join store on bill_store = left(storecode,11) 
 inner join company on left(bill_store,4) = comp_code
 ORDER BY inv_number ";
            ssprocessds.view_billsumm.Rows.Clear();
            AddParms("@startdate", startdate, "SQL");
            AddParms("@enddate", enddate, "SQL");
            FillData(ssprocessds, "view_billsumm", commandtext, CommandType.Text);
            frmLoading.Close();
            ShowVsReport(reportpath + "billsum.rdlc", "DataSet1", ssprocessds.view_billsumm, ReportTitle);


        }


        public void TaxSummary(int[] monthandyear)
        {
            DateTime startdate = Convert.ToDateTime(monthandyear[0].ToString() + "/01/ " + monthandyear[1].ToString());
            DateTime enddate = Convert.ToDateTime(monthandyear[0].ToString() + "/" + DateTime.DaysInMonth(monthandyear[1], monthandyear[0]).ToString());
            ClearParameters();
            string commandtext = "";
            FrmLoading frmLoading = new FrmLoading();
            frmLoading.Show();
            string ReportTitle = "Tax Summary - " + monthandyear[0].ToString() + "/" + DateTime.DaysInMonth(monthandyear[1], monthandyear[0]).ToString() + "/" + monthandyear[1].ToString();
            commandtext = @"SELECT inv_date, taxareaname, taxrate, sum(charged) AS charged, sum(taxable) AS taxable, sum(nontaxable) AS nontaxable, sum(tax) AS tax FROM 
(select * from view_taxsummary where inv_date BETWEEN @startdate AND  @enddate and charged <> 0 
union all
SELECT invdte, taxdistrict, taxrate, charged, taxable, tax, nontaxable  FROM morphistaxdata where invdte BETWEEN @startdate AND  @enddate and charged <> 0 ) t
GROUP BY inv_date, taxareaname, taxrate order by 2";
            ssprocessds.view_taxsummary.Rows.Clear();
            AddParms("@startdate", startdate, "SQL");
            AddParms("@enddate", enddate, "SQL");
            FillData(ssprocessds, "view_taxsummary", commandtext, CommandType.Text);
            frmLoading.Close();
            ShowVsReport(reportpath + "taxsumm.rdlc", "DataSet1", ssprocessds.view_taxsummary, ReportTitle);


        }

        public void RevenueSummary(int[] monthandyear)
        {
            DateTime startdate = Convert.ToDateTime(monthandyear[0].ToString() + "/01/ " + monthandyear[1].ToString());
            DateTime enddate = Convert.ToDateTime(monthandyear[0].ToString() + "/" + DateTime.DaysInMonth(monthandyear[1], monthandyear[0]).ToString());
            ClearParameters();
            string commandtext = "";
            FrmLoading frmLoading = new FrmLoading();
            frmLoading.Show();
            string ReportTitle = "Revenue Summary - " + monthandyear[0].ToString() + "/" + DateTime.DaysInMonth(monthandyear[1], monthandyear[0]).ToString() + "/" + monthandyear[1].ToString();
            commandtext = "select * from view_revenuesummary where inv_date BETWEEN @startdate AND  @enddate and charged <> 0 order by 2";
            ssprocessds.view_revenuesummary.Rows.Clear();
            AddParms("@startdate", startdate, "SQL");
            AddParms("@enddate", enddate, "SQL");
            FillData(ssprocessds, "view_revenuesummary", commandtext, CommandType.Text);
            frmLoading.Close();
            ShowVsReport(reportpath + "revsumm.rdlc", "DataSet1", ssprocessds.view_revenuesummary, ReportTitle);


        }
        public void DisplayVSInvoice(string invnumber, string outputmethod)
        {
            string currentstorecode = "";
            string reportfilename = "VSCompanyBill.rdlc";
            int storelineid = 1;
            ClearParameters();
            string commandtext = "";
            string ReportTitle = "Sales Invoice";
            commandtext = "select * from view_vsinvoice where inv_number = @invnumber order by storecode";
            ssprocessds.view_vsinvoice.Rows.Clear();
            AddParms("@invnumber", invnumber, "SQL");
            FillData(ssprocessds, "view_vsinvoice", commandtext, CommandType.Text);
            if (ssprocessds.view_vsinvoice.Rows.Count > 0)
            {
                currentstorecode = ssprocessds.view_vsinvoice[0].storecode;
                for (int r = 0; r < ssprocessds.view_vsinvoice.Rows.Count; r++)
                {
                    if (ssprocessds.view_vsinvoice[r].storecode != currentstorecode)
                    {
                        storelineid = 1;
                        currentstorecode = ssprocessds.view_vsinvoice[r].storecode;
                    }
                    ssprocessds.view_vsinvoice[r].lineid = storelineid;
                    storelineid++;
                }
            }
            ShowVsReport(reportpath + reportfilename, "DataSet1", ssprocessds.view_vsinvoice, ReportTitle);

        }

        public void SafeandSoundSmartSafePostingSummary()
        {
            DateTime reportdate = DateTime.Now.Date;
            bool cont = true;
            if (commonAppDataMethods.GetSingleDate("Report Date", 100, 100))
            {
                reportdate = commonAppDataMethods.SelectedDate;
            }
            else
            {
                cont = false;
            }
            if (cont)
            {
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                string currentevent = "";
                bool breaker = false;
                string currentsystemid = "";
                decimal currentpostingamount = 0;
                int currentrowid = 0;
                ReportTitle = "Safe and Sound Smart Safe Posting Summary - " + String.Format("{0:MM/dd/yyyy}", reportdate);
                mySQLDataMethods.FillSmartsafetrans(reportdate);
                frmLoading.Close();
                mysqlrptds.safetrans.Rows.Clear();
                mysqldatads.safetrans.Rows.Clear();
                if (mySQLDataMethods.mysqlds.safetrans.Rows.Count > 0)
                {
                    for (int r = 0; r < mySQLDataMethods.mysqlds.safetrans.Rows.Count; r++)
                    {
                        if ((mySQLDataMethods.mysqlds.safetrans[r].eventcode.Contains("Cash In") || mySQLDataMethods.mysqlds.safetrans[r].eventcode.Contains("Stacker removed")))
                        {
                            mysqldatads.safetrans.ImportRow(mySQLDataMethods.mysqlds.safetrans[r]);
                        }
                    }
                    currentsystemid = mysqldatads.safetrans[0].systemid;
                    for (int r = 0; r < mysqldatads.safetrans.Rows.Count; r++)
                    {
                        if (currentsystemid == mysqldatads.safetrans[r].systemid && mysqldatads.safetrans[r].eventcode.Contains("Cash In"))
                        {
                            currentpostingamount += mysqldatads.safetrans[r].eventamount;
                            continue;
                        }

                        if (currentsystemid != mysqldatads.safetrans[r].systemid && mysqldatads.safetrans[r].eventcode.Contains("Cash In"))
                        {
                            if (currentpostingamount != 0)
                            {
                                mysqlrptds.safetrans.Rows.Add();
                                currentrowid = mysqlrptds.safetrans.Rows.Count - 1;
                                mysqlrptds.safetrans[currentrowid].systemid = currentsystemid;
                                mysqlrptds.safetrans[currentrowid].eventamount = currentpostingamount;

                            }
                            currentsystemid = mysqldatads.safetrans[r].systemid;
                            currentpostingamount = mysqldatads.safetrans[r].eventamount;
                            continue;

                        }
                        if (mysqldatads.safetrans[r].eventcode.Contains("Stacker removed"))
                        {
                            if (currentpostingamount != 0)
                            {
                                mysqlrptds.safetrans.Rows.Add();
                                currentrowid = mysqlrptds.safetrans.Rows.Count - 1;
                                mysqlrptds.safetrans[currentrowid].systemid = currentsystemid;
                                mysqlrptds.safetrans[currentrowid].eventamount = currentpostingamount;
                                currentpostingamount = 0;
                            }
                            currentsystemid = mysqldatads.safetrans[r].systemid;
                            continue;
                        }

                    }
                    // Flush the last posting, if any
                    if (currentpostingamount != 0)
                    {
                        mysqlrptds.safetrans.Rows.Add();
                        currentrowid = mysqlrptds.safetrans.Rows.Count - 1;
                        mysqlrptds.safetrans[currentrowid].systemid = currentsystemid;
                        mysqlrptds.safetrans[currentrowid].eventamount = currentpostingamount;

                    }

                    if (mysqlrptds.safetrans.Rows.Count > 0)
                    {
                        ShowVsReport(reportpath + "SafeAndSoundSmartSafeSummary.rdlc", "mysqldataDs", mysqlrptds.safetrans, ReportTitle);
                    }
                    else
                    {
                        wsgUtilities.wsgNotice("No Matching Records");
                    }
                }
                else
                {
                    wsgUtilities.wsgNotice("No Matching Records");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }


        public void ShowVsInvoice(string invnumber)
        {
            reportpath = reportpath + "VSCompanyBill.rdlc";
            string currentstorecode = "";
            string reportfilename = "VSCompanyBill.rdlc";
            int storelineid = 1;
            ClearParameters();
            string commandtext = "";
            string ReportTitle = "Sales Invoice";




            commandtext = "select * from view_vsinvoice where inv_number = @invnumber order by storecode, bill_type";
            ssprocessds.view_vsinvoice.Rows.Clear();
            AddParms("@invnumber", invnumber, "SQL");
            FillData(ssprocessds, "view_vsinvoice", commandtext, CommandType.Text);

            //Lineid is needed to alternate columns
            if (ssprocessds.view_vsinvoice.Rows.Count > 0)
            {
                storelineid = 1;
                currentstorecode = ssprocessds.view_vsinvoice[0].storecode;
                for (int r = 0; r < ssprocessds.view_vsinvoice.Rows.Count; r++)
                {
                    if (ssprocessds.view_vsinvoice[r].storecode != currentstorecode)
                    {

                        currentstorecode = ssprocessds.view_vsinvoice[r].storecode;
                        storelineid++;
                    }
                    ssprocessds.view_vsinvoice[r].lineid = storelineid;
                }
            }

            // Report parameters
            string[] billingcompanydata = { "Safe and Sound Armored Courier, Inc", "PO Box 1463", "Bayville, NY 11709-0463", "516-628-1137" };
            string[] billingaddressdata = { "fff", "ffff", "fff", "fff", "ff" };
            string[] shippingaddressdata = { "fff", "fff", "fff", "fff", "ff" };
            string RptTitle = "Invoice";
            FrmVSReportViewer frmVSReportViewer = new FrmVSReportViewer();
            frmVSReportViewer.Text = RptTitle;
            frmVSReportViewer.reportViewerVSReport.ProcessingMode = ProcessingMode.Local;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Clear();
            frmVSReportViewer.reportViewerVSReport.LocalReport.ReportPath = reportpath;
            // Note: The report must have a parameter with the name RptTitle
            // Use that parameter in the title textbox
            // Billing Company Address Parameters
            ReportParameter pbc1 = new ReportParameter("RptTitle", RptTitle);
            ReportParameter pbc2 = new ReportParameter("BillingCompanyName", billingcompanydata[0]);
            ReportParameter pbc3 = new ReportParameter("BillingCompanyStreet", billingcompanydata[1]);
            ReportParameter pbc4 = new ReportParameter("BillingCompanyCityStateZip", billingcompanydata[2]);
            ReportParameter pbc5 = new ReportParameter("BillingCompanyPhone", billingcompanydata[3]);


            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pbc1 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pbc2 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pbc3 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pbc4 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pbc5 });

            // Bill To parameters.
            ReportParameter pba1 = new ReportParameter("baddr1", billingaddressdata[0]);
            ReportParameter pba2 = new ReportParameter("baddr2", billingaddressdata[1]);
            ReportParameter pba3 = new ReportParameter("baddr3", billingaddressdata[2]);
            ReportParameter pba4 = new ReportParameter("baddr4", billingaddressdata[3]);
            ReportParameter pba5 = new ReportParameter("baddr5", billingaddressdata[4]);


            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pba1 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pba2 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pba3 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pba4 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pba5 });


            // Ship To parameters.
            ReportParameter psa1 = new ReportParameter("saddr1", shippingaddressdata[0]);
            ReportParameter psa2 = new ReportParameter("saddr2", shippingaddressdata[1]);
            ReportParameter psa3 = new ReportParameter("saddr3", shippingaddressdata[2]);
            ReportParameter psa4 = new ReportParameter("saddr4", shippingaddressdata[3]);
            ReportParameter psa5 = new ReportParameter("saddr5", shippingaddressdata[4]);

            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { psa1 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { psa2 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { psa3 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { psa4 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { psa5 });

            // PO, Contract, FEDID
            ReportParameter ppo1 = new ReportParameter("Ponum", "12345");
            ReportParameter pct1 = new ReportParameter("Contract", "ABCD");
            ReportParameter pfed = new ReportParameter("Fedid", "abcd");
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { ppo1 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pct1 });
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { pfed });



            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = ssprocessds.view_vsinvoice;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Add(rds);
            frmVSReportViewer.reportViewerVSReport.RefreshReport();
            frmVSReportViewer.ShowDialog();

        }





        public void ShowVsReport(string ReportPath, string DataSetName, DataTable dt, string RptTitle)
        {
            FrmVSReportViewer frmVSReportViewer = new FrmVSReportViewer();
            frmVSReportViewer.Text = RptTitle;
            frmVSReportViewer.reportViewerVSReport.ProcessingMode = ProcessingMode.Local;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Clear();
            frmVSReportViewer.reportViewerVSReport.LocalReport.ReportPath = ReportPath;
            // Note: The report must have a parameter with the name RptTitle
            // Use that parameter in the title textbox
            ReportParameter p = new ReportParameter("RptTitle", RptTitle);
            frmVSReportViewer.reportViewerVSReport.LocalReport.SetParameters(new ReportParameter[] { p });
            ReportDataSource rds = new ReportDataSource();
            rds.Name = DataSetName;
            rds.Value = dt;
            frmVSReportViewer.reportViewerVSReport.LocalReport.DataSources.Add(rds);
            frmVSReportViewer.reportViewerVSReport.RefreshReport();
            frmVSReportViewer.ShowDialog();

        }


    }
}
