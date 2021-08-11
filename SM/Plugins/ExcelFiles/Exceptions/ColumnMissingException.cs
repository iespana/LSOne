using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.ExcelFiles.Exceptions
{

    public class ColumnMissingException : Exception
    {
        string columnName;

        public ColumnMissingException (string columnName)
            : base(Properties.Resources.MandatoryColumnmissing + " " + columnName)
	    {
            this.columnName = columnName;
	    }

        protected ColumnMissingException() { }
        protected ColumnMissingException(string message, Exception inner) : base(message, inner) { }
        protected ColumnMissingException( 
	        System.Runtime.Serialization.SerializationInfo info, 
	        System.Runtime.Serialization.StreamingContext context ) : base( info, context ) { }

        public string ColumnName
        {
            get { return columnName; }
        }
    }
}
