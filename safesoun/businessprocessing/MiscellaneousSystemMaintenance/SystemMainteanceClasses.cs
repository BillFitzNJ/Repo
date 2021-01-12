using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommonAppClasses;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using WSGUtilitieslib;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiscellaneousSystemMaintenance
{
  public class MiscSysInf : WSGDataAccess
  {
    AppUtilities appUtilities = new AppUtilities();
    AppConstants myAppconstants = new AppConstants();
    WSGUtilities wsgUtilities = new WSGUtilities("SO Information");
    AppInformation appInformation = new AppInformation("SQL", "SQLConnString");
    public MiscSysInf(string DataStore, string AppConfigName)
      : base(DataStore, AppConfigName)
    {
  
    }
    public void ClearSomastLocks()
    {
      this.ClearParameters();
      try
      {
        ExecuteCommand("wsgsp_clearsomastlocks", CommandType.StoredProcedure);
        wsgUtilities.wsgNotice("Somast locks cleared");
      }
      catch (SqlException ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
    public void ClearArcustLocks()
    {
      this.ClearParameters();
      try
      {
        ExecuteCommand("wsgsp_cleararcustlocks", CommandType.StoredProcedure);
        wsgUtilities.wsgNotice("Arcust locks cleared");
      }
      catch (SqlException ex)
      {
        MessageBox.Show(ex.Message);
      }
    }
    public void ClearInvoicing()
    {
       UnlockInvoicing();
       wsgUtilities.wsgNotice("Invoicing cleared");
    }
}


}
