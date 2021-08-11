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
    public class GiftCardData : SqlServerDataProviderBase,  IGiftCardData
    {
        private static string ResolveSort(GiftCard.SortEnum sort, bool ascending)
        {
            string order = ascending ? "ASC" : "DESC";

            switch (sort)
            {
                case GiftCard.SortEnum.ID:
                    return $"Len(GIFTCARDID) {order}, GIFTCARDID {order}";
                case GiftCard.SortEnum.Balance:
                    return $"BALANCE {order}";
                case GiftCard.SortEnum.Currency:
                    return $"CURRENCY {order}";
                case GiftCard.SortEnum.Active:
                    return $"ACTIVE {order}";
                case GiftCard.SortEnum.Refillable:
                    return $"REFILLABLE {order}";
                case GiftCard.SortEnum.CreatedDate:
                    return $"DATECREATED {order}";
                case GiftCard.SortEnum.LastUsedDate:
                    return $"LASTUSEDDATE {order}";
            }

            return "";
        }

        private static void PopulateGiftCard(IDataReader dr, GiftCard giftCard)
        {
            giftCard.ID = (string)dr["GIFTCARDID"];
            giftCard.Balance = (decimal)dr["BALANCE"];
            giftCard.Currency = (string)dr["CURRENCY"];
            giftCard.Active = ((byte)dr["ACTIVE"] != 0);
            giftCard.Issued = ((byte)dr["ISSUED"] != 0);
            giftCard.Refillable = ((byte)dr["REFILLABLE"] != 0);
            giftCard.CreatedDate = dr["DATECREATED"] == DBNull.Value ? new Date() : new Date((DateTime)dr["DATECREATED"]);
            giftCard.LastUsedDate = dr["LASTUSEDDATE"] == DBNull.Value ? new Date() : new Date((DateTime)dr["LASTUSEDDATE"]);
        }

        /// <summary>
        /// Checks if a gift card with a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID to check for</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOGIFTCARDTABLE", "GIFTCARDID", id);
        }

        /// <summary>
        /// Gets a gift card by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the gift card to fetch.</param>
        /// <returns>The requested gift card or null if it was not found</returns>
        public virtual GiftCard Get(IConnectionManager entry, RecordIdentifier giftCardID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT g.GIFTCARDID,ISNULL(g.BALANCE,0.0) as BALANCE, g.CURRENCY,g.ACTIVE,g.ISSUED, g.REFILLABLE, g.DATECREATED, g.LASTUSEDDATE " +
                    "from RBOGIFTCARDTABLE g " +
                    "where g.GIFTCARDID = @giftCardID AND g.DATAAREAID = @dataareaid";
    
                MakeParam(cmd, "dataareaid", entry.Connection.DataAreaId);
                MakeParam(cmd, "giftCardID", (string)giftCardID);

                var result = Execute<GiftCard>(entry, cmd, CommandType.Text, PopulateGiftCard);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Searches for gift cards
        /// </summary>
        /// <param name="entry">Entry info</param>
        /// <param name="itemCount">Number of items found</param>
        /// <param name="filter">Search filter</param>
        /// <returns>List of found gift cards</returns>
        public List<GiftCard> AdvancedSearch(IConnectionManager entry, GiftCardFilter filter, out int itemCount)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                string extraConditions = "";
                if (!string.IsNullOrEmpty(filter.VoucherID))
                {
                    extraConditions += " and GIFTCARDID Like @idSearchstring";

                    MakeParam(cmd, "idSearchstring", PreProcessSearchText(filter.VoucherID, true, filter.VoucherIDBeginsWith) + "%");
                }

                if (filter.Status != null)
                {
                    switch (filter.Status)
                    {
                        case GiftCardStatusEnum.Active:
                            extraConditions += " and ACTIVE = 1";
                            break;
                        case GiftCardStatusEnum.Inactive:
                            extraConditions += " and ACTIVE = 0";
                            break;
                        case GiftCardStatusEnum.Empty:
                            extraConditions += " and BALANCE = 0";
                            break;
                        case GiftCardStatusEnum.NotEmpty:
                            extraConditions += " and BALANCE > 0";
                            break;
                        case GiftCardStatusEnum.All:
                            //Nothing
                            break;
                    }
                }

                if (!RecordIdentifier.IsEmptyOrNull(filter.CurrencyID))
                {
                    extraConditions += " and CURRENCY = @currencyId";
                    MakeParam(cmd, "currencyId", filter.CurrencyID);
                }

                if(filter.Refillable.HasValue)
                {
                    if (filter.Refillable.Value)
                    {
                        extraConditions += " and REFILLABLE = 1";
                    }
                    else
                    {
                        extraConditions += " and REFILLABLE = 0";
                    }
                }

                if(filter.FromBalance.HasValue && filter.ToBalance.HasValue)
                {
                    if (filter.FromBalance == 0 && filter.ToBalance == 0)
                    {
                        extraConditions += " and BALANCE = 0 ";
                    }
                    else
                    {
                        if (filter.FromBalance != 0)
                        {
                            extraConditions += " and BALANCE >= " + filter.FromBalance;
                        }

                        if (filter.ToBalance != 0)
                        {
                            extraConditions += " and BALANCE <= " + filter.ToBalance;
                        }
                    }
                }

                if(!filter.FromCreatedDate.IsEmpty)
                {
                    extraConditions += " and g.DATECREATED >= @fromCreatedDate";
                    MakeParam(cmd, "fromCreatedDate", filter.FromCreatedDate.DateTime, SqlDbType.DateTime);
                }

                if (!filter.ToCreatedDate.IsEmpty)
                {
                    extraConditions += " and g.DATECREATED <= @toCreatedDate";
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
                    "SELECT g.GIFTCARDID,ISNULL(g.BALANCE,0.0) as BALANCE, g.CURRENCY,g.ACTIVE, g.ISSUED,g.REFILLABLE, g.DATECREATED, g.LASTUSEDDATE, ROW_NUMBER() OVER(order by " + ResolveSort(filter.Sort, filter.SortAscending) + ") AS ROW " +
                    "from RBOGIFTCARDTABLE g " +
                    "where g.DATAAREAID = @dataareaid" + extraConditions + ") s " +
                    "where s.ROW between " + filter.RowFrom + " and " + filter.RowTo;

                List<GiftCard> results = Execute<GiftCard>(entry, cmd, CommandType.Text, PopulateGiftCard);
                itemCount = results.Count;
                return results;
            }
        }

        /// <summary>
        /// Adds a given amount to a gift card with a given id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the gift card to add to</param>
        /// <param name="amount">The amount to add to the gift card</param>
        /// <param name="storeID">ID of the store where the operation is done or empty string if head office</param>
        /// <param name="terminalID">ID of the terminal where the operation is done or empty string if on store or headoffice level</param>
        /// <param name="userID">ID of the Site Manager user, or Guid.Empty if not avalible</param>
        /// <param name="staffID">ID of the POS staff or empty string if not avalible</param>
        /// <returns>Balance on the gift card after the transaction</returns>
        public virtual decimal AddToGiftCard(IConnectionManager entry, RecordIdentifier giftCardID, decimal amount, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier userID, RecordIdentifier staffID)
        {
            var card = Get(entry, giftCardID);
            var line = new GiftCardLine();

            ValidateSecurity(entry, BusinessObjects.Permission.ManageGiftCards);

            if (card == null)
            {
                throw new DataIntegrityException(typeof(GiftCard), giftCardID);
            }

            card.Balance += amount;

            Save(entry, card);

            line.Amount = amount;
            line.GiftCardID = card.ID;
            line.Operation = GiftCardLine.GiftCardLineEnum.AddToGiftCard;
            line.StoreID = storeID;
            line.TerminalID = terminalID;
            line.UserID = userID;
            line.StaffID = staffID;
            if (giftCardID.HasSecondaryID)
            {
                line.TransactionID = giftCardID.SecondaryID;
            }

            Providers.GiftCardLineData.Save(entry, line);

            return card.Balance;
        }

        /// <summary>
        /// Activates a gift card
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the giftcard</param>
        /// <param name="storeID">ID of the store where the operation is done or empty string if head office</param>
        /// <param name="terminalID">ID of the terminal where the operation is done or empty string if on store or headoffice level</param>
        /// <param name="userID">ID of the Site Manager user, or Guid.Empty if not avalible</param>
        /// <param name="staffID">ID of the POS staff or empty string if not avalible</param>
        /// <param name="transactionID">The ID of the transaction if there is any</param>
        /// /// <param name="receiptID">The receipt ID of the transaction if there is any</param>
        /// <returns></returns>
        public virtual bool Activate(IConnectionManager entry, RecordIdentifier giftCardID, RecordIdentifier storeID,RecordIdentifier terminalID,RecordIdentifier userID,RecordIdentifier staffID,RecordIdentifier transactionID, RecordIdentifier receiptID)
        {
            var card = Get(entry, giftCardID);
            var line = new GiftCardLine();

            ValidateSecurity(entry, BusinessObjects.Permission.ManageGiftCards);

            if (card == null)
            {
                throw new DataIntegrityException(typeof(GiftCard), giftCardID);
            }

            if (card.Active)
            {
                return false;
            }

            card.Active = true;

            Save(entry, card);

            line.Amount = card.Balance;
            line.GiftCardID = card.ID;
            line.Operation = GiftCardLine.GiftCardLineEnum.Activate;
            line.StoreID = storeID;
            line.TerminalID = terminalID;
            line.TransactionID = transactionID;
            line.ReceiptID = receiptID;
            line.UserID = userID;
            line.StaffID = staffID;

            Providers.GiftCardLineData.Save(entry, line);

            return true;
        }

        /// <summary>
        /// Activates a gift card
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the giftcard</param>
        /// <returns></returns>
        public virtual bool MarkIssued(IConnectionManager entry, RecordIdentifier giftCardID)
        {
            var card = Get(entry, giftCardID);

            ValidateSecurity(entry, Permission.ManageGiftCards);

            if (card == null)
            {
                throw new DataIntegrityException(typeof(GiftCard), giftCardID);
            }

            if (card.Issued)
            {
                return false;
            }

            card.Issued = true;

            Save(entry, card);

            return true;
        }

        /// <summary>
        /// Deactivates a gift card
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the giftcard</param>
        /// <param name="storeID">ID of the store where the operation is done or empty string if head office</param>
        /// <param name="terminalID">ID of the terminal where the operation is done or empty string if on store or headoffice level</param>
        /// <param name="userID">ID of the Site Manager user, or Guid.Empty if not avalible</param>
        /// <param name="staffID">ID of the POS staff or empty string if not avalible</param>
        /// <param name="transactionNumber">The ID of the transaction if there is any</param>
        /// <returns></returns>
        public virtual bool Deactivate(IConnectionManager entry, RecordIdentifier giftCardID, RecordIdentifier storeID, RecordIdentifier terminalID, RecordIdentifier userID, RecordIdentifier staffID, RecordIdentifier transactionNumber)
        {
            var card = Get(entry, giftCardID);
            var line = new GiftCardLine();

            ValidateSecurity(entry, BusinessObjects.Permission.ManageGiftCards);

            if (card == null)
            {
                throw new DataIntegrityException(typeof(GiftCard), giftCardID);
            }

            if (!card.Active)
            {
                return false;
            }

            card.Active = false;

            Save(entry, card);

            line.Amount = card.Balance;
            line.GiftCardID = card.ID;
            line.Operation = GiftCardLine.GiftCardLineEnum.Deactivate;
            line.StoreID = storeID;
            line.TerminalID = terminalID;
            line.TransactionID = transactionNumber;
            line.UserID = userID;
            line.StaffID = staffID;

            Providers.GiftCardLineData.Save(entry, line);

            return true;
        }

        /// <summary>
        /// Saves a gift card, creating new one if the ID was empty or record did not exist, else updates.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCard">The gift card to save</param>
        public virtual void Save(IConnectionManager entry, GiftCard giftCard)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageGiftCards);

            giftCard.Validate();

            var statement = new SqlServerStatement("RBOGIFTCARDTABLE");

            bool isNew = false;
            if (giftCard.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                giftCard.ID = DataProviderFactory.Instance.GenerateNumber<IGiftCardData, GiftCard>(entry); 
            }

            if (isNew || !Exists(entry, giftCard.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("GIFTCARDID", (string) giftCard.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("GIFTCARDID", (string) giftCard.ID);
            }

            statement.AddField("BALANCE", giftCard.Balance, SqlDbType.Decimal);
            statement.AddField("CURRENCY", (string) giftCard.Currency);
            statement.AddField("ACTIVE", giftCard.Active ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ISSUED", giftCard.Issued ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("REFILLABLE", giftCard.Refillable ? 1 : 0, SqlDbType.TinyInt);

            if (giftCard.CreatedDate.IsEmpty)
            {
                statement.AddField("DATECREATED", DBNull.Value, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("DATECREATED", giftCard.CreatedDate.DateTime, SqlDbType.DateTime);
            }

            if (giftCard.LastUsedDate.IsEmpty)
            {
                statement.AddField("LASTUSEDDATE", DBNull.Value, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("LASTUSEDDATE", giftCard.LastUsedDate.DateTime, SqlDbType.DateTime);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves a gift card, creating new one if the ID was empty or record did not exist, else updates.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCard">The gift card to save</param>
        /// <param name="prefix">Prefix for the ID added for barcode usage</param>
        /// <param name="numberSequenceLowest"></param>
        public virtual void Save(IConnectionManager entry, GiftCard giftCard, string prefix, int? numberSequenceLowest = null)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.ManageGiftCards);

            giftCard.Validate();

            var statement = new SqlServerStatement("RBOGIFTCARDTABLE");

            bool isNew = false;
            if (giftCard.ID == RecordIdentifier.Empty)
            {
                isNew = true;

                if (numberSequenceLowest != null)
                {
                    Providers.NumberSequenceData.SetNumberSequenceLowest(entry, SequenceID, numberSequenceLowest);
                }

                giftCard.ID = DataProviderFactory.Instance.GenerateNumber<IGiftCardData, GiftCard>(entry);
            }

            if (isNew || !Exists(entry, giftCard.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("GIFTCARDID", prefix + (string)giftCard.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("GIFTCARDID", (string)giftCard.ID);
            }

            statement.AddField("BALANCE", giftCard.Balance, SqlDbType.Decimal);
            statement.AddField("CURRENCY", (string)giftCard.Currency);
            statement.AddField("ACTIVE", giftCard.Active ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ISSUED", giftCard.Issued ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("REFILLABLE", giftCard.Refillable ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("BARCODE", prefix != "" ? prefix + (string)giftCard.ID : null);

            if (giftCard.CreatedDate.IsEmpty)
            {
                statement.AddField("DATECREATED", DBNull.Value, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("DATECREATED", giftCard.CreatedDate.DateTime, SqlDbType.DateTime);
            }

            if (giftCard.LastUsedDate.IsEmpty)
            {
                statement.AddField("LASTUSEDDATE", DBNull.Value, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("LASTUSEDDATE", giftCard.LastUsedDate.DateTime, SqlDbType.DateTime);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Delete a gift card by given ID and gift card lines assoiated with it 
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="id">ID of the gift card</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOGIFTCARDTABLE", "GIFTCARDID", id, Permission.ManageGiftCards);
            DeleteRecord(entry, "RBOGIFTCARDTRANSACTIONS", "GIFTCARDID", id, Permission.ManageGiftCards);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "GIFTCARD"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOGIFTCARDTABLE", "GIFTCARDID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
