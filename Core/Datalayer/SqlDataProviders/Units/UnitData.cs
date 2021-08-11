using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Units;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.SqlDataProviders.Units
{
    /// <summary>
    /// A data provider class for a unit.
    /// </summary>
    public class UnitData : SqlServerDataProviderBase, IUnitData
    {
        private static string ResolveSort(UnitSorting sort, bool backwards)
        {
            var sortString = "";

            switch (sort)
            {
                case UnitSorting.ID:
                    sortString = "u.UNITID DESC";
                    break;
                case UnitSorting.Description:
                    sortString = "TXT DESC";
                    break;
                case UnitSorting.MinimumDecimals:
                    sortString = "MINUNITDECIMALS DESC";
                    break;
                case UnitSorting.MaximumDecimals:
                    sortString = "UNITDECIMALS DESC";
                    break;
            }

            if (backwards)
            {
                sortString = sortString.Replace("DESC", "ASC");
            }

            return sortString;
        }

        private static string GetUnitType(UnitTypeEnum unitTypeEnum)
        {
            var unitType = "";

            switch (unitTypeEnum)
            {
                case UnitTypeEnum.SalesUnit:
                    unitType = "itm.SALESUNITID";
                    break;
                case UnitTypeEnum.InventoryUnit:
                    unitType = "itm.INVENTORYUNITID";
                    break;
                case UnitTypeEnum.PurchaseUnit:
                    unitType = "itm.PURCHASEUNITID";
                    break;
            }

            return unitType;
        }

        private static void PopulateUnit(IDataReader dr, Unit unit)
        {
            unit.ID = (string)dr["UNITID"];
            unit.Text = (string)dr["Description"];
            unit.MaximumDecimals = (int)dr["UNITDECIMALS"];
            unit.MinimumDecimals = (int)dr["MINUNITDECIMALS"];
        }

        /// <summary>
        /// Gets a unit with a given unit ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitID">The ID of the unit to fetch</param>
        /// <param name="cacheType">Optional parameter to specify if cache may be used</param>
        /// <returns>A unit with a given id</returns>
        public virtual Unit Get(IConnectionManager entry, RecordIdentifier unitID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select UNITID, ISNULL(TXT,'') as Description, ISNULL(UNITDECIMALS,0) as UNITDECIMALS,ISNULL(MINUNITDECIMALS,0) as MINUNITDECIMALS " +
                    "From UNIT " +
                    "Where DATAAREAID = @dataAreaId and UNITID = @unitId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "unitId", (string)unitID);

                return Get<Unit>(entry, cmd, unitID, PopulateUnit, cacheType, UsageIntentEnum.Normal);
            }
        }

        public virtual List<Unit> GetAllUnits(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select UNITID,ISNULL(TXT,'') as Description,ISNULL(UNITDECIMALS,0) as UNITDECIMALS, ISNULL(MINUNITDECIMALS,0) as MINUNITDECIMALS " +
                    "from UNIT " +
                    "where DATAAREAID = @dataAreaId " +
                    "order by Description";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<Unit>(entry, cmd, CommandType.Text, PopulateUnit);
            }
        }

        /// <summary>
        /// Gets all units, sorted by a specified column
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortEnum">Enum defining how to sort the results.</param>
        /// <param name="backwardsSort">Whether to sort backwards or not</param>
        /// <returns>A list of units, meeting the above criteria</returns>
        public virtual List<Unit> GetUnits(IConnectionManager entry, UnitSorting sortEnum, bool backwardsSort)
        {
            return GetUnitForItem(entry, RecordIdentifier.Empty, sortEnum, backwardsSort, UnitTypeEnum.InventoryUnit);        
        }
        
        /// <summary>
        /// Gets all units that a specific item is convertable to or from
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the unit. Insert RecordIdentifier.Empty to get all units in the system</param>
        /// <param name="sortEnum">Enum defining how to sort the results.</param>
        /// <param name="backwardsSort">Whether to sort backwards or not</param>
        /// <returns>A list of units, meeting the above criteria</returns>
        public virtual List<Unit> GetUnitForItem(IConnectionManager entry, RecordIdentifier itemID, UnitSorting sortEnum, bool backwardsSort, UnitTypeEnum unitType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                var sort = ResolveSort(sortEnum, backwardsSort);
                var unit = GetUnitType(unitType);
                cmd.CommandText =
                    "Select u.UNITID, ISNULL(u.TXT,'') as Description, ISNULL(u.UNITDECIMALS,0) as UNITDECIMALS, ISNULL(u.MINUNITDECIMALS,0) as MINUNITDECIMALS ";
                if (itemID.IsGuid)
                {
                    itemID = GetReadableIDFromMasterID(entry, itemID, "RETAILITEM", "ITEMID");
                }

                if (itemID == RecordIdentifier.Empty)
                {
                    cmd.CommandText += "from UNIT u ";
                    cmd.CommandText += "Order by " + sort;
                }
                else
                {
                    cmd.CommandText += "from retailitem itm  " +
                    "left outer join UNIT u on  " +
                    unit + " = u.UNITID  " +
                    "where  " +
                    "itm.ITEMID = @itemID and  " +
                    "u.UNITID is NOT NULL " +
                    "Order by  " + sort;

                    MakeParam(cmd, "itemID", (string)itemID.PrimaryID);
                }

                return Execute<Unit>(entry, cmd, CommandType.Text, PopulateUnit);
            }
        }

        public List<Unit> GetUnitsForItem(IConnectionManager entry, RecordIdentifier itemID, UnitSorting sortEnum, bool backwardsSort)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                var sort = ResolveSort(sortEnum, backwardsSort);

                string select = "Select u.UNITID, ISNULL(u.TXT,'') as Description, ISNULL(u.UNITDECIMALS,0) as UNITDECIMALS, ISNULL(u.MINUNITDECIMALS,0) as MINUNITDECIMALS ";
                if (itemID.IsGuid)
                {
                    itemID = GetReadableIDFromMasterID(entry, itemID, "RETAILITEM", "ITEMID");
                }
              
                cmd.CommandText = select + 
                                  @"from UNIT u
                                  join RETAILITEM r on r.INVENTORYUNITID = u.UNITID
                                  where r.ITEMID = @itemID
                                  union "
                                  + select +
                                  @"from UNIT u
                                  join RETAILITEM r on r.PURCHASEUNITID = u.UNITID
                                  where r.ITEMID = @itemID
                                  union "
                                  + select +
                                  @"from UNIT u
                                  join RETAILITEM r on r.SALESUNITID = u.UNITID
                                  where r.ITEMID = @itemID  
                                  order by " + sort;

                MakeParam(cmd, "itemID", (string)itemID.PrimaryID);

                return Execute<Unit>(entry, cmd, CommandType.Text, PopulateUnit);
            }
        }

        /// <summary>
        /// Gets a list of data entities containing ids and descriptions of units 
        /// The data entities are ordered by the units description
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities containing ids and descriptions of units</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "UNIT", "TXT", "UNITID", "TXT");
        }

        /// <summary>
        /// Deletes a unit with a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitID">The ID of the unit to delete</param>
        public virtual bool Delete(IConnectionManager entry, RecordIdentifier unitID)
        {
            if (UnitInUse(entry, unitID))
            {
                return false;
            }

            DeleteRecord<Unit>(entry, "UNIT", "UNITID", unitID, Permission.ManageUnits);

            return true;
        }

        /// <summary>
        /// Checks whether a unit with a given ID exists in the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unitID">The ID of the unit to delete</param>
        /// <returns>Whether the unit exists or not</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier unitID)
        {
            return RecordExists<Unit>(entry, "UNIT", "UNITID", unitID);
        }

        /// <summary>
        /// Returns true if a unit with the given description exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="description">The unit description to look for</param>
        /// <returns></returns>
        public virtual bool UnitDescriptionExists(IConnectionManager entry, string description)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"select COUNT(*)
                                    from UNIT 
                                    where lower(TXT) = lower(@description)";

                MakeParam(cmd, "description", description);

                return (int)entry.Connection.ExecuteScalar(cmd) > 0;
            }
        }

        /// <summary>
        /// Returns the unit ID given the description
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="description">The unit description to look for</param>
        /// <returns></returns>
        public virtual RecordIdentifier GetIdFromDescription(IConnectionManager entry, string description)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = "Select UNITID,ISNULL(TXT,'') as Description,ISNULL(UNITDECIMALS,0) as UNITDECIMALS, ISNULL(MINUNITDECIMALS,0) as MINUNITDECIMALS " +
                    "from UNIT " +
                    "where TXT = @DESCRIPTION";

                MakeParam(cmd, "DESCRIPTION", description);

                return Execute<Unit>(entry, cmd, CommandType.Text, PopulateUnit).FirstOrDefault()?.ID;
            }
        }

        /// <summary>
        /// Saves a unit object to the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="unit">The unit to save</param>
        public virtual void Save(IConnectionManager entry,Unit unit)
        {
            var statement = new SqlServerStatement("UNIT");

            ValidateSecurity(entry, Permission.ManageUnits);

            bool isNew = false;
            if (unit.ID.IsEmpty)
            {
                isNew = true;
                unit.ID = DataProviderFactory.Instance.GenerateNumber<IUnitData, Unit>(entry);
            }

            if (isNew || !Exists(entry, unit.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("UNITID", (string)unit.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("UNITID", (string)unit.ID);
            }

            statement.AddField("TXT", unit.Text);
            statement.AddField("UNITDECIMALS", unit.MaximumDecimals, SqlDbType.Int);
            statement.AddField("MINUNITDECIMALS", unit.MinimumDecimals, SqlDbType.Int);

            Save(entry, unit, statement);
        }

        /// <summary>
        /// Gets the decimal limiter for a given unit ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="unitID">The ID of the unit</param>
        /// <returns>The decimal limiter for the given unit or a default limiter if the unit is not found</returns>
        public virtual DecimalLimit GetNumberLimitForUnit(IConnectionManager entry, RecordIdentifier unitID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            Unit unit = null;

            if (!RecordIdentifier.IsEmptyOrNull(unitID))
            {
                unit = Get(entry, unitID, cacheType);
            }

            if (unit != null)
            {
                return new DecimalLimit(unit.MinimumDecimals, unit.MaximumDecimals);
            }
            return entry.GetDecimalSetting(DecimalSettingEnum.Quantity);
        }

        /// <summary>
        /// Returns true if the quantity is allowed for the specific unit.
        /// E.g. it is illegal to sell 4,6 bottles of coke but allright for metric goods.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="qty">Quantity that we are checking if is ok</param>
        /// <param name="unitID">ID of the unit object we are checking against</param>
        /// <returns>If the given quantity is allowed for the given unit</returns>
        public virtual bool IsQuantityAllowed(IConnectionManager entry, decimal qty, RecordIdentifier unitID)
        {
            // If the unit of measure is not defined then we cannot determine if the qty is allowed.
            if (unitID == "")
                return true;

            decimal modResult = qty % 1m;
            if (modResult == 0)
            {
                // Qty is a whole unit, e.g. 2,00
                // We can always sell whole units, so this is allowed
                return true;
            }
            
            // Qty is a broken unit, e.g. 5,70
            var unit = Get(entry, unitID, CacheType.CacheTypeApplicationLifeTime);
            if (unit == null)
            {
                return true;
            }

            // Find the correct comma char for the location.
            var commaChar = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            var tempStr = Convert.ToString(qty.Normalize());
            var commaIndex = (tempStr.IndexOf(commaChar) + 1); // the first numbers plus decimal point

            int qtyDecimals = 0;
            if (commaIndex > 0)
            {
                qtyDecimals = ((tempStr.Length) - commaIndex);     //total length minus beginning numbers and decimal = number of decimal points 
            }

            if (unit.MaximumDecimals < qtyDecimals)
            {
                return false;
            }

            return true;
        }

        private static bool UnitInUse(IConnectionManager entry, RecordIdentifier unitID)
        {
            return RecordExists(entry, "RETAILITEM", "INVENTORYUNITID", unitID, false) ||
                   RecordExists(entry, "RETAILITEM", "PURCHASEUNITID", unitID, false) ||
                   RecordExists(entry, "RETAILITEM", "SALESUNITID", unitID, false) ||
                   RecordExists(entry, "UNITCONVERT", "TOUNIT", unitID) ||
                   RecordExists(entry, "UNITCONVERT", "FROMUNIT", unitID) ||
                   RecordExists(entry, "INVENTITEMBARCODE", "UNITID", unitID) ||
                   RecordExists(entry, "RBOINVENTLINKEDITEM", "UNIT", unitID) ||
                   RecordExists(entry, "PRICEDISCTABLE", "UNITID", unitID) ||
                   RecordExists(entry, "VENDORITEMS", "UNITID", unitID) ||
                   RecordExists(entry, "PURCHASEORDERLINE", "UNITID", unitID) ||
                   RecordExists(entry, "INVENTJOURNALTRANS", "UNITID", unitID) ||
                   RecordExists(entry, "INVENTORYTRANSFERORDERLINE", "UNITID", unitID) ||
                   RecordExists(entry, "INVENTORYTRANSFERREQUESTLINE", "UNITID", unitID) ||
                   RecordExists(entry, "INVENTTRANS", "UNITID", unitID) ||
                   RecordExists(entry, "PURCHASEWORKSHEETLINE", "UNITID", unitID) ||
                   RecordExists(entry, "RBOINFOCODETABLESPECIFIC", "UNITOFMEASURE", unitID) ||
                   RecordExists(entry, "RBOINFORMATIONSUBCODETABLE", "UNITOFMEASURE", unitID);
        }

        public virtual string GetUnitDescription(IConnectionManager entry, RecordIdentifier unitId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select UNITID, ISNULL(TXT,'') as Description, ISNULL(UNITDECIMALS,0) as UNITDECIMALS,ISNULL(MINUNITDECIMALS,0) as MINUNITDECIMALS " +
                                    "From UNIT " +
                                    "Where DATAAREAID = @dataAreaId and UNITID = @unitId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "unitId", (string)unitId);

                var list = Execute<Unit>(entry, cmd, CommandType.Text, PopulateUnit);

                return (list.Count > 0) ? list[0].Text : "";
            }
        }
        
        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
        /// <returns>List of items</returns>
        public virtual List<Unit> GetCompareList(IConnectionManager entry, List<Unit> itemsToCompare)
        {
            var columns = new List<TableColumn>
            {
                new TableColumn { ColumnName = "UNITID", TableAlias = "U"},
                new TableColumn { ColumnName = "TXT", ColumnAlias = "Description", TableAlias = "U"},
                new TableColumn { ColumnName = "UNITDECIMALS", IsNull = true, NullValue = "0" },
                new TableColumn { ColumnName = "MINUNITDECIMALS", IsNull = true, NullValue = "0" },
            };

            return GetCompareListInBatches(entry, itemsToCompare, "UNIT", "UNITID", columns, null, PopulateUnit);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry,id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "UNITS"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "UNIT", "UNITID", sequenceFormat, startingRecord, numberOfRecords);
        }
        #endregion
    }
}