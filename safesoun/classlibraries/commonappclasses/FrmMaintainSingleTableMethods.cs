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
namespace CommonAppClasses
{
  public class FrmMaintainSingleTableMethods: WSGDataAccess 
  {
    public int currentidcol{get; set;}
    public Form menuform{get; set;}
    public string currentablename{get; set;}
    public DataTable currentdatatable = new DataTable();

    AppUtilities appUtilities = new AppUtilities();
    AppConstants myAppconstants = new AppConstants();
    WSGUtilities wsgUtilities = new WSGUtilities("Armed Courier");
    public FrmMaintainSingleTable parentForm = new FrmMaintainSingleTable();
    FrmSelectorMethods frmSelector = new FrmSelectorMethods();
    public string CurrentState = "Select";
    public FrmMaintainSingleTableMethods()
      : base("SQL","SQLConnString")
    {
      EstablishAppConstants();
      SetEvents();
      SetControls();
      RefreshControls();
    }
    public virtual void SetControls()
    {
       //The child override method establishes the unique controls for this app
    }
    public virtual void ShowParent()
    {
      parentForm.MdiParent = menuform;
      parentForm.Show();
    }
    public virtual void EstablishAppConstants()
    {
      //The child override method establishes the unique names for this app
    }
    public virtual void RefreshControls()
    {
      DisableControls();
      parentForm.buttonClose.Enabled = true;
      switch (CurrentState)
      {
        case "Select":
          {
            DisableTextBoxes();
            parentForm.buttonSelect.Enabled = true;
            parentForm.buttonInsert.Enabled = true;
            
            break;
          }
        case "View":
          {
            DisableTextBoxes();
            DisableCheckBoxes();
            parentForm.buttonEdit.Enabled = true;
            parentForm.buttonDelete.Enabled = true;
            parentForm.buttonCancel.Enabled = true;
              break;
          }
        case "Edit":
          {
            EnableTextBoxes();
            EnableComboxBoxes();
            EnableListBoxes();
            EnableCheckBoxes();
            parentForm.buttonSave.Enabled = true;
            parentForm.buttonCancel.Enabled = true;
            break;
          }
        case "Insert":
          {
           EnableTextBoxes();
           EnableCheckBoxes();
           EnableComboxBoxes();
           EnableListBoxes();
           parentForm.buttonSave.Enabled = true;
           parentForm.buttonCancel.Enabled = true;
           break;
          }
      }
      // The child's override method handles any additional controls
    }
    public void DisableControls()
    {
      foreach (Control c in parentForm.Controls)
      {
        if ( ! ( c  is Label))
        {
         c.Enabled = false;
        }
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
   
    }
    public virtual void SetInsertState(object sender, EventArgs e)
    {
      // The child's override method establishes the blank datatable row
      CurrentState = "Insert";
      RefreshControls();
    }
    public virtual void ProcessCancel(object sender, EventArgs e)
    {
        if (CurrentState == "View")
        {
            parentForm.Update();
            currentdatatable.Rows.Clear();
            CurrentState = "Select";
            RefreshControls();
        }
        else
        {
            if (wsgUtilities.wsgReply("Abandon Edit"))
            {
                parentForm.Update();
                if (CurrentState == "Edit")
                {
                    UnlockTableRow(currentidcol, currentablename);
                }
                currentdatatable.Rows.Clear();
                CurrentState = "Select";
                RefreshControls();
            }
        }
     }
 
    public virtual void CloseparentForm(object sender, EventArgs e)
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


    
    public void DisableTextBoxes()
    {
      // Loop thru all the controls on each tab page and disable text boxes and  buttons  
      foreach (Control c in parentForm.Controls)
      {
          if (c is TextBox || c is CheckBox)
            c.Enabled = false;
      }
    }

    public void EnableTextBoxes()
    {
      // Loop thru all the controls on each tab page and enable text boxes and  buttons  
      foreach (Control c in parentForm.Controls)
      {
          if (c is TextBox || c is CheckBox)
            c.Enabled = true;
      }
    }

    public void EnableListBoxes()
    {
        // Loop thru all the controls on each tab page and enable listboxes  
        foreach (Control c in parentForm.Controls)
        {
            if (c is ListBox)
                c.Enabled = true;
        }
    }
   
    public void EnableComboxBoxes()
    {
        // Loop thru all the controls on each tab page and enable comboboxes  
        foreach (Control c in parentForm.Controls)
        {
            if (c is ComboBox)
                c.Enabled = true;
        }
    }
    public void DisableCheckBoxes()
    {
        // Loop thru all the controls on each tab page and disable checkboxes  
        foreach (Control c in parentForm.Controls)
        {
            if (c is TextBox)
                c.Enabled = false;
        }
    }

    public void EnableCheckBoxes()
    {
        // Loop thru all the controls on each tab page and enable checkboxes  
        foreach (Control c in parentForm.Controls)
        {
            if (c is CheckBox)
                c.Enabled = true;
        }
    }

    public void SetGroupBox(GroupBox GBox, int GLeft, int GTop, int GWidth, int Gheight, string GText)
    {
        // Set the position and width
        GBox.Text = GText;
        GBox.Top = GTop;
        GBox.Left = GLeft;
        GBox.Width = GWidth;
        GBox.Height = Gheight;
        parentForm.Controls.Add(GBox);
    }
      public void SetTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, Label TLabel, string LText)
    {
      // Set the position, size and the databindings
      TLabel.Left = TLeft;
      TLabel.Text = LText;
      TLabel.Top = TTop;
      TBox.Left = TLeft + 125;
      TBox.Text = "";
      TBox.Top = TTop;
      TBox.Width = TWidth;
      TBox.DataBindings.Clear();
      TBox.DataBindings.Add("Text", dt, TColumnname);
      parentForm.Controls.Add(TBox);
      parentForm.Controls.Add(TLabel);
    }

