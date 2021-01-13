using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using BusinessTransactions;
using System.Configuration;
using System.Globalization;
using System.Data;
using System.IO;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace BusinessTransactions
{
    public class PickupMethods : WSGDataAccess
    {
        atm atmds = new atm();
        ssprocess ssprocessds = new ssprocess();
        WSGUtilities wsgUtilities = new WSGUtilities("Reports");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        FrmPickupSelector frmPickupSelector = new FrmPickupSelector();
        BindingSource bindingSelectorData = new BindingSource();
             
        public PickupMethods()
            : base("SQL", "SQLConnString")
        {

        }

        public void EditPickups()
        {
            string storecode = commonAppDataMethods.SelectCompanyAndStore();
            DateTime pickupdate = new DateTime();
            if (storecode.TrimEnd() != "")
            {
                if (commonAppDataMethods.GetSingleDate("Enter Pickup Date", 10000, 10000))
                {

                    pickupdate = commonAppDataMethods.SelectedDate.Date;
                    ssprocessds.view_expandedhhpickup.Rows.Clear();
                    ClearParameters();
                    AddParms("@pickupdate", pickupdate, "SQL");
                    AddParms("@storecode", storecode, "SQL");
                    string commandtext = "SELECT  * from view_expandedhhpickup where storecode = @storecode AND pickupdate >= @pickupdate AND pickupdate < DATEADD(d,1, @pickupdate)";
                    FillData(ssprocessds, "view_expandedhhpickup", commandtext, CommandType.Text);
                    if (ssprocessds.view_expandedhhpickup.Rows.Count > 0)
                    {
                        bindingSelectorData.DataSource = ssprocessds.view_expandedhhpickup;

                        frmPickupSelector.dataGridViewPickups.RowHeadersVisible = false;
                        frmPickupSelector.dataGridViewPickups.AutoGenerateColumns = false;
                        frmPickupSelector.dataGridViewPickups.AllowUserToAddRows = false;
                        frmPickupSelector.dataGridViewPickups.DataSource = bindingSelectorData;
                        frmPickupSelector.dataGridViewPickups.DefaultCellStyle.BackColor = Color.LightGray;
                        frmPickupSelector.dataGridViewPickups.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
                        frmPickupSelector.dataGridViewPickups.Columns[0].Selected = true;
                        frmPickupSelector.dataGridViewPickups.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(CaptureDoubleClick);
                        frmPickupSelector.ShowDialog();
                    }
                    else
                    {
                        wsgUtilities.wsgNotice("There are no pickups that day for that location");
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
        private void CaptureDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
             int   SelectedIdcol = CaptureIdCol( frmPickupSelector.dataGridViewPickups);
            if (wsgUtilities.wsgReply("Delete this pickup"))
            {
                ClearParameters();
                AddParms("@idcol", SelectedIdcol, "SQL");
                string commandtext =  "DELETE  from  hhpickup where idcol = @idcol";
                ExecuteCommand(commandtext, CommandType.Text);
                wsgUtilities.wsgNotice("Operation Complete");
                frmPickupSelector.Close();
            }
        }
    }
}