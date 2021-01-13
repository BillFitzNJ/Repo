using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.IO;
using System.ComponentModel;
using BusinessTransactions;
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

namespace BusinessTransactions
{

    public class SynchMethods : WSGDataAccess
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
        Icon AppIcon = new Icon("appicon.ico");
        ssprocess openprocessds = new ssprocess();
        ssprocess tempprocessds = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        ssprocess closeprocessds = new ssprocess();
        mysqldata mysqldatads = new mysqldata();
        ssprocess ssprocessds = new ssprocess();
        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        #endregion
        public SynchMethods()
            : base("SQL", "SQLConnString")
        {

        }
        public void StartApp()
        {
            if (wsgUtilities.wsgReply("Do you want to synchronize reference tables?"))
            {
                bool updatedropschedule = true;
                bool updatestore = true;
                bool updatedriveer = true;
                
                string commandstring = "";
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                if (updatedropschedule)
                {
                    // Load source drops schedule
                    mysqldatads.dropschedule.Rows.Clear();
                    commandstring = "select d.storecode, driver_1, d.daynumber, COALESCE(v.sequence,0) AS sequence, schday   from (SELECT ds.storecode, daynumber, driver_1, schday FRom dropschedule ds INNER join store st ON ds.storecode = st.storecode  where st.active = 1) d ";
                    commandstring += " LEFT JOIN  view_driverstorepickuptime v ON d.storecode = v.storecode AND d.driver_1 = v.driver and v.daynumber = d.daynumber ";
                    ClearParameters();
                    FillData(mysqldatads, "dropschedule", commandstring, CommandType.Text);

                    // Load target drop schedule
                    mySQLDataMethods.FillDropSchedule();
                    // Loop thru the rows in the source to see which rows in the target are missing.
                    for (int r = 0; r <= mysqldatads.dropschedule.Rows.Count - 1; r++)
                    {
                        commandstring = "storecode = '" + mysqldatads.dropschedule[r].storecode + "' AND driver_1 = '" + mysqldatads.dropschedule[r].driver_1 + "' AND daynumber =  " + mysqldatads.dropschedule[r].daynumber.ToString().TrimStart();
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
                if (updatestore)
                {
                    // Store
                    // Load source table
                    ssprocessds.store.Rows.Clear();
                    commandstring = "SELECT storecode, store_name, f_address, f_city, f_state, attention, pu_address,";
                    commandstring += "d_code, d_name, d_address, phone, custno, SPACE(2) as PW FROM store WHERE active = 1";
                    ClearParameters();
                    FillData(mysqldatads, "store", commandstring, CommandType.Text);
                    // Load target table
                    mySQLDataMethods.FillStore();
                    // Loop thru the rows in the source to see which rows in the target are missing.
                    for (int r = 0; r <= mysqldatads.store.Rows.Count - 1; r++)
                    {
                        commandstring = "storecode = '" + mysqldatads.store[r].storecode + "'";
                        foundRows = mySQLDataMethods.mysqlds.store.Select(commandstring);
                        if (foundRows.Length < 1)
                        {
                            // Add the row to the target 
                            // The default password is PW

                            mySQLDataMethods.InsertStore(mysqldatads.store[r]);
                        }
                        else
                        {
                            updatestore = false;
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
                if (updatedriveer)
                {
                    //Driver
                    mySQLDataMethods.ClearTable("driver");
                    mysqldatads.driver.Rows.Clear();
                    commandstring = "select  number,  firstname, initial, lastname, address, city, state, zip, bulkcoin, idcol from driver";
                    ClearParameters();
                    FillData(mysqldatads, "driver", commandstring, CommandType.Text);
                    mySQLDataMethods.UpdateMySQLDatatable(mysqldatads.driver, "idcol", true);
                    frmLoading.Close();
                    wsgUtilities.wsgNotice("Reference Data Synchronization Complete");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }

        }

    }

}