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
  public class FrmMaintainAppEventMethods : FrmMaintainSingleTableMethods
  {

    AppUtilities appUtilities = new AppUtilities();
    AppConstants myAppconstants = new AppConstants();
    WSGUtilities wsgUtilities = new WSGUtilities("Maintain Event");

    // Any controls being added must be defined here
    TextBox textBoxAppeventname = new TextBox();
    // Establish the dataset
    sysdata sysdatads = new sysdata();
    
   public override void EstablishAppConstants()
   {
      currentablename = "appevent";
      SetIdcol(sysdatads.appevent.idcolColumn);
      parentForm.Text = "Maintain Application Event";
   }
     public override void SetControls()
    {
      // Establish any custom controls here 
      textBoxAppeventname.Left = 50;
      textBoxAppeventname.Text = "";
      textBoxAppeventname.Top = 50;
      textBoxAppeventname.Width = 500;
      textBoxAppeventname.DataBindings.Add("Text", sysdatads.appevent, "appeventname");
      parentForm.Controls.Add(textBoxAppeventname);
    }
    public override void SetInsertState(object sender, EventArgs e)
    {
      EstablishBlankDataTableRow(sysdatads.appevent);
      currentidcol = sysdatads.appevent[0].idcol;
      base.SetInsertState(sender, e);
    }

    public override void RefreshControls()
    {
      // Handle any custome controls here
      textBoxAppeventname.Enabled = false;
      base.RefreshControls();
      switch (CurrentState)
      {
        case "Select":
          {
            textBoxAppeventname.Text = "";
            textBoxAppeventname.Enabled = false;
            break;
          }
        case "Edit":
          {
            textBoxAppeventname.Enabled = true;
            break;
          }
        case "Insert":
          {
            textBoxAppeventname.Enabled = true;
            break;
          }
      }
    }
    public override void SaveCurrentDataTable(object sender, EventArgs e)
    {
      // Save the table
      GenerateAppTableRowSave(sysdatads.appevent[0]);
      base.SaveCurrentDataTable(sender, e);
    }
  
    public override void ProcessDelete(object sender, EventArgs e)
    {
      base.ProcessDelete(sender, e);
      sysdatads.appevent.Rows.Clear();
    }
    public override void ProcessSelect(object sender, EventArgs e)
    {
      currentidcol = 0;
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
      int selectedeventid = frmSelectorMethods.ShowSelector();
      // If a row has been selected fill the data and process
      if (selectedeventid > 0)
      {
        sysdatads.appevent.Rows.Clear();
        ClearParameters();
        AddParms("@idcol", selectedeventid, "SQL");
        FillData(sysdatads, "appevent", "wsgsp_getsingleappevent", CommandType.StoredProcedure);
        currentidcol = selectedeventid;
        CurrentState = "View";
        RefreshControls();
      }
    }
  }
}
