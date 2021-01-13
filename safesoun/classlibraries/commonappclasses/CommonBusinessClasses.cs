using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Net.Mail;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CommonAppClasses
{



    public class CommonAppDataMethods : WSGDataAccess
    {
        public DateTime SelectedDate { get; set; }
        public DateTime SelectedStartDate { get; set; }
        public DateTime SelectedEndDate { get; set; }
        FrmSelector frmSelector = new FrmSelector();
        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        AppUtilities appUtilities = new AppUtilities();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Armed Courier");
        public ssprocess ssprocessselectords = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        sysdata sysdatads = new sysdata();

        public CommonAppDataMethods()
            : base("SQL", "SQLConnString")
        {

            SetIdcol(ssprocessds.emailbank.idcolColumn);
            SetIdcol(ssprocessds.bank.idcolColumn);

        }
        public int SelectSmartSafe()
        {
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.view_expandedsafemast.Rows.Clear();
            string commandtext = "SELECT * FROM view_expandedsafemast ORDER BY storename";
            ClearParameters();
            FillData(ssprocessselectords, "view_expandedsafemast", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Smart Safe";
            frmSelectorMethods.dtSource = ssprocessselectords.view_expandedsafemast;
            frmSelectorMethods.columncount = 5;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "storecodecol";
            frmSelectorMethods.colheadertext[0] = "Store Code";
            frmSelectorMethods.coldatapropertyname[0] = "storecode";
            frmSelectorMethods.colwidth[0] = 100;
            frmSelectorMethods.colname[1] = "storenamecol";
            frmSelectorMethods.colheadertext[1] = "Store";
            frmSelectorMethods.coldatapropertyname[1] = "storename";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.colname[2] = "Manufacturecol";
            frmSelectorMethods.colheadertext[2] = "Manufacturer";
            frmSelectorMethods.coldatapropertyname[2] = "manufacturer";
            frmSelectorMethods.colwidth[2] = 100;
            frmSelectorMethods.colname[3] = "Serialnumberccol";
            frmSelectorMethods.colheadertext[3] = "Serial Number";
            frmSelectorMethods.coldatapropertyname[3] = "serialnumber";
            frmSelectorMethods.colwidth[3] = 150;
            frmSelectorMethods.colname[3] = "Bankfedidcol";
            frmSelectorMethods.colheadertext[4] = "Bank";
            frmSelectorMethods.coldatapropertyname[4] = "bankfedid";
            frmSelectorMethods.colwidth[4] = 150;
            frmSelectorMethods.SetGrid();

            frmSelectorMethods.SetGrid();
            frmSelector.Width = 1500;
            int selectedsafemastid = frmSelectorMethods.ShowSelector();
            return selectedsafemastid;
        }

        public int SelectSafekeepingBank()
        {
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.bank.Rows.Clear();
            string commandtext = "SELECT * FROM bank WHERE safekeeping = 'Y' ORDER BY bankfedid";
            ClearParameters();
            FillData(ssprocessselectords, "bank", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Bank";
            frmSelectorMethods.dtSource = ssprocessselectords.bank;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "BankfedIdcol";
            frmSelectorMethods.colheadertext[0] = "Bank Fed Id";
            frmSelectorMethods.coldatapropertyname[0] = "bankfedid";
            frmSelectorMethods.colwidth[0] = 300;
            frmSelectorMethods.colname[1] = "Namecol";
            frmSelectorMethods.colheadertext[1] = "Bank Name";
            frmSelectorMethods.coldatapropertyname[1] = "name";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 700;
            int selectedbankid = frmSelectorMethods.ShowSelector();
            return selectedbankid;
        }
        public int SelectRateSchedule()
        {
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.rateschedule.Rows.Clear();
            string commandtext = "SELECT * FROM rateschedule ORDER BY schedulename";
            ClearParameters();
            FillData(ssprocessselectords, "rateschedule", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Rate Schedule";
            frmSelectorMethods.dtSource = ssprocessselectords.rateschedule;
            frmSelectorMethods.columncount = 1;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "schedulename";
            frmSelectorMethods.colheadertext[0] = "Schedule";
            frmSelectorMethods.coldatapropertyname[0] = "schedulename";
            frmSelectorMethods.colwidth[0] = 450;
            frmSelectorMethods.SetGrid();
            int selectedratescheduleid = frmSelectorMethods.ShowSelector();
            return selectedratescheduleid;
        }


        public int SelectBankSmartSafe(string bankfedid)
        {
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.view_expandedsafemast.Rows.Clear();
            string commandtext = "SELECT * FROM view_expandedsafemast WHERE bankfedid = @bankfedid ORDER BY storename";
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(ssprocessselectords, "view_expandedsafemast", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Smart Safe";
            frmSelectorMethods.dtSource = ssprocessselectords.view_expandedsafemast;
            frmSelectorMethods.columncount = 5;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "storecodecol";
            frmSelectorMethods.colheadertext[0] = "Store Code";
            frmSelectorMethods.coldatapropertyname[0] = "storecode";
            frmSelectorMethods.colwidth[0] = 100;
            frmSelectorMethods.colname[1] = "storenamecol";
            frmSelectorMethods.colheadertext[1] = "Store";
            frmSelectorMethods.coldatapropertyname[1] = "storename";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.colname[2] = "Manufacturecol";
            frmSelectorMethods.colheadertext[2] = "Manufacturer";
            frmSelectorMethods.coldatapropertyname[2] = "manufacturer";
            frmSelectorMethods.colwidth[2] = 100;
            frmSelectorMethods.colname[3] = "Serialnumberccol";
            frmSelectorMethods.colheadertext[3] = "Serial Number";
            frmSelectorMethods.coldatapropertyname[3] = "serialnumber";
            frmSelectorMethods.colwidth[3] = 150;
            frmSelectorMethods.colname[3] = "Bankfedidcol";
            frmSelectorMethods.colheadertext[4] = "Bank";
            frmSelectorMethods.coldatapropertyname[4] = "bankfedid";
            frmSelectorMethods.colwidth[4] = 150;
            frmSelectorMethods.SetGrid();
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 1500;
            int selectedsafemastid = frmSelectorMethods.ShowSelector();
            return selectedsafemastid;
        }





        public DateTime GetLastEventOccurence(string eventcode)
        {
            DateTime lasteventdatetime = DateTime.MinValue;
            sysdatads.eventlog.Rows.Clear();
            string commandtext = "SELECT TOP 1* FROM eventlog where eventcode = @eventcode ORDER BY adddate DESC";
            ClearParameters();
            AddParms("@eventcode", eventcode, "SQL");
            FillData(sysdatads, "eventlog", commandtext, CommandType.Text);
            if (sysdatads.eventlog.Rows.Count > 0)
            {
                lasteventdatetime = sysdatads.eventlog[0].adddate;
            }
            return lasteventdatetime;
        }



        public bool CheckEventDescription(string eventdesc)
        {
            bool descfound = false;
            sysdatads.eventlog.Rows.Clear();
            string commandtext = "SELECT * FROM eventlog where eventdesc = @eventdesc";
            ClearParameters();
            AddParms("@eventdesc", eventdesc, "SQL");
            FillData(sysdatads, "eventlog", commandtext, CommandType.Text);
            if (sysdatads.eventlog.Rows.Count > 0)
            {
                descfound = true;
            }

            return descfound;
        }

        public string SelectDriver()
        {
            string selecteddrivernumber = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.bank.Rows.Clear();
            string commandtext = "SELECT * FROM view_expandeddriver ORDER BY number";
            ClearParameters();
            FillData(ssprocessselectords, "view_expandeddriver", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Driver";
            frmSelectorMethods.dtSource = ssprocessselectords.view_expandeddriver;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "driver";
            frmSelectorMethods.colheadertext[0] = "Number";
            frmSelectorMethods.coldatapropertyname[0] = "number";
            frmSelectorMethods.colwidth[0] = 30;
            frmSelectorMethods.colname[1] = "Namecol";
            frmSelectorMethods.colheadertext[1] = "Driver Name";
            frmSelectorMethods.coldatapropertyname[1] = "fullname";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 700;
            int selecteddriverid = frmSelectorMethods.ShowSelector();
            if (selecteddriverid > 0)
            {
                ssprocessselectords.driver.Rows.Clear();
                commandtext = "SELECT * FROM driver where idcol = @idcol";
                ClearParameters();
                AddParms("@idcol", selecteddriverid, "SQL");
                FillData(ssprocessselectords, "driver", commandtext, CommandType.Text);
                selecteddrivernumber = ssprocessselectords.driver[0].number;
            }
            return selecteddrivernumber;
        }
        public int SelectUser()
        {
            sysdatads.appuser.Rows.Clear();
            string commandtext = "SELECT * FROM appuser ORDER BY username";
            ClearParameters();
            FillData(sysdatads, "appuser", commandtext, CommandType.Text);
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            frmSelectorMethods.FormText = "Select User";
            frmSelectorMethods.dtSource = sysdatads.appuser;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Useridcol";
            frmSelectorMethods.colheadertext[0] = "User ID";
            frmSelectorMethods.coldatapropertyname[0] = "userid";
            frmSelectorMethods.colwidth[0] = 60;
            frmSelectorMethods.colname[1] = "Usernamecol";
            frmSelectorMethods.colheadertext[1] = "User Name";
            frmSelectorMethods.coldatapropertyname[1] = "username";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.SetGrid();
            int UseridCol = frmSelectorMethods.ShowSelector();
            return UseridCol;
        }
        public int SelectUserrole()
        {
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            sysdatads.approle.Rows.Clear();
            string commandtext = "SELECT * FROM approle ORDER BY userrole";
            ClearParameters();
            FillData(sysdatads, "approle", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select User Role";
            frmSelectorMethods.dtSource = sysdatads.approle;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Userrolecol";
            frmSelectorMethods.colheadertext[0] = "Role";
            frmSelectorMethods.coldatapropertyname[0] = "userrole";
            frmSelectorMethods.colwidth[0] = 60;
            frmSelectorMethods.colname[1] = "Descripcol";
            frmSelectorMethods.colheadertext[1] = "Desciptions";
            frmSelectorMethods.coldatapropertyname[1] = "descrip";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 700;

            int selectedid = frmSelectorMethods.ShowSelector();
            return selectedid;

        }

        private void DecimalToCurrencyString(object sender, ConvertEventArgs cevent)
        {
            // The method converts only to string type. Test this using the DesiredType.
            if (cevent.DesiredType != typeof(string)) return;

            // Use the ToString method to format the value as currency ("c").
            cevent.Value = ((decimal)cevent.Value).ToString("N2");
        }


        private void CurrencyStringToDecimal(object sender, ConvertEventArgs cevent)
        {
            // The method converts back to decimal type only. 
            if (cevent.DesiredType != typeof(decimal)) return;

            // Converts the string back to decimal using the static Parse method.
            try
            {
                cevent.Value = Decimal.Parse(cevent.Value.ToString(),
                  NumberStyles.Currency, null);
            }
            catch
            {
                cevent.Value = 0.00;
            }

        }

        public void SetTextBoxCurrencyBinding(TextBox txtbox, DataTable dt, string fieldname)
        {

            Binding b = new Binding("Text", dt, fieldname);
            {
                b.Format += new ConvertEventHandler(DecimalToCurrencyString);
                b.Parse += new ConvertEventHandler(CurrencyStringToDecimal);
            }
            txtbox.DataBindings.Clear();
            txtbox.DataBindings.Add(b);

        }


        public string SelectDropStore(string driver, string dropfunder)
        {
            string selectedstore = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.view_coinroute.Rows.Clear();
            string commandtext = "";
            commandtext = "SELECT * FROM view_coinroute WHERE driver_1 = @driver ";

            // Rapid Armored Stores begin with 4600
            switch (dropfunder)
            {
                case "Signature":
                    {
                        commandtext += " AND LEFT(store,4) <> '4600'  AND LEFT(store,4) <> '4600' AND store IN ";

                        break;
                    }
                case "Rapid":
                    {
                        commandtext += " AND LEFT(store,4) = '4600' AND store NOT IN ";

                        break;
                    }
                case "SafeAndSound":
                    {
                        commandtext += " AND LEFT(store,4) <> '4600'  AND LEFT(store,4) <> '4075' AND store NOT IN ";

                        break;
                    }
                case "FEGS":
                    {
                        commandtext += " AND LEFT(store,4) = '4075' AND store NOT IN ";

                        break;
                    }
            }


            commandtext += "(SELECT s.storecode from  store s INNER JOIN company c ON  LEFT(s.storecode,4)  = c.comp_code ";
            commandtext += "WHERE s.sigcoin = 'Y' AND c.bankfedid = '8923847' and RTRIM(c.account) <> '') ";
            commandtext += " ORDER BY store_name";
            ClearParameters();
            AddParms("@driver", driver, "SQL");
            FillData(ssprocessselectords, "view_coinroute", commandtext, CommandType.Text);
            if (ssprocessselectords.view_coinroute.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Location";
                frmSelectorMethods.dtSource = ssprocessselectords.view_coinroute;
                frmSelectorMethods.columncount = 2;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "storecode";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "store";
                frmSelectorMethods.colwidth[0] = 100;
                frmSelectorMethods.colname[1] = "storename";
                frmSelectorMethods.colheadertext[1] = "Store Name";
                frmSelectorMethods.coldatapropertyname[1] = "storelocation";
                frmSelectorMethods.colwidth[1] = 300;
                frmSelectorMethods.SetGridWithFormatting(StoredataGridViewSelector_CellFormatting);
                frmSelector.Width = 750;

                int selectedstoreid = frmSelectorMethods.ShowSelector();
                if (selectedstoreid > 0)
                {
                    ssprocessselectords.store.Rows.Clear();
                    ssprocessselectords.store.Rows.Clear();
                    commandtext = "SELECT * FROM store where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedstoreid, "SQL");
                    FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
                    selectedstore = ssprocessselectords.store[0].storecode;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no stores for this driver");
            }
            return selectedstore;
        }
        public string SelectSalePerson()
        {
            string selectedsalesperson = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.salesperson.Rows.Clear();
            string commandtext = "SELECT * from salesperson  ORDER BY slscode";
            FillData(ssprocessselectords, "salesperson", commandtext, CommandType.Text);
            if (ssprocessselectords.salesperson.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Salesperson";
                frmSelectorMethods.dtSource = ssprocessselectords.salesperson;
                frmSelectorMethods.columncount = 2;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "code";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "slscode";
                frmSelectorMethods.colwidth[0] = 300;
                frmSelectorMethods.colname[1] = "name";
                frmSelectorMethods.colheadertext[1] = "Name";
                frmSelectorMethods.coldatapropertyname[1] = "slsname";
                frmSelectorMethods.colwidth[1] = 100;
                frmSelector.Width = 600;
                frmSelectorMethods.SetGrid();
                int selectedsalespersonid = frmSelectorMethods.ShowSelector();
                if (selectedsalespersonid > 0)
                {
                    ssprocessselectords.salesperson.Rows.Clear();
                    commandtext = "SELECT * FROM salesperson  where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedsalespersonid, "SQL");
                    FillData(ssprocessselectords, "salesperson", commandtext, CommandType.Text);
                    selectedsalesperson = ssprocessselectords.salesperson[0].slscode;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no salespeoples");
            }
            return selectedsalesperson;
        }

        public decimal gettaxarea(string city, string state, string zip)
        {
            decimal taxarea = 1;
            switch (state)
            {
                case "NJ":
                    {
                        taxarea = 5;
                        break;
                    }
                case "CT":
                    {
                        taxarea = 6;
                        break;
                    }
                default:
                    {
                        if (zip.TrimEnd().TrimStart() != "")
                        {
                            ssprocessselectords.zipdata.Rows.Clear();
                            string commandtext = "SELECT * FROM zipdata  where zipcode = @zipcode";
                            ClearParameters();
                            AddParms("@zipcode", zip, "SQL");
                            FillData(ssprocessselectords, "zipdata", commandtext, CommandType.Text);
                            if (ssprocessselectords.zipdata.Rows.Count > 0)
                            {
                                switch (ssprocessselectords.zipdata[0].county.Substring(0, 4).ToUpper())
                                {
                                    case "NASS":
                                        {
                                            taxarea = 2;
                                            break;
                                        }
                                    case "SUFF":
                                        {
                                            taxarea = 3;
                                            break;
                                        }

                                }
                            }
                        }
                        break;
                    }

            }
            return taxarea;

        }

        public string GetSalespersonname(string slscode)
        {
            string salespersonname = "";
            ssprocessselectords.salesperson.Rows.Clear();
            string commandtext = "SELECT * FROM salesperson  where slscode = @slscode";
            ClearParameters();
            AddParms("@slscode", slscode, "SQL");
            FillData(ssprocessselectords, "salesperson", commandtext, CommandType.Text);
            if (ssprocessselectords.salesperson.Rows.Count > 0)
            {
                salespersonname = ssprocessselectords.salesperson[0].slsname;
            }
            else
            {
                salespersonname = "Unknown";
            }
            return salespersonname;
        }

        public string GetChargeName(string chgcode)
        {
            string chgdesc = "";
            ClearParameters();
            AddParms("@chgcode", chgcode, "SQL");

            string commandtext = "SELECT * from chargetype  where chgcode =@chgcode";
            FillData(ssprocessselectords, "chargetype", commandtext, CommandType.Text);
            if (ssprocessselectords.chargetype.Rows.Count > 0)
            {
                chgdesc = ssprocessselectords.chargetype[0].chgdesc;
            }
            else
            {
                chgdesc = "Unkown";
            }
            return chgdesc;
        }

        public string SelectChargeType()
        {
            string selectedchargetype = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.chargetype.Rows.Clear();
            string commandtext = "SELECT * from chargetype  ORDER BY chgcode";
            FillData(ssprocessselectords, "chargetype", commandtext, CommandType.Text);
            if (ssprocessselectords.chargetype.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Charge Type";
                frmSelectorMethods.dtSource = ssprocessselectords.chargetype;
                frmSelectorMethods.columncount = 2;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "chgcode";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "chgcode";
                frmSelectorMethods.colwidth[0] = 100;
                frmSelectorMethods.colname[1] = "desc";
                frmSelectorMethods.colheadertext[1] = "Description";
                frmSelectorMethods.coldatapropertyname[1] = "chgdesc";
                frmSelectorMethods.colwidth[1] = 200;
                frmSelector.Width = 600;
                frmSelectorMethods.SetGrid();
                int chargetypeid = frmSelectorMethods.ShowSelector();
                if (chargetypeid > 0)
                {
                    ssprocessselectords.chargetype.Rows.Clear();
                    commandtext = "SELECT * FROM chargetype  where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", chargetypeid, "SQL");
                    FillData(ssprocessselectords, "chargetype", commandtext, CommandType.Text);
                    selectedchargetype = ssprocessselectords.chargetype[0].chgcode;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no charge types");
            }
            return selectedchargetype;
        }


        public string SelectCompany()
        {
            string selectedcompany = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.company.Rows.Clear();
            string commandtext = "SELECT * from company  ORDER BY comp_code";
            FillData(ssprocessselectords, "company", commandtext, CommandType.Text);
            if (ssprocessselectords.company.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Customer";
                frmSelectorMethods.dtSource = ssprocessselectords.company;
                frmSelectorMethods.columncount = 3;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "comp_code";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "comp_code";
                frmSelectorMethods.colwidth[0] = 75;
                frmSelectorMethods.colname[1] = "name";
                frmSelectorMethods.colheadertext[1] = "Customer";
                frmSelectorMethods.coldatapropertyname[1] = "name";
                frmSelectorMethods.colwidth[1] = 300;
                frmSelectorMethods.colname[2] = "address";
                frmSelectorMethods.colheadertext[2] = "Address";
                frmSelectorMethods.coldatapropertyname[2] = "address";
                frmSelectorMethods.colwidth[2] = 250;
                frmSelector.Width = 850;
                frmSelectorMethods.SetGrid();
                int selectedcompanyid = frmSelectorMethods.ShowSelector();
                if (selectedcompanyid > 0)
                {
                    ssprocessselectords.company.Rows.Clear();
                    commandtext = "SELECT * FROM company  where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedcompanyid, "SQL");
                    FillData(ssprocessselectords, "company", commandtext, CommandType.Text);
                    selectedcompany = ssprocessselectords.company[0].comp_code;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no customers");
            }
            return selectedcompany;
        }

        public string SelectActiveCompany()
        {
            string selectedcompany = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.company.Rows.Clear();
            string commandtext = "SELECT * from company  WHERE inactive = 0  ORDER BY name";
            FillData(ssprocessselectords, "company", commandtext, CommandType.Text);
            if (ssprocessselectords.company.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Customer";
                frmSelectorMethods.dtSource = ssprocessselectords.company;
                frmSelectorMethods.columncount = 3;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "comp_code";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "comp_code";
                frmSelectorMethods.colwidth[0] = 100;
                frmSelectorMethods.colname[1] = "name";
                frmSelectorMethods.colheadertext[1] = "Customer";
                frmSelectorMethods.coldatapropertyname[1] = "name";
                frmSelectorMethods.colwidth[1] = 300;
                frmSelector.Width = 600;
                frmSelectorMethods.SetGrid();
                int selectedcompanyid = frmSelectorMethods.ShowSelector();
                if (selectedcompanyid > 0)
                {
                    ssprocessselectords.company.Rows.Clear();
                    commandtext = "SELECT * FROM company  where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedcompanyid, "SQL");
                    FillData(ssprocessselectords, "company", commandtext, CommandType.Text);
                    selectedcompany = ssprocessselectords.company[0].comp_code;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no customers");
            }
            return selectedcompany;
        }

        public string SelectMoneyCenterCompany()
        {
            string selectedcompany = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.company.Rows.Clear();
            string commandtext = "SELECT * from company a WHERE EXISTS ";
            commandtext += " (SELECT storecode FROM store b WHERE active = 1  AND LEFT(b.storecode,4) = a.comp_code) ";
            commandtext += " ORDER BY a.name";
            FillData(ssprocessselectords, "company", commandtext, CommandType.Text);
            if (ssprocessselectords.company.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Customer";
                frmSelectorMethods.dtSource = ssprocessselectords.company;
                frmSelectorMethods.columncount = 3;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "name";
                frmSelectorMethods.colheadertext[0] = "Customer";
                frmSelectorMethods.coldatapropertyname[0] = "name";
                frmSelectorMethods.colwidth[0] = 300;
                frmSelectorMethods.colname[1] = "comp_code";
                frmSelectorMethods.colheadertext[1] = "Code";
                frmSelectorMethods.coldatapropertyname[1] = "comp_code";
                frmSelectorMethods.colwidth[1] = 100;
                frmSelector.Width = 600;
                frmSelectorMethods.SetGridWithFormatting(CompanydataGridViewSelector_CellFormatting);
                int selectedcompanyid = frmSelectorMethods.ShowSelector();
                if (selectedcompanyid > 0)
                {
                    ssprocessselectords.company.Rows.Clear();
                    commandtext = "SELECT * FROM company  where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedcompanyid, "SQL");
                    FillData(ssprocessselectords, "company", commandtext, CommandType.Text);
                    selectedcompany = ssprocessselectords.company[0].comp_code;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no customers");
            }
            return selectedcompany;
        }

        public string SelectMoneyCenterStore(string SearchType, string SearchArgument)
        {
            string selectedstore = "";
            string commandtext = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.store.Rows.Clear();
            if (SearchType == "Company")
            {
                commandtext = "SELECT * FROM  store WHERE LEFT(storecode,4) = @companycode AND active = 1 ORDER BY store_name ";
                AddParms("@companycode", SearchArgument, "SQL");
            }
            else
            {
                commandtext = "SELECT * FROM store WHERE active = 1 AND storecode IN (SELECT storecode FROM dropschedule WHERE driver_1  = @driver) order by storecode";
                ClearParameters();
                AddParms("@driver", SearchArgument, "SQL");
            }
            FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
            if (ssprocessselectords.store.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Location";
                frmSelectorMethods.dtSource = ssprocessselectords.store;
                frmSelectorMethods.columncount = 3;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "storecode";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "storecode";
                frmSelectorMethods.colwidth[0] = 100;
                frmSelectorMethods.colname[1] = "storename";
                frmSelectorMethods.colheadertext[1] = "Store Name";
                frmSelectorMethods.coldatapropertyname[1] = "store_name";
                frmSelectorMethods.colwidth[1] = 300;
                frmSelectorMethods.colname[2] = "address";
                frmSelectorMethods.colheadertext[2] = "Store Address";
                frmSelectorMethods.coldatapropertyname[2] = "f_address";
                frmSelectorMethods.colwidth[2] = 300;
                frmSelector.Width = 1000;
                frmSelectorMethods.SetGridWithFormatting(StoreMoneyCenter_CellFormatting);
                frmSelectorMethods.SetGrid();
                int selectedstoreid = frmSelectorMethods.ShowSelector();
                if (selectedstoreid > 0)
                {
                    ssprocessselectords.store.Rows.Clear();
                    ssprocessselectords.store.Rows.Clear();
                    commandtext = "SELECT * FROM store where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedstoreid, "SQL");
                    FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
                    selectedstore = ssprocessselectords.store[0].storecode;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no stores for this customer");
            }
            return selectedstore;
        }

        public string SelectCompanyAndStore()
        {
            StoreSelectorMethods storeSelectorMethods = new StoreSelectorMethods();
            string comp_code = "";
            string storecode = "";
            comp_code = SelectActiveCompany();
            if (comp_code != "")
            {

                storecode = storeSelectorMethods.SelectStore(comp_code);
            }

            return storecode;

        }

        public string SelectStore(string companycode)
        {
            string selectedstore = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            frmSelectorMethods.FormText = "Stores for " + GetCompanyName(companycode);
            ssprocessselectords.store.Rows.Clear();
            string commandtext = "SELECT  RIGHT(LEFT(storecode,11),6) AS storecode, store_name, f_address, idcol FROM  store WHERE LEFT(storecode,4) = @companycode AND active = 1 ORDER BY store_name ";
            ClearParameters();
            AddParms("@companycode", companycode, "SQL");

            FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
            if (ssprocessselectords.store.Rows.Count > 0)
            {
                frmSelectorMethods.dtSource = ssprocessselectords.store;
                frmSelectorMethods.columncount = 3;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "storecode";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "storecode";
                frmSelectorMethods.colwidth[0] = 100;
                frmSelectorMethods.colname[1] = "storename";
                frmSelectorMethods.colheadertext[1] = "Store Name";
                frmSelectorMethods.coldatapropertyname[1] = "store_name";
                frmSelectorMethods.colwidth[1] = 300;
                frmSelectorMethods.colname[2] = "address";
                frmSelectorMethods.colheadertext[2] = "Store Address";
                frmSelectorMethods.coldatapropertyname[2] = "f_address";
                frmSelectorMethods.colwidth[2] = 300;
                frmSelector.Width = 1000;
                frmSelectorMethods.SetGrid();
                int selectedstoreid = frmSelectorMethods.ShowSelector();
                if (selectedstoreid > 0)
                {
                    ssprocessselectords.store.Rows.Clear();
                    ssprocessselectords.store.Rows.Clear();
                    commandtext = "SELECT * FROM store where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedstoreid, "SQL");
                    FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
                    selectedstore = ssprocessselectords.store[0].storecode;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no stores for this customer");
            }
            return selectedstore;
        }

        public int SelectSpecialCharge()
        {
            int selectedcharge = 0;
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            frmSelectorMethods.FormText = "Special Charges";
            ssprocessselectords.view_expandedspecialcharge.Rows.Clear();
            string commandtext = "SELECT * from view_expandedspecialcharge order by chgdate desc";
            ClearParameters();

            FillData(ssprocessselectords, "view_expandedspecialcharge", commandtext, CommandType.Text);
            if (ssprocessselectords.view_expandedspecialcharge.Rows.Count > 0)
            {
                frmSelectorMethods.dtSource = ssprocessselectords.view_expandedspecialcharge;
                frmSelectorMethods.columncount = 5;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "storecode";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "storecode";
                frmSelectorMethods.colwidth[0] = 100;
                frmSelectorMethods.colname[1] = "storename";
                frmSelectorMethods.colheadertext[1] = "Store Name";
                frmSelectorMethods.coldatapropertyname[1] = "storename";
                frmSelectorMethods.colwidth[1] = 300;
                frmSelectorMethods.colname[2] = "chargename";
                frmSelectorMethods.colheadertext[2] = "Charge";
                frmSelectorMethods.coldatapropertyname[2] = "chargename";
                frmSelectorMethods.colwidth[2] = 200;
                frmSelectorMethods.colname[3] = "chgdate";
                frmSelectorMethods.colheadertext[3] = "Charge Date";
                frmSelectorMethods.coldatapropertyname[3] = "chgdate";
                frmSelectorMethods.coldefaultcellstyle[3] = "MM/dd/yyyy";
                frmSelectorMethods.colwidth[3] = 100;
                frmSelectorMethods.colname[4] = "chgamt";
                frmSelectorMethods.colheadertext[4] = "Amount";
                frmSelectorMethods.coldatapropertyname[4] = "chgamt";
                frmSelectorMethods.colwidth[4] = 100;
                frmSelectorMethods.coldefaultcellstyle[4] = "###,###.00";
                frmSelector.Width = 900;
                frmSelectorMethods.SetGrid();
                selectedcharge = frmSelectorMethods.ShowSelector();
            }
            else
            {
                wsgUtilities.wsgNotice("There are no special charges");
            }
            return selectedcharge;
        }


        public string SelectDriverStore(string driver)
        {
            string selectedstore = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.store.Rows.Clear();
            string commandtext = "SELECT * FROM store WHERE active = 1 AND storecode IN (SELECT storecode FROM dropschedule WHERE driver_1  = @driver) order by storecode";
            ClearParameters();
            AddParms("@driver", driver, "SQL");

            FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
            if (ssprocessselectords.store.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Location";
                frmSelectorMethods.dtSource = ssprocessselectords.store;
                frmSelectorMethods.columncount = 3;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "storecode";
                frmSelectorMethods.colheadertext[0] = "Code";
                frmSelectorMethods.coldatapropertyname[0] = "storecode";
                frmSelectorMethods.colwidth[0] = 100;
                frmSelectorMethods.colname[1] = "storename";
                frmSelectorMethods.colheadertext[1] = "Store Name";
                frmSelectorMethods.coldatapropertyname[1] = "store_name";
                frmSelectorMethods.colwidth[1] = 300;
                frmSelectorMethods.colname[2] = "address";
                frmSelectorMethods.colheadertext[2] = "Store Address";
                frmSelectorMethods.coldatapropertyname[2] = "f_address";
                frmSelectorMethods.colwidth[2] = 300;
                frmSelector.Width = 1000;
                frmSelectorMethods.SetGrid();
                int selectedstoreid = frmSelectorMethods.ShowSelector();
                if (selectedstoreid > 0)
                {
                    ssprocessselectords.store.Rows.Clear();
                    ssprocessselectords.store.Rows.Clear();
                    commandtext = "SELECT * FROM store where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedstoreid, "SQL");
                    FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
                    selectedstore = ssprocessselectords.store[0].storecode;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no stores for this customer");
            }
            return selectedstore;
        }

        bool CompanyHasMoneyCenter(string comp_code)
        {
            bool hasmoneycenter = false;
            ssprocess checkcoinds = new ssprocess();
            ssprocessds.compmoneycenter.Rows.Clear();
            checkcoinds.coindrop.Rows.Clear();
            string commandtext = "SELECT * FROM compmoneycenter WHERE comp_code = @comp_code";
            ClearParameters();
            AddParms("@comp_code", comp_code, "SQL");
            FillData(ssprocessds, "compmoneycenter", commandtext, CommandType.Text);
            if (ssprocessds.compmoneycenter.Rows.Count > 0)
            {
                hasmoneycenter = true;
            }
            return hasmoneycenter;
        }

        public bool StoreHasMoneyCenter(string store)
        {

            ssprocess checkstoreds = new ssprocess();
            bool hasmoneycenter = false;
            checkstoreds.storemoneycenter.Rows.Clear();
            store = store.Substring(0, 11);
            string commandtext = "SELECT * FROM storemoneycenter WHERE LEFT(storecode,11) = @store";
            ClearParameters();
            AddParms("@store", store, "SQL");
            FillData(checkstoreds, "storemoneycenter", commandtext, CommandType.Text);
            if (checkstoreds.storemoneycenter.Rows.Count > 0)
            {
                hasmoneycenter = true;
            }
            return hasmoneycenter;
        }




        public bool StoreHasCoindrop(string store)
        {
            bool hascoin = false;
            ssprocess checkcoinds = new ssprocess();
            checkcoinds.coindrop.Rows.Clear();
            store = store.Substring(0, 11);
            string commandtext = "SELECT * FROM coindrop WHERE LEFT(store,11) = @store AND dropdate = @dropdate";
            ClearParameters();
            AddParms("@store", store, "SQL");
            AddParms("@dropdate", SelectedDate, "SQL");
            FillData(checkcoinds, "coindrop", commandtext, CommandType.Text);
            if (checkcoinds.coindrop.Rows.Count > 0)
            {
                hascoin = true;
            }
            return hascoin;
        }
        private void StoredataGridViewSelector_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.ColumnIndex == 0)
                {
                    if (StoreHasCoindrop((string)e.Value))
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.SelectionBackColor = Color.DarkRed;
                    }
                }
            }
        }
        private void StoreMoneyCenter_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                if (e.ColumnIndex == 0)
                {
                    if (StoreHasMoneyCenter((string)e.Value))
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.SelectionBackColor = Color.DarkRed;
                    }
                }
            }
        }
        private void CompanydataGridViewSelector_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.Value != null)
            {
                if (e.ColumnIndex == 1)
                {
                    if (CompanyHasMoneyCenter((string)e.Value))
                    {
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.SelectionBackColor = Color.DarkRed;
                    }
                }
            }
        }

        public int SelectAppProcess()
        {
            sysdata sysdataselectords = new sysdata();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            sysdataselectords.appprocess.Rows.Clear();
            string commandtext = "SELECT * FROM appprocess ORDER BY process";
            ;
            ClearParameters();
            FillData(sysdataselectords, "appprocess", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Process";
            frmSelectorMethods.dtSource = sysdataselectords.appprocess;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Processcol";
            frmSelectorMethods.colheadertext[0] = "Process";
            frmSelectorMethods.coldatapropertyname[0] = "process";
            frmSelectorMethods.colwidth[0] = 100;
            frmSelectorMethods.colname[1] = "Descripcol";
            frmSelectorMethods.colheadertext[1] = "Description";
            frmSelectorMethods.coldatapropertyname[1] = "descrip";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 700;
            int selectedProcessid = frmSelectorMethods.ShowSelector();
            return selectedProcessid;
        }
        public int SelectAppriv()
        {
            sysdatads.apppriv.Rows.Clear();
            string commandtext = "SELECT * FROM apppriv ORDER BY userrole,process";
            ClearParameters();
            FillData(sysdatads, "apppriv", commandtext, CommandType.Text);
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            frmSelectorMethods.FormText = "Select Privilege";
            frmSelectorMethods.dtSource = sysdatads.apppriv;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Rolecol";
            frmSelectorMethods.colheadertext[0] = "Role";
            frmSelectorMethods.coldatapropertyname[0] = "userrole";
            frmSelectorMethods.colwidth[0] = 70;
            frmSelectorMethods.colname[1] = "Processcol";
            frmSelectorMethods.colheadertext[1] = "Process";
            frmSelectorMethods.coldatapropertyname[1] = "process";
            frmSelectorMethods.colwidth[1] = 70;
            frmSelectorMethods.SetGrid();
            int selectedid = frmSelectorMethods.ShowSelector();
            return selectedid;
        }
        public int SelectMoneyCenter()
        {
            ssprocess ssprocessselectords = new ssprocess();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.moneycenter.Rows.Clear();
            string commandtext = "SELECT * FROM moneycenter ORDER BY centername";
            ClearParameters();
            FillData(ssprocessselectords, "moneycenter", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Money Center";
            frmSelectorMethods.dtSource = ssprocessselectords.moneycenter;
            frmSelectorMethods.columncount = 1;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Centernamecol";
            frmSelectorMethods.colheadertext[0] = "Center Name";
            frmSelectorMethods.coldatapropertyname[0] = "centername";
            frmSelectorMethods.colwidth[0] = 250;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 400;
            int selectedMoneycenterid = frmSelectorMethods.ShowSelector();
            return selectedMoneycenterid;
        }

        public int SelectCompanyMoneyCenter(string comp_code)
        {
            int selectedMoneycenterid = 0;
            ssprocess ssprocessselectords = new ssprocess();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.moneycenter.Rows.Clear();
            string commandtext = "SELECT *  FROM moneycenter WHERE idcol IN ";
            commandtext += "(SELECT moneycenterid FROM compmoneycenter WHERE comp_code = @comp_code)";
            ClearParameters();
            AddParms("@comp_code", comp_code, "SQL");
            FillData(ssprocessselectords, "moneycenter", commandtext, CommandType.Text);
            if (ssprocessselectords.moneycenter.Rows.Count < 1)
            {
                selectedMoneycenterid = -1;
            }
            else
                if (ssprocessselectords.moneycenter.Rows.Count == 1)
                {
                    selectedMoneycenterid = ssprocessselectords.moneycenter[0].idcol;
                }
                else
                {
                    frmSelectorMethods.FormText = "Select Money Center";
                    frmSelectorMethods.dtSource = ssprocessselectords.moneycenter;
                    frmSelectorMethods.columncount = 1;
                    frmSelectorMethods.SetColumns();
                    frmSelectorMethods.colname[0] = "Centernamecol";
                    frmSelectorMethods.colheadertext[0] = "Center Name";
                    frmSelectorMethods.coldatapropertyname[0] = "centername";
                    frmSelectorMethods.colwidth[0] = 250;
                    frmSelectorMethods.SetGrid();
                    frmSelector.Width = 400;
                    selectedMoneycenterid = frmSelectorMethods.ShowSelector();
                }
            return selectedMoneycenterid;
        }


        public string GetMoneyCenterName(int idcol)
        {
            string moneycentername = "";
            String CommandText = "SELECT * FROM moneycenter WHERE idcol  = @idcol";
            ssprocessselectords.moneycenter.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", idcol, "SQL");
            FillData(ssprocessselectords, "moneycenter", CommandText, CommandType.Text);
            if (ssprocessselectords.moneycenter.Rows.Count > 0)
            {
                moneycentername = ssprocessselectords.moneycenter[0].centername.TrimEnd();
            }
            return moneycentername;
        }

        public string SelectSmartsafeBankFedid()
        {
            string Bankfedid = "";
            int bankid = SelectSmartsafeBank();
            if (bankid > 0)
            {
                ssprocessds.bank.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", bankid, "SQL");
                FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
                Bankfedid = ssprocessds.bank[0].bankfedid;
            }
            return Bankfedid;

        }

        public string SelectBankFedid()
        {
            string Bankfedid = "";
            int bankid = SelectBank();
            if (bankid > 0)
            {
                ssprocessds.bank.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", bankid, "SQL");
                FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
                Bankfedid = ssprocessds.bank[0].bankfedid;
            }
            return Bankfedid;

        }

        public string[] GetCustomerData(string storecode)
        {
            string[] customerdata = new string[2];
            customerdata[0] = "";
            customerdata[1] = "";
            String CommandText = "SELECT * FROM acstdt01  WHERE LEFT(storecode,11) = @storecode";
            ClearParameters();
            AddParms("@storecode", storecode.Substring(0, 11), "SQL");
            ssprocessds.acstdt01.Rows.Clear();
            FillData(ssprocessds, "acstdt01", CommandText, CommandType.Text);
            if (ssprocessds.acstdt01.Rows.Count > 0)
            {
                if (ssprocessds.acstdt01[0].account.TrimEnd() != "")
                {
                    customerdata[0] = ssprocessds.acstdt01[0].account;
                    customerdata[1] = ssprocessds.acstdt01[0].bankno;
                }
            }

            if (customerdata[0].TrimEnd() == "")
            {
                CommandText = "SELECT * FROM company  WHERE comp_code = @compcode";
                ClearParameters();
                AddParms("@compcode", storecode.Substring(0, 4), "SQL");
                ssprocessds.company.Rows.Clear();
                FillData(ssprocessds, "company", CommandText, CommandType.Text);
                if (ssprocessds.company.Rows.Count > 0)
                {
                    customerdata[0] = ssprocessds.company[0].account;
                    customerdata[1] = ssprocessds.company[0].bankno;
                }
            }
            return customerdata;
        }
        public string GetBankFedIdFromBankId(int BankId)
        {
            string BankFedId = "";
            ssprocessds.bank.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", BankId, "SQL");
            FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
            BankFedId = ssprocessds.bank[0].bankfedid;
            return BankFedId;

        }
        public string GetBankNameFromBankId(int BankId)
        {
            string BankName = "";
            ssprocessds.bank.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", BankId, "SQL");
            FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
            BankName = ssprocessds.bank[0].name;
            return BankName;

        }
        public string SelectManifestCompany()
        {
            string selectedcompany = "";
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.company.Rows.Clear();
            string commandtext = "SELECT comp_code, name, idcol from view_manifestcompany ORDER BY name ";

            FillData(ssprocessselectords, "company", commandtext, CommandType.Text);
            if (ssprocessselectords.company.Rows.Count > 0)
            {
                frmSelectorMethods.FormText = "Select Customer";
                frmSelectorMethods.dtSource = ssprocessselectords.company;
                frmSelectorMethods.columncount = 3;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "name";
                frmSelectorMethods.colheadertext[0] = "Customer";
                frmSelectorMethods.coldatapropertyname[0] = "name";
                frmSelectorMethods.colwidth[0] = 300;
                frmSelectorMethods.colname[1] = "comp_code";
                frmSelectorMethods.colheadertext[1] = "Code";
                frmSelectorMethods.coldatapropertyname[1] = "comp_code";
                frmSelectorMethods.colwidth[1] = 100;
                frmSelector.Width = 600;
                frmSelectorMethods.SetGrid();
                int selectedcompanyid = frmSelectorMethods.ShowSelector();
                if (selectedcompanyid > 0)
                {
                    ssprocessselectords.company.Rows.Clear();
                    commandtext = "SELECT * FROM company  where idcol = @idcol";
                    ClearParameters();
                    AddParms("@idcol", selectedcompanyid, "SQL");
                    FillData(ssprocessselectords, "company", commandtext, CommandType.Text);
                    selectedcompany = ssprocessselectords.company[0].comp_code;
                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are no customers");
            }
            return selectedcompany;
        }


        public int SelectSmartsafeBank()
        {
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.bank.Rows.Clear();
            string commandtext = "SELECT * FROM bank WHERE smartsafe = 'Y'";
            ClearParameters();
            FillData(ssprocessselectords, "bank", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Bank";
            frmSelectorMethods.dtSource = ssprocessselectords.bank;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "BankfedIdcol";
            frmSelectorMethods.colheadertext[0] = "Bank Fed Id";
            frmSelectorMethods.coldatapropertyname[0] = "bankfedid";
            frmSelectorMethods.colwidth[0] = 300;
            frmSelectorMethods.colname[1] = "Namecol";
            frmSelectorMethods.colheadertext[1] = "Bank Name";
            frmSelectorMethods.coldatapropertyname[1] = "name";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 700;
            int selectedbankid = frmSelectorMethods.ShowSelector();
            return selectedbankid;
        }

        public int SelectBank()
        {
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.bank.Rows.Clear();
            string commandtext = "SELECT * FROM bank ORDER BY bankfedid";
            ClearParameters();
            FillData(ssprocessselectords, "bank", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Bank";
            frmSelectorMethods.dtSource = ssprocessselectords.bank;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "BankfedIdcol";
            frmSelectorMethods.colheadertext[0] = "Bank Fed Id";
            frmSelectorMethods.coldatapropertyname[0] = "bankfedid";
            frmSelectorMethods.colwidth[0] = 300;
            frmSelectorMethods.colname[1] = "Namecol";
            frmSelectorMethods.colheadertext[1] = "Bank Name";
            frmSelectorMethods.coldatapropertyname[1] = "name";
            frmSelectorMethods.colwidth[1] = 300;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 700;
            int selectedbankid = frmSelectorMethods.ShowSelector();
            return selectedbankid;
        }

        public DateTime GetLastCloseDate(string bankfedid)
        {
            ssprocess ssprocessselectords = new ssprocess();
            DateTime CloseDate = DateTime.Now.Date;
            String CommandText = "SELECT TOP 1 * FROM balance WHERE bankfedid = @bankfedid ORDER BY postdate DESC";
            ssprocessselectords.balance.Rows.Clear();
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(ssprocessselectords, "balance", CommandText, CommandType.Text);
            if (ssprocessselectords.balance.Rows.Count > 0)
            {
                CloseDate = ssprocessselectords.balance[0].postdate.Date;
            }
            return CloseDate.Date;
        }
        public DateTime GetNextPostDate(string bankfedid)
        {
            ssprocess ssprocessselectords = new ssprocess();
            DateTime NextDate = DateTime.Now.Date;
            String CommandText = "SELECT TOP 1 * FROM balance WHERE bankfedid = @bankfedid ORDER BY postdate DESC";
            ssprocessselectords.balance.Rows.Clear();
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(ssprocessselectords, "balance", CommandText, CommandType.Text);
            if (ssprocessselectords.balance.Rows.Count > 0)
            {
                NextDate = ssprocessselectords.balance[0].postdate.Date.AddDays(1);
            }
            return NextDate.Date;
        }

        public bool GetTwoDates(string labeltext)
        {
            FrmGetTwoDates frmGetTwoDates = new FrmGetTwoDates();
            frmGetTwoDates.LabelDateText.Text = labeltext;
            frmGetTwoDates.ShowDialog();
            SelectedStartDate = frmGetTwoDates.SelectedStartDate.Date;
            SelectedEndDate = frmGetTwoDates.SelectedEndDate.Date;
            return frmGetTwoDates.DateOk;
        }
        public bool GetSingleDate(string labeltext, int minlimit, int maxlimit)
        {
            FrmGetDate frmGetDate = new FrmGetDate();
            frmGetDate.LabelDateText.Text = labeltext;
            if (minlimit != null)
            {
                frmGetDate.dateTimePicker1.MinDate = DateTime.Now.Date.AddDays(-minlimit);
            }
            if (maxlimit != null)
            {
                frmGetDate.dateTimePicker1.MaxDate = DateTime.Now.Date.AddDays(maxlimit);
            }

            frmGetDate.ShowDialog();
            SelectedDate = Convert.ToDateTime(frmGetDate.SelectedDate.Date.ToString("MM/dd/yyyy"));
            return frmGetDate.DateOk;
        }
        public string GetRolebyId(int roleid)
        {
            string userrole = "";
            string commandtext = "SELECT * FROM approle WHERE idcol = @idcol";
            sysdatads.approle.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", roleid, "SQL");
            FillData(sysdatads, "approle", commandtext, CommandType.Text);
            userrole = sysdatads.approle[0].userrole;
            return userrole;
        }


        public bool IsRoleAuthorized(string process)
        {

            bool roleok = false;
            if (ConfigurationManager.AppSettings["TestMode"] != "True")
            {

                string commandtext = "";
                commandtext = "SELECT * FROM appuser WHERE userid = @userid";
                sysdatads.appuser.Rows.Clear();
                ClearParameters();
                AddParms("@userid", AppUserClass.AppUserId, "SQL");
                FillData(sysdatads, "appuser", commandtext, CommandType.Text);
                if (sysdatads.appuser.Rows.Count > 0)
                {
                    if (sysdatads.appuser[0].userrole != "SYAD")
                    {
                        commandtext = "SELECT * FROM apppriv WHERE userrole = @userrole AND process = @process";
                        sysdatads.apppriv.Rows.Clear();
                        ClearParameters();
                        AddParms("@userrole", sysdatads.appuser[0].userrole, "SQL");
                        AddParms("@process", process, "SQL");
                        FillData(sysdatads, "apppriv", commandtext, CommandType.Text);
                        if (sysdatads.apppriv.Rows.Count > 0)
                        {
                            roleok = true;
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("User not authorized for this task.");

                        }
                    }
                    else
                    {
                        roleok = true;

                    }
                }
                else
                {
                    wsgUtilities.wsgNotice("Invalid User ID. Access Denied");
                }

            }
            else
            {
                roleok = true;
            }
            return roleok;
        }

        public void CleanUpDataTable(string tablename, string datecolumnname, int years)
        {
            int cutoffyear = DateTime.Now.Date.AddYears(-years).Year;
            string commandtext = "DELETE FROM " + tablename + " WHERE YEAR(" + datecolumnname + ") < @cutoffyear";
            ClearParameters();
            AddParms("@cutoffyear", cutoffyear, "SQL");
            ExecuteCommand(commandtext, CommandType.Text);
        }
        public void SendCompanyEmail(List<string> attachmentnames, string compcode, string mailsubject, string mailtext)
        {
            try
            {

                MailMessage CompanyMessage = new MailMessage();
                // Add recipients
                if (ConfigurationManager.AppSettings["TestMode"] == "True")
                {
                    CompanyMessage.To.Clear();
                    CompanyMessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                    CompanyMessage.Attachments.Clear();
                    foreach (string attachmentname in attachmentnames)
                    {
                        CompanyMessage.Attachments.Add(new System.Net.Mail.Attachment(attachmentname));
                    }
                    // Send the email
                    appUtilities.SendEmail(CompanyMessage, mailsubject, mailtext);
                }
                else
                {
                    ssprocessds.companyemailaddress.Rows.Clear();
                    string commandtext = "SELECT * FROM companyemailaddress WHERE comp_code = @compcode ORDER BY emailaddr";
                    ClearParameters();
                    AddParms("@compcode", compcode, "SQL");
                    FillData(ssprocessds, "companyemailaddress", commandtext, CommandType.Text);
                    if (ssprocessds.companyemailaddress.Rows.Count > 0)
                    {
                        for (int i = 0; i <= ssprocessds.companyemailaddress.Rows.Count - 1; i++)
                        {
                            CompanyMessage.To.Add(new MailAddress(ssprocessds.companyemailaddress[i].emailaddr.TrimEnd()));
                        }
                        // Add Attachements 
                        CompanyMessage.Attachments.Clear();
                        foreach (string attachmentname in attachmentnames)
                        {
                            CompanyMessage.Attachments.Add(new System.Net.Mail.Attachment(attachmentname));
                        }
                        // Send the email
                        appUtilities.SendEmail(CompanyMessage, mailsubject, mailtext);
                    }
                }
            }
            catch (Exception ex)
            {
                appUtilities.logEvent("Email sending failure company " + compcode, "EMFAIL", "Failure", true);
            }
        }

        public void SendSelectedCompanyEmail(string documenttosend, List<string> attachmentnames, string compcode, string mailsubject, string mailtext)
        {
            try
            {

                MailMessage CompanyMessage = new MailMessage();
                // Add recipients
                if (ConfigurationManager.AppSettings["TestMode"] == "True")
                {
                    CompanyMessage.To.Clear();
                    CompanyMessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                    CompanyMessage.Attachments.Clear();
                    foreach (string attachmentname in attachmentnames)
                    {
                        CompanyMessage.Attachments.Add(new System.Net.Mail.Attachment(attachmentname));
                    }
                    // Send the email
                    appUtilities.SendEmail(CompanyMessage, mailsubject, mailtext);
                }
                else
                {
                    ssprocessds.companyemailaddress.Rows.Clear();
                    string commandtext = "SELECT * FROM companyemailaddress WHERE comp_code = @compcode ORDER BY emailaddr";
                    ClearParameters();
                    AddParms("@compcode", compcode, "SQL");
                    FillData(ssprocessds, "companyemailaddress", commandtext, CommandType.Text);
                    
                    // Remove any addresses not specified for the document in question
                    if (ssprocessds.companyemailaddress.Rows.Count > 0)
                    {
                        for (int i = 0; i <= ssprocessds.companyemailaddress.Rows.Count - 1; i++)
                        {

                            switch (documenttosend)
                            {
                                case "Invoice":
                                    {
                                        if (!ssprocessds.companyemailaddress[i].sendinvoice)
                                        {
                                            ssprocessds.companyemailaddress.Rows[i].Delete();
                                        }
                                        break;
                                    }
                                case "Count":
                                    {
                                        if (!ssprocessds.companyemailaddress[i].sendcount)
                                        {
                                            ssprocessds.companyemailaddress.Rows[i].Delete();
                                        }
                                        break;
                                    }
                                case "Pickup":
                                    {
                                        if (!ssprocessds.companyemailaddress[i].sendpickup)
                                        {
                                            ssprocessds.companyemailaddress.Rows[i].Delete();
                                        }
                                        break;
                                    }

                            }
                        }
                    }
                    ssprocessds.companyemailaddress.AcceptChanges();
                    if (ssprocessds.companyemailaddress.Rows.Count > 0)
                    {
                        for (int i = 0; i <= ssprocessds.companyemailaddress.Rows.Count - 1; i++)
                        {
                            CompanyMessage.To.Add(new MailAddress(ssprocessds.companyemailaddress
                            [i].emailaddr.TrimEnd()));
                        }
                        // Add Attachements 
                        CompanyMessage.Attachments.Clear();
                        foreach (string attachmentname in attachmentnames)
                        {
                            CompanyMessage.Attachments.Add(new System.Net.Mail.Attachment(attachmentname));
                        }
                        // Send the email
                        appUtilities.SendEmail(CompanyMessage, mailsubject, mailtext);
                    }
                }
            }
            catch (Exception ex)
            {
                appUtilities.logEvent("Email sending failure company " + compcode, "EMFAIL", "Failure", true);
            }
        }



        public void SendBankEmail(List<string> attachmentnames, string bankfedid, string mailsubject, string mailtext)
        {
            MailMessage BankMessage = new MailMessage();
            if (ConfigurationManager.AppSettings["TestMode"] == "True")
            {
                BankMessage.To.Clear();
                BankMessage.To.Add(ConfigurationManager.AppSettings["TestEmailAddress"]);
                BankMessage.Attachments.Clear();
                foreach (string attachmentname in attachmentnames)
                {
                    BankMessage.Attachments.Add(new System.Net.Mail.Attachment(attachmentname));
                }
                // Send the email
                appUtilities.SendEmail(BankMessage, mailsubject, mailtext);
            }
            else
            {

                // Add recipients
                ssprocessds.bank.Rows.Clear();
                string commandtext = "SELECT * FROM emailbank WHERE bankfedid = @bankfedid ORDER BY emailaddr";
                ClearParameters();
                AddParms("@bankfedid", bankfedid, "SQL");
                FillData(ssprocessds, "emailbank", commandtext, CommandType.Text);
                if (ssprocessds.emailbank.Rows.Count > 0)
                {
                    for (int i = 0; i <= ssprocessds.emailbank.Rows.Count - 1; i++)
                    {
                        BankMessage.To.Add(new MailAddress(ssprocessds.emailbank[i].emailaddr.TrimEnd()));
                    }
                    // Add Attachements 
                    BankMessage.Attachments.Clear();
                    foreach (string attachmentname in attachmentnames)
                    {
                        BankMessage.Attachments.Add(new System.Net.Mail.Attachment(attachmentname));
                    }
                    // Send the email
                    appUtilities.SendEmail(BankMessage, mailsubject, mailtext);
                }
            }
        }

        public String SelectCoinFunder()
        {
            string CoinFunder = "";
            FrmSelectCoinFunder frmSelectCoinFunder = new FrmSelectCoinFunder();
            frmSelectCoinFunder.ShowDialog();
            CoinFunder = frmSelectCoinFunder.CoinFunder;
            return CoinFunder;
        }

        public String GetCompanyName(string compcode)
        {
            string companyname = "";
            String CommandText = "SELECT * FROM company WHERE comp_code  = @compcode";
            ssprocessselectords.company.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode, "SQL");
            FillData(ssprocessselectords, "company", CommandText, CommandType.Text);
            if (ssprocessselectords.company.Rows.Count > 0)
            {
                companyname = ssprocessselectords.company[0].name.TrimEnd();
            }
            return companyname;
        }
        public String GetStoreName(string storecode)
        {
            string storename = "Unknown";
            String CommandText = "SELECT * FROM store WHERE storecode =@storecode";
            ssprocessselectords.store.Rows.Clear();
            ClearParameters();
            AddParms("@storecode", storecode, "SQL");
            FillData(ssprocessselectords, "store", CommandText, CommandType.Text);
            if (ssprocessselectords.store.Rows.Count > 0)
            {
                storename = ssprocessselectords.store[0].store_name.TrimEnd();
            }
            return storename;
        }

        public String GetAssignedMoneyCenter(string storecode)
        {
            storecode = storecode.Substring(0, 11);
            string moneycentername = "Unknown";
            String CommandText = "SELECT * FROM view_expandedstore WHERE storecode = @storecode";
            ssprocessds.view_expandedstore.Rows.Clear();
            ClearParameters();
            AddParms("@storecode", storecode, "SQL");
            FillData(ssprocessds, "view_expandedstore", CommandText, CommandType.Text);
            if (ssprocessds.view_expandedstore.Rows.Count > 0)
            {
                moneycentername = ssprocessds.view_expandedstore[0].moneycentername.TrimEnd();
            }
            return moneycentername;
        }

        public String GetCoinInventoryHolder(string storecode, string driver)
        {
            string commandtext = "";
            bool holderassigned = false;
            // The default holder is Safe and Sound
            string bankfedid = "10000000";

            // Position store and driver tables
            ssprocessds.store.Rows.Clear();
            this.ClearParameters();
            this.AddParms("@store", storecode.Substring(0, 11), "SQL");
            commandtext = "SELECT * FROM store WHERE LEFT(storecode,11) = @store";
            FillData(ssprocessds, "store", commandtext, CommandType.Text);
            if (ssprocessds.store.Rows.Count < 1)
            {
                bankfedid = "";
                holderassigned = true;
            }
            ssprocessds.driver.Rows.Clear();
            this.ClearParameters();
            this.AddParms("@driver", driver, "SQL");
            commandtext = "SELECT * FROM  driver  WHERE number = @driver";
            FillData(ssprocessds, "driver", commandtext, CommandType.Text);
            if (ssprocessds.driver.Rows.Count < 1)
            {
                bankfedid = "";
                holderassigned = true;
            }
            if (!holderassigned && ssprocessds.store[0].sigcoin == "Y")
            {
                bankfedid = "";
                holderassigned = true;

            }


            if (!holderassigned)
            {
                // Eliminate all Uncle Guiseppe stores - they are serviced by Rapid... except Port Washington which Safe and Sound services
                if (storecode.Substring(0, 4) == "8250" && storecode.TrimEnd() != "8250-000001")
                {
                    bankfedid = "";
                    holderassigned = true;
                }
            }
            if (!holderassigned && storecode.Substring(0, 4) == "4075")
            {
                bankfedid = "FEGS";
                holderassigned = true;

            }
            if (!holderassigned)
            {
                if (storecode.Substring(0, 4) == "4600")
                {
                    bankfedid = "RA111";
                    holderassigned = true;
                }
            }

            if (!holderassigned)
            {
                if (ssprocessds.driver[0].bulkcoin && storecode.Substring(0, 4) != "1800")
                {

                    bankfedid = "";
                    holderassigned = true;
                }
            }

            return bankfedid;

        }
        public String GetDriverName(string drivernumber)
        {
            string drivername = "Unknown";
            String CommandText = "SELECT TOP 1 * FROM driver WHERE number = @drivernumber";
            ssprocessselectords.driver.Rows.Clear();
            ClearParameters();
            AddParms("@drivernumber", drivernumber, "SQL");
            FillData(ssprocessselectords, "driver", CommandText, CommandType.Text);
            if (ssprocessselectords.driver.Rows.Count > 0)
            {
                drivername = ssprocessselectords.driver[0].firstname.TrimEnd() + " " + ssprocessselectords.driver[0].lastname;
            }
            return drivername;
        }
        public string SelectInvoiceNumber()
        {
            FrmSelectInvoice frmSelectInvoice = new FrmSelectInvoice();
            frmSelectInvoice.numericUpDownYear.Value = DateTime.Now.AddMonths(-1).Year;
            frmSelectInvoice.numericUpDownMonth.Value = DateTime.Now.AddMonths(-1).Month;

            string commandtext = "";
            string InvoiceNumber = "";
            int i = 1;
            while (i >= 1)
            {
                frmSelectInvoice.ShowDialog();
                if (!frmSelectInvoice.cont)
                {
                    InvoiceNumber = "";
                    break;
                }
                if (frmSelectInvoice.textBoxInvoiceNumber.Text.TrimEnd() != "")
                {
                    InvoiceNumber = frmSelectInvoice.textBoxInvoiceNumber.Text.TrimEnd();
                    ssprocessds.view_ExpandedBilling.Rows.Clear();
                    commandtext = "SELECT * FROM view_expandedbilling WHERE inv_number = @invnumber";
                    ClearParameters();
                    AddParms("@invnumber", InvoiceNumber, "SQL");
                    FillData(ssprocessds, "view_expandedbilling", commandtext, CommandType.Text);
                    if (ssprocessds.view_ExpandedBilling.Rows.Count > 0)
                    {

                        break;
                    }
                    else
                    {
                        wsgUtilities.wsgNotice("Invoice Number " + InvoiceNumber + " does not exist");
                        InvoiceNumber = "";
                    }

                }
                else
                {
                    if (frmSelectInvoice.SelectedCompany.TrimEnd() != "")
                    {
                        commandtext = "SELECT * FROM view_BillingStoreBillTypeSummary WHERE LEFT(storecode,4) = @company AND MONTH(inv_date) = @rptmonth AND YEAR(inv_date) = @rptyear  ORDER BY storecode, bill_type  ";
                        ClearParameters();
                        AddParms("@company", frmSelectInvoice.SelectedCompany.TrimEnd(), "SQL");
                        AddParms("@rptmonth", frmSelectInvoice.numericUpDownMonth.Value, "SQL");
                        AddParms("@rptyear", frmSelectInvoice.numericUpDownYear.Value, "SQL");
                        FillData(ssprocessds, "view_BillingStoreBillTypeSummary", commandtext, CommandType.Text);
                        if (ssprocessds.view_BillingStoreBillTypeSummary.Rows.Count > 1)
                        {
                            InvoiceNumber = ssprocessds.view_BillingStoreBillTypeSummary[0].inv_number;
                            break;
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("Cannot Locate and Invoice for that company that month");

                        }
                    }
                    else
                    {
                        wsgUtilities.wsgNotice("Please specify an invoice number or a company number");

                    }
                }
            }

            return InvoiceNumber;
        }

        public DateTime GetLastDateOfMonth(DateTime startdate)
        {
            DateTime lastdateofmonth = startdate;

            while (lastdateofmonth.Month == startdate.Month)
            {
                lastdateofmonth = lastdateofmonth.AddDays(1);
            }
            lastdateofmonth = lastdateofmonth.AddDays(-1);
            return lastdateofmonth;
        }
        public int[] GetMonthAndYear()
        {
            DateTime selectordate = DateTime.Now.AddMonths(-1).Date;
            int[] MonthYear = new int[2];
            FrmGetMonthAndYear frmGetMonthAndYear = new FrmGetMonthAndYear();
            frmGetMonthAndYear.numericUpDownMonth.Value = selectordate.Month;
            frmGetMonthAndYear.numericUpDownYear.Value = selectordate.Year;
            frmGetMonthAndYear.ShowDialog();
            if (frmGetMonthAndYear.DataOK)
            {
                MonthYear[0] = Convert.ToInt32(frmGetMonthAndYear.numericUpDownMonth.Value);
                MonthYear[1] = Convert.ToInt32(frmGetMonthAndYear.numericUpDownYear.Value);
            }
            else
            {
                MonthYear[0] = 0;
                MonthYear[1] = 0;
            }
            return MonthYear;
        }
        public string SelectRegion(string compcode)
        {
            string regioncode = "";
            mySQLDataMethods.GetRegions(compcode);
            if (mySQLDataMethods.mysqlselectords.region.Rows.Count > 1)
            {
                FrmSelector frmSelector = new FrmSelector();
                FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
                frmSelectorMethods.FormText = "Select Region";
                frmSelectorMethods.dtSource = mySQLDataMethods.mysqlselectords.region;
                frmSelectorMethods.columncount = 2;
                frmSelectorMethods.SetColumns();
                frmSelectorMethods.colname[0] = "Regioncodecol";
                frmSelectorMethods.colheadertext[0] = "Region Code";
                frmSelectorMethods.coldatapropertyname[0] = "regioncode";
                frmSelectorMethods.colwidth[0] = 100;
                frmSelectorMethods.colname[1] = "RegionNamecol";
                frmSelectorMethods.colheadertext[1] = "Region Name";
                frmSelectorMethods.coldatapropertyname[1] = "regionname";
                frmSelectorMethods.colwidth[1] = 250;
                frmSelectorMethods.SetGrid();
                frmSelector.Width = 700;
                regioncode = frmSelectorMethods.ShowStringSelector("regioncode");
            }
            else
            {
                if (mySQLDataMethods.mysqlselectords.region.Rows.Count > 0)
                {
                    regioncode = mySQLDataMethods.mysqlselectords.region[0].regioncode;
                }
                else
                {
                    wsgUtilities.wsgNotice("There are no regions for this company");
                }
            }
            return regioncode;
        }

        public void getSmartSafeUndeclared(DateTime processdate, string serialnumber)
        {
            string commandtext = "  SELECT serialnumber, bankfedid, sum (saidtocontain) as unverified FROM ";
            commandtext += " (select * from view_expandedsmartsafetrans where eventcode = 'DECL' AND postingdate <= @selectdate AND serialnumber = @serialnumber  and (verifyid = 0 ";
            commandtext += " OR  (verifyid <> 0 AND  verifyid IN  (SELECT idcol FROM smartsafetrans WHERE eventcode =  'VER' AND postingdate > @selectdate )))) u";
            commandtext += " GROUP BY serialnumber, bankfedid ";
            ClearParameters();
            AddParms("@selectdate", processdate, "SQL");
            AddParms("@serialnumber", processdate, "SQL");
            FillData(sysdatads, "eventlog", commandtext, CommandType.Text);

        }




    }
    public class DateTimeSelectionMethods
    {
        public DateTime SelectedDate { get; set; }
        public string SelectedTime { get; set; }

        public bool GetDateAndTime()
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            FrmGetDateAndTime frmGetDateAndTime = new FrmGetDateAndTime();
            frmGetDateAndTime.ShowDialog();
            if (frmGetDateAndTime.OKtoProceed)
            {
                SelectedDate = Convert.ToDateTime(frmGetDateAndTime.SelectedDateAndTime.Date.ToString("MM/dd/yyyy"));
                SelectedTime = TimeZoneInfo.ConvertTime(frmGetDateAndTime.SelectedDateAndTime, timeZone).ToString("HH:mm:ss:tt");
            }
            return frmGetDateAndTime.OKtoProceed;
        }
    }
    public class MemoMethods
    {
        public string memodata = "";
        public bool updatememo = false;
        public FrmMemo frmMemo = new FrmMemo();
        public bool ShowMemo()
        {
            frmMemo.textBoxMemo.Text = memodata;
            frmMemo.textBoxMemo.Focus();
            frmMemo.ShowDialog();
            memodata = frmMemo.textBoxMemo.Text;
            if (frmMemo.CancelUpdate)
            {
                updatememo = false;
            }
            else
            {
                updatememo = true;
            }
            return updatememo;
        }

    }

    public class WSGTextBoxMethods
    {

        public TextBox CreateTabOnEnterTextbox()
        {
            TextBox WSGTextbox = new TextBox();
            WSGTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            return WSGTextbox;
        }

        private void SendTabonEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

    }
}