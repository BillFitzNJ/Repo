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
    public class MaintainRecurringChargeMethods : FrmMaintainSingleTableMethods
    {
        VFPAcdataMethods vfpAcdataMethods = new VFPAcdataMethods();
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Recurring Charges");
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        public string Storecode = "";
        Label labelFrequencyDescription = new Label();
        int tabIndex = 0;
        string commandtext = "";
        NumericUpDown numericUpDownChgAmt = new NumericUpDown();
        TextBox TextBoxChgAmt = new TextBox();
        Label LabelChgAmt = new Label();
        TextBox TexBoxChgfreq = new TextBox();
        ComboBox ComboBoxChargeType = new ComboBox();
        Label LabelChargeType = new Label();
        DataGridView dataGridViewRecurringChargeSelector = new DataGridView();
        BindingSource bindingSelectorData = new BindingSource();

        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.salesperson;
            currentablename = "recurringcharge";
            SetIdcol(ssprocessds.recurringcharge.idcolColumn);
            parentForm.Text = "Maintain Recurring Charges";

        }
        public override void SetControls()
        {
            ssprocessds.chargetype.Rows.Clear();
            commandtext = "SELECT * FROM chargetype ORDER by chgdesc";
            ClearParameters();
            
            FillData(ssprocessds, "chargetype", commandtext, CommandType.Text);
         
            
            ComboBoxChargeType.DisplayMember = "chgdesc";
            ComboBoxChargeType.ValueMember = "chgcode";

            parentForm.buttonDelete.Visible = true;
            int TextTop = 100;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 600;
            SetNumericUpDown(numericUpDownChgAmt, LeftStart, TextTop, 75, ssprocessds.recurringcharge,  "chgamt", LabelChgAmt, "Charge Amount");
            numericUpDownChgAmt.Maximum = 5000;
            numericUpDownChgAmt.DecimalPlaces = 2;  
            TextTop += 22;
            SetTextBoxAndLabelText(TexBoxChgfreq, LeftStart, TextTop, 15, ssprocessds.recurringcharge, "chgfreq", "Frequency");
            TexBoxChgfreq.Left = LeftStart + 100;
            TexBoxChgfreq.MaxLength = 5;
            labelFrequencyDescription.Text = "1 = once per month. 2 = once per day";
            labelFrequencyDescription.Font = new Font(labelFrequencyDescription.Font, FontStyle.Bold);
            labelFrequencyDescription.Left = LeftStart + 200;
            labelFrequencyDescription.Top = TextTop;
            labelFrequencyDescription.AutoSize = true;
            parentForm.Controls.Add(labelFrequencyDescription);
            TextTop += 22;
           
            SetBoundComboBox(ComboBoxChargeType, LeftStart, TextTop, 150, LabelChargeType, "Charge Type", "chgcode", "chgdesc" , ssprocessds.chargetype);
            LabelChargeType.Font = new Font(LabelChargeType.Font, FontStyle.Bold);
            ComboBoxChargeType.Left = LeftStart + 100;
            numericUpDownChgAmt.Left = ComboBoxChargeType.Left;
            TextTop += 22;
            EstablishDatagridViewRecurringChargeSelector(TextTop + 25, LeftStart);
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (cont)
            {
                ssprocessds.recurringcharge[0].chgtype = ComboBoxChargeType.SelectedValue.ToString();
                GenerateAppTableRowSave(ssprocessds.recurringcharge[0]);
                RefreshDatagridview();
                ClearCalculatedFields();
                ssprocessds.recurringcharge.Rows.Clear();
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
                parentForm.Text = "Processing recurring charges  for " + commonAppDataMethods.GetStoreName(Storecode);
                RefreshDatagridview();
                CurrentState = "StoreSelected";
                RefreshControls();
            }

        }
        public override void SetEvents()
        {
            base.SetEvents();
            dataGridViewRecurringChargeSelector.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(CaptureDoubleClick);
            dataGridViewRecurringChargeSelector.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectorKeyDown);

        }
        public override void SetEditState(object sender, EventArgs e)
        {
            string editstatus = base.LockTableRow(ssprocessds.recurringcharge[0].idcol, "recurringcharge");
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
            LoadCurrentRecurringCharge();
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            ClearCalculatedFields();
            EstablishBlankDataTableRow(ssprocessds.recurringcharge);
            ssprocessds.recurringcharge[0].storecode = Storecode;
            ssprocessds.recurringcharge[0].chgfreq = "1";
            ssprocessds.recurringcharge[0].taxable = "Y";
            ssprocessds.recurringcharge[0].chargeno = "1";
            CurrentState = "Insert";
            RefreshControls();
        }
        private void SelectorKeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Return)
            {
                LoadCurrentRecurringCharge();
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
                    ssprocessds.recurringcharge.Rows.Clear();
                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
                else
                {
                    ssprocessselectords.recurringcharge.Rows.Clear();
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
            ssprocessds.recurringcharge.Rows.Clear();
            ClearCalculatedFields();
            parentForm.Update();
        }
        public void LoadCurrentRecurringCharge()
        {
            ClearCalculatedFields();
            ssprocessds.recurringcharge.Rows.Clear();
            currentidcol = base.CaptureIdCol(dataGridViewRecurringChargeSelector);
            commandtext = "SELECT * FROM recurringcharge WHERE idcol = @idcol";
            ClearParameters();
            AddParms("@idcol", currentidcol, "SQL");
            FillData(ssprocessds, "recurringcharge", commandtext, CommandType.Text);
            ComboBoxChargeType.SelectedValue = ssprocessds.recurringcharge[0].chgtype;
            CurrentState = "View";
            ssprocessds.recurringcharge.AcceptChanges();
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
                        ComboBoxChargeType.Visible = false;
                        parentForm.buttonInsert.Enabled = false;
                        break;
                    }

                case "StoreSelected":
                    {
                        parentForm.Text = commonAppDataMethods.GetStoreName(Storecode).TrimEnd();
                        ComboBoxChargeType.Visible = false;
                        parentForm.buttonInsert.Enabled = true;
                        TexBoxChgfreq.Visible = true;
                        dataGridViewRecurringChargeSelector.Enabled = true;
                        dataGridViewRecurringChargeSelector.Focus();
                        break;
                    }

                case "View":
                    {
                        TexBoxChgfreq.Visible = true;
                        ComboBoxChargeType.Visible = true;
                        parentForm.buttonEdit.Enabled = true;
                        dataGridViewRecurringChargeSelector.Enabled = true;
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

            ssprocessselectords.recurringcharge.Rows.Clear();
            commandtext = "SELECT * FROM recurringcharge WHERE storecode = @storecode ORDER BY chargeno";
            ClearParameters();
            AddParms("@storecode", Storecode, "SQL");
            FillData(ssprocessselectords, "recurringcharge", commandtext, CommandType.Text);

        }

        public void ClearCalculatedFields()
        {
            ComboBoxChargeType.SelectedIndex = 1;
        }
        public void EstablishDatagridViewRecurringChargeSelector(int TextTop, int LeftStart)
        {
            bindingSelectorData.DataSource = ssprocessselectords.recurringcharge;
            dataGridViewRecurringChargeSelector.DataSource = bindingSelectorData;
            DataGridViewTextBoxColumn ColumnChgType = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnChgAmt = new DataGridViewTextBoxColumn();

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

            // Column ChgType
            ColumnChgType.DataPropertyName = "chgtype";
            ColumnChgType.HeaderText = "Type";
            ColumnChgType.Name = "ColumnChgType";
            ColumnChgType.ReadOnly = true;
            ColumnChgType.Width = 150;
            // 

            // ColumnChgAmt
            // 
            ColumnChgAmt.DataPropertyName = "chgamt";
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            ColumnChgAmt.DefaultCellStyle = dataGridViewCellStyle2;
            ColumnChgAmt.HeaderText = "Amt";
            ColumnChgAmt.Name = "ColumnChgAmt";
            ColumnChgAmt.ReadOnly = true;
            ColumnChgAmt.Width = 75;
            // 
            dataGridViewRecurringChargeSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            ColumnChgType,
            ColumnChgAmt,
            });
            dataGridViewRecurringChargeSelector.Top = TextTop;
            dataGridViewRecurringChargeSelector.Width = ColumnChgAmt.Width + ColumnChgType.Width;
            dataGridViewRecurringChargeSelector.Left = LeftStart;
            dataGridViewRecurringChargeSelector.Height = 200;

            dataGridViewRecurringChargeSelector.EnableHeadersVisualStyles = false;
            dataGridViewRecurringChargeSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewRecurringChargeSelector.ReadOnly = true;
            dataGridViewRecurringChargeSelector.RowHeadersVisible = false;
            dataGridViewRecurringChargeSelector.AutoGenerateColumns = false;
            dataGridViewRecurringChargeSelector.AllowUserToAddRows = false;
            parentForm.Controls.Add(dataGridViewRecurringChargeSelector);

        }

   
    }

}
