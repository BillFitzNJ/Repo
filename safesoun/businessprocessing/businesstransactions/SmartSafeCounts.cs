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
    public class SmartSafeCounts : WSGDataAccess
    {
        ssprocess ssprocessds = new ssprocess();
        ssprocess workingssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        FrmSmartSafeCount frmSmartSafeCount = new FrmSmartSafeCount();
        public Form menuform { get; set; }
        GroupBoxCountMethods countingGroupBoxMethods = new GroupBoxCountMethods();
        BindingSource DeclaredHistory = new BindingSource();
        DateTime PostingDate = new DateTime();
        GroupBox countingGroupBox = new GroupBox();
        WSGUtilities wsgUtilities = new WSGUtilities("Smart Safe Verified");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        string CommandString = "";
        string CurrentState = "";
        string bankfedid = "CCSmartSafe";
        public SmartSafeCounts()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.smartsafetrans.idcolColumn);
            countingGroupBox = countingGroupBoxMethods.SetGroupBox(ssprocessds.smartsafetrans, ssprocessds);
            frmSmartSafeCount.panelCounts.Controls.Add(countingGroupBox);
        }

        public void EnterSmartSafeCounts()
        {

            frmSmartSafeCount.Show();
            CurrentState = "Select Safe";
            SetEvents();
            RefreshControls();
        }
        public void SetEvents()
        {
            frmSmartSafeCount.buttonClose.Click += new System.EventHandler(CaptureSmartSafeCountClose);
            frmSmartSafeCount.buttonSelectSmartSafe.Click += new System.EventHandler(SelectCountSmartSafe);
            countingGroupBoxMethods.ButtonSave.Click += new System.EventHandler(SaveSmartSafeTransaction);
            countingGroupBoxMethods.ButtonCancel.Click += new System.EventHandler(CancelSmartSafeCount);
            frmSmartSafeCount.textBoxDenominations.TextChanged += new EventHandler(CheckDenominationText);
        }

        private void CancelSmartSafeCount(object sender, EventArgs e)
        {
            frmSmartSafeCount.Close();
        }

        private void SaveSmartSafeTransaction(object sender, EventArgs e)
        {
                SaveSmartSafeCount();
        }

        private void SaveSmartSafeCount()
        {

            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@store", ssprocessds.smartsafetrans[0].store, "SQL");
            AddParms("@safeid", ssprocessds.smartsafetrans[0].safeid, "SQL");
            AddParms("@hundreds", ssprocessds.smartsafetrans[0].hundreds, "SQL");
            AddParms("@fiftys", ssprocessds.smartsafetrans[0].fiftys, "SQL");
            AddParms("@twentys", ssprocessds.smartsafetrans[0].twentys, "SQL");
            AddParms("@tens", ssprocessds.smartsafetrans[0].tens, "SQL");
            AddParms("@fives", ssprocessds.smartsafetrans[0].fives, "SQL");
            AddParms("@twos", ssprocessds.smartsafetrans[0].twos, "SQL");
            AddParms("@ones", ssprocessds.smartsafetrans[0].ones, "SQL");
            AddParms("@mixedcoin", ssprocessds.smartsafetrans[0].mixedcoin, "SQL");
            AddParms("@postingdate", ssprocessds.smartsafetrans[0].postingdate, "SQL");
            AddParms("@adduser", AppUserClass.AppUserId, "SQL");
            ExecuteCommand("wsgsp_Insert_smartsafecount", CommandType.StoredProcedure);
            try
            {
                ExecuteCommand("wsgsp_Insert_smartsafecount", CommandType.StoredProcedure);
              
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            wsgUtilities.wsgNotice("Posting Complete");
            ssprocessds.smartsafetrans.Rows.Clear();
            CurrentState = "Select Safe";
            RefreshControls();
        }


        private void CaptureSmartSafeCountClose(object sender, EventArgs e)
        {
            frmSmartSafeCount.Close();
        }
        private void SelectCountSmartSafe(object sender, EventArgs e)
        {
            bool cont = true;
            string[] customerdata = new string[2];
            int smartsafeid = commonAppDataMethods.SelectBankSmartSafe(bankfedid);
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
                if (cont)
                {
                    PostingDate = commonAppDataMethods.GetNextPostDate(bankfedid);
                    //Create count row
                    ssprocessds.smartsafetrans.Rows.Clear();
                    EstablishBlankDataTableRow(ssprocessds.smartsafetrans);
                    ssprocessds.smartsafetrans[0].bankfedid = bankfedid;
                    ssprocessds.smartsafetrans[0].postingdate = PostingDate;
                    ssprocessds.smartsafetrans[0].customerdda = customerdata[0];
                    ssprocessds.smartsafetrans[0].masteraccountid = customerdata[1];
                    ssprocessds.smartsafetrans[0].safeid = ssprocessds.view_expandedsafemast[0].idcol;
                    ssprocessds.smartsafetrans[0].eventcode = "VER";
                    ssprocessds.smartsafetrans[0].store = ssprocessds.view_expandedsafemast[0].storecode;
                    frmSmartSafeCount.labelSafeSerialNumber.Text = ssprocessds.view_expandedsafemast[0].serialnumber;
                    countingGroupBoxMethods.RefreshTotals();
                    CurrentState = "Enter Verified";
                    RefreshControls();

                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }


        public void RefreshControls()
        {
            DisableControls();
            frmSmartSafeCount.buttonClose.Enabled = true;
            switch (CurrentState)
            {
                case "Select Safe":
                    {
                        frmSmartSafeCount.buttonSelectSmartSafe.Enabled = true;
                        frmSmartSafeCount.buttonClose.Enabled = true;
                        frmSmartSafeCount.buttonSelectSmartSafe.Enabled = true;
                        break;
                    }
                case "Enter Verified":
                    {
                        frmSmartSafeCount.textBoxDenominations.Enabled = true;
                        frmSmartSafeCount.panelCounts.Enabled = true;
                        frmSmartSafeCount.buttonSelectSmartSafe.Enabled = false;
                        frmSmartSafeCount.Focus();
                        countingGroupBox.Enabled = true;

                    }
                    break;
            }
        }
        public void CheckDenominationText(object sender, EventArgs e)
        {
            if (frmSmartSafeCount.textBoxDenominations.Text.Count(t => t == '\n') == 7)
            {
                ConvertDenominationString();
                frmSmartSafeCount.textBoxDenominations.Text = String.Empty;
                countingGroupBoxMethods.TextBoxMixedcoin.Focus();
            }

        }
        public void ConvertDenominationString()
        {
            string[] moneylines = System.Text.RegularExpressions.Regex.Split(frmSmartSafeCount.textBoxDenominations.Text, "\r\n");
            ssprocessds.smartsafetrans[0].ones = Convert.ToDecimal(moneylines[0].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].twos = Convert.ToDecimal(moneylines[1].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].fives = Convert.ToDecimal(moneylines[2].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].tens = Convert.ToDecimal(moneylines[3].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].twentys = Convert.ToDecimal(moneylines[4].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].fiftys = Convert.ToDecimal(moneylines[5].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].hundreds = Convert.ToDecimal(moneylines[6].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans.AcceptChanges();
            countingGroupBoxMethods.RefreshTotals();
        }
        public void DisableControls()
        {
            foreach (Control c in frmSmartSafeCount.Controls)
            {
                if (!(c is Label))
                {
                    c.Enabled = false;
                }
            }
        }
    }
}
