using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using BusinessTransactions;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net;
using System.Windows.Forms;
using System.Globalization;
using WSGUtilitieslib;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
namespace BusinessTransactions
{
    public class SmartSafeDeclared : WSGDataAccess
    {

        ssprocess ssprocessds = new ssprocess();
        ssprocess workingssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        FrmSmartSafeDeclared frmSmartSafeDeclared = new FrmSmartSafeDeclared();
        string bankfedid = "";
        public Form menuform { get; set; }
        BindingSource DeclaredHistory = new BindingSource();
        DateTime PostingDate = new DateTime();
        WSGUtilities wsgUtilities = new WSGUtilities("Smart Safe Declared");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        string CommandString = "";
        string CurrentState = "";
        public SmartSafeDeclared()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.smartsafetrans.idcolColumn);

        }

        public void EnterSmartSafeDeclared()
        {
          
            DeclaredHistory.DataSource = ssprocessselectords.smartsafetrans;
            frmSmartSafeDeclared.dataGridViewPriorDeclared.AutoGenerateColumns = false;
            frmSmartSafeDeclared.dataGridViewPriorDeclared.DataSource = DeclaredHistory;
            frmSmartSafeDeclared.dataGridViewPriorDeclared.RowsDefaultCellStyle.BackColor = Color.LightGray;
            frmSmartSafeDeclared.dataGridViewPriorDeclared.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            frmSmartSafeDeclared.MdiParent = menuform;
            frmSmartSafeDeclared.Show();
            CurrentState = "Select Safe";
            SetTextBoxCurrencyBinding(frmSmartSafeDeclared.textBoxSaidtocontain, ssprocessds, "smartsafetrans.saidtocontain");
            SetEvents();
            RefreshControls();
        }

        public void SetEvents()
        {
            frmSmartSafeDeclared.buttonClose.Click += new System.EventHandler(CaptureSmartSafeDeclaredClose);
            frmSmartSafeDeclared.buttonSelectSmartSafe.Click += new System.EventHandler(SelectDeclaredSmartSafe);
            frmSmartSafeDeclared.buttonSave.Click += new System.EventHandler(SaveDeclared);

        }

        private void SaveDeclared(object sender, EventArgs e)
        {
            if (ssprocessds.smartsafetrans[0].saidtocontain != 0)
            {
                GenerateAppTableRowSave(ssprocessds.smartsafetrans[0]);
                ssprocessds.smartsafetrans.Rows.Clear();
                ssprocessselectords.smartsafetrans.Rows.Clear();
                wsgUtilities.wsgNotice("Posting complete");
                CurrentState = "Select Safe";
                RefreshControls();
            }
            else
            {
                wsgUtilities.wsgNotice("There must be a declared amount");
            }
        }

        private void CaptureSmartSafeDeclaredClose(object sender, EventArgs e)
        {
            frmSmartSafeDeclared.Close();
        }
        private void SelectDeclaredSmartSafe(object sender, EventArgs e)
        {
            string[] customerdata = new string[2];
            int smartsafeid = commonAppDataMethods.SelectSmartSafe();
            if (smartsafeid != 0)
            {
                ssprocessds.view_expandedsafemast.Rows.Clear();
                CommandString = "SELECT * FROM view_expandedsafemast WHERE idcol = @idcol";
                ClearParameters();
                AddParms("@idcol", smartsafeid, "SQL");
                FillData(ssprocessds, "view_expandedsafemast", CommandString, CommandType.Text);
                // Prepare the declared row
                ssprocessds.smartsafetrans.Rows.Clear();
                EstablishBlankDataTableRow(ssprocessds.smartsafetrans);
                customerdata = commonAppDataMethods.GetCustomerData(ssprocessds.view_expandedsafemast[0].storecode);
                if (customerdata[0].TrimEnd() == "")
                {
                    wsgUtilities.wsgNotice("There is no account information for that Smart Safe");
                }
                else
                {
                    // Fill Datagrid source
                    ClearParameters();
                    AddParms("@safeid", ssprocessds.view_expandedsafemast[0].idcol, "SQL");
                    CommandString = " SELECT CAST(postingdate AS date) AS postingdate, saidtocontain  FROM smartsafetrans WHERE  safeid =  @safeid AND eventcode = 'DECL' AND verifyid = 0";
                    ssprocessselectords.smartsafetrans.Rows.Clear();
                    FillData(ssprocessselectords, "smartsafetrans", CommandString, CommandType.Text);
                    
                    // If Signature Bank, use "SMARTSAFE", otherwise use bankfedid
                    if (ssprocessds.view_expandedsafemast[0].bankfedid.Substring(0,1) == "8" )
                    {
                         bankfedid  = "SMARTSAFE";
                    }
                    else
                    {
                         bankfedid = ssprocessds.view_expandedsafemast[0].bankfedid.TrimEnd();
                    }
                    PostingDate = commonAppDataMethods.GetNextPostDate(bankfedid);
                    //Create declared row
                    ssprocessds.smartsafetrans[0].bankfedid = bankfedid;
                    ssprocessds.smartsafetrans[0].postingdate = PostingDate;
                    ssprocessds.smartsafetrans[0].customerdda = customerdata[0];
                    ssprocessds.smartsafetrans[0].masteraccountid = customerdata[1];
                    ssprocessds.smartsafetrans[0].safeid = ssprocessds.view_expandedsafemast[0].idcol;
                    ssprocessds.smartsafetrans[0].eventcode = "DECL";
                    ssprocessds.smartsafetrans[0].store = ssprocessds.view_expandedsafemast[0].storecode;
                    frmSmartSafeDeclared.labelSmartSafeInformation.Text = ssprocessds.view_expandedsafemast[0].storename;
                    CurrentState = "Enter Declared";
                    RefreshControls();
                  

                }
            }
            else
            {
                wsgUtilities.wsgNotice("There are not Smart Safes");
            }
        }

        public void DisableControls()
        {
            foreach (Control c in frmSmartSafeDeclared.Controls)
            {
                if (!(c is Label))
                {
                    c.Enabled = false;
                }
            }
        }
        public void RefreshControls()
        {
            DisableControls();
            frmSmartSafeDeclared.buttonClose.Enabled = true;
            switch (CurrentState)
            {
                case "Select Safe":
                    {
                        frmSmartSafeDeclared.buttonSelectSmartSafe.Enabled = true;
                        frmSmartSafeDeclared.buttonClose.Enabled = true;
                        frmSmartSafeDeclared.buttonSelectSmartSafe.Enabled = true;
                        break;
                    }
                case "Enter Declared":
                    {
                        frmSmartSafeDeclared.buttonSelectSmartSafe.Enabled = false;
                        frmSmartSafeDeclared.buttonSave.Enabled = true;
                        frmSmartSafeDeclared.buttonCancel.Enabled = true;
                        frmSmartSafeDeclared.textBoxSaidtocontain.Enabled = true;
                        frmSmartSafeDeclared.Focus();
                        break;
                    }

            }
        }
        private void SetTextBoxDollarsBinding(TextBox txtbox, DataSet ds, string fieldname)
        {
            Binding b = new Binding("Text", ds, fieldname);
            {
                b.Format += new ConvertEventHandler(DecimalToDollarsString);
                b.Parse += new ConvertEventHandler(CurrencyStringToDecimal);
            }
            txtbox.DataBindings.Clear();
            txtbox.DataBindings.Add(b);
        }

        private void SetTextBoxCurrencyBinding(TextBox txtbox, DataSet ds, string fieldname)
        {

            Binding b = new Binding("Text", ds, fieldname);
            {
                b.Format += new ConvertEventHandler(DecimalToCurrencyString);
                b.Parse += new ConvertEventHandler(CurrencyStringToDecimal);
            }
            txtbox.DataBindings.Clear();
            txtbox.DataBindings.Add(b);

        }
        private void DecimalToDollarsString(object sender, ConvertEventArgs cevent)
        {
            // The method converts only to string type. Test this using the DesiredType.
            if (cevent.DesiredType != typeof(string)) return;

            // Use the ToString method to format the value as currency ("c").
            cevent.Value = ((Decimal)cevent.Value).ToString("N0");
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
            cevent.Value = Decimal.Parse(cevent.Value.ToString(),
            NumberStyles.Currency, null);
        }




    }
}
