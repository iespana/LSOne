using System;
using System.Data;
using System.Drawing;
using System.IO;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.DataProviders.Companies;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Companies
{
	public class CompanyInfoData : SqlServerDataProviderBase, ICompanyInfoData
	{
		private static void PopulateCompanyInfo(IConnectionManager entry, IDataReader dr, CompanyInfo company, object includeReportFormatting)
		{
			company.ID = (int)dr["Key_"];
			company.Text = (string)dr["NAME"];
			company.CurrencyCode = (string)dr["CURRENCYCODE"];
			company.CurrencyCodeText = (string)dr["CURRENCYCODETEXT"];
			company.Phone = (string)dr["PHONE"];
			company.Fax = (string)dr["TELEFAX"];
			company.Email = (string)dr["EMAIL"];
			company.TaxNumber = (string)dr["VATNUM"];
			company.LanguageCode = (string)dr["LANGUAGECODE"];
			company.RegistrationNumber = (string)dr["COREGNUM"];

            var address = company.Address;
			address.Address1 = (string)dr["STREET"];
			address.Address2 = (string)dr["ADDRESS"];
			address.Zip = (string)dr["ZIPCODE"];
			address.City = (string)dr["CITY"];
			address.State = (string)dr["STATE"];
			address.Country = (string)dr["COUNTRYREGIONID"];
			address.AddressFormat = (dr["ADDRESSFORMAT"] == DBNull.Value) ? entry.Settings.AddressFormat : (Address.AddressFormatEnum)((int)(dr["ADDRESSFORMAT"]));

			var countryName = (string)dr["COUNTRYNAME"];

			if (dr["COMPANYLOGO"] == DBNull.Value)
			{
				company.CompanyLogo = null;
			}
			else
			{
				var blobData = new MemoryStream((Byte[])dr["COMPANYLOGO"]);
				company.CompanyLogo = Image.FromStream(blobData);
			}        

			if ((bool)includeReportFormatting)
			{
				company.AddressFormatted = entry.Settings.LocalizationContext.FormatMultipleLines(address, countryName, "\n");
			}
		}

		public virtual CompanyInfo Get(IConnectionManager entry,bool includeReportFormatting)
		{
			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				ValidateSecurity(entry);

				cmd.CommandText = @"Select ci.Key_
										, ISNULL(ci.CURRENCYCODE,'') as CURRENCYCODE
										, ISNULL(cu.TXT,'') as CURRENCYCODETEXT
										, ISNULL(ci.STREET,'') as STREET
										, ISNULL(ci.ADDRESS,'') as ADDRESS
										, ADDRESSFORMAT
										, ISNULL(ci.ZIPCODE,'') as ZIPCODE 
										, ISNULL(ci.CITY,'') as CITY
										, ISNULL(ci.STATE,'') as STATE
										, ISNULL(ci.COUNTRYREGIONID,'') as COUNTRYREGIONID 
										, ISNULL(ci.PHONE,'') as PHONE
										, ISNULL(ci.TELEFAX,'') as TELEFAX
										, ISNULL(ci.EMAIL,'') as EMAIL 
										, ISNULL(ci.NAME,'') as NAME
										, ISNULL(ci.VATNUM, '') as VATNUM
										, ISNULL(ci.LANGUAGECODE,'') as LANGUAGECODE 
										, ISNULL(cy.NAME,'') AS COUNTRYNAME 
										, ci.COMPANYLOGO 
										, ci.COREGNUM 
								from COMPANYINFO ci 
									left outer join CURRENCY cu on cu.CURRENCYCODE = ci.CURRENCYCODE and cu.DataareaID = ci.DataareaID 
									left outer join COUNTRY cy on ci.COUNTRYREGIONID = cy.COUNTRYID and ci.DATAAREAID = cy.DATAAREAID 
								where ci.DataareaID = @dataAreaId";

				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

				var results = Execute<CompanyInfo>(entry, cmd, CommandType.Text, includeReportFormatting, PopulateCompanyInfo);

				return results.Count > 0 ? results[0] : new CompanyInfo(entry.Settings.AddressFormat);
			}
		}

		public virtual bool HasCompanyCurrency(IConnectionManager entry)
		{
			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"select count(1) from COMPANYINFO ci where CURRENCYCODE is not null and  CURRENCYCODE <> ''";

				return ((int)entry.Connection.ExecuteScalar(cmd) > 0);
			}
		}

		public virtual string CompanyCurrencyCode(IConnectionManager entry)
		{
			using (IDbCommand cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					@"select ISNULL(CURRENCYCODE,'') as CURRENCYCODE from COMPANYINFO";

				object result = entry.Connection.ExecuteScalar(cmd);

				if(result is DBNull)
				{
					return "";
				}
				else
				{
					return (string)result;
				}
			}
		}

		public virtual void Delete(IConnectionManager entry, RecordIdentifier ID)
		{
			throw new NotImplementedException();
		}

		public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
		{
			return RecordExists(entry, "COMPANYINFO", "KEY_", id);
		}

		public virtual void Save(IConnectionManager entry, CompanyInfo companyInfo)
		{
			var statement = new SqlServerStatement("COMPANYINFO");

			ValidateSecurity(entry, Permission.StoreEdit);

			if (!Exists(entry, companyInfo.ID))
			{
				statement.StatementType = StatementType.Insert;

				statement.AddField("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddField("KEY_", (int)companyInfo.ID, SqlDbType.Int);
			}
			else
			{
				statement.StatementType = StatementType.Update;

				statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
				statement.AddCondition("KEY_", (int)companyInfo.ID, SqlDbType.Int);
			}

			statement.AddField("CURRENCYCODE", (string)companyInfo.CurrencyCode);
			statement.AddField("STREET", companyInfo.Address.Address1);
			statement.AddField("ADDRESS", companyInfo.Address.Address2);
			statement.AddField("ADDRESSFORMAT", (int)companyInfo.Address.AddressFormat, SqlDbType.Int);
			statement.AddField("ZIPCODE", companyInfo.Address.Zip);
			statement.AddField("CITY", companyInfo.Address.City);
			statement.AddField("STATE", companyInfo.Address.State);
			statement.AddField("COUNTRYREGIONID", (string)companyInfo.Address.Country);
			statement.AddField("PHONE", companyInfo.Phone);
			statement.AddField("EMAIL", companyInfo.Email);
			statement.AddField("TELEFAX", companyInfo.Fax);
			statement.AddField("NAME", companyInfo.Text);
			statement.AddField("VATNUM", companyInfo.TaxNumber);
			statement.AddField("LANGUAGECODE", companyInfo.LanguageCode);
			statement.AddField("COREGNUM", companyInfo.RegistrationNumber);

            if (companyInfo.CompanyLogo != null)
			{
				using (var ms = new MemoryStream())
				{
					companyInfo.CompanyLogo.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					var data = ms.ToArray();
					statement.AddField("COMPANYLOGO", data, SqlDbType.VarBinary);
				}
			}
			else if (companyInfo.CompanyLogo == null)
			{
				statement.AddField("COMPANYLOGO", DBNull.Value, SqlDbType.VarBinary);
			}
			
			entry.Connection.ExecuteStatement(statement); 
		}
	}
}
