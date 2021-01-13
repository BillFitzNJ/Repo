using System;
using System.IO;
using System.Windows;
using System.Configuration;
using System.Windows.Forms;
using System.Collections.Generic;
using CommonAppClasses;
using WSGUtilitieslib;
using System.Net;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Text;




namespace BusinessTransactions
{
    public class SmartSafeImportMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("SmartSafeImport");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess ssprocessds = new ssprocess();
        ssprocess sssearchprocessds = new ssprocess();

        DateTime PostingDate = new DateTime();
        DateTime businessDate = new DateTime();
        string transactiontype = "";
        DateTime processdate = new DateTime();
        string businessDateString = "";
        AMSECDataSet amsecds = new AMSECDataSet();
        FrmLoading frmLoading = new FrmLoading();
        string safeserialnumber = "";
        bool cont = true;
        string commandstring = "";
        string[] customerdata = new string[2];
        string bankfedid = "SMARTSAFE";
        public SmartSafeImportMethods()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.smartsafetrans.idcolColumn);
            SetIdcol(ssprocessds.safedeposit.idcolColumn);
        }

        public void ImmportSmartSafeXML()
        {
            if (commonAppDataMethods.GetSingleDate("Selecte Activity Posting Date", 200, 200))
            {
                businessDate = commonAppDataMethods.SelectedDate.Date;
            }
            else
            {

            }



            if (!wsgUtilities.wsgReply("Do you want to import Smart Safe Data from AMSEC?"))
            {
                cont = false;
            }
            if (cont)
            {
                frmLoading.Show();
                PostingDate = commonAppDataMethods.GetNextPostDate(bankfedid);
                businessDateString = businessDate.ToString("yyyyMMdd");
                string IPAddress = ConfigurationManager.AppSettings["IMPORTFTPADDRESS"];
                string LoginName = ConfigurationManager.AppSettings["IMPORTFTPUSERID"];
                string Password = ConfigurationManager.AppSettings["IMPORTFTPPW"];
                IPAddress = "ftp://" + IPAddress + "/";



                string WildCard = "*" + businessDateString + "*";
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
                    ReadXMLFile(IPAddress, LoginName, Password, Files[f]);
                }
                wsgUtilities.wsgNotice("Import Complete");
                frmLoading.Close();
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }






        public void ReadXMLFile(string IPAddress, string LoginName, string Password, string filename)
        {
            try
            {
                bool cont = true;
                IPAddress = IPAddress + "/" + filename;
                System.Net.FtpWebRequest reqFileFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(IPAddress));
                reqFileFTP.Credentials = new System.Net.NetworkCredential(LoginName, Password);
                reqFileFTP.Timeout = 600000;
                reqFileFTP.UseBinary = false;
                reqFileFTP.UsePassive = true;
                reqFileFTP.KeepAlive = true;




                if (cont)
                {

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
                    amsecds.ManualDrop.Rows.Clear();
                    amsecds.ManualDrops.Rows.Clear();
                    amsecds.Cassette.Rows.Clear();
                    amsecds.CurrentEod.Rows.Clear();
                    amsecds.SafeAndSound.Rows.Clear();
                    amsecds.ReadXml(xmlstream);
                    if (amsecds.SafeAndSound.Rows.Count > 0)
                    {
                        if (Convert.ToDateTime(amsecds.SafeAndSound[0].StartDateTime) >= businessDate.AddDays(-3) && amsecds.SafeAndSound[0].filetype == "END_OF_DAY")
                        {
                            // Currently doing  Declared  for Signature Bank 
                            // Locate Smart Safe ID using serial number


                            ssprocessds.safemast.Rows.Clear();
                            commandstring = "SELECT * FROM safemast WHERE serialnumber = @serialnumber ";
                            ClearParameters();
                            AddParms("serialnumber", amsecds.SafeAndSound[0].SerialNumber);
                            ssprocessds.safemast.Rows.Clear();
                            FillData(ssprocessds, "safemast", commandstring, CommandType.Text);
                            if (ssprocessds.safemast.Rows.Count > 0)
                            {
                                if (ssprocessds.safemast[0].bankfedid.TrimEnd() == "SMARTSAFE")
                                {
                                    // Develop amounts
                                    declaredamount = Convert.ToDecimal(amsecds.CurrentEod[0].Deposit);
                                    declaredamount += Convert.ToDecimal(amsecds.ManualDrops[0].Cash);
                                    declaredamount += Convert.ToDecimal(amsecds.ManualDrops[0].Coins);
                                    checksamount += Convert.ToDecimal(amsecds.ManualDrops[0].Checks);

                                    // Prevent duplicate entries
                                    commandstring = "SELECT * FROM view_expandedsmartsafetrans WHERE eventcode = 'DECL' AND RTRIM(serialnumber) = @serialnumber AND pickupdate = @businessdate ";
                                    ClearParameters();
                                    ssprocessds.view_expandedsmartsafetrans.Rows.Clear();
                                    AddParms("serialnumber", amsecds.SafeAndSound[0].SerialNumber.TrimEnd());
                                    AddParms("businessdate", businessDate.Date);
                                    FillData(ssprocessds, "view_expandedsmartsafetrans", commandstring, CommandType.Text);
                                    if (ssprocessds.view_expandedsmartsafetrans.Rows.Count < 1)
                                    {
                                        customerdata = commonAppDataMethods.GetCustomerData(ssprocessds.safemast[0].storecode);
                                        ssprocessds.smartsafetrans.Rows.Add();
                                        EstablishBlankDataTableRow(ssprocessds.smartsafetrans);
                                        ssprocessds.smartsafetrans[0].bankfedid = ssprocessds.safemast[0].bankfedid;
                                        ssprocessds.smartsafetrans[0].pickupdate = businessDate;
                                        ssprocessds.smartsafetrans[0].postingdate = PostingDate;
                                        ssprocessds.smartsafetrans[0].customerdda = customerdata[0];
                                        ssprocessds.smartsafetrans[0].masteraccountid = customerdata[1];
                                        ssprocessds.smartsafetrans[0].safeid = ssprocessds.safemast[0].idcol;
                                        ssprocessds.smartsafetrans[0].store = ssprocessds.safemast[0].storecode;
                                        ssprocessds.smartsafetrans[0].eventcode = "DECL";
                                        ssprocessds.smartsafetrans[0].saidtocontain = declaredamount;
                                        ssprocessds.smartsafetrans[0].checks = checksamount;
                                        GenerateAppTableRowSave(ssprocessds.smartsafetrans[0]);
                                    }
                                    // Safe deposit
                                    // WLF - removed 03/03/20 - moved to ssprowler



                                }
                            }

                        }
                        else
                        {
                            /**     if(Convert.ToDecimal(amsecds.CurrentEod[0].Deposit) != 0)
                                 {
                                     MessageBox.Show(amsecds.SafeAndSound[0].SerialNumber + "has courier pickup deposit amount");
                                 }
                              */
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
            }
        }

    }
}

