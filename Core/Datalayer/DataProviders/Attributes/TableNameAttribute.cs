using System;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.DataLayer.DataProviders.Attributes
{
	/// <summary>
	/// Specifies a conditional attribute
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple=false, Inherited=true)]
	public class TableNameAttribute : Attribute
	{
		public TableNameAttribute(string tableName)
		{
			TableName = tableName;
		}

		/// <summary>
		/// The name of the table
		/// </summary>
		public string TableName { get; private set; }
	}
}