using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    /// Data provider class for goods receiving documents
    /// </summary>
    public class GoodsReceivingDocumentData : SqlServerDataProviderBase, IGoodsReceivingDocumentData
    {
        private static List<TableColumn> countColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "ISNULL(COUNT(GOODSRECEIVINGID), 0) ", ColumnAlias = "TOTAL"}
        };

        private static List<TableColumn> goodsReceivingColumns = new List<TableColumn>
        {

            new TableColumn {ColumnName = "GOODSRECEIVINGID " , TableAlias = "a"},
            new TableColumn {ColumnName = "PURCHASEORDERID " , TableAlias = "a"},
            new TableColumn {ColumnName = "STATUS " , TableAlias = "a"},
            new TableColumn {ColumnName = "DESCRIPTION", ColumnAlias = "DESCRIPTION", IsNull = true, NullValue = "''", TableAlias = "b"},
            new TableColumn {ColumnName = "NAME " , ColumnAlias = "VENDORNAME", IsNull = true, NullValue = "''", TableAlias = "v"},
            new TableColumn {ColumnName = "ACCOUNTNUM ", ColumnAlias = "VENDORID", TableAlias = "v"},
            new TableColumn {ColumnName = "NAME " , ColumnAlias = "STORENAME", IsNull = true, NullValue = "''", TableAlias = "r"},
            new TableColumn {ColumnName = "STOREID " ,TableAlias = "b"},
            new TableColumn {ColumnName = "CREATEDDATE " , ColumnAlias = "CREATEDDATE", IsNull = true, NullValue = "0", TableAlias = "a"},
            new TableColumn {ColumnName = "POSTEDDATE " , ColumnAlias = "POSTEDDATE", IsNull = true, NullValue = "0", TableAlias = "a"}
        };

        private static List<Join> listJoins = new List<Join>
        {
            new Join
            {
                Condition = "a.PURCHASEORDERID = b.PURCHASEORDERID AND a.DATAAREAID = b.DATAAREAID",
                Table = "PURCHASEORDERS",
                TableAlias = "b"
            },
            new Join
            {
                Condition = "a.DATAAREAID = v.DATAAREAID and b.VENDORID = v.ACCOUNTNUM AND V.DELETED = 0",
                Table = "VENDTABLE",
                TableAlias = "v"
            },
            new Join
            {
                Condition = " r.DATAAREAID = a.DATAAREAID and r.STOREID = b.STOREID ",
                JoinType = "LEFT OUTER",
                Table = "RBOSTORETABLE",
                TableAlias = "r"
            }
        };

        private static string ResolveSort(GoodsReceivingDocumentSorting sort)
        {
            switch (sort)
            {
                case GoodsReceivingDocumentSorting.GoodsReceivingID:
                    return "a.GOODSRECEIVINGID";
                case GoodsReceivingDocumentSorting.Status:
                    return "a.STATUS";
                case GoodsReceivingDocumentSorting.VendorName:
                    return "VENDORNAME";
            }

            return "";
        }

        private static void PopulateGoodsReceivingDocument(IDataReader dr, GoodsReceivingDocument goodsReceivingDocument)
        {
            goodsReceivingDocument.GoodsReceivingID = (string)dr["GOODSRECEIVINGID"];
            goodsReceivingDocument.PurchaseOrderID = (string)dr["PURCHASEORDERID"];
            goodsReceivingDocument.Status = (GoodsReceivingStatusEnum)dr["STATUS"];
            goodsReceivingDocument.Description = (string)dr["DESCRIPTION"];
            goodsReceivingDocument.VendorName = (string)dr["VENDORNAME"];
            goodsReceivingDocument.VendorID = (string)dr["VENDORID"];
            goodsReceivingDocument.StoreName = (string)dr["STORENAME"];
            goodsReceivingDocument.StoreID = (string)dr["STOREID"];
            goodsReceivingDocument.CreatedDate = Date.FromAxaptaDate(dr["CREATEDDATE"]);
            goodsReceivingDocument.PostedDate = Date.FromAxaptaDate(dr["POSTEDDATE"]);
        }

        private static void PopulateGoodsReceivingDocumentWithLineTotals(IDataReader dr, GoodsReceivingDocument goodsReceivingDocument)
        {
            PopulateGoodsReceivingDocument(dr, goodsReceivingDocument);

            goodsReceivingDocument.TotalQuantity = AsDecimal(dr["TOTALQUANTITY"]);
            goodsReceivingDocument.NumberOfLines = AsInt(dr["NUMBEROFLINES"]);
        }

        /// <summary>
        /// Gets a list of all Goods Receiving Documents for a specific store. The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="storeID">The store ID</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all Goods Receiving Documents for a specific store</returns>
        public virtual List<GoodsReceivingDocument> GetGoodsReceivingDocumentsForStore(IConnectionManager entry, RecordIdentifier storeID, GoodsReceivingDocumentSorting sortBy, bool sortBackwards)
        {
            GoodsReceivingDocumentSearch searchCriteria = new GoodsReceivingDocumentSearch();
            searchCriteria.StoreID = storeID;

            return AdvancedSearch(entry, searchCriteria, sortBy, sortBackwards);
        }

        /// <summary>
        /// Gets a list of all Goods Receiving Documents for a specific vendor . The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="vendorID">The ID of the vendor</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all Goods Receiving Documents</returns>
        public virtual List<GoodsReceivingDocument> GetGoodsReceivingDocuments(IConnectionManager entry, RecordIdentifier vendorID, GoodsReceivingDocumentSorting sortBy, bool sortBackwards)
        {
            GoodsReceivingDocumentSearch searchCriteria = new GoodsReceivingDocumentSearch();
            searchCriteria.VendorID = vendorID;

            return AdvancedSearch(entry, searchCriteria, sortBy, sortBackwards);
        }

        private List<Condition> GenerateSearchConditions(IConnectionManager entry, IDbCommand cmd, GoodsReceivingDocumentSearch searchCriteria)
        {
            List<Condition> conditions = new List<Condition>();
            conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.DATAAREAID = @DATAAREAID " });
            MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

            if (!string.IsNullOrEmpty((string)searchCriteria.DocumentID))
            {
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.GOODSRECEIVINGID = @GOODSRECEIVINGID " });
                MakeParam(cmd, "GOODSRECEIVINGID", (string)searchCriteria.DocumentID);
            }

            if (!string.IsNullOrEmpty((string)searchCriteria.PurchaseOrderID))
            {
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.PURCHASEORDERID = @PURCHASEORDERID " });
                MakeParam(cmd, "PURCHASEORDERID", (string)searchCriteria.PurchaseOrderID);
            }

            if (!string.IsNullOrEmpty((string)searchCriteria.VendorID))
            {
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "b.VENDORID = @VENDORID " });
                MakeParam(cmd, "VENDORID", (string)searchCriteria.VendorID);
            }

            if (!string.IsNullOrEmpty((string)searchCriteria.StoreID))
            {
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "r.STOREID = @STOREID " });
                MakeParam(cmd, "STOREID", (string)searchCriteria.StoreID);
            }

            if (searchCriteria.Status != null)
            {
                conditions.Add(new Condition { Operator = "AND", ConditionValue = "a.STATUS = @STATUS " });
                MakeParam(cmd, "STATUS", (int)searchCriteria.Status, SqlDbType.Int);
            }

            searchCriteria.CreatedFrom = searchCriteria.CreatedFrom ?? Date.Empty;
            searchCriteria.CreatedTo = searchCriteria.CreatedTo ?? Date.Empty;
            if (searchCriteria.CreatedFrom != Date.Empty || searchCriteria.CreatedTo != Date.Empty)
            {
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = $"A.CREATEDDATE BETWEEN @CREATEDFROM AND @CREATEDTO "
                });

                MakeParam(cmd, "CREATEDFROM", searchCriteria.CreatedFrom.DateTime, SqlDbType.DateTime);
                MakeParam(cmd, "CREATEDTO", searchCriteria.CreatedTo.DateTime, SqlDbType.DateTime);
            }

            searchCriteria.PostedFrom = searchCriteria.PostedFrom ?? Date.Empty;
            searchCriteria.PostedTo = searchCriteria.PostedTo ?? Date.Empty;
            if (searchCriteria.PostedFrom != Date.Empty || searchCriteria.PostedTo != Date.Empty)
            {
                conditions.Add(new Condition
                {
                    Operator = "AND",
                    ConditionValue = $"A.POSTEDDATE BETWEEN @POSTEDFROM AND @POSTEDTO "
                });

                MakeParam(cmd, "POSTEDFROM", searchCriteria.PostedFrom.DateTime, SqlDbType.DateTime);
                MakeParam(cmd, "POSTEDTO", searchCriteria.PostedTo.DateTime, SqlDbType.DateTime);
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
                                $@" (A.GOODSRECEIVINGID Like @DESCRIPTION{index} 
                                        or A.PURCHASEORDERID LIKE @DESCRIPTION{index} 
                                        OR b.DESCRIPTION LIKE @DESCRIPTION{index}) ",
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

        public virtual int CountSearchResults(IConnectionManager entry, GoodsReceivingDocumentSearch searchCriteria)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = GenerateSearchConditions(entry, cmd, searchCriteria);

                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("GOODSRECEIVING", "a"),
                    QueryPartGenerator.InternalColumnGenerator(countColumns),
                    QueryPartGenerator.JoinGenerator(listJoins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    string.Empty
                    );

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }

        /// <summary>
        /// Returns all goods receiving documents that are found using the search criteria set in parameter <see cref="GoodsReceivingDocumentSearch"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="searchCriteria">The search criteria to search by</param>
        /// <param name="sortBy">The sorting of the result set</param>
        /// <param name="sortBackwards">If true the sorting is backwards</param>
        /// <returns></returns>
        public virtual List<GoodsReceivingDocument> AdvancedSearch(IConnectionManager entry, GoodsReceivingDocumentSearch searchCriteria, GoodsReceivingDocumentSorting sortBy, bool sortBackwards)
        {
            ValidateSecurity(entry);

            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<TableColumn> columns = new List<TableColumn>(goodsReceivingColumns);
                List<Join> joins = new List<Join>(listJoins);

                string groupSortString = "";
                DataPopulator<GoodsReceivingDocument> populator = PopulateGoodsReceivingDocument;

                if(searchCriteria.IncludeLineTotals)
                {
                    groupSortString += "GROUP BY " + QueryPartGenerator.GroupByColumnGenerator(columns);

                    columns.Add(new TableColumn { ColumnName = "SUM(grl.RECEIVEDQUANTITY)", ColumnAlias = "TOTALQUANTITY" });
                    columns.Add(new TableColumn { ColumnName = "COUNT(grl.GOODSRECEIVINGID)", ColumnAlias = "NUMBEROFLINES" });
                    joins.Add(new Join { Table = "GOODSRECEIVINGLINE", TableAlias = "grl", JoinType = "LEFT", Condition = "a.GOODSRECEIVINGID = grl.GOODSRECEIVINGID" });
                    populator = PopulateGoodsReceivingDocumentWithLineTotals;
                }

                groupSortString += " ORDER BY " + ResolveSort(sortBy) + (sortBackwards ? " DESC" : " ASC");

                List<Condition> conditions = GenerateSearchConditions(entry, cmd, searchCriteria);
                cmd.CommandText = string.Format(
                    QueryTemplates.BaseQuery("GOODSRECEIVING", "a", searchCriteria.LimitResultTo),
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.JoinGenerator(joins),
                    QueryPartGenerator.ConditionGenerator(conditions),
                    groupSortString
                    );

                return Execute<GoodsReceivingDocument>(entry, cmd, CommandType.Text, populator);
            }
        }

        /// <summary>
        /// Gets a list of all Goods Receiving Documents . The list is sorted by the the parameter 'sort'
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Set to true if wanting backwards sort</param>
        /// <returns>A list of all Goods Receiving Documents</returns>
        public virtual List<GoodsReceivingDocument> GetGoodsReceivingDocuments(IConnectionManager entry, GoodsReceivingDocumentSorting sortBy, bool sortBackwards)
        {
            return AdvancedSearch(entry, new GoodsReceivingDocumentSearch(), sortBy, sortBackwards);
        }

        /// <summary>
        /// Checks if a goods receiving document with a given ID exists in the database.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document to check for</param>
        /// <returns>Whether a goods receiving document with a given ID exists in the database</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            return RecordExists(entry, "GOODSRECEIVING", "GOODSRECEIVINGID", goodsReceivingDocumentID);
        }

        /// <summary>
        /// Deletes a goods receiving document with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            DeleteRecord(entry, "GOODSRECEIVINGLINE", "GOODSRECEIVINGID", goodsReceivingDocumentID, BusinessObjects.Permission.ManageGoodsReceivingDocuments);
            DeleteRecord(entry, "GOODSRECEIVING", "GOODSRECEIVINGID", goodsReceivingDocumentID, BusinessObjects.Permission.ManageGoodsReceivingDocuments);
        }

        /// <summary>
        /// Gets a goods receiving document with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsReceivingDocumentID">The ID of the goods receiving document to get</param>
        /// <returns>A goods receiving document with a given ID</returns>
        public virtual GoodsReceivingDocument Get(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            GoodsReceivingDocumentSearch searchCriteria = new GoodsReceivingDocumentSearch();
            searchCriteria.DocumentID = goodsReceivingDocumentID;

            List<GoodsReceivingDocument> result = AdvancedSearch(entry, searchCriteria, GoodsReceivingDocumentSorting.GoodsReceivingID, false);

            return result != null ? result.FirstOrDefault() : new GoodsReceivingDocument();
        }

        /// <summary>
        /// Saves a given goods receiving document into the database
        /// </summary>
        /// <remarks>Requires the 'ManageGoodsReceivingDocuments' permission</remarks>
        /// <param name="entry">The entry into the database</param>
        /// <param name="goodsRecevingDocument">The goods receiving document to save</param>
        public virtual void Save(IConnectionManager entry, GoodsReceivingDocument goodsRecevingDocument)
        {
            SqlServerStatement statement = new SqlServerStatement("GOODSRECEIVING", false);

            ValidateSecurity(entry, BusinessObjects.Permission.ManageGoodsReceivingDocuments);

            if (!Exists(entry, goodsRecevingDocument.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("GOODSRECEIVINGID", (string)goodsRecevingDocument.GoodsReceivingID);
                statement.AddKey("PURCHASEORDERID", (string)goodsRecevingDocument.PurchaseOrderID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("GOODSRECEIVINGID", (string)goodsRecevingDocument.GoodsReceivingID);
                statement.AddCondition("PURCHASEORDERID", (string)goodsRecevingDocument.PurchaseOrderID);
            }

            statement.AddField("STATUS", (int)goodsRecevingDocument.Status, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool HasPostedLines(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                List<Condition> conditions = new List<Condition>
                {
                    new Condition
                    {
                        ConditionValue = "GOODSRECEIVINGID = @GOODSRECEIVINGDOCUMENTID",
                        Operator = "AND"
                    },
                    new Condition
                    {
                        ConditionValue = "POSTED = 1",
                        Operator = "AND"
                    }
                };
                cmd.CommandText =
                    $@"SELECT TOP 1 1
                        FROM GOODSRECEIVINGLINE 
                        {QueryPartGenerator.ConditionGenerator(conditions)}";

                MakeParam(cmd, "GOODSRECEIVINGDOCUMENTID", (string)goodsReceivingDocumentID);
                var hasLines = entry.Connection.ExecuteScalar(cmd);
                return hasLines != null;
            }
            
        }

        public virtual bool HasLines(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                
                Condition condition = new Condition
                {
                    ConditionValue = "GOODSRECEIVINGID = @GOODSRECEIVINGDOCUMENTID", Operator = "AND"
                };
                cmd.CommandText =
                    $@"SELECT TOP 1 1
                        FROM GOODSRECEIVINGLINE 
                        {QueryPartGenerator.ConditionGenerator(condition)}";                 
                
                MakeParam(cmd, "GOODSRECEIVINGDOCUMENTID", (string)goodsReceivingDocumentID);
                var hasLines = entry.Connection.ExecuteScalar(cmd);
                return hasLines != null;
            }
          
        }

        // This functions assumes that the connection between Purchase orders and Goods receiving documents is one-to-one. This is not assured in the database.
        public virtual RecordIdentifier GetPurchaseOrderID(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT PURCHASEORDERID " +
                    "FROM GOODSRECEIVING " +
                    "WHERE DATAAREAID = @DATAAREAID AND GOODSRECEIVINGID = @GOODSRECEIVINGDOCUMENTID";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "GOODSRECEIVINGDOCUMENTID", (string)goodsReceivingDocumentID);

                return (string)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual bool FullyReceived(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            if(!IsPosted(entry, goodsReceivingDocumentID))
            {
                return false;
            }

            RecordIdentifier purchaseOrderID = GetPurchaseOrderID(entry, goodsReceivingDocumentID);
            List<PurchaseOrderLine> pols = Providers.PurchaseOrderLineData.GetPurchaseOrderLines(entry, purchaseOrderID, RecordIdentifier.Empty, false);

            List<GoodsReceivingDocumentLine> grdls = Providers.GoodsReceivingDocumentLineData.GetGoodsReceivingDocumentLines(entry, goodsReceivingDocumentID);

            foreach (PurchaseOrderLine purchaseOrderLine in pols.Where(p => !p.ItemInventoryExcluded))
            {
                decimal orderedQty = purchaseOrderLine.Quantity;

                decimal receivedQty = (from g in grdls
                                   where g.PurchaseOrderLineNumber == purchaseOrderLine.LineNumber
                                   select g.ReceivedQuantity).Sum();

                if (orderedQty > receivedQty)
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsPosted(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    @"SELECT STATUS
					  FROM GOODSRECEIVING
					  WHERE GOODSRECEIVINGID = @GOODSRECEIVINGDOCUMENTID";

                MakeParam(cmd, "GOODSRECEIVINGDOCUMENTID", (string)goodsReceivingDocumentID);

                int status = (int)entry.Connection.ExecuteScalar(cmd);
                return status == (int)GoodsReceivingStatusEnum.Posted;
            }
        }

        public virtual bool AllLinesPosted(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            List<GoodsReceivingDocumentLine> grdls = Providers.GoodsReceivingDocumentLineData.GetGoodsReceivingDocumentLines(entry, goodsReceivingDocumentID);

            return grdls.All(line => line.Posted);
        }

        /// <summary>
        /// Updates the status of a given goods receiving document if necessary. Returns whether the document was updated or not.
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="goodsReceivingDocumentID"></param>
        /// <returns></returns>
        public virtual bool UpdateStatus(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            GoodsReceivingDocument grd = Get(entry, goodsReceivingDocumentID);

            // If all lines have been posted and the goods receiving document is
            // fully received, set its status as 'Posted'
            bool fullyReceived = FullyReceived(entry, goodsReceivingDocumentID);
            bool allLinesPosted = AllLinesPosted(entry, goodsReceivingDocumentID);

            GoodsReceivingStatusEnum newStatus;
            if (fullyReceived && allLinesPosted)
            {
                newStatus = GoodsReceivingStatusEnum.Posted;
            }
            else
            {
                newStatus = GoodsReceivingStatusEnum.Active;
            }

            if (grd.Status != newStatus)
            {
                grd.Status = newStatus;
                Save(entry, grd);
                return true;
            }

            return false;
        }

        public virtual List<GoodsReceivingDocumentLine> GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(IConnectionManager entry, RecordIdentifier purchaseOrderLineID)
        {
            RecordIdentifier purchaseOrderID = purchaseOrderLineID.PrimaryID;
            RecordIdentifier goodsReceivingDocumentID = purchaseOrderID;
            RecordIdentifier purchaseOrderLineNumber = purchaseOrderLineID.SecondaryID;

            List<GoodsReceivingDocumentLine> grdlsForThePurchaseOrder = Providers.GoodsReceivingDocumentLineData.GetGoodsReceivingDocumentLines(entry, goodsReceivingDocumentID);

            List<GoodsReceivingDocumentLine> grdlsMatchingThePurchaseOrderLine =
                (from g in grdlsForThePurchaseOrder
                 where g.purchaseOrderLine.LineNumber == purchaseOrderLineNumber
                 select g).ToList();

            return grdlsMatchingThePurchaseOrderLine;
        }

        public virtual void DeleteLinesForAPurchaseOrderLine(IConnectionManager entry, RecordIdentifier purchaseOrderLineID)
        {
            List<GoodsReceivingDocumentLine> grdls = GetGoodsReceivingDocumentLinesForAPurchaseOrderLine(entry, purchaseOrderLineID);

            foreach (GoodsReceivingDocumentLine line in grdls)
            {
                Providers.GoodsReceivingDocumentLineData.Delete(entry, line.ID);
            }
        }

        public virtual void DeleteGoodsReceivingDocumentsForPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaseOrderID)
        {
            List<RecordIdentifier> ids = GetGoodsReceivingDocumentIDsForPurchaseOrder(entry, purchaseOrderID);

            foreach (RecordIdentifier id in ids)
            {
                Delete(entry, id);
            }
        }

        public virtual int CountDocuments(IConnectionManager entry)
        {
            ValidateSecurity(entry);

            return Count(entry, "GOODSRECEIVING");
        }

        public virtual GoodsReceivingPostResult PostGoodsReceivingDocument(IConnectionManager entry, RecordIdentifier goodsReceivingDocumentID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand("spINVENTORY_PostAllGoodsReceivingLines"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;

                MakeParam(cmd, "GOODSRECEIVINGID", goodsReceivingDocumentID.StringValue);
                MakeParam(cmd, "USERID", entry.CurrentUser.ID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                SqlParameter postingResult = MakeParam(cmd, "POSTINGRESULT", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (GoodsReceivingPostResult)((int)postingResult.Value);
            }
        }

        internal static List<RecordIdentifier> GetGoodsReceivingDocumentIDsForPurchaseOrder(IConnectionManager entry, RecordIdentifier purchaseOrderID)
        {
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT GOODSRECEIVINGID " +
                                   ",PURCHASEORDERID " +
                                  "FROM GOODSRECEIVING " +
                                  "WHERE PURCHASEORDERID = @PURCHASEORDERID AND DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "PURCHASEORDERID", (string)purchaseOrderID);

                List<RecordIdentifier> result = new List<RecordIdentifier>();
                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        result.Add((string)dr["GOODSRECEIVINGID"]);
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
                return result;
            }
        }
    }
}
