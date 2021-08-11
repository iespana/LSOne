using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Financials;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Financial
{
    public class IncomeExpenseAccountData : SqlServerDataProviderBase, IIncomeExpenseAccountData
    {
        public virtual List<DataEntity> GetCopyList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOINCOMEEXPENSEACCOUNTTABLE", "NAME", "ACCOUNTNUM", "ACCOUNTNUM");
        }

        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "ACCOUNTNUM, " +
                    "ISNULL(STOREID, '') as STOREID, " +
                    "ISNULL(NAME,'') as NAME, " +
                    "ISNULL(NAMEALIAS,'') as NAMEALIAS, " +
                    "ISNULL(ACCOUNTTYPE,0) as ACCOUNTTYPE, " +
                    "ISNULL(LEDGERACCOUNT,'') as LEDGERACCOUNT, " +
                    "ISNULL(MESSAGELINE1,'') as MESSAGELINE1, " +
                    "ISNULL(MESSAGELINE2,'') as MESSAGELINE2, " +
                    "ISNULL(SLIPTEXT1,'') as SLIPTEXT1, " +
                    "ISNULL(SLIPTEXT2,'') as SLIPTEXT2, " +
                    "ISNULL(TAXCODEID,'') as TAXCODEID, " +
                    "MODIFIEDDATE, " +
                    "ISNULL(MODIFIEDTIME,0) as MODIFIEDTIME, " +
                    "ISNULL(MODIFIEDBY,'') as MODIFIEDBY, " +
                    "ISNULL(MODIFIEDTRANSACTIONID,'') as MODIFIEDTRANSACTIONID " +
                    "from RBOINCOMEEXPENSEACCOUNTTABLE ";
            }
        }

        private static void PopulateIncomeExpenseAccount(IDataReader dr, IncomeExpenseAccount incomeExpenseAccount)
        {
            incomeExpenseAccount.AccountNum = (string)dr["ACCOUNTNUM"];
            incomeExpenseAccount.Name = (string)dr["NAME"];
            incomeExpenseAccount.StoreID = (string)dr["STOREID"];
            incomeExpenseAccount.NameAlias = (string)dr["NAMEALIAS"];
            incomeExpenseAccount.AccountType = (IncomeExpenseAccount.AccountTypeEnum)((int)dr["ACCOUNTTYPE"]);
            incomeExpenseAccount.LedgerAccount = (string)dr["LEDGERACCOUNT"];
            incomeExpenseAccount.MessageLine1 = (string)dr["MESSAGELINE1"];
            incomeExpenseAccount.MessageLine2 = (string)dr["MESSAGELINE2"];
            incomeExpenseAccount.SlipText1 = (string)dr["SLIPTEXT1"];
            incomeExpenseAccount.SlipText2 = (string)dr["SLIPTEXT2"];
            incomeExpenseAccount.TaxCodeID = (string)dr["TAXCODEID"];
            incomeExpenseAccount.ModifiedDate = dr.IsDBNull(dr.GetOrdinal("MODIFIEDDATE")) ? new DateTime() : dr.GetDateTime(dr.GetOrdinal("MODIFIEDDATE"));
            incomeExpenseAccount.ModifiedTime = (int)dr["MODIFIEDTIME"];
            incomeExpenseAccount.ModiefiedBy = (string)dr["MODIFIEDBY"];
            incomeExpenseAccount.ModifiedTransactionID = (int)dr["MODIFIEDTRANSACTIONID"];
        }

        /// <summary>
        /// Gets an IncomeExpenseAccount object with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="incomeExpenseAccountID">The ID of the incomeExpenseAccount to get</param>
        /// <returns>The income-expense account with the given ID</returns>
        public virtual IncomeExpenseAccount Get(IConnectionManager entry, RecordIdentifier incomeExpenseAccountID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                if (incomeExpenseAccountID.SecondaryID == RecordIdentifier.Empty ||
                    incomeExpenseAccountID.SecondaryID == null)
                {
                    cmd.CommandText =
                        BaseSelectString +
                        "where ACCOUNTNUM = @accountNum and DATAAREAID = @dataAreaId";
                    MakeParam(cmd, "accountNum", (string) incomeExpenseAccountID.PrimaryID);
                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                }
                else
                {
                    cmd.CommandText =
                        BaseSelectString +
                        "where ACCOUNTNUM = @accountNum and STOREID = @storeId and DATAAREAID = @dataAreaId";

                    MakeParam(cmd, "accountNum", (string) incomeExpenseAccountID.PrimaryID);
                    MakeParam(cmd, "storeId", (string) incomeExpenseAccountID.SecondaryID);
                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                }

                var result = Execute<IncomeExpenseAccount>(entry, cmd, CommandType.Text,
                    PopulateIncomeExpenseAccount);

                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual IncomeExpenseAccount GetLists(IConnectionManager entry, RecordIdentifier id)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSelectString +
                    "where ACCOUNTNUM = @accountNum and  DATAAREAID = @dataAreaId";

                MakeParam(cmd, "accountNum", (string) id.PrimaryID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Execute<IncomeExpenseAccount>(entry, cmd, CommandType.Text,
                    PopulateIncomeExpenseAccount);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets all income-expense accounts
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>All income-expense accounts</returns>
        public virtual List<IncomeExpenseAccount> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<IncomeExpenseAccount>(entry, cmd, CommandType.Text, PopulateIncomeExpenseAccount);
            }
        }

        /// <summary>
        /// Gets all income-expense accounts of the given type
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="accountType">The type of account to get</param>
        /// <returns>All income-expense accounts</returns>
        public virtual List<IncomeExpenseAccount> GetList(IConnectionManager entry, IncomeExpenseAccount.AccountTypeEnum accountType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                if (accountType == IncomeExpenseAccount.AccountTypeEnum.All)
                {
                    cmd.CommandText =
                        BaseSelectString +
                        "where DATAAREAID = @dataAreaId";
                }
                else
                {
                    cmd.CommandText =
                        BaseSelectString +
                        "where DATAAREAID = @dataAreaId and ACCOUNTTYPE = @accountType";
                }

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "accountType", (int) accountType, SqlDbType.Int);

                return Execute<IncomeExpenseAccount>(entry, cmd, CommandType.Text, PopulateIncomeExpenseAccount);
            }
        }

        public virtual List<IncomeExpenseAccount> GetListForStore(IConnectionManager entry, IncomeExpenseAccount.AccountTypeEnum accountType, RecordIdentifier storeid)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {               
                cmd.CommandText = BaseSelectString + "where DATAAREAID = @dataAreaId and  STOREID = @storeId ";
                if (accountType != IncomeExpenseAccount.AccountTypeEnum.All)
                {
                    cmd.CommandText = cmd.CommandText + " and ACCOUNTTYPE = @accountType  ";
                    MakeParam(cmd, "accountType", (int) accountType, SqlDbType.Int);
                }
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeId", storeid);

                return Execute<IncomeExpenseAccount>(entry, cmd, CommandType.Text, PopulateIncomeExpenseAccount);
            }
        }

        [Obsolete("Use the overridden Save method", true)]
        public virtual void Save(IConnectionManager entry, IncomeExpenseAccount incomeExpenseAccount)
        {
            throw new NotImplementedException();
        }

        public virtual void Save(IConnectionManager entry, IncomeExpenseAccount incomeExpenseAccount, bool isNew)
        {
            ValidateSecurity(entry);

            incomeExpenseAccount.Validate();

            var t = new TimeSpan(DateTime.Now.Hour,DateTime.Now.Minute, DateTime.Now.Second);
            var secondsNow = (int)t.TotalSeconds;           

            var statement = new SqlServerStatement("RBOINCOMEEXPENSEACCOUNTTABLE");

            if (incomeExpenseAccount.AccountNum == "" || incomeExpenseAccount.AccountNum == RecordIdentifier.Empty || isNew)
            {
                incomeExpenseAccount.AccountNum = DataProviderFactory.Instance.GenerateNumber<IIncomeExpenseAccountData, IncomeExpenseAccount>(entry);
            }

            if (!Exists(entry, incomeExpenseAccount.AccountNum))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ACCOUNTNUM", (string)incomeExpenseAccount.AccountNum);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ACCOUNTNUM", (string)incomeExpenseAccount.AccountNum);
            }

            statement.AddField("STOREID", (string)incomeExpenseAccount.StoreID);
            statement.AddField("NAME" , incomeExpenseAccount.Name);
            statement.AddField("NAMEALIAS", incomeExpenseAccount.NameAlias);
            statement.AddField("ACCOUNTTYPE",(int)incomeExpenseAccount.AccountType, SqlDbType.Int);
            statement.AddField("LEDGERACCOUNT",(string)incomeExpenseAccount.LedgerAccount);
            statement.AddField("MODIFIEDDATE", DateTime.Now.Date , SqlDbType.Date);
            statement.AddField("ModifiedTime", secondsNow, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier incomeExpenseID)
        {
            return RecordExists(
                entry,
                "RBOINCOMEEXPENSEACCOUNTTABLE",
                "accountNum",
                incomeExpenseID);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier incomeExpenseAccountID)
        {
            DeleteRecord(
                entry,
                "RBOINCOMEEXPENSEACCOUNTTABLE",
                "ACCOUNTNUM",
                incomeExpenseAccountID,
                "");
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "INCOMEEXPENSEACCOUNT"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOINCOMEEXPENSEACCOUNTTABLE", "ACCOUNTNUM", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

    }
}
