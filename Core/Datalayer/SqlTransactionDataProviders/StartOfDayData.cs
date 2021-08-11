using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using LSOne.Utilities.IO.JSON;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class StartOfDayData : SqlServerDataProviderBase, IStartOfDayData
    {
        public virtual bool FloatRequired(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT TOP 1 [TYPE] " +
                                  "FROM [RBOTRANSACTIONTABLE] " +
                                  "WHERE [TYPE] NOT IN (0,1,9,19) " +
                                  "AND STORE = @storeID " +
                                  "AND TERMINAL = @terminalID " +
                                  "AND DATAAREAID = @dataAreaID " +
                                  "ORDER BY TRANSACTIONID DESC";
                MakeParam(cmd, "storeID", entry.CurrentStoreID);
                MakeParam(cmd, "terminalID", entry.CurrentTerminalID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                object result = entry.Connection.ExecuteScalar(cmd);

                return !(result is int) || (int)result == 12;
            }
        }

        public virtual decimal GetFloatsFromLastTenderDeclaration(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier tenderTypeID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT SUM(T.AMOUNT) " +
                                  "FROM (SELECT sum(pay.AMOUNTTENDERED) AS AMOUNT " +
                                  "FROM RBOTRANSACTIONTABLE a " +
                                  "join RBOTRANSACTIONPAYMENTTRANS pay on pay.TRANSACTIONID = a.TRANSACTIONID and pay.DATAAREAID = a.DATAAREAID AND pay.TENDERTYPE = @tenderTypeID and pay.STORE = a.STORE and pay.TERMINAL = a.TERMINAL " +
                                  "left outer join (Select top 1 transactionid, TRANSDATE from RBOTRANSACTIONTABLE where [type] = 12 and DATAAREAID = @dataAreaID and STORE = @storeID and TERMINAL = @terminalID order by TRANSDATE desc) b on 1 = 1 " +
                                  "where a.TYPE = 5 and (a.TRANSDATE > b.TRANSDATE or b.transactionid is null) and a.DATAAREAID = @dataAreaID and a.STORE = @storeID and a.TERMINAL = @terminalID " +
                                  "UNION ALL " +
                                  "SELECT J.AMOUNT " +
                                  "FROM (SELECT sum(pay.AMOUNTTENDERED) AS AMOUNT " +
                                  "FROM RBOTRANSACTIONTABLE a " +
                                  "join RBOTRANSACTIONPAYMENTTRANS pay on pay.TRANSACTIONID = a.TRANSACTIONID and pay.DATAAREAID = a.DATAAREAID AND pay.TENDERTYPE = @tenderTypeID and pay.STORE = a.STORE and pay.TERMINAL = a.TERMINAL " +
                                  "left outer join (Select top 1 transactionid, TRANSDATE from RBOTRANSACTIONTABLE where [type] = 12 and DATAAREAID = @dataAreaID and STORE = @storeID and TERMINAL = @terminalID order by TRANSDATE desc) b on 1 = 1 " +
                                  "where a.TYPE = 2 and (a.TRANSDATE > b.TRANSDATE or b.transactionid is null) and a.DATAAREAID = @dataAreaID and a.STORE = @storeID and a.TERMINAL = @terminalID) AS J " +
                                  "WHERE J.AMOUNT < 0) " +
                                  "AS T";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "tenderTypeID", tenderTypeID.DBValue, tenderTypeID.DBType);
                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "terminalID", terminalID);
                object result = entry.Connection.ExecuteScalar(cmd);
                return result is decimal ? (decimal) result : default(decimal);
            }
        }

        public virtual decimal GetStartOfDayAmount(IConnectionManager entry, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier tenderTypeID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT SUM(T.AMOUNT) " +
                                  "FROM (SELECT sum(pay.AMOUNTTENDERED) AS AMOUNT " +
                                  "FROM RBOTRANSACTIONTABLE a " +
                                  "join RBOTRANSACTIONPAYMENTTRANS pay on pay.TRANSACTIONID = a.TRANSACTIONID and pay.DATAAREAID = a.DATAAREAID AND pay.STORE = a.STORE AND pay.TERMINAL = a.TERMINAL AND pay.TENDERTYPE = @tenderTypeID " +
                                  "left outer join (Select top 1 transactionid, TRANSDATE from RBOTRANSACTIONTABLE where [type] = 12 and DATAAREAID = @dataAreaID and STORE = @storeID and TERMINAL = @terminalID order by TRANSDATE desc) b on 1 = 1 " +
                                  "left outer join (select * from (Select ROW_NUMBER() over (order by TRANSDATE desc) as rownum, transactionid, TRANSDATE from RBOTRANSACTIONTABLE where [type] = 12 and DATAAREAID = @dataAreaID and STORE = @storeID and TERMINAL = @terminalID) as g where rownum = 2) as c on 1 = 1 " +
                                  "where a.TYPE = 5 and ((a.TRANSDATE < b.TRANSDATE or b.TRANSACTIONID is null) and (a.TRANSDATE > c.TRANSDATE or c.TRANSACTIONID is null)) and a.DATAAREAID = @dataAreaID and a.STORE = @storeID and a.TERMINAL = @terminalID " +
                                  "UNION ALL " +
                                  "SELECT J.AMOUNT " +
                                  "FROM (SELECT sum(pay.AMOUNTTENDERED) AS AMOUNT " +
                                  "FROM RBOTRANSACTIONTABLE a " +
                                  "join RBOTRANSACTIONPAYMENTTRANS pay on pay.TRANSACTIONID = a.TRANSACTIONID and pay.DATAAREAID = a.DATAAREAID AND pay.STORE = a.STORE AND pay.TERMINAL = a.TERMINAL AND pay.TENDERTYPE = @tenderTypeID " +
                                  "left outer join (Select top 1 transactionid, TRANSDATE from RBOTRANSACTIONTABLE where [type] = 12 and DATAAREAID = @dataAreaID and STORE = @storeID and TERMINAL = @terminalID order by TRANSDATE desc) b on 1 = 1 " +
                                  "left outer join (select * from (Select ROW_NUMBER() over (order by TRANSDATE desc) as rownum, transactionid, TRANSDATE from RBOTRANSACTIONTABLE where [type] = 12 and STORE = @storeID and TERMINAL = @terminalID and DATAAREAID = @dataAreaID) as g where rownum = 2) as c on 1 = 1 " +
                                  "where a.TYPE = 2 and ((a.TRANSDATE < b.TRANSDATE or b.TRANSACTIONID is null) and (a.TRANSDATE > c.TRANSDATE or c.TRANSACTIONID is null)) and a.DATAAREAID = @dataAreaID and a.STORE = @storeID and a.TERMINAL = @terminalID) AS J " +
                                  "WHERE J.AMOUNT < 0) " +
                                  "AS T";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "tenderTypeID", tenderTypeID.DBValue, tenderTypeID.DBType);
                MakeParam(cmd, "storeID", storeID);
                MakeParam(cmd, "terminalID", terminalID);
                object result = entry.Connection.ExecuteScalar(cmd);
                return result is decimal ? (decimal)result : default(decimal);
            }
        }

        public virtual DateTime? GetBusinessDay()
        {
            FolderItem file = GetFile();
            if (file.Exists)
            {
                try
                {
                    string data = File.ReadAllText(file.AbsolutePath);
                    return JsonConvert.DeserializeObject<DateTime>(data);
                }
                catch
                {
                    file.Delete();
                    return null;
                }
            }
            return null;
        }

        public virtual DateTime? GetBusinessSystemDay()
        {
            FolderItem file = GetBusinessSystemDayFile();
            if (file.Exists)
            {
                try
                {

                
                string data = File.ReadAllText(file.AbsolutePath);
                return JsonConvert.DeserializeObject<DateTime>(data);
                }
                catch
                {
                    file.Delete();
                    return null;
                }
            }
            return null;
        }

        public virtual void SaveBusinessDay(DateTime? day)
        {
            FolderItem file = GetFile();
            if (day == null || day.Value == default(DateTime))
            {
                file.Delete();
            }
            else
            {
                string data = JsonConvert.SerializeObject(day.Value);
                File.WriteAllText(file.AbsolutePath,data);
            }
        }

        public virtual void SaveBusinessSystemDay(DateTime? day)
        {
            FolderItem file = GetBusinessSystemDayFile();
            if (day == null || day.Value == default(DateTime))
            {
                file.Delete();
            }
            else
            {
                string data = JsonConvert.SerializeObject(day.Value);
                File.WriteAllText(file.AbsolutePath, data);
            }
        }

        private static FolderItem GetFile()
        {
            FolderItem appData = FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData);
            appData = appData.CreateChildDirectory("LS Retail");
            appData = appData.CreateChildDirectory("LS POS");
            
            #pragma warning disable 618
            string fileName = "BusinessDay";
            #pragma warning restore 618
            return appData.Child(fileName);
        }

        private static FolderItem GetBusinessSystemDayFile()
        {
            FolderItem appData = FolderItem.GetSpecialFolder(Environment.SpecialFolder.CommonApplicationData);
            appData = appData.CreateChildDirectory("LS Retail");
            appData = appData.CreateChildDirectory("LS POS");

            #pragma warning disable 618
            string fileName = "BusinessSystemDay";
            #pragma warning restore 618
            return appData.Child(fileName);
        }

        public virtual void Save(IConnectionManager entry, RecordIdentifier item)
        {
            throw new NotSupportedException();
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotSupportedException();
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotSupportedException();
        }
    }
}
