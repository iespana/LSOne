using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace LSOne.DataLayer.SqlConnector.QueryHelpers
{
    public enum AggregationSetting
    {
        None,
        ExternalAggregation,
        InternalAggregation
        
    };
    
public class TableColumn
    {
       
        private string _columnAlias;
        private string _tableAlias;

        public TableColumn()
        {
            _columnAlias = string.Empty;
            _tableAlias = string.Empty;
            ColumnName = string.Empty;
            NullValue = "''";

        }
        public string TableAlias
        {
            get { return string.IsNullOrEmpty(_tableAlias) ? string.Empty : _tableAlias + "."; }
            set { _tableAlias = value; }
        }

        public bool SortDescending { get; set; }
        public string ColumnName { get; set; }
        public bool ExcludeExternally { get; set; }
        public bool IsCustomExternalColumn { get; set; }
        public string CustomText { get; set; }
        public bool IsNull { get; set; }

        public string ColumnAlias
        {
            get { return string.IsNullOrEmpty(_columnAlias) ? ColumnName : _columnAlias; }
            set { _columnAlias = value; }
        }

        public string NullValue { get; set; }
        public AggregationSetting AggregateExternally { get; set; }
        public string AggregateFunction { get; set; }
        public override string ToString()
        {
            string isNullStart = IsNull ? "ISNULL(" : "";
            string isNullEnd = IsNull ? $", {NullValue})" : "";
            string aggregateStart = AggregateFunction != string.Empty && AggregateExternally == AggregationSetting.InternalAggregation ? $"{AggregateFunction}(" : string.Empty;
            string aggregateEnd = AggregateFunction != string.Empty && AggregateExternally == AggregationSetting.InternalAggregation ? $")" : string.Empty;
            return $"{aggregateStart}{isNullStart}{TableAlias}{ColumnName}{isNullEnd}{aggregateEnd}{(ColumnAlias == ColumnName && !IsNull && AggregateExternally != AggregationSetting.InternalAggregation ? string.Empty : " AS " + ColumnAlias)}";
        }

        public string ToSortString(bool externalColumn = false)
        {
            if (externalColumn)
            {
                return string.Format("{0}{1}", ColumnAlias, SortDescending ? " DESC" : " ASC");
            }
            return string.Format("{0}{1}{2}", TableAlias, ColumnName, SortDescending ? " DESC" : " ASC");

        }
        public string ToGroupByString(bool externalColumn = false)
        {
            if (externalColumn)
            {
                return  ColumnAlias;
            }
            return string.Format("{0}{1}", TableAlias, ColumnName);

        }
    }
}
