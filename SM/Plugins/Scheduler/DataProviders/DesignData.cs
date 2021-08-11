using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.Scheduler.BusinessObjects;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.DataProviders
{
    public class DesignData : SqlServerDataProvider, IDesignData
    {
        #region Select strings

        private static string BaseGetDatabaseDesignsString
        {
            get { return @" SELECT 
	                            Id,
	                            Description,
	                            CodePage,
	                            Enabled
                              FROM JscDatabaseDesigns"; }
        }

        private static string TableMapGetString
        {
            get { return @"                         
                        SELECT 
                            tm.Id,
                            tm.FromTable,
                            tm.ToTable,
                            tm.Description,
							tdFrom.DatabaseDesign FromDatabaseDesign,
							tdfrom.Enabled FromEnabled,
							tdfrom.TableName FromTableName,
							tdto.DatabaseDesign ToDatabaseDesign,
							tdto.Enabled ToEnabled,
							tdto.TableName ToTableName,
                            ddfrom.Description FromDDdescription,
	                        ddfrom.CodePage FromDDCodePage,
	                        ddfrom.Enabled FromDDEnabled,
                            ddto.Description ToDDDescription,
	                        ddto.CodePage ToDDCodePage,
	                        ddto.Enabled ToDDEnabled
                        FROM JscTableMap tm
						    join JscTableDesigns tdfrom on tm.FromTable = tdfrom.Id
						    join JscTableDesigns tdto on tm.ToTable =  tdto.id
                            join JscDatabaseDesigns ddfrom on tdFrom.DatabaseDesign = ddfrom.id
                            join JscDatabaseDesigns ddto on tdTo.DatabaseDesign = ddto.id";
            }
        }

        private static string FieldDesignGetString
        {
            get { return @" SELECT
                            Id,
                            TableDesign,
                            FieldName,
                            Sequence,
                            DataType,
                            Length,
                            FieldClass,
                            FieldOption,
                            Enabled
                        FROM 
                            JscFieldDesigns"; }
        }

        private static string TableDesignGetString
        {
            get { return @"SELECT  
                                td.Id,
                                td.DatabaseDesign,
                                td.TableName,
                                td.Enabled
                            FROM JscTableDesigns td "; }
        }

        private static string LinkedTableGetString
        {
            get { return @"    SELECT 
                                    lt.Id,
                                    lt.FromTable,
                                    lt.ToTable
                                FROM 
                                    JscLinkedTables lt "; }
        }

        #endregion

        #region Populators

        private static void PopulateDatabaseDesignDataSelector(IDataReader dr, DataSelector selector)
        {
            selector.GuidId = (Guid) dr["ID"];
            selector.Text = (string) dr["Description"];
        }

        private static void PopulateDatabaseDesign(IDataReader dr, JscDatabaseDesign design)
        {
            design.ID = (Guid) dr["ID"];
            design.Description = (string) dr["Description"];
            design.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            design.CodePage = dr["CodePage"] == DBNull.Value ? 0 : (int) dr["CodePage"];
        }

        private static void PopulateLinkedTable(IDataReader dr, JscLinkedTable table)
        {
            table.ID = (Guid) dr["ID"];
            table.FromTable = (Guid) dr["FromTable"];
            table.ToTable = (Guid) dr["ToTable"];
        }

        private static void PopulateLinkedFilter(IDataReader dr, JscLinkedFilter filter)
        {
            filter.ID = (Guid)dr["ID"];
            filter.LinkType = (LinkedFilterType)((Int16)dr["LinkType"]);
            filter.LinkedTable = (Guid)dr["LinkedTable"];
            filter.LinkedField = (Guid)dr["LinkedField"];
            filter.ToField = dr["ToField"] == DBNull.Value ? RecordIdentifier.Empty : (Guid)dr["ToField"];
            filter.Filter = dr["Filter"] == DBNull.Value ? null : (string)dr["Filter"];
        }

        private static void PopulateTableDesign(IDataReader dr, JscTableDesign design)
        {
            design.ID = (Guid) dr["ID"];
            design.DatabaseDesign = (Guid) dr["DatabaseDesign"];
            design.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            design.TableName = (string) dr["TableName"];
        }

        private static void PopulateTableMap(IDataReader dr, JscTableMap map)
        {
            map.Description = dr["Description"] == DBNull.Value ? null : (string) dr["Description"];
            map.FromTable = (Guid) dr["FromTable"];
            map.ID = (Guid) dr["ID"];
            map.ToTable = (Guid) dr["ToTable"];

            map.FromJscTableDesign = new JscTableDesign {ID = map.FromTable};
            map.FromJscTableDesign.DatabaseDesign = (Guid) dr["FromDatabaseDesign"];
            map.FromJscTableDesign.Enabled = (Convert.ToInt16(dr["FromENABLED"]) != 0);
            map.FromJscTableDesign.TableName = (string) dr["FromTableName"];
            map.FromJscTableDesign.JscDatabaseDesign = new JscDatabaseDesign
                {
                    ID = map.FromJscTableDesign.DatabaseDesign
                };
            map.FromJscTableDesign.JscDatabaseDesign.Description = (string) dr["FromDDDescription"];
            map.FromJscTableDesign.JscDatabaseDesign.Enabled = (Convert.ToInt16(dr["FromDDENABLED"]) != 0);
            map.FromJscTableDesign.JscDatabaseDesign.CodePage = dr["FromDDCodePage"] == DBNull.Value
                                                                    ? 0
                                                                    : (int) dr["FromDDCodePage"];

            map.ToJscTableDesign = new JscTableDesign {ID = map.ToTable};
            map.ToJscTableDesign.DatabaseDesign = (Guid) dr["ToDatabaseDesign"];
            map.ToJscTableDesign.Enabled = (Convert.ToInt16(dr["ToENABLED"]) != 0);
            map.ToJscTableDesign.TableName = (string) dr["toTableName"];
            map.ToJscTableDesign.JscDatabaseDesign = new JscDatabaseDesign {ID = map.ToJscTableDesign.DatabaseDesign};
            map.ToJscTableDesign.JscDatabaseDesign.Description = (string) dr["ToDDDescription"];
            map.ToJscTableDesign.JscDatabaseDesign.Enabled = (Convert.ToInt16(dr["ToDDENABLED"]) != 0);
            map.ToJscTableDesign.JscDatabaseDesign.CodePage = dr["ToDDCodePage"] == DBNull.Value
                                                                  ? 0
                                                                  : (int) dr["toDDCodePage"];
        }

        private static void PopulateFieldMap(IDataReader dr, JscFieldMap map)
        {
            map.ConversionMethod = (ConversionMethod) (Int16) dr["ConversionMethod"];
            map.ConversionValue = dr["ConversionValue"] == DBNull.Value ? null : (string) dr["ConversionValue"];
            map.FromField = (Guid) dr["FromField"];
            map.ID = (Guid) dr["ID"];
            map.TableMap = (Guid) dr["TableMap"];
            map.ToField = (Guid) dr["ToField"];
        }

        private static void PopulateFieldDesign(IDataReader dr, JscFieldDesign design)
        {
            design.DataType = (DDDataType) (Int16) dr["DataType"];
            design.Enabled = (Convert.ToInt16(dr["ENABLED"]) != 0);
            design.FieldClass = (short) dr["FieldClass"];
            design.FieldName = (string) dr["FieldName"];
            design.FieldOption = dr["FieldOption"] == DBNull.Value ? null : (string) dr["FieldOption"];
            design.ID = (Guid) dr["Id"];
            design.Length = dr["Length"] == DBNull.Value ? 0 : (int) dr["Length"];
            design.Sequence = (int) dr["Sequence"];
            design.TableDesign = (Guid) dr["TableDesign"];
        }

     
        #endregion

        #region Delete and Save

        public void Save(IConnectionManager entry, JscTableDesign tableDesign)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseDesignEdit);

            var statement = new SqlServerStatement("JscTableDesigns");

            bool isNew = false;
            if (tableDesign.ID == null || tableDesign.ID == RecordIdentifier.Empty)
            {
                var tableDesigns = GetTableDesigns(entry, tableDesign.DatabaseDesign, false).ToList();
                var design = tableDesigns.Find(d => d.TableName == tableDesign.TableName);
                if (design == null)
                {
                    isNew = true;
                    tableDesign.ID = Guid.NewGuid();
                }
                else
                {
                    tableDesign.ID = design.ID;
                }
            }

            if (isNew || !RecordExists(entry, "JscTableDesigns", "Id", tableDesign.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", (Guid)tableDesign.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Id", (Guid)tableDesign.ID, SqlDbType.UniqueIdentifier);
            }

            if (tableDesign.DatabaseDesign != null)
            {
                statement.AddField("DATABASEDESIGN", tableDesign.DatabaseDesign.ToString());
            }

            statement.AddField("TableName", tableDesign.TableName);
            statement.AddField("ENABLED", tableDesign.Enabled ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Save(IConnectionManager entry, JscFieldDesign fieldDesign)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseDesignEdit);

            var statement = new SqlServerStatement("JscFieldDesigns");

            bool isNew = false;
            if (fieldDesign.ID == null || fieldDesign.ID == RecordIdentifier.Empty)
            {
                var fieldDesigns = GetFieldDesigns(entry, fieldDesign.TableDesign).ToList();
                var design = fieldDesigns.Find(d => d.FieldName == fieldDesign.FieldName);

                isNew = design == null;
                fieldDesign.ID = design == null ? Guid.NewGuid() : design.ID;
            }

            if (isNew || !RecordExists(entry, "JscFieldDesigns", "Id", fieldDesign.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", (Guid)fieldDesign.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Id", (Guid)fieldDesign.ID, SqlDbType.UniqueIdentifier);
            }

            if (fieldDesign.TableDesign != null)
            {
                statement.AddField("TableDesign", fieldDesign.TableDesign.ToString());
            }

            statement.AddField("FieldName", fieldDesign.FieldName);
            statement.AddField("Sequence", fieldDesign.Sequence, SqlDbType.Int);
            statement.AddField("DataType", fieldDesign.DataType, SqlDbType.SmallInt);
            if (fieldDesign.Length == null)
                statement.AddField("Length", DBNull.Value, SqlDbType.Int);
            else
                statement.AddField("Length", fieldDesign.Length, SqlDbType.Int);
            statement.AddField("FieldOption", fieldDesign.FieldOption);
            statement.AddField("FieldClass", fieldDesign.FieldClass, SqlDbType.SmallInt);
            statement.AddField("ENABLED", fieldDesign.Enabled ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Save(IConnectionManager entry, JscDatabaseDesign databaseDesign)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseDesignEdit);

            var statement = new SqlServerStatement("JscDatabaseDesigns");

            bool isNew = false;
            if (databaseDesign.ID == null || databaseDesign.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                databaseDesign.ID = Guid.NewGuid();
            }

            if (isNew || !RecordExists(entry, "JscDatabaseDesigns", "Id", databaseDesign.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", (Guid) databaseDesign.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Id", (Guid) databaseDesign.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("Description", databaseDesign.Description);
            if (databaseDesign.CodePage != null)
            {
                statement.AddField("CodePage", databaseDesign.CodePage, SqlDbType.Int);
            }
            statement.AddField("ENABLED", databaseDesign.Enabled ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Save(IConnectionManager entry, JscTableMap tableMap)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseDesignEdit);

            var statement = new SqlServerStatement("JscTableMap");

            bool isNew = false;
            if (tableMap.ID == null || tableMap.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                tableMap.ID = Guid.NewGuid();
            }

            if (isNew || !RecordExists(entry, "JscTableMap", "Id", tableMap.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("Id", (Guid) tableMap.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("Id", (Guid) tableMap.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("FromTable", (Guid) tableMap.FromTable, SqlDbType.UniqueIdentifier);
            statement.AddField("ToTable", (Guid) tableMap.ToTable, SqlDbType.UniqueIdentifier);
            statement.AddField("Description", tableMap.Description);

            entry.Connection.ExecuteStatement(statement);
            if (tableMap.JscFieldMaps != null)
            {
                foreach (var jscFieldMap in tableMap.JscFieldMaps)
                {
                    var fieldstatement = new SqlServerStatement("JscFieldMap");

                    isNew = false;
                    if (jscFieldMap.ID == null || jscFieldMap.ID == RecordIdentifier.Empty)
                    {
                        isNew = true;
                        jscFieldMap.ID = Guid.NewGuid();
                    }

                    if (isNew || !RecordExists(entry, "JscFieldMap", "Id", jscFieldMap.ID))
                    {
                        fieldstatement.StatementType = StatementType.Insert;

                        fieldstatement.AddKey("Id", (Guid) jscFieldMap.ID, SqlDbType.UniqueIdentifier);
                    }
                    else
                    {
                        fieldstatement.StatementType = StatementType.Update;

                        fieldstatement.AddCondition("Id", (Guid) jscFieldMap.ID, SqlDbType.UniqueIdentifier);
                    }

                    if (jscFieldMap.TableMap == null || jscFieldMap.TableMap.IsEmpty)
                    {
                        fieldstatement.AddField("TableMap", (Guid) tableMap.ID, SqlDbType.UniqueIdentifier);
                    }
                    else
                    {
                        fieldstatement.AddField("TableMap", (Guid) jscFieldMap.TableMap, SqlDbType.UniqueIdentifier);
                    }
                    fieldstatement.AddField(
                        "FromField",
                        jscFieldMap.FromField == null && jscFieldMap.FromJscFieldDesign != null
                            ? (Guid) jscFieldMap.FromJscFieldDesign.ID
                            : (Guid) jscFieldMap.FromField,
                        SqlDbType.UniqueIdentifier);
                    fieldstatement.AddField(
                        "ToField",
                        jscFieldMap.ToField == null && jscFieldMap.ToJscFieldDesign != null
                            ? (Guid) jscFieldMap.ToJscFieldDesign.ID
                            : (Guid) jscFieldMap.ToField,
                        SqlDbType.UniqueIdentifier);
                    fieldstatement.AddField("ConversionValue", jscFieldMap.ConversionValue);
                    fieldstatement.AddField("ConversionMethod", jscFieldMap.ConversionMethod, SqlDbType.SmallInt);

                    entry.Connection.ExecuteStatement(fieldstatement);
                }
            }
        }

        public void Save(IConnectionManager entry, JscLinkedTable linkedTable)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseDesignEdit);

            var statement = new SqlServerStatement("JscLinkedTables");

            bool isNew = false;
            if (linkedTable.ID == null || linkedTable.ID == RecordIdentifier.Empty)
            {
                var linkedTables = GetAllLinkedTables(entry);
                var table =
                    linkedTables.ToList()
                                .Find(t => linkedTable.FromTable == t.FromTable && linkedTable.ToTable == t.ToTable);
                if (table == null)
                {
                    isNew = true;
                    linkedTable.ID = Guid.NewGuid();
                }
                else
                {
                    linkedTable.ID = table.ID;
                }
            }

            if (isNew || !RecordExists(entry, "JscLinkedTables", "Id", linkedTable.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", (Guid) linkedTable.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Id", (Guid) linkedTable.ID, SqlDbType.UniqueIdentifier);
            }

            if (linkedTable.FromTable == null && linkedTable.FromJscTableDesign == null)
            {
                throw new ArgumentException("FromTable cannot be null");
            }

            statement.AddField("FromTable",
                               linkedTable.FromTable == null
                                   ? (Guid) linkedTable.FromJscTableDesign.ID
                                   : (Guid) linkedTable.FromTable, SqlDbType.UniqueIdentifier);

            if (linkedTable.ToTable == null && linkedTable.ToJscTableDesign == null)
            {
                throw new ArgumentException("ToTable cannot be null");
            }
            statement.AddField("ToTable",
                               linkedTable.ToTable == null
                                   ? (Guid) linkedTable.ToJscTableDesign.ID
                                   : (Guid) linkedTable.ToTable, SqlDbType.UniqueIdentifier);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Save(IConnectionManager entry, JscLinkedFilter linkedFilter)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseDesignEdit);

            var statement = new SqlServerStatement("JscLinkedFilters");

            bool isNew = false;
            if (linkedFilter.ID == null || linkedFilter.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                linkedFilter.ID = Guid.NewGuid();
            }

            if (isNew || !RecordExists(entry, "JscLinkedFilters", "Id", linkedFilter.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", (Guid) linkedFilter.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Id", (Guid) linkedFilter.ID, SqlDbType.UniqueIdentifier);
            }
            if (linkedFilter.LinkedTable == null && linkedFilter.JscLinkedTable == null)
            {
                throw new ArgumentException("Linked table cannot be null!");
            }
            statement.AddField("LinkedTable",
                               linkedFilter.LinkedTable == null
                                   ? (Guid) linkedFilter.JscLinkedTable.ID
                                   : (Guid) linkedFilter.LinkedTable,
                               SqlDbType.UniqueIdentifier);

            if (linkedFilter.LinkedField == null && linkedFilter.LinkedJscFieldDesign == null)
            {
                throw new ArgumentException("Linked field cannot be null!");
            }
            statement.AddField("LinkedField",
                               linkedFilter.LinkedField == null
                                   ? (Guid) linkedFilter.LinkedJscFieldDesign.ID
                                   : (Guid) linkedFilter.LinkedField,
                               SqlDbType.UniqueIdentifier);


            statement.AddField("LinkType", linkedFilter.LinkType, SqlDbType.SmallInt);
            if (linkedFilter.ToField == null && linkedFilter.ToJscFieldDesign == null)
            {
                throw new ArgumentException("To field cannot be null!");
            }
            statement.AddField("ToField",
                               linkedFilter.ToField == null
                                   ? (Guid) linkedFilter.ToJscFieldDesign.ID
                                   : (Guid) linkedFilter.ToField,
                               SqlDbType.UniqueIdentifier);

            statement.AddField("Filter", linkedFilter.Filter);

            entry.Connection.ExecuteStatement(statement);
        }

        public void Save(IConnectionManager entry, JscSubJobFromTableFilter fromTableFilter)
        {
            var statement = new SqlServerStatement("JscSubJobFromTableFilters");

            bool isNew = false;
            if (fromTableFilter.ID == null || fromTableFilter.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                fromTableFilter.ID = Guid.NewGuid();
            }

            if (isNew || !RecordExists(entry, "JscSubJobFromTableFilters", "Id", fromTableFilter.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("Id", (Guid) fromTableFilter.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("Id", (Guid) fromTableFilter.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("SubJob", (Guid) fromTableFilter.SubJob, SqlDbType.UniqueIdentifier);
            statement.AddField("Field", (Guid) fromTableFilter.Field, SqlDbType.UniqueIdentifier);
            statement.AddField("FilterType", fromTableFilter.FilterType, SqlDbType.SmallInt);
            statement.AddField("Value1", fromTableFilter.Value1);
            statement.AddField("Value2", fromTableFilter.Value2);
            statement.AddField("ApplyFilter", fromTableFilter.ApplyFilter, SqlDbType.SmallInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public void DeleteTableMaps(IConnectionManager entry, IEnumerable<JscTableMap> tableMaps)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseMapView);

            var transaction = entry.CreateTransaction();
            try
            {
                foreach (var tableMap in tableMaps)
                {
                    if (tableMap.JscFieldMaps != null)
                    {
                        foreach (var fieldMap in tableMap.JscFieldMaps)
                        {
                            DeleteRecord(transaction, "JscFieldMap", "Id", fieldMap.ID, SchedulerPermissions.DatabaseMapView);
                        }
                    }
                    DeleteRecord(transaction, "JscTableMap", "Id", tableMap.ID, SchedulerPermissions.DatabaseMapView);
                }

            }

            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();
            }

        }

        public void DeleteFieldMap(IConnectionManager entry, JscFieldMap fieldMapToDelete)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseMapEdit);

            if (!(fieldMapToDelete.ID == null || fieldMapToDelete.ID.IsEmpty))
            {
                DeleteRecord(entry, "JscFieldMap", "Id", fieldMapToDelete.ID, SchedulerPermissions.DatabaseMapEdit);
            }
        }

        public void Delete(IConnectionManager entry, JscSubJobFromTableFilter fromTableFilter)
        {
            ValidateSecurity(entry, SchedulerPermissions.JobSubjobEdit);


            if (!fromTableFilter.ID.IsEmpty)
            {
                DeleteRecord(entry, "JscSubJobFromTableFilters", "Id", fromTableFilter.ID, SchedulerPermissions.DatabaseMapEdit);
            }

        }

        public void DeleteLinkedFilter(IConnectionManager entry, JscLinkedFilter linkedFilter)
        {
            if (!linkedFilter.ID.IsEmpty)
            {
                DeleteRecord(entry, "JscLinkedFilters", "Id", linkedFilter.ID, SchedulerPermissions.DatabaseDesignEdit);
            }
        }

        public void DeleteLinkedTable(IConnectionManager entry, JscLinkedTable linkedTable)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseDesignEdit);
            var transaction = entry.CreateTransaction();
            try
            {
                DeleteRecord(transaction, "JscLinkedFilters", "Id", linkedTable.ID, SchedulerPermissions.DatabaseDesignEdit);
                DeleteRecord(transaction, "JscLinkedTables", "Id", linkedTable.ID, SchedulerPermissions.DatabaseDesignEdit);
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                transaction.Commit();
            }
        }

        #endregion

        public List<DataSelector> GetDatabaseDesignList(IConnectionManager entry)
        {
            ValidateSecurity(entry,
                             new[]
                                 {
                                     SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.LocationView, SchedulerPermissions.JobSubjobView,
                                     SchedulerPermissions.DatabaseMapView
                                 });

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseGetDatabaseDesignsString + @" 
                              WHERE Enabled = 1 

                              ORDER BY Description";


                List<DataSelector> records = Execute<DataSelector>(entry, cmd, CommandType.Text,
                                                                   PopulateDatabaseDesignDataSelector);

                return records;
            }
        }

        public IEnumerable<JscTableMap> GetDatabaseMaps(IConnectionManager entry, RecordIdentifier tableDesignFilterFrom, RecordIdentifier tableDesignFilterTo)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseMapView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = TableMapGetString;


                if (!(tableDesignFilterTo == null || tableDesignFilterTo.IsEmpty))
                {
                    cmd.CommandText += " Where tm.ToTable = @toTable";
                    MakeParam(cmd, "toTable", (Guid) tableDesignFilterTo);
                }
                if (!(tableDesignFilterFrom == null || tableDesignFilterFrom.IsEmpty))
                {

                    cmd.CommandText +=
                        string.Format(" {0} tm.FromTable = @fromTable ",
                        (tableDesignFilterTo == null || tableDesignFilterTo.IsEmpty)?"Where": "AND");

                    MakeParam(cmd, "fromTable", (Guid) tableDesignFilterFrom);
                }

                cmd.CommandText += " Order by  tdFrom.TableName, tdTo.TableName";


                var records = Execute<JscTableMap>(entry, cmd, CommandType.Text, PopulateTableMap);
                foreach (var jscTableMap in records)
                {
                    jscTableMap.JscFieldMaps = GetFieldMaps(entry, jscTableMap).ToList();
                }

                return records;
            }
        }

        public JscTableMap GetTableMap(IConnectionManager entry, RecordIdentifier tableMapId)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseMapView);

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = TableMapGetString + @"                         
                        WHERE tm.Id = @mapID";

                MakeParam(cmd, "mapID", (Guid) tableMapId);

                var records = Execute<JscTableMap>(entry, cmd, CommandType.Text, PopulateTableMap);
                if (records.Count > 0)
                {
                    records[0].JscFieldMaps = GetFieldMaps(entry, records[0]).ToList();

                    return records[0];
                }
                return null;
            }
        }

        public IEnumerable<JscFieldDesign> GetFieldDesigns(IConnectionManager entry, RecordIdentifier tableDesignId)
        {
            ValidateSecurity(entry,
                             new[] { SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.JobSubjobView, SchedulerPermissions.DatabaseMapView });

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = FieldDesignGetString +@"
                                WHERE 
                                    TableDesign = @tableDesign";

                MakeParam(cmd, "tableDesign", (Guid) tableDesignId);

                return Execute<JscFieldDesign>(entry, cmd, CommandType.Text, PopulateFieldDesign);

            }
        }

        public JscFieldDesign GetFieldDesign(IConnectionManager entry, RecordIdentifier fieldDesignId)
        {
            ValidateSecurity(entry,
                             new[] { SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.JobSubjobView, SchedulerPermissions.DatabaseMapView });

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = FieldDesignGetString +@"
                                WHERE 
                                    Id = @fieldDesign";

                MakeParam(cmd, "fieldDesign", (Guid) fieldDesignId);

                var records = Execute<JscFieldDesign>(entry, cmd, CommandType.Text, PopulateFieldDesign);

                if (records.Count > 0)
                {
                    return records[0];
                }
                return null;
            }
        }

        private IEnumerable<JscFieldMap> GetFieldMaps(IConnectionManager entry, JscTableMap tableMap)
        {
            ValidateSecurity(entry,
                             new[] { SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.JobSubjobView, SchedulerPermissions.DatabaseMapView });

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"
                                    SELECT 
                                    Id,
                                    TableMap,
                                    FromField,
                                    ToField,
                                    ConversionMethod,
                                    ConversionValue
                                FROM 
                                    JscFieldMap
                                WHERE 
                                    TableMap = @tableMap";

                MakeParam(cmd, "tableMap", (Guid) tableMap.ID);

                var records = Execute<JscFieldMap>(entry, cmd, CommandType.Text, PopulateFieldMap);
                foreach (var jscFieldMap in records)
                {
                    jscFieldMap.FromJscFieldDesign = GetFieldDesign(entry, jscFieldMap.FromField);
                    jscFieldMap.ToJscFieldDesign = GetFieldDesign(entry, jscFieldMap.ToField);

                    jscFieldMap.JscTableMap = tableMap;
                }
                return records;
            }
        }

        public IEnumerable<JscFieldDesign> GetFieldDesignsOrderedBySequence(IConnectionManager entry, RecordIdentifier tableDesignId)
        {
            ValidateSecurity(entry, new[] { SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.DatabaseMapView });

            return GetFieldDesigns(entry, tableDesignId).OrderBy(tableDesign => tableDesign.Sequence);
        }

        public IEnumerable<JscFieldDesign> GetFieldDesignsOrderedByFieldName(IConnectionManager entry, RecordIdentifier tableDesignId)
        {
            ValidateSecurity(entry,
                             new[]
                                 {
                                     SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.LocationView, SchedulerPermissions.JobSubjobView,
                                     SchedulerPermissions.DatabaseMapView
                                 });

            return GetFieldDesigns(entry, tableDesignId).OrderBy(tableDesign => tableDesign.FieldName);
        }

        public IEnumerable<JscDatabaseDesign> GetDatabaseDesigns(IConnectionManager entry, bool includeDisabled)
        {
            ValidateSecurity(entry,
                             new[]
                                 {
                                     SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.LocationView, SchedulerPermissions.JobSubjobView,
                                     SchedulerPermissions.DatabaseMapView
                                 });
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseGetDatabaseDesignsString + @"
                                {0}
                                ORDER BY Description";

                cmd.CommandText = string.Format(cmd.CommandText, includeDisabled ? "" : "Where Enabled = 1");

                return Execute<JscDatabaseDesign>(entry, cmd, CommandType.Text, PopulateDatabaseDesign);
            }
        }

        public IEnumerable<JscTableDesign> GetTableDesigns(IConnectionManager entry, RecordIdentifier databaseDesignId, bool onlyWhereLinkedTablesExit)
        {
            ValidateSecurity(entry,
                             new[]
                                 {
                                     SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.LocationView, SchedulerPermissions.JobSubjobView,
                                     SchedulerPermissions.DatabaseMapView
                                 });

            using (var cmd = entry.Connection.CreateCommand())
            {

                if (!onlyWhereLinkedTablesExit)
                {


                    cmd.CommandText = TableDesignGetString+  @"
                                        WHERE DatabaseDesign = @databaseDesign
                                        Order by TableName";

                }
                else
                {
                    cmd.CommandText = TableDesignGetString + @"
                                        inner join 
                                            JscLinkedTables lt on td.Id = lt.FromTable
                                        WHERE 
                                            DatabaseDesign = @databaseDesign
                                        ORDER BY Tablename";
                }

                MakeParam(cmd, "databaseDesign", (Guid) databaseDesignId);

                var records = Execute<JscTableDesign>(entry, cmd, CommandType.Text, PopulateTableDesign);

                if (records != null)
                {
                    foreach (var jscTableDesign in records)
                    {
                        jscTableDesign.JscDatabaseDesign = GetDatabaseDesign(entry, jscTableDesign.DatabaseDesign);
                    }
                }

                return records;
            }
        }

        private static IEnumerable<JscLinkedTable> GetAllLinkedTables(IConnectionManager entry)
        {
            ValidateSecurity(entry, new[] { SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.DatabaseMapView });

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = LinkedTableGetString + @"
                                JOIN 
                                    JscTableDesigns td ON lt.ToTable = td.Id
                                ORDER BY td.TableName";


                return Execute<JscLinkedTable>(entry, cmd, CommandType.Text, PopulateLinkedTable);
            }
        }

        public IEnumerable<JscLinkedTable> GetLinkedTables(IConnectionManager entry,RecordIdentifier tableDesignId)
        {
            ValidateSecurity(entry, new[] { SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.DatabaseMapView });

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = LinkedTableGetString + @"
                                JOIN 
                                    JscTableDesigns td ON lt.ToTable = td.Id
                                WHERE 
                                    lt.FromTable = @fromTable    
                                ORDER BY td.TableName";
                MakeParam(cmd, "fromTable", (Guid) tableDesignId);


                var records = Execute<JscLinkedTable>(entry, cmd, CommandType.Text, PopulateLinkedTable);
                foreach (var jscLinkedTable in records)
                {
                    jscLinkedTable.ToJscTableDesign = GetTableDesign(entry, jscLinkedTable.ToTable);
                    jscLinkedTable.FromJscTableDesign = GetTableDesign(entry, jscLinkedTable.FromTable);
                    jscLinkedTable.JscLinkedFilters = GetLinkedFilters(entry, jscLinkedTable.ID);
                }

                return records;
            }
        }

        private JscLinkedTable GetLinkedTable(IConnectionManager entry, RecordIdentifier tableId)
        {
            ValidateSecurity(entry, new[] { SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.DatabaseMapView });

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = LinkedTableGetString + @"
                                JOIN 
                                    JscTableDesigns td ON lt.ToTable = td.Id
                                WHERE 
                                    lt.ID = @TableId    
                                ORDER BY td.TableName";
                MakeParam(cmd, "TableId", (Guid) tableId);


                var records = Execute<JscLinkedTable>(entry, cmd, CommandType.Text, PopulateLinkedTable);
                if (records.Count > 0)
                {

                    records[0].ToJscTableDesign = GetTableDesign(entry, records[0].ToTable);
                    records[0].FromJscTableDesign = GetTableDesign(entry, records[0].FromTable);
                    records[0].JscLinkedFilters = GetLinkedFilters(entry, records[0].ID);

                    return records[0];
                }
                return null;
            }
        }

        public List<JscLinkedFilter> GetLinkedFilters(IConnectionManager entry, RecordIdentifier tableDesignId)
        {
            ValidateSecurity(entry, new[] { SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.DatabaseMapView });

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"Select
                                    Id,
                                    LinkedTable,
                                    LinkedField,
                                    LinkType,
                                    ToField,
                                    Filter
                                      FROM JscLinkedFilters
                               
                                WHERE 
                                    LinkedTable = @fromTable";

                MakeParam(cmd, "fromTable", (Guid) tableDesignId);


                var records =  Execute<JscLinkedFilter>(entry, cmd, CommandType.Text, PopulateLinkedFilter);



                return records;
            }
        }

        public IEnumerable<JscLocation> GetLocationsUsingDatabase(IConnectionManager entry,RecordIdentifier databaseId)
        {
            ValidateSecurity(entry,
                             new[] {SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.LocationView, SchedulerPermissions.JobSubjobView});

            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText =
                    @" 
                        SELECT 
                            Id,
                            Name,
                            ExDataAreaId,
                            ExCode,
                            DatabaseDesign,
                            LocationKind,
                            DefaultOwner,
                            DDHost,
                            DDPort,
                            DDNetMode,
                            Enabled,
                            Company,
                            UserId,
                            Password,
                            DBServerIsUsed,
                            DBServerHost,
                            DBPathName,
                            DBDriverType,
                            DBConnectionString,
                            SystemTag
                        FROM JscLocations
                        WHERE 
                            DatabaseDesign = @databaseDesign";


                MakeParam(cmd, "databaseDesign", (Guid) databaseId);

                return Execute<JscLocation>(entry, cmd, CommandType.Text, DataProviderFactory.Instance.Get<ILocationData, JscLocation>().PopulateLocation);
            }
        }

        public bool ExistsLinkedTable(IConnectionManager entry, RecordIdentifier fromTable, RecordIdentifier toTable)
        {
            ValidateSecurity(entry, SchedulerPermissions.DatabaseDesignView);

            using (var cmd = entry.Connection.CreateCommand())
            {

                cmd.CommandText = LinkedTableGetString +
                    @" 
                       
                              WHERE 
                            FromTable = @fromTable  AND 
                            ToTable = @toTable";

                MakeParam(cmd, "toTable", (Guid) toTable);
                MakeParam(cmd, "fromTable", (Guid) fromTable);

                return Execute<JscLinkedTable>(entry, cmd, CommandType.Text, PopulateLinkedTable).Any();
            }
        }

        public JscTableDesign GetTableDesign(IConnectionManager entry, RecordIdentifier tableDesignId)
        {
            ValidateSecurity(entry,
                             new[]
                                 {
                                     SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.LocationView, SchedulerPermissions.JobSubjobView,
                                     SchedulerPermissions.DatabaseMapView
                                 });

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = TableDesignGetString + @"
                                    WHERE Id = @tableDesignId";


                MakeParam(cmd, "tableDesignId", (Guid) tableDesignId);

                var records = Execute<JscTableDesign>(entry, cmd, CommandType.Text, PopulateTableDesign);
                if (records.Count > 0)
                {
                    if (records[0].DatabaseDesign != null)
                    {
                        records[0].JscDatabaseDesign = GetDatabaseDesign(entry, records[0].DatabaseDesign);
                    }
                    return records[0];
                }
                return null;
            }
        }

        public JscDatabaseDesign GetDatabaseDesign(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry,
                             new[] {SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.LocationView, SchedulerPermissions.DatabaseMapView});
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseGetDatabaseDesignsString + @"
                                WHERE Id = @databaseDesignID";

                MakeParam(cmd, "databaseDesignID", (Guid) id);


                var records = Execute<JscDatabaseDesign>(entry, cmd, CommandType.Text, PopulateDatabaseDesign);
                if (records.Count > 0)
                {
                    return records[0];
                }
                return null;
            }
        }

        public bool HasFields(IConnectionManager entry, RecordIdentifier tableDesignId)
        {
            ValidateSecurity(entry, new[] {SchedulerPermissions.DatabaseDesignView, SchedulerPermissions.DatabaseMapView});

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = FieldDesignGetString + @"
                                WHERE 
                                    TableDesign = @tableDesignId";

                MakeParam(cmd, "tableDesignId", (Guid) tableDesignId);

                return Execute<JscFieldDesign>(entry, cmd, CommandType.Text, PopulateFieldDesign).Any();
            }
        }
    }
}
