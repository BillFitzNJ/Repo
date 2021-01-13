using System;
using System.Collections.Generic;
using System.IO;
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
    public class GroupBoxVerifiedMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Armed Courier");
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public GroupBox parentGroupBox = new GroupBox();
        public string commandtext { get; set; }
        public string bankfedid { get; set; }
        public decimal droptotal { get; set; }
        // Custom Controls
        public DataSet ds = new DataSet();
        public DataTable dt = new DataTable();
        Label LabelHundreds = new Label();
        Label LabelFiftys = new Label();
        Label LabelTwentys = new Label();
        Label LabelTens = new Label();
        Label LabelFives = new Label();
        Label LabelOnes = new Label();
        Label LabelTwos = new Label();
        Label LabelMixedcoin = new Label();
        public Label LabelDeclared = new Label();
        public Label LabelVerified = new Label();
        public Label LabelDifference = new Label();
        Label LabelNotes = new Label();
        public Decimal TotalDeclared = 0;
        public Decimal TotalVerified = new decimal();
        public Decimal TotalDifference = new decimal();
        public Button ButtonSave = new Button();
        public Button ButtonCancel = new Button();
        public TextBox TextBoxHundreds = new TextBox();
        public TextBox TextBoxFiftys = new TextBox();
        public TextBox TextBoxTwentys = new TextBox();
        public TextBox TextBoxTens = new TextBox();
        public TextBox TextBoxFives = new TextBox();
        public TextBox TextBoxOnes = new TextBox();
        public TextBox TextBoxTwos = new TextBox();
        public TextBox TextBoxMixedcoin = new TextBox();
        public TextBox TextBoxDeclared = new TextBox();
        public TextBox TextBoxVerified = new TextBox();
        public TextBox TextBoxDifference = new TextBox();
        public TextBox TextBoxNotes = new TextBox();

        ssprocess openprocessds = new ssprocess();
        ssprocess tempprocessds = new ssprocess();
        ssprocess rptprocessds = new ssprocess();
        ssprocess closeprocessds = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        public GroupBoxVerifiedMethods()
            : base("SQL", "SQLConnString")
        {
        }

        public GroupBox SetGroupBox(DataTable sourcedt, DataSet sourceds)
        {
            TotalVerified = 0;
            TotalDifference = 0;
            sourcedt.Rows.Clear();
            ds = sourceds;
            dt = sourcedt;
            parentGroupBox.Padding = new Padding(0);
            parentGroupBox.Dock = DockStyle.Left;
            SetControls();
            SetEvents();
            return parentGroupBox;
        }
        public void SetEvents()
        {
            TextBoxHundreds.Validating += new System.ComponentModel.CancelEventHandler(CheckHundreds);
            TextBoxHundreds.Validated += new System.EventHandler(TotalCurrentPostings);
            TextBoxHundreds.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxFiftys.Validating += new System.ComponentModel.CancelEventHandler(CheckFiftys);
            TextBoxFiftys.Validated += new System.EventHandler(TotalCurrentPostings);
            TextBoxFiftys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxTwentys.Validating += new System.ComponentModel.CancelEventHandler(CheckTwentys);
            TextBoxTwentys.Validated += new System.EventHandler(TotalCurrentPostings);
            TextBoxTwentys.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxTens.Validating += new System.ComponentModel.CancelEventHandler(CheckTens);
            TextBoxTens.Validated += new System.EventHandler(TotalCurrentPostings);
            TextBoxTens.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxFives.Validating += new System.ComponentModel.CancelEventHandler(CheckFives);
            TextBoxFives.Validated += new System.EventHandler(TotalCurrentPostings);
            TextBoxFives.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxTwos.Validating += new System.ComponentModel.CancelEventHandler(CheckTwos);
            TextBoxTwos.Validated += new System.EventHandler(TotalCurrentPostings);
            TextBoxTwos.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxOnes.Validated += new System.EventHandler(TotalCurrentPostings);
            TextBoxOnes.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
            TextBoxMixedcoin.Validated += new System.EventHandler(TotalCurrentPostings);
            TextBoxMixedcoin.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SendTabonEnter);
        }
        public void SetControls()
        {
            parentGroupBox.Controls.Clear();
            parentGroupBox.Top = 0;
            parentGroupBox.Left = 0;
            int TextTop = 10;
            int LeftStart = 5;
            int ButtonHeight = 30;
            int ButtonWidth = 100;
            int ButtonTop = 10;
            int ButtonLeft = 350;
            int TextBoxWidth = 80;
            int TextBoxHeight = 25;
            #region Set TextBoxes
            SetDollarsTextBox(TextBoxOnes, LeftStart, TextTop, TextBoxWidth, ds, dt.TableName + ".ones", LabelOnes, "Ones");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxTwos, LeftStart, TextTop, TextBoxWidth, ds, dt.TableName + ".twos", LabelTwos, "Twos");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxFives, LeftStart, TextTop, TextBoxWidth, ds, dt.TableName + ".fives", LabelFives, "Fives");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxTens, LeftStart, TextTop, TextBoxWidth, ds, dt.TableName + ".tens", LabelTens, "Tens");

            // Start new row
            TextTop = 10;
            LeftStart = 175;
            SetDollarsTextBox(TextBoxTwentys, LeftStart, TextTop, TextBoxWidth, ds, dt.TableName + ".twentys", LabelTwentys, "Twentys");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxFiftys, LeftStart, TextTop, TextBoxWidth, ds, dt.TableName + ".fiftys", LabelFiftys, "Fiftys");
            TextTop += TextBoxHeight;
            SetDollarsTextBox(TextBoxHundreds, LeftStart, TextTop, TextBoxWidth, ds, dt.TableName + ".hundreds", LabelHundreds, "Hundreds");
            TextTop += TextBoxHeight;
            SetCurrencyTextBox(TextBoxMixedcoin, LeftStart, TextTop, TextBoxWidth, ds, dt.TableName + ".mixedcoin", LabelMixedcoin, "Mixed Coin");
            LeftStart = 360;
            TextTop = TextBoxFiftys.Top + 10;
            parentGroupBox.Controls.Add(LabelDeclared);
            LabelDeclared.Left = 360;
            LabelDeclared.Top = TextTop;
            LabelDeclared.Width = 200;
            TextTop += TextBoxHeight;
            parentGroupBox.Controls.Add(LabelVerified);
            LabelVerified.Left = 360;
            LabelVerified.Top = TextTop;
            TextTop += TextBoxHeight;
            parentGroupBox.Controls.Add(LabelDifference);
            LabelDifference.Left = 360;
            LabelDifference.Top = TextTop;
            LabelDifference.Width = 200;
            TextTop += TextBoxHeight;

            #endregion

            LeftStart = 10;
            TextTop = TextBoxTens.Top + TextBoxHeight;
            // Add the notes textbox
            LabelNotes.Left = LeftStart;
            LabelNotes.Top = TextTop;
            LabelNotes.Text = "Notes";

            TextTop += TextBoxHeight;
            TextBoxNotes.Top = TextTop;
            TextBoxNotes.Left = LeftStart;
            TextBoxNotes.Multiline = true;
            TextBoxNotes.Height = TextBoxHeight * 2;
            TextBoxNotes.Width = 350;
            TextBoxNotes.DataBindings.Add("Text", ds, dt.TableName + ".notes");
            parentGroupBox.Controls.Add(TextBoxNotes);
            parentGroupBox.Controls.Add(LabelNotes);

            SetButton(ButtonSave, "Save", ButtonHeight, ButtonWidth, ButtonTop, ButtonLeft);
            SetButton(ButtonCancel, "Cancel", ButtonHeight, ButtonWidth, ButtonTop, ButtonLeft + ButtonWidth + 10);
            foreach (Control c in parentGroupBox.Controls)
            {
                if (c is TextBox)
                {
                    c.TabIndex = 50;
                }
            }

            // Set Tabindex
            TextBoxOnes.TabIndex = 1;
            TextBoxTwos.TabIndex = 2;
            TextBoxFives.TabIndex = 3;
            TextBoxTens.TabIndex = 4;
            TextBoxTwentys.TabIndex = 5;
            TextBoxFiftys.TabIndex = 6;
            TextBoxHundreds.TabIndex = 7;
            TextBoxMixedcoin.TabIndex = 8;
            ButtonSave.TabIndex = 12;
            LeftStart = 50;
            parentGroupBox.Width = 600;
            parentGroupBox.Height = TextBoxTens.Top + TextBoxHeight + 25;

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
            TBox.Left = TLeft + 75;
            TBox.ReadOnly = false;
            TBox.Text = "";
            TBox.Top = TTop;
            TBox.Width = TWidth;
            SetTextBoxDollarsBinding(TBox, ds, TColumnname);
            parentGroupBox.Controls.Add(TBox);
            parentGroupBox.Controls.Add(TLabel);
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
            TBox.Left = TLeft + 75;
            TBox.ReadOnly = false;
            TBox.Text = "";
            TBox.Top = TTop;
            TBox.Width = TWidth;
            SetTextBoxCurrencyBinding(TBox, ds, TColumnname);
            parentGroupBox.Controls.Add(TBox);
            parentGroupBox.Controls.Add(TLabel);
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
        private void DecimalToDollarsString(object sender, ConvertEventArgs cevent)
        {
            // The method converts only to string type. Test this using the DesiredType.
            if (cevent.DesiredType != typeof(string)) return;

            // Use the ToString method to format the value as currency ("c").
            cevent.Value = ((Decimal)cevent.Value).ToString("N0");
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
            parentGroupBox.Controls.Add(ButtonTarget);
        }

        public void SetTextTextBox(TextBox TBox, int TLeft, int TTop, int Theight, int TWidth, DataTable dt, string TColumnname, Label TLabel, string LText, bool Multiline, bool ReadOnly)
        {
            // Set the position, size and the databindings. Add to the form
            TLabel.Left = TLeft;
            TLabel.Width = TWidth;
            TLabel.Text = LText.TrimEnd();
            TLabel.TextAlign = ContentAlignment.MiddleLeft;
            TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
            TLabel.Top = TTop - 20;
            TBox.Left = TLeft;
            TBox.Multiline = Multiline;
            TBox.Height = Theight;
            TBox.ReadOnly = ReadOnly;
            TBox.Text = "";
            TBox.Top = TTop;
            TBox.Width = TWidth;
            TBox.DataBindings.Add("Text", dt, TColumnname);
            parentGroupBox.Controls.Add(TBox);
            parentGroupBox.Controls.Add(TLabel);

        }
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
        private void CheckTwos(object sender, CancelEventArgs e)
        {
            decimal x = ConvertMoney(((TextBox)sender).Text);
            if (x % 2 != 0)
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

        private void SendTabonEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SendKeys.Send("{TAB}");
            }
        }

        public void RefreshTotals()
        {
            foreach (DataRow dr in dt.Rows)
            {

                TotalVerified = (Convert.ToDecimal(dr["hundreds"]) + Convert.ToDecimal(dr["fiftys"]) + Convert.ToDecimal(dr["twentys"]));
                TotalVerified += (Convert.ToDecimal(dr["tens"]) + Convert.ToDecimal(dr["fives"]) + Convert.ToDecimal(dr["twos"]));
                TotalVerified += (Convert.ToDecimal(dr["ones"]) + Convert.ToDecimal(dr["mixedcoin"]));
                TotalDifference = TotalVerified - TotalDeclared;
                LabelVerified.Text = "Counted: " + TotalVerified.ToString("#,#.00#;(#,#.00#");
                LabelDeclared.Text = "Declared: " + TotalDeclared.ToString("#,#.00#;(#,#.00#");
                LabelDifference.Text = "Difference: " + TotalDifference.ToString("#,#.00#;(#,#.00#)");
            }

        }
        public void TotalCurrentPostings(object sender, EventArgs e)
        {
            RefreshTotals();
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

    }

}