      public void SetCasedUnLabelledTextBox(int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname,  string charactercase)
      {
          // Set the position, size and the databindings
          TextBox TBox = new TextBox();
          TBox.Text = "";
          TBox.Top = TTop;
          TBox.Left = TLeft;
          TBox.Width = TWidth;
          if (charactercase == "U")
          {
              TBox.CharacterCasing = CharacterCasing.Upper;
          }
          TBox.DataBindings.Clear();
          TBox.DataBindings.Add("Text", dt, TColumnname);
          if (charactercase == "U")
          {
              TBox.CharacterCasing = CharacterCasing.Upper;
          }
          parentForm.Controls.Add(TBox);
      
      }
      public void SetCasedLabelledTextBox(int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, string LText, string charactercase)
      {
          // Set the position, size and the databindings
          TextBox TBox = new TextBox();
          Label TLabel = new Label();
          TLabel.Left = TLeft;
          TLabel.Text = LText;
          TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
          TLabel.Top = TTop;
          TBox.Left = TLeft + 125;
          TBox.Text = "";
          TBox.Top = TTop;
          TBox.Width = TWidth;
          if (charactercase == "U")
          {
              TBox.CharacterCasing = CharacterCasing.Upper;
          }
          TBox.DataBindings.Clear();
          TBox.DataBindings.Add("Text", dt, TColumnname);
          parentForm.Controls.Add(TBox);
          parentForm.Controls.Add(TLabel);
      }
      public void SetLabelledTextBox( int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname,  string LText)
      {
          // Set the position, size and the databindings
          TextBox TBox = new TextBox();
          Label TLabel = new Label();
          TLabel.Left = TLeft;
          TLabel.Text = LText;
          TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
          TLabel.Top = TTop;
          TBox.Left = TLeft + 125;
          TBox.Text = "";
          TBox.Top = TTop;
          TBox.Width = TWidth;
          TBox.DataBindings.Clear();
          TBox.DataBindings.Add("Text", dt, TColumnname);
          parentForm.Controls.Add(TBox);
          parentForm.Controls.Add(TLabel);
      }

      public void SetLabel(int TLeft, int TTop, string LText)
      {
          Label TLabel = new Label();
          TLabel.AutoSize = true;
          TLabel.Left = TLeft;
          TLabel.Text = LText;
          TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
          TLabel.Top = TTop;
          parentForm.Controls.Add(TLabel);
      }

