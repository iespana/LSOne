using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Card
{
    public class CardToTenderMappingData : SqlServerDataProviderBase, ICardToTenderMappingData
    {
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "POSISCARDTOTENDERMAPPING", "TENDERID", "CARDID", "CARDID");
        }

        private static void PopulateCardTender(IDataReader dr, CardToTenderMapping cardTender)
        {
            cardTender.ID = (string)dr["CARDID"];
            cardTender.Text = Conversion.ToStr((int)dr["TERNERID"]);
            cardTender.BrokerID = (string)dr["BROKERID"];
        }

        public virtual CardToTenderMapping Get(IConnectionManager entry, RecordIdentifier cardID, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT CARDID, ISNULL(TENDERID, 0), BROKERID
                                    FROM POSISCARDTOTENDERMAPPING WHERE DATAAREAID = @dataAreaID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                return Get<CardToTenderMapping>(entry, cmd, cardID, PopulateCardTender, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual CardToTenderMapping Get(IConnectionManager entry, RecordIdentifier brokerID, RecordIdentifier tenderType, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT CARDID, ISNULL(TENDERID, 0), BROKERID
                                    FROM POSISCARDTOTENDERMAPPING
                                    WHERE DATAAREAID = @dataAreaID AND BROKERID = @brokerID AND TENDERID = @tenderType";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "brokerID", brokerID);
                MakeParam(cmd, "tenderType", tenderType);
                return Get<CardToTenderMapping>(entry, cmd, brokerID, PopulateCardTender, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier cardID)
        {
            return RecordExists<DataEntity>(entry, "POSISCARDTOTENDERMAPPING", "CARDID", cardID);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier cardID)
        {
            ValidateSecurity(entry);
            DeleteRecord<CardToTenderMapping>(entry, "POSISCARDTOTENDERMAPPING", "CARDID", cardID,
                LSOne.DataLayer.BusinessObjects.Permission.CardTypesEdit);
        }

        public virtual void Save(IConnectionManager entry, CardToTenderMapping cardTender)
        {
            if (cardTender.ID == RecordIdentifier.Empty || cardTender.BrokerID == RecordIdentifier.Empty)
            {
                throw new ArgumentException();
            }
            var statement = new SqlServerStatement("POSISCARDTOTENDERMAPPING");
            if (Exists(entry, cardTender.ID))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("CARDID", cardTender.ID, SqlDbType.Int);
                statement.AddCondition("BROKERID", (string)cardTender.BrokerID);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("CARDID", cardTender.ID, SqlDbType.Int);
                statement.AddKey("BROKERID", (string)cardTender.BrokerID);
            }
            statement.AddField("TENDERID", Conversion.ToInt(cardTender.Text), SqlDbType.Int);
            Save(entry, cardTender, statement);
        }
    }
}
