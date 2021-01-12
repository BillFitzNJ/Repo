using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using WSGUtilitieslib;
using CommonAppClasses;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiscellaneousSystemMaintenance
{
  public partial class FrmGetUser : Form
  {
    private BindingSource bindingUserData = new BindingSource();
    public SqlConnection conn = new SqlConnection();
    public System.Windows.Forms.ToolStripMenuItem parenttoolstripmenuitem = null;
    AppUtilities appUtilities = new AppUtilities();
    AppConstants myAppconstants = new AppConstants();
    WSGUtilities wsgUtilities = new WSGUtilities("User Maintenance");
    MiscSysdataInf miscSysdataInf = new  MiscSysdataInf("SQL", "SQLConnString");
    public FrmGetUser()
    {
      InitializeComponent();
      bindingUserData.DataSource = miscSysdataInf.listsysdatads.appuser;
      dataGridViewUserData.DataSource = bindingUserData;
      dataGridViewUserData.RowsDefaultCellStyle.BackColor = Color.LightGray;
      dataGridViewUserData.AlternatingRowsDefaultCellStyle.BackColor = Color.DarkGray;
      dataGridViewUserData.AutoGenerateColumns = false;
      dataGridViewUserData.Focus();
      filldatagrid();
    }

    private int selectedUserId;
    public int SelectedUserId
    {

      get
      {
        return selectedUserId;
      }
      set
      {
        selectedUserId = value;
      }

    }

    private void filldatagrid()
    {
      miscSysdataInf.GetAppUsers();
      if (miscSysdataInf.listsysdatads.appuser.Rows.Count < 1)
      {
        wsgUtilities.wsgNotice("There are no active users.");
      }
    } // end filldatagrid

    public void CaptureUserId()
    {

      SelectedUserId = miscSysdataInf.CaptureIdCol(dataGridViewUserData);

    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      SelectedUserId = 0;
      this.Close();
    } // end click cancel button

    private void dataGridViewUserData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
      CaptureUserId();

      this.Close();
    }

    private void dataGridViewUserData_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Return)
      {
        CaptureUserId();
        this.Close();
      }
    } // End double click

  }
}
