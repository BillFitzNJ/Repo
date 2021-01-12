using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
namespace ApplicationMaintenance
{


    public class SmartSafeMaintenanceMethods : WSGDataAccess
    {
       
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Smart Safe Maintenance");
        AppUtilities appUtilities = new AppUtilities();
        public Form menuform { get; set; }
        public bool cont;
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        MySQLDataMethods mysqlDataMethods = new MySQLDataMethods();
        ssprocess ssprocesssearchds = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        FrmMaintainSmartSafe frmMaintainSmartSafe = new FrmMaintainSmartSafe();
        string CurrentState = "";
        string commandstring = "";
        string comp_code = "";
        public SmartSafeMaintenanceMethods()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.safemast.idcolColumn);
        }

        public void StartApp()
        {
            frmMaintainSmartSafe.MdiParent = menuform;
            SetEvents();
            SetSmartSafeBindings();
            CurrentState = "Select";
            RefreshControls();
            frmMaintainSmartSafe.Show();
        }


        public void SetEvents()
        {
            frmMaintainSmartSafe.buttonClose.Click += new System.EventHandler(buttonClose_Click);
            frmMaintainSmartSafe.buttonInsert.Click += new System.EventHandler(buttonInsert_Click);
            frmMaintainSmartSafe.buttonEdit.Click += new System.EventHandler(buttonEdit_Click);
            frmMaintainSmartSafe.buttonSelect.Click += new System.EventHandler(buttonSelect_Click);
            frmMaintainSmartSafe.buttonDelete.Click += new System.EventHandler(buttonDelete_Click);
            frmMaintainSmartSafe.buttonSave.Click += new System.EventHandler(buttonSave_Click);
            frmMaintainSmartSafe.buttonSelectStore.Click += new System.EventHandler(buttonSelectStore_Click);
            frmMaintainSmartSafe.buttonSelectBank.Click += new System.EventHandler(buttonSelectBank_Click);
       
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            frmMaintainSmartSafe.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ssprocessds.safemast[0].manufacturer = frmMaintainSmartSafe.comboBoxManufacturer.SelectedItem.ToString(); ; 
            ssprocessds.safemast.AcceptChanges();
            if (ValidateSafe())
            {
                GenerateAppTableRowSave(ssprocessds.safemast[0]);
                mysqlDataMethods.MergeMySQLDataRow(ssprocessds.safemast, "serialnumber");
                wsgUtilities.wsgNotice("Save Complete");
                CurrentState = "Select";
                RefreshControls();
            }
        }

        private bool ValidateSafe()
        {
            bool safeok = true;
            if (ssprocessds.safemast[0].bankfedid.TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("There  must be a bank");
                safeok = false;
             
            }
            if (safeok)
            {

                if (ssprocessds.safemast[0].storecode.TrimEnd() == "")
                {
                    wsgUtilities.wsgNotice("There  must be a store");
                    safeok = false;

                }
            }
            if (safeok)
            {

                if (ssprocessds.safemast[0].serialnumber.TrimEnd() == "")
                {
                    wsgUtilities.wsgNotice("There  must be a serial number");
                    safeok = false;

                }
            } 
            if (safeok)
            {
                ssprocesssearchds.safemast.Rows.Clear();
                commandstring = "SELECT * FROM safemast WHERE serialnumber= @serialnumber";
                ClearParameters();
                AddParms("@serialnumber", ssprocessds.safemast[0].serialnumber, "SQL");
                FillData(ssprocesssearchds, "safemast", commandstring, CommandType.Text);
                if (ssprocesssearchds.safemast.Rows.Count > 0)
                {
                    wsgUtilities.wsgNotice("That serial number is already in the file");
                    safeok = false;
                }
            }
            return safeok;
        }
        private void buttonEdit_Click(object sender, EventArgs e)
        {
            CurrentState = "Edit";
            RefreshControls();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (wsgUtilities.wsgReply("Delete this item?"))
            {
                commandstring = "DELETE FROM safemast WHERE idcol= @idcol";
                ClearParameters();
                AddParms("@idcol", ssprocessds.safemast[0].idcol, "SQL");
                ssprocessds.safemast.Rows.Clear();
                frmMaintainSmartSafe.textBoxStorename.Text = "";
                ExecuteCommand(commandstring, CommandType.Text);
                wsgUtilities.wsgNotice("Item Deleted");
                CurrentState = "Select";
                RefreshControls();
            }
            else
            {
                wsgUtilities.wsgNotice("Deletion cancelled");

            }

        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            int selectedsafemastid = commonAppDataMethods.SelectSmartSafe();
            if (selectedsafemastid > 0)
            {
                ssprocessds.safemast.Rows.Clear();
                commandstring = "SELECT * FROM safemast WHERE idcol= @idcol";
                ClearParameters();
                AddParms("@idcol", selectedsafemastid, "SQL");
                FillData(ssprocessds, "safemast", commandstring, CommandType.Text);
                frmMaintainSmartSafe.comboBoxManufacturer.SelectedItem = ssprocessds.safemast[0].manufacturer.TrimStart().TrimEnd(); 
                RefreshStoreName();
                CurrentState = "View";
                RefreshControls();
            }

        }


        private void buttonSelectBank_Click(object sender, EventArgs e)
        {
            ssprocessds.safemast[0].bankfedid = commonAppDataMethods.SelectBankFedid();
            ssprocessds.safemast.AcceptChanges();
            RefreshStoreName();
        }

        private void buttonSelectStore_Click(object sender, EventArgs e)
        {
            ssprocessds.safemast[0].storecode = commonAppDataMethods.SelectCompanyAndStore();
            ssprocessds.safemast.AcceptChanges();
            RefreshStoreName();
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            CurrentState = "Insert";
            EstablishBlankDataTableRow(ssprocessds.safemast);
            // Establish defaults
            ssprocessds.safemast[0].bankfedid = "CCSMARTSAFE7";
            ssprocessds.safemast[0].manufacturer = "AMSEC";
            frmMaintainSmartSafe.comboBoxManufacturer.SelectedItem = ssprocessds.safemast[0].manufacturer; 

            RefreshControls();
        }

        public void RefreshBank()
        {
            frmMaintainSmartSafe.textBoxBankFedid.Text = ssprocessds.safemast[0].bankfedid;
        }
  
        public void RefreshStoreName()
        {
            frmMaintainSmartSafe.textBoxStorename.Text = commonAppDataMethods.GetStoreName(ssprocessds.safemast[0].storecode);
        }
        public void SetSmartSafeBindings()
        {
            frmMaintainSmartSafe.textBoxSerialNumber.DataBindings.Clear();
            frmMaintainSmartSafe.textBoxSerialNumber.DataBindings.Add("Text", ssprocessds.safemast, "serialnumber");
            frmMaintainSmartSafe.textBoxStorecode.DataBindings.Clear();
            frmMaintainSmartSafe.textBoxStorecode.DataBindings.Add("Text", ssprocessds.safemast, "storecode");
            frmMaintainSmartSafe.textBoxBankFedid.DataBindings.Clear();
            frmMaintainSmartSafe.textBoxBankFedid.DataBindings.Add("Text", ssprocessds.safemast, "bankfedid");
        

        }

        public void RefreshControls()
        {
            DisableControls();
            frmMaintainSmartSafe.buttonClose.Enabled = true;
            switch (CurrentState)
            {
                case "Select":
                    frmMaintainSmartSafe.buttonSelect.Enabled = true;
                    frmMaintainSmartSafe.buttonInsert.Enabled = true;
                    break;

                case "View":
                    frmMaintainSmartSafe.buttonSelect.Enabled = true;
                    frmMaintainSmartSafe.buttonEdit.Enabled = true;
                    frmMaintainSmartSafe.buttonDelete.Enabled = true;
                    break;
                case "Insert":
                    frmMaintainSmartSafe.buttonSave.Enabled = true;
                    frmMaintainSmartSafe.buttonSelectStore.Enabled = true;
                    frmMaintainSmartSafe.textBoxSerialNumber.Enabled = true;
                    frmMaintainSmartSafe.buttonSelectBank.Enabled = true;
                    frmMaintainSmartSafe.comboBoxManufacturer.Enabled = true;
                    break;
                case "Edit":
                    frmMaintainSmartSafe.buttonSave.Enabled = true;
                    frmMaintainSmartSafe.buttonSelectStore.Enabled = true;
                    frmMaintainSmartSafe.textBoxSerialNumber.Enabled = true;
                    frmMaintainSmartSafe.comboBoxManufacturer.Enabled = true;
                    frmMaintainSmartSafe.buttonSelectBank.Enabled = true;
                    break;
            }

        }

        public void DisableControls()
        {
            foreach (Control c in frmMaintainSmartSafe.Controls)
            {
                if (!(c is Label))
                {
                    c.Enabled = false;
                }
            }

        }
    }
}
