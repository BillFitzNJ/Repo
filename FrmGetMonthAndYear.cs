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
    public partial class FrmGetMonthAndYear : Form
    {
        public bool DataOK = true;
        public FrmGetMonthAndYear()
        {
            InitializeComponent();
        }

        private void buttonProceed_Click(object sender, EventArgs e)
        {
            DataOK = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DataOK = false;
            this.Close();
        }
    }
}
