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
    public class SmartSafeIncomingBulk : WSGDataAccess
    {

        ssprocess ssprocessds = new ssprocess();
        ssprocess workingssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        FrmSmartSafeBulk  frmSmartSafeBulk = new FrmSmartSafeBulk();
        string bankfedid = "SMARTSAFE";
        public Form menuform { get; set; }
        GroupBoxVerifiedMethods countingGroupBoxMethods = new GroupBoxVerifiedMethods();
        BindingSource DeclaredHistory = new BindingSource();
        DateTime PostingDate = new DateTime();
        GroupBox countingGroupBox = new GroupBox();
        WSGUtilities wsgUtilities = new WSGUtilities("Smart Safe Verified");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        string CommandString = "";
        string CurrentState = "";
        public SmartSafeIncomingBulk()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.smartsafetrans.idcolColumn);
            countingGroupBox = countingGroupBoxMethods.SetGroupBox(ssprocessds.smartsafetrans, ssprocessds);
            frmSmartSafeBulk.panelVerified.Controls.Add(countingGroupBox);    
            SetEvents();
        }

        public void EnterSmartSafeIncomingBulk()
        {
            PostingDate = commonAppDataMethods.GetNextPostDate(bankfedid);
           
           frmSmartSafeBulk.MdiParent = menuform;
           frmSmartSafeBulk.Text = "SmartSafe Incoming Bulk";
            frmSmartSafeBulk.Show();
            ssprocessds.smartsafetrans.Rows.Clear();
            EstablishBlankDataTableRow(ssprocessds.smartsafetrans);
            ssprocessds.smartsafetrans[0].bankfedid = bankfedid;
            ssprocessds.smartsafetrans[0].postingdate = PostingDate;
            ssprocessds.smartsafetrans[0].customerdda = "";
            ssprocessds.smartsafetrans[0].masteraccountid = "";
            ssprocessds.smartsafetrans[0].safeid = 0;
            ssprocessds.smartsafetrans[0].eventcode = "FED";
            ssprocessds.smartsafetrans[0].store = "";
            countingGroupBoxMethods.LabelDeclared.Visible = false;
            countingGroupBoxMethods.LabelDifference.Visible = false;
            countingGroupBoxMethods.RefreshTotals();
             frmSmartSafeBulk.Show();

        }
        public void SetEvents()
        {
            countingGroupBoxMethods.ButtonSave.Click += new System.EventHandler(SaveSmartSafeIncomingBulk);
            countingGroupBoxMethods.ButtonCancel.Click += new System.EventHandler(CancelSmartSafeIncomingBulk);

        }

        private void SaveSmartSafeIncomingBulk(object sender, EventArgs e)
        {

            ClearParameters();
            AddParms("@bankfedid", ssprocessds.smartsafetrans[0].bankfedid, "SQL");
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

            try
            {
                ExecuteCommand("wsgsp_Insert_smartsafeincomingbulk", CommandType.StoredProcedure);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            wsgUtilities.wsgNotice("Posting Complete");
          frmSmartSafeBulk.Close();
        }
        private void CancelSmartSafeIncomingBulk(object sender, EventArgs e)
        {
        frmSmartSafeBulk.Close();
        }

    }

}
