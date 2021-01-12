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
    public class MaintainSpecialChargeMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Bank");
        string commandtext = "";
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocesssearchds = new ssprocess();
        DateTimePicker DateTimePickerChargedate = new DateTimePicker();
        NumericUpDown numericUpDownChgAmt = new NumericUpDown();
        Label LabelChargedate = new Label();
        Label LabelChgAmt = new Label();
        NumericUpDown numericUpDownDrivercommamt = new NumericUpDown();
        NumericUpDown numericUpDownSalescommamt = new NumericUpDown();
        Label LabelDrivercommamt = new Label();
        TextBox TextBoxOrderno = new TextBox();
        Label LabelOrderno = new Label();
     
        Label LabelSalescommamt = new Label();
        TextBox TextBoxChgnotes = new TextBox();
        Label LabelChgnotes = new Label();
        Label LabelDrivername = new Label();
        Label LabelSalespersonname = new Label();
        Label LabelChargetypename = new Label();
        Label LabelStorename = new Label();
        Button ButtonStore = new Button();
        Button ButtonChargetype = new Button();
        Button ButtonDriver = new Button();
        Button ButtonSalesperson = new Button();
        CheckBox CheckboxTaxable = new CheckBox();
        Label LabelTaxable = new Label();
        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = true;
            currentdatatable = ssprocessds.specialcharge;
            currentablename = "specialcharge";
            SetIdcol(ssprocessds.specialcharge.idcolColumn);
            parentForm.Text = "Maintain Special Charge";

        }
        public override void SetControls()
        {
            int TextTop = 100;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 400;
            SetNumericUpDown(numericUpDownChgAmt, LeftStart, TextTop, 75, ssprocessds.specialcharge, "chgamt", LabelChgAmt, "Charge Amount");
            numericUpDownChgAmt.DecimalPlaces = 2;
            TextTop += 22;
            SetNumericUpDown(numericUpDownDrivercommamt, LeftStart, TextTop, 75, ssprocessds.specialcharge, "drivercommamt", LabelDrivercommamt, "Driver Comm");
            numericUpDownDrivercommamt.DecimalPlaces = 2;
            TextTop += 22;
            SetNumericUpDown(numericUpDownSalescommamt, LeftStart, TextTop, 75, ssprocessds.specialcharge, "salescommamt", LabelSalescommamt, "Sales Comm");
            numericUpDownSalescommamt.DecimalPlaces = 2;
            TextTop += 22;
            LabelChargedate.Text = "Charge Date";
            LabelChargedate.Top = TextTop;
            LabelChargedate.AutoSize = true;
            LabelChargedate.Left = LeftStart;
            parentForm.Controls.Add(LabelChargedate);
            DateTimePickerChargedate.CustomFormat = "";
            this.DateTimePickerChargedate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateTimePickerChargedate.Top = TextTop;
            this.DateTimePickerChargedate.Left = LeftStart + 125;
            this.DateTimePickerChargedate.Name = "dateTimePickerChargedate";
            this.DateTimePickerChargedate.Size = new System.Drawing.Size(103, 20);
            DateTimePickerChargedate.MaxDate = DateTime.MaxValue;
            parentForm.Controls.Add(DateTimePickerChargedate);
            TextTop += 22;
            SetTextBox(TextBoxChgnotes, LeftStart, TextTop, 300, ssprocessds.specialcharge, "chgnotes", LabelChgnotes, "Notes");
            TextTop += 22;
            SetTextBox(TextBoxOrderno, LeftStart, TextTop, 300, ssprocessds.specialcharge, "orderno", LabelOrderno, "Order Number");
            TextTop += 22;
            ButtonStore.Height = parentForm.buttonSave.Height;
            ButtonStore.Top = TextTop;
            ButtonStore.Left = LeftStart;
            ButtonStore.Text = "Store";
            parentForm.Controls.Add(ButtonStore);
            LabelStorename.Top = TextTop;
            LabelStorename.Left = LeftStart + 125;
            LabelStorename.AutoSize = true;
            parentForm.Controls.Add(LabelStorename);
            TextTop += 22;
            ButtonChargetype.Height = parentForm.buttonSave.Height;
            ButtonChargetype.Top = TextTop;
            ButtonChargetype.Left = LeftStart;
            ButtonChargetype.Text = "Charge Type";
            parentForm.Controls.Add(ButtonChargetype);
            LabelChargetypename.Top = TextTop;
            LabelChargetypename.Left = LeftStart + 125;
            LabelChargetypename.AutoSize = true;
            parentForm.Controls.Add(LabelChargetypename);
            TextTop += 22;

            ButtonDriver.Height = parentForm.buttonSave.Height;
            ButtonDriver.Top = TextTop;
            ButtonDriver.Left = LeftStart;
            ButtonDriver.Text = "Driver";
            parentForm.Controls.Add(ButtonDriver);
            LabelDrivername.Top = TextTop;
            LabelDrivername.Left = LeftStart + 125;
            LabelDrivername.AutoSize = true;
            parentForm.Controls.Add(LabelDrivername);

            TextTop += 22;
            ButtonSalesperson.Height = parentForm.buttonSave.Height;
            ButtonSalesperson.Top = TextTop;
            ButtonSalesperson.Left = LeftStart;
            ButtonSalesperson.Text = "Sales Person";
            parentForm.Controls.Add(ButtonSalesperson);
            LabelSalespersonname.Top = TextTop;
            LabelSalespersonname.Left = LeftStart + 125;
            LabelSalespersonname.AutoSize = true;
            parentForm.Controls.Add(LabelSalespersonname);
            TextTop += 22;
            SetBitCheckBox(CheckboxTaxable, LeftStart, TextTop, 150, ssprocessds.specialcharge, "taxable", LabelTaxable, "Taxable");

        }
        public override void SetEvents()
        {
            ButtonSalesperson.Click += new System.EventHandler(ButtonSalesperson_click);
            ButtonStore.Click += new System.EventHandler(ButtonStore_click);
            ButtonDriver.Click += new System.EventHandler(ButtonDriver_click);
            ButtonChargetype.Click += new System.EventHandler(ButtonChargetype_click);
            base.SetEvents();
        }

        public override void ProcessSelect(object sender, EventArgs e)
        {
           currentidcol = commonAppDataMethods.SelectSpecialCharge();
           if (currentidcol > 0)
           {
               ssprocessds.specialcharge.Rows.Clear();
               commandtext = "SELECT * FROM  specialcharge WHERE idcol = @idcol";
               ClearParameters();
               AddParms("@idcol", currentidcol, "SQL");
               FillData(ssprocessds, "specialcharge", commandtext, CommandType.Text);
               CurrentState = "View";
               DateTimePickerChargedate.Value = ssprocessds.specialcharge[0].chgdate;
               RefreshControls();
           }        

        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            ssprocessds.specialcharge.Rows.Clear();
        }
        public void ClearLabels()
        {
            LabelChargetypename.Text = "";
            LabelStorename.Text = "";
            LabelSalespersonname.Text = "";
            LabelDrivername.Text = "";
     
        }
   
        public void RefreshLabels()
        {
            RefreshChargename(ssprocessds.specialcharge[0].chgcode);
            RefreshStorename(ssprocessds.specialcharge[0].storecode);
            RefreshSalespersonname(ssprocessds.specialcharge[0].salesperson);
            RefreshDrivername(ssprocessds.specialcharge[0].driver);
       
        }
        public void ButtonDriver_click(object sender, EventArgs e)
        {
            string selecteddriver = commonAppDataMethods.SelectDriver();
            if (selecteddriver.Length > 0)
            {
                ssprocessds.specialcharge[0].driver = selecteddriver;
            }
            else
            {
                ssprocessds.specialcharge[0].driver = "";
            }
            RefreshDrivername(ssprocessds.specialcharge[0].driver);
            parentForm.Update();
        }
        private void ButtonStore_click(object sender, EventArgs e)
        {
            ssprocessds.specialcharge[0].storecode = commonAppDataMethods.SelectCompanyAndStore();
            ssprocessds.specialcharge.AcceptChanges();
            RefreshStorename(ssprocessds.specialcharge[0].storecode);
        }

        public void ButtonChargetype_click(object sender, EventArgs e)
        {
            ssprocessds.specialcharge[0].chgcode = commonAppDataMethods.SelectChargeType();
            RefreshChargename(ssprocessds.specialcharge[0].chgcode);
        }
        public void ButtonSalesperson_click(object sender, EventArgs e)
        {
            string selectedsalesperson = commonAppDataMethods.SelectSalePerson();
            if (selectedsalesperson.Length > 0)
            {
                ssprocessds.specialcharge[0].salesperson = selectedsalesperson;
            }
            else
            {
                ssprocessds.specialcharge[0].salesperson = "  ";
            }
            RefreshSalespersonname(ssprocessds.specialcharge[0].salesperson);
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.specialcharge);
            ssprocessds.specialcharge[0].chgdate = DateTime.Now.Date;
            RefreshLabels();
            base.SetInsertState(sender, e);
        }

        public void RefreshChargename(string chgcode)
        {
            LabelChargetypename.Text = commonAppDataMethods.GetChargeName(chgcode);
        }

        public void RefreshSalespersonname(string slscode)
        {
            LabelSalespersonname.Text = commonAppDataMethods.GetSalespersonname(slscode);
        }
        public void RefreshStorename(string storecode)
        {
            LabelStorename.Text = commonAppDataMethods.GetStoreName(storecode);
        }

        public void RefreshDrivername(string driver)
        {
            LabelDrivername.Text = commonAppDataMethods.GetDriverName(driver);
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;
            if (ssprocessds.specialcharge[0].storecode.TrimEnd().TrimStart() == "")
            {
                wsgUtilities.wsgNotice("There must be a store");
                cont = false;
            }
            if (ssprocessds.specialcharge[0].chgcode.TrimEnd().TrimStart() == "")
            {
                wsgUtilities.wsgNotice("There must be a charge type");
                cont = false;
            }
            if (ssprocessds.specialcharge[0].chgamt == 0)
            {
                wsgUtilities.wsgNotice("There must be a charge amount");
                cont = false;
            }
            if (cont)
            {
                if (ssprocessds.specialcharge[0].chgnotes.TrimEnd().Length > 80)
                {
                    ssprocessds.specialcharge[0].chgnotes = ssprocessds.specialcharge[0].chgnotes.TrimEnd().Substring(0, 80);
                }
                ssprocessds.specialcharge[0].chgdate = DateTimePickerChargedate.Value;
                 GenerateAppTableRowSave(ssprocessds.specialcharge[0]);
                base.SaveCurrentDataTable(sender, e);
                wsgUtilities.wsgNotice("Operation complete");
            }
        }

        
        public override void RefreshControls()
        {
            base.RefreshControls();
            switch (CurrentState)
            {

                case "Select":
                    {
                        ClearLabels();
                        break;
                        
                    }

                case "View":
                    {
                        parentForm.buttonEdit.Enabled = true;
                        parentForm.buttonInsert.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonDelete.Enabled = true;
                        break;
                    }

                case "Edit":
                    {

                        ButtonStore.Enabled = true;
                        ButtonSalesperson.Enabled = true;
                        ButtonDriver.Enabled = true;
                        ButtonChargetype.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        DateTimePickerChargedate.Enabled = true;
                        numericUpDownChgAmt.Enabled = true;
                        numericUpDownDrivercommamt.Enabled = true;
                        numericUpDownSalescommamt.Enabled = true;

                        break;
                    }

                case "Insert":
                    {
                        ButtonStore.Enabled = true;
                        ButtonSalesperson.Enabled = true;
                        ButtonDriver.Enabled = true;
                        ButtonChargetype.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        DateTimePickerChargedate.Enabled = true;
                        numericUpDownChgAmt.Enabled = true;
                        numericUpDownDrivercommamt.Enabled = true;
                        numericUpDownSalescommamt.Enabled = true;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }


    }

}


