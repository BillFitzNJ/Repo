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
namespace MiscellaneousSystemMaintenance
{
  public class MaintainSubscriptionMethods : FrmMaintainSingleTableMethods
  {
    AppUtilities appUtilities = new AppUtilities();
    AppConstants myAppconstants = new AppConstants();
    WSGUtilities wsgUtilities = new WSGUtilities("Maintain Event");

    // Any controls being added must be defined here
    Button buttonSelectApppuser = new Button();
    Button buttonSelectAppevent = new Button();
    Label  labelAppuserName = new Label();
    Label  labelAppeventName = new Label(); 
    public int CurrentAppeventid = new int();
    public int CurrentUserid = new int();
 
    // Establish the datasets
    sysdata sysdatads = new sysdata();
    sysdata listds = new sysdata();
    public override void EstablishAppConstants()
    {
      CurrentAppeventid = 0;
      CurrentUserid = 0;
      currentablename = "appeventsubscription";
      SetIdcol(sysdatads.appeventsubscription.idcolColumn);
      parentForm.Text = "Maintain Subscription";
    }
    public override void SetControls()
    {
      // Establish any custom controls here 
      buttonSelectAppevent.Left = 50;
      buttonSelectAppevent.Text = "Select Event";
      buttonSelectAppevent.Top = 50;
      buttonSelectAppevent.Width = 100;
      buttonSelectAppevent.Height = 25;

      buttonSelectApppuser.Left = 200;
      buttonSelectApppuser.Text = "Select User";
      buttonSelectApppuser.Top = 50;
      buttonSelectApppuser.Width = 100;
      buttonSelectApppuser.Height = 25;

      labelAppuserName.Left = buttonSelectApppuser.Left;
      labelAppuserName.Top = buttonSelectApppuser.Top + buttonSelectApppuser.Height + 10;
      labelAppuserName.Text = "";

      labelAppeventName.Left = buttonSelectAppevent.Left;
      labelAppeventName.Top = buttonSelectAppevent.Top + buttonSelectAppevent.Height + 10;
      labelAppeventName.Text = "";
      parentForm.Controls.Add(buttonSelectApppuser);
      parentForm.Controls.Add(buttonSelectAppevent);
      parentForm.Controls.Add(labelAppuserName);
      parentForm.Controls.Add(labelAppeventName);
    }
    public override void SetInsertState(object sender, EventArgs e)
    {
      EstablishBlankDataTableRow(sysdatads.appeventsubscription);
      currentidcol = sysdatads.appeventsubscription[0].idcol;
      base.SetInsertState(sender, e);
    }
    public override void SetEvents()
    {
      base.SetEvents();
      buttonSelectAppevent.Click += new System.EventHandler(ProcessSelectAppevent);
      buttonSelectApppuser.Click += new System.EventHandler(ProcessSelectUser);
    }
   
    public override void RefreshControls()
    {
      // Handle any custome controls here
      buttonSelectAppevent.Enabled = false;
      buttonSelectApppuser.Enabled = false;
      base.RefreshControls();
      switch (CurrentState)
      {
        case "Select":
          {
            labelAppuserName.Text = "";
            labelAppeventName.Text = "";
            break;
          }
        case "Edit":
          {
            buttonSelectAppevent.Enabled = true;
            buttonSelectApppuser.Enabled = true;
            break;
          }
        case "Insert":
          {
            buttonSelectAppevent.Enabled = true;
            buttonSelectApppuser.Enabled = true;
            break;
          }
      }
    }
    public override void SaveCurrentDataTable(object sender, EventArgs e)
    {
     // Populate the user and appevent id columns
     sysdatads.appeventsubscription[0].appeventidcol = CurrentAppeventid;
     sysdatads.appeventsubscription[0].useridcol = CurrentUserid;

      // Save the table
      GenerateAppTableRowSave(sysdatads.appeventsubscription[0]);
      base.SaveCurrentDataTable(sender, e);
    }
    public void ProcessSelectUser(object sender, EventArgs e)
    {
      CurrentUserid = 0;
      sysdatads.appuser.Rows.Clear();
      string commandtext = "SELECT * FROM appuser ORDER BY username";
      ClearParameters();
      FillData(sysdatads, "appuser", commandtext, CommandType.Text);
      FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
      frmSelectorMethods.FormText = "Select User";
      frmSelectorMethods.dtSource = sysdatads.appuser;
      frmSelectorMethods.columncount = 2;
      frmSelectorMethods.SetColumns();
      frmSelectorMethods.colname[0] = "Useridcol";
      frmSelectorMethods.colheadertext[0] = "User ID";
      frmSelectorMethods.coldatapropertyname[0] = "userid";
      frmSelectorMethods.colwidth[0] = 60;
      frmSelectorMethods.colname[1] = "Usernamecol";
      frmSelectorMethods.colheadertext[1] = "User Name";
      frmSelectorMethods.coldatapropertyname[1] = "username";
      frmSelectorMethods.colwidth[1] = 300;
      frmSelectorMethods.SetGrid();
      CurrentUserid = frmSelectorMethods.ShowSelector();
      // If a row has been selected fill the data and process
      if (CurrentUserid > 0)
      {
        commandtext = "SELECT * FROM appuser WHERE idcol = @idcol";
        sysdatads.appuser.Rows.Clear();
        ClearParameters();
        AddParms("@idcol", CurrentUserid, "SQL");
        FillData(sysdatads, "appuser", commandtext, CommandType.Text);
        labelAppuserName.Text = sysdatads.appuser[0].username;
        RefreshControls();
      }
  
    }
 
