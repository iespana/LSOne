using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Customers
{
	/// <summary>
	/// Data provider class for customer data entities
	/// </summary>
	public class CustomerAddressData : SqlServerDataProviderBase, ICustomerAddressData
	{
		private static string BaseSql
		{
			get
			{
				return
					@"Select A.ID,
							A.ACCOUNTNUM, 
							A.ADDRESSTYPE,
							ISNULL(A.ADDRESS,'') as ADDRESS,
							ISNULL(A.STREET,'') as STREET,
							ISNULL(A.CITY,'') as CITY, 
							ISNULL(A.ZIPCODE,'') as ZIPCODE,
							ISNULL(A.STATE,'') as STATE,
							ISNULL(A.COUNTRY,'') as COUNTRY, 
							A.ADDRESSFORMAT,
							ISNULL(A.CONTACTNAME,'') as CONTACTNAME,
							ISNULL(A.PHONE,'') as PHONE,
							ISNULL(A.CELLULARPHONE,'') as CELLULARPHONE,
							ISNULL(A.EMAIL,'') as EMAIL,
							ISNULL(A.TAXGROUP,'') as TAXGROUP, 
							ISNULL(TG.TAXGROUPNAME,'') as TAXGROUPNAME, 
							ISNULL(A.ISDEFAULT, 0) as ISDEFAULT 
					FROM CUSTOMERADDRESS A 
						left outer join TAXGROUPHEADING TG on A.TAXGROUP = TG.TAXGROUP and A.DATAAREAID = TG.DATAAREAID 
					WHERE A.DATAAREAID = @dataAreaId and A.ACCOUNTNUM = @accountNum 
					ORDER BY A.ADDRESSTYPE";
			}
		}

		private static void PopulateCustomerAddress(IConnectionManager entry, IDataReader dr, CustomerAddress customerAddress, object defaultFormat)
		{
			customerAddress.ID = (Guid)dr["ID"];
			customerAddress.CustomerID = AsString(dr["ACCOUNTNUM"]);
			customerAddress.ContactName = AsString(dr["CONTACTNAME"]);
			customerAddress.Telephone = AsString(dr["PHONE"]);
			customerAddress.MobilePhone = AsString(dr["CELLULARPHONE"]);
			customerAddress.Email = AsString(dr["EMAIL"]);
			customerAddress.IsDefault = AsBool(dr["ISDEFAULT"]);

			var address = customerAddress.Address;

			address.AddressType = (Address.AddressTypes)AsInt(dr["ADDRESSTYPE"]);
			address.Address1 = AsString(dr["STREET"]);
			address.Address2 = AsString(dr["ADDRESS"]);
			address.Zip = AsString(dr["ZIPCODE"]);
			address.City = AsString(dr["CITY"]);
			address.State = AsString(dr["STATE"]);
			address.Country = AsString(dr["COUNTRY"]);
			address.AddressFormat = (dr["ADDRESSFORMAT"] == DBNull.Value) ? (Address.AddressFormatEnum)defaultFormat : (Address.AddressFormatEnum)((int)(dr["ADDRESSFORMAT"]));

			customerAddress.TaxGroup = AsString(dr["TAXGROUP"]);
			customerAddress.TaxGroupName = AsString(dr["TAXGROUPNAME"]);

			customerAddress.InDatabase = true;
		}

		/// <summary>
		/// Gets all address for a particular customer
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="accountNum">Customer id</param>
		/// <returns>List of addresses for the customer</returns>
		public virtual List<CustomerAddress> GetListForCustomer(IConnectionManager entry, RecordIdentifier accountNum)
		{
			ValidateSecurity(entry);

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = BaseSql;

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "accountNum", accountNum);

				return Execute<CustomerAddress>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateCustomerAddress);
			}
		}

		/// <summary>
		/// Gets a specific address for a customer
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="accountNum">ID of the customer to fetch</param>
		/// <param name="addressType">Address to retrieve</param>
		/// <returns>The requested customer address</returns>
		public virtual CustomerAddress Get(IConnectionManager entry, RecordIdentifier accountNum, Address.AddressTypes addressType)
		{
			ValidateSecurity(entry);

			var list = GetListForCustomer(entry, accountNum);

			if (list == null || list.Count == 0)
				return null;

			CustomerAddress defaultAddress = null;
			foreach (var address in list)
			{
				if (address.Address.AddressType == addressType && (address.IsDefault || defaultAddress == null))
					defaultAddress = address;
			}

			return defaultAddress;
		}

		[Obsolete("Use one of the overridden Delete methods", true)]
		public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Deletes the specified customer
		/// </summary>
		/// <remarks>Edit customer permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="customerAddress">Address</param>
		public virtual void Delete(IConnectionManager entry, CustomerAddress customerAddress)
		{
			if (!customerAddress.InDatabase)
				return;

			Delete(entry, customerAddress.CustomerID, customerAddress.ID);
			customerAddress.InDatabase = false;
		}

		/// <summary>
		/// Deletes a customer address by a given ID
		/// </summary>
		/// <remarks>Edit customer permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="accountNum">ID of the customer whose address should be deleted</param>
		/// <param name="addressType">Address to delete</param>
		public virtual void Delete(IConnectionManager entry, RecordIdentifier accountNum, RecordIdentifier id)
		{
			var statement = new SqlServerStatement("CUSTOMERADDRESS") { StatementType = StatementType.Delete };
			statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
			statement.AddCondition("ACCOUNTNUM", (string)accountNum);
			statement.AddCondition("ID", (Guid)id, SqlDbType.UniqueIdentifier);

			entry.Connection.ExecuteStatement(statement);
		}

		/// <summary>
		/// Deletes all addresses for a given customer
		/// </summary>
		/// <remarks>Edit customer permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="accountNum">ID of the customer whose addresses should be deleted</param>
		public virtual void DeleteAll(IConnectionManager entry, RecordIdentifier accountNum)
		{
			var statement = new SqlServerStatement("CUSTOMERADDRESS") { StatementType = StatementType.Delete };
			statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
			statement.AddCondition("ACCOUNTNUM", (string)accountNum);

			entry.Connection.ExecuteStatement(statement);
		}


		[Obsolete("Use the overridden Exists method", true)]
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier accountNum)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Checks if a customer by a given ID exists
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="accountNum">ID of the customer to check for</param>
		/// <param name="addressID">Address to check</param>
		/// <returns>True if the customer exists, else false</returns>
		public virtual bool Exists(IConnectionManager entry, RecordIdentifier accountNum, RecordIdentifier addressID)
		{
			return RecordExists(entry, "CUSTOMERADDRESS", new[] { "ACCOUNTNUM", "ID" },
								new RecordIdentifier(accountNum, addressID));
		}

		/// <summary>
		/// Saves a customer address to the database. If the record does not exist then a Insert is done, else it executes a update statement.
		/// </summary>
		/// <remarks>Edit customer permission is needed to execute this method</remarks>
		/// <param name="entry">Entry into the database</param>
		/// <param name="customerAddress">The customer address to be saved</param>
		public virtual void Save(IConnectionManager entry, CustomerAddress customerAddress)
		{
			if (customerAddress.Address == null)
			{
				return;
			}

			ValidateSecurity(entry, Permission.CustomerEdit);

			customerAddress.Validate();

			var statement = new SqlServerStatement("CUSTOMERADDRESS");

			bool isNew = false;
			if (RecordIdentifier.IsEmptyOrNull(customerAddress.ID))
			{
				isNew = true;
				customerAddress.ID = Guid.NewGuid();
			}

			if (isNew || !Exists(entry, customerAddress.CustomerID, customerAddress.ID))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("ACCOUNTNUM", (string)customerAddress.CustomerID);
				statement.AddKey("ID", (Guid)customerAddress.ID, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("ACCOUNTNUM", (string)customerAddress.CustomerID);
				statement.AddCondition("ID", (Guid)customerAddress.ID, SqlDbType.UniqueIdentifier);
			}


			statement.AddField("ADDRESS", customerAddress.Address.Address2);
			statement.AddField("STREET", customerAddress.Address.Address1);
			statement.AddField("CITY", customerAddress.Address.City);
			statement.AddField("ZIPCODE", customerAddress.Address.Zip);
			statement.AddField("STATE", customerAddress.Address.State);
			statement.AddField("COUNTRY", (string)customerAddress.Address.Country);
			statement.AddField("ADDRESSFORMAT", (int)customerAddress.Address.AddressFormat, SqlDbType.Int);
			statement.AddField("COUNTY", customerAddress.Address.County);
			statement.AddField("ADDRESSTYPE", (int)customerAddress.Address.AddressType, SqlDbType.Int);

			statement.AddField("CONTACTNAME", customerAddress.ContactName);

			statement.AddField("PHONE", customerAddress.Telephone);
			statement.AddField("CELLULARPHONE", customerAddress.MobilePhone);
			statement.AddField("EMAIL", customerAddress.Email);
			statement.AddField("ISDEFAULT", customerAddress.IsDefault, SqlDbType.Bit);

			statement.AddField("TAXGROUP", (string)customerAddress.TaxGroup);

			Save(entry, customerAddress, statement);

			customerAddress.InDatabase = true;
		}

		public virtual bool HasDefaultAddress(IConnectionManager entry, RecordIdentifier customerID, Address.AddressTypes addressType)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = "SELECT 1 FROM CUSTOMERADDRESS WHERE ACCOUNTNUM = @customerID AND ADDRESSTYPE = @addressType AND ISDEFAULT = 1";
				MakeParam(cmd, "customerID", (string)customerID);
				MakeParam(cmd, "addressType", (int)addressType, SqlDbType.Int);
				return entry.Connection.ExecuteScalar(cmd) != null;
			}
		}

		public bool HasAddress(IConnectionManager entry, RecordIdentifier customerID)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"SELECT 1
					  FROM CUSTOMERADDRESS
					  WHERE ACCOUNTNUM = @customerID";

				MakeParam(cmd, "customerID", (string)customerID);

				return entry.Connection.ExecuteScalar(cmd) != null;
			}
		}

		public virtual void SetAddressAsDefault(IConnectionManager entry, RecordIdentifier customerID, CustomerAddress customerAddress)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText = "UPDATE CUSTOMERADDRESS SET ISDEFAULT = 0 WHERE ACCOUNTNUM = @customerID AND ADDRESSTYPE = @addressType AND ISDEFAULT = 1";
				MakeParam(cmd, "customerID", (string)customerID);
				MakeParam(cmd, "addressType", (int)customerAddress.Address.AddressType, SqlDbType.Int);
				entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
			}

			customerAddress.IsDefault = true;
			Save(entry, customerAddress);
		}
	}
}
