using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Infocodes
{
    public class InfocodeSubcodeData : SqlServerDataProviderBase, IInfocodeSubcodeData
    {
        private static string ResolveSort(InfocodeSubcodeSorting sortEnum, bool sortBackwards)
        {
            var sortString = " Order By ";

            switch (sortEnum)
            {
                case InfocodeSubcodeSorting.Description:
                    sortString += "DESCRIPTION ASC";
                    break;
                case InfocodeSubcodeSorting.TriggerFunction:
                    sortString += "TRIGGERFUNCTION ASC";
                    break;
                case InfocodeSubcodeSorting.TriggerCode:
                    sortString += "TRIGGERCODE ASC";
                    break;
                case InfocodeSubcodeSorting.Prompt:
                    sortString += "INFOCODEPROMPT ASC";
                    break;
                case InfocodeSubcodeSorting.ListType:
                    sortString += "USAGECATEGORY ASC";
                    break;
            }

            if (sortBackwards)
            {
                sortString = sortString.Replace("ASC","DESC");
            }

            return sortString;
        }

        private static string BaseSelectString
        {
            get
            {
                return
                    "SELECT INFOCODEID,SUBCODEID,ISNULL(DESCRIPTION,'') AS DESCRIPTION," +
                    "ISNULL(TRIGGERFUNCTION,0) AS TRIGGERFUNCTION," +
                    "ISNULL(TRIGGERCODE,'') AS TRIGGERCODE," +
                    "ISNULL(PRICETYPE,0) AS PRICETYPE," +
                    "ISNULL(AMOUNTPERCENT,0) AS AMOUNTPERCENT," +
                    "ISNULL(VARIANTCODE,'') AS VARIANTCODE," +
                    "ISNULL(VARIANTNEEDED,0) AS VARIANTNEEDED," +
                    "ISNULL(QTYLINKEDTOTRIGGERLINE,0) AS QTYLINKEDTOTRIGGERLINE," +
                    "ISNULL(PRICEHANDLING,0) AS PRICEHANDLING," +
                    "ISNULL(UNITOFMEASURE,'') AS UNITOFMEASURE," +
                    "ISNULL(QTYPERUNITOFMEASURE,0) AS QTYPERUNITOFMEASURE," +
                    "ISNULL(INFOCODEPROMPT,'') AS INFOCODEPROMPT," +
                    "ISNULL(MAXSELECTION,0) AS MAXSELECTION," +
                    "ISNULL(SERIALLOTNEEDED,0) AS SERIALLOTNEEDED," +
                    "ISNULL(USAGECATEGORY,'') AS USAGECATEGORY," +
                    "ISNULL(ITEMNAME,'') AS ITEMNAME," +
                    "ISNULL(VARIANTNAME,'') AS VARIANTNAME " +
                    "FROM RBOINFORMATIONSUBCODETABLE t ";
            }
        }

        private static void PopulateInfocodeSubcode(IDataReader dr, InfocodeSubcode infocodeSubcode)
        {
            infocodeSubcode.InfocodeId = (string)dr["INFOCODEID"];
            infocodeSubcode.SubcodeId = (string)dr["SUBCODEID"];
            infocodeSubcode.Text = (string)dr["DESCRIPTION"];
            infocodeSubcode.TriggerFunction = (TriggerFunctions)dr["TRIGGERFUNCTION"];
            infocodeSubcode.TriggerCode = (string)dr["TRIGGERCODE"];
            infocodeSubcode.PriceType = (PriceTypes)dr["PRICETYPE"];
            infocodeSubcode.AmountPercent = (decimal)dr["AMOUNTPERCENT"];
            infocodeSubcode.VariantCode = (string)dr["VARIANTCODE"];
            infocodeSubcode.VariantNeeded = ((byte)dr["VARIANTNEEDED"] != 0);
            infocodeSubcode.QtyLinkedToTriggerLine = ((byte)dr["QTYLINKEDTOTRIGGERLINE"] != 0);
            infocodeSubcode.PriceHandling = (PriceHandlings)dr["PRICEHANDLING"];
            infocodeSubcode.UnitOfMeasure = (string)dr["UNITOFMEASURE"];
            infocodeSubcode.QtyPerUnitOfMeasure = (decimal)dr["QTYPERUNITOFMEASURE"];
            infocodeSubcode.InfocodePrompt = (string)dr["INFOCODEPROMPT"];
            infocodeSubcode.MaxSelection = (int)dr["MAXSELECTION"];
            infocodeSubcode.SerialLotNeeded = ((byte)dr["SERIALLOTNEEDED"] != 0);
            infocodeSubcode.UsageCategory = (UsageCategoriesEnum)dr["USAGECATEGORY"];
            infocodeSubcode.ItemName = (string) dr["ITEMNAME"];
            infocodeSubcode.VariantDescription = (string) dr["VARIANTNAME"];
        }

        /// <summary>
        /// Gets the infocodeSubcode with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeSubcodeID">The id of the infocodeSubcode (InfocodeID, SubcodeID)</param>
        /// <returns></returns>
        public virtual InfocodeSubcode Get(IConnectionManager entry, RecordIdentifier infocodeSubcodeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "left join RETAILITEM r on t.TRIGGERCODE = r.ITEMID " +
                    "where DATAAREAID = @dataAreaId and INFOCODEID = @infocodeId and SUBCODEID = @subCodeId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "infocodeId", (string)infocodeSubcodeID);
                MakeParam(cmd, "subCodeId", (string)infocodeSubcodeID.SecondaryID);

                var result = Execute<InfocodeSubcode>(entry, cmd, CommandType.Text, PopulateInfocodeSubcode);
                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual List<InfocodeSubcode> GetListForInfocodeTaxGroupOnly(IConnectionManager entry, RecordIdentifier infocodeID, InfocodeSubcodeSorting sortEnum, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "left join RETAILITEM r on t.TRIGGERCODE = r.ITEMID " +
                    "where DATAAREAID = @dataAreaId and INFOCODEID = @infocodeId and TRIGGERFUNCTION = 6 " + ResolveSort(sortEnum, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "infocodeId", (string)infocodeID);

                return Execute<InfocodeSubcode>(entry, cmd, CommandType.Text, PopulateInfocodeSubcode);
            }
        }

        /// <summary>
        /// Returns a list of all infocodeSubcodes for the given infocode ID, in ASC order based on Infocode Description
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeID">The id of the Infocode</param>
        /// <returns>A list of all infocodeSubcodes for the given infocode ID</returns>
        public virtual List<InfocodeSubcode> GetListForInfocode(IConnectionManager entry, RecordIdentifier infocodeID)
        {
            return GetListForInfocode(entry, infocodeID, InfocodeSubcodeSorting.Description, false);
        }

        /// <summary>
        /// Returns a list of all infocodeSubcodes for the given infocode ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeID">The id of the Infocode</param>
        /// <param name="sortEnum">An enum which defines the sort ordering of the result set</param>
        /// <param name="sortBackwards">Wether to reverse the ordering of the result set or not</param>
        /// <returns>A list of all infocodeSubcodes for the given infocode ID</returns>
        public virtual List<InfocodeSubcode> GetListForInfocode(IConnectionManager entry, RecordIdentifier infocodeID, InfocodeSubcodeSorting sortEnum, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "left join RETAILITEM r on t.TRIGGERCODE = r.ITEMID " +
                    "where DATAAREAID = @dataAreaId and INFOCODEID = @infocodeId" + ResolveSort(sortEnum, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "infocodeId", (string)infocodeID);

                return Execute<InfocodeSubcode>(entry, cmd, CommandType.Text, PopulateInfocodeSubcode);
            }
        }

        /// <summary>
        /// Checks if an infocodeSubcode with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeSubcodeID">The ID of the infocodeSubcode to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier infocodeSubcodeID)
        {
            return RecordExists(entry, "RBOINFORMATIONSUBCODETABLE", new[] { "INFOCODEID", "SUBCODEID" }, infocodeSubcodeID);
        }

        private static bool SubcodeIDExists(IConnectionManager entry, RecordIdentifier subcodeID)
        {
            return RecordExists(entry, "RBOINFORMATIONSUBCODETABLE", new[] { "SUBCODEID" }, subcodeID);
        }

        /// <summary>
        /// Deletes an infocodeSubecode with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeSubcodeID">The ID of the infocodeSubcode to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier infocodeSubcodeID)
        {
            DeleteRecord(entry, "RBOINFORMATIONSUBCODETABLE", new[] { "INFOCODEID", "SUBCODEID" }, infocodeSubcodeID, BusinessObjects.Permission.InfocodeEdit);
        }

        /// <summary>
        /// Saves an InfocodeSubcode object into the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="subcode">The InfocodeSubcode to be saved</param>
        public virtual void Save(IConnectionManager entry, InfocodeSubcode subcode)
        {
            var statement = new SqlServerStatement("RBOINFORMATIONSUBCODETABLE");
            ValidateSecurity(entry, BusinessObjects.Permission.InfocodeEdit);

            bool isNew = false;
            if (subcode.SubcodeId == RecordIdentifier.Empty)
            {
                isNew = true;
                subcode.SubcodeId = DataProviderFactory.Instance.GenerateNumber<IInfocodeSubcodeData, InfocodeSubcode>(entry);
            }

            if (isNew || !Exists(entry, subcode.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("INFOCODEID", (string) subcode.ID.PrimaryID);
                statement.AddKey("SUBCODEID", (string) subcode.ID.SecondaryID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("INFOCODEID", (string) subcode.ID.PrimaryID);
                statement.AddCondition("SUBCODEID", (string) subcode.ID.SecondaryID);
            }
            statement.AddField("DESCRIPTION", subcode.Text);
            statement.AddField("TRIGGERFUNCTION", (int) subcode.TriggerFunction, SqlDbType.Int);
            statement.AddField("TRIGGERCODE", (string) subcode.TriggerCode);
            statement.AddField("PRICETYPE", (int) subcode.PriceType, SqlDbType.Int);
            statement.AddField("AMOUNTPERCENT", subcode.AmountPercent, SqlDbType.Decimal);
            statement.AddField("VARIANTCODE", subcode.VariantCode.ToString());
            statement.AddField("VARIANTNEEDED", subcode.VariantNeeded ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("QTYLINKEDTOTRIGGERLINE", subcode.QtyLinkedToTriggerLine ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("PRICEHANDLING", (int) subcode.PriceHandling, SqlDbType.Int);
            statement.AddField("UNITOFMEASURE", (string) subcode.UnitOfMeasure);
            statement.AddField("QTYPERUNITOFMEASURE", subcode.QtyPerUnitOfMeasure, SqlDbType.Decimal);
            statement.AddField("INFOCODEPROMPT", subcode.InfocodePrompt);
            statement.AddField("MAXSELECTION", subcode.MaxSelection, SqlDbType.Int);
            statement.AddField("SERIALLOTNEEDED", subcode.SerialLotNeeded ? 1 : 0, SqlDbType.TinyInt);
            statement.AddField("USAGECATEGORY", (int) subcode.UsageCategory, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
           return SubcodeIDExists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "INFOCODESUBCODE"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOINFORMATIONSUBCODETABLE", "SUBCODEID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
