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
    public class ProcessBillGeneration : WSGDataAccess
    {

        WSGUtilities wsgUtilities = new WSGUtilities("Billing Generation");
        AppUtilities appUtilities = new AppUtilities();
        CommonAppDataMethods commonAppDataMethods = new CommonAppDataMethods();
        public Billingappdata billingappdata = new Billingappdata();
        string billingcompany = "";
        string nextinvno = " ";
        string billstore = "";
        string storeselectstring = "";
        DateTime starttime = new DateTime();
        string billgroup = "";
        decimal taxrate = 0;
        decimal taxarea = 0;
        decimal taxable = 0;
        decimal dropcharges = 0;
        string commandstring = "";
        FrmLoading frmLoading = new FrmLoading();
        ssprocess sssearcgprocessds = new ssprocess();
        ssprocess ssprocessds = new ssprocess();
        BackgroundWorker billWorker = new BackgroundWorker();
        ssprocess workingcursords = new ssprocess();
        ssprocess storeds = new ssprocess();
        ssprocess tempds = new ssprocess();
        ssprocess testds = new ssprocess();

        public ProcessBillGeneration()
            : base("SQL", "SQLConnString")
        {
            SetIdcol(ssprocessds.billing.idcolColumn);
            SetIdcol(workingcursords.billing.idcolColumn);
            SetIdcol(tempds.billing.idcolColumn);
        }



        public bool ValidateInvoiceDate(DateTime invoicedate)
        {
            bool cont = true;
            testds.billing.Rows.Clear();
            commandstring = "select top 1 * from billing where inv_date = @invdate and billstat = 'P'";
            ClearParameters();
            AddParms("@invdate", invoicedate, "SQL");
            FillData(testds, "billing", commandstring, CommandType.Text);
            if (testds.billing.Rows.Count > 0)
            {
                cont = false;
            }
            return cont;
        }

        public void generatebills()
        {

            starttime = DateTime.Now;
            frmLoading.Text = "Generating Bills";
            frmLoading.progressBarLoading.Step = 1;
            frmLoading.Show();

            // Run bill creation in the background


            billWorker.DoWork += new DoWorkEventHandler(CreateBills);
            billWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(billsfinished);
            billWorker.WorkerReportsProgress = true;
            billWorker.ProgressChanged += new ProgressChangedEventHandler(billworkerProgressChanged);
            billWorker.RunWorkerAsync();

        }


        private void billworkerProgressChanged(object sender,
           ProgressChangedEventArgs e)
        {
            frmLoading.progressBarLoading.PerformStep();
            frmLoading.BringToFront();
        }

        private void billsfinished(
            object sender, RunWorkerCompletedEventArgs e)
        {
            DateTime endtime = DateTime.Now;
            wsgUtilities.wsgNotice("Billing Generated");
            wsgUtilities.wsgNotice("Elapsed time- " + endtime.Subtract(starttime).TotalMinutes.ToString("0.##"));
            frmLoading.Close();
        }

        public void CreateBills(object sender, DoWorkEventArgs e)
        {



            // Establish invoice date range
            int daysinbillingmonth = DateTime.DaysInMonth(billingappdata.billingyear, billingappdata.billingmonth);
            billingappdata.startdate = new DateTime(billingappdata.billingyear, billingappdata.billingmonth, 1);
            billingappdata.enddate = new DateTime(billingappdata.billingyear, billingappdata.billingmonth, daysinbillingmonth);
            DateTime starttime = new DateTime();
            DateTime endtime = new DateTime();

            // Establish next invoice number
            ClearParameters();
            ssprocessds.sysdata.Rows.Clear();
            commandstring = "SELECT * from sysdata";
            FillData(ssprocessds, "sysdata", commandstring, CommandType.Text);

            nextinvno = billingappdata.enddate.Year.ToString() + "-" + ssprocessds.sysdata[0].nextinvno.ToString().TrimEnd().TrimStart().PadLeft(5, '0');

            // Load any prior bills for this period  to obtain invoice numbers.
            ClearParameters();
            ssprocessds.view_billinginvnumber.Rows.Clear();
            commandstring = "SELECT DISTINCT inv_number, bill_store, atmcomp, bill_group FROM billing where CONVERT(date,inv_date) = @enddate";
            AddParms("@enddate", billingappdata.enddate, "SQL");
            FillData(ssprocessds, "view_billinginvnumber", commandstring, CommandType.Text);


            commandstring = "select * from view_storebillingdata where start_date  <= @enddate and stop_date >= @startdate  AND LEFT(storecode,4) <> SPACE(4)  order by storecode";
            ssprocessds.view_storebillingdata.Rows.Clear();
            ClearParameters();
            AddParms("@startdate", billingappdata.startdate, "SQL");
            AddParms("@enddate", billingappdata.enddate, "SQL");
            FillData(ssprocessds, "view_storebillingdata", commandstring, CommandType.Text);
            ssprocessds.billing.Rows.Clear();

            frmLoading.progressBarLoading.Maximum = ssprocessds.view_storebillingdata.Rows.Count;
            frmLoading.BringToFront();

            for (int s = 0; s <= ssprocessds.view_storebillingdata.Rows.Count - 1; s++)
            {


                if (billingappdata.storecode.TrimEnd() != "")
                {
                    if (ssprocessds.view_storebillingdata[s].storecode != billingappdata.storecode)
                    {
                        continue;
                    }
                }
                else
                {
                    if (billingappdata.compcode.TrimEnd() != "")
                    {
                        if (ssprocessds.view_storebillingdata[s].storecode.Substring(0, 4) != billingappdata.compcode)
                        {
                            continue;
                        }
                    }

                }
                if (ssprocessds.view_storebillingdata[s].no_bill)
                {
                    continue;
                }

                // Eliminate no bill stores

                // Locate company 
                ssprocessds.company.Rows.Clear();
                commandstring = "SELECT * FROM company where comp_code =  @compcode";
                ClearParameters();
                AddParms("@compcode", ssprocessds.view_storebillingdata[s].storecode.Substring(0, 4));
                FillData(ssprocessds, "company", commandstring, CommandType.Text);
                if (ssprocessds.company.Rows.Count < 1)
                {
                    continue;
                }
                // Develop bill store variable to distinguish store bills and company bills
                if (ssprocessds.company[0].billtype == "C")
                {
                    billstore = ssprocessds.company[0].comp_code;
                    billgroup = "1";
                }
                else
                {
                    billstore = ssprocessds.view_storebillingdata[s].storecode.Substring(0, 11);
                    billgroup = "2";

                }
                taxarea = ssprocessds.view_storebillingdata[s].tax_area;
                taxrate = ssprocessds.view_storebillingdata[s].taxrate;
                storeds.billing.Rows.Clear();

                // Billing functions
                //Slips
                if (!ssprocessds.company[0].atmowner)
                {
                    GenerateSlips(ssprocessds.view_storebillingdata[s].storecode, ssprocessds.view_storebillingdata[s].rte_base, ssprocessds.view_storebillingdata[s].packrate,
                        ssprocessds.view_storebillingdata[s].envelope, ssprocessds.company[0].atmfillrate, ssprocessds.view_storebillingdata[s].cg_drp_rt);
                }

                // Coin Orders

                // Develop bill type for coin orders
                if (ssprocessds.company[0].billtype == "C")
                {
                    billstore = ssprocessds.company[0].comp_code;
                }
                else
                {
                    billstore = ssprocessds.view_storebillingdata[s].storecode;
                }
                if (ssprocessds.company[0].billtype == "C")
                {
                    billstore = ssprocessds.company[0].comp_code;
                    billgroup = "1";
                }
                else
                {
                    billstore = ssprocessds.view_storebillingdata[s].storecode.Substring(0, 11);
                    billgroup = "2";

                }


                switch (ssprocessds.company[0].coinbilltype)
                {
                    case "M":
                        {
                            // Do nothing. Use the existing bill group
                            break;
                        }
                    case "C":
                        {
                            billgroup = "3";
                            billstore = ssprocessds.company[0].comp_code;

                            break;
                        }
                    case "S":
                        {
                            billgroup = "3";
                            billstore = ssprocessds.view_storebillingdata[s].storecode;

                            break;
                        }
                }
                if (!ssprocessds.company[0].atmowner)
                {
                    GenerateCoin(ssprocessds.view_storebillingdata[s].storecode, ssprocessds.company[0].rollrate, ssprocessds.company[0].straprate, ssprocessds.view_storebillingdata[s].cg_drp_rt, ssprocessds.view_storebillingdata[s].flatdrop, ssprocessds.company[0].coinsurcharge);
                }
                bool caculatedeposits = false;

                switch (ssprocessds.company[0].comp_code)
                {
                    case "9730":
                        {
                            caculatedeposits = true;
                            break;
                        }

                    case "5920":
                        {
                            caculatedeposits = true;
                            break;
                        }

                    case "5921":
                        {
                            caculatedeposits = true;
                            break;
                        }

                }

                if (ssprocessds.company[0].deporate > 0)
                {
                    caculatedeposits = true;
                }

                if (caculatedeposits)
                {
                    if (ssprocessds.company[0].billtype == "C")
                    {
                        billstore = ssprocessds.company[0].comp_code;
                        billgroup = "1";
                    }
                    else
                    {
                        billstore = ssprocessds.view_storebillingdata[s].storecode.Substring(0, 11);
                        billgroup = "2";

                    }

                    // Develop bill type for deposits
                    if (ssprocessds.company[0].billtype == "C")
                    {
                        billstore = ssprocessds.company[0].comp_code;
                    }
                    else
                    {
                        billstore = ssprocessds.view_storebillingdata[s].storecode;
                    }

                    switch (ssprocessds.company[0].depositsbilltype)
                    {
                        case "M":
                            {
                                break;
                            }
                        case "C":
                            {
                                billgroup = "4";
                                billstore = ssprocessds.company[0].comp_code;

                                break;
                            }
                        case "S":
                            {
                                billgroup = "4";
                                billstore = ssprocessds.view_storebillingdata[s].storecode;

                                break;
                            }
                    }

                    if (!ssprocessds.company[0].atmowner)
                    {
                        GenerateDeposits(ssprocessds.view_storebillingdata[s].storecode, ssprocessds.company[0].deporate, ssprocessds.company[0].deptickfee);
                    }
                  }

                // Develop bill store variable to distinguish store bills and company bills
                if (ssprocessds.company[0].billtype == "C")
                {
                    billstore = ssprocessds.company[0].comp_code;
                    billgroup = "1";
                }
                else
                {
                    billstore = ssprocessds.view_storebillingdata[s].storecode.Substring(0, 11);
                    billgroup = "2";

                }
                if (!ssprocessds.company[0].atmowner)
                {
                    GenerateRecurringcharges(ssprocessds.view_storebillingdata[s].storecode);
                    GenerateSpecialcharges(ssprocessds.view_storebillingdata[s].storecode);
                }
                   if (ssprocessds.company[0].atmowner)
                {
                    // Generate ATM Fill and service calls
                   GenerateAtmFills(ssprocessds.view_storebillingdata[s].storecode, ssprocessds.company[0].atmfillrate, ssprocessds.company[0].atmemerfillrate, ssprocessds.company[0].atmunschedfillrate);
                   GenerateAtmService(ssprocessds.view_storebillingdata[s].storecode, ssprocessds.company[0].atmrepfir, ssprocessds.company[0].atmrepsec, ssprocessds.company[0].atmfillrate);
                }
                billWorker.ReportProgress(s);



                if (storeds.billing.Rows.Count > 0)
                {
                    dropcharges = 0;
                    // If needed, compute Compliance fee and Fuel Surcharge
                    if (ssprocessds.company[0].compliance_rate != 0)
                    {
                        dropcharges = Convert.ToDecimal(storeds.billing.Compute("Sum(charged)", "bill_type = 'REGULAR DROPS' OR bill_type = 'REGULARD DROP'"));
                        if (dropcharges > 0)
                        {
                            AddTempBillingline(ssprocessds.view_storebillingdata[s].storecode);
                            tempds.billing[0].rate_base = Math.Round((ssprocessds.company[0].compliance_rate / 100) * dropcharges, 2);
                            tempds.billing[0].charged = Math.Round((ssprocessds.company[0].compliance_rate / 100) * dropcharges, 2);
                            tempds.billing[0].slip_date = billingappdata.enddate;
                            tempds.billing[0].tax_rate = taxrate;
                            tempds.billing[0].num_drops = 1;
                            tempds.billing[0].trancode = "CF";
                            tempds.billing[0].bill_type = "COMPLIANCE FEE";
                            SaveTempBillingLine();
                        }

                    }
                    if (ssprocessds.company[0].fuel_rate != 0)
                    {

                        dropcharges = Convert.ToDecimal(storeds.billing.Compute("Sum(charged)", "bill_type = 'REGULAR DROPS' OR bill_type = 'REGULARD DROP'"));
                        if (dropcharges > 0)
                        {
                            AddTempBillingline(ssprocessds.view_storebillingdata[s].storecode);
                            tempds.billing[0].rate_base = Math.Round((ssprocessds.company[0].fuel_rate / 100) * dropcharges, 2);
                            tempds.billing[0].charged = Math.Round((ssprocessds.company[0].fuel_rate / 100) * dropcharges, 2);
                            tempds.billing[0].slip_date = billingappdata.enddate;
                            tempds.billing[0].tax_rate = taxrate;
                            tempds.billing[0].num_drops = 1;
                            tempds.billing[0].trancode = "SS";
                            tempds.billing[0].bill_type = "FUEL SURCHARGE";
                            SaveTempBillingLine();
                        }
                    }


                    // After all store billing items have been added, compute store tax
                    if (taxrate > 0)
                    {
                        taxable = 0;
                        for (int t = 0; t <= storeds.billing.Rows.Count - 1; t++)
                        {
                            if (storeds.billing[t].tax_rate > 0)
                            {
                                taxable += storeds.billing[t].charged;
                            }
                        }
                        if (taxable != 0)
                        {
                            // Tax only goes on the first row
                            storeds.billing[0].storetxbl = taxable;
                            storeds.billing[0].storetax = Math.Round(taxable * (taxrate / 100), 2);
                        }
                    }




                    //Import store rows
                    for (int t = 0; t <= storeds.billing.Rows.Count - 1; t++)
                    {
                        workingcursords.billing.ImportRow(storeds.billing[t]);
                    }
                }
            }


            // Delete any bills for this period
            commandstring = "DELETE FROM billing where inv_date = @invdate ";
            ClearParameters();
            AddParms("@invdate", billingappdata.enddate, "SQL");


            if (billingappdata.storecode.TrimEnd() != "")
            {
                commandstring = "DELETE FROM billing where inv_date = @invdate AND storecode = @storecode ";
                AddParms("@storecode", billingappdata.storecode, "SQL");

            }
            else
            {
                if (billingappdata.compcode.TrimEnd() != "")
                {
                    commandstring = "DELETE FROM billing where inv_date = @invdate AND LEFT(storecode,4) = @compcode ";
                    AddParms("@compcode", billingappdata.compcode, "SQL");

                }

            }


            ExecuteCommand(commandstring, CommandType.Text);

            // Assign invoice numbers, save billing data

            // Create a one data row for each invoice group
            frmLoading.Text = "Assigning Invoice Numbers";
            frmLoading.progressBarLoading.Value = 1;
            DataView billview = new DataView(workingcursords.billing);
            DataTable dtinvnumber = billview.ToTable(true, "inv_number", "bill_store", "atmcomp", "bill_group");
            frmLoading.progressBarLoading.Maximum = dtinvnumber.Rows.Count;
            frmLoading.progressBarLoading.Value = 1;
            int invno = ssprocessds.sysdata[0].nextinvno;
            foreach (DataRow s in dtinvnumber.Rows)
            {

                billWorker.ReportProgress(1);
                // See if this group has already been assigned an invoice number

                storeselectstring = "bill_store='" + s[1] + "' AND atmcomp = '" + s[2] + "' AND  bill_group = '" + s[3] + "'";
                // See if the group has been an invoice number in previously generated bills
                DataRow[] priorresult = ssprocessds.view_billinginvnumber.Select(storeselectstring);
                if (priorresult.Length > 0)
                {
                    s[0] = priorresult[0]["inv_number"].ToString();
                }
                if (s[0].ToString().TrimEnd() == "")
                {

                    s[0] = billingappdata.enddate.Year.ToString() + "-" + invno.ToString().TrimEnd().TrimStart().PadLeft(5, '0');
                    invno++;

                }
            }
            ClearParameters();
            commandstring = "UPDATE sysdata SET nextinvno = @nextinvno";
            AddParms("@nextinvno", invno);
            ExecuteCommand(commandstring, CommandType.Text);

            // Now apply the invoice number to the billing row
            DataTable result = new DataTable();
            for (int s = 0; s <= workingcursords.billing.Rows.Count - 1; s++)
            {
                result = dtinvnumber.Select("bill_store = '" + workingcursords.billing[s].bill_store + "' AND bill_group = '" + workingcursords.billing[s].bill_group + "'").CopyToDataTable();
                if (result.Rows.Count > 0)
                {
                    workingcursords.billing[s].inv_number = result.Rows[0].Field<string>(0);
                    GenerateAppTableRowSave(workingcursords.billing[s]);
                }
            }


        }

        private void GenerateAtmFills(string storecode, decimal atmfillrate, decimal atmemerfillrate, decimal atmunschedfillrate)
        {
            ssprocessds.view_ATMDropNet.Rows.Clear();
            ClearParameters();
            AddParms("@storecode", storecode, "SQL");
            AddParms("@startdate", billingappdata.startdate, "SQL");
            AddParms("@enddate", billingappdata.enddate, "SQL");
            commandstring = "SELECT * FROM view_atmdropnet  WHERE store = @storecode AND dropdate >= @startdate AND dropdate <=  @enddate";
            FillData(ssprocessds, "view_atmdropnet", commandstring, CommandType.Text);
            for (int r = 0; r <= ssprocessds.view_ATMDropNet.Rows.Count - 1; r++)
            {

                // Eliminate zero amounts. They might be caused by returned to vault transactions.
                if (ssprocessds.view_ATMDropNet[r].hundreds + ssprocessds.view_ATMDropNet[r].fiftys + ssprocessds.view_ATMDropNet[r].twentys
                    + ssprocessds.view_ATMDropNet[r].tens + ssprocessds.view_ATMDropNet[r].fives + ssprocessds.view_ATMDropNet[r].ones == 0)
                {
                    continue;
                }
                AddTempBillingline(storecode);
                tempds.billing[0].tax_rate = taxrate;
                if (ssprocessds.view_ATMDropNet[r].erfill == "Y" && atmemerfillrate != 0)
                {
                    tempds.billing[0].charged = atmemerfillrate;
                }
                else
                {
                    if (ssprocessds.view_ATMDropNet[r].unschedfill == "Y" && atmunschedfillrate != 0)
                    {
                        tempds.billing[0].charged = atmunschedfillrate;
                    }
                    else
                    {
                        tempds.billing[0].charged = atmfillrate;
                    }
                }
                tempds.billing[0].rate_base = tempds.billing[0].charged;
                tempds.billing[0].num_drops = 1;
                tempds.billing[0].atmid = ssprocessds.view_ATMDropNet[r].atmid;
                tempds.billing[0].slip_date = ssprocessds.view_ATMDropNet[r].dropdate;
                tempds.billing[0].trancode = "FC";
                tempds.billing[0].bill_type = "FILL CANISTER";
                SaveTempBillingLine();

            }
        }

        private void GenerateAtmService(string storecode, decimal atmrepfir, decimal atmrepsec, decimal atmfillrate)
        {
            decimal batteryrate = 1.99m;
            commandstring = "SELECT * FROM atmservice WHERE store = @store AND orderdate BETWEEN @startdate AND @enddate";
            ssprocessds.atmservice.Rows.Clear();
            ClearParameters();
            AddParms("@store", storecode, "SQL");
            AddParms("@startdate", billingappdata.startdate, "SQL");
            AddParms("@enddate", billingappdata.enddate, "SQL");
            FillData(ssprocessds, "atmservice", commandstring, CommandType.Text);

            for (int s = 0; s <= ssprocessds.atmservice.Rows.Count - 1; s++)
            {
                AddTempBillingline(storecode);
                tempds.billing[0].tax_rate = taxrate;
                tempds.billing[0].num_drops = 1;
                tempds.billing[0].atmid = ssprocessds.atmservice[s].atmid;
                tempds.billing[0].slip_date = ssprocessds.atmservice[s].orderdate;
                tempds.billing[0].trancode = "AR";
                tempds.billing[0].bill_type = "SERVICE CALL";
                if (ssprocessds.atmservice[s].batterycount > 0)
                {
                    tempds.billing[0].charged = ssprocessds.atmservice[s].batterycount * batteryrate;

                }
                else
                {
                    switch (ssprocessds.atmservice[s].servicetype)
                    {
                        case "D":
                            {
                                tempds.billing[0].charged = atmfillrate;
                                break;
                            }
                        case "S":
                            {
                                tempds.billing[0].charged = atmrepsec;
                                break;
                            }

                        case "U":
                            {
                                tempds.billing[0].charged = atmrepfir;
                                tempds.billing[0].bill_type = "Unable to load " + ssprocessds.atmservice[s].atmid.TrimEnd();
                                break;
                            }
         
                        default:
                            {
                                tempds.billing[0].charged = atmrepfir;
                                break;
                       
                            }
                    }         
                }
                tempds.billing[0].num_drops = 1;
                tempds.billing[0].rate_base = tempds.billing[0].charged;
                SaveTempBillingLine();

            }
        }
        private void GenerateDeposits(string storecode, decimal deporate, decimal depticketfee)
        {
            bool usenoterate = false;
            decimal noterate = 0m;
            bool usesafedeposits = false;
            bool applyminimum = true;
            int numdrops = 0;

            // Certain clients use a note rate, rather than a volume rate
            switch (storecode.Substring(0, 4))
            {
                case "9730":
                    {
                        noterate = .75m;
                        usenoterate = true;
                        break;
                    }
                case "5920":
                    {
                        noterate = .015m;
                        usenoterate = true;
                        break;
                    }
                case "5921":
                    {
                        noterate = .015m;
                        usenoterate = true;
                        break;
                    }


            }
            ssprocessds.depdetail.Rows.Clear();
            {
                // Regular deposits
                commandstring = "SELECT * FROM depdetail  WHERE store = @store   AND postingdate >=  @startdate AND postingdate <= @enddate AND (eventcode  = 'V'  OR eventcode = 'C')";
                ClearParameters();
                ssprocessds.coindrop.Rows.Clear();
                AddParms("@store", storecode, "SQL");
                AddParms("@startdate", billingappdata.startdate, "SQL");
                AddParms("@enddate", billingappdata.enddate, "SQL");
                FillData(ssprocessds, "depdetail", commandstring, CommandType.Text);

                if (ssprocessds.depdetail.Rows.Count > 0)
                {


                    for (int s = 0; s <= ssprocessds.depdetail.Rows.Count - 1; s++)
                    {
                        AddTempBillingline(storecode);
                        if (!usenoterate)
                        {
                            tempds.billing[0].rate_base = deporate;
                        }
                        else
                        {
                            tempds.billing[0].rate_base = noterate;
                        }
                        tempds.billing[0].slip_date = ssprocessds.depdetail[s].postingdate;
                        tempds.billing[0].tax_rate = 0;
                        tempds.billing[0].trancode = "DP";
                        if (!usenoterate)
                        {

                            tempds.billing[0].bill_type = "TOTAL DEPOSIT";
                        }
                        else
                        {
                            tempds.billing[0].bill_type = "NOTES COUNTED";
                        }
                        if (ssprocessds.depdetail[s].bankfedid.Substring(0, 5) == "COUNT")
                        {
                            // Counting customer
                            tempds.billing[0].num_drops = ssprocessds.depdetail[s].total_dep;

                        }
                        else
                        {
                            if (!usenoterate)
                            {
                                // Safekeeping customer
                                tempds.billing[0].num_drops = ssprocessds.depdetail[s].hundreds + ssprocessds.depdetail[s].fiftys + ssprocessds.depdetail[s].twentys +
                                    ssprocessds.depdetail[s].tens + ssprocessds.depdetail[s].fives + ssprocessds.depdetail[s].ones + ssprocessds.depdetail[s].sba +
                                    ssprocessds.depdetail[s].halves + ssprocessds.depdetail[s].quarters + ssprocessds.depdetail[s].dimes + ssprocessds.depdetail[s].nickels +
                                    ssprocessds.depdetail[s].pennies + ssprocessds.depdetail[s].mixedcoin;
                            }
                            else
                            {
                                tempds.billing[0].num_drops = ssprocessds.depdetail[s].hundreds / 100 + ssprocessds.depdetail[s].fiftys / 50 + ssprocessds.depdetail[s].twentys / 20 +
                                 ssprocessds.depdetail[s].tens / 10 + ssprocessds.depdetail[s].fives / 5 + ssprocessds.depdetail[s].ones;
                            }
                        }
                        if (!usenoterate)
                        {
                            tempds.billing[0].charged = Math.Round(deporate * (tempds.billing[0].num_drops / 1000), 2);
                        }
                        else
                        {
                            tempds.billing[0].charged = Math.Round(tempds.billing[0].num_drops * noterate, 2);
                        }
                        // Apply the minimum charge per bag, with come exceptions
                        switch (storecode.Substring(0, 4))
                        {
                            case "4950":
                                {
                                    applyminimum = false;
                                    break;
                                }

                            case "8175":
                                {
                                    applyminimum = false;
                                    break;
                                }
                            case "0315":
                                {
                                    applyminimum = false;
                                    break;
                                }
                            case "0323":
                                {
                                    applyminimum = false;
                                    break;
                                }
                            default:
                                {
                                    applyminimum = true;
                                    break;
                                }
                        }
                        if (applyminimum)
                        {
                            if (tempds.billing[0].charged < deporate)
                            {
                                tempds.billing[0].charged = deporate;
                            }
                        }
                        SaveTempBillingLine();
                        // If applicable, apply a deposit ticket fee for each bag counted
                        if (depticketfee != 0)
                        {
                            AddTempBillingline(storecode);
                            tempds.billing[0].rate_base = depticketfee;
                            tempds.billing[0].charged = depticketfee;
                            tempds.billing[0].num_drops = 1;
                            tempds.billing[0].slip_date = ssprocessds.depdetail[s].postingdate;
                            tempds.billing[0].tax_rate = 0;
                            tempds.billing[0].trancode = "DF";
                            tempds.billing[0].bill_type = "DEPOSIT FEE";
                            SaveTempBillingLine();

                        }

                    }
                }
                else
                {
                    // No counting rows
                    // Check for smartsafe transactions

                    // Certain companies use safedeposits - special formula

                    switch (storecode.Substring(0, 4))
                    {
                        case "1141":
                            {
                                usesafedeposits = true;
                                break;
                            }
                        case "1142":
                            {
                                usesafedeposits = true;
                                break;
                            }

                        case "1143":
                            {
                                usesafedeposits = true;
                                break;
                            }
                        case "1144":
                            {
                                usesafedeposits = true;
                                break;
                            }
                        case "1146":
                            {
                                usesafedeposits = true;
                                break;
                            }

                        case "1147":
                            {
                                usesafedeposits = true;
                                break;
                            }

                    }
                    if (!usesafedeposits)
                    {
                        ssprocessds.smartsafetrans.Rows.Clear();
                        // Use smartsafe transactions
                        commandstring = "SELECT * FROM smartsafetrans WHERE LEFT(STORE,11) =  @store AND LEFT(eventcode, 3) = 'VER' AND postingdate >= @startdate AND postingdate <= @enddate";
                        ClearParameters();
                        AddParms("@store", storecode, "SQL");
                        AddParms("@startdate", billingappdata.startdate, "SQL");
                        AddParms("@enddate", billingappdata.enddate, "SQL");
                        FillData(ssprocessds, "smartsafetrans", commandstring, CommandType.Text);
                        if (ssprocessds.smartsafetrans.Rows.Count > 0)
                        {
                            for (int s = 0; s <= ssprocessds.smartsafetrans.Rows.Count - 1; s++)
                            {
                                AddTempBillingline(storecode);
                                tempds.billing[0].rate_base = deporate;
                                tempds.billing[0].slip_date = ssprocessds.smartsafetrans[s].postingdate;
                                tempds.billing[0].tax_rate = 0;
                                tempds.billing[0].num_drops = ssprocessds.smartsafetrans[s].hundreds + ssprocessds.smartsafetrans[s].fiftys + ssprocessds.smartsafetrans[s].twentys +
                                    ssprocessds.smartsafetrans[s].tens + ssprocessds.smartsafetrans[s].fives +
                                    ssprocessds.smartsafetrans[s].twos + ssprocessds.smartsafetrans[s].ones;
                                tempds.billing[0].charged = Math.Round(deporate * (tempds.billing[0].num_drops / 1000), 2);
                                tempds.billing[0].trancode = "DP";
                                tempds.billing[0].bill_type = "TOTAL DEPOSIT";
                                SaveTempBillingLine();
                            }
                        }
                    }
                    else
                    {
                        ssprocessds.view_smartsafedeposits.Rows.Clear();
                        commandstring = " select  d.*, m.storecode from view_smartsafedeposits d inner join safemast m on d.serialnumber = m.serialnumber";
                        commandstring += " where year(depositdate) = year(@enddate) and month(depositdate) = month(@enddate)  and left(m.storecode,11) = @storecode";
                        ClearParameters();
                        AddParms("@storecode", storecode, "SQL");
                        AddParms("@enddate", billingappdata.enddate, "SQL");
                        FillData(ssprocessds, "view_smartsafedeposits", commandstring, CommandType.Text);
                        if (ssprocessds.view_smartsafedeposits.Rows.Count > 0)
                        {
                            for (int s = 0; s <= ssprocessds.view_smartsafedeposits.Count - 1; s++)
                            {
                                AddTempBillingline(storecode);
                                tempds.billing[0].rate_base = deporate;
                                tempds.billing[0].slip_date = ssprocessds.view_smartsafedeposits[s].depositdate.Date;
                                tempds.billing[0].tax_rate = 0;
                                tempds.billing[0].num_drops = ssprocessds.view_smartsafedeposits[s].deposits;
                                tempds.billing[0].charged = Math.Round(deporate * (tempds.billing[0].num_drops / 1000), 2);
                                tempds.billing[0].trancode = "DP";
                                tempds.billing[0].bill_type = "TOTAL DEPOSIT";
                                SaveTempBillingLine();
                            }

                        }
                    }

                }

            }
        }

        private void GenerateCoin(string storecode, decimal rollrate, decimal straprate, decimal droprate, decimal flatdrop, decimal coinsurcharge)
        {
            string calendardayofweek = "";
            string coindayofweek = "";
            int rollcount = 0;
            int strapcount = 0;
            // Include only stores with a rate for coin
            // There are two forms - regular coin and recurring coin


            // Regular coin
            commandstring = "SELECT * FROM coindrop WHERE store = @storecode AND  dropdate >= @startdate AND dropdate <=  @enddate";
            ClearParameters();
            ssprocessds.coindrop.Rows.Clear();
            AddParms("@storecode", storecode, "SQL");
            AddParms("@startdate", billingappdata.startdate, "SQL");
            AddParms("@enddate", billingappdata.enddate, "SQL");
            FillData(ssprocessds, "coindrop", commandstring, CommandType.Text);

            // Recurring Coin
            commandstring = "SELECT * FROM recurringcoin  WHERE LEFT(storecode,11) = LEFT(@storecode,11) ";
            ClearParameters();
            ssprocessds.recurringcoin.Rows.Clear();
            AddParms("@storecode", storecode, "SQL");
            FillData(ssprocessds, "recurringcoin", commandstring, CommandType.Text);
            int ds = 0;
            for (int r = 0; r <= ssprocessds.recurringcoin.Rows.Count - 1; r++)
            {
                DateTime runningdate = billingappdata.startdate;
                // Create a coin drop row for any recurring coin orders
                DateTime DropDate = billingappdata.startdate;
                ds = 0;
                coindayofweek = Convert.ToString(Convert.ToInt32(ssprocessds.recurringcoin[r].dayofweek) - 1);



                while (DropDate <= billingappdata.enddate)
                {
                    // Set day of the week forward to make Monday the first day of the week


                    calendardayofweek = Convert.ToString((int)DropDate.DayOfWeek);
                    if (coindayofweek == calendardayofweek)
                    {
                        ssprocessds.coindrop.Rows.Add();
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].store = ssprocessds.recurringcoin[r].storecode;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].dropdate = DropDate;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].store = ssprocessds.recurringcoin[r].storecode;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].hundreds = ssprocessds.recurringcoin[r].hundreds;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].fiftys = ssprocessds.recurringcoin[r].fiftys;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].twentys = ssprocessds.recurringcoin[r].twentys;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].tens = ssprocessds.recurringcoin[r].tens;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].fives = ssprocessds.recurringcoin[r].fives;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].ones = ssprocessds.recurringcoin[r].singles;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].quarters = ssprocessds.recurringcoin[r].quarters;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].dimes = ssprocessds.recurringcoin[r].dimes;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].nickels = ssprocessds.recurringcoin[r].nickels;
                        ssprocessds.coindrop[ssprocessds.coindrop.Rows.Count - 1].pennies = ssprocessds.recurringcoin[r].pennies;
                        ds += 7;

                    }
                    else
                    {
                        ds++;

                    }
                    DropDate = billingappdata.startdate.AddDays(ds);
                }
            }

            // Create flat drop charge if needed
            if (flatdrop != 0)
            {
                AddTempBillingline(storecode);
                tempds.billing[0].rate_base = flatdrop;
                tempds.billing[0].charged = flatdrop;
                tempds.billing[0].tax_rate = taxrate;
                tempds.billing[0].num_drops = 1;
                tempds.billing[0].trancode = "CD";
                tempds.billing[0].bill_type = "CURRENCY DROP";
                SaveTempBillingLine();
            }

            for (int d = 0; d <= ssprocessds.coindrop.Rows.Count - 1; d++)
            {
                if (droprate != 0)
                {
                    AddTempBillingline(storecode);
                    tempds.billing[0].rate_base = droprate;
                    tempds.billing[0].charged = droprate;
                    tempds.billing[0].tax_rate = taxrate;
                    tempds.billing[0].slip_date = billingappdata.enddate;
                    tempds.billing[0].num_drops = 1;
                    tempds.billing[0].trancode = "CD";
                    tempds.billing[0].bill_type = "CURRENCY DROP";
                    SaveTempBillingLine();

                }
                rollcount = (int)(ssprocessds.coindrop[d].quarters / 10 + ssprocessds.coindrop[d].dimes / 5 + ssprocessds.coindrop[d].nickels / 2 +
                   ssprocessds.coindrop[d].pennies / .5M + .9M);
                strapcount = (int)(ssprocessds.coindrop[d].hundreds / 10000 + .9M);
                strapcount += (int)(ssprocessds.coindrop[d].fiftys / 5000 + .9M);
                strapcount += (int)(ssprocessds.coindrop[d].twentys / 2000 + .9M);
                strapcount += (int)(ssprocessds.coindrop[d].tens / 1000 + .9M);
                strapcount += (int)(ssprocessds.coindrop[d].fives / 500 + .9M);
                strapcount += (int)(ssprocessds.coindrop[d].ones / 100 + .9M);

                if (rollcount != 0 && rollrate != 0)
                {
                    AddTempBillingline(storecode);
                    tempds.billing[0].rate_base = rollrate;
                    tempds.billing[0].num_drops = rollcount;
                    tempds.billing[0].trancode = "CN";
                    tempds.billing[0].slip_date = ssprocessds.coindrop[d].dropdate;
                    tempds.billing[0].charged = rollcount * rollrate;
                    tempds.billing[0].bill_type = "ROLLS COIN";
                    SaveTempBillingLine();
                }

                if (strapcount != 0 && straprate != 0)
                {
                    AddTempBillingline(storecode);
                    tempds.billing[0].rate_base = straprate;
                    tempds.billing[0].num_drops = strapcount;
                    tempds.billing[0].trancode = "ST";
                    tempds.billing[0].slip_date = ssprocessds.coindrop[d].dropdate;
                    tempds.billing[0].charged = straprate * strapcount;
                    tempds.billing[0].bill_type = "STRAPS BILLS";
                    SaveTempBillingLine();
                }
                if (coinsurcharge != 0)
                {
                    AddTempBillingline(storecode);
                    tempds.billing[0].rate_base = coinsurcharge;
                    tempds.billing[0].charged = coinsurcharge;
                    tempds.billing[0].slip_date = billingappdata.enddate;
                    tempds.billing[0].num_drops = 1;
                    tempds.billing[0].trancode = "ST";
                    tempds.billing[0].bill_type = "CHANGE PREP";
                    SaveTempBillingLine();

                }
            }


        }
        private void GenerateSlips(string storecode, decimal rte_base, decimal packrate, decimal enveloperate, decimal atmfillrate, decimal droprate)
        {
            // Include only stores with a rate for slips
            if (rte_base != 0  || atmfillrate  != 0 )
            {
                ssprocessds.slips.Rows.Clear();
                ClearParameters();
                if (billgroup == "1")
                {
                    commandstring = "SELECT SUM(num_drops) AS num_drops, chgcode, trancode  FROM slips   WHERE left(storecode,11) = @storecode AND slip_date >=  @startdate AND slip_date <= @enddate GROUP BY storecode, chgcode, trancode";
                }
                else
                {
                    commandstring = "SELECT slip_date, num_drops,chgcode, trancode FROM slips  WHERE storecode = @storecode AND slip_date >= @startdate AND slip_date <= @enddate";
                }
                AddParms("@storecode", storecode, "SQL");
                AddParms("@startdate", billingappdata.startdate, "SQL");
                AddParms("@enddate", billingappdata.enddate, "SQL");
                FillData(ssprocessds, "slips", commandstring, CommandType.Text);
                for (int r = 0; r <= ssprocessds.slips.Rows.Count - 1; r++)
                {
                    AddTempBillingline(storecode);
                    tempds.billing[0].tax_rate = taxrate;
                    if (billgroup == "1")
                    {
                        tempds.billing[0].slip_date = billingappdata.enddate;
                    }
                    else
                    {
                        tempds.billing[0].slip_date = ssprocessds.slips[r].slip_date;
                    }
                    tempds.billing[0].num_drops = ssprocessds.slips[r].num_drops;
                    switch (ssprocessds.slips[r].chgcode.TrimEnd())
                    {


                        case "FC":
                            {
                                tempds.billing[0].trancode = "FC";
                                tempds.billing[0].rate_base = rte_base;
                                tempds.billing[0].bill_type = "FILL CANISTER";
                                tempds.billing[0].charged = ssprocessds.slips[r].num_drops * atmfillrate;
                                break;
                            }
                        case "REGDRP":
                            {
                                tempds.billing[0].trancode = "SL";
                                tempds.billing[0].rate_base = rte_base;
                                tempds.billing[0].bill_type = "REGULAR DROPS";
                                tempds.billing[0].charged = ssprocessds.slips[r].num_drops * rte_base;
                                break;
                            }
                        case "PACK":
                            {
                                tempds.billing[0].rate_base = packrate;
                                tempds.billing[0].bill_type = "PACKAGING";
                                tempds.billing[0].trancode = "PK";
                                tempds.billing[0].charged = ssprocessds.slips[r].num_drops * packrate;
                                break;
                            }
                        case "PAY":
                            {
                                tempds.billing[0].rate_base = packrate;
                                tempds.billing[0].bill_type = "PAY ENVELOPE";
                                tempds.billing[0].trancode = "PE";
                                tempds.billing[0].charged = ssprocessds.slips[r].num_drops * enveloperate;
                                break;
                            }
                        case "CURDR":
                            {
                                tempds.billing[0].rate_base = droprate;
                                tempds.billing[0].bill_type = "CURRENCY DROP";
                                tempds.billing[0].trancode = "CD";
                                tempds.billing[0].charged = ssprocessds.slips[r].num_drops * droprate;
                                break;
                            }
                        default:
                            {
                                ClearParameters();
                                ssprocessds.chargetype.Rows.Clear();
                                commandstring = "SELECT * FROM chargetype WHERE chgcode = @chgcode";
                                AddParms("@chgcode", ssprocessds.slips[r].chgcode, "SQL");
                                FillData(ssprocessds, "chargetype", commandstring, CommandType.Text);
                                if (ssprocessds.chargetype.Rows.Count > 0)
                                {
                                    tempds.billing[0].rate_base = droprate;
                                    tempds.billing[0].bill_type = ssprocessds.chargetype[0].chgdesc;
                                    tempds.billing[0].trancode = ssprocessds.slips[r].trancode;
                                    tempds.billing[0].charged = ssprocessds.slips[r].num_drops * droprate;
                                }
                                else
                                {
                                    tempds.billing[0].rate_base = droprate;
                                    tempds.billing[0].bill_type = "CURRENCY DROP";
                                    tempds.billing[0].trancode = "CD";
                                    tempds.billing[0].charged = ssprocessds.slips[r].num_drops * droprate;
                                }

                                break;
                            }

                    }
                    if (tempds.billing[0].charged != 0)
                    {
                        SaveTempBillingLine();
                    }

                }
            }
        }
        private void GenerateRecurringcharges(string storecode)
        {
            TimeSpan duration = new TimeSpan();
            int billingdays = 1;
            commandstring = "SELECT * FROM recurringcharge WHERE LEFT(storecode,11) = @storecode";
            ClearParameters();
            ssprocessds.recurringcharge.Rows.Clear();
            AddParms("@storecode", storecode, "SQL");
            FillData(ssprocessds, "recurringcharge", commandstring, CommandType.Text);
            if (ssprocessds.recurringcharge.Rows.Count > 0)
            {
                for (int r = 0; r <= ssprocessds.recurringcharge.Rows.Count - 1; r++)
                {
                    AddTempBillingline(storecode);
                    if (ssprocessds.recurringcharge[r].chgtype.TrimEnd() != "CASH PROC")
                    {
                        tempds.billing[0].tax_rate = taxrate;
                  
                    }
                    else
                    {
                        tempds.billing[0].tax_rate = 0;
                        tempds.billing[0].tax_area = 0;

                    }


                    tempds.billing[0].slip_date = billingappdata.enddate;
                    tempds.billing[0].num_drops = 1;
                    if (ssprocessds.recurringcharge[r].chgfreq == "2")
                    {
                        duration = billingappdata.startdate.Date - billingappdata.enddate.Date;
                        billingdays = duration.Days + 1;
                        tempds.billing[0].charged = ssprocessds.recurringcharge[r].chgamt * billingdays;
                    }
                    else
                    {
                        tempds.billing[0].charged = ssprocessds.recurringcharge[r].chgamt;
                    }
                    tempds.billing[0].rate_base = tempds.billing[0].charged; 
                    // Develop bill type
                    commandstring = "SELECT * FROM chargetype  WHERE chgcode = @chgcode";
                    ClearParameters();
                    ssprocessds.chargetype.Rows.Clear();
                    AddParms("@chgcode", ssprocessds.recurringcharge[r].chgtype, "SQL");
                    FillData(ssprocessds, "chargetype", commandstring, CommandType.Text);
                    if (ssprocessds.chargetype.Rows.Count > 0)
                    {
                        tempds.billing[0].bill_type = ssprocessds.chargetype[0].chgdesc;
                    }
                    else
                    {
                        tempds.billing[0].bill_type = ssprocessds.recurringcharge[r].chgtype;
                    }
                    if (ssprocessds.recurringcharge[r].chgtype == "REGDRP")
                    {
                        tempds.billing[0].trancode = "SL";
                    }
                    SaveTempBillingLine();
                }
            }


        }

        private void GenerateSpecialcharges(string storecode)
        {
            commandstring = "SELECT * FROM view_expandedspecialcharge  WHERE storecode = @storecode   AND CONVERT(DATE, chgdate) >=  @startdate AND CONVERT(DATE, chgdate)  <= @enddate";
            ClearParameters();
            ssprocessds.view_expandedspecialcharge.Rows.Clear();
            AddParms("@storecode", storecode, "SQL");
            AddParms("@startdate", billingappdata.startdate, "SQL");
            AddParms("@enddate", billingappdata.enddate, "SQL");
            FillData(ssprocessds, "view_expandedspecialcharge", commandstring, CommandType.Text);
            if (ssprocessds.view_expandedspecialcharge.Rows.Count > 0)
            {
                for (int r = 0; r <= ssprocessds.view_expandedspecialcharge.Rows.Count - 1; r++)
                {

                    AddTempBillingline(storecode);
                    if (ssprocessds.view_expandedspecialcharge[r].taxable)
                    {
                        tempds.billing[0].tax_rate = taxrate;
                    }
                    tempds.billing[0].slip_date = billingappdata.enddate;
                    tempds.billing[0].num_drops = 1;
                    tempds.billing[0].rate_base = ssprocessds.view_expandedspecialcharge[r].chgamt;
                    tempds.billing[0].charged = ssprocessds.view_expandedspecialcharge[r].chgamt;
                    if (ssprocessds.view_expandedspecialcharge[r].orderno.TrimEnd().TrimStart() != "")
                    {
                        tempds.billing[0].bill_type = "Order #-" + ssprocessds.view_expandedspecialcharge[r].orderno.TrimEnd().TrimStart();
                    }
                    else
                    {
                        tempds.billing[0].bill_type = ssprocessds.view_expandedspecialcharge[r].chargename.TrimEnd();
                    }
                    SaveTempBillingLine();
                }
            }
        }
        private void AddTempBillingline(string storecode)
        {
            tempds.billing.Rows.Clear();
            tempds.billing.Rows.Add();
            EstablishBlankDataTableRow(tempds.billing);
            tempds.billing[0].storecode = storecode;
            tempds.billing[0].inv_date = billingappdata.enddate;
            tempds.billing[0].tax_area = taxarea;
            tempds.billing[0].bill_group = billgroup;
            tempds.billing[0].bill_store = billstore;
            tempds.billing[0].billstat = "U";
        }
        private void SaveTempBillingLine()
        {
            storeds.billing.ImportRow(tempds.billing[0]);
        }

    }

    public class Billingappdata
    {
        public int billingmonth { get; set; }
        public int billingyear { get; set; }
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
  
        public string storecode { get; set; }
        public string compcode { get; set; }
    }   

}
