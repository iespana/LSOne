using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    public class CardInfoData : SqlServerDataProviderBase, ICardInfoData
    {
        private static void PopulateCardInfo(IDataReader dr, CardInfo cardInfo)
        {
            cardInfo.ID = (string)dr["CARDTYPEID"];
            cardInfo.CardName = (string)dr["NAME"];
            cardInfo.TenderTypeId = (string)dr["TENDERTYPEID"];
            cardInfo.CardFee = (decimal)dr["CARDFEE"];

            cardInfo.ModulusCheck = ((byte)dr["CHECKMODULUS"] != 0);
            cardInfo.ExpDateCheck = ((byte)dr["CHECKEXPIREDDATE"] != 0);
            cardInfo.ProcessLocally = ((byte)dr["PROCESSLOCALLY"] != 0);

            cardInfo.AllowManualInput = ((byte)dr["ALLOWMANUALINPUT"] != 0);
            cardInfo.EnterFleetInfo = ((byte)dr["ENTERFLEETINFO"] != 0);


            cardInfo.CardType = (CardTypesEnum)(int)dr["CARDTYPES"];
            cardInfo.Issuer = (string)dr["CARDISSUER"];
            cardInfo.BinTo = (string)dr["CARDNUMBERTO"];
            cardInfo.BinFrom = (string)dr["CARDNUMBERFROM"];
            cardInfo.BinLength = (int)dr["CARDNUMBERLENGTH"];
        }

        public virtual List<CardInfo> GetAll(IConnectionManager entry) 
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select DISTINCT ISNULL(c.CARDTYPEID,'') AS CARDTYPEID, ISNULL(c.NAME,'') as NAME, ISNULL(s.TENDERTYPEID,'') as TENDERTYPEID, 
                                    ISNULL(s.CARDFEE,0) as CARDFEE, ISNULL(s.CHECKMODULUS,0) as CHECKMODULUS, 
                                    ISNULL(s.CHECKEXPIREDDATE,0) as CHECKEXPIREDDATE, ISNULL(s.PROCESSLOCALLY,1) as PROCESSLOCALLY,
                                    ISNULL(s.ALLOWMANUALINPUT,0) as ALLOWMANUALINPUT,ISNULL(s.ENTERFLEETINFO,0) as ENTERFLEETINFO,
                                    ISNULL(c.CARDTYPES,0) as CARDTYPES,ISNULL(c.CARDISSUER,'') as CARDISSUER,
                                    ISNULL(n.CARDNUMBERTO,'') as CARDNUMBERTO, ISNULL(n.CARDNUMBERFROM,'') as CARDNUMBERFROM,
                                    ISNULL(n.CARDNUMBERLENGTH,0) as CARDNUMBERLENGTH 
                                    from RBOTenderTypeCardTable c 
                                    LEFT OUTER JOIN RBOTenderTypeCardNumbers n on c.DATAAREAID = n.DATAAREAID AND c.CARDTYPEID = n.CARDTYPEID
                                    LEFT OUTER JOIN RBOStoreTenderTypeCardTable s ON c.DATAAREAID = s.DATAAREAID and s.CARDTYPEID = c.CARDTYPEID
                                    where c.DATAAREAID = @dataAreaID                                     
                                    order by ISNULL(n.CARDNUMBERLENGTH,0) desc";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<CardInfo>(entry, cmd, CommandType.Text, PopulateCardInfo);
            }
        }

        public virtual void Delete(IConnectionManager entry, string cardTypeID)
        {
            DeleteRecord(entry, "RBOTENDERTYPECARDTABLE", "CARDTYPEID", cardTypeID, Permission.CardTypesEdit);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier cardTypeID)
        {
            return RecordExists(entry, "RBOTENDERTYPECARDTABLE", "CARDTYPEID", cardTypeID);
        }

        public virtual bool InUse(IConnectionManager entry, RecordIdentifier currencyID)
        {
            return RecordExists(entry, "RBOSTORETENDERTYPECARDTABLE", "CARDTYPEID", currencyID);
        }

        public virtual void Save(IConnectionManager entry, CardInfo cardInfo)
        {
            bool isNew = false;
            var statement = entry.Connection.CreateStatement("RBOTENDERTYPECARDTABLE");

            ValidateSecurity(entry, Permission.CardTypesEdit);

            if (cardInfo.ID.IsEmpty)
            {
                isNew = true;
                cardInfo.ID = DataProviderFactory.Instance.GenerateNumber<ICardInfoData, CardInfo>(entry);
            }

            if (isNew || !Exists(entry, cardInfo.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CARDTYPEID", (string)cardInfo.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CARDTYPEID", (string)cardInfo.ID);
            }

            statement.AddField("NAME", cardInfo.CardName);
            statement.AddField("CardTypes", (int)cardInfo.CardType, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
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
