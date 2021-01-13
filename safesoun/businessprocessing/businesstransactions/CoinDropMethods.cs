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
    public class CoinDropMethods : WSGDataAccess
    {
        #region Declarations
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Coin Processing");
        BusinessReports.BusinessReportsMethods brMethods = new BusinessReports.BusinessReportsMethods();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public Form parentForm = new Form();
        public Form menuform { get; set; }
        public DateTime DropDate { get; set; }
        public string DropFunder = "";
        public string CurrentState = "Select";
        public string commandtext { get; set; }
        public string CurrentTransaction { get; set; }
        public string bankfedid { get; set; }
        public decimal droptotal { get; set; }
        string reportpath = ConfigurationManager.AppSettings["ReportPath"];

        public DateTime lastinventoryclosedate { get; set; }

        public string driver { get; set; }
        public string storecode { get; set; }
        // Custom Controls

        Label LabelHundreds = new Label();
        Label LabelFiftys = new Label();
        Label LabelTwentys = new Label();
        Label LabelTens = new Label();
        Label LabelFives = new Label();
        Label LabelOnes = new Label();
        Label LabelQuarters = new Label();
        Label LabelDimes = new Label();
        Label LabelNickels = new Label();
        Label LabelPennies = new Label();
        Label LabelTotal = new Label();
        Label LabelDropStatus = new Label();

        Button ButtonDelete = new Button();
        Button ButtonSave = new Button();
        Button ButtonExit = new Button();

        TextBox TextBoxHundreds = new TextBox();
        TextBox TextBoxFiftys = new TextBox();
        TextBox TextBoxTwentys = new TextBox();
        TextBox TextBoxTens = new TextBox();
        TextBox TextBoxFives = new TextBox();
        TextBox TextBoxOnes = new TextBox();
        TextBox TextBoxQuarters = new TextBox();
        TextBox TextBoxDimes = new TextBox();
        TextBox TextBoxNickels = new TextBox();
        TextBox TextBoxPennies = new TextBox();
        TextBox TextBoxTotal = new TextBox();
        Icon AppIcon = new Icon("appicon.ico");
        string[] customerdata = new string[2];
        ssprocess openprocessds = new ssprocess();
        ssprocess tempprocessds = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        ssprocess closeprocessds = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        #endregion
        public CoinDropMethods()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.coindrop.idcolColumn);

        }
        public void StartApp()
        {
            bool cont = true;
            string coinbank = "";
            DateTime lastclosedate = DateTime.Now;
            SetEvents();
            driver = commonAppDataMethods.SelectDriver();
            if (driver.TrimEnd() != "")
            {
                GetDriver(driver);
                if (commonAppDataMethods.GetSingleDate("Enter Coin Scheduling Sheet Date", 30, 7))
                {
                    DropDate = commonAppDataMethods.SelectedDate.Date;
                    while (1 == 1)
                    {
                        cont = true;
                        storecode = commonAppDataMethods.SelectDropStore(driver, DropFunder);
                        if (storecode.TrimEnd() != "")
                        {
                            // Drop date must be later than the last close date for the funder
                            storecode = storecode.Substring(0, 11);
                            GetStore(storecode);
                            coinbank = commonAppDataMethods.GetCoinInventoryHolder(storecode, driver);
                            if (coinbank.TrimEnd() != "")
                            {
                                lastclosedate = commonAppDataMethods.GetLastCloseDate(coinbank);
                                if (DropDate <= lastclosedate)
                                {
                                    wsgUtilities.wsgNotice("Vault " + coinbank + " was closed " + lastclosedate.ToShortDateString());
                                    cont = false;
                                }
                            }
                            if (cont)
                            {

                                SetControls();
                                EstablishBlankDataTableRow(ssprocessds.coindrop);
                                if (EstablishData())
                                {
                                    ShowParent();

                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

        }
        public void ShowParent()
        {

            customerdata = commonAppDataMethods.GetCustomerData(ssprocessds.store[0].storecode);

            parentForm.Text = commonAppDataMethods.GetDriverName(driver) + " Activity for " + ssprocessds.store[0].store_name.TrimEnd() + " - " + String.Format("{0:MM/dd/yyyy}", DropDate) +
                " Account " + customerdata[0].TrimEnd();
            parentForm.ShowDialog();


        }
        public void SetControls()
        {
            parentForm.Controls.Clear();
            parentForm.Top = 100;
            parentForm.StartPosition = FormStartPosition.CenterParent;
            int LabelTop = 70;
            int TextTop = 100;
            int LeftStart = 50;
            int ButtonHeight = 30;
            int ButtonWidth = 100;
            int ButtonTop = 25;
            int ButtonLeft = 50;
            int TextBoxWidth = 80;
            int TextBoxHeight = 40;
            LabelDropStatus.Top = LabelTop;
            LabelDropStatus.Left = LeftStart;
            LabelDropStatus.Width = 300;
            LabelDropStatus.TextAlign = ContentAlignment.MiddleLeft;
            LabelDropStatus.Font = new Font(LabelDropStatus.Font, FontStyle.Bold);
            parentForm.Controls.Add(LabelDropStatus);
            #region Set TextBoxes
            LeftStart = 50;
            SetDollarsTextBox(TextBoxHundreds, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.hundreds", LabelHundreds, "Hundreds");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxFiftys, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.fiftys", LabelFiftys, "Fiftys");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxTwentys, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.twentys", LabelTwentys, "Twentys");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxTens, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.tens", LabelTens, "Tens");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxFives, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.fives", LabelFives, "Fives");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxOnes, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.ones", LabelOnes, "Ones");
            TextTop += TextBoxHeight;
            SetCurrencyTextBox(TextBoxQuarters, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.quarters", LabelQuarters, "Quarters");
            TextTop += TextBoxHeight;
            SetCurrencyTextBox(TextBoxDimes, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.dimes", LabelDimes, "Dimes");
            TextTop += TextBoxHeight;
            SetCurrencyTextBox(TextBoxNickels, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.nickels", LabelNickels, "Nickels");
            TextTop += TextBoxHeight;
            SetCurrencyTextBox(TextBoxPennies, LeftStart, TextTop, TextBoxWidth, ssprocessds, "coindrop.pennies", LabelPennies, "Pennies");
            TextTop += TextBoxHeight;
            SetTotalTextBox(LeftStart + 200, TextBoxHundreds.Top, TextBoxWidth);

            #endregion

            SetButton(ButtonSave, "Save", ButtonHeight, ButtonWidth, ButtonTop, ButtonLeft);
            ButtonLeft += ButtonWidth + 5;
            SetButton(ButtonDelete, "Delete", ButtonHeight, ButtonWidth, ButtonTop, ButtonLeft);
            ButtonLeft += ButtonWidth + 5;

            SetButton(ButtonExit, "Exit", ButtonHeight, ButtonWidth, ButtonTop, ButtonLeft);
            ButtonLeft += ButtonWidth + 5;

            foreach (Control c in parentForm.Controls)
            {
                if (c is TextBox)
                {
                    c.TabIndex = 50;
                }
            }

            // Set Tabindex
            TextBoxHundreds.TabIndex = 2;
            TextBoxFiftys.TabIndex = 3;
            TextBoxTwentys.TabIndex = 4;
            TextBoxTens.TabIndex = 5;
            TextBoxFives.TabIndex = 6;
            TextBoxOnes.TabIndex = 7;
            TextBoxQuarters.TabIndex = 8;
            TextBoxDimes.TabIndex = 9;
            TextBoxNickels.TabIndex = 10;
            TextBoxPennies.TabIndex = 11;
            ButtonSave.TabIndex = 12;
            LeftStart = 50;
            parentForm.Width = 750;
            parentForm.Height = TextBoxPennies.Top + TextBoxHeight + 25;
            parentForm.Icon = AppIcon;
        }

        public void SetEvents()
        {
            ButtonExit.Click += new System.EventHandler(CloseparentForm);
            ButtonSave.Click += new System.EventHandler(SaveCoinDrop);
            ButtonDelete.Click += new System.EventHandler(DeleteCoinDrop);
            TextBoxHundreds.Validating += new System.ComponentModel.CancelEventHandler(CheckHundreds);
            TextBoxHundreds.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxHundreds.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxFiftys.Validating += new System.ComponentModel.CancelEventHandler(CheckFiftys);
            TextBoxFiftys.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxFiftys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxTwentys.Validating += new System.ComponentModel.CancelEventHandler(CheckTwentys);
            TextBoxTwentys.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxTwentys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxTens.Validating += new System.ComponentModel.CancelEventHandler(CheckTens);
            TextBoxTens.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxTens.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxFives.Validating += new System.ComponentModel.CancelEventHandler(CheckFives);
            TextBoxFives.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxFives.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxOnes.Validating += new System.ComponentModel.CancelEventHandler(CheckOnes);
            TextBoxOnes.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxOnes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxQuarters.Validating += new System.ComponentModel.CancelEventHandler(CheckQuarters);
            TextBoxQuarters.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxQuarters.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxDimes.Validating += new System.ComponentModel.CancelEventHandler(CheckDimes);
            TextBoxDimes.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxDimes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxNickels.Validating += new System.ComponentModel.CancelEventHandler(CheckNickels);
            TextBoxNickels.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxNickels.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxPennies.Validating += new System.ComponentModel.CancelEventHandler(CheckPennies);
            TextBoxPennies.Validated += new System.EventHandler(ComputeDropTotal);
            TextBoxPennies.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
        }

        public void SetTotalTextBox(int TLeft, int TTop, int TWidth)
        {
            // Set the position, size and the databindings. Add to the form
            LabelTotal.Left = TLeft;
            LabelTotal.Text = "Total";
            LabelTotal.TextAlign = ContentAlignment.MiddleLeft;
            LabelTotal.Font = new Font(LabelTotal.Font, FontStyle.Bold);
            LabelTotal.Top = TTop;
            TextBoxTotal.TextAlign = HorizontalAlignment.Right;
            TextBoxTotal.Left = TLeft + 100;
            TextBoxTotal.ReadOnly = false;
            TextBoxTotal.Text = "";
            TextBoxTotal.Top = TTop;
            TextBoxTotal.ReadOnly = true;
            TextBoxTotal.Width = TWidth;
            parentForm.Controls.Add(TextBoxTotal);
            parentForm.Controls.Add(LabelTotal);
        }
        public void SetDollarsTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataSet ds, string TColumnname, Label TLabel, string LText)
        {
            // Set the position, size and the databindings. Add to the form
            TLabel.Left = TLeft;
            TLabel.Text = LText.TrimEnd();
            TLabel.TextAlign = ContentAlignment.MiddleLeft;
            TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
            TLabel.Top = TTop;
            TBox.TextAlign = HorizontalAlignment.Right;
            TBox.Left = TLeft + 100;
            TBox.ReadOnly = false;
            TBox.Text = "";
            TBox.Top = TTop;
            TBox.Width = TWidth;
            SetTextBoxDollarsBinding(TBox, ds, TColumnname);
            parentForm.Controls.Add(TBox);
            parentForm.Controls.Add(TLabel);
        }
        public void SetCurrencyTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataSet ds, string TColumnname, Label TLabel, string LText)
        {
            // Set the position, size and the databindings. Add to the form
            TLabel.Top = TTop;
            TLabel.Left = TLeft;
            TLabel.Text = LText.TrimEnd();
            TLabel.TextAlign = ContentAlignment.MiddleLeft;
            TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
            TBox.TextAlign = HorizontalAlignment.Right;
            TBox.Left = TLeft + 100;
            TBox.ReadOnly = false;
            TBox.Text = "";
            TBox.Top = TTop;
            TBox.Width = TWidth;
            SetTextBoxCurrencyBinding(TBox, ds, TColumnname);
            parentForm.Controls.Add(TBox);
            parentForm.Controls.Add(TLabel);
        }
        private void SetTextBoxDollarsBinding(TextBox txtbox, DataSet ds, string fieldname)
        {
            Binding b = new Binding("Text", ds, fieldname);
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

        private void ComputeDropTotal(object sender, EventArgs e)
        {

            droptotal = ssprocessds.coindrop[0].hundreds + ssprocessds.coindrop[0].fiftys + ssprocessds.coindrop[0].twentys +
            ssprocessds.coindrop[0].tens + ssprocessds.coindrop[0].fives + ssprocessds.coindrop[0].ones +
            ssprocessds.coindrop[0].quarters + ssprocessds.coindrop[0].dimes + ssprocessds.coindrop[0].nickels +
            ssprocessds.coindrop[0].pennies;
            TextBoxTotal.Text = droptotal.ToString("N2");
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
        public void SetButton(Button ButtonTarget, string Text, int Height, int Width, int Top, int Left)
        {
            ButtonTarget.Text = Text.TrimEnd();
            ButtonTarget.Height = Height;
            ButtonTarget.Width = Width;
            ButtonTarget.Top = Top;
            ButtonTarget.Left = Left;
            parentForm.Controls.Add(ButtonTarget);
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
            ssprocessds.store.Rows.Clear();
            commandtext = "SELECT * FROM store where LEFT(storecode,11) = @storecode";
            ClearParameters();
            AddParms("@storecode", storecode, "SQL");
            FillData(ssprocessds, "store", commandtext, CommandType.Text);
        }
        #region Validation
        private void CheckHundreds(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % 100 != 0)
            {
                wsgUtilities.wsgNotice("Invalid Amount");
                e.Cancel = true;
            }
        }
        private void CheckPennies(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);

        }
        private void CheckFiftys(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % 50 != 0)
            {
                wsgUtilities.wsgNotice("Invalid Amount");
                e.Cancel = true;
            }

        }
        private void CheckTwentys(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % 20 != 0)
            {
                wsgUtilities.wsgNotice("Invalid Amount");
                e.Cancel = true;
            }

        }
        private void CheckTens(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % 10 != 0)
            {
                wsgUtilities.wsgNotice("Invalid Amount");
                e.Cancel = true;
            }

        }

        private void CheckFives(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % 5 != 0)
            {
                wsgUtilities.wsgNotice("Invalid Amount");
                e.Cancel = true;
            }
        }
        private void CheckOnes(object sender, CancelEventArgs e)
        {
        }

        public void CloseparentForm(object sender, EventArgs e)
        {
            parentForm.Close();
        }

        private void CheckDimes(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % .10M != 0)
            {
                wsgUtilities.wsgNotice("Invalid Amount");
                e.Cancel = true;
            }

        }
        private void CheckQuarters(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % .25M != 0)
            {
                wsgUtilities.wsgNotice("Invalid Amount");
                e.Cancel = true;
            }

        }

        public decimal ConvertMoney(string textamount)
        {
            decimal convamount = 0;
            try
            {
                convamount = Convert.ToDecimal(textamount);
            }
            catch
            {
                wsgUtilities.wsgNotice("Invalid Amount");
            }
            return convamount;
        }

        private void CheckNickels(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % .05M != 0)
            {
                wsgUtilities.wsgNotice("Invalid Amount");
                e.Cancel = true;
            }

        }

        #endregion
        #region Event Methods
        public void DeleteCoinDrop(object sender, EventArgs e)
        {
            if (wsgUtilities.wsgReply("Delete this drop?"))
            {
                ClearParameters();
                AddParms("@transid", ssprocessds.coindrop[0].idcol, "SQL");
                try
                {
                    ExecuteCommand("sp_delete_coindrop", CommandType.StoredProcedure);
                    ButtonDelete.Enabled = false;
                    wsgUtilities.wsgNotice("Coin Drop Deleted");
                    parentForm.Close();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                wsgUtilities.wsgNotice("Deletion Cancelled");
            }
        }
        public void SaveCoinDrop(object sender, EventArgs e)
        {
            try
            {
                GenerateAppTableRowSave(ssprocessds.coindrop[0]);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

            parentForm.Close();
        }
        #endregion




        public void CoinOrderListing(string scope, string document)
        {
            bool ContApp = true;
            FrmLoading frmLoading = new FrmLoading();
            string ReportTitle = "";
            DateTime ReportStartDate = DateTime.Now.Date;
            DateTime ReportEndDate = DateTime.Now.Date;
            string SelectedDriver = "";
            string commandtext = "";
            string company = commonAppDataMethods.SelectCoinFunder();
            if (company.TrimEnd() == "")
            {
                ContApp = false;
            }
            if (ContApp)
            {
                if (commonAppDataMethods.GetTwoDates("Enter Report Dates"))
                {
                    frmLoading.Show();
                    ReportStartDate = commonAppDataMethods.SelectedStartDate;
                    ReportEndDate = commonAppDataMethods.SelectedEndDate;

                    ReportTitle = "Coin Order Listing - " + String.Format("{0:MM/dd/yyyy}", ReportStartDate) +
                    " thru " + String.Format("{0:MM/dd/yyyy}", ReportEndDate);
                }
                else
                {
                    ContApp = false;

                }
            }
            if (ContApp)
            {
                ClearParameters();
                AddParms("@startdate", ReportStartDate, "SQL");
                AddParms("@enddate", ReportEndDate, "SQL");

                commandtext = "SELECT * FROM view_expandedcoindrop WHERE  ";
                switch (company)
                {
                    case "Rapid":
                        {
                            // Rapid
                            commandtext += " LEFT(store,4) = '4600' AND bankcoin = 0 ";
                            break;
                        }

                    case "FEGS":
                        {
                            // FEGS
                            commandtext += " LEFT(store,4) = '4075' AND bankcoin = 0 ";
                            break;
                        }
                    case "Signature":
                        {
                            // Signature
                            commandtext += " bankcoin  <> 0 AND bankfedid = @bankfedid ";
                            AddParms("@bankfedid", ConfigurationManager.AppSettings["SignatureInventoryID"], "SQL");

                            break;
                        }
                    case "BNP":
                        {
                            // BNP
                            commandtext += " bankcoin  <> 0 AND bankfedid = @bankfedid ";
                            AddParms("@bankfedid", ConfigurationManager.AppSettings["BNPInventoryID"], "SQL");

                            break;
                        }
                    default:
                        {
                            // Safe and Sound
                            commandtext += " LEFT(store,4) <> '4600' AND LEFT(store,4) <> '4075'  AND bankcoin = 0 ";
                            break;
                        }
                }

                commandtext += " AND dropdate >= @startdate AND dropdate <= @enddate ";

                if (scope != "A")
                {


                    SelectedDriver = commonAppDataMethods.SelectDriver();
                    if (SelectedDriver.TrimEnd() != "")
                    {
                        commandtext += " AND driver = @driver ";
                        rptprocessds.view_expandedcoindrop.Rows.Clear();
                        AddParms("@driver", SelectedDriver, "SQL");
                        commandtext += " ORDER By dropdate, store_name ";

                    }
                    else
                    {
                        ContApp = false;
                    }
                }
                else
                {
                    commandtext += " ORDER By drivername, dropdate, store_name ";
                }

            }
            if (ContApp)
            {
                rptprocessds.view_expandedcoindrop.Rows.Clear();
                FillData(rptprocessds, "view_expandedcoindrop", commandtext, CommandType.Text);
                frmLoading.Close();
                if (rptprocessds.view_expandedcoindrop.Rows.Count > 0)
                {
                    if (document == "L")
                    {
                        brMethods.ShowVsReport(reportpath + "VSCoinOrderList.rdlc", "ssprocessDs", rptprocessds.view_expandedcoindrop, ReportTitle);

                    }
                    else
                    {
                        brMethods.ShowVsReport(reportpath + "VSCoinSlip.rdlc", "ssprocessDs", rptprocessds.view_expandedcoindrop, ReportTitle);

                    }
                }
                else
                {
                    frmLoading.Close();
                    wsgUtilities.wsgNotice("There are no matching records");

                }
            }
            else
            {
                wsgUtilities.wsgNotice("Operation Cancelled");
            }
        }

        public bool EstablishData()
        {
            bool cont = true;
            commandtext = "SELECT * FROM coindrop WHERE driver = @driver AND store = @storecode AND dropdate = @dropdate";
            ssprocessds.coindrop.Rows.Clear();
            ClearParameters();
            AddParms("@driver", driver, "SQL");
            AddParms("@storecode", storecode, "SQL");
            AddParms("@dropdate", DropDate, "SQL");
            FillData(ssprocessds, "coindrop", commandtext, CommandType.Text);
            if (ssprocessds.coindrop.Rows.Count < 1)
            {
                EstablishBlankDataTableRow(ssprocessds.coindrop);
                ssprocessds.coindrop[0].driver = driver;
                ssprocessds.coindrop[0].store = storecode;
                ssprocessds.coindrop[0].dropdate = DropDate;
                ssprocessds.coindrop[0].inventorydate = Convert.ToDateTime("12/31/3099");
                ssprocessds.coindrop[0].filled = "N";
                ssprocessds.coindrop[0].imported = "N";
                ButtonDelete.Enabled = false;
                LabelDropStatus.Text = "";
            }
            else
            {
                if (ssprocessds.coindrop[0].inventorydate <= lastinventoryclosedate)
                {
                    wsgUtilities.wsgNotice("The drop cannot be edited. Seek help");
                    cont = false;

                }
                else
                {
                    ButtonDelete.Enabled = true;
                    LabelDropStatus.Text = "Editing Existing Drop";
                }
            }
            return cont;
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
