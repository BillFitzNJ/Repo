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
    class RegionAssignmentMethods : WSGDataAccess
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
        string Regioncode = "";
        Button ButtonAssignstores = new Button();
        Button ButtonReviewunassignstores = new Button();
        MySQLDataMethods mySQLDataMethods = new MySQLDataMethods();
        MySQLDataMethods unassignedmySQLDataMethods = new MySQLDataMethods();
        MySQLDataMethods assignedmySQLDataMethods = new MySQLDataMethods();
        FrmRegionAssignment frmRegionAssignment = new FrmRegionAssignment();
        public RegionAssignmentMethods()
            : base("SQL", "SQLConnString")
        {
        }

        public void StartApp(string compcode, string regioncode)
        {
            Compcode = compcode;
            Regioncode = regioncode;
            unassignedmySQLDataMethods.GetUnassignedCompanyStores(Compcode);
            assignedmySQLDataMethods.GetassignedRegionStores(Compcode, Regioncode);
            SetBindings();
            SetEvents();
            frmRegionAssignment.ShowDialog();
            return;
        }

        public void SetEvents()
        {
            frmRegionAssignment.listBoxUnassigned.DoubleClick += new System.EventHandler(listBoxUnassigned_DoubleClick);
            frmRegionAssignment.listBoxAssigned.DoubleClick += new System.EventHandler(listBoxAssigned_DoubleClick);
            frmRegionAssignment.buttonSave.Click += new System.EventHandler(buttonSave_Click);
            frmRegionAssignment.buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
        }
        private void listBoxAssigned_DoubleClick(object sender, EventArgs e)
        {
            if (assignedmySQLDataMethods.mysqlselectords.expandedstore.Rows.Count > 0)
            {
                if (wsgUtilities.wsgReply("Remove this item?"))
                {
                    int rowcount = unassignedmySQLDataMethods.mysqlselectords.expandedstore.Rows.Count - 1;
                    unassignedmySQLDataMethods.mysqlselectords.expandedstore.Rows.Add();
                    if (assignedmySQLDataMethods.mysqlselectords.expandedstore.Rows.Count > 0)
                    {
                        unassignedmySQLDataMethods.mysqlselectords.expandedstore[rowcount].storecode = assignedmySQLDataMethods.mysqlselectords.expandedstore[frmRegionAssignment.listBoxAssigned.SelectedIndex].storecode;
                        unassignedmySQLDataMethods.mysqlselectords.expandedstore[rowcount].location = assignedmySQLDataMethods.mysqlselectords.expandedstore[frmRegionAssignment.listBoxAssigned.SelectedIndex].location; ;
                        assignedmySQLDataMethods.mysqlselectords.expandedstore.Rows[frmRegionAssignment.listBoxAssigned.SelectedIndex].Delete();
                        assignedmySQLDataMethods.mysqlselectords.expandedstore.AcceptChanges();
                        unassignedmySQLDataMethods.mysqlselectords.expandedstore.AcceptChanges();
                    }
                }

            }
            else
            {
                wsgUtilities.wsgNotice("There are no assigned money centers");
            }
        }

        private void listBoxUnassigned_DoubleClick(object sender, EventArgs e)
        {
            if (unassignedmySQLDataMethods.mysqlselectords.expandedstore.Rows.Count > 0)
            {
                assignedmySQLDataMethods.mysqlselectords.expandedstore.Rows.Add();
                int rowcount = assignedmySQLDataMethods.mysqlselectords.expandedstore.Rows.Count - 1;
                assignedmySQLDataMethods.mysqlselectords.expandedstore[rowcount].storecode = unassignedmySQLDataMethods.mysqlselectords.expandedstore[frmRegionAssignment.listBoxUnassigned.SelectedIndex].storecode; ;
                assignedmySQLDataMethods.mysqlselectords.expandedstore[rowcount].location = unassignedmySQLDataMethods.mysqlselectords.expandedstore[frmRegionAssignment.listBoxUnassigned.SelectedIndex].location; ;
                unassignedmySQLDataMethods.mysqlselectords.expandedstore.Rows[frmRegionAssignment.listBoxUnassigned.SelectedIndex].Delete();
                assignedmySQLDataMethods.mysqlselectords.expandedstore.AcceptChanges();
                unassignedmySQLDataMethods.mysqlselectords.expandedstore.AcceptChanges();
            }
            else
            {
                wsgUtilities.wsgNotice("There are no unassigned money centers");
            }
        }
        public void SetBindings()
        {
            frmRegionAssignment.listBoxAssigned.ValueMember = "storecode";
            frmRegionAssignment.listBoxAssigned.DisplayMember = "location";
            frmRegionAssignment.listBoxAssigned.DataSource = assignedmySQLDataMethods.mysqlselectords.expandedstore;
            frmRegionAssignment.listBoxUnassigned.ValueMember = "storecode";
            frmRegionAssignment.listBoxUnassigned.DisplayMember = "location";
            frmRegionAssignment.listBoxUnassigned.DataSource = unassignedmySQLDataMethods.mysqlselectords.expandedstore;
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (wsgUtilities.wsgReply("Abandon Edit"))
            {
                frmRegionAssignment.Close();
            }
        }


        private void buttonSave_Click(object sender, EventArgs e)
        {
            mySQLDataMethods.ClearRegionStores(Compcode, Regioncode);
            mySQLDataMethods.mysqlds.storeregion.Rows.Clear();
            mySQLDataMethods.mysqlds.storeregion.Rows.Add();
            EstablishBlankDataTableRow(mySQLDataMethods.mysqlds.storeregion);
            mySQLDataMethods.mysqlds.storeregion[0].regioncode = Regioncode;
            for (int r = 0; r <= assignedmySQLDataMethods.mysqlselectords.expandedstore.Rows.Count -1; r++)
            {
                mySQLDataMethods.mysqlds.storeregion[0].storecode = assignedmySQLDataMethods.mysqlselectords.expandedstore[r].storecode;
                mySQLDataMethods.SaveMySQLDatatableRow(mySQLDataMethods.mysqlds.storeregion[0]);
            }
            wsgUtilities.wsgNotice("Operation Complete");
            frmRegionAssignment.Close();
        }
    }
}
