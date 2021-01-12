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
    public class MaintainSlipsMethods : FrmMaintainSingleTableMethods
    {
        VFPAcdataMethods vfpAcdataMethods = new VFPAcdataMethods();
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Slips");
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        public string Storecode = "";
        int tabIndex = 0;
        string commandtext = "";
        Button ButtonDriver = new Button();
        DateTimePicker dateTimePickerSlipdate = new DateTimePicker();
        TextBox TextBoxSlipday = new TextBox();
        TextBox TextBoxSlipdate = new TextBox();
        TextBox TextBoxNumdrops = new TextBox();
        Label LabelNumdrops = new Label();
        Label LabelDriver1 = new Label();
        TextBox TextBoxDriverName = new TextBox();
        TextBox TextBoxRate = new TextBox();
        ComboBox ComboBoxBillType = new ComboBox();
        Label LabelBillType = new Label();
        Label LabelSlipdate = new Label();
        DataGridView dataGridViewSlipSelector = new DataGridView();
        BindingSource bindingSelectorData = new BindingSource();

        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.salesperson;
            currentablename = "slips";
            SetIdcol(ssprocessds.slips.idcolColumn);
            parentForm.Text = "Maintain Slips Data";

        }
        public override void SetControls()
        {

            parentForm.buttonDelete.Visible = true;
            ssprocessds.chargetype.Rows.Clear();
            commandtext = "SELECT * FROM chargetype ORDER BY chgdesc";
            FillData(ssprocessds, "chargetype", commandtext, CommandType.Text);
            int TextTop = 100;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 600;
            SetDollarsTextBox(TextBoxNumdrops, LeftStart, TextTop, 75, ssprocessds.slips, "num_drops", LabelNumdrops, "# of drops");
            // Override defaults
            LabelNumdrops.Top -= 5;
            TextBoxNumdrops.Top = LabelNumdrops.Top;
            TextBoxNumdrops.Left = LabelNumdrops.Left + 100;
            ButtonDriver.Height = parentForm.buttonSave.Height - 3;
            ButtonDriver.Width = 75;
            ButtonDriver.Top = TextTop;
            ButtonDriver.Left = LeftStart;
            ButtonDriver.Text = "Driver";
            parentForm.Controls.Add(ButtonDriver);
            parentForm.Controls.Add(TextBoxDriverName);
            TextBoxDriverName.Left = LeftStart + 100;
            TextBoxDriverName.Top = TextTop;
            TextBoxDriverName.Width = 250;
            TextBoxDriverName.ReadOnly = true;
            TextTop += 22;

            SetBoundComboBox(ComboBoxBillType, LeftStart, TextTop, 150, LabelBillType, "Slip Type", "chgcode", "chgdesc", ssprocessds.chargetype);
            LabelBillType.Font = new Font(LabelBillType.Font, FontStyle.Bold);
            ComboBoxBillType.Left = LeftStart + 100;
            TextTop += 22;

            LabelSlipdate.Text = "Slip Date";
            LabelSlipdate.Top = TextTop;
            LabelSlipdate.Width = 70;
            LabelSlipdate.Left = LeftStart;
            LabelSlipdate.Font = new Font(LabelSlipdate.Font, FontStyle.Bold);
            parentForm.Controls.Add(LabelSlipdate);
            dateTimePickerSlipdate.CustomFormat = "";
            dateTimePickerSlipdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dateTimePickerSlipdate.Top = TextTop;
            dateTimePickerSlipdate.Left = LeftStart + 100;
            dateTimePickerSlipdate.Name = "dateTimePickerSlipdate";
            dateTimePickerSlipdate.Size = new System.Drawing.Size(97, 20);
            dateTimePickerSlipdate.MinDate = DateTime.Now.Date.AddDays(-500);
            dateTimePickerSlipdate.TabIndex = tabIndex;
            dateTimePickerSlipdate.MaxDate = DateTime.Now.Date.AddDays(90);
            dateTimePickerSlipdate.DataBindings.Add("Value", ssprocessds, "slips.slip_date");
            parentForm.Controls.Add(dateTimePickerSlipdate);
            EstablishDatagridViewSlips(TextTop + 25, LeftStart);
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            Storecode = commonAppDataMethods.SelectCompanyAndStore();

            // If a row has been selected fill the data and process
            if (Storecode.TrimEnd().Length > 0)
            {
                parentForm.Text = "Processing slips for " + commonAppDataMethods.GetStoreName(Storecode);
                RefreshDatagridview();
                CurrentState = "StoreSelected";
                RefreshControls();
            }

        }
        public override void SetEvents()
        {
            base.SetEvents();
            ButtonDriver.Click += new System.EventHandler(ButtonDriver_click);
            dataGridViewSlipSelector.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(CaptureDoubleClick);
            dataGridViewSlipSelector.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectorKeyDown);

        }
        public override void SetEditState(object sender, EventArgs e)
        {
            string editstatus = base.LockTableRow(ssprocessds.slips[0].idcol, "slips");
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
            LoadCurrentSlip();
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            ClearCalculatedFields();
            EstablishBlankDataTableRow(ssprocessds.slips);
            ssprocessds.slips[0].storecode = Storecode;
            ssprocessds.slips[0].num_drops = 1;
            ComboBoxBillType.SelectedValue = "REGDRP";
            CurrentState = "Insert";
            RefreshControls();
        }
        private void SelectorKeyDown(object sender, KeyEventArgs e)
        {

            // Use incremental search
            int ix = 0;
            if (e.KeyCode == Keys.Return)
            {
                LoadCurrentSlip();
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
                    ssprocessds.slips.Rows.Clear();

                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
            }
            else
            {
                ClearCalculatedFields();
                if (CurrentState == "View")
                {
                    ssprocessds.slips.Rows.Clear();
                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
                else
                {
                    ssprocessds.view_expandedslip.Rows.Clear();
                    CurrentState = "Select";
                    RefreshControls();

                }
            }
        }

        public void ButtonDriver_click(object sender, EventArgs e)
        {
            string selecteddriver = commonAppDataMethods.SelectDriver();
            if (selecteddriver.Length > 0)
            {
                ssprocessds.slips[0].driver_1 = selecteddriver;
            }
            else
            {
                ssprocessds.slips[0].driver_1 = "";
            }
            RefreshDriverName(ssprocessds.slips[0].driver_1);
            parentForm.Update();
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (cont)
            {
                ssprocessds.slips[0].slip_date = dateTimePickerSlipdate.Value;
                ssprocessds.slips[0].chgcode = ComboBoxBillType.SelectedValue.ToString();
                GenerateAppTableRowSave(ssprocessds.slips[0]);
                RefreshDatagridview();
                ClearCalculatedFields();
                ssprocessds.slips.Rows.Clear();
                CurrentState = "StoreSelected";
                RefreshControls();
            }
        }

        public override void ProcessDelete(object sender, EventArgs e)
        {
            base.ProcessDelete(sender, e);
            ssprocessds.slips.Rows.Clear();
            ClearCalculatedFields();
            RefreshDatagridview();
            CurrentState = "View";
            RefreshControls();
            parentForm.Update();
        }
        public void LoadCurrentSlip()
        {
            ClearCalculatedFields();
            ssprocessds.slips.Rows.Clear();
            currentidcol = base.CaptureIdCol(dataGridViewSlipSelector);
            commandtext = "SELECT * FROM slips WHERE idcol = @idcol";
            ClearParameters();
            AddParms("@idcol", currentidcol, "SQL");
            FillData(ssprocessds, "slips", commandtext, CommandType.Text);
            dateTimePickerSlipdate.Value = ssprocessds.slips[0].slip_date;
            ComboBoxBillType.SelectedValue = ssprocessds.slips[0].chgcode;
            RefreshDriverName(ssprocessds.slips[0].driver_1);
            CurrentState = "View";
            currentidcol = ssprocessds.slips[0].idcol;
            ssprocessds.slips.AcceptChanges();
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
                        dataGridViewSlipSelector.Enabled = true;
                        dataGridViewSlipSelector.Focus();
                        break;
                    }

                case "View":
                    {
                        parentForm.buttonEdit.Enabled = true;
                        dataGridViewSlipSelector.Enabled = true;
                        parentForm.buttonInsert.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonDelete.Enabled = true;
                        break;
                    }

                case "Edit":
                    {
                        TextBoxNumdrops.ReadOnly = false;
                        TextBoxNumdrops.Enabled = true;
                        ButtonDriver.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        ComboBoxBillType.Enabled = true;
                        dateTimePickerSlipdate.Enabled = true;
                        break;
                    }

                case "Insert":
                    {
                        TextBoxNumdrops.ReadOnly = false;
                        TextBoxNumdrops.Enabled = true;
                        ButtonDriver.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        ComboBoxBillType.Enabled = true;
                        dateTimePickerSlipdate.Enabled = true;
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
            ssprocessds.view_expandedslip.Rows.Clear();
            commandtext = "SELECT * FROM view_expandedslip WHERE storecode = @storecode ORDER BY slip_date";
            ClearParameters();
            AddParms("@storecode", Storecode, "SQL");
            FillData(ssprocessds, "view_expandedslip", commandtext, CommandType.Text);

        }

        public void ClearCalculatedFields()
        {
            TextBoxDriverName.Text = "";
            ComboBoxBillType.SelectedIndex = 1;
            dateTimePickerSlipdate.Value = DateTime.Now.Date;
        }
        public void RefreshDriverName(string driver)
        {
            TextBoxDriverName.Text = commonAppDataMethods.GetDriverName(driver);
        }
        public void EstablishDatagridViewSlips(int TextTop, int LeftStart)
        {
            bindingSelectorData.DataSource = ssprocessds.view_expandedslip;
            dataGridViewSlipSelector.DataSource = bindingSelectorData;
            DataGridViewTextBoxColumn ColumnDay = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnNumDrops = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnChgCode = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnDriver = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnDrivername = new DataGridViewTextBoxColumn();

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

            ColumnDay.DataPropertyName = "slip_date";
            ColumnDay.HeaderText = "SlipDate";
            ColumnDay.Name = "ColumnDay";
            ColumnDay.ReadOnly = true;
            ColumnDay.Width = 75;
            // 
            // Column ChgCode
            ColumnChgCode.DataPropertyName = "chgcode";
            ColumnChgCode.HeaderText = "Type";
            ColumnChgCode.Name = "ColumnChgCode";
            ColumnChgCode.ReadOnly = true;
            ColumnChgCode.Width = 75;
            // 

            // ColumnNumDrops
            // 
            ColumnNumDrops.DataPropertyName = "num_drops";
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            ColumnNumDrops.DefaultCellStyle = dataGridViewCellStyle2;
            ColumnNumDrops.HeaderText = "No.";
            ColumnNumDrops.Name = "ColumnNumDrops";
            ColumnNumDrops.ReadOnly = true;
            ColumnNumDrops.Width = 50;
            // 
            // 
            // ColumnDriver
            // 
            ColumnDriver.DataPropertyName = "driver_1";
            ColumnDriver.HeaderText = "Driver";
            ColumnDriver.Name = "ColumnDriver";
            ColumnDriver.ReadOnly = true;
            ColumnDriver.Width = 75;
            // 
            // ColumnDrivername
            // 

            ColumnDrivername.DataPropertyName = "drivername";
            ColumnDrivername.HeaderText = "Driver Name";
            ColumnDrivername.Name = "ColumnDrivername";
            ColumnDrivername.ReadOnly = true;
            ColumnDrivername.Width = 200;
            // 
            // 
            dataGridViewSlipSelector.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            ColumnDay,
            ColumnChgCode,
            ColumnNumDrops,
            ColumnDriver,
            ColumnDrivername,
            });
            dataGridViewSlipSelector.Top = TextTop;
            dataGridViewSlipSelector.Width = 475;
            dataGridViewSlipSelector.Left = LeftStart;
            dataGridViewSlipSelector.Height = 300;

            dataGridViewSlipSelector.EnableHeadersVisualStyles = false;
            dataGridViewSlipSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewSlipSelector.Name = "dataGridViewSlipSelector";
            dataGridViewSlipSelector.ReadOnly = true;
            dataGridViewSlipSelector.RowHeadersVisible = false;
            dataGridViewSlipSelector.AutoGenerateColumns = false;
            dataGridViewSlipSelector.AllowUserToAddRows = false;
            parentForm.Controls.Add(dataGridViewSlipSelector);

        }

    }
}
