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
  public partial class FrmGetDateAndTime : Form
  {
    public bool OKtoProceed = false;
    public DateTime SelectedDateAndTime { get; set; }
    public FrmGetDateAndTime()
    {
      InitializeComponent();
    }

   
    private void buttonProceed_Click(object sender, EventArgs e)
    {
     SelectedDateAndTime = dateTimePickerDateTime.Value;
     OKtoProceed = true;
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
