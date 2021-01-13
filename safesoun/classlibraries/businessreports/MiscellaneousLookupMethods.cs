using System;
using System.Collections.Generic;
using Microsoft.Reporting.WinForms;
using CommonAppClasses;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;
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
namespace BusinessReports
{


    public class MiscellaneousLookupMethods : WSGDataAccess
    {
        public MiscellaneousLookupMethods()
            : base("SQL", "SQLConnString")
        {
        }
        public void AccountLookup()
        {
            ssprocess ssprocessselectords = new ssprocess();
            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            ssprocessselectords.view_accountlookup.Rows.Clear();
            string commandtext = "SELECT * FROM  view_accountlookup ORDER BY account";
            ;
            ClearParameters();
            FillData(ssprocessselectords, "view_accountlookup", commandtext, CommandType.Text);
            frmSelectorMethods.FormText = "Account Lookup";
            frmSelectorMethods.dtSource = ssprocessselectords.view_accountlookup;
            frmSelectorMethods.columncount = 3;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "Account";
            frmSelectorMethods.colheadertext[0] = "Account";
            frmSelectorMethods.coldatapropertyname[0] = "account";
            frmSelectorMethods.colwidth[0] = 100;
            frmSelectorMethods.colname[1] = "storecode";
            frmSelectorMethods.colheadertext[1] = "Code";
            frmSelectorMethods.coldatapropertyname[1] = "storecode";
            frmSelectorMethods.colwidth[1] = 100;
             frmSelectorMethods.colname[2] = "Accountname";
            frmSelectorMethods.colheadertext[2] = "Account Name";
            frmSelectorMethods.coldatapropertyname[2] = "accountname";
            frmSelectorMethods.colwidth[2] = 500;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 750;
            string selectacount = frmSelectorMethods.ShowStringSelector("account");

        }
    }
}