      public void SetUnlabelledTextBox(int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname)
      {
          // Set the position, size and the databindings
          TextBox TBox = new TextBox();
          TBox.Left = TLeft;
          TBox.Text = "";
          TBox.Top = TTop;
          TBox.Width = TWidth;
          TBox.DataBindings.Clear();
          TBox.DataBindings.Add("Text", dt, TColumnname);
          parentForm.Controls.Add(TBox);
      }
    public void SetTextBoxAndLabelText(TextBox TBox, int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, string LText)
    {
        Label TLabel = new Label();
        // Set the position, size and the databindings
        TLabel.Left = TLeft;
        TLabel.Text = LText;
        TLabel.Top = TTop;
        TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
        if (TLabel.Text.Length > 0)
        {
            TBox.Left = TLeft + 125;
            parentForm.Controls.Add(TLabel);
        }
        else
        {
            TBox.Left = TLeft;
        }
        TBox.Text = "";
        TBox.Top = TTop;
        TBox.Width = TWidth;
        TBox.DataBindings.Clear();
        TBox.DataBindings.Add("Text", dt, TColumnname);
        parentForm.Controls.Add(TBox);
        
    }
   
    public virtual void SetEditState(object sender, EventArgs e)
    {
      string editstatus = LockTableRow( currentidcol, currentablename);
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
    public virtual void SaveCurrentDataTable(object sender, EventArgs e)
    {
      // The child's override methods perform that table save.
      currentdatatable.Rows.Clear();
      CurrentState = "Select";
      RefreshControls();
    }
    public virtual void ProcessSelect(object sender, EventArgs e)
    {
     // The child's override method populates the grid and activates the selector form, 
     // which returns the selected id
    }
    public virtual void ProcessDelete(object sender, EventArgs e)
    {

      if (wsgUtilities.wsgReply("Delete this item?"))
      {
        DeleteTablerow( currentablename,currentidcol);
      }
      else
      {
        wsgUtilities.wsgNotice("Deletion Cancelled");
      }
      CurrentState = "Select";
      RefreshControls();
      //The child's override method clears the datatable's rows
    }

    public void SetBitCheckBox(CheckBox CBox, int CLeft, int CTop, int CWidth, DataTable dt, string CColumnname, Label CLabel, string CText)
    {

        // Set the position, size and the databindings. Add to the form
        CLabel.Left = CLeft;
        CLabel.Width = CWidth;
        CLabel.Text = CText.TrimEnd();
        CLabel.Top = CTop +5 ;
        CBox.Left = CLeft +125;
        CBox.Text = "";
        CBox.Top = CTop+1;
        CBox.Width = 15;
        SetCheckBoxBitBinding(CBox, dt, CColumnname);
        parentForm.Controls.Add(CBox);
        parentForm.Controls.Add(CLabel);
       
    }
   public void  SetCheckBoxBitBinding(CheckBox CBox, DataTable dt, string columnname )
    {
        CBox.DataBindings.Clear();
       CBox.DataBindings.Add("checked", dt, columnname);
    }

   public virtual void SetDateTimeTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, Label TLabel, string LText)
   {
       // Set the position, size and the databindings. Add to the form
       TLabel.Left = TLeft;
       TLabel.Text = LText.TrimEnd();
       TLabel.AutoSize = true;
       TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
       TLabel.Top = TTop ;
       TBox.TextAlign = HorizontalAlignment.Left;
       TBox.Left = TLeft + 125;
       TBox.ReadOnly = true;
       TBox.Text = "";
       TBox.Top = TTop;
       TBox.Width = TWidth;
       TBox.ReadOnly = false;
       TBox.MaxLength = 9;
       SetTextBoxDatetimeBinding(TBox, dt, TColumnname);
       parentForm.Controls.Add(TBox);
       parentForm.Controls.Add(TLabel);
   }


   public virtual void SetUnLabelledCurrencyTextBox(int TLeft, int TTop, int TWidth, DataSet ds, string TColumnname)
   {
       TextBox TBox = new TextBox();
       // Set the position, size and the databindings. Add to the form
       TBox.TextAlign = HorizontalAlignment.Right;
       TBox.Left = TLeft;
       TBox.ReadOnly = true;
       TBox.Text = "";
       TBox.Top = TTop;
       TBox.Width = TWidth;
       SetTextBoxCurrencyBinding(TBox, ds, TColumnname);
       parentForm.Controls.Add(TBox);
   }

