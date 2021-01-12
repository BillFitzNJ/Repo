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
    public class MaintainRateScheduleMethods : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain Recurring Charges");
        ssprocess ssprocessds = new ssprocess();
        ssprocess ssprocessselectords = new ssprocess();
        Label labelFrequencyDescription = new Label();
        int tabIndex = 0;
        string commandtext = "";
        NumericUpDown numericUpDownChgAmt = new NumericUpDown();
        TextBox TextBoxChgAmt = new TextBox();
        Label LabelChgAmt = new Label();
        TextBox TexBoxChgfreq = new TextBox();
        ComboBox ComboBoxChargeType = new ComboBox();
        Label LabelChargeType = new Label();
        DataGridView dataGridViewRateScheduleSelector = new DataGridView();
        BindingSource bindingSelectorData = new BindingSource();

        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentdatatable = ssprocessds.salesperson;
            currentablename = "rateschedule";
            SetIdcol(ssprocessds.rateschedule.idcolColumn);
            parentForm.Text = "Maintain Rate Schedule";

        }
        public override void SetControls()
        {
            parentForm.buttonDelete.Visible = true;
            int TextTop = 100;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 600;
            SetLabelledTextBox(LeftStart, TextTop, 200, ssprocessds.rateschedule, "schedulename", "Schedule Name");
            TextTop += 22;
            TextTop += 22;
            SetLabel(LeftStart, TextTop, "Processing Charge per $100");
            TextTop += 22;
            SetLabelledDollarsTextBox(LeftStart, TextTop, 100, ssprocessds.rateschedule, "tierfourmin", "Tier 4");
            SetUnLabelledCurrencyTextBox(LeftStart + 250, TextTop, 50, ssprocessds, "rateschedule.tierfourrate");
            TextTop += 22;
            SetLabelledDollarsTextBox(LeftStart, TextTop, 100, ssprocessds.rateschedule, "tierthreemin", "Tier 3");
            SetUnLabelledCurrencyTextBox(LeftStart + 250, TextTop, 50, ssprocessds, "rateschedule.tierthreerate");
            TextTop += 22;
            SetLabelledDollarsTextBox(LeftStart, TextTop, 100, ssprocessds.rateschedule, "tiertwomin", "Tier 2");
            SetUnLabelledCurrencyTextBox(LeftStart + 250, TextTop, 50, ssprocessds, "rateschedule.tiertworate");
            TextTop += 22;
            SetLabelledDollarsTextBox(LeftStart, TextTop, 100, ssprocessds.rateschedule, "tieronemin", "Tier 1");
            SetUnLabelledCurrencyTextBox(LeftStart + 250, TextTop, 50, ssprocessds, "rateschedule.tieronerate");
            TextTop += 22;
            TextTop += 22;
            SetLabelledCurrencyTextBox(LeftStart, TextTop, 100, ssprocessds, "rateschedule.depoticketfee", "Deposit Ticket Fee");
            TextTop += 22;
            SetLabelledCurrencyTextBox(LeftStart, TextTop, 100, ssprocessds, "rateschedule.coinorder", "Change Order");
            TextTop += 22;
            SetLabelledCurrencyTextBox(LeftStart, TextTop, 100, ssprocessds, "rateschedule.rollrate", "Roll Rate");
            TextTop += 22;
            SetLabelledCurrencyTextBox(LeftStart, TextTop, 100, ssprocessds, "rateschedule.straprate", "Strap Rate");
            TextTop += 22;
            TextTop += 22;
            SetLabelledCurrencyTextBox(LeftStart, TextTop, 100, ssprocessds, "rateschedule.currencydropfee", "Delivery Fee");
            TextTop += 22;
            SetLabelledCurrencyTextBox(LeftStart, TextTop, 100, ssprocessds, "rateschedule.pickuprate", "Armored Pickup Rate");
            TextTop += 22;
            SetLabelledCurrencyTextBox(LeftStart, TextTop, 100, ssprocessds, "rateschedule.fuelsurcharge", "Fuel Surcharge");
            TextTop += 22;
            SetLabelledCurrencyTextBox(LeftStart, TextTop, 100, ssprocessds, "rateschedule.adjustmentfee", "Adjustments");
            TextTop += 22;
        
        }


        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (cont)
            {
                GenerateAppTableRowSave(ssprocessds.rateschedule[0]);
                CurrentState = "View";
                RefreshControls();
            }
        }

        public override void ProcessSelect(object sender, EventArgs e)
        {
            currentidcol = commonAppDataMethods.SelectRateSchedule();
            if (currentidcol > 0)
            {
                ssprocessds.rateschedule.Rows.Clear();
                commandtext = "SELECT * FROM  rateschedule WHERE idcol = @idcol";
                ClearParameters();
                AddParms("@idcol", currentidcol, "SQL");
                FillData(ssprocessds, "rateschedule", commandtext, CommandType.Text);
                CurrentState = "View";
                RefreshControls();
            }
        
        }
        public override void SetEvents()
        {
            base.SetEvents();
     
        }
        public override void SetEditState(object sender, EventArgs e)
        {
            string editstatus = base.LockTableRow(ssprocessds.rateschedule[0].idcol, "rateschedule");
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

        public override void SetInsertState(object sender, EventArgs e)
        {
            EstablishBlankDataTableRow(ssprocessds.rateschedule);
            CurrentState = "Insert";
            RefreshControls();
        }
    
        public override void ProcessCancel(object sender, EventArgs e)
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

                    CurrentState = "StoreSelected";
                    RefreshControls();
                }
            }
            else
            {
                if (CurrentState == "View")
                {
                    ssprocessds.rateschedule.Rows.Clear();
                    CurrentState = "Select";
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
            CurrentState = "Select";
            RefreshControls();
            ssprocessds.rateschedule.Rows.Clear();
            parentForm.Update();
        }
        public override void RefreshControls()
        {
            base.RefreshControls();
            switch (CurrentState)
            {

                case "Select":
                    {
                        break;
                    }


                case "View":
                    {
                        parentForm.buttonEdit.Enabled = true;
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

     
    }

}
