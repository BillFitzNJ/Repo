using System;
using System.IO;
using System.Data;
using System.Web;
using System.Threading;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using BusinessReports;
using CommonAppClasses;
using WSGUtilitieslib;
using System.Text;
using WinSCP;

namespace AnalysisClassess
{
    public class AnalysisMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Web Site Synchronization");
        AppUtilities appUtilities = new AppUtilities();
        sysdata sydatads = new sysdata();
        AMSECDataSet amsecds = new AMSECDataSet();
        mysqldata mysqldatads = new mysqldata();
        ssprocess ssprocessds = new ssprocess();
        ssprocess sssearchprocessds = new ssprocess();
        AnalysisClasses.MySQLMethods mySQLMethods = new AnalysisClasses.MySQLMethods();
        ssprocess tempprocessds = new ssprocess();
        DataRow[] foundRows;
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        mysqldata mysqlsearchds = new mysqldata();
        List<string> EmailAttachments = new List<string>();
        DateTime processdate = DateTime.Now.Date;
        string commandstring = "";
        public AnalysisMethods()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.store.idcolColumn);
            SetIdcol(mySQLMethods.mysqldatads.hhpickup.idcolColumn);
            SetIdcol(mySQLMethods.mysqltempdatads.hhpickup.idcolColumn);
            AppUserClass.AppUserId = "WSG";

        }

        public void ExecuteReview(DateTime lastreviewdatetime)
        {

            try
            {
                SendValetManagerData();
            }
            catch (Exception x)
            {
                appUtilities.logEvent("Valet Manager Sending Error- " + string.Format("{0:MM/dd/yy}", processdate), "Prowler", x.StackTrace, true);
            }





            // IN PROCRESS .. set off for now... JNS 2-24-2020
            // ImmportSmartSafeXML();
            try
            {
                CheckClosing();
            }
            catch (Exception x)
            {
                appUtilities.logEvent("Closing Check Error- " + string.Format("{0:MM/dd/yy}", processdate), "Prowler", x.StackTrace, true);
            }

            try
            {
                SendPickupData();
            }

            catch (Exception x)
            {
                appUtilities.logEvent("DTS Send Error- " + string.Format("{0:MM/dd/yy}", processdate), "Prowler", x.StackTrace, true);
            }

            try
            {
                DTSPickupCheck();
            }
            catch (Exception x)
            {
                appUtilities.logEvent("DTS Pickup Check Error- " + string.Format("{0:MM/dd/yy}", processdate), "Prowler", x.StackTrace, true);
            }
            try
            {
                ImportSafedepositSmartSafeXML();
            }
            catch (Exception x)
            {
                appUtilities.logEvent("Safedeposit import Error- " + string.Format("{0:MM/dd/yy}", processdate), "Prowler", x.StackTrace, true);
            }

            try
            {
                SystemHousekeeping();
            }
            catch (Exception x)
            {
                appUtilities.logEvent("Housekeeping Error- " + string.Format("{0:MM/dd/yy}", processdate), "Prowler", x.StackTrace, true);
            }

          


        }


        public void SendValetManagerData()
        {
            bool cont = true;
            string filedate = string.Format("{0:MMddyy}", DateTime.Now.Date);
            string sourcefilename = ConfigurationManager.AppSettings["XLSFilesPath"] + "GP" + filedate + ".xlsx";
            if (File.Exists(sourcefilename))
            {
                if (!commonAppDataMethods.CheckEventDescription("Send Valet Manager Data" + string.Format("{0:MM/dd/yy}", DateTime.Now.Date)))
                {
                    appUtilities.sendFTP(sourcefilename, ConfigurationManager.AppSettings["ValetmanagerUserid"], ConfigurationManager.AppSettings["ValetmanagerURL"], ConfigurationManager.AppSettings["ValetmanagerPassword"]);
                }
                appUtilities.logEvent("Send Valet Manager Data" + string.Format("{0:MM/dd/yy}", DateTime.Now.Date), "Complete", "SHK", false);
            }
        }
        public void ImportSafedepositSmartSafeXML()
        {
            SetIdcol(ssprocessds.safedeposit.idcolColumn);
            bool cont = true;

            if (!commonAppDataMethods.CheckEventDescription("AMSEC Safedeposit Import" + string.Format("{0:MM/dd/yy}", DateTime.Now.Date)))
            {

                readsafedeposithistory("1142");
                readsafedeposithistory("1143");
                readsafedeposithistory("1141");
                readsafedeposithistory("1147");
                readsafedeposithistory("1144");
                readsafedeposithistory("1146");
            }
            appUtilities.logEvent("AMSEC Safedeposit Import" + string.Format("{0:MM/dd/yy}", DateTime.Now.Date), "Complete", "SHK", false);



        }

        public void readsafedeposithistory(string compcode)
        {
            string businessDateString = "";
            sssearchprocessds.safemast.Rows.Clear();
            ClearParameters();
            commandstring = "SELECT * FROM safemast where LEFT(storecode,4) = @compcode";
            AddParms("@compcode", compcode, "SQL");
            FillData(sssearchprocessds, "safemast", commandstring, CommandType.Text);

            if (sssearchprocessds.safemast.Rows.Count > 0)
            {
                //bool cont = true;
                string serialnumber = sssearchprocessds.safemast[0].serialnumber.TrimStart().TrimEnd();

                string IPAddress = ConfigurationManager.AppSettings["IMPORTFTPADDRESS"];
                string LoginName = ConfigurationManager.AppSettings["IMPORTFTPUSERID"];
                string Password = ConfigurationManager.AppSettings["IMPORTFTPPW"];
                IPAddress = "ftp://" + IPAddress + "/";

                DateTime businessDate = DateTime.Now.Date.AddDays(-1);
                businessDateString = businessDate.ToString("yyyyMMdd");
                // Get yesterday's files
                string WildCard = "*" + serialnumber + "-" + businessDateString + "*";
                string returnstring = "";
                //Connect to the FTP
                FtpWebRequest request = WebRequest.Create(IPAddress + WildCard) as FtpWebRequest;
                //Specify we're Listing a directory
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(LoginName, Password);

                StringWriter sw = new StringWriter();
                //Get a reponse
                WebResponse response = request.GetResponse();
                Stream responseStream = response.GetResponseStream();

                //Convert the response to a string
                int ch;
                while ((ch = responseStream.ReadByte()) != -1)
                    returnstring = returnstring + Convert.ToChar(ch);

                //clean up
                responseStream.Close();
                response.Close();

                //split the string by new line
                string[] sep = { "\r\n" };
                string[] Files = returnstring.Split(sep, StringSplitOptions.RemoveEmptyEntries);


                for (int f = 0; f <= Files.Length - 1; f++)
                {
                    ReadSafedepositXMLFile(IPAddress, LoginName, Password, Files[f]);
                }

            }
        }



        public Boolean ReadSafedepositXMLFile(string IPAddress, string LoginName, string Password, string filename)
        {
            Boolean result = true;
            try
            {
                IPAddress = IPAddress + "/" + filename;
                System.Net.FtpWebRequest reqFileFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(IPAddress));
                reqFileFTP.Credentials = new System.Net.NetworkCredential(LoginName, Password);
                reqFileFTP.Timeout = 600000;
                reqFileFTP.UseBinary = false;
                reqFileFTP.UsePassive = true;
                reqFileFTP.KeepAlive = false;

                DateTime processdate = DateTime.Now;

                reqFileFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                System.Net.FtpWebResponse fileresponse = (System.Net.FtpWebResponse)reqFileFTP.GetResponse();
                System.IO.StreamReader filestreamReader = new System.IO.StreamReader(fileresponse.GetResponseStream());
                decimal declaredamount = 0;
                decimal checksamount = 0;
                string t = new StreamReader(fileresponse.GetResponseStream(), Encoding.Default).ReadToEnd();

                MemoryStream xmlstream = new MemoryStream();
                byte[] byteArray = Encoding.UTF8.GetBytes(t);
                xmlstream.Write(byteArray, 0, byteArray.Length);
                //set the position at the beginning.
                xmlstream.Position = 0;
                string transactiontype = " ";
                amsecds.ManualDrop.Rows.Clear();
                amsecds.ManualDrops.Rows.Clear();
                amsecds.Cassette.Rows.Clear();
                amsecds.CurrentEod.Rows.Clear();
                amsecds.SafeAndSound.Rows.Clear();
                amsecds.ReadXml(xmlstream);
                if (amsecds.SafeAndSound.Rows.Count > 0)
                {

                    // do we need to look to make sure SN is a SMARTSAFE & AMSEC safe from safemast?
                    ssprocessds.safemast.Rows.Clear();
                    commandstring = "SELECT * FROM safemast WHERE serialnumber = @serialnumber ";
                    ClearParameters();
                    AddParms("serialnumber", amsecds.SafeAndSound[0].SerialNumber);
                    ssprocessds.safemast.Rows.Clear();
                    FillData(ssprocessds, "safemast", commandstring, CommandType.Text);
                    if (ssprocessds.safemast.Rows.Count > 0)
                    {
                        if (ssprocessds.safemast[0].bankfedid.TrimEnd() == "SMARTSAFE"
                            && ssprocessds.safemast[0].manufacturer.TrimEnd() == "AMSEC")
                        {

                            // End of day or carrier pickup

                            if (amsecds.SafeAndSound[0].filetype == "END_OF_DAY")
                            {
                                transactiontype = "EOD";
                                processdate = Convert.ToDateTime(amsecds.SafeAndSound[0].EODDateTime).Date;
                            }
                            else
                            {

                                transactiontype = "CPU";
                                processdate = Convert.ToDateTime(amsecds.SafeAndSound[0].StartDateTime).Date;

                            }
                            // Check to see if this transaction has been previously imported
                            sssearchprocessds.safedeposit.Rows.Clear();
                            ClearParameters();
                            commandstring = "SELECT * FROM safedeposit where serialnumber = @serialnumber AND trantype = @trantype AND depositdate = @depositdate";
                            AddParms("@serialnumber", amsecds.SafeAndSound[0].SerialNumber, "SQL");
                            AddParms("@trantype", transactiontype, "SQL");
                            AddParms("@depositdate", processdate, "SQL");
                            FillData(sssearchprocessds, "safedeposit", commandstring, CommandType.Text);
                            if (sssearchprocessds.safedeposit.Rows.Count < 1)
                            {
                                // Develop amounts
                                declaredamount = Convert.ToDecimal(amsecds.CurrentEod[0].Deposit);
                                declaredamount += Convert.ToDecimal(amsecds.ManualDrops[0].Cash);
                                declaredamount += Convert.ToDecimal(amsecds.ManualDrops[0].Coins);
                                checksamount += Convert.ToDecimal(amsecds.ManualDrops[0].Checks);
                                ssprocessds.safedeposit.Rows.Add();
                                EstablishBlankDataTableRow(ssprocessds.safedeposit);
                                ssprocessds.safedeposit[0].serialnumber = amsecds.SafeAndSound[0].SerialNumber;
                                ssprocessds.safedeposit[0].depositdate = processdate;
                                ssprocessds.safedeposit[0].reconciliationnumber = Convert.ToInt16(amsecds.SafeAndSound[0].ReconcilationNumber);
                                ssprocessds.safedeposit[0].checks = checksamount;
                                ssprocessds.safedeposit[0].depositamount = declaredamount;
                                ssprocessds.safedeposit[0].trantype = transactiontype;
                                GenerateAppTableRowSave(ssprocessds.safedeposit[0]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                appUtilities.logEvent("AMSEC Deposit Import Error " + string.Format("{0:MM/dd/yy}", processdate), "Error :" + ex.Message + " Stack : " + ex.StackTrace, "SHK", true);
                result = false;
            }
            return result;

        }


        public void DTSPickupCheck()
        {
            try
            {

                DateTime dtsprocessdate = processdate.AddDays(-1).Date;

                string mailsubject = "DTS Pickup Check " + string.Format("{0:MM/dd/yy}", dtsprocessdate);

                if (!commonAppDataMethods.CheckEventDescription(mailsubject))
                {
                    string storecodes = "";
                    mySQLMethods.FillMissingDTSPickups(dtsprocessdate);
                    if (mySQLMethods.mysqldatads.hhpickup.Rows.Count > 0)
                    {
                        storecodes = "Missing locatiions: ";
                        for (int i = 0; i <= mySQLMethods.mysqldatads.hhpickup.Rows.Count - 1; i++)
                        {
                            if (i == 0)
                            {
                                storecodes += mySQLMethods.mysqldatads.hhpickup[i].storecode;
                            }
                            else
                            {

                                if (mySQLMethods.mysqldatads.hhpickup[i].storecode != mySQLMethods.mysqldatads.hhpickup[i - 1].storecode)
                                {
                                    storecodes += ", " + mySQLMethods.mysqldatads.hhpickup[i].storecode;
                                }
                            }
                        }
                    }
                    else
                    {
                        storecodes = "No missing locatiions";
                    }
                    MailMessage dtsMessage = new MailMessage();
                    // Add recipients
                    dtsMessage.To.Clear();
                    dtsMessage.Attachments.Clear();
                    if (ConfigurationManager.AppSettings["TestMode"] == "True")
                    {
                        dtsMessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);

                    }
                    else
                    {
                        dtsMessage.To.Add(ConfigurationManager.AppSettings["DTSemail"]);

                    }

                    // Send the email
                    appUtilities.SendEmail(dtsMessage, mailsubject, storecodes);
                    appUtilities.logEvent(mailsubject, "DTSchk", "OK", false);
                }
            }
            catch
            {
                throw;
            }
        }

        public void SendPickupData()
        {
            DateTime processdate = DateTime.Now.Date.AddDays(-1);

            if (!commonAppDataMethods.CheckEventDescription("Send DTS File " + processdate.ToString("MMddyyyy")))
            {

                string textfilename = ConfigurationManager.AppSettings["FTPFilepath"] +
                 "SafeSound_" + processdate.ToString("MMddyyyy") + ".csv";
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
                DateTime arrivaldate = processdate;
                DateTime pickupdate = processdate;
                DateTime departuredate = processdate;
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
                string commandtext = "SELECT * FROM store WHERE active = 1 AND (LEFT(storecode,4) = @compcode1) OR (LEFT(storecode,4) = @compcode2)   ORDER BY storecode";
                FillData(ssprocessds, "store", commandtext, CommandType.Text);

                int recordsProcessed = 0;       // JNS for T-Mobile DTS

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
                        case "7851":
                            {
                                company = "Sprint";
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
                    int mysqldatarow = 0;

                    // Fill Mysql pickups
                    mySQLMethods.FillDTSMySqlPickups(ssprocessds.store[r].storecode.TrimEnd(), processdate);
                    // Fill SQL Server pickups
                    FillDTSSSQLPickups(ssprocessds.store[r].storecode.TrimEnd(), processdate);
                    if (ssprocessds.hhpickup.Rows.Count > 0)
                    {
                        for (int i = 0; i <= ssprocessds.hhpickup.Rows.Count - 1; i++)
                        {
                            mySQLMethods.mysqltempdatads.hhpickup.Rows.Clear();
                            mySQLMethods.mysqltempdatads.hhpickup.Rows.Add();
                            mySQLMethods.mysqltempdatads.hhpickup[0].storecode = ssprocessds.hhpickup[i].storecode;
                            mySQLMethods.mysqltempdatads.hhpickup[0].pickupdate = ssprocessds.hhpickup[i].pickupdate;
                            mySQLMethods.mysqltempdatads.hhpickup[0].sealnumber = ssprocessds.hhpickup[i].sealnumber;
                            mySQLMethods.mysqltempdatads.hhpickup[0].amount = ssprocessds.hhpickup[i].amount;
                            mySQLMethods.mysqltempdatads.hhpickup[mysqldatarow].pickuptime = ssprocessds.hhpickup[i].pickuptime;
                            mySQLMethods.mysqldatads.hhpickup.ImportRow(mySQLMethods.mysqltempdatads.hhpickup[0]);
                        }

                    }

                    if (mySQLMethods.mysqldatads.hhpickup.Rows.Count > 0)
                    {
                        storeactive = true;
                        // not longer need to do since T-Mobile is happening on php side
                        /*
                        if (ssprocessds.store[r].storecode.Substring(0, 4) != "7822")
                        {
                            for (int i = 0; i <= mySQLMethods.mysqldatads.hhpickup.Rows.Count - 1; i++)
                            {


                                action = "Pickup";
                                customerbagnumber = mySQLMethods.mysqldatads.hhpickup[i].sealnumber.TrimEnd();
                                carrierbarcode = "";
                                saidtocontain = mySQLMethods.mysqldatads.hhpickup[i].amount;
                                arrivaldate = mySQLMethods.mysqldatads.hhpickup[i].pickupdate;
                                pickupdate = mySQLMethods.mysqldatads.hhpickup[i].pickupdate;
                                departuredate = mySQLMethods.mysqldatads.hhpickup[i].pickupdate;
                                pickuptime = mySQLMethods.mysqldatads.hhpickup[i].pickuptime;
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

                                textout.WriteLine(lineout);
                            }

                        }
                        */
                    }
                    // Check for currency drops

                    ssprocessds.currencydrop.Rows.Clear();
                    ClearParameters();
                    AddParms("@storecode", ssprocessds.store[r].storecode.TrimEnd(), "SQL");
                    AddParms("@deliverydate", processdate, "SQL");
                    commandtext = "SELECT * FROM currencydrop WHERE LEFT(storecode,11) = @storecode AND deliverydate = @deliverydate ORDER BY storecode";
                    FillData(ssprocessds, "currencydrop", commandtext, CommandType.Text);
                    if (ssprocessds.currencydrop.Rows.Count > 0)
                    {
                        storeactive = true;
                        // not longer need to do since T-Mobile is happening on php side
                        /*
                        if (ssprocessds.store[r].storecode.Substring(0, 4) != "7822")
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
                        */
                    }

                    // JNS Added  && ssprocessds.store[r].storecode.Substring(0, 4) != "7850"
                    if (!storeactive && 
                        ssprocessds.store[r].storecode.Substring(0, 4) != "7850" &&
                        ssprocessds.store[r].storecode.Substring(0, 4) != "7851"
                        )
                    {
                        recordsProcessed++;
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
                        lineout += processdate.ToString("yyyy-MM-dd ") + ",";
                        // Pickup date
                        lineout += processdate.ToString("yyyy-MM-dd ") + ",";
                        //Departure date
                        lineout += processdate.ToString("yyyy-MM-dd ") + ",";
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
                try
                {
                    if (ConfigurationManager.AppSettings["TestMode"] != "True")
                    {

                        if (recordsProcessed > 0)
                            appUtilities.sendFTP(textfilename, "ifsuseracct", @"ftp://ftp.ssarmored.com/", "PuFu5Ar5");
                    }
                    else
                    {

                        wsgUtilities.wsgNotice("Test Complete");
                    }
                    appUtilities.logEvent("Send DTS File " + processdate.ToString("MMddyyyy"), "DTS", "Complete", true);
                }
                catch
                {
                    throw;
                }
            }
        }
   
    
        public void FillDTSSSQLPickups(string storecode, DateTime dtsprocessdate)
        {
            ssprocessds.hhpickup.Rows.Clear();
            string commandstring = "select * from hhpickup  WHERE  storecode = @storecode  and pickupdate = @dtsprocessdate and adduser <> 'WSG'";
            ClearParameters();
            AddParms("@storecode", storecode);
            AddParms("@dtsprocessdate", dtsprocessdate);
            FillData(ssprocessds, "hhpickup", commandstring, CommandType.Text);
        }

        public void CheckClosing()
        {
            bool closingOK = true;
            // Check to see if the prior day has been closed
            DateTime closingdate = DateTime.Now.Date.AddDays(-1);
            string bnpbankfedid = "021406667";
            if (!commonAppDataMethods.CheckEventDescription("Check Closing " + string.Format("{0:MM/dd/yy}", DateTime.Now.Date)))
            {
                ssprocessds.balance.Rows.Clear();
                ClearParameters();
                commandstring = "SELECT * FROM balance where bankfedid = @bankfedid AND postdate = @postdate";
                AddParms("@bankfedid", bnpbankfedid, "SQL");
                AddParms("@postdate", closingdate, "SQL");
                FillData(ssprocessds, "balance", commandstring, CommandType.Text);
                if (ssprocessds.balance.Rows.Count < 1)
                {
                    closingOK = false;
                }

                if (!closingOK)
                {
                    try
                    {
                        string mailsubject = "BNP not closed " + string.Format("{0:MM/dd/yy}", closingdate);

                        MailMessage dtsMessage = new MailMessage();
                        // Add recipients
                        dtsMessage.To.Clear();
                        dtsMessage.Attachments.Clear();
                        if (ConfigurationManager.AppSettings["TestMode"] == "True")
                        {
                            dtsMessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);

                        }
                        else
                        {
                            dtsMessage.To.Add(ConfigurationManager.AppSettings["SSAlertEmailAddressOne"]);
                            dtsMessage.To.Add(ConfigurationManager.AppSettings["SSAlertEmailAddressTwo"]);
                        }

                        // Send the email
                        appUtilities.SendEmail(dtsMessage, mailsubject, "BNP Not closed");
                    }
                    catch
                    {
                        throw;
                    }

                }
                appUtilities.logEvent("Check Closing " + string.Format("{0:MM/dd/yy}", DateTime.Now.Date), "Clschk", "OK", false);

            }
        }


        public void CheckReports()
        {
            BusinessReportsMethods businessReportsMethods = new BusinessReportsMethods();
            businessReportsMethods.reportpath = ConfigurationManager.AppSettings["REPORTPATH"];
            try
            {
                DateTime reportdate = DateTime.Now.Date.AddDays(-1);


                if (!commonAppDataMethods.CheckEventDescription("Send Activity " + string.Format("{0:MM/dd/yy}", reportdate)))
                {
                    businessReportsMethods.SendPickupReports(reportdate, false);
                }
            }
            catch
            {
                throw;
            }
        }
        public void SystemHousekeeping()
        {
            try
            {

                if (!commonAppDataMethods.CheckEventDescription("System Housekeeping" + string.Format("{0:MM/dd/yy}", processdate)))
                {
                    CleanupFTPSite();
                    CleanupPDFandXLSfiles();
                    // CleanupDataTables();
                    appUtilities.logEvent("System Housekeeping" + string.Format("{0:MM/dd/yy}", processdate), "Complete", "SHK", false);
                }
            }
            catch
            {
                throw;
            }

        }
        public void CleanupDataTables()
        {
            try
            {
                // Clean up database tables
                commonAppDataMethods.CleanUpDataTable("atmjournal", "closedatetime", 5);
                commonAppDataMethods.CleanUpDataTable("atmdrop", "dropdate", 5);
                commonAppDataMethods.CleanUpDataTable("coindrop", "dropdate", 5);
                commonAppDataMethods.CleanUpDataTable("invtrans", "trandate", 4);
                commonAppDataMethods.CleanUpDataTable("depdetail", "postingdate", 5);
                commonAppDataMethods.CleanUpDataTable("billing", "inv_date", 5);
                commonAppDataMethods.CleanUpDataTable("eventlog", "adddate", 10);
            }
            catch
            {
                throw;
            }
        }
        // Recursive function used to dig into the folders and remove files 
        // older than specific number of months starting at a specific folder
        public void CleanupFilesByDirectorys(string dirPath, int numberOfMonths)
        {
            DirectoryInfo dirbase = new DirectoryInfo(dirPath);
            DirectoryInfo[] dirs = dirbase.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // recursive call on the sub directories
                CleanupFilesByDirectorys(dir.FullName, numberOfMonths);
            }
            // Now go ahead and do the files in this directory
            FileInfo[] files = dirbase.GetFiles("*.*");
            foreach (FileInfo f in files)
            {
                if (f.CreationTime.Date < processdate.AddMonths(-1 * numberOfMonths))
                {
                    f.Delete();
                }
            }
        }
        public void CleanupPDFandXLSfiles()
        {
            try
            {
                if (!commonAppDataMethods.CheckEventDescription("PdfXlsClean " + processdate.ToString("MMddyyyy")))
                {
                    int numberOfPdfMonths = int.Parse(ConfigurationManager.AppSettings["PDFCLEANMONTHS"]);
                    int numberOfXlsMonths = int.Parse(ConfigurationManager.AppSettings["XLSCLEANMONTHS"]);
                    CleanupFilesByDirectorys(ConfigurationManager.AppSettings["PDFFilesPath"], numberOfPdfMonths);
                    CleanupFilesByDirectorys(ConfigurationManager.AppSettings["XLSFilesPath"], numberOfXlsMonths);
                    appUtilities.logEvent("PdfXlsClean " + processdate.ToString("MMddyyyy"), "PdfXlsClean", "Complete", false);
                }

            }
            catch (Exception e)
            {
                appUtilities.logEvent("PdfXlsClean Error " + processdate.ToString("MMddyyyy"), "PdfXlsClean", e.Message, true);
                throw;
            }
        }
        public void CleanupFTPSite()
        {
            string fileOn = "";
            try
            {
                if (!commonAppDataMethods.CheckEventDescription("FTPClean " + processdate.ToString("MMddyyyy")))
                {
                    // we do not want to overlap ftp cleans
                    if (!commonAppDataMethods.CheckEventDescription("FTPClean InProgress " + processdate.ToString("MMddyyyy")))
                    {
                        appUtilities.logEvent("FTPClean InProgress " + processdate.ToString("MMddyyyy"), "FTPClean", "InProgress", false);
                        string IPAddress = ConfigurationManager.AppSettings["IMPORTFTPADDRESS"];
                        string LoginName = ConfigurationManager.AppSettings["IMPORTFTPUSERID"];
                        string Password = ConfigurationManager.AppSettings["IMPORTFTPPW"];
                        string ArchivePath = ConfigurationManager.AppSettings["IMPORTFTPARCHPATH"];
                        int numberOfMonths = int.Parse(ConfigurationManager.AppSettings["FTPCLEANMONTHS"]);
                        int FtpPortnumber = int.Parse(ConfigurationManager.AppSettings["IMPORTFTPPORT"]);
                        SessionOptions sessionOptions = new SessionOptions
                        {
                            Protocol = Protocol.Sftp,
                            HostName = IPAddress,
                            PortNumber = FtpPortnumber,
                            UserName = LoginName,
                            Password = Password,
                            SshHostKeyFingerprint = "ssh-rsa 2048 ET3/wzPmJAlINFW+n+yfaNUmYFGTV8pEymFRH4i7SRc="
                        };



                        using (Session session = new Session())
                        {
                            // Connect
                            session.Open(sessionOptions);
                            TransferOptions transferOptions = new TransferOptions();
                            transferOptions.TransferMode = TransferMode.Binary;
                            // Get file Info List
                            List<RemoteFileInfo> filesToDelete = new List<RemoteFileInfo>();
                            List<RemoteFileInfo> remoteFileListResult;
                            remoteFileListResult =
                                session.EnumerateRemoteFiles("/", "*", EnumerationOptions.None).ToList();
                            if (remoteFileListResult.Count > 0)
                            {
                                appUtilities.logEvent("FTPClean found directory list" + processdate.ToString("MMddyyyy"), "FTPClean", "Found " + remoteFileListResult.Count, false);
                                foreach (RemoteFileInfo fileInfo in remoteFileListResult)
                                {
                                    if (!fileInfo.IsDirectory)
                                    {
                                        // If file is older than 6 months
                                        if (fileInfo.LastWriteTime < DateTime.Now.Date.AddMonths(-1 * numberOfMonths))
                                        {
                                            // Download the file into the archive directory
                                            // The 3rd parameter is 'remove file'
                                            // We will do the download and then if download is successfull...
                                            // we will remove the file in the same call.
                                            TransferOperationResult transferResult;
                                            transferResult =
                                                session.GetFiles(fileInfo.FullName, ArchivePath + @"\" + fileInfo.Name, true, transferOptions);
                                            try
                                            {
                                                // Throw on any error
                                                transferResult.Check();
                                                // if there was not an error
                                                // save the fileInfo
                                                filesToDelete.Add(fileInfo);
                                            }
                                            catch (Exception e)
                                            {
                                                appUtilities.logEvent("FTPClean Error " + processdate.ToString("MMddyyyy"), "FTPCleanErr", "File: " + fileInfo.Name + " " + e.Message, true);
                                                throw;
                                            }
                                        }
                                    }
                                }
                                appUtilities.logEvent("FTPClean Archive complete" + processdate.ToString("MMddyyyy"), "FTPClean", "Archived " + filesToDelete.Count + " files", false);
                            }
                        }
                        appUtilities.logEvent("FTPClean " + processdate.ToString("MMddyyyy"), "FTPClean", "Complete", false);
                    }
                }
            }
            catch (Exception e)
            {
                appUtilities.logEvent("FTPClean Error " + processdate.ToString("MMddyyyy"), "FTPCleanErr", fileOn + " " + e.Message, true);
                throw;
            }
        }
        public bool ReadFTP(string ftpaddress, string loginname, string loginpassword, string sourcefile, string targetfile)
        {
            bool result = true;
            FileInfo fileInf = new FileInfo(targetfile);
            string targetfilename = fileInf.Name;

            string fullURI = "ftp://" + ftpaddress + "//" + sourcefile;
            FtpWebRequest reqFTP;
            // Create FtpWebRequest object from the Uri provided
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(fullURI));

            // Provide the WebPermission Credintials
            reqFTP.Credentials = new NetworkCredential(loginname,
                                                       loginpassword);
            // By default KeepAlive is true, where the control connection is 
            // not closed after a command is executed.
            reqFTP.KeepAlive = false;
            // Set passive mode = true
            reqFTP.UsePassive = true;
            // Specify the command to be executed.
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            // Set a time limit for the operation to complete (milliseconds).
            reqFTP.Timeout = 600000;
            // Specify the data transfer type.
            reqFTP.UseBinary = true;


            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            //GET THE FTP RESPONSE
            try
            {
                FtpWebResponse ftpResponse = (FtpWebResponse)reqFTP.GetResponse();

                Stream responseStream = ftpResponse.GetResponseStream();
                FileStream fileStream = new FileStream(targetfile, FileMode.Create);
                int length = 2048;
                Byte[] buffer = new Byte[length];

                int bytesRead = responseStream.Read(buffer, 0, length);

                while (bytesRead > 0)
                {
                    fileStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, length);
                }

                fileStream.Close();
                responseStream.Close();
            }
            catch
            {
                result = false;
            }
            return result;
        }
        public string DeleteFTPFile(string LoginName, string Password, string filename)
        {

            // File name includes the ip address

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filename);
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = new NetworkCredential(LoginName, Password);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                return response.StatusDescription;
            }

        }
    }

}



