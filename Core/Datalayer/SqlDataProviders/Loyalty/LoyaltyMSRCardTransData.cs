using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Loyalty
{
    /// <summary>
    /// A Data provider that retrieves the data for the business object <see cref="LoyaltyMSRCardTrans"/>
    /// </summary>
    public class LoyaltyMSRCardTransData : SqlServerDataProviderBase, ILoyaltyMSRCardTransData
    {
	    private static string BaseSelectString
		{
			get
			{
				return
					@"SELECT ISNULL(a.[CARDNUMBER], '') CARDNUMBER
					, a.[LINENUM] LINENUM
					, ISNULL(a.[TRANSACTIONID], '') TRANSACTIONID
					, ISNULL(a.[RECEIPTID], '') RECEIPTID
					, ISNULL(a.[POINTS], 0) POINTS
					, a.[DATEOFISSUE] DATEOFISSUE
					, ISNULL(a.[STOREID], '') STOREID
					, ISNULL(rbo.NAME, '') as STORENAME
					, ISNULL(a.[TERMINALID], '') TERMINALID
					, ISNULL(rboterm.NAME, '') as TERMINALNAME
					, ISNULL(a.[SEQUENCENUMBER], 0) SEQUENCENUMBER
					, ISNULL(a.[LOYALTYCUSTID], '') LOYALTYCUSTID
					, ISNULL(cust.NAME, '') as CUSTNAME
					, ISNULL(a.[ENTRYTYPE], 0) ENTRYTYPE
					, a.[EXPIRATIONDATE] EXPIRATIONDATE
					, ISNULL(a.[LOYALTYSCHEMEID], '') LOYALTYSCHEMEID
					, ISNULL(rboscheme.DESCRIPTION, '') as SCHEMEDESCRIPTION
					, ISNULL(a.[STATEMENTID], '') STATEMENTID
					, ISNULL(a.[STATEMENTCODE], '') STATEMENTCODE
					, ISNULL(a.[STAFFID], '') STAFFID
					, ISNULL(staff.NAME,'') AS STAFFNAME
					, ISNULL(a.[LOYALTYPOINTTRANSLINENUM], 0) LOYALTYPOINTTRANSLINENUM

					, a.[MODIFIEDDATE] MODIFIEDDATE
					, a.[MODIFIEDTIME] MODIFIEDTIME
					, ISNULL(a.[MODIFIEDBY], '00000000-0000-0000-0000-000000000000') MODIFIEDBY
					, a.[CREATEDDATE] CREATEDDATE
					, a.[CREATEDTIME] CREATEDTIME
					, ISNULL(a.[CREATEDBY], '00000000-0000-0000-0000-000000000000') CREATEDBY
					, ISNULL(a.[TYPE], 0) TYPE
					, ISNULL(a.[REMAININGPOINTS], 0) REMAININGPOINTS
					, ISNULL(a.[STATUS], 1) [STATUS]";
			}
		}

		private static void PopulateLoyaltyMsrCardTrans(IDataReader dr, LoyaltyMSRCardTrans loyaltyMsrCardTrans)
		{
			loyaltyMsrCardTrans.ID = (string)dr["CARDNUMBER"];
			loyaltyMsrCardTrans.ID.SecondaryID = (decimal)dr["LINENUM"];
			loyaltyMsrCardTrans.CardNumber = (string)dr["CARDNUMBER"];
			loyaltyMsrCardTrans.LineNumber = (decimal)dr["LINENUM"];

			loyaltyMsrCardTrans.TransactionID = (string)dr["TRANSACTIONID"];
			loyaltyMsrCardTrans.ReceiptID = (string)dr["RECEIPTID"];
			loyaltyMsrCardTrans.Points = (decimal)dr["POINTS"];
			loyaltyMsrCardTrans.DateOfIssue = (dr["DATEOFISSUE"] == null) ? Date.Empty : Date.FromAxaptaDate(dr["DATEOFISSUE"]);
			loyaltyMsrCardTrans.StoreID = (string)dr["STOREID"];
			loyaltyMsrCardTrans.StoreName = (string)dr["STORENAME"];
			loyaltyMsrCardTrans.TerminalID = (string)dr["TERMINALID"];
			loyaltyMsrCardTrans.TerminalName = (string)dr["TERMINALNAME"];
			loyaltyMsrCardTrans.SequenceNumber = (int)dr["SEQUENCENUMBER"];
			loyaltyMsrCardTrans.CustomerID = (string)dr["LOYALTYCUSTID"];
			loyaltyMsrCardTrans.CustomerName = (string)(dr["CUSTNAME"]);
			loyaltyMsrCardTrans.EntryType = (LoyaltyMSRCardTrans.EntryTypeEnum)dr["ENTRYTYPE"];
			loyaltyMsrCardTrans.ExpirationDate = (dr["EXPIRATIONDATE"] == null) ? Date.Empty : Date.FromAxaptaDate(dr["EXPIRATIONDATE"]);
			loyaltyMsrCardTrans.SchemeID = (string)dr["LOYALTYSCHEMEID"];
			loyaltyMsrCardTrans.SchemeDescription = (string)dr["SCHEMEDESCRIPTION"];
			loyaltyMsrCardTrans.StatementID = (string)dr["STATEMENTID"];
			loyaltyMsrCardTrans.StatementCode = (string)dr["STATEMENTCODE"];
			loyaltyMsrCardTrans.StaffID = (string)dr["STAFFID"];
			loyaltyMsrCardTrans.StaffName = (string)dr["STAFFNAME"];
			loyaltyMsrCardTrans.LoyPointsTransLineNumber = (decimal)dr["LOYALTYPOINTTRANSLINENUM"];
			loyaltyMsrCardTrans.ModifiedDate = (dr["MODIFIEDDATE"] == null) ? Date.Empty : new Date((DateTime)dr["MODIFIEDDATE"]);
			loyaltyMsrCardTrans.ModifiedTime = (int)dr["MODIFIEDTIME"];
			loyaltyMsrCardTrans.ModifiedBy = (Guid)dr["MODIFIEDBY"];
			loyaltyMsrCardTrans.CreatedDate = (dr["CREATEDDATE"] == null) ? Date.Empty : new Date((DateTime)dr["CREATEDDATE"]);
			loyaltyMsrCardTrans.CreatedTime = (int)dr["CREATEDTIME"];
			loyaltyMsrCardTrans.CreatedBy = (Guid)dr["CREATEDBY"];
			loyaltyMsrCardTrans.Type = (LoyaltyMSRCardTrans.TypeEnum)dr["TYPE"];
			loyaltyMsrCardTrans.RemainingPoints = (decimal)dr["REMAININGPOINTS"];
			loyaltyMsrCardTrans.Open = (byte)dr["STATUS"] != 0;
		}

		/// <summary>
		/// Gets the specified entry.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="loyMsrCardTransID">The CardNumber and LineNumber of the entry.</param>
		/// <returns>An instance of <see cref="LoyaltyMSRCardTrans"/></returns>
		public virtual LoyaltyMSRCardTrans Get(IConnectionManager entry, RecordIdentifier loyMsrCardTransID)
		{
			List<LoyaltyMSRCardTrans> result;

			using (SqlCommand cmd = new SqlCommand())
			{
				cmd.CommandText = BaseSelectString
					+ @" FROM RBOLOYALTYTRANS as a 
					LEFT OUTER JOIN RBOSTORETABLE rbo on a.STOREID = rbo.STOREID and a.DATAAREAID = rbo.DATAAREAID 
					LEFT OUTER JOIN RBOTERMINALTABLE rboterm on a.TERMINALID = rboterm.TERMINALID and a.STOREID = rboterm.STOREID and a.DATAAREAID = rboterm.DATAAREAID 
					LEFT OUTER JOIN RBOLOYALTYSCHEMESTABLE rboscheme on a.LOYALTYSCHEMEID = rboscheme.LOYALTYSCHEMEID and a.DATAAREAID = rboscheme.DATAAREAID 
					LEFT OUTER JOIN CUSTOMER cust on a.LOYALTYCUSTID = cust.ACCOUNTNUM and a.DATAAREAID = cust.DATAAREAID
					LEFT OUTER JOIN USERS u ON a.STAFFID = u.LOGIN AND a.DATAAREAID = u.DATAAREAID
					LEFT OUTER JOIN RBOSTAFFTABLE staff ON staff.STAFFID = u.STAFFID AND a.DATAAREAID = staff.DATAAREAID
					where a.CARDNUMBER = @cardNumber and a.DATAAREAID = @dataAreaId and a.LINENUM = @lineNumber ";

				MakeParam(cmd, "cardNumber", (string)loyMsrCardTransID.PrimaryID);
				MakeParam(cmd, "lineNumber", (decimal)loyMsrCardTransID.SecondaryID, SqlDbType.Decimal);
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				result = Execute<LoyaltyMSRCardTrans>(entry, cmd, CommandType.Text, PopulateLoyaltyMsrCardTrans);
			}

			return result.Count > 0 ? result[0] : null;
		}

        public virtual int GetListCount(IConnectionManager entry, RecordIdentifier cardNumber)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select Count('x') from RBOLOYALTYTRANS where CARDNUMBER = @cardNumber and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "cardNumber", (string)cardNumber);

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }

        public virtual int NumberOfCustomerTransactionsForCard(IConnectionManager entry, RecordIdentifier cardNumber)
        {
            // Customer transactions have an empty transaction id
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select Count('x') from RBOLOYALTYTRANS where CARDNUMBER = @cardNumber and TRANSACTIONID = '' and DATAAREAID = @dataAreaId and TYPE <> 0";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "cardNumber", (string)cardNumber);

                return (int)entry.Connection.ExecuteScalar(cmd);
            }
        }
        
		/// <summary>
		/// Gets the list of transactions.
		/// </summary>
		/// <param name="entry">The entry into the database</param> 
		/// <param name="storeFilter">Filter by store. Note that if this is null or empty then filter is disabled</param>
		/// <param name="terminalFilter">Filter by terminal. Note that if this is null or empty then filter is disabled</param>
		/// <param name="msrCardFilter">Fulter by MSRCard. Note that if this is null or empty then filter is disabled</param>
		/// <param name="schemeFilter">Filter by scheme. Note that if this is null or empty then filter is disabled</param>
		/// <param name="typeFilter">Filter by type. Note that if this is less than zero then filter is disabled</param>
		/// <param name="openFilter">Filter by status. Note that if this is less than zero then filter is disabled</param>
		/// <param name="entryTypeFilter">Filter by entry type. Note that if this is less than zero then filter is disabled</param>
		/// <param name="customerFilter">Filter by customer. Note that if this is null or empty then filter is disabled</param>
		/// <param name="receiptID">Filter by receiptID. Note that if this is null or empty then filter is disabled</param>
		/// <param name="dateFrom">Filter by date. Note that if this is empty then filter is disabled</param>
		/// <param name="dateTo">Filter by date. Note that if this is empty then filter is disabled</param>
		/// <param name="expiredateFrom">Filter by expire date. Note that if this is empty then filter is disabled</param>
		/// <param name="expiredateTo">Filter by expire date. Note that if this is empty then filter is disabled</param>
		/// <param name="rowFrom">The number of the first row to fetch</param>
		/// <param name="rowTo">The number of the last row to fetch</param>
		/// <param name="backwards">Set to true if wanting backwards sort</param>
		/// <returns>A list of instances of <see cref="LoyaltyMSRCardTrans"/></returns>
		public List<LoyaltyMSRCardTrans> GetList(IConnectionManager entry,
			string storeFilter,
			string terminalFilter,
			string msrCardFilter,
			string schemeFilter,
			int typeFilter,
			int openFilter,
			int entryTypeFilter,
			string customerFilter,
			string receiptID,
			Date dateFrom,
			Date dateTo,
			Date expiredateFrom,
			Date expiredateTo,
			int rowFrom, int rowTo, bool backwards = false)
		{

			List<LoyaltyMSRCardTrans> result;

			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				string sort;
				sort = "SEQUENCENUMBER ASC";
				if (backwards)
				{
					sort = sort.Replace("ASC", "DESC");
				}

				if (rowTo > 0)
				{
					cmd.CommandText = "select s.* from (";
				}

				cmd.CommandText = cmd.CommandText + BaseSelectString;
				if (rowTo > 0)
				{
					cmd.CommandText = cmd.CommandText
					+ ",ROW_NUMBER() OVER(";
					cmd.CommandText = cmd.CommandText
						+ " ORDER BY " + sort;
					cmd.CommandText = cmd.CommandText
						+ ") AS ROW";
				}

				cmd.CommandText = cmd.CommandText + @" FROM RBOLOYALTYTRANS as a 
				LEFT OUTER JOIN RBOSTORETABLE rbo on a.STOREID = rbo.STOREID and a.DATAAREAID = rbo.DATAAREAID 
				LEFT OUTER JOIN RBOTERMINALTABLE rboterm on a.TERMINALID = rboterm.TERMINALID and a.STOREID = rboterm.STOREID and a.DATAAREAID = rboterm.DATAAREAID 
				LEFT OUTER JOIN RBOLOYALTYSCHEMESTABLE rboscheme on a.LOYALTYSCHEMEID = rboscheme.LOYALTYSCHEMEID and a.DATAAREAID = rboscheme.DATAAREAID 
				LEFT OUTER JOIN CUSTOMER cust on a.LOYALTYCUSTID = cust.ACCOUNTNUM and a.DATAAREAID = cust.DATAAREAID
				LEFT OUTER JOIN USERS u ON a.STAFFID = u.LOGIN AND a.DATAAREAID = u.DATAAREAID
				LEFT OUTER JOIN RBOSTAFFTABLE staff ON staff.STAFFID = u.STAFFID AND a.DATAAREAID = staff.DATAAREAID
				where a.DATAAREAID = @dataAreaId";

				#region filtering
				if (!String.IsNullOrEmpty(storeFilter))
				{
					cmd.CommandText += " and a.STOREID = @storeId";
					MakeParam(cmd, "storeId", storeFilter);
				}
				if (!String.IsNullOrEmpty(terminalFilter))
				{
					cmd.CommandText = cmd.CommandText + " and a.TERMINALID = @terminalId";
					MakeParam(cmd, "terminalId", terminalFilter);
				}
				if (!String.IsNullOrEmpty(msrCardFilter))
				{
					cmd.CommandText = cmd.CommandText + " and a.CARDNUMBER = @cardNumber";
					MakeParam(cmd, "cardNumber", msrCardFilter);
				}
				if (!String.IsNullOrEmpty(schemeFilter))
				{
					cmd.CommandText = cmd.CommandText + " and a.LOYALTYSCHEMEID = @schemeId";
					MakeParam(cmd, "schemeId", schemeFilter);
				}
				if (typeFilter > 0)
				{
				    if (typeFilter >= (int)LoyaltyMSRCardTransTypeSearchEnum.IssuePoints)
				    {
                        LoyaltyMSRCardTransTypeSearchEnum types = (LoyaltyMSRCardTransTypeSearchEnum)typeFilter;
				        cmd.CommandText += " and (";
                        cmd.CommandText += (types & LoyaltyMSRCardTransTypeSearchEnum.IssuePoints) == LoyaltyMSRCardTransTypeSearchEnum.IssuePoints ? "a.[TYPE] = 0"  : "";
				        cmd.CommandText += (types & LoyaltyMSRCardTransTypeSearchEnum.UsePoints) == LoyaltyMSRCardTransTypeSearchEnum.UsePoints ? 
                            ((types & LoyaltyMSRCardTransTypeSearchEnum.IssuePoints) == LoyaltyMSRCardTransTypeSearchEnum.IssuePoints ? " or " : "") + "a.[TYPE] = 1" : "";
				        cmd.CommandText += (types & LoyaltyMSRCardTransTypeSearchEnum.ExpirePoints) == LoyaltyMSRCardTransTypeSearchEnum.ExpirePoints ? 
                            ((types & LoyaltyMSRCardTransTypeSearchEnum.IssuePoints) == LoyaltyMSRCardTransTypeSearchEnum.IssuePoints || (types & LoyaltyMSRCardTransTypeSearchEnum.UsePoints) == LoyaltyMSRCardTransTypeSearchEnum.UsePoints ? " or " : "") + "a.[TYPE] = 2" : "";
				        cmd.CommandText += ")";
				    }
				    else
				    {
				        cmd.CommandText = cmd.CommandText + " and a.[TYPE] = @typeValue";
				        MakeParam(cmd, "typeValue", typeFilter);
				    }
				}
				if (openFilter >= 0)
				{
					cmd.CommandText = cmd.CommandText + " and a.[STATUS] = @openValue";
					MakeParam(cmd, "openValue", openFilter);
				}
				if (entryTypeFilter >= 0)
				{
					cmd.CommandText = cmd.CommandText + " and a.[ENTRYTYPE] = @entrytypeValue";
					MakeParam(cmd, "entrytypeValue", entryTypeFilter);
				}
				if (!String.IsNullOrEmpty(customerFilter))
				{
					cmd.CommandText = cmd.CommandText + " and a.LOYALTYCUSTID = @customerId";
					MakeParam(cmd, "customerId", customerFilter);
				}
				if (!String.IsNullOrEmpty(receiptID))
				{
				    string modifiedSearchString = PreProcessSearchText(receiptID, true, true);
                    cmd.CommandText += " and a.RECEIPTID like @receiptID";
					MakeParam(cmd, "receiptID", modifiedSearchString);
				}
				if (dateFrom != Date.Empty)
				{
					cmd.CommandText += " and a.CREATEDDATE >= @dateFrom";
					MakeParam(cmd, "dateFrom", dateFrom.DateTime.Date, SqlDbType.DateTime);
				}
				if (dateTo != Date.Empty)
				{
					cmd.CommandText += " and a.CREATEDDATE <= @dateTo";
					MakeParam(cmd, "dateTo", dateTo.DateTime.Date, SqlDbType.DateTime);
				}
				if (expiredateFrom != Date.Empty)
				{
					cmd.CommandText += " and a.EXPIRATIONDATE >= @expiredateFrom";
					MakeParam(cmd, "expiredateFrom", expiredateFrom.DateTime.Date, SqlDbType.DateTime);
				}
				if (expiredateTo != Date.Empty)
				{
					cmd.CommandText += " and a.EXPIRATIONDATE <= @expiredateTo";
					MakeParam(cmd, "expiredateTo", expiredateTo.DateTime.Date, SqlDbType.DateTime);
				}
				#endregion filtering

				if (rowTo > 0)
				{
					cmd.CommandText = cmd.CommandText
					+ @") as s
					 where 
					(s.ROW between " + rowFrom + " and " + rowTo + ") ";
				}

				if (rowTo <= 0)
				{
					cmd.CommandText = cmd.CommandText + @" ORDER BY " + sort;
				}

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				result = Execute<LoyaltyMSRCardTrans>(entry, cmd, CommandType.Text, PopulateLoyaltyMsrCardTrans);
			}

			return result;
		}

        public virtual List<LoyaltyMSRCardTrans> GetListForCard(IConnectionManager entry, RecordIdentifier cardId)
        {
            return GetList(
                entry
                ,""
                ,""
                ,(string)cardId
                ,""
                ,-1
                ,-1
                ,-1
                ,""
                ,""
                , Date.Empty
                , Date.Empty
                , Date.Empty
                , Date.Empty
                , -1
                , 1
                );
        }

		public virtual decimal GetMaxLineNumber(IConnectionManager entry, RecordIdentifier cardNum)
		{

			decimal? result = 0;

			using (SqlCommand cmd = new SqlCommand())
			{
				cmd.CommandText = @"SELECT 
					MAX([LINENUM]) 
					FROM [RBOLOYALTYTRANS] 
					WHERE DATAAREAID = @dataAreaId AND CARDNUMBER = @cardNum";

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "cardNum", cardNum);

				var returnValue = entry.Connection.ExecuteScalar(cmd);

				if (DBNull.Value != returnValue)
				{
					result = (decimal?)returnValue;
				}
			}

			return (result == null) ? 0 : (int)result;
		}

		private static int GetMaxSequenceNumber(IConnectionManager entry)
		{

			int? result = 0;

			using (SqlCommand cmd = new SqlCommand())
			{
				cmd.CommandText = @"SELECT 
					MAX([SEQUENCENUMBER]) 
					FROM [RBOLOYALTYTRANS] 
					WHERE DATAAREAID = @dataAreaId ";

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				var returnValue = entry.Connection.ExecuteScalar(cmd);
				if (DBNull.Value != returnValue)
				{
					result = (int?)returnValue;
				}
			}

			return (result == null) ? 0 : (int)result;
		}

		public virtual bool Exists(IConnectionManager entry, RecordIdentifier loyaltyTransID)
		{
			return RecordExists(entry, "RBOLOYALTYTRANS", new string[] { "CARDNUMBER", "LINENUM" }, loyaltyTransID);
		}

		/// <summary>
		/// Saves the loyalty transaction 
		/// </summary>
		/// <param name="entry">Entry into the database</param>
		/// <param name="loyaltyTrans">Loyalty transaction to save</param>
		public virtual void Save(IConnectionManager entry, LoyaltyMSRCardTrans loyaltyTrans)
		{

            ValidateSecurity(entry, BusinessObjects.Permission.LoyaltyRequest);

			bool isNew = false;

			SqlServerStatement statement = new SqlServerStatement("RBOLOYALTYTRANS");

			if ((loyaltyTrans.ID == null) || (loyaltyTrans.ID.IsEmpty) || (string)loyaltyTrans.ID == "")
			{
				loyaltyTrans.ID = new RecordIdentifier(loyaltyTrans.CardNumber, loyaltyTrans.LineNumber);
			}

			if (!Exists(entry, loyaltyTrans.ID))
			{

				isNew = true;

				statement.StatementType = StatementType.Insert;

				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("CARDNUMBER", loyaltyTrans.CardNumber);
				statement.AddKey("LINENUM", loyaltyTrans.LineNumber, SqlDbType.Decimal);

			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("CARDNUMBER", loyaltyTrans.CardNumber);
				statement.AddCondition("LINENUM", loyaltyTrans.LineNumber, SqlDbType.Decimal);
			}

			statement.AddField("TRANSACTIONID", (string)loyaltyTrans.TransactionID);
			statement.AddField("RECEIPTID", (string)loyaltyTrans.ReceiptID);
			statement.AddField("POINTS", loyaltyTrans.Points, SqlDbType.Decimal);
			statement.AddField("STOREID", (string)loyaltyTrans.StoreID);
			statement.AddField("TERMINALID", (string)loyaltyTrans.TerminalID);

			statement.AddField("SEQUENCENUMBER", GetMaxSequenceNumber(entry)+1, SqlDbType.Int);

			statement.AddField("LOYALTYCUSTID", (string)loyaltyTrans.CustomerID);
			statement.AddField("ENTRYTYPE", loyaltyTrans.EntryType, SqlDbType.Int);
			statement.AddField("LOYALTYSCHEMEID", (string)loyaltyTrans.SchemeID);
			statement.AddField("STAFFID", (string)loyaltyTrans.StaffID);
			statement.AddField("LOYALTYPOINTTRANSLINENUM", loyaltyTrans.LoyPointsTransLineNumber, SqlDbType.Int);

			statement.AddField("REMAININGPOINTS", loyaltyTrans.RemainingPoints, SqlDbType.Decimal);

			if (loyaltyTrans.ExpirationDate != null)
			{
				DateTime expDate = new DateTime(
				loyaltyTrans.ExpirationDate.DateTime.Year,
				loyaltyTrans.ExpirationDate.DateTime.Month,
				loyaltyTrans.ExpirationDate.DateTime.Day,
				23,
				59,
				59,
				0,
				loyaltyTrans.ExpirationDate.DateTime.Kind);
				statement.AddField("EXPIRATIONDATE", expDate, SqlDbType.DateTime);
			}
			if (loyaltyTrans.DateOfIssue != null)
			{
				statement.AddField("DATEOFISSUE", loyaltyTrans.DateOfIssue.DateTime, SqlDbType.DateTime);
			}

			statement.AddField("STATUS", (loyaltyTrans.Open == true) ? 1 : 0, SqlDbType.TinyInt);

            RecordIdentifier userID = entry.CurrentUser == null ? RecordIdentifier.Empty : entry.CurrentUser.ID;
            if (userID == RecordIdentifier.Empty)
			{
				statement.AddField("MODIFIEDBY", DBNull.Value, SqlDbType.UniqueIdentifier);
			}
			else
			{
				statement.AddField("MODIFIEDBY", (Guid)userID, SqlDbType.UniqueIdentifier);
			}

			if (isNew)
			{
				statement.AddField("CREATEDDATE", DateTime.Now, SqlDbType.DateTime);
				statement.AddField("CREATEDTIME", DateTime.Now.TimeOfDay.TotalSeconds, SqlDbType.Int);
				if (userID == RecordIdentifier.Empty)
				{
					statement.AddField("CREATEDBY", DBNull.Value, SqlDbType.UniqueIdentifier);
				}
				else
				{
					statement.AddField("CREATEDBY", (Guid)userID, SqlDbType.UniqueIdentifier);
				}
			}

			statement.AddField("TYPE", ((byte)loyaltyTrans.Type).ToString());

			entry.Connection.ExecuteStatement(statement);
		}

        public virtual LoyaltyCustomer.ErrorCodes UpdateRemainingPoints(IConnectionManager entry, RecordIdentifier CardID, RecordIdentifier LoyaltyCustID, decimal usedPoints)
		{
            ValidateSecurity(entry, BusinessObjects.Permission.LoyaltyRequest);

            LoyaltyCustomer.ErrorCodes ret = LoyaltyCustomer.ErrorCodes.NoErrors;

			using (SqlCommand cmd = new SqlCommand("spLOYALTY_UpdateRemainingPoints"))
			{
				MakeParam(cmd, "LoyaltyCustID", LoyaltyCustID);
				MakeParam(cmd, "CardNumber", CardID);
				MakeParam(cmd, "UsedPointsAmount", usedPoints, SqlDbType.Decimal);
				MakeParam(cmd, "PointsUseDate", DateTime.Now, SqlDbType.DateTime);
				MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);

				// Return value as parameter
				SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
				returnValue.Direction = ParameterDirection.ReturnValue;
				cmd.Parameters.Add(returnValue);

				entry.Connection.ExecuteNonQuery(cmd, false);

				ret = (LoyaltyCustomer.ErrorCodes)returnValue.Value;

				// ret:
				//  0 - OK
				//  1 - Loyalty card could not be found
				//  2 - Loyalty card is blocked;
				//  3 - Loyalty card is no tender card - points could not be used
				//  4 - Insert/Update error

			}

            return ret;
		}

        public virtual LoyaltyCustomer.ErrorCodes? SetExpirePoints(IConnectionManager entry, RecordIdentifier CardID, RecordIdentifier loyaltyCustID, DateTime ToDate, RecordIdentifier UserId = null)
		{
            ValidateSecurity(entry, BusinessObjects.Permission.LoyaltyRequest);

			if (UserId == null || UserId == RecordIdentifier.Empty)
			{
				UserId = entry.CurrentUser.ID;
			}

			int? ret = 0;

			using (SqlCommand cmd = new SqlCommand("spLOYALTY_ExpirePoints"))
			{
				cmd.CommandType = CommandType.StoredProcedure;

				MakeParam(cmd, "LoyaltyCustID", loyaltyCustID);
				MakeParam(cmd, "CardNumber", CardID);
				MakeParam(cmd, "ToDate", ToDate, SqlDbType.DateTime);
				MakeParam(cmd, "DataAreaID", entry.Connection.DataAreaId);
				MakeParam(cmd, "UserID", (Guid)UserId, SqlDbType.UniqueIdentifier);

				// Return value as parameter
				SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
				returnValue.Direction = ParameterDirection.ReturnValue;
				cmd.Parameters.Add(returnValue);

				entry.Connection.ExecuteNonQuery(cmd, false);

				ret = (int?)returnValue.Value;
			}

			// ret:
			// 0 - OK
			// 1 - "Loyalty card could not be found"
			// 2 - "Loyalty card is blocked";
			// 3 - "Insert/Update error";

            return (LoyaltyCustomer.ErrorCodes)ret;
		}

		public virtual LoyaltyMSRCard.TenderTypeEnum? GetLoyaltyCardType(IConnectionManager entry, RecordIdentifier cardNumber)
		{
			ValidateSecurity(entry);

			int? cardType = null;

			using (SqlCommand cmd = new SqlCommand())
			{
				cmd.CommandText = @"SELECT ISNULL(LOYALTYTENDER,-1) FROM RBOLOYALTYMSRCARDTABLE WHERE CARDNUMBER=@CardNumber AND DATAAREAID=@dataAreaId";

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "CardNumber", cardNumber);

				cardType = (int?)entry.Connection.ExecuteScalar(cmd);
			}

			if ((cardType == null || cardType == -1))
			{
				return null;
			}

			return (LoyaltyMSRCard.TenderTypeEnum?)cardType;
		}

		public virtual void GetLoyaltyPointsStatus(IConnectionManager entry, RecordIdentifier customerId, LoyaltyPointStatus pointStatus)
		{
			ValidateSecurity(entry);

			decimal? result = 0;

			pointStatus.LoyaltyTenderType = GetLoyaltyCardType(entry, pointStatus.CardNumber);

			using (SqlCommand cmd = new SqlCommand())
			{
				cmd.CommandText = @"SELECT " +
					@"ISNULL(SUM(REMAININGPOINTS),0) as [REMAININGPOINTS] " +
					@"FROM [RBOLOYALTYTRANS] " +
					@"WHERE DATAAREAID = @dataAreaId AND [STATUS]='1' AND [ENTRYTYPE]='0' ";

				if (pointStatus.LoyaltyTenderType == LoyaltyMSRCard.TenderTypeEnum.ContactTender)
				{
					cmd.CommandText += @"AND  [LOYALTYCUSTID] = @custId ";
					MakeParam(cmd, "custId", customerId);
				}
				else
				{
					cmd.CommandText += @"AND  [CARDNUMBER] = @CardNum ";
					MakeParam(cmd, "CardNum", pointStatus.CardNumber);
				}

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				result = (decimal?)entry.Connection.ExecuteScalar(cmd);
			}

			pointStatus.Points = (result == null) ? 0 : (decimal)result;
		}

        public virtual void UpdateIssuedLoyaltyPointsForCustomer(IConnectionManager entry, RecordIdentifier loyalityCardId, RecordIdentifier customerId)
        {
            // Get every loyality transaction for the card
            var loyaltyTransactions = Providers.LoyaltyMSRCardTransData.GetListForCard(entry, loyalityCardId);

            // Set the new customer Id on each of these transactions
            foreach (var transaction in loyaltyTransactions)
            {
                transaction.CustomerID = customerId;
                Providers.LoyaltyMSRCardTransData.Save(entry, transaction);
            }
        }
	}
}
