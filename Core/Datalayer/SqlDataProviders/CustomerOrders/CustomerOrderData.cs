using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.CustomerOrders;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.CustomerOrders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.CustomerOrders
{
    public partial class CustomerOrderData : SqlServerDataProviderBase, ICustomerOrderData
    {
        /// <summary>
        /// Select statement format: 
        ///     Select 
        ///     -- Columns 
        ///     {0} 
        ///     FROM CUSTOMERORDERS A  
        /// </summary>
        private string NoCondition
        {
            get
            {
                return @"
    Select 
        -- Columns
        {0}
    FROM CUSTOMERORDERS A         
    ";
            }
        }

        /// <summary>
        /// Select statement format: 
        ///     Select 
        ///     -- Columns 
        ///     {0} 
        ///     FROM CUSTOMERORDERS A  
        ///     --Conditions 
        ///     {1}       
        /// </summary>
        private string WithCondition
        {
            get
            {
                return @"
    Select 
        -- Columns
        {0}
    FROM CUSTOMERORDERS A  
        --Conditions
        {1}       
    ";
            }
        }

        private static Dictionary<ColumnPopulation, List<TableColumn>> selectionColumns = new Dictionary<ColumnPopulation, List<TableColumn>>();

        private Dictionary<ColumnPopulation, List<TableColumn>> SelectionColumns
        {
            get
            {
                if (selectionColumns.Count == 0)
                {
                    selectionColumns.Add(ColumnPopulation.DataEntity, new List<TableColumn>
                    {
                        new TableColumn {ColumnName = "ID", TableAlias = "A"},
                        new TableColumn {ColumnName = "CUSTOMERID", TableAlias = "A"},
                        new TableColumn {ColumnName = "REFERENCE", TableAlias = "A"},
                        new TableColumn {ColumnName = "SOURCE", TableAlias = "A"},
                        new TableColumn {ColumnName = "DELIVERY", TableAlias = "A"},
                        new TableColumn {ColumnName = "EXPIRES", TableAlias = "A"},
                        new TableColumn {ColumnName = "DELIVERYLOCATIONID", TableAlias = "A"},
                        new TableColumn {ColumnName = "ORDERTYPE", TableAlias = "A"},
                        new TableColumn {ColumnName = "ORDERXML", TableAlias = "A"},
                        new TableColumn {ColumnName = "CREATEDSTOREID", TableAlias = "A"},
                        new TableColumn {ColumnName = "CREATEDBY", TableAlias = "A"},
                        new TableColumn {ColumnName = "CREATEDTERMINALID", TableAlias = "A"},
                        new TableColumn {ColumnName = "CREATEDDATE", TableAlias = "A"},
                        new TableColumn {ColumnName = "COMMENT", TableAlias = "A"},
                        new TableColumn {ColumnName = "STATUS", TableAlias = "A"},
                    });
                }
                return selectionColumns;
            }
        }

        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new NotImplementedException();
            //DeleteRecord<CustomerOrder>(entry, "CUSTOMERORDERS", "ID", ID, BusinessObjects.Permission.ManageCustomerOrders);
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            if (ID.DBType == SqlDbType.NVarChar)
            {
                return RecordExists(entry, "CUSTOMERORDERS", "REFERENCE", ID, false);
            }

            return RecordExists(entry, "CUSTOMERORDERS", "ID", ID, false);
        }

        /// <summary>
        /// Generates a reference number for a customer order. If no number sequence is selected in the settings object then
        /// a default number sequence is used
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="orderSettings">The settings for the customer order or quote.</param>
        /// <returns></returns>
        public RecordIdentifier GenerateReference(IConnectionManager entry, CustomerOrderSettings orderSettings)
        {
            SequenceID = orderSettings.NumberSeries;
            return DataProviderFactory.Instance.Get<INumberSequenceData, NumberSequence>().GenerateNumberFromSequence(entry, this);
        }

        /// <summary>
        /// Returns a specific customer order
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="ID">The unique ID of the customer order to retrieve</param>
        /// <returns>A customer order object</returns>
        public CustomerOrder Get(IConnectionManager entry, RecordIdentifier ID)
        {
            List<CustomerOrder> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = SelectionColumns[ColumnPopulation.DataEntity].ToList();
                List<Condition> conditions = null;
                conditions = ID.DBType == SqlDbType.UniqueIdentifier ? 
                        new List<Condition> { new Condition { Operator = "AND", ConditionValue = "A.ID = @ID" } } : 
                        new List<Condition> {new Condition {Operator = "AND", ConditionValue = "A.REFERENCE = @ID"}};
                string commandText = WithCondition;

                cmd.CommandText = string.Format(commandText,
                    QueryPartGenerator.InternalColumnGenerator(columns),
                    QueryPartGenerator.ConditionGenerator(conditions));

                MakeParam(cmd, "ID", ID);

                records = Execute<CustomerOrder>(entry, cmd, CommandType.Text, PopulateCustomerOrder);
            }

            return records.Count > 0 ? records.FirstOrDefault() : new CustomerOrder();
        }

        /// <summary>
        /// Returns a list of all existing customer orders
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of customer orders</returns>
        public List<CustomerOrder> GetList(IConnectionManager entry)
        {
            List<CustomerOrder> records;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                List<TableColumn> columns = SelectionColumns[ColumnPopulation.DataEntity].ToList();
                string commandText = WithCondition;

                cmd.CommandText = string.Format(commandText, QueryPartGenerator.InternalColumnGenerator(columns), "");

                records = Execute<CustomerOrder>(entry, cmd, CommandType.Text, PopulateCustomerOrder);
            }

            return records;
        }

        protected virtual void PopulateCustomerOrder(IDataReader dr, CustomerOrder order)
        {
            order.ID = (Guid)dr["ID"];
            order.Reference = (string) dr["REFERENCE"];
            order.Comment = (string)dr["COMMENT"];
            order.CreatedDate = (DateTime) dr["CREATEDDATE"];
            order.ExpiryDate = new Date((DateTime)dr["EXPIRES"]);
            order.OrderXML = dr["ORDERXML"].ToString();
            order.DeliveryLocation = (string)dr["DELIVERYLOCATIONID"];
            order.StaffID = (string)dr["CREATEDBY"];
            order.StoreID = (string)dr["CREATEDSTOREID"];
            order.TerminalID = (string)dr["CREATEDTERMINALID"];
            order.Source = (Guid)dr["SOURCE"];
            order.Delivery = (Guid)dr["DELIVERY"];
            order.Status = (CustomerOrderStatus) (int)dr["STATUS"];
            order.CustomerID = (string) dr["CUSTOMERID"];

            RecordIdentifier orderType = (Guid)dr["ORDERTYPE"];
            order.OrderType = orderType.StringValue.ToUpperInvariant() == CustomerOrderSettingsConstants.ConstCustomerOrder.ToUpperInvariant() ? CustomerOrderType.CustomerOrder : CustomerOrderType.Quote;
        }

        /// <summary>
        /// Saves the customer order to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The customer order to be saved</param>
        public void Save(IConnectionManager entry, CustomerOrder item)
        {
            Save(entry, item, false);
        }


        /// <summary>
        /// Saves the customer order to the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="item">The customer order to be saved</param>
        /// <param name="excludeOrderXML">If true then the orderXML value is not saved</param>
        public void Save(IConnectionManager entry, CustomerOrder item, bool excludeOrderXML)
        {
            var statement = new SqlServerStatement("CUSTOMERORDERS");
            statement.UpdateColumnOptimizer = item;

            ValidateSecurity(entry, BusinessObjects.Permission.ManageCustomerOrders);

            var isNew = false;
            if (item.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                item.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, item.ID))
            {
                statement.StatementType = StatementType.Insert;

                if (string.IsNullOrWhiteSpace(item.ID.StringValue))
                {
                    item.ID = Guid.NewGuid();
                }

                statement.AddKey("ID", new Guid((string)item.ID), SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                
                statement.AddCondition("ID", new Guid((string)item.ID), SqlDbType.UniqueIdentifier);
            }

            CustomerOrderSettings orderSettings = Providers.CustomerOrderSettingsData.Get(entry, item.OrderType);

            if (string.IsNullOrEmpty(item.Reference.StringValue))
            {
                item.Reference = GenerateReference(entry, orderSettings);
            }

            statement.AddField("REFERENCE", (string)item.Reference);
            statement.AddField("CUSTOMERID", (string)item.CustomerID);

            item.Source = item.Source ?? Guid.Empty;
            item.Delivery = item.Delivery ?? Guid.Empty;
            item.ExpiryDate = item.ExpiryDate ?? new Date(DateTime.MinValue);

            statement.AddField("SOURCE", item.Source.DBType == SqlDbType.UniqueIdentifier ? (Guid) item.Source : Guid.Empty, SqlDbType.UniqueIdentifier);
            statement.AddField("DELIVERY", item.Delivery.DBType == SqlDbType.UniqueIdentifier ? (Guid) item.Delivery : Guid.Empty, SqlDbType.UniqueIdentifier);
            statement.AddField("EXPIRES", item.ExpiryDate.ToAxaptaSQLDate(), SqlDbType.DateTime);

            item.DeliveryLocation = item.DeliveryLocation ?? RecordIdentifier.Empty;
            statement.AddField("DELIVERYLOCATIONID", (string)item.DeliveryLocation);

            if (orderSettings.ID.DBType != SqlDbType.UniqueIdentifier)
            {
                orderSettings.ID = new Guid((string)orderSettings.ID);
            }
            statement.AddField("ORDERTYPE", (Guid)orderSettings.ID, SqlDbType.UniqueIdentifier);

            if (!excludeOrderXML)
            {
                statement.AddField("ORDERXML", item.OrderXML);
            }

            statement.AddField("CREATEDSTOREID", (string)item.StoreID);
            statement.AddField("CREATEDBY", (string)item.StaffID);
            statement.AddField("CREATEDTERMINALID", (string)item.TerminalID);

            item.Comment = item.Comment ?? "";
            statement.AddField("COMMENT", item.Comment);
            statement.AddField("STATUS", (int)item.Status, SqlDbType.Int);


            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Searches for customer orders that match the search parameters set in the <see cref="CustomerOrderSearch"/> object
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="totalRecordsMatching">How many records match the search criteria</param>
        /// <param name="numberOfRecordsToReturn">The number of records to return</param>
        /// <param name="searchCriteria">The criteria to search by</param>
        /// <returns></returns>
        public virtual List<CustomerOrder> AdvancedSearch(IConnectionManager entry,
            out int totalRecordsMatching,
            int numberOfRecordsToReturn,
            CustomerOrderSearch searchCriteria
            )
        {

            string whereConditions = "";

            using (var cmd = entry.Connection.CreateCommand())
            using (var cmdCount = entry.Connection.CreateCommand())
            {

                if (searchCriteria.ID != null && !string.IsNullOrEmpty(searchCriteria.ID.StringValue))
                {
                    whereConditions += " AND (C.ID = @ID) ";

                    MakeParam(cmd, "ID", new Guid((string)searchCriteria.ID), SqlDbType.UniqueIdentifier);
                    MakeParam(cmdCount, "ID", new Guid((string)searchCriteria.ID), SqlDbType.UniqueIdentifier);
                }

                if (!string.IsNullOrEmpty(searchCriteria.Reference.StringValue))
                {
                    searchCriteria.Reference = (searchCriteria.ReferenceBeginsWith ? "" : "%") + searchCriteria.Reference + "%";

                    whereConditions += " AND (C.REFERENCE LIKE @searchString) ";

                    MakeParam(cmd, "searchString", searchCriteria.Reference);
                    MakeParam(cmdCount, "searchString", searchCriteria.Reference);
                }

                if (!string.IsNullOrEmpty(searchCriteria.CustomerID.StringValue))
                {
                    whereConditions += " AND (C.CUSTOMERID = @customerID) ";

                    MakeParam(cmd, "customerID", searchCriteria.CustomerID);
                    MakeParam(cmdCount, "customerID", searchCriteria.CustomerID);
                }

                if (!string.IsNullOrEmpty(searchCriteria.Delivery.StringValue) && (searchCriteria.Delivery != Guid.Empty))
                {
                    whereConditions += " AND (C.DELIVERY = @delivery) ";

                    MakeParam(cmd, "delivery", new Guid((string)searchCriteria.Delivery), SqlDbType.UniqueIdentifier);
                    MakeParam(cmdCount, "delivery", new Guid((string)searchCriteria.Delivery), SqlDbType.UniqueIdentifier);
                }

                if (!string.IsNullOrEmpty(searchCriteria.Source.StringValue) && (searchCriteria.Source != Guid.Empty))
                {
                    whereConditions += " AND (C.SOURCE = @source) ";

                    MakeParam(cmd, "source", new Guid((string)searchCriteria.Source), SqlDbType.UniqueIdentifier);
                    MakeParam(cmdCount, "source", new Guid((string)searchCriteria.Source), SqlDbType.UniqueIdentifier);
                }

                if (!string.IsNullOrEmpty(searchCriteria.DeliveryLocation.StringValue))
                {
                    whereConditions += " AND (C.DELIVERYLOCATIONID = @deliveryLocation) ";

                    MakeParam(cmd, "deliveryLocation", searchCriteria.DeliveryLocation);
                    MakeParam(cmdCount, "deliveryLocation", searchCriteria.DeliveryLocation);
                }

                if (!string.IsNullOrEmpty(searchCriteria.Comment))
                {
                    searchCriteria.Comment = PreProcessSearchText(searchCriteria.Comment, true, false);

                    whereConditions += " AND (C.COMMENT LIKE @comment) ";

                    MakeParam(cmd, "comment", searchCriteria.Comment);
                    MakeParam(cmdCount, "comment", searchCriteria.Comment);
                }

                if (searchCriteria.Status > 0)
                {
                    if (searchCriteria.Status >= (int)CustomerOrderStatus.Open)
                    {
                        CustomerOrderStatus types = (CustomerOrderStatus)searchCriteria.Status;
                        whereConditions += " AND (C.[STATUS] IN ( ";
                        string list = "";
                        list += (types & CustomerOrderStatus.Open) == CustomerOrderStatus.Open ? "4, " : "";
                        list += (types & CustomerOrderStatus.Closed) == CustomerOrderStatus.Closed ? "8, " : "";
                        list += (types & CustomerOrderStatus.Cancelled) == CustomerOrderStatus.Cancelled ? "16, " : "";
                        list += (types & CustomerOrderStatus.Printed) == CustomerOrderStatus.Printed ? "64, " : "";
                        list += (types & CustomerOrderStatus.Ready) == CustomerOrderStatus.Ready ? "128, " : "";
                        list += (types & CustomerOrderStatus.Delivered) == CustomerOrderStatus.Delivered ? "256, " : "";

                        list = list.Substring(0, list.Length - 2);

                        whereConditions += list + ")) ";
                    }
                    else
                    {
                        whereConditions += " AND C.[STATUS] = @status ";
                        MakeParam(cmd, "status", searchCriteria.Status, SqlDbType.Int);
                        MakeParam(cmdCount, "status", searchCriteria.Status, SqlDbType.Int);
                    }
                }

                if (searchCriteria.OrderType != CustomerOrderType.None)
                {
                    whereConditions += " AND (C.ORDERTYPE = @orderType) ";

                    Guid orderGuid = searchCriteria.OrderType == CustomerOrderType.CustomerOrder ? new Guid(CustomerOrderSettingsConstants.ConstCustomerOrder) : new Guid(CustomerOrderSettingsConstants.ConstQuote);
                    MakeParam(cmd, "orderType", orderGuid);
                    MakeParam(cmdCount, "orderType", orderGuid);
                }

                if (searchCriteria.Expired == null)
                {
                    whereConditions += " AND (C.EXPIRES >= GETDATE()) ";
                }
                else if (searchCriteria.Expired != null)
                {
                    if ((bool) searchCriteria.Expired)
                    {
                        whereConditions += " AND (C.EXPIRES < GETDATE()) ";
                    }
                    else
                    {
                        whereConditions += " AND (C.EXPIRES >= GETDATE()) ";
                    }
                }

                cmd.CommandText = @"                    
                    SELECT TOP <nrofrows> C.ID,
                        ISNULL(C.REFERENCE,'') as REFERENCE,
                        ISNULL(C.SOURCE, '00000000-0000-0000-0000-000000000000') as SOURCE,
                        ISNULL(C.DELIVERY,'00000000-0000-0000-0000-000000000000') as DELIVERY,
                        C.EXPIRES as EXPIRES,
                        ISNULL(C.DELIVERYLOCATIONID,'') as DELIVERYLOCATIONID,
                        ISNULL(C.ORDERTYPE,'00000000-0000-0000-0000-000000000000') as ORDERTYPE,
                        ISNULL(C.ORDERXML, '') as ORDERXML,
                        ISNULL(C.CREATEDSTOREID, '') as CREATEDSTOREID,
                        ISNULL(C.CREATEDBY, '') as CREATEDBY,
                        ISNULL(C.CREATEDTERMINALID, '') as CREATEDTERMINALID,
                        ISNULL(C.STATUS, '') as STATUS,
                        C.CREATEDDATE as CREATEDDATE,
                        ISNULL(C.COMMENT, '') as COMMENT,
                        ISNULL(C.CUSTOMERID, '') AS CUSTOMERID
                    from CUSTOMERORDERS C 
                    ";

                whereConditions = AdvancedResolveWhereCondition(whereConditions);
                cmd.CommandText = cmd.CommandText.Replace("<nrofrows>", Conversion.ToStr(numberOfRecordsToReturn));

                if (!string.IsNullOrEmpty(whereConditions))
                {
                    cmd.CommandText += @" where <whereConditions> ";
                    cmd.CommandText = cmd.CommandText.Replace("<whereConditions>", whereConditions);
                }

                cmd.CommandText += @" ORDER BY REFERENCE DESC";
                
                // Do a count of all records first
                cmdCount.CommandText = @"SELECT ISNULL(COUNT(*), 0) FROM CUSTOMERORDERS C " + (string.IsNullOrEmpty(whereConditions) ? "" : " WHERE " + whereConditions);
                totalRecordsMatching = (int)entry.Connection.ExecuteScalar(cmdCount);

                //Then get the search results
                List<CustomerOrder> results = Execute<CustomerOrder>(entry, cmd, CommandType.Text, PopulateCustomerOrder);

                if (searchCriteria.RetrieveOrderXML)
                {
                    return results;
                }

                foreach (CustomerOrder order in results)
                {
                    order.OrderXML = "";
                }

                return results;
            }
        }

        protected virtual string AdvancedResolveWhereCondition(string whereCondition)
        {
            if (string.IsNullOrEmpty(whereCondition))
            {
                return whereCondition;
            }

            if (whereCondition.IndexOf("(") > -1)
            {
                return whereCondition.Substring(whereCondition.IndexOf("(") - 1);
            }

            return whereCondition;
        }

        protected virtual string AdvancedResolveSort(CustomerOrderSorting sort, bool sortBackwards)
        {
            switch (sort)
            {
                case CustomerOrderSorting.Reference:
                    return "REFERENCE" + (sortBackwards ? " DESC" : " ASC");
                case CustomerOrderSorting.Comment:
                    return "COMMENT" + (sortBackwards ? " DESC" : " ASC");
                case CustomerOrderSorting.CreatedDate:
                    return "CREATEDDATE" + (sortBackwards ? " DESC" : " ASC");
                case CustomerOrderSorting.Expires:
                    return "EXPIRES" + (sortBackwards ? " DESC" : " ASC");
            }
            return "";
        }

        #region ISequenceable Members

        private RecordIdentifier sequenceID = RecordIdentifier.Empty;

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            set { sequenceID = value; }
            get
            {
                if (sequenceID.StringValue == "")
                {
                    return "CUSTOMERORDERS";
                }
                else
                {
                    return sequenceID;
                }
            }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "CUSTOMERORDERS", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}

