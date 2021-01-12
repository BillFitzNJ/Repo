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
    public class MaintainCompanyMethods : FrmMaintainSingleTableMethods
    {
        VFPArdataMethods vfpArdatamethods = new VFPArdataMethods();
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess ssprocesstestds = new ssprocess();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Company");
        string commandtext = "";
        string currentcomp_code = "";
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();
        TextBox TextBoxCompcode = new TextBox();
        TextBox TextBoxName = new TextBox();
        TextBox TextBoxBillname = new TextBox();
        TextBox TextBoxAddress = new TextBox();
        TextBox TextBoxAddress_2 = new TextBox();
        TextBox TextBoxCity = new TextBox();
        TextBox TextBoxContract = new TextBox();
        TextBox TextBoxState = new TextBox();
        TextBox TextBoxZip = new TextBox();
        TextBox TextBoxAttention = new TextBox();
        Label LabelBankfedid = new Label();
        Label LabelCompcode = new Label();
        Label LabelName = new Label();
        Label LabelBillname = new Label();
        Label LabelAddress = new Label();
        Label LabelAddress_2 = new Label();
        Label LabelCity = new Label();
        Label LabelContract = new Label();
        Label LabelState = new Label();
        Label LabelZip = new Label();
        Label LabelAttention = new Label();
        TextBox TextBoxBankfedid = new TextBox();
        TextBox TextBoxAccount = new TextBox();
        Label LabelAccount = new Label();
        TextBox TextBoxCustno = new TextBox();
        Label LabelCustno = new Label();
        TextBox TextBoxBankno = new TextBox();
        Label LabelBankno = new Label();
        TextBox TextBoxFtpaddress = new TextBox();
        Label LabelFtpaddress = new Label();
        TextBox TextBoxDestname = new TextBox();
        Label LabelDestname = new Label();
        TextBox TextBoxServerport = new TextBox();
        Label LabelServerport = new Label();
        TextBox TextBoxUsername = new TextBox();
        Label LabelUsername = new Label();
        TextBox TextBoxPassword = new TextBox();
        Label LabelPassword = new Label();
        TextBox TextBoxFuel_rate = new TextBox();
        Label LabelFuel_rate = new Label();
        TextBox TextBoxCompliance_rate = new TextBox();
        Label LabelCompliance_rate = new Label();
        TextBox TextBoxDeptickfee = new TextBox();
        Label LabelDeptickfee = new Label();
        TextBox TextBoxAtmslcom1 = new TextBox();
        Label LabelAtmslcom1 = new Label();
        TextBox TextBoxAtmslcom2 = new TextBox();
        Label LabelAtmslcom2 = new Label();
        TextBox TextBoxAtmdrfcom = new TextBox();
        Label LabelAtmdrfcom = new Label();
        TextBox TextBoxAtmrepfir = new TextBox();
        Label LabelAtmrepfir = new Label();
        TextBox TextBoxAtmrepsec = new TextBox();
        Label LabelAtmrepsec = new Label();
        TextBox TextBoxAtmsls1 = new TextBox();
        TextBox TextBoxAtmsls2 = new TextBox();
        TextBox TextBoxSls1 = new TextBox();
        TextBox TextBoxSls2 = new TextBox();
        TextBox TextBoxCommrate1 = new TextBox();
        TextBox TextBoxCommrate2 = new TextBox();
 
        Label LabelAtmsls1 = new Label();
        Label LabelAtmsls2 = new Label();
        TextBox TextBoxAtmfillrate = new TextBox();
        TextBox TextBoxAtmemerfillrate = new TextBox();
        Label LabelAtmfillrate = new Label();
        Label LabelAtmemerfillrate = new Label();
        TextBox TextBoxAtmunschedfillrate = new TextBox();
        Label LabelAtmunschedfillrate = new Label();
        TextBox TextBoxSigaddr = new TextBox();
        Label LabelSigaddr = new Label();
        TextBox TextBoxCompgroup = new TextBox();
        Label LabelCompgroup = new Label();
        TextBox TextBoxCoinsurcharge = new TextBox();
        Label LabelCoinsurcharge = new Label();
        TextBox TextBoxStraprate = new TextBox();
        Label LabelStraprate = new Label();
        TextBox TextBoxRollrate = new TextBox();
        Label LabelRollrate = new Label();
        TextBox TextBoxDepositrate = new TextBox();
        Label LabelDepositrate = new Label();
        CheckBox CheckBoxInactive = new CheckBox();
        Label LabelInactive = new Label();
        CheckBox CheckBoxIncludeindeposits = new CheckBox();
        Label LabelIncludeindeposits = new Label();
        CheckBox CheckBoxSigbill = new CheckBox();
        Label LabelSigbill = new Label();
        CheckBox CheckBoxSigcbill = new CheckBox();
        Label LabelSigcbill = new Label();
        CheckBox CheckBoxSigdbill = new CheckBox();
        Label LabelSigdbill = new Label();
        CheckBox CheckBoxAtmowner = new CheckBox();
        Label LabelAtmowner = new Label();
        CheckBox CheckBoxSpreadsheet = new CheckBox();
        Label LabelSpreadsheet = new Label();
        CheckBox CheckBoxTextfile = new CheckBox();
        Label LabelTextfile = new Label();
        CheckBox CheckBoxEmail = new CheckBox();
        Label LabelEmail = new Label();
        CheckBox CheckBoxFax = new CheckBox();
        Label LabelFax = new Label();
        CheckBox CheckBoxCheckinfo = new CheckBox();
        Label LabelCheckinfo = new Label();
        CheckBox CheckBoxSepdeposit = new CheckBox();
        Label LabelSepdeposit = new Label();
        CheckBox CheckBoxDailypickupreports = new CheckBox();
        Label LabelDailypickupreports = new Label();
        CheckBox CheckBoxWeeklypickupreports = new CheckBox();
        Label LabelWeeklypickupreports = new Label();
        CheckBox CheckBoxSummsub = new CheckBox();
        Label LabelSummsub = new Label();
        CheckBox CheckBoxFtp = new CheckBox();
        Label LabelFtp = new Label();
        CheckBox CheckBoxPdf = new CheckBox();
        Label LabelPdf = new Label();
        Label LabelInvoiceSpreadsheet = new Label();
        Label LabelLabel = new Label();
        Label Labelnolabel = new Label();       
        CheckBox CheckBoxInvoiceSpreadsheet = new CheckBox();
        Label LabelInvoicePdf = new Label();
        CheckBox CheckBoxInvoicePdf = new CheckBox();
        CheckBox CheckBoxLabel = new CheckBox();
        CheckBox CheckBoxShowacct = new CheckBox();
        Label LabelShowacct = new Label();
  
        Button ButtonForward = new Button();
        Button ButtonBack = new Button();
        Button ButtonCompgroup = new Button();
        Button ButtonEmailaddresses = new Button();
        Button ButtonCompanyusers = new Button();
        Button ButtonBankfedid = new Button();
        Button ButtonAtmsls1 = new Button();
        Button ButtonAtmsls2 = new Button();
        Button ButtonSigaddr = new Button();
        Button ButtonSls1 = new Button();
        Button ButtonSls2 = new Button();
        GroupBox groupBoxBillType = new GroupBox();
        RadioButton radioButtonCompanyBill = new RadioButton();
        RadioButton radioButtonStoreBill = new RadioButton();
        GroupBox groupBoxDepositBilling = new GroupBox();
        RadioButton radioButtonDepositsMainBill = new RadioButton();
        RadioButton radioButtonDepositsSeparateCompanyBill = new RadioButton();
        RadioButton radioButtonDepositsSeparateStoreBill = new RadioButton();
        GroupBox groupBoxCoinBilling = new GroupBox();
        RadioButton radioButtonCoinMainBill = new RadioButton();
        RadioButton radioButtonCoinSeparateCompanyBill = new RadioButton();
        RadioButton radioButtonCoinSeparateStoreBill = new RadioButton();


        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.company;
            currentablename = "company";
            SetIdcol(ssprocessds.company.idcolColumn);
            parentForm.Text = "Maintain Company Data";

        }

        public override void SetEvents()
        {
            ButtonBankfedid.Click += new System.EventHandler(ButtonBankFedidClick);
            ButtonEmailaddresses.Click += new System.EventHandler(ProcessEmailaddresses);
            ButtonForward.Click += new System.EventHandler(ProcessForward);
            ButtonBack.Click += new System.EventHandler(ProcessBack);
            ButtonSigaddr.Click += new System.EventHandler(ButtonSigaddr_click);
            ButtonAtmsls1.Click += new System.EventHandler(ButtonAtmsls1_click);
            ButtonAtmsls2.Click += new System.EventHandler(ButtonAtmsls2_click);
            ButtonSls1.Click += new System.EventHandler(ButtonSls1_click);
            ButtonSls2.Click += new System.EventHandler(ButtonSls2_click);
            ButtonCompgroup.Click += new System.EventHandler(ButtonCompgroup_click);
            TextBoxCompcode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            base.SetEvents();
        }

        public void ProcessNewCompany(object sender, EventArgs e)
        {
            if (CurrentState == "Insert")
            {
                bool companyok = true;
                // Validate the new new company code
                if (ssprocessds.company[0].comp_code.TrimEnd().Length != 4)
                {
                    wsgUtilities.wsgNotice("The company code must be four characters in length");
                    companyok = false;
                }

                if (companyok)
                {
                    ssprocesstestds.company.Rows.Clear();
                    commandtext = "SELECT * FROM company where comp_code = @compcode";
                    ClearParameters();
                    AddParms("@compcode", ssprocessds.company[0].comp_code, "SQL");
                    FillData(ssprocesstestds, "company", commandtext, CommandType.Text);
                    if (ssprocesstestds.company.Rows.Count > 0)
                    {
                        wsgUtilities.wsgNotice("There is an existing company with that code");
                        companyok = false;
                    }
                }
                if (companyok)
                {
                    foreach (Control c in parentForm.Controls)
                    {
                        if (c is TextBox)
                            c.Enabled = true;
                    }
                    TextBoxCompcode.Enabled = false;
                    TextBoxCompcode.ReadOnly = true;
                    parentForm.buttonSave.Enabled = true;
                }
            }

        }


        public void ProcessEmailaddresses(object sender, EventArgs e)
        {
            FrmMaintainCompanyEmailAddresses frmMaintainCompanyEmailAddresses = new FrmMaintainCompanyEmailAddresses();
            frmMaintainCompanyEmailAddresses.CurrentCompCode = ssprocessds.company[0].comp_code;
            frmMaintainCompanyEmailAddresses.ShowParent();
        }
        public void ButtonAtmsls1_click(object sender, EventArgs e)
        {
            string selectedsalesperson = commonAppDataMethods.SelectSalePerson();
            if (selectedsalesperson.Length > 0)
            {
                ssprocessds.company[0].atmsls1 = selectedsalesperson;
            }
            else
            {
                ssprocessds.company[0].atmsls1 = "";
            }
        }

        public void ButtonSls1_click(object sender, EventArgs e)
        {
            string selectedsalesperson = commonAppDataMethods.SelectSalePerson();
            if (selectedsalesperson.Length > 0)
            {
                ssprocessds.company[0].sls1 = selectedsalesperson;
            }
            else
            {
                ssprocessds.company[0].sls1 = "";
            }
        }

        public void ButtonSls2_click(object sender, EventArgs e)
        {
            string selectedsalesperson = commonAppDataMethods.SelectSalePerson();
            if (selectedsalesperson.Length > 0)
            {
                ssprocessds.company[0].sls2 = selectedsalesperson;
            }
            else
            {
                ssprocessds.company[0].sls2 = "";
            }
        }
        public void ButtonCompgroup_click(object sender, EventArgs e)
        {

            string selectedcompany = commonAppDataMethods.SelectCompany();
            if (selectedcompany.Length > 0)
            {
                ssprocessds.company[0].compgroup = selectedcompany;
            }
            else
            {
                ssprocessds.company[0].compgroup = "";
            }
        }

        public void ButtonAtmsls2_click(object sender, EventArgs e)
        {
            string selectedsalesperson = commonAppDataMethods.SelectSalePerson();
            if (selectedsalesperson.Length > 0)
            {
                ssprocessds.company[0].atmsls2 = selectedsalesperson;
            }
            else
            {
                ssprocessds.company[0].atmsls2 = "";
            }
        }

        public void ProcessBack(object sender, EventArgs e)
        {

            ssprocesstestds.company.Rows.Clear();
            commandtext = "SELECT TOP 1.* FROM company where comp_code < @compcode ORDER BY comp_code DESC";
            ClearParameters();
            AddParms("@compcode", ssprocessds.company[0].comp_code, "SQL");
            FillData(ssprocesstestds, "company", commandtext, CommandType.Text);
            if (ssprocesstestds.company.Rows.Count > 0)
            {
                ssprocessds.company.Rows.Clear();
                ssprocessds.company.ImportRow(ssprocesstestds.company[0]);
                RefreshRadioButtons();
            }
            else
            {
                wsgUtilities.wsgNotice("You have reached the beginning of the file");
            }


        }

        public void ProcessForward(object sender, EventArgs e)
        {
            ssprocesstestds.company.Rows.Clear();
            commandtext = "SELECT TOP 1.* FROM company where comp_code > @compcode ORDER BY comp_code ASC";
            ClearParameters();
            AddParms("@compcode", ssprocessds.company[0].comp_code, "SQL");
            FillData(ssprocesstestds, "company", commandtext, CommandType.Text);
            if (ssprocesstestds.company.Rows.Count > 0)
            {
                ssprocessds.company.Rows.Clear();
                ssprocessds.company.ImportRow(ssprocesstestds.company[0]);
                RefreshRadioButtons();
            }
            else
            {
                wsgUtilities.wsgNotice("You have reached the end of the file");
            }

        }
        public void ButtonSigaddr_click(object sender, EventArgs e)
        {
            string selectedcompany = commonAppDataMethods.SelectCompany();
            if (selectedcompany.Length > 0)
            {
                ssprocessds.company[0].sigaddr = selectedcompany;
            }
            else
            {
                ssprocessds.company[0].sigaddr = "";
            }
        }

        public void RefreshRadioButtons()
        {
            // Bill Type
            if (ssprocessds.company[0].billtype == "S")
            {
                radioButtonStoreBill.Checked = true;
            }
            else
            {
                radioButtonCompanyBill.Checked = true;

            }
            // Deposits Billing
            switch (ssprocessds.company[0].depositsbilltype)
            {
                case "S":
                    {
                        radioButtonDepositsSeparateStoreBill.Checked = true;
                        break;
                    }
                case "C":
                    {
                        radioButtonDepositsSeparateCompanyBill.Checked = true;
                        break;
                    }
                default:
                    {
                        radioButtonDepositsMainBill.Checked = true;
                        break;

                    }
            }
            // Coin Billing
            switch (ssprocessds.company[0].coinbilltype)
            {
                case "S":
                    {
                        radioButtonCoinSeparateStoreBill.Checked = true;
                        break;
                    }
                case "C":
                    {
                        radioButtonCoinSeparateCompanyBill.Checked = true;
                        break;
                    }
                default:
                    {
                        radioButtonCoinMainBill.Checked = true;
                        break;

                    }
            }
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            currentidcol = 0;
            string selectedcompany = commonAppDataMethods.SelectCompany();

            // If a row has been selected fill the data and process
            if (selectedcompany.TrimEnd().Length > 0)
            {
                ssprocessds.company.Rows.Clear();
                commandtext = "SELECT * FROM company WHERE comp_code = @comp_code";
                ClearParameters();
                AddParms("@comp_code", selectedcompany, "SQL");
                FillData(ssprocessds, "company", commandtext, CommandType.Text);
                currentidcol = ssprocessds.company[0].idcol;
                currentcomp_code = ssprocessds.company[0].comp_code;
                CurrentState = "View";
                RefreshRadioButtons();
                RefreshControls();
            }
        }

        public override void RefreshControls()
        {
            base.RefreshControls();
            ButtonEmailaddresses.Enabled = false;
            TextBoxBankfedid.ReadOnly = true;
            TextBoxCompcode.ReadOnly = true;
            TextBoxCustno.ReadOnly = true;
            TextBoxAtmsls1.ReadOnly = true;
            TextBoxAtmsls2.ReadOnly = true;
            TextBoxSls1.ReadOnly = true;
            TextBoxSls2.ReadOnly = true;
            TextBoxSigaddr.ReadOnly = true;
            TextBoxCompgroup.ReadOnly = true;
            switch (CurrentState)
            {

                case "View":
                    {
                        ButtonForward.Enabled = true;
                        ButtonBack.Enabled = true;
                        break;
                    }

                case "Edit":
                    {
                        ButtonEmailaddresses.Enabled = true;
                        ButtonCompanyusers.Enabled = true;
                        ButtonBankfedid.Enabled = true;
                        ButtonAtmsls1.Enabled = true;
                        ButtonAtmsls2.Enabled = true;
                        ButtonSls1.Enabled = true;
                        ButtonSls2.Enabled = true;
                        ButtonSigaddr.Enabled = true;
                        ButtonCompgroup.Enabled = true;
                        groupBoxDepositBilling.Enabled = true;
                        groupBoxCoinBilling.Enabled = true;
                        groupBoxBillType.Enabled = true;
                        break;
                    }

                case "Insert":
                    {
                        TextBoxCompcode.Enabled = true;
                        TextBoxCompcode.ReadOnly = false;
                        TextBoxCompcode.Focus();
                        ButtonEmailaddresses.Enabled = true;
                        ButtonCompanyusers.Enabled = true;
                        ButtonBankfedid.Enabled = true;
                        ButtonAtmsls1.Enabled = true;
                        ButtonAtmsls2.Enabled = true;
                        ButtonSls1.Enabled = true;
                        ButtonSls2.Enabled = true;
                        ButtonSigaddr.Enabled = true;
                        ButtonCompgroup.Enabled = true;
                        groupBoxDepositBilling.Enabled = true;
                        groupBoxCoinBilling.Enabled = true;
                        groupBoxBillType.Enabled = true;
                        break;
                    }

                default:
                    {
                        ButtonBack.Enabled = false;
                        ButtonForward.Enabled = false;
                        break;
                    }
            }

        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.company);
            RefreshRadioButtons();
            base.SetInsertState(sender, e);
        }

        public void ButtonBankFedidClick(object sender, EventArgs e)
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
                ssprocessds.company[0].bankfedid = ssprocessds.bank[0].bankfedid;
            }
            else
            {
                ssprocessds.company[0].bankfedid = "";
            }
        }

        public override void SetCurrencyTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataSet ds, string TColumnname, Label TLabel, string LText)
        {
            base.SetCurrencyTextBox(TBox, TLeft, TTop, TWidth, ds, TColumnname, TLabel, LText);
            TLabel.Left = TLeft;
            TLabel.Text = LText.TrimEnd();
            TLabel.Width = 120;
            TLabel.Top = TTop;
            TBox.ReadOnly = false;
            TBox.TextAlign = HorizontalAlignment.Right;
            TLabel.Font = LabelName.Font;
            TBox.Left = TLeft + 125;
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;
            // Validations

            if (ssprocessds.company[0].idcol < 1)
            {

                int number3 = 0;
                cont = int.TryParse(ssprocessds.company[0].comp_code, out number3);
                if (cont != true)
                {
                    wsgUtilities.wsgNotice("The company code must contain four numbers");
                }

                // Validate the new new company code
                if (ssprocessds.company[0].comp_code.TrimEnd().Length != 4)
                {
                    wsgUtilities.wsgNotice("The company code must be four characters in length");
                    cont = false;
                }

                if (cont)
                {
                    ssprocesstestds.company.Rows.Clear();
                    commandtext = "SELECT * FROM company where comp_code = @compcode";
                    ClearParameters();
                    AddParms("@compcode", ssprocessds.company[0].comp_code, "SQL");
                    FillData(ssprocesstestds, "company", commandtext, CommandType.Text);
                    if (ssprocesstestds.company.Rows.Count > 0)
                    {
                        wsgUtilities.wsgNotice("There is an existing company with that code");
                        cont = false;
                    }
                }

            }
            if (cont)
            {
                if (ssprocessds.company[0].name.TrimEnd().Length < 5)
                {
                    wsgUtilities.wsgNotice("Please enter the complete company name");
                    cont = false;
                }
            }

            // Translate Radio Buttons

            // Bill Type
            if (radioButtonStoreBill.Checked)
            {
                ssprocessds.company[0].billtype = "S";
            }

            else
            {
                ssprocessds.company[0].billtype = "C";

            }
            // Deposits Billing
            if (radioButtonDepositsSeparateStoreBill.Checked)
            {
                ssprocessds.company[0].depositsbilltype = "S";
            }
            else
            {

                if (radioButtonDepositsSeparateCompanyBill.Checked)
                {
                    ssprocessds.company[0].depositsbilltype = "C";
                }
                else
                {
                    ssprocessds.company[0].depositsbilltype = "M";

                }
            }

            // Coin Billing
            if (radioButtonCoinSeparateStoreBill.Checked)
            {
                ssprocessds.company[0].coinbilltype = "S";
            }
            else
            {

                if (radioButtonCoinSeparateCompanyBill.Checked)
                {
                    ssprocessds.company[0].coinbilltype = "C";
                }
                else
                {
                    ssprocessds.company[0].coinbilltype = "M";

                }
            }

      
            // Trim the customerdda
            ssprocessds.company[0].account = ssprocessds.company[0].account.TrimStart();


            if (cont)
            {

                // Update the arcust table
                if (ssprocessds.company[0].idcol > 0 && ssprocessds.company[0].custno.TrimEnd() != "")
                {
                    vfpArdatamethods.UpdateArcust(ssprocessds.company[0].custno, ssprocessds.company[0].name,
                    ssprocessds.company[0].address, ssprocessds.company[0].city, ssprocessds.company[0].state,
                     ssprocessds.company[0].zip, ssprocessds.company[0].attention, ssprocessds.company[0].phone);
                }
                  else
                {
                    // Capture the customer number being created in VFP 
                    ssprocessds.company[0].custno = vfpArdatamethods.CreateArcust(ssprocessds.company[0].name,
                    ssprocessds.company[0].address, ssprocessds.company[0].city, ssprocessds.company[0].state,
                    ssprocessds.company[0].zip, ssprocessds.company[0].attention, ssprocessds.company[0].phone);
                }
                // Save the company table
                GenerateAppTableRowSave(ssprocessds.company[0]);
                CurrentState = "View";
                RefreshControls();
            }
        }
        public override void SetControls()
        {

            int TextTop = 50;
            int LeftStart = 50;
            parentForm.Width = 840;
            parentForm.Height = 750;

            //Bill Type
            radioButtonCompanyBill.Text = "Company Bill";
            radioButtonCompanyBill.Location = new Point(5, 14);
            groupBoxBillType.Controls.Add(radioButtonCompanyBill);
            radioButtonStoreBill.Text = "Store Bill";
            radioButtonStoreBill.Location = new Point(5, 36);
            groupBoxBillType.Controls.Add(radioButtonStoreBill);

            //Deposits Billing
            radioButtonDepositsMainBill.Text = "Main Bill";
            radioButtonDepositsMainBill.Location = new Point(5, 14);
            groupBoxDepositBilling.Controls.Add(radioButtonDepositsMainBill);
            radioButtonDepositsSeparateCompanyBill.Text = "Separate Company Bill";
            radioButtonDepositsSeparateCompanyBill.Location = new Point(5, 36);
            radioButtonDepositsSeparateCompanyBill.AutoSize = true;
            groupBoxDepositBilling.Controls.Add(radioButtonDepositsSeparateCompanyBill);
            radioButtonDepositsSeparateStoreBill.Text = "Separate Store Bill";
            radioButtonDepositsSeparateStoreBill.AutoSize = true;
            radioButtonDepositsSeparateStoreBill.Location = new Point(5, 54);
            groupBoxDepositBilling.Controls.Add(radioButtonDepositsSeparateStoreBill);

            //Coin Billing
            radioButtonCoinMainBill.Text = "Main Bill";
            radioButtonCoinMainBill.Location = new Point(5, 14);
            groupBoxCoinBilling.Controls.Add(radioButtonCoinMainBill);
            radioButtonCoinSeparateCompanyBill.Text = "Separate Company Bill";
            radioButtonCoinSeparateCompanyBill.Location = new Point(5, 36);
            radioButtonCoinSeparateCompanyBill.AutoSize = true;
            groupBoxCoinBilling.Controls.Add(radioButtonCoinSeparateCompanyBill);
            radioButtonCoinSeparateStoreBill.Text = "Separate Store Bill";
            radioButtonCoinSeparateStoreBill.AutoSize = true;
            radioButtonCoinSeparateStoreBill.Location = new Point(5, 54);
            groupBoxCoinBilling.Controls.Add(radioButtonCoinSeparateStoreBill);

            // Establish any custom controls here 
            ButtonEmailaddresses.Text = "Email Addresses";
            ButtonEmailaddresses.Height = parentForm.buttonSave.Height;
            ButtonEmailaddresses.Width = 100;
            ButtonEmailaddresses.Top = TextTop;
            ButtonEmailaddresses.Left = LeftStart;
            parentForm.Controls.Add(ButtonEmailaddresses);
            ButtonCompanyusers.Text = "Company Users";
            ButtonCompanyusers.Height = parentForm.buttonSave.Height;
            ButtonCompanyusers.Width = 100;
            ButtonCompanyusers.Top = TextTop;
            ButtonCompanyusers.Left = LeftStart + 100;
            parentForm.Controls.Add(ButtonCompanyusers);
            ButtonForward.Text = "Forward";
            ButtonForward.Height = parentForm.buttonSave.Height;
            ButtonForward.Width = 75;
            ButtonForward.Top = TextTop;
            ButtonForward.Left = LeftStart + 250;
            parentForm.Controls.Add(ButtonForward);
            ButtonBack.Text = "Back";
            ButtonBack.Height = parentForm.buttonSave.Height;
            ButtonBack.Width = 75;
            ButtonBack.Top = TextTop;
            ButtonBack.Left = LeftStart + 325;
            parentForm.Controls.Add(ButtonBack);


            TextTop += 35;

            SetTextBox(TextBoxCompcode, LeftStart, TextTop, 35, ssprocessds.company, "comp_code", LabelCompcode, "Company Code");
            TextBoxCompcode.MaxLength = 4;
            TextTop += 22;
            SetTextBox(TextBoxName, LeftStart, TextTop, 175, ssprocessds.company, "name", LabelName, "Company Name");
            TextBoxName.MaxLength = 40;
            SetTextBox(TextBoxCustno, LeftStart + 350, TextTop, 75, ssprocessds.company, "custno", LabelCustno, "SBT Customer");
            SetGroupBox(groupBoxBillType, LeftStart + 555, TextTop, 100, 65, "Bill Type");
            TextTop += 22;
            SetTextBox(TextBoxBillname, LeftStart, TextTop, 175, ssprocessds.company, "billname", LabelBillname, "Billing Name");
            TextBoxBillname.MaxLength = 40;
            SetCurrencyTextBox(TextBoxAtmfillrate, LeftStart + 350, TextTop, 75, ssprocessds, "company.atmfillrate", LabelAtmfillrate, "ATM Fill Rate");
            TextTop += 22;
            SetTextBox(TextBoxAddress, LeftStart, TextTop, 200, ssprocessds.company, "address", LabelAddress, "Company Address");
            TextBoxAddress.MaxLength = 40;
            SetCurrencyTextBox(TextBoxAtmemerfillrate, LeftStart + 350, TextTop, 75, ssprocessds, "company.atmemerfillrate", LabelAtmemerfillrate, "ATM Emer Fill Rate");
            TextTop += 22;
            SetTextBox(TextBoxAddress_2, LeftStart, TextTop, 200, ssprocessds.company, "address_2", LabelAddress_2, "Address Line 2");
            TextBoxAddress_2.MaxLength = 40;
            SetGroupBox(groupBoxDepositBilling, LeftStart + 555, TextTop, 150, 80, "Bill Deposits");
            SetCurrencyTextBox(TextBoxAtmunschedfillrate, LeftStart + 350, TextTop, 75, ssprocessds, "company.atmunschedfillrate", LabelAtmunschedfillrate, "ATM Unsched Fill Rate");
            TextTop += 22;
            SetTextBox(TextBoxCity, LeftStart, TextTop, 200, ssprocessds.company, "city", LabelCity, "City");
            SetCurrencyTextBox(TextBoxFuel_rate, LeftStart + 350, TextTop, 75, ssprocessds, "company.fuel_rate", LabelFuel_rate, "Fuel Rate");
            TextTop += 22;
            SetTextBox(TextBoxState, LeftStart, TextTop, 25, ssprocessds.company, "state", LabelState, "State");
            TextBoxState.MaxLength = 2;
            SetCurrencyTextBox(TextBoxCoinsurcharge, LeftStart + 350, TextTop, 75, ssprocessds, "company.coinsurcharge", LabelCoinsurcharge, "Coin Surcharge");
            TextTop += 22;
            SetTextBox(TextBoxZip, LeftStart, TextTop, 50, ssprocessds.company, "zip", LabelZip, "Zip");
            TextBoxZip.MaxLength = 5;
            SetCurrencyTextBox(TextBoxAtmslcom1, LeftStart + 350, TextTop, 75, ssprocessds, "company.atmslcom1", LabelAtmslcom1, "ATM Sls 1");
            TextTop += 22;
            SetTextBox(TextBoxAttention, LeftStart, TextTop, 200, ssprocessds.company, "attention", LabelAttention, "Attention");
            SetCurrencyTextBox(TextBoxAtmslcom2, LeftStart + 350, TextTop, 75, ssprocessds, "company.atmslcom2", LabelAtmslcom2, "ATM Sls 2");
            SetGroupBox(groupBoxCoinBilling, LeftStart + 555, TextTop, 150, 80, "Bill Coin");
            TextTop += 22;
            SetCurrencyTextBox(TextBoxAtmdrfcom, LeftStart + 350, TextTop, 75, ssprocessds, "company.atmdrfcom", LabelAtmdrfcom, "ATM Dr Fill");
            TextTop += 22;
            SetTextBox(TextBoxBankno, LeftStart, TextTop, 100, ssprocessds.company, "bankno", LabelBankno, "Bank Customer");
            SetCurrencyTextBox(TextBoxAtmrepfir, LeftStart + 350, TextTop, 75, ssprocessds, "company.atmrepfir", LabelAtmrepfir, "ATM Rep Fir");
            TextTop += 22;
            SetTextBox(TextBoxFtpaddress, LeftStart, TextTop, 200, ssprocessds.company, "ftpaddress", LabelFtpaddress, "FTP Address");
            SetCurrencyTextBox(TextBoxAtmrepsec, LeftStart + 350, TextTop, 75, ssprocessds, "company.atmrepsec", LabelAtmrepsec, "ATM Rep Sec");
            TextTop += 22;
            SetTextBox(TextBoxContract, LeftStart, TextTop, 75, ssprocessds.company, "contract", LabelContract, "Contract");
            SetCurrencyTextBox(TextBoxCompliance_rate, LeftStart + 350, TextTop, 75, ssprocessds, "company.compliance_rate", LabelCompliance_rate, "Compliance Rate");
            TextTop += 22;
            SetTextBox(TextBoxDestname, LeftStart, TextTop, 200, ssprocessds.company, "destname", LabelDestname, "Destination Name");
            SetCurrencyTextBox(TextBoxStraprate, LeftStart + 350, TextTop, 75, ssprocessds, "company.straprate", LabelStraprate, "Strap Rate");
            TextTop += 22;
            SetTextBox(TextBoxServerport, LeftStart, TextTop, 75, ssprocessds.company, "serverport", LabelServerport, "Server Port");
            SetCurrencyTextBox(TextBoxRollrate, LeftStart + 350, TextTop, 75, ssprocessds, "company.rollrate", LabelRollrate, "Roll Rate");
            TextTop += 22;
            SetTextBox(TextBoxUsername, LeftStart, TextTop, 200, ssprocessds.company, "username", LabelUsername, "User Name");
            SetCurrencyTextBox(TextBoxDepositrate, LeftStart + 350, TextTop, 75, ssprocessds, "company.deporate", LabelDepositrate, "Deposit Rate");
            TextTop += 22;
            SetCurrencyTextBox(TextBoxDeptickfee, LeftStart + 350, TextTop, 75, ssprocessds, "company.deptickfee", LabelDeptickfee, "Dep Ticket Fee");
            TextTop += 22;
            ButtonBankfedid.Height = parentForm.buttonSave.Height - 3;
            ButtonBankfedid.Width = 100;
            ButtonBankfedid.Top = TextTop;
            ButtonBankfedid.Left = LeftStart + 335;
            ButtonBankfedid.Text = "Bank Fed Id";
            SetTextBox(TextBoxBankfedid, LeftStart, TextTop, 100, ssprocessds.company, "bankfedid", LabelBankfedid, "");
            parentForm.Controls.Add(ButtonBankfedid);
            // Override the left setting 
            TextBoxBankfedid.Left = LeftStart + 475;
            SetTextBox(TextBoxPassword, LeftStart, TextTop, 200, ssprocessds.company, "password", LabelPassword, "Password");
            SetTextBox(TextBoxAccount, LeftStart + 350, TextTop, 100, ssprocessds.company, "account", LabelAccount, "BankAccount");
            TextTop += 22;
            TextTop += 22;
            // Checkboxes
            SetBitCheckBox(CheckBoxInactive, LeftStart, TextTop, 150, ssprocessds.company, "inactive", LabelInactive, "Company is inactive");
            SetBitCheckBox(CheckBoxIncludeindeposits, LeftStart + 175, TextTop, 150, ssprocessds.company, "includeindeposits", LabelIncludeindeposits, "Include in deposits");
            // ATM Sis buttons
            ButtonAtmsls1.Height = parentForm.buttonSave.Height - 3;
            ButtonAtmsls1.Width = 100;
            ButtonAtmsls1.Top = TextTop;
            ButtonAtmsls1.Left = LeftStart + 335;
            ButtonAtmsls1.Text = "ATM Sls 1";
            SetTextBox(TextBoxAtmsls1, LeftStart, TextTop, 35, ssprocessds.company, "atmsls1", LabelAtmsls1, "");
            // Override the left setting 
            TextBoxAtmsls1.Left = LeftStart + 475;
            parentForm.Controls.Add(ButtonAtmsls1);

            // Salesperson  button
            ButtonSls1.Height = parentForm.buttonSave.Height - 3;
            ButtonSls1.Width = 75;
            ButtonSls1.Top = TextTop;
            ButtonSls1.Left = LeftStart + 525;
            ButtonSls1.Text = "Sales 1";
            SetTextBox(TextBoxSls1, LeftStart + 610, TextTop, 35, ssprocessds.company, "sls1", Labelnolabel, "");
            SetCurrencyTextBox(TextBoxCommrate1, LeftStart + 615, TextTop, 75, ssprocessds, "company.slscomm1", Labelnolabel, "");
            TextBoxSls1.Left = LeftStart + 610;
            TextBoxCommrate1.Left = LeftStart + 650;
            parentForm.Controls.Add(ButtonSls1);
          
            TextTop += 22;
            SetBitCheckBox(CheckBoxSigbill, LeftStart, TextTop, 150, ssprocessds.company, "sigbill", LabelSigbill, "Signature Bill");
            SetBitCheckBox(CheckBoxSigcbill, LeftStart + 175, TextTop, 150, ssprocessds.company, "sigcbill", LabelSigcbill, "Signature Coin Bill");
            ButtonAtmsls2.Height = parentForm.buttonSave.Height - 3;
            ButtonAtmsls2.Width = 100;
            ButtonAtmsls2.Top = TextTop;
            ButtonAtmsls2.Left = LeftStart + 335;
            ButtonAtmsls2.Text = "ATM Sls 2";
            SetTextBox(TextBoxAtmsls2, LeftStart, TextTop, 35, ssprocessds.company, "atmsls2", LabelAtmsls2, "");
            parentForm.Controls.Add(ButtonAtmsls2);
            TextBoxAtmsls2.Left = LeftStart + 475;
            // Salesperson 2 button
            ButtonSls2.Height = parentForm.buttonSave.Height - 3;
            ButtonSls2.Width = 75;
            ButtonSls2.Top = TextTop;
            ButtonSls2.Left = LeftStart + 525;
            ButtonSls2.Text = "Sales 2";
            SetTextBox(TextBoxSls2, LeftStart + 610, TextTop, 35, ssprocessds.company, "sls2", Labelnolabel, "");
            SetCurrencyTextBox(TextBoxCommrate2, LeftStart + 615, TextTop, 75, ssprocessds, "company.slscomm2", Labelnolabel, "");
            TextBoxSls2.Left = LeftStart + 610;
            TextBoxCommrate2.Left = LeftStart + 650;
            parentForm.Controls.Add(ButtonSls2);
        

            TextTop += 22;
            SetBitCheckBox(CheckBoxSigdbill, LeftStart, TextTop, 150, ssprocessds.company, "sigdbill", LabelSigdbill, "Signature Deposits Bill");
            SetBitCheckBox(CheckBoxAtmowner, LeftStart + 175, TextTop, 150, ssprocessds.company, "atmowner", LabelAtmowner, "ATM Owner");
            ButtonSigaddr.Height = parentForm.buttonSave.Height - 3;
            ButtonSigaddr.Width = 100;
            ButtonSigaddr.Top = TextTop;
            ButtonSigaddr.Left = LeftStart + 335;
            ButtonSigaddr.Text = "Sig Addr";
            SetTextBox(TextBoxSigaddr, LeftStart, TextTop, 35, ssprocessds.company, "sigaddr", LabelSigaddr, "");
            parentForm.Controls.Add(ButtonSigaddr);
            TextBoxSigaddr.Left = LeftStart + 475;
            TextTop += 22;
            SetBitCheckBox(CheckBoxSpreadsheet, LeftStart, TextTop, 150, ssprocessds.company, "spreadsht", LabelSpreadsheet, "Spreadsheet");
            SetBitCheckBox(CheckBoxTextfile, LeftStart + 175, TextTop, 150, ssprocessds.company, "textfile", LabelTextfile, "Text File");
            ButtonCompgroup.Height = parentForm.buttonSave.Height - 3;
            ButtonCompgroup.Width = 100;
            ButtonCompgroup.Top = TextTop;
            ButtonCompgroup.Left = LeftStart + 335;
            ButtonCompgroup.Text = "Group";
            SetTextBox(TextBoxCompgroup, LeftStart, TextTop, 35, ssprocessds.company, "compgroup", LabelCompgroup, "");
            // Override the left setting 
            TextBoxCompgroup.Left = LeftStart + 475;
            parentForm.Controls.Add(ButtonCompgroup);
            TextTop += 22;
            SetBitCheckBox(CheckBoxEmail, LeftStart, TextTop, 150, ssprocessds.company, "email", LabelEmail, "Email");
            SetBitCheckBox(CheckBoxFax, LeftStart + 175, TextTop, 150, ssprocessds.company, "fax", LabelFax, "Fax");
            ButtonBankfedid.Height = parentForm.buttonSave.Height - 3;
            ButtonBankfedid.Width = 100;
            ButtonBankfedid.Top = TextTop;
            ButtonBankfedid.Left = LeftStart + 335;
            ButtonBankfedid.Text = "Bank Fed Id";
            SetTextBox(TextBoxBankfedid, LeftStart, TextTop, 100, ssprocessds.company, "bankfedid", LabelBankfedid, "");
            parentForm.Controls.Add(ButtonBankfedid);
            // Override the left setting 
            TextBoxBankfedid.Left = LeftStart + 475;
            TextTop += 22;
            SetBitCheckBox(CheckBoxCheckinfo, LeftStart, TextTop, 150, ssprocessds.company, "checkinfo", LabelCheckinfo, "Check Information");
            SetBitCheckBox(CheckBoxSepdeposit, LeftStart + 175, TextTop, 150, ssprocessds.company, "sepdeposit", LabelSepdeposit, "Separate Deposit");
            SetBitCheckBox(CheckBoxDailypickupreports, LeftStart + 350, TextTop, 150, ssprocessds.company, "dailypickupreports", LabelDailypickupreports, "Daily Pickup Reports");
           
            TextTop += 22;
            SetBitCheckBox(CheckBoxSummsub, LeftStart, TextTop, 150, ssprocessds.company, "summsub", LabelSummsub, "Summarize Postings");
            SetBitCheckBox(CheckBoxFtp, LeftStart + 175, TextTop, 150, ssprocessds.company, "ftp", LabelFtp, "FTP");
            SetBitCheckBox(CheckBoxWeeklypickupreports, LeftStart + 350, TextTop, 150, ssprocessds.company, "weeklypickupreports", LabelWeeklypickupreports, "Weekly Pickup Reports");
            TextTop += 22;
            SetBitCheckBox(CheckBoxPdf, LeftStart, TextTop, 150, ssprocessds.company, "pdf", LabelPdf, "PDF");
            SetBitCheckBox(CheckBoxInvoiceSpreadsheet, LeftStart + 175, TextTop, 150, ssprocessds.company, "invoicespreadsheet", LabelInvoiceSpreadsheet, "Invoice Spreadsheet");
            SetBitCheckBox(CheckBoxInvoicePdf, LeftStart + 350, TextTop, 150, ssprocessds.company, "invoicepdf", LabelInvoicePdf, "Invoice PDF");
            TextTop += 22;
            SetBitCheckBox(CheckBoxLabel, LeftStart , TextTop, 150, ssprocessds.company, "label", LabelLabel, "Label");
            SetBitCheckBox(CheckBoxShowacct, LeftStart + 175, TextTop, 150, ssprocessds.company, "showacct", LabelShowacct, "Show account");
           
            // Set all textboxes to upper case
            foreach (Control c in parentForm.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).CharacterCasing = CharacterCasing.Upper;
                }
            }
        }
        private void SendTabonEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public void SetDepositBillRadioButtons()
        {
            switch (ssprocessds.company[0].comp_code)
            {
                case "1":
                    radioButtonDepositsMainBill.Checked = true;
                    break;
            }
        }
    }
}
