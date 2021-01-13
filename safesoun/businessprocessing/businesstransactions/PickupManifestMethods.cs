using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using BusinessTransactions;
using System.Windows.Forms;
using System.Configuration;

using System.IO;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using WSGUtilitieslib;
using Microsoft.Reporting.WinForms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BusinessTransactions
{
    public class PickupManifestMethods : WSGDataAccess
    {
        BindingSource bindingSourceManifestDetails = new BindingSource();
        BindingSource bindingSourceSearchResults = new BindingSource();
        BindingSource bindingSourceCustomerInquiry = new BindingSource();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Pickup Manifest");
        AppUtilities appUtilities = new AppUtilities();
        FrmLoading frmLoading = new FrmLoading();
        string CommandString = "";
        string manifestdriver = "";
        public Form menuform { get; set; }
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        MySQLMethods mySQLMethods = new MySQLMethods();
        ssprocess ssprocessds = new ssprocess();
        ssprocess currentitemds = new ssprocess();
        ssprocess searchds = new ssprocess();
        ssprocess currentmanifestds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        DateTime? SearchDate { get; set; }
        BusinessReports.BusinessReportsMethods brMethods = new BusinessReports.BusinessReportsMethods();
        ssprocess ssprocesssearchds = new ssprocess();
        public PickupManifestMethods()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.bagmandt.idcolColumn);
            SetIdcol(ssprocessds.bagmanhd.idcolColumn);
            SetIdcol(ssprocessds.hhpickup.idcolColumn);
            SetIdcol(ssprocessds.schedulenote.idcolColumn);
            SetIdcol(ssprocessds.currencydrop.idcolColumn);
        }

        public void StartPickupManifest()
        {
            manifestdriver = "";
            string driver = commonAppDataMethods.SelectDriver();
            if (driver.TrimEnd() != "")
            {
                if (wsgUtilities.wsgReply("Do you want to create driver manifests?"))
                {
                    CommandString = "SELECT * FROM driver WHERE number  = @driver";
                    ClearParameters();
                    AddParms("@driver", driver, "SQL");
                    FillData(ssprocessds, "driver", CommandString, CommandType.Text);
                    if (ssprocessds.driver[0].drivermanifest)
                    {
                        manifestdriver = driver;
                    }
                    else
                    {
                        manifestdriver = "";
                    }
                    frmLoading.Show();
                    // Import hhpickup rows
                    mySQLMethods.FillPickups(driver);
                    ssprocessds.hhpickup.Rows.Clear();
                    ssprocessds.hhpickup.Rows.Add();
                    for (int r = 0; r <= mySQLMethods.mysqldatads.hhpickup.Rows.Count - 1; r++)
                    {

                        EstablishBlankDataTableRow(ssprocessds.hhpickup);
                        ssprocessds.hhpickup[0].storecode = mySQLMethods.mysqldatads.hhpickup[r].storecode;
                        ssprocessds.hhpickup[0].sealnumber = mySQLMethods.mysqldatads.hhpickup[r].sealnumber;
                        ssprocessds.hhpickup[0].amount = mySQLMethods.mysqldatads.hhpickup[r].amount;
                        ssprocessds.hhpickup[0].driver = mySQLMethods.mysqldatads.hhpickup[r].driver;
                        ssprocessds.hhpickup[0].pickupdate = mySQLMethods.mysqldatads.hhpickup[r].pickupdate;
                        ssprocessds.hhpickup[0].pickuptime = mySQLMethods.mysqldatads.hhpickup[r].pickuptime;
                        ssprocessds.hhpickup[0].pickupid = mySQLMethods.mysqldatads.hhpickup[r].idcol;
                        ssprocessds.hhpickup[0].dtsdepositid = mySQLMethods.mysqldatads.hhpickup[r].dtsdepositid;
                        ssprocessds.hhpickup[0].postingdate = DateTime.Now.AddDays(550);
                        GenerateAppTableRowSave(ssprocessds.hhpickup[0]);
                        mySQLMethods.UpdateHHpickup(mySQLMethods.mysqldatads.hhpickup[r].idcol);
                    }

                   mySQLMethods.FillScheduleNote(driver);
                    ssprocessds.schedulenote.Rows.Clear();
                    for (int r = 0; r <= mySQLMethods.mysqldatads.schedulenote.Rows.Count - 1; r++)
                    {
                
                        EstablishBlankDataTableRow(ssprocessds.schedulenote);
                        ssprocessds.schedulenote[0].storecode = mySQLMethods.mysqldatads.schedulenote[r].storecode;
                        ssprocessds.schedulenote[0].driver = mySQLMethods.mysqldatads.schedulenote[r].driver;
                        ssprocessds.schedulenote[0].notes = mySQLMethods.mysqldatads.schedulenote[r].notes;
                        ssprocessds.schedulenote[0].slipdate = mySQLMethods.mysqldatads.schedulenote[r].slipdate.Date;
                        ssprocessds.schedulenote[0].actioncompleted = mySQLMethods.mysqldatads.schedulenote[r].actioncompleted;
                        GenerateAppTableRowSave(ssprocessds.schedulenote[0]);
                        mySQLMethods.UpdateSchedulenote(mySQLMethods.mysqldatads.schedulenote[r].idcol);
                    }
                    mySQLMethods.FillCurrencyDrops(driver);
                    ssprocessds.currencydrop.Rows.Clear();
                    for (int r = 0; r <= mySQLMethods.mysqldatads.currencydrop.Rows.Count - 1; r++)
                    {
                
                        EstablishBlankDataTableRow(ssprocessds.currencydrop);
                        ssprocessds.currencydrop[0].storecode = mySQLMethods.mysqldatads.currencydrop[r].storecode;
                        ssprocessds.currencydrop[0].driver = mySQLMethods.mysqldatads.currencydrop[r].driver;
                        ssprocessds.currencydrop[0].notes = mySQLMethods.mysqldatads.currencydrop[r].notes;
                        ssprocessds.currencydrop[0].amount = mySQLMethods.mysqldatads.currencydrop[r].amount;
                        ssprocessds.currencydrop[0].deliverydate = mySQLMethods.mysqldatads.currencydrop[r].deliverydate.Date;
                        GenerateAppTableRowSave(ssprocessds.currencydrop[0]);
                        mySQLMethods.UpdateCurrencyDrop(mySQLMethods.mysqldatads.currencydrop[r].idcol);
                    }
 

                    ssprocessds.view_expandedhhpickup.Rows.Clear();
                    CommandString = "SELECT * FROM view_expandedhhpickup  WHERE manid = 0 AND driver = @driver ORDER BY pickupid";
                    ClearParameters();
                    AddParms("@driver", driver, "SQL");
                    FillData(ssprocessds, "view_expandedhhpickup", CommandString, CommandType.Text);
                    for (int i = 0; i <= ssprocessds.view_expandedhhpickup.Rows.Count - 1; i++)
                    {
                        // Eliminate bags not assigned to money centers
                        if (ssprocessds.view_expandedhhpickup[i].moneycenterid == 0)
                        {
                            continue;
                        }
                        EstablishBlankDataTableRow(ssprocessds.bagmandt);
                        // Find the open manifest for this money center, if any
                        searchds.bagmanhd.Rows.Clear();
                        CommandString = "SELECT * FROM bagmanhd WHERE moneycenterid = @moneycenterid AND driver = @driver AND RTRIM(sealnumber) = 'Auto'";
                        ClearParameters();
                        AddParms("@moneycenterid", ssprocessds.view_expandedhhpickup[i].moneycenterid, "SQL");
                        AddParms("@driver", manifestdriver, "SQL");
                        FillData(searchds, "bagmanhd", CommandString, CommandType.Text);
                        if (searchds.bagmanhd.Rows.Count < 1)
                        {
                            ssprocessds.bagmandt[0].manifest = CreateBagmanhd(ssprocessds.view_expandedhhpickup[i].moneycenterid);
                        }
                        else
                        {
                            ssprocessds.bagmandt[0].manifest = searchds.bagmanhd[0].idcol;
                        }
                        ssprocessds.bagmandt[0].customer = ssprocessds.view_expandedhhpickup[i].companyname;
                        ssprocessds.bagmandt[0].sealnumber = ssprocessds.view_expandedhhpickup[i].sealnumber;
                        ssprocessds.bagmandt[0].amount = ssprocessds.view_expandedhhpickup[i].amount;
                        ssprocessds.bagmandt[0].comp_code = "Mix";
                        GenerateAppTableRowSave(ssprocessds.bagmandt[0]);
                        ClearParameters();
                        AddParms("@manid", ssprocessds.bagmandt[0].manifest, "SQL");

                        AddParms("@idcol", ssprocessds.view_expandedhhpickup[i].idcol, "SQL");
                        CommandString = "UPDATE  hhpickup SET manid = @manid  WHERE idcol = @idcol";
                        ExecuteCommand(CommandString, CommandType.Text);
                    }
                    frmLoading.Close();
                    wsgUtilities.wsgNotice("Manifest Generation Complete");

                    if (manifestdriver.TrimEnd() != "")
                    {
                        if (wsgUtilities.wsgReply("Do you want to print today's manifests?"))
                        {
                            string sealnumber = "";
                            searchds.bagmanhd.Rows.Clear();
                            CommandString = "SELECT * FROM bagmanhd WHERE driver = @driver AND  CONVERT(date, adddate) = @mandate";
                            ClearParameters();
                            AddParms("@driver", manifestdriver, "SQL");
                            AddParms("@mandate", DateTime.Now.Date, "SQL");
                            FillData(searchds, "bagmanhd", CommandString, CommandType.Text);
                            if (searchds.bagmanhd.Rows.Count > 0)
                            {
                                for (int i = 0; i <= searchds.bagmanhd.Rows.Count - 1; i++)
                                {

                                    // Capture seal number
                                    FrmEnterSealnumber frmEnterSealnumber = new FrmEnterSealnumber();
                                    frmEnterSealnumber.textBoxSealnumber.Text = searchds.bagmanhd[i].sealnumber.PadLeft(25);
                                    frmEnterSealnumber.labelMoneyCenterName.Text = commonAppDataMethods.GetMoneyCenterName(searchds.bagmanhd[i].moneycenterid);
                                    frmEnterSealnumber.ShowDialog();
                                    sealnumber = frmEnterSealnumber.textBoxSealnumber.Text;
                                    //Update manifest with seal number
                                    CommandString = "UPDATE bagmanhd  SET sealnumber = @sealnumber WHERE idcol = @idcol";
                                    ClearParameters();
                                    AddParms("@sealnumber", sealnumber, "SQL");
                                    AddParms("@idcol", searchds.bagmanhd[i].idcol, "SQL");
                                    ExecuteCommand(CommandString, CommandType.Text);
                                    brMethods.PrintVSManifest(searchds.bagmanhd[i].idcol, "V");
                             
                                }
                                wsgUtilities.wsgNotice("Printing complete");
                            }
                            else
                            {
                                wsgUtilities.wsgNotice("There are no manifests to print");
                            }
                        }
                        else
                        {
                            wsgUtilities.wsgNotice("Manifest Printing Cancelled");
                        }
                    }


                }
                else
                {
                    wsgUtilities.wsgNotice("Operation Cancelled");
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }

        public int CreateBagmanhd(int moneycenterid)
        {
            int manifestid = 0;
            ClearParameters();
            AddParms("@comp_code", "Mix", "SQL");
            AddParms("@sealnumber", "Auto", "SQL");
            AddParms("@moneycenterid", moneycenterid, "SQL");
            AddParms("@postingdate", DateTime.Now.Date, "SQL");
            AddParms("@userid", AppUserClass.AppUserId, "SQL");
            AddParms("@driver", manifestdriver, "SQL");
            AddIntOutputParm("@manifestid");
            manifestid = ExecuteIntOutputCommand("wsgsp_Insert_bagmanhd", CommandType.StoredProcedure);
            // Reload bagmanhd
            currentmanifestds.bagmanhd.Rows.Clear();
            AddParms("@idcol", manifestid, "SQL");
            return manifestid;
        }

    }

    public class MySQLMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Event");
        AppUtilities appUtilities = new AppUtilities();
        public mysqldata mysqldatads = new mysqldata();
        string commandstring = "";
        public MySQLMethods()
            : base("MySQL", "MySQLConnStringWithDB")
        {

        }



        public void ClearTable(string tablename)
        {
            string commandstring = "DELETE FROM " + tablename;
            ExecuteCommand(commandstring, CommandType.Text);
        }



        public void UpdateHHpickup(int idcol)
        {
            ClearParameters();
            AddParms("@idcol", idcol, "MySQL");

            commandstring = " UPDATE hhpickup SET  `imported` = 'Y'  WHERE idcol = @idcol";
            try
            {
                ExecuteCommand(commandstring, CommandType.Text);
            }

            catch (Exception e)
            {
                appUtilities.logEvent("Synch to MySQL failure", "01", e.Message, true);

            }
        }

        public void UpdateCurrencyDrop(int idcol)
        {
            ClearParameters();
            AddParms("@idcol", idcol, "MySQL");

            commandstring = " UPDATE currencydrop SET  `imported` = 'Y'  WHERE idcol = @idcol";
            try
            {
                ExecuteCommand(commandstring, CommandType.Text);
            }

            catch (Exception e)
            {
                appUtilities.logEvent("Synch to MySQL failure", "01", e.Message, true);

            }
        }


        public void UpdateSchedulenote(int idcol)
        {
            ClearParameters();
            AddParms("@idcol", idcol, "MySQL");

            commandstring = " UPDATE schedulenote SET  `imported` = 'Y'  WHERE idcol = @idcol";
            try
            {
                ExecuteCommand(commandstring, CommandType.Text);
            }

            catch (Exception e)
            {
                appUtilities.logEvent("Synch to MySQL failure", "01", e.Message, true);

            }
        }





        public void UpdateMySQLDatatable(DataTable Sourcetable, string Idname, bool Inserting)
        {
            if (ConfigurationManager.AppSettings["UpdateMySQL"] == "Y")
            {
                for (int r = 0; r <= Sourcetable.Rows.Count - 1; r++)
                {
                    GenerateAppTableRowSaveGeneral(Sourcetable.Rows[r], Idname, Inserting);
                }
            }
        }

        public void FillScheduleNote(string driver)
        {
            mysqldatads.schedulenote.Rows.Clear();
            string commandstring = "select  storecode,  driver,  notes, idcol, date_add(slipdate,  INTERVAL 1 minute) AS slipdate, actioncompleted from schedulenote  WHERE driver = @driver and imported <> 'Y'";
            ClearParameters();
            ClearParameters();
            AddParms("@driver", driver, "MySQL");
            FillData(mysqldatads, "schedulenote", commandstring, CommandType.Text);
        }


        public void FillCurrencyDrops(string driver)
        {
            mysqldatads.currencydrop.Rows.Clear();
            string commandstring = "select storecode, driver, amount, notes, date_add(deliverydate,  INTERVAL 1 minute) AS deliverydate, idcol FROM  currencydrop WHERE driver = @driver and imported <> 'Y' AND RTRIM(lckuser) <> 'DTS' ";
            ClearParameters();
            AddParms("@driver", driver, "MySQL");
            FillData(mysqldatads, "currencydrop", commandstring, CommandType.Text);
        }
        public void FillPickups(string driver)
        {

            mysqldatads.hhpickup.Rows.Clear();
            string commandstring = "select storecode, sealnumber, amount, driver,  date_add(pickupdate,  INTERVAL 1 minute) AS pickupdate, pickuptime, dtsdepositid, idcol from  hhpickup WHERE driver = @driver and  pickupstatus = 'Y' AND imported <> 'Y'";
            ClearParameters();
            AddParms("@driver", driver, "MySQL");
            FillData(mysqldatads, "hhpickup", commandstring, CommandType.Text);
        }
    }

}