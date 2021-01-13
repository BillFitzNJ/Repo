using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Data;
using WSGUtilitieslib;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CommonAppClasses
{

    public class VFPAcdataMethods : WSGDataAccess
    {
        public VFPAcdata vfpacdatads = new VFPAcdata();
        public VFPAcdataMethods()
            : base("VFP", "VFPAcDataPath")
        {

        }


        public void GetSlipsByDate(DateTime slip_date)
        {
            string datestring = String.Format("{0:MM/dd/yyyy}", slip_date);

            vfpacdatads.slips.Rows.Clear();
            this.oAdapter.TableMappings.Clear();
            this.oAdapter.TableMappings.Add("Table", "slips");
            ClearParameters();
            this.AddVFPStringParm("@slip_date", 10, datestring);
            string CommandString = "SELECT * from slips01 WHERE DTOC(slip_date) = ?";
            this.FillData(vfpacdatads, "slips", CommandString, CommandType.Text);

        }



    } // class
    public class VFPArdataMethods : WSGDataAccess
    {
        VFPArdata vfpardatads = new VFPArdata();
        public VFPArdataMethods()
            : base("VFP", "VFPArDataPath")
        {

        }


        public void UpdateArcust(string custno, string company, string address1, string city, string state,
            string zip, string contact, string phone)
        {

            string commandstring = "UPDATE arcust01 SET company =?, address1 =?, city =?, state = ?, zip =?, contact =?, ";
            commandstring += "phone =?, adddate = DATE(), lckdate = DATE(), lckuser =?, adduser=? WHERE custno=?";
            ClearParameters();
            this.AddVFPStringParm("@company", 35, company);
            this.AddVFPStringParm("@address1", 30, address1);
            this.AddVFPStringParm("@city", 20, city);
            this.AddVFPStringParm("@state", 10, state);
            this.AddVFPStringParm("@zip", 10, zip);
            this.AddVFPStringParm("@contact", 20, contact);
            this.AddVFPStringParm("@phone", 20, phone);
            this.AddVFPStringParm("@lckuser", 4, AppUserClass.AppUserId);
            this.AddVFPStringParm("@adduser", 4, AppUserClass.AppUserId);
            this.AddVFPStringParm("@custno", 6, custno);
            int commandresult = this.ExecuteCommand(commandstring, CommandType.Text);
        }

        public string CreateArcust(string company, string address1, string city, string state,
          string zip, string contact, string phone)
        {
            string commandstring = "";
            string custno = "";
            // Create customer number   
            for (int customercount = 1; customercount <= 1000; customercount++)
            {
                // Create customer number
                vfpardatads.arcust.Rows.Clear();
                custno = company.Substring(0, 2) + customercount.ToString("0000");
                ClearParameters();
                this.AddVFPStringParm("@custno", 6, custno);
                commandstring = "SELECT * from arcust01 WHERE custno = ?";
                this.FillData(vfpardatads, "arcust", commandstring, CommandType.Text);
                if (vfpardatads.arcust.Rows.Count < 1)
                {
                    break;
                }
            }


            this.vfpardatads.arcust.Rows.Clear();
            this.oAdapter.TableMappings.Clear();
            this.oAdapter.TableMappings.Add("Table", "arcust");
            this.ClearParameters();
            // Create all columns to avoid nulls. Populate custno for later update

            commandstring = "INSERT INTO arcust01 (custgen, custno, adduser, adddate, addtime ";
            string ValuesString = "VALUES (' ', ?, ?, ?, ?";
            this.AddVFPStringParm("@custno", 6, custno);
            this.AddVFPStringParm("@adduser", 4, AppUserClass.AppUserId);
            this.AddVFPDateParm("@adddate", DateTime.Now);
            this.AddVFPStringParm("@addtime", 8, DateTime.Now.ToString("HH:mm:ss tt"));


            for (int i = 0; i < vfpardatads.arcust.Columns.Count; i++)
            {
                string columnname = vfpardatads.arcust.Columns[i].ColumnName;
                // Omit custno column
                if (vfpardatads.arcust.Columns[i].ColumnName == "custno")
                {
                    continue;
                }
                if (vfpardatads.arcust.Columns[i].ColumnName == "custgen")
                {
                    continue;
                }
                if (vfpardatads.arcust.Columns[i].ColumnName == "adduser")
                {
                    continue;
                }
                if (vfpardatads.arcust.Columns[i].ColumnName == "adddate")
                {
                    continue;
                }
                if (vfpardatads.arcust.Columns[i].ColumnName == "addtime")
                {
                    continue;
                }

                if (vfpardatads.arcust.Columns[i].DataType == typeof(System.String))
                {
                    commandstring += ", " + columnname;
                    ValuesString += ", '  '";
                    continue;
                }
                if (vfpardatads.arcust.Columns[i].DataType == typeof(System.Decimal))
                {
                    commandstring += ", " + columnname;
                    ValuesString += ", 0";
                    continue;
                }

                if (vfpardatads.arcust.Columns[i].DataType == typeof(System.DateTime))
                {
                    commandstring += ", " + columnname;
                    ValuesString += ", CTOD('  /  /  ')";
                    continue;
                }
                if (vfpardatads.arcust.Columns[i].DataType == typeof(System.Boolean))
                {
                    commandstring += ", " + columnname;
                    ValuesString += ", .F.";
                }
                if (vfpardatads.arcust.Columns[i].DataType == typeof(System.Int32))
                {
                    commandstring += ", " + columnname;
                    ValuesString += ", 0";
                    continue;
                }
                if (vfpardatads.arcust.Columns[i].DataType == typeof(System.Int16))
                {
                    commandstring += ", " + columnname;
                    ValuesString += ", 0";
                    continue;
                }
            }
            commandstring += ") " + ValuesString + ")";
            int commandresult = this.ExecuteCommand(commandstring, CommandType.Text);
            // Now populate the remaining columns.
            UpdateArcust( custno, company, address1, city,  state, zip, contact, phone) ;

            return custno;
        }



    } // class
}
