using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.IO;
using System.ComponentModel;
using BusinessTransactions;
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

namespace BusinessTransactions
{

    public class StopMethods : WSGDataAccess
    {
        DateTimeSelectionMethods dateTimeSelectionMethods = new DateTimeSelectionMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Stop Processing");
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public Form parentForm = new Form();
        public Form menuform { get; set; }
        public string CurrentState = "Select";
        public bool EditingRow = false;
        public string commandtext { get; set; }
        decimal StopBagTotal { get; set; }
        int StopBagCount { get; set; }
        TextBox TextboxSealnumber = new TextBox();
        TextBox TextboxBagamount = new TextBox();
        TextBox TextboxStopBagTotal = new TextBox();
        TextBox TextboxStopBagCount = new TextBox();
        public string CurrentTransaction { get; set; }
        public string driver { get; set; }
        public string storecode { get; set; }
        public string companycode { get; set; }
        public int CurrentBagRow { get; set; }
        public DateTime PickupDate { get; set; }
        public string PickupTime { get; set; }
        // Set the eventhandler for cursor movement
        // Custom Controls
        DataGridView dgBags = new dgv();
        Button ButtonSave = new Button();
        Button ButtonExit = new Button();
        Icon AppIcon = new Icon("appicon.ico");
        ssprocess openprocessds = new ssprocess();
        ssprocess tempprocessds = new ssprocess();
        ssprocess inputprocessds = new ssprocess();
        ssprocess insertprocessds = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        public StopMethods()
            : base("SQL", "SQLConnString")
        {

            SetIdcol(tempprocessds.hhpickup.idcolColumn);
            SetIdcol(insertprocessds.hhpickup.idcolColumn);
            SetControls();
            SetEvents();
       
        }
        public void StartApp()
        {
            if (dateTimeSelectionMethods.GetDateAndTime())
            {
                PickupDate = dateTimeSelectionMethods.SelectedDate;
                PickupTime = dateTimeSelectionMethods.SelectedTime.Substring(0, 8);
                string dr = commonAppDataMethods.SelectDriver();
                driver = dr;
                if (driver.TrimEnd() != "")
                {
                    // Driver Loop  
                    while (1 == 1)
                    {

                        companycode = commonAppDataMethods.SelectActiveCompany();
                        if (companycode.TrimEnd() == "")
                        {
                            break;
                        }
                        // Company Loop
                        while (1 == 1)
                        {
                            storecode = commonAppDataMethods.SelectStore(companycode);
                            if (storecode.TrimEnd() != "")
                            {
                                EditingRow = false;
                                CurrentBagRow = 0;
                                storecode = storecode.Substring(0, 11);
                                GetStore(storecode);
                                inputprocessds.hhpickup.Rows.Clear();
                                EstablishBlankDataTableRow(inputprocessds.hhpickup);
                                SetPickupRows();
                                SetTotals();
                                ShowParent();
                            }
                            else
                            {
                                break;
                            }
                        }
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
        public void SetTotals()
        {
            StopBagCount = 0;
            StopBagTotal = 0;
            for (int i = 0; i <= tempprocessds.hhpickup.Rows.Count - 1; i++)
            {
                if (tempprocessds.hhpickup[i].amount == 0)
                {
                    continue;
                }
                StopBagCount++;
                StopBagTotal += tempprocessds.hhpickup[i].amount;
            }
            TextboxStopBagCount.Text = StopBagCount.ToString("N0");
            TextboxStopBagTotal.Text = StopBagTotal.ToString("N2");
        }
        public void SetPickupRows()
        {
            tempprocessds.hhpickup.Rows.Clear();
            // Load any prior pickups for this driver, store, date
            GetPickups(driver, storecode, DateTime.Now.Date);

            for (int x = 0; x <= openprocessds.hhpickup.Rows.Count - 1; x++)
            {
                tempprocessds.hhpickup.ImportRow(openprocessds.hhpickup.Rows[x]);
            }
            tempprocessds.hhpickup.AcceptChanges();
        }
        public void ShowParent()
        {
            parentForm.Text =   ssprocessds.store[0].store_name.TrimEnd() + " - " + commonAppDataMethods.GetAssignedMoneyCenter( ssprocessds.store[0].storecode);
            TextboxSealnumber.Focus();
            parentForm.ShowDialog();
        }
        public void SetControls()
        {

            parentForm.Top = 100;
            parentForm.StartPosition = FormStartPosition.CenterParent;
            int ButtonHeight = 30;
            int ButtonWidth = 100;
            int ButtonTop = 25;
            int ButtonLeft = 50;
            parentForm.Text = "Stop Processing";

            SetInputTextbox(ButtonLeft, TextboxSealnumber, ButtonTop + ButtonHeight + 5, 100, "Seal Number", "sealnumber",1);
            SetInputTextbox(ButtonLeft + TextboxSealnumber.Width + 10, TextboxBagamount, ButtonTop + ButtonHeight + 5, 100, "Amount", "amount",2);
            // Use currency binding for amount
            TextboxBagamount.DataBindings.Clear();
            commonAppDataMethods.SetTextBoxCurrencyBinding(TextboxBagamount, inputprocessds.hhpickup, "amount");
            dgBags.DataSource = null;
            dgBags.Top = TextboxSealnumber.Top + 50;
            dgBags.ColumnCount = 2;
            dgBags.AllowUserToDeleteRows = false;
            dgBags.AllowUserToAddRows = false;
            dgBags.AllowUserToOrderColumns = false;
            dgBags.Columns[0].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgBags.Columns[1].SortMode = DataGridViewColumnSortMode.Programmatic;
            dgBags.RowHeadersVisible = false;
            dgBags.AutoGenerateColumns = false;
            DataView dvHhPickup = new DataView(tempprocessds.hhpickup);
            //     dvHhPickup.Sort = "sealnumber DESC";
            BindingSource bindingdgBags = new BindingSource();
            bindingdgBags.DataSource = dvHhPickup;
            dgBags.DataSource = bindingdgBags;
            dgBags.Left = 10;
            dgBags.Height = 350;
            dgBags.ReadOnly = false;
            dgBags.Columns[0].Name = "colSealnumber";
            dgBags.Columns[0].DataPropertyName = "sealnumber";
            dgBags.Columns[0].HeaderText = "Seal Number";
            dgBags.Columns[0].Width = 200;
            dgBags.Columns[0].ReadOnly = true;
            dgBags.Columns[1].Name = "colAmount";
            dgBags.Columns[1].DataPropertyName = "amount";
            dgBags.Columns[1].HeaderText = "Amount";
            dgBags.Columns[1].Width = 100;
            dgBags.Columns[1].ReadOnly = true;
            dgBags.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgBags.Columns[1].DefaultCellStyle.Format = "c";
            dgBags.Width = dgBags.Columns[0].Width + dgBags.Columns[1].Width + 5;
            parentForm.Controls.Add(dgBags);
            SetButton(ButtonSave, "Save", ButtonHeight, ButtonWidth, ButtonTop, ButtonLeft);
            ButtonLeft += ButtonWidth + 5;
            ButtonLeft += ButtonWidth + 5;
            SetButton(ButtonExit, "Exit", ButtonHeight, ButtonWidth, ButtonTop, ButtonLeft);
            ButtonLeft += ButtonWidth + 5;
            SetTotalTextBox(dgBags.Left + dgBags.Width + 15, TextboxStopBagCount, dgBags.Top, 100, "Bag Count");
            SetTotalTextBox(dgBags.Left + dgBags.Width + 15, TextboxStopBagTotal, dgBags.Top + TextboxStopBagCount.Height + 5, 100, "Bag Total");
            // Set Tabindex
            ButtonSave.TabIndex = 11;
            parentForm.Width = 600;
            parentForm.Icon = AppIcon;
            parentForm.Height = 600;
        }

        public void SetEvents()
        {
            ButtonExit.Click += new System.EventHandler(CloseparentForm);
            ButtonSave.Click += new System.EventHandler(SavePickups);
            TextboxSealnumber.KeyUp += new KeyEventHandler(SealnumberKeyUp);
            TextboxBagamount.KeyUp += new KeyEventHandler(BagamountKeyUp);
            TextboxBagamount.LostFocus += new EventHandler(BagamountLostFocus);
            TextboxBagamount.GotFocus += new EventHandler(BagamountGotFocus);
            dgBags.CellContentDoubleClick += new DataGridViewCellEventHandler(dgBagsCellDoubleClick);
            dgBags.KeyDown += new System.Windows.Forms.KeyEventHandler(CaptureEnter);

        }
        private void dgBagsCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EstablishEditRow();
        }

        public void EstablishEditRow()
        {
            int rowid = CaptureIdCol(dgBags);
            for (int i = 0; i <= tempprocessds.hhpickup.Rows.Count - 1; i++)
            {
                if (tempprocessds.hhpickup[i].idcol == rowid)
                {
                    CurrentBagRow = i;
                    break;
                }
            }
            inputprocessds.hhpickup[0].sealnumber = tempprocessds.hhpickup[CurrentBagRow].sealnumber;
            inputprocessds.hhpickup[0].amount = tempprocessds.hhpickup[CurrentBagRow].amount;

            EditingRow = true;
            TextboxSealnumber.Focus();
        }
        public void SetInputTextbox(int TLeft, TextBox Tbox, int TTop, int TWidth, string labeltext, string bindingsource, int tabindex)
        {
            Label LabelInput = new Label();
            // Set the position, size and the databindings. Add to the form
            LabelInput.Left = TLeft;
            LabelInput.Text = labeltext;
            LabelInput.TextAlign = ContentAlignment.MiddleLeft;
            LabelInput.Font = new Font(LabelInput.Font, FontStyle.Bold);
            LabelInput.Top = TTop;
            Tbox.TextAlign = HorizontalAlignment.Right;
            Tbox.TabIndex = tabindex;
            Tbox.Left = TLeft;
            Tbox.ReadOnly = false;
            Tbox.Text = "";
            Tbox.Top = TTop + 20;
            Tbox.DataBindings.Clear();
            Tbox.DataBindings.Add("Text", inputprocessds.hhpickup, bindingsource);
            Tbox.ReadOnly = false;
            Tbox.CharacterCasing = CharacterCasing.Upper;
            Tbox.Width = TWidth;
            parentForm.Controls.Add(Tbox);
            parentForm.Controls.Add(LabelInput);
        }

        public void SetTotalTextBox(int TLeft, TextBox Tbox, int TTop, int TWidth, string labeltext)
        {
            Label LabelTotal = new Label();
            // Set the position, size and the databindings. Add to the form
            LabelTotal.Left = TLeft;
            LabelTotal.Text = labeltext;
            LabelTotal.TextAlign = ContentAlignment.MiddleLeft;
            LabelTotal.Font = new Font(LabelTotal.Font, FontStyle.Bold);
            LabelTotal.Top = TTop;
            Tbox.TextAlign = HorizontalAlignment.Right;
            Tbox.Left = TLeft + 100;
            Tbox.ReadOnly = false;
            Tbox.Text = "";
            Tbox.Top = TTop;
            Tbox.ReadOnly = true;
            Tbox.Width = TWidth;
            parentForm.Controls.Add(Tbox);
            parentForm.Controls.Add(LabelTotal);
        }
        private void CaptureEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                EstablishEditRow();
            }
        }

        public void SetButton(Button ButtonTarget, string Text, int Height, int Width, int Top, int Left)
        {
            ButtonTarget.Text = Text.TrimEnd();
            ButtonTarget.Height = Height;
            ButtonTarget.Width = Width;
            ButtonTarget.Top = Top;
            ButtonTarget.Left = Left;
            parentForm.Controls.Add(ButtonTarget);
        }
        public void SealnumberKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public void BagamountGotFocus(object sender, EventArgs e)
        {
            if (TextboxSealnumber.Text == "")
            {
                TextboxSealnumber.Focus();
            }
        }
        public void BagamountLostFocus(object sender, EventArgs e)
        {

            int rowpointer = 0;
            if (EditingRow)
            {
                rowpointer = CurrentBagRow;
            }
            else
            {
                if (tempprocessds.hhpickup.Rows.Count > 0)
                {
                    rowpointer = tempprocessds.hhpickup.Rows.Count;
                }
                else
                {
                    rowpointer = 0;
                }
                if (inputprocessds.hhpickup[0].amount != 0)
                {
                    insertprocessds.hhpickup.Rows.Clear();
                    EstablishBlankDataTableRow(insertprocessds.hhpickup);
                    tempprocessds.hhpickup.ImportRow(insertprocessds.hhpickup.Rows[0]);
                }
            }
            inputprocessds.hhpickup.AcceptChanges();
            if (EditingRow && inputprocessds.hhpickup[0].amount == 0)
            {
                tempprocessds.hhpickup.Rows[rowpointer].Delete();
                tempprocessds.hhpickup.AcceptChanges();
            }
            else
            {
                if (inputprocessds.hhpickup[0].amount != 0)
                {
                    tempprocessds.hhpickup[rowpointer].imported = "N";
                    tempprocessds.hhpickup[rowpointer].lckstat = " ";
                    tempprocessds.hhpickup[rowpointer].storecode = storecode;
                    tempprocessds.hhpickup[rowpointer].driver = driver;
                    tempprocessds.hhpickup[rowpointer].pickuptime = PickupTime;
                    tempprocessds.hhpickup[rowpointer].pickupdate = PickupDate;
                    tempprocessds.hhpickup[rowpointer].sealnumber = inputprocessds.hhpickup[0].sealnumber;
                    tempprocessds.hhpickup[rowpointer].amount = inputprocessds.hhpickup[0].amount;
                    tempprocessds.hhpickup.AcceptChanges();
                }
            }
            SetTotals();
            inputprocessds.hhpickup[0].amount = 0;
            inputprocessds.hhpickup[0].sealnumber = "";
            inputprocessds.hhpickup.AcceptChanges();
            TextboxSealnumber.Focus();
            EditingRow = false;
            CurrentBagRow = 0;
        }

        public void BagamountKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                SendKeys.Send("{TAB}");
            }
        }





