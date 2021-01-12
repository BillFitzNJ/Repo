using System;
using System.Collections.Generic;
using CommonAppClasses;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
namespace ApplicationMaintenance
{


    public class CompanyMoneycenterAssignmentMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Moneycenter Assignment");
        AppUtilities appUtilities = new AppUtilities();
        public Form menuform { get; set; }
        public bool cont;
        FrmMoneycenterAssignment frmMoneyCenterAssignment = new FrmMoneycenterAssignment();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess assignedssprocessds = new ssprocess();
        ssprocess unassignedssprocessds = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        string commandstring = "";
        string comp_code = "";
        public CompanyMoneycenterAssignmentMethods()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.compmoneycenter.idcolColumn);
        }

        public void StartApp()
        {
            SetEvents();
            SetBindings();
            ProcessApp();
        }
        public void SetEvents()
        {
            frmMoneyCenterAssignment.listBoxAssigned.MouseDown += new MouseEventHandler(listBoxAssignedMouseClick);
            frmMoneyCenterAssignment.listBoxUnassigned.DoubleClick += new System.EventHandler(listBoxUnassigned_DoubleClick);
            frmMoneyCenterAssignment.listBoxAssigned.DoubleClick += new System.EventHandler(listBoxAssigned_DoubleClick);
            frmMoneyCenterAssignment.buttonSave.Click += new System.EventHandler(buttonSave_Click);
            frmMoneyCenterAssignment.buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
        }
        public void SetBindings()
        {
            frmMoneyCenterAssignment.listBoxAssigned.ValueMember = "moneycenterid";
            frmMoneyCenterAssignment.listBoxAssigned.DisplayMember = "centername";
            frmMoneyCenterAssignment.listBoxAssigned.DataSource = assignedssprocessds.view_expandedcompmoneycenter;
            frmMoneyCenterAssignment.listBoxUnassigned.ValueMember = "idcol";
            frmMoneyCenterAssignment.listBoxUnassigned.DisplayMember = "centername";
            frmMoneyCenterAssignment.listBoxUnassigned.DataSource = unassignedssprocessds.moneycenter;
        }

        private void listBoxUnassigned_DoubleClick(object sender, EventArgs e)
        {
            if (unassignedssprocessds.moneycenter.Rows.Count > 0)
            {
                assignedssprocessds.view_expandedcompmoneycenter.Rows.Add();
                int rowcount = assignedssprocessds.view_expandedcompmoneycenter.Rows.Count - 1;
                assignedssprocessds.view_expandedcompmoneycenter[rowcount].centername = unassignedssprocessds.moneycenter[frmMoneyCenterAssignment.listBoxUnassigned.SelectedIndex].centername; ;
                assignedssprocessds.view_expandedcompmoneycenter[rowcount].moneycenterid = unassignedssprocessds.moneycenter[frmMoneyCenterAssignment.listBoxUnassigned.SelectedIndex].idcol; ;
                assignedssprocessds.view_expandedcompmoneycenter[rowcount].defaultmoneycenter = false;
                unassignedssprocessds.moneycenter.Rows[frmMoneyCenterAssignment.listBoxUnassigned.SelectedIndex].Delete();
                unassignedssprocessds.moneycenter.AcceptChanges();
                assignedssprocessds.view_expandedcompmoneycenter.AcceptChanges();
            }
            else
            {
                wsgUtilities.wsgNotice("There are no unassigned money centers");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            frmMoneyCenterAssignment.Close();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Remove any prior assignments
            ClearParameters();
            this.AddParms("@comp_code", comp_code, "SQL");
            commandstring = "DELETE FROM compmoneycenter WHERE comp_code = @comp_code";
            ExecuteCommand(commandstring, CommandType.Text);
            // Loop thru assignments and add to table
            for (int i = 0; i <= assignedssprocessds.view_expandedcompmoneycenter.Rows.Count - 1; i++)
            {
                ssprocessds.compmoneycenter.Rows.Clear();
                EstablishBlankDataTableRow(ssprocessds.compmoneycenter);
                ssprocessds.compmoneycenter[0].comp_code = comp_code;
                ssprocessds.compmoneycenter[0].moneycenterid = assignedssprocessds.view_expandedcompmoneycenter[i].moneycenterid;
                ssprocessds.compmoneycenter[0].defaultmoneycenter = assignedssprocessds.view_expandedcompmoneycenter[i].defaultmoneycenter;
                GenerateAppTableRowSave(ssprocessds.compmoneycenter[0]);
            }
            frmMoneyCenterAssignment.Close();
        }

        private void listBoxAssignedMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (assignedssprocessds.view_expandedcompmoneycenter.Rows.Count > 0)
                {
                    ClearDefaultMoneyCenters();
                    assignedssprocessds.view_expandedcompmoneycenter[frmMoneyCenterAssignment.listBoxAssigned.SelectedIndex].defaultmoneycenter = true;
                    RefreshDefaultMoneyCenterLabel();
                }
                else
                {
                    wsgUtilities.wsgNotice("There are no assigned money centers");
                }
            }
        }
        private void listBoxAssigned_DoubleClick(object sender, EventArgs e)
        {
            if (assignedssprocessds.view_expandedcompmoneycenter.Rows.Count > 0)
            {
                if (wsgUtilities.wsgReply("Remove this item?"))
                {
                    int rowcount = unassignedssprocessds.moneycenter.Rows.Count - 1;
                    unassignedssprocessds.moneycenter.Rows.Add();
                    unassignedssprocessds.moneycenter[rowcount].centername = assignedssprocessds.view_expandedcompmoneycenter[frmMoneyCenterAssignment.listBoxAssigned.SelectedIndex].centername;
                    unassignedssprocessds.moneycenter[rowcount].idcol = assignedssprocessds.view_expandedcompmoneycenter[frmMoneyCenterAssignment.listBoxAssigned.SelectedIndex].moneycenterid;
                    assignedssprocessds.view_expandedcompmoneycenter.Rows[frmMoneyCenterAssignment.listBoxAssigned.SelectedIndex].Delete();
                    unassignedssprocessds.moneycenter.AcceptChanges();
                    assignedssprocessds.view_expandedcompmoneycenter.AcceptChanges();
                    RefreshDefaultMoneyCenterLabel();
                }

            }
            else
            {
                wsgUtilities.wsgNotice("There are no assigned money centers");
            }
        }

        public void ShowAssignmentForm()
        {
            frmMoneyCenterAssignment.ShowDialog();
        }
        public void EstablishAssigned()
        {
            commandstring = " SELECT * FROM view_expandedcompmoneycenter where comp_code = @comp_code ORDER BY centername";
            ClearParameters();
            assignedssprocessds.view_expandedcompmoneycenter.Rows.Clear();
            AddParms("@comp_code", comp_code, "SQL");
            FillData(assignedssprocessds, "view_expandedcompmoneycenter", commandstring, CommandType.Text);
        }

        public void ClearDefaultMoneyCenters()
        {
            for (int i = 0; i <= assignedssprocessds.view_expandedcompmoneycenter.Rows.Count - 1; i++)
            {
                assignedssprocessds.view_expandedcompmoneycenter[i].defaultmoneycenter = false;
            }
        }

        public void RefreshDefaultMoneyCenterLabel()
        {
            frmMoneyCenterAssignment.labelDefaultMoneyCenter.Text = "No Default Money Center";
            for (int i = 0; i <= assignedssprocessds.view_expandedcompmoneycenter.Rows.Count - 1; i++)
            {
                if (assignedssprocessds.view_expandedcompmoneycenter[i].defaultmoneycenter == true)
                {
                    frmMoneyCenterAssignment.labelDefaultMoneyCenter.Text = "Default Money Center- " + assignedssprocessds.view_expandedcompmoneycenter[i].centername.TrimEnd();
                }
            }
        }
        public void EstablishUnAssigned()
        {
            commandstring = " SELECT * FROM moneycenter where idcol ";
            commandstring += " NOT IN (SELECT moneycenterid FROM compmoneycenter ";
            commandstring += " WHERE comp_code = @comp_code) AND RTRIM(centername) <> '' ORDER BY LTRIM(centername)";
            ClearParameters();
            ClearParameters();
            unassignedssprocessds.moneycenter.Rows.Clear();
            AddParms("@comp_code", comp_code, "SQL");
            FillData(unassignedssprocessds, "moneycenter", commandstring, CommandType.Text);
        }
        public void ProcessApp()
        {
            comp_code = "";
            cont = true;
            while (cont)
            {
                comp_code = commonAppDataMethods.SelectMoneyCenterCompany();
                if (comp_code.TrimEnd() != "")
                {
                    EstablishAssigned();
                    EstablishUnAssigned();
                    RefreshDefaultMoneyCenterLabel();
                    ShowAssignmentForm();
                }
                else
                {
                    break;
                }
            }
        }
    }

    //////
    public class StoreMoneycenterAssignmentMethods : WSGDataAccess
    {
        AppConstants myAppconstants = new AppConstants();
        WSGUtilities wsgUtilities = new WSGUtilities("Moneycenter Assignment");
        AppUtilities appUtilities = new AppUtilities();
        public Form menuform { get; set; }
        public bool cont;
        FrmMoneycenterAssignment frmMoneyCenterAssignment = new FrmMoneycenterAssignment();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess assignedssprocessds = new ssprocess();
        ssprocess unassignedssprocessds = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        string commandstring = "";
        string comp_code = "";
        string store_code = "";
        public StoreMoneycenterAssignmentMethods()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.storemoneycenter.idcolColumn);
        }

        public void StartApp(string AssignmentMethod)
        {
            SetEvents();
            SetBindings();
            ProcessApp(AssignmentMethod);
        }
        public void SetEvents()
        {

            frmMoneyCenterAssignment.listBoxUnassigned.DoubleClick += new System.EventHandler(listBoxUnassigned_DoubleClick);
            frmMoneyCenterAssignment.listBoxAssigned.DoubleClick += new System.EventHandler(listBoxAssigned_DoubleClick);
            frmMoneyCenterAssignment.buttonSave.Click += new System.EventHandler(buttonSave_Click);
            frmMoneyCenterAssignment.buttonCancel.Click += new System.EventHandler(buttonCancel_Click);
        }
        public void SetBindings()
        {
            frmMoneyCenterAssignment.listBoxAssigned.ValueMember = "idcol";
            frmMoneyCenterAssignment.listBoxAssigned.DisplayMember = "centername";
            frmMoneyCenterAssignment.listBoxAssigned.DataSource = assignedssprocessds.moneycenter;
            frmMoneyCenterAssignment.listBoxUnassigned.ValueMember = "idcol";
            frmMoneyCenterAssignment.listBoxUnassigned.DisplayMember = "centername";
            frmMoneyCenterAssignment.listBoxUnassigned.DataSource = unassignedssprocessds.moneycenter;
        }

        private void listBoxUnassigned_DoubleClick(object sender, EventArgs e)
        {
            if (unassignedssprocessds.moneycenter.Rows.Count > 0)
            {
                assignedssprocessds.moneycenter.ImportRow(unassignedssprocessds.moneycenter.Rows[frmMoneyCenterAssignment.listBoxUnassigned.SelectedIndex]);
                unassignedssprocessds.moneycenter.Rows[frmMoneyCenterAssignment.listBoxUnassigned.SelectedIndex].Delete();
                unassignedssprocessds.moneycenter.AcceptChanges();
                assignedssprocessds.moneycenter.AcceptChanges();
            }
            else
            {
                wsgUtilities.wsgNotice("There are no assigned alerts");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            frmMoneyCenterAssignment.Close();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Remove any prior assignments
            ClearParameters();
            this.AddParms("@store_code", store_code, "SQL");
            commandstring = "DELETE FROM storemoneycenter WHERE storecode = @store_code";
            ExecuteCommand(commandstring, CommandType.Text);
            // Add one assignment
            if (assignedssprocessds.moneycenter.Rows.Count > 0)
            {
                ssprocessds.storemoneycenter.Rows.Clear();
                EstablishBlankDataTableRow(ssprocessds.storemoneycenter);
                ssprocessds.storemoneycenter[0].storecode = store_code;
                ssprocessds.storemoneycenter[0].moneycenterid = assignedssprocessds.moneycenter[0].idcol;
                GenerateAppTableRowSave(ssprocessds.storemoneycenter[0]);
            }
            frmMoneyCenterAssignment.Close();
        }

        private void listBoxAssigned_DoubleClick(object sender, EventArgs e)
        {
            if (assignedssprocessds.moneycenter.Rows.Count > 0)
            {
                unassignedssprocessds.moneycenter.ImportRow(assignedssprocessds.moneycenter.Rows[frmMoneyCenterAssignment.listBoxAssigned.SelectedIndex]);
                assignedssprocessds.moneycenter.Rows[frmMoneyCenterAssignment.listBoxAssigned.SelectedIndex].Delete();
                unassignedssprocessds.moneycenter.AcceptChanges();
                assignedssprocessds.moneycenter.AcceptChanges();
            }
            else
            {
                wsgUtilities.wsgNotice("There are no assigned alerts");
            }
        }

        public void ShowAssignmentForm()
        {
            frmMoneyCenterAssignment.labelDefaultInstructions.Visible = false;
            frmMoneyCenterAssignment.labelDefaultMoneyCenter.Visible = false;
            frmMoneyCenterAssignment.ShowDialog();
        }
        public void EstablishAssigned()
        {
            commandstring = " SELECT centername, idcol FROM moneycenter where idcol ";
            commandstring += " IN (SELECT moneycenterid FROM storemoneycenter ";
            commandstring += " WHERE storecode = @store_code)";
            ClearParameters();
            assignedssprocessds.moneycenter.Rows.Clear();
            AddParms("@store_code", store_code, "SQL");
            FillData(assignedssprocessds, "moneycenter", commandstring, CommandType.Text);
        }
        public void EstablishUnAssigned()
        {
            commandstring = " SELECT centername, idcol FROM moneycenter where idcol ";
            commandstring += " NOT IN (SELECT moneycenterid FROM storemoneycenter ";
            commandstring += " WHERE storecode = @store_code)";
            ClearParameters();
            unassignedssprocessds.moneycenter.Rows.Clear();
            AddParms("@store_code", comp_code, "SQL");
            FillData(unassignedssprocessds, "moneycenter", commandstring, CommandType.Text);
        }
        public void ProcessApp(string AssignmentMethod)
        {
            cont = true;
            if (AssignmentMethod == "Company")
            {
                comp_code = "";

                while (cont)
                {
                    comp_code = commonAppDataMethods.SelectManifestCompany();

                    if (comp_code.TrimEnd() != "")
                    {
                        while (cont)
                        {
                            store_code = commonAppDataMethods.SelectMoneyCenterStore("Company", comp_code);
                            if (store_code.TrimEnd() != "")
                            {
                                EstablishAssigned();
                                EstablishUnAssigned();
                                ShowAssignmentForm();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                string driver = commonAppDataMethods.SelectDriver();
                if (driver.TrimEnd() != "")
                {
                    while (cont)
                    {
                        store_code = commonAppDataMethods.SelectMoneyCenterStore("Driver", driver);
                        if (store_code.TrimEnd() != "")
                        {
                            EstablishAssigned();
                            EstablishUnAssigned();
                            ShowAssignmentForm();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }



}

