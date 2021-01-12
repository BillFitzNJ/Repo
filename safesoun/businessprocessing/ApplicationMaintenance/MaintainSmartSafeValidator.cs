using System;
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
    public class MaintainSmartSafeValidator : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain User");
        string commandtext = "";
        // Establish the dataset
        mysqldata mysqlds = new mysqldata();
        ssprocess ssprocessds = new ssprocess();
        Label LabelSafeid = new Label();
        Button ButtonSafeid = new Button();
        TextBox TextBoxDeviceid = new TextBox();
        Label LabelTextBoxid = new Label();
        Label Safeid = new Label();


        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentablename = "validator";
            parentForm.Text = "Maintain Smart Safe Validator";
            currentdatatable = mySQLDataMethods.mysqlds.validator;
            SetIdcol(mySQLDataMethods.mysqlds.validator.idcolColumn);
        }
        public override void SetControls()
        {
            LabelSafeid.AutoSize = true;
            int TextTop = 50;
            int LeftStart = 50;
            SetTextBox(TextBoxDeviceid, LeftStart, TextTop, 150, mySQLDataMethods.mysqlds.validator, "deviceid", LabelTextBoxid, "Device ID");
            TextTop += 22;
            ButtonSafeid.Text = "Smart Safe";
            ButtonSafeid.Height = parentForm.buttonSave.Height;
            ButtonSafeid.Width = 100;
            ButtonSafeid.Top = TextTop;
            ButtonSafeid.Left = LeftStart;
            parentForm.Controls.Add(ButtonSafeid);
            LabelSafeid.Top = TextTop + 5;
            LabelSafeid.Left = ButtonSafeid.Left + ButtonSafeid.Width + 15;
            parentForm.Controls.Add(LabelSafeid);
        }

        public override void SetEvents()
        {
            ButtonSafeid.Click += new System.EventHandler(SelectSmartSafe);
            base.SetEvents();
        }


        public override void SetInsertState(object sender, EventArgs e)
        {
            mySQLDataMethods.mysqlds.validator.Rows.Clear();
            mySQLDataMethods.mysqlds.validator.Rows.Add();
            EstablishBlankDataTableRow(mySQLDataMethods.mysqlds.validator);
            CurrentState = "Insert";
            RefreshControls();
        }

        public override void ProcessSelect(object sender, EventArgs e)
        {
             mySQLDataMethods.GetValidators();

            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            frmSelectorMethods.FormText = "Select Validator";
            frmSelectorMethods.dtSource = mySQLDataMethods.mysqlselectords.validator;
            frmSelectorMethods.columncount = 1;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "DeviceIdcol";
            frmSelectorMethods.colheadertext[0] = "Device ID";
            frmSelectorMethods.coldatapropertyname[0] = "deviceid";
            frmSelectorMethods.colwidth[0] = 100;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 400;
            int validatoridcol = frmSelectorMethods.ShowSelector();
            if (validatoridcol > 0)
            {

                mySQLDataMethods.GetSingleValidator(validatoridcol);
                LabelSafeid.Text = mySQLDataMethods.mysqlds.validator[0].safeid; 
            }
            CurrentState = "View";
            RefreshControls();
        }
        public override void SetEditState(object sender, EventArgs e)
        {
            CurrentState = "Edit";
            RefreshControls();
        }

        public override void RefreshControls()
        {
            base.RefreshControls();

            switch (CurrentState)
            {
                case "Select":
                    {
                       LabelSafeid.Text = "";
                        break;
                    }

                case "Edit":
                    {
                        TextBoxDeviceid.Enabled = false;
                        ButtonSafeid.Enabled = true;
                        break;
                    }
                case "Insert":
                    {
                        ButtonSafeid.Enabled = true;
                        break;
                    }

            }

        }


        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {


            bool cont = true;

            if (mySQLDataMethods.mysqlds.validator[0].safeid.TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("There must be a safe ID");
                cont = false;
            }

            if (cont)
            {

                if (CurrentState == "Insert")
                {
                    // Check for existence of the ID
                    if (mySQLDataMethods.ValidatoridExists(mySQLDataMethods.mysqlds.validator[0].deviceid))
                    {
                        wsgUtilities.wsgNotice("Validator " + mySQLDataMethods.mysqlds.validator[0].deviceid.TrimEnd() + " is already in the file");
                        cont = false;
                    }

                }
            }
            if (cont)
            {
                mySQLDataMethods.SaveMySQLDatatableRow(mySQLDataMethods.mysqlds.validator[0]);
                CurrentState = "View";
            }
        
            RefreshControls();
        }

        public void SelectSmartSafe(object sender, EventArgs e)
        {
            int selectedsafemastid = commonAppDataMethods.SelectSmartSafe();
            if (selectedsafemastid > 0)
            {
                ssprocessds.safemast.Rows.Clear();
                string  commandstring = "SELECT * FROM safemast WHERE idcol= @idcol";
                ClearParameters();
                AddParms("@idcol", selectedsafemastid, "SQL");
                FillData(ssprocessds, "safemast", commandstring, CommandType.Text);
                LabelSafeid.Text = ssprocessds.safemast[0].serialnumber;
                mySQLDataMethods.mysqlds.validator[0].safeid = ssprocessds.safemast[0].serialnumber;
            }
        
        }
    }
}