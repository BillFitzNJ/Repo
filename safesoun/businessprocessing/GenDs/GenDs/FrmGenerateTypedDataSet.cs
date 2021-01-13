using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GenDs
{
    public partial class FrmGenerateTypedDataSet : Form
    {
        string connectionString = ConfigurationManager.AppSettings["SQLConnString"];
        string mysqlconnectionString = ConfigurationManager.AppSettings["MySQLConnString"];
        // Establish VFP Connection
        OleDbConnection vfpardataconn = new OleDbConnection(ConfigurationManager.AppSettings["ArdataConnString"]);
        OleDbConnection vfpacdataconn = new OleDbConnection(ConfigurationManager.AppSettings["AcdataConnString"]);
        OleDbConnection vfpsysdataconn = new OleDbConnection(ConfigurationManager.AppSettings["Pro6ConnString"]);

        // set up the DataSet with the DataSet name
        DataSet ds = new DataSet();


        public FrmGenerateTypedDataSet()
        {
            InitializeComponent();
        }




        private void buttonCreateATMDataset_Click(object sender, EventArgs e)
        {
            ds.DataSetName = "atm";
            ds.Tables.Clear();
            // Tables
            CreateDataTable("atmmast");
            CreateDataTable("atmdrop");
            CreateDataTable("atmdropimport");
            CreateDataTable("atmwksht");
            CreateDataTable("atmjournal");
            CreateDataTable("atmservice");
            // Views
            CreateDataTable("view_activeatmdrop");
            CreateDataTable("view_activefundingcredit");
            CreateDataTable("view_activeATMDropUngrouped");
            CreateDataTable("view_ExpandedATMDrop");
            CreateDataTable("view_ExpandedATMDropImport");
            CreateDataTable("view_ATMDropWithStore");
            CreateDataTable("view_atmjournalhistory");
            CreateDataTable("view_ATMWkshtDispatchData");
            CreateDataTable("view_fundingrequestactivity");
            CreateDataTable("view_expandedFRactivity");
            CreateDataTable("view_fundingrequestreceipts");
            CreateSchema("atm.xsd");

        }


        public void CreateSchema(string filename)
        {
            //Write out the schema
            ds.WriteXmlSchema(filename);
            MessageBox.Show(filename + " successfully created");
        }


        public void CreateVirtualVFPDataTable(string selectstring, string targettablename, OleDbConnection vfpconn)
        {

            try
            {
                // set up the Oledb Command and DataAdapter with the Select Command
                OleDbCommand oledbc = new OleDbCommand(selectstring);
                oledbc.CommandType = CommandType.Text;
                oledbc.Connection = vfpconn;
                OleDbDataAdapter da = new OleDbDataAdapter(oledbc);
                da.TableMappings.Clear();
                da.TableMappings.Add("Table", targettablename);
                // and fill the Datatable
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\r\n" + ex.InnerException.Message;
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }


        private void CreateVFPDataTable(string sourcetablename, string targettablename, OleDbConnection vfpconn)
        {

            try
            {
                // set up the Oledb Command and DataAdapter with the Select Command
                OleDbCommand oledbc = new OleDbCommand("SELECT top 1 * FROM " + sourcetablename + " ORDER BY 1");
                oledbc.CommandType = CommandType.Text;
                oledbc.Connection = vfpconn;
                OleDbDataAdapter da = new OleDbDataAdapter(oledbc);
                da.TableMappings.Clear();
                da.TableMappings.Add("Table", targettablename);
                // and fill the Datatable
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\r\n" + ex.InnerException.Message;
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }

        private void CreateDataTable(string tablename)
        {
            // set up the connection with the database name
            // set up the connection with the database name
            //  string connectionString = TestConnection + ";database= meycotest ";

            try
            {
                // set up the Sql Command and DataAdapter with the Stored Proc name
                SqlCommand sc = new SqlCommand("SELECT TOP 0 *  FROM " + tablename, new SqlConnection(connectionString));
                sc.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(sc);
                da.TableMappings.Clear();
                da.TableMappings.Add("Table", tablename);
                // and fill the DataSet
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\r\n" + ex.InnerException.Message;
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }

        private void buttonCreateVFPApplicationDataSet_Click(object sender, EventArgs e)
        {
        }


        private void buttonCreateSystemTablesDataset_Click(object sender, EventArgs e)
        {
            ds.DataSetName = "sysdata";
            ds.Tables.Clear();
            // Tables
            CreateDataTable("appuser");
            CreateDataTable("apppriv");
            CreateDataTable("approle");
            CreateDataTable("appprocess");
            CreateDataTable("appevent");
            CreateDataTable("appinfo");
            CreateDataTable("appeventsubscription");
            CreateDataTable("eventlog");
            // Views
            CreateDataTable("view_expandedsubscription");
            CreateSchema("sysdata.xsd");

        }

       

        private void buttonCreateVFPArdataDataset_Click(object sender, EventArgs e)
        {
            ds.DataSetName = "VFPArdata";
            ds.Tables.Clear();
            //Tables
            CreateVFPDataTable("armast01", "armast", vfpardataconn);
            CreateVFPDataTable("arcust01", "arcust", vfpardataconn);

            CreateSchema("VFPArdata.xsd");

        }

        private void buttonCreateSSProcesDataSet_Click(object sender, EventArgs e)
        {
            ds.DataSetName = "ssprocess";
            ds.Tables.Clear();
            // Tables
            CreateDataTable("zipdata");
            CreateDataTable("invtrans");
            CreateDataTable("atmmast");
            CreateDataTable("atmdrop");
            CreateDataTable("atmservice");
            CreateDataTable("atmjournal");
            CreateDataTable("dropschedule");
            CreateDataTable("coindrop");
            CreateDataTable("acstdt01");
            CreateDataTable("bank");
            CreateDataTable("emailbank");
            CreateDataTable("storemoneycenter");
            CreateDataTable("storeemailaddress");
            CreateDataTable("compmoneycenter");
            CreateDataTable("rateschedule");
            CreateDataTable("moneycenter");
            CreateDataTable("bagmandt");
            CreateDataTable("bagmanhd");
            CreateDataTable("driver");
            CreateDataTable("store");
            CreateDataTable("safemast");
            CreateDataTable("validator");
            CreateDataTable("company");
            CreateDataTable("hhpickup");
            CreateDataTable("balance");
            CreateDataTable("depdetail");
            CreateDataTable("companyemailaddress");
            CreateDataTable("safemast");
            CreateDataTable("companyuser");
            CreateDataTable("smartsafetrans");
            CreateDataTable("salesperson");
            CreateDataTable("scheduleday");
            CreateDataTable("slips");
            CreateDataTable("recurringcharge");
            CreateDataTable("recurringcoin");
            CreateDataTable("chargetype");
            CreateDataTable("storebilladdress");
            CreateDataTable("schedulenote");
            CreateDataTable("currencydrop");
            CreateDataTable("chargetype");
            CreateDataTable("specialcharge");
            CreateDataTable("safedeposit");
            CreateDataTable("billing");
            CreateDataTable("arinvoice");
            CreateDataTable("morphistaxdata");
            CreateDataTable("morphisinvoice");
            CreateDataTable("sysdata");
            // Views
            CreateDataTable("view_ATMDropInvStatus");
            CreateDataTable("view_DepositedCRV");
            CreateDataTable("view_ATMDropInvtransDenom");
            CreateDataTable("view_ActiveATMDrop");
            CreateDataTable("view_ExpandedATMJournal");
            CreateDataTable("view_ATMJournalHistory");
            CreateDataTable("view_ATMDropNet");
            CreateDataTable("view_ATMDropNetWithStore");
            CreateDataTable("view_ExpandedATMDrop");
            CreateDataTable("view_unreturnedcoindrops");
            CreateDataTable("view_ATMDropWithStoreAndDriver");
            CreateDataTable("view_DenominatedInvtrans");
            CreateDataTable("view_UnimportedATMJournal");
            CreateDataTable("view_DenominatedBalance");
            CreateDataTable("view_drivercoinsummary");
            CreateDataTable("view_expandedcoindrop");
            CreateDataTable("view_expandedhhpickup");
            CreateDataTable("view_coinroute");
            CreateDataTable("view_driverdata");
            CreateDataTable("view_expandeddriver");
            CreateDataTable("view_mathinvtrans");
            CreateDataTable("view_proofsheet");
            CreateDataTable("view_safekeepingproofsheet");
            CreateDataTable("view_expandedbagmandt");
            CreateDataTable("view_netdeclared");
            CreateDataTable("view_grossverified");
            CreateDataTable("view_netverified");
            CreateDataTable("view_customernetchange");
            CreateDataTable("view_expandedbagmanhd");
            CreateDataTable("view_manifestbagsearch");
            CreateDataTable("view_expandedcompmoneycenter");
            CreateDataTable("view_expandedstoremoneycenter");
            CreateDataTable("view_expandedstore");
            CreateDataTable("view_SignatureBillSummary");
            CreateDataTable("view_customersearch");
            CreateDataTable("view_expandedsafemast");
            CreateDataTable("view_expandedsmartsafetrans");
            CreateDataTable("view_smartsafeverified");
            CreateDataTable("view_smartsafeactivityanalysis");
            CreateDataTable("view_totalinvtrans");
            CreateDataTable("view_inventoriedcoindrop");
            CreateDataTable("view_expandeddropschedule");
            CreateDataTable("view_expandeddropschedule");
            CreateDataTable("view_expandedslip");
            CreateDataTable("view_expandedrecurringcoin");
            CreateDataTable("view_SignatureBillSummary");
            CreateDataTable("view_ExpandedBilling");
            CreateDataTable("view_ExpandedBillingStoreSummary");
            CreateDataTable("view_BillingStoreBillTypeSummary");
            CreateDataTable("view_storebillingsummary");
            CreateDataTable("view_accountlookup");
            CreateDataTable("view_storeservicedays");
            CreateDataTable("view_storeservicedaysbrief");
            CreateDataTable("view_scheduledays");
            CreateDataTable("view_tmobilebilling");
            CreateDataTable("view_billingchargecount");
            CreateDataTable("view_expandedschedulenote");
            CreateDataTable("view_distinctstoreslipday");
            CreateDataTable("view_expandedcurrencydrop");
            CreateDataTable("view_pickupactivity");
            CreateDataTable("view_coinorderreporting");
            CreateDataTable("view_inventoriedsmartsafetrans");
            CreateDataTable("view_familydollarbillingsummary");
            CreateDataTable("view_expandedspecialcharge");
            CreateDataTable("view_financialoperationanalyis");
            CreateDataTable("view_smartsafedeclaredselector");
            CreateDataTable("view_smartsafedeposits");
            CreateDataTable("view_storebillingdata");
            CreateDataTable("view_invoicestoretax");
            CreateDataTable("view_vsinvoice");
            CreateDataTable("view_revenuesummary");
            CreateDataTable("view_billsumm");
            CreateDataTable("view_taxsummary");
            string selectstring = "SELECT top 1 storecode, store_name, space(35) AS company, SPACE(10) AS inv_number, SPACE(5) AS servicecode, SPACE(10) AS customerdda, GETDATE() AS inv_date, 10.00 AS rate_base, 10.00 AS charged, SPACE(35) AS  bill_type, 10.00 AS  num_drops, 10.00 storetax FROM  store ORDER BY 1";
            CreateVirtualDatatable(selectstring, "view_billingdata");
            selectstring = "SELECT TOP 1 inv_number, bill_store, atmcomp, bill_group FROM billing ORDER BY 1";
            CreateVirtualDatatable(selectstring, "view_billinginvnumber");
            selectstring = "select top 1 * from view_vsstoreinvoice order by lineid ";
            CreateVirtualDatatable(selectstring, "view_vsstoreinvoice");
            selectstring = "SELECT idcol, storecode, inv_number from billing";
            CreateVirtualDatatable(selectstring, "view_firstbillingstoreid");
            selectstring = "SELECT TOP 1 inv_number, inv_date, bill_store, charged as pretax, storetax as tax FROM billing ORDER BY 1";
            CreateVirtualDatatable(selectstring, "view_distinctbillinginvoice");
       

            CreateSchema("ssprocess.xsd");


        }

        private void buttonCreateSlipsReportDataset_Click(object sender, EventArgs e)
        {
            ds.DataSetName = "slipsrptds";
            // Create the slips
            DataTable dt = new DataTable();
            dt.TableName = "view_expandedslipsdata";
            // Driver Number
            dt.Columns.Add("driver_1", typeof(string));
            // Slip Day
            dt.Columns.Add("slip_day", typeof(string));
            // Driver Name
            dt.Columns.Add("drivername", typeof(string));
            // Slip Date
            dt.Columns.Add("slip_date", typeof(DateTime));
            // Store Name
            dt.Columns.Add("store_name", typeof(string));
            // PU_Address
            dt.Columns.Add("pu_address", typeof(string));
            // Phone
            dt.Columns.Add("phone", typeof(string));
            // Deliver Name (Bank)
            dt.Columns.Add("d_name", typeof(string));
            // Delivery Address
            dt.Columns.Add("d_address", typeof(string));
            // Store Code
            dt.Columns.Add("storecode", typeof(string));
            // Slip Number
            dt.Columns.Add("slipno", typeof(Int32));
            ds.Tables.Add(dt);


            DataTable dt2 = new DataTable();
            dt2.TableName = "view_doublelipsdata";
            // Driver Number
            dt2.Columns.Add("driver_1_1", typeof(string));
            // Slip Day
            dt2.Columns.Add("slip_day_1", typeof(string));
            // Driver Name
            dt2.Columns.Add("drivername_1", typeof(string));
            // Slip Date
            dt2.Columns.Add("slip_date_1", typeof(DateTime));
            // Store Name
            dt2.Columns.Add("store_name_1", typeof(string));
            // PU_Address
            dt2.Columns.Add("pu_address_1", typeof(string));
            // Phone
            dt2.Columns.Add("phone_1", typeof(string));
            // Deliver Name (Bank)
            dt2.Columns.Add("d_name_1", typeof(string));
            // Delivery Address
            dt2.Columns.Add("d_address_1", typeof(string));
            // Store Code
            dt2.Columns.Add("storecode_1", typeof(string));
            // Slip Number
            dt2.Columns.Add("slipno_1", typeof(Int32));
            // Driver Number
            dt2.Columns.Add("driver_1_2", typeof(string));
            // Slip Day
            dt2.Columns.Add("slip_day_2", typeof(string));
            // Driver Name
            dt2.Columns.Add("drivername_2", typeof(string));
            // Slip Date
            dt2.Columns.Add("slip_date_2", typeof(DateTime));
            // Store Name
            dt2.Columns.Add("store_name_2", typeof(string));
            // PU_Address
            dt2.Columns.Add("pu_address_2", typeof(string));
            // Phone
            dt2.Columns.Add("phone_2", typeof(string));
            // Deliver Name (Bank)
            dt2.Columns.Add("d_name_2", typeof(string));
            // Delivery Address
            dt2.Columns.Add("d_address_2", typeof(string));
            // Store Code
            dt2.Columns.Add("storecode_2", typeof(string));
            // Slip Number
            dt2.Columns.Add("slipno_2", typeof(Int32));
            ds.Tables.Add(dt2);




            CreateVFPDataTable("stores01", "acstore", vfpacdataconn);
            CreateVFPDataTable("driver01", "acdriver", vfpacdataconn);
            CreateVFPDataTable("slips01", "acslips", vfpacdataconn);
            CreateSchema("slipsrprtds.xsd");
        }


        private void CreateVirtualDatatable(string selectstring, string tablename)
        {

            try
            {
                // set up the Sql Command and DataAdapter with the Stored Proc name
                SqlCommand sc = new SqlCommand(selectstring, new SqlConnection(connectionString));
                sc.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(sc);
                da.TableMappings.Clear();
                da.TableMappings.Add("Table", tablename);
                // and fill the DataSet
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\r\n" + ex.InnerException.Message;
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }

        private void CreatePartialDatatable(string selectstring, string tablename)
        {

            try
            {
                // set up the Sql Command and DataAdapter with the Stored Proc name
                SqlCommand sc = new SqlCommand(selectstring + tablename, new SqlConnection(connectionString));
                sc.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(sc);
                da.TableMappings.Clear();
                da.TableMappings.Add("Table", tablename);
                // and fill the DataSet
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\r\n" + ex.InnerException.Message;
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }
        private void buttonMySQLDataset_Click(object sender, EventArgs e)
        {
            string selectstring = "";
            ds.DataSetName = "mysqldata";
            ds.Tables.Clear();
            CreateMySQLDatatable("store");
            CreateMySQLDatatable("driver");
            CreateMySQLDatatable("hhpickup");
            CreateMySQLDatatable("coindrop");
            CreateMySQLDatatable("currencydrop");
            CreateMySQLDatatable("region");
            CreateMySQLDatatable("safemast");
            CreateMySQLDatatable("validator");
            CreateMySQLDatatable("storeregion");
            CreateMySQLDatatable("dropschedule");
            CreateMySQLDatatable("moneycenter");
            CreateMySQLDatatable("compmoneycenter");
            CreateMySQLDatatable("storemoneycenter");
            CreateMySQLDatatable("schedulenote");
            CreateMySQLDatatable("atmmast");
            CreateMySQLDatatable("safetrans");
            CreateMySQLDatatable("atmdrop");
            CreateMySQLDatatable("atmjournal");
            CreateVirtualMySQLDatatable("expandedstore", " select *, concat(storecode, ' - ', f_address , ' - ', f_city) as location from store");
            CreateMySQLDatatable("user");
            CreateSchema("mysqldata.xsd");
        }


        private void CreateVirtualMySQLDatatable(string tablename, string selectstring)
        {

            try
            {
                // set up the Sql Command and DataAdapter with the Stored Proc name
                MySqlCommand sc = new MySqlCommand(selectstring + " LIMIT 1 ", new MySqlConnection(mysqlconnectionString));
                sc.CommandType = CommandType.Text;
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                da.TableMappings.Clear();
                da.TableMappings.Add("Table", tablename);
                // and fill the DataSet
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\r\n" + ex.InnerException.Message;
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }
        private void CreateMySQLDatatable(string tablename)
        {

            try
            {
                // set up the Sql Command and DataAdapter with the Stored Proc name
                MySqlCommand sc = new MySqlCommand("SELECT * FROM " + tablename + " LIMIT 1 ", new MySqlConnection(mysqlconnectionString));
                sc.CommandType = CommandType.Text;
                MySqlDataAdapter da = new MySqlDataAdapter(sc);
                da.TableMappings.Clear();
                da.TableMappings.Add("Table", tablename);
                // and fill the DataSet
                da.Fill(ds);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (ex.InnerException != null)
                    msg += "\r\n" + ex.InnerException.Message;
                MessageBox.Show("The following error occurred: " + ex.Message);
            }
        }
        private void buttonCreateReportDataset_Click(object sender, EventArgs e)
        {
            ds.DataSetName = "reportsds";
            ds.Tables.Clear();
            // Views
            CreateDataTable("view_expandedcompmoneycenter");
            CreateDataTable("view_expandedstoremoneycenter");
            CreateDataTable("view_expandedhhpickup");
            CreateSchema("reportsds.xsd");

        }

    }
}
