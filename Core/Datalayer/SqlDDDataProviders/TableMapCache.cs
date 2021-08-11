using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.DataProviders
{
    internal class TableMapCache : SqlServerDataProvider
    {
        private HashSet<RecordIdentifier> tableIds = new HashSet<RecordIdentifier>();
        public TableMapCache(IConnectionManager entry, Guid sourceDatabaseDesign, Guid destinationDatabaseDesign)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"                         
                        SELECT 
                            tm.Id,
                            tm.FromTable,
                            tm.ToTable,
                            tm.Description
                        FROM JscTableMap tm
						    join JscTableDesigns tdfrom on tm.FromTable = tdfrom.Id
						    join JscTableDesigns tdto on tm.ToTable =  tdto.id
                        WHERE
                            tdfrom.DatabaseDesign == @sourceDatabaseDesign &&
                            tdto.DatabaseDesign == @destinationDatabaseDesign";

                MakeParam(cmd, "sourceDatabaseDesign", sourceDatabaseDesign);
                MakeParam(cmd, "destinationDatabaseDesign", destinationDatabaseDesign);

                var fromTableIds = Execute(entry, cmd, CommandType.Text, "FromTable");
                
                foreach (var tableId in fromTableIds)
                {
                    tableIds.Add(tableId);
                }
            }
        }

        public bool Exists(RecordIdentifier tableDesign)
        {
            return tableIds.Contains(tableDesign);
        }
    }
}