   public virtual void SetLabelledCurrencyTextBox( int TLeft, int TTop, int TWidth, DataSet ds, string TColumnname, string LText)
   {
       TextBox TBox = new TextBox();
       // Set the position, size and the databindings. Add to the form
       Label TLabel = new Label();
       TLabel.Left = TLeft;
       TLabel.Text = LText.TrimEnd();
       TLabel.AutoSize = true;
       TLabel.Top = TTop;
       TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
       TBox.TextAlign = HorizontalAlignment.Right;
       TBox.Left = TLeft + 130;
       TBox.ReadOnly = true;
       TBox.Text = "";
       TBox.Top = TTop;
       TBox.Width = TWidth;
       SetTextBoxCurrencyBinding(TBox, ds, TColumnname);
       parentForm.Controls.Add(TBox);
       parentForm.Controls.Add(TLabel);
   }

    public virtual void SetCurrencyTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataSet ds, string TColumnname, Label TLabel, string LText)
    {
        // Set the position, size and the databindings. Add to the form
        TLabel.Left = TLeft;
        TLabel.Text = LText.TrimEnd();
        TLabel.AutoSize = true;
        TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
        TLabel.Top = TTop - 20;
        TBox.TextAlign = HorizontalAlignment.Right;
        TBox.Left = TLeft;
        TBox.ReadOnly = true;
        TBox.Text = "";
        TBox.Top = TTop;
        TBox.Width = TWidth;
        SetTextBoxCurrencyBinding(TBox, ds, TColumnname);
        parentForm.Controls.Add(TBox);
        parentForm.Controls.Add(TLabel);
    }

    public void SetLabelledNumericUpDown( int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, string LText, int decimalplaces, decimal maxvalue)
    {
        // Set the position, size and the databindings
        Label TLabel = new Label();
        NumericUpDown NupDown = new NumericUpDown();
        TLabel.Left = TLeft;
        TLabel.Text = LText;
        TLabel.Top = TTop;
        TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
        NupDown.Left = TLeft + 125;
        NupDown.Top = TTop;
        NupDown.ThousandsSeparator = true;
        NupDown.TextAlign = HorizontalAlignment.Right;
        NupDown.Width = TWidth;
        NupDown.Maximum = maxvalue;
        NupDown.DataBindings.Clear();
        NupDown.DataBindings.Add("Value", dt, TColumnname, true, DataSourceUpdateMode.OnPropertyChanged);
        parentForm.Controls.Add(NupDown);
        parentForm.Controls.Add(TLabel);
    }

    public void SetUnlabelledNumericUpDown(int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, int decimalplaces, decimal maxvalue)
    {
        // Set the position, size and the databindings
        Label TLabel = new Label();
        NumericUpDown NupDown = new NumericUpDown();
        NupDown.Left = TLeft;
        NupDown.Top = TTop;
        NupDown.ThousandsSeparator = true;
        NupDown.TextAlign = HorizontalAlignment.Right;
        NupDown.Width = TWidth;
        NupDown.Maximum = maxvalue;
        NupDown.DataBindings.Clear();
        NupDown.DataBindings.Add("Value", dt, TColumnname, true, DataSourceUpdateMode.OnPropertyChanged);
        parentForm.Controls.Add(NupDown);
        parentForm.Controls.Add(TLabel);
    }

    public void SetNumericUpDown(NumericUpDown NupDown, int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, Label TLabel, string LText)
    {
        // Set the position, size and the databindings
        TLabel.Left = TLeft;
        TLabel.Text = LText;
        TLabel.Top = TTop;
        TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
        NupDown.Left = TLeft + 125;
        NupDown.Top = TTop;
        NupDown.Maximum = decimal.MaxValue;
        NupDown.ThousandsSeparator = true;
        NupDown.TextAlign = HorizontalAlignment.Right;
        NupDown.Width = TWidth;
        NupDown.DataBindings.Clear();
        NupDown.DataBindings.Add("Value", dt, TColumnname, true, DataSourceUpdateMode.OnPropertyChanged);
        parentForm.Controls.Add(NupDown);
        parentForm.Controls.Add(TLabel);
    }

