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
    public class SmartSafeDailyPickupMethods : WSGDataAccess
    {

        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocesslistds = new ssprocess();

        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Event");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        FrmSmartSafeDailyActivity frmSmartSafeDailyActivity = new FrmSmartSafeDailyActivity();
        public SmartSafeDailyPickupMethods()
            : base("SQL", "SQLConnString")
        {

            SetIdcol(ssprocessds.smartsafetrans.idcolColumn);
            SetEvents();
            SetBindings();
        }

        public void StartDailyActivity()
        {

            ssprocesslistds.smartsafetrans.Rows.Clear();
            bool cont = true;
            int selectedsafemastid = 0;
            while (cont)
            {
                selectedsafemastid = commonAppDataMethods.SelectSmartSafe();
                if (selectedsafemastid > 0)
                {
                    ssprocessds.smartsafetrans.Rows.Clear();
                    EstablishBlankDataTableRow(ssprocessds.smartsafetrans);
                    frmSmartSafeDailyActivity.ShowDialog();
                }
                else
                {
                    break;
                }
            }

        }

        public void SetEvents()
        {
            frmSmartSafeDailyActivity.buttonClose.Click += new System.EventHandler(buttonClose_Click);
            frmSmartSafeDailyActivity.textBoxDailyAmount.KeyDown += new System.Windows.Forms.KeyEventHandler(SendTabonEnter);
            frmSmartSafeDailyActivity.textBoxDailyAmount.LostFocus += new System.EventHandler(ProcessDailyEntry);
        }
        public void SetBindings()
        {
            SetTextBoxCurrencyBinding(frmSmartSafeDailyActivity.textBoxDailyAmount, ssprocessds, "smartsafetrans.saidtocontain");
        }
        public void buttonClose_Click(object sender, EventArgs e)
        {
            frmSmartSafeDailyActivity.Close();
        }
        public void ProcessDailyEntry(object sender, EventArgs e)
        {
            frmSmartSafeDailyActivity.Close();
        }

        private void SetTextBoxCurrencyBinding(TextBox txtbox, DataSet ds, string fieldname)
        {
            Binding b = new Binding("Text", ds, fieldname);
            {
                b.Format += new ConvertEventHandler(DecimalToCurrencyString);
                b.Parse += new ConvertEventHandler(CurrencyStringToDecimal);
            }
            txtbox.DataBindings.Clear();
            txtbox.DataBindings.Add(b);
        }

        private void DecimalToCurrencyString(object sender, ConvertEventArgs cevent)
        {
            // The method converts only to string type. Test this using the DesiredType.
            if (cevent.DesiredType != typeof(string)) return;

            // Use the ToString method to format the value as currency ("c").
            cevent.Value = ((decimal)cevent.Value).ToString("N2");
        }
        private void CurrencyStringToDecimal(object sender, ConvertEventArgs cevent)
        {
            // The method converts back to decimal type only. 
            if (cevent.DesiredType != typeof(decimal)) return;

            // Converts the string back to decimal using the static Parse method.
            cevent.Value = Decimal.Parse(cevent.Value.ToString(),
            NumberStyles.Currency, null);
        }
        private void SendTabonEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }

        }
    }

}