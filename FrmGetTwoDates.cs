using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using WSGUtilitieslib;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonAppClasses
{
  public partial class FrmGetTwoDates : Form
  {
    public Boolean DateOk = true;
    public Boolean OKToClose = false;
    public DateTime SelectedStartDate{get;set;}
    public DateTime SelectedEndDate { get; set; }
    WSGUtilities wsgUtilities = new WSGUtilities("Date Selection");
    public FrmGetTwoDates()
    {
     SelectedStartDate = DateTime.Now;
     SelectedEndDate = DateTime.Now;
     InitializeComponent();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
       OKToClose = true;
       DateOk = false;
       this.Close();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      if (SelectedEndDate < SelectedStartDate)
      {
       wsgUtilities.wsgNotice("The Ending Date is earlier than the starting date");
      }
      else
      {
        OKToClose = true;
        this.Close();
      }
    }

    private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
    {
      SelectedStartDate = dateTimePickerStart.Value.Date;
    }

    private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
    {
      SelectedEndDate = dateTimePickerEnd.Value.Date;
    }

    private void FrmGetTwoDates_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!OKToClose)
      { e.Cancel = true; }
    }
  }
}
