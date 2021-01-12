using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Data;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ApplicationMaintenance
{
    public class MaintainSalesperson : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Sales Person");
        string commandtext = "";
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();
        TextBox TextBoxSlscode = new TextBox();
        Label LabelSlscode = new Label();
        TextBox TextBoxSlsname = new TextBox();
        Label LabelSlsname = new Label();
        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.salesperson;
            currentablename = "salesperson";
            SetIdcol(ssprocessds.salesperson.idcolColumn);
            parentForm.Text = "Maintain Salesperson Data";

        }
        public override void SetControls()
        {

            int TextTop = 50;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 200;
            SetTextBox(TextBoxSlscode, LeftStart, TextTop, 75, ssprocessds.salesperson, "slscode", LabelSlscode, "Code");
            TextBoxSlscode.CharacterCasing = CharacterCasing.Upper;
            TextBoxSlscode.MaxLength = 4;
            TextTop += 22;
            SetTextBox(TextBoxSlsname, LeftStart, TextTop, 300, ssprocessds.salesperson, "slsname", LabelSlsname, "Name");

        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            string selectedsaleperson = commonAppDataMethods.SelectSalePerson();

            // If a row has been selected fill the data and process
            if (selectedsaleperson.TrimEnd().Length > 0)
            {
                ssprocessds.salesperson.Rows.Clear();
                commandtext = "SELECT * FROM salesperson WHERE slscode = @slscode";
                ClearParameters();
                AddParms("@slscode", selectedsaleperson, "SQL");
                FillData(ssprocessds, "salesperson", commandtext, CommandType.Text);
                currentidcol = ssprocessds.salesperson[0].idcol;
                CurrentState = "View";
                RefreshControls();
            }
        }
        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (cont)
            {
               // Be sure that there are no more than 3 characters for the code
                ssprocessds.salesperson[0].slscode = ssprocessds.salesperson[0].slscode.PadRight(3);
                ssprocessds.salesperson[0].slscode = ssprocessds.salesperson[0].slscode.Substring(0, 3);
                GenerateAppTableRowSave(ssprocessds.salesperson[0]);
                base.SaveCurrentDataTable(sender, e);
            }
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.salesperson);
            base.SetInsertState(sender, e);
        }
    }

    public class MaintainChargeType : FrmMaintainSingleTableMethods
    {
        ssprocess ssprocesssearchds = new ssprocess();
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Charge Type");
        string commandtext = "";
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();
        TextBox TextBoxChgcode = new TextBox();
        Label LabelChgcode = new Label();
        TextBox TextBoxChgdesc = new TextBox();
        Label LabelChgdesc = new Label();
        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.chargetype;
            currentablename = "chargetype";
            SetIdcol(ssprocessds.chargetype.idcolColumn);
            parentForm.Text = "Maintain Charge Type";

        }
        public override void SetControls()
        {

            int TextTop = 50;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 200;
            SetTextBox(TextBoxChgcode, LeftStart, TextTop, 75, ssprocessds.chargetype, "chgcode", LabelChgcode, "Code");
            TextBoxChgcode.CharacterCasing = CharacterCasing.Upper;
            TextBoxChgcode.MaxLength = 6;
            TextTop += 22;
            SetTextBox(TextBoxChgdesc, LeftStart, TextTop, 300, ssprocessds.chargetype, "chgdesc", LabelChgdesc, "Description");

        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            string  selectedchargetype = commonAppDataMethods.SelectChargeType();

            // If a row has been selected fill the data and process
            if  (selectedchargetype.TrimEnd().Length > 0)
            {
                ssprocessds.chargetype.Rows.Clear();
                commandtext = "SELECT * FROM chargetype WHERE chgcode = @chgcode";
                ClearParameters();
                AddParms("@chgcode", selectedchargetype, "SQL");
                FillData(ssprocessds, "chargetype", commandtext, CommandType.Text);
                currentidcol = ssprocessds.chargetype[0].idcol;
                CurrentState = "View";
                RefreshControls();
            }
        }
        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (ssprocessds.chargetype[0].chgcode.TrimEnd().Length < 1)
            {
                wsgUtilities.wsgNotice("There must be a charge code");
                cont = false;
            
            }
            if (ssprocessds.chargetype[0].chgdesc.TrimEnd().Length < 1)
            {
                wsgUtilities.wsgNotice("There must be a description");
                cont = false;

            }


            // Prevent Duplicates
            if (ssprocessds.chargetype[0].idcol < 1)
            {
                ssprocesssearchds.chargetype.Rows.Clear();
                commandtext = "SELECT * FROM chargetype WHERE chgcode = @chgcode";
                ClearParameters();
                AddParms("@chgcode", ssprocessds.chargetype[0].chgcode, "SQL");
                FillData(ssprocesssearchds, "chargetype", commandtext, CommandType.Text);
                if (ssprocesssearchds.chargetype.Rows.Count > 0)
                {
                    wsgUtilities.wsgNotice("That charge code is already in the file");
                   cont = false;
                }

            }

            if (cont)
            {
                ssprocessds.chargetype[0].chgcode = ssprocessds.chargetype[0].chgcode.PadRight(6).Substring(0, 6);
                ssprocessds.chargetype[0].chgdesc = ssprocessds.chargetype[0].chgdesc.PadRight(25).Substring(0, 25); 
                GenerateAppTableRowSave(ssprocessds.chargetype[0]);
                base.SaveCurrentDataTable(sender, e);
            }
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.chargetype);
            base.SetInsertState(sender, e);
        }
        public override void RefreshControls()
        {
            base.RefreshControls();
            if (CurrentState == "Edit")
            {
                TextBoxChgcode.Enabled = false;
            }
        }

    }

    public class FrmMaintainBankMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Bank");
        // Any controls being added must be defined here
        TextBox TextBoxName = new TextBox();
        TextBox TextBoxBankfedid = new TextBox();
        TextBox TextBoxOutFtpaddress = new TextBox();
        TextBox TextBoxDestname = new TextBox();
        TextBox TextBoxOutServerport = new TextBox();
        TextBox TextBoxOutUsername = new TextBox();
        TextBox TextBoxOutPassword = new TextBox();
        TextBox TextBoxEmailaddr = new TextBox();
        TextBox TextBoxInFtpaddress = new TextBox();
        TextBox TextBoxInServerport = new TextBox();
        TextBox TextBoxInUsername = new TextBox();
        TextBox TextBoxInPassword = new TextBox();
        TextBox TextBoxCitaccount = new TextBox();
        TextBox TextBoxCohaccount = new TextBox();
        TextBox TextBoxRcnum = new TextBox();
        TextBox TextBoxDdasusp = new TextBox();
        TextBox TextBranchno = new TextBox();
        TextBox TextBoxCoinreceipt = new TextBox();
        TextBox TextBoxAtmbank = new TextBox();
        TextBox TextBoxSafemain = new TextBox();
        TextBox TextBoxSafekeeping = new TextBox();
        TextBox TextBoxCheckreceipt = new TextBox();
        TextBox TextBoxPettycash = new TextBox();
        TextBox TextBoxSmartsafe = new TextBox();
   
        Button ButtonEmailaddresses = new Button();
        Label LabelName = new Label();
        Label LabelBankfedid = new Label();
        Label LabelOutFtpaddr = new Label();
        Label LabelDestname = new Label();
        Label LabelOutServerport = new Label();
        Label LabelOutUsername = new Label();
        Label LabelOutPassword = new Label();
        Label LabelInFtpaddr = new Label();
        Label LabelInServerport = new Label();
        Label LabelInUsername = new Label();
        Label LabelInPassword = new Label();
        Label LabelEmailaddr = new Label();
        Label LabelCitaccount = new Label();
        Label LabelCohaccount = new Label();
        Label LabelRcnum = new Label();
        Label LabelDdasusp = new Label();
        Label LabelBranchno = new Label();
        Label LabelCoinreceipt = new Label();
        Label LabelAtmbank = new Label();
        Label LabelSafemain = new Label();
        Label LabelSafekeeping = new Label();
        Label LabelCheckreceipt = new Label();
        Label LabelPettycash = new Label();
        Label LabelSmartsafe = new Label();

        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();

        public override void EstablishAppConstants()
        {
            currentablename = "bank";
            SetIdcol(ssprocessds.bank.idcolColumn);
            parentForm.Text = "Maintain Bank Data";
        }
        public override void SetControls()
        {
            int TextTop = 50;
            int LeftStart = 50;
            // Establish any custom controls here 
            ButtonEmailaddresses.Text = "Email Addresses";
            ButtonEmailaddresses.Height = 30;
            ButtonEmailaddresses.Width = 100;
            ButtonEmailaddresses.Top = TextTop;
            ButtonEmailaddresses.Left = LeftStart + 100;
            TextTop += 35;
            parentForm.Controls.Add(ButtonEmailaddresses);
            SetTextBox(TextBoxName, LeftStart, TextTop, 175, ssprocessds.bank, "name", LabelName, "Bank Name");
            TextTop += 22;
            SetTextBox(TextBoxBankfedid, LeftStart, TextTop, 100, ssprocessds.bank, "bankfedid", LabelBankfedid, "Bank Fed ID");
            TextTop += 22;
            SetTextBox(TextBoxOutFtpaddress, LeftStart, TextTop, 250, ssprocessds.bank, "outftpaddress", LabelOutFtpaddr, "Out FTP Address");
            TextTop += 22;
            SetTextBox(TextBoxDestname, LeftStart, TextTop, 250, ssprocessds.bank, "destname", LabelDestname, "Destination Name");
            TextTop += 22;
            SetTextBox(TextBoxOutServerport, LeftStart, TextTop, 40, ssprocessds.bank, "outserverport", LabelOutServerport, "Out Server Port");
            TextTop += 22;
            SetTextBox(TextBoxOutUsername, LeftStart, TextTop, 150, ssprocessds.bank, "outusername", LabelOutUsername, "Out User Name");
            TextTop += 22;
            SetTextBox(TextBoxOutPassword, LeftStart, TextTop, 175, ssprocessds.bank, "outpassword", LabelOutPassword, "Out Password");
            TextTop += 22;
            SetTextBox(TextBoxInFtpaddress, LeftStart, TextTop, 250, ssprocessds.bank, "inftpaddress", LabelInFtpaddr, "In FTP Address");
            TextTop += 22;
            SetTextBox(TextBoxInServerport, LeftStart, TextTop, 40, ssprocessds.bank, "inserverport", LabelInServerport, "In Server Port");
            TextTop += 22;
            SetTextBox(TextBoxInUsername, LeftStart, TextTop, 150, ssprocessds.bank, "inusername", LabelInUsername, "In User Name");
            TextTop += 22;
            SetTextBox(TextBoxInPassword, LeftStart, TextTop, 175, ssprocessds.bank, "inpassword", LabelInPassword, "In Password");
            TextTop += 22;

            SetTextBox(TextBoxCitaccount, LeftStart, TextTop, 80, ssprocessds.bank, "citaccount", LabelCitaccount, "CIT Account");
            TextTop += 22;
            SetTextBox(TextBoxCohaccount, LeftStart, TextTop, 80, ssprocessds.bank, "cohaccount", LabelCohaccount, "COH Account");
            TextTop += 22;
            SetTextBox(TextBoxRcnum, LeftStart, TextTop, 80, ssprocessds.bank, "rcnum", LabelRcnum, "RC Number");
            TextTop += 22;
            SetTextBox(TextBranchno, LeftStart, TextTop, 80, ssprocessds.bank, "branchno", LabelBranchno, "Branch Number");
            TextTop += 22;
            SetTextBox(TextBoxCoinreceipt, LeftStart, TextTop, 15, ssprocessds.bank, "coinreceipt", LabelCoinreceipt, "Coin Receipt");
            TextBoxCoinreceipt.CharacterCasing = CharacterCasing.Upper;
            TextTop += 22;
            SetTextBox(TextBoxAtmbank, LeftStart, TextTop, 15, ssprocessds.bank, "atmbank", LabelAtmbank, "ATM Bank");
            TextBoxAtmbank.CharacterCasing = CharacterCasing.Upper;
            TextTop += 22;
            SetTextBox(TextBoxSafemain, LeftStart, TextTop, 15, ssprocessds.bank, "safemain", LabelSafemain, "S & S Main Account");
            TextBoxSafemain.CharacterCasing = CharacterCasing.Upper;
            TextTop += 22;
            SetTextBox(TextBoxSafekeeping, LeftStart, TextTop, 15, ssprocessds.bank, "safekeeping", LabelSafekeeping, "Safekeeeping");
            TextBoxSafekeeping.CharacterCasing = CharacterCasing.Upper;
            TextTop += 22;
            SetTextBox(TextBoxCheckreceipt, LeftStart, TextTop, 15, ssprocessds.bank, "checkreceipt", LabelCheckreceipt, "Check Receipt");
            TextBoxCheckreceipt.CharacterCasing = CharacterCasing.Upper;
            TextTop += 22;
            SetTextBox(TextBoxPettycash, LeftStart, TextTop, 15, ssprocessds.bank, "pettycash", LabelPettycash, "Petty Cash");
            TextBoxPettycash.CharacterCasing = CharacterCasing.Upper;
            TextTop += 22;
            SetTextBox(TextBoxSmartsafe, LeftStart, TextTop, 15, ssprocessds.bank, "smartsafe", LabelSmartsafe, "Smart Safe");
            TextBoxPettycash.CharacterCasing = CharacterCasing.Upper;
            TextTop += 22;
        
            parentForm.Height = 625;
        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.bank);
            currentidcol = ssprocessds.bank[0].idcol;
            base.SetInsertState(sender, e);
        }

        public override void RefreshControls()
        {
            base.RefreshControls();

            if (CurrentState == "View")
            {
                ButtonEmailaddresses.Enabled = true;
            }
            else
            {
                ButtonEmailaddresses.Enabled = false;
            }
            //   TextBoxBankfedid.ReadOnly = true;

        }
        public override void SetEvents()
        {
            ButtonEmailaddresses.Click += new System.EventHandler(ProcessEmailaddresses);
            base.SetEvents();
        }
        public void ProcessEmailaddresses(object sender, EventArgs e)
        {
            FrmMaintainBankEmaiAddressMethods frmMaintainBankEmailAddressMethods = new FrmMaintainBankEmaiAddressMethods();
            frmMaintainBankEmailAddressMethods.ShowParent();
            frmMaintainBankEmailAddressMethods.bankfedid = ssprocessds.bank[0].bankfedid;
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            // Save the table
            // Trim single character columns to prevent overflow
            ssprocessds.bank[0].checkreceipt = ssprocessds.bank[0].checkreceipt.TrimStart().TrimEnd();
            ssprocessds.bank[0].atmbank = ssprocessds.bank[0].atmbank.TrimStart().TrimEnd();
            ssprocessds.bank[0].safekeeping = ssprocessds.bank[0].safekeeping.TrimStart().TrimEnd();
            ssprocessds.bank[0].coinreceipt = ssprocessds.bank[0].coinreceipt.TrimStart().TrimEnd();
            ssprocessds.bank[0].safemain = ssprocessds.bank[0].safemain.TrimStart().TrimEnd();
            ssprocessds.bank[0].pettycash = ssprocessds.bank[0].pettycash.TrimStart().TrimEnd();

            GenerateAppTableRowSave(ssprocessds.bank[0]);
            base.SaveCurrentDataTable(sender, e);
        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            ssprocessds.bank.Rows.Clear();
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            currentidcol = 0;
            int selectedbankid = commonAppDataMethods.SelectBank();
            // If a row has been selected fill the data and process
            if (selectedbankid > 0)
            {
                ssprocessds.bank.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", selectedbankid, "SQL");
                FillData(ssprocessds, "bank", "wsgsp_getbankbyIdcol", CommandType.StoredProcedure);
                currentidcol = selectedbankid;
                CurrentState = "View";
                RefreshControls();
            }
        }
    }

    public class FrmMaintainBankEmaiAddressMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Bank Email Addresses");
        // Any controls being added must be defined here
        TextBox TextBoxEmailaddr = new TextBox();
        Label LabelEmailaddr = new Label();
        ssprocess ssprocessds = new ssprocess();
        CheckBox checkboxInvoiceonly = new CheckBox();
        CheckBox checkboxCountonly = new CheckBox();
        CheckBox checkboxPickuponly = new CheckBox();
        Label labelInvoiceonly = new Label();
        Label labelCountonly = new Label();
        Label labelPickupnly = new Label();

        public string bankfedid { get; set; }
        // Establish the dataset
        ssprocess ssemailds = new ssprocess();

        public override void EstablishAppConstants()
        {
            currentablename = "emailbank";
            SetIdcol(ssemailds.emailbank.idcolColumn);
            parentForm.Text = "Maintain Bank Email Addresses";
        }
        public override void SetControls()
        {

            int TextTop = 50;
            int LeftStart = 50;
            // Establish any custom controls here 
            TextTop += 35;
            SetTextBox(TextBoxEmailaddr, LeftStart, TextTop, 300, ssemailds.emailbank, "emailaddr", LabelEmailaddr, "Email Address");
            TextTop += 22;
            parentForm.Height = 200;
        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssemailds.emailbank);
            ssemailds.emailbank[0].bankfedid = bankfedid;
            currentidcol = ssemailds.emailbank[0].idcol;
            base.SetInsertState(sender, e);
        }


        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            // Save the table
            GenerateAppTableRowSave(ssemailds.emailbank[0]);
            base.SaveCurrentDataTable(sender, e);
        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            ssemailds.emailbank.Rows.Clear();
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {

            ssprocess ssprocessselectords = new ssprocess();
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.bank.Rows.Clear();
            string commandtext = "SELECT * FROM emailbank WHERE bankfedid = @bankfedid ";
            commandtext += " ORDER BY emailaddr";
            ;
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(ssprocessselectords, "emailbank", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Email Address";
            frmSelectorMethods.dtSource = ssprocessselectords.emailbank;
            frmSelectorMethods.columncount = 1;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Emailaddresscol";
            frmSelectorMethods.colheadertext[0] = "Email Address";
            frmSelectorMethods.coldatapropertyname[0] = "emailaddr";
            frmSelectorMethods.colwidth[0] = 600;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 700;
            int selectemailaddressid = frmSelectorMethods.ShowSelector();

            // If a row has been selected fill the data and process
            if (selectemailaddressid > 0)
            {
                ssemailds.emailbank.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", selectemailaddressid, "SQL");
                commandtext = "SELECT * FROM emailbank WHERE idcol = @idcol";
                FillData(ssemailds, "emailbank", commandtext, CommandType.Text);
                currentidcol = selectemailaddressid;
                CurrentState = "View";
                RefreshControls();
            }
        }
    }
    public class FrmMaintainApproleMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Bank Email Addresses");
        // Any controls being added must be defined here
        TextBox TextBoxUserrole = new TextBox();
        Label LabelUserrole = new Label();
        TextBox TextBoxDescrip = new TextBox();
        Label LabelDescrip = new Label();
        // Establish the dataset
        sysdata sysdatads = new sysdata();

        public override void EstablishAppConstants()
        {
            currentablename = "approle";
            SetIdcol(sysdatads.approle.idcolColumn);
            parentForm.Text = "Maintain User Role";
        }
        public override void SetControls()
        {
            int TextTop = 50;
            int LeftStart = 50;
            // Establish any custom controls here 
            TextTop += 35;
            SetTextBox(TextBoxUserrole, LeftStart, TextTop, 300, sysdatads.approle, "userrole", LabelUserrole, "Role");
            TextBoxUserrole.MaxLength = 4;
            TextTop += 22;
            SetTextBox(TextBoxDescrip, LeftStart, TextTop, 300, sysdatads.approle, "descrip", LabelDescrip, "Description");
            TextBoxDescrip.MaxLength = 35;
            parentForm.Height = 200;
        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(sysdatads.approle);
            currentidcol = sysdatads.approle[0].idcol;
            base.SetInsertState(sender, e);
        }


        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            // Save the table
            GenerateAppTableRowSave(sysdatads.approle[0]);
            base.SaveCurrentDataTable(sender, e);
        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            sysdatads.approle.Rows.Clear();
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            currentidcol = commonAppDataMethods.SelectUserrole();
            // If a row has been selected fill the data and process
            if (currentidcol > 0)
            {
                sysdatads.approle.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", currentidcol, "SQL");
                string commandtext = "SELECT * FROM approle where idcol = @idcol";
                FillData(sysdatads, "approle", commandtext, CommandType.Text);

                CurrentState = "View";
                RefreshControls();
            }
        }
    }
    public class FrmMaintainProcessMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Process");
        // Any controls being added must be defined here
        TextBox TextBoxProcess = new TextBox();
        Label LabelProcess = new Label();
        TextBox TextBoxDescrip = new TextBox();
        Label LabelDescrip = new Label();

        // Establish the dataset
        sysdata sysdatads = new sysdata();

        public override void EstablishAppConstants()
        {
            currentablename = "appprocess";
            SetIdcol(sysdatads.appprocess.idcolColumn);
            parentForm.Text = "Maintain Process";
        }
        public override void SetControls()
        {
            int TextTop = 50;
            int LeftStart = 50;
            // Establish any custom controls here 
            TextTop += 35;
            SetTextBox(TextBoxProcess, LeftStart, TextTop, 70, sysdatads.appprocess, "process", LabelProcess, "Process");
            TextTop += 22;
            SetTextBox(TextBoxDescrip, LeftStart, TextTop, 300, sysdatads.appprocess, "descrip", LabelDescrip, "Description");
            TextTop += 22;
            parentForm.Height = 200;
        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(sysdatads.appprocess);
            currentidcol = sysdatads.appprocess[0].idcol;
            base.SetInsertState(sender, e);
        }


        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            // Save the table
            GenerateAppTableRowSave(sysdatads.appprocess[0]);
            base.SaveCurrentDataTable(sender, e);
        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            sysdatads.appprocess.Rows.Clear();
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {

            currentidcol = commonAppDataMethods.SelectAppProcess();

            // If a row has been selected fill the data and process
            if (currentidcol > 0)
            {
                sysdatads.appprocess.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", currentidcol, "SQL");
                string commandtext = "SELECT * FROM appprocess WHERE idcol = @idcol";
                FillData(sysdatads, "appprocess", commandtext, CommandType.Text);
                CurrentState = "View";
                RefreshControls();
            }
        }
    }

    public class FrmMaintainMoneyCenterMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Moneycenter");
        // Any controls being added must be defined here
        TextBox TextBoxCentername = new TextBox();
        Label LabelCentername = new Label();
        TextBox TextBoxActive = new TextBox();
        Label LabelActive = new Label();
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();

        public override void EstablishAppConstants()
        {
            currentablename = "moneycenter";
            SetIdcol(ssprocessds.moneycenter.idcolColumn);
            parentForm.Text = "Maintain Money Center";
        }
        public override void SetControls()
        {
            int TextTop = 50;
            int LeftStart = 50;
            // Establish any custom controls here 
            TextTop += 35;
            SetTextBox(TextBoxCentername, LeftStart, TextTop, 200, ssprocessds.moneycenter, "centername", LabelCentername, "Money Center");
            TextTop += 22;
            parentForm.Height = 200;
        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.moneycenter);
            currentidcol = ssprocessds.moneycenter[0].idcol;
            base.SetInsertState(sender, e);
        }


        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            // Save the table
            GenerateAppTableRowSave(ssprocessds.moneycenter[0]);
            base.SaveCurrentDataTable(sender, e);
        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            ssprocessds.moneycenter.Rows.Clear();
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {

            currentidcol = commonAppDataMethods.SelectMoneyCenter();

            // If a row has been selected fill the data and process
            if (currentidcol > 0)
            {
                ssprocessds.moneycenter.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", currentidcol, "SQL");
                string commandtext = "SELECT * FROM moneycenter WHERE idcol = @idcol";
                FillData(ssprocessds, "moneycenter", commandtext, CommandType.Text);
                CurrentState = "View";
                RefreshControls();
            }
        }
    }

    public class FrmMaintainPrivilegeMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Privilege");
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        // Any controls being added must be defined here
        Button buttonSelectApprole = new Button();
        Button buttonSelectAppprocess = new Button();
        Label labelApprole = new Label();
        Label labelAppprocessName = new Label();
        public int CurrentAppprocessid = new int();
        public string CurrentUserid;
        public string CurrentProcess;
        // Establish the datasets
        sysdata sysdatads = new sysdata();
        sysdata listds = new sysdata();
        public override void EstablishAppConstants()
        {
            CurrentAppprocessid = 0;
            CurrentUserid = "";
            CurrentProcess = "";
            currentablename = "apppriv";
            SetIdcol(sysdatads.apppriv.idcolColumn);
            parentForm.Text = "Maintain Privileges";
        }
        public override void SetEvents()
        {
            base.SetEvents();
            buttonSelectAppprocess.Click += new System.EventHandler(ProcessSelectAppprocess);
            buttonSelectApprole.Click += new System.EventHandler(ProcessSelectApprole);
        }
        public override void SetControls()
        {

            // Establish any custom controls here     
            buttonSelectApprole.Left = 50;
            buttonSelectApprole.Text = "Select Role";
            buttonSelectApprole.Top = 50;
            buttonSelectApprole.Width = 100;
            buttonSelectApprole.Height = 25;

            buttonSelectAppprocess.Left = 200;
            buttonSelectAppprocess.Text = "Select Process";
            buttonSelectAppprocess.Top = 50;
            buttonSelectAppprocess.Width = 100;
            buttonSelectAppprocess.Height = 25;


            labelApprole.Left = buttonSelectApprole.Left;
            labelApprole.Top = buttonSelectApprole.Top + buttonSelectApprole.Height + 10;
            labelApprole.Text = "";

            labelAppprocessName.Left = buttonSelectAppprocess.Left;
            labelAppprocessName.Top = buttonSelectAppprocess.Top + buttonSelectAppprocess.Height + 10;
            labelAppprocessName.Text = "";
            parentForm.Controls.Add(buttonSelectApprole);
            parentForm.Controls.Add(buttonSelectAppprocess);
            parentForm.Controls.Add(labelApprole);
            parentForm.Controls.Add(labelAppprocessName);
        }
        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            // Save the table
            GenerateAppTableRowSave(sysdatads.apppriv[0]);
            base.SaveCurrentDataTable(sender, e);
        }
        public override void RefreshControls()
        {
            // Handle any custome controls here
            buttonSelectAppprocess.Enabled = false;
            buttonSelectApprole.Enabled = false;
            base.RefreshControls();
            switch (CurrentState)
            {
                case "Select":
                    {
                        labelApprole.Text = "";
                        labelAppprocessName.Text = "";
                        break;
                    }
                case "Edit":
                    {
                        buttonSelectAppprocess.Enabled = true;
                        buttonSelectApprole.Enabled = true;
                        break;
                    }
                case "Insert":
                    {
                        buttonSelectAppprocess.Enabled = true;
                        buttonSelectApprole.Enabled = true;
                        break;
                    }
            }
        }

        public void ProcessSelectApprole(object sender, EventArgs e)
        {
            int approleid = commonAppDataMethods.SelectUserrole();
            // If a row has been selected fill the data and process
            if (approleid > 0)
            {
                string commandtext = "SELECT * FROM approle WHERE idcol = @idcol";
                sysdatads.approle.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", approleid, "SQL");
                FillData(sysdatads, "approle", commandtext, CommandType.Text);
                labelApprole.Text = sysdatads.approle[0].userrole;
                sysdatads.apppriv[0].userrole = sysdatads.approle[0].userrole;
                RefreshControls();
            }
            else
            {
                approleid = 0;
            }
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(sysdatads.apppriv);
            currentidcol = sysdatads.apppriv[0].idcol;
            base.SetInsertState(sender, e);
        }


        public void ProcessSelectAppprocess(object sender, EventArgs e)
        {
            int CurrentAppprocessid = commonAppDataMethods.SelectAppProcess();
            if (CurrentAppprocessid > 0)
            {
                string commandtext = "SELECT * FROM appprocess WHERE idcol = @idcol";
                sysdatads.appprocess.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", CurrentAppprocessid, "SQL");
                FillData(sysdatads, "appprocess", commandtext, CommandType.Text);
                labelAppprocessName.Text = sysdatads.appprocess[0].process;
                sysdatads.apppriv[0].process = sysdatads.appprocess[0].process;
                RefreshControls();
            }
            else
            {
                CurrentAppprocessid = 0;
            }
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            currentidcol = commonAppDataMethods.SelectAppriv();
            // If a row has been selected fill the data and process
            if (currentidcol > 0)
            {
                string commandtext = "SELECT * FROM apppriv WHERE idcol = @idcol";
                sysdatads.apppriv.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", currentidcol, "SQL");
                FillData(sysdatads, "apppriv", commandtext, CommandType.Text);
                labelApprole.Text = sysdatads.apppriv[0].userrole;
                labelAppprocessName.Text = sysdatads.apppriv[0].process;
                CurrentState = "View";
                RefreshControls();
            }
        }

    }

    public class FrmMaintainCompanyEmailAddresses : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Privilege");
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        // Any controls being added must be defined here
        public string CurrentCompCode;
        // Establish the datasets
        ssprocess ssprocessds = new ssprocess();
        sysdata sysdatads = new sysdata();
        sysdata listds = new sysdata();
        TextBox TextBoxRecipname = new TextBox();
        Label LabelRecipname = new Label();
        TextBox TextBoxEmailAddr = new TextBox();
        Label LabelEmailAddr = new Label();
        CheckBox checkboxSendinvoice = new CheckBox();
        CheckBox checkboxSendcount = new CheckBox();
        CheckBox checkboxSendpickup = new CheckBox();
        Label labelSendinvoice = new Label();
        Label labelSendcount = new Label();
        Label labelSendpickup = new Label();

        public override void EstablishAppConstants()
        {
            currentablename = "companyemailaddress";
            SetIdcol(ssprocessds.companyemailaddress.idcolColumn);
            parentForm.Text = "Maintain Company Email Addresses";
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            // Save the table
            GenerateAppTableRowSave(ssprocessds.companyemailaddress[0]);
            base.SaveCurrentDataTable(sender, e);
        }
        public override void SetControls()
        {

            int TextTop = 50;
            int LeftStart = 50;
            // Establish any custom controls here 
            TextTop += 35;
            SetTextBox(TextBoxRecipname, LeftStart, TextTop, 300, ssprocessds.companyemailaddress, "recipname", LabelRecipname, "Recipient");
            TextTop += 22;
            SetTextBox(TextBoxEmailAddr, LeftStart, TextTop, 300, ssprocessds.companyemailaddress, "emailaddr", LabelEmailAddr, "Email Address");
            TextTop += 22;
            SetBitCheckBox(checkboxSendinvoice, LeftStart, TextTop, 100, ssprocessds.companyemailaddress, "sendinvoice", labelSendinvoice, "Send Invoice");
            labelSendinvoice.Font = new Font(labelSendinvoice.Font, FontStyle.Bold);
            TextTop += 22;
            SetBitCheckBox(checkboxSendcount, LeftStart, TextTop, 100, ssprocessds.companyemailaddress, "sendcount", labelSendcount, "Send Count");
            labelSendcount.Font = new Font(labelSendcount.Font, FontStyle.Bold);
            TextTop += 22;
            SetBitCheckBox(checkboxSendpickup, LeftStart, TextTop, 100, ssprocessds.companyemailaddress, "sendpickup", labelSendpickup, "Send Pickup");
            labelSendpickup.Font = new Font(labelSendpickup.Font, FontStyle.Bold);
            TextTop += 22;
   
            parentForm.Height = 300;
        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            ssprocessds.companyemailaddress.Rows.Clear();
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.companyemailaddress);
            ssprocessds.companyemailaddress[0].comp_code = CurrentCompCode;
            ssprocessds.companyemailaddress[0].sendinvoice = true;
            ssprocessds.companyemailaddress[0].sendcount = true;
            ssprocessds.companyemailaddress[0].sendpickup = true;
            base.SetInsertState(sender, e);
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {

            ssprocess ssprocessselectords = new ssprocess();
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.companyemailaddress.Rows.Clear();
            string commandtext = "SELECT * FROM companyemailaddress WHERE comp_code = @comp_code ";
            commandtext += " ORDER BY recipname, emailaddr";
            ClearParameters();
            AddParms("@comp_code", CurrentCompCode, "SQL");
            FillData(ssprocessselectords, "companyemailaddress", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Email Address";
            frmSelectorMethods.dtSource = ssprocessselectords.companyemailaddress;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Namecol";
            frmSelectorMethods.colheadertext[0] = "Recipient Name";
            frmSelectorMethods.coldatapropertyname[0] = "recipname";
            frmSelectorMethods.colwidth[0] = 250;
            frmSelectorMethods.colname[1] = "Emailaddrcol";
            frmSelectorMethods.colheadertext[1] = "Email Address";
            frmSelectorMethods.coldatapropertyname[1] = "emailaddr";
            frmSelectorMethods.colwidth[1] = 250;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 1250;
            int selectemailaddressid = frmSelectorMethods.ShowSelector();

            // If a row has been selected fill the data and process
            if (selectemailaddressid > 0)
            {
                ssprocessds.companyemailaddress.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", selectemailaddressid, "SQL");
                commandtext = "SELECT * FROM companyemailaddress WHERE idcol = @idcol";
                FillData(ssprocessds, "companyemailaddress", commandtext, CommandType.Text);
                currentidcol = selectemailaddressid;
                CurrentState = "View";
                RefreshControls();
            }
        }
    }

    public class FrmMaintainStoreEmailAddresses : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Privilege");
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        // Any controls being added must be defined here
        public string CurrentStoreCode;
        // Establish the datasets
        ssprocess ssprocessds = new ssprocess();
        sysdata sysdatads = new sysdata();
        sysdata listds = new sysdata();
        TextBox TextBoxRecipname = new TextBox();
        Label LabelRecipname = new Label();
        TextBox TextBoxEmailAddr = new TextBox();
        Label LabelEmailAddr = new Label();

        public override void EstablishAppConstants()
        {
            currentablename = "storeemailaddress";
            SetIdcol(ssprocessds.storeemailaddress.idcolColumn);
            parentForm.Text = "Maintain Store Email Addresses";
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            // Save the table
            GenerateAppTableRowSave(ssprocessds.storeemailaddress[0]);
            base.SaveCurrentDataTable(sender, e);
        }
        public override void SetControls()
        {

            int TextTop = 50;
            int LeftStart = 50;
            // Establish any custom controls here 
            TextTop += 35;
            SetTextBox(TextBoxRecipname, LeftStart, TextTop, 300, ssprocessds.storeemailaddress, "recipname", LabelRecipname, "Recipient");
            TextTop += 22;
            SetTextBox(TextBoxEmailAddr, LeftStart, TextTop, 300, ssprocessds.storeemailaddress, "emailaddr", LabelEmailAddr, "Email Address");
            parentForm.Height = 200;
        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            ssprocessds.companyemailaddress.Rows.Clear();
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.storeemailaddress);
            ssprocessds.storeemailaddress[0].storecode = CurrentStoreCode;
            base.SetInsertState(sender, e);
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {

            ssprocess ssprocessselectords = new ssprocess();
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.companyemailaddress.Rows.Clear();
            string commandtext = "SELECT * FROM storeemailaddress WHERE storecode = @storecode ";
            commandtext += " ORDER BY recipname, emailaddr";
            ClearParameters();
            AddParms("@storecode",CurrentStoreCode, "SQL");
            FillData(ssprocessselectords, "storeemailaddress", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Email Address";
            frmSelectorMethods.dtSource = ssprocessselectords.storeemailaddress;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Namecol";
            frmSelectorMethods.colheadertext[0] = "Recipient Name";
            frmSelectorMethods.coldatapropertyname[0] = "recipname";
            frmSelectorMethods.colwidth[0] = 250;
            frmSelectorMethods.colname[1] = "Emailaddrcol";
            frmSelectorMethods.colheadertext[1] = "Email Address";
            frmSelectorMethods.coldatapropertyname[1] = "emailaddr";
            frmSelectorMethods.colwidth[1] = 250;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 1250;
            int selectemailaddressid = frmSelectorMethods.ShowSelector();

            // If a row has been selected fill the data and process
            if (selectemailaddressid > 0)
            {
                ssprocessds.companyemailaddress.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", selectemailaddressid, "SQL");
                commandtext = "SELECT * FROM storeemailaddress WHERE idcol = @idcol";
                FillData(ssprocessds, "storeemailaddress", commandtext, CommandType.Text);
                currentidcol = selectemailaddressid;
                CurrentState = "View";
                RefreshControls();
            }
        }
    }
  

    
    public class FrmMaintainDriverMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Driver");
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocesssearchds = new ssprocess();
        CheckBox checkboxBulkcoin = new CheckBox();
        Label labelBulkcoin = new Label();
        CheckBox checkboxDrivermanifest = new CheckBox();
        Label labelDrivermanifest = new Label();
        TextBox textboxNumber = new TextBox();
        Label labelNumber = new Label();
        public override void EstablishAppConstants()
        {
            currentablename = "driver";
            SetIdcol(ssprocessds.driver.idcolColumn);
            parentForm.Text = "Maintain Driver Information";
        }


        public override void SetControls()
        {
            parentForm.buttonDelete.Visible = false;
            int TextTop = 50;
            int LeftStart = 50;
            // Establish any custom controls here 
            TextTop += 35;
            SetTextBox(textboxNumber, LeftStart, TextTop, 50, ssprocessds.driver, "number", labelNumber, "Number");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 250, ssprocessds.driver, "firstname", "First Name");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 250, ssprocessds.driver, "lastname", "Last Name");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 250, ssprocessds.driver, "Address", "Address");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 250, ssprocessds.driver, "city", "City, St, Zip");
            SetCasedUnLabelledTextBox(LeftStart + 380, TextTop, 25, ssprocessds.driver, "state", "U");
            SetUnlabelledTextBox(LeftStart + 425, TextTop, 80, ssprocessds.driver, "zip");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 250, ssprocessds.driver, "phone", "Phone");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 250, ssprocessds.driver, "company", "Company");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 250, ssprocessds.driver, "emaddr", "Email");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 80, ssprocessds.driver, "vendno", "Vendor");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 90, ssprocessds.driver, "cellcarr", "Cell Carrier");
            TextTop += 22;
            SetBitCheckBox(checkboxBulkcoin, LeftStart, TextTop, 100, ssprocessds.driver, "bulkcoin", labelBulkcoin, "Bulk Coin");
            labelBulkcoin.Font = new Font(labelBulkcoin.Font, FontStyle.Bold);
            TextTop += 22;
            SetBitCheckBox(checkboxDrivermanifest, LeftStart, TextTop, 100, ssprocessds.driver, "drivermanifest", labelDrivermanifest, "Manifest");
            labelDrivermanifest.Font = new Font(labelDrivermanifest.Font, FontStyle.Bold);

            TextTop += 22;
            parentForm.Height = 425;
        }
        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {

            bool saveOK = true;

            if (ssprocessds.driver[0].idcol < 1)
            {
                ssprocesssearchds.driver.Rows.Clear();
                ClearParameters();
                AddParms("@number", ssprocessds.driver[0].number, "SQL");
                string commandtext = "SELECT * FROM driver WHERE number = @number";
                FillData(ssprocesssearchds, "driver", commandtext, CommandType.Text);
                if (ssprocesssearchds.driver.Rows.Count > 0)
                {
                    wsgUtilities.wsgNotice("That driver number is already in the file");
                    saveOK = false;
                }

            }

            if (saveOK)
            {
                // Save the table
                // Prevent overflows
                ssprocessds.driver[0].number = ssprocessds.driver[0].number.PadRight(100).Substring(0, 2).TrimStart().TrimEnd();
                ssprocessds.driver[0].firstname = ssprocessds.driver[0].firstname.PadRight(100).Substring(0, 15).TrimStart().TrimEnd();
                ssprocessds.driver[0].lastname = ssprocessds.driver[0].lastname.PadRight(100).Substring(0, 20).TrimStart().TrimEnd();
                ssprocessds.driver[0].address = ssprocessds.driver[0].address.PadRight(100).Substring(0, 30).TrimStart().TrimEnd();
                ssprocessds.driver[0].city = ssprocessds.driver[0].city.PadRight(100).Substring(0, 20).TrimStart().TrimEnd();
                ssprocessds.driver[0].state = ssprocessds.driver[0].state.PadRight(100).Substring(0, 2).TrimStart().TrimEnd();
                ssprocessds.driver[0].zip = ssprocessds.driver[0].zip.PadRight(100).Substring(0, 10).TrimStart().TrimEnd();
                ssprocessds.driver[0].phone = ssprocessds.driver[0].phone.PadRight(100).Substring(0, 12).TrimStart().TrimEnd();
                ssprocessds.driver[0].company = ssprocessds.driver[0].company.PadRight(100).Substring(0, 15).TrimStart().TrimEnd();
                ssprocessds.driver[0].cellcarr = ssprocessds.driver[0].cellcarr.PadRight(100).Substring(0, 20).TrimStart().TrimEnd();
                ssprocessds.driver[0].emaddr = ssprocessds.driver[0].emaddr.PadRight(100).Substring(0, 65).TrimStart().TrimEnd();
                ssprocessds.driver[0].vendno = ssprocessds.driver[0].vendno.PadRight(100).Substring(0, 6).TrimStart().TrimEnd();
                GenerateAppTableRowSave(ssprocessds.driver[0]);
                base.SaveCurrentDataTable(sender, e);
            }
        }

        public override void ProcessSelect(object sender, EventArgs e)
        {

            ssprocess ssprocessselectords = new ssprocess();
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.driver.Rows.Clear();
            string commandtext = "SELECT * FROM driver ORDER BY number ";
            FillData(ssprocessselectords, "driver", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Driver";
            frmSelectorMethods.dtSource = ssprocessselectords.driver;
            frmSelectorMethods.columncount = 3;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Numbercol";
            frmSelectorMethods.colheadertext[0] = "Number";
            frmSelectorMethods.coldatapropertyname[0] = "number";
            frmSelectorMethods.colwidth[0] = 75;
            frmSelectorMethods.colname[1] = "Firstnamecol";
            frmSelectorMethods.colheadertext[1] = "First Name";
            frmSelectorMethods.coldatapropertyname[1] = "firstname";
            frmSelectorMethods.colwidth[1] = 250;
            frmSelectorMethods.colname[2] = "Lastnamecol";
            frmSelectorMethods.colheadertext[2] = "Last Name";
            frmSelectorMethods.coldatapropertyname[2] = "lastname";
            frmSelectorMethods.colwidth[2] = 250;

            frmSelectorMethods.SetGrid();
            frmSelector.Width = 1250;
            int selecteddriverid = frmSelectorMethods.ShowSelector();

            // If a row has been selected fill the data and process
            if (selecteddriverid > 0)
            {
                ssprocessds.driver.Rows.Clear();
                ClearParameters();
                AddParms("@idcol", selecteddriverid, "SQL");
                commandtext = "SELECT * FROM driver WHERE idcol = @idcol";
                FillData(ssprocessds, "driver", commandtext, CommandType.Text);
                currentidcol = selecteddriverid;
                CurrentState = "View";
                textboxNumber.ReadOnly = true;
                RefreshControls();
            }
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            textboxNumber.ReadOnly = false;
            EstablishBlankDataTableRow(ssprocessds.driver);
            base.SetInsertState(sender, e);
        }

    }
    public class FrmMaintainScheduledayMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Schedule Day");
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocesssearchds = new ssprocess();
        ComboBox comboboxScheduleDay = new ComboBox();
        Label labelDay = new Label();
        public override void EstablishAppConstants()
        {
            currentablename = "scheduleday";
            SetIdcol(ssprocessds.scheduleday.idcolColumn);
            parentForm.Text = "Maintain Schedule Day Information";
        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            comboboxScheduleDay.SelectedItem = "SUN";
            EstablishBlankDataTableRow(ssprocessds.scheduleday);
            base.SetInsertState(sender, e);
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {

            ssprocess ssprocessselectords = new ssprocess();
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.scheduleday.Rows.Clear();
            string commandtext = "SELECT * FROM scheduleday ORDER BY schday";
            FillData(ssprocessselectords, "scheduleday", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Select Schedule Day";
            frmSelectorMethods.dtSource = ssprocessselectords.scheduleday;
            frmSelectorMethods.columncount = 3;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Schdaycol";
            frmSelectorMethods.colheadertext[0] = "Code";
            frmSelectorMethods.coldatapropertyname[0] = "schday";
            frmSelectorMethods.colwidth[0] = 90;
            frmSelectorMethods.colname[1] = "Descripcol";
            frmSelectorMethods.colheadertext[1] = "Description";
            frmSelectorMethods.coldatapropertyname[1] = "descrip";
            frmSelectorMethods.colwidth[1] = 250;
            frmSelectorMethods.colname[2] = "Dropdaycol";
            frmSelectorMethods.colheadertext[2] = "Drop Day";
            frmSelectorMethods.coldatapropertyname[2] = "dropday";
            frmSelectorMethods.colwidth[2] = 90;

            frmSelectorMethods.SetGrid();
            frmSelector.Width = 1250;
            int  scheduledayid = frmSelectorMethods.ShowSelector();

            // If a row has been selected fill the data and process
            if (scheduledayid > 0)
            {
                ssprocessds.scheduleday.Rows.Clear();
                ClearParameters();
                AddParms("@idcol" ,scheduledayid, "SQL");
                commandtext = "SELECT * FROM scheduleday WHERE idcol = @idcol";
                FillData(ssprocessds, "scheduleday", commandtext, CommandType.Text);
                currentidcol = scheduledayid;
                comboboxScheduleDay.SelectedItem =  Convert.ToString(ssprocessds.scheduleday[0].dropday);
                CurrentState = "View";
                RefreshControls();
            }
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {

            bool saveOK = true;

            if (ssprocessds.scheduleday[0].descrip.TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("There must be a description");
                saveOK = false;
            }

            if (saveOK)
            {
                if (ssprocessds.scheduleday[0].schday.TrimEnd() == "")
                {
                    wsgUtilities.wsgNotice("There must be a day code");
                    saveOK = false;
                }
            }

            if (saveOK)
            {

                if (ssprocessds.scheduleday[0].idcol < 1)
                {
                    ssprocesssearchds.scheduleday.Rows.Clear();
                    ClearParameters();
                    AddParms("@schday", ssprocessds.scheduleday[0].schday, "SQL");
                    string commandtext = "SELECT * FROM scheduleday WHERE schday = @schday";
                    FillData(ssprocesssearchds, "scheduleday", commandtext, CommandType.Text);
                    if (ssprocesssearchds.scheduleday.Rows.Count > 0)
                    {
                        wsgUtilities.wsgNotice("That day is already in the file");
                        saveOK = false;
                    }

                }
            }
            if (saveOK)
            {
                // Save the table
                // Prevent overflows
                ssprocessds.scheduleday[0].dropday = Convert.ToString(comboboxScheduleDay.SelectedItem).PadRight(3);
                ssprocessds.scheduleday[0].descrip =   ssprocessds.scheduleday[0].descrip.PadRight(30).ToUpper();           
                GenerateAppTableRowSave(ssprocessds.scheduleday[0]);
                base.SaveCurrentDataTable(sender, e);
            }
        }


        public override void SetControls()
        {
            parentForm.buttonDelete.Visible = false;
            int TextTop = 50;
            int LeftStart = 50;
            comboboxScheduleDay.SelectedItem = "SUN";
            // Fill compbobox
            comboboxScheduleDay.Items.Clear();
            comboboxScheduleDay.Items.Add("SUN");
            comboboxScheduleDay.Items.Add("MON");
            comboboxScheduleDay.Items.Add("TUE");
            comboboxScheduleDay.Items.Add("WED");
            comboboxScheduleDay.Items.Add("THU");
            comboboxScheduleDay.Items.Add("FRI");
            comboboxScheduleDay.Items.Add("SAT");
            comboboxScheduleDay.Items.Add("UNK");
            SetUnboundComboBox(comboboxScheduleDay, LeftStart, TextTop, 80, labelDay, "Day of the week");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 60, ssprocessds.scheduleday, "schday", "Day Code");
            TextTop += 22;
            SetLabelledTextBox(LeftStart, TextTop, 150, ssprocessds.scheduleday, "descrip", "Description");
            TextTop += 22;
        
        }
    }
}
