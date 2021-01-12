using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
namespace ApplicationMaintenance
{

    public class GetNewStorecodeMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Moneycenter Assignment");
        AppUtilities appUtilities = new AppUtilities();
        public Form menuform { get; set; }
        BindingSource bindingsourceStore = new BindingSource();
        public bool cont;
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess ssprocessSelectords = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocesssearchds = new ssprocess();
        FrmGetNewStorecode frmGetNewStorecode = new FrmGetNewStorecode();
        string CurrentState = "";
        string commandstring = "";
        string NewStorecode = "";
        string storecodesuffix = "";
        public GetNewStorecodeMethods()
            : base("SQL", "SQLConnString")
        {
            SetEvents();
        }

        public string GetNewStoreCode(string compcode)
        {
            commandstring = "SELECT * FROM company WHERE comp_code  = @compcode";
            ssprocessSelectords.company.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode, "SQL");
            FillData(ssprocessSelectords, "company", commandstring, CommandType.Text);

            bindingsourceStore.DataSource = ssprocessSelectords.store;
            frmGetNewStorecode.dataGridViewStoreData.DataSource = bindingsourceStore;
            frmGetNewStorecode.dataGridViewStoreData.RowsDefaultCellStyle.BackColor = Color.LightGray;
            frmGetNewStorecode.dataGridViewStoreData.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            frmGetNewStorecode.dataGridViewStoreData.AutoGenerateColumns = false;
            ssprocessSelectords.store.Rows.Clear();
            commandstring = "SELECT * FROM store WHERE LEFT(storecode,4) = @compcode";
            ClearParameters();
            AddParms("@compcode", compcode, "SQL");
            FillData(ssprocessSelectords, "store", commandstring, CommandType.Text);

            frmGetNewStorecode.labelCompanyData.Text = "Stores for company " + ssprocessSelectords.company[0].name.TrimEnd();
            frmGetNewStorecode.textBoxStorecode.Focus(); ;
            frmGetNewStorecode.textBoxStorecode.DataBindings.Clear();
            frmGetNewStorecode.textBoxStorecode.Text = "";
            frmGetNewStorecode.ShowDialog();
            return NewStorecode;

        }

        public void SetEvents()
        {

            frmGetNewStorecode.buttonCancel.Click += new System.EventHandler(ProcessCancel);
            frmGetNewStorecode.buttonProceed.Click += new System.EventHandler(ButtonProceed_Click);
            frmGetNewStorecode.textBoxStorecode.TextChanged += new EventHandler(TextBoxStorecode__TextChanged);
        }

        public void ProcessCancel(object sender, EventArgs e)
        {
            NewStorecode = "";
            frmGetNewStorecode.Close();
        }


        protected void TextBoxStorecode__TextChanged(object sender, EventArgs e)
        {

            if (frmGetNewStorecode.textBoxStorecode.Text.TrimEnd().Length == 6)
            {
                if (ValidaateNewStoreCode())
                {
                    frmGetNewStorecode.Close();
                }
            }
        }
        public bool ValidaateNewStoreCode()
        {
            cont = true;
            if (frmGetNewStorecode.textBoxStorecode.Text.TrimEnd().Length != 6)
            {
                wsgUtilities.wsgNotice("The store code must end with 6 characters");
                cont = false;
            }
            if (cont)
            {
                NewStorecode = ssprocessSelectords.company[0].comp_code + "-" + frmGetNewStorecode.textBoxStorecode.Text;


                ssprocessds.store.Rows.Clear();
                commandstring = "SELECT * FROM store WHERE LEFT(storecode,11) = @storecode";
                ClearParameters();
                AddParms("@storecode", NewStorecode, "SQL");
                FillData(ssprocessds, "store", commandstring, CommandType.Text);
                if (ssprocessds.store.Rows.Count > 0)
                {
                    wsgUtilities.wsgNotice("That store code is in use");
                    cont = false;
                }

            }
            return cont;
        }

        public void ButtonProceed_Click(object sender, EventArgs e)
        {
            bool cont = true;

            if (ValidaateNewStoreCode())
            {
                frmGetNewStorecode.Close();
            }
        }


    }
}

