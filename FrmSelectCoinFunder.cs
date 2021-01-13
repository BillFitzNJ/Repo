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
  public partial class FrmSelectCoinFunder : Form
  {
    public String CoinFunder = "";
    public FrmSelectCoinFunder()
    {
      InitializeComponent();
    }

    private void buttonSafeAndSound_Click(object sender, EventArgs e)
    {
      CoinFunder = "SafeAndSound";
      this.Close();
    }

    private void buttonSignature_Click(object sender, EventArgs e)
    {
      CoinFunder = "Signature";
      this.Close();
    }

    private void buttonRapid_Click(object sender, EventArgs e)
    {
      CoinFunder = "Rapid";
      this.Close();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void buttonFEGS_Click(object sender, EventArgs e)
    {
        CoinFunder = "FEGS";
        this.Close();
    }
  }
}
