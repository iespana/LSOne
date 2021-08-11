using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

// This class will be renamed to RetailItemData when we are ready to switch !!!!

namespace LSOne.DataLayer.SqlDataProviders.ItemMaster
{
    /// <summary>
    /// Data provider class for a retail item.
    /// </summary>
    public partial class RetailItemDataNew : SqlServerDataProviderBase, IRetailItemDataNew
    {
        private Dictionary<ColumnPopulation, List<TableColumn>> selectionColumns = new Dictionary<ColumnPopulation, List<TableColumn>>();

        private Dictionary<ColumnPopulation, List<TableColumn>> SelectionColumns
        {
            get
            {
                if (selectionColumns.Count == 0)
                {
                    selectionColumns.Add(ColumnPopulation.IDOnly, new List<TableColumn>
                    {
                        new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
                        new TableColumn {ColumnName = "ITEMNAME", TableAlias = "A"}

                    });
                    selectionColumns.Add(ColumnPopulation.Simple, new List<TableColumn>
                    {
                        new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
                        new TableColumn {ColumnName = "ITEMNAME", TableAlias = "A"},
                        new TableColumn {ColumnName = "ITEMTYPE", TableAlias = "A"},
                        new TableColumn {ColumnName = "NAMEALIAS", TableAlias = "A"},
                        new TableColumn {ColumnName = "RETAILGROUPID", TableAlias = "A"},
                        new TableColumn {ColumnName = "RETAILDEPARTMENTID", TableAlias = "A"},
                        new TableColumn {ColumnName = "RETAILDIVISIONID", TableAlias = "A"},


                    });
                    selectionColumns.Add(ColumnPopulation.POS, new List<TableColumn>
                    {
                        new TableColumn {ColumnName = "ITEMID", TableAlias = "ss"},
                        new TableColumn {ColumnName = "ITEMNAME", TableAlias = "ss"}

                    });
                    selectionColumns.Add(ColumnPopulation.SiteManager, new List<TableColumn>
                    {
                        new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
                        new TableColumn {ColumnName = "ITEMNAME", TableAlias = "A"},
                        new TableColumn {ColumnName = "ITEMTYPE", TableAlias = "A"},
                        new TableColumn {ColumnName = "NAMEALIAS", TableAlias = "A"},
                        new TableColumn {ColumnName = "RETAILGROUPID", TableAlias = "A"},
                        new TableColumn {ColumnName = "RETAILDEPARTMENTID", TableAlias = "A"},
                        new TableColumn {ColumnName = "RETAILDIVISIONID", TableAlias = "A"},
                        new TableColumn {ColumnName = "ITEMID", TableAlias = "A"},
                        new TableColumn {ColumnName = "MASTERID", TableAlias = "A"},
                        new TableColumn {ColumnName = "HEADERITEMID", TableAlias = "A"},
                        new TableColumn {ColumnName = "VARIANTNAME", TableAlias = "A"},
                        new TableColumn {ColumnName = "DEFAULTVENDORID", TableAlias = "A"},
                        new TableColumn
                        {
                            ColumnName = "ISNULL(A.EXTENDEDDESCRIPTION,'') ",
                            TableAlias = "",
                            ColumnAlias = "EXTENDEDDESCRIPTION"
                        },
                        new TableColumn {ColumnName = "ZEROPRICEVALID", TableAlias = "A"},
                        new TableColumn {ColumnName = "QTYBECOMESNEGATIVE", TableAlias = "A"},
                        new TableColumn {ColumnName = "NODISCOUNTALLOWED", TableAlias = "A"},
                        new TableColumn {ColumnName = "KEYINPRICE", TableAlias = "A"},
                        new TableColumn {ColumnName = "SCALEITEM", TableAlias = "A"},
                        new TableColumn {ColumnName = "KEYINQTY", TableAlias = "A"},
                        new TableColumn {ColumnName = "BLOCKEDONPOS", TableAlias = "A"},
                        new TableColumn {ColumnName = "BARCODESETUPID", TableAlias = "A"},
                        new TableColumn {ColumnName = "PRINTVARIANTSSHELFLABELS", TableAlias = "A"},
                        new TableColumn {ColumnName = "FUELITEM", TableAlias = "A"},
                        new TableColumn {ColumnName = "GRADEID", TableAlias = "A"},
                        new TableColumn {ColumnName = "MUSTKEYINCOMMENT", TableAlias = "A"},
                        new TableColumn {ColumnName = "DATETOBEBLOCKED", TableAlias = "A"},
                        new TableColumn {ColumnName = "DATETOACTIVATEITEM", TableAlias = "A"},
                        new TableColumn {ColumnName = "PROFITMARGIN", TableAlias = "A"},
                        new TableColumn {ColumnName = "VALIDATIONPERIODID", TableAlias = "A"},
                        new TableColumn {ColumnName = "MUSTSELECTUOM", TableAlias = "A"},
                        new TableColumn {ColumnName = "INVENTORYUNITID", TableAlias = "A"},
                        new TableColumn {ColumnName = "PURCHASEUNITID", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESUNITID", TableAlias = "A"},
                        new TableColumn {ColumnName = "PURCHASEPRICE", TableAlias = "A"},
                        new TableColumn {ColumnName = "PURCHASEPRICEINCLTAX", TableAlias = "A"},
                        new TableColumn {ColumnName = "PURCHASEPRICEUNIT", TableAlias = "A"},
                        new TableColumn {ColumnName = "PURCHASEMARKUP", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESPRICE", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESPRICEINCLTAX", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESPRICEUNIT", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESMARKUP", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESLINEDISC", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESMULTILINEDISC", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESALLOWTOTALDISCOUNT", TableAlias = "A"},
                        new TableColumn {ColumnName = "SALESTAXITEMGROUPID", TableAlias = "A"},
                        new TableColumn {ColumnName = "DELETED", TableAlias = "A"},
                        new TableColumn {ColumnName = "ISNULL(iir.NAME,'')", ColumnAlias = "RETAILGROUPNAME"},
                        new TableColumn {ColumnName = "ISNULL(iid.NAME,'')", ColumnAlias = "RETAILDEPARTMENTNAME"},
                        new TableColumn {ColumnName = "ISNULL(div.NAME,'')", ColumnAlias = "RETAILDIVISIONNAME"},

                    });
                }
                return selectionColumns;
            }
        }

        private List<Join> itemJoins = new List<Join>
        {
            new Join
            {
                Condition = " A.RETAILGROUPID = iir.GROUPID",
                JoinType = "LEFT OUTER",
                Table = "RBOINVENTITEMRETAILGROUP",
                TableAlias = "iir"
            },
            new Join
            {
                Condition = " a.RETAILDEPARTMENTID = iid.DEPARTMENTID",
                JoinType = "LEFT OUTER",
                Table = "RBOINVENTITEMDEPARTMENT",
                TableAlias = "IID"
            },
            new Join
            {
                Condition = " A.RETAILDIVISIONID = div.DIVISIONID",
                JoinType = "LEFT OUTER",
                Table = "RBOINVENTITEMRETAILDIVISION",
                TableAlias = "div"
            },
        };
                   


        /// <summary>
        /// Determines the type of information that is being viewed. Default value is Sales
        /// </summary>
        public enum ModuleTypeEnum
        {
            /// <summary>
            /// Information that have to do with inventory pricing, storage, units and etc. Not currently used
            /// </summary>
            Inventory = 0,
            /// <summary>
            /// Information that have to do with purchase pricing, units and etc. Not currently used
            /// </summary>
            Purchase = 1,
            /// <summary>
            /// Information that have to do with sale pricing, storage, units and etc.
            /// </summary>
            Sales = 2
        }

        protected virtual void PopulateItem(IDataReader dr, RetailItemNew item)
        {
            PopulateSimpleItem(dr, item);
            item.MasterID = (Guid) dr["MASTERID"];
            item.HeaderItemID = dr["HEADERITEMID"] == DBNull.Value ? Guid.Empty : (Guid) dr["HEADERITEMID"];
            item.VariantName = (string) dr["VARIANTNAME"];
            item.DefaultVendorID = (string) dr["DEFAULTVENDORID"];
            item.ExtendedDescription = (string) dr["EXTENDEDDESCRIPTION"];
            item.ZeroPriceValid = (bool) dr["ZEROPRICEVALID"];
            item.QuantityBecomesNegative = (bool) dr["QTYBECOMESNEGATIVE"];
            item.NoDiscountAllowed = (bool) dr["NODISCOUNTALLOWED"];
            item.KeyInPrice = (RetailItemNew.KeyInPriceEnum) (byte) dr["KEYINPRICE"];
            item.ScaleItem = (bool) dr["SCALEITEM"];
            item.KeyInQuantity = (RetailItemNew.KeyInQuantityEnum) (byte) dr["KEYINQTY"];
            item.BlockedOnPOS = (bool) dr["BLOCKEDONPOS"];
            item.BarCodeSetupID = (string) dr["BARCODESETUPID"];
            item.PrintVariantsShelfLabels = (bool) dr["PRINTVARIANTSSHELFLABELS"];
            item.IsFuelItem = (bool) dr["FUELITEM"];
            item.GradeID = (string) dr["GRADEID"];
            item.MustKeyInComment = (bool) dr["MUSTKEYINCOMMENT"];
            item.DateToBeBlocked = Date.FromAxaptaDate(dr["DATETOBEBLOCKED"]);
            item.DateToActivateItem = Date.FromAxaptaDate(dr["DATETOACTIVATEITEM"]);
            item.ProfitMargin = (decimal) dr["PROFITMARGIN"];
            item.ValidationPeriodID = (string) dr["VALIDATIONPERIODID"];
            item.MustSelectUOM = (bool) dr["MUSTSELECTUOM"];
            item.InventoryUnitID = (string) dr["INVENTORYUNITID"];
            item.PurchaseUnitID = (string) dr["PURCHASEUNITID"];
            item.SalesUnitID = (string) dr["SALESUNITID"];
            item.PurchasePrice = (decimal) dr["PURCHASEPRICE"];
            item.PurchasePriceIncludingTax = (decimal) dr["PURCHASEPRICEINCLTAX"];
            item.PurchasePriceUnit = (decimal) dr["PURCHASEPRICEUNIT"];
            item.PurchaseMarkup = (decimal) dr["PURCHASEMARKUP"];
            item.SalesPrice = (decimal) dr["SALESPRICE"];
            item.SalesPriceIncludingTax = (decimal) dr["SALESPRICEINCLTAX"];
            item.SalesPriceUnit = (decimal) dr["SALESPRICEUNIT"];
            item.SalesMarkup = (decimal) dr["SALESMARKUP"];
            item.SalesLineDiscount = (string) dr["SALESLINEDISC"];
            item.SalesMultiLineDiscount = (string) dr["SALESMULTILINEDISC"];
            item.SalesAllowTotalDiscount = (bool) dr["SALESALLOWTOTALDISCOUNT"];
            item.SalesTaxItemGroupID = (string) dr["SALESTAXITEMGROUPID"];
            item.Deleted = (bool) dr["DELETED"];
            item.RetailDepartmentName = (string) dr["RETAILDEPARTMENTNAME"];
            item.RetailGroupName = (string) dr["RETAILGROUPNAME"];
            item.RetailDivisionName = (string) dr["RETAILDIVISIONNAME"];
        }

        protected virtual void PopulateItemWithCount(IConnectionManager entry, IDataReader dr, RetailItemNew item, ref int rowCount)

        {
           PopulateItem(dr,item);
           PopulateRowCount(entry,dr,ref rowCount);

        }

        protected virtual void PopulateSimpleItem(IDataReader dr, SimpleRetailItem item)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];
            item.ItemType = (ItemTypeEnum)(byte)dr["ITEMTYPE"];
            item.NameAlias = (string)dr["NAMEALIAS"];
            item.RetailGroupID = (string)dr["RETAILGROUPID"];
            item.RetailDepartmentID = (string)dr["RETAILDEPARTMENTID"];
            item.RetailDivisionID = (string)dr["RETAILDIVISIONID"];

            
        }
        protected virtual void PopulateSimpleItemWithCount(IConnectionManager entry, IDataReader dr, SimpleRetailItem item, ref int rowCount)
        {
            PopulateSimpleItem(dr, item);
            PopulateRowCount(entry, dr, ref rowCount);

        }
       
        protected virtual void PopulateItemID(IDataReader dr, RecordIdentifier id)
        {
            id = (string)dr["ITEMID"];
        }
        protected virtual void PopulateItemIDWithCount(IConnectionManager entry, IDataReader dr, RecordIdentifier id, ref int rowCount)           
        {
            id = (string)dr["ITEMID"];
            PopulateRowCount(entry, dr, ref rowCount);

        }
        
        protected virtual void PopulateDataEntityWithCount(IConnectionManager entry, IDataReader dr, DataEntity item, ref int rowCount)
        {
            item.ID = (string)dr["ITEMID"];
            item.Text = (string)dr["ITEMNAME"];

            PopulateRowCount(entry, dr, ref rowCount);

        }
        
        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 || entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }
       
        /// <summary>
        /// Looks up the unit id for a item by a given id. The type of unit depends on the module type.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">Id of the retail item in the database</param>
        /// <param name="module">Module type enum which determines what type of unit id we are returning</param>
        /// <returns>The unit ID of an item depending on the module type</returns>
        public virtual RecordIdentifier GetItemUnitID(IConnectionManager entry, RecordIdentifier itemID, RetailItemNew.UnitTypeEnum module)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the unit information on a specific item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item to update</param>
        /// <param name="unitID">The new unit ID information</param>
        /// <param name="module">Which module information to update (inventory, purchase,
        /// sales)</param>
        public virtual void UpdateUnitID(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier unitID, RetailItemNew.UnitTypeEnum module)
        {
            //var item = Get(entry, itemID);
            //if (item[module].Unit != unitID)
            //{
            //    item[module].Unit = unitID;
            //    item[module].Dirty = true;
            //    Save(entry, item);
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="ItemListItem"/>
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchString">The text to search for. Searches in item name, in the ID field and the search alias field</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
        /// <param name="sort">A string defining the sort column</param>
        /// <returns>A list of found items</returns>
        public virtual List<SimpleRetailItem> Search(IConnectionManager entry, string searchString, int rowFrom,
            int rowTo, bool beginsWith, string sort)
        {

            string modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);
            int totalRecordsMatched;
            return AdvancedSearchOptimized<SimpleRetailItem>(entry, rowFrom, rowTo, sort, false, out totalRecordsMatched,
                ColumnPopulation.Simple, modifiedSearchString,beginsWith);
        }

        public virtual List<DataEntity> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, string culture, bool beginsWith)
        {
            string modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);
            int totalRecordsMatched;
            return AdvancedSearchOptimized<DataEntity>(entry, rowFrom, rowTo, string.Empty, false, out totalRecordsMatched,
                ColumnPopulation.DataEntity, modifiedSearchString, beginsWith, null, null, null, null, null, null, false, culture);

        }

        public List<RecordIdentifier> AdvancedSearchIDOnly(IConnectionManager entry,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier vendorID = null,
            string barCode = null,
            bool barCodeBeginsWith = true,
            RecordIdentifier specialGroup = null)
        {
            int totalRecordsMatching = 0;
            return AdvancedSearchOptimized<RecordIdentifier>(entry, 0, int.MaxValue, string.Empty, false, out totalRecordsMatching,
                 ColumnPopulation.IDOnly, idOrDescription,
                 idOrDescriptionBeginsWith, retailGroupID, retailDepartmentID, taxGroupID,
                 variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup);
        }

        public List<DataEntity> AdvancedSearchDataEntity(IConnectionManager entry,
            int rowFrom, 
            int rowTo,
            string idOrDescription,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier vendorID = null,
            string barCode = null,
            bool barCodeBeginsWith = true,
            RecordIdentifier specialGroup = null)
        {
            int totalRecordsMatching = 0;
            return AdvancedSearchOptimized<DataEntity>(entry, rowFrom, rowTo, string.Empty, false, out totalRecordsMatching,
                 ColumnPopulation.IDOnly, idOrDescription,
                 idOrDescriptionBeginsWith, retailGroupID, retailDepartmentID, taxGroupID,
                 variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup);
        }

        public List<SimpleRetailItem> AdvancedSearchOptimized(
            IConnectionManager entry, 
            int rowFrom, 
            int rowTo, 
            string sort, 
            out int totalRecordsMatching,
            string idOrDescription = null, 
            bool idOrDescriptionBeginsWith = true, 
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null, 
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null, 
            RecordIdentifier vendorID = null, 
            string barCode = null,
            bool barCodeBeginsWith = true, 
            RecordIdentifier specialGroup = null)
        {
            return AdvancedSearchOptimized<SimpleRetailItem>(entry, rowFrom, rowTo, sort, true, out totalRecordsMatching,
                ColumnPopulation.Simple, idOrDescription, idOrDescriptionBeginsWith, retailGroupID, retailDepartmentID,
                taxGroupID, variantGroupID, vendorID, barCode, barCodeBeginsWith, specialGroup);
        }
      
        

        private List<T> AdvancedSearchOptimized<T>(
            IConnectionManager entry,
            int rowFrom, 
            int rowTo, 
            string sort,
            bool doCount,
            out int totalRecordsMatching,
            ColumnPopulation populationMethod,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier vendorID = null,
            string barCode = null,
            bool barCodeBeginsWith = true,
            RecordIdentifier specialGroup = null,
            string culture = null)
        {
            List<TableColumn> columns = new List<TableColumn>();
            List<Condition> conditions = new List<Condition>();
            List<Join> joins = new List<Join>();
            List<Condition> externalConditions = new List<Condition>();

            using (var cmd = entry.Connection.CreateCommand())
            {
                foreach (var selectionColumn in SelectionColumns[populationMethod])
                {
                    columns.Add(selectionColumn);
                }
                if (doCount &&
                    (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                     entry.Connection.DatabaseVersion == ServerVersion.Unknown))
                {
                    columns.Add(new TableColumn
                    {
                        ColumnName = string.Format("ROW_NUMBER() OVER(order by {0})",string.IsNullOrEmpty(sort) ? "ITEMID" : sort),
                        ColumnAlias = "ROW"
                    });
                    columns.Add(new TableColumn
                    {
                        ColumnName = string.Format("COUNT(1) OVER ( ORDER BY {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )",string.IsNullOrEmpty(sort) ? "ITEMID" : sort),
                        ColumnAlias = "ROW_COUNT"
                    });
                    externalConditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ss.ROW between @rowFrom and @rowTo"
                    });
                    MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                    MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);
                }
                
                string commandText = QueryTemplates.PagingQuery("RETAILITEM", "A", "ss");

                if (idOrDescription != null && idOrDescription.Trim().Length > 0)
                {
                    string searchString = PreProcessSearchText(idOrDescription, true, idOrDescriptionBeginsWith);
                    conditions.Add(new Condition{Operator = "AND", ConditionValue = " (A.ITEMNAME Like @description or A.ITEMID Like @description or A.NAMEALIAS Like @description) "});
                    
                    MakeParam(cmd, "description", searchString);
                }
                if (retailGroupID != null)
                {
                    conditions.Add(new Condition{Operator = "AND", ConditionValue =  "A.RETAILGROUPID = @retailGroup "});

                    MakeParamNoCheck(cmd, "retailGroup", (string)retailGroupID);
                }

                if (retailDepartmentID != null)
                {
                     conditions.Add( new Condition{Operator = "AND", ConditionValue =  "A.RETAILDEPARTMENTID = @retailDepartment "});

                    MakeParamNoCheck(cmd, "retailDepartment", (string)retailDepartmentID);
                }

                if (taxGroupID != null)
                {
                    conditions.Add(new Condition{Operator = "AND", ConditionValue = "A.SALESTAXITEMGROUPID = @taxGroup "});

                    MakeParamNoCheck(cmd, "taxGroup", (string)taxGroupID);
                }

                if (variantGroupID != null)
                {
                    //whereConditions += " and it.DIMGROUPID = @variantGroupID ";

                    //MakeParamNoCheck(cmd, "variantGroupID", (string)variantGroupID);
                }

                if (vendorID != null)
                {
                    joins.Add(new Join { Condition = " VI.RETAILITEMID = IT.ITEMID AND VI.VENDORID = @vendorID", JoinType = "INNER", Table = "VENDORITEMS", TableAlias = "VI" });

                    MakeParamNoCheck(cmd, "vendorID", (string)vendorID);
                }

                if (barCode != null)
                {
                    joins.Add(new Join { Condition = "A.ITEMID = ibarcode.ITEMID", JoinType = "LEFT OUTER", Table = "INVENTITEMBARCODE", TableAlias = "ibarcode" });

                    barCode = (barCodeBeginsWith ? "" : "%") + barCode + "%";

                    conditions.Add(new Condition{Operator = "AND", ConditionValue = "ibarcode.ITEMBARCODE like @barCodeSearchString "});

                    MakeParamNoCheck(cmd, "barCodeSearchString", barCode);
                }

                if (specialGroup != null)
                {
                    joins.Add(new Join { Condition = "SG.ITEMID = A.ITEMID AND SG.GROUPID = @specialGroupID", JoinType = "INNER", Table = "RBOSPECIALGROUPITEMS", TableAlias = "SG" });

                    MakeParamNoCheck(cmd, "specialGroupID", (string)specialGroup);
                }
                if (variantGroupID != null)
                {
                    conditions.Add(new Condition{Operator = "AND", ConditionValue = " it.DIMGROUPID = @variantGroupID "});

                    MakeParamNoCheck(cmd, "variantGroupID", (string)variantGroupID);
                }
                if (culture != null && culture != "")
                {
                    joins.Add(new Join {Table = "RBOINVENTTRANSLATIONS", Condition = "tr.ITEMID = it.ITEMID and tr.DATAAREAID = it.DATAAREAID and tr.CULTURENAME = @culture", JoinType = "LEFT OUTER", TableAlias = "tr"});
                    MakeParamNoCheck(cmd, "culture", culture);
                    MakeParamNoCheck(cmd, "searchString", idOrDescription);
                    conditions.Add(new Condition { Operator = "OR", ConditionValue = " tr.DESCRIPTION like @searchString " });

                }

                cmd.CommandText = string.Format(commandText, 
                    QueryPartGenerator.ExternalColumnGenerator(columns,"ss"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.IsNullOrEmpty(sort) ? string.Empty : string.Format("ORDER BY {0}", sort));
              
               
                totalRecordsMatching = 0;

                switch (populationMethod)
                {
                    case ColumnPopulation.IDOnly:
                        var recordidentifiers = Execute<RecordIdentifier, int>(entry, cmd, CommandType.Text,
                            ref totalRecordsMatching,
                            PopulateItemIDWithCount);
                        return CollectionHelper.ForceConvertList<T, RecordIdentifier>(recordidentifiers);

                    case ColumnPopulation.POS:
                        throw new NotImplementedException();

                    case ColumnPopulation.Simple:
                        var simpleItems = Execute<SimpleRetailItem, int>(entry, cmd, CommandType.Text,
                            ref totalRecordsMatching,
                            PopulateSimpleItemWithCount);
                        return CollectionHelper.ForceConvertList<T, SimpleRetailItem>(simpleItems);

                    case ColumnPopulation.DataEntity:
                        var entities = Execute<DataEntity, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching,
                            PopulateDataEntityWithCount);
                        return CollectionHelper.ForceConvertList<T, DataEntity>(entities);

                }

            }
            return null;
        }
        
        public List<SimpleRetailItem> AdvancedSearch(IConnectionManager entry,
            int rowFrom, 
            int rowTo, 
            string sort,
            out int totalRecordsMatching,
            string idOrDescription = null,
            bool idOrDescriptionBeginsWith = true,
            RecordIdentifier retailGroupID = null,
            RecordIdentifier retailDepartmentID = null,
            RecordIdentifier taxGroupID = null,
            RecordIdentifier variantGroupID = null,
            RecordIdentifier vendorID = null,
            string barCode = null,
            bool barCodeBeginsWith = true,
            RecordIdentifier specialGroup = null)
        {
            return AdvancedSearchOptimized<SimpleRetailItem>(
                entry, 
                rowFrom, 
                rowTo, 
                sort, 
                true, 
                out totalRecordsMatching,
                ColumnPopulation.Simple, 
                idOrDescription,
                idOrDescriptionBeginsWith, 
                retailGroupID, 
                retailDepartmentID, 
                taxGroupID,
                variantGroupID, 
                vendorID, 
                barCode, 
                barCodeBeginsWith, 
                specialGroup);

        }

        /// <summary>
        /// Searched for retail items that contain a given search text, and returns the results as a List of <see cref="ItemListItem"/> . 
        /// Additionally this search function accepts a Dictionary with a combination of <see cref="RetailItemSearchEnum"/> and a string and resolves this list to add additional 
        /// search filters to the query
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchString">The item id or item name to search for</param>
        /// <param name="rowFrom"></param>
        /// <param name="rowTo"></param>
        /// <param name="beginsWith">Indicates wether you only want to search for items that begin with the given item name or item id</param>
        /// <param name="sort">The name of the column you want to sort by</param>
        /// <param name="advancedSearchParameters">Additional search parameters</param>
        /// <returns></returns>
        public virtual List<SimpleRetailItem> AdvancedSearch(
            IConnectionManager entry, 
            string searchString, 
            int rowFrom, 
            int rowTo, 
            bool beginsWith, 
            string sort, 
            Dictionary<RetailItemSearchEnum, string> advancedSearchParameters)
        {
            RecordIdentifier retailGroupID = null;
            RecordIdentifier retailDepartmentID = null;
            RecordIdentifier taxGroupID = null;
            RecordIdentifier variantGroupID = null;
            RecordIdentifier vendorID = null;
            string barCode = null;
            bool barCodeBeginsWith = true;
            RecordIdentifier specialGroup = null;

            // Go through the advanced search conditions
            foreach (var pair in advancedSearchParameters)
            {
                switch (pair.Key)
                {
                    case RetailItemSearchEnum.RetailGroup:

                        retailGroupID = pair.Value;
                        break;

                    case RetailItemSearchEnum.RetailDepartment:

                        retailDepartmentID = pair.Value;
                        break;

                    case RetailItemSearchEnum.TaxGroup:

                        taxGroupID = pair.Value;
                        break;

                    case RetailItemSearchEnum.VariantGroup:
                        variantGroupID = pair.Value;
                        break;

                    case RetailItemSearchEnum.Vendor:
                        vendorID = pair.Value;
                        break;

                    case RetailItemSearchEnum.BarCode:
                        barCode = pair.Value;
                        break;

                    case RetailItemSearchEnum.SpecialGroup:
                        specialGroup = pair.Value;
                        break;
                }
            }

            int totalRecordsMatching;
            return AdvancedSearchOptimized<SimpleRetailItem>(
                entry, 
                rowFrom, 
                rowTo, 
                sort, 
                false, 
                out totalRecordsMatching,
                ColumnPopulation.Simple, 
                searchString,
                beginsWith, 
                retailGroupID, 
                retailDepartmentID, 
                taxGroupID,
                variantGroupID, 
                vendorID, 
                barCode, 
                barCodeBeginsWith, 
                specialGroup);
        }

        public virtual List<DataEntity> GetForecourtItems(IConnectionManager entry, RecordIdentifier gradeID)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select 
                                        R.[ITEMID], 
                                        R.[ITEMNAME] 
                                    FROM 
                                        RETAILITEM R
                                    WHERE 
                                        R.GRADEID=@gradeID";
                MakeParam(cmd, "gradeID", gradeID);
                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }

       
        /// <summary>
        /// Gets a retail item from the database by a given ID.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The id of the retail item to fetch</param>
        /// <param name="cacheType">Cache</param>
        /// <returns>The retail item, or null if not found</returns>
        public virtual RetailItemNew Get(IConnectionManager entry, RecordIdentifier itemID, CacheType cacheType = CacheType.CacheTypeNone)
        {
            RetailItemNew result;
            if (cacheType != CacheType.CacheTypeNone)
            {
                result = (RetailItemNew)entry.Cache.GetEntityFromCache(typeof(RetailItemNew), itemID);
                if (result != null)
                {
                    return result;
                }
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in SelectionColumns[ColumnPopulation.SiteManager])
                {
                    columns.Add(selectionColumn);
                }
                List<Condition> conditions = new List<Condition>{new Condition{Operator = "AND", ConditionValue = "A.ITEMID = @itemID"}};
                
                string commandText = @"
    Select 
        -- Columns
        {0}
    FROM RETAILITEM A 
        --Joins
        {1}
        --Conditions
        {2}
    ";
                

                AddCustomHandling(ref columns, ref conditions );
                cmd.CommandText = string.Format(commandText, 
                    QueryPartGenerator.InternalColumnGenerator(columns), 
                    QueryPartGenerator.JoinGenerator(itemJoins),
                    QueryPartGenerator.ConditionGenerator(conditions));
                MakeParam(cmd, "itemID", (string)itemID);
                
                var records = Execute<RetailItemNew>(entry, cmd, CommandType.Text, PopulateItem);

                result = (records.Count > 0) ? records[0] : null;
            }

           
            if (result != null && cacheType != CacheType.CacheTypeNone)
            {
                entry.Cache.AddEntityToCache(itemID, result, cacheType);
            }

            return result;
        }

        /// <summary>
        /// Gets retail items in the system for a specific ID using a LIKE query
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">Item ID</param>
        /// <returns>The retail item, or null if not found</returns>
        public virtual List<RetailItemNew> GetItemsByItemPattern(IConnectionManager entry, string itemID)
        {
            //TODO This is redundant
            List<RetailItemNew> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in SelectionColumns[ColumnPopulation.SiteManager])
                {
                    columns.Add(selectionColumn);
                }
                List<Condition> conditions = new List<Condition> { new Condition { Operator = "AND", ConditionValue = "A.ITEMID LIKE @itemID" } };

                string commandText = @"
    Select 
        -- Columns
        {0}
    FROM RETAILITEM A 
        --Conditions
        {1}
    ";


                AddCustomHandling(ref columns, ref conditions);
                cmd.CommandText = string.Format(commandText,
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.ConditionGenerator(conditions));
                
                MakeParam(cmd, "itemId", itemID + "%");

                records = Execute<RetailItemNew>(entry, cmd, CommandType.Text, PopulateItem);
            }

          

            return records;
        }


        /// <summary>
        /// Gets all retail items in the system
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>The retail item, or null if not found</returns>
        public virtual List<RetailItemNew> GetAllItems(IConnectionManager entry)
        {
            List<RetailItemNew> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = new List<TableColumn>();
                foreach (var selectionColumn in SelectionColumns[ColumnPopulation.SiteManager])
                {
                    columns.Add(selectionColumn);
                }
                List<Condition> conditions = new List<Condition> ();

                string commandText = @"
    Select 
        -- Columns
        {0}
    FROM RETAILITEM A 
        --Conditions
        {1}
    ";


                AddCustomHandling(ref columns, ref conditions);
                cmd.CommandText = string.Format(commandText,
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.ConditionGenerator(conditions));

                records = Execute<RetailItemNew>(entry, cmd, CommandType.Text, PopulateItem);
            }

            

            return records;
        }


        public bool ItemCanBeDeleted(IConnectionManager entry, RecordIdentifier id)
        {
            if (
                RecordExists(entry, "PURCHASEORDERLINE", "RETAILITEMID", id) ||
                RecordExists(entry, "PURCHASEORDERLINE", "VENDORITEMID", id) ||
                RecordExists(entry, "RBOTRANSACTIONFUELTRANS", "ITEMID", id) ||
                RecordExists(entry, "RBOTRANSACTIONSALESTRANS", "ITEMID", id)

                )
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// Deletes a retail item by a given ID.
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID of the retail item to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            throw new NotImplementedException();
            //if (ItemCanBeDeleted(entry, id))
            //{
            //    DeleteItem(entry, id);
            //}
            //else
            //{
            //    var item = Get(entry, id);
            //    item.DateToBeBlocked = new Date(DateTime.Now);
            //    item.Dirty = true;
            //    Save(entry, item);
            //}
        }

        private void DeleteItem(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "INVENTTABLE", "ITEMID", id, BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "RBOINVENTTABLE", "ITEMID", id, BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "INVENTTABLEMODULE", "ITEMID", id, BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "INVENTDIMCOMBINATION", "ITEMID", id, BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "INVENTITEMBARCODE", "ITEMID", id, BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "INVENTITEMBARCODE", "ITEMID", id, BusinessObjects.Permission.ItemsEdit);
            DeleteRecord(entry, "VENDORITEMS", "RETAILITEMID", id, "");
            DeleteRecord(entry, "RBOSPECIALGROUPITEMS", "ITEMID", id, BusinessObjects.Permission.ItemsEdit);

            var secondaryIDBackup = id.SecondaryID;
            id.SecondaryID = new RecordIdentifier(1);
            DeleteRecord(entry, "RBOINFOCODETABLESPECIFIC", new[] { "REFRELATION", "REFTABLEID" }, id, BusinessObjects.Permission.ItemsEdit);
            id.SecondaryID = secondaryIDBackup;
        }

        public virtual int ItemCount(IConnectionManager entry)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT COUNT(1) FROM RETAILITEM";

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }




        /// <summary>
        ///  Checks if a retail item with a given ID exists in the database.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemID">The ID of the retail item to check for</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier itemID)
        {
            return RecordExists(entry, "RETAILITEM", "ITEMID", itemID, false) ;
        }

        /// <summary>
        /// Checks if any item is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any item uses the tax group, else false</returns>
        public virtual bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID)
        {
            return RecordExists(entry, "INVENTTABLEMODULE", new[] { "MODULETYPE", "TAXITEMGROUPID" }, new RecordIdentifier(2, taxgroupID));
        }

        private static bool ItemRecordExists(IConnectionManager entry, RecordIdentifier itemID)
        {
            return RecordExists(entry, "RBOINVENTTABLE", "ITEMID", itemID);
        }

        private static bool InventTableModuleExists(IConnectionManager entry, RecordIdentifier itemID, int moduleID)
        {
            return RecordExists(entry, "INVENTTABLEMODULE", new[] { "ITEMID", "MODULETYPE" }, new RecordIdentifier(itemID, moduleID));
        }

        /// <summary>
        /// Saves the retail item and its related module records, it only saves those that are in need of saving.
        /// (Thus master record or sub record that has the Dirty property as true get saved)
        /// </summary>
        /// <remarks>Edit retail items permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="item">The retail item to save</param>
        public virtual void Save(IConnectionManager entry, RetailItemNew item)
        {
            var statement = new SqlServerStatement("RETAILITEM");

            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);
            if (item.ID == RecordIdentifier.Empty || !Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;
                if (item.MasterID == RecordIdentifier.Empty)
                {
                    item.MasterID = Guid.NewGuid();
                }
                statement.AddKey("MASTERID", (Guid)item.MasterID, SqlDbType.UniqueIdentifier);
                if (item.ID == RecordIdentifier.Empty)
                {
                    // Should we create a new item or update what exists
                    item.ID = DataProviderFactory.Instance.GenerateNumber<IRetailItemData, RetailItem>(entry);
                }
                statement.AddField("ITEMID", (string) item.ID);
            }

            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("MASTERID", (Guid)item.MasterID, SqlDbType.UniqueIdentifier);
            }

            if (!RecordIdentifier.IsEmptyOrNull(item.HeaderItemID))
            {
                statement.AddField("HEADERITEMID", (Guid) item.HeaderItemID, SqlDbType.UniqueIdentifier);
            }
            statement.AddField("ITEMNAME", item.Text);
            statement.AddField("VARIANTNAME", item.VariantName);
            statement.AddField("ITEMTYPE", item.ItemType, SqlDbType.TinyInt);
            if (!RecordIdentifier.IsEmptyOrNull(item.DefaultVendorID))
            {
                statement.AddField("DEFAULTVENDORID", (string) item.DefaultVendorID);
            }
            statement.AddField("NAMEALIAS", item.NameAlias);
            statement.AddField("EXTENDEDDESCRIPTION", item.ExtendedDescription);
            statement.AddField("RETAILGROUPID", (string)item.RetailGroupID);
            statement.AddField("RETAILDEPARTMENTID", (string)item.RetailDepartmentID);
            statement.AddField("RETAILDIVISIONID", (string)item.RetailDivisionID);
            statement.AddField("ZEROPRICEVALID", item.ZeroPriceValid, SqlDbType.Bit);
            statement.AddField("QTYBECOMESNEGATIVE", item.QuantityBecomesNegative, SqlDbType.Bit);
            statement.AddField("NODISCOUNTALLOWED", item.NoDiscountAllowed, SqlDbType.Bit);
            statement.AddField("KEYINPRICE", item.KeyInPrice, SqlDbType.Bit);
            statement.AddField("SCALEITEM", item.ScaleItem, SqlDbType.Bit);
            statement.AddField("KEYINQTY", item.KeyInQuantity, SqlDbType.Bit);
            statement.AddField("BLOCKEDONPOS", item.BlockedOnPOS, SqlDbType.Bit);
            statement.AddField("BARCODESETUPID", (string)item.BarCodeSetupID);
            statement.AddField("PRINTVARIANTSSHELFLABELS", item.PrintVariantsShelfLabels, SqlDbType.Bit);
            statement.AddField("FUELITEM", item.IsFuelItem, SqlDbType.Bit);
            statement.AddField("GRADEID", item.GradeID);
            statement.AddField("MUSTKEYINCOMMENT", item.MustKeyInComment, SqlDbType.Bit);
            statement.AddField("DATETOBEBLOCKED", item.DateToBeBlocked.ToAxaptaSQLDate(), SqlDbType.DateTime);
            statement.AddField("DATETOACTIVATEITEM", item.DateToActivateItem.ToAxaptaSQLDate(), SqlDbType.DateTime);
            statement.AddField("PROFITMARGIN", item.ProfitMargin, SqlDbType.Decimal);
            statement.AddField("VALIDATIONPERIODID", (string)item.ValidationPeriodID);
            statement.AddField("MUSTSELECTUOM", item.MustSelectUOM, SqlDbType.Bit);
            statement.AddField("INVENTORYUNITID", (string)item.InventoryUnitID);
            statement.AddField("PURCHASEUNITID", (string)item.PurchaseUnitID);
            statement.AddField("SALESUNITID", (string)item.SalesUnitID);
            statement.AddField("PURCHASEPRICE", item.PurchasePrice, SqlDbType.Decimal);
            statement.AddField("PURCHASEPRICEINCLTAX", item.PurchasePriceIncludingTax, SqlDbType.Decimal);
            statement.AddField("PURCHASEPRICEUNIT", item.PurchasePriceUnit, SqlDbType.Decimal);
            statement.AddField("PURCHASEMARKUP", item.PurchaseMarkup, SqlDbType.Decimal);
            statement.AddField("SALESPRICE", item.SalesPrice, SqlDbType.Decimal);
            statement.AddField("SALESPRICEINCLTAX", item.SalesPriceIncludingTax, SqlDbType.Decimal);
            statement.AddField("SALESPRICEUNIT", item.SalesPriceUnit, SqlDbType.Decimal);
            statement.AddField("SALESMARKUP", item.SalesMarkup, SqlDbType.Decimal);
            statement.AddField("SALESLINEDISC", (string)item.SalesLineDiscount);
            statement.AddField("SALESMULTILINEDISC", (string)item.SalesMultiLineDiscount);
            statement.AddField("SALESALLOWTOTALDISCOUNT", item.SalesAllowTotalDiscount, SqlDbType.Bit);
            statement.AddField("SALESTAXITEMGROUPID", (string)item.SalesTaxItemGroupID);
            statement.AddField("DELETED", item.Deleted, SqlDbType.Bit);


            SaveCustomA(entry, statement, item);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Gets an items item sales tax group. Returns an empty record identifier if item has no item sales tax group
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="itemId">The items ID</param>
        /// <returns>Items item sales tax group</returns>
        public virtual RecordIdentifier GetItemsItemSalesTaxGroupID(IConnectionManager entry, RecordIdentifier itemId)
        {
            throw new NotImplementedException();
            //using (var cmd = entry.Connection.CreateCommand())
            //{
            //    cmd.CommandText =
            //        "select ISNULL(TAXITEMGROUPID,'') as TAXITEMGROUPID " +
            //        "from INVENTTABLEMODULE " +
            //        "where MODULETYPE = 2 and ITEMID = @itemId and DATAAREAID = @dataAreaId";

            //    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            //    MakeParam(cmd, "itemId", (string)itemId);

            //    RecordIdentifier result = "";
            //    IDataReader dr = null;
            //    try
            //    {
            //        dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

            //        if (dr.Read())
            //        {
            //            result = (string)dr["TAXITEMGROUPID"];
            //        }
            //    }
            //    finally
            //    {
            //        if (dr != null)
            //        {
            //            dr.Close();
            //        }
            //    }
            //    return result;
            //}
        }

        /// <summary>
        /// Gets the latest purchase price of an item
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        /// <param name="itemID">The item who's purchase price we want</param>
        /// <returns>The latest purchase price of an item</returns>
        public virtual decimal GetLatestPurchasePrice(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select SALESPRICE from RETAILITEM " +
                    "where ITEMID = @itemID ";

                MakeParam(cmd, "itemID", (string)itemID);

                return (decimal)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Returns the default vendor for a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <returns>
        /// Returns the vendor ID
        /// </returns>
        public virtual string GetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Select DEFAULTVENDORID from RETAILITEM " +
                    "where ITEMID = @itemID ";

                MakeParam(cmd, "itemID", (string)itemID);

                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Returns true if the item has a default vendor
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <returns>
        /// Returns true if the item has a default vendor
        /// </returns>
        public virtual bool ItemHasDefaultVendor(IConnectionManager entry, RecordIdentifier itemID)
        {
            var defaultVendor = GetItemsDefaultVendor(entry, itemID);

            return (defaultVendor != "");
        }

        public virtual bool ItemHasDimentionGroup(IConnectionManager entry, ItemListItem itemID)
        {
            throw new NotImplementedException();
//            using (var cmd = entry.Connection.CreateCommand())
//            {
//                cmd.CommandType = CommandType.Text;
//                cmd.CommandText = @"SELECT COLORGROUP + SIZEGROUP + STYLEGROUP 
//                                  from RBOINVENTTABLE
//                                  where ((Colorgroup <> '' and colorgroup is not null)
//                                  or    (sizegroup <> '' and sizegroup is not null)
//                                  or    (stylegroup <> '' and stylegroup is not null))
//                                  and itemid = @itemID";

//                MakeParam(cmd, "itemID", (string)itemID.ID);

//                return ((string)entry.Connection.ExecuteScalar(cmd) != "");
//            }
        }

        //public virtual bool ItemHasDimentionGroup(IConnectionManager entry, ItemListItem itemID)
        //{
        //    var hasDimentionGroup = GetItemDimentionGroup(entry, itemID);

        //    return (hasDimentionGroup != "");
        //}

        /// <summary>
        /// Sets a vendor as a default on a given item
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemID">The unique ID of the item</param>
        /// <param name="vendorItemID">The unique ID of the vendor item</param>
        public virtual void SetItemsDefaultVendor(IConnectionManager entry, RecordIdentifier itemID, RecordIdentifier vendorItemID)
        {
            var statement = new SqlServerStatement("RETAILITEM");

            ValidateSecurity(entry, BusinessObjects.Permission.CurrencyEdit);

            if (Exists(entry, itemID))
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("ITEMID", (string)itemID);

                statement.AddField("DEFAULTVENDORID", (string)vendorItemID);

                entry.Connection.ExecuteStatement(statement);
            }

        }

        public virtual RetailItem.RetailItemModule GetPriceModule(IConnectionManager entry, RecordIdentifier itemID)
        {
            //using (var cmd = entry.Connection.CreateCommand())
            //{
            //    cmd.CommandText = BaseGetModuleString + @" WHERE iu.DATAAREAID = @dataAreaID AND iu.ITEMID = @itemID AND iu.MODULETYPE = 2";
            //    MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
            //    MakeParam(cmd, "itemID", itemID);

            //    var list = Execute<RetailItem.RetailItemModule>(entry, cmd, CommandType.Text, PopulateModule);
            //    return list.Count > 0 ? list[0] : null;
            //}
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns item ids for all items that have a tax group with the given tax code ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxCodeID">ID of the tax code</param>
        public virtual List<RecordIdentifier> GetItemIDsFromTaxCode(IConnectionManager entry, RecordIdentifier taxCodeID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                                    SELECT itm.ITEMID 
                                    FROM INVENTTABLE it
                                    JOIN INVENTTABLEMODULE itm on it.ITEMID = itm.ITEMID and it.DATAAREAID = itm.DATAAREAID
                                    JOIN TAXONITEM toi on itm.TAXITEMGROUPID = toi.TAXITEMGROUP and itm.DATAAREAID = toi.DATAAREAID
                                    Where toi.TAXCODE = @taxCodeID 
                                    AND itm.MODULETYPE = 2
                                    AND it.DATAAREAID = @dataAreaID";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "taxCodeID", (string)taxCodeID);

                var dataEntities = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");
                return (from x in dataEntities
                        select x.ID).ToList();
            }
        }

        public virtual List<RecordIdentifier> GetItemIDsFromTaxGroup(IConnectionManager entry, RecordIdentifier itemSalesTaxGroupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                                @"SELECT itm.ITEMID 
                                FROM INVENTTABLEMODULE itm
                                Where itm.TAXITEMGROUPID = @itemSalesTaxGroupID 
                                AND itm.MODULETYPE = 2
                                AND itm.DATAAREAID = @dataAreaID";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemSalesTaxGroupID", (string)itemSalesTaxGroupID);

                var dataEntities = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");
                return (from x in dataEntities
                        select x.ID).ToList();
            }
        }

        public virtual List<RecordIdentifier> GetItemIDsOfItemsWithTaxGroup(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                                @"SELECT itm.ITEMID 
                                FROM INVENTTABLEMODULE itm
                                inner join INVENTTABLE it on it.ITEMID = itm.ITEMID and it.DATAAREAID = itm.DATAAREAID
                                Where itm.TAXITEMGROUPID <> '' 
                                AND itm.MODULETYPE = 2
                                AND itm.DATAAREAID = @dataAreaID";


                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                var dataEntities = Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMID", "ITEMID");
                return (from x in dataEntities
                        select x.ID).ToList();
            }
        }

        public virtual int Count(IConnectionManager entry)
        {
            return Count(entry, "RBOINVENTTABLE");
        }

        public virtual List<DataEntity> FindItem(IConnectionManager entry, string searchText)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select ITEMID, ITEMNAME from INVENTTABLE " +
                                  "where (ITEMID Like @itemID or ITEMNAME Like @itemName) and DATAAREAID = @dataAreaId order by ITEMNAME";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemID", "%" + searchText + "%");
                MakeParam(cmd, "itemName", "%" + searchText + "%");

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "ITEMNAME", "ITEMID");
            }
        }

        public virtual List<DataEntity> FindItemDepartment(IConnectionManager entry, string searchText)
        {
            ValidateSecurity(entry);

            // Stupility here since sometimes in the database you have items with group even if the groups do not exist
            // so we need to both look in the groups and in the items
            // Down the road when we have ensured that groups always exist then this should be refactured

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select distinct k.* from " +
                                  "(Select ITEMGROUPID,ISNULL(Name,'') as Name " +
                                  "from INVENTITEMGROUP " +
                                  "where (ITEMGROUPID Like @itemGroupID or Name Like @Name) and DATAAREAID = @dataAreaId " +
                                  "union " +
                                  "Select i.ITEMGROUPID,ISNULL(ig.Name,'') as Name " +
                                  "from INVENTTABLE i " +
                                  "left outer join INVENTITEMGROUP ig on i.DATAAREAID = ig.DATAAREAID and i.ITEMGROUPID = ig.ITEMGROUPID " +
                                  "where (i.ITEMGROUPID Like @itemGroupID or ig.Name Like @name) and i.DATAAREAID = @dataAreaId) k " +
                                  "order by ITEMGROUPID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "itemGroupID", "%" + searchText + "%");
                MakeParam(cmd, "name", "%" + searchText + "%");

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "Name", "ITEMGROUPID");
            }
        }

        public virtual void ReAssignItemDepartmentAndDivision(IConnectionManager entry)
        {

            ValidateSecurity(entry, BusinessObjects.Permission.ItemsEdit);

            using (var cmd = entry.Connection.CreateCommand(
                @"
                    UPDATE 
                        rboinventtable
                    SET 
                        DIVISIONID = div.DIVISIONID,
                        ITEMDEPARTMENT = dep.DEPARTMENTID
                    FROM RBOINVENTITEMRETAILGROUP rg
                    JOIN RBOINVENTITEMDEPARTMENT dep on rg.DEPARTMENTID = dep.DEPARTMENTID AND rg.DATAAREAID = dep.DATAAREAID
                    JOIN RBOINVENTITEMRETAILDIVISION div  on dep.divisionid = div.DIVISIONID AND div.DATAAREAID = dep.DATAAREAID
                    WHERE 
                        RBOINVENTTABLE.ITEMGROUP = rg.GROUPID 
                        and rboinventtable.DATAAREAID = @DATAAREAID
                        and RG.DATAAREAID = rboinventtable.DATAAREAID"))
            {
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }

        }

        /// <summary>
        /// Adds an item - attribute connection for the given item master ID and attribute ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemID">The retail item master ID</param>
        /// <param name="attributeID">The ID of the dimension attribute</param>
        public virtual void AddDimensionAttribute(IConnectionManager entry, RecordIdentifier retailItemID, RecordIdentifier attributeID)
        {
            ValidateSecurity(entry, Permission.ItemsEdit);
            bool exists;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"
                SELECT 
                    RETAILITEMID,
                    DIMENSIONATTRIBUTEID
                  FROM RETAILITEMDIMENSIONATTRIBUTE
                  WHERE 
                    RETAILITEMID = @retailItemID AND
                    DIMENSIONATTRIBUTEID = @dimensionAttributeID";
                MakeParam(cmd, "retailItemID", retailItemID);
                MakeParam(cmd, "dimensionAttributeID", attributeID);
                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    exists =  dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                        dr.Dispose();
                    }
                }
            }

            var statement = new SqlServerStatement("RETAILITEMDIMENSIONATTRIBUTE");
            if (!exists)
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("RETAILITEMID", (Guid) retailItemID, SqlDbType.UniqueIdentifier);
                statement.AddKey("DIMENSIONATTRIBUTEID", (Guid) attributeID, SqlDbType.UniqueIdentifier);
                entry.Connection.ExecuteStatement(statement);
            }
          
        }

        /// <summary>
        /// Removes the item - attribute connection for the given item master ID and attribute ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemID">The retail item master ID</param>
        /// <param name="attributeID">The ID of the dimension attribute</param>
        public virtual void RemoveDimensionAttribute(IConnectionManager entry, RecordIdentifier retailItemID,
            RecordIdentifier attributeID)
        {
            ValidateSecurity(entry, Permission.ItemsEdit);

            var statement = new SqlServerStatement("RETAILITEMDIMENSIONATTRIBUTE", StatementType.Delete);

            statement.AddCondition("RETAILITEMID", (Guid) retailItemID, SqlDbType.UniqueIdentifier);
            statement.AddCondition("DIMENSIONATTRIBUTEID", (Guid) attributeID, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        #region Custom overrideables
        protected virtual void AddCustomHandling(ref List<TableColumn> columns, ref List<Condition> conditions  )
        {
            //Add columns and conditions
        }

        protected virtual void SaveCustomA(IConnectionManager entry, SqlServerStatement statement, RetailItemNew retailItem)
        {
        }

        protected virtual void SaveCustomB(IConnectionManager entry, SqlServerStatement statement, RetailItem retailItem)
        {
        }

        protected virtual void SaveCustomModule(IConnectionManager entry, SqlServerStatement statement, RetailItem.RetailItemModule module)
        {
        }
        #endregion

        #region ISequenceable Members

        /// <summary>
        /// Returns true if information about the given class exists in the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The unique sequence ID to search for</param>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return ItemRecordExists(entry, id);
        }

        /// <summary>
        /// Returns a unique ID for the class
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "ITEM"; }
        }

        #endregion


        public System.Drawing.Image GetDefaultImage(IConnectionManager entry, RecordIdentifier itemID)
        {
            throw new NotImplementedException();
        }

        public List<ItemImage> GetImages(IConnectionManager entry, RecordIdentifier itemID)
        {
            throw new NotImplementedException();
        }

        public void SaveImage(IConnectionManager entry, RecordIdentifier itemID, System.Drawing.Image image)
        {
            throw new NotImplementedException();
        }

        public void SaveImage(IConnectionManager entry, RecordIdentifier itemID, System.Drawing.Image image, int index)
        {
            throw new NotImplementedException();
        }

        public void SaveImage(IConnectionManager entry, ItemImage itemImage)
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(IConnectionManager entry, ItemImage itemImage)
        {
            throw new NotImplementedException();
        }

        public void DeleteImages(IConnectionManager entry, RecordIdentifier itemID)
        {
            throw new NotImplementedException();
        }

        public void ResequenceImages(IConnectionManager entry, List<ItemImage> itemImages)
        {
            throw new NotImplementedException();
        }
    }
}
