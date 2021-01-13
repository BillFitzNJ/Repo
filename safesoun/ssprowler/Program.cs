using System;
using System.IO;
using System.Data;
using System.Web;
using System.Net;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using BusinessReports;
using CommonAppClasses;
using WSGUtilitieslib;
using System.Text;

namespace ssprowler
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ProwlerMethods prowlerMethods = new ProwlerMethods();
            prowlerMethods.StartApp();
        }
    }
}
