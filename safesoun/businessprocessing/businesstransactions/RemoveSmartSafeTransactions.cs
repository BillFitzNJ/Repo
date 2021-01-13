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
    public class RemoveSmartSafeTransactions : WSGDataAccess
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
        public RemoveSmartSafeTransactions()
            : base("SQL", "SQLConnString")
        {

        }

        public void StartApp(string Transtype)
        {
            PostingDate = commonAppDataMethods.GetNextPostDate(bankfedid);
            int smartsafeid = commonAppDataMethods.SelectSmartSafe();
            if (smartsafeid != 0)
            {

                CommandString = " SELECT *  FROM safemast  WHERE idcol = @safeid ";
                ClearParameters();
                AddParms("@safeid", smartsafeid, "SQL");
                FillData(ssprocessds, "safemast", CommandString, CommandType.Text);


                // If Signature Bank, use "SMARTSAFE", otherwise use bankfedid
                if (ssprocessds.safemast[0].bankfedid.Substring(0, 1) == "8")
                {
                    bankfedid = "SMARTSAFE";
                }
                else
                {
                    bankfedid = ssprocessds.safemast[0].bankfedid.TrimEnd();
                }

                PostingDate = commonAppDataMethods.GetNextPostDate(bankfedid);

                if (Transtype == "D")
                {
                    // Find any declared entries for this safe and date
                    CommandString = " SELECT *  FROM view_expandedsmartsafetrans ";
                    CommandString += " WHERE safeid = @safeid AND CAST(postingdate AS date) =  @postingdate  AND eventcode = 'DECL' AND verifyid = 0";
                }
                else
                {
                    // Find any verified entries for this safe and date
                    CommandString = "SELECT * FROM view_expandedsmartsafetrans ";
                    CommandString += "   WHERE safeid = @safeid AND CAST(postingdate AS date) = @postingdate  AND eventcode = 'VER' ";
                }



                ssprocessselectords.view_expandedsmartsafetrans.Rows.Clear();
                ClearParameters();
                AddParms("@safeid", smartsafeid, "SQL");
                AddParms("@postingdate", PostingDate, "SQL");
                FillData(ssprocessselectords, "view_expandedsmartsafetrans", CommandString, CommandType.Text);
                if (ssprocessselectords.view_expandedsmartsafetrans.Rows.Count > 0)
                {

                    if (Transtype == "D")
                    {
                        if (wsgUtilities.wsgReply("Delete declared entries for " + ssprocessselectords.view_expandedsmartsafetrans[0].storename.TrimEnd() + "?") == true)
                        {
                            ClearParameters();
                            AddParms("@safeid", smartsafeid, "SQL");
                            AddParms("@postingdate", PostingDate, "SQL");

                            CommandString = "DELETE FROM smartsafetrans WHERE safeid = @safeid AND CAST(postingdate AS date) =  @postingdate  AND eventcode = 'DECL' AND verifyid = 0";
                            ExecuteCommand(CommandString, CommandType.Text);
                            wsgUtilities.wsgNotice("Operation Complete");
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("Operation Cancelled");
                        }
                    }
                    else
                    {
                        if (wsgUtilities.wsgReply("Delete verified entry for " + ssprocessselectords.view_expandedsmartsafetrans[0].storename.TrimEnd() + "?") == true)
                        {
                            // Delete the verfied transaction
                            ClearParameters();
                            AddParms("@idcol", ssprocessselectords.view_expandedsmartsafetrans[0].idcol, "SQL");
                            CommandString = " DELETE FROM smartsafetrans WHERE idcol = @idcol";
                            ExecuteCommand(CommandString, CommandType.Text);
                            //  Remove verifed indicator from declared transactions.
                            ClearParameters();
                            AddParms("@idcol", ssprocessselectords.view_expandedsmartsafetrans[0].idcol, "SQL");
                            CommandString = " UPDATE smartsafetrans SET verifyid = 0 WHERE eventcode = 'DECL' AND verifyid  = @idcol";
                            ExecuteCommand(CommandString, CommandType.Text);
                            wsgUtilities.wsgNotice("Operation Complete");
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("Operation Cancelled");
                        }

                    }

                }
                else
                {
                    wsgUtilities.wsgNotice("There are no removeable transactions for that safe for this date");
                }

            }
            else
            {
                wsgUtilities.wsgNotice("Transaction Cancelled");
            }
        }
    }
}
