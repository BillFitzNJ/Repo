using System;
using System.Collections.Generic;
using CommonAppClasses;
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

namespace ApplicationMaintenance
{
    public class MaintainRecurringCoinMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Recurring Coin");
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        public string Storecode = "";
        int tabIndex = 0;
        string commandtext = "";
        NumericUpDown numericUpDownHundreds = new NumericUpDown();
        Label LabelHundreds = new Label();
        NumericUpDown numericUpDownFiftys = new NumericUpDown();
        Label LabelFiftys = new Label();
        NumericUpDown numericUpDownTwentys = new NumericUpDown();
        Label LabelTwentys = new Label();
        NumericUpDown numericUpDownTens = new NumericUpDown();
        Label LabelTens = new Label();
        NumericUpDown numericUpDownFives = new NumericUpDown();
        Label LabelFives = new Label();
        NumericUpDown numericUpDownSingles = new NumericUpDown();
        Label LabelSingles = new Label();
        NumericUpDown numericUpDownQuarters = new NumericUpDown();
        Label LabelQuarters = new Label();
        NumericUpDown numericUpDownDimes = new NumericUpDown();
        Label LabelDimes = new Label();
        NumericUpDown numericUpDownNickels = new NumericUpDown();
        Label LabelNickels = new Label();
        NumericUpDown numericUpDownPennies = new NumericUpDown();
        Label LabelPennies = new Label();
        ComboBox ComboBoxDropDay = new ComboBox();
        Label LabelDropDay = new Label();

