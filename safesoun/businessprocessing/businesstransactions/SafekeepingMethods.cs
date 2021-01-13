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
    public class SafekeepingMethods : WSGDataAccess
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
        public SafekeepingMethods()
            : base("SQL", "SQLConnString")
        {

        }

        #region WestchesterFiles
        public void SendWestchesterFiles()
        {

            // This appiclation is not in production. The code is here for future use.
            wsgUtilities.wsgNotice("This application is not in production");
            return;

            bool cont = true;
            List<string> ddas = new List<string>();
            List<decimal> amounts = new List<decimal>();
            List<string> accounts = new List<string>();
            List<string> stores = new List<string>();
            List<string> debitcredits = new List<string>();
            List<string> companies = new List<string>();
            decimal credittotal = 0;
            decimal debittotal = 0;

            DateTime PostingDate = DateTime.Now.Date;
            string debitcredit = "";
            string pgpsourcefilename = ConfigurationManager.AppSettings["FTPFilepath"] + "ss" + "072914.txt";
            string achfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "ACHI038.txt";
            string bankname = "Westchester Bank".PadRight(23);
            string companyname = "Westchester".PadRight(16);
            string accountnumber = "";
            string bankfedid = "261961406".PadRight(10);
            string bankroutingnumber = "02191454";
            string bankroutingnumbercheckdigit = "4";
            string standardentryclass = "CCD";       // Start the text file
            string entrydescription = "Deposit".PadRight(10);
            TextWriter Textout = new StreamWriter(achfilename);
            //Start the text line
            string lineout = "";
            //  string pgpsourcefilename = ConfigurationManager.AppSettings["FTPFilepath"] + "ss" + String.Format("{0:MMddyy}", PostingDate) + ".txt"; 
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(pgpsourcefilename))
            {
                String dataline;
                // Read lines from the file until the end of the file is reached.
                while ((dataline = sr.ReadLine()) != null)
                {
                    // Skip total and header lines
                    if (dataline.Substring(0, 1) != "D")
                    {
                        continue;
                    }
                    accountnumber = dataline.Substring(22, 10);
                    if (accountnumber.TrimEnd() == "0100000004" || accountnumber.TrimEnd() == "0100000005")
                    {
                        continue;
                    }
                    // Test plug
                    accountnumber = "31055841";
                    accounts.Add(accountnumber);
                    stores.Add(dataline.Substring(42, 10));
                    amounts.Add(Convert.ToDecimal(dataline.Substring(52, 11)));
                    debitcredit = dataline.Substring(80, 1);
                    if (debitcredit == "C")
                    {
                        credittotal += Convert.ToDecimal(dataline.Substring(52, 11));
                    }
                    else
                    {
                        debittotal += Convert.ToDecimal(dataline.Substring(52, 11));
                    }
                    debitcredits.Add(debitcredit);
                    // Test plug
                    companies.Add("TEST2");
                    //    companies.Add(commonAppDataMethods.GetCompanyName(dataline.Substring(42, 4)));
                    break;
                }

                // Create the file header
                lineout = "1";                                                      //1 record type code - 1 is master
                lineout += "01";                                                    // 2 priority code - always use 01 
                lineout += " " + bankroutingnumber + bankroutingnumbercheckdigit;   // 3 company bank routing number including checkdigit, preceded by a blank
                lineout += bankfedid.PadRight(10);                                  // 4 fedid
                lineout += String.Format("{0:yyMMdd}", PostingDate);                // 5 file creation date
                lineout += DateTime.Now.ToString("HHmm");                           // 6 file creation time format 0230
                lineout += "A";                                                     // 7 file id modifier - use A
                lineout += "094";                                                   // 8 record size - must be 94 characters
                lineout += "10";                                                    // 9 blocking factor - use 10
                lineout += "1";                                                     // 10 format code - use 1
                lineout += bankname.PadRight(23);                                   // 11 company bank name, i.e. First National
                lineout += bankname.PadRight(23);                                   // 12 company bank name, same as #11
                lineout += " ".PadRight(8);                                         // 13 reference code - do not know purpose
                Textout.WriteLine(lineout);

                // Create batch header
                lineout = "5";                                        // 1 record type code - 5
                lineout += "200";                                     // 2 service class code - 200
                lineout += companyname;                               // 3 company name, i.e. Jones Trade School
                lineout += "".PadRight(20);                           //  4 company disc data  - blank
                lineout += bankfedid;                                 // 5 company EFT id
                lineout += standardentryclass;                        // 6 std entry class code - always use PPD
                lineout += entrydescription;                          // 7 company entry desc - 10 chars content whatever desc you want
                lineout += String.Format("{0:yyMMdd}", PostingDate);  // 8 company desc date - same as 9
                lineout += String.Format("{0:yyMMdd}", PostingDate);  // 9 eft entry date - same as 8
                lineout += "".PadRight(3);                            // 10 settlement date julian - blank
                lineout += "1";                                       // 11 originator status code - 1 for financial institution
                lineout += bankroutingnumber.Substring(0, 8);         // 12 dfi id - comp.bank acct #, same as #3 in first record 
                lineout += "1".PadLeft(7, '0');                       // 13 batch # - usually 0000001
                Textout.WriteLine(lineout);


                debittotal = 0;
                credittotal = 0;
                decimal routinghash = 0;
                // Loop though the lists. Create detail lines and accumulate totals
                for (int i = 0; i < accounts.Count; i++) // Loop through List with for
                {
                    lineout = "6";                                          // 1 record type code - 6
                    if (debitcredits[i] == "C")
                    {
                        credittotal += amounts[i];

                        lineout += "22";                                      // 2 22 for credit
                    }
                    else
                    {
                        debittotal += amounts[i];
                        lineout += "27";                                      // 2 27 for debit - always 27
                    }
                    lineout += bankroutingnumber;                           //  3/4 receiving dfi id + chk digit      - often called routing number - of payer's bank
                    lineout += bankroutingnumbercheckdigit;
                    lineout += accounts[i].PadRight(17);                    // 5 dfi acct # - account number at bank
                    lineout += (amounts[i]).ToString().PadLeft(10, '0');    // 6 payee amount - 10 digit string left padded with 0's
                    lineout += stores[i].PadRight(15);                      // 7 payee's indiv id #, create as desired -- use storecode
                    lineout += companies[i].PadRight(22).Substring(0, 22);  // 8 payee's name name      
                    lineout += "".PadRight(2);                              //  9 disc data - leave blank
                    lineout += "0";                                         //  10 addenda record indicator
                    lineout += bankroutingnumber + (i + 1).ToString().PadLeft(7, '0');     //  11 tracer number - bankrouting number plus sequence number                                             // 11 trace # 
                    // Accumulate totals  
                    routinghash += Convert.ToDecimal(bankroutingnumber);
                    Textout.WriteLine(lineout);
                }

                if (cont)
                {
                    // Batch control record
                    lineout = "8";                                             // 1 record type code
                    lineout += "200";                                          //2 service class code 200
                    lineout += amounts.Count.ToString().PadLeft(6, '0');       //3 entry/addenda count
                    lineout += routinghash.ToString().PadLeft(10, '0');        // 4 entry hash  
                    lineout += debittotal.ToString().PadLeft(12, '0');         // 5 debit total
                    lineout += credittotal.ToString().PadLeft(12, '0');        // 6 credit total
                    lineout += bankfedid;                                      // 7 bankfedid again
                    lineout += "".PadRight(19);                                // 8 msg auth code
                    lineout += "".PadRight(6);                                 //  9 reserved
                    lineout += bankroutingnumber;                              // 10 company bank routing number again
                    lineout += "1".PadLeft(7, '0');                            // 11 batch # - 0000001
                    Textout.WriteLine(lineout);

                    // Compute the block count and determine how much, if any, padding is required.
                    // Assume a blocking factor of 10
                    int paddingneeded = 0;
                    if ((amounts.Count + 4) % 10 > 0)
                    {
                        paddingneeded = 10 - (amounts.Count + 4) % 10;
                    }
                    int blockcount = (amounts.Count + 4 + paddingneeded) / 10;

                    // File Control Record
                    lineout = "9";                                             // 9 record type code
                    lineout += "1".PadLeft(6, '0');                            // 2 batch count
                    lineout += blockcount.ToString().PadLeft(6, '0');          // 3 block count
                    lineout += amounts.Count.ToString().PadLeft(8, '0');       //  4 entry/addenda count
                    lineout += routinghash.ToString().PadLeft(10, '0');        // 5 entry hash 
                    lineout += debittotal.ToString().PadLeft(12, '0');         // 6 debit total
                    lineout += credittotal.ToString().PadLeft(12, '0');        // 7 credit total
                    lineout += "".PadRight(39);
                    Textout.WriteLine(lineout);

                    // Add any needed padding
                    for (int i = 1; i < paddingneeded; i++)
                    {
                        Textout.WriteLine("".PadRight(94));
                    }

                    // Close the text file
                    Textout.Close();
                    // Send the text file
                    wsgUtilities.wsgNotice("File Sent");
                }
            }

        }
        #endregion



        public void SendSignatureBillingFile()
        {
            string commandstring = "";
            // Force Signature Bank's idcol
            int selectedbankid = 1;
            ssprocessds.bank.Rows.Clear();
            commandstring = "SELECT * FROM bank WHERE idcol = @idcol";
            ClearParameters();
            AddParms("@idcol", selectedbankid, "SQL");
            FillData(ssprocessds, "bank", commandstring, CommandType.Text);

            FrmLoading frmLoading = new FrmLoading();
            int[] monthandyear = commonAppDataMethods.GetMonthAndYear();
            if (monthandyear[0] != 0)
            {
                if (wsgUtilities.wsgReply("Do you want to send the Signature Billing File?"))
                {

                    frmLoading.Show();

                    DateTime BillingDate = commonAppDataMethods.GetLastDateOfMonth(Convert.ToDateTime(monthandyear[0].ToString() + "/28/" + monthandyear[1].ToString()));

                    string outputfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "8923847" + "\\" + "billfile" + String.Format("{0:MMddyy}", BillingDate) + ".xlsx";
                    string pdffilename = ConfigurationManager.AppSettings["FTPFilepath"] + "8923847" + "\\" + "ss" + String.Format("{0:MMyy}", BillingDate) + ".pdf";

                    string textline = "";
                    string CommandString = "SELECT * FROM   view_signaturebillsummary WHERE CONVERT(date, inv_date) = @billing ORDER BY inv_number, servicecode";
                    frmLoading.Show();
                    ssprocessds.view_SignatureBillSummary.Rows.Clear();
                    ClearParameters();
                    AddParms("@billing", BillingDate, "SQL");
                    FillData(ssprocessds, "view_signaturebillsummary", CommandString, CommandType.Text);
                    if (ssprocessds.view_SignatureBillSummary.Rows.Count > 0)
                    {
                        businessReportMethods.CreateSignatureBillingRegistePDF(BillingDate, pdffilename);
                        businessReportMethods.CreateSignatureBillingSpreadsheet(ssprocessds.view_SignatureBillSummary, outputfilename, BillingDate);
                        appUtilities.SendSftp("63.117.175.22", "safeandsounduser1", "B$d3LXbvNe", outputfilename);
                        appUtilities.SendSftp("63.117.175.22", "safeandsounduser1", "B$d3LXbvNe", pdffilename);

                        wsgUtilities.wsgNotice("Operation Complete");
                    }

                    else
                    {
                        wsgUtilities.wsgNotice("No Matching Records");
                    }
                    frmLoading.Hide();

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



        public void SendSafeKeepingData()
        {
            string bankfedid = "";
            int selectedbankid = commonAppDataMethods.SelectSafekeepingBank();

            // If a row has been selected fill the data and process
            if (selectedbankid > 0)
            {
                ssprocessds.bank.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", selectedbankid, "SQL");
                FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
                bankfedid = ssprocessds.bank[0].bankfedid.TrimEnd();
            }

            switch (bankfedid)
            {
                case ("8923847"):
                    {
                        // Signature Bank
                        SendSignatureFiles();
                        break;
                    }
                case ("026002794"):
                    {
                        // Bank Leumi
                        SendBankLeumiFiles();
                        break;
                    }
                case ("FBSMARTSAFE"):
                    {
                        // Flushing Bank
                   //      SendFlushingBankFiles();
                        DateTime postingdate = Convert.ToDateTime("11/07/2020");
                        CreateFlushingBankSmartsafeFile(postingdate);
                        break;
                    }
                default:
                    {
                        wsgUtilities.wsgNotice("There are no transmission procedures for that bank");
                        break;
                    }
            }
        }

        public void SendFlushingBankFiles()
        {
            string inputdataline = "";
            string outputdataline = "";
         //   string cashonhandaccount = ssprocessds.bank[0].cohaccount.TrimEnd();
         //   string cashintransitaccount = ssprocessds.bank[0].citaccount.TrimEnd();
            // Flushing bank uses an account structure that is incompatible with other banks, hence the hard coding here.
            string cashonhandaccount = "1001-04-100-0";
            string cashintransitaccount = "1001-04-103-0";
          
            string fedaccount = "";
            string storecode = "";

            string tellernumber = "9974";
            string commandstring = "";
            bool cont = true;
            DateTime PostingDate = DateTime.Now.Date;
            string sourcefilename = "";
            if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 200, 200))
            {
                PostingDate = commonAppDataMethods.SelectedDate.Date;

                sourcefilename = ConfigurationManager.AppSettings["FTPFilepath"] + "\\flushingbank\\" + "ss" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                if (!File.Exists(sourcefilename))
                {
                    wsgUtilities.wsgNotice("There is no file for that date");
                    cont = false;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
                cont = false;
            }
            if (cont)
            {
                string textfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "\\flushingbank\\SAFEANDSOUND" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                if (File.Exists(textfilename))
                {
                    File.Delete(textfilename);
                }

                StreamWriter Textout = new StreamWriter(textfilename);


                // Start the text file

                // Header lines
                // Line 1
                string headerline = "SA/MSINA";
                Textout.WriteLine(headerline);
                // Line 2
                headerline = "SA/MSMAA//004/99/999";
                Textout.WriteLine(headerline);
                // Line 3
                headerline = "SA/MSAON";
                Textout.WriteLine(headerline);
                // Line 4
                headerline = "SA/MSTAS//" + tellernumber;
                Textout.WriteLine(headerline);
                // Line 5
                headerline = "SA/MSTRA//" + tellernumber; Textout.WriteLine(headerline);

                // Detail lines

                using (StreamReader sr = new StreamReader(sourcefilename))
                {
                    String smartdata;
                    // Read lines from the file until the source table.
                    while ((inputdataline = sr.ReadLine()) != null)
                    {


                        /* Action depends on transaction code

                         GS – GL Cash in transit – declared
                         CS-  Customer DDA – declared
                         GV- GL Cash in vault – verified OR Fed transfer
                         CA – Customer DDA  - verified adjustment, if any
                       
                         Smart Safes:
                         SS – GL Cash in transit – declared
                         DS-  Customer DDA – declared
                         SV- GL Cash in vault – verified
                         DA – Customer DDA  - verified adjustment, if any

                         */

                        // Only use detail lines
                        if (inputdataline.Substring(0, 1) == "D")
                        {
                            switch (inputdataline.Substring(1, 2))
                            {
                                case "GV":
                                    if (inputdataline.Substring(12, 5).TrimEnd().TrimStart() != "")
                                    {
                                        // FED Transfer
                                        if (inputdataline.Substring(80, 1) == "C")
                                        {
                                            // Credit. It  could be a credit to FED or to the vault

                                            if (inputdataline.Substring(23, 9) == cashonhandaccount.TrimEnd())
                                            {
                                                // Outgoing FED - Credit Cash on Hand
                                                // GL Account number
                                                outputdataline = "SA-GLJE//" + cashonhandaccount + "/";
                                                outputdataline += inputdataline.Substring(52, 10).TrimStart(new Char[] { '0' }) + "///" + "Outgoing FED Cash";
                                                Textout.WriteLine(outputdataline);
                                                break;
                                            }
                                            else
                                            {
                                                //Incoming FED - Credit FED account


                                                // GL Account number
                                                outputdataline = "SA-GLJE//" + inputdataline.Substring(23, 9) + "/";
                                                outputdataline += inputdataline.Substring(52, 10).TrimStart(new Char[] { '0' }) + "///" + "Incoming FED FED";
                                                Textout.WriteLine(outputdataline);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                           
                                            // More work required here if this goes live
                                            // Debit. It could be a debit to the vault or the FED
                                            if (inputdataline.Substring(23, 9) == cashonhandaccount.TrimEnd())
                                            {
                                                // Incoming FED - Debit Cash on Hand


                                                // GL Account number
                                                outputdataline = "SA+GLJE//" + inputdataline.Substring(23, 9) + "/";
                                                outputdataline += inputdataline.Substring(52, 10).TrimStart(new Char[] { '0' }) + "///" + "Incoming FED Cash";
                                                Textout.WriteLine(outputdataline);
                                                break;
                                            }
                                            else
                                            {
                                                // Outgoing FED - Debit FED account


                                                // GL Account number
                                                outputdataline = "SA+GLJE//" + inputdataline.Substring(23, 9) + "/";
                                                outputdataline += inputdataline.Substring(52, 10).TrimStart(new Char[] { '0' }) + "///" + "Outgoing FED FED";
                                                Textout.WriteLine(outputdataline);
                                                break;
                                            }
                                        }


                                    }
                                    else
                                    {
                                        break;
                                    }

                                case "GS":
                                    {
                                        // GL Account number
                                        outputdataline = "SA-GLJE//" + cashintransitaccount + "/";
                                        outputdataline += inputdataline.Substring(52, 10).TrimStart(new Char[] { '0' }) + "///" + "Cash GL for Declared"; ;
                                        Textout.WriteLine(outputdataline);
                                        break;
                                    }
                                case "CS":
                                    {
                                        storecode = inputdataline.Substring(42, 10);

                                        // DDA Account number
                                        outputdataline = "SA+NDDFJE//" + inputdataline.Substring(22, 10) + "/";
                                        // Credit amount 
                                        outputdataline += inputdataline.Substring(52, 10).TrimStart(new Char[] { '0' }) + "///" + storecode;
                                        Textout.WriteLine(outputdataline);

                                        break;
                                    }

                                case "CA":
                                    {
                                        
                                        // Verified adjustment
                                        storecode = inputdataline.Substring(42, 10);

                                        // Debit and credits are handled differently
                                        if (inputdataline.Substring(80, 1) == "C")
                                        {
                                            // Credit
                                            //Two entries 
                                            // Customer - use DDA aAccount number
                                            outputdataline = "SA+NDDFJE//" + inputdataline.Substring(22, 10) + "/" + String.Format("{0:MMddyy}", PostingDate.AddDays(-1));
                                            // Credit amount 
                                            outputdataline += "00" + inputdataline.Substring(52, 10) + "///" + storecode;
                                            Textout.WriteLine(outputdataline);
                                            // Gl Use vault account number
                                            outputdataline = "SA-GLBFJE//" + cashonhandaccount + "/" + inputdataline.Substring(52, 10).TrimStart(new Char[] { '0' }) + "/" + String.Format("{0:MMddyy}", PostingDate.AddDays(-1)) + "//GL  Verification Adjustment";
                                            Textout.WriteLine(outputdataline);


                                        }
                                        else
                                        {
                                            // Debit
                                            //Two entries 
                                            // Customer - use DDA aAccount number
                                            outputdataline = "SA-NDDFJE//" + inputdataline.Substring(22, 10) + "/" + String.Format("{0:MMddyy}", PostingDate.AddDays(-1));
                                            // Credit amount 
                                            outputdataline += "00" + inputdataline.Substring(52, 10) + "///" + storecode;
                                            Textout.WriteLine(outputdataline);
                                            // Gl Use vault account number
                                            outputdataline = "SA+GLBFJE//" + cashonhandaccount + "/" + inputdataline.Substring(52, 10).TrimStart(new Char[] { '0' }) + "/" + String.Format("{0:MMddyy}", PostingDate.AddDays(-1)) + "//GL Verification Adjustment";
                                            Textout.WriteLine(outputdataline);


                                        }
                                        break;
                                    }

                                default:
                                    {
                                        continue;
                                        break;
                                    }
                            }



                        }

                    }

                    // Footer lines
                    // Line 1
                    string footerline = "SA/MSINA";
                    Textout.WriteLine(footerline);
                    // Line 2
                    footerline = "SA/MSAOF";
                    Textout.WriteLine(footerline);

                    Textout.Flush();
                    Textout.Close();

                }
                wsgUtilities.wsgNotice("Operation Complete");
            }
        }
        public void SendBankLeumiFiles()
        {
            string commandstring = "";
            bool cont = true;
            DateTime PostingDate = DateTime.Now.Date;
            string sourcefilename = "";
            if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 200, 200))
            {
                PostingDate = commonAppDataMethods.SelectedDate.Date;

                sourcefilename = ConfigurationManager.AppSettings["FTPFilepath"] + "\\026002794\\" + "ss" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                if (!File.Exists(sourcefilename))
                {
                    wsgUtilities.wsgNotice("There is no file for that date");
                    cont = false;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
                cont = false;
            }
            if (cont)
            {
                string textfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "\\026002794\\" + "leumi" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                if (File.Exists(textfilename))
                {
                    File.Delete(textfilename);
                }

                StreamWriter Textout = new StreamWriter(textfilename);


                // Start the text file

                using (StreamReader sr = new StreamReader(sourcefilename))
                {
                    String dataline;
                    String smartdata;
                    // Read lines from the file until the source table.
                    while ((dataline = sr.ReadLine()) != null)
                    {

                        if (dataline.Substring(1, 2) == "CS" || dataline.Substring(1, 2) == "CA")
                        {

                            string store = dataline.Substring(42, 10);
                            string compcode = dataline.Substring(42, 4);
                            store = store.Replace(" ", "0");

                            // For transactions that affect customer account, show demographic data
                            ssprocessds.store.Rows.Clear();
                            commandstring = "SELECT * FROM store WHERE storecode = @storecode";
                            ClearParameters();
                            AddParms("@storecode", dataline.Substring(42, 4) + "-" + dataline.Substring(46, 6), "SQL");
                            FillData(ssprocessds, "store", commandstring, CommandType.Text);
                            string storeLocation = "";
                            string stroreName = "";
                            if (ssprocessds.store.Rows.Count > 0)
                            {
                                // name
                                stroreName = ssprocessds.store[0].store_name.TrimEnd();
                                // location
                                if (
                                    !String.IsNullOrEmpty(ssprocessds.store[0].f_city.TrimEnd()) &&
                                    !String.IsNullOrEmpty(ssprocessds.store[0].f_state.TrimEnd())
                                    )
                                {
                                    storeLocation = ssprocessds.store[0].f_city.TrimEnd() + " " + ssprocessds.store[0].f_state.TrimEnd();
                                }
                                else
                                {
                                    ssprocessds.company.Rows.Clear();
                                    commandstring = "SELECT * FROM company WHERE comp_code = @comp_code";
                                    ClearParameters();
                                    AddParms("@comp_code", compcode, "SQL");
                                    FillData(ssprocessds, "company", commandstring, CommandType.Text);
                                    storeLocation = ssprocessds.company[0].city.TrimEnd() + " " + ssprocessds.company[0].state.TrimEnd();
                                }
                                dataline +=
                                    String.Format("{0,30}", stroreName);
                                dataline +=
                                    String.Format("{0,30}", storeLocation);
                            }
                            else
                            {
                                ssprocessds.company.Rows.Clear();
                                commandstring = "SELECT * FROM company WHERE comp_code = @comp_code";
                                ClearParameters();
                                AddParms("@comp_code", compcode, "SQL");
                                FillData(ssprocessds, "company", commandstring, CommandType.Text);
                                if (ssprocessds.store.Rows.Count > 0)
                                {
                                    stroreName = ssprocessds.company[0].name.TrimEnd();
                                    storeLocation = ssprocessds.company[0].city.TrimEnd() + " " + ssprocessds.company[0].state.TrimEnd();
                                    dataline +=
                                        String.Format("{0,30}", stroreName);
                                    dataline +=
                                        String.Format("{0,30}", storeLocation);
                                }
                                else
                                {
                                    for (int i = 0; i < 60; i++)
                                    {
                                        dataline += " ";
                                    }
                                }

                            }


                            /* Bill's original


                            if (ssprocessds.store.Rows.Count > 0)
                            {
                                dataline += ssprocessds.store[0].store_name.TrimEnd().PadLeft(30) + (ssprocessds.store[0].f_city.TrimEnd() +  " " + ssprocessds.store[0].f_state.TrimEnd()).PadLeft(30); 
                            }
                        
                            */
                        }
                        Textout.WriteLine(dataline);
                    }
                    Textout.Flush();
                    Textout.Close();

                }
                wsgUtilities.wsgNotice("Operation Complete");
            }
        }
        public bool MergeSignatureFiles(DateTime PostingDate, string BankFedid)
        {
            ssprocess ssprocessds = new ssprocess();
            ssprocess declaredssprocessds = new ssprocess();
            ssprocess sortedssprocess = new ssprocess();
            ssprocess openssprocessds = new ssprocess();
            ssprocess closedssprocessds = new ssprocess();
            BankFedid = "8923847";
            string commandstring = "";
            decimal debittotal = 0;
            decimal credittotal = 0;
            decimal transtotal = 0;
            int debitcount = 0;
            int creditcount = 0;
            string smartsafebankfedid = "SMARTSAFE";

            // Get all Smart Safe transctions for this posting date except verified
            commandstring = "SELECT * FROM smartsafetrans WHERE bankfedid = @bankfedid AND postingdate = @postingdate AND eventcode <> 'VER'";
            ClearParameters();
            ssprocessds.smartsafetrans.Rows.Clear();
            AddParms("@postingdate", PostingDate.Date, "SQL");
            AddParms("@bankfedid", smartsafebankfedid, "SQL");
            FillData(ssprocessds, "smartsafetrans", commandstring, CommandType.Text);

            // Get all Smart Safe verified transctions for this posting date
            commandstring = "SELECT * FROM view_smartsafeverified WHERE bankfedid = @bankfedid AND postingdate = @postingdate ";
            ssprocessds.view_smartsafeverified.Rows.Clear();
            ClearParameters();
            AddParms("@postingdate", PostingDate.Date, "SQL");
            AddParms("@bankfedid", smartsafebankfedid, "SQL");
            FillData(ssprocessds, "view_smartsafeverified", commandstring, CommandType.Text);


            bool ll_cont = true;

            string sourcetextfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "8923847" + "\\" + "ss" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
            string textfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "8923847" + "\\Merged\\" + "SAFESOUND" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
            string totalsfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "8923847" + "\\Merged\\" + "SAFEANDSOUNDTOTALS" + String.Format("{0:MMddyy}", PostingDate) + ".txt";

            StreamWriter Textout = new StreamWriter(textfilename);


            //    "SafeSoundTestFile"
            //    string textfilename = ConfigurationManager.AppSettings["FTPFilepath"] +
            //    "SafeSoundTestFile" + commonAppDataMethods.SelectedDate.Date.ToString("MMddyyyy") +
            //     DateTime.Now.ToString("yyyy-MM-dd HHmm ").Substring(11, 4) + ".csv";
            // Start the text file

            using (StreamReader sr = new StreamReader(sourcetextfilename))
            {

                string cashintransitacct = "100000011";
                string cashacct = "100000010";
                string rcnum = "01020008";
                string branchnumber = "426";
                debittotal = 0;
                credittotal = 0;
                debitcount = 0;
                creditcount = 0;
                String dataline;
                String smartdata;
                // Read lines from the file until the end of the file is reached.
                // Populate the data table
                while ((dataline = sr.ReadLine()) != null)
                {

                    // When total line is reached, add lines
                    if (dataline.Substring(0, 1) == "T")
                    {
                        // Add declared and FED tranactions
                        for (int i = 0; i <= ssprocessds.smartsafetrans.Rows.Count - 1; i++)
                        {
                            transtotal = 0;
                            switch (ssprocessds.smartsafetrans[i].eventcode.TrimEnd())
                            {

                                case "DECL":
                                    {
                                        debitcount++;
                                        creditcount++;
                                        debittotal += ssprocessds.smartsafetrans[i].saidtocontain;
                                        credittotal += ssprocessds.smartsafetrans[i].saidtocontain;

                                        // Write two lines - Debit Cash in transit and credit the DDA
                                        smartdata = "DSSSSA" + " ".PadLeft(6) + ssprocessds.smartsafetrans[i].masteraccountid.Substring(0, 10) + cashintransitacct.TrimEnd().PadLeft(10, '0') + ssprocessds.smartsafetrans[i].idcol.ToString().PadRight(10);
                                        smartdata += branchnumber.PadLeft(10, '0') + (Math.Abs(ssprocessds.smartsafetrans[i].saidtocontain * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                        smartdata += "000000" + "D".PadLeft(12) + " ".PadLeft(10);
                                        Textout.WriteLine(smartdata);
                                        smartdata = "DDSSSA" + " ".PadLeft(6) + ssprocessds.smartsafetrans[i].masteraccountid.Substring(0, 10) + ssprocessds.smartsafetrans[i].customerdda.TrimEnd().PadLeft(10, '0') + ssprocessds.smartsafetrans[i].idcol.ToString().PadRight(10);
                                        smartdata += ssprocessds.smartsafetrans[i].store.Substring(0, 4) + ssprocessds.smartsafetrans[i].store.Substring(5, 6) + (Math.Abs(ssprocessds.smartsafetrans[i].saidtocontain * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                        smartdata += "000000" + "C".PadLeft(12) + " ".PadLeft(10);
                                        Textout.WriteLine(smartdata);
                                        break;
                                    }
                                case "FED":
                                    {
                                        transtotal = ssprocessds.smartsafetrans[i].hundreds + ssprocessds.smartsafetrans[i].fiftys + ssprocessds.smartsafetrans[i].twentys;
                                        transtotal += ssprocessds.smartsafetrans[i].tens + ssprocessds.smartsafetrans[i].fives + ssprocessds.smartsafetrans[i].twos;
                                        transtotal += ssprocessds.smartsafetrans[i].ones + ssprocessds.smartsafetrans[i].mixedcoin;


                                        debitcount++;
                                        creditcount++;
                                        debittotal += transtotal;
                                        credittotal += transtotal;

                                        // Write two lines - Debit the recieving account and credit the sending account
                                        smartdata = "DGVSSA" + " ".PadLeft(6) + "01020003".PadRight(10) + "100000002".PadLeft(10, '0') + ssprocessds.smartsafetrans[i].idcol.ToString().PadRight(10);
                                        smartdata += branchnumber.PadLeft(10, '0') + (Math.Abs(transtotal * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                        smartdata += "000000" + "D".PadLeft(12) + " ".PadLeft(10);
                                        Textout.WriteLine(smartdata);
                                        smartdata = "DGVSSA" + " ".PadLeft(6) + rcnum.PadRight(10) + cashacct.TrimEnd().PadLeft(10, '0') + ssprocessds.smartsafetrans[i].idcol.ToString().PadRight(10);
                                        smartdata += branchnumber.PadLeft(10, '0') + (Math.Abs(transtotal * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                        smartdata += "000000" + "C".PadLeft(12) + " ".PadLeft(10);
                                        Textout.WriteLine(smartdata);
                                        break;
                                    }
                                case "BULK":
                                    {
                                        transtotal = ssprocessds.smartsafetrans[i].hundreds + ssprocessds.smartsafetrans[i].fiftys + ssprocessds.smartsafetrans[i].twentys;
                                        transtotal += ssprocessds.smartsafetrans[i].tens + ssprocessds.smartsafetrans[i].fives + ssprocessds.smartsafetrans[i].twos;
                                        transtotal += ssprocessds.smartsafetrans[i].ones + ssprocessds.smartsafetrans[i].mixedcoin;


                                        debitcount++;
                                        creditcount++;
                                        debittotal += transtotal;
                                        credittotal += transtotal;

                                        // Write two lines - credit the sending account and debit the receiving account
                                        smartdata = "DGVSSA" + " ".PadLeft(6) + "01020003".PadRight(10) + "100000002".PadLeft(10, '0') + ssprocessds.smartsafetrans[i].idcol.ToString().PadRight(10);
                                        smartdata += branchnumber.PadLeft(10, '0') + (Math.Abs(transtotal * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                        smartdata += "000000" + "C".PadLeft(12) + " ".PadLeft(10);
                                        Textout.WriteLine(smartdata);
                                        smartdata = "DGVSSA" + " ".PadLeft(6) + rcnum.PadRight(10) + cashacct.TrimEnd().PadLeft(10, '0') + ssprocessds.smartsafetrans[i].idcol.ToString().PadRight(10);
                                        smartdata += branchnumber.PadLeft(10, '0') + (Math.Abs(transtotal * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                        smartdata += "000000" + "D".PadLeft(12) + " ".PadLeft(10);
                                        Textout.WriteLine(smartdata);
                                        break;
                                    }
                               
                                default:
                                    {
                                        break;
                                    }
                            }
                        }
                        for (int v = 0; v <= ssprocessds.view_smartsafeverified.Rows.Count - 1; v++)
                        {

                            debitcount++;
                            creditcount++;
                            debittotal += ssprocessds.view_smartsafeverified[v].totaldeposit;
                            credittotal += Math.Abs(ssprocessds.view_smartsafeverified[v].saidtocontain);

                            // Write the debit Cash line
                            smartdata = "DSVSSA" + " ".PadLeft(6) + ssprocessds.view_smartsafeverified[v].masteraccountid.Substring(0, 10) + cashacct.TrimEnd().PadLeft(10, '0') + ssprocessds.view_smartsafeverified[v].idcol.ToString().PadRight(10);
                            smartdata += branchnumber.PadLeft(10, '0') + (Math.Abs(ssprocessds.view_smartsafeverified[v].totaldeposit * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                            smartdata += "000000" + "D".PadLeft(12) + " ".PadLeft(10);
                            Textout.WriteLine(smartdata);

                            // Locate the related declared transactions and create one entry for each.
                            commandstring = "SELECT * FROM smartsafetrans WHERE eventcode = 'DECL' AND verifyid = @verifyid";
                            declaredssprocessds.smartsafetrans.Rows.Clear();
                            ClearParameters();
                            ssprocessds.smartsafetrans.Rows.Clear();
                            AddParms("@verifyid", ssprocessds.view_smartsafeverified[v].idcol, "SQL");
                            FillData(declaredssprocessds, "smartsafetrans", commandstring, CommandType.Text);
                            for (int d = 0; d <= declaredssprocessds.smartsafetrans.Rows.Count - 1; d++)
                            {


                                smartdata = "DSVSSA" + " ".PadLeft(6) + ssprocessds.view_smartsafeverified[v].masteraccountid.Substring(0, 10) + cashintransitacct.TrimEnd().PadLeft(10, '0') + ssprocessds.view_smartsafeverified[v].idcol.ToString().PadRight(10);
                                smartdata += branchnumber.PadLeft(10, '0') + (Math.Abs(declaredssprocessds.smartsafetrans[d].saidtocontain * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                smartdata += "000000" + "C".PadLeft(12) + " ".PadLeft(10);
                                Textout.WriteLine(smartdata);
                            }

                            // If there is a difference, write a debit or credit to the customers DDA.
                            if (ssprocessds.view_smartsafeverified[v].discrepancy != 0)
                            {
                                smartdata = "DDASSA" + " ".PadLeft(6) + ssprocessds.view_smartsafeverified[v].masteraccountid.Substring(0, 10) + ssprocessds.view_smartsafeverified[v].customerdda.TrimEnd().PadLeft(10, '0') + ssprocessds.view_smartsafeverified[v].idcol.ToString().PadRight(10);
                                smartdata += ssprocessds.view_smartsafeverified[v].store.Substring(0, 4) + ssprocessds.view_smartsafeverified[v].store.Substring(5, 6) + (Math.Abs(ssprocessds.view_smartsafeverified[v].discrepancy * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                smartdata += "000000" + (Math.Abs(ssprocessds.view_smartsafeverified[v].saidtocontain * 100)).ToString().PadLeft(14, '0').Substring(0, 11);
                                if (ssprocessds.view_smartsafeverified[v].discrepancy < 0)
                                {
                                    smartdata += "D";
                                    debittotal += Math.Abs(ssprocessds.view_smartsafeverified[v].discrepancy);
                                    debitcount++;
                                }
                                else
                                {
                                    smartdata += "C";
                                    credittotal += Math.Abs(ssprocessds.view_smartsafeverified[v].discrepancy);
                                    creditcount++;
                                }
                                smartdata += " ".PadLeft(10) + String.Format("{0:MM/dd/yyyy}", ssprocessds.view_smartsafeverified[v].postingdate);
                                Textout.WriteLine(smartdata);

                            }

                        }

                        // Develop totals
                        credittotal += Convert.ToDecimal(dataline.Substring(30, 11)) / 100;
                        creditcount += Convert.ToInt32(dataline.Substring(19, 11));
                        debittotal += Convert.ToDecimal(dataline.Substring(52, 11)) / 100;
                        debitcount += Convert.ToInt32(dataline.Substring(41, 11));
                        smartdata = "T" + String.Format("{0:MM/dd/yyyy}", PostingDate);
                        smartdata += String.Format("{0: hh}", DateTime.Now).TrimStart().TrimEnd();
                        smartdata += String.Format("{0: mm}", DateTime.Now).TrimStart().TrimEnd();
                        smartdata += String.Format("{0: ss}", DateTime.Now).TrimStart().TrimEnd();
                        smartdata += "  ";
                        smartdata += creditcount.ToString().PadLeft(11, '0');
                        smartdata += (credittotal * 100).ToString().PadLeft(14, '0').Substring(0, 11);
                        smartdata += debitcount.ToString().PadLeft(11, '0');
                        smartdata += (debittotal * 100).ToString().PadLeft(14, '0').Substring(0, 11);

                        Textout.WriteLine(smartdata);
                    }
                    else
                    {
                        Textout.WriteLine(dataline);

                    }
                }
            }
            Textout.Flush();
            Textout.Close();
            StreamWriter TotalFileout = new StreamWriter(totalsfilename);

            string totalline = "Signature Bank File Control Totals - Date " + String.Format("{0:MMddyy}", PostingDate);
            TotalFileout.WriteLine(totalline);
            totalline = "To: FIDELITY Information Services";
            TotalFileout.WriteLine(totalline);
            totalline = "FNFIS ECC Mainframe Operations";
            TotalFileout.WriteLine(totalline);
            totalline = "Fax No. 904-854-4215";
            TotalFileout.WriteLine(totalline);
            totalline = "Email: ecc.fax@fnf.com or fnfis.lit.cc.mainframe.operations@fisglobal.com";
            TotalFileout.WriteLine(totalline);
            totalline = "Phone No. 501-220-7200";
            TotalFileout.WriteLine(totalline);
            totalline = "From: Safe and Sound 516-328-1195";
            TotalFileout.WriteLine(totalline);
            totalline = "Job to pull file to COP Mainframe: C175P710";
            TotalFileout.WriteLine(totalline);
            totalline = "Connect Process Name: BK75ANT";
            TotalFileout.WriteLine(totalline);
            totalline = "FIDELITY Catalogued Data Set Names:";
            TotalFileout.WriteLine(totalline);
            totalline = "Server: C:\\FTPROOT\\SAFESOUND\\SAFESOUND";
            TotalFileout.WriteLine(totalline);
            totalline = "Mainframe: PSD1.XM.P750.SAFESND.INPUT(+1)";
            TotalFileout.WriteLine(totalline);
            totalline = "File Batch Number:8809";
            TotalFileout.WriteLine(totalline);
            totalline = "RI Verification Job: Ri75P026";
            TotalFileout.WriteLine(totalline);
            totalline = "Debits  - Count " + debitcount.ToString("N0") + " Amount " + debittotal.ToString("N2");
            TotalFileout.WriteLine(totalline);
            totalline = "Credit  - Count " + creditcount.ToString("N0") + " Amount " + credittotal.ToString("N2");
            TotalFileout.WriteLine(totalline);
            TotalFileout.Flush();
            TotalFileout.Flush();
            TotalFileout.Close();


            return ll_cont;

        }

        public bool CreateFlushingBankSmartsafeFile(DateTime PostingDate)
        {
            ssprocess ssprocessds = new ssprocess();
            ssprocess declaredssprocessds = new ssprocess();
            ssprocess sortedssprocess = new ssprocess();
            ssprocess openssprocessds = new ssprocess();
            ssprocess closedssprocessds = new ssprocess();
            string tellernumber = "9974";
            string smartdata = "";
            string commandstring = "";
            decimal transtotal = 0;
            int debitcount = 0;
            int creditcount = 0;
            // Plug bank fed id for tes = should be FBSMARTSAFE
            string smartsafebankfedid = "SMARTSAFE";
            ssprocessds.bank.Rows.Clear();
            commandstring = "SELECT * FROM bank WHERE RTRIM(bankfedid) = @bankfedid";
            ClearParameters();
            AddParms("@bankfedid", smartsafebankfedid, "SQL");
            FillData(ssprocessds, "bank", commandstring, CommandType.Text);
       //     string cashonhandaccount = ssprocessds.bank[0].cohaccount.TrimEnd();
       //     string cashintransitaccount = ssprocessds.bank[0].citaccount.TrimEnd();
            string fedaccount = "1010005090";
            // Flushing bank uses an account structure that is incompatible with other banks, hence the hard coding here.
            string cashonhandaccount = "1001041000";
            string cashintransitaccount = "1001041030";
       
           

         
            // Get all Smart Safe transctions for this posting date except verified
            commandstring = "SELECT * FROM smartsafetrans WHERE bankfedid = @bankfedid AND postingdate = @postingdate AND eventcode <> 'VER'";
            ClearParameters();
            ssprocessds.smartsafetrans.Rows.Clear();
            AddParms("@postingdate", PostingDate.Date, "SQL");
            AddParms("@bankfedid", smartsafebankfedid, "SQL");
            FillData(ssprocessds, "smartsafetrans", commandstring, CommandType.Text);

            // Get all Smart Safe verified transctions for this posting date
            commandstring = "SELECT * FROM view_smartsafeverified WHERE bankfedid = @bankfedid AND postingdate = @postingdate ";
            ssprocessds.view_smartsafeverified.Rows.Clear();
            ClearParameters();
            AddParms("@postingdate", PostingDate.Date, "SQL");
            AddParms("@bankfedid", smartsafebankfedid, "SQL");
            FillData(ssprocessds, "view_smartsafeverified", commandstring, CommandType.Text);


            bool ll_cont = true;

            string textfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "flushingbank" + "\\" + "SMARTSAFESAFEANDSOUND" + String.Format("{0:MMddyy}", PostingDate) + ".txt";

            StreamWriter Textout = new StreamWriter(textfilename);


            // Start the text file

            // Header lines
            // Line 1
            string headerline = "SA/MSINA";
            Textout.WriteLine(headerline);
            // Line 2
            headerline = "SA/MSMAA//004/99/999";
            Textout.WriteLine(headerline);
            // Line 3
            headerline = "SA/MSAON";
            Textout.WriteLine(headerline);
            // Line 4
            headerline = "SA/MSTAS//" + tellernumber;
            Textout.WriteLine(headerline);
            // Line 5
            headerline = "SA/MSTRA//" + tellernumber; Textout.WriteLine(headerline);

            // Detail
            string storecode = "";
            string outputdataline = "";
            string rcnum = "01020008";
            string branchnumber = "426";
            debitcount = 0;
            creditcount = 0;
            String dataline;
            // Read lines from the file until the end of the file is reached.
            // Populate the data table
            // Add declared and FED tranactions
            for (int i = 0; i <= ssprocessds.smartsafetrans.Rows.Count - 1; i++)
            {
                storecode = ssprocessds.smartsafetrans[i].store.Substring(0, 11);
                   
                transtotal = 0;
                switch (ssprocessds.smartsafetrans[i].eventcode.TrimEnd())
                {
                   
                    case "DECL":
                        {

                            // Credit
                            //Two entries 
                            // Customer - use DDA aAccount number
                            outputdataline = "SA+NDDFJE//" + ssprocessds.smartsafetrans[i].customerdda.TrimEnd().PadLeft(10, '0') + "/" ;
                            // Credit amount 
                            outputdataline += ((int) Math.Abs(ssprocessds.smartsafetrans[i].saidtocontain * 100)).ToString().TrimStart() + "///" + storecode + " Customer Declared";
                            Textout.WriteLine(outputdataline);
                            // Gl Use vault account number
                            outputdataline = "SA-GLJE//" + cashonhandaccount + "/" + ((int) Math.Abs(ssprocessds.smartsafetrans[i].saidtocontain * 100)).ToString().TrimStart() +  "///Declared GL";
                            Textout.WriteLine(outputdataline);
                            break;
                        }
                    case "FED":
                        {
                          // Outgoing

                            // Outgoing to FED - Debit FED account Credit Cash on Hand

                            // Compute the total
                            transtotal = ssprocessds.smartsafetrans[i].hundreds + ssprocessds.smartsafetrans[i].fiftys +
                                ssprocessds.smartsafetrans[i].twentys + ssprocessds.smartsafetrans[i].tens +
                                ssprocessds.smartsafetrans[i].fives + ssprocessds.smartsafetrans[i].twos + ssprocessds.smartsafetrans[i].ones +
                                ssprocessds.smartsafetrans[i].mixedcoin;
                  
                            // FED GL Account number
                            outputdataline = "SA-GLJE//" + fedaccount + "/";
                            outputdataline +=   ( (int) Math.Abs(transtotal * 100)).ToString().TrimStart() + "///" + "Outgoing FED FED";
                            Textout.WriteLine(outputdataline);
                            // Cash on hand GL Account number
                            outputdataline = "SA+GLJE//" + cashonhandaccount + "/";
                            outputdataline += ((int) Math.Abs(transtotal * 100)).ToString().TrimStart() + "///" + "Outgoing FED Vault";
                            Textout.WriteLine(outputdataline);
              
                            
                            break;
                                     
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            for (int v = 0; v <= ssprocessds.view_smartsafeverified.Rows.Count - 1; v++)
            {
                storecode = ssprocessds.view_smartsafeverified[v].store;
                // Locate the related declared transactions and create one entry for each.
                commandstring = "SELECT * FROM smartsafetrans WHERE eventcode = 'DECL' AND verifyid = @verifyid";
                declaredssprocessds.smartsafetrans.Rows.Clear();
                ClearParameters();
                AddParms("@verifyid", ssprocessds.view_smartsafeverified[v].idcol, "SQL");
                FillData(declaredssprocessds, "smartsafetrans", commandstring, CommandType.Text);
                for (int d = 0; d <= declaredssprocessds.smartsafetrans.Rows.Count - 1; d++)
                {

                    // Gl Use vault account number
                    outputdataline = "SA-GLJE//" + cashonhandaccount + "/" + ((int) Math.Abs(declaredssprocessds.smartsafetrans[d].saidtocontain * 100)).ToString().TrimStart() +  "///GL  Verification Credit";
                    Textout.WriteLine(outputdataline);

                }



                transtotal = ssprocessds.view_smartsafeverified[v].hundreds + ssprocessds.view_smartsafeverified[v].fiftys +
                            ssprocessds.view_smartsafeverified[v].twentys + ssprocessds.view_smartsafeverified[v].tens +
                            ssprocessds.view_smartsafeverified[v].fives + ssprocessds.view_smartsafeverified[v].twos + ssprocessds.view_smartsafeverified[v].ones +
                            ssprocessds.view_smartsafeverified[v].mixedcoin;
                  
              

                // Write the debit Cash line
                // Gl Use vault account number
                outputdataline = "SA-GLJE//" + cashonhandaccount + "/" + ((int) Math.Abs(transtotal * 100)).ToString().TrimStart() + "///GL  Verification Debit";
                Textout.WriteLine(outputdataline);
           
            
                // If there is a difference, write a debit or credit to the customers DDA.
                if (ssprocessds.view_smartsafeverified[v].discrepancy != 0)
                {
                    if (ssprocessds.view_smartsafeverified[v].discrepancy > 0)
                    {

                     
                        // Customer Credit
                        //Two entries 
                        // Customer - use DDA aAccount number
                        outputdataline = "SA+NDDFJE//" + ssprocessds.view_smartsafeverified[v].customerdda.TrimEnd() + "/";
                        // Credit amount 
                        outputdataline +=  (Math.Abs(ssprocessds.view_smartsafeverified[v].discrepancy * 100)).ToString().TrimStart()+ "///" + storecode + "Customer adjustment";
                        Textout.WriteLine(outputdataline);
                        // Gl Use vault account number
                        outputdataline = "SA-GLJE//" + cashonhandaccount + "/" +  ( (int) Math.Abs(ssprocessds.view_smartsafeverified[v].discrepancy * 100)).ToString().TrimStart() + "///GL  Verification Adjustment";
                        Textout.WriteLine(outputdataline);


                    }
                    else
                    {
                        // Customer Debit
                        //Two entries 
                        // Customer - use DDA aAccount number
                        outputdataline = "SA-NDDFJE//" + ssprocessds.view_smartsafeverified[v].customerdda.TrimEnd() + "/" ;
                        // Credit amount 
                        outputdataline +=  ((int) Math.Abs(ssprocessds.view_smartsafeverified[v].discrepancy * 100)).ToString().TrimStart() +  "///" + storecode + "Customer adjustment";
                        Textout.WriteLine(outputdataline);
                        // Gl Use vault account number
                        outputdataline = "SA+GLJE//" + cashonhandaccount + "/" + ( (int) Math.Abs(ssprocessds.view_smartsafeverified[v].discrepancy * 100)).ToString().TrimStart()+ "///GL  Verification Adjustment";
                        Textout.WriteLine(outputdataline);

                    }
                }
                else
                {
                    // Write the credit Cash line
                    // Gl Use vault account number
                    outputdataline = "SA+GLJE//" + cashintransitaccount + "/" +  ((int) Math.Abs(transtotal * 100)).ToString().TrimStart()  + "///GL  Verification Credit";
                    Textout.WriteLine(outputdataline);
          
                }

            }
            // Footer lines
            // Line 1
            string footerline = "SA/MSINA";
            Textout.WriteLine(footerline);
            // Line 2
            footerline = "SA/MSAOF";
            Textout.WriteLine(footerline);
            Textout.Flush();
            Textout.Close();
            wsgUtilities.wsgNotice("Operation Complete");
            return ll_cont;
        }



        public void SendSafekeepingBankeEmail()
        {
            DateTime PostingDate = new DateTime();

            bool transmitok = true;
            string proofsheetfilename = "";
            string activityfilename = "";
            string bankfedid = "";
            int selectedbankid = commonAppDataMethods.SelectBank();

            // If a row has been selected fill the data and process
            if (selectedbankid > 0)
            {
                ssprocessds.bank.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", selectedbankid, "SQL");
                FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
                bankfedid = ssprocessds.bank[0].bankfedid;
                // Special Processing for certain banks
                if (bankfedid.TrimEnd() == ConfigurationManager.AppSettings["SignatureInventoryID"])
                {
                    SendSignatureEmail();
                }
                else
                {
                    if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 200, 200))
                    {
                        PostingDate = commonAppDataMethods.SelectedDate.Date;
                        proofsheetfilename = ConfigurationManager.AppSettings["PDFFilespath"] + bankfedid.TrimEnd() + "\\" + "ps" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                        activityfilename = ConfigurationManager.AppSettings["PDFFilespath"] + bankfedid.TrimEnd() + "\\" + "ac" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                    }
                    else
                    {
                        wsgUtilities.wsgNotice("Operation cancelled");
                        transmitok = false;
                    }
                    string emailsubject = "Safe and Sound e-mail notification " + String.Format("{0:MM/dd/yy}", PostingDate);

                    if (transmitok)
                    {
                        if (!File.Exists(proofsheetfilename))
                        {
                            wsgUtilities.wsgNotice("The proof sheet file for that date does not exist. Run the proof sheet and try to send again.");
                            transmitok = false;
                        }
                    }
                    if (transmitok)
                    {

                        if (!File.Exists(activityfilename))
                        {
                            wsgUtilities.wsgNotice("The activity report file for that date does not exist. Run the activity and try to send again.");
                            transmitok = false;
                        }
                    }

                    // Develop email address for the bank
                    ssprocessds.emailbank.Clear();
                    string commandstring = "SELECT * FROM emailbank WHERE bankfedid  = @bankfedid";
                    ClearParameters();
                    AddParms("@bankfedid", ssprocessds.bank[0].bankfedid, "SQL");
                    FillData(ssprocessds, "emailbank", commandstring, CommandType.Text);
                    if (ssprocessds.emailbank.Rows.Count < 1)
                    {
                        wsgUtilities.wsgNotice("There are no email addresses for this bank");
                        transmitok = false;
                    }


                    if (transmitok)
                    {
                        if (!wsgUtilities.wsgReply("Do you want to send the Safekeeping Emails?"))
                        {
                            wsgUtilities.wsgNotice("Operation cancelled");
                            transmitok = false;
                        }
                    }

                    if (transmitok)
                    {

                        System.Net.Mail.MailMessage bankMessage = new System.Net.Mail.MailMessage();
                        bankMessage.To.Clear();

                        for (int i = 0; i <= ssprocessds.emailbank.Rows.Count - 1; i++)
                        {
                            bankMessage.To.Add(ssprocessds.emailbank[i].emailaddr.TrimEnd());
                        }
                        if (ConfigurationManager.AppSettings["TestMode"] == "True")
                        {
                            bankMessage.To.Clear();
                            bankMessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                        }

                        // Send the proof sheet, activity report and total files
                        bankMessage.Attachments.Clear();
                        bankMessage.Attachments.Add(new System.Net.Mail.Attachment(proofsheetfilename));
                        bankMessage.Attachments.Add(new System.Net.Mail.Attachment(activityfilename));
                        // Send the pdf's
                        appUtilities.SendEmail(bankMessage, emailsubject, "See Attached");
                        wsgUtilities.wsgNotice("Email Transmission complete.");
                    }
                }
            }
        }


        public void SendSignatureEmail()
        {
            string commandstring = "";
            ssprocessds.bank.Rows.Clear();
            commandstring = "SELECT * FROM bank WHERE RTRIM(bankfedid) = @bankfedid";
            ClearParameters();
            AddParms("@bankfedid", ConfigurationManager.AppSettings["SignatureInventoryID"], "SQL");
            FillData(ssprocessds, "bank", commandstring, CommandType.Text);

            DateTime PostingDate = new DateTime();
            if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 200, 200))
            {
                bool transmitok = true;
                PostingDate = commonAppDataMethods.SelectedDate.Date;

                string totalsfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "8923847" + "\\Merged\\" + "SAFEANDSOUNDTOTALS" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                string proofsheetfilename = ConfigurationManager.AppSettings["PDFFilespath"] + "8923847\\" + "ps" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                string activityfilename = ConfigurationManager.AppSettings["PDFFilepath"] + "8923847\\" + "ps" + "ac" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                string ssproofsheetfilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "smartsafeproofsheet" + String.Format("{0:MM-dd-yy}", PostingDate) + ".pdf";
                string ssactivityfilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "smartsafeactivity" + String.Format("{0:MM-dd-yy}", PostingDate) + ".pdf";
                string pgpsourcefilenamewithfedid = ConfigurationManager.AppSettings["FTPFilespath"] + ssprocessds.bank[0].bankfedid.TrimEnd() + "\\" + "ss" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                string totalsfilenamewithfedid = ConfigurationManager.AppSettings["FTPFilepath"] + ssprocessds.bank[0].bankfedid.TrimEnd() + "\\Merged\\" + "SAFEANDSOUNDTOTALS" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                string proofsheetfilenamewithfedid = ConfigurationManager.AppSettings["PDFFilespath"] + ssprocessds.bank[0].bankfedid.TrimEnd() + "\\" + "ps" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                string activityfilenamewithfedid = ConfigurationManager.AppSettings["PDFFilespath"] + ssprocessds.bank[0].bankfedid.TrimEnd() + "\\" + "ac" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                string ssunverifiedfilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "smartsafeunverified" + String.Format("{0:MM-dd-yy}", PostingDate) + ".pdf";

                string emailsubject = "Safe and Sound e-mail notification " + String.Format("{0:MM/dd/yy}", PostingDate);
                if (transmitok)
                {


                    if (!File.Exists(ssproofsheetfilename))
                    {
                        wsgUtilities.wsgNotice("The Smart Safe Proof Sheet for that date does not exist. Run the proof sheet and try to send again.");
                        transmitok = false;

                    }
                }
                if (transmitok)
                {


                    if (!File.Exists(ssactivityfilename))
                    {
                        wsgUtilities.wsgNotice("The Smart Safe Activity Analysis for that date does not exist. Run the Activity Analysyis and try to send again.");
                        transmitok = false;

                    }
                }
                if (transmitok)
                {


                    if (!File.Exists(ssunverifiedfilename))
                    {
                        wsgUtilities.wsgNotice("The Smart Safe Unverified Analysis for that date does not exist. Run the report and try to send again.");
                        transmitok = false;

                    }
                }
                if (transmitok)
                {
                    if (!File.Exists(proofsheetfilename))
                    {
                        if (File.Exists(proofsheetfilenamewithfedid))
                        {
                            // Use the new location if it exists
                            proofsheetfilename = proofsheetfilenamewithfedid;
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("The proof sheet file for that date does not exist. Run the proof sheet and try to send again.");
                            transmitok = false;
                        }
                    }
                }
                if (transmitok)
                {

                    if (!File.Exists(activityfilename))
                    {
                        if (File.Exists(activityfilenamewithfedid))
                        {
                            // Use the new location if it exists
                            activityfilename = activityfilenamewithfedid;
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("The activity report file for that date does not exist. Run the activity and try to send again.");
                            transmitok = false;
                        }
                    }
                }

                // Develop email address for the bank
                ssprocessds.emailbank.Clear();
                commandstring = "SELECT * FROM emailbank WHERE bankfedid  = @bankfedid";
                ClearParameters();
                AddParms("@bankfedid", ssprocessds.bank[0].bankfedid, "SQL");
                FillData(ssprocessds, "emailbank", commandstring, CommandType.Text);
                if (ssprocessds.emailbank.Rows.Count < 1)
                {
                    wsgUtilities.wsgNotice("There are no email addresses for this bank");
                    transmitok = false;
                }


                if (transmitok)
                {
                    if (!wsgUtilities.wsgReply("Do you want to send the Signature Emails?"))
                    {
                        transmitok = false;
                    }
                }

                if (transmitok)
                {

                    System.Net.Mail.MailMessage bankMessage = new System.Net.Mail.MailMessage();
                    bankMessage.To.Clear();

                    for (int i = 0; i <= ssprocessds.emailbank.Rows.Count - 1; i++)
                    {
                        bankMessage.To.Add(ssprocessds.emailbank[i].emailaddr.TrimEnd());
                    }
                    if (ConfigurationManager.AppSettings["TestMode"] == "True")
                    {
                        bankMessage.To.Clear();
                        bankMessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                    }

                    // Send the proof sheet, activity report and total files
                    bankMessage.Attachments.Clear();
                    bankMessage.Attachments.Add(new System.Net.Mail.Attachment(ssproofsheetfilename));
                    bankMessage.Attachments.Add(new System.Net.Mail.Attachment(ssactivityfilename));
                    bankMessage.Attachments.Add(new System.Net.Mail.Attachment(proofsheetfilename));
                    bankMessage.Attachments.Add(new System.Net.Mail.Attachment(activityfilename));
                    bankMessage.Attachments.Add(new System.Net.Mail.Attachment(ssunverifiedfilename));
                    // Send the pdf's
                    appUtilities.SendEmail(bankMessage, emailsubject, "See Attached");
                    wsgUtilities.wsgNotice("Email Transmission complete.");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }

        public void SendSignatureFiles()
        {
            string commandstring = "";
            // Force Signature Bank's idcol
            int selectedbankid = 1;
            // If a bank has been selected, get the date
            if (selectedbankid > 0)
            {
                ssprocessds.bank.Rows.Clear();
                commandstring = "SELECT * FROM bank WHERE idcol = @idcol";
                ClearParameters();
                AddParms("@idcol", selectedbankid, "SQL");
                FillData(ssprocessds, "bank", commandstring, CommandType.Text);

                // Establish the license key
                SBUtils.Unit.SetLicenseKey("76F551EF177055D6A4E0697CACC85371DF67130AE9310684F0B7F9BF202E83C9BF49820A2B63C593E685D05A87A7BC2FEEB8F2F3B6D365457F3154B038F6CFF96EAAA4BDF67FBB0B97BB88856A267D0F87CC9452CC79450FEABABEE205D453A81913F18463EE6FBF0DD43B004176F940D8B4A55C6A4C624E1A3EFB9761D7EA7E1C0C662BDF7F602B88CD6763EFA3E18C228202DA50284F90A229C9A185140BCDF4C9C655887BE7F9FFA71CB34AE6885A7D26A2330EE188E80FD31125613B2E0840D3AE9E330AF88CB48A8805CEEDD2F845A69A6FC406C16EE39E5C70F35B5EB09E5E5E5B71D34D5E83ACE0091F9795027E4CC75A116D401CDABF2B40679BDF0E");

                DateTime PostingDate = new DateTime();
                if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 200, 200))
                {
                    bool transmitok = true;
                    PostingDate = commonAppDataMethods.SelectedDate.Date;
                    ssprocessds.balance.Rows.Clear();
                    commandstring = "SELECT * from balance where bankfedid = '8923847' AND postdate = @postingdate";
                    ClearParameters();
                    AddParms("@postingdate", PostingDate, "SQL");
                    FillData(ssprocessds, "balance", commandstring, CommandType.Text);
                    if (ssprocessds.balance.Rows.Count < 1)
                    {
                        wsgUtilities.wsgNotice("That date has not been closed");
                        transmitok = false;
                    }
                    if (transmitok)
                    {
                        transmitok = MergeSignatureFiles(PostingDate, "8923847");
                    }


                    string pubkeyfilename = ConfigurationManager.AppSettings["NHPBankPath"] + "signykey.pgp";
                    string pgpoutputfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "safesound.pgp";
                    string recipientemailaddress = "security@signatureny.com";
                    string pgpsourcefilename = ConfigurationManager.AppSettings["FTPFilepath"] + "8923847" + "\\Merged\\" + "SAFESOUND" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                    string totalsfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "8923847" + "\\Merged\\" + "SAFEANDSOUNDTOTALS" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                    string proofsheetfilename = ConfigurationManager.AppSettings["PDFFilespath"] + "8923847\\" + "ps" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                    string activityfilename = ConfigurationManager.AppSettings["PDFFilepath"] + "8923847\\" + "ps" + "ac" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                    string ssproofsheetfilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "smartsafeproofsheet" + String.Format("{0:MM-dd-yy}", PostingDate) + ".pdf";
                    string ssactivityfilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "smartsafeactivity" + String.Format("{0:MM-dd-yy}", PostingDate) + ".pdf";
                    string pgpsourcefilenamewithfedid = ConfigurationManager.AppSettings["FTPFilespath"] + ssprocessds.bank[0].bankfedid.TrimEnd() + "\\" + "ss" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                    string totalsfilenamewithfedid = ConfigurationManager.AppSettings["FTPFilepath"] + ssprocessds.bank[0].bankfedid.TrimEnd() + "\\Merged\\" + "SAFEANDSOUNDTOTALS" + String.Format("{0:MMddyy}", PostingDate) + ".txt";
                    string proofsheetfilenamewithfedid = ConfigurationManager.AppSettings["PDFFilespath"] + ssprocessds.bank[0].bankfedid.TrimEnd() + "\\" + "ps" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                    string activityfilenamewithfedid = ConfigurationManager.AppSettings["PDFFilespath"] + ssprocessds.bank[0].bankfedid.TrimEnd() + "\\" + "ac" + String.Format("{0:MMddyy}", PostingDate) + ".pdf";
                    string ssunverifiedfilename = ConfigurationManager.AppSettings["PDFFilesPath"] + "smartsafeunverified" + String.Format("{0:MM-dd-yy}", PostingDate) + ".pdf";

                    string emailsubject = "Safe and Sound e-mail notification " + String.Format("{0:MM/dd/yy}", PostingDate);
                    if (transmitok)
                    {


                        if (!File.Exists(pgpsourcefilename))
                        {
                            wsgUtilities.wsgNotice("The transmission file for that date does not exist. Run the proof sheet and try to send again.");
                            transmitok = false;

                        }
                    }
                    if (transmitok)
                    {


                        if (!File.Exists(ssproofsheetfilename))
                        {
                            wsgUtilities.wsgNotice("The Smart Safe Proof Sheet for that date does not exist. Run the proof sheet and try to send again.");
                            transmitok = false;

                        }
                    }
                    if (transmitok)
                    {


                        if (!File.Exists(ssactivityfilename))
                        {
                            wsgUtilities.wsgNotice("The Smart Safe Activity Analysis for that date does not exist. Run the Activity Analysyis and try to send again.");
                            transmitok = false;

                        }
                    }
                    if (transmitok)
                    {


                        if (!File.Exists(ssunverifiedfilename))
                        {
                            wsgUtilities.wsgNotice("The Smart Safe Unverified Analysis for that date does not exist. Run the report and try to send again.");
                            transmitok = false;

                        }
                    }
                    if (transmitok)
                    {
                        if (!File.Exists(proofsheetfilename))
                        {
                            if (File.Exists(proofsheetfilenamewithfedid))
                            {
                                // Use the new location if it exists
                                proofsheetfilename = proofsheetfilenamewithfedid;
                            }
                            else
                            {
                                wsgUtilities.wsgNotice("The proof sheet file for that date does not exist. Run the proof sheet and try to send again.");
                                transmitok = false;
                            }
                        }
                    }
                    if (transmitok)
                    {

                        if (!File.Exists(activityfilename))
                        {
                            if (File.Exists(activityfilenamewithfedid))
                            {
                                // Use the new location if it exists
                                activityfilename = activityfilenamewithfedid;
                            }
                            else
                            {
                                wsgUtilities.wsgNotice("The activity report file for that date does not exist. Run the activity and try to send again.");
                                transmitok = false;
                            }
                        }
                    }

                    if (transmitok)
                    {

                        if (!File.Exists(totalsfilename))
                        {
                            if (File.Exists(totalsfilenamewithfedid))
                            {
                                // Use the new location if it exists
                                totalsfilename = totalsfilenamewithfedid;
                            }
                            else
                            {
                                wsgUtilities.wsgNotice("The totals file for that date does not exist. Run the activity and try to send again.");
                                transmitok = false;
                            }
                        }
                    }

                    if (transmitok)
                    {
                        if (!wsgUtilities.wsgReply("Do you want to send the Signature Files?"))
                        {
                            transmitok = false;
                        }
                    }

                    if (transmitok)
                    {
                        try
                        {
                            // Send the file
                            if (ConfigurationManager.AppSettings["TestMode"] != "True")
                            {
                                appUtilities.SendSftp(ssprocessds.bank[0].outftpaddress.TrimEnd(), ssprocessds.bank[0].outusername.TrimEnd(), ssprocessds.bank[0].outpassword.TrimEnd(), pgpsourcefilename);
                            }
                            appUtilities.logEvent("Signature file Sent", "Sig File", "Success", false);
                        }
                        catch (Exception ex)
                        {
                            appUtilities.logEvent("Signature transmission error", "Sig File", ex.Message, true);
                        }

                        System.Net.Mail.MailMessage bankMessage = new System.Net.Mail.MailMessage();
                        bankMessage.To.Clear();
                        // Read the total text file for inclusion in the email to the bank
                        StreamReader streamReader = new StreamReader(totalsfilename);
                        string totalstext = streamReader.ReadToEnd();
                        streamReader.Close();
                        // Develop email address for the bank
                        ssprocessds.emailbank.Clear();
                        commandstring = "SELECT * FROM emailbank WHERE bankfedid  = @bankfedid";
                        ClearParameters();
                        AddParms("@bankfedid", ssprocessds.bank[0].bankfedid, "SQL");
                        FillData(ssprocessds, "emailbank", commandstring, CommandType.Text);
                        for (int i = 0; i <= ssprocessds.emailbank.Rows.Count - 1; i++)
                        {
                            bankMessage.To.Add(ssprocessds.emailbank[i].emailaddr.TrimEnd());
                        }
                        if (ConfigurationManager.AppSettings["TestMode"] == "True")
                        {
                            bankMessage.To.Clear();
                            bankMessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                        }
                        // Send the totals Control Message
                        appUtilities.SendEmail(bankMessage, emailsubject, totalstext);

                        // Send the proof sheet, activity report and total files
                        bankMessage.Attachments.Clear();
                        bankMessage.Attachments.Add(new System.Net.Mail.Attachment(ssproofsheetfilename));
                        bankMessage.Attachments.Add(new System.Net.Mail.Attachment(ssactivityfilename));
                        bankMessage.Attachments.Add(new System.Net.Mail.Attachment(proofsheetfilename));
                        bankMessage.Attachments.Add(new System.Net.Mail.Attachment(activityfilename));
                        bankMessage.Attachments.Add(new System.Net.Mail.Attachment(totalsfilename));
                        bankMessage.Attachments.Add(new System.Net.Mail.Attachment(ssunverifiedfilename));
                        // Send the pdf's
                        appUtilities.SendEmail(bankMessage, emailsubject, "See Attached");
                        wsgUtilities.wsgNotice("Transmission complete.");
                    }
                }
            }
        }
    }
}
