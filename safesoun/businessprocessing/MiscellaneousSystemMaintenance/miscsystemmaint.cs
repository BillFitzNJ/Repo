using System;
using System.Collections.Generic;
using CommonAppClasses;
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
namespace MiscellaneousSystemMaintenance
{
  public class MiscSysdataInf : WSGDataAccess
  {
    public sysdata sysdatads { get; set; }
    public sysdata listsysdatads { get; set; }
    public sysdata testsysdatads  { get; set; }
    WSGUtilities wsgUtilities = new WSGUtilities("Miscellaneous System Information");
    public SqlConnection conn = new SqlConnection();
    AppUtilities appUtilities = new AppUtilities();
    AppConstants myAppconstants = new AppConstants();

    public MiscSysdataInf(string DataStore, string AppConfigName)
      : base(DataStore, AppConfigName)
    {
      // Establish the SQL Connection string
      conn.ConnectionString = myAppconstants.SQLConnectionString;
      sysdatads = new sysdata();
      listsysdatads = new sysdata();
      testsysdatads = new sysdata();
      SetIdcol(sysdatads.appuser.idcolColumn);
    }
    public void InitializeAppuser()
    {
      EstablishBlankDataTableRow(sysdatads.appuser);
    }

    public void GetAppUsers()
    {
      this.ClearParameters();
      listsysdatads.appuser.Rows.Clear();
      this.FillData(listsysdatads, "appuser", "wsgsp_getappusers", CommandType.StoredProcedure);
    }
    public void GetSingleAppUser(int idcol)
    {
      this.ClearParameters();
      sysdatads.appuser.Rows.Clear();
      this.AddParms("@idcol", idcol, "SQL");
      this.FillData(sysdatads, "appuser", "wsgsp_getsingleappuser", CommandType.StoredProcedure);
    }
    public bool SaveAppuser()
    {
      bool OkToSave = true;
      if (sysdatads.appuser[0].idcol < 1)
      {
        // test code 
        this.ClearParameters();
        testsysdatads.appuser.Rows.Clear();
        this.AddParms("@userid", sysdatads.appuser[0].userid, "SQL");
        this.FillData(testsysdatads, "appuser", "wsgsp_getappuserbyuserid", CommandType.StoredProcedure);
        if (testsysdatads.appuser.Rows.Count > 0)
        {
          wsgUtilities.wsgNotice("The user id already exists");
          OkToSave = false;
        }
      }
      if (OkToSave)
      {
        GenerateAppTableRowSave(sysdatads.appuser[0]);
      }
      return OkToSave;
    }
    public string LockAppuser(int idcol)
    {
      return LockTableRow( idcol, "appuser");
    }

  }
}