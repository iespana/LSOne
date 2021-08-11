using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.Cryptography;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    public class SiteServiceProfileData : SqlServerDataProviderBase, ISiteServiceProfileData
	{
		private static string ProfileIsUsedColumn =
			@"CAST(CASE WHEN EXISTS (SELECT 1 FROM RBOTERMINALTABLE TE WHERE TE.TRANSACTIONSERVICEPROFILE = P.PROFILEID OR TE.HOSPTRANSSTORESERVERPROFILE = P.PROFILEID)
						  OR EXISTS (SELECT 1 FROM RBOSTORETABLE ST WHERE ST.TRANSACTIONSERVICEPROFILE = P.PROFILEID)
						  OR EXISTS (SELECT 1 FROM RBOPARAMETERS PRM WHERE PRM.SITESERVICEPROFILE = P.PROFILEID)
						THEN 1
						ELSE 0
				   END AS BIT)";

		private static List<TableColumn> SiteServiceProfileColumns = new List<TableColumn>()
		{
			new TableColumn { ColumnName = "PROFILEID", TableAlias = "P", ColumnAlias = "PROFILEID" },
			new TableColumn { ColumnName = "NAME", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "NAME" },
			new TableColumn { ColumnName = "AOSINSTANCE", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "AOSINSTANCE" },
			new TableColumn { ColumnName = "AOSSERVER", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "AOSSERVER" },
			new TableColumn { ColumnName = "AOSPORT", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "AOSPORT" },
			new TableColumn { ColumnName = "CENTRALTABLESERVER", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "CENTRALTABLESERVER" },
			new TableColumn { ColumnName = "CENTRALTABLESERVERPORT", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CENTRALTABLESERVERPORT" },
			new TableColumn { ColumnName = "USERNAME", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "USERNAME" },
			new TableColumn { ColumnName = "PASSWORD", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "PASSWORD" },
			new TableColumn { ColumnName = "COMPANY", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "COMPANY" },
			new TableColumn { ColumnName = "DOMAIN", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "DOMAIN" },
			new TableColumn { ColumnName = "AXVERSION", TableAlias = "P", IsNull = true, NullValue = "-1", ColumnAlias = "AXVERSION" },
			new TableColumn { ColumnName = "CONFIGURATION", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "CONFIGURATION" },
			new TableColumn { ColumnName = "LANGUAGE", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "LANGUAGE" },
			new TableColumn { ColumnName = "TSCUSTOMER", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "TSCUSTOMER" },
			new TableColumn { ColumnName = "TSSTAFF", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "TSSTAFF" },
			new TableColumn { ColumnName = "TSINVENTORYLOOKUP", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "TSINVENTORYLOOKUP" },
			new TableColumn { ColumnName = "ISSUEGIFTCARDOPTION", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "ISSUEGIFTCARDOPTION" },
			new TableColumn { ColumnName = "USEGIFTCARDS", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "USEGIFTCARDS" },
			new TableColumn { ColumnName = "USECENTRALSUSPENSION", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "USECENTRALSUSPENSION" },
			new TableColumn { ColumnName = "USERCONFIRMATION", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "USERCONFIRMATION" },
			new TableColumn { ColumnName = "USECREDITVOUCHERS", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "USECREDITVOUCHERS" },
			new TableColumn { ColumnName = "CUSTOMERADDDEFAULTTAXGROUPNAME", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "CUSTOMERADDDEFAULTTAXGROUPNAME" },
			new TableColumn { ColumnName = "CUSTOMERADDDEFAULTTAXGROUP", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "CUSTOMERADDDEFAULTTAXGROUP" },
			new TableColumn { ColumnName = "CASHCUSTOMERSETTING", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CASHCUSTOMERSETTING" },
			new TableColumn { ColumnName = "GIFTCARDREFILLSETTING", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "GIFTCARDREFILLSETTING" },
			new TableColumn { ColumnName = "MAXIMUMGIFTCARDAMOUNT", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "MAXIMUMGIFTCARDAMOUNT" },			
			new TableColumn { ColumnName = "CENTRALRETURNLOOKUP", TableAlias = "P", IsNull = true, NullValue = "1", ColumnAlias = "CENTRALRETURNLOOKUP" },
			new TableColumn { ColumnName = "USESERIALNUMBERS", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "USESERIALNUMBERS" },
			new TableColumn { ColumnName = "SENDRECEIPTEMAIL", TableAlias = "P", IsNull = true, NullValue = "3", ColumnAlias = "SENDRECEIPTEMAIL" },
			new TableColumn { ColumnName = "EMAILWINDOWSPRINTERCONFIGURATIONID", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "EMAILWINDOWSPRINTERCONFIGURATIONID" },
            new TableColumn { ColumnName = "ALLOWCUSTOMERMANUALID", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "ALLOWCUSTOMERMANUALID" },
			new TableColumn { ColumnName = "CUSTOMERDEFAULTCREDITLIMIT", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMERDEFAULTCREDITLIMIT" },
			new TableColumn { ColumnName = "CUSTOMERNAMEMANDATORY", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMERNAMEMANDATORY" },
			new TableColumn { ColumnName = "CUSTOMERSEARCHALIASMANDATORY", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMERSEARCHALIASMANDATORY" },
			new TableColumn { ColumnName = "CUSTOMERADDRESSMANDATORY", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMERADDRESSMANDATORY" },
			new TableColumn { ColumnName = "CUSTOMERPHONEMANDATORY", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMERPHONEMANDATORY" },
			new TableColumn { ColumnName = "CUSTOMEREMAILMANDATORY", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMEREMAILMANDATORY" },
			new TableColumn { ColumnName = "CUSTOMERRECEIPTEMAILMANDATORY", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMERRECEIPTEMAILMANDATORY" },
			new TableColumn { ColumnName = "CUSTOMERGENDERMANDATORY", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMERGENDERMANDATORY" },
			new TableColumn { ColumnName = "CUSTOMERBIRTHDATEMANDATORY", TableAlias = "P", IsNull = true, NullValue = "0", ColumnAlias = "CUSTOMERBIRTHDATEMANDATORY" },
			new TableColumn { ColumnName = "IFAUTHTOKEN", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "IFAUTHTOKEN" },
			new TableColumn { ColumnName = "IFTCPPORT", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "IFTCPPORT" },
			new TableColumn { ColumnName = "IFHTTPPORT", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "IFHTTPPORT" },
			new TableColumn { ColumnName = "IFPROTOCOLS", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "IFPROTOCOLS" },
			new TableColumn { ColumnName = "IFSSLTHUMBNAIL", TableAlias = "P", IsNull = true, NullValue = "''", ColumnAlias = "IFSSLTHUMBNAIL" },
            new TableColumn { ColumnName = ProfileIsUsedColumn, ColumnAlias = "PROFILEISUSED" },
            new TableColumn { ColumnName = "TIMEOUT", TableAlias = "P" },
            new TableColumn { ColumnName = "MAXMESSAGESIZE", TableAlias = "P" },
		};

		public virtual List<DataEntity> GetList(IConnectionManager entry)
		{
			return GetList(entry, "NAME");
		}

		public virtual List<SiteServiceProfile> GetSelectList(IConnectionManager entry)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				Condition condition = new Condition { Operator = "AND", ConditionValue = "P.DATAAREAID = @dataAreaId" };
				
				cmd.CommandText = string.Format(
								   QueryTemplates.BaseQuery("POSTRANSACTIONSERVICEPROFILE", "P"),
								   QueryPartGenerator.InternalColumnGenerator(SiteServiceProfileColumns),
								   string.Empty,
								   QueryPartGenerator.ConditionGenerator(condition),
								   string.Empty);
				
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				return Execute<SiteServiceProfile>(entry, cmd, CommandType.Text, PopulateProfile);
			}
		}

		public virtual List<DataEntity> GetList(IConnectionManager entry, string sort)
		{
			return GetList<DataEntity>(entry, "POSTRANSACTIONSERVICEPROFILE", "NAME", "ProfileID", sort);
		}

		public virtual List<SiteServiceProfile> GetSiteServiceProfileList(IConnectionManager entry, string sort)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText =
				@"SELECT PROFILEID, 
						 ISNULL(NAME,'') as NAME,
						 " + ProfileIsUsedColumn + @" AS PROFILEISUSED
					   FROM POSTRANSACTIONSERVICEPROFILE P
					   WHERE DATAAREAID = @dataAreaId
					   ORDER BY " + sort;

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				return Execute<SiteServiceProfile>(entry, cmd, CommandType.Text, PopulateSiteServiceProfileList);
			}
		}

		private static void PopulateSiteServiceProfileList(IDataReader dr, SiteServiceProfile profile)
		{
			profile.ID = (string)dr["PROFILEID"];
			profile.Text = (string)dr["NAME"];
			profile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
		}

		private static void PopulateProfile(IDataReader dr, SiteServiceProfile profile)
		{
			profile.ID = (string)dr["PROFILEID"];
			profile.Text = (string)dr["NAME"];
			profile.AosInstance = (string)dr["AOSINSTANCE"];
			profile.AosServer = (string)dr["AOSSERVER"];
			if (profile.AosServer !=  string.Empty)
			{
				profile.UseAxTransactionServices = true;
			}
            profile.AosPort = TryParse((string)dr["AOSPORT"]) ?? 0;

            profile.SiteServiceAddress = (string)dr["CENTRALTABLESERVER"];
            profile.SiteServicePortNumber = TryParse((string)dr["CENTRALTABLESERVERPORT"]) ?? 0;
            profile.Timeout = (int)dr["TIMEOUT"];
            profile.MaxMessageSize = (int)dr["MAXMESSAGESIZE"];

            profile.UserName = (string)dr["USERNAME"];
			profile.Password = (string)dr["PASSWORD"];
			profile.Company = (string)dr["COMPANY"];
			profile.Domain = (string)dr["DOMAIN"];
			profile.AxVersion = (int)dr["AXVERSION"];
			profile.Configuration = (string)dr["CONFIGURATION"];
			profile.Language = (string)dr["LANGUAGE"];
			profile.CheckCustomer = ((byte)dr["TSCUSTOMER"] != 0);
			profile.CheckStaff = ((byte)dr["TSSTAFF"] != 0);
			profile.UseInventoryLookup = ((byte)dr["TSINVENTORYLOOKUP"] != 0);
			profile.IssueGiftCardOption = (SiteServiceProfile.IssueGiftCardOptionEnum)((int)dr["ISSUEGIFTCARDOPTION"]);
			profile.UseGiftCards = ((byte)dr["USEGIFTCARDS"] != 0);
			profile.UseCentralSuspensions = ((byte)dr["USECENTRALSUSPENSION"] != 0);
			profile.UserConfirmation = ((byte)dr["USERCONFIRMATION"] != 0);
			profile.UseCreditVouchers = ((byte)dr["USECREDITVOUCHERS"] != 0);
			profile.NewCustomerDefaultTaxGroup = (string)dr["CUSTOMERADDDEFAULTTAXGROUP"];
			profile.NewCustomerDefaultTaxGroupName = (string)dr["CUSTOMERADDDEFAULTTAXGROUPNAME"];
			profile.CashCustomerSetting = (SiteServiceProfile.CashCustomerSettingEnum)dr["CASHCUSTOMERSETTING"];
			profile.GiftCardRefillSetting = (SiteServiceProfile.GiftCardRefillSettingEnum) dr["GIFTCARDREFILLSETTING"];
			profile.MaximumGiftCardAmount = (decimal) dr["MAXIMUMGIFTCARDAMOUNT"];			
			profile.UseCentralReturns = ((byte)dr["CENTRALRETURNLOOKUP"] != 0);
			profile.UseSerialNumbers = ((bool)dr["USESERIALNUMBERS"]);
			profile.SendReceiptEmails = (ReceiptEmailOptionsEnum)(int)dr["SENDRECEIPTEMAIL"];
			profile.EmailWindowsPrinterConfigurationID = (string)dr["EMAILWINDOWSPRINTERCONFIGURATIONID"];
            profile.ProfileIsUsed = (bool)dr["PROFILEISUSED"];
			profile.AllowCustomerManualID = (bool)dr["ALLOWCUSTOMERMANUALID"];
			profile.DefaultCreditLimit = (decimal)dr["CUSTOMERDEFAULTCREDITLIMIT"];
			profile.CustomerNameIsMandatory = (bool)dr["CUSTOMERNAMEMANDATORY"];
			profile.CustomerSearchAliasIsMandatory = (bool)dr["CUSTOMERSEARCHALIASMANDATORY"];
			profile.CustomerAddressIsMandatory = (bool)dr["CUSTOMERADDRESSMANDATORY"];
			profile.CustomerPhoneIsMandatory = (bool)dr["CUSTOMERPHONEMANDATORY"];
			profile.CustomerEmailIsMandatory = (bool)dr["CUSTOMEREMAILMANDATORY"];
			profile.CustomerReceiptEmailIsMandatory = (bool)dr["CUSTOMERRECEIPTEMAILMANDATORY"];
			profile.CustomerGenderIsMandatory = (bool)dr["CUSTOMERGENDERMANDATORY"];
			profile.CustomerBirthDateIsMandatory = (bool)dr["CUSTOMERBIRTHDATEMANDATORY"];

			profile.IFAuthToken = (string)dr["IFAUTHTOKEN"];
			profile.IFTcpPort = (string)dr["IFTCPPORT"];
			profile.IFHttpPort = (string)dr["IFHTTPPORT"];
			profile.IFProtocols = (string)dr["IFPROTOCOLS"];
			string sslThumbnail = dr["IFSSLTHUMBNAIL"] as string;
			profile.IFSSLCertificateThumbnail = SecureStringHelper.FromString(string.IsNullOrEmpty(sslThumbnail) 
																				? "" 
																				: Cipher.Decrypt(sslThumbnail, SSLPrivateKey));

			profile.KDSWebServiceUrl = GetKDSWebServiceUrl(profile.SiteServiceAddress);
		}

		public virtual SiteServiceProfile Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone)
		{
			ValidateSecurity(entry);
			using (var cmd = entry.Connection.CreateCommand())
			{
				Condition condition = new Condition { Operator = "AND", ConditionValue = "P.DATAAREAID = @dataAreaId AND P.PROFILEID = @id" };

				cmd.CommandText = string.Format(
				   QueryTemplates.BaseQuery("POSTRANSACTIONSERVICEPROFILE", "P"),
				   QueryPartGenerator.InternalColumnGenerator(SiteServiceProfileColumns),
				   string.Empty,
				   QueryPartGenerator.ConditionGenerator(condition),
				   string.Empty);

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "id", (string)id);

				return Get<SiteServiceProfile>(entry, cmd, id, PopulateProfile, cacheType, UsageIntentEnum.Normal);
			}
		}

		/// <summary>
		/// Retrieves the StoreServer address and port for a given terminal and populates the ref values given.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="terminalId">The id of the terminal to get the store server profile for</param>
		/// <param name="storeId">The ID of the store to get the store server profile for</param>
		/// <param name="storeServerAddress">The address of the store server</param>
		/// <param name="storeServerPort">The port for the store server</param>
		public virtual void GetStoreServerAddressAndPort(IConnectionManager entry, string terminalId, string storeId, ref string storeServerAddress, ref ushort storeServerPort)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					"select P.CENTRALTABLESERVER, P.CENTRALTABLESERVERPORT " +
					"from POSTRANSACTIONSERVICEPROFILE P " +
					"join RBOTERMINALTABLE R on R.TRANSACTIONSERVICEPROFILE = P.PROFILEID and R.DATAAREAID = P.DATAAREAID and R.TERMINALID = @terminalId and R.STOREID = @storeId" +
					"and P.DATAAREAID = @dataAreaId";

				MakeParam(cmd, "terminalId", terminalId);
				MakeParam(cmd, "storeId", storeId);
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				IDataReader dr = null;
				try
				{
					dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

					if (dr.Read())
					{
						storeServerAddress = (string)dr["CENTRALTABLESERVER"];
						storeServerPort = Convert.ToUInt16((string)dr["CENTRALTABLESERVERPORT"]);
					}
				}
				finally
				{
					if (dr != null)
					{
						dr.Close();
						dr.Dispose();
					}
				}
			}
		}

		/// <summary>
		/// Retrieves the gift card option for the given terminal
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="terminalId">The ID for the terminal</param>
		/// <param name="storeId">The ID for the store</param>
		/// <returns>The issue gift card option for the given terminal</returns>
		public virtual SiteServiceProfile.IssueGiftCardOptionEnum GetTerminalGiftCardOption(IConnectionManager entry, RecordIdentifier terminalId, RecordIdentifier storeId)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					"select ISNULL(P.ISSUEGIFTCARDOPTION, 0) as ISSUEGIFTCARDOPTION " +
					"from POSTRANSACTIONSERVICEPROFILE P " +
					"join RBOTERMINALTABLE R on R.TRANSACTIONSERVICEPROFILE = P.PROFILEID and R.DATAAREAID = P.DATAAREAID and R.TERMINALID = @terminalId and R.STOREID = @storeId " +
					"and P.DATAAREAID = @dataAreaId";

				MakeParam(cmd, "terminalId", terminalId);
				MakeParam(cmd, "storeId", storeId);
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				return (SiteServiceProfile.IssueGiftCardOptionEnum)((int)entry.Connection.ExecuteScalar(cmd));
			}
		}

		/// <summary>
		/// Retrieves the Store server profile for the given terminal
		/// </summary>
		/// <param name="entry">The entry into the database</param>
		/// <param name="terminalId">The ID of the terminal to get the profile for</param>
		/// <param name="storeId">The ID of the terminal to get the profile for</param>
		/// <param name="cacheType">The type of cache to be used</param>
		/// <returns>The store server profile for the given terminal</returns>
		public virtual SiteServiceProfile GetTerminalProfile(IConnectionManager entry, RecordIdentifier terminalId, RecordIdentifier storeId, CacheType cacheType = CacheType.CacheTypeNone)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				Condition condition = new Condition { Operator = "AND", ConditionValue = "P.DATAAREAID = @dataAreaId" };
				List<Join> joins = new List<Join>
				{
					new Join
					{
						Table = "RBOTERMINALTABLE",
						TableAlias = "R",
						Condition = "R.TRANSACTIONSERVICEPROFILE = P.PROFILEID AND R.DATAAREAID = P.DATAAREAID AND R.TERMINALID = @terminalId AND R.STOREID = @storeId"
					}
				};

				cmd.CommandText = string.Format(
				   QueryTemplates.BaseQuery("POSTRANSACTIONSERVICEPROFILE", "P"),
				   QueryPartGenerator.InternalColumnGenerator(SiteServiceProfileColumns),
				   QueryPartGenerator.JoinGenerator(joins),
				   QueryPartGenerator.ConditionGenerator(condition),
				   string.Empty);

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "terminalId", (string)terminalId);
				MakeParam(cmd, "storeId", (string)storeId);

				return Get<SiteServiceProfile>(entry, cmd, terminalId, PopulateProfile, cacheType, UsageIntentEnum.Normal);
			}
		}

		public virtual SiteServiceProfile GetStoreProfile(IConnectionManager entry, RecordIdentifier storeId, CacheType cacheType = CacheType.CacheTypeNone)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				Condition condition = new Condition { Operator = "AND", ConditionValue = "P.DATAAREAID = @dataAreaId" };
				List<Join> joins = new List<Join>
				{
					new Join
					{
						Table = "RBOSTORETABLE",
						TableAlias = "R",
						Condition = "R.TRANSACTIONSERVICEPROFILE = P.PROFILEID AND R.DATAAREAID = P.DATAAREAID AND R.STOREID = @storeId"
					}
				};

				cmd.CommandText = string.Format(
				   QueryTemplates.BaseQuery("POSTRANSACTIONSERVICEPROFILE", "P"),
				   QueryPartGenerator.InternalColumnGenerator(SiteServiceProfileColumns),
				   QueryPartGenerator.JoinGenerator(joins),
				   QueryPartGenerator.ConditionGenerator(condition),
				   string.Empty);

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
				MakeParam(cmd, "storeId", (string)storeId);

				return Get<SiteServiceProfile>(entry, cmd, storeId, PopulateProfile, cacheType, UsageIntentEnum.Normal);
			}
		}

		public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
		{
			return RecordExists<SiteServiceProfile>(entry, "POSTRANSACTIONSERVICEPROFILE", "PROFILEID", id);
		}

		public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
		{
			DeleteRecord<SiteServiceProfile>(entry, "POSTRANSACTIONSERVICEPROFILE", "PROFILEID", id, Permission.TransactionServiceProfileEdit);
		}

		public virtual void Save(IConnectionManager entry, SiteServiceProfile profile)
		{
			var statement = new SqlServerStatement("POSTRANSACTIONSERVICEPROFILE");

			ValidateSecurity(entry, Permission.VisualProfileEdit);

			profile.Validate();

			bool isNew = false;
			if (profile.ID == RecordIdentifier.Empty)
			{
				isNew = true;
				profile.ID = DataProviderFactory.Instance.GenerateNumber<ISiteServiceProfileData, SiteServiceProfile>(entry); 
			}

			if (isNew || !Exists(entry, profile.ID))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddKey("PROFILEID", (string)profile.ID);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("PROFILEID", (string)profile.ID);
			}

			statement.AddField("NAME", profile.Text);
			statement.AddField("AOSINSTANCE", profile.AosInstance);
			statement.AddField("AOSSERVER", profile.AosServer);
			statement.AddField("AOSPORT", profile.AosPort.ToString());
			statement.AddField("CENTRALTABLESERVER", profile.SiteServiceAddress);
			statement.AddField("CENTRALTABLESERVERPORT", profile.SiteServicePortNumber.ToString());
            statement.AddField("TIMEOUT", profile.Timeout.ToString());
            statement.AddField("MAXMESSAGESIZE", profile.MaxMessageSize.ToString());
            statement.AddField("USERNAME", profile.UserName);
			statement.AddField("PASSWORD", profile.Password);
			statement.AddField("COMPANY", profile.Company);
			statement.AddField("DOMAIN", profile.Domain);
			statement.AddField("AXVERSION", profile.AxVersion, SqlDbType.Int);
			statement.AddField("CONFIGURATION", profile.Configuration);
			statement.AddField("LANGUAGE", profile.Language);
			statement.AddField("TSCUSTOMER", profile.CheckCustomer ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("TSSTAFF", profile.CheckStaff ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("TSINVENTORYLOOKUP", profile.UseInventoryLookup ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("ISSUEGIFTCARDOPTION", (int)profile.IssueGiftCardOption, SqlDbType.Int);
			statement.AddField("USEGIFTCARDS", profile.UseGiftCards ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("USECENTRALSUSPENSION", profile.UseCentralSuspensions ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("USERCONFIRMATION", profile.UserConfirmation ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("USECREDITVOUCHERS", profile.UseCreditVouchers ? 1 : 0, SqlDbType.TinyInt);
			statement.AddField("CUSTOMERADDDEFAULTTAXGROUP", (string)profile.NewCustomerDefaultTaxGroup);
			statement.AddField("CUSTOMERADDDEFAULTTAXGROUPNAME", profile.NewCustomerDefaultTaxGroupName);
			statement.AddField("CASHCUSTOMERSETTING", (int)profile.CashCustomerSetting, SqlDbType.Int);
			statement.AddField("GIFTCARDREFILLSETTING", (int)profile.GiftCardRefillSetting, SqlDbType.Int);
			statement.AddField("MAXIMUMGIFTCARDAMOUNT", profile.MaximumGiftCardAmount, SqlDbType.Decimal);			
			statement.AddField("CENTRALRETURNLOOKUP", profile.UseCentralReturns ? 1 : 0, SqlDbType.Int);
			statement.AddField("USESERIALNUMBERS", profile.UseSerialNumbers, SqlDbType.Bit);
			statement.AddField("SENDRECEIPTEMAIL", (int)profile.SendReceiptEmails, SqlDbType.Int);
			statement.AddField("EMAILWINDOWSPRINTERCONFIGURATIONID", (string)profile.EmailWindowsPrinterConfigurationID);
            statement.AddField("ALLOWCUSTOMERMANUALID", profile.AllowCustomerManualID, SqlDbType.Bit);
			statement.AddField("CUSTOMERDEFAULTCREDITLIMIT", profile.DefaultCreditLimit, SqlDbType.Decimal);
			statement.AddField("CUSTOMERNAMEMANDATORY", profile.CustomerNameIsMandatory, SqlDbType.Bit);
			statement.AddField("CUSTOMERSEARCHALIASMANDATORY", profile.CustomerSearchAliasIsMandatory, SqlDbType.Bit);
			statement.AddField("CUSTOMERADDRESSMANDATORY", profile.CustomerAddressIsMandatory, SqlDbType.Bit);
			statement.AddField("CUSTOMERPHONEMANDATORY", profile.CustomerPhoneIsMandatory, SqlDbType.Bit);
			statement.AddField("CUSTOMEREMAILMANDATORY", profile.CustomerEmailIsMandatory, SqlDbType.Bit);
			statement.AddField("CUSTOMERRECEIPTEMAILMANDATORY", profile.CustomerReceiptEmailIsMandatory, SqlDbType.Bit);
			statement.AddField("CUSTOMERGENDERMANDATORY", profile.CustomerGenderIsMandatory, SqlDbType.Bit);
			statement.AddField("CUSTOMERBIRTHDATEMANDATORY", profile.CustomerBirthDateIsMandatory, SqlDbType.Bit);
			statement.AddField("IFTCPPORT", profile.IFTcpPort);
			statement.AddField("IFHTTPPORT", profile.IFHttpPort);
			statement.AddField("IFPROTOCOLS", profile.IFProtocols);
			Save(entry, profile, statement);
		}

		public virtual void SaveSSLThumbnail(IConnectionManager entry, SiteServiceProfile profile)
		{
			var statement = new SqlServerStatement("POSTRANSACTIONSERVICEPROFILE");

			ValidateSecurity(entry, Permission.VisualProfileEdit);

			profile.Validate();
			statement.StatementType = StatementType.Update;

			statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
			statement.AddCondition("PROFILEID", (string)profile.ID);
			statement.AddField("IFSSLTHUMBNAIL", Cipher.Encrypt(SecureStringHelper.ToString(profile.IFSSLCertificateThumbnail), SSLPrivateKey));
			Save(entry, profile, statement);
		}

		private static string SSLPrivateKey
		{
			get
			{
				return "1NXB5RQLVQS6WD8SGRMQBU43U8HW3OAG27DG17LH4SQCHWSUZTM30DQH3AZ7VO5M";
			}
		}

        private static int? TryParse(string input)
        {
            if (!int.TryParse(input, out int result))
            {
                return null;
            }

            return result;
        }

		private static string GetKDSWebServiceUrl(string hostName)
		{
			Uri httpAddress = new UriBuilder(Uri.UriSchemeHttp, hostName, 9110, KDSLSOneWebServiceConstants.EndpointName).Uri;
			return httpAddress.ToString();
		}

		#region ISequenceable Members

		public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
		{
			return Exists(entry, id);
		}

		public RecordIdentifier SequenceID
		{
			get { return "TRANSACTIONPRF"; }
		}

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSTRANSACTIONSERVICEPROFILE", "PROFILEID", sequenceFormat, startingRecord, numberOfRecords);
        }

		#endregion
	}
}