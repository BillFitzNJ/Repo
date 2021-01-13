using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BusinessTransactions
{
  public partial class FrmBagManifest : Form
  {
    public FrmBagManifest()
    {
      InitializeComponent();
    }

    protected override CreateParams CreateParams
    {
        get
        {
            const int CP_NOCLOSE = 0x200;
            CreateParams myCp = base.CreateParams;
            myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE;
            return myCp;
        }
    }

    

   
  }
}
