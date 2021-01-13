using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using BusinessTransactions;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.IO;
using System.Xml.Linq;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BusinessTransactions
{
    public class MiscellaneousBusinessMethods : WSGDataAccess
    {

        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Armed Courier");
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        BusinessReports.BusinessReportsMethods brMethods = new BusinessReports.BusinessReportsMethods();
        public Form menuform { get; set; }
        public DateTime LastClosedate { get; set; }
        AppUtilities appUtilities = new AppUtilities();
        string commandtext { get; set; }
        public string bankfedid { get; set; }
        ssprocess ssprocessds = new ssprocess();
        public MiscellaneousBusinessMethods()
            : base("SQL", "SQLConnString")
        {

        }

        public void SendFISERVData()
        {
            string vaultname = "Dallas Vault";
            string entityreferenceid = "3000001025";
            string xmlfilename = ConfigurationManager.AppSettings["FTPFilepath"] +
            " DIF_Safe and Sound_CCCC_" + DateTime.Now.ToString("yyyMMdd_hhmmss") + ".xml";

            string postingdate = DateTime.Now.AddDays(-1).ToString("yyyy_MM_ddThh:mm:ss") + "_09:00";
            string transmissiondate = DateTime.Now.ToString("yyyy_MM_dd");
            string depositreferenceid = "3886622341";
            string bagnumber = "87633349";
            XDocument doc = new XDocument();
            XElement corpointdepositimport = new XElement("CorpointDepositImport", new XAttribute("fileCreatedDate", transmissiondate), new XAttribute("company", "XXXX"));
            XElement deposits = new XElement("Deposits");

            // Loop through deposits here

            XElement deposit = new XElement("Deposit",
                     new XAttribute("customerReferenceId", ""),
                     new XAttribute("vaultReferenceId", vaultname),
                     new XAttribute("entityReferenceId", entityreferenceid),
                     new XAttribute("reversal", "N"),
                     new XAttribute("smartSafeReferenceId", ""),
                     new XAttribute("depositReferenceId", depositreferenceid),
                     new XAttribute("postingdate", postingdate),
                     new XAttribute("settledate", postingdate),
                     new XAttribute("bagnumber", bagnumber),
                     new XAttribute("serialnumber", bagnumber));

            XElement detail = new XElement("Detail",
                     new XAttribute("Currency", "USD"),
                     new XAttribute("declaredAmount", "400.50"),
                     new XAttribute("declaredCashAmount", "400.50"),
                     new XAttribute("declaredCheckAmount", "0.00"),
                     new XAttribute("verifiedAmount", "350.50"),
                     new XAttribute("verifiedCashAmount", "350.00"),
                     new XAttribute("verifiedCheckAmount", "0.00"),
                     new XAttribute("verifiedNotesAmount", "350.00"),
                     new XAttribute("verifiedCoinAmount", "0.50"),
                     new XAttribute("shortageAmount", "50.00"),
                     new XAttribute("overageAmount", "0.00"),
                     new XAttribute("standardCoinBagCount", "0"),
                     new XAttribute("nonStandardCoinBagCount", "0"),
                     new XAttribute("wrappedCoinBagCount", "0"),
                     new XAttribute("stcCoinBagCount", "0"));

            XElement adjustmentreasons = new XElement("AdjustmentReasons");
            XElement reason = new XElement("Reason", "Short");
            adjustmentreasons.Add(reason);
            detail.Add(adjustmentreasons);

            XElement item100 = new XElement("item");

            item100.Add(new XAttribute("denom", "USDN10000"));
            item100.Add(new XAttribute("amount", "300.00"));
            item100.Add(new XAttribute("nonStandardCount", "3"));
            item100.Add(new XAttribute("standardcount", "0"));
            detail.Add(item100);

            XElement item10 = new XElement("item");

            item10.Add(new XAttribute("denom", "USDN01000"));
            item10.Add(new XAttribute("amount", "50.00"));
            item10.Add(new XAttribute("nonStandardCount", "5"),
            new XAttribute("standardcount", "0"));

            detail.Add(item10);

            XElement itemcoin = new XElement("item");

            itemcoin.Add(new XAttribute("denom", "USDNLTCOIN"));
            itemcoin.Add(new XAttribute("amount", ".50"));

            detail.Add(itemcoin);


            deposit.Add(detail);
            deposits.Add(deposit);
            corpointdepositimport.Add(deposits);
            doc.Add(corpointdepositimport);


            if (File.Exists(xmlfilename))
            {
                File.Delete(xmlfilename);
            }
            doc.Save(xmlfilename);

            wsgUtilities.wsgNotice("Operation complete");
        }
        public void SendPickupData()
        {
            wsgUtilities.wsgNotice("This function is no longer in use");
            // WLF 03/5/19 This function has been moved to prowler
            /*
            
            if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 200, 200))
            {
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                string textfilename = ConfigurationManager.AppSettings["FTPFilepath"] +
                 "SafeSound_" + commonAppDataMethods.SelectedDate.Date.ToString("MMddyyyy") + ".csv";
                CultureInfo provider = CultureInfo.InvariantCulture;
                string neutraldatestring = DateTime.ParseExact("01/01/2001", "d", provider).ToString("yyyy-MM-dd HHmm ");
                bool storeactive = false;
                // Establish output variables
                string action = "Delivery";
                string company = "";
                string pickuptime = "";
                string deliverytime = "";
                string location = "";
                string customerbagnumber = "Customer Bag";
                string carrierbarcode = "Carrier Bar";
                decimal saidtocontain = 150.01M;
                DateTime arrivaldate = commonAppDataMethods.SelectedDate.Date;
                DateTime pickupdate = commonAppDataMethods.SelectedDate.Date;
                DateTime departuredate = commonAppDataMethods.SelectedDate.Date;
                int premisetime = 15;
                string noservicecode = "NS";
                string noservicereason = "No service requested";
                string scanlocation = "T";
                // Start the text file
                TextWriter textout = new StreamWriter(textfilename);
                string lineout = "";
                ssprocessds.store.Rows.Clear();
                ClearParameters();
                AddParms("@compcode1", "7822", "SQL");
                AddParms("@compcode2", "7850", "SQL");
                commandtext = "SELECT * FROM store WHERE active = 1 AND (LEFT(storecode,4) = @compcode1 OR LEFT(storecode,4) = @compcode2)  ORDER BY storecode";
                FillData(ssprocessds, "store", commandtext, CommandType.Text);
                for (int r = 0; r <= ssprocessds.store.Rows.Count - 1; r++)
                {
                    switch (ssprocessds.store[r].storecode.Substring(0, 4))
                    {
                        case "7822":
                            {
                                company = "Starbucks";
                                break;
                            }
                        case "7850":
                            {
                                company = "TMobile";
                                break;
                            }


                    }



                    storeactive = false;
                    pickuptime = "";
                    deliverytime = "";
                    // Establish default values for locations with no service that day
                    action = "Delivery";
                    customerbagnumber = "Customer Bag";
                    carrierbarcode = "Carrier Bar";
                    saidtocontain = 150.01M;
                    arrivaldate = DateTime.ParseExact("01/01/2001", "d", provider);
                    pickupdate = DateTime.ParseExact("01/01/2001", "d", provider);
                    departuredate = DateTime.ParseExact("01/01/2001", "d", provider);
                    premisetime = 6;
                    noservicecode = "NS";
                    noservicereason = "No service requested";
                    scanlocation = "T";

                    // Check for pickups

                    ssprocessds.hhpickup.Rows.Clear();
                    ClearParameters();
                    AddParms("@storecode", ssprocessds.store[r].storecode.TrimEnd(), "SQL");
                    AddParms("@pickupdate", commonAppDataMethods.SelectedDate, "SQL");
                    commandtext = "SELECT * FROM  hhpickup WHERE LEFT(storecode,11) = @storecode AND pickupdate >= @pickupdate AND pickupdate < DATEADD(day,1, @pickupdate)   ORDER BY storecode";
                    FillData(ssprocessds, "hhpickup", commandtext, CommandType.Text);

                    if (ssprocessds.hhpickup.Rows.Count > 0)
                    {

                        storeactive = true;
                        for (int i = 0; i <= ssprocessds.hhpickup.Rows.Count - 1; i++)
                        {


                            action = "Pickup";
                            customerbagnumber = ssprocessds.hhpickup[i].sealnumber.TrimEnd();
                            carrierbarcode = "";
                            saidtocontain = ssprocessds.hhpickup[i].amount;
                            arrivaldate = ssprocessds.hhpickup[i].pickupdate;
                            pickupdate = ssprocessds.hhpickup[i].pickupdate;
                            departuredate = ssprocessds.hhpickup[i].pickupdate;
                            pickuptime = ssprocessds.hhpickup[i].pickuptime;
                            deliverytime = pickuptime;
                            premisetime = 0;
                            noservicecode = "";
                            noservicereason = "";
                            // Prepare output
                            location = ssprocessds.store[r].storecode.TrimEnd().Substring(ssprocessds.store[r].storecode.TrimEnd().Length - 5);
                            lineout = "";
                            lineout += action + ",";
                            lineout += company + ",";
                            lineout += location + ","; ;
                            lineout += customerbagnumber + ",";
                            lineout += carrierbarcode + ",";
                            lineout += saidtocontain.ToString("F") + ",";
                            lineout += arrivaldate.ToString("yyyy-MM-dd ") + pickuptime.Substring(0, 5) + ",";
                            lineout += pickupdate.ToString("yyyy-MM-dd ") + pickuptime.Substring(0, 5) + ",";
                            lineout += departuredate.ToString("yyyy-MM-dd ") + pickuptime.Substring(0, 5) + ",";
                            lineout += premisetime.ToString("N") + ",";
                            lineout += noservicecode + ",";
                            lineout += noservicereason + ",";
                            lineout += scanlocation + ",";
                            lineout += " ,";
                            lineout += ssprocessds.store[r].storecode.TrimEnd();
                            if (ssprocessds.store[r].storecode.Substring(0, 4) != "7822")
                            {
                                textout.WriteLine(lineout);
                            }
                        }
                    }
                    // Check for currency drops

                    ssprocessds.currencydrop.Rows.Clear();
                    ClearParameters();
                    AddParms("@storecode", ssprocessds.store[r].storecode.TrimEnd(), "SQL");
                    AddParms("@deliverydate", commonAppDataMethods.SelectedDate.Date, "SQL");
                    commandtext = "SELECT * FROM currencydrop WHERE LEFT(storecode,11) = @storecode AND deliverydate = @deliverydate ORDER BY storecode";
                    FillData(ssprocessds, "currencydrop", commandtext, CommandType.Text);
                    if (ssprocessds.currencydrop.Rows.Count > 0)
                    {

                        if (deliverytime.TrimEnd() == "")
                        {
                            deliverytime = "18:30";
                        }
                        storeactive = true;
                        for (int i = 0; i <= ssprocessds.currencydrop.Rows.Count - 1; i++)
                        {

                            action = "Delivery";
                            customerbagnumber = "";
                            carrierbarcode = "";
                            saidtocontain = ssprocessds.currencydrop[i].amount;
                            arrivaldate = ssprocessds.currencydrop[i].deliverydate;
                            pickupdate = ssprocessds.currencydrop[i].deliverydate;
                            departuredate = ssprocessds.currencydrop[i].deliverydate;
                            premisetime = 0;
                            noservicecode = "";
                            noservicereason = "";
                            // Prepare output
                            location = ssprocessds.store[r].storecode.TrimEnd().Substring(ssprocessds.store[r].storecode.TrimEnd().Length - 5);
                            lineout = "";
                            lineout += action + ",";
                            lineout += company + ",";
                            lineout += location + ","; ;
                            lineout += customerbagnumber + ",";
                            lineout += carrierbarcode + ",";
                            lineout += saidtocontain.ToString("F") + ",";
                            lineout += arrivaldate.ToString("yyyy-MM-dd ") + deliverytime + ",";
                            lineout += pickupdate.ToString("yyyy-MM-dd ") + deliverytime + ",";
                            lineout += departuredate.ToString("yyyy-MM-dd ") + deliverytime + ",";
                            lineout += premisetime.ToString("N") + ",";
                            lineout += noservicecode + ",";
                            lineout += noservicereason + ",";
                            lineout += scanlocation + ",";
                            lineout += scanlocation + ",";
                            lineout += ssprocessds.store[r].storecode.TrimEnd();
                            if (ssprocessds.store[r].storecode.Substring(0, 4) != "7822")
                            {
                                textout.WriteLine(lineout);
                            }
                        }
                    }



                    if (!storeactive)
                    {

                        // Prepare output for inactive store
                        action = "Pickup";
                        noservicecode = "NS";
                        noservicereason = "No service requested";
                        location = ssprocessds.store[r].storecode.TrimEnd().Substring(ssprocessds.store[r].storecode.TrimEnd().Length - 5);
                        lineout = "";
                        lineout += action + ",";
                        lineout += company + ",";
                        lineout += location + ","; ;
                        // Customer bag number
                        lineout += ",";
                        //Carrier barcode
                        lineout += ",";
                        //Said to contain
                        lineout += 0.ToString("F") + ",";
                        // Arrival date
                        lineout += commonAppDataMethods.SelectedDate.Date.ToString("yyyy-MM-dd ") + ",";
                        // Pickup date
                        lineout += commonAppDataMethods.SelectedDate.Date.ToString("yyyy-MM-dd ") + ",";
                        //Departure date
                        lineout += commonAppDataMethods.SelectedDate.Date.ToString("yyyy-MM-dd ") + ",";
                        // Premise time
                        lineout += 0.ToString("N") + ",";
                        lineout += noservicecode + ",";
                        lineout += noservicereason + ",";
                        lineout += scanlocation + ",";
                        lineout += scanlocation + ",";
                        lineout += ssprocessds.store[r].storecode.TrimEnd();
                        textout.WriteLine(lineout);
                    }
                }
                textout.Close();
                frmLoading.Close();
                try
                {
                    if (ConfigurationManager.AppSettings["TestMode"] != "True")
                    {
                        appUtilities.sendFTP(textfilename, "ifsuseracct", @"ftp://ftp.ssarmored.com/", "PuFu5Ar5");
                        wsgUtilities.wsgNotice("Operation Complete");
                    }
                    else
                    {
                        wsgUtilities.wsgNotice("Test Complete");
                    }
                    appUtilities.logEvent("Send DTS File " + commonAppDataMethods.SelectedDate.Date.ToString("MMddyyyy"), "DTS", "Complete", true);
                }
                catch (Exception ex)
                {
                    wsgUtilities.wsgNotice("FTP Error " + ex.Message);
                }

            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
       
          */
        }

        public void ReopenClosedBalance()
        {
            bool cont = true;
            bankfedid = "YY888888888";
            int balanceidcol = 0;
            ssprocessds.balance.Rows.Clear();
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            commandtext = "SELECT TOP 1 * FROM balance WHERE bankfedid  = @bankfedid ORDER BY postdate DESC";
            FillData(ssprocessds, "balance", commandtext, CommandType.Text);
            if (ssprocessds.balance.Rows.Count > 0)
            {
                LastClosedate = ssprocessds.balance[0].postdate;

                // Check for transactions  after the last close
                ssprocessds.invtrans.Rows.Clear();
                commandtext = "SELECT * FROM invtrans WHERE bankfedid = 'YY888888888' and trandate > @lastclosedate";
                ClearParameters();
                AddParms("@lastclosedate", LastClosedate, "SQL");
                FillData(ssprocessds, "invtrans", commandtext, CommandType.Text);
                if (ssprocessds.invtrans.Rows.Count >= 1)
                {
                    if (!wsgUtilities.wsgReply("There are postings aftr the close date. Do you want to reopen anyway?"))
                    {
                        cont = false;
                    }
                }
                if (cont)
                {

                    if (wsgUtilities.wsgReply("Delete closing balance for Cash Connect for " + String.Format("{0:MM/dd/yyyy}", LastClosedate.Date) + "?"))
                    {
                        commandtext = "DELETE FROM balance WHERE idcol  = @idcol";
                        balanceidcol = ssprocessds.balance[0].idcol;
                        ClearParameters();
                        AddParms("@idcol", balanceidcol, "SQL");
                        ExecuteCommand(commandtext, CommandType.Text);
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
            else
            {
                wsgUtilities.wsgNotice("There is no closing balance for this bank account");
            }
        }

    }

}