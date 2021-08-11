using System;
using System.Data;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionObjects.MemoryTables
{
    [Serializable]
    public class PeriodicDiscount : IPeriodicDiscount
    {
        private DataTable periodDiscountTable;

        public PeriodicDiscount()
        {
            CreatePeriodDiscountTable();
        }

        public void Add(DataTable periodDiscountData, string itemId)
        {
            foreach (DataRow dataRow in periodDiscountData.Select())
            {
                DataRow row;
                row = periodDiscountTable.NewRow();
                row["OfferId"] = dataRow["OfferId"];
                row["Description"] = dataRow["Description"];
                row["PDType"] = dataRow["PDType"];
                row["Priority"] = dataRow["Priority"];
                row["DiscValidPeriodId"] = dataRow["DiscValidPeriodId"];
                row["DiscountType"] = dataRow["DiscountType"];
                row["SameDiffMMLines"] = dataRow["SameDiffMMLines"];
                row["NoOfLinesToTrigger"] = dataRow["NoOfLinesToTrigger"];
                row["DealPriceValue"] = dataRow["DealPriceValue"];
                row["DiscountPctValue"] = dataRow["DiscountPctValue"];
                row["DiscountAmountValue"] = dataRow["DiscountAmountValue"];
                row["NoOfLeastExpItems"] = dataRow["NoOfLeastExpItems"];
                row["NoOfTimesApplicable"] = dataRow["NoOfTimesApplicable"];
                row["LineId"] = dataRow["LineId"];
                row["ProductType"] = dataRow["ProductType"];
                row["PriceGroup"] = dataRow["PriceGroup"];
                if ((int)dataRow["ProductType"] == 5 || (int)dataRow["ProductType"] == 3) //DiscountOfferProductTypes.SpecialGroup
                {
                    row["Id"] = itemId;
                }
                else
                {                    
                    row["Id"] = dataRow["Id"];
                }                
                row["DealPriceOrDiscPct"] = dataRow["DealPriceOrDiscPct"];
                row["LineGroup"] = dataRow["LineGroup"];
                row["DiscType"] = dataRow["DiscType"];
                row["NoOfItemsNeeded"] = dataRow["NoOfItemsNeeded"];
                periodDiscountTable.Rows.Add(row);

            }
        }

        public void AddItem(string id)
        {
            DataRow row;
            row = periodDiscountTable.NewRow();
            row["OfferId"] = "";
            row["Id"] = id;
            row["ProductType"] = "0";
            periodDiscountTable.Rows.Add(row);
        }

        public DataTable GetItem(string itemId)
        {
            DataView periodDiscountView = new DataView(periodDiscountTable);
            periodDiscountView.RowFilter = "ProductType=0 AND Id='" + itemId + "'";
            return periodDiscountView.ToTable();
        }

        public void AddRetailGroup(string id)
        {
            DataRow row;
            row = periodDiscountTable.NewRow();
            row["OfferId"] = "";
            row["Id"] = id;
            row["ProductType"] = "1";
            periodDiscountTable.Rows.Add(row);
        }

        public DataTable GetRetailGroup(string groupID)
        {
            DataView periodDiscountView = new DataView(periodDiscountTable);
            periodDiscountView.RowFilter = "ProductType=1 AND Id='" + groupID + "'";
            return periodDiscountView.ToTable();
        }

        public void AddBarcode(string id)
        {
            DataRow row;
            row = periodDiscountTable.NewRow();
            row["OfferId"] = "";
            row["Id"] = id;
            row["ProductType"] = "4";
            periodDiscountTable.Rows.Add(row);
        }

        public DataTable GetBarcode(string barcode)
        {
            DataView periodDiscountView = new DataView(periodDiscountTable);
            periodDiscountView.RowFilter = "ProductType=4 AND Id='" + barcode + "'";
            return periodDiscountView.ToTable();
        }

        public void AddDepartment(string id)
        {
            DataRow row;
            row = periodDiscountTable.NewRow();
            row["OfferId"] = "";
            row["Id"] = id;
            row["ProductType"] = "2";
            periodDiscountTable.Rows.Add(row);
        }

        public DataTable GetDepartment(string departmentID)
        {
            DataView periodDiscountView = new DataView(periodDiscountTable);
            periodDiscountView.RowFilter = "ProductType=2 AND Id='" + departmentID + "'";
            return periodDiscountView.ToTable();
        }

        public DataTable Get(string itemId, string retailGroupId, string departmentId)
        {
            DataView periodDiscountView = new DataView(periodDiscountTable);
            periodDiscountView.RowFilter = "Id='" + itemId + "' OR Id='" + retailGroupId + "' OR Id='" + departmentId + "'";
            return periodDiscountView.ToTable();
        }

        public void Clear()
        {
            periodDiscountTable.Clear();
        }

        private void CreatePeriodDiscountTable()
        {
            // Create a new DataTable.
            periodDiscountTable = new DataTable("PeriodDiscountTable");
            DataColumn column;

            //Adding OfferId
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "OfferId";
            column.AutoIncrement = false;
            column.Caption = "OfferId";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding Description
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Description";
            column.AutoIncrement = false;
            column.Caption = "Description";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding Status
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Status";
            column.AutoIncrement = false;
            column.Caption = "Status";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding PDType
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "PDType";
            column.AutoIncrement = false;
            column.Caption = "PDType";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding Priority
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "Priority";
            column.AutoIncrement = false;
            column.Caption = "Priority";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding DiscValidPeriodId
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "DiscValidPeriodId";
            column.AutoIncrement = false;
            column.Caption = "DiscValidPeriodId";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding DiscountType
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "DiscountType";
            column.AutoIncrement = false;
            column.Caption = "DiscountType";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding SameDiffMMLines
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "SameDiffMMLines";
            column.AutoIncrement = false;
            column.Caption = "SameDiffMMLines";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding NoOfLinesToTrigger
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "NoOfLinesToTrigger";
            column.AutoIncrement = false;
            column.Caption = "NoOfLinesToTrigger";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding DealPriceValue
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Decimal");
            column.ColumnName = "DealPriceValue";
            column.AutoIncrement = false;
            column.Caption = "DealPriceValue";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding DiscountPctValue
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Decimal");
            column.ColumnName = "DiscountPctValue";
            column.AutoIncrement = false;
            column.Caption = "DiscountPctValue";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding DiscountAmountValue
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Decimal");
            column.ColumnName = "DiscountAmountValue";
            column.AutoIncrement = false;
            column.Caption = "DiscountAmountValue";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding NoOfLeastExpItems
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "NoOfLeastExpItems";
            column.AutoIncrement = false;
            column.Caption = "NoOfLeastExpItems";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding NoOfTimesApplicable
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "NoOfTimesApplicable";
            column.AutoIncrement = false;
            column.Caption = "NoOfTimesApplicable";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding LineId
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "LineId";
            column.AutoIncrement = false;
            column.Caption = "LineId";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding ProductType
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ProductType";
            column.AutoIncrement = false;
            column.Caption = "ProductType";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding Id item og departmentid
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "Id";
            column.AutoIncrement = false;
            column.Caption = "Id";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding DealPriceOrDiscPct
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Decimal");
            column.ColumnName = "DealPriceOrDiscPct";
            column.AutoIncrement = false;
            column.Caption = "DealPriceOrDiscPct";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding LineGroup
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "LineGroup";
            column.AutoIncrement = false;
            column.Caption = "LineGroup";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding DiscType
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "DiscType";
            column.AutoIncrement = false;
            column.Caption = "DiscType";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding NoOfItemsNeeded
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "NoOfItemsNeeded";
            column.AutoIncrement = false;
            column.Caption = "NoOfItemsNeeded";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Adding NoOfLeastExpItems
            column = new DataColumn();
            column.DataType = System.Type.GetType("System.String");
            column.ColumnName = "PriceGroup";
            column.AutoIncrement = false;
            column.Caption = "PriceGroup";
            column.ReadOnly = false;
            column.Unique = false;
            periodDiscountTable.Columns.Add(column);

            //Creat the primary key of the table.
            /*DataColumn[] pk = new DataColumn[2];
            pk[0] = periodDiscountTable.Columns["OfferId"];
            pk[1] = periodDiscountTable.Columns["LineId"];
            periodDiscountTable.PrimaryKey = pk;
            */
        }
    }
}
