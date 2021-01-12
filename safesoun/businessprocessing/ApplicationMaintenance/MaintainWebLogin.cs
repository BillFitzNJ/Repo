using System;
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
    public class MaintainWebLogin : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Maintain User");
        string commandtext = "";
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();
        mysqldata mysqlds = new mysqldata();
        TextBox TextBoxUserid = new TextBox();
        Label LabelUserid = new Label();
        TextBox TextBoxPassword = new TextBox();
        Label LabelPassword = new Label();
        TextBox TextBoxUsername = new TextBox();
        Label LabelUsername = new Label();
        TextBox TextBoxEmailaddress = new TextBox();
        Label LabelEmailaddress = new Label();
        Label LabelDrivername = new Label();
        Button ButtonDriver = new Button();
        Label LabelStorename = new Label();
        Button ButtonStore = new Button();
        Label LabelCompanyname = new Label();
        Button ButtonCompany = new Button();
        Label LabelRegionname = new Label();
        Button ButtonRegion = new Button();
        TextBox TextBoxSequencestops = new TextBox();
        Label LabelSequencestops = new Label();
        TextBox TextBoxAccessSafeData = new TextBox();
        Label LabelAccessSafeData = new Label();


        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        public override void EstablishAppConstants()
        {
            parentForm.buttonDelete.Visible = false;
            currentablename = "user";
            parentForm.Text = "Maintain Web Login";
            currentdatatable = mySQLDataMethods.mysqlds.user;
        }

        public override void SetEvents()
        {
            ButtonDriver.Click += new System.EventHandler(SelectDriver);
            ButtonStore.Click += new System.EventHandler(SelectStore);
            ButtonCompany.Click += new System.EventHandler(SelectCompany);
            ButtonRegion.Click += new System.EventHandler(ProcesSelectRegion);
            base.SetEvents();
        }
        public override void SetControls()
        {
            LabelDrivername.AutoSize = true;
            int TextTop = 50;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 350;
            SetTextBox(TextBoxUserid, LeftStart, TextTop, 100, mySQLDataMethods.mysqlds.user, "userid", LabelUserid, "Userid");
            TextBoxUserid.CharacterCasing = CharacterCasing.Upper;
            TextBoxUserid.MaxLength = 15;
            TextTop += 22;
            SetTextBox(TextBoxUsername, LeftStart, TextTop, 200, mySQLDataMethods.mysqlds.user, "username", LabelUsername, "User Name");
            TextBoxUsername.MaxLength = 45;
            TextTop += 22;
            SetTextBox(TextBoxPassword, LeftStart, TextTop, 200, mySQLDataMethods.mysqlds.user, "password", LabelPassword, "Password");
            TextBoxPassword.MaxLength = 35;
            TextTop += 22;
            SetTextBox(TextBoxEmailaddress, LeftStart, TextTop, 200, mySQLDataMethods.mysqlds.user, "emailaddress", LabelEmailaddress, "Email Address");
            TextBoxPassword.MaxLength = 35;
            TextTop += 22;
            SetTextBox(TextBoxSequencestops, LeftStart, TextTop, 20, mySQLDataMethods.mysqlds.user, "sequencestops", LabelSequencestops, "Sequence Stops");
            TextTop += 22;
            SetTextBox(TextBoxAccessSafeData, LeftStart, TextTop, 20, mySQLDataMethods.mysqlds.user, "access_safe_data", LabelAccessSafeData, "Access Safes");
            TextTop += 22;
          
            ButtonDriver.Text = "Driver";
            ButtonDriver.Height = parentForm.buttonSave.Height;
            ButtonDriver.Width = 100;
            ButtonDriver.Top = TextTop;
            ButtonDriver.Left = TextBoxUsername.Left;
            parentForm.Controls.Add(ButtonDriver);
            LabelDrivername.Top = TextTop + 5;
            LabelDrivername.Left = ButtonDriver.Left + ButtonDriver.Width + 15;
            parentForm.Controls.Add(LabelDrivername);
            TextTop += 22;
            ButtonStore.Text = "Store";
            ButtonStore.Height = parentForm.buttonSave.Height;
            ButtonStore.Width = 100;
            ButtonStore.Top = TextTop;
            ButtonStore.Left = TextBoxUsername.Left;
            parentForm.Controls.Add(ButtonStore);
            LabelStorename.Top = TextTop + 5;
            LabelStorename.Left = ButtonStore.Left + ButtonStore.Width + 15;
            LabelStorename.Width = 300;
            parentForm.Controls.Add(LabelStorename);
            TextTop += 22;
            ButtonCompany.Text = "Company";
            ButtonCompany.Height = parentForm.buttonSave.Height;
            ButtonCompany.Width = 100;
            ButtonCompany.Top = TextTop;
            ButtonCompany.Left = TextBoxUsername.Left;
            parentForm.Controls.Add(ButtonCompany);
            LabelCompanyname.Top = TextTop + 5;
            LabelCompanyname.Left = ButtonCompany.Left + ButtonCompany.Width + 15;
            parentForm.Controls.Add(LabelCompanyname);
            TextTop += 22;
            ButtonRegion.Text = "Region";
            ButtonRegion.Height = parentForm.buttonSave.Height;
            ButtonRegion.Width = 100;
            ButtonRegion.Top = TextTop;
            ButtonRegion.Left = TextBoxUsername.Left;
            parentForm.Controls.Add(ButtonRegion);
            LabelRegionname.Top = TextTop + 5;
            LabelRegionname.Left = ButtonRegion.Left + ButtonCompany.Width + 15;
            parentForm.Controls.Add(LabelRegionname);
        }
        public override void ProcessSelect(object sender, EventArgs e)
        {
            mySQLDataMethods.GetUsers();

            FrmSelector frmSelector = new FrmSelector();
            FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
            frmSelectorMethods.FormText = "Select User";
            frmSelectorMethods.dtSource = mySQLDataMethods.mysqlselectords.user;
            frmSelectorMethods.columncount = 2;
            frmSelectorMethods.SetColumns();
            frmSelectorMethods.colname[0] = "UserdIdcol";
            frmSelectorMethods.colheadertext[0] = "User ID";
            frmSelectorMethods.coldatapropertyname[0] = "userid";
            frmSelectorMethods.colwidth[0] = 100;
            frmSelectorMethods.colname[1] = "Namecol";
            frmSelectorMethods.colheadertext[1] = "User Name";
            frmSelectorMethods.coldatapropertyname[1] = "username";
            frmSelectorMethods.colwidth[1] = 250;
            frmSelectorMethods.SetGrid();
            frmSelector.Width = 700;
            int useridcol = frmSelectorMethods.ShowSelector();
            if (useridcol > 0)
            {

                mySQLDataMethods.GetSingleUser(useridcol);
                RefreshLabels();
            }
            CurrentState = "View";
            RefreshControls();
        }

        public void ProcesSelectRegion(object sender, EventArgs e)
        {
            if (mySQLDataMethods.mysqlds.user[0].compcode.TrimEnd() != "")
            {
                mySQLDataMethods.mysqlds.user[0].regioncode = commonAppDataMethods.SelectRegion(mySQLDataMethods.mysqlds.user[0].compcode);
            }
            else
            {
                wsgUtilities.wsgNotice("Select a company");

            }
            RefreshLabels();
        }

        public void SelectDriver(object sender, EventArgs e)
        {
            mySQLDataMethods.mysqlds.user[0].driver = commonAppDataMethods.SelectDriver();
            RefreshLabels();
        }

        public void SelectCompany(object sender, EventArgs e)
        {
            mySQLDataMethods.mysqlds.user[0].compcode = commonAppDataMethods.SelectCompany();
            if (mySQLDataMethods.mysqlds.user[0].compcode.TrimEnd() == "")
            {
                mySQLDataMethods.mysqlds.user[0].regioncode = "";
            }
            RefreshLabels();
        }


        public void SelectStore(object sender, EventArgs e)
        {
            mySQLDataMethods.mysqlds.user[0].storecode = commonAppDataMethods.SelectCompanyAndStore();
            RefreshLabels();
        }


        public void RefreshLabels()
        {
            LabelDrivername.Text = commonAppDataMethods.GetDriverName(mySQLDataMethods.mysqlds.user[0].driver);
            LabelStorename.Text = commonAppDataMethods.GetStoreName(mySQLDataMethods.mysqlds.user[0].storecode);
            LabelCompanyname.Text = commonAppDataMethods.GetCompanyName(mySQLDataMethods.mysqlds.user[0].compcode);
            LabelRegionname.Text = mySQLDataMethods.GetRegionName(mySQLDataMethods.mysqlds.user[0].compcode, mySQLDataMethods.mysqlds.user[0].regioncode);
        }

        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
           
            
            bool cont = true;

            if (mySQLDataMethods.mysqlds.user[0].userid.TrimEnd() == "" )
            {
                wsgUtilities.wsgNotice("There must be a user ID");
                cont = false;
            }

            if (CurrentState == "Insert")
            { 
              // Check for existence of the ID
                if (mySQLDataMethods.UseridExists(mySQLDataMethods.mysqlds.user[0].userid))
                { 
                  wsgUtilities.wsgNotice("User " + mySQLDataMethods.mysqlds.user[0].userid.TrimEnd() + " is already in the file" );
                  cont = false;
                }
              
            }
           
            if (mySQLDataMethods.mysqlds.user[0].username.TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("There must be a user name");
                cont = false;
            }
            if (mySQLDataMethods.mysqlds.user[0].password.TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("There must be a password");
                cont = false;
            }

            if (mySQLDataMethods.mysqlds.user[0].driver.TrimEnd() != "" && (mySQLDataMethods.mysqlds.user[0].storecode.TrimEnd() != "" || mySQLDataMethods.mysqlds.user[0].compcode.TrimEnd() != ""))
            {
                wsgUtilities.wsgNotice("A driver cannot be a store of a company");
                cont = false;
            }
            if (mySQLDataMethods.mysqlds.user[0].storecode.TrimEnd() != "" && mySQLDataMethods.mysqlds.user[0].compcode.TrimEnd() != "")
            {
                wsgUtilities.wsgNotice("A store cannot be a company");
                cont = false;
            }
            if (mySQLDataMethods.mysqlds.user[0].regioncode.TrimEnd() != "" && mySQLDataMethods.mysqlds.user[0].compcode.TrimEnd() == "")
            {
                wsgUtilities.wsgNotice("A region must be associated withe a company");
                cont = false;
            }


            if (cont)
            {
                mySQLDataMethods.SaveMySQLDatatableRow(mySQLDataMethods.mysqlds.user[0]);
            }
            CurrentState = "View";
            RefreshControls();
        }

        public override void SetInsertState(object sender, EventArgs e)
        {
            mySQLDataMethods.mysqlds.user.Rows.Add();
            EstablishBlankDataTableRow(mySQLDataMethods.mysqlds.user);
            mySQLDataMethods.mysqlds.user[0].sequencestops = "N";
            mySQLDataMethods.mysqlds.user[0].access_safe_data = "N";
            CurrentState = "Insert";
            RefreshControls();
        }

        public override void SetEditState(object sender, EventArgs e)
        {
            CurrentState = "Edit";
            RefreshControls();
        }
        public override void RefreshControls()
        {
            base.RefreshControls();

            switch (CurrentState)
            {
                case "Select":
                    {
                        LabelDrivername.Text = "";
                        LabelStorename.Text = "";
                        LabelCompanyname.Text = "";
                        break;
                    }

                case "Edit":
                    {
                        ButtonDriver.Enabled = true;
                        ButtonStore.Enabled = true;
                        ButtonCompany.Enabled = true;
                        ButtonRegion.Enabled = true;
                        break;
                    }
                case "Insert":
                    {
                        ButtonDriver.Enabled = true;
                        ButtonStore.Enabled = true;
                        ButtonCompany.Enabled = true;
                        ButtonRegion.Enabled = true;
                        break;
                    }

            }

        }

        public override void ProcessCancel(object sender, EventArgs e)
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
                    currentdatatable.Rows.Clear();
                    CurrentState = "Select";
                    RefreshControls();
                }
            }
        }
    }

}