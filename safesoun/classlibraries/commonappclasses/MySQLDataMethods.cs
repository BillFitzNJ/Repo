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
        public ssprocess ssprocessds = new ssprocess();
        AppUtilities appUtilities = new AppUtilities();
        public MySQLDataMethods()
            : base("MySQL", "MySQLConnStringWithDB")
        {
            SetIdcol(mysqlds.store.idcolColumn);
            SetIdcol(mysqlds.user.idcolColumn);
            SetIdcol(mysqlds.storeregion.idcolColumn);
        }


        public void GetSingleValidator(int idcol)
        {
            mysqlds.validator.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", idcol);
            commandtext = "SELECT * FROM validator user WHERE idcol = @idcol ";

            FillData(mysqlds, "validator", commandtext, CommandType.Text);
        }

        public void GetSingleUser(int idcol)
        {
            mysqlds.user.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", idcol);
            commandtext = "SELECT * FROM user WHERE idcol = @idcol ";

            FillData(mysqlds, "user", commandtext, CommandType.Text);
        }

        public bool ValidatoridExists(string validatorid)
        {
            bool IdExists = false;
            mysqltestds.validator.Rows.Clear();
            ClearParameters();
            AddParms("@deviceid", validatorid);
            commandtext = "SELECT * FROM validator WHERE deviceid = @deviceid";
            FillData(mysqltestds, "validator", commandtext, CommandType.Text);
            if (mysqltestds.validator.Rows.Count > 0)
            {
                IdExists = true;
            }
            return IdExists;
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
        public void GetValidators()
        {
            mysqlselectords.validator.Rows.Clear();
            ClearParameters();
            commandtext = "SELECT * FROM validator ORDER BY deviceid";
            FillData(mysqlselectords, "validator", commandtext, CommandType.Text);
        }

        public void GetUsers()
        {
            mysqlselectords.user.Rows.Clear();
            ClearParameters();
            commandtext = "SELECT * FROM user ORDER BY userid";
            FillData(mysqlselectords, "user", commandtext, CommandType.Text);
        }

        public void DeleteRegion(string compcode, string regioncode)
        {
            string regionname = "";
            mysqlselectords.region.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            AddParms("@regioncode", regioncode);
            commandtext = "DELETE FROM region WHERE compcode = @compcode AND regioncode = @regioncode";
            ExecuteCommand(commandtext, CommandType.Text);
            commandtext = "DELETE FROM storeregion WHERE LEFT(storecode,4) = @compcode AND regioncode = @regioncode";
            ExecuteCommand(commandtext, CommandType.Text);
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


        public void FillMoneyCenter(int idcol)
        {
            mysqlselectords.moneycenter.Rows.Clear();
            ClearParameters();
            AddParms("@idcol", idcol);
            commandtext = "SELECT * FROM moneycenter WHERE idcol = @idcol";
            FillData(mysqlselectords, "moneycenter", commandtext, CommandType.Text);
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
            commandtext = "SELECT *, concat( f_address , ' - ', f_city) as location FROM storeregion WHERE LEFT(storecode,4) = @compcode AND regioncode = @regioncode ORDER BY storecode";
            FillData(mysqlselectords, "storeregion", commandtext, CommandType.Text);
        }

        public void GetassignedRegionStores(string compcode, string regioncode)
        {
            mysqlselectords.store.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            AddParms("@regioncode", regioncode);
            commandtext = "SELECT *,  concat( storecode, ' - ', f_address , ' - ', f_city) as location FROM store WHERE LEFT(storecode,4) = @compcode AND storecode IN (select storecode from storeregion WHERE regioncode = @regioncode) ORDER BY storecode";
            FillData(mysqlselectords, "expandedstore", commandtext, CommandType.Text);
        }
        public void GetUnassignedCompanyStores(string compcode)
        {
            mysqlselectords.store.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            commandtext = "SELECT * , concat( storecode, ' - ', f_address , ' - ', f_city) as location FROM store WHERE LEFT(storecode,4) = @compcode AND storecode NOT  IN (select storecode from storeregion ) ORDER BY storecode";
            FillData(mysqlselectords, "expandedstore", commandtext, CommandType.Text);
        }

        public void ClearRegionStores(string compcode, string regioncode)
        {
            string regionname = "";
            mysqlds.region.Rows.Clear();
            ClearParameters();
            AddParms("@compcode", compcode);
            AddParms("@regioncode", regioncode);
            commandtext = "DELETE FROM storeregion WHERE LEFT(storecode,4) = @compcode AND regioncode = @regioncode";
            ExecuteCommand(commandtext, CommandType.Text);
     
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
        public void InsertMoneyCenter(string centernamee, int idcol)
        {

            ClearParameters();
            AddParms("@centername", centernamee, "MySQL");
            AddParms("@idcol", idcol, "MySQL");
            AddParms("@lckstat", "", "MySQL");
            AddParms("@lckuser", AppUserClass.AppUserId, "MySQL");
            AddParms("@adduser", AppUserClass.AppUserId, "MySQL");
            AddParms("@lckdate",DateTime.Now.Date, "MySQL");
            AddParms("@adddate",DateTime.Now.Date, "MySQL");
            commandtext = "INSERT INTO moneycenter (`centername`, `idcol`, `lckstat`, `lckuser`, `adduser`,`lckdate`, `adddate`  ) VALUES (@centername, @idcol, @lckstat, @lckuser, @adduser, @lckdate, @adddate)";
            ExecuteCommand(commandtext, CommandType.Text);
        }

        public void UpdateMoneyCenter(string centernamee, int idcol)
        {

            ClearParameters();
            AddParms("@centername", centernamee, "MySQL");
            AddParms("@idcol", idcol, "MySQL");
            AddParms("@lckdate", DateTime.Now.Date, "MySQL");
            commandtext = "UPDATE moneycenter SET centername = @centername, lckdate = @lckdate WHERE idcol = @idcol)";
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


        public void DeleteCurrentAtmdrops(DateTime dropdate)
        {
            ClearParameters();
            AddParms("@dropdate", dropdate, "MySQL");
            commandtext = "DELETE FROM atmdrop WHERE dropdate <= @dropdate";
            ExecuteCommand(commandtext, CommandType.Text);
        }

        public void FillSmartsafetrans(DateTime  eventdate)
        {
            mysqlds.safetrans.Rows.Clear();
            ClearParameters();
            AddParms("@eventtime", eventdate.Date, "MySQL");
            commandtext = "SELECT * FROM safetrans WHERE date(safetrans.eventtime) =  @eventtime ORDER BY systemid, eventtime ";
            try
            {
                FillData(mysqlds, "safetrans", commandtext, CommandType.Text);
            }
            catch (Exception e)
            {
                appUtilities.logEvent("MySQL failure", "01", e.Message, true);

            }
        }

        public void FillCompanyRegionUsers(string compcode, string regioncode)
        {
           mysqlsearchds.user.Rows.Clear();
            ClearParameters();
            AddParms("@regioncode", regioncode, "MySQL");
            AddParms("@compcode", compcode, "MySQL");
            commandtext = "SELECT * FROM user WHERE compcode = @compcode AND regioncode = @regioncode";
     
        }

        public void UpdateAtmdrop()
        {
            mysqlsearchds.atmdrop.Rows.Clear();
            ClearParameters();
            AddParms("@atmid", mysqlds.atmdrop[0].atmid, "MySQL");
            AddParms("@dropdate",mysqlds.atmdrop[0].dropdate, "MySQL");
            AddParms("@trantype", mysqlds.atmdrop[0].trantype, "MySQL");
            commandtext = "SELECT * FROM atmdrop WHERE atmid = @atmid AND dropdate = @dropdate AND trantype = @trantype";
            try
            {
                FillData(mysqlsearchds, "atmdrop", commandtext, CommandType.Text);
                if (mysqlsearchds.atmdrop.Rows.Count < 1)
                {
                    GenerateAppTableRowSaveGeneral(mysqlds.atmdrop[0], "None", true);
                }
                else
                {
        //            MessageBox.Show(mysqlds.atmdrop[0].atmid);
                }
            }

            catch (Exception e)
            {
                appUtilities.logEvent("Synch to MySQL failure", "01", e.Message, true);

            }
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

        public void MergeMySQLDataRow(DataTable target, string Idname)
        {
            bool Inserting = true;          
            ClearParameters();
            string idvalue =  target.Rows[0][Idname].ToString();
            AddParms("@idname",idvalue, "MySQL");
            

            string commandtext = "SELECT * FROM " + target.TableName + " WHERE " + Idname + " = @idname";
            FillData(mysqlsearchds, target.TableName, commandtext, CommandType.Text);
            if (mysqlsearchds.Tables[target.TableName].Rows.Count > 0)
            {
                Inserting = false;  
            }
            GenerateAppTableRowMerge(target.Rows[0], Idname, Inserting);
            
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
