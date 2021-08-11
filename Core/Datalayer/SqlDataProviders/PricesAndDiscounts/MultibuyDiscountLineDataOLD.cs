using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    /// <summary>
    /// Data provider class for a configuration line on a Multibuy discount
    /// </summary>
    public class MultibuyDiscountLineDataOLD : SqlServerDataProviderBase
    {
        private static void PopulateMultibuyDiscountLine(IDataReader dr, MultibuyDiscountLine line)
        {
            line.OfferID = (string)dr["OFFERID"];
            line.MinQuantity = (decimal)dr["MINQUANTITY"];
            line.PriceOrDiscountPercent = (decimal)dr["UNITPRICEORDISCPCT"];
        }

        public virtual MultibuyDiscountLine GetMultibuyOfferForQuantity(IConnectionManager entry, RecordIdentifier offerID, decimal quantity )
        {
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT OFFERID, MINQUANTITY, ISNULL(UNITPRICEORDISCPCT, 0) AS UNITPRICEORDISCPCT FROM POSMULTIBUYDISCOUNTLINE 
                                  WHERE MINQUANTITY = (SELECT MAX(MINQUANTITY) FROM POSMULTIBUYDISCOUNTLINE WHERE MINQUANTITY <= @minQuantity AND OFFERID = @offerID AND DATAAREAID = @dataAreaID) 
                                  AND DATAAREAID = @dataAreaID AND OFFERID = @offerID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", offerID);
                MakeParam(cmd, "minQuantity", quantity, SqlDbType.Decimal);
                
                var lines = Execute<MultibuyDiscountLine>(entry, cmd, CommandType.Text, PopulateMultibuyDiscountLine);
                return lines.Count > 0 ? lines[0] : null;
            }
        }

        /// <summary>
        /// Gets all configurations for a given offer.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">ID of the Offer</param>
        /// <returns>List of MultibuyDiscountLine entities</returns>
        public virtual List<MultibuyDiscountLine> GetAllForOffer(IConnectionManager entry, RecordIdentifier offerID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select OFFERID,MINQUANTITY,ISNULL(UNITPRICEORDISCPCT,0.0) as UNITPRICEORDISCPCT " +
                    "from POSMULTIBUYDISCOUNTLINE " +
                    "where OFFERID = @offerID and DATAAREAID = @dataAreaID " +
                    "order by MINQUANTITY";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string)offerID);

                return Execute<MultibuyDiscountLine>(entry, cmd, CommandType.Text, PopulateMultibuyDiscountLine);
            }
        }

        /// <summary>
        /// Checks if a configuration with a given ID exists in the database.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the configuration to check for. Note the RecordIdentifier here has to contain double Key, OfferID, MinQuantity</param>
        /// <returns>True if a configuration exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "POSMULTIBUYDISCOUNTLINE", new[]{"OFFERID","MINQUANTITY"}, id);
        }

        /// <summary>
        /// Deletes a configuration with a given ID.
        /// </summary>
        /// <remarks>Manage discounts permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the configuration to delete for. Note the RecordIdentifier here has to contain double Key, OfferID, MinQuantity</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "POSMULTIBUYDISCOUNTLINE", new[] { "OFFERID", "MINQUANTITY" }, id, BusinessObjects.Permission.ManageDiscounts);
        }

        /// <summary>
        /// Saves the entity. If the record does not exist then its inserted, else its updated. 
        /// </summary>
        /// <remarks>Manage discounts permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="line">The data entity to be saved</param>
        public virtual void Save(IConnectionManager entry, MultibuyDiscountLine line)
        {
            var statement = new SqlServerStatement("POSMULTIBUYDISCOUNTLINE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageDiscounts);

            if (!Exists(entry, line.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("OFFERID", (string)line.OfferID);
                statement.AddKey("MINQUANTITY", (decimal)line.MinQuantity, SqlDbType.Decimal);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("OFFERID", (string)line.ID);
                statement.AddCondition("MINQUANTITY", (decimal)line.MinQuantity, SqlDbType.Decimal);
            }

            statement.AddField("UNITPRICEORDISCPCT", line.PriceOrDiscountPercent, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
