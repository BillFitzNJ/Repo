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
    class StoreSelectorMethods : WSGDataAccess
    {
        public ssprocess ssprocessselectords = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        BindingSource bindingSelectorData = new BindingSource();
        string selectedstorecode = "";
        CommonAppDataMethods commonAddDataMethods = new CommonAppDataMethods();
        FrmStoreSelector frmStoreSelector = new FrmStoreSelector();
        private string CurrentRowKey = "";
        private string CompanyCode;
        public StoreSelectorMethods()
            : base("SQL", "SQLConnString")
        {
            SetEvents();
        }
        public string SelectStore(string companycode)
        {
            CurrentRowKey = "";
            CompanyCode = companycode;
            RefreshData(true);
            bindingSelectorData.DataSource = ssprocessselectords.store;
            frmStoreSelector.dataGridViewStoreSelector.RowHeadersVisible = false;
            frmStoreSelector.dataGridViewStoreSelector.AutoGenerateColumns = false;
            frmStoreSelector.dataGridViewStoreSelector.AllowUserToAddRows = false;
            frmStoreSelector.dataGridViewStoreSelector.DataSource = bindingSelectorData;
            frmStoreSelector.dataGridViewStoreSelector.RowsDefaultCellStyle.BackColor = Color.LightGray;
            frmStoreSelector.dataGridViewStoreSelector.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            frmStoreSelector.dataGridViewStoreSelector.Columns[0].Selected = true;
            frmStoreSelector.checkBoxIncludeinactive.Checked = false;
            frmStoreSelector.dataGridViewStoreSelector.Focus();
            frmStoreSelector.Text = "Locations for " + commonAddDataMethods.GetCompanyName(companycode);
            frmStoreSelector.ShowDialog();
            return selectedstorecode;
        }

        private void SetEvents()
        {
         frmStoreSelector.dataGridViewStoreSelector.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(CaptureDoubleClick);
         frmStoreSelector.dataGridViewStoreSelector.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectorKeyDown);
         frmStoreSelector.buttonCancel.Click += new System.EventHandler(CaptureCancel);
         frmStoreSelector.checkBoxIncludeinactive.CheckedChanged += checkBoxIncludeinactive_CheckedChanged;

        }

        private void checkBoxIncludeinactive_CheckedChanged(object sender, EventArgs e)
        {
            if (frmStoreSelector.checkBoxIncludeinactive.Checked)
            {
                RefreshData(false);
            }
            else
            {
                RefreshData(true);
            }
        }


        private void RefreshData(bool includeonlyactive)
        {
            string commandtext = "SELECT RIGHT(LEFT(storecode,11),6) AS storecode, store_name, f_address, idcol FROM  store WHERE LEFT(storecode,4) = @companycode ";
            if (includeonlyactive)
            {
                commandtext += " AND active = 1 ";
            }
            
            commandtext +=      " ORDER BY storecode ";
            ClearParameters();
            AddParms("@companycode", CompanyCode, "SQL");
            ssprocessselectords.store.Rows.Clear();
            FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
            if (ssprocessselectords.store.Rows.Count > 0)
            {
            }
        }

        public void CaptureStoreIdcol()
        {
            int selectedstoreid = CaptureIdCol(frmStoreSelector.dataGridViewStoreSelector);
            ssprocessselectords.store.Rows.Clear();
            ssprocessselectords.store.Rows.Clear();
            string commandtext = "SELECT * FROM store where idcol = @idcol";
            ClearParameters();
            AddParms("@idcol", selectedstoreid, "SQL");
            FillData(ssprocessselectords, "store", commandtext, CommandType.Text);
            selectedstorecode = ssprocessselectords.store[0].storecode;
        }
        private void CaptureDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CaptureStoreIdcol();
            frmStoreSelector.Close();
        }
        private void CaptureCancel(object sender, EventArgs e)
        {
            frmStoreSelector.Close();
        }


        private void SelectorKeyDown(object sender, KeyEventArgs e)
        {

            // Use incremental search
            int ix = 0;
            switch (e.KeyCode)
            {
                case Keys.Return:
                    {
                        CaptureStoreIdcol();
                        frmStoreSelector.Close();
                        break;
                    }
                case Keys.Home:
                    {
                        CurrentRowKey = "";
                        frmStoreSelector.dataGridViewStoreSelector.CurrentCell = frmStoreSelector.dataGridViewStoreSelector.Rows[0].Cells[0];
                        break;
                    }
                case Keys.Up:
                    {
                        break;
                    }
                case Keys.PageUp:
                    {
                        break;
                    }
                case Keys.PageDown:
                    {
                        break;
                    }
                case Keys.Down:
                    {
                        break;
                    }
                default:
                    {
                        CurrentRowKey += Convert.ToChar(e.KeyCode).ToString().ToUpper();
                     
                        if (CurrentRowKey.Length > 6)
                        {
                            CurrentRowKey = "";
                            break;
                        }
                        while (ix < frmStoreSelector.dataGridViewStoreSelector.RowCount - 1)
                        {
                            string x = frmStoreSelector.dataGridViewStoreSelector.Rows[ix].Cells[0].Value.ToString().ToUpper();
                            if (x.Substring(0, CurrentRowKey.Length) == CurrentRowKey)
                            {
                                frmStoreSelector.dataGridViewStoreSelector.CurrentCell = frmStoreSelector.dataGridViewStoreSelector.Rows[ix].Cells[0];
                                break;
                            }
                            else
                            {
                                ix++;
                                continue;
                            }
                        }
                        break;
                    }
            }

        }
    }
}
