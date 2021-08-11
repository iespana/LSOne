using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.StoreManagement
{
    public class PaymentTypeCardTypesData : SqlServerDataProviderBase, IPaymentTypeCardTypesData
    {
        public List<DataEntity> GetUnusedCardTypesForTender(IConnectionManager entry, RecordIdentifier storeID,
                                                                   RecordIdentifier tenderTypeID,
                                                                   RecordIdentifier selectedCardType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select a.CARDTYPEID, a.NAME from RBOTENDERTYPECARDTABLE a " +
                                  "where a.DATAAREAID = @dataAreaId " +
                                  "and not Exists(Select 'x' from RBOSTORETENDERTYPECARDTABLE " +
                                  "where DATAAREAID = @dataAreaId and STOREID=@storeID and TENDERTYPEID=@tenderTypeID and CARDTYPEID = a.CARDTYPEID) " +
                                  "or a.CARDTYPEID=@selectedCardType " +
                                  "order by a.NAME";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string) storeID);
                MakeParam(cmd, "tenderTypeID", (string) tenderTypeID);
                MakeParam(cmd, "selectedCardType", (string) selectedCardType);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "CARDTYPEID");
            }
        }

        private static void PopulateCard(IDataReader dr, StoreCardType cardType)
        {
            cardType.StoreID = (string)dr["STOREID"];
            cardType.TenderTypeID = (string)dr["TENDERTYPEID"];
            cardType.CardTypeID = (string)dr["CARDTYPEID"];
            cardType.Description = (string)dr["NAME"];
            cardType.CheckModulus = ((byte)dr["CHECKMODULUS"] != 0);
            cardType.CheckExpiredDate = ((byte)dr["CHECKEXPIREDDATE"] != 0);
            cardType.ProcessLocally = ((byte)dr["PROCESSLOCALLY"] != 0);
            cardType.AllowManualInput = ((byte)dr["ALLOWMANUALINPUT"] != 0);
        }

        private static string SelectCardString()
        {
            return "Select STOREID,TENDERTYPEID,CARDTYPEID,ISNULL(NAME,'') as NAME," +
                  "ISNULL(CHECKMODULUS,0) as CHECKMODULUS,ISNULL(CHECKEXPIREDDATE,0) as CHECKEXPIREDDATE," +
                  "ISNULL(PROCESSLOCALLY,0) as PROCESSLOCALLY,ISNULL(ALLOWMANUALINPUT,0) as ALLOWMANUALINPUT " +
                  "from RBOSTORETENDERTYPECARDTABLE ";
        }

        public StoreCardType GetCardForTenderType(IConnectionManager entry, RecordIdentifier storeID,
                                                    RecordIdentifier tenderTypeID, RecordIdentifier cardTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = SelectCardString() +
                                  "where DATAAREAID = @dataAreaId and STOREID=@storeID and TENDERTYPEID=@tenderTypeID and CARDTYPEID=@cardTypeID " +
                                  "order by Name";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string) storeID);
                MakeParam(cmd, "tenderTypeID", (string) tenderTypeID);
                MakeParam(cmd, "cardTypeID", (string) cardTypeID);

                return Execute<StoreCardType>(entry, cmd, CommandType.Text, PopulateCard)[0];
            }
        }

        public List<StoreCardType> GetCardListForTenderType(IConnectionManager entry, RecordIdentifier storeID,
                                                              RecordIdentifier tenderTypeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = SelectCardString() +
                                  "where DATAAREAID = @dataAreaId and STOREID=@storeID and TENDERTYPEID=@tenderTypeID " +
                                  "order by Name";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeID", (string) storeID);
                MakeParam(cmd, "tenderTypeID", (string) tenderTypeID);

                return Execute<StoreCardType>(entry, cmd, CommandType.Text, PopulateCard);
            }
        }

        public virtual bool Exists(IConnectionManager entry, StoreCardType cardType)
        {
            // Make a damn tripple record identifier
            var identifier = (RecordIdentifier)cardType.StoreID.Clone();
            identifier.SecondaryID = (RecordIdentifier)cardType.TenderTypeID.Clone();
            identifier.SecondaryID.SecondaryID = (RecordIdentifier)cardType.CardTypeID.Clone();

            return RecordExists(entry, "RBOSTORETENDERTYPECARDTABLE", new[] { "STOREID", "TENDERTYPEID", "CARDTYPEID" }, identifier);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier storeTenderAndCardIdentifier)
        {
            DeleteRecord(entry, "RBOSTORETENDERTYPECARDTABLE", new[] { "STOREID", "TENDERTYPEID", "CARDTYPEID" }, storeTenderAndCardIdentifier, BusinessObjects.Permission.StoreEdit);
        }

        public virtual void Save(IConnectionManager entry, StoreCardType cardType)
        {
            var statement = new SqlServerStatement("RBOSTORETENDERTYPECARDTABLE");

            ValidateSecurity(entry, BusinessObjects.Permission.StoreEdit);

            if (!Exists(entry, cardType))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("STOREID", (string)cardType.StoreID);
                statement.AddKey("TENDERTYPEID", (string)cardType.TenderTypeID);
                statement.AddKey("CARDTYPEID", (string)cardType.CardTypeID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("STOREID", (string)cardType.StoreID);
                statement.AddCondition("TENDERTYPEID", (string)cardType.TenderTypeID);
                statement.AddCondition("CARDTYPEID", (string)cardType.CardTypeID);
            }

            statement.AddField("NAME", cardType.Description);
            statement.AddField("CHECKMODULUS", cardType.CheckModulus ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("CHECKEXPIREDDATE", cardType.CheckExpiredDate ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PROCESSLOCALLY", cardType.ProcessLocally ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("ALLOWMANUALINPUT", cardType.AllowManualInput ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
