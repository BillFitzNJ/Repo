using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommonAppClasses;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using WSGUtilitieslib;
using System.Text;
using System.Windows.Forms;

namespace MiscellaneousSystemMaintenance
{
  public partial class FrmMaintainUser : Form
  {
    private BindingSource bindingUserData = new BindingSource();
    public SqlConnection conn = new SqlConnection();
    public System.Windows.Forms.ToolStripMenuItem parenttoolstripmenuitem = null;
    AppUtilities appUtilities = new AppUtilities();
    CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
    AppConstants myAppconstants = new AppConstants();
    MiscSysdataInf miscSysdataInf = new MiscSysdataInf("SQL", "SQLConnString");
    WSGUtilities wsgUtilities = new WSGUtilities("User Maintenance");
    public FrmMaintainUser()
    {
      conn.ConnectionString = myAppconstants.SQLConnectionString;
      InitializeComponent();
      CurrentState = "Select";
      RefreshControls();
      // Data bindings
      textBoxEmailAddress.DataBindings.Add("Text", miscSysdataInf.sysdatads.appuser, "emailaddress");
      textBoxUsername.DataBindings.Add("Text", miscSysdataInf.sysdatads.appuser, "username");
      textBoxUserid.DataBindings.Add("Text", miscSysdataInf.sysdatads.appuser, "userid");
      textBoxPassword.DataBindings.Add("Text", miscSysdataInf.sysdatads.appuser, "passwd");
    }

    public int SelectedUserId { get; set; }
    public string CurrentState { get; set; }
    public string AppUserstatus { get; set; }

    private void buttonSelectUser_Click(object sender, EventArgs e)
    {
      // Selects User to to be processed
      SelectedUserId = commonAppDataMethods.SelectUser();
      if (SelectedUserId > 0)
      {
        miscSysdataInf.GetSingleAppUser(SelectedUserId);

        if (miscSysdataInf.sysdatads.appuser[0].userstatus == "I")
        {
          AppUserstatus = "Inactive";
        }
        else
        {
          AppUserstatus = "Active";
        }
        labelUserrole.Text = miscSysdataInf.sysdatads.appuser[0].userrole;


        CurrentState = "View";
        RefreshControls();
      }
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      if (AppUserstatus == "Active")
      {
        miscSysdataInf.sysdatads.appuser[0].userstatus = "A";
      }
      else
      {
        miscSysdataInf.sysdatads.appuser[0].userstatus = "I";
      }
      if (miscSysdataInf.SaveAppuser())
      {
        wsgUtilities.wsgNotice("User data updated");
        CurrentState = "Select";
        RefreshControls();
      }
      else
      {
        wsgUtilities.wsgNotice("User data update cancelled");
      }

    } //end save

    private void DisableFields()
    {

      //Disable the form's fields
      textBoxUserid.Enabled = false;
      textBoxUsername.Enabled = false;
      textBoxPassword.Enabled = false;
      textBoxEmailAddress.Enabled = false;
      buttonSelectApprole.Enabled = false;
    }

    private void EnableFields()
    {
      //Enable the form's fields
      textBoxUsername.Enabled = true;
      textBoxPassword.Enabled = true;
      textBoxEmailAddress.Enabled = true;
      buttonSelectApprole.Enabled = true;
    }
    private void RefreshControls()
    {
      switch (CurrentState)
      {
        case "Select":
          //Initialize the form's fields
          labelUserStatus.Text = "";
          textBoxUserid.Text = " ";
          textBoxUsername.Text = " ";
          textBoxPassword.Text = " ";
          textBoxEmailAddress.Text = " ";
          // Disable the form's Controls
          DisableFields();
          // Establish the proper button status
          buttonCancel.Enabled = true;
          buttonInsert.Enabled = true;
          buttonSelectUser.Enabled = true;
          buttonEdit.Enabled = false;
          buttonSave.Enabled = false;
          buttonUserstatus.Enabled = false;

          break;

        case "View":
          // Disable the form's Controls
          DisableFields();
          if (AppUserstatus == "Active")
          {
            labelUserStatus.Text = "User status = Active";
            buttonUserstatus.Text = "Deactivate";
          }
          else
          {
            labelUserStatus.Text = "User status = Inactive";
            buttonUserstatus.Text = "Activate";
          }
          // Establish the proper button status
          buttonCancel.Enabled = true;
          buttonInsert.Enabled = true;
          buttonSelectUser.Enabled = true;
          buttonEdit.Enabled = true;
          buttonSave.Enabled = false;
          buttonUserstatus.Enabled = false;
          break;


        case "Edit":
          // Enable the form's Controls
          EnableFields();
          // Establish the proper button status
          buttonCancel.Enabled = true;
          buttonInsert.Enabled = false;
          buttonSelectUser.Enabled = false;
          buttonEdit.Enabled = false;
          buttonSave.Enabled = true;
          buttonUserstatus.Enabled = true;

          break;

        case "Insert":
          //Initialize the form's fields
          textBoxUserid.Text = "    ";
          textBoxUsername.Text = " ";
          textBoxPassword.Text = " ";
          textBoxEmailAddress.Text = " ";
          labelUserrole.Text = "";
          // Enable the form's Controls
          EnableFields();
          textBoxUserid.Enabled = true;
          // Establish the proper button status
          buttonCancel.Enabled = true;
          buttonInsert.Enabled = false;
          buttonSelectUser.Enabled = false;
          buttonEdit.Enabled = false;
          buttonSave.Enabled = true;
          buttonUserstatus.Enabled = false;
          break;

      }
      this.Update();
    }


    private void buttonEdit_Click(object sender, EventArgs e)
    {
      CurrentState = "Edit";
      RefreshControls();
    }

  

    private void buttonUserstatus_Click(object sender, EventArgs e)
    {
      if (AppUserstatus == "Active")
      {
        AppUserstatus = "Inactive";
        labelUserStatus.Text = "User status = Inactive";
        buttonUserstatus.Text = "Activate";
      }
      else
      {
        AppUserstatus = "Active";
        labelUserStatus.Text = "User status = Active";
        buttonUserstatus.Text = "Deactivate";

      }
    }

    private void buttonInsert_Click(object sender, EventArgs e)
    {
      miscSysdataInf.InitializeAppuser();
      miscSysdataInf.sysdatads.appuser[0].userstatus = "A";
      AppUserstatus = "Active";
      miscSysdataInf.sysdatads.appuser[0].userrole = "OPER";
      labelUserrole.Text = "OPER";
      labelUserStatus.Text = "User status = Active";
      buttonUserstatus.Text = "Deactivate";
      CurrentState = "Insert";
      RefreshControls();
    }

    private void listBoxUserrole_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void buttonSelectApprole_Click(object sender, EventArgs e)
    {
      int approleid = commonAppDataMethods.SelectUserrole();
      string userrole = "";
      // If a row has been selected fill the data and process
      if (approleid > 0)
      {
        userrole = commonAppDataMethods.GetRolebyId(approleid);
      }
      else
      {
        userrole = "UNK";
      }
      miscSysdataInf.sysdatads.appuser[0].userrole = userrole;
      labelUserrole.Text = userrole;
    }

  }
}
