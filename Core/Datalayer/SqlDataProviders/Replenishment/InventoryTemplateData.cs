using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.Replenishment.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Replenishment;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.DataLayer.SqlDataProviders.Replenishment
{
    public class InventoryTemplateData : SqlServerDataProviderBase, IInventoryTemplateData
    {
        private static void PopulateInventoryTemplate(IDataReader dr, InventoryTemplate template)
        {
            template.ID = (string)dr["ID"];
            template.Text = (string)dr["NAME"];
            template.AllStores = (bool)dr["ALLSTORES"];
            template.ChangeVendorInLine = (bool)dr["CHANGEVENDORINLINE"];
            template.ChangeUomInLine = (bool)dr["CHANGEUOMINLINE"];
            template.CalculateSuggestedQuantity = (bool)dr["CALCULATESUGGESTEDQUANTITY"];
            template.SetQuantityToSuggestedQuantity = (bool)dr["SETQUANTITYTOSUGGESTEDQUANTITY"];
            template.DisplayReorderPoint = (bool)dr["DISPLAYREORDERPOINT"];
            template.DisplayMaximumInventory = (bool)dr["DISPLAYMAXIMUMINVENTORY"];
            template.DisplayBarcode = (bool)dr["DISPLAYBARCODE"];
            template.AddLinesWithZeroSuggestedQuantity = (bool) dr["ADDLINESWITHZEROSUGGESTEDQTY"];
            template.UseBarcodeUom = (bool) dr["USEBARCODEUOM"];
            template.AllowAddNewLine = (bool) dr["ALLOWADDNEWLINE"];
            template.AllowImageImport = (bool)dr["ALLOWIMAGEIMPORT"];
            template.TemplateEntryType = (TemplateEntryTypeEnum)dr["TEMPLATEENTRYTYPE"];
            template.UnitSelection = (UnitSelectionEnum)dr["UNITSELECTION"];
            template.EnteringType = (EnteringTypeEnum)dr["ENTERINGTYPE"];
            template.QuantityMethod = (QuantityMethodEnum)dr["QUANTITYMETHOD"];
            template.DefaultQuantity = (decimal)dr["DEFAULTQUANTITY"];
            template.AreaID = AsGuid(dr["AREAID"]);
            template.DefaultStore = (string)dr["DEFAULTSTORE"];
            template.DefaultVendor = (string)dr["DEFAULTVENDOR"];
            template.AutoPopulateTransferOrderReceivingQuantity = (bool)dr["AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY"];
            template.CreateGoodsReceivingDocument = (bool)dr["CREATEGOODSRECEIVINGDOCUMENT"];
            template.AutoPopulateGoodsReceivingDocument = (bool)dr["AUTOPOPULATEGOODSRECEIVINGDOCUMENT"];
        }

        private static void PopulateInventoryTemplateListItem(IDataReader dr, InventoryTemplateListItem template)
        {
            template.ID = (string)dr["ID"];
            template.Text = (string)dr["NAME"];
            template.AllStores = (bool)dr["ALLSTORES"];
            template.StoreCount = (int)dr["StoreCount"];
            template.Type = (TemplateEntryTypeEnum)dr["TEMPLATEENTRYTYPE"];
        }

        public virtual string GetFirstStoreName(IConnectionManager entry, RecordIdentifier templateId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"select top 1 s.NAME from INVENTORYTEMPLATESTORECONNECTION i
                                    left outer join RBOSTORETABLE s on i.STOREID = s.STOREID and i.DATAAREAID = s.DATAAREAID
                                    where i.INVENTORYTEMPLATEID = @templateID and i.DATAAREAID = @dataAreaID ";

                    MakeParam(cmd,"templateID",(string)templateId);
                    MakeParam(cmd,"dataAreaID",entry.Connection.DataAreaId);

                    object value = entry.Connection.ExecuteScalar(cmd);

                    if (value is DBNull)
                    {
                        return "";
                    }

                    return (string)value;
            }
        }

        public virtual List<TemplateListItem> GetInventoryTemplatesByType(IConnectionManager entry, TemplateEntryTypeEnum templateEntryType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = $@"SELECT T.ID AS TEMPLATEID, 
                                           T.NAME AS TEMPLATENAME, 
                                           ISNULL(SC.STOREID, AST.STOREID) AS STOREID, 
                                           ISNULL(S.NAME, AST.NAME) AS STORENAME 
                                    FROM INVENTORYTEMPLATE T
                                    LEFT JOIN INVENTORYTEMPLATESTORECONNECTION SC on T.ID = SC.INVENTORYTEMPLATEID AND T.ALLSTORES = 0
                                    LEFT JOIN RBOSTORETABLE S ON SC.STOREID = S.STOREID
                                    LEFT JOIN RBOSTORETABLE AST ON T.ALLSTORES = 1
                                    WHERE T.TEMPLATEENTRYTYPE = {(int)templateEntryType}";

                return Execute<TemplateListItem>(entry, cmd, CommandType.Text, PopulateTemplateListItem);
            }
        }

        private static void PopulateTemplateListItem(IDataReader dr, TemplateListItem template)
        {
            template.TemplateID = (string)dr["TEMPLATEID"];
            template.StoreID = (string)dr["STOREID"];
            template.TemplateName = (string)dr["TEMPLATENAME"];
            template.StoreName = (string)dr["STORENAME"];
        }

        public virtual InventoryTemplateListItem GetListItem(IConnectionManager entry, RecordIdentifier templateId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sql = @"SELECT i.ID
                                ,i.NAME
                                ,i.ALLSTORES
                                ,Count(t.ID) as StoreCount
                                ,i.TEMPLATEENTRYTYPE
                         FROM INVENTORYTEMPLATE i 
                         LEFT OUTER JOIN INVENTORYTEMPLATESTORECONNECTION t on t.INVENTORYTEMPLATEID = i.ID
                         WHERE i.DATAAREAID = @dataAreaId AND i.ID = @templateID
                         GROUP BY i.ID,i.NAME, i.ALLSTORES, i.TEMPLATEENTRYTYPE";

                cmd.CommandText = sql;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "templateID", (string)templateId);
                
                var result = Execute<InventoryTemplateListItem>(entry, cmd, CommandType.Text, PopulateInventoryTemplateListItem);
                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual List<InventoryTemplateListItem> GetList(IConnectionManager entry, InventoryTemplateListFilter filter)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sql = @"SELECT i.ID
                                ,i.NAME
                                ,i.ALLSTORES
                                ,Count(t.ID) as StoreCount
                                ,i.TEMPLATEENTRYTYPE
                         FROM INVENTORYTEMPLATE i 
                         LEFT OUTER JOIN INVENTORYTEMPLATESTORECONNECTION t on t.INVENTORYTEMPLATEID = i.ID
                         WHERE i.DATAAREAID = @dataAreaId ";

                if(!string.IsNullOrEmpty(filter.Description))
                {
                    sql += " AND i.NAME LIKE @description ";
                    MakeParam(cmd, "description", PreProcessSearchText(filter.Description, true, filter.DescriptionBeginsWith));
                }

                if(filter.EntryType.HasValue)
                {
                    sql += " AND i.TEMPLATEENTRYTYPE = @entryType ";
                    MakeParam(cmd, "entryType", (int)filter.EntryType.Value, SqlDbType.Int);
                }

                sql += "GROUP BY i.ID,i.NAME, i.ALLSTORES, i.TEMPLATEENTRYTYPE";
                cmd.CommandText = sql;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<InventoryTemplateListItem>(entry, cmd, CommandType.Text, PopulateInventoryTemplateListItem);
            }
        }

        public virtual List<InventoryTemplateListItem> GetListForStore(IConnectionManager entry, RecordIdentifier storeId, InventoryTemplateListFilter filter)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sql = @"SELECT i.ID
                                ,i.NAME
                                ,i.ALLSTORES
                                ,Count(t.ID) as StoreCount
                                ,i.TEMPLATEENTRYTYPE
                         FROM INVENTORYTEMPLATE i 
                         LEFT OUTER JOIN INVENTORYTEMPLATESTORECONNECTION t on t.INVENTORYTEMPLATEID = i.ID
                         WHERE i.DATAAREAID = @dataAreaId AND (t.STOREID = @storeId OR i.ALLSTORES = 1) ";

                if (!string.IsNullOrEmpty(filter.Description))
                {
                    sql += " AND i.NAME LIKE @description ";
                    MakeParam(cmd, "description", PreProcessSearchText(filter.Description, true, filter.DescriptionBeginsWith));
                }

                if (filter.EntryType.HasValue)
                {
                    sql += " AND i.TEMPLATEENTRYTYPE = @entryType ";
                    MakeParam(cmd, "entryType", (int)filter.EntryType.Value, SqlDbType.Int);
                }

                sql += "GROUP BY i.ID,i.NAME, i.ALLSTORES, i.TEMPLATEENTRYTYPE";
                cmd.CommandText = sql;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "storeId", storeId);

                return Execute<InventoryTemplateListItem>(entry, cmd, CommandType.Text, PopulateInventoryTemplateListItem);
            }
        }

        public virtual InventoryTemplate Get(IConnectionManager entry, RecordIdentifier templateId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sql = @"SELECT i.ID
                                ,i.NAME
                                ,i.ALLSTORES
                                ,i.CHANGEVENDORINLINE
                                ,i.CHANGEUOMINLINE
                                ,i.CALCULATESUGGESTEDQUANTITY
                                ,i.SETQUANTITYTOSUGGESTEDQUANTITY
                                ,i.DISPLAYREORDERPOINT
                                ,i.DISPLAYMAXIMUMINVENTORY
                                ,i.DISPLAYBARCODE
                                ,i.ADDLINESWITHZEROSUGGESTEDQTY
                                ,i.USEBARCODEUOM
                                ,i.ALLOWADDNEWLINE
                                ,i.ALLOWIMAGEIMPORT
                                ,i.TEMPLATEENTRYTYPE
                                ,i.UNITSELECTION
                                ,i.ENTERINGTYPE
                                ,i.QUANTITYMETHOD
                                ,i.DEFAULTQUANTITY
                                ,i.AREAID
                                ,ISNULL(i.DEFAULTSTORE, '') AS DEFAULTSTORE
                                ,ISNULL(i.DEFAULTVENDOR, '') AS DEFAULTVENDOR
                                ,AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY AS AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY
                                ,i.CREATEGOODSRECEIVINGDOCUMENT
                                ,i.AUTOPOPULATEGOODSRECEIVINGDOCUMENT
                         FROM INVENTORYTEMPLATE i 
                         where i.ID = @id and i.DATAAREAID = @dataAreaId";
                         
                cmd.CommandText = sql;

                MakeParam(cmd, "id", (string)templateId);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                List<InventoryTemplate> templates = Execute<InventoryTemplate>(entry, cmd, CommandType.Text, PopulateInventoryTemplate);
                return templates.Count > 0 ? templates[0] : null;
            }
        }

        public virtual List<InventoryTemplate> GetTemplatesOnAllStores(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                string sql = @"SELECT i.ID
                                ,i.NAME
                                ,i.ALLSTORES
                                ,i.CHANGEVENDORINLINE
                                ,i.CHANGEUOMINLINE
                                ,i.CALCULATESUGGESTEDQUANTITY
                                ,i.SETQUANTITYTOSUGGESTEDQUANTITY
                                ,i.DISPLAYREORDERPOINT
                                ,i.DISPLAYMAXIMUMINVENTORY
                                ,i.DISPLAYBARCODE
                                ,i.ADDLINESWITHZEROSUGGESTEDQTY 
                                ,i.USEBARCODEUOM
                                ,i.ALLOWADDNEWLINE
                                ,i.ALLOWIMAGEIMPORT
                                ,i.TEMPLATEENTRYTYPE
                                ,i.UNITSELECTION
                                ,i.ENTERINGTYPE
                                ,i.QUANTITYMETHOD
                                ,i.DEFAULTQUANTITY
                                ,i.AREAID
                                ,ISNULL(i.DEFAULTSTORE, '') AS DEFAULTSTORE
                                ,ISNULL(i.DEFAULTVENDOR, '') AS DEFAULTVENDOR
                                ,AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY AS AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY
                                ,i.CREATEGOODSRECEIVINGDOCUMENT
                                ,i.AUTOPOPULATEGOODSRECEIVINGDOCUMENT
                         FROM INVENTORYTEMPLATE i 
                         where i.DATAAREAID = @dataAreaId and i.ALLSTORES ='1'";

                cmd.CommandText = sql;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<InventoryTemplate>(entry, cmd, CommandType.Text, PopulateInventoryTemplate);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier templateId)
        {
            DeleteRecord(entry, "INVENTORYTEMPLATE", "ID", templateId, BusinessObjects.Permission.ManageReplenishment);
            DeleteRecord(entry, "INVENTORYTEMPLATESTORECONNECTION", "INVENTORYTEMPLATEID", templateId, BusinessObjects.Permission.ManageReplenishment);
            DeleteRecord(entry, "INVENTORYTEMPLATEITEMFILTERSECTIONS", "INVENTORYTEMPLATEIDID", templateId, BusinessObjects.Permission.ManageReplenishment);
            DeleteRecord(entry, "INVENTORYTEMPLATEITEMFILTERSELECTIONS", "INVENTORYTEMPLATEIDID", templateId, BusinessObjects.Permission.ManageReplenishment);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier templateId)
        {
            return RecordExists(entry, "INVENTORYTEMPLATE", "ID", templateId);        
        }

        /// <summary>
        /// Saves a given tempate to the database. Returns the ID of the template
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public virtual void Save(IConnectionManager entry, InventoryTemplate template)
        {
            var statement = new SqlServerStatement("INVENTORYTEMPLATE");

            template.Validate();

            bool isNew = false;
            if (template.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                template.ID = DataProviderFactory.Instance.GenerateNumber<IInventoryTemplateData, InventoryTemplate>(entry);
            }

            if (isNew || !Exists(entry, template.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)template.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)template.ID);
            }

            statement.AddField("NAME", template.Text);
            statement.AddField("ALLSTORES", template.AllStores, SqlDbType.Bit);
            statement.AddField("CHANGEVENDORINLINE", template.ChangeVendorInLine, SqlDbType.Bit);
            statement.AddField("CHANGEUOMINLINE", template.ChangeUomInLine, SqlDbType.Bit);
            statement.AddField("CALCULATESUGGESTEDQUANTITY", template.CalculateSuggestedQuantity, SqlDbType.Bit);
            statement.AddField("SETQUANTITYTOSUGGESTEDQUANTITY", template.SetQuantityToSuggestedQuantity, SqlDbType.Bit);
            statement.AddField("DISPLAYREORDERPOINT", template.DisplayReorderPoint, SqlDbType.Bit);
            statement.AddField("DISPLAYMAXIMUMINVENTORY", template.DisplayMaximumInventory, SqlDbType.Bit);
            statement.AddField("DISPLAYBARCODE", template.DisplayBarcode, SqlDbType.Bit);
            statement.AddField("ADDLINESWITHZEROSUGGESTEDQTY", template.AddLinesWithZeroSuggestedQuantity, SqlDbType.Bit);
            statement.AddField("USEBARCODEUOM", template.UseBarcodeUom, SqlDbType.Bit);
            statement.AddField("ALLOWADDNEWLINE", template.AllowAddNewLine, SqlDbType.Bit);
            statement.AddField("ALLOWIMAGEIMPORT", template.AllowImageImport, SqlDbType.Bit);
            statement.AddField("TEMPLATEENTRYTYPE", (int)template.TemplateEntryType, SqlDbType.Int);
            statement.AddField("UNITSELECTION", (int)template.UnitSelection, SqlDbType.Int);
            statement.AddField("ENTERINGTYPE", (int)template.EnteringType, SqlDbType.Int);
            statement.AddField("QUANTITYMETHOD", (int)template.QuantityMethod, SqlDbType.Int);
            statement.AddField("DEFAULTQUANTITY", template.DefaultQuantity, SqlDbType.Decimal);
            statement.AddField("DEFAULTSTORE", (string)template.DefaultStore);
            statement.AddField("DEFAULTVENDOR", (string)template.DefaultVendor);
            statement.AddField("AUTOPOPULATETRANSFERORDERRECEIVINGQUANTITY", template.AutoPopulateTransferOrderReceivingQuantity, SqlDbType.Int);
            statement.AddField("CREATEGOODSRECEIVINGDOCUMENT", template.CreateGoodsReceivingDocument, SqlDbType.Bit);
            statement.AddField("AUTOPOPULATEGOODSRECEIVINGDOCUMENT", template.AutoPopulateGoodsReceivingDocument, SqlDbType.Bit);

            if (template.AreaID == Guid.Empty)
            {
                statement.AddField("AREAID", DBNull.Value, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("AREAID", template.AreaID, SqlDbType.UniqueIdentifier);
            }

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return new RecordIdentifier("INVENTORYTEMPLATE"); }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "INVENTORYTEMPLATE", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }
    }
}