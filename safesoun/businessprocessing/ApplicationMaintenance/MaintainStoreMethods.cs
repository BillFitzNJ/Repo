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
    public class MaintainStoreMethods : FrmMaintainSingleTableMethods
    {
        MySQLSynchMethods mySQLSynchMethods = new MySQLSynchMethods();
        VFPArdataMethods vfpArdatamethods = new VFPArdataMethods();
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Bank");
        string commandtext = "";
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocessworkingds = new ssprocess();

        // TextBoxes
        TextBox TextBoxSlscode = new TextBox();
        TextBox TextBoxStorecode = new TextBox();
        Label LabelStorecode = new Label();
        TextBox TextBoxFaddress = new TextBox();
        Label LabelFaddress = new Label();
        TextBox TextBoxStorename = new TextBox();
        Label LabelStorename = new Label();
        TextBox TextBoxFcity = new TextBox();
        Label LabelFcity = new Label();
        TextBox TextBoxFstate = new TextBox();
        Label LabelFstate = new Label();
        TextBox TextBoxFzip = new TextBox();
        Label LabelFzip = new Label();
        TextBox TextBoxPhone = new TextBox();
        Label LabelPhone = new Label();
        TextBox TextBoxAttention = new TextBox();
        Label LabelAttention = new Label();
        TextBox TextBoxCustcode = new TextBox();
        Label LabelCustcode = new Label();
        TextBox TextBoxPuaddress = new TextBox();
        Label LabelPuaddress = new Label();
        TextBox TextBoxDname = new TextBox();
        Label LabelDname = new Label();
        TextBox TextBoxDaddress = new TextBox();
        Label LabelDaddress = new Label();
        TextBox TextBoxRtebase = new TextBox();
        Label LabelRtebase = new Label();
        TextBox TextBoxFlatdrop = new TextBox();
        Label LabelFlatdrop = new Label();
        TextBox TextBoxEnvelope = new TextBox();
        Label LabelEnvelope = new Label();
        TextBox TextBoxCgdrprt = new TextBox();
        Label LabelCgdrprt = new Label();
        TextBox TextBoxAtmrate = new TextBox();
        Label LabelAtmrate = new Label();
        TextBox TextBoxPaydlrate = new TextBox();
        Label LabelPaydlrate = new Label();
        TextBox TextBoxPackrate = new TextBox();
        Label LabelPackrate = new Label();
        Label LabelBillmemo = new Label();
        Label LabelStartdate = new Label();
        TextBox TextBoxStartdate = new TextBox();
        Label LabelStopdate = new Label();
        DateTimePicker DateTimePickerStopdate = new DateTimePicker();
        DateTimePicker DateTimePickerStartdate = new DateTimePicker();
        Label LabelCustno = new Label();
        TextBox TextBoxCustno = new TextBox();


        // ComboBoxes
        ComboBox ComboboxTaxarea = new ComboBox();
        Label LabelTaxarea = new Label();

        // CheckBoxes
        CheckBox CheckBoxActive = new CheckBox();
        Label LabelActive = new Label();
        CheckBox CheckBoxNoslip = new CheckBox();
        Label LabelNoslip = new Label();
        CheckBox CheckBoxExternalcoin = new CheckBox();
        Label LabelExternalcoin = new Label();
        CheckBox CheckBoxBankcoin = new CheckBox();
        Label LabelBankcoin = new Label();

        // Buttons
        Button ButtonBillMemo = new Button();
        Button ButtonPuaddress = new Button();
        Button ButtonForward = new Button();
        Button ButtonBack = new Button();
        Button ButtonDropSchedule = new Button();
        Button ButtonSlips = new Button();
        Button ButtonRecurringCharges = new Button();
        Button ButtonRecurringCoin = new Button();
        Button ButtonBillingAddress = new Button();
        Button ButtonEmailAddresses = new Button();
        GetNewStorecodeMethods getNewStoreCodeMethods = new GetNewStorecodeMethods();
        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.store;
            currentablename = "store";
            SetIdcol(ssprocessds.store.idcolColumn);
            parentForm.Text = "Maintain Store Data";

        }

        public override void SetEvents()
        {
            ButtonBack.Click += new System.EventHandler(ProcessBack);
            ButtonForward.Click += new System.EventHandler(ProcessForward);
            ButtonPuaddress.Click += new System.EventHandler(ButtonPuaddress_click);
            ButtonBillMemo.Click += new System.EventHandler(ButtonBillMemo_click);
            ButtonDropSchedule.Click += new System.EventHandler(ButtonDropSchedule_click);
            ButtonSlips.Click += new System.EventHandler(ButtonSlips_click);
            ButtonRecurringCharges.Click += new System.EventHandler(ButtonRecurringCharges_click);
            ButtonBillingAddress.Click += new System.EventHandler(ButtonBillingAddress_click);
            ButtonRecurringCoin.Click += new System.EventHandler(ButtonRecurringCoin_click);
            ButtonEmailAddresses.Click += new System.EventHandler(ProcessStoreEmailaddresses);
            base.SetEvents();
        }
        public override void SetControls()
        {
            // Fill compboboxes 
            ComboboxTaxarea.Items.Add("NEW YORK");
            ComboboxTaxarea.Items.Add("NASSAU");
            ComboboxTaxarea.Items.Add("SUFFOLK");
            ComboboxTaxarea.Items.Add("TAX EXEMPT");
            ComboboxTaxarea.Items.Add("NEW JERSEY");
            ComboboxTaxarea.Items.Add("CONN");
            ComboboxTaxarea.Items.Add("NONTAXABLE");


            int TextTop = 50;
            int LeftStart = 50;
            parentForm.Width = 850;
            parentForm.Height = 575;
            SetTextBox(TextBoxStorecode, LeftStart, TextTop, 75, ssprocessds.store, "storecode", LabelStorecode, "Code");
            ButtonForward.Height = parentForm.buttonSave.Height - 3;
            ButtonForward.Width = 100;
            ButtonForward.Top = TextTop;
            ButtonForward.Left = LeftStart + 475;
            ButtonForward.Text = "Forward";
            parentForm.Controls.Add(ButtonForward);
            ButtonBack.Height = parentForm.buttonSave.Height - 3;
            ButtonBack.Width = 100;
            ButtonBack.Top = TextTop;
            ButtonBack.Left = LeftStart + 575;
            ButtonBack.Text = "Back";
            parentForm.Controls.Add(ButtonBack);
            TextTop += 22;
            SetTextBox(TextBoxStorename, LeftStart, TextTop, 250, ssprocessds.store, "store_name", LabelStorename, "Name");
            ButtonDropSchedule.Height = parentForm.buttonSave.Height;
            ButtonDropSchedule.Width = 150;
            ButtonDropSchedule.Top = TextTop;
            ButtonDropSchedule.Left = LeftStart + 575;
            ButtonDropSchedule.Text = "Drop Schedule";
            parentForm.Controls.Add(ButtonDropSchedule);
            TextTop += 22;
            SetTextBox(TextBoxFaddress, LeftStart, TextTop, 250, ssprocessds.store, "f_address", LabelFaddress, "Address");
            ButtonSlips.Height = parentForm.buttonSave.Height;
            ButtonSlips.Width = 150;
            ButtonSlips.Top = TextTop;
            ButtonSlips.Left = ButtonDropSchedule.Left;
            ButtonSlips.Text = "Slips";
            parentForm.Controls.Add(ButtonSlips);
            TextTop += 22;
            SetTextBox(TextBoxFcity, LeftStart, TextTop, 250, ssprocessds.store, "f_city", LabelFcity, "City");
            ButtonRecurringCharges.Height = parentForm.buttonSave.Height;
            ButtonRecurringCharges.Width = 150;
            ButtonRecurringCharges.Top = TextTop;
            ButtonRecurringCharges.Left = ButtonDropSchedule.Left;
            ButtonRecurringCharges.Text = "Recurring Charges";
            parentForm.Controls.Add(ButtonRecurringCharges);
            TextTop += 22;
            SetTextBox(TextBoxFstate, LeftStart, TextTop, 75, ssprocessds.store, "f_state", LabelFstate, "State");
            SetTextBox(TextBoxCustno, LeftStart + 200, TextTop, 75, ssprocessds.store, "custno", LabelCustno, "SBT Cust");
            TextBoxCustno.Left = LabelCustno.Left + 65;
            ButtonRecurringCoin.Height = parentForm.buttonSave.Height;
            ButtonRecurringCoin.Width = 150;
            ButtonRecurringCoin.Top = TextTop;
            ButtonRecurringCoin.Left = ButtonDropSchedule.Left;
            ButtonRecurringCoin.Text = "Recurring Coin";
            parentForm.Controls.Add(ButtonRecurringCoin);
            TextTop += 22;
            SetTextBox(TextBoxFzip, LeftStart, TextTop, 75, ssprocessds.store, "f_zip", LabelFzip, "Zip");
            ButtonBillingAddress.Height = parentForm.buttonSave.Height;
            ButtonBillingAddress.Width = 150;
            ButtonBillingAddress.Top = TextTop;
            ButtonBillingAddress.Left = ButtonDropSchedule.Left;
            ButtonBillingAddress.Text = "Billing Address";
            parentForm.Controls.Add(ButtonBillingAddress);
            TextTop += 22;
            SetTextBox(TextBoxPhone, LeftStart, TextTop, 150, ssprocessds.store, "phone", LabelPhone, "Phone");
            LabelBillmemo.Height = parentForm.buttonSave.Height;
            LabelBillmemo.Width = 75;
            LabelBillmemo.Top = TextTop  + 5;
            LabelBillmemo.Left = ButtonDropSchedule.Left -80;
            LabelBillmemo.Text = "Store Notes";
            parentForm.Controls.Add(LabelBillmemo);

            ButtonBillMemo.Height = parentForm.buttonSave.Height;
            ButtonBillMemo.Width = 150;
            ButtonBillMemo.Top = TextTop;
            ButtonBillMemo.Left = ButtonDropSchedule.Left;
            ButtonBillMemo.Text = "Store Notes";
            parentForm.Controls.Add(ButtonBillMemo);
            TextTop += 22;
            SetTextBox(TextBoxAttention, LeftStart, TextTop, 150, ssprocessds.store, "attention", LabelAttention, "Attention");
            ButtonEmailAddresses.Height = parentForm.buttonSave.Height;
            ButtonEmailAddresses.Width = 150;
            ButtonEmailAddresses.Top = TextTop;
            ButtonEmailAddresses.Left = ButtonDropSchedule.Left;
            ButtonEmailAddresses.Text = "Email Addresses";
            parentForm.Controls.Add(ButtonEmailAddresses);

            TextTop += 22;
            SetTextBox(TextBoxCustcode, LeftStart, TextTop, 100, ssprocessds.store, "custcode", LabelCustcode, "Customer Code");
            TextTop += 22;
            SetUnboundComboBox(ComboboxTaxarea, LeftStart, TextTop, 100, LabelTaxarea, "Tax Area");
            TextTop += 22;

            SetBitCheckBox(CheckBoxBankcoin, LeftStart + 585, TextTop, 250, ssprocessds.store, "bankcoin", LabelBankcoin, "Bank Coin");
            CheckBoxBankcoin.Left = LabelBankcoin.Left + 90;
            TextTop += 22;
            SetTextBox(TextBoxPuaddress, LeftStart, TextTop, 250, ssprocessds.store, "pu_address", LabelPuaddress, "");
            // Remove the label to make room for the button
            parentForm.Controls.Remove(LabelPuaddress);
            ButtonPuaddress.Height = parentForm.buttonSave.Height - 3;
            ButtonPuaddress.Width = 100;
            ButtonPuaddress.Top = TextTop;
            ButtonPuaddress.Left = LeftStart;
            ButtonPuaddress.Text = "Same PU";
            parentForm.Controls.Add(ButtonPuaddress);

            TextTop += 22;
            SetTextBox(TextBoxDname, LeftStart, TextTop, 250, ssprocessds.store, "d_name", LabelDname, "Bank");
            TextBoxDname.TabIndex = 13;
       
            TextTop += 22;
            SetTextBox(TextBoxDaddress, LeftStart, TextTop, 250, ssprocessds.store, "d_address", LabelDaddress, "Address");
            TextBoxDaddress.TabIndex = 14;

            SetBitCheckBox(CheckBoxNoslip, LeftStart + 450, TextTop, 150, ssprocessds.store, "no_slip", LabelNoslip, "No Slip");
            // Override the checkbox position
            CheckBoxNoslip.Left = LabelNoslip.Left + 50;
            SetBitCheckBox(CheckBoxExternalcoin, LeftStart +585, TextTop, 250, ssprocessds.store, "externalcoin", LabelExternalcoin, "External Coin");
            // Override the checkbox position
            // Code to override the default behavior
            LabelExternalcoin.AutoSize = true;
            LabelExternalcoin.BringToFront();
            CheckBoxExternalcoin.Left = LabelExternalcoin.Left + 90;

            TextTop += 22;
            SetBitCheckBox(CheckBoxActive, LeftStart + 450, TextTop, 150, ssprocessds.store, "active", LabelActive, "Active");
            // Override the checkbox position
            CheckBoxActive.Left = LabelActive.Left + 50;
            TextTop += 32;
            SetCurrencyTextBox(TextBoxRtebase, LeftStart, TextTop, 75, ssprocessds, "store.rte_base", LabelRtebase, "Pickup Rate");
            SetCurrencyTextBox(TextBoxFlatdrop, LeftStart + 225, TextTop, 75, ssprocessds, "store.flatdrop", LabelFlatdrop, "Flat Drop");
            SetCurrencyTextBox(TextBoxEnvelope, LeftStart + 450, TextTop, 75, ssprocessds, "store.envelope", LabelEnvelope, "Envelope");
            TextTop += 22;
            SetCurrencyTextBox(TextBoxCgdrprt, LeftStart, TextTop, 75, ssprocessds, "store.cg_drp_rt", LabelCgdrprt, "Drop Rate");
            TextTop += 22;
            SetCurrencyTextBox(TextBoxAtmrate, LeftStart, TextTop, 75, ssprocessds, "store.atmrate", LabelAtmrate, "ATM Rate");
            SetCurrencyTextBox(TextBoxPaydlrate, LeftStart + 225, TextTop, 75, ssprocessds, "store.paydlrate", LabelPaydlrate, "PR DelRate");
            SetCurrencyTextBox(TextBoxPackrate, LeftStart + 450, TextTop, 75, ssprocessds, "store.packrate", LabelPackrate, "Pkg Rate");
            TextTop += 22;
            LabelStartdate.Text = "Start Date";
            LabelStartdate.Top = TextTop;
            LabelStartdate.Width = 70;
            LabelStartdate.Left = LeftStart;
            parentForm.Controls.Add(LabelStartdate);
            DateTimePickerStartdate.CustomFormat = "";
            this.DateTimePickerStartdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateTimePickerStartdate.Top = TextTop;
            this.DateTimePickerStartdate.Left = LeftStart + 125;
            this.DateTimePickerStartdate.Name = "dateTimePickerCthru";
            this.DateTimePickerStartdate.Size = new System.Drawing.Size(103, 20);
            DateTimePickerStartdate.MaxDate = DateTime.MaxValue;
            parentForm.Controls.Add(DateTimePickerStartdate);


            LabelStopdate.Text = "Stop Date";
            LabelStopdate.Top = TextTop;
            LabelStopdate.Width = 70;
            LabelStopdate.Left = LeftStart + 275;
            parentForm.Controls.Add(LabelStopdate);
            DateTimePickerStopdate.CustomFormat = "";
            this.DateTimePickerStopdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateTimePickerStopdate.Top = TextTop;
            this.DateTimePickerStopdate.Left = LeftStart + 350;
            this.DateTimePickerStopdate.Name = "dateTimePickerCthru";
            this.DateTimePickerStopdate.Size = new System.Drawing.Size(103, 20);
            DateTimePickerStopdate.MaxDate = DateTime.MaxValue;
            parentForm.Controls.Add(DateTimePickerStopdate);


            //     SetDateTimeTextBox(TextBoxStopdate, LeftStart + 225, TextTop, 55, ssprocessds.store, "stop_date", LabelStopdate, "Stop Date");

            // Set textboxes to upper casing  
            foreach (Control c in parentForm.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).CharacterCasing = CharacterCasing.Upper;

                }
                // Set labels to bold
                if (c is Label)
                {

                    (c as Label).Font = new Font(c.Font, FontStyle.Bold);
                }
            }
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            string selectedstorecode = commonAppDataMethods.SelectCompanyAndStore();

            // If a row has been selected fill the data and process
            if (selectedstorecode.TrimEnd().Length > 0)
            {
                ssprocessds.store.Rows.Clear();
                commandtext = "SELECT * FROM  store WHERE storecode = @storecode";
                ClearParameters();
                AddParms("@storecode", selectedstorecode, "SQL");
                FillData(ssprocessds, "store", commandtext, CommandType.Text);
                RefreshDateTimePickers();
                RefreshComboboxes();
                CurrentState = "View";
                RefreshControls();
            }
        }
        public void RefreshComboboxes()
        {
            currentidcol = ssprocessds.store[0].idcol;
            ComboboxTaxarea.SelectedIndex = Convert.ToInt16(ssprocessds.store[0].tax_area - 1);
        }

        public void RefreshDateTimePickers()
        {
            DateTimePickerStopdate.Value = ssprocessds.store[0].stop_date;
            DateTimePickerStartdate.Value = ssprocessds.store[0].start_date;
        }
        public void ButtonBillMemo_click(object sender, EventArgs e)
        {
            MemoMethods memoMethods = new MemoMethods();
            memoMethods.memodata = ssprocessds.store[0].bill_memo;
            memoMethods.frmMemo.Text = commonAppDataMethods.GetStoreName(ssprocessds.store[0].storecode).TrimEnd();
            memoMethods.ShowMemo();
            if (memoMethods.updatememo)
            {
                ssprocessds.store[0].bill_memo = memoMethods.memodata;
            }
        }

       
        public void ButtonDropSchedule_click(object sender, EventArgs e)
        {
            MaintainDropScheduleMethods maintDropScheduleMethods = new MaintainDropScheduleMethods();
            maintDropScheduleMethods.StoreCode = ssprocessds.store[0].storecode;
            maintDropScheduleMethods.RefreshDatagridview();
            maintDropScheduleMethods.CurrentState = "StoreSelected";
            maintDropScheduleMethods.RefreshControls();
            maintDropScheduleMethods.parentForm.ShowDialog();
        }

        public void ProcessStoreEmailaddresses(object sender, EventArgs e)
        {
            FrmMaintainStoreEmailAddresses frmMaintainStoreEmailAddresses = new FrmMaintainStoreEmailAddresses();
            frmMaintainStoreEmailAddresses.CurrentStoreCode = ssprocessds.store[0].storecode;
            frmMaintainStoreEmailAddresses.ShowParent();
        }


        public void ButtonRecurringCharges_click(object sender, EventArgs e)
        {
            MaintainRecurringChargeMethods maintainRecurringChargeMethods = new MaintainRecurringChargeMethods();
            maintainRecurringChargeMethods.Storecode = ssprocessds.store[0].storecode;
            maintainRecurringChargeMethods.RefreshDatagridview();
            maintainRecurringChargeMethods.CurrentState = "StoreSelected";
            maintainRecurringChargeMethods.RefreshControls();
            maintainRecurringChargeMethods.parentForm.ShowDialog();
        }
        public void ButtonRecurringCoin_click(object sender, EventArgs e)
        {
            MaintainRecurringCoinMethods maintainRecurringCoinMethods = new MaintainRecurringCoinMethods();
            maintainRecurringCoinMethods.Storecode = ssprocessds.store[0].storecode;
            maintainRecurringCoinMethods.RefreshDatagridview();
            maintainRecurringCoinMethods.CurrentState = "StoreSelected";
            maintainRecurringCoinMethods.RefreshControls();
            maintainRecurringCoinMethods.parentForm.ShowDialog();
        }
        public void ButtonSlips_click(object sender, EventArgs e)
        {
            MaintainSlipsMethods maintainSlipsMethods = new MaintainSlipsMethods();
            maintainSlipsMethods.Storecode = ssprocessds.store[0].storecode;
            maintainSlipsMethods.CurrentState = "StoreSelected";
            maintainSlipsMethods.RefreshControls();
            maintainSlipsMethods.RefreshDatagridview();
            maintainSlipsMethods.parentForm.ShowDialog();
        }
        public void ButtonBillingAddress_click(object sender, EventArgs e)
        {
            MaintainStoreBillAddressMethods maintainStoreBillAddressMethods = new MaintainStoreBillAddressMethods();
            maintainStoreBillAddressMethods.Storecode = ssprocessds.store[0].storecode;
            maintainStoreBillAddressMethods.LoadBillingAddress();
            maintainStoreBillAddressMethods.parentForm.ShowDialog();
        }
        public void ButtonPuaddress_click(object sender, EventArgs e)
        {
            ssprocessds.store[0].pu_address = ssprocessds.store[0].f_address;
            ssprocessds.store.AcceptChanges();
        }

        public void ButtonSalesmn_click(object sender, EventArgs e)
        {
            string selectedsalesperson = commonAppDataMethods.SelectSalePerson();
            if (selectedsalesperson.Length > 0)
            {
                ssprocessds.store[0].salesmn = selectedsalesperson;
            }
            else
            {
                ssprocessds.store[0].salesmn = "";
            }
        }

        public void ButtonSalesmn2_click(object sender, EventArgs e)
        {
            string selectedsalesperson = commonAppDataMethods.SelectSalePerson();
            if (selectedsalesperson.Length > 0)
            {
                ssprocessds.store[0].salesmn2 = selectedsalesperson;
            }
            else
            {
                ssprocessds.store[0].salesmn2 = "";
            }
        }

        public void ProcessBack(object sender, EventArgs e)
        {

            ssprocessworkingds.store.Rows.Clear();
            commandtext = "SELECT TOP 1.* FROM store where storecode < @storecode AND LEFT(storecode,4) = LEFT(@storecode,4)  ORDER BY storecode DESC";
            ClearParameters();
            AddParms("@storecode", ssprocessds.store[0].storecode, "SQL");
            FillData(ssprocessworkingds, "store", commandtext, CommandType.Text);
            if (ssprocessworkingds.store.Rows.Count > 0)
            {
                ssprocessds.store.Rows.Clear();
                ssprocessds.store.ImportRow(ssprocessworkingds.store[0]);
                RefreshComboboxes();
                RefreshDateTimePickers();
            }
            else
            {
                wsgUtilities.wsgNotice("You have reached the first store for this company");
            }


        }

        public void ProcessForward(object sender, EventArgs e)
        {
            ssprocessworkingds.store.Rows.Clear();
            commandtext = "SELECT TOP 1.* FROM store where storecode > @storecode AND LEFT(storecode,4) = LEFT(@storecode,4)  ORDER BY storecode ASC";
            ClearParameters();
            AddParms("@storecode", ssprocessds.store[0].storecode, "SQL");
            FillData(ssprocessworkingds, "store", commandtext, CommandType.Text);
            if (ssprocessworkingds.store.Rows.Count > 0)
            {
                ssprocessds.store.Rows.Clear();
                ssprocessds.store.ImportRow(ssprocessworkingds.store[0]);
                RefreshComboboxes();
                RefreshDateTimePickers();
            }
            else
            {
                wsgUtilities.wsgNotice("You have reached the last store for this company");
            }


        }

        public override void ProcessCancel(object sender, EventArgs e)
        {
            base.ProcessCancel(sender, e);
            ComboboxTaxarea.SelectedIndex = -1;

        }
        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;
            if (ssprocessds.store[0].storecode.PadRight(11).Substring(0, 4).TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("There must be a storecode. Please correct");
                cont = false;
            }
            if (ssprocessds.store[0].store_name.TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("There must be a store name. Please correct");
                cont = false;
            }
            if (cont)
            {
                ssprocessds.store[0].stop_date = DateTimePickerStopdate.Value;
                ssprocessds.store[0].start_date = DateTimePickerStartdate.Value;
                ssprocessds.store[0].store_name = ssprocessds.store[0].store_name.PadRight(30).Substring(0, 30).TrimEnd();
                ssprocessds.store[0].f_address = ssprocessds.store[0].f_address.PadRight(25).Substring(0, 25).TrimEnd();
                ssprocessds.store[0].f_city = ssprocessds.store[0].f_city.PadRight(15).Substring(0, 15).TrimEnd();
                ssprocessds.store[0].tax_area = Convert.ToDecimal(ComboboxTaxarea.SelectedIndex + 1);
                ssprocessds.store[0].phone = ssprocessds.store[0].phone.PadRight(12).Substring(0, 12).TrimEnd();

                // Update the arcust table
                if (ssprocessds.store[0].idcol > 0 && ssprocessds.store[0].custno.TrimEnd() != "")
                {
                    vfpArdatamethods.UpdateArcust(ssprocessds.store[0].custno, ssprocessds.store[0].store_name,
                    ssprocessds.store[0].f_address, ssprocessds.store[0].f_city, ssprocessds.store[0].f_state,
                     ssprocessds.store[0].f_zip, ssprocessds.store[0].attention, ssprocessds.store[0].phone);
                }
                /* WLF 10/05/18 Don't create new customers here. The billing program will do it.
                    else
                    {
                        // Capture the customer number being created in VFP 
                        ssprocessds.store[0].custno = vfpArdatamethods.CreateArcust(ssprocessds.store[0].store_name,
                        ssprocessds.store[0].f_address, ssprocessds.store[0].f_city, ssprocessds.store[0].f_state,
                        ssprocessds.store[0].f_zip, ssprocessds.store[0].attention, ssprocessds.store[0].phone);
                    }
                    */
                string selectedstorecode = ssprocessds.store[0].storecode;
                // Trim columns that might cause sql truncation crashes



                GenerateAppTableRowSave(ssprocessds.store[0]);
                // Update the MySql table
                mySQLSynchMethods.updatemysqlstore(selectedstorecode);
                // Refresh the data table
                ssprocessds.store.Rows.Clear();
                commandtext = "SELECT * FROM  store WHERE storecode = @storecode";
                ClearParameters();
                AddParms("@storecode", selectedstorecode, "SQL");
                FillData(ssprocessds, "store", commandtext, CommandType.Text);
                currentidcol = ssprocessds.store[0].idcol;
                CurrentState = "View";
                RefreshControls();
            }
        }

        public override void RefreshControls()
        {
            base.RefreshControls();
            LabelBillmemo.Text = "";
            LabelBillmemo.ForeColor = Color.Black;
            switch (CurrentState)
            {

                case "View":
                    {
                        if (ssprocessds.store[0].bill_memo.TrimEnd() != "")
                        {
                            LabelBillmemo.Text = "See Notes";
                            LabelBillmemo.ForeColor = Color.Red;
                        }
                        ButtonForward.Enabled = true;
                        ButtonBack.Enabled = true;
                        break;
                    }

                case "Edit":
                    {
                        if (ssprocessds.store[0].bill_memo.TrimEnd() != "")
                        {
                            LabelBillmemo.Text = "See Notes";
                            LabelBillmemo.ForeColor = Color.Red;
                        }
                        DateTimePickerStopdate.Enabled = true;
                        DateTimePickerStartdate.Enabled = true;
                        ButtonBillingAddress.Enabled = true;
                        ButtonDropSchedule.Enabled = true;
                        ButtonSlips.Enabled = true;
                        ButtonRecurringCharges.Enabled = true;
                        ButtonRecurringCoin.Enabled = true;
                        ButtonBillMemo.Enabled = true;
                        ButtonPuaddress.Enabled = true;
                        ButtonEmailAddresses.Enabled = true;
                        break;
                    }

                case "Insert":
                    {
                        DateTimePickerStopdate.Enabled = true;
                        DateTimePickerStartdate.Enabled = true;
                        ButtonBillingAddress.Enabled = true;
                        ButtonDropSchedule.Enabled = true;
                        ButtonSlips.Enabled = true;
                        ButtonRecurringCharges.Enabled = true;
                        ButtonRecurringCoin.Enabled = true;
                        ButtonBillMemo.Enabled = true;
                        ButtonPuaddress.Enabled = true;
                        ButtonEmailAddresses.Enabled = true;
                        break;
                    }

            }
            TextBoxStorecode.ReadOnly = true;
            TextBoxCustno.ReadOnly = true;
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
            TBox.Left = TLeft + 125;
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            string selectedcompany = commonAppDataMethods.SelectCompany();

            if (selectedcompany.TrimEnd().Length > 0)
            {
                commandtext = "SELECT * FROM company WHERE comp_code  = @compcode";
                ssprocessds.company.Rows.Clear();
                ClearParameters();
                AddParms("@compcode", selectedcompany, "SQL");
                FillData(ssprocessds, "company", commandtext, CommandType.Text);
                string newstorecode = getNewStoreCodeMethods.GetNewStoreCode(selectedcompany);
                if (newstorecode.TrimEnd().Length > 0)
                {

                    ComboboxTaxarea.SelectedIndex = 1;
                    EstablishBlankDataTableRow(ssprocessds.store);
                    ssprocessds.store[0].storecode = newstorecode;
                    ssprocessds.store[0].sscoin = true;
                    ssprocessds.store[0].active = true;
                    ssprocessds.store[0].cg_drp_rt = 20.00M;
                    ssprocessds.store[0].roll_25 = .20M;
                    ssprocessds.store[0].strap_5 = .50M;
                    ssprocessds.store[0].stop_date = Convert.ToDateTime("12/31/2099");
                    ssprocessds.store[0].start_date = DateTime.Now.Date;
                    base.SetInsertState(sender, e);
                    CurrentState = "Insert";
                    RefreshControls();
                    RefreshDateTimePickers();

                }
                else
                {
                    CurrentState = "Select";
                    RefreshControls();

                }
            }
        }
    }
}