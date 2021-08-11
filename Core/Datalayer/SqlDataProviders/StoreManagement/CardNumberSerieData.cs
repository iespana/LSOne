using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.StoreManagement
{
    public class CardNumberSerieData : SqlServerDataProviderBase, ICardNumberSerieData
    {
        private static void PopulateSerie(IDataReader dr, CardNumberSerie numberSerie)
        {
            numberSerie.CardTypeID = (string)dr["CARDTYPEID"];
            numberSerie.CardNumberFrom = (string)dr["CARDNUMBERFROM"];
            numberSerie.CardNumberTo = (string)dr["CARDNUMBERTO"];
        }

        /// <summary>
        /// Gets a specific payment type for a given store or all payment types for a given store.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="cardTypeID"></param>
        /// <returns></returns>
        public virtual List<CardNumberSerie> GetNumberSeries(IConnectionManager entry, RecordIdentifier cardTypeID)
        {
            ValidateSecurity(entry, Permission.CardTypesView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select CARDTYPEID, CARDNUMBERFROM, CARDNUMBERTO " +
                                  "from RBOTENDERTYPECARDNUMBERS " +
                                  "where DATAAREAID = @dataAreaId and CARDTYPEID = @cardTypeID order by CARDNUMBERFROM";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "cardTypeID", (string) cardTypeID);

                return Execute<CardNumberSerie>(entry, cmd, CommandType.Text, PopulateSerie);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier fullSerieIdentifier)
        {
            DeleteRecord(entry, "RBOTENDERTYPECARDNUMBERS",
                new string[] {"CARDTYPEID", "CARDNUMBERFROM", "CARDNUMBERTO"}, fullSerieIdentifier,
                Permission.CardTypesEdit);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier fullSerieIdentifier)
        {
            return RecordExists(entry, "RBOTENDERTYPECARDNUMBERS", new string[] { "CARDTYPEID", "CARDNUMBERFROM", "CARDNUMBERTO" }, fullSerieIdentifier);
        }

        public virtual void Save(IConnectionManager entry, RecordIdentifier fullSerieIdentifier, CardNumberSerie serie)
        {
            ValidateSecurity(entry, Permission.CardTypesEdit);

            var statement = entry.Connection.CreateStatement("RBOTENDERTYPECARDNUMBERS");

            if (!fullSerieIdentifier.HasSecondaryID)
            {
                statement.StatementType = StatementType.Insert;

                statement.AddField("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddField("CARDTYPEID", (string) fullSerieIdentifier);

                statement.AddKey("CARDNUMBERFROM", serie.CardNumberFrom);
                statement.AddKey("CARDNUMBERTO", serie.CardNumberTo);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CARDTYPEID", (string) fullSerieIdentifier);
                statement.AddCondition("CARDNUMBERFROM", (string) fullSerieIdentifier.SecondaryID);
                statement.AddCondition("CARDNUMBERTO", (string) fullSerieIdentifier.SecondaryID.SecondaryID);

                statement.AddField("CARDNUMBERFROM", serie.CardNumberFrom);
                statement.AddField("CARDNUMBERTO", serie.CardNumberTo);
            }

            statement.AddField("CARDNUMBERLENGTH", serie.CardNumberFrom.Length, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
