using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Inventory
{
    /// <summary>
    /// Data provider class for vendors
    /// </summary>
    public class VendorData : SqlServerDataProviderBase, IVendorData
    {
        private static List<TableColumn> vendorColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ACCOUNTNUM " , TableAlias = "v"},
            new TableColumn {ColumnName = "NAME " , TableAlias = "v"},
            new TableColumn {ColumnName = "ISNULL(ADDRESS,'') " ,ColumnAlias = "ADDRESS"},
            new TableColumn {ColumnName = "ISNULL(STREET,'') " ,ColumnAlias = "STREET"},
            new TableColumn {ColumnName = "ISNULL(ZIPCODE,'') " ,ColumnAlias = "ZIPCODE"},
            new TableColumn {ColumnName = "ISNULL(CITY,'') " ,ColumnAlias = "CITY"},
            new TableColumn {ColumnName = "ISNULL(COUNTY,'') " ,ColumnAlias = "COUNTY"},
            new TableColumn {ColumnName = "ISNULL(STATE,'') " ,ColumnAlias = "STATE"},
            new TableColumn {ColumnName = "ISNULL(COUNTRY,'') " ,ColumnAlias = "COUNTRY"},
            new TableColumn {ColumnName = "ADDRESSFORMAT " , TableAlias = "v"},
            new TableColumn {ColumnName = "ISNULL(PHONE,'') " ,ColumnAlias = "PHONE"},
            new TableColumn {ColumnName = "ISNULL(FAX,'') " ,ColumnAlias = "FAX"},
            new TableColumn {ColumnName = "ISNULL(EMAIL,'') " ,ColumnAlias = "EMAIL"},
            new TableColumn {ColumnName = "ISNULL(LANGUAGEID,'') " ,ColumnAlias = "LANGUAGEID"},
            new TableColumn {ColumnName = "ISNULL(CURRENCY,'') " ,ColumnAlias = "CURRENCY"},
            new TableColumn {ColumnName = "ISNULL([MEMO],'') " ,ColumnAlias = "NOTES"},
            new TableColumn {ColumnName = "ISNULL(cur.TXT,'') " ,ColumnAlias = "CURRENCYDESCRIPTION"},
            new TableColumn {ColumnName = "ISNULL(DEFAULTCONTACTID,'') " ,ColumnAlias = "DEFAULTCONTACTID"},
            new TableColumn {ColumnName = "ISNULL(v.TAXGROUP,'') " , ColumnAlias = "TAXGROUP"},
            new TableColumn {ColumnName = "ISNULL(t.TAXGROUPNAME,'') " , ColumnAlias = "TAXGROUPNAME"},
            new TableColumn {ColumnName = "ISNULL(v.TAXCALCULATIONMETHOD,0) " , ColumnAlias = "TAXCALCULATIONMETHOD"},
            new TableColumn {ColumnName = "ISNULL(v.DEFAULTDELIVERYTIME,0) " , ColumnAlias = "DEFAULTDELIVERYTIME"},
            new TableColumn {ColumnName = "ISNULL(v.DELIVERYDAYSTYPE,0) " , ColumnAlias = "DELIVERYDAYSTYPE"},
            new TableColumn {ColumnName = "ISNULL(v.DELETED,0) " , ColumnAlias = "DELETED"}
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "v.CURRENCY = cur.CURRENCYCODE and v.DATAAREAID = cur.DATAAREAID ",
                JoinType = "LEFT OUTER",
                Table = "CURRENCY",
                TableAlias = "cur"
            },
            new Join
            {
                Condition = "t.TAXGROUP = v.TAXGROUP and t.DATAAREAID = v.DATAAREAID ",
                JoinType = "LEFT OUTER",
                Table = "TAXGROUPHEADING",
                TableAlias = "t"
            }
        };

        private static string ResolveSort(VendorSorting sort, bool backwards)
        {
            string sortString = "";

            switch (sort)
            {
                case VendorSorting.ID:
                    sortString = "ACCOUNTNUM ASC";
                    break;

                case VendorSorting.Description:
                    sortString = "NAME ASC";
                    break;

                case VendorSorting.Phone:
                    sortString = "PHONE ASC";
                    break;

                case VendorSorting.Address:
                    sortString = "STREET ASC,ADDRESS ASC,ZIPCODE ASC,CITY ASC,STATE ASC,COUNTRY ASC";
                    break;
            }

            if (backwards)
            {
                sortString = sortString.Replace("ASC", "DESC");
            }

            return sortString;
        }

        /// <summary>
        /// Gets a list of DataEntity that contains active Vendor ID and Vendor Description. The list is sorted by Vendor description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>( entry, 
                                        showDeleted: false, 
                                        tableName: "VENDTABLE", 
                                        nameField: "NAME", idField: "ACCOUNTNUM", 
                                        orderField: "NAME");
        }

        /// <summary>
        /// Gets a list of DataEntity that contains active Vendor ID and Vendor Description. The list is sorted by the column specified in the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>Gets a list of DataEntity that contains CurrencyCode and Currency Description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, VendorSorting sortBy, bool sortBackwards)
        {
            if (sortBy != VendorSorting.ID && sortBy != VendorSorting.Description)
            {
                throw new NotSupportedException();
            }

            return GetList<DataEntity>(entry, 
                                       showDeleted: false, 
                                       tableName: "VENDTABLE", 
                                       nameField: "NAME", 
                                       idField: "ACCOUNTNUM", 
                                       orderField: ResolveSort(sortBy, sortBackwards));
        }

        private static void PopulateVendorInfo(IConnectionManager entry, IDataReader dr, Vendor vendor, object defaultFormat)
        {
            vendor.ID = (string)dr["ACCOUNTNUM"];
            vendor.Text = (string)dr["NAME"];

            vendor.Address1 = (string)dr["STREET"];
            vendor.Address2 = (string)dr["ADDRESS"];
            vendor.ZipCode = (string)dr["ZIPCODE"];
            vendor.City = (string)dr["CITY"];
            vendor.State = (string)dr["STATE"];
            vendor.Country = (string)dr["COUNTRY"];
            vendor.AddressFormat = (dr["ADDRESSFORMAT"] == DBNull.Value) ? (Address.AddressFormatEnum)defaultFormat : (Address.AddressFormatEnum)((int)(dr["ADDRESSFORMAT"]));

            vendor.Phone = (string)dr["PHONE"];
            vendor.Fax = (string)dr["FAX"];
            vendor.Email = (string)dr["EMAIL"];
            vendor.LanguageID = (string)dr["LANGUAGEID"];
            vendor.CurrencyID = (string)dr["CURRENCY"];
            vendor.CurrencyDescription = (string)dr["CURRENCYDESCRIPTION"];
            vendor.LongDescription = (string)dr["NOTES"];
            vendor.DefaultContactID = (string)dr["DEFAULTCONTACTID"];
            vendor.TaxGroup = (string)dr["TAXGROUP"];
            vendor.TaxGroupName = (string)dr["TAXGROUPNAME"];
            vendor.TaxCalculationMethod = (TaxCalculationMethodEnum)dr["TAXCALCULATIONMETHOD"];
            vendor.DefaultDeliveryTime = (int)dr["DEFAULTDELIVERYTIME"];
            vendor.DeliveryDaysType = (DeliveryDaysTypeEnum)dr["DELIVERYDAYSTYPE"];
            vendor.Deleted = (bool)dr["DELETED"];

            vendor.Dirty = false;
        }

        /// <summary>
        /// Gets a vendor with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The ID of the vendor to get</param>
        /// <returns>A vendor with a given ID</returns>
        public virtual Vendor Get(IConnectionManager entry, RecordIdentifier vendorID)
        {
            VendorSearch search = new VendorSearch();
            search.VendorID = vendorID;

            List<Vendor> result = AdvancedSearch(entry, search);

            return result.Count == 1 ? result[0] : new Vendor();
        }

        /// <summary>
        /// Gets a vendor with a given ID and deleted status
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The ID of the vendor to get</param>
        /// <param name="deleted">The deleted status of the vendor</param>
        /// <returns>A vendor with a given ID</returns>
        public virtual Vendor GetVendor(IConnectionManager entry, RecordIdentifier vendorID, bool? deleted = null)
        {
            VendorSearch search = new VendorSearch();
            search.VendorID = vendorID;
            search.Deleted = deleted;

            List<Vendor> result = AdvancedSearch(entry, search);

            return result.Count == 1 ? result[0] : new Vendor();
        }

        /// <summary>
        /// Gets all vendors
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>All vendors in the database</returns>
        public virtual List<Vendor> GetVendors(IConnectionManager entry, VendorSorting sortBy, bool sortBackwards)
        {
            VendorSearch search = new VendorSearch { Deleted = false };
            return AdvancedSearch(entry, search, sortBy, sortBackwards);
        }

        protected virtual List<Condition> GenerateSearchConditions(IConnectionManager entry, IDbCommand cmd, VendorSearch searchCriteria)
        {
            List<Condition> conditions = new List<Condition>();
            conditions.Add(new Condition { Operator = "AND", ConditionValue = "v.DATAAREAID = @DATAAREAID " });
            MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

            if (!string.IsNullOrEmpty((string)searchCriteria.VendorID))
            {
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "ACCOUNTNUM = @ACCOUNTCODE " });
                MakeParam(cmd, "ACCOUNTCODE", (string)searchCriteria.VendorID);
            }

            if (searchCriteria.Deleted != null)
            {
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "DELETED = @DELETED " });
                MakeParam(cmd, "DELETED", searchCriteria.Deleted, SqlDbType.Bit);
            }

            if (searchCriteria.Description != null && searchCriteria.Description.Count > 0)
            {
                List<Condition> searchConditions = new List<Condition>();
                for (int index = 0; index < searchCriteria.Description.Count; index++)
                {
                    string searchToken = PreProcessSearchText(searchCriteria.Description[index], true, searchCriteria.DescriptionBeginsWith);

                    if (!string.IsNullOrEmpty(searchToken))
                    {
                        searchConditions.Add(new Condition
                        {
                            ConditionValue =
                                $@" (v.ACCOUNTNUM Like @DESCRIPTION{index} 
                                        or v.NAME LIKE @DESCRIPTION{index}) ",
                            Operator = "AND"
                        });

                        MakeParam(cmd, $"DESCRIPTION{index}", searchToken);
                    }
                }
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue =
                        $" ({QueryPartGenerator.ConditionGenerator(searchConditions, true)}) "
                });
            }

            return conditions;
        }

        public virtual List<Vendor> AdvancedSearch(IConnectionManager entry, VendorSearch searchCriteria)
        {
            return AdvancedSearch(entry, searchCriteria, VendorSorting.ID, false);
        }

        public virtual List<Vendor> AdvancedSearch(IConnectionManager entry, VendorSearch searchCriteria, VendorSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = GenerateSearchConditions(entry, cmd, searchCriteria);
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("VENDTABLE", "v"),
                    QueryPartGenerator.InternalColumnGenerator(vendorColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    "ORDER BY " + ResolveSort(sortBy, sortBackwards)
                    );

                return Execute<Vendor>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateVendorInfo);
            }
        }

        /// <summary>
        /// Deletes a vendor by a given ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">The ID of the vendor to be deleted</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier vendorID)
        {
            if (entry.HasPermission(Permission.VendorEdit))
            {
                MarkAsDeleted(entry, "VENDTABLE", "ACCOUNTNUM", vendorID, Permission.VendorEdit);
            }
        }

        /// <summary>
        /// Activates a vendor that has been deleted by a given ID.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">The ID of the vendor to be activated</param>
        public virtual void Activate(IConnectionManager entry, RecordIdentifier vendorID)
        {
            if (entry.HasPermission(Permission.VendorEdit))
            {
                MarkAsRestored(entry, "VENDTABLE", "ACCOUNTNUM", vendorID, Permission.VendorEdit);
            }
        }

        /// <summary>
        /// Checks if a vendor by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">ID of the vendor to check for</param>
        /// <returns>True if the vendor exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier vendorID)
        {
            return RecordExists(entry, "VENDTABLE", "ACCOUNTNUM", vendorID);
        }

        /// <summary>
        /// Checks if any vendor is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any vendor uses the tax group, else false</returns>
        public virtual bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID)
        {
            return RecordExists(entry, "VENDTABLE", "TAXGROUP", taxgroupID);
        }

        /// <summary>
        /// Saves a vendor to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <remarks>Edit vendor permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendor">The vendor to be saved</param>
        public virtual void Save(IConnectionManager entry, Vendor vendor)
        {
            SqlServerStatement statement = new SqlServerStatement("VENDTABLE");
            statement.UpdateColumnOptimizer = vendor;

            ValidateSecurity(entry, BusinessObjects.Permission.VendorEdit);

            vendor.Validate();

            bool isNew = false;
            if (vendor.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                vendor.ID = DataProviderFactory.Instance.GenerateNumber<IVendorData, Vendor>(entry);
            }

            if (isNew || !Exists(entry, vendor.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ACCOUNTNUM", (string)vendor.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ACCOUNTNUM", (string)vendor.ID);
            }

            statement.AddField("NAME", vendor.Text);
            statement.AddField("ADDRESS", vendor.Address2);
            statement.AddField("STREET", vendor.Address1);
            statement.AddField("CITY", vendor.City);
            statement.AddField("ZIPCODE", vendor.ZipCode);
            statement.AddField("STATE", vendor.State);
            statement.AddField("COUNTRY", (string)vendor.Country);
            statement.AddField("PHONE", vendor.Phone);
            statement.AddField("FAX", vendor.Fax);
            statement.AddField("EMAIL", vendor.Email);
            statement.AddField("LANGUAGEID", (string)vendor.LanguageID);
            statement.AddField("CURRENCY", (string)vendor.CurrencyID);
            statement.AddField("DEFAULTCONTACTID", (string)vendor.DefaultContactID);
            statement.AddField("MEMO", vendor.LongDescription);
            statement.AddField("ADDRESSFORMAT", (int)vendor.AddressFormat, SqlDbType.Int);
            statement.AddField("TAXGROUP", (string)vendor.TaxGroup);
            statement.AddField("TAXCALCULATIONMETHOD", (int)vendor.TaxCalculationMethod, SqlDbType.Int);
            statement.AddField("DEFAULTDELIVERYTIME", vendor.DefaultDeliveryTime, SqlDbType.Int);
            statement.AddField("DELIVERYDAYSTYPE", (int)vendor.DeliveryDaysType, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Saves and returns the vendor information
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendor">The information to be saved</param>
        /// <returns>Vendor</returns>
        public virtual Vendor SaveAndReturnVendor(IConnectionManager entry, Vendor vendor)
        {
            SqlServerStatement statement = new SqlServerStatement("VENDTABLE");
            statement.UpdateColumnOptimizer = vendor;

            ValidateSecurity(entry, BusinessObjects.Permission.VendorEdit);

            vendor.Validate();

            bool isNew = false;
            if (vendor.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                vendor.ID = DataProviderFactory.Instance.GenerateNumber<IVendorData, Vendor>(entry);
            }

            if (isNew || !Exists(entry, vendor.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ACCOUNTNUM", (string)vendor.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ACCOUNTNUM", (string)vendor.ID);
            }

            statement.AddField("NAME", vendor.Text);
            statement.AddField("ADDRESS", vendor.Address2);
            statement.AddField("STREET", vendor.Address1);
            statement.AddField("CITY", vendor.City);
            statement.AddField("ZIPCODE", vendor.ZipCode);
            statement.AddField("STATE", vendor.State);
            statement.AddField("COUNTRY", (string)vendor.Country);
            statement.AddField("PHONE", vendor.Phone);
            statement.AddField("FAX", vendor.Fax);
            statement.AddField("EMAIL", vendor.Email);
            statement.AddField("LANGUAGEID", (string)vendor.LanguageID);
            statement.AddField("CURRENCY", (string)vendor.CurrencyID);
            statement.AddField("DEFAULTCONTACTID", (string)vendor.DefaultContactID);
            statement.AddField("MEMO", vendor.LongDescription);
            statement.AddField("ADDRESSFORMAT", (int)vendor.AddressFormat, SqlDbType.Int);
            statement.AddField("TAXGROUP", (string)vendor.TaxGroup);
            statement.AddField("TAXCALCULATIONMETHOD", (int)vendor.TaxCalculationMethod, SqlDbType.Int);
            statement.AddField("DEFAULTDELIVERYTIME", vendor.DefaultDeliveryTime, SqlDbType.Int);
            statement.AddField("DELIVERYDAYSTYPE", (int)vendor.DeliveryDaysType, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);

            return vendor;
        }

        public virtual RecordIdentifier GetVendorsSalesTaxGroupID(IConnectionManager entry, RecordIdentifier vendorID)
        {
            Vendor vendor = Get(entry, vendorID);
            return vendor.TaxGroup;
        }

        /// <summary>
        /// Sets the default contact on the vendor
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="vendorID">The unique ID of the vendor to be updated</param>
        /// <param name="contactID">The unique ID of the contact that is to be the default contact on the vendor</param>
        public virtual void SetDefaultContact(IConnectionManager entry, RecordIdentifier vendorID, RecordIdentifier contactID)
        {
            Vendor vendor = Get(entry, vendorID);
            vendor.DefaultContactID = contactID;
            Save(entry, vendor);
        }

        /// <summary>
        /// Gets all the items from the database matching the list of <see cref="itemsToCompare" />
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items <see cref="itemsToCompare"</param>
        /// <returns>List of items</returns>
        public virtual List<Vendor> GetCompareList(IConnectionManager entry, List<Vendor> itemsToCompare)
        {
            DataPopulator<Vendor> populator = (dr, item) =>
            {
                PopulateVendorInfo(entry, dr, item, null);
            };

            return GetCompareListInBatches(entry, itemsToCompare, "VENDTABLE", "ACCOUNTNUM", vendorColumns, listJoins, populator);
        }

        #region ISequenceable Members

        /// <summary>
        /// Checks if a sequence with a given ID exists for vendors
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID to check for</param>
        /// <returns>True if it exists, else false</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// ID into the sequence manager.
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "Vendors"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "VENDTABLE", "ACCOUNTNUM", sequenceFormat, startingRecord, numberOfRecords);
        }
        #endregion

        public bool DeleteVendorsCanExecute(IConnectionManager entry, List<RecordIdentifier> vendors, out List<RecordIdentifier> linkedVendors)
        {
            linkedVendors = new List<RecordIdentifier>();
            string sql = @" SELECT DISTINCT ACCOUNTNUM,NAME
                            FROM VENDTABLE v
                            WHERE v.ACCOUNTNUM in ({0}) AND
                                  ((SELECT COUNT(*) FROM PURCHASEORDERS po WHERE po.VENDORID = v.ACCOUNTNUM AND po.PURCHASESTATUS = 0) > 0 OR
                                   (SELECT COUNT(*) FROM PURCHASEWORKSHEETLINE pwl WHERE pwl.VENDORID = v.ACCOUNTNUM) > 0 OR
                                   (SELECT COUNT(*) FROM PURCHASEORDERS PO WHERE po.VENDORID = v.ACCOUNTNUM AND (SELECT COUNT(*) FROM GOODSRECEIVING gr WHERE gr.PURCHASEORDERID = po.PURCHASEORDERID) = 0) > 0 OR
	                               (SELECT COUNT(*) FROM GOODSRECEIVING gr INNER JOIN PURCHASEORDERS po ON gr.PURCHASEORDERID = po.PURCHASEORDERID WHERE po.VENDORID = v.ACCOUNTNUM AND gr.STATUS = 0) > 0)";


            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(sql, string.Join(",", vendors.Select(v => $"'{v}'")));

                List<Vendor> vendorList = Execute<Vendor>(entry, cmd, CommandType.Text, "NAME", "ACCOUNTNUM");
                if (vendorList == null || vendorList.Count == 0)
                {
                    return true;
                }
                else
                {
                    foreach (Vendor v in vendorList)
                    {
                        linkedVendors.Add(v.ID);
                    }

                    return false;
                }
            }
        }
    }
}