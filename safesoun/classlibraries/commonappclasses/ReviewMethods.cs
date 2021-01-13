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
using WSGUtilitieslib;
using System.Text;


namespace CommonAppClasses
{
    public class ReviewMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Web Site Synchronization");
        AppUtilities appUtilities = new AppUtilities();
        sysdata sydatads = new sysdata();
        AMSECDataSet amsecds = new AMSECDataSet();
        mysqldata mysqldatads = new mysqldata();
        ssprocess ssprocessds = new ssprocess();
        ssprocess sssearchprocessds = new ssprocess();
        ssprocess tempprocessds = new ssprocess();
        DataRow[] foundRows;
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        mysqldata mysqlsearchds = new mysqldata();
        List<string> EmailAttachments = new List<string>();
        DateTime processdate = DateTime.Now.Date;
        string commandstring = "";
        public ReviewMethods()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.store.idcolColumn);
            AppUserClass.AppUserId = "WSG";

        }


        public void StartReview()
        {
            DateTime reviewdate = DateTime.Now.Date;
            string reviewname = "DAILYREVIEW" + reviewdate.ToString("yyyyMMdd");
            if (!commonAppDataMethods.CheckEventDescription(reviewname))
            {
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                ImportMorphisDropData(reviewdate);
                ImportMorphisServiceData(reviewdate);
                appUtilities.logEvent(reviewname, "DAILYREVIEW", "Complete", false);
                frmLoading.Close();
            }
        }


        public void ImportMorphisDropData(DateTime reviewdate)
        {
            // Where the spreadheet will be stored
            string morphisfilename = ConfigurationManager.AppSettings["morphisdroppath"];

            // Name of the file to be imported
            string businessDateString = "ATMFILL" + reviewdate.AddDays(-1).ToString("yyyyMMdd");

            if (File.Exists(morphisfilename))
            {
                File.Delete(morphisfilename);
            }

            if (!commonAppDataMethods.CheckEventDescription(businessDateString))
            {
                string IPAddress = ConfigurationManager.AppSettings["IMPORTFTPADDRESS"];
                string LoginName = ConfigurationManager.AppSettings["IMPORTFTPUSERID"];
                string Password = ConfigurationManager.AppSettings["IMPORTFTPPW"];

                bool webfilefound = false;
                webfilefound = ReadFTP(IPAddress, LoginName, Password, businessDateString + ".xls", morphisfilename);


                if (webfilefound)
                {

                    //    AppUserClass.AppUserId = "Conv";
                    Microsoft.Office.Interop.Excel.Application xl = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook workbook = xl.Workbooks.Open(morphisfilename);
                    Microsoft.Office.Interop.Excel.Worksheet sheet = workbook.Sheets[1];
                    int wkshtid = 10115;
                    string atmid = "";
                    string driver = "";
                    string compcode = "";
                    string companyname = "";
                    DateTime dropdate = DateTime.Now;
                    decimal zipcode = 0;
                    string excelcolumn = "";
                    int numRows = sheet.UsedRange.Rows.Count;
                    string storenumber = "";
                    string storecode = "";
                    string servicetype = "";
                    for (int rowIndex = 1; rowIndex <= numRows; rowIndex++)
                    {
                        // Skip Title row
                        if (rowIndex == 1)
                        {
                            continue;
                        }
                        ssprocessds.store.Rows.Clear();
                        ssprocessds.store.Rows.Add();
                        EstablishBlankDataTableRow(ssprocessds.store);

                        ssprocessds.atmdrop.Rows.Clear();
                        ssprocessds.atmdrop.Rows.Add();
                        EstablishBlankDataTableRow(ssprocessds.atmdrop);


                        //Establish cells           
                        Microsoft.Office.Interop.Excel.Range companynamecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 1];
                        Microsoft.Office.Interop.Excel.Range drivercell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 2];
                        Microsoft.Office.Interop.Excel.Range atmidcell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 3];
                        Microsoft.Office.Interop.Excel.Range storenamecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 4];
                        Microsoft.Office.Interop.Excel.Range storeaddress1cell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 5];
                        Microsoft.Office.Interop.Excel.Range storeaddress2cell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 6];
                        Microsoft.Office.Interop.Excel.Range storecitycell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 7];
                        Microsoft.Office.Interop.Excel.Range storezipcell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 8];
                        Microsoft.Office.Interop.Excel.Range storestatecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 9];
                        Microsoft.Office.Interop.Excel.Range dropdatecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 10];
                        Microsoft.Office.Interop.Excel.Range servicetypecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 11];

                        // Establish ATM Id and drop date
                        atmid = atmidcell.Value;
                        dropdate = (DateTime)dropdatecell.Value;

                        // Check to see if this drop has been previously imported
                        ssprocessds.atmdrop.Rows.Clear();
                        ClearParameters();
                        commandstring = "SELECT * FROM atmdrop where atmid = @atmid AND dropdate = @dropdate";
                        AddParms("@atmid", atmid, "SQL");
                        AddParms("@dropdate", dropdate, "SQL");

                        FillData(ssprocessds, "atmdrop", commandstring, CommandType.Text);
                        if (ssprocessds.atmdrop.Rows.Count > 0)
                        {
                            continue;
                        }

                        // See if the ATM is in the old system
                        ssprocessds.atmmast.Rows.Clear();
                        ClearParameters();
                        commandstring = "SELECT * FROM atmmast where atmid = @atmid";
                        AddParms("@atmid", atmid, "SQL");

                        FillData(ssprocessds, "atmmast", commandstring, CommandType.Text);
                        if (ssprocessds.atmmast.Rows.Count > 0)
                        {
                            compcode = ssprocessds.atmmast[0].owner;

                        }
                        else
                        {

                            // Locate the owner
                            //The company number is determined from the company name
                            companyname = (string)companynamecell.Value.ToUpper();
                            if (companyname.Substring(0, 5).ToUpper() == "ATM W")
                            {
                                compcode = "5590";
                            }
                            else
                            {
                                ssprocessds.company.Rows.Clear();
                                ClearParameters();
                                commandstring = "SELECT * FROM company WHERE UPPER(name) = @compname";
                                AddParms("@compname", companyname, "SQL");
                                FillData(ssprocessds, "company", commandstring, CommandType.Text);
                                if (ssprocessds.company.Rows.Count > 0)
                                {
                                    compcode = ssprocessds.company[0].comp_code;
                                }
                                else
                                {
                                    // Process unfound company
                                    compcode = "7777";
                                    appUtilities.logEvent("Invalid Compcode +" + atmid, "ATMFill", "Complete", true);

                                }
                            }
                        }
                        // Locate the driver
                        driver = (string)drivercell.Value;
                        driver = driver.Substring(0, 2);
                        if (driver.TrimEnd().Length < 2)
                        {
                            driver = "0" + driver.TrimEnd();
                        }
                        ssprocessds.driver.Rows.Clear();
                        ClearParameters();
                        commandstring = "SELECT * FROM driver WHERE number = @driver";
                        AddParms("@driver", driver, "SQL");
                        FillData(ssprocessds, "driver", commandstring, CommandType.Text);
                        if (ssprocessds.driver.Rows.Count < 1)
                        {
                            // Process unfound driver
                            driver = "88";
                            appUtilities.logEvent("Invalid Driver +" + atmid, "ATMServ", "Complete", true);

                        }


                        // Get the final 6 characters of the atm id.
                        storenumber = atmid.Substring(atmid.Length - 6);
                        // store number - add the last 6 character of the atm id to the company code
                        storenumber = storenumber.TrimEnd().TrimStart();
                        storenumber = storenumber.PadLeft(6, '0');
                        storecode = compcode + "-" + storenumber;
                        if (!DoesStoreExist(storecode))
                        {
                            ssprocessds.store[0].storecode = storecode;
                            // store name
                            ssprocessds.store[0].store_name = (string)storenamecell.Value;
                            // store address
                            ssprocessds.store[0].f_address = (string)storeaddress1cell.Value;
                            // store city
                            ssprocessds.store[0].f_city = (string)storecitycell.Value;
                            // store state
                            ssprocessds.store[0].f_state = (string)storestatecell.Value;
                            try
                            {
                                zipcode = (decimal)storezipcell.Value;
                            }
                            catch
                            {
                                zipcode = 10011;
                            }
                            ssprocessds.store[0].f_zip = Convert.ToString(Convert.ToUInt32(zipcode));
                            ssprocessds.store[0].f_zip = ssprocessds.store[0].f_zip.PadRight(5, '0');
                            ssprocessds.store[0].f_zip = ssprocessds.store[0].f_zip.Substring(0, 5);
                            ssprocessds.store[0].custno = "";
                            ssprocessds.store[0].sigcoin = "N";
                            ssprocessds.store[0].active = true;
                            ssprocessds.store[0].cg_drp_rt = 20.00M;
                            ssprocessds.store[0].roll_25 = .20M;
                            ssprocessds.store[0].strap_5 = .50M;
                            ssprocessds.store[0].stop_date = Convert.ToDateTime("12/31/2099");
                            ssprocessds.store[0].start_date = DateTime.Now.Date;
                            ssprocessds.store[0].store_name = ssprocessds.store[0].store_name.PadRight(30).Substring(0, 30).TrimEnd();
                            ssprocessds.store[0].f_address = ssprocessds.store[0].f_address.PadRight(25).Substring(0, 25).TrimEnd();
                            ssprocessds.store[0].f_city = ssprocessds.store[0].f_city.PadRight(15).Substring(0, 15).TrimEnd();
                            ssprocessds.store[0].f_state = ssprocessds.store[0].f_state.TrimEnd().TrimStart();
                            ssprocessds.store[0].f_zip = ssprocessds.store[0].f_zip.Substring(0, 5).TrimEnd();
                            ssprocessds.store[0].tax_area = commonAppDataMethods.gettaxarea(ssprocessds.store[0].f_city, ssprocessds.store[0].f_state, ssprocessds.store[0].f_zip);
                            GenerateAppTableRowSave(ssprocessds.store[0]);
                        }

                        // Create ATM Drop
                        ClearParameters();
                        AddParms("@atmid", atmid);
                        AddParms("@wkshtid", wkshtid);
                        AddParms("@store", storecode);
                        AddParms("@driver", driver);
                        AddParms("@dropdate", dropdate);
                        AddParms("@ones", 1);
                        AddParms("@fives", 0);
                        AddParms("@tens", 0);
                        AddParms("@twentys", 0);
                        AddParms("@fiftys", 0);
                        AddParms("@hundreds", 0);
                        //Emergency
                        if (servicetypecell.Value.Equals("Emergency"))
                            AddParms("@erfill", "Y");
                        else
                            AddParms("@erfill", "N");
                        AddParms("@trantype", "D");
                        AddParms("@adduser", "MPH");
                        // Unscheduled
                        if (servicetypecell.Value.Equals("Unscheduled"))
                            AddParms("@unschedfill", "Y");
                        else
                            AddParms("@unschedfill", "N");
                        ExecuteCommand("sp_Insert_AtmWkShtDrop", CommandType.StoredProcedure);
                    }
                    xl.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xl);
                    appUtilities.logEvent(businessDateString, "ATMFill", "Complete", false);
                }
            }
        }

        public void ImportMorphisServiceData(DateTime reviewdate)
        {
            // Where the spreadsheet will be saved
            string morphisfilename = ConfigurationManager.AppSettings["morphisservicepath"];
            string businessDateString = "ATMSERV" + reviewdate.AddDays(-1).ToString("yyyyMMdd");
            bool webfilefound = false;
            string IPAddress = ConfigurationManager.AppSettings["IMPORTFTPADDRESS"];
            string LoginName = ConfigurationManager.AppSettings["IMPORTFTPUSERID"];
            string Password = ConfigurationManager.AppSettings["IMPORTFTPPW"];

            if (File.Exists(morphisfilename))
            {
                File.Delete(morphisfilename);
            }


            if (!commonAppDataMethods.CheckEventDescription(businessDateString))
            {

                webfilefound = ReadFTP(IPAddress, LoginName, Password, businessDateString + ".xls", morphisfilename);
                if (webfilefound)
                {

                    AppUserClass.AppUserId = "Conv";
                    Microsoft.Office.Interop.Excel.Application xl = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook workbook = xl.Workbooks.Open(morphisfilename);
                    Microsoft.Office.Interop.Excel.Worksheet sheet = workbook.Sheets[1];
                    int wkshtid = 10115;
                    string atmid = "";
                    string driver = "";
                    string companyname = "";
                    string compcode = "";
                    string stringpartscost = "";
                    string stringbatterycount = "";
                    string servicer = "";
                    string servicetype = "";
                    decimal zipcode = 0;
                    string servicetypecode = "";
                    DateTime servicedate = DateTime.Now;
                    int batterycount = 0;
                    decimal partscost = 0;
                    string excelcolumn = "";
                    int numRows = sheet.UsedRange.Rows.Count;
                    string storenumber = "";
                    string storecode = "";
                    for (int rowIndex = 1; rowIndex <= numRows; rowIndex++)
                    {
                        // Skip Title row
                        if (rowIndex == 1)
                        {
                            continue;
                        }
                        ssprocessds.store.Rows.Clear();
                        ssprocessds.store.Rows.Add();
                        EstablishBlankDataTableRow(ssprocessds.store);

                        ssprocessds.atmservice.Rows.Clear();
                        ssprocessds.atmservice.Rows.Add();
                        EstablishBlankDataTableRow(ssprocessds.atmservice);


                        //Establish cells           
                        Microsoft.Office.Interop.Excel.Range companynamecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 1];
                        Microsoft.Office.Interop.Excel.Range drivercell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 3];
                        Microsoft.Office.Interop.Excel.Range atmidcell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 4];
                        Microsoft.Office.Interop.Excel.Range storenamecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 5];
                        Microsoft.Office.Interop.Excel.Range storeaddress1cell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 6];
                        Microsoft.Office.Interop.Excel.Range storeaddress2cell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 7];
                        Microsoft.Office.Interop.Excel.Range storecitycell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 8];
                        Microsoft.Office.Interop.Excel.Range storezipcell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 9];
                        Microsoft.Office.Interop.Excel.Range storestatecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 10];
                        Microsoft.Office.Interop.Excel.Range servicedatecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 11];
                        Microsoft.Office.Interop.Excel.Range batterycountcell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 12];
                        Microsoft.Office.Interop.Excel.Range partscostcell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 13];
                        Microsoft.Office.Interop.Excel.Range servicercell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 15];
                        Microsoft.Office.Interop.Excel.Range servicetypecell = (Microsoft.Office.Interop.Excel.Range)sheet.Cells[rowIndex, 16];




                        // Establish ATM Id and service date
                        atmid = atmidcell.Value;
                        servicedate = (DateTime)servicedatecell.Value;
                        servicedate = servicedate.Date;

                        // Check to see if this service has been previously imported
                        ssprocessds.atmservice.Rows.Clear();
                        ClearParameters();
                        commandstring = "SELECT * FROM atmservice where atmid = @atmid AND CAST(orderdate AS date) = CAST(@servicedate AS date)";
                        AddParms("@atmid", atmid, "SQL");
                        AddParms("@servicedate", servicedate, "SQL");

                        FillData(ssprocessds, "atmservice", commandstring, CommandType.Text);
                        if (ssprocessds.atmservice.Rows.Count > 0)
                        {
                            continue;
                        }

                        // Locate the owner
                        //The company number is determined from the company name
                        companyname = (string)companynamecell.Value.ToUpper();

                        // See if the ATM is in the old system
                        ssprocessds.atmmast.Rows.Clear();
                        ClearParameters();
                        commandstring = "SELECT * FROM atmmast where atmid = @atmid";
                        AddParms("@atmid", atmid, "SQL");

                        FillData(ssprocessds, "atmmast", commandstring, CommandType.Text);
                        if (ssprocessds.atmmast.Rows.Count > 0)
                        {
                            compcode = ssprocessds.atmmast[0].owner;

                        }
                        else
                        {

                            // Locate the owner
                            //The company number is determined from the company name
                            companyname = (string)companynamecell.Value.ToUpper();
                            if (companyname.Substring(0, 5).ToUpper() == "ATM W")
                            {
                                compcode = "5590";
                            }
                            else
                            {
                                ssprocessds.company.Rows.Clear();
                                ClearParameters();
                                commandstring = "SELECT * FROM company WHERE UPPER(name) = @compname";
                                AddParms("@compname", companyname, "SQL");
                                FillData(ssprocessds, "company", commandstring, CommandType.Text);
                                if (ssprocessds.company.Rows.Count > 0)
                                {
                                    compcode = ssprocessds.company[0].comp_code;
                                }
                                else
                                {
                                    // Process unfound company
                                    compcode = "7777";
                                    appUtilities.logEvent("Invalid Compcode +" + atmid, "ATMFill", "Complete", true);

                                }
                            }
                        }
                        // Locate the driver
                        driver = (string)drivercell.Value;
                        driver = driver.Substring(0, 2);
                        if (driver.TrimEnd().Length < 2)
                        {
                            driver = "0" + driver.TrimEnd();
                        }
                        ssprocessds.driver.Rows.Clear();
                        ClearParameters();
                        commandstring = "SELECT * FROM driver WHERE number = @driver";
                        AddParms("@driver", driver, "SQL");
                        FillData(ssprocessds, "driver", commandstring, CommandType.Text);
                        if (ssprocessds.driver.Rows.Count < 1)
                        {
                            // Process unfound driver
                            driver = "88";
                            appUtilities.logEvent("Invalid Driver +" + atmid, "ATMServ", "Complete", true);

                        }





                        // Get the final 6 characters of the atm id.
                        storenumber = atmid.Substring(atmid.Length - 6);
                        // store number - add the last 6 character of the atm id to the company code
                        storenumber = storenumber.TrimEnd().TrimStart();
                        storenumber = storenumber.PadLeft(6, '0');
                        storecode = compcode + "-" + storenumber;
                        if (!DoesStoreExist(storecode))
                        {
                            ssprocessds.store[0].storecode = storecode;
                            // store name
                            ssprocessds.store[0].store_name = (string)storenamecell.Value;
                            // store address
                            ssprocessds.store[0].f_address = (string)storeaddress1cell.Value;
                            // store address
                            ssprocessds.store[0].f_city = (string)storecitycell.Value;
                            // store city
                            ssprocessds.store[0].f_state = (string)storestatecell.Value;
                            // store state
                            zipcode = Convert.ToDecimal(storezipcell.Value);
                            ssprocessds.store[0].f_zip = Convert.ToString(Convert.ToUInt32(zipcode));
                            ssprocessds.store[0].f_zip = ssprocessds.store[0].f_zip.PadRight(5, '0');
                            ssprocessds.store[0].f_zip = ssprocessds.store[0].f_zip.Substring(0, 5);
                            ssprocessds.store[0].custno = "";
                            ssprocessds.store[0].sigcoin = "N";
                            ssprocessds.store[0].active = true;
                            ssprocessds.store[0].cg_drp_rt = 20.00M;
                            ssprocessds.store[0].roll_25 = .20M;
                            ssprocessds.store[0].strap_5 = .50M;
                            ssprocessds.store[0].stop_date = Convert.ToDateTime("12/31/2099");
                            ssprocessds.store[0].start_date = DateTime.Now.Date;
                            ssprocessds.store[0].store_name = ssprocessds.store[0].store_name.PadRight(30).Substring(0, 30).TrimEnd();
                            ssprocessds.store[0].f_address = ssprocessds.store[0].f_address.PadRight(25).Substring(0, 25).TrimEnd();
                            ssprocessds.store[0].f_city = ssprocessds.store[0].f_city.PadRight(15).Substring(0, 15).TrimEnd();
                            ssprocessds.store[0].f_state = ssprocessds.store[0].f_state.TrimEnd().TrimStart();
                            ssprocessds.store[0].f_zip = ssprocessds.store[0].f_zip.Substring(0, 5).TrimEnd();
                            ssprocessds.store[0].tax_area = commonAppDataMethods.gettaxarea(ssprocessds.store[0].f_city, ssprocessds.store[0].f_state, ssprocessds.store[0].f_zip);
                            GenerateAppTableRowSave(ssprocessds.store[0]);
                        }
                        stringpartscost = (String)partscostcell.Value;
                        if (stringpartscost == null)
                        {
                            stringpartscost = "0";
                        }
                        partscost = Convert.ToDecimal(stringpartscost);
                        servicer = (string)servicercell.Value;
                        stringbatterycount = Convert.ToString(batterycountcell.Value);
                        batterycount = Convert.ToInt16(stringbatterycount);


                        // Develop service type
                        servicetype = (string)servicetypecell.Value;

                        switch (servicetype.ToUpper().Substring(0, 4))
                        {
                            case "FIRS":
                                {
                                    servicetypecode = "F";
                                    break;
                                }
                            case "SECO":
                                {
                                    servicetypecode = "S";
                                    break;
                                }
                            case "FAIL":
                                {
                                    servicetypecode = "U";
                                    break;
                                }
                            case "DEIN":
                                {
                                    servicetypecode = "D";
                                    break;
                                }

                            default:
                                {
                                    servicetypecode = "F";
                                    break;
                                }

                        }

                        // Create ATM Service Row
                        ClearParameters();
                        AddParms("@atmid", atmid);
                        AddParms("@servicetype", servicetypecode);
                        AddParms("@servicer", servicer);
                        AddParms("@store", storecode);
                        AddParms("@driver", driver);
                        AddParms("@orderdate", servicedate);
                        AddParms("@partscost", partscost);
                        AddParms("@batterycount", batterycount);
                        AddParms("@notes", "Import");
                        AddParms("@adduser", "MPH");
                        ExecuteCommand("sp_Insert_AtmService", CommandType.StoredProcedure);
                    }
                    xl.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xl);
                    appUtilities.logEvent(businessDateString, "ATMServ", "Complete", false);
                }
            }
        }

        public bool DoesStoreExist(string storecode)
        {
            bool storeexists = false;
            sssearchprocessds.store.Rows.Clear();
            ClearParameters();
            commandstring = "SELECT * FROM store WHERE storecode = @storecode";
            AddParms("@storecode", storecode, "SQL");
            FillData(sssearchprocessds, "store", commandstring, CommandType.Text);
            if (sssearchprocessds.store.Rows.Count > 0)
            {
                storeexists = true;
            }
            return storeexists;
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






    }
}
