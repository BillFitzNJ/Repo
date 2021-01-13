using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Data;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CommonAppClasses
{

    public class MySQLSynchMethods : WSGDataAccess
    {
        #region Declarations
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Synch Processing");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public Form parentForm = new Form();
        public Form menuform { get; set; }
        DataRow[] foundRows;
        public string commandtext { get; set; }
        // Custom Controls
        ssprocess openprocessds = new ssprocess();
        ssprocess tempprocessds = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        ssprocess closeprocessds = new ssprocess();
        mysqldata mysqldatads = new mysqldata();
        ssprocess ssprocessds = new ssprocess();
        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        #endregion
        public MySQLSynchMethods()
            : base("SQL", "SQLConnString")
        {

        }
        public void StartApp()
        {

            string commandstring = "";

            if (ConfigurationManager.AppSettings["SynchDropSchedule"] == "True")
            {
                updatemysqldropschedule("");

            }
            if (ConfigurationManager.AppSettings["SynchStore"] == "True")
            {
                updatemysqlstore("");

         }
            if (ConfigurationManager.AppSettings["SynchDriver"] == "True")
            {
                //Driver
                mySQLDataMethods.ClearTable("driver");
                mysqldatads.driver.Rows.Clear();
                commandstring = "select  number,  firstname, initial, lastname, address, emaddr, city, state, zip, bulkcoin, idcol from driver";
                ClearParameters();
                FillData(mysqldatads, "driver", commandstring, CommandType.Text);
                mySQLDataMethods.UpdateMySQLDatatable(mysqldatads.driver, "idcol", true);
            }
            if (ConfigurationManager.AppSettings["SynchMoneyCenterTables"] == "True")
            {
                //Money Center
                   mySQLDataMethods.ClearTable("moneycenter");
                    ssprocessds.moneycenter.Rows.Clear();
                    commandstring = " select * from moneycenter";
                    ClearParameters();
                    FillData(ssprocessds, "moneycenter", commandstring, CommandType.Text);

                    for (int r = 0; r <= ssprocessds.moneycenter.Rows.Count - 1; r++)
                    {
                        mySQLDataMethods.InsertMoneyCenter(ssprocessds.moneycenter[r].centername, ssprocessds.moneycenter[r].idcol);
                    }
                
                //Store Money Center
                mySQLDataMethods.ClearTable("storemoneycenter");
                ssprocessds.storemoneycenter.Rows.Clear();
                commandstring = " select * from storemoneycenter";
                FillData(ssprocessds, "storemoneycenter", commandstring, CommandType.Text);
                mySQLDataMethods.UpdateMySQLDatatable(ssprocessds.storemoneycenter, "idcol", true);

                //Company Money Center
                mySQLDataMethods.ClearTable("compmoneycenter");
                ssprocessds.compmoneycenter.Rows.Clear();
                commandstring = " select * from compmoneycenter";
                FillData(ssprocessds, "compmoneycenter", commandstring, CommandType.Text);
                mySQLDataMethods.UpdateMySQLDatatable(ssprocessds.compmoneycenter, "idcol", true);

            }

            if (ConfigurationManager.AppSettings["SynchATMMast"] == "True")
            { 
                ssprocessds.atmmast.Rows.Clear();
                commandstring = "select *  from atmmast where atmid in (select atmid from atmdrop where dropdate > getdate() - 300)";
                FillData(ssprocessds, "atmmast", commandstring, CommandType.Text);
                mySQLDataMethods.UpdateMySQLDatatable(ssprocessds.atmmast, "idcol", true);
                
              
            }

            if (ConfigurationManager.AppSettings["SynchATMDrop"] == "True")
            {
           
                ssprocessds.atmdrop.Rows.Clear();
                commandstring = "select *  from atmdrop where dropdate >= getdate() - 10  and dropstatus <> 'V'";
                FillData(ssprocessds, "atmdrop", commandstring, CommandType.Text);
                for (int r = 0; r <= ssprocessds.atmdrop.Rows.Count - 1; r++)
                {
                    mySQLDataMethods.mysqlds.atmdrop.Rows.Clear();
                    mySQLDataMethods.mysqlds.atmdrop.ImportRow(ssprocessds.atmdrop[r]);
                    mySQLDataMethods.UpdateAtmdrop();
                }
            }
       
        }




        public void updatemysqlstore(string storecode)
        {
            // Store
            // Load source table
            mysqldatads.store.Rows.Clear();
            string commandstring = "SELECT storecode, store_name, f_address, f_city, f_state, f_zip, attention, pu_address, d_address, custno ,";
            commandstring += "d_code, d_name, d_address, phone, custno, SPACE(2) as PW FROM store WHERE active = 1 and  start_date <= GETDATE() and stop_date >= GETDATE() ";
            ClearParameters();
            if (storecode.TrimEnd() != "")
            {
                AddParms("@storecode", storecode, "SQL");
                commandstring += " AND storecode = @storecode";
            }

            FillData(mysqldatads, "store", commandstring, CommandType.Text);
            // Load target table
            mySQLDataMethods.FillStore(storecode);
            // Loop thru the rows in the source to see which rows in the target are missing.
            for (int r = 0; r <= mysqldatads.store.Rows.Count - 1; r++)
            {
                commandstring = "storecode = '" + mysqldatads.store[r].storecode.Substring(0, 11) + "'";
                foundRows = mySQLDataMethods.mysqlsearchds.store.Select(commandstring);
                if (foundRows.Length < 1)
                {
                    // Add the row to the target 
                    // The default password is PW

                    mySQLDataMethods.InsertStore(mysqldatads.store[r]);
                }
                else
                {
                    bool updatestore = false;
                    // Store row found. Check certain columns and update if needed.
                    tempprocessds.store.Rows.Clear();
                    tempprocessds.store.ImportRow(foundRows[0]);
                    if (mysqldatads.store[r].store_name.TrimEnd() != tempprocessds.store[0].store_name.TrimEnd())
                    {
                        updatestore = true;
                    }
                    if (mysqldatads.store[r].f_address.TrimEnd() != tempprocessds.store[0].f_address.TrimEnd())
                    {
                        updatestore = true;
                    }
                    if (mysqldatads.store[r].f_city.TrimEnd() != tempprocessds.store[0].f_city.TrimEnd())
                    {
                        updatestore = true;
                    }
                    if (mysqldatads.store[r].f_state.TrimEnd() != tempprocessds.store[0].f_state.TrimEnd())
                    {
                        updatestore = true;
                    }
                    if (mysqldatads.store[r].phone.TrimEnd() != tempprocessds.store[0].phone.TrimEnd())
                    {
                        updatestore = true;
                    }

                    if (updatestore)
                    {
                        mySQLDataMethods.UpdateStore(mysqldatads.store[r]);
                    }
                }
            }
        }

        public void updatemysqldropschedule(string storecode)
        {
            // Load source drops schedule
            mysqldatads.dropschedule.Rows.Clear();
            string commandstring = "select d.storecode, driver_1, d.daynumber, COALESCE(v.sequence,0) AS sequence, schday   from (SELECT ds.storecode, daynumber, driver_1, schday FRom dropschedule ds INNER join store st ON ds.storecode = st.storecode  where st.active = 1) d ";
            commandstring += " LEFT JOIN  view_driverstorepickuptime v ON d.storecode = v.storecode AND d.driver_1 = v.driver and v.daynumber = d.daynumber ";
            ClearParameters();
            if (storecode.TrimEnd() != "")
            {
                AddParms("@storecode", storecode, "SQL");
                commandstring += " where d.storecode = @storecode";
            }
            FillData(mysqldatads, "dropschedule", commandstring, CommandType.Text);

            // Load target drop schedule
            mySQLDataMethods.FillDropSchedule(storecode);
            // Loop thru the rows in the source to see which rows in the target are missing.
            for (int r = 0; r <= mysqldatads.dropschedule.Rows.Count - 1; r++)
            {
                commandstring = " storecode = '" + mysqldatads.dropschedule[r].storecode + "' AND driver_1 = '" + mysqldatads.dropschedule[r].driver_1 + "' AND daynumber =  " + mysqldatads.dropschedule[r].daynumber.ToString().TrimStart();
                DataRow[] foundDropSchedule = mySQLDataMethods.mysqlds.dropschedule.Select(commandstring);
                if (foundDropSchedule.Length < 1)
                {
                    // Add the row to the target 
                    mySQLDataMethods.InsertDropSchedule(mysqldatads.dropschedule[r].storecode, mysqldatads.dropschedule[r].driver_1, mysqldatads.dropschedule[r].daynumber,
                          mysqldatads.dropschedule[r].sequence, mysqldatads.dropschedule[r].schday);
                }
                else
                {

                    if (Convert.ToInt16(foundDropSchedule[0]["sequence"]) != mysqldatads.dropschedule[r].sequence)
                    {
                        // Update  the target 
                        mySQLDataMethods.UpdateDropSchedule(mysqldatads.dropschedule[r].storecode, mysqldatads.dropschedule[r].driver_1, mysqldatads.dropschedule[r].daynumber,
                           mysqldatads.dropschedule[r].sequence, mysqldatads.dropschedule[r].schday);
                    }
                }

            }
            // Loop thru the rows in the target to see which rows in the source are missing.
            for (int r = 0; r <= mySQLDataMethods.mysqlds.dropschedule.Rows.Count - 1; r++)
            {
                commandstring = "storecode = '" + mySQLDataMethods.mysqlds.dropschedule[r].storecode + "' AND driver_1 = '" + mySQLDataMethods.mysqlds.dropschedule[r].driver_1 + "' AND daynumber = '" + mySQLDataMethods.mysqlds.dropschedule[r].daynumber + "'";
                if (mysqldatads.dropschedule.Select(commandstring).Length < 1)
                {
                    // Delete the row from the target 
                    mySQLDataMethods.DeleteDropSchedule(mySQLDataMethods.mysqlds.dropschedule[r].storecode, mySQLDataMethods.mysqlds.dropschedule[r].driver_1, mySQLDataMethods.mysqlds.dropschedule[r].daynumber);
                }
            }

        }

    }
}




















