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
    public class ReopenSmartSafeProofsheet : WSGDataAccess
    {

        ssprocess ssprocessopends = new ssprocess();
        ssprocess ssprocesscloseds = new ssprocess();
        ssprocess ssprocessalltransds = new ssprocess();
        ssprocess ssprocessverifiedds = new ssprocess();
        FrmSmartSafeDeclared frmSmartSafeDeclared = new FrmSmartSafeDeclared();
        public Form menuform { get; set; }
        WSGUtilities wsgUtilities = new WSGUtilities("Smart Safe");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        bool cont = true;
        int closeidcol = 0;
        public ReopenSmartSafeProofsheet()
            : base("SQL", "SQLConnString")
        {
        }
        public void StartReopenSmartSafeProofSheet()
        {
            string bankfedid = "SMARTSAFE";

            string commandtext = "";
            FrmLoading frmLoading = new FrmLoading();
            DateTime thisclosedate;
            commandtext = "SELECT TOP 1 *  FROM balance  WHERE bankfedid = @bankfedid ORDER BY postdate DESC";
            ssprocessopends.balance.Rows.Clear();
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(ssprocessopends, "balance", commandtext, CommandType.Text);
            if (ssprocessopends.balance.Rows.Count > 0)
            {
                  closeidcol  = ssprocessopends.balance[0].idcol;
            }
            else
            {
                wsgUtilities.wsgNotice("There is no prior close");
                cont = false;
       
            }

            if (cont)
            {
            if (wsgUtilities.wsgReply("Delete closing balance for Smart Safe for " + String.Format("{0:MM/dd/yyyy}", ssprocessopends.balance[0].postdate) + "?"))
            {
                commandtext = "DELETE FROM balance WHERE idcol  = @idcol";
                ClearParameters();
                AddParms("@idcol", closeidcol, "SQL");
                ExecuteCommand(commandtext, CommandType.Text);
                wsgUtilities.wsgNotice("Operation Complete");
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
            }
        }
    }

}
