using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Card;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Card
{
    public class CardTypeData : SqlServerDataProviderBase, ICardTypeData
    {
        public virtual void Save(GenericConnector.Interfaces.IConnectionManager entry, BusinessObjects.Card.CardType item)
        {
            bool isNew = false;
            var statement = entry.Connection.CreateStatement("RBOTENDERTYPECARDTABLE");

            ValidateSecurity(entry, Permission.CardTypesEdit);

            if (item.ID.IsEmpty)
            {
                isNew = true;
                item.ID = DataProviderFactory.Instance.GenerateNumber<ICardTypeData, CardType>(entry);
            }

            if (isNew || !Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CARDTYPEID", (string)item.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CARDTYPEID", (string)item.ID);
            }

            statement.AddField("NAME", item.Text);
            statement.AddField("CardTypes", (int)item.CardTypes, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
        

        public virtual void Delete(GenericConnector.Interfaces.IConnectionManager entry, Utilities.DataTypes.RecordIdentifier ID)
        {
            DeleteRecord(entry, "RBOTENDERTYPECARDTABLE", "CARDTYPEID", ID, Permission.CardTypesEdit);
        }

        public virtual bool Exists(GenericConnector.Interfaces.IConnectionManager entry, Utilities.DataTypes.RecordIdentifier ID)
        {
            return RecordExists(entry, "RBOTENDERTYPECARDTABLE", "CARDTYPEID", ID);
        }

        /// <summary>
        /// Returns a simple list of card types that exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "RBOTENDERTYPECARDTABLE", "NAME", "CARDTYPEID", "NAME");
        }

        public virtual CardType Get(IConnectionManager entry, RecordIdentifier cardID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select CARDTYPEID, ISNULL(NAME,'') as NAME, ISNULL(CARDTYPES,500) as CARDTYPES " +
                                  "from RBOTENDERTYPECARDTABLE where CARDTYPEID = @cardID and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "cardID", (string)cardID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var list = Execute<CardType>(entry, cmd, CommandType.Text, PopulateCardType);
                return (list != null && list.Count > 0) ? list[0] : null;
            }
        }

        private static void PopulateCardType(IDataReader dr, CardType cardType)
        {
            cardType.ID = (string)dr["CARDTYPEID"];
            cardType.Text = (string)dr["NAME"];
            cardType.CardTypes = (CardTypesEnum)(int)dr["CARDTYPES"];
        }

        public virtual bool CardNameExists(IConnectionManager entry, string description)
        {
            ValidateSecurity(entry, Permission.CardTypesEdit);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(COUNT(NAME), 0) AS NAMEEXISTS
                                    FROM RBOTENDERTYPECARDTABLE
                                    WHERE DATAAREAID = @DATAAREAID
                                    AND NAME = @CARDNAME";

                MakeParam(cmd, "CARDNAME", description);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                return (int)entry.Connection.ExecuteScalar(cmd) > 0;
            }
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "CARDTYPE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOTENDERTYPECARDTABLE", "CARDTYPEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
