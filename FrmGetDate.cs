using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CommonAppClasses
{
  public partial class FrmGetDate : Form
  {
    public DateTime SelectedDate {get;set;}
    private bool OKToClose {get;set;}
    public Boolean DateOk {get; set;}
    public FrmGetDate()
    {
      OKToClose = false;
      InitializeComponent();
      SelectedDate = DateTime.Now.Date;
      DateOk = true;
    }

    private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
    {
      SelectedDate = dateTimePicker1.Value;
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      OKToClose = true;
      DateOk = false;
      this.Close();
    }

    private void buttonOK_Click(object sender, EventArgs e)
    {
      OKToClose = true;
      this.Close();
    }

    private void FrmGetDate_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (!OKToClose) 
      { e.Cancel = true; }
    }

  
  }
}
