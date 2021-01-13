using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using BusinessTransactions;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BusinessTransactions
{
    public class InvoiceTransmissionMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Transmission");
        AppUtilities appUtilities = new AppUtilities();
        BusinessReports.BusinessReportsMethods businessReportsMethods = new BusinessReports.BusinessReportsMethods();
        FrmLoading frmLoading = new FrmLoading();
        public ProgressBar progressBarMain = new ProgressBar();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocesssearchds = new ssprocess();
        public InvoiceTransmissionMethods()
            : base("SQL", "SQLConnString")
        {

        }

        public void SendInvoiceSpreadsheetBatchSingleCompanyMultiplePeriods()
        {
            FrmInvoiceRangeSelector frmInvoiceRangeSelector = new FrmInvoiceRangeSelector();
            string InvoiceNumber = "";
            frmInvoiceRangeSelector.ShowDialog();
            if (frmInvoiceRangeSelector.cont)
            {
                if (frmInvoiceRangeSelector.CompCode.TrimEnd() != "")
                {
                    string startstring = frmInvoiceRangeSelector.numericUpDownStartMonth.Value.ToString() + "/01/" + frmInvoiceRangeSelector.numericUpDownStartYear.Value.ToString();
                    DateTime StartInvoiceDate = Convert.ToDateTime(startstring);
                    string commandstring = "SELECT DISTINCT inv_number FROM view_expandedbilling   WHERE bill_store = @bill_store AND inv_date ";
                    commandstring += " BETWEEN CONVERT(DATETIME,CAST(@fromyear AS VARCHAR(4))+'-'+CAST(@frommonth AS VARCHAR(2))+'-01',120) AND ";
                    commandstring += "   DATEADD(m,1,CONVERT(DATETIME,CAST(@toyear AS VARCHAR(4))+'-'+CAST(@tomonth AS VARCHAR(2))+'-01',120))-1 ";
                    commandstring += "ORDER BY inv_number";
                    string spreadsheetfilename = "";
                    ClearParameters();
                    AddParms("@fromyear", frmInvoiceRangeSelector.numericUpDownStartYear.Value, "SQL");
                    AddParms("@frommonth", frmInvoiceRangeSelector.numericUpDownStartMonth.Value, "SQL");
                    AddParms("@toyear", frmInvoiceRangeSelector.numericUpDownEndYear.Value, "SQL");
                    AddParms("@tomonth", frmInvoiceRangeSelector.numericUpDownEndMonth.Value, "SQL");
                    AddParms("@bill_store", frmInvoiceRangeSelector.CompCode, "SQL");

                    ssprocessds.view_ExpandedBillingStoreSummary.Rows.Clear();
                    FillData(ssprocessds, "view_ExpandedBillingStoreSummary", commandstring, CommandType.Text);
                    List<string> AttachmentNames = new List<string>();
                    if (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count > 0)
                    {
                        if (wsgUtilities.wsgReply("Do you want to email Invoice Spreadsheets?"))
                        {
                            frmLoading.Show();
                            for (int i = 0; i <= ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count - 1; i++)
                            {
                                spreadsheetfilename = businessReportsMethods.CreateBillingSpreadsheet(ssprocessds.view_ExpandedBillingStoreSummary[i].inv_number);
                                if (spreadsheetfilename.TrimEnd() != "")
                                {
                                    AttachmentNames.Add(spreadsheetfilename);
                                }
                            }
                            commonAppDataMethods.SendSelectedCompanyEmail("Invoice", AttachmentNames, frmInvoiceRangeSelector.CompCode, "Safe and Sound Billing " + commonAppDataMethods.GetCompanyName(frmInvoiceRangeSelector.CompCode), "See Attached");
                            frmLoading.Hide();
                            wsgUtilities.wsgNotice("Operation Complete");
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("Operation Cancelled");
                        }
                    }
                    else
                    {
                        wsgUtilities.wsgNotice("There are no invoices for that customer and period");
                    }

                }
                else
                {
                    wsgUtilities.wsgNotice("Please specify a company");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancellled");
            }



        }

        public void SendInvoiceSpreadsheetPDFDesignatedCompanies()
        {
            List<string> AttachmentNames = new List<string>();
            string spreadsheetfilename = "";
            string pdffilename = "";
            FrmLoading frmBatchLoading = new FrmLoading();
            frmBatchLoading.Text = "Batch Invoice Transmission";
            bool cont = true;
            int[] monthandyear = commonAppDataMethods.GetMonthAndYear();
            if (monthandyear[0] != 0)
            {


                ClearParameters();
                ssprocessds.company.Rows.Clear();
                string commandstring = "SELECT * FROM company WHERE invoicespreadsheet = 1 OR invoicepdf = 1 OR (comp_code = '2645' OR comp_code = '7822' OR comp_code = '7850') ORDER BY comp_code ";
                FillData(ssprocessds, "company", commandstring, CommandType.Text);
                if (ssprocessds.company.Rows.Count > 0)
                {
                    if (wsgUtilities.wsgReply("There are " + ssprocessds.company.Rows.Count.ToString() + " companies. Proceed?"))
                    {
                        try
                        {
                            frmBatchLoading.Show();
                            frmBatchLoading.progressBarLoading.Visible = true;
                            frmBatchLoading.progressBarLoading.Maximum = ssprocessds.company.Rows.Count;
                            frmBatchLoading.progressBarLoading.Step = 1;
                            frmBatchLoading.progressBarLoading.Refresh();

                            for (int r = 0; r <= ssprocessds.company.Rows.Count - 1; r++)
                            {
                                frmBatchLoading.progressBarLoading.PerformStep();
                                switch (ssprocessds.company[r].comp_code)
                                {
                                    case "2645":
                                        {
                                            SendFamilyDollarInvoice(monthandyear);
                                            break;
                                        }

                                    case "7822":
                                        {
                                            SendStarbucksInvoice(monthandyear);
                                            break;
                                        }

                                    case "7850":
                                        {
                                            SendTMobileInvoice(monthandyear);
                                            break;
                                        }

                                    default:
                                        {
                                            AttachmentNames.Clear();
                                            ssprocessds.view_ExpandedBillingStoreSummary.Rows.Clear();
                                            ClearParameters();
                                            AddParms("@invmonth", monthandyear[0], "SQL");
                                            AddParms("@invyear", monthandyear[1], "SQL");
                                            AddParms("@comp_code", ssprocessds.company[r].comp_code, "SQL");

                                            commandstring = "SELECT DISTINCT inv_number FROM view_ExpandedBilling WHERE MONTH(inv_date) = @invmonth  AND YEAR(inv_date) = @invyear and bill_store  = @comp_code";
                                            FillData(ssprocessds, "view_ExpandedBillingStoreSummary", commandstring, CommandType.Text);
                                            if (ssprocessds.view_ExpandedBillingStoreSummary.Rows.Count > 0)
                                            {
                                                spreadsheetfilename = businessReportsMethods.CreateBillingSpreadsheet(ssprocessds.view_ExpandedBillingStoreSummary[0].inv_number);
                                                if (spreadsheetfilename.TrimEnd() != "")
                                                {
                                                    AttachmentNames.Add(spreadsheetfilename);
                                                }
                                                pdffilename = ConfigurationManager.AppSettings["InvoicePDFPath"] + ssprocessds.view_ExpandedBillingStoreSummary[0].inv_number + ".pdf";
                                                if (File.Exists(pdffilename))
                                                {
                                                    AttachmentNames.Add(pdffilename);
                                                }
                                            }
                                            commonAppDataMethods.SendCompanyEmail(AttachmentNames, ssprocessds.company[r].comp_code, "Safe and Sound Billing " + commonAppDataMethods.GetCompanyName(ssprocessds.company[r].comp_code), "See Attached");
                                            break;
                                        }
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("{0} Exception caught.", e);
                        }
                        progressBarMain.Visible = false;
                        frmBatchLoading.Hide();
                        wsgUtilities.wsgNotice("Operation Complete");

                    }
                    else
                    {
                        wsgUtilities.wsgNotice("Operation Cancellled");
                    }
                }
                else
                {
                    wsgUtilities.wsgNotice("There are no companies with invoice spreadsheet indications");
                }

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancellled");
            }
        }


        public void SendInvoiceSpreadsheet()
        {
            string InvoiceNumber = commonAppDataMethods.SelectInvoiceNumber();
            int[] monthandyear = new int[2];
            if (InvoiceNumber.TrimEnd() != "")
            {
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                string pdffilename = "";
                ssprocessds.view_BillingStoreBillTypeSummary.Rows.Clear();
                ClearParameters();
                AddParms("@invnumber", InvoiceNumber, "SQL");
                string commandtext = "SELECT * FROM view_BillingStoreBillTypeSummary WHERE inv_number = @invnumber";
                FillData(ssprocessds, "view_BillingStoreBillTypeSummary", commandtext, CommandType.Text);
                if (ssprocessds.view_BillingStoreBillTypeSummary.Rows.Count > 0)
                {
                    monthandyear[0] = ssprocessds.view_BillingStoreBillTypeSummary[0].inv_date.Month;
                    monthandyear[1] = ssprocessds.view_BillingStoreBillTypeSummary[0].inv_date.Year;
                    switch (ssprocessds.view_BillingStoreBillTypeSummary[0].storecode.Substring(0, 4))
                    {
                        case "7822":
                            {
                                frmLoading.Show();
                                SendStarbucksInvoice(monthandyear);
                                frmLoading.Hide();
                                wsgUtilities.wsgNotice("Operation Complete");

                                break;
                            }

                        case "7850":
                            {
                                frmLoading.Show();
                                SendTMobileInvoice(monthandyear);
                                frmLoading.Hide();
                                wsgUtilities.wsgNotice("Operation Complete");
                                break;
                            }
                        case "2645":
                            {
                                frmLoading.Show();
                                SendFamilyDollarInvoice(monthandyear);
                                break;
                                frmLoading.Hide();
                                wsgUtilities.wsgNotice("Operation Complete");

                            }

                        default:
                            {
                                frmLoading.Show();
                                string spreadsheetfilename = businessReportsMethods.CreateBillingSpreadsheet(InvoiceNumber);
                                List<string> AttachmentNames = new List<string>();

                                if (spreadsheetfilename.TrimEnd() != "")
                                {
                                    AttachmentNames.Add(spreadsheetfilename);
                                }
                                pdffilename = ConfigurationManager.AppSettings["InvoicePDFPath"] + ssprocessds.view_BillingStoreBillTypeSummary[0].inv_number + ".pdf";
                                if (File.Exists(pdffilename))
                                {
                                    AttachmentNames.Add(pdffilename);
                                }

                                commonAppDataMethods.SendCompanyEmail(AttachmentNames, ssprocessds.view_BillingStoreBillTypeSummary[0].storecode.Substring(0, 4), "Safe and Sound Billing " + ssprocessds.view_BillingStoreBillTypeSummary[0].inv_date.ToShortDateString(), "Safe and Sound Billing " + ssprocessds.view_BillingStoreBillTypeSummary[0].inv_date.ToShortDateString() + "- See Attached");
                                frmLoading.Hide();
                                wsgUtilities.wsgNotice("Operation Complete");
                                break;
                            }
                    }
                }
                else
                {
                    wsgUtilities.wsgNotice("No bills found for that invoice");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancellled");
            }



        }

        public void SendTMobileInvoice(int[] monthandyear)
        {
            bool cont = true;

            ssprocessds.view_BillingStoreBillTypeSummary.Rows.Clear();
            string commandtext = "SELECT * FROM view_BillingStoreBillTypeSummary WHERE LEFT(storecode,4) = @company AND MONTH(inv_date) = @rptmonth AND YEAR(inv_date) = @rptyear  ORDER BY storecode, bill_type  ";
            ClearParameters();
            AddParms("@company", "7850", "SQL");
            AddParms("@rptmonth", monthandyear[0], "SQL");
            AddParms("@rptyear", monthandyear[1], "SQL");
            FillData(ssprocessds, "view_BillingStoreBillTypeSummary", commandtext, CommandType.Text);
            if (ssprocessds.view_BillingStoreBillTypeSummary.Rows.Count < 1)
            {
                wsgUtilities.wsgNotice("There is no TMobile invoice for that month and year");
                cont = false;

            }


            if (cont)
            {
                ssprocessds.view_tmobilebilling.Rows.Clear();
                string spreadsheetfilename = businessReportsMethods.CreateTMobileBillingSpreadsheet(ssprocessds.view_BillingStoreBillTypeSummary[0].inv_number);
                ClearParameters();
                AddParms("@invnumber", ssprocessds.view_BillingStoreBillTypeSummary[0].inv_number, "SQL");
                commandtext = "SELECT TOP 1 * FROM  view_tmobilebilling WHERE inv_number = @invnumber ORDER BY inv_number ";
                FillData(ssprocessds, "view_tmobilebilling", commandtext, CommandType.Text);

                List<string> AttachmentNames = new List<string>();

                if (spreadsheetfilename.TrimEnd() != "")
                {
                    AttachmentNames.Add(spreadsheetfilename);
                }
                commonAppDataMethods.SendCompanyEmail(AttachmentNames, "7850", "Safe and Sound Billing " + ssprocessds.view_tmobilebilling[0].inv_date.ToShortDateString(), "Safe and Sound Billing " + ssprocessds.view_tmobilebilling[0].inv_date.ToShortDateString() + "- See Attached");
            }



        }

        public void SendStarbucksInvoice(int[] monthandyear)
        {
            string commandtext = "";
            ssprocessds.view_BillingStoreBillTypeSummary.Rows.Clear();
            commandtext = "SELECT * FROM view_BillingStoreBillTypeSummary WHERE LEFT(storecode,4) = @company AND MONTH(inv_date) = @rptmonth AND YEAR(inv_date) = @rptyear  ORDER BY storecode, bill_type  ";
            ClearParameters();
            AddParms("@company", "7822", "SQL");
            AddParms("@rptmonth", monthandyear[0], "SQL");
            AddParms("@rptyear", monthandyear[1], "SQL");
            FillData(ssprocessds, "view_BillingStoreBillTypeSummary", commandtext, CommandType.Text);
            if (ssprocessds.view_BillingStoreBillTypeSummary.Rows.Count > 0)
            {
                string textfilename1 = businessReportsMethods.CreateStarbucksBillingSpreadsheet(ssprocessds.view_BillingStoreBillTypeSummary[0].inv_number, 1);
                string textfilename2 = businessReportsMethods.CreateStarbucksBillingSpreadsheet(ssprocessds.view_BillingStoreBillTypeSummary[0].inv_number, 2);
                List<string> AttachmentNames = new List<string>();
                AttachmentNames.Add(textfilename1);
                AttachmentNames.Add(textfilename2);
                string spreadsheetfilename = businessReportsMethods.CreateBillingSpreadsheet(ssprocessds.view_BillingStoreBillTypeSummary[0].inv_number);
                if (spreadsheetfilename.TrimEnd() != "")
                {
                    AttachmentNames.Add(spreadsheetfilename);

                }
                commonAppDataMethods.SendCompanyEmail(AttachmentNames, "7822", "Safe and Sound Billing " + monthandyear[0].ToString() + "/" + monthandyear[1].ToString(), "Safe and Sound Billing " + monthandyear[0].ToString() + "/" + monthandyear[1].ToString() + "- See Attached");

            }

        }


        public void SendFamilyDollarInvoice(int[] monthandyear)
        {
            bool cont = true;
            string commandtext = "";
            decimal invoiceamount = 0;
            string textline = "";
            string servicedescription = "";
            List<string> AttachmentNames = new List<string>();
            string textfilename1 = "";
            string textfilename2 = "";
            string pdffilename1 = "";
            string pdffilename2 = "";

            if (monthandyear[0] != 0)
            {

                textfilename1 = businessReportsMethods.CreateFamilyDollarBillingSpreadSheet(monthandyear[0], monthandyear[1], 1);
                textfilename2 = businessReportsMethods.CreateFamilyDollarBillingSpreadSheet(monthandyear[0], monthandyear[1], 2);
                // Extract the invoice number from the text file name
                pdffilename1 = ConfigurationManager.AppSettings["InvoicePDFPath"] + textfilename1.Substring(textfilename1.Length - 14, 10) + ".pdf";
                pdffilename2 = ConfigurationManager.AppSettings["InvoicePDFPath"] + textfilename2.Substring(textfilename2.Length - 14, 10) + ".pdf";
                if (File.Exists(pdffilename1))
                {
                    AttachmentNames.Add(pdffilename1);
                }
                if (File.Exists(pdffilename2))
                {
                    AttachmentNames.Add(pdffilename2);
                }
                AttachmentNames.Add(textfilename1);
                AttachmentNames.Add(textfilename2);
                commonAppDataMethods.SendCompanyEmail(AttachmentNames, "2645", "Safe and Sound Billing " + monthandyear[0].ToString() + "/" + monthandyear[1].ToString(), "Safe and Sound Billing " + monthandyear[0].ToString() + "/" + monthandyear[1].ToString() + "- See Attached");
            }
        }



    }

}