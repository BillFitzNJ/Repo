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
    public partial class FrmSmartSafeDeclared : Form
    {
        public Form menuform { get; set; }
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Event");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public FrmSmartSafeDeclared()
        {
            InitializeComponent();
        }
      }
}
