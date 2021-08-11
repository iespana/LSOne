using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.Containers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Replenishment
{
    public class PurchaseWorksheetData : SqlServerDataProviderBase, IPurchaseWorksheetData
    {
        private static string BaseSQL
        {
            get
            {
                return @"SELECT pw.ID
                                ,pw.INVENTORYTEMPLATEID
                                ,pw.STOREID
                                ,COUNT(pwl.ID) as NUMBEROFLINES
                        FROM PURCHASEWORKSHEET pw
                        LEFT JOIN PURCHASEWORKSHEETLINE pwl on pw.ID = pwl.PURCHASEWORKSHEETID and pw.DATAAREAID = pwl.DATAAREAID and pwl.DELETED = 0
                        LEFT JOIN RETAILITEM item on pwl.ITEMID = item.ITEMID 
";
            }
        }

        private static void PopulatePurchaseWorksheet(IDataReader dr, PurchaseWorksheet worksheet)
        {
            worksheet.ID = (string)dr["ID"];
            worksheet.InventoryTemplateID = (string)dr["INVENTORYTEMPLATEID"];
            worksheet.StoreId = (string)dr["STOREID"];
            worksheet.NumberOfLinesInWorksheet = (int) dr["NUMBEROFLINES"];
        }

        public virtual List<PurchaseWorksheet> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                  @"WHERE (item.DELETED = 0 or item.DELETED is NULL) AND pw.DATAAREAID = @dataAreaId 
                                    GROUP BY pw.ID, pw.INVENTORYTEMPLATEID, pw.STOREID";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var worksheets = Execute<PurchaseWorksheet>(entry, cmd, CommandType.Text, PopulatePurchaseWorksheet);
                return worksheets;
            }
        }

        public virtual PurchaseWorksheet Get(IConnectionManager entry, RecordIdentifier worksheetId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                  @"WHERE pw.ID = @id and (item.DELETED = 0 or item.DELETED is NULL) and pw.DATAAREAID = @dataAreaId 
                                    GROUP BY pw.ID, pw.INVENTORYTEMPLATEID, pw.STOREID";

                MakeParam(cmd, "id", (string)worksheetId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var worksheets = Execute<PurchaseWorksheet>(entry, cmd, CommandType.Text, PopulatePurchaseWorksheet);
                return worksheets.Count > 0 ? worksheets[0] : null;
            }
        }

        public virtual List<PurchaseWorksheet> GetWorksheetsFromTemplateID(IConnectionManager entry, RecordIdentifier inventoryTemplateID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL + @"WHERE pw.INVENTORYTEMPLATEID = @inventorytemplateid 
                                              GROUP BY pw.INVENTORYTEMPLATEID, pw.ID, pw.STOREID";

                MakeParam(cmd, "inventorytemplateid", inventoryTemplateID);

                return Execute<PurchaseWorksheet>(entry, cmd, CommandType.Text, PopulatePurchaseWorksheet);
            }
        }

        public virtual PurchaseWorksheet GetWorksheetFromTemplateIDAndStoreID(IConnectionManager entry, RecordIdentifier inventoryTemplateID, RecordIdentifier storeID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL + @"WHERE pw.INVENTORYTEMPLATEID = @inventorytemplateid and pw.STOREID = @storeid
                                              GROUP BY pw.INVENTORYTEMPLATEID, pw.ID, pw.STOREID";

                MakeParam(cmd, "inventorytemplateid", inventoryTemplateID);
                MakeParam(cmd, "storeid", storeID);

                var worksheets = Execute<PurchaseWorksheet>(entry, cmd, CommandType.Text, PopulatePurchaseWorksheet);
                return worksheets.Count > 0 ? worksheets[0] : null;
            }
        }


        public virtual List<PurchaseWorksheet> GetListByInventoryTemplate(IConnectionManager entry, RecordIdentifier inventoryTemplateID)
        {
            using(IDbCommand cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSQL +
                                  @"WHERE pw.INVENTORYTEMPLATEID = @inventoryTemplateID AND pw.DATAAREAID = @dataAreaID 
                                    GROUP BY pw.ID, pw.INVENTORYTEMPLATEID, pw.STOREID";
                MakeParam(cmd, "inventoryTemplateID", inventoryTemplateID);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<PurchaseWorksheet>(entry, cmd, CommandType.Text, PopulatePurchaseWorksheet);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier worksheetId)
        {
            DeleteRecord(entry, "PURCHASEWORKSHEET", "ID", worksheetId, BusinessObjects.Permission.ManageReplenishment);
        }

        public virtual void DeleteForInverotryTemplate(IConnectionManager entry, RecordIdentifier inventoryTemplateID)
        {
            DeleteRecord(entry, "PURCHASEWORKSHEET", "INVENOTRYTEMPLATEID", inventoryTemplateID, BusinessObjects.Permission.ManageReplenishment);
        }

        public virtual void DeleteForStore(IConnectionManager entry, RecordIdentifier storeID)
        {
            DeleteRecord(entry, "PURCHASEWORKSHEET", "STOREID", storeID, BusinessObjects.Permission.ManageReplenishment);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier worksheetId)
        {
            return RecordExists(entry, "PURCHASEWORKSHEET", "ID", worksheetId);        
        }

        public virtual bool ExistsForStore(IConnectionManager entry, RecordIdentifier storeID)
        {
            return RecordExists(entry, "PURCHASEWORKSHEET", "STOREID", storeID);
        }

        /// <summary>
        /// Saves a given purchase worksheet to the database. Returns the ID of the worksheet
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        public virtual RecordIdentifier Save(IConnectionManager entry, PurchaseWorksheet worksheet)
        {
            var statement = new SqlServerStatement("PURCHASEWORKSHEET");

            bool isNew = false;
            if (worksheet.ID == RecordIdentifier.Empty || worksheet.ID == null)
            {
                isNew = true;
                worksheet.ID = DataProviderFactory.Instance.GenerateNumber<IPurchaseWorksheetData, PurchaseWorksheet>(entry);
            }

            if (isNew || !Exists(entry, worksheet.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)worksheet.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)worksheet.ID);
            }

            statement.AddField("INVENTORYTEMPLATEID", (string)worksheet.InventoryTemplateID);
            statement.AddField("STOREID", (string)worksheet.StoreId);

            entry.Connection.ExecuteStatement(statement);
            return worksheet.ID;
        }

        public virtual int CreatePurchaseWorksheetLinesFromFilter(IConnectionManager entry, RecordIdentifier purchaseWorksheetID, InventoryTemplateFilterContainer filter)
        {
            using (SqlCommand cmd = new SqlCommand("spINVENTORY_CreatePurchaseWorksheetLinesFromFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                string filterDelimiter = ";";

                MakeParam(cmd, "PURCHASEWORKSHEETID", purchaseWorksheetID);
                MakeParam(cmd, "RETAILGROUPS", filter.RetailGroups.Count == 0 ? "" : filter.RetailGroups.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "RETAILDEPARTMENTS", filter.RetailDepartments.Count == 0 ? "" : filter.RetailDepartments.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "VENDORS", filter.Vendors.Count == 0 ? "" : filter.Vendors.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y));
                MakeParam(cmd, "SPECIALGROUPS", filter.SpecialGroups.Count == 0 ? "" : filter.SpecialGroups.Select(x => x.StringValue).Aggregate((x, y) => x + filterDelimiter + y), SqlDbType.VarChar);
                MakeParam(cmd, "FILTERDELIMITER", filterDelimiter, SqlDbType.VarChar);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                SqlParameter insertedRows = MakeParam(cmd, "INSERTEDRECORDS", "", SqlDbType.Int, ParameterDirection.Output, 4);

                entry.Connection.ExecuteNonQuery(cmd, false);

                return (int)insertedRows.Value;
            }
        }

        public virtual void RefreshPurchaseWorksheetLines(IConnectionManager entry, RecordIdentifier purchaseWorksheetID)
        {
            using (SqlCommand cmd = new SqlCommand("spINVENTORY_RefreshPurchaseWorksheetLines"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                MakeParam(cmd, "PURCHASEWORKSHEETID", purchaseWorksheetID);
                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);

                entry.Connection.ExecuteNonQuery(cmd, false);
            }
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return new RecordIdentifier("PURCHASEWORKSHEET"); }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "PURCHASEWORKSHEET", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }
    }
}