    public void ProcessSelectAppevent(object sender, EventArgs e)
    {
      CurrentAppeventid = 0;
      sysdatads.appevent.Rows.Clear();
      string commandtext = "SELECT * FROM appevent ORDER BY appeventname";
      ClearParameters();
      FillData(sysdatads, "appevent", commandtext, CommandType.Text);
      FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
      frmSelectorMethods.FormText = "Select Event";
      frmSelectorMethods.dtSource = sysdatads.appevent;
      frmSelectorMethods.columncount = 1;
      frmSelectorMethods.SetColumns();
      frmSelectorMethods.colname[0] = "Eventcol";
      frmSelectorMethods.colheadertext[0] = "Event Name";
      frmSelectorMethods.coldatapropertyname[0] = "appeventname";
      frmSelectorMethods.colwidth[0] = 300;
      frmSelectorMethods.SetGrid(); 
      CurrentAppeventid = frmSelectorMethods.ShowSelector();
      // If a row has been selected fill the data and process
      if (CurrentAppeventid > 0)
      {
        sysdatads.appevent.Rows.Clear();
        ClearParameters();
        AddParms("@idcol", CurrentAppeventid, "SQL");
        FillData(sysdatads, "appevent", "wsgsp_getsingleappevent", CommandType.StoredProcedure);
        labelAppeventName.Text = sysdatads.appevent[0].appeventname;
        RefreshControls();
      }
    }
    public override void ProcessDelete(object sender, EventArgs e)
    {
      base.ProcessDelete(sender, e);
      sysdatads.appeventsubscription.Rows.Clear();
    }
    public override void ProcessSelect(object sender, EventArgs e)
    {
      currentidcol = 0;
      listds.view_expandedsubscription.Rows.Clear();
      string commandtext = "SELECT * FROM view_expandedsubscription ORDER BY appeventname";
      ClearParameters();
      FillData(listds, "view_expandedsubscription", commandtext, CommandType.Text);
      FrmSelectorMethods frmSelectorMethods = new FrmSelectorMethods();
      frmSelectorMethods.FormText = "Select Subscription";
      frmSelectorMethods.dtSource =  listds.view_expandedsubscription;
      frmSelectorMethods.columncount = 2;
      frmSelectorMethods.SetColumns();
      frmSelectorMethods.colname[0] = "Eventnamecol";
      frmSelectorMethods.colheadertext[0] = "Event Name";
      frmSelectorMethods.coldatapropertyname[0] = "appeventname";
      frmSelectorMethods.colwidth[0] = 300;
      frmSelectorMethods.colname[1] = "Usernamecol";
      frmSelectorMethods.colheadertext[1] = "User Name";
      frmSelectorMethods.coldatapropertyname[1] = "username";
      frmSelectorMethods.colwidth[1] = 300;
      frmSelectorMethods.SetGrid();
      int selectedsubscriptionid = frmSelectorMethods.ShowSelector();
      // If a row has been selected fill the data and process
      if (selectedsubscriptionid > 0)
      {
        sysdatads.appeventsubscription.Rows.Clear();
        ClearParameters();
        AddParms("@idcol", selectedsubscriptionid, "SQL");
        FillData(sysdatads, "appeventsubscription", "wsgsp_getsingleappeventsubscription", CommandType.StoredProcedure);
        currentidcol = selectedsubscriptionid;
        // Get the descriptions for the appuser and appevent
        listds.view_expandedsubscription.Rows.Clear();
        commandtext = "SELECT * FROM view_expandedsubscription where idcol = @idcol";
        ClearParameters();
        AddParms("@idcol", selectedsubscriptionid, "SQL");
        FillData(listds, "view_expandedsubscription", commandtext, CommandType.Text);
        labelAppeventName.Text = listds.view_expandedsubscription[0].appeventname;
        labelAppuserName.Text = listds.view_expandedsubscription[0].username;
        CurrentState = "View";
        RefreshControls();
      }
    }
  }
}
