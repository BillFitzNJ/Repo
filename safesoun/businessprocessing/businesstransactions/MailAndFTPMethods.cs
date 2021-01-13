 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Net;
using System.Configuration;
using System.Drawing;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using WSGUtilitieslib;
using System.Linq;
using System.Text;
using System.Windows.Forms;
 
 namespace CommonAppClasses
{
  public class SafekeepingMethods : WSGDataAccess
  {
    AppConstants myAppconstants = new AppConstants();
    WSGUtilities wsgUtilities = new WSGUtilities("Safekeeping");
    AppUtilities appUtilities = new AppUtilities();
    CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
    ssprocess ssprocessds = new ssprocess();

    ssprocess ssprocesssearchds = new ssprocess();
    public SafekeepingMethods()
      : base("SQL", "SQLConnString")
    {

    }
    
  
  }
}
 