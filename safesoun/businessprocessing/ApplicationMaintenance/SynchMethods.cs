using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.IO;
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
    public class SynchMethods : WSGDataAccess
    {
        #region Declarations
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Synch Processing");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public Form parentForm = new Form();
        public Form menuform { get; set; }
        DataRow[] foundRows;
        public string commandtext { get; set; }
        // Custom Controls
        Icon AppIcon = new Icon("appicon.ico");
        ssprocess openprocessds = new ssprocess();
        ssprocess tempprocessds = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        ssprocess closeprocessds = new ssprocess();
        mysqldata mysqldatads = new mysqldata();
        ssprocess ssprocessds = new ssprocess();
        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        MySQLSynchMethods mySQLSynchMethods = new MySQLSynchMethods();
        #endregion
        public SynchMethods()
            : base("SQL", "SQLConnString")
        {

        }
        public void StartApp()
        {
            if (wsgUtilities.wsgReply("Do you want to synchronize reference tables?"))
            {
                bool updatedropschedule = false;
                bool updatestore = true;
                bool updatedriver = false;
                bool updatemoneycenters = true;
            
                FrmLoading frmLoading = new FrmLoading();

                frmLoading.Show();
                mySQLSynchMethods.StartApp();
                frmLoading.Close();
                wsgUtilities.wsgNotice("Operation Complete");
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");

            }
        }
    }
}