        public void SavePickupData()
        {
            DataRow[] foundRows;
            string searchexpression;
            commandtext = "DELETE FROM hhpickup WHERE idcol = @idcol";
            // Check for rows to be deleted
            for (int i = 0; i <= openprocessds.hhpickup.Rows.Count - 1; i++)
            {
                searchexpression = "idcol = " + openprocessds.hhpickup[i].idcol.ToString().TrimStart();

                foundRows = tempprocessds.hhpickup.Select(searchexpression);
                if (foundRows.Length < 1)
                {
                    ClearParameters();
                    AddParms("@idcol", openprocessds.hhpickup[i].idcol, "SQL");
                    try
                    {
                        ExecuteCommand(commandtext, CommandType.Text);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            for (int i = 0; i <= tempprocessds.hhpickup.Rows.Count - 1; i++)
            {
                if (tempprocessds.hhpickup[i].amount != 0)
                {
                    tempprocessds.hhpickup[i].lckstat = "";
                    tempprocessds.hhpickup[i].imported = "N";
                    tempprocessds.hhpickup[i].postingdate = DateTime.Now.Date.AddDays(550);
                    tempprocessds.hhpickup[i].dtsdepositid = 0;
                    GenerateAppTableRowSave(tempprocessds.hhpickup[i]);
                }
            }



            // Refresh the table of saved pickups
            GetPickups(driver, storecode, DateTime.Now.Date);
        }
        public void SavePickups(object sender, EventArgs e)
        {
            SavePickupData();
            SetPickupRows();
            wsgUtilities.wsgNotice("Save Complete");
            parentForm.Close();
        }

        public void GetDriver(string driver)
        {
            ssprocessds.driver.Rows.Clear();
            commandtext = "SELECT * FROM driver where number = @driver";
            ClearParameters();
            AddParms("@driver", driver, "SQL");
            FillData(ssprocessds, "driver", commandtext, CommandType.Text);
        }
        public void GetStore(string driver)
        {
            ClearParameters();
            ssprocessds.store.Rows.Clear();
            commandtext = "SELECT * FROM store where LEFT(storecode,11) = @storecode";
            AddParms("@storecode", storecode, "SQL");
            FillData(ssprocessds, "store", commandtext, CommandType.Text);
        }

        public void GetPickups(string driver, string storecode, DateTime pickupdate)
        {
            ClearParameters();
            AddParms("@storecode", storecode, "SQL");
            AddParms("@driver", driver, "SQL");
            AddParms("@pickupdate", pickupdate, "SQL");
            openprocessds.hhpickup.Rows.Clear();
            commandtext = "SELECT * FROM hhpickup WHERE storecode = @storecode ";
            commandtext += "AND driver = @driver AND imported <> 'Y' AND pickupdate = @pickupdate";
            FillData(openprocessds, "hhpickup", commandtext, CommandType.Text);
        }

        public void CloseparentForm(object sender, EventArgs e)
        {
            parentForm.Close();
        }


    }
    // Required to process the enter key properly
    public class dgv : DataGridView
    {
        /*
          protected override bool ProcessDialogKey(Keys keyData)
          {
            Keys key = (keyData & Keys.KeyCode);
            if (key == Keys.Enter)
            {
              return this.ProcessRightKey(keyData);
            }
            return base.ProcessDialogKey(keyData);
          }
          protected override bool ProcessDataGridViewKey(KeyEventArgs e)
          {
            if (e.KeyCode == Keys.Enter)
            {
         //      return false;
            }
            return base.ProcessDataGridViewKey(e);
          }
  
         */

    }

}
