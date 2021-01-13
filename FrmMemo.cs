using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.Net.Mail;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CommonAppClasses
{
    public partial class FrmMemo : Form
    {
        AppUtilities appUtilities = new AppUtilities();
        AppConstants myAppconstants = new AppConstants();
        public bool CancelUpdate = false;
        public FrmMemo()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            CancelUpdate = true;
            this.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
           textBoxMemo.Text = textBoxMemo.Text + Environment.NewLine + AppUserClass.AppUserId + " - " + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString() + Environment.NewLine;
         
            CancelUpdate = false;
            this.Close();
        }
    }
}
