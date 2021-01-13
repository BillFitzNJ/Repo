using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Drawing;
using CommonAppClasses;
using System.Data.OleDb;
using System.Data.SqlClient;
using WSGUtilitieslib;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessTransactions
{
  public partial class FrmExtractBankFiles : Form
  {
    AppUtilities appUtilities = new AppUtilities();
    WSGUtilities wsgUtilities = new WSGUtilities("FTP Send");
    public FrmExtractBankFiles()
    {
      InitializeComponent();
    }

  
  }
}
