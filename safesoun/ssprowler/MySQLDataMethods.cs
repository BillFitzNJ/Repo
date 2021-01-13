using System;
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
    public class MySQLDataMethods : WSGDataAccess
    {
        string commandtext = "";

        public mysqldata mysqltestds = new mysqldata();
        public mysqldata mysqlselectords = new mysqldata();
        public mysqldata mysqlds = new mysqldata();
        public mysqldata mysqlsearchds = new mysqldata();
   
        AppUtilities appUtilities = new AppUtilities();
        public MySQLDataMethods()
            : base("MySQL", "MySQLConnStringWithDB")
        {
            SetIdcol(mysqlds.store.idcolColumn);
            SetIdcol(mysqlds.user.idcolColumn);
            SetIdcol(mysqlds.storeregion.idcolColumn);
        }

        public void GetSingleUser(int idcol)
        {
            mysqlds.user.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", idcol);
            commandtext = "SELECT * FROM user WHERE idcol = @idcol ";

            FillData(mysqlds, "user", commandtext, CommandType.Text);
        }

        public bool UseridExists(string userid)
        {
            bool IdExists = false;
            mysqltestds.user.Rows.Clear();
            ClearParameters();
            AddParms("@userid", userid);
            commandtext = "SELECT * FROM user WHERE  userid = @userid";
            FillData(mysqltestds, "user", commandtext, CommandType.Text);
            if (mysqltestds.user.Rows.Count > 0)
            {
                IdExists = true;
            }
            return IdExists;
        }

        public void SaveMySQLDatatableRow(DataRow dr)
        {
            GenerateAppTableRowSave(dr);
        }
        public void GetUsers()
        {
            mysqlselectords.user.Rows.Clear();
            ClearParameters();
            commandtext = "SELECT * FROM user ORDER BY userid";
            FillData(mysqlselectords, "user", commandtext, CommandType.Text);
        }

        public string GetRegionName(string compcode, string regioncode)
        {
            string regionname = "";
            mysqlselectords.region.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            AddParms("@regioncode", regioncode);
            commandtext = "SELECT * FROM region WHERE compcode = @compcode AND regioncode = @regioncode";
            FillData(mysqlselectords, "region", commandtext, CommandType.Text);
            if (mysqlselectords.region.Rows.Count > 0)
            {
                regionname = mysqlselectords.region[0].regionname;
            }
            return regionname;
        }


        public void FillRegion(string compcode)
        {
            mysqlselectords.region.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            commandtext = "SELECT * FROM region WHERE compcode = @compcode  ORDER BY regioncode";
            FillData(mysqlselectords, "region", commandtext, CommandType.Text);
        }


        public void FillStoreRegion(string compcode, string regioncode)
        {
            mysqlselectords.storeregion.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            AddParms("@regioncode", regioncode);
            commandtext = "SELECT * FROM storeregion WHERE LEFT(storecode,4) = @compcode AND regioncode = @regioncode ORDER BY storecode";
            FillData(mysqlselectords, "storeregion", commandtext, CommandType.Text);
        }

        public void GetassignedRegionStores(string compcode, string regioncode)
        {
            mysqlselectords.store.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            AddParms("@regioncode", regioncode);
            commandtext = "SELECT * FROM store WHERE LEFT(storecode,4) = @compcode AND storecode IN (select storecode from storeregion WHERE regioncode = @regioncode) ORDER BY storecode";
            FillData(mysqlselectords, "store", commandtext, CommandType.Text);
        }
        public void GetUnassignedCompanyStores(string compcode)
        {
            mysqlselectords.store.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            commandtext = "SELECT * FROM store WHERE LEFT(storecode,4) = @compcode AND storecode NOT  IN (select storecode from storeregion ) ORDER BY storecode";
            FillData(mysqlselectords, "store", commandtext, CommandType.Text);
        }

        public void GetRegion(string compcode, string regioncode)
        {
            string regionname = "";
            mysqlds.region.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            AddParms("@regioncode", regioncode);
            commandtext = "SELECT * FROM region WHERE compcode = @compcode AND regioncode = @regioncode";
            FillData(mysqlds, "region", commandtext, CommandType.Text);
        }

        public void DeleteStoreRegion(string regioncode, string storecode)
        {
            ClearParameters();
            AddParms("@regioncode", regioncode);
            AddParms("@storecode", storecode);
            commandtext = "DELETE FROM storeregion WHERE regioncode = @regioncode AND storecode = @storecode";
            ExecuteCommand(commandtext, CommandType.Text);
        }

        public void GetRegions(string compcode)
        {
            mysqlselectords.region.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            commandtext = "SELECT * FROM region WHERE compcode = @compcode";
            FillData(mysqlselectords, "region", commandtext, CommandType.Text);
        }

        public void FillDropSchedule(string storecode)
        {
            mysqlds.dropschedule.Rows.Clear();
            commandtext = "SELECT * FROM dropschedule";
            ClearParameters();
            if (storecode.TrimEnd() != "")
            {
                AddParms("@storecode", storecode);
                commandtext += " WHERE storecode = @storecode";
            }
            FillData(mysqlds, "dropschedule", commandtext, CommandType.Text);
        }



        public void FillStore(string storecode)
        {
            mysqlsearchds.store.Rows.Clear();
            commandtext = "SELECT LEFT(storecode,11) AS storecode, store_name, f_address, f_city, f_state, phone FROM store";
            ClearParameters();
            if (storecode.TrimEnd() != "")
            {
                AddParms("@storecode", storecode);
                commandtext += " WHERE storecode = @storecode";
            }
            
            FillData(mysqlsearchds, "store", commandtext, CommandType.Text);
        }

        public void ClearTable(string tablename)
        {
            string commandstring = "DELETE FROM " + tablename;
            ExecuteCommand(commandstring, CommandType.Text);
        }

        public void DeleteDropSchedule(string storecode, string driver_1, decimal daynumber)
        {
            ClearParameters();
            AddParms("@storecode", storecode, "MySQL");
            AddParms("@driver_1", driver_1, "MySQL");
            AddParms("@daynumber", daynumber, "MySQL");
            commandtext = "DELETE FROM dropschedule WHERE storecode = @storecode AND driver_1 = @driver_1 AND daynumber = @daynumber";
            ExecuteCommand(commandtext, CommandType.Text);
        }
        public void InsertDropSchedule(string storecode, string driver_1, decimal daynumber, decimal sequence, string schday)
        {


            ClearParameters();
            AddParms("@storecode", storecode, "MySQL");
            AddParms("@driver_1", driver_1, "MySQL");
            AddParms("@daynumber", daynumber, "MySQL");
            AddParms("@sequence", sequence, "MySQL");
            AddParms("@schday", schday, "MySQL");

            commandtext = "INSERT INTO dropschedule (`storecode`, `driver_1`, `daynumber`, `sequence`, `schday`  ) VALUES (@storecode, @driver_1, @daynumber, @sequence, @schday)";
            ExecuteCommand(commandtext, CommandType.Text);
        }
        public void UpdateDropSchedule(string storecode, string driver_1, decimal daynumber, decimal sequence, string schday)
        {


            ClearParameters();
            AddParms("@storecode", storecode, "MySQL");
            AddParms("@driver_1", driver_1, "MySQL");
            AddParms("@daynumber", daynumber, "MySQL");
            AddParms("@sequence", sequence, "MySQL");
            AddParms("@schday", schday, "MySQL");

            commandtext = "UPDATE dropschedule SET `sequence` = @sequence, `schday` = @schday  WHERE storecode = @storecode AND driver_1 = @driver_1 AND daynumber = @daynumber";
            ExecuteCommand(commandtext, CommandType.Text);
        }
        public void InsertStore(mysqldata.storeRow Row)
        {
            mysqlds.store.Rows.Clear();
            mysqlds.store.Rows.Add();
            EstablishBlankDataTableRow(mysqlds.store);
            mysqlds.store[0].PW = "PW";
            mysqlds.store[0].storecode = Row.storecode;
            mysqlds.store[0].store_name = Row.store_name;
            mysqlds.store[0].f_address = Row.f_address;
            mysqlds.store[0].f_city = Row.f_city;
            mysqlds.store[0].f_state = Row.f_state;
            mysqlds.store[0].phone = Row.phone;
            mysqlds.store[0].d_name = Row.d_name;
            mysqlds.store[0].d_address = Row.d_address;
            GenerateAppTableRowSaveGeneral(mysqlds.store[0], "idcol", true);
        }

        public void FillCompanyRegionUsers(string compcode, string regioncode)
        {
           mysqlsearchds.user.Rows.Clear();
            ClearParameters();
            AddParms("@regioncode", regioncode, "MySQL");
            AddParms("@compcode", compcode, "MySQL");
            commandtext = "SELECT * FROM user WHERE compcode = @compcode AND regioncode = @regioncode";
            FillData(mysqlsearchds, "user", commandtext, CommandType.Text);
     
        }

        public void UpdateStore(DataRow Row)
        {
            ClearParameters();
            SetAllParameters(Row);
            commandtext = " UPDATE store SET `store_name` = @store_name, `f_address` = @f_address, `f_city` = @f_city, ";
            commandtext += " `f_state` = @f_state, `attention` = @attention, `pu_address` = @pu_address, `d_code` = @d_code, ";
            commandtext += " `d_name` = @d_name, `phone` = @phone ";
            commandtext += " WHERE storecode = @storecode";
            try
            {
                ExecuteCommand(commandtext, CommandType.Text);
            }

            catch (Exception e)
            {
                appUtilities.logEvent("Synch to MySQL failure", "01", e.Message, true);

            }
        }
        public void UpdateMySQLDatatable(DataTable Sourcetable, string Idname, bool Inserting)
        {
            for (int r = 0; r <= Sourcetable.Rows.Count - 1; r++)
            {
                GenerateAppTableRowSaveGeneral(Sourcetable.Rows[r], Idname, Inserting);
            }
        }

    }

}
