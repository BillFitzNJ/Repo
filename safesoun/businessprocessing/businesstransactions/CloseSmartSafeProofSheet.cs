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
    public class CloseSmartSafeProofSheet : WSGDataAccess
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
        public CloseSmartSafeProofSheet()
            : base("SQL", "SQLConnString")
        {
          SetIdcol(ssprocesscloseds.balance.idcolColumn);
        }

    
      
     

        public void StartCloseSmartSafeProofSheet()
        {
           string bankfedid = "SMARTSAFE";

            string commandtext = "";
            FrmLoading frmLoading = new FrmLoading();
            DateTime thisclosedate;
            commandtext = "SELECT TOP 1 *  FROM balance  WHERE bankfedid = @bankfedid ORDER BY postdate DESC";
            ssprocessopends.balance.Rows.Clear();
            ClearParameters();
            AddParms("@bankfedid",bankfedid, "SQL");
            FillData(ssprocessopends, "balance", commandtext, CommandType.Text);
            if (ssprocessopends.balance.Rows.Count > 0)
            {
                thisclosedate = ssprocessopends.balance[0].postdate.AddDays(1);
            }
            else
            {
                // No prior balance use current date
                thisclosedate = DateTime.Now.Date;
                ssprocessopends.balance.Rows.Add();
                InitializeDataRow(ssprocessopends.balance[0]);
                ssprocessopends.balance[0].bankfedid = bankfedid;
            }
            
            
           if (wsgUtilities.wsgReply("After closing the Proof Sheet date will be " +  String.Format("{0:MM-dd-yy}",thisclosedate) + ". Do you want to proceed?"))
           { 
            
            
            // Process non-verified transactions for the day following the last close date
            ssprocessalltransds.balance.Rows.Clear();
            commandtext = "IF EXISTS (SELECT * FROM smartsafetrans WHERE eventcode <> 'VER' AND postingdate =  @postingdate AND bankfedid = @bankfedid) ";
            commandtext += " select postingdate, saidtocontain = SUM(saidtocontain), hundreds  = SUM(dbo.GetSmartTransValue(eventcode, hundreds)), fiftys = SUM(dbo.GetSmartTransValue(eventcode,fiftys)), twentys  = SUM(dbo.GetSmartTransValue(eventcode,  twentys)),";
            commandtext += " tens  = SUM(dbo.GetSmartTransValue(eventcode, tens)), fives  = SUM(dbo.GetSmartTransValue(eventcode, fives)) , twos  = SUM(dbo.GetSmartTransValue(eventcode, twos)), ";
            commandtext += " ones  = SUM(dbo.GetSmartTransValue(eventcode, ones)), mixedcoin = SUM(dbo.GetSmartTransValue(eventcode, mixedcoin)) ";
            commandtext += " from smartsafetrans WHERE postingdate = @postingdate AND eventcode <> 'VER'AND bankfedid = @bankfedid GROUP by postingdate ";
            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@postingdate", thisclosedate, "SQL");
            FillData(ssprocessalltransds, "balance", commandtext, CommandType.Text);
            if (ssprocessalltransds.balance.Rows.Count < 1)
            {
                ssprocessalltransds.balance.Rows.Add();
                InitializeDataRow(ssprocessalltransds.balance[0]);
            }


            ssprocessverifiedds.balance.Rows.Clear();
            ClearParameters();
            AddParms("@postingdate", thisclosedate, "SQL");
            AddParms("bankfedid", bankfedid, "SQL");
            commandtext = "IF EXISTS (SELECT * FROM smartsafetrans WHERE eventcode = 'VER' AND postingdate =  @postingdate AND bankfedid = @bankfedid) ";
            commandtext += "SELECT COALESCE(sum(saidtocontain),-999) AS saidtocontain, SUM(hundreds) AS hundreds, SUM(fiftys) AS fiftys, SUM(twentys) AS twentys, SUM(tens) AS tens, SUM(fives) AS fives, SUM(twos) AS twos, ";
            commandtext += "SUM(ones) AS ones, SUM(mixedcoin) AS mixedcoin from view_smartsafeverified WHERE postingdate = @postingdate AND bankfedid = @bankfedid ";
            FillData(ssprocessverifiedds, "balance", commandtext, CommandType.Text);
            if (ssprocessverifiedds.balance.Rows.Count > 0)
            {
                if (ssprocessverifiedds.balance[0].saidtocontain == -999)
                {
                    ssprocessverifiedds.balance.Rows.Clear();
                }
            }
               
           if (ssprocessverifiedds.balance.Rows.Count <1)
            {
                ssprocessverifiedds.balance.Rows.Add();
                InitializeDataRow(ssprocessverifiedds.balance[0]);
            }

            ssprocesscloseds.balance.Rows.Add();
            InitializeDataRow(ssprocesscloseds.balance[0]);
            ssprocesscloseds.balance[0].postdate = thisclosedate;
            ssprocesscloseds.balance[0].bankfedid = bankfedid;
           
            // Add opening balance
            ssprocesscloseds.balance[0].saidtocontain = ssprocessopends.balance[0].saidtocontain;
            ssprocesscloseds.balance[0].hundreds = ssprocessopends.balance[0].hundreds;
            ssprocesscloseds.balance[0].fiftys = ssprocessopends.balance[0].fiftys;
            ssprocesscloseds.balance[0].twentys = ssprocessopends.balance[0].twentys;
            ssprocesscloseds.balance[0].tens = ssprocessopends.balance[0].tens;
            ssprocesscloseds.balance[0].fives = ssprocessopends.balance[0].fives;
            ssprocesscloseds.balance[0].twos = ssprocessopends.balance[0].twos;
            ssprocesscloseds.balance[0].ones = ssprocessopends.balance[0].ones;
            ssprocesscloseds.balance[0].mixedcoin = ssprocessopends.balance[0].mixedcoin;



            // Establish all transactions except verified
            ssprocesscloseds.balance[0].saidtocontain += ssprocessalltransds.balance[0].saidtocontain;
            ssprocesscloseds.balance[0].hundreds += ssprocessalltransds.balance[0].hundreds;
            ssprocesscloseds.balance[0].fiftys += ssprocessalltransds.balance[0].fiftys;
            ssprocesscloseds.balance[0].twentys += ssprocessalltransds.balance[0].twentys;
            ssprocesscloseds.balance[0].tens += ssprocessalltransds.balance[0].tens;
            ssprocesscloseds.balance[0].fives += ssprocessalltransds.balance[0].fives;
            ssprocesscloseds.balance[0].twos += ssprocessalltransds.balance[0].twos;
            ssprocesscloseds.balance[0].ones += ssprocessalltransds.balance[0].ones;
            ssprocesscloseds.balance[0].mixedcoin += ssprocessalltransds.balance[0].mixedcoin;


            // Establish verified transactions
            ssprocesscloseds.balance[0].saidtocontain += ssprocessverifiedds.balance[0].saidtocontain;
            ssprocesscloseds.balance[0].hundreds += ssprocessverifiedds.balance[0].hundreds;
            ssprocesscloseds.balance[0].fiftys += ssprocessverifiedds.balance[0].fiftys;
            ssprocesscloseds.balance[0].twentys += ssprocessverifiedds.balance[0].twentys;
            ssprocesscloseds.balance[0].tens += ssprocessverifiedds.balance[0].tens;
            ssprocesscloseds.balance[0].fives += ssprocessverifiedds.balance[0].fives;
            ssprocesscloseds.balance[0].twos += ssprocessverifiedds.balance[0].twos;
            ssprocesscloseds.balance[0].ones += ssprocessverifiedds.balance[0].ones;
            ssprocesscloseds.balance[0].mixedcoin += ssprocessverifiedds.balance[0].mixedcoin;
         
            // Insert the balance row

            ClearParameters();
            AddParms("@bankfedid", bankfedid, "SQL");
            AddParms("@postingdate", thisclosedate, "SQL");
            AddParms("@hundreds", ssprocesscloseds.balance[0].hundreds, "SQL");
            AddParms("@fiftys", ssprocesscloseds.balance[0].fiftys, "SQL");
            AddParms("@twentys", ssprocesscloseds.balance[0].twentys, "SQL");
            AddParms("@tens", ssprocesscloseds.balance[0].tens, "SQL");
            AddParms("@fives", ssprocesscloseds.balance[0].fives, "SQL");
            AddParms("@twos", ssprocesscloseds.balance[0].twos, "SQL");
            AddParms("@ones", ssprocesscloseds.balance[0].ones, "SQL");
            AddParms("@sba", ssprocesscloseds.balance[0].sba, "SQL");
            AddParms("@halves", ssprocesscloseds.balance[0].halves, "SQL");
            AddParms("@quarters", ssprocesscloseds.balance[0].quarters, "SQL");
            AddParms("@dimes", ssprocesscloseds.balance[0].dimes, "SQL");
            AddParms("@nickels", ssprocesscloseds.balance[0].nickels, "SQL");
            AddParms("@pennies", ssprocesscloseds.balance[0].pennies, "SQL");
            AddParms("@mixedcoin", ssprocesscloseds.balance[0].mixedcoin, "SQL");
            AddParms("@foodstamps", ssprocesscloseds.balance[0].foodstamps, "SQL");
            AddParms("@saidtocontain", ssprocesscloseds.balance[0].saidtocontain, "SQL");
            ExecuteCommand("sp_insert_balance", CommandType.StoredProcedure);
            wsgUtilities.wsgNotice("Closing of " + String.Format("{0:MM/dd/yyyy}", thisclosedate) + " Completed. ");
           }

           else
           {
               wsgUtilities.wsgNotice("Closing Cancelled");
           
           }
           
        }
    }

}