using System;
using CommonAppClasses;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WSGUtilitieslib;
namespace BusinessTransactions
{
    public class PostBills : WSGDataAccess
    {

        WSGUtilities wsgUtilities = new WSGUtilities("Billing Generation");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        ssprocess ssprocessds = new ssprocess();

        public PostBills()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.arinvoice.idcolColumn);
        }

        public void StartApp()
        {
            DateTime invoicedate = new DateTime();
            bool cont = true;
            ssprocessds.view_distinctbillinginvoice.Rows.Clear();
            string commandstring = "select distinct inv_number, inv_date, bill_store, sum(charged) as pretax, sum(storetax) as TAX from billing  where inv_date = '2020-11-30'  group by inv_number, bill_store, inv_date order by inv_number";
            FillData(ssprocessds, "view_distinctbillinginvoice", commandstring, CommandType.Text);
            if (ssprocessds.view_distinctbillinginvoice.Rows.Count < 1)
            {
                wsgUtilities.wsgNotice("There are no bills to post");
                cont = false;
            }
            else
            {
                cont = wsgUtilities.wsgReply("Do you want to post bills?");
            }
            if (cont)
            {
                DateTime starttime = DateTime.Now;
                FrmLoading frmLoading = new FrmLoading();
                frmLoading.Show();
                invoicedate = ssprocessds.view_distinctbillinginvoice[0].inv_date;
                for (int s = 0; s <= ssprocessds.view_distinctbillinginvoice.Rows.Count - 1; s++)
                {
                    ssprocessds.arinvoice.Rows.Clear();
                    EstablishBlankDataTableRow(ssprocessds.arinvoice);
                    ssprocessds.arinvoice[0].invno = ssprocessds.view_distinctbillinginvoice[s].inv_number;
                    ssprocessds.arinvoice[0].invdte = ssprocessds.view_distinctbillinginvoice[s].inv_date;
                    ssprocessds.arinvoice[0].tax = ssprocessds.view_distinctbillinginvoice[s].tax;
                    ssprocessds.arinvoice[0].invamt = ssprocessds.view_distinctbillinginvoice[s].pretax + ssprocessds.view_distinctbillinginvoice[s].tax;
                    ssprocessds.arinvoice[0].custno = ssprocessds.view_distinctbillinginvoice[s].bill_store.Substring(0, 4);
                    ssprocessds.arinvoice[0].invtype = "I";
                    ssprocessds.arinvoice[0].invstatus = "A";
                    ssprocessds.arinvoice[0].description = "Operating Invoice";
                    GenerateAppTableRowSave(ssprocessds.arinvoice[0]);

                    // Mark invoice posted

                    ClearParameters();
                    AddParms("@inv_number", ssprocessds.view_distinctbillinginvoice[s].inv_number, "SQL");
                    commandstring = "UPDATE billing SET billstat = 'P' WHERE inv_number = @inv_number";
                    ExecuteCommand(commandstring, CommandType.Text);

                }
                // Check for Morphis Bills
                ssprocessds.morphisinvoice.Rows.Clear();
                commandstring = "SELECT invno, custno, SUM(invamt) AS invamt, SUM(taxamt) as taxamt FROM  morphisinvoice WHERE invdte =  @invdte group by invno, custno ORDER BY invno ";
                ClearParameters();
                AddParms("@invdte", invoicedate, "SQL");
                FillData(ssprocessds, "morphisinvoice", commandstring, CommandType.Text);
                if (ssprocessds.morphisinvoice.Rows.Count > 0)
                {
                    for (int s = 0; s <= ssprocessds.morphisinvoice.Rows.Count - 1; s++)
                    {
                        ssprocessds.arinvoice.Rows.Clear();
                        EstablishBlankDataTableRow(ssprocessds.arinvoice);
                        ssprocessds.arinvoice[0].invno = ssprocessds.morphisinvoice[s].invno;
                        ssprocessds.arinvoice[0].invdte = invoicedate;
                        ssprocessds.arinvoice[0].tax = ssprocessds.morphisinvoice[s].taxamt;
                        ssprocessds.arinvoice[0].invamt = ssprocessds.morphisinvoice[s].invamt;
                        ssprocessds.arinvoice[0].custno = ssprocessds.morphisinvoice[s].custno;
                        ssprocessds.arinvoice[0].invtype = "I";
                        ssprocessds.arinvoice[0].invstatus = "A";
                        ssprocessds.arinvoice[0].description = "Morphis Invoice";
                        GenerateAppTableRowSave(ssprocessds.arinvoice[0]);
                       

                    }
        
                }
                frmLoading.Close();
                wsgUtilities.wsgNotice("Billing Posted");
                wsgUtilities.wsgNotice("Elapsed time- " + DateTime.Now.Subtract(starttime).TotalMinutes.ToString("0.##"));
            }

        }



    }
}
