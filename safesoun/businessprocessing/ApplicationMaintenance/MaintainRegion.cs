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
    public class MaintainRegion : FrmMaintainSingleTableMethods
    {
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        AppConstants myAppconstants = new AppConstants();

        WSGUtilities wsgUtilities = new WSGUtilities("Maintain User");
        string Compcode = "";
        string commandtext = "";
        // Establish the dataset
        ssprocess ssprocessds = new ssprocess();
        mysqldata mysqlds = new mysqldata();
        FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
        TextBox TextBoxRegioncode = new TextBox();
        Label LabelRegioncode = new Label();
        TextBox TextBoxRegionname = new TextBox();
        Label LabelRegionname = new Label();
        Button ButtonAssignstores = new Button();
      

        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        public override void EstablishAppConstants()
        {
            SetIdcol(mySQLDataMethods.mysqlds.region.idcolColumn);
            currentablename = "Region";
            parentForm.Text = "Maintain Web Region";
            currentdatatable = mySQLDataMethods.mysqlds.region;
        }

        public override void SetEvents()
        {
            ButtonAssignstores.Click += new System.EventHandler(ProcessAssignRegionStores);
           
            base.SetEvents();
        }
        public override void RefreshControls()
        {
            base.RefreshControls();
            ButtonAssignstores.Enabled = false;
        
            switch (CurrentState)
            {
                case "Select":
                    {
                        break;
                    }

                case "Edit":
                    {
                        ButtonAssignstores.Enabled = true;
                        break;
                    }
            }

        }



        public override void SetControls()
        {
            int TextTop = 50;
            int LeftStart = 50;
            parentForm.Width = 700;
            parentForm.Height = 350;
            SetTextBox(TextBoxRegioncode, LeftStart, TextTop, 100, mySQLDataMethods.mysqlds.region, "regioncode", LabelRegioncode, "Region Code");
            TextBoxRegioncode.CharacterCasing = CharacterCasing.Upper;
            TextBoxRegioncode.MaxLength = 10;
            parentForm.Controls.Add(TextBoxRegioncode);
            TextTop += 22;
            SetTextBox(TextBoxRegionname, LeftStart, TextTop, 100, mySQLDataMethods.mysqlds.region, "regionname", LabelRegionname, "Region Name");
            parentForm.Controls.Add(TextBoxRegionname);
            TextTop += 22;
            ButtonAssignstores.Text = "Assign Stores";
            ButtonAssignstores.Height = parentForm.buttonSave.Height;
            ButtonAssignstores.Width = 100;
            ButtonAssignstores.Top = TextTop;
            ButtonAssignstores.Left = TextBoxRegionname.Left;
            parentForm.Controls.Add(ButtonAssignstores);
            TextTop += 25;
        
        }
        public override void SetEditState(object sender, EventArgs e)
        {
            CurrentState = "Edit";
            RefreshControls();
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
        public override void ShowParent()
        {
            Compcode = commonAppDataMethods.SelectActiveCompany();
            if (Compcode.TrimEnd() != "")
            {
                parentForm.Text = "Regions for: " + commonAppDataMethods.GetCompanyName(Compcode);
                base.ShowParent();
            }
        }
        public override void SaveCurrentDataTable(object sender, EventArgs e)
        {
            bool cont = true;

            if (cont)
            {
                mySQLDataMethods.SaveMySQLDatatableRow(mySQLDataMethods.mysqlds.region[0]);
            }
            CurrentState = "View";
            RefreshControls();
        }
        public override void SetInsertState(object sender, EventArgs e)
        {
            mySQLDataMethods.mysqlds.region.Rows.Add();
            EstablishBlankDataTableRow(mySQLDataMethods.mysqlds.region);
            mySQLDataMethods.mysqlds.region[0].compcode = Compcode;
            CurrentState = "Insert";
            RefreshControls();
        }

       public override void ProcessDelete(object sender, EventArgs e)
        {
            if (wsgUtilities.wsgReply("All assigned stores will be removed fromt his region. Delete this item?"))
            {

                mySQLDataMethods.DeleteRegion(mySQLDataMethods.mysqlds.region[0].compcode, mySQLDataMethods.mysqlds.region[0].regioncode);
                mySQLDataMethods.mysqlds.region.Rows.Clear();
                mySQLDataMethods.mysqlds.region.AcceptChanges();
                wsgUtilities.wsgNotice("Item Deleted");
                CurrentState = "Select";
                RefreshControls();
            }
            else
            {
                wsgUtilities.wsgNotice("Deletion cancelled");

            }

        }
        public void ProcessAssignRegionStores(object sender, EventArgs e)
        {
            RegionAssignmentMethods regionAssignmentMethods = new RegionAssignmentMethods();
            regionAssignmentMethods.StartApp(Compcode,mySQLDataMethods.mysqlds.region[0].regioncode );

        }

       
        public override void ProcessSelect(object sender, EventArgs e)
        {
            string regioncode = commonAppDataMethods.SelectRegion(Compcode);
            if (regioncode.TrimEnd() != "")
            {
                mySQLDataMethods.GetRegion(Compcode, regioncode);
                CurrentState = "View";
                RefreshControls();
            }
        }
    }
}