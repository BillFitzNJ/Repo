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
    public class MaintainDropScheduleMethods : WSGDataAccess
    {
        MySQLSynchMethods mySQLSynchMethods = new MySQLSynchMethods();
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Drop Schedule");
        string commandtext = "";

        public int currentidcol { get; set; }
        public Form menuform { get; set; }
        public string currentablename { get; set; }
        public DataTable currentdatatable = new DataTable();
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        public string CurrentState = "Select";
        int tabIndex = 0;
        // Textboxes
 //       WSGTextBoxMethods wSGTextBoxMethods = new WSGTextBoxMethods();
        TextBox TextBoxNumdrops = new  TextBox();
        Label LabelNumdrops = new Label();
        TextBox TextBoxCommtype = new TextBox();
        Label LabelCommtype = new Label();
        TextBox TextBoxCommission= new TextBox();
        Label LabelCommission = new Label();
        TextBox TextBoxDriver = new TextBox();
        Label LabelDriver = new Label();
        // Buttons  
        Button ButtonDriver = new Button();
        Button ButtonMon = new Button();
        Button ButtonTue = new Button();
        Button ButtonSun = new Button();
        Button ButtonWed = new Button();
        Button ButtonThu = new Button();
        Button ButtonFri = new Button();
        Button ButtonSat = new Button();
        Button ButtonReplicate = new Button();
        // Data grid view
        DataGridView dataGridViewDrops = new DataGridView();

        public   FrmMaintainDropSchedule parentForm = new FrmMaintainDropSchedule();
        DateTimePicker DateTimePickerCfrom = new DateTimePicker();
        DateTimePicker DateTimePickerCthru = new DateTimePicker();
        
        Label LabelCfrom = new Label();
        Label LabelCthru = new Label();
        ComboBox ComboBoxScheduleday = new ComboBox();
        Label LabelScheduleday = new Label();
        public string StoreCode = "";
        public int SelectedIdcol = 0;
        public MaintainDropScheduleMethods()
            : base("SQL", "SQLConnString")
        {
            EstablishAppConstants();
            SetControls();
            SetEvents();
            RefreshControls();
        }

        public void EstablishAppConstants()
        {
            currentdatatable = ssprocessds.dropschedule;
            currentablename = "dropschedule";
            SetIdcol(ssprocessds.dropschedule.idcolColumn);
            parentForm.Text = "Maintain Drop Schedule";
            BindingSource bindingSelectorData = new BindingSource();
            bindingSelectorData.DataSource = ssprocessds.view_expandeddropschedule;
            dataGridViewDrops.AutoGenerateColumns = false;
            dataGridViewDrops.AllowUserToAddRows = false;
            dataGridViewDrops.DataSource = bindingSelectorData;
            dataGridViewDrops.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewDrops.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
            // Fill the scheduleday view
            ssprocessds.scheduleday.Rows.Clear();
            commandtext = "SELECT * FROM scheduleday ORDER by  descrip";
            ClearParameters();
            FillData(ssprocessds, "scheduleday", commandtext, CommandType.Text);

            ComboBoxScheduleday.DataSource = ssprocessds.scheduleday;
            ComboBoxScheduleday.ValueMember = "schday";
            ComboBoxScheduleday.DisplayMember = "descrip";
            CurrentState = "Select";
            RefreshControls();
        }
        public void ProcessSelect(object sender, EventArgs e)
        {
            StoreCode = commonAppDataMethods.SelectCompanyAndStore();

            // If a row has been selected fill the data and process
            if (StoreCode.TrimEnd().Length > 0)
            {
                RefreshDatagridview();
                CurrentState = "StoreSelected";
                RefreshControls();
            }
        }

        public void SetInsertState(object sender, EventArgs e)
        {
            ssprocessds.dropschedule.Rows.Clear();
            EstablishBlankDataTableRow(ssprocessds.dropschedule);
            ssprocessds.dropschedule[0].storecode = StoreCode;
            ssprocessds.dropschedule[0].commtype = "P";
            ssprocessds.dropschedule[0].num_drops = 1;
            ssprocessds.dropschedule[0].cfrom = Convert.ToDateTime("01/01/1900");
            ssprocessds.dropschedule[0].cthru = Convert.ToDateTime("12/31/3099");
            ssprocessds.dropschedule.AcceptChanges();
            DateTimePickerCfrom.Value = ssprocessds.dropschedule[0].cfrom;
            DateTimePickerCthru.Value = ssprocessds.dropschedule[0].cthru;
            parentForm.Update();
            CurrentState = "Insert";
            RefreshControls();
        }

        public void SetControls()
        {
            parentForm.Height = 600;
            int TextTop = 75;
            int LeftStart = 50;
            SetBoundComboBox(ComboBoxScheduleday, LeftStart, TextTop, 150, LabelScheduleday, "Drop Day", "schday", "descrip", ssprocessds.scheduleday);
            TextTop += 22;
          
            ButtonDriver.Height = parentForm.buttonSave.Height - 3;
            ButtonDriver.Width = 75;
            ButtonDriver.Top = TextTop;
            ButtonDriver.Left = LeftStart ;
            ButtonDriver.Text = "Driver";
            ButtonDriver.TabIndex = tabIndex;
            SetTextBox(TextBoxDriver, LeftStart -40, TextTop +2, 25, ssprocessds.dropschedule, "driver_1", LabelDriver, "");
            SetDollarsTextBox(TextBoxNumdrops, LeftStart+ 125 , TextTop, 25, ssprocessds.dropschedule, "num_drops", LabelNumdrops, "Num Drops");
            TextTop += 22;
 
            SetTextBox(TextBoxCommtype, LeftStart,  TextTop , 15, ssprocessds.dropschedule, "commtype", LabelCommtype, "Comm Type");
            TextBoxCommtype.Left = TextBoxDriver.Left;
            SetDollarsTextBox(TextBoxCommission, LeftStart + 125, TextTop, 25, ssprocessds.dropschedule, "commission", LabelCommission, "Commission");
         
            tabIndex += 1;
            parentForm.Controls.Add(ButtonDriver);
            TextBoxDriver.ReadOnly = true;
            LabelDriver.Visible = false;


            TextTop += 32;

            // dateTimePickerCfrom
            // 
            LabelCfrom.Text = "Temp From";
            LabelCfrom.Top = TextTop;
            LabelCfrom.Width = 70;
            LabelCfrom.Left = LeftStart;
            parentForm.Controls.Add(LabelCfrom);
            this.DateTimePickerCfrom.CustomFormat = "";
            this.DateTimePickerCfrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateTimePickerCfrom.Top = TextTop;
            this.DateTimePickerCfrom.Left = LeftStart + 75;
            this.DateTimePickerCfrom.Name = "dateTimePickerCfrom";
            this.DateTimePickerCfrom.Size = new System.Drawing.Size(97, 20);
            DateTimePickerCfrom.MinDate = DateTime.MinValue;
            DateTimePickerCfrom.TabIndex = tabIndex;
            tabIndex += 1;
            DateTimePickerCfrom.MaxDate = DateTime.MaxValue;
            DateTimePickerCfrom.DataBindings.Add("Value", ssprocessds, "dropschedule.cfrom");
            parentForm.Controls.Add(DateTimePickerCfrom);
            // 
            // dateTimePickerCthru
            // 
            LabelCthru.Text = "Temp Thru";
            LabelCthru.Top = TextTop;
            LabelCthru.Width = 70;
            LabelCthru.Left = LeftStart + 250;
            parentForm.Controls.Add(LabelCthru);
            this.DateTimePickerCthru.CustomFormat = "";
            this.DateTimePickerCthru.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateTimePickerCthru.Top = TextTop;
            this.DateTimePickerCthru.Left = LeftStart + 350;
            this.DateTimePickerCthru.Name = "dateTimePickerCthru";
            this.DateTimePickerCthru.Size = new System.Drawing.Size(103, 20);
            DateTimePickerCthru.TabIndex = tabIndex;
            tabIndex += 1;
            DateTimePickerCthru.MinDate = DateTime.Now;
            DateTimePickerCthru.MaxDate = DateTime.MaxValue;

            DateTimePickerCthru.DataBindings.Add("Value", ssprocessds, "dropschedule.cthru");
            // 
            parentForm.Controls.Add(DateTimePickerCthru);

            TextTop += 32;


            // Day Buttons
            ButtonSun.Height = parentForm.buttonSave.Height;
            ButtonSun.Width = 50;
            ButtonSun.Top = TextTop;
            ButtonSun.Left = LeftStart;
            ButtonSun.Text = "Sun";
            parentForm.Controls.Add(ButtonSun);
            ButtonMon.Height = parentForm.buttonSave.Height;
            ButtonMon.Width = 50;
            ButtonMon.Top = TextTop;
            ButtonMon.Left = LeftStart + 55;
            ButtonMon.Text = "Mon";
            parentForm.Controls.Add(ButtonMon);
            ButtonTue.Height = parentForm.buttonSave.Height;
            ButtonTue.Width = 50;
            ButtonTue.Top = TextTop;
            ButtonTue.Left = LeftStart + 110;
            ButtonTue.Text = "Tue";
            parentForm.Controls.Add(ButtonTue);
            ButtonWed.Height = parentForm.buttonSave.Height;
            ButtonWed.Width = 50;
            ButtonWed.Top = TextTop;
            ButtonWed.Left = LeftStart + 165;
            ButtonWed.Text = "Wed";
            parentForm.Controls.Add(ButtonWed);
            ButtonThu.Height = parentForm.buttonSave.Height;
            ButtonThu.Width = 50;
            ButtonThu.Top = TextTop;
            ButtonThu.Left = LeftStart + 220;
            ButtonThu.Text = "Thu";
            parentForm.Controls.Add(ButtonThu);
            ButtonFri.Height = parentForm.buttonSave.Height;
            ButtonFri.Width = 50;
            ButtonFri.Top = TextTop;
            ButtonFri.Left = LeftStart + 275;
            ButtonFri.Text = "Fri";
            parentForm.Controls.Add(ButtonFri);
            ButtonSat.Height = parentForm.buttonSave.Height;
            ButtonSat.Width = 50;
            ButtonSat.Top = TextTop;
            ButtonSat.Left = LeftStart + 330;
            ButtonSat.Text = "Sat";
            parentForm.Controls.Add(ButtonSat);

            ButtonReplicate.Height = parentForm.buttonSave.Height;
            ButtonReplicate.Width = 85;
            ButtonReplicate.Top = TextTop;
            ButtonReplicate.Left = LeftStart + 400;
            ButtonReplicate.Text = "Replicate";
            parentForm.Controls.Add(ButtonReplicate);

            parentForm.Width = ButtonReplicate.Left + ButtonReplicate.Width + 75;

            TextTop += 40;
            EstablishDatagridViewDrops(TextTop, LeftStart);

            // Set textboxes to upper casing  
            foreach (Control c in parentForm.Controls)
            {
                if (c is TextBox)
                {
                    (c as TextBox).CharacterCasing = CharacterCasing.Upper;

                }
                // Set labels to bold
                if (c is Label)
                {

                    (c as Label).Font = new Font(c.Font, FontStyle.Bold);
                }
            }
        }
        public void ShowParent()
        {
            parentForm.MdiParent = menuform;
            parentForm.Update();
            parentForm.Show();
        }
        public void RefreshDatagridview()
        {
            ssprocessds.view_expandeddropschedule.Rows.Clear();
            commandtext = "SELECT * FROM view_expandeddropschedule WHERE storecode = @storecode ORDER BY schday";
            ClearParameters();
            AddParms("@storecode", StoreCode, "SQL");
            FillData(ssprocessds, "view_expandeddropschedule", commandtext, CommandType.Text);

        }
        public void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (cont)
            {
                ssprocessds.dropschedule[0].cfrom = DateTimePickerCfrom.Value;
                ssprocessds.dropschedule[0].cthru = DateTimePickerCthru.Value;
                ssprocessds.dropschedule[0].schday = Convert.ToString(ComboBoxScheduleday.SelectedValue);

                if (Convert.ToInt32(ssprocessds.dropschedule[0].schday) < 8)
                { 
                   ssprocessds.dropschedule[0].daynumber = Convert.ToInt32(ssprocessds.dropschedule[0].schday);
                }
                else
                {
                    ssprocessds.dropschedule[0].daynumber = 9;
                }
                
                ssprocessds.dropschedule.AcceptChanges();
                GenerateAppTableRowSave(ssprocessds.dropschedule[0]);
                // Refresh MySQL tables
            mySQLSynchMethods.updatemysqldropschedule(ssprocessds.dropschedule[0].storecode);
                RefreshDatagridview();
                ssprocessds.dropschedule.Rows.Clear();
                CurrentState = "StoreSelected";
                RefreshControls();
            }

        }

        public void ProcessSunday(object sender, EventArgs e)
        {
            AddDailyDrop("001");
        }

        public void ProcessMonday(object sender, EventArgs e)
        {
            AddDailyDrop("002");
        }

        public void ProcessTuesday(object sender, EventArgs e)
        {
            AddDailyDrop("003");
        }
        public void ProcessWednesday(object sender, EventArgs e)
        {
            AddDailyDrop("004");
        }
        public void ProcessThursday(object sender, EventArgs e)
        {
            AddDailyDrop("005");
        }
        public void ProcessFriday(object sender, EventArgs e)
        {
            AddDailyDrop("006");
        }
        public void ProcessSaturday(object sender, EventArgs e)
        {
            AddDailyDrop("007");
        }
        public void ProcessDelete(object sender, EventArgs e)
        {

            if (wsgUtilities.wsgReply("Delete this item?"))
            {
                DeleteTablerow(currentablename, currentidcol);
                RefreshDatagridview();
                CurrentState = "StoreSelected";
                RefreshControls();
            }
            else
            {
                wsgUtilities.wsgNotice("Deletion Cancelled");
            }
        }
        public void CaptureDropIdcol()
        {
            currentidcol = CaptureIdCol(dataGridViewDrops);
            ssprocessds.dropschedule.Rows.Clear();
            commandtext = "SELECT * FROM dropschedule WHERE idcol = @idcol";
            ClearParameters();
            AddParms("@idcol", currentidcol, "SQL");

            FillData(ssprocessds, "dropschedule", commandtext, CommandType.Text);
            if (ssprocessds.dropschedule[0].cfrom >= DateTimePickerCfrom.MinDate)
            {
                DateTimePickerCfrom.Value = ssprocessds.dropschedule[0].cfrom;
            }
            else
            {
                DateTimePickerCfrom.Value = DateTimePickerCfrom.MinDate;
            }

            if (ssprocessds.dropschedule[0].cthru >= DateTimePickerCthru.MinDate)
            {
                DateTimePickerCthru.Value = ssprocessds.dropschedule[0].cthru;
            }
            else
            {
                DateTimePickerCthru.Value = DateTimePickerCthru.MinDate;
            }
               
            ComboBoxScheduleday.SelectedValue = ssprocessds.dropschedule[0].schday;
            CurrentState = "View";
            parentForm.Update();
            RefreshControls();

        }
        private void CaptureDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            CaptureDropIdcol();
        }
        private void SelectorKeyDown(object sender, KeyEventArgs e)
        {

            // Use incremental search
            int ix = 0;
            if (e.KeyCode == Keys.Return)
            {
                CaptureDropIdcol();
            }
        }
        public virtual void SetEvents()
        {
            parentForm.buttonEdit.Click += new System.EventHandler(SetEditState);
            parentForm.buttonInsert.Click += new System.EventHandler(SetInsertState);
            parentForm.buttonSave.Click += new System.EventHandler(SaveCurrentDataTable);
            parentForm.buttonClose.Click += new System.EventHandler(CloseparentForm);
            parentForm.buttonDelete.Click += new System.EventHandler(ProcessDelete);
            parentForm.buttonSelect.Click += new System.EventHandler(ProcessSelect);
            parentForm.buttonCancel.Click += new System.EventHandler(ProcessCancel);
            ButtonReplicate.Click += new System.EventHandler(ProcessReplicate);
            ButtonDriver.Click += new System.EventHandler(ButtonDriver_click);
            dataGridViewDrops.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(CaptureDoubleClick);
            dataGridViewDrops.KeyDown += new System.Windows.Forms.KeyEventHandler(SelectorKeyDown);
            ButtonSun.Click += new System.EventHandler(ProcessSunday);
            ButtonMon.Click += new System.EventHandler(ProcessMonday);
            ButtonTue.Click += new System.EventHandler(ProcessTuesday);
            ButtonWed.Click += new System.EventHandler(ProcessWednesday);
            ButtonThu.Click += new System.EventHandler(ProcessThursday);
            ButtonFri.Click += new System.EventHandler(ProcessFriday);
            ButtonSat.Click += new System.EventHandler(ProcessSaturday);
        }

        public void ButtonDriver_click(object sender, EventArgs e)
        {
            string selecteddriver = commonAppDataMethods.SelectDriver();
            if (selecteddriver.Length > 0)
            {
                ssprocessds.dropschedule[0].driver_1 = selecteddriver;
            }
            else
            {
                ssprocessds.dropschedule[0].driver_1 = "";
            }
            parentForm.Update();
        }
        public void AddDailyDrop(string schday)
        {
            ssprocessds.dropschedule.Rows.Clear();
            EstablishBlankDataTableRow(ssprocessds.dropschedule);
            ssprocessds.dropschedule[0].storecode = StoreCode;
            ssprocessds.dropschedule[0].schday = schday;
            if (Convert.ToDecimal(schday) < 8)
            {
               ssprocessds.dropschedule[0].daynumber = Convert.ToDecimal(schday); 
            }
            else
            { 
              ssprocessds.dropschedule[0].daynumber = 9;
            }
            ssprocessds.dropschedule[0].num_drops = 1;
            ssprocessds.dropschedule[0].commtype = "P";
            GenerateAppTableRowSave(ssprocessds.dropschedule[0]);
            RefreshDatagridview();
        }
        public  void ProcessReplicate(object sender, EventArgs e)
        {
            if (wsgUtilities.wsgReply("Replicate this drop?"))
            {

                ClearParameters();
                AddParms("@storecode", StoreCode, "SQL");
                AddParms("@driver_1", ssprocessds.dropschedule[0].driver_1, "SQL");
                AddParms("@commission", ssprocessds.dropschedule[0].commission, "SQL");
                AddParms("@commtype", ssprocessds.dropschedule[0].commtype, "SQL");
                commandtext = "UPDATE dropschedule SET driver_1 = @driver_1, commission = @commission, commtype = @commtype WHERE storecode = @storecode ";
                ExecuteCommand(commandtext, CommandType.Text);
                RefreshDatagridview();
            }
            else
            {
                wsgUtilities.wsgNotice("Replicate cancelled");
            }
        }
        public virtual void ProcessCancel(object sender, EventArgs e)
        {

            if (CurrentState == "Edit" || CurrentState == "Insert")
            {
                if (wsgUtilities.wsgReply("Abandon Edit"))
                {
                    parentForm.Update();
                    if (CurrentState == "Edit")
                    {
                        UnlockTableRow(currentidcol, currentablename);
                    }
                    ssprocessds.dropschedule.Rows.Clear();
                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
            }
            else
            {
                if (CurrentState == "View")
                {
                    ssprocessds.dropschedule.Rows.Clear();
                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
                else
                {
                    ssprocessds.view_expandeddropschedule.Rows.Clear();
                    CurrentState = "Select";
                    RefreshControls();

                }
            }
        }

        public void SetEditState(object sender, EventArgs e)
        {
            string editstatus = LockTableRow(currentidcol, currentablename);
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
        public void CloseparentForm(object sender, EventArgs e)
        {
            bool OkToClose = true;
            if (CurrentState == "Edit" || CurrentState == "Insert")
            {
                if (!wsgUtilities.wsgReply("Abandon Edit"))
                {
                    OkToClose = false;
                }
            }
            if (OkToClose)
            {
                if (CurrentState == "Edit")
                {
                    UnlockTableRow(currentidcol, currentablename);
                }
                parentForm.Close();
            }
        }

        public void SetCurrencyTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, Label TLabel, string LText)
        {
            TBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TLabel.Left = TLeft;
            TLabel.Text = LText.TrimEnd();
            TLabel.Width = 120;
            TLabel.Top = TTop;
            TBox.Top = TTop;
            TBox.ReadOnly = false;
            TBox.TextAlign = HorizontalAlignment.Right;
            TBox.Left = TLeft + 125;
            TBox.TabIndex = tabIndex;
            tabIndex += 1;
            SetTextBoxCurrencyBindingT(TBox, dt, TColumnname);
            parentForm.Controls.Add(TBox);
            parentForm.Controls.Add(TLabel);

        }



        public void EstablishDatagridViewDrops(int TextTop, int LeftStart)
        {
            DataGridViewTextBoxColumn ColumnDay = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn ColumnNumDrops = new DataGridViewTextBoxColumn();
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

            ColumnDay.DataPropertyName = "descrip";
            ColumnDay.HeaderText = "Day";
            ColumnDay.Name = "ColumnDay";
            ColumnDay.ReadOnly = true;
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
            dataGridViewDrops.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            ColumnDay,
            ColumnNumDrops,
            ColumnDriver,
            ColumnDrivername,
            });
            dataGridViewDrops.Top = TextTop;
            dataGridViewDrops.Width = 400;
            dataGridViewDrops.Left = LeftStart;
            dataGridViewDrops.Height = 300;

            dataGridViewDrops.EnableHeadersVisualStyles = false;
            dataGridViewDrops.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewDrops.Name = "dataGridViewDrops";
            dataGridViewDrops.ReadOnly = true;
            dataGridViewDrops.RowHeadersVisible = false;


            parentForm.Controls.Add(dataGridViewDrops);

        }
      
        private void CheckTextBox(object sender, CancelEventArgs e)
        {
            e.Cancel = false;

        }
        private void SendTabonEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
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

        public void SetTextBoxCurrencyBindingT(TextBox txtbox, DataTable dt, string fieldname)
        {

            Binding b = new Binding("Text", dt, fieldname);
            {
                b.Format += new ConvertEventHandler(DecimalToCurrencyString);
                b.Parse += new ConvertEventHandler(CurrencyStringToDecimal);
            }
            txtbox.DataBindings.Clear();
            txtbox.DataBindings.Add(b);

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



        public void SetDollarsTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, Label TLabel, string LText)
        {
            // Set the position, size and the databindings. Add to the form
            TBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TLabel.Left = TLeft;
            TLabel.Text = LText.TrimEnd();
            TLabel.TextAlign = ContentAlignment.MiddleCenter;
            TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
            TLabel.Top = TTop;
            TBox.TextAlign = HorizontalAlignment.Right;
            TBox.Left = TLeft + 125;
            TBox.Text = "";
            TBox.Top = TTop;
            TBox.Width = TWidth;
            TBox.TabIndex = tabIndex;
            tabIndex += 1;
            SetTextBoxDollarsBinding(TBox, dt, TColumnname);
            parentForm.Controls.Add(TBox);
            parentForm.Controls.Add(TLabel);
        }
        private void SetTextBoxDollarsBinding(TextBox txtbox, DataTable dt, string columnname)
        {
            Binding b = new Binding("Text", dt, columnname);
            {
                b.Format += new ConvertEventHandler(DecimalToDollarsString);
                b.Parse += new ConvertEventHandler(CurrencyStringToDecimal);
            }
            txtbox.DataBindings.Clear();
            txtbox.DataBindings.Add(b);
        }
        private void DecimalToDollarsString(object sender, ConvertEventArgs cevent)
        {
            // The method converts only to string type. Test this using the DesiredType.
            if (cevent.DesiredType != typeof(string)) return;

            // Use the ToString method to format the value as currency ("c").
            cevent.Value = ((Decimal)cevent.Value).ToString("N0");
        }

        public void SetTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, Label TLabel, string LText)
        {
            // Set the position, size and the databindings
            TBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TLabel.Left = TLeft;
            TLabel.Text = LText;
            TLabel.Top = TTop;
            TBox.Left = TLeft + 125;
            TBox.Text = "";
            TBox.Top = TTop;
            TBox.Width = TWidth;
            TBox.TabIndex = tabIndex;
            tabIndex += 1;
            TBox.DataBindings.Clear();
            TBox.DataBindings.Add("Text", dt, TColumnname);
            parentForm.Controls.Add(TBox);
            parentForm.Controls.Add(TLabel);
        }
        public void SetBoundComboBox(ComboBox CBox, int CLeft, int CTop, int CWidth, Label CLabel, string CText, string ValueMember, string DisplayMember, DataTable Dt)
        {
            CLabel.Text = CText;
            CLabel.Top = CTop;
            CLabel.Left = CLeft;
            CBox.Top = CTop;
            CBox.Left = CLeft + 75;
            CBox.Width = CWidth;
            CLabel.Text = CText;
            CLabel.Top = CTop;
            CBox.TabIndex = tabIndex;
            tabIndex += 1;
            CBox.ValueMember = ValueMember;
            CBox.DisplayMember = DisplayMember;
            CBox.DataSource = Dt;
            parentForm.Controls.Add(CBox);
            parentForm.Controls.Add(CLabel);
        }



        public void RefreshControls()
        {
            DisableControls();
            parentForm.buttonClose.Enabled = true;
            switch (CurrentState)
            {

                case "View":
                    {
                        ComboBoxScheduleday.Visible = true;
                        parentForm.buttonEdit.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonDelete.Enabled = true;
                        ButtonReplicate.Enabled = true;
                        break;
                    }
                case "Select":
                    {
                        ComboBoxScheduleday.Visible = false;
                        parentForm.buttonSelect.Enabled = true;

                        break;
                    }
                case "StoreSelected":
                    {
                        parentForm.Text = commonAppDataMethods.GetStoreName(StoreCode).TrimEnd();
                        ComboBoxScheduleday.Visible= false;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonInsert.Enabled = true;
                        dataGridViewDrops.Enabled = true;
                        ButtonSun.Enabled = true;
                        ButtonMon.Enabled = true;
                        ButtonTue.Enabled = true;
                        ButtonWed.Enabled = true;
                        ButtonThu.Enabled = true;
                        ButtonFri.Enabled = true;
                        ButtonSat.Enabled = true;
                        break;
                    }
                case "Edit":
                    {
                        ComboBoxScheduleday.Visible = true;
                        ComboBoxScheduleday.Enabled = true;
                        ButtonDriver.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        TextBoxNumdrops.Enabled = true;
                        TextBoxCommtype.Enabled = true;
                        TextBoxCommission.Enabled = true;
                        DateTimePickerCfrom.Enabled = true;
                        DateTimePickerCthru.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        parentForm.buttonDelete.Enabled = true;
                        break;
                    }

                case "Insert":
                    {
                        ComboBoxScheduleday.Visible = true;
                        ComboBoxScheduleday.Enabled = true;
                        ButtonDriver.Enabled = true;
                        parentForm.buttonSave.Enabled = true;
                        parentForm.buttonCancel.Enabled = true;
                        TextBoxNumdrops.Enabled = true;
                        TextBoxCommtype.Enabled = true;
                        TextBoxCommission.Enabled = true;
                        DateTimePickerCfrom.Enabled = true;
                        DateTimePickerCthru.Enabled = true;
                        break;
                    }

            }

        }
        public void DisableControls()
        {
            foreach (Control c in parentForm.Controls)
            {
                if (!(c is Label))
                {
                    c.Enabled = false;
                }
            }
        }

    }
}