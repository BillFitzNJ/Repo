using System;
using System.Collections.Generic;
using Microsoft.Reporting.WinForms;
using CommonAppClasses;
using Microsoft.Office.Core;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;
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
    public class SmartSafeSpreadsheetMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Transmission");
        AppUtilities appUtilities = new AppUtilities();
        BusinessReports.BusinessReportsMethods businessReportsMethods = new BusinessReports.BusinessReportsMethods();
        FrmLoading frmLoading = new FrmLoading();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocesssearchds = new ssprocess();

        public SmartSafeSpreadsheetMethods()
            : base("SQL", "SQLConnString")
        {

        }



        public void StartApp()
        {
            List<string> AttachmentNames = new List<string>();
            string bankfedid = "CCSmartSafe";
            DateTime SheetDate = new DateTime();
            if (commonAppDataMethods.GetSingleDate("Enter Posting Date", 200, 200))
            {

                 SheetDate = commonAppDataMethods.SelectedDate.Date;
                 if (SheetDate <= commonAppDataMethods.GetNextPostDate(bankfedid).AddDays(-1).Date)
                 {
                     AttachmentNames.Add(CreateSmartSafeSpreadsheet(bankfedid, SheetDate));
                     commonAppDataMethods.SendBankEmail(AttachmentNames, bankfedid, "Safe and Sound Smart Safe Transaction Details " + String.Format("{0:MM-dd-yy}", SheetDate), "See attached files");
                     wsgUtilities.wsgNotice("Operation Complete");
                 }
                 else
                 {
                     wsgUtilities.wsgNotice("That date has not been closed in the inventory");
           
                 }
                }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }


        public string CreateSmartSafeSpreadsheet(string bankfedid, DateTime SheetDate)
        {


            string commandtext = "";
            string outputfilename = ConfigurationManager.AppSettings["FTPFilepath"] + "Smart Safe -cash connect" + String.Format("{0:MMddyy}", SheetDate) + ".xlsx";
            Microsoft.Office.Interop.Excel.Application objApp = new Microsoft.Office.Interop.Excel.Application();
            string sheettitle = bankfedid + String.Format("{0:MMddyy}", SheetDate);
            object misValue = System.Reflection.Missing.Value;
            Excel.Workbooks objBooks = objApp.Workbooks;
            Excel.Workbook objBook = objBooks.Add(misValue);
            Excel.Worksheet xlWorkSheet = objBook.Worksheets.get_Item(1);
            xlWorkSheet.Name = "Safe Recap";
            xlWorkSheet.Cells[1, 1] = "WSFS Safe Recap/Inventory";
            xlWorkSheet.Cells[2, 1] = " Courier Branch:";
            xlWorkSheet.Cells[2, 2] = "Rapid Armored Using Subcontractor Safe and Sound Armored Courier, Inc.";
            xlWorkSheet.Cells[3, 1] = " Courier Branch:";
            xlWorkSheet.Cells[3, 2] = "3200";
            xlWorkSheet.Cells[4, 1] = "Report Date (Date Deposited):";
            xlWorkSheet.Cells[4, 2] = SheetDate;
            xlWorkSheet.Cells[6, 1] = "Vault Summary Records";
            xlWorkSheet.Cells[6, 2] = "Hundreds";
            xlWorkSheet.Cells[6, 3] = "Fiftys";
            xlWorkSheet.Cells[6, 4] = "Twentys";
            xlWorkSheet.Cells[6, 5] = "Tens";
            xlWorkSheet.Cells[6, 6] = "Fives";
            xlWorkSheet.Cells[6, 7] = "Twos";
            xlWorkSheet.Cells[6, 8] = "Ones";
            xlWorkSheet.Cells[6, 9] = "OneCoin";
            xlWorkSheet.Cells[6, 10] = "Quarters";
            xlWorkSheet.Cells[6, 11] = "Dimes";
            xlWorkSheet.Cells[6, 12] = "Nickels";
            xlWorkSheet.Cells[6, 13] = "Pennies";
            xlWorkSheet.Cells[6, 14] = "Totals";
            xlWorkSheet.Cells[6, 15] = "Notes";
         
            xlWorkSheet.Range["A6:O6"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            ssprocessds.balance.Rows.Clear();
            ClearParameters();
            AddParms("@sheetdate", SheetDate, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");
            commandtext = "SELECT TOP 1.* FROM balance  WHERE postdate < @sheetdate AND bankfedid = @bankfedid ORDER BY postdate DESC";
            FillData(ssprocessds, "balance", commandtext, CommandType.Text);

            // Balance information
            // Opening balance
            xlWorkSheet.Cells[7, 1] = "Beginnning Vault Cash";
            xlWorkSheet.Cells[7, 2] = ssprocessds.balance[0].hundreds;
            xlWorkSheet.Cells[7, 3] = ssprocessds.balance[0].fiftys;
            xlWorkSheet.Cells[7, 4] = ssprocessds.balance[0].twentys;
            xlWorkSheet.Cells[7, 5] = ssprocessds.balance[0].tens;
            xlWorkSheet.Cells[7, 6] = ssprocessds.balance[0].fives;
            xlWorkSheet.Cells[7, 7] = ssprocessds.balance[0].twos;
            xlWorkSheet.Cells[7, 8] = ssprocessds.balance[0].ones;
            xlWorkSheet.Cells[7, 9] =  0;
            xlWorkSheet.Cells[7, 10] = 0;
            xlWorkSheet.Cells[7, 11] = 0;
            xlWorkSheet.Cells[7, 12] = 0;
            xlWorkSheet.Cells[7, 13] = 0;
            xlWorkSheet.Cells[7, 14] = ssprocessds.balance[0].hundreds + ssprocessds.balance[0].fiftys + ssprocessds.balance[0].twentys + ssprocessds.balance[0].tens + ssprocessds.balance[0].fives
                + ssprocessds.balance[0].twos + ssprocessds.balance[0].ones + ssprocessds.balance[0].sba;

            xlWorkSheet.Cells[8, 1] = "Incoming Shipments";
            xlWorkSheet.Cells[8, 2] = 0.00m;
            xlWorkSheet.Cells[8, 3] = 0.00m;
            xlWorkSheet.Cells[8, 4] = 0.00m;
            xlWorkSheet.Cells[8, 5] = 0.00m;
            xlWorkSheet.Cells[8, 6] = 0m;
            xlWorkSheet.Cells[8, 7] = 0m;
            xlWorkSheet.Cells[8, 8] = 0m;
            xlWorkSheet.Cells[8, 9] = 0m;
            xlWorkSheet.Cells[8, 10] = 0m;
            xlWorkSheet.Cells[8, 11] = 0m;
            xlWorkSheet.Cells[8, 12] = 0m;
            xlWorkSheet.Cells[8, 13] = 0m;


            // Today's verified transactions
            xlWorkSheet.Cells[9, 1] = "Incoming Deposits";
            xlWorkSheet.Cells[9, 2] = 0.00m;
            xlWorkSheet.Cells[9, 3] = 0.00m;
            xlWorkSheet.Cells[9, 4] = 0.00m;
            xlWorkSheet.Cells[9, 5] = 0.00m;
            xlWorkSheet.Cells[9, 6] = 0m;
            xlWorkSheet.Cells[9, 7] = 0m;
            xlWorkSheet.Cells[9, 8] = 0m;
            xlWorkSheet.Cells[9, 9] = 0m;
            xlWorkSheet.Cells[9, 10] = 0m;
            xlWorkSheet.Cells[9, 11] = 0m;
            xlWorkSheet.Cells[9, 12] = 0m;
            xlWorkSheet.Cells[9, 13] = 0m;
            xlWorkSheet.Cells[9, 14] = 0m;


            ssprocessds.view_inventoriedsmartsafetrans.Rows.Clear();
            ClearParameters();
            AddParms("@sheetdate", SheetDate, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");

            // Incoming Verified
            commandtext = "SELECT * FROM  view_inventoriedsmartsafetrans  WHERE trandescrip  = 'SMARTVER' AND CONVERT(DATE, trandate)  = CONVERT(date, @sheetdate) AND bankfedid = @bankfedid";
            FillData(ssprocessds, "view_inventoriedsmartsafetrans", commandtext, CommandType.Text);

            for (int i = 0; i <= ssprocessds.view_inventoriedsmartsafetrans.Rows.Count - 1; i++)
            {
                xlWorkSheet.Cells[9, 2] = Convert.ToDecimal(xlWorkSheet.Cells[9, 2].value) + ssprocessds.view_inventoriedsmartsafetrans[i].hundreds;
                xlWorkSheet.Cells[9, 3] = Convert.ToDecimal(xlWorkSheet.Cells[9, 3].value) + ssprocessds.view_inventoriedsmartsafetrans[i].fiftys;
                xlWorkSheet.Cells[9, 4] = Convert.ToDecimal(xlWorkSheet.Cells[9, 4].value) + ssprocessds.view_inventoriedsmartsafetrans[i].twentys;
                xlWorkSheet.Cells[9, 5] = Convert.ToDecimal(xlWorkSheet.Cells[9, 5].value) + ssprocessds.view_inventoriedsmartsafetrans[i].tens;
                xlWorkSheet.Cells[9, 6] = Convert.ToDecimal(xlWorkSheet.Cells[9, 6].value) + ssprocessds.view_inventoriedsmartsafetrans[i].fives;
                xlWorkSheet.Cells[9, 7] = Convert.ToDecimal(xlWorkSheet.Cells[9, 7].value) + ssprocessds.view_inventoriedsmartsafetrans[i].twos;
                xlWorkSheet.Cells[9, 8] = Convert.ToDecimal(xlWorkSheet.Cells[9, 8].value) + ssprocessds.view_inventoriedsmartsafetrans[i].ones;
                xlWorkSheet.Cells[9, 14] = Convert.ToDecimal(xlWorkSheet.Cells[9, 8].value) + ssprocessds.view_inventoriedsmartsafetrans[i].cashtotal;

            }

            // FED Shipments
            xlWorkSheet.Cells[10, 1] = "Shipment to Correspondent Bank";
            xlWorkSheet.Cells[10, 2] = 0.00m;
            xlWorkSheet.Cells[10, 3] = 0.00m;
            xlWorkSheet.Cells[10, 4] = 0.00m;
            xlWorkSheet.Cells[10, 5] = 0.00m;
            xlWorkSheet.Cells[10, 6] = 0m;
            xlWorkSheet.Cells[10, 7] = 0m;
            xlWorkSheet.Cells[10, 8] = 0m;
            xlWorkSheet.Cells[10, 9] = 0m;
            xlWorkSheet.Cells[10, 10] = 0m;
            xlWorkSheet.Cells[10, 11] = 0m;
            xlWorkSheet.Cells[10, 12] = 0m;
            xlWorkSheet.Cells[10, 13] = 0m;
            xlWorkSheet.Cells[10, 14] = 0m;

            ssprocessds.view_mathinvtrans.Rows.Clear();
            ClearParameters();
            AddParms("@sheetdate", SheetDate, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");

            //  Outgoing FED
            commandtext = "SELECT * FROM  view_mathinvtrans  WHERE trandescrip  = 'BulkOut' AND CONVERT(DATE, trandate)  = CONVERT(date, @sheetdate) AND bankfedid = @bankfedid";
            FillData(ssprocessds, "view_mathinvtrans", commandtext, CommandType.Text);

            for (int i = 0; i <= ssprocessds.view_mathinvtrans.Rows.Count - 1; i++)
            {
                xlWorkSheet.Cells[10, 2] = Convert.ToDecimal(xlWorkSheet.Cells[10, 2].value) + ssprocessds.view_mathinvtrans[i].hundreds;
                xlWorkSheet.Cells[10, 3] = Convert.ToDecimal(xlWorkSheet.Cells[10, 3].value) + ssprocessds.view_mathinvtrans[i].fiftys;
                xlWorkSheet.Cells[10, 4] = Convert.ToDecimal(xlWorkSheet.Cells[10, 4].value) + ssprocessds.view_mathinvtrans[i].twentys;
                xlWorkSheet.Cells[10, 5] = Convert.ToDecimal(xlWorkSheet.Cells[10, 5].value) + ssprocessds.view_mathinvtrans[i].tens;
                xlWorkSheet.Cells[10, 6] = Convert.ToDecimal(xlWorkSheet.Cells[10, 6].value) + ssprocessds.view_mathinvtrans[i].fives;
                xlWorkSheet.Cells[10, 7] = Convert.ToDecimal(xlWorkSheet.Cells[10, 7].value) + ssprocessds.view_mathinvtrans[i].twos;
                xlWorkSheet.Cells[10, 8] = Convert.ToDecimal(xlWorkSheet.Cells[10, 8].value) + ssprocessds.view_mathinvtrans[i].ones;
                xlWorkSheet.Cells[10, 14] = Convert.ToDecimal(xlWorkSheet.Cells[10, 8].value) + ssprocessds.view_mathinvtrans[i].hundreds + ssprocessds.view_mathinvtrans[i].fiftys + ssprocessds.view_mathinvtrans[i].twentys +
                   +ssprocessds.view_mathinvtrans[i].tens + ssprocessds.view_mathinvtrans[i].fives + ssprocessds.view_mathinvtrans[i].twos + ssprocessds.view_mathinvtrans[i].ones; 
            }

          


            xlWorkSheet.Cells[11, 1] = "Inter-Inventory Transfer";
            xlWorkSheet.Cells[11, 2] = 0.00m;
            xlWorkSheet.Cells[11, 3] = 0.00m;
            xlWorkSheet.Cells[11, 4] = 0.00m;
            xlWorkSheet.Cells[11, 5] = 0.00m;
            xlWorkSheet.Cells[11, 6] = 0m;
            xlWorkSheet.Cells[11, 7] = 0m;
            xlWorkSheet.Cells[11, 8] = 0m;
            xlWorkSheet.Cells[11, 9] = 0m;
            xlWorkSheet.Cells[11, 10] = 0m;
            xlWorkSheet.Cells[11, 11] = 0m;
            xlWorkSheet.Cells[11, 12] = 0m;
            xlWorkSheet.Cells[11, 13] = 0m;

            xlWorkSheet.Cells[12, 1] = "Mutilated/Unfit Cash In Inventory";
            xlWorkSheet.Cells[12, 2] = 0.00m;
            xlWorkSheet.Cells[12, 3] = 0.00m;
            xlWorkSheet.Cells[12, 4] = 0.00m;
            xlWorkSheet.Cells[12, 5] = 0.00m;
            xlWorkSheet.Cells[12, 6] = 0m;
            xlWorkSheet.Cells[12, 7] = 0m;
            xlWorkSheet.Cells[12, 8] = 0m;
            xlWorkSheet.Cells[12, 9] = 0m;
            xlWorkSheet.Cells[12, 10] = 0m;
            xlWorkSheet.Cells[12, 11] = 0m;
            xlWorkSheet.Cells[12, 12] = 0m;
            xlWorkSheet.Cells[12, 13] = 0m;

            xlWorkSheet.Cells[13, 1] = "Other - Must Provide Explanation";
            xlWorkSheet.Cells[13, 2] = 0.00m;
            xlWorkSheet.Cells[13, 3] = 0.00m;
            xlWorkSheet.Cells[13, 4] = 0.00m;
            xlWorkSheet.Cells[13, 5] = 0.00m;
            xlWorkSheet.Cells[13, 6] = 0m;
            xlWorkSheet.Cells[13, 7] = 0m;
            xlWorkSheet.Cells[13, 8] = 0m;
            xlWorkSheet.Cells[13, 9] = 0m;
            xlWorkSheet.Cells[13, 10] = 0m;
            xlWorkSheet.Cells[13, 11] = 0m;
            xlWorkSheet.Cells[13, 12] = 0m;
            xlWorkSheet.Cells[13, 13] = 0m;

            ClearParameters();
            AddParms("@sheetdate", SheetDate, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");
            ssprocessds.balance.Rows.Clear();
            commandtext = "SELECT  * FROM balance  WHERE postdate  = @sheetdate AND bankfedid = @bankfedid";
            FillData(ssprocessds, "balance", commandtext, CommandType.Text);

            // Closing balance
            xlWorkSheet.Cells[14, 1] = "Ending Inventory Balance";
            xlWorkSheet.Cells[14, 2] = ssprocessds.balance[0].hundreds;
            xlWorkSheet.Cells[14, 3] = ssprocessds.balance[0].fiftys;
            xlWorkSheet.Cells[14, 4] = ssprocessds.balance[0].twentys;
            xlWorkSheet.Cells[14, 5] = ssprocessds.balance[0].tens;
            xlWorkSheet.Cells[14, 6] = ssprocessds.balance[0].fives;
            xlWorkSheet.Cells[14, 7] = ssprocessds.balance[0].twos;
            xlWorkSheet.Cells[14, 8] = ssprocessds.balance[0].ones;
            xlWorkSheet.Cells[14, 9] = 0;
            xlWorkSheet.Cells[14, 10] = 0;
            xlWorkSheet.Cells[14, 11] = 0;
            xlWorkSheet.Cells[14, 12] = 0;
            xlWorkSheet.Cells[14, 13] = 0;
            xlWorkSheet.Cells[14, 14] = ssprocessds.balance[0].hundreds + ssprocessds.balance[0].fiftys + ssprocessds.balance[0].twentys + ssprocessds.balance[0].tens + ssprocessds.balance[0].fives
                    + ssprocessds.balance[0].twos + ssprocessds.balance[0].ones + ssprocessds.balance[0].sba;


            Excel.Range dollarsandcentsange = (Excel.Range)xlWorkSheet.get_Range("B7", "N14");
            dollarsandcentsange.NumberFormat = "$#,##0.00";
            dollarsandcentsange.HorizontalAlignment = HorizontalAlignment.Right;
            commandtext = "SELECT * FROM view_inventoriedsmartsafetrans  WHERE trandescrip = 'SMARTVER' AND CONVERT(DATE, trandate)  = CONVERT(date, @sheetdate) AND bankfedid = @bankfedid";
            ssprocessds.view_inventoriedsmartsafetrans.Rows.Clear();
            ClearParameters();
            AddParms("@sheetdate", SheetDate, "SQL");
            AddParms("@bankfedid", bankfedid, "SQL");
            FillData(ssprocessds, "view_inventoriedsmartsafetrans", commandtext, CommandType.Text);

            // Safe Detail
            xlWorkSheet.Cells[16, 1] = "Safe Detail Records";
            xlWorkSheet.Cells[17, 1] = "Safe ID";
            xlWorkSheet.Cells[17, 2] = "Trans Code";
            xlWorkSheet.Cells[17, 3] = "Pickup Date";
            xlWorkSheet.Cells[17, 4] = "Pickup Time";
            xlWorkSheet.Cells[17, 5] = "Declared Pickup";
            xlWorkSheet.Cells[17, 6] = "Valid 100's";
            xlWorkSheet.Cells[17, 7] = "Valid 50's";
            xlWorkSheet.Cells[17, 8] = "Valid 20's";
            xlWorkSheet.Cells[17, 9] = "Valid 10's";
            xlWorkSheet.Cells[17, 10] = "Valid 5's";
            xlWorkSheet.Cells[17, 11] = "Valid 2's";
            xlWorkSheet.Cells[17, 12] = "Valid 1's";
            xlWorkSheet.Cells[17, 13] = "Valid 1 Coin";
            xlWorkSheet.Cells[17, 14] = "Valid Quarters";
            xlWorkSheet.Cells[17, 15] = "Valid Dimes";
            xlWorkSheet.Cells[17, 16] = "Valid Nickels";
            xlWorkSheet.Cells[17, 17] = "Valid Pennies";
            xlWorkSheet.Cells[17, 18] = "UnValid 100's";
            xlWorkSheet.Cells[17, 19] = "UnValid 50's";
            xlWorkSheet.Cells[17, 20] = "UnValid 20's";
            xlWorkSheet.Cells[17, 21] = "UnValid 10's";
            xlWorkSheet.Cells[17, 22] = "UnValid 5's";
            xlWorkSheet.Cells[17, 23] = "UnValid 2's";
            xlWorkSheet.Cells[17, 24] = "UnValid 1's";
            xlWorkSheet.Cells[17, 25] = "UnValid 1 Coin";
            xlWorkSheet.Cells[17, 26] = "UnValid Quarters";
            xlWorkSheet.Cells[17, 27] = "UnUnValid Dimes";
            xlWorkSheet.Cells[17, 28] = "UnValid Nickels";
            xlWorkSheet.Cells[17, 29] = "UnValid Pennies";
            xlWorkSheet.Cells[17, 30] = "Total Pickup";
            xlWorkSheet.Cells[17, 31] = "Over/Short";
            xlWorkSheet.Cells[17, 32] = "Notes";
            Excel.Range hrange = (Excel.Range)xlWorkSheet.get_Range("A17", "AF17");
            hrange.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            
            decimal totalhundreds = 0;
            decimal totalfiftys = 0;
            decimal totaltwentys = 0;
            decimal totaltens = 0;
            decimal totalfives = 0;
            decimal totaltwos = 0;
            decimal totalones = 0;
            decimal totalall = 0;
            int saferowstart = 17;
            int saferow = saferowstart;
            int column = 0;
            for (int i = 0; i <= ssprocessds.view_inventoriedsmartsafetrans.Rows.Count - 1; i++)
            {
               
                saferow ++;
                xlWorkSheet.Cells[saferow, 1] = ssprocessds.view_inventoriedsmartsafetrans[i].serialnumber;
                xlWorkSheet.Cells[saferow, 2] = 7;
                xlWorkSheet.Cells[saferow, 5] = ssprocessds.view_inventoriedsmartsafetrans[i].cashtotal;
                xlWorkSheet.Cells[saferow, 6] = ssprocessds.view_inventoriedsmartsafetrans[i].hundreds;
                totalhundreds += ssprocessds.view_inventoriedsmartsafetrans[i].hundreds;
                xlWorkSheet.Cells[saferow, 7] = ssprocessds.view_inventoriedsmartsafetrans[i].fiftys;
                totalfiftys += ssprocessds.view_inventoriedsmartsafetrans[i].fiftys;
                xlWorkSheet.Cells[saferow, 8] = ssprocessds.view_inventoriedsmartsafetrans[i].twentys;
                totaltwentys += ssprocessds.view_inventoriedsmartsafetrans[i].twentys;
                xlWorkSheet.Cells[saferow, 9] = ssprocessds.view_inventoriedsmartsafetrans[i].tens;
                totaltens += ssprocessds.view_inventoriedsmartsafetrans[i].tens;
                xlWorkSheet.Cells[saferow, 10] = ssprocessds.view_inventoriedsmartsafetrans[i].fives;
                totalfives += ssprocessds.view_inventoriedsmartsafetrans[i].fives;
                xlWorkSheet.Cells[saferow, 11] = ssprocessds.view_inventoriedsmartsafetrans[i].twos;
                totaltwos += ssprocessds.view_inventoriedsmartsafetrans[i].twos;
                xlWorkSheet.Cells[saferow, 12] = ssprocessds.view_inventoriedsmartsafetrans[i].ones;
                totalones += ssprocessds.view_inventoriedsmartsafetrans[i].ones;
                xlWorkSheet.Cells[saferow, 30] = ssprocessds.view_inventoriedsmartsafetrans[i].cashtotal;
                totalall += ssprocessds.view_inventoriedsmartsafetrans[i].cashtotal;
                xlWorkSheet.Cells[saferow, 31] = 0m;
                column = 13;
                while (column < 30)
                {
                    xlWorkSheet.Cells[saferow, column] = 0m;
                    column++;
                }
            }
            saferow++;
            xlWorkSheet.Cells[saferow, 2] = "TOTALS";
            xlWorkSheet.Cells[saferow, 6] = totalhundreds;
            xlWorkSheet.Cells[saferow, 7] = totalfiftys;
            xlWorkSheet.Cells[saferow, 8] = totaltwentys;
            xlWorkSheet.Cells[saferow, 9] = totaltens;
            xlWorkSheet.Cells[saferow, 10] = totalfives;
            xlWorkSheet.Cells[saferow, 11] = totaltwos;
            xlWorkSheet.Cells[saferow, 12] = totalones;
            column = 13;
            while (column < 30)
            {
                xlWorkSheet.Cells[saferow, column] = 0m;
                column++;
            }
            xlWorkSheet.Cells[saferow, 30] = totalall;
            xlWorkSheet.Cells[saferow, 31] = 0m;
            Excel.Range detailrange = (Excel.Range)xlWorkSheet.get_Range("E" + saferowstart.ToString().TrimEnd().TrimStart(), "AE" + saferow.ToString().TrimEnd().TrimStart());
            detailrange.HorizontalAlignment = HorizontalAlignment.Right;
            detailrange.NumberFormat = "$#,##0.00";

            Excel.Range c1 = (Excel.Range)xlWorkSheet.Cells[1, 1];
            Excel.Range c2 = (Excel.Range)xlWorkSheet.Cells[1, 30];
            Excel.Range range = xlWorkSheet.get_Range(c1, c2);
            range.Columns.AutoFit();

            Excel.Range d1 = (Excel.Range)xlWorkSheet.Cells[2, 1];
            Excel.Range d2 = (Excel.Range)xlWorkSheet.Cells[2, 30];
            Excel.Range drange = xlWorkSheet.get_Range(d1, d2);
            drange.Columns.ColumnWidth = 15;

            if (File.Exists(outputfilename))
            {
                File.Delete(outputfilename);
            }
            objBook.SaveAs(outputfilename, Excel.XlFileFormat.xlOpenXMLWorkbook,
            misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            objBook.Close(true, misValue, misValue);
            objApp.Quit();
            releaseObject(xlWorkSheet);
            releaseObject(objBook);
            releaseObject(objApp);
            return outputfilename;
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }


    }
}
