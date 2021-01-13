using System;
using System.Collections.Generic;
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
    public class FrmSelectorMethods : WSGDataAccess
    {

        public FrmSelector frmSelector = new FrmSelector();
        sysdata.appeventDataTable currentDataTable = new sysdata.appeventDataTable();
        //
        sysdata sysdatads = new sysdata();
        AppUtilities appUtilities = new AppUtilities();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Selector");
        public DataTable dtSource = new DataTable();
        public int SelectedIdcol = 0;
        public bool SelectIdcol = true;
        public string SelectColumnName = "";
        public string SelectedString = "";
        public int columncount = 0;
        public int[] colwidth;
        private string CurrentRowKey = "";
        public string[] colname;
        public string[] colheadertext;
        public string[] coldatapropertyname;
        public string[] coldefaultcellstyle;
        public string CurrentState = "Select";
        public string FormText { get; set; }
        public FrmSelectorMethods()
            : base("SQL", "SQLConnString")
        {
            SetEvents();
            dtSource.Rows.Clear();
        }

        public void SetEvents()
        {
            frmSelector.dataGridViewSelector.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(CaptureDoubleClick);
            frmSelector.dataGridViewSelector.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectorKeyDown);
            frmSelector.buttonCancel.Click += new System.EventHandler(CaptureClose);

        }
        public void SetColumns()
        {
            colwidth = new int[columncount];
            colname = new string[columncount];
            colheadertext = new string[columncount];
            coldefaultcellstyle = new string[columncount];
            coldatapropertyname = new string[columncount];
            frmSelector.dataGridViewSelector.ColumnCount = columncount;
        }
        public int ShowSelector()
        {
            SelectIdcol = true;
            SelectedIdcol = 0;
            frmSelector.ShowDialog();
            return SelectedIdcol;
        }
        public string ShowStringSelector(string selectColumnName )
        {
            SelectColumnName = selectColumnName;
            SelectIdcol = false; 
            SelectedString = "";
            //   SetGrid();
            frmSelector.ShowDialog();
            return SelectedString;
        }
     
        public void SetGrid()
        {
            BindingSource bindingSelectorData = new BindingSource();
            bindingSelectorData.DataSource = dtSource;
            frmSelector.Text = FormText;
            frmSelector.dataGridViewSelector.Left = frmSelector.Left + 10;
            frmSelector.dataGridViewSelector.Width = 0;
            frmSelector.dataGridViewSelector.ReadOnly = true;
            for (int i = 0; i <= columncount - 1; i++)
            {
                frmSelector.dataGridViewSelector.Columns[i].Name = colname[i];
                frmSelector.dataGridViewSelector.Columns[i].DataPropertyName = coldatapropertyname[i];
                frmSelector.dataGridViewSelector.Columns[i].HeaderText = colheadertext[i];
                frmSelector.dataGridViewSelector.Columns[i].Width = colwidth[i];
                if (coldefaultcellstyle[i] != null)
                {
                    frmSelector.dataGridViewSelector.Columns[i].DefaultCellStyle.Format = coldefaultcellstyle[i];
                    frmSelector.dataGridViewSelector.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                frmSelector.dataGridViewSelector.Columns[i].ReadOnly = true;
                frmSelector.dataGridViewSelector.Width += colwidth[i];
            }

            frmSelector.Width = frmSelector.dataGridViewSelector.Width + 30;
            frmSelector.dataGridViewSelector.RowHeadersVisible = false;
            frmSelector.dataGridViewSelector.AutoGenerateColumns = false;
            frmSelector.dataGridViewSelector.AllowUserToAddRows = false;
            frmSelector.dataGridViewSelector.DataSource = bindingSelectorData;
            frmSelector.dataGridViewSelector.RowsDefaultCellStyle.BackColor = Color.LightGray;
            frmSelector.dataGridViewSelector.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            frmSelector.buttonCancel.Left = frmSelector.Width -100;
            frmSelector.dataGridViewSelector.Columns[0].Selected = true;
            if (frmSelector.dataGridViewSelector.Rows.Count > 0)
            {
                frmSelector.dataGridViewSelector.Rows[0].Selected = true;
            }
           
        }

        public void SetGridWithFormatting(DataGridViewCellFormattingEventHandler formatHandler)
        {
            BindingSource bindingSelectorData = new BindingSource();
            bindingSelectorData.DataSource = dtSource;
            frmSelector.Text = FormText;
            frmSelector.dataGridViewSelector.Left = frmSelector.Left + 10;
            frmSelector.dataGridViewSelector.ColumnCount = columncount;
            frmSelector.dataGridViewSelector.Width = 0;
            frmSelector.dataGridViewSelector.ReadOnly = true;
            for (int i = 0; i <= columncount - 1; i++)
            {
                frmSelector.dataGridViewSelector.Columns[i].Name = colname[i];
                frmSelector.dataGridViewSelector.Columns[i].DataPropertyName = coldatapropertyname[i];
                frmSelector.dataGridViewSelector.Columns[i].HeaderText = colheadertext[i];
                frmSelector.dataGridViewSelector.Columns[i].Width = colwidth[i];
                frmSelector.dataGridViewSelector.Columns[i].ReadOnly = true;
                frmSelector.dataGridViewSelector.Width += colwidth[i];
            }

            frmSelector.Width = frmSelector.dataGridViewSelector.Width + 30;
            frmSelector.dataGridViewSelector.RowHeadersVisible = false;
            frmSelector.dataGridViewSelector.AutoGenerateColumns = false;
            frmSelector.dataGridViewSelector.AllowUserToAddRows = false;
            frmSelector.dataGridViewSelector.DataSource = bindingSelectorData;
            frmSelector.buttonCancel.Left = frmSelector.Width - 90;
            frmSelector.dataGridViewSelector.Columns[0].Selected = true;
            if (frmSelector.dataGridViewSelector.Rows.Count > 0)
            {
                frmSelector.dataGridViewSelector.Rows[0].Selected = true;
            }

            frmSelector.dataGridViewSelector.CellFormatting += formatHandler;

        }


        public void CaptureSearchColumn()
        {
            if (SelectIdcol)
            {
                SelectedIdcol = CaptureIdCol(frmSelector.dataGridViewSelector);
            }
            else
            {
                SelectedString = CaptureDataGridColumn(frmSelector.dataGridViewSelector, SelectColumnName);
            }
         }
        private void CaptureDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CaptureSearchColumn();
            frmSelector.Close();
        }
        private void CaptureClose(object sender, EventArgs e)
        {
            frmSelector.Close();
        }

        public string CaptureDataGridColumn(DataGridView myDataGridView, string Column)
        {
            CurrencyManager xCM =
           (CurrencyManager)myDataGridView.BindingContext[myDataGridView.DataSource,
             myDataGridView.DataMember];
             DataRowView xDRV = (DataRowView)xCM.Current;
             DataRow xRow = xDRV.Row;
            return xRow[Column].ToString();
        }
        private void SelectorKeyDown(object sender, KeyEventArgs e)
        {

            // Use incremental search
            int ix = 0;
            switch (e.KeyCode)
            {
                case Keys.Return:
                    {
                        CaptureSearchColumn();
                        frmSelector.Close();
                        break;
                    }
                case Keys.Home:
                    {
                        CurrentRowKey = "";
                        frmSelector.dataGridViewSelector.CurrentCell = frmSelector.dataGridViewSelector.Rows[0].Cells[0];
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
                        if (CurrentRowKey.Length > 6)
                        {
                            CurrentRowKey = "";
                        }
                        CurrentRowKey += Convert.ToChar(e.KeyCode).ToString().ToUpper();
                        while (ix < frmSelector.dataGridViewSelector.RowCount - 1)
                        {
                            string x = frmSelector.dataGridViewSelector.Rows[ix].Cells[0].Value.ToString().ToUpper().PadRight(CurrentRowKey.Length,' ');
                            if (x.Substring(0, CurrentRowKey.Length) == CurrentRowKey)
                            {
                                frmSelector.dataGridViewSelector.CurrentCell = frmSelector.dataGridViewSelector.Rows[ix].Cells[0];
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