    public void EnableInputControls()
    {
       

        foreach (Control c in parentForm.Controls)
        {
            if ((c is TextBox))
            {
               (c as TextBox).ReadOnly = false;
                c.Visible = true;
                c.Enabled = true;
                continue;
            }
            if ((c is ComboBox))
            {
                c.Visible = true;
                c.Enabled = true;
                continue;
            }
            if ((c is NumericUpDown))
            {
                c.Visible = true;
                c.Enabled = true;
                continue;
            }

        }
    }
    public void SetUnboundComboBox(ComboBox CBox, int CLeft, int CTop, int CWidth, Label CLabel, string CText)
    {
        // Set the position, size and the databindings
        CLabel.Left = CLeft;
        CLabel.Text = CText;
        CLabel.Top = CTop;
        CBox.Left = CLeft + 125;
        CBox.Top = CTop;
        CBox.Width = CWidth;
   
        parentForm.Controls.Add(CBox);
        parentForm.Controls.Add(CLabel);
    }
    public void SetUnboundListBox(ListBox LBox, int LLeft, int LTop, int LWidth, int LHeight, Label LLabel, string LText)
    {
        // Set the position, size and the databindings
        LLabel.Left = LLeft;
        LLabel.Text = LText;
        LLabel.Top = LTop;
        LBox.Left = LLeft + 125;
        LBox.Top = LTop;
        LBox.Width = LWidth;
        LBox.Height = LHeight;
   
        parentForm.Controls.Add(LBox);
        parentForm.Controls.Add(LLabel);
    }

    private void DatetimeToString(object sender, ConvertEventArgs cevent)
    {
        // The method converts only to string type. Test this using the DesiredType.
        if (cevent.DesiredType != typeof(string)) return;

        // Use the ToString method to format the value as currency ("c").
        cevent.Value = ((DateTime)cevent.Value).ToString("MM/dd/yy");
    }


    private void DateTimeStringToDateTime(object sender, ConvertEventArgs cevent)
    {
        // The method converts back to decimal type only. 
        if (cevent.DesiredType != typeof(DateTime)) return;

        // Converts the string back to decimal using the static Parse method.
        cevent.Value = DateTime.Parse(cevent.Value.ToString());
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


    private void SetTextBoxDatetimeBinding(TextBox txtbox,  DataTable dt, string fieldname)
    {

        Binding b = new Binding("Text", dt, fieldname);
        {
            b.Format += new ConvertEventHandler(DatetimeToString);
            b.Parse += new ConvertEventHandler(DateTimeStringToDateTime);
        }
        txtbox.DataBindings.Clear();
        txtbox.DataBindings.Add(b);

    }


    private void SetTextBoxCurrencyBindingT(TextBox txtbox, DataTable dt, string fieldname)
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

    public void SetLabelledDollarsTextBox( int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, string LText)
    {
        TextBox TBox = new TextBox();
        Label TLabel = new Label();
        // Set the position, size and the databindings. Add to the form
        TLabel.Left = TLeft;
        TLabel.AutoSize = true;
        TLabel.Top = TTop;
        TLabel.Text = LText.TrimEnd();
        TLabel.TextAlign = ContentAlignment.MiddleLeft;
        TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
        TBox.TextAlign = HorizontalAlignment.Right;
        TBox.Left =  TLeft + 130;
        TBox.ReadOnly = true;
        TBox.Text = "";
        TBox.Top = TTop;
        TBox.Width = TWidth;
        SetTextBoxDollarsBinding(TBox, dt, TColumnname);
        parentForm.Controls.Add(TBox);
        parentForm.Controls.Add(TLabel);
    }
    public void SetDollarsTextBox(TextBox TBox, int TLeft, int TTop, int TWidth, DataTable dt, string TColumnname, Label TLabel, string LText)
    {
        // Set the position, size and the databindings. Add to the form
        TLabel.Left = TLeft;
        TLabel.Width = TWidth;
        TLabel.Text = LText.TrimEnd();
        TLabel.TextAlign = ContentAlignment.MiddleCenter;
        TLabel.Font = new Font(TLabel.Font, FontStyle.Bold);
        TLabel.Top = TTop - 20;
        TBox.TextAlign = HorizontalAlignment.Right;
        TBox.Left = TLeft;
        TBox.ReadOnly = true;
        TBox.Text = "";
        TBox.Top = TTop;
        TBox.Width = TWidth;
        SetTextBoxDollarsBinding(TBox, dt, TColumnname);
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
        CLabel.Font = new Font(CLabel.Font, FontStyle.Bold);
        CBox.ValueMember = ValueMember;
        CBox.DisplayMember = DisplayMember;
        CBox.DataSource = Dt;
        parentForm.Controls.Add(CBox);
        parentForm.Controls.Add(CLabel);
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

  }
}
