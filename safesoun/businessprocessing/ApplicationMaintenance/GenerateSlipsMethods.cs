using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Data;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ApplicationMaintenance
{
    public class GenerateSlipsMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Generate Slips");
        ssprocess ssprocessds = new ssprocess();
        ssprocess testprocessds = new ssprocess();
        ssprocess tempprocessds = new ssprocess();
        AppUtilities appUtilities = new AppUtilities();
        FrmGenerateSlips parentform = new FrmGenerateSlips();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        DateTime[,] slipdates = new DateTime[6, 2];
        public GenerateSlipsMethods()
            : base("SQL", "SQLConnString")
        {
            SetEvents();
            SetIdcol(ssprocessds.slips.idcolColumn);
        }

        public void StartApp()
        {
            //Populate date arrays
            DateTime WeekDate = DateTime.Now.Date;
            for (int i = 0; i <= 5; i++)
            {
                while (true)
                {
                    if ((int)WeekDate.DayOfWeek == 1)
                    {
                        slipdates[i, 0] = WeekDate;
                        WeekDate = WeekDate.AddDays(6);
                        slipdates[i, 1] = WeekDate;
                        WeekDate = WeekDate.AddDays(1);
                        break;
                    }
                    else
                    {
                        WeekDate = WeekDate.AddDays(1);
                    }
                }
            }
            parentform.radioButtonSlipdate1.Text = String.Format("{0:MM/dd/yyyy}", slipdates[0, 0]) + " - " + String.Format("{0:MM/dd/yyyy}", slipdates[0, 1]);
            parentform.radioButtonSlipdate2.Text = String.Format("{0:MM/dd/yyyy}", slipdates[1, 0]) + " - " + String.Format("{0:MM/dd/yyyy}", slipdates[1, 1]);
            parentform.radioButtonSlipdate3.Text = String.Format("{0:MM/dd/yyyy}", slipdates[2, 0]) + " - " + String.Format("{0:MM/dd/yyyy}", slipdates[2, 1]);
            parentform.radioButtonSlipdate4.Text = String.Format("{0:MM/dd/yyyy}", slipdates[3, 0]) + " - " + String.Format("{0:MM/dd/yyyy}", slipdates[3, 1]);
            parentform.radioButtonSlipdate5.Text = String.Format("{0:MM/dd/yyyy}", slipdates[4, 0]) + " - " + String.Format("{0:MM/dd/yyyy}", slipdates[4, 1]);
            parentform.radioButtonSlipdate6.Text = String.Format("{0:MM/dd/yyyy}", slipdates[5, 0]) + " - " + String.Format("{0:MM/dd/yyyy}", slipdates[5, 1]);
            parentform.ShowDialog();
        }

        public void SetEvents()
        {
            parentform.buttonProceed.Click += new System.EventHandler(GenerateSlips);
            parentform.buttonCancel.Click += new System.EventHandler(ProcessCancel);
            parentform.radioButtonSlipdate1.CheckedChanged += new System.EventHandler(radioButtonSlipdate1Changed);
            parentform.radioButtonSlipdate2.CheckedChanged += new System.EventHandler(radioButtonSlipdate2Changed);
            parentform.radioButtonSlipdate3.CheckedChanged += new System.EventHandler(radioButtonSlipdate3Changed);
            parentform.radioButtonSlipdate4.CheckedChanged += new System.EventHandler(radioButtonSlipdate4Changed);
            parentform.radioButtonSlipdate5.CheckedChanged += new System.EventHandler(radioButtonSlipdate5Changed);
            parentform.radioButtonSlipdate6.CheckedChanged += new System.EventHandler(radioButtonSlipdate6Changed);

        }


        public void radioButtonSlipdate1Changed(object sender, EventArgs e)
        {
            if (parentform.radioButtonSlipdate1.Checked)
            {
                parentform.dateTimePickerBeginDate.Value = slipdates[0, 0];
                parentform.dateTimePickerEndDate.Value = slipdates[0, 1];

            }

        }
        public void radioButtonSlipdate2Changed(object sender, EventArgs e)
        {
            if (parentform.radioButtonSlipdate2.Checked)
            {
                parentform.dateTimePickerBeginDate.Value = slipdates[1, 0];
                parentform.dateTimePickerEndDate.Value = slipdates[1, 1];

            }

        }
        public void radioButtonSlipdate3Changed(object sender, EventArgs e)
        {
            if (parentform.radioButtonSlipdate3.Checked)
            {
                parentform.dateTimePickerBeginDate.Value = slipdates[2, 0];
                parentform.dateTimePickerEndDate.Value = slipdates[2, 1];

            }

        }
        public void radioButtonSlipdate4Changed(object sender, EventArgs e)
        {
            if (parentform.radioButtonSlipdate4.Checked)
            {
                parentform.dateTimePickerBeginDate.Value = slipdates[3, 0];
                parentform.dateTimePickerEndDate.Value = slipdates[3, 1];

            }

        }
        public void radioButtonSlipdate5Changed(object sender, EventArgs e)
        {
            if (parentform.radioButtonSlipdate5.Checked)
            {
                parentform.dateTimePickerBeginDate.Value = slipdates[4, 0];
                parentform.dateTimePickerEndDate.Value = slipdates[4, 1];

            }

        }

        public void radioButtonSlipdate6Changed(object sender, EventArgs e)
        {
            if (parentform.radioButtonSlipdate6.Checked)
            {
                parentform.dateTimePickerBeginDate.Value = slipdates[5, 0];
                parentform.dateTimePickerEndDate.Value = slipdates[5, 1];

            }

        }


        public void ProcessCancel(object sender, EventArgs e)
        {
            parentform.Close();
        }
        public void GenerateSlips(object sender, EventArgs e)
        {
            DateTime validStopDate = Convert.ToDateTime("01/01/2005");
            bool GenerateSlips = true;
            // Create a work table and then insert into the database

            ClearParameters();
            ssprocessds.slips.Rows.Clear();
            DateTime SlipDate = new DateTime();
            string commandtext = "SELECT * from slips where slip_date between  @begindate and @enddate";
            AddParms("@begindate", parentform.dateTimePickerBeginDate.Value.Date, "SQL");
            AddParms("@enddate", parentform.dateTimePickerEndDate.Value.Date, "SQL");
            FillData(testprocessds, "slips", commandtext, CommandType.Text);
            if (testprocessds.slips.Rows.Count > 0)
            {
                GenerateSlips = wsgUtilities.wsgReply("There are slips in the file for the dates you selected. Do you want to proceed?");
            }
            else
            {
                GenerateSlips = wsgUtilities.wsgReply("Do you want to generate slips?");

            }
            if (GenerateSlips)
            {
                ClearParameters();
                commandtext = "SELECT * FROM view_expandeddropschedule WHERE active = 1 AND 1 = 1 ";

                if (parentform.textBoxDriver.Text.TrimEnd() != "")
                {
                    AddParms("@driver_1", parentform.textBoxDriver.Text.TrimEnd(), "SQL");
                    commandtext += " AND driver_1 = @driver_1 ";

                }

                if (parentform.textBoxCompanyCode.Text.TrimEnd() != "")
                {
                    if (parentform.textBoxStore.Text.TrimEnd() != "")
                    {
                        AddParms("@storecode", parentform.textBoxCompanyCode.Text.TrimEnd() + "-" + parentform.textBoxStore.Text.TrimEnd(), "SQL");
                        commandtext += " AND  storecode = @storecode ";
                    }
                    else
                    {
                        AddParms("@compcode", parentform.textBoxCompanyCode.Text.TrimEnd(), "SQL");
                        commandtext += " AND LEFT(storecode,4)  = @compcode ";
                    }
                }

                commandtext += " ORDER BY driver_1, daynumber";
                ssprocessds.view_expandeddropschedule.Rows.Clear();
                FillData(ssprocessds, "view_expandeddropschedule", commandtext, CommandType.Text);
                parentform.progressBarSlips.Maximum = ssprocessds.view_expandeddropschedule.Rows.Count;
                for (int i = 0; i <= ssprocessds.view_expandeddropschedule.Rows.Count - 1; i++)
                {
                    parentform.progressBarSlips.Value = i;
                    parentform.progressBarSlips.PerformStep();


                    SlipDate = parentform.dateTimePickerBeginDate.Value.Date;
                    while (SlipDate <= parentform.dateTimePickerEndDate.Value.Date)
                    {
                        bool CreateSlip = true;
                        // Skip slips for store which have not started or have stopped.
                        if (ssprocessds.view_expandeddropschedule[i].start_date.Date > SlipDate.Date)
                        {
                            CreateSlip = false;
                        }
                        if (ssprocessds.view_expandeddropschedule[i].stop_date <= SlipDate.Date)
                        {
                            CreateSlip = false;
                        }

                        if (CreateSlip)
                        {
                            if ((Convert.ToInt32(ssprocessds.view_expandeddropschedule[i].schday) != (int)SlipDate.DayOfWeek +1)  && (!IsCycle((int)SlipDate.AddDays(1).DayOfWeek, SlipDate.Month, SlipDate, ssprocessds.view_expandeddropschedule[i].schday)))
                            {
                                CreateSlip = false;
                            }
                        }

                        if (CreateSlip)
                        {
                            // If there is a slip for this date for this store, don't generate another one
                            testprocessds.slips.Rows.Clear();
                            commandtext = "SELECT * FROM slips where LEFT(storecode,11) = @storecode AND slip_date = @slip_date";
                            ClearParameters();
                            AddParms("@storecode", ssprocessds.view_expandeddropschedule[i].storecode.Substring(0, 11), "SQL");
                            AddParms("@slip_date", SlipDate, "SQL");

                            FillData(testprocessds, "slips", commandtext, CommandType.Text);
                            if (testprocessds.slips.Rows.Count > 0)
                            {
                                CreateSlip = false;
                            }
                        }



                        if (CreateSlip)
                        {
                            tempprocessds.slips.Rows.Clear();
                            EstablishBlankDataTableRow(tempprocessds.slips);
                            tempprocessds.slips[0].storecode = ssprocessds.view_expandeddropschedule[i].storecode;
                            tempprocessds.slips[0].slip_date = SlipDate;
                            tempprocessds.slips[0].commission = ssprocessds.view_expandeddropschedule[i].commission;
                            tempprocessds.slips[0].commtype = ssprocessds.view_expandeddropschedule[i].commtype;
                            tempprocessds.slips[0].route = ssprocessds.view_expandeddropschedule[i].route;
                            tempprocessds.slips[0].adjustment = ssprocessds.view_expandeddropschedule[i].adjustment;
                            tempprocessds.slips[0].num_drops = ssprocessds.view_expandeddropschedule[i].num_drops;
                            tempprocessds.slips[0].driver_1 = ssprocessds.view_expandeddropschedule[i].driver_1;
                            tempprocessds.slips[0].slip_day = ssprocessds.view_expandeddropschedule[i].dropday;
                            tempprocessds.slips[0].chgcode = "REGDRP";
                            tempprocessds.slips[0].trancode = "SL";
                            ssprocessds.slips.ImportRow(tempprocessds.slips[0]);
                        }
                        SlipDate = SlipDate.AddDays(1);
                    }

                }


                int slipcount = 1;

                for (int i = 0; i <= ssprocessds.slips.Rows.Count - 1; i++)
                {
                    ClearParameters();
                    AddParms("@storecode", ssprocessds.slips[i].storecode, "SQL");
                    AddParms("@slip_date", ssprocessds.slips[i].slip_date, "SQL");
                    AddParms("@commission", ssprocessds.slips[i].commission, "SQL");
                    AddParms("@commtype", ssprocessds.slips[i].commtype, "SQL");
                    AddParms("@adjustment", ssprocessds.slips[i].adjustment, "SQL");
                    AddParms("@num_drops", ssprocessds.slips[i].num_drops, "SQL");
                    AddParms("@slip_type", ssprocessds.slips[i].slip_type, "SQL");
                    AddParms("@driver_1", ssprocessds.slips[i].driver_1, "SQL");
                    AddParms("@slip_day", ssprocessds.slips[i].slip_day, "SQL");
                    AddParms("@trancode", ssprocessds.slips[i].trancode, "SQL");
                    AddParms("@chgcode", ssprocessds.slips[i].chgcode, "SQL");
                    AddParms("@adduser", AppUserClass.AppUserId, "SQL");
                    AddParms("@slipno", slipcount, "SQL");

                    ExecuteCommand("wsgsp_insertslip", CommandType.StoredProcedure);

                    slipcount++;
                }


                wsgUtilities.wsgNotice(ssprocessds.slips.Rows.Count.ToString().TrimEnd() + " Slips Generated");
                parentform.progressBarSlips.Value = 0;
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");

            }
        }

        public bool IsCycle(int SlipDayofWeek, int SlipMonth, DateTime SlipDate, string ScheduleDay)
        {
            bool NeedSlip = false;
            if (Convert.ToInt32(ScheduleDay) > 7)
            {
                switch (ScheduleDay)
                {

                    case "008":
                        {
                            //  First Sunday of the Month
                            if (SlipDayofWeek == 1 && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "009":
                        {
                            //  First Monday of the Month
                            if (SlipDayofWeek == 2 && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "010":
                        {
                            //  First Tuesday of the Month
                            if (SlipDayofWeek == 3 && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "011":
                        {
                            //  First Wednesday of the Month
                            if (SlipDayofWeek == 4 && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "012":
                        {
                            //  First Thursday of the Month
                            if (SlipDayofWeek == 5 && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "013":
                        {
                            //  First Friday of the Month
                            if (SlipDayofWeek == 6 && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "014":
                        {
                            //  First Saturday of the Month
                            if (SlipDayofWeek == 7 && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    case "033":
                        {
                            //   Every Other Sunday - beginning with the first Sunday, 1/6/02
                            if ((SlipDayofWeek == 1) && ((SlipDate -  Convert.ToDateTime("01/06/2002").Date).TotalDays%2 == 0) )
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {
                               
                                break;
                            }
                        }
                    case "015":
                        {
                            //   Every Other Monday- beginning with the first Monday, 1/7/02
                            if ((SlipDayofWeek == 2) && ((SlipDate - Convert.ToDateTime("01/07/2002").Date).TotalDays % 2 == 0))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "016":
                        {
                            //   Every Other Tuesday- beginning with the first Tuesday, 1/8/02
                            if ((SlipDayofWeek == 3) && ((SlipDate - Convert.ToDateTime("01/08/2002").Date).TotalDays % 2 == 0))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "017":
                        {
                            //   Every Other Wednesday- beginning with the first Wednesday, 1/9/02
                            if ((SlipDayofWeek == 4) && ((SlipDate - Convert.ToDateTime("01/09/2002").Date).TotalDays % 2 == 0))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "018":
                        {
                            //   Every Other Thursday- beginning with the first Thursday, 1/10/02
                            if ((SlipDayofWeek == 5) && ((SlipDate - Convert.ToDateTime("01/10/2002").Date).TotalDays % 2 == 0))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "019":
                        {
                            //   Every Other Friday- beginning with the first Friday, 1/11/02
                            if ((SlipDayofWeek == 6) && ((SlipDate - Convert.ToDateTime("01/11/2002").Date).TotalDays % 2 == 0))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "034":
                        {
                            //   Every Other Saturday- beginning with the first Saturday, 1/12/02
                            if ((SlipDayofWeek == 7) && ((SlipDate - Convert.ToDateTime("01/12/2002").Date).TotalDays % 2 == 0))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "020":
                        {
                            //   First Thursday of Every Odd Month
                            if ((SlipDayofWeek == 5) && (SlipDate.AddDays(-7).Month != SlipDate.Month) && (SlipDate.Month %2 != 0))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "021":
                        {
                            //  Second Sunday of the month
                            if ((SlipDate.Day > 7) && (SlipDayofWeek == 1) && (SlipDate.AddDays(-14).Month != SlipDate.Month) )
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "022":
                        {
                            //  Second Monday of the month
                            if ((SlipDate.Day > 7) && (SlipDayofWeek == 2) && (SlipDate.AddDays(-14).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "023":
                        {
                            //  Second Tuesday of the month
                            if ((SlipDate.Day > 7) && (SlipDayofWeek == 3) && (SlipDate.AddDays(-14).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "024":
                        {
                            //  Second Wednesday of the month
                            if ((SlipDate.Day > 7) && (SlipDayofWeek == 4) && (SlipDate.AddDays(-14).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "025":
                        {
                            //  Second Thursday of the month
                            if ((SlipDate.Day > 7) && (SlipDayofWeek == 5) && (SlipDate.AddDays(-14).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "026":
                        {
                            //  Second Friday of the month
                            if ((SlipDate.Day > 7) && (SlipDayofWeek == 6) && (SlipDate.AddDays(-14).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "027":
                        {
                            //  Second Saturday of the month
                            if ((SlipDate.Day > 7) && (SlipDayofWeek == 7) && (SlipDate.AddDays(-14).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }


                    case "028":
                        {
                            //  Last Monday of the month
                            if ((SlipDayofWeek == 2) && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "029":
                        {
                            //  Last Tuesday of the month
                            if ((SlipDayofWeek == 3) && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "030":
                        {
                            //  Last Wednesday of the month
                            if ((SlipDayofWeek == 4) && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "031":
                        {
                            //  Last Thursday of the month
                            if ((SlipDayofWeek == 5) && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "032":
                        {
                            //  Last Friday of the month
                            if ((SlipDayofWeek == 6) && (SlipDate.AddDays(-7).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }


                    case "041":
                        {
                            //  Third Sunday of the month
                            if (SlipDate.Day > 14 && (SlipDayofWeek == 1) && (SlipDate.AddDays(-21).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }

                    case "042":
                        {
                            //  Third Monday of the month
                            if (SlipDate.Day > 14 && (SlipDayofWeek == 2) && (SlipDate.AddDays(-21).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "043":
                        {
                            //  Third Tuesday of the month
                            if (SlipDate.Day > 14 && (SlipDayofWeek == 3) && (SlipDate.AddDays(-21).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "044":
                        {
                            //  Third Wednesday of the month
                            if (SlipDate.Day > 14 && (SlipDayofWeek == 4) && (SlipDate.AddDays(-21).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "045":
                        {
                            //  Third Thursday of the month
                            if (SlipDate.Day > 14 && (SlipDayofWeek == 5) && (SlipDate.AddDays(-21).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "046":
                        {
                            //  Third Friday of the month
                            if (SlipDate.Day > 14 && (SlipDayofWeek == 6) && (SlipDate.AddDays(-21).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                    case "047":
                        {
                            //  Third Saturday of the month
                            if (SlipDate.Day > 14 && (SlipDayofWeek == 7) && (SlipDate.AddDays(-21).Month != SlipDate.Month))
                            {
                                NeedSlip = true;
                                break;
                            }
                            else
                            {

                                break;
                            }
                        }
                }
            }
            return NeedSlip;
        }

    }

}
