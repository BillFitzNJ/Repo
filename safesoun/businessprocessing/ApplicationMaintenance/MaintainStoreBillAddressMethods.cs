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
    public class MaintainStoreBillAddressMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Store Bill Address");
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        public string Storecode = "";
        int tabIndex = 0;
        string commandtext = "";
        NumericUpDown numericUpDownChgAmt = new NumericUpDown();
        TextBox TextBoxCompany = new TextBox();
        TextBox TextBoxAddr1 = new TextBox();
        TextBox TextBoxAddr2 = new TextBox();
        TextBox TextBoxAttn = new TextBox();
        TextBox TextBoxCity = new TextBox();
        TextBox TextBoxState = new TextBox();
        TextBox TextBoxZip = new TextBox();
       
     
        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.storebilladdress;
            currentablename = "storebilladdress";
            SetIdcol(ssprocessds.storebilladdress.idcolColumn);
            parentForm.Text = "Maintain Store Billing Address";

        }
        public override void SetControls()
        {
        
            parentForm.buttonDelete.Visible = true;
            int TextTop = 100;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 600;
            SetTextBoxAndLabelText(TextBoxCompany, LeftStart, TextTop, 250, ssprocessds.storebilladdress, "company", "Company");
            TextTop += 22;
            SetTextBoxAndLabelText(TextBoxAddr1, LeftStart, TextTop, 250, ssprocessds.storebilladdress, "addr1", "Street Address");
            TextTop += 22;
            SetTextBoxAndLabelText(TextBoxAddr2, LeftStart, TextTop, 250, ssprocessds.storebilladdress, "addr2", "Street Address 2");
            TextTop += 22;
            SetTextBoxAndLabelText(TextBoxCity, LeftStart, TextTop, 250, ssprocessds.storebilladdress, "city", "City, State, Zip");
            SetTextBoxAndLabelText(TextBoxState, LeftStart + 395, TextTop, 20, ssprocessds.storebilladdress, "State", "");
            SetTextBoxAndLabelText(TextBoxZip, LeftStart + 415, TextTop, 60, ssprocessds.storebilladdress, "zip", "");
            TextTop += 22;
            SetTextBoxAndLabelText(TextBoxAttn, LeftStart, TextTop, 250, ssprocessds.storebilladdress, "attn", "Attention");
            TextTop += 22;
       
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (cont)
            {
                GenerateAppTableRowSave(ssprocessds.storebilladdress[0]);
                ssprocessds.storebilladdress.Rows.Clear();
                CurrentState = "Select";
                RefreshControls();
            }
        }

        public override void ProcessSelect(object sender, EventArgs e)
        {
            Storecode = commonAppDataMethods.SelectCompanyAndStore();
            LoadBillingAddress();
   
        }

        public void LoadBillingAddress()
        {
            ssprocessds.storebilladdress.Rows.Clear();
            commandtext = "SELECT * FROM  storebilladdress WHERE storecode = @storecode";
            ClearParameters();
            AddParms("@storecode", Storecode, "SQL");
            FillData(ssprocessds, "storebilladdress", commandtext, CommandType.Text);
            if (ssprocessds.storebilladdress.Rows.Count > 0)
            {
                currentidcol = ssprocessds.storebilladdress[0].idcol;

                parentForm.Text = "Processing billing address  for " + commonAppDataMethods.GetStoreName(Storecode);
                CurrentState = "View";
            }
            else
            {
                CurrentState = "StoreSelected";
            }
            RefreshControls();
        }
        public override void SetEvents()
        {
            base.SetEvents();
 
        }
        public override void SetEditState(object sender, EventArgs e)
        {
            string editstatus = base.LockTableRow(ssprocessds.storebilladdress[0].idcol, "storebilladdress");
            if (editstatus == "OK")
            {

                CurrentState = "Edit";
                RefreshControls();
            }
            else
            {
                if (wsgUtilities.wsgReply(editstatus + ". Do you want to override?"))
                {
                    CurrentState = "Edit";
                    RefreshControls();
                    editstatus = "OK";
                    UnlockTableRow(currentidcol, currentablename);
                }
            }

        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.storebilladdress);
            ssprocessds.storebilladdress[0].storecode = Storecode;
            CurrentState = "Insert";
            RefreshControls();
        }
        public override void ProcessCancel(object sender, EventArgs e)
        {

            if (CurrentState == "Edit" || CurrentState == "Insert")
            {
                if (wsgUtilities.wsgReply("Abandon Edit"))
                {
                    parentForm.Update();
                    if (CurrentState == "Edit")
                    {
                        UnlockTableRow(currentidcol, currentablename);

                    }

                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
            }
            else
            {
                if (CurrentState == "View")
                {
                    ssprocessds.storebilladdress.Rows.Clear();
                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
                else
                {
                    ssprocessselectords.storebilladdress.Rows.Clear();
                    CurrentState = "Select";
                    RefreshControls();

                }
            }
        }



        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            CurrentState = "Select";
            RefreshControls();
            ssprocessds.storebilladdress.Rows.Clear();
        }
        public override void RefreshControls()
        {
            base.RefreshControls();
            switch (CurrentState)
            {

                case "Select":
                    {
                        parentForm.buttonInsert.Enabled = false;
                        break;
                    }

                case "StoreSelected":
                    {
                        parentForm.Text = commonAppDataMethods.GetStoreName(Storecode).TrimEnd();
                        parentForm.buttonInsert.Enabled = true;
                        break;
                    }

                case "View":
                    {
                        parentForm.buttonEdit.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonDelete.Enabled = true;
                        break;
                    }

                case "Edit":
                    {
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        break;
                    }

                case "Insert":
                    {
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
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
