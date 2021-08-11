using System;
using System.Data;
using LSOne.Services.Interfaces.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionObjects.MemoryTables
{
    [Serializable]
    public class Period : IPeriod
    {
        private DataTable periodTable;        

        public Period()
        {
            CreatePeriodTable();
        }

        /// <summary>
        /// Returns if info is found if a period is found, and if it is valid or not.
        /// </summary>
        /// <param name="periodId">The period id to check</param>
        /// <returns>Returns if period is found, and if it is valid or not.</returns>
        public PeriodStatus IsValid(string periodId)
        {
            PeriodStatus periodStatus = PeriodStatus.NotFoundInMemoryTable;

            if (periodId == "")
            {
                return PeriodStatus.IsValid;
            }

            foreach (DataRow row in periodTable.Select("PeriodId='" + periodId + "'", ""))
            {
                periodStatus = (bool)row["Valid"] ? PeriodStatus.IsValid : PeriodStatus.IsInvalid;
            }

            return periodStatus;
        }

        /// <summary>
        /// To clear the memory table if needed.
        /// </summary>
        public void Clear()
        {
            periodTable.Clear();
        }

        /// <summary>
        /// Create the memory table to hold infomation if a period is valid or not.
        /// </summary>
        private void CreatePeriodTable()
        {
            // Create a new DataTable.
            periodTable = new DataTable("PeriodTable");
            DataColumn column;

            //Adding PeriodId
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "PeriodId";
            column.AutoIncrement = false;
            column.Caption = "PeriodId";
            column.ReadOnly = false;
            column.Unique = true;
            periodTable.Columns.Add(column);

            //Adding Valid
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Boolean");
            column.ColumnName = "Valid";
            column.AutoIncrement = false;
            column.Caption = "Valid";
            column.ReadOnly = false;
            column.Unique = false;
            periodTable.Columns.Add(column);

            //Creat the primary key of the table.
            DataColumn[] pk = new DataColumn[1];
            pk[0] = periodTable.Columns["PeriodId"];
            periodTable.PrimaryKey = pk;
        }

        /// <summary>
        /// Add's a new period if not found in memorytable
        /// </summary>
        /// <param name="periodId">The id of the peroid.</param>
        /// <param name="valid">True if a period is valid, else it is false.</param>
        public void Add(string periodId, Boolean valid)
        {
            Boolean found = false;
            foreach (DataRow row in periodTable.Select("PeriodId='" + periodId + "'", ""))
            {
                found = true;
            }
            if (!found)
            {
                DataRow periodRow = periodTable.NewRow();
                periodRow["PeriodId"] = periodId;
                periodRow["Valid"] = valid;
                periodTable.Rows.Add(periodRow);
            }
        }
    }

    
}
