using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewPlugins.Administration.DataLayer.DataEntities;

namespace LSOne.ViewPlugins.Administration.QueryResults
{
    [Serializable()]
    public class AuditLogResult
    {
        private List<string> columnNames;
        private List<AuditLogRecord> records;

        public AuditLogResult(List<string> columnNames,List<AuditLogRecord> records)
        {
            this.columnNames = columnNames;
            this.records     = records;
        }

        public List<string> ColumnNames
        {
            get { return columnNames; }
            private set { columnNames = value; }
        }

        public List<AuditLogRecord> Records
        {
            get { return records; }
            private set { records = value; }
        }
    }
}

