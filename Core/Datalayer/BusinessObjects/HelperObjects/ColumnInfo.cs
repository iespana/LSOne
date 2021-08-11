using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects.HelperObjects
{
    public enum ColumnVariableType
    {
        String = 0,
        Decimal = 1,
        Integer = 2,
        Boolean = 3,
    }

    public class ColumnInfo
    {
        public ReceiptItemType ReceiptItem { get; private set; }
        public string ColumnValue { get; private set; }
        public ColumnVariableType ColumnType { get; private set; }
        public bool Visible { get; private set; }
        public bool Updated { get; private set; }
        //public bool ValueSet { get; set; }

        public ColumnInfo(ReceiptItemType ReceiptItem, string ColumnValue, ColumnVariableType ColumnType, bool Visible)
        {
            this.ReceiptItem = ReceiptItem;
            this.ColumnValue = ColumnValue;
            this.Visible = Visible;
            this.ColumnType = ColumnType;
        }

        public void UpdateColumnInfo(string ColumnValue, bool Visible)
        {
            this.ColumnValue = ColumnValue;
            this.Visible = Visible;
            this.Updated = true;
        }

        public void UpdateColumnInfo(string ColumnValue)
        {
            this.ColumnValue = ColumnValue;
            this.Updated = true;
        }

        public void UpdateColumnInfo(bool ColumnValue)
        {
            if (ColumnValue == true)
            {
                this.ColumnValue = "True";
            }
            else
            {
                this.ColumnValue = "False";
            }
        }

        public void ColumnVisible(bool Visible)
        {
            this.Visible = Visible;
            this.Updated = true;
        }

        public void ResetUpdated()
        {
            Updated = false;
        }
    }
}
