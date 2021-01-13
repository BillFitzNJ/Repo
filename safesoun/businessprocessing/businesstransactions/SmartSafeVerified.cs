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
    public class SmartSafeVerified : WSGDataAccess
    {
        ssprocess ssprocessds = new ssprocess();
        ssprocess unassigneddeclaredssprocessds = new ssprocess();
        ssprocess assigneddeclaredssprocessds = new ssprocess();
        ssprocess workingssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        FrmSmartSafeVerified frmSmartSafeVerified = new FrmSmartSafeVerified();
        BindingSource bindingSourceUnassigned = new BindingSource();
        BindingSource bindingSourceAssigned = new BindingSource();
        string bankfedid = "";
        public Form menuform { get; set; }
        GroupBoxVerifiedMethods groupBoxVerifiedBoxMethods = new GroupBoxVerifiedMethods();
        DateTime PostingDate = new DateTime();
        GroupBox verifiedGroupBox = new GroupBox();
        WSGUtilities wsgUtilities = new WSGUtilities("Smart Safe Verified");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        string CommandString = "";
        string CurrentState = "";
        public SmartSafeVerified()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.smartsafetrans.idcolColumn);
            verifiedGroupBox = groupBoxVerifiedBoxMethods.SetGroupBox(ssprocessds.smartsafetrans, ssprocessds);
            frmSmartSafeVerified.panelVerified.Controls.Add(verifiedGroupBox);
        }

        public void EnterSmartSafeVerified()
        {

            frmSmartSafeVerified.MdiParent = menuform;
            frmSmartSafeVerified.Show();
            CurrentState = "Select Safe";
            SetEvents();
            RefreshControls();
            bindingSourceAssigned.DataSource = assigneddeclaredssprocessds.view_smartsafedeclaredselector;
            frmSmartSafeVerified.dataGridViewAssignedDeclared.AutoGenerateColumns = false;
            frmSmartSafeVerified.dataGridViewAssignedDeclared.DataSource = bindingSourceAssigned;
            frmSmartSafeVerified.dataGridViewAssignedDeclared.RowsDefaultCellStyle.BackColor = Color.LightGray;
            frmSmartSafeVerified.dataGridViewAssignedDeclared.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;

            bindingSourceUnassigned.DataSource = unassigneddeclaredssprocessds.view_smartsafedeclaredselector;
            frmSmartSafeVerified.dataGridViewUnassignedDeclared.AutoGenerateColumns = false;
            frmSmartSafeVerified.dataGridViewUnassignedDeclared.DataSource = bindingSourceUnassigned;
            frmSmartSafeVerified.dataGridViewUnassignedDeclared.RowsDefaultCellStyle.BackColor = Color.LightGray;
            frmSmartSafeVerified.dataGridViewAssignedDeclared.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;

            frmSmartSafeVerified.MdiParent = menuform;
            frmSmartSafeVerified.Show();

        }
        public void SetEvents()
        {
            frmSmartSafeVerified.buttonClose.Click += new System.EventHandler(CaptureSmartSafeVerifiedClose);
            frmSmartSafeVerified.buttonSelectSmartSafe.Click += new System.EventHandler(SelectVerifiedSmartSafe);
            groupBoxVerifiedBoxMethods.ButtonSave.Click += new System.EventHandler(SaveSmartSafeTransaction);
            groupBoxVerifiedBoxMethods.ButtonCancel.Click += new System.EventHandler(CancelSmartSafeVerified);
            frmSmartSafeVerified.textBoxDenominations.TextChanged += new EventHandler(CheckDenominationText);
            frmSmartSafeVerified.dataGridViewAssignedDeclared.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(ProcessAssignedDataGridViewDoubleClick);
            frmSmartSafeVerified.dataGridViewUnassignedDeclared.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(ProcessUnassignedDataGridViewDoubleClick);



        }


        private void CancelSmartSafeVerified(object sender, EventArgs e)
        {
            frmSmartSafeVerified.Close();
        }

        private void SaveSmartSafeTransaction(object sender, EventArgs e)
        {
            if (ssprocessds.smartsafetrans[0].bankfedid == "SMARTSAFE")
            {
                SaveSmartSafeVerified();
            }
            else
            {
                SaveSmartSafeCount();
            }
        }
        private void ProcessUnassignedDataGridViewDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            assigneddeclaredssprocessds.view_smartsafedeclaredselector.ImportRow(unassigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows[frmSmartSafeVerified.dataGridViewUnassignedDeclared.CurrentCell.RowIndex]);
            unassigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows[frmSmartSafeVerified.dataGridViewUnassignedDeclared.CurrentCell.RowIndex].Delete();
            assigneddeclaredssprocessds.view_smartsafedeclaredselector.AcceptChanges();
            unassigneddeclaredssprocessds.view_smartsafedeclaredselector.AcceptChanges();
            RefreshDeclaredTotal();
        }
        private void ProcessAssignedDataGridViewDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            unassigneddeclaredssprocessds.view_smartsafedeclaredselector.ImportRow(assigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows[frmSmartSafeVerified.dataGridViewAssignedDeclared.CurrentCell.RowIndex]);
            assigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows[frmSmartSafeVerified.dataGridViewAssignedDeclared.CurrentCell.RowIndex].Delete();
            assigneddeclaredssprocessds.view_smartsafedeclaredselector.AcceptChanges();
            unassigneddeclaredssprocessds.view_smartsafedeclaredselector.AcceptChanges();
            RefreshDeclaredTotal();
        }
        private void SaveSmartSafeVerified()
        {
            ClearParameters();
            AddParms("@bankfedid", ssprocessds.smartsafetrans[0].bankfedid, "SQL");
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
            AddParms("@customerdda", ssprocessds.smartsafetrans[0].customerdda, "SQL");
            AddParms("@postingdate", ssprocessds.smartsafetrans[0].postingdate, "SQL");
            AddParms("@adduser", AppUserClass.AppUserId, "SQL");
            AddParms("@masteraccountid", ssprocessds.smartsafetrans[0].masteraccountid, "SQL");

            AddIntOutputParm("@verifiedid");

            int verifyid = ExecuteIntOutputCommand("wsgsp_Insert_smartsafeverified", CommandType.StoredProcedure);

            CommandString = "UPDATE smartsafetrans SET verifyid = @verifyid WHERE idcol =  @idcol";

            for (int i = 0; i <= assigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows.Count - 1; i++)
            {
                ClearParameters();
                AddParms("@idcol", assigneddeclaredssprocessds.view_smartsafedeclaredselector[i].declid, "SQL");
                AddParms("@verifyid", verifyid);
                try
                {
                    ExecuteCommand(CommandString, CommandType.Text);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            wsgUtilities.wsgNotice("Posting Complete");
            assigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows.Clear();
            unassigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows.Clear();
            ssprocessds.smartsafetrans.Rows.Clear();
            ssprocessselectords.smartsafetrans.Rows.Clear();
            CurrentState = "Select Safe";
            RefreshControls();
        }

        private void SaveSmartSafeCount()
        {
            ClearParameters();
            AddParms("@bankfedid", ssprocessds.smartsafetrans[0].bankfedid, "SQL");
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
            AddParms("@customerdda", ssprocessds.smartsafetrans[0].customerdda, "SQL");
            AddParms("@postingdate", ssprocessds.smartsafetrans[0].postingdate, "SQL");
            AddParms("@adduser", AppUserClass.AppUserId, "SQL");
            AddParms("@masteraccountid", ssprocessds.smartsafetrans[0].masteraccountid, "SQL");
            ExecuteCommand("wsgsp_Insert_smartsafevcount", CommandType.StoredProcedure);
            try
            {
                ExecuteCommand(CommandString, CommandType.Text);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            wsgUtilities.wsgNotice("Posting Complete");
            ssprocessds.smartsafetrans.Rows.Clear();
            ssprocessselectords.smartsafetrans.Rows.Clear();
            CurrentState = "Select Safe";
            RefreshControls();
        }


        private void CaptureSmartSafeVerifiedClose(object sender, EventArgs e)
        {
            frmSmartSafeVerified.Close();
        }
        private void SelectVerifiedSmartSafe(object sender, EventArgs e)
        {
            bool cont = true;
            string[] customerdata = new string[2];
            int smartsafeid = commonAppDataMethods.SelectBankSmartSafe("SMARTSAFE");
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
                if (ssprocessds.view_expandedsafemast[0].bankfedid.TrimEnd() == "SMARTSAFE")
                {
                    customerdata = commonAppDataMethods.GetCustomerData(ssprocessds.view_expandedsafemast[0].storecode);
                    if (customerdata[0].TrimEnd() == "")
                    {
                        wsgUtilities.wsgNotice("There is no account information for that Smart Safe");
                        cont = false;
                    }
                }
                if (cont)
                {
                    bankfedid = ssprocessds.view_expandedsafemast[0].bankfedid.TrimEnd();
                    PostingDate = commonAppDataMethods.GetNextPostDate(bankfedid);

                    if (bankfedid == "SMARTSAFE")
                    {

                        // Fill Declared source
                        ClearParameters();
                        AddParms("@safeid", ssprocessds.view_expandedsafemast[0].idcol, "SQL");
                        CommandString = "SELECT * FROM view_smartsafedeclaredselector WHERE  safeid =  @safeid AND verifyid = 0";
                        assigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows.Clear();
                        FillData(assigneddeclaredssprocessds, "view_smartsafedeclaredselector", CommandString, CommandType.Text);

                        if (assigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows.Count > 0)
                        {
                            RefreshDeclaredTotal();
                            //Create verified row
                            ssprocessds.smartsafetrans.Rows.Clear();
                            EstablishBlankDataTableRow(ssprocessds.smartsafetrans);
                            ssprocessds.smartsafetrans[0].bankfedid = bankfedid;
                            ssprocessds.smartsafetrans[0].postingdate = PostingDate;
                            ssprocessds.smartsafetrans[0].customerdda = customerdata[0];
                            ssprocessds.smartsafetrans[0].masteraccountid = customerdata[1];
                            ssprocessds.smartsafetrans[0].safeid = ssprocessds.view_expandedsafemast[0].idcol;
                            ssprocessds.smartsafetrans[0].eventcode = "VER";
                            ssprocessds.smartsafetrans[0].store = ssprocessds.view_expandedsafemast[0].storecode;
                            frmSmartSafeVerified.labelSmartSafeInformation.Text = ssprocessds.view_expandedsafemast[0].storename;
                            groupBoxVerifiedBoxMethods.RefreshTotals();
                            CurrentState = "Enter Verified";
                            RefreshControls();
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("There are no open declared entries for this Smart Safe");
                            CurrentState = "Select Safe";
                            RefreshControls();
                        }
                    }
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation cancelled");
            }
        }

        public void RefreshDeclaredTotal()
        {
            groupBoxVerifiedBoxMethods.TotalDeclared = 0;

            // Accumulate Declared
            for (int i = 0; i <= assigneddeclaredssprocessds.view_smartsafedeclaredselector.Rows.Count - 1; i++)
            {
                groupBoxVerifiedBoxMethods.TotalDeclared += assigneddeclaredssprocessds.view_smartsafedeclaredselector[i].saidtocontain;
                groupBoxVerifiedBoxMethods.RefreshTotals();
            }
            
        }
        public void RefreshControls()
        {
            DisableControls();
            frmSmartSafeVerified.buttonClose.Enabled = true;
            switch (CurrentState)
            {
                case "Select Safe":
                    {
                        frmSmartSafeVerified.buttonSelectSmartSafe.Enabled = true;
                        frmSmartSafeVerified.buttonClose.Enabled = true;
                        frmSmartSafeVerified.buttonSelectSmartSafe.Enabled = true;
                        break;
                    }
                case "Enter Verified":
                    {
                        frmSmartSafeVerified.dataGridViewAssignedDeclared.Enabled = true;
                        frmSmartSafeVerified.dataGridViewUnassignedDeclared.Enabled = true;
                        frmSmartSafeVerified.textBoxDenominations.Enabled = true;
                        frmSmartSafeVerified.panelVerified.Enabled = true;
                        frmSmartSafeVerified.buttonSelectSmartSafe.Enabled = false;
                        frmSmartSafeVerified.Focus();
                        verifiedGroupBox.Enabled = true;
                    }
                    break;
            }
        }
        public void CheckDenominationText(object sender, EventArgs e)
        {
            if (frmSmartSafeVerified.textBoxDenominations.Text.Count(t => t == '\n') == 7)
            {
                ConvertDenominationString();
                frmSmartSafeVerified.textBoxDenominations.Text = String.Empty;
                groupBoxVerifiedBoxMethods.TextBoxMixedcoin.Focus();
            }
        }
        public void ConvertDenominationString()
        {
            string[] moneylines = System.Text.RegularExpressions.Regex.Split(frmSmartSafeVerified.textBoxDenominations.Text, "\r\n");
            ssprocessds.smartsafetrans[0].ones = Convert.ToDecimal(moneylines[0].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].twos = Convert.ToDecimal(moneylines[1].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].fives = Convert.ToDecimal(moneylines[2].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].tens = Convert.ToDecimal(moneylines[3].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].twentys = Convert.ToDecimal(moneylines[4].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].fiftys = Convert.ToDecimal(moneylines[5].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans[0].hundreds = Convert.ToDecimal(moneylines[6].TrimEnd().TrimStart());
            ssprocessds.smartsafetrans.AcceptChanges();
            groupBoxVerifiedBoxMethods.RefreshTotals();
        }
        public void DisableControls()
        {
            foreach (Control c in frmSmartSafeVerified.Controls)
            {
                if (!(c is Label))
                {
                    c.Enabled = false;
                }
            }
        }
    }
}
