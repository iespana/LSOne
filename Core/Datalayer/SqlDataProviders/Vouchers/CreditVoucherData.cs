using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Vouchers;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Vouchers
{
    public class CreditVoucherData : SqlServerDataProviderBase, ICreditVoucherData
    {
        private static string ResolveSort(CreditVoucher.SortEnum sort, bool ascending)
        {
            string order = ascending ? "ASC" : "DESC";

            switch (sort)
            {
                case CreditVoucher.SortEnum.ID:
                    return $"Len(VOUCHERID) {order}, VOUCHERID {order}";
                case CreditVoucher.SortEnum.Balance:
                    return $"BALANCE {order}";
                case CreditVoucher.SortEnum.Currency:
                    return $"CURRENCY {order}";
                case CreditVoucher.SortEnum.CreatedDate:
                    return $"CREATEDDATE {order}";
                case CreditVoucher.SortEnum.LastUsedDate:
                    return $"LASTUSEDDATE {order}";
            }

            return "";
        }

        private static void PopulateCreditVoucher(IDataReader dr, CreditVoucher voucher)
        {
            voucher.ID = (string)dr["VOUCHERID"];
            voucher.Balance = (decimal)dr["BALANCE"];
            voucher.Currency = (string)dr["CURRENCY"];
            voucher.CreatedDate = dr["CREATEDDATE"] == DBNull.Value ? new Date() : new Date((DateTime)dr["CREATEDDATE"]);
            voucher.LastUsedDate = dr["LASTUSEDDATE"] == DBNull.Value ? new Date() : new Date((DateTime)dr["LASTUSEDDATE"]);
        }

        /// <summary>
        /// Checks if a credit voucher with a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID to check for</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOCREDITVOUCHERTABLE", "VOUCHERID", id);
        }

        /// <summary>
        /// Gets a credit voucher by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="voucherID">ID of the credit voucher to fetch.</param>
        /// <returns>The requested credit voucher or null if it was not found</returns>
        public virtual CreditVoucher Get(IConnectionManager entry, RecordIdentifier voucherID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT g.VOUCHERID,ISNULL(g.BALANCE,0.0) as BALANCE, g.CURRENCY, g.CREATEDDATE, g.LASTUSEDDATE " +
                    "from RBOCREDITVOUCHERTABLE g " +
                    "where g.VOUCHERID = @voucherID AND g.DATAAREAID = @dataareaid";

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "voucherID", (string)voucherID);

                var result = Execute<CreditVoucher>(entry, cmd, CommandType.Text, PopulateCreditVoucher);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Searches for credit vouchers
        /// </summary>
        /// <param name="entry">Database entry</param>
        /// <param name="itemCount">Out parameter. Returns the total number of returned items</param>
        /// <param name="filter">Search filter</param>
        /// <returns>A list of credit vouchers</returns>
        public virtual List<CreditVoucher> Search(IConnectionManager entry, CreditVoucherFilter filter, out int itemCount)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                string extraConditions = "";
                if (!string.IsNullOrEmpty(filter.VoucherID))
                {
                    extraConditions += " and VOUCHERID Like @idSearchstring";

                    MakeParam(cmd, "idSearchstring", PreProcessSearchText(filter.VoucherID, true , filter.VoucherIDBeginsWith) + "%");
                }

                if(filter.Status != null)
                {
                    switch(filter.Status)
                    {
                        case CreditVoucherStatusEnum.NotEmpty:
                            extraConditions += " and BALANCE > 0";
                            break;
                        case CreditVoucherStatusEnum.Empty:
                            extraConditions += " and BALANCE = 0";
                            break;
                        case CreditVoucherStatusEnum.All:
                            //Nothing
                            break;
                    }
                }

                if(!RecordIdentifier.IsEmptyOrNull(filter.CurrencyID))
                {
                    extraConditions += " and CURRENCY = @currencyId";
                    MakeParam(cmd, "currencyId", filter.CurrencyID);
                }

                if (!filter.FromCreatedDate.IsEmpty)
                {
                    extraConditions += " and g.CREATEDDATE >= @fromCreatedDate";
                    MakeParam(cmd, "fromCreatedDate", filter.FromCreatedDate.DateTime, SqlDbType.DateTime);
                }

                if (!filter.ToCreatedDate.IsEmpty)
                {
                    extraConditions += " and g.CREATEDDATE <= @toCreatedDate";
                    MakeParam(cmd, "toCreatedDate", filter.ToCreatedDate.DateTime, SqlDbType.DateTime);
                }

                if (!filter.FromLastUsedDate.IsEmpty)
                {
                    extraConditions += " and g.LASTUSEDDATE >= @fromLastUsedDate";
                    MakeParam(cmd, "fromLastUsedDate", filter.FromLastUsedDate.DateTime, SqlDbType.DateTime);
                }

                if (!filter.ToLastUsedDate.IsEmpty)
                {
                    extraConditions += " and g.LASTUSEDDATE <= @toLastUsedDate";
                    MakeParam(cmd, "toLastUsedDate", filter.ToLastUsedDate.DateTime, SqlDbType.DateTime);
                }

                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);

                cmd.CommandText =
                    "select s.* from(" +
                    "SELECT g.VOUCHERID,ISNULL(g.BALANCE,0.0) as BALANCE, g.CURRENCY, g.CREATEDDATE, g.LASTUSEDDATE, ROW_NUMBER() OVER(order by " + ResolveSort(filter.Sort, filter.SortAscending) + ") AS ROW " +
                    "from RBOCREDITVOUCHERTABLE g " +
                    "where g.DATAAREAID = @dataareaid" + extraConditions + ") s " +
                    "where s.ROW between " + filter.RowFrom + " and " + filter.RowTo;

                List<CreditVoucher> results = Execute<CreditVoucher>(entry, cmd, CommandType.Text, PopulateCreditVoucher);
                itemCount = results.Count;
                return results;
            }
        }

        /// <summary>
        /// Adds a given amount to a credit voucher with a given id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="creditVoucherID">ID of the credit voucher to add to</param>
        /// <param name="amount">The amount to add to the credit voucher</param>
        /// <param name="storeID">ID of the store where the operation is done or empty string if head office</param>
        /// <param name="terminalID">ID of the terminal where the operation is done or empty string if on store or headoffice level</param>
        /// <param name="userID">ID of the Site Manager user, or Guid.Empty if not avalible</param>
        /// <param name="staffID">ID of the POS staff or empty string if not avalible</param>
        /// <returns>Balance on the credit voucher after the transaction</returns>
        public virtual decimal AddToCreditVoucher(IConnectionManager entry, RecordIdentifier creditVoucherID, decimal amount, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier userID, RecordIdentifier staffID)
        {
            var voucher = Get(entry, creditVoucherID);
            var line = new CreditVoucherLine();

            ValidateSecurity(entry, Permission.ManageCreditVouchers);

            if (voucher == null)
            {
                throw new DataIntegrityException(typeof(CreditVoucher), creditVoucherID);
            }

            voucher.Balance += amount;

            Save(entry, voucher);

            line.Amount = amount;
            line.CreditVoucherID = voucher.ID;
            line.Operation = CreditVoucherLine.CreditVoucherLineEnum.AddCreditVoucher;
            line.StoreID = storeID;
            line.TerminalID = terminalID;
            line.UserID = userID;
            line.StaffID = staffID;

            Providers.CreditVoucherLineData.Save(entry, line);

            return voucher.Balance;
        }

        /// <summary>
        /// Saves a credit voucher, creating new one if the ID was empty or record did not exist, else updates.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="voucher">The credit voucher to save</param>
        public virtual void Save(IConnectionManager entry, CreditVoucher voucher)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageCreditVouchers);

            voucher.Validate();

            var statement = new SqlServerStatement("RBOCREDITVOUCHERTABLE");

            bool isNew = false;
            if (voucher.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                voucher.ID = DataProviderFactory.Instance.GenerateNumber<ICreditVoucherData, CreditVoucher>(entry);
            }

            if (isNew || !Exists(entry, voucher.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("VOUCHERID", (string) voucher.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("VOUCHERID", (string) voucher.ID);
            }

            statement.AddField("BALANCE", voucher.Balance, SqlDbType.Decimal);
            statement.AddField("CURRENCY", (string) voucher.Currency);

            if(voucher.CreatedDate.IsEmpty)
            {
                statement.AddField("CREATEDDATE", DBNull.Value, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("CREATEDDATE", voucher.CreatedDate.DateTime, SqlDbType.DateTime);
            }

            if (voucher.LastUsedDate.IsEmpty)
            {
                statement.AddField("LASTUSEDDATE", DBNull.Value, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("LASTUSEDDATE", voucher.LastUsedDate.DateTime, SqlDbType.DateTime);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Delete a credit voucher by given ID and credit voucher lines assoiated with it 
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="id">ID of the credit voucher</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOCREDITVOUCHERTABLE", "VOUCHERID", id, BusinessObjects.Permission.ManageCreditVouchers);
            DeleteRecord(entry, "RBOCREDITVOUCHERTRANSACTIONS", "VOUCHERID", id, BusinessObjects.Permission.ManageCreditVouchers);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "CREDITVOUCHER"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOCREDITVOUCHERTABLE", "VOUCHERID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
