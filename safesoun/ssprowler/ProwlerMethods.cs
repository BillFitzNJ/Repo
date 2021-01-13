using System;
using System.IO;
using System.Data;
using System.Configuration;
using AnalysisClasses;
using CommonAppClasses;
using System.Collections.Generic;
using System.Linq;
using WSGUtilitieslib;
using System.Text;

namespace ssprowler
{
    class ProwlerMethods : WSGDataAccess
    {

        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Web Site Synchronization");
        AppUtilities appUtilities = new AppUtilities();
        sysdata sydatads = new sysdata();
        ssprocess tempprocessds = new ssprocess();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        DateTime processdate = DateTime.Now.Date;
        AnalysisClassess.AnalysisMethods analysisMethods = new AnalysisClassess.AnalysisMethods();
        string commandstring = "";

        public ProwlerMethods()
            : base("SQL", "SQLConnString")
        {
            AppUserClass.AppUserId = "WSG";
        }

        public void StartApp()
        {
            bool exectuteprowler = true;


            if (exectuteprowler)
            {
                // Set time to the start of today
                DateTime LastProwlerDateTime = DateTime.Now.Date;
                TimeSpan ts = new TimeSpan(0, 0, 0);
                LastProwlerDateTime = LastProwlerDateTime.Date + ts;
                analysisMethods.ExecuteReview(LastProwlerDateTime);
                appUtilities.logEvent("Prowler Execution- " + string.Format("{0:MM/dd/yy}", processdate), "Prowler", "OK", false);

            }
        }


    }


}
