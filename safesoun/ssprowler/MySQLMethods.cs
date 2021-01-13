using System;
using System.IO;
using System.Data;
using System.Web;
using System.Net;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using BusinessReports;
using CommonAppClasses;
using WSGUtilitieslib;
using System.Text;
namespace AnalysisClasses
{
    public class MySQLMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Event");
        AppUtilities appUtilities = new AppUtilities();
        public mysqldata mysqldatads = new mysqldata();
        public mysqldata mysqltempdatads = new mysqldata();
        string commandstring = "";
        public MySQLMethods()
            : base("MySQL", "MySQLConnStringWithDB")
        {

        }
    

        public void FillDTSMySqlPickups(string storecode, DateTime dtsprocessdate)
        {
            mysqldatads.hhpickup.Rows.Clear();
            string commandstring = "select * from hhpickup where pickupstatus = 'Y' AND  storecode = @storecode and pickupstatus = 'Y' and pickupdate = @dtsprocessdate";
            ClearParameters();
            AddParms("@storecode", storecode);
            AddParms("@dtsprocessdate", dtsprocessdate);
            FillData(mysqldatads, "hhpickup", commandstring, CommandType.Text);
        }
        public void FillMissingDTSPickups(DateTime dtsprocessdate)
        {
            mysqldatads.hhpickup.Rows.Clear();
            string commandstring = "select * from hhpickup where  left(storecode,4) = '7822' and pickupdate = @dtsprocessdate and adduser <> 'wsg'";
            ClearParameters();
            AddParms("@dtsprocessdate", dtsprocessdate);
            FillData(mysqldatads, "hhpickup", commandstring, CommandType.Text);
        }
    
    }
}