        DataGridView dataGridViewRecurringCoinSelector = new DataGridView();
        BindingSource bindingSelectorData = new BindingSource();

        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.recurringcoin;
            currentablename = "recurringcoin";
            SetIdcol(ssprocessds.recurringcoin.idcolColumn);
            parentForm.Text = "Maintain Recurring Coin";

        }
        public override void SetControls()
        {
            // Fill the scheduleday view
            ssprocessds.scheduleday.Rows.Clear();
            commandtext = "SELECT RIGHT(schday,1) AS schday , LEFT(descrip,9) AS  descrip FROM  scheduleday WHERE schday < '008'   ORDER by 1";
            ClearParameters();
            FillData(ssprocessds, "scheduleday", commandtext, CommandType.Text);

            ComboBoxDropDay.DataSource = ssprocessds.scheduleday;
            ComboBoxDropDay.ValueMember = "schday";
            ComboBoxDropDay.DisplayMember = "descrip";


            parentForm.buttonDelete.Visible = true;
            int TextTop = 100;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 600;
            SetNumericUpDown(numericUpDownHundreds, LeftStart, TextTop, 75, ssprocessds.recurringcoin, "hundreds", LabelHundreds, "Hundreds");
            numericUpDownHundreds.Maximum = 10000;
            numericUpDownHundreds.DecimalPlaces = 0;
            numericUpDownHundreds.Increment = 1000;
            SetNumericUpDown(numericUpDownSingles, LeftStart + 250, TextTop, 75, ssprocessds.recurringcoin, "singles", LabelSingles, "Singles");
            numericUpDownSingles.Maximum = 100;
            numericUpDownSingles.DecimalPlaces = 0;
            numericUpDownSingles.Increment = 100;
            SetBoundComboBox(ComboBoxDropDay, LeftStart + 500, TextTop, 100, LabelDropDay, "Drop Day", "schday", "descrip", ssprocessds.scheduleday);
            TextTop += 22;
            SetNumericUpDown(numericUpDownFiftys, LeftStart, TextTop, 75, ssprocessds.recurringcoin, "fiftys", LabelFiftys, "Fiftys");
            numericUpDownFiftys.Maximum = 5000;
            numericUpDownFiftys.DecimalPlaces = 0;
            numericUpDownFiftys.Increment = 1000;
            SetNumericUpDown(numericUpDownQuarters, LeftStart + 250, TextTop, 75, ssprocessds.recurringcoin, "quarters", LabelQuarters, "Quarters");
            numericUpDownQuarters.Maximum = 100;
            numericUpDownQuarters.DecimalPlaces = 0;
            numericUpDownQuarters.Increment = 10;
            TextTop += 22;
            SetNumericUpDown(numericUpDownTwentys, LeftStart, TextTop, 75, ssprocessds.recurringcoin, "twentys", LabelTwentys, "Twentys");
            numericUpDownTwentys.Maximum = 5000;
            numericUpDownTwentys.Increment = 500;
            numericUpDownTwentys.DecimalPlaces = 0;
            SetNumericUpDown(numericUpDownDimes, LeftStart + 250, TextTop, 75, ssprocessds.recurringcoin, "dimes", LabelDimes, "Dimes");
            numericUpDownDimes.Maximum = 100;
            numericUpDownDimes.DecimalPlaces = 0;
            numericUpDownDimes.Increment = 5;
            TextTop += 22;
            SetNumericUpDown(numericUpDownTens, LeftStart, TextTop, 75, ssprocessds.recurringcoin, "tens", LabelTens, "Tens");
            numericUpDownTens.Maximum = 5000;
            numericUpDownTens.DecimalPlaces = 0;
            numericUpDownTens.Increment = 500;
            SetNumericUpDown(numericUpDownNickels, LeftStart + 250, TextTop, 75, ssprocessds.recurringcoin, "Nickels", LabelNickels, "Nickels");
            numericUpDownNickels.Maximum = 100;
            numericUpDownNickels.DecimalPlaces = 0;
            numericUpDownNickels.Increment = 2;
            TextTop += 22;
            SetNumericUpDown(numericUpDownFives, LeftStart, TextTop, 75, ssprocessds.recurringcoin, "fives", LabelFives, "Fives");
            numericUpDownFives.Maximum = 5000;
            numericUpDownFives.DecimalPlaces = 0;
            numericUpDownFives.Increment = 500;
            SetNumericUpDown(numericUpDownPennies, LeftStart + 250, TextTop, 75, ssprocessds.recurringcoin, "pennies", LabelPennies, "Pennies");
            numericUpDownPennies.Maximum = 100;
            numericUpDownPennies.DecimalPlaces = 0;
            TextTop += 22;



            EstablishDatagridViewRecurringCoinSelector(TextTop + 25, LeftStart);
            parentForm.Width = dataGridViewRecurringCoinSelector.Width + LeftStart + 25;
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (cont)
            {
                ssprocessds.recurringcoin[0].dayofweek = Convert.ToString(ComboBoxDropDay.SelectedValue);
                GenerateAppTableRowSave(ssprocessds.recurringcoin[0]);
                RefreshDatagridview();
                ssprocessds.recurringcoin.Rows.Clear();
                CurrentState = "StoreSelected";
                RefreshControls();
            }
        }

        public override void ProcessSelect(object sender, EventArgs e)
        {
            Storecode = commonAppDataMethods.SelectCompanyAndStore();

            // If a row has been selected fill the data and process
            if (Storecode.TrimEnd().Length > 0)
            {
                parentForm.Text = "Processing recurring  coin  for " + commonAppDataMethods.GetStoreName(Storecode);
                RefreshDatagridview();
                CurrentState = "StoreSelected";
                RefreshControls();
            }

        }
        public override void SetEvents()
        {
            base.SetEvents();
            dataGridViewRecurringCoinSelector.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(CaptureDoubleClick);
            dataGridViewRecurringCoinSelector.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectorKeyDown);

        }
        public override void SetEditState(object sender, EventArgs e)
        {
            string editstatus = base.LockTableRow(ssprocessds.recurringcoin[0].idcol, "recurringcoin");
            if (editstatus == "OK")
            {

                CurrentState = "Edit";
                RefreshControls();
            }
            else
            {
                if (wsgUtilities.wsgReply(editstatus + ". Do you want to override?"))
                {
                    CurrentState = "Edit";
                    RefreshControls();
                    editstatus = "OK";
                    UnlockTableRow(currentidcol, currentablename);
                }
            }

        }

        private void CaptureDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadCurrentRecurringCoin();
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            ClearCalculatedFields();
            EstablishBlankDataTableRow(ssprocessds.recurringcoin);
            ssprocessds.recurringcoin[0].storecode = Storecode;
            CurrentState = "Insert";
            RefreshControls();
        }
        private void SelectorKeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Return)
            {
                LoadCurrentRecurringCoin();
            }
        }
        public override void ProcessCancel(object sender, EventArgs e)
        {

            if (CurrentState == "Edit" || CurrentState == "Insert")
            {
                if (wsgUtilities.wsgReply("Abandon Edit"))
                {
                    ClearCalculatedFields();
                    parentForm.Update();
                    if (CurrentState == "Edit")
                    {
                        UnlockTableRow(currentidcol, currentablename);

                    }

                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
            }
            else
            {
                ClearCalculatedFields();
                if (CurrentState == "View")
                {
                    ssprocessds.recurringcoin.Rows.Clear();
                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
                else
                {
                    ssprocessselectords.recurringcoin.Rows.Clear();
                    CurrentState = "Select";
                    RefreshControls();

                }
            }
        }



        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            RefreshDatagridview();
            CurrentState = "StoreSelected";
            RefreshControls();
            ssprocessds.recurringcoin.Rows.Clear();
            ClearCalculatedFields();
            parentForm.Update();
        }
        public void LoadCurrentRecurringCoin()
        {
            ClearCalculatedFields();
            ssprocessds.recurringcharge.Rows.Clear();
            currentidcol = base.CaptureIdCol(dataGridViewRecurringCoinSelector);
            commandtext = "SELECT * FROM recurringcoin WHERE idcol = @idcol";
            ClearParameters();
            AddParms("@idcol", currentidcol, "SQL");
            FillData(ssprocessds, "recurringcoin", commandtext, CommandType.Text);
            CurrentState = "View";
            ssprocessds.recurringcoin.AcceptChanges();
            parentForm.Update();
            RefreshControls();
        }
        public override void RefreshControls()
        {
            base.RefreshControls();
            switch (CurrentState)
            {

                case "Select":
                    {
                        parentForm.buttonInsert.Enabled = false;
                        break;
                    }

                case "StoreSelected":
                    {
                        parentForm.Text = commonAppDataMethods.GetStoreName(Storecode).TrimEnd();
                        parentForm.buttonInsert.Enabled = true;
                        dataGridViewRecurringCoinSelector.Enabled = true;
                        dataGridViewRecurringCoinSelector.Focus();
                        break;
                    }

                case "View":
                    {
                        parentForm.buttonEdit.Enabled = true;
                        dataGridViewRecurringCoinSelector.Enabled = true;
                        parentForm.buttonInsert.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonDelete.Enabled = true;
                        break;
                    }

                case "Edit":
                    {
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        EnableInputControls();
                        break;
                    }

                case "Insert":
                    {
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        EnableInputControls();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        public void RefreshDatagridview()
        {

            ssprocessselectords.view_expandedrecurringcoin.Rows.Clear();
            commandtext = "SELECT * FROM view_expandedrecurringcoin WHERE storecode = @storecode ORDER BY dayofweek";
            ClearParameters();
            AddParms("@storecode", Storecode, "SQL");
            FillData(ssprocessselectords, "view_expandedrecurringcoin", commandtext, CommandType.Text);

        }

        public void ClearCalculatedFields()
        {
        }
        public void EstablishDatagridViewRecurringCoinSelector(int TextTop, int LeftStart)
        {
            bindingSelectorData.DataSource = ssprocessselectords.view_expandedrecurringcoin;
            dataGridViewRecurringCoinSelector.DataSource = bindingSelectorData;
            DataGridViewTextBoxColumn ColumnDay = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnHundreds = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnFiftys = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnTwentys = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnTens = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnFives = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnSingles = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnQuarters = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnDimes = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnNickels = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnPennies = new DataGridViewTextBoxColumn();


            DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();



            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            // Column Day
            ColumnDay.DataPropertyName = "descrip";
            ColumnDay.HeaderText = "Day";
            ColumnDay.Name = "ColumnDay";
            ColumnDay.ReadOnly = true;
            ColumnDay.Width = 150;
            dataGridViewRecurringCoinSelector.Columns.Add(ColumnDay);
            dataGridViewRecurringCoinSelector.Width = ColumnDay.Width;

            // 
            SetDataGridviewCoinColumn(ColumnHundreds, "hundreds");
            SetDataGridviewCoinColumn(ColumnFiftys, "fiftys");
            SetDataGridviewCoinColumn(ColumnTwentys, "twentys");
            SetDataGridviewCoinColumn(ColumnTens, "tens");
            SetDataGridviewCoinColumn(ColumnFives, "fives");
            SetDataGridviewCoinColumn(ColumnSingles, "singles");
            SetDataGridviewCoinColumn(ColumnQuarters, "quarters");
            SetDataGridviewCoinColumn(ColumnDimes, "dimes");
            SetDataGridviewCoinColumn(ColumnNickels, "nickels");
            SetDataGridviewCoinColumn(ColumnPennies, "pennies");
            dataGridViewRecurringCoinSelector.Top = TextTop;
            dataGridViewRecurringCoinSelector.Left = LeftStart;
            dataGridViewRecurringCoinSelector.Height = 200;
            dataGridViewRecurringCoinSelector.EnableHeadersVisualStyles = false;
            dataGridViewRecurringCoinSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewRecurringCoinSelector.ReadOnly = true;
            dataGridViewRecurringCoinSelector.RowHeadersVisible = false;
            dataGridViewRecurringCoinSelector.AutoGenerateColumns = false;
            dataGridViewRecurringCoinSelector.AllowUserToAddRows = false;
            parentForm.Controls.Add(dataGridViewRecurringCoinSelector);

        }


        public void SetDataGridviewCoinColumn(DataGridViewTextBoxColumn dc, string dpname)
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            dc.DataPropertyName = dpname;
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dc.DefaultCellStyle = dataGridViewCellStyle2;
            dc.HeaderText = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(dpname);
            dc.ReadOnly = true;
            dc.Width = 75;
            dataGridViewRecurringCoinSelector.Columns.Add(dc);
            dataGridViewRecurringCoinSelector.Width += dc.Width;
        }


    }

}
