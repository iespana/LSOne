using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Units
{
    /// <summary>
    /// A data provider class for a unit conversion object.
    /// </summary>
    public class UnitConversionData : SqlServerDataProviderBase, IUnitConversionData
    {
        private const string BaseSqlString = "Select uc.FROMUNIT, uc.TOUNIT, uc.ITEMID, ISNULL( uc.FACTOR,0) as FACTOR, ISNULL(uc.MARKUP, 0) as MARKUP, ISNULL(it.ITEMNAME, '') as ITEMNAME, " +
                                                "ISNULL(u1.TXT,'') as FROMUNITNAME, ISNULL(u2.TXT,'') as TOUNITNAME " +
                                                "From UNITCONVERT uc " +
                                                "left outer join Unit u1 on u1.DATAAREAID = uc.DATAAREAID and u1.UNITID = uc.FROMUNIT " +
                                                "left outer join Unit u2 on u2.DATAAREAID = uc.DATAAREAID and u2.UNITID = uc.TOUNIT " +
                                                "left outer join  retailitem it on it.ITEMID = uc.ITEMID ";

        private const string BaseSqlString2 = "UNION Select  @unitID,  @unitID, '',1,0,'',ISNULL(un.TXT,'') as FROMUNITNAME, ISNULL(un.TXT,'') as TOUNITNAME " +
                                                "From UNIT un WHERE un.DATAAREAID = @dataAreaID and un.UNITID = @unitID ";

        private static void PopulateUnitConversion(IDataReader dr, UnitConversion unitConversion)
        {
            unitConversion.ItemID = (string)dr["ITEMID"];
            unitConversion.ItemName = (string)dr["ITEMNAME"];
            unitConversion.FromUnitID = (string)dr["FROMUNIT"];
            unitConversion.FromUnitName = (string)dr["FROMUNITNAME"];
            unitConversion.ToUnitID = (string)dr["TOUNIT"];
            unitConversion.ToUnitName = (string)dr["TOUNITNAME"];
            unitConversion.Factor = (decimal)dr["FACTOR"];
            unitConversion.Markup = (decimal)dr["MARKUP"];
        }

        /// <summary>
        /// Gets a unit conversion object with a given ID. It first searches for a rule converting from the FromUnit to the ToUnit in the unitconversionID, 
        /// and then searches for a rule converting from the ToUnit to the FromUnit in the unitconversionID. It returns the first not null result.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitConversionID">The ID of the unit convertion object to fetch</param>
        /// <returns>A unit conversion object with a given ID</returns>
        public virtual UnitConversion Get(IConnectionManager entry, RecordIdentifier unitConversionID)
        {
            var itemID = (string)unitConversionID.PrimaryID;
            var fromUnitID = (string)unitConversionID.SecondaryID.PrimaryID;
            var toUnit = (string)unitConversionID.SecondaryID.SecondaryID;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                // check to see if there is a rule from the FromUnit to the ToUnit
                cmd.CommandText =
                    BaseSqlString +
                    "Where uc.FROMUNIT = @fromUnit and uc.TOUNIT = @toUnit and uc.ITEMID = @itemId and uc.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemId", itemID);
                MakeParam(cmd, "fromUnit", fromUnitID);
                MakeParam(cmd, "toUnit", toUnit);

                var unitConversions = Execute<UnitConversion>(entry, cmd, CommandType.Text, PopulateUnitConversion);

                if (unitConversions.Count > 0)
                {
                    return unitConversions.Count > 0 ? unitConversions[0] : null;
                }
            }

            // check to see if there is a rule from the ToUnit to the FromUnit
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSqlString +
                    "Where uc.FROMUNIT = @toUnit and uc.TOUNIT = @fromUnit and uc.ITEMID = @itemId and uc.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemId", itemID);
                MakeParam(cmd, "fromUnit", fromUnitID);
                MakeParam(cmd, "toUnit", toUnit);

                var unitConversions = Execute<UnitConversion>(entry, cmd, CommandType.Text, PopulateUnitConversion);

                if (unitConversions.Count > 0)
                {
                    // We are retrieving a conversion in the opposite direction to what we planned, so we have to reverse the factor as well.
                    unitConversions[0].Factor = 1 / unitConversions[0].Factor;
                    return unitConversions[0];
                }
                if (itemID != "") // Avoiding infinite recursion with this if check
                {
                    // Set the itemID to empty string which means we are searching for a global unit conversion rule
                    itemID = "";
                    var newUnitConversionID = new RecordIdentifier(itemID,new RecordIdentifier(fromUnitID, toUnit));
                    return Get(entry, newUnitConversionID);
                }
                return unitConversions.Count > 0 ? unitConversions[0] : null;
            }
        }

        /// <summary>
        /// Gets all general unit conversion objects. General means they are not tied to an item. 
        /// The objects are ordered by the FromUnit field.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortColumn">The index (measured by the UnitConverionView columns) to sort by</param>
        /// <param name="backwardsSort">Whether to sort the result backwards or not</param>
        /// <returns>A list of unit conversion objects meeting the above criteria</returns>
        public virtual List<UnitConversion> GetUnitConversions(IConnectionManager entry, int sortColumn, bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                string[] columns = { "ITEMID", "FROMUNIT", "TOUNIT", "FACTOR", "MARKUP" };

                string sort = "";

                if (sortColumn < columns.Length)
                {
                    sort = " order by " + columns[sortColumn] + (backwardsSort ? " DESC" : " ASC");
                }

                cmd.CommandText =
                    BaseSqlString + "Where uc.DATAAREAID = @dataAreaId and uc.ITEMID = '' " + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<UnitConversion>(entry, cmd, CommandType.Text, PopulateUnitConversion);
            }
        }

        /// <summary>
        /// Gets all the unit conversion objects that convert an item with a given ID. 
        /// The objects are ordered by the FromUnit field.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the item to get unit conversion objects for</param>
        /// <param name="sortColumn">The index (measured by the UnitConverionView columns) to sort by</param>
        /// <param name="backwardsSort">Whether to sort the result backwards or not</param>
        /// <returns>All the unit conversion objects that convert an item with a given ID</returns>
        public virtual List<UnitConversion> GetUnitConversions(IConnectionManager entry, RecordIdentifier itemID, int sortColumn, bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                string[] columns = { "ITEMID", "FROMUNIT", "TOUNIT", "FACTOR", "MARKUP" };

                var sort = "";

                if (sortColumn < columns.Length)
                {
                    sort = " order by " + columns[sortColumn] + (backwardsSort ? " DESC" : " ASC");
                }

                cmd.CommandText =
                    BaseSqlString + "Where uc.DATAAREAID = @dataAreaId and uc.ITEMID = @itemId" + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemId", (string)itemID);

                return Execute<UnitConversion>(entry, cmd, CommandType.Text, PopulateUnitConversion);
            }
        }

        /// <summary>
        /// Gets the coversion rules that apply to an item, this includes the global rules. 
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <param name="itemID">ID of the item that the rules apply to.</param>
        /// <param name="unitID">The unit that is currently being used to measure the quatitiy of the item.</param>
        /// <returns></returns>
        public virtual List<UnitConversion> GetAllConversionsForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID)
        {
            var unitConversion = GetConversionsForItem(entry, itemID, unitID);
            // fetch where unitID is in either FromUnitID or in ToUnitID, but not if in both (that is only fetched for global)
            var usableRules = unitConversion.FindAll(f => (f.FromUnitID == unitID || f.ToUnitID == unitID) && (f.FromUnitID != f.ToUnitID));
            
            var globalRules = GetConversionsForItem(entry, "", unitID);
            usableRules.AddRange(globalRules.FindAll(f => f.FromUnitID == unitID || f.ToUnitID == unitID));
                        
            return usableRules;
        }

        public virtual List<UnitConversion> GetConversionsForItem(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSqlString + @"WHERE uc.DATAAREAID = @dataAreaID AND uc.ITEMID = @itemID AND (uc.FROMUNIT = @unitID OR uc.TOUNIT = @unitID)";
                cmd.CommandText += BaseSqlString2;
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", itemID);
                MakeParam(cmd, "unitID", unitID);
                return Execute<UnitConversion>(entry, cmd, CommandType.Text, PopulateUnitConversion);
            }
        }

        /// <summary>
        /// Deletes a unit conversion object with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitConversionID">The ID of the unit conversion object to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier unitConversionID)
        {
            DeleteRecord(entry, "UNITCONVERT", new[] { "ITEMID", "FROMUNIT", "TOUNIT" }, unitConversionID, BusinessObjects.Permission.ManageRetailGroups);
        }

        /// <summary>
        /// Deletes all unit conversion rules from database by performing a TRUNCATE on UNITCONVERT table.
        /// </summary>
        /// <param name="entry"></param>
        public virtual void DeleteAll(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "TRUNCATE TABLE UNITCONVERT";

                entry.Connection.ExecuteNonQuery(cmd, false, CommandType.Text);
            }
        }

        /// <summary>
        /// Check whether a unit conversion object with a given ID exists.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitConversionID">The ID of the unit conversion to check for</param>
        /// <returns>Whether the unit conversion object with the given ID exists or not</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier unitConversionID)
        {
            var forwardConversionExists = RecordExists(entry, "UNITCONVERT", new[] { "ITEMID", "FROMUNIT", "TOUNIT" }, unitConversionID);
            var backwardsConversionExists = RecordExists(entry, "UNITCONVERT", new[] { "ITEMID", "TOUNIT", "FROMUNIT" }, unitConversionID);
            return (forwardConversionExists || backwardsConversionExists);
        }

        /// <summary>
        /// Saves a unit conversion object to the data base.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitConversion">The unit conversion object to save</param>
        public virtual void Save(IConnectionManager entry, UnitConversion unitConversion)
        {
            var statement = new SqlServerStatement("UNITCONVERT");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageUnits);

            if (!Exists(entry, unitConversion.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ITEMID", (string)unitConversion.ItemID);
                statement.AddKey("FROMUNIT", (string)unitConversion.FromUnitID);
                statement.AddKey("TOUNIT", (string)unitConversion.ToUnitID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ITEMID", (string)unitConversion.ItemID);
                statement.AddCondition("FROMUNIT", (string)unitConversion.FromUnitID);
                statement.AddCondition("TOUNIT", (string)unitConversion.ToUnitID);
            }

            statement.AddField("FACTOR", unitConversion.Factor, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Whether a unit conversion rule exists between two units for a specific item.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The item to check for. Passing an empty string here checks whether a general conversion rule exists between the two units</param>
        /// <param name="fromUnitID">The ID of the unit to convert from</param>
        /// <param name="toUnitID">The ID of the unit to convert to</param>
        /// <returns>Whether a unit conversion rule exists between two units for a specific item.</returns>
        public virtual bool UnitConversionRuleExists(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier fromUnitID, RecordIdentifier toUnitID)
        {
            var list = GetConvertableTo(entry, itemID, toUnitID);

            foreach (Unit item in list)
            {
                if (item.ID == fromUnitID.PrimaryID) //We check primary ID because there are some cases when the Secondary ID is populated with the conversion unit i.e. the ToUnitID
                {
                    return true;
                }
            }

            return false;
        }

        private static void PopulateUnit(IDataReader dr, Unit unit)
        {
            unit.ID = (string)dr["UNITID"];
            unit.Text = (string)dr["Description"];
            unit.MaximumDecimals = (int)dr["UNITDECIMALS"];
            unit.MinimumDecimals = (int)dr["MINUNITDECIMALS"];
        }

        /// <summary>
        /// Gets a list of DataEntities containing unitID and unit descriptions for units that are convertable to a unit with a given unit ID and for a given retail item ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The item who's unit conversions we are dealing with</param>
        /// <param name="unitID">The unit id of the unit that the returned unit data entities are convertable to</param>
        /// <returns></returns>
        public List<Unit> GetConvertableTo(IConnectionManager entry, RecordIdentifier itemID,
                                                  RecordIdentifier unitID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT UNITID,ISNULL(TXT,'') as Description, ISNULL(UNITDECIMALS,0) as UNITDECIMALS, ISNULL(MINUNITDECIMALS,0) as MINUNITDECIMALS from UNIT " +
                    "where UNITID = @unitID and DATAAREAID = @dataAreaID " +
                    "union " +
                    "SELECT a.TOUNIT as UNITID,ISNULL(b.TXT,'') as Description, ISNULL(b.UNITDECIMALS,0) as UNITDECIMALS, ISNULL(b.MINUNITDECIMALS,0) as MINUNITDECIMALS " +
                    "from UNITCONVERT a " +
                    "left outer join UNIT b on a.TOUNIT = b.UNITID and a.DATAAREAID = b.DATAAREAID " +
                    "where a.DATAAREAID = @dataAreaID and ((a.ITEMID = @itemId or a.ITEMID = '') and a.FromUnit = @unitID) " +
                    "union " +
                    "SELECT a.FROMUNIT as UNITID ,ISNULL(b.TXT,'') as Description, ISNULL(b.UNITDECIMALS,0) as UNITDECIMALS, ISNULL(b.MINUNITDECIMALS,0) as MINUNITDECIMALS " +
                    "from UNITCONVERT a " +
                    "left outer join UNIT b on a.FROMUNIT = b.UNITID and a.DATAAREAID = b.DATAAREAID " +
                    "where a.DATAAREAID = @dataAreaID and ((a.ITEMID = @itemId or a.ITEMID = '') and a.ToUnit = @unitID)";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemId", (string)itemID);
                MakeParam(cmd, "unitID", (string)unitID);

                return Execute<Unit>(entry, cmd, CommandType.Text, PopulateUnit);
            }
        }

        /// <summary>
        /// Returns the factor for a unit. It is able to fallback to another rule if it exists in the other direction.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitFrom">The unit that is to be changed</param>
        /// <param name="unitTo">The unit that is to be changed to</param>
        /// <param name="itemID">The units that this factor applies to</param>
        /// <returns></returns>
        public virtual decimal GetUnitOfMeasureConversionFactor(IConnectionManager entry, RecordIdentifier unitFrom, RecordIdentifier unitTo, RecordIdentifier itemID)
        {
            //Missing identifier or conversion between the same unit.
            if ((unitTo == "") || (unitFrom == "") || (unitTo == unitFrom))
            {
                return 1;
            }
            //Getting the factor for in the correct direction.
            var unitConversionIdentifier = new RecordIdentifier(itemID, new RecordIdentifier(unitFrom, unitTo));
            var unitConversion = Get(entry, unitConversionIdentifier);
            if (unitConversion != null)
            {
                return unitConversion.Factor;
            }
            //Getting the factor in the other direction.
            unitConversionIdentifier = new RecordIdentifier(itemID, new RecordIdentifier(unitTo, unitFrom));
            unitConversion = Get(entry, unitConversionIdentifier);
            if (unitConversion != null)
            {
                return 1 / unitConversion.Factor;
            }
            //Going for global setting of those parameters.
            if (itemID != "")
            {
                return GetUnitOfMeasureConversionFactor(entry, unitFrom, unitTo, ""); //Getting global settings
            }

            //Nothing found.
            return 1;
        }

        /// <summary>
        /// Used when creating conversion rules so you cannot for example create a unit conversion rule to prevent that you can make rule that says 1 cm = 3 cm. 
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">ID of the item</param>
        /// <param name="unitID">The unit to convert to</param>
        /// <returns></returns>
        public virtual List<DataEntity> GetConvertableToWithoutCurrentUnit(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    //"SELECT UNITID,TXT from UNIT " +
                    //"where UNITID = @unitID and DATAAREAID = @dataAreaID " +
                    //"union " +
                    "SELECT a.TOUNIT as UNITID,b.TXT " +
                    "from UNITCONVERT a " +
                    "left outer join UNIT b on a.TOUNIT = b.UNITID and a.DATAAREAID = b.DATAAREAID " +
                    "where a.DATAAREAID = @dataAreaID and ((a.ITEMID = @itemId or a.ITEMID = '') and a.FromUnit = @unitID) " +
                    "union " +
                    "SELECT a.FROMUNIT as UNITID ,b.TXT " +
                    "from UNITCONVERT a " +
                    "left outer join UNIT b on a.FROMUNIT = b.UNITID and a.DATAAREAID = b.DATAAREAID " +
                    "where a.DATAAREAID = @dataAreaID and ((a.ITEMID = @itemId or a.ITEMID = '') and a.ToUnit = @unitID)";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", (string)itemID);
                MakeParam(cmd, "unitID", (string)unitID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "TXT", "UNITID");
            }
        }

        /// <summary>
        /// Converts some quantity of an item from one unit to another using unit conversion rules. If a unit conversion rule does not exist,
        /// this function returns 0
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The items who's quantity we are changing</param>
        /// <param name="fromUnitID">The unit ID to convert from</param>
        /// <param name="toUnitID">The unit ID to convert to</param>
        /// <param name="originalQty">The original quantity of the item</param>
        /// <returns>The new quantity or 0 if a unit conversion rule does not exist</returns>
        public decimal ConvertQtyBetweenUnits(
            IConnectionManager entry,
            RecordIdentifier itemID,
            RecordIdentifier fromUnitID,
            RecordIdentifier toUnitID,
            decimal originalQty)
        {
            var unitConversionID = new RecordIdentifier(itemID, new RecordIdentifier(fromUnitID, toUnitID));

            if (fromUnitID == toUnitID)
            {
                return originalQty;
            }

            var unitConversion = Get(entry, unitConversionID);
            if (unitConversion != null)
                return originalQty / unitConversion.Factor;
            return 1;
        }

        /// <summary>
        /// Creates an inverted rule for a given rule.
        /// </summary>
        /// <param name="rule">Orginal rule.</param>
        /// <returns></returns>
        public virtual UnitConversion ReverseRule(UnitConversion rule)
        {
            var unitConversion = new UnitConversion
            {
                ItemName = rule.ItemName,
                ItemID = rule.ItemID,
                ToUnitID = rule.FromUnitID,
                ToUnitName = rule.FromUnitName,
                FromUnitID = rule.ToUnitID,
                FromUnitName = rule.ToUnitName,
                UsageIntent = rule.UsageIntent,
                Text = rule.Text,
                Factor = 1 / rule.Factor,
                ID = new RecordIdentifier(rule.ItemID, new RecordIdentifier(rule.ToUnitID, rule.FromUnitID))
            };
            return unitConversion;

        }

        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
        /// <returns>List of items</returns>
        public virtual List<UnitConversion> GetCompareList(IConnectionManager entry, List<UnitConversion> itemsToCompare)
        {
            DataPopulator<UnitConversion> populator = (dr, unitConversion) =>
            {
                unitConversion.ItemID = (string)dr["ITEMID"];
                unitConversion.FromUnitID = (string)dr["FROMUNIT"];
                unitConversion.ToUnitID = (string)dr["TOUNIT"];
                unitConversion.Factor = (decimal)dr["FACTOR"];
                unitConversion.Markup = (decimal)dr["MARKUP"];
            };

            var columns = new List<TableColumn>
            {
                new TableColumn {ColumnName = "ITEMID", TableAlias = "U"},
                new TableColumn {ColumnName = "FROMUNIT", TableAlias = "U"},
                new TableColumn {ColumnName = "TOUNIT", TableAlias = "U"},
                new TableColumn {ColumnName = "FACTOR", IsNull = true, NullValue = "0"},
                new TableColumn {ColumnName = "MARKUP", IsNull = true, NullValue = "0"},
            };

            return GetCompareListInBatches(entry, itemsToCompare, "UNITCONVERT", new string [] { "ITEMID", "FROMUNIT", "TOUNIT" }, columns, null, populator);
        }
    }
}