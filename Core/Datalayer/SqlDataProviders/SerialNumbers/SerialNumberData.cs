using LSOne.DataLayer.DataProviders.SerialNumbers;
using LSOne.DataLayer.SqlConnector.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.DataProviders;
using System.Data;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.SqlDataProviders.SerialNumbers
{
    public class SerialNumberData : SqlServerDataProviderBase, ISerialNumberData
    {

        private static List<TableColumn> serialNumberColumns = new List<TableColumn>()
        {
            new TableColumn { ColumnName = "ID", TableAlias = "SN" },
            new TableColumn { ColumnName = "ITEMMASTERID", TableAlias = "SN" },
            new TableColumn { ColumnName = "SERIALNUMBER", TableAlias = "SN" },
            new TableColumn { ColumnName = "TYPEOFSERIAL", TableAlias = "SN" },
            new TableColumn { ColumnName = "CREATEDDATE", TableAlias = "SN" },
            new TableColumn { ColumnName = "USEDDATE", TableAlias = "SN" },
            new TableColumn { ColumnName = "REFERENCE", TableAlias = "SN" },
            new TableColumn { ColumnName = "MANUALENTRY", TableAlias = "SN" },
            new TableColumn { ColumnName = "RESERVED", TableAlias = "SN" },
            new TableColumn { ColumnName = "RI.ITEMNAME", ColumnAlias = "ITEMDESCRIPTION" },
            new TableColumn { ColumnName = "RI.VARIANTNAME", ColumnAlias = "ITEMVARIANT" },
            new TableColumn { ColumnName = "RI.ITEMID", ColumnAlias = "ITEMID" }
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "RI.MASTERID = SN.ITEMMASTERID",
                JoinType = "INNER",
                Table = "RETAILITEM",
                TableAlias = "RI"
            },
            new Join
            {
                Condition = " RI.RETAILGROUPMASTERID = RG.MASTERID AND RG.DELETED =0",
                JoinType = "LEFT OUTER",
                Table = "RETAILGROUP",
                TableAlias = "RG"
            }
        };

        private static void PopulateSerialNumber(IDataReader dr, SerialNumber serialNumber)
        {
            serialNumber.ID = (Guid)dr["ID"];
            serialNumber.ItemMasterID = (Guid)dr["ITEMMASTERID"];
            serialNumber.ItemID = (string)dr["ITEMID"];
            serialNumber.SerialNo = (string)dr["SERIALNUMBER"];
            serialNumber.ItemDescription = (string)dr["ITEMDESCRIPTION"];
            serialNumber.ItemVariant = (string)dr["ITEMVARIANT"];
            serialNumber.SerialType = (TypeOfSerial)((byte)dr["TYPEOFSERIAL"]);
            serialNumber.CreateDate = (DateTime)dr["CREATEDDATE"];
            serialNumber.UsedDate = (dr["USEDDATE"] == DBNull.Value) ? null : (DateTime?)dr["USEDDATE"];
            serialNumber.Reference = (dr["REFERENCE"] == DBNull.Value) ? string.Empty : (string)dr["REFERENCE"];
            serialNumber.ManualEntry = (bool)dr["MANUALENTRY"];
            serialNumber.Reserved = (bool)dr["RESERVED"];
        }

        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        protected virtual void PopulateSerialNumberWithCount(IConnectionManager entry, IDataReader dr, SerialNumber item, ref int rowCount)
        {
            PopulateSerialNumber(dr, item);
            PopulateRowCount(entry, dr, ref rowCount);
        }

        public RecordIdentifier SequenceID
        {
            get
            {
                return "SERIALNUMBERS";
            }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RETAILITEMSERIALNUMBERS", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        public virtual List<SerialNumber> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format(QueryTemplates.BaseQuery("RETAILITEMSERIALNUMBERS", "SN"),
                    QueryPartGenerator.InternalColumnGenerator(serialNumberColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    string.Empty,
                    string.Empty);

                return Execute<SerialNumber>(entry, cmd, CommandType.Text, PopulateSerialNumber);
            }
        }

        private string GetInternalSortColumn(SerialNumberSorting sorting)
        {
            switch (sorting)
            {
                case SerialNumberSorting.ItemID:
                    return "RI.ITEMID";
                case SerialNumberSorting.ItemDescription:
                    return "RI.ITEMNAME";
                case SerialNumberSorting.ItemVariant:
                    return "RI.VARIANTNAME";
                case SerialNumberSorting.SerialNumber:
                    return "SN.SERIALNUMBER";
                case SerialNumberSorting.TypeOfSerial:
                    return "SN.TYPEOFSERIAL";
                case SerialNumberSorting.Sold:
                    return "SN.USEDDATE";
                case SerialNumberSorting.Reference:
                    return "SN.REFERENCE";
                case SerialNumberSorting.ManualEntry:
                    return "SN.MANUALENTRY";
                default:
                    return string.Empty;
            }
        }

        private string GetExternalSortColumn(SerialNumberSorting sorting)
        {
            switch (sorting)
            {
                case SerialNumberSorting.ItemID:
                    return "SS.ITEMID";
                case SerialNumberSorting.ItemDescription:
                    return "SS.ITEMDESCRIPTION";
                case SerialNumberSorting.ItemVariant:
                    return "SS.ITEMVARIANT";
                case SerialNumberSorting.SerialNumber:
                    return "SS.SERIALNUMBER";
                case SerialNumberSorting.TypeOfSerial:
                    return "SS.TYPEOFSERIAL";
                case SerialNumberSorting.Sold:
                    return "SS.USEDDATE";
                case SerialNumberSorting.Reference:
                    return "SS.REFERENCE";
                case SerialNumberSorting.ManualEntry:
                    return "SS.MANUALENTRY";
                default:
                    return string.Empty;
            }
        }

        public virtual List<SerialNumber> GetActiveSerialNumbersByItem(IConnectionManager entry, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching)
        {
            SerialNumberFilter filter = new SerialNumberFilter();
            filter.ItemMasterID = itemMasterID;
            filter.SerialNumber = serialNumber;
            filter.SerialType = TypeOfSerial.SerialNumber;
            filter.SerialNumberBeginsWith = false;
            filter.ManualEntrySet = true;
            filter.ManualEntry = false;
            filter.ShowSoldSerialNumbers = false;
            filter.ShowReservedSerialNumbers = false;
            filter.RowFrom = rowFrom;
            filter.RowTo = rowTo;
            filter.SortBy = sortBy;
            filter.SortAscending = sortAscending;
            totalRecordsMatching = 0;
            return GetListByFilter(entry, filter, out totalRecordsMatching);
        }

        /// <summary>
        /// Gets all sold serial numbers for a specific item.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="itemMasterID"> Item master id</param>
        /// <param name="rowFrom">start from row</param>
        /// <param name="rowTo">to row</param>
        /// <param name="sortBy">sort by specific column</param>
        /// <param name="sortAscending">if true sort ascending, otherwise descending </param>
        /// <param name="totalRecordsMatching">the total number of records</param>
        /// <returns></returns>
        public virtual List<SerialNumber> GetSoldSerialNumbersByItem(IConnectionManager entry, RecordIdentifier itemMasterID, string serialNumber,
            int rowFrom, int rowTo, SerialNumberSorting sortBy, bool sortAscending, out int totalRecordsMatching)
        {
            totalRecordsMatching = 0;

            List<Join> joins = new List<Join>();
            List<Condition> conditions = new List<Condition>();
            List<TableColumn> columns = new List<TableColumn>();
            List<Condition> externalConditions = new List<Condition>();

            string internalSorting = string.Format("{0} {1}", GetInternalSortColumn(sortBy), sortAscending ? "ASC" : "DESC");
            string externalSorting = string.Format("{0} {1}", GetExternalSortColumn(sortBy), sortAscending ? "ASC" : "DESC");

            joins.Add(new Join
            {
                Condition = "RI.MASTERID = SN.ITEMMASTERID",
                JoinType = "INNER",
                Table = "RETAILITEM",
                TableAlias = "RI"
            });

            using (var cmd = entry.Connection.CreateCommand())
            {
                columns.AddRange(serialNumberColumns);

                if ((entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 || entry.Connection.DatabaseVersion == ServerVersion.Unknown))
                {
                    columns.Add(new TableColumn
                    {
                        ColumnName =
                            string.Format("ROW_NUMBER() OVER(order by {0})", internalSorting),
                        ColumnAlias = "ROW"
                    });
                    columns.Add(new TableColumn
                    {
                        ColumnName =
                            string.Format(
                                "COUNT(1) OVER ( ORDER BY {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )", internalSorting),
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

                if (itemMasterID != null && itemMasterID != RecordIdentifier.Empty)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " (SN.ITEMMASTERID = @itemMasterID) "
                    });

                    MakeParam(cmd, "itemMasterID", itemMasterID);
                }

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue =
                        " (SN.TYPEOFSERIAL = 0) "
                });
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue =
                        " (SN.USEDDATE IS NOT NULL) "
                });
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue =
                        " (SN.REFERENCE IS NOT NULL AND SN.REFERENCE <> '') "
                });

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEMSERIALNUMBERS", "SN", "ss"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Format("ORDER BY {0}", externalSorting));

                return Execute<SerialNumber, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateSerialNumberWithCount);
            }
        }

        public virtual List<SerialNumber> GetListByFilter(IConnectionManager entry, SerialNumberFilter filter, out int totalRecordsMatching)
        {
            totalRecordsMatching = 0;
            if (filter == null)
            {
                return new List<SerialNumber>();
            }

            List<Join> joins = new List<Join>();
            List<Condition> conditions = new List<Condition>();
            List<TableColumn> columns = new List<TableColumn>();
            List<Condition> externalConditions = new List<Condition>();

            string internalSorting = string.Format("{0} {1}", GetInternalSortColumn(filter.SortBy), filter.SortAscending ? "ASC" : "DESC");
            string externalSorting = string.Format("{0} {1}", GetExternalSortColumn(filter.SortBy), filter.SortAscending ? "ASC" : "DESC");

            joins.AddRange(listJoins);

            using (var cmd = entry.Connection.CreateCommand())
            {
                columns.AddRange(serialNumberColumns);

                if (!((SqlServerConnection)entry.Connection).IsConnected)
                {
                    ((SqlServerConnection)entry.Connection).Open();
                }

                if ((entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 || entry.Connection.DatabaseVersion == ServerVersion.Unknown))
                {
                    columns.Add(new TableColumn
                    {
                        ColumnName =
                            string.Format("ROW_NUMBER() OVER(order by {0})", internalSorting),
                        ColumnAlias = "ROW"
                    });
                    columns.Add(new TableColumn
                    {
                        ColumnName =
                            string.Format(
                                "COUNT(1) OVER ( ORDER BY {0} RANGE BETWEEN UNBOUNDED PRECEDING AND UNBOUNDED FOLLOWING )", internalSorting),
                        ColumnAlias = "ROW_COUNT"
                    });
                    externalConditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ss.ROW between @rowFrom and @rowTo"
                    });
                    MakeParam(cmd, "rowFrom", filter.RowFrom, SqlDbType.Int);
                    MakeParam(cmd, "rowTo", filter.RowTo, SqlDbType.Int);
                }

                if (filter.ItemMasterID != null && filter.ItemMasterID != RecordIdentifier.Empty)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " (SN.ITEMMASTERID = @itemMasterID) "
                    });

                    MakeParam(cmd, "itemMasterID", filter.ItemMasterID);
                }

                if (!string.IsNullOrEmpty(filter.Description))
                {
                    string searchString = PreProcessSearchText(filter.Description, true, filter.DescriptionBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " (RI.ITEMNAME Like @description or RI.ITEMID Like @description or RI.VARIANTNAME Like @description or RI.NAMEALIAS Like @description) "
                    });

                    MakeParam(cmd, "description", searchString);
                }
                if (!string.IsNullOrEmpty(filter.Variant))
                {
                    string searchString = PreProcessSearchText(filter.Variant, true, filter.VariantBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " (RI.VARIANTNAME LIKE @variant) "
                    });

                    MakeParam(cmd, "variant", searchString);
                }
                if (!string.IsNullOrEmpty(filter.SerialNumber))
                {
                    string searchString = PreProcessSearchText(filter.SerialNumber, true, filter.SerialNumberBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " (SN.SERIALNUMBER LIKE @serialNumber) "
                    });

                    MakeParam(cmd, "serialNumber", searchString);
                }
                if (!string.IsNullOrEmpty(filter.Reference))
                {
                    string searchString = PreProcessSearchText(filter.Reference, true, filter.ReferenceBeginsWith);
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " (SN.REFERENCE LIKE @reference) "
                    });

                    MakeParam(cmd, "reference", searchString);
                }

                if (!string.IsNullOrEmpty(filter.Barcode))
                {
                    joins.Add(new Join
                    {
                        Condition = "RI.ITEMID = ibarcode.ITEMID",
                        JoinType = "LEFT OUTER",
                        Table = "INVENTITEMBARCODE",
                        TableAlias = "ibarcode"
                    });

                    filter.Barcode = (filter.BarcodeBeginsWith ? "" : "%") + filter.Barcode + "%";

                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ibarcode.ITEMBARCODE like @barCodeSearchString "
                    });

                    MakeParamNoCheck(cmd, "barCodeSearchString", filter.Barcode);

                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "ibarcode.DELETED = 0 "
                    });
                }
                if (filter.SerialType != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue =
                            " (SN.TYPEOFSERIAL = @typeOfSerial) "
                    });

                    MakeParam(cmd, "typeOfSerial", (int)filter.SerialType.Value);
                }

                if (filter.ManualEntrySet)
                {
                    if (filter.ManualEntry)
                    {
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue =
                                    " (SN.MANUALENTRY = 1) "
                        });
                    }
                    else
                    {
                        conditions.Add(new Condition
                        {
                            Operator = "AND",
                            ConditionValue =
                                    " (SN.MANUALENTRY = 0) "
                        });
                    }
                }

                if (filter.SpecialGroup != null)
                {
                    joins.Add(new Join
                    {
                        Condition = "SG.MEMBERMASTERID = RI.MASTERID AND SG.GROUPID = @specialGroupID",
                        JoinType = "INNER",
                        Table = "SPECIALGROUPITEMS",
                        TableAlias = "SG"
                    });

                    MakeParamNoCheck(cmd, "specialGroupID", (string)filter.SpecialGroup);
                }

                if (filter.SoldStartDate != null && filter.SoldEndDate != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"SN.USEDDATE BETWEEN @dateFrom AND @dateTo "
                    });

                    MakeParam(cmd, "dateFrom", filter.SoldStartDate, SqlDbType.DateTime);
                    MakeParam(cmd, "dateTo", filter.SoldEndDate, SqlDbType.DateTime);
                }
                else if (filter.SoldEndDate != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"SN.USEDDATE < @dateTo "
                    });

                    MakeParam(cmd, "dateTo", filter.SoldEndDate, SqlDbType.DateTime);
                }
                else if (filter.SoldStartDate != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"SN.USEDDATE > @dateFrom "
                    });

                    MakeParam(cmd, "dateFrom", filter.SoldStartDate, SqlDbType.DateTime);
                }

                if (!filter.ShowSoldSerialNumbers)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"SN.USEDDATE IS NULL "
                    });
                }
                if (!filter.ShowReservedSerialNumbers)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"SN.RESERVED = 0 "
                    });
                }

                if (filter.RetailGroup != null)
                {
                    conditions.Add(new Condition { Operator = "AND", ConditionValue = "RI.RETAILGROUPMASTERID = @retailGroup " });

                    MakeParam(cmd, "retailGroup", (Guid)filter.RetailGroup, SqlDbType.UniqueIdentifier);
                }

                if (filter.RetailDepartment != null)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = "RG.DEPARTMENTMASTERID = @retailDepartment "
                    });

                    MakeParam(cmd, "retailDepartment", (Guid)filter.RetailDepartment, SqlDbType.UniqueIdentifier);
                }

                if(filter.ShowDeletedItems)
                {
                    conditions.Add(new Condition
                    {
                        Operator = "AND",
                        ConditionValue = $"RI.DELETED = 1"
                    });
                }

                cmd.CommandText = string.Format(QueryTemplates.PagingQuery("RETAILITEMSERIALNUMBERS", "SN", "ss"),
                    QueryPartGenerator.ExternalColumnGenerator(columns, "ss"),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    QueryPartGenerator.ConditionGenerator(externalConditions),
                    string.Format("ORDER BY {0}", externalSorting));

                return Execute<SerialNumber, int>(entry, cmd, CommandType.Text, ref totalRecordsMatching, PopulateSerialNumberWithCount);
            }
        }

        public virtual SerialNumber Get(IConnectionManager entry, RecordIdentifier ID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "SN.ID = @id"
                });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEMSERIALNUMBERS", "SN"),
                    QueryPartGenerator.InternalColumnGenerator(serialNumberColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "id", (string)ID);

                var serialNumbers = Execute<SerialNumber>(entry, cmd, CommandType.Text, PopulateSerialNumber);

                return (serialNumbers.Count > 0) ? serialNumbers[0] : null;
            }
        }

        public virtual SerialNumber GetByItemAndSerialNumber(IConnectionManager entry, RecordIdentifier itemMasterID, string serialNumber)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "SN.ITEMMASTERID = @itemMasterID"
                });

                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = "SN.SERIALNUMBER = @serialNumber"
                });

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("RETAILITEMSERIALNUMBERS", "SN"),
                    QueryPartGenerator.InternalColumnGenerator(serialNumberColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                MakeParam(cmd, "itemMasterID", (string)itemMasterID);
                MakeParam(cmd, "serialNumber", (string)serialNumber);

                var serialNumbers = Execute<SerialNumber>(entry, cmd, CommandType.Text, PopulateSerialNumber);

                return (serialNumbers.Count > 0) ? serialNumbers[0] : null;
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            ValidateSecurity(entry, Permission.ManageSerialNumbers);

            SqlServerStatement statement = new SqlServerStatement("RETAILITEMSERIALNUMBERS", StatementType.Delete);
            statement.AddCondition("ID", (string)ID);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Serial number is marked as being reserved. If it is manually entered, then the serial number will be added to the database and marked as reserved.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="serialNumber"></param>
        public virtual void Reserve(IConnectionManager entry, SerialNumber serialNumber)
        {
            serialNumber.Reserved = true;
            Save(entry, serialNumber);
        }

        /// <summary>
        /// The list of serial numbers will be cleared. If they were reserved than this flag is removed, if they were manually entered than they will be deleted.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="serialNumbers"></param>
        public virtual void ClearReserve(IConnectionManager entry, List<SerialNumber> serialNumbers)
        {
            if (serialNumbers == null || serialNumbers.Count == 0)
            {
                return;
            }

            using (var updateCmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();

                conditions = new List<Condition>();
                string updateSQL = @"UPDATE RETAILITEMSERIALNUMBERS
                                              SET RESERVED = 0, 
                                                  USEDDATE = NULL,
                                                  REFERENCE = ''
                                              {0} ";
                serialNumbers.ForEach(sn =>
                {
                    int index = serialNumbers.IndexOf(sn);
                    conditions.Add(new Condition
                    {
                        Operator = "OR",
                        ConditionValue = "((ITEMMASTERID = @itemMasterID_" + index + ") AND (SERIALNUMBER = @serialNumber_" + index + "))"
                    });
                    MakeParam(updateCmd, "itemMasterID_" + index, (string)sn.ItemMasterID);
                    MakeParam(updateCmd, "serialNumber_" + index, (string)sn.SerialNo);
                });

                updateCmd.CommandText = string.Format(
                updateSQL,
                QueryPartGenerator.ConditionGenerator(conditions)
                );

                Execute<SerialNumber>(entry, updateCmd, CommandType.Text, PopulateSerialNumber);
            }

            using (var deleteCmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();

                conditions = new List<Condition>();
                string deleteSQL = @"DELETE SN FROM RETAILITEMSERIALNUMBERS SN {0} ";
                serialNumbers.ForEach(sn =>
                {
                    int index = serialNumbers.IndexOf(sn);
                    conditions.Add(new Condition
                    {
                        Operator = "OR",
                        ConditionValue = "((ITEMMASTERID = @itemMasterID_" + index + ") AND (SERIALNUMBER = @serialNumber_" + index + ") AND MANUALENTRY = 1)"
                    });
                    MakeParam(deleteCmd, "itemMasterID_" + index, (string)sn.ItemMasterID);
                    MakeParam(deleteCmd, "serialNumber_" + index, (string)sn.SerialNo);
                });

                deleteCmd.CommandText = string.Format(
                deleteSQL,
                QueryPartGenerator.ConditionGenerator(conditions)
                );

                Execute<SerialNumber>(entry, deleteCmd, CommandType.Text, PopulateSerialNumber);
            }
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            return RecordExists(entry, "RETAILITEMSERIALNUMBERS", "ID", ID, false);
        }

        /// <summary>
        /// Saves a serial number. If it comes from excel import and there is already a serial number instance based on serial number and item id, then only the type will be updated.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="serialNumber"></param>
        /// <param name="excelImport"></param>
        /// <returns>Returns true is the save was successfully. False otherwise</returns>
        public virtual bool Save(IConnectionManager entry, SerialNumber serialNumber, bool excelImport = false)
        {
            SqlServerStatement statement = new SqlServerStatement("RETAILITEMSERIALNUMBERS");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageSerialNumbers);

            serialNumber.Validate();

            bool isNew = false;
            if (serialNumber.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                serialNumber.ID = (RecordIdentifier)Guid.NewGuid();
            }

            SerialNumber sn = GetByItemAndSerialNumber(entry, serialNumber.ItemMasterID, serialNumber.SerialNo);

            if (isNew && sn == null)
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("ID", (Guid)serialNumber.ID, SqlDbType.UniqueIdentifier);
                serialNumber.CreateDate = DateTime.Now;
            }
            else if (isNew && sn != null && excelImport)
            {
                statement.StatementType = StatementType.Update;
                sn.SerialType = serialNumber.SerialType;
                serialNumber = sn;
                statement.AddCondition("ID", (Guid)serialNumber.ID, SqlDbType.UniqueIdentifier);
            }
            else if (sn == null || sn.ID == serialNumber.ID)
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("ID", (Guid)serialNumber.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                return false;
            }

            statement.AddField("ITEMMASTERID", (Guid)serialNumber.ItemMasterID, SqlDbType.UniqueIdentifier);
            statement.AddField("SERIALNUMBER", serialNumber.SerialNo);
            statement.AddField("TYPEOFSERIAL", (int)serialNumber.SerialType, SqlDbType.Int);
            statement.AddField("CREATEDDATE", serialNumber.CreateDate, SqlDbType.DateTime);
            if (serialNumber.UsedDate.HasValue)
            {
                statement.AddField("USEDDATE", serialNumber.UsedDate.HasValue, SqlDbType.DateTime);
            }
            else
            {
                statement.AddField("USEDDATE", DBNull.Value, SqlDbType.DateTime);
            }
            statement.AddField("REFERENCE", serialNumber.Reference);
            statement.AddField("MANUALENTRY", serialNumber.ManualEntry, SqlDbType.Bit);
            statement.AddField("RESERVED", serialNumber.Reserved, SqlDbType.Bit);

            entry.Connection.ExecuteStatement(statement);

            return true;
        }

        public virtual void Save(IConnectionManager entry, SerialNumber serialNumber)
        {
            Save(entry, serialNumber, false);
        }
        /// <summary>
        /// Marks the items in the list as being sold (used). Part of the transaction conclude.
        /// Items are marked as used, set if manually entered and set the receiptId.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="serialNumbers">The list of serial numbers to be verified and updated</param>
        /// <returns></returns>
        public virtual void UseSerialNumbers(IConnectionManager entry, List<SerialNumber> serialNumbers)
        {
            if (serialNumbers == null || serialNumbers.Count == 0)
            {
                return;
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<Condition> conditions = new List<Condition>();
                //Mark serial numbers as sold -> set UsedDate to now and link the reference to the receipt.

                string reference = serialNumbers.FirstOrDefault().Reference;
                conditions = new List<Condition>();
                using (var updateCmd = entry.Connection.CreateCommand())
                {
                    string updateSQL = @"UPDATE RETAILITEMSERIALNUMBERS
                                              SET USEDDATE = @usedDate,
                                              REFERENCE = @reference
                                              {0} ";
                    MakeParam(updateCmd, "usedDate", DateTime.Now, SqlDbType.DateTime);
                    MakeParam(updateCmd, "reference", reference);

                    serialNumbers.ForEach(sn =>
                    {
                        int index = serialNumbers.IndexOf(sn);
                        conditions.Add(new Condition
                        {
                            Operator = "OR",
                            ConditionValue = "((ITEMMASTERID = @itemMasterID_" + index + ") AND (SERIALNUMBER = @serialNumber_" + index + "))"
                        });
                        MakeParam(updateCmd, "itemMasterID_" + index, (string)sn.ItemMasterID);
                        MakeParam(updateCmd, "serialNumber_" + index, (string)sn.SerialNo);
                    });

                    updateCmd.CommandText = string.Format(
                    updateSQL,
                    QueryPartGenerator.ConditionGenerator(conditions)
                    );

                    Execute<SerialNumber>(entry, updateCmd, CommandType.Text, PopulateSerialNumber);

                }
            }
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }


    }
}
