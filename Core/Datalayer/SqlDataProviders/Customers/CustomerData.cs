using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Tools;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.BusinessObjects.Companies;

namespace LSOne.DataLayer.SqlDataProviders.Customers
{
    /// <summary>
    /// Data provider class for customer data entities
    /// </summary>
    public class CustomerData : SqlServerDataProviderBase, ICustomerData
    {
        public const string CustomColumns = "<CUSTOMCOLUMNS>";
        public const string CustomConditions = "<CUSTOMCONDITIONS>";
        protected SalesTaxGroup taxExemptTaxGroup;

        protected virtual string BaseListSql
        {
            get
            {
                return
                    "Select C.ACCOUNTNUM, " +
                    "C.MASTERID, " +
                    "ISNULL(C.NAME,'') as NAME," +
                    "ISNULL(C.INVOICEACCOUNT,'') as INVOICEACCOUNT, " +
                    "<ROW> " +
                    "ISNULL(C.PRICEGROUP, '') as PRICEGROUP," +
                    "ISNULL(C.LINEDISC, '') as LINEDISC," +
                    "ISNULL(C.ENDDISC, '') as ENDDISC," +
                    "ISNULL(C.TAXGROUP, '') as TAXGROUP," +
                    "ISNULL(C.BLOCKED, 0) as BLOCKED," +
                    "ISNULL(C.CREDITMAX, 0) as CREDITMAX," +
                    "ISNULL(C.NONCHARGABLEACCOUNT, 0) as NONCHARGABLEACCOUNT," +
                    "ISNULL(C.FIRSTNAME,'') as FIRSTNAME," +
                    "ISNULL(C.MIDDLENAME,'') as MIDDLENAME," +
                    "ISNULL(C.LASTNAME,'') as LASTNAME, " +
                    "ISNULL(A.ADDRESS,'') as ADDRESS," +
                    "ISNULL(A.STREET,'') as STREET," +
                    "ISNULL(A.CITY,'') as CITY, " +
                    "ISNULL(A.ZIPCODE,'') as ZIPCODE," +
                    "ISNULL(A.STATE,'') as STATE," +
                    "ISNULL(A.COUNTRY,'') as COUNTRY, " +
                    "ISNULL(A.ADDRESSFORMAT, 0) as ADDRESSFORMAT," +
                    "ISNULL(C.NAMEPREFIX,'') as NAMEPREFIX," +
                    "ISNULL(C.NAMESUFFIX,'') as NAMESUFFIX," +
                    "ISNULL(C.ORGID,'') as ORGID, " +
                    "ISNULL(C.NAMEALIAS, '') as NAMEALIAS, " +
                    "C.LOCALLYSAVED, " +
                    "C.DELETED, " +
                    "ISNULL(C.TAXEXEMPT, 0) AS TAXEXEMPT " +
                    "<ADDITIONALCOLUMNS>" +
                    CustomColumns +
                    " FROM CUSTOMER C " +
                    "<JOINS>" +
                    "WHERE C.DATAAREAID = @dataAreaId " +
                    "<WHERE>" +
                    CustomConditions;
            }
        }

        protected virtual string BaseSql
        {
            get
            {
                return BaseListSql
                    .Replace("<ROW>", "")
                    .Replace("<ADDITIONALCOLUMNS>",
                             ", ISNULL(c.CURRENCY,'') as CURRENCY," +
                             "ISNULL(c.LANGUAGEID,'') as LANGUAGEID," +
                             "ISNULL(C.INVOICEACCOUNT,'') as INVOICEACCOUNT, " +
                             "ISNULL(c.TAXGROUP,'') as TAXGROUP," +
                             "ISNULL(TG.TAXGROUPNAME,'') as TAXGROUPNAME," +
                             "ISNULL(c.PRICEGROUP,'') as PRICEGROUP," +
                             "ISNULL(c.LINEDISC,'') as LINEDISC," +
                             "ISNULL(c.MULTILINEDISC,'') as MULTILINEDISC," +
                             "ISNULL(c.ENDDISC,'') as ENDDISC," +
                             "ISNULL(c.CREDITMAX,0) as CREDITMAX," +
                             "ISNULL(c.BLOCKED,0) as BLOCKED," +
                             "ISNULL(c.NONCHARGABLEACCOUNT,0) as NONCHARGABLEACCOUNT," +
                             "ISNULL(c.CUSTGROUP,'') as CUSTGROUP," +
                             "ISNULL(c.PHONE,'') as PHONE," +
                             "ISNULL(c.CELLULARPHONE,'') as CELLULARPHONE," +
                             "ISNULL(c.EMAIL,'') as EMAIL," +
                             "ISNULL(c.VATNUM,'') as VATNUM, " +
                             "ISNULL(c.INCLTAX,0) as INCLTAX," +
                             "ISNULL(c.RECEIPTOPTION,0) as RECEIPTOPTION," +
                             "ISNULL(c.RECEIPTEMAIL,'') as RECEIPTEMAIL," +
                             "ISNULL(C.GENDER, 0) AS GENDER, " +
                             "ISNULL(C.DATEOFBIRTH, '1900-01-01') AS DATEOFBIRTH, " +
                             "ISNULL(c.URL,'') as URL, " +
                             "ISNULL(c.TAXOFFICE,'') as TAXOFFICE," +
                             "ISNULL(c.USEPURCHREQUEST,0) as USEPURCHREQUEST," +
                             "C.DELETED, " +
                             "ISNULL(c.TAXEXEMPT, 0) as TAXEXEMPT " +
                             "<ADDITIONALCOLUMNS>")
                    .Replace("<JOINS>",
                             "left outer join TAXGROUPHEADING TG on c.TAXGROUP = TG.TAXGROUP and c.DATAAREAID = TG.DATAAREAID " +
                             "<JOINS>");
            }
        }

        protected virtual string BaseSqlWithAddressOverRows
        {
            get
            {
                return
                    "SELECT s.* from(" +
                    BaseListSql
                        .Replace("<ROW>", "ROW_NUMBER() OVER(order by <ORDER>) AS ROW, ")
                        .Replace("<ADDITIONALCOLUMNS>", ", ISNULL(IC.NAME, '') AS INVOICEACCOUNTNAME ")
                        .Replace("<JOINS>", "left outer join CUSTOMERADDRESS A on C.ACCOUNTNUM=A.ACCOUNTNUM AND A.ADDRESSTYPE=1 AND A.ISDEFAULT=1 left join CUSTOMER IC on IC.ACCOUNTNUM = C.INVOICEACCOUNT ") +
                    ") s ";
            }
        }

        protected virtual string BaseSqlWithAddress
        {
            get
            {
                return BaseListSql
                    .Replace("<ROW>", "")
                    .Replace("<ADDITIONALCOLUMNS>", ", '' AS INVOICEACCOUNTNAME ")
                    .Replace("<JOINS>", "left outer join CUSTOMERADDRESS A on C.ACCOUNTNUM=A.ACCOUNTNUM AND A.ADDRESSTYPE=1 AND A.ISDEFAULT=1");
            }
        }

        protected virtual string ResolveSort(CustomerSorting sort, bool backwards)
        {
            if (backwards)
            {
                switch (sort)
                {
                    case CustomerSorting.ID:
                        return "Len(C.ACCOUNTNUM) DESC,C.ACCOUNTNUM DESC";

                    case CustomerSorting.Name:
                        return "C.NAME DESC";

                    case CustomerSorting.InvoiceAccount:
                        return "C.INVOICEACCOUNT DESC";

                    case CustomerSorting.Address:
                        return "A.STREET ASC,A.ADDRESS ASC,A.ZIPCODE ASC,A.CITY ASC,A.STATE ASC,A.COUNTRY ASC";

                    case CustomerSorting.FirstName:
                        return "C.FIRSTNAME DESC";


                    case CustomerSorting.LastName:
                        return "C.LASTNAME DESC";
                }
            }
            else
            {
                switch (sort)
                {
                    case CustomerSorting.ID:
                        return "Len(C.ACCOUNTNUM),C.ACCOUNTNUM";

                    case CustomerSorting.Name:
                        return "C.NAME";

                    case CustomerSorting.InvoiceAccount:
                        return "C.INVOICEACCOUNT";

                    case CustomerSorting.Address:
                        return "A.STREET DESC,A.ADDRESS DESC,A.ZIPCODE DESC,A.CITY DESC,A.STATE DESC,A.COUNTRY ";

                    case CustomerSorting.FirstName:
                        return "C.FIRSTNAME ASC";

                    case CustomerSorting.LastName:
                        return "C.LASTNAME ASC";
                }
            }

            return "";
        }

        protected virtual string AdvancedResolveSort(CustomerSorting sort, bool sortBackwards)
        {
            switch (sort)
            {
                case CustomerSorting.ID:
                    return "ACCOUNTNUM" + (sortBackwards ? " DESC" : " ASC");
                case CustomerSorting.Name:
                    return "NAME" + (sortBackwards ? " DESC" : " ASC");
                case CustomerSorting.CashCustomer:
                    return "NONCHARGABLEACCOUNT" + (sortBackwards ? " DESC" : " ASC");
                case CustomerSorting.SalesTaxGroup:
                    return "TAXGROUP" + (sortBackwards ? " DESC" : " ASC");
                case CustomerSorting.PriceGroup:
                    return "PRICEGROUP" + (sortBackwards ? " DESC" : " ASC");
                case CustomerSorting.LineDiscountGroup:
                    return "LINEDISC" + (sortBackwards ? " DESC" : " ASC");
                case CustomerSorting.TotalDiscountGroup:
                    return "ENDDISC" + (sortBackwards ? " DESC" : " ASC");
                case CustomerSorting.CreditLimit:
                    return "CREDITMAX" + (sortBackwards ? " DESC" : " ASC");
                case CustomerSorting.Blocked:
                    return "BLOCKED" + (sortBackwards ? " DESC" : " ASC");
            }
            return "";
        }

        protected virtual void PopulateName(IDataReader dr, Name name)
        {
            name.First = (string)dr["FIRSTNAME"];
            name.Last = (string)dr["LASTNAME"];
            name.Middle = (string)dr["MIDDLENAME"];
            name.Prefix = (string)dr["NAMEPREFIX"];
            name.Suffix = (string)dr["NAMESUFFIX"];
        }

        protected virtual void PopulateCustomerListItemWithoutAddress(IConnectionManager entry, IDataReader dr, CustomerListItem customerListItem, object defaultFormat)
        {
            customerListItem.ID = (string)dr["ACCOUNTNUM"];
            customerListItem.MasterID = AsGuid(dr["MASTERID"]);
            customerListItem.Text = (string)dr["NAME"];
            customerListItem.FirstName = (string)dr["FIRSTNAME"];
            customerListItem.LastName = (string)dr["LASTNAME"];
            customerListItem.MiddleName = (string)dr["MIDDLENAME"];
            customerListItem.Prefix = (string)dr["NAMEPREFIX"];
            customerListItem.Suffix = (string)dr["NAMESUFFIX"];
            customerListItem.Deleted = (bool)dr["DELETED"];

            customerListItem.AccountNumber = (string)dr["INVOICEACCOUNT"];
            customerListItem.InvoiceAccountName = (string)dr["INVOICEACCOUNTNAME"];

            customerListItem.SearchName = (string)dr["NAMEALIAS"];
            customerListItem.IdentificationNumber = (string)dr["ORGID"];
            customerListItem.LocallySaved = ((byte)dr["LOCALLYSAVED"] != 0);
        }

        protected virtual void PopulateCustomerListItem(IConnectionManager entry, IDataReader dr, CustomerListItem customerListItem, object defaultFormat)
        {
            PopulateCustomerListItemWithoutAddress(entry, dr, customerListItem, defaultFormat);

            var address = new CustomerAddress
            {
                Address =
                        {
                            Address1 = (string) dr["STREET"],
                            Address2 = (string) dr["ADDRESS"],
                            Zip = (string) dr["ZIPCODE"],
                            City = (string) dr["CITY"],
                            State = (string) dr["STATE"],
                            Country = (string) dr["COUNTRY"],
                            AddressFormat =
                                (dr["ADDRESSFORMAT"] == DBNull.Value)
                                    ? (Address.AddressFormatEnum) defaultFormat
                                    : (Address.AddressFormatEnum) ((int) (dr["ADDRESSFORMAT"]))
                        }
            };

            customerListItem.Addresses.Add(address);
        }

        protected virtual void PopulateCustomerListItemAdvanced(IConnectionManager entry, IDataReader dr, CustomerListItemAdvanced customerListItemAdvanced, object defaultFormat)
        {
            customerListItemAdvanced.ID = (string)dr["ACCOUNTNUM"];
            customerListItemAdvanced.MasterID = AsGuid(dr["MASTERID"]);
            customerListItemAdvanced.Text = (string)dr["NAME"];
            customerListItemAdvanced.CashCustomer = ((byte)dr["NONCHARGABLEACCOUNT"] == 1);
            customerListItemAdvanced.TaxExempt = (TaxExemptEnum)dr["TAXEXEMPT"];
            customerListItemAdvanced.SalesTaxGroupID = (string)dr["TAXGROUP"];
            customerListItemAdvanced.PriceGroupID = (string)dr["PRICEGROUP"];
            customerListItemAdvanced.LineDiscountGroupID = (string)dr["LINEDISC"];
            customerListItemAdvanced.TotalDiscountGroupID = (string)dr["ENDDISC"];
            customerListItemAdvanced.CreditLimit = (decimal)dr["CREDITMAX"];
            customerListItemAdvanced.Blocked = (BlockedEnum)dr["BLOCKED"];
            customerListItemAdvanced.Deleted = ((bool)dr["DELETED"]);            
        }

        protected virtual void PopulateCustomer(IConnectionManager entry, IDataReader dr, Customer customer, object defaultFormat)
        {
            // From CUSTOMER
            // --------------------------------------------------------------------
            customer.ID = (string)dr["ACCOUNTNUM"];
            customer.MasterID = AsGuid(dr["MASTERID"]);
            customer.Text = (string)dr["NAME"];
            customer.FirstName = (string)dr["FIRSTNAME"];
            customer.LastName = (string)dr["LASTNAME"];
            customer.MiddleName = (string)dr["MIDDLENAME"];
            customer.Prefix = (string)dr["NAMEPREFIX"];
            customer.Suffix = (string)dr["NAMESUFFIX"];
            //PopulateName(dr, customer.Name);
            customer.AccountNumber = (string)dr["INVOICEACCOUNT"];
            customer.Currency = (string)dr["CURRENCY"];
            customer.LanguageCode = (string)dr["LANGUAGEID"];
            customer.TaxGroup = (string)dr["TAXGROUP"];
            customer.PriceGroupID = (string)dr["PRICEGROUP"];
            customer.LineDiscountID = (string)dr["LINEDISC"];
            customer.MultiLineDiscountID = (string)dr["MULTILINEDISC"];
            customer.FinalDiscountID = (string)dr["ENDDISC"];
            customer.MaxCredit = (decimal)dr["CREDITMAX"];
            customer.IdentificationNumber = (string)dr["ORGID"];
            customer.Blocked = (BlockedEnum)dr["BLOCKED"];
            customer.NonChargableAccount = ((byte)dr["NONCHARGABLEACCOUNT"] == 1);
            customer.SearchName = (string)dr["NAMEALIAS"];
            customer.CustomerGroupID = (string)dr["CUSTGROUP"];
            customer.PricesIncludeSalesTax = ((byte)dr["INCLTAX"] == 1);
            customer.Url = (string)dr["URL"];
            customer.TaxOffice = (string)dr["TAXOFFICE"];
            customer.UsePurchaseRequest = ((byte)dr["USEPURCHREQUEST"] == 1);
            customer.Deleted = ((bool)dr["DELETED"]);
            //item.IsLoyaltyCustomer = ((byte)dr["LOYALTYCUSTOMER"] == 1);

            customer.Telephone = (string)dr["PHONE"];
            customer.MobilePhone = (string)dr["CELLULARPHONE"];
            customer.VatNum = (string)dr["VATNUM"];
            customer.Email = (string)dr["EMAIL"];
            customer.TaxExempt = (TaxExemptEnum)dr["TAXEXEMPT"];

            customer.LocallySaved = ((byte)dr["LOCALLYSAVED"] != 0);

            customer.Gender = (GenderEnum)(int)dr["GENDER"];
            customer.DateOfBirth = (DateTime)dr["DATEOFBIRTH"];
            customer.ReceiptSettings = (int)dr["RECEIPTOPTION"] == -1 ? ReceiptSettingsEnum.Printed : (ReceiptSettingsEnum)dr["RECEIPTOPTION"];
            customer.ReceiptEmailAddress = (string)dr["RECEIPTEMAIL"];
        }

        protected virtual void PopulateCustomerWithDefaultAddressWithCount(IConnectionManager entry, IDataReader dr, Customer customer, ref int rowCount, object defaultFormat)
        {
            PopulateCustomerWithDefaultAddress(entry, dr, customer, defaultFormat);
            PopulateRowCount(entry, dr, ref rowCount);

        }
        protected virtual void PopulateRowCount(IConnectionManager entry, IDataReader dr, ref int rowCount)
        {
            if (entry.Connection.DatabaseVersion == ServerVersion.SQLServer2012 ||
                entry.Connection.DatabaseVersion == ServerVersion.Unknown)
            {
                rowCount = (int)dr["Row_Count"];
            }
        }

        private void PopulateCustomerWithDefaultAddress(IConnectionManager entry, IDataReader dr, Customer customer, object defaultFormat)
        {
            PopulateCustomer(entry, dr, customer, defaultFormat);
            customer.Addresses = new List<CustomerAddress>();

            CustomerAddress customerAddress = new CustomerAddress();
            customerAddress.ID = AsString(dr["ACCOUNTNUM"]);
            customerAddress.Telephone = AsString(dr["PHONE"]);
            customerAddress.MobilePhone = AsString(dr["CELLULARPHONE"]);
            customerAddress.Email = AsString(dr["EMAIL"]);
            customerAddress.IsDefault = true;//AsBool(dr["ISDEFAULT"]);

            var address = customerAddress.Address;

            address.AddressType = Address.AddressTypes.Shipping;
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

            customer.Addresses.Add(customerAddress);
        }

        private void PopulateCustomerWithAddress(IConnectionManager entry, IDataReader dr, Customer customer, object defaultFormat)
        {
            if (customer.ID == RecordIdentifier.Empty)
            {
                PopulateCustomer(entry, dr, customer, defaultFormat);
            }

            if (customer.Addresses == null)
            {
                customer.Addresses = new List<CustomerAddress>();
            }

            CustomerAddress customerAddress = new CustomerAddress();
            customerAddress.ID = AsString(dr["ACCOUNTNUM"]);
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
            address.County = AsString(dr["COUNTY"]);
            address.AddressFormat = (dr["ADDRESSFORMAT"] == DBNull.Value) ? (Address.AddressFormatEnum)defaultFormat : (Address.AddressFormatEnum)((int)(dr["ADDRESSFORMAT"]));

            customerAddress.TaxGroup = AsString(dr["TAXGROUP"]);
            customerAddress.TaxGroupName = AsString(dr["TAXGROUPNAME"]);

            customerAddress.InDatabase = true;

            customer.Addresses.Add(customerAddress);
        }

        protected virtual void PopulateCustomerAll(IConnectionManager entry, IDataReader dr, Customer customer, object defaultFormat)
        {
            PopulateCustomer(entry, dr, customer, defaultFormat);

            customer.CurrencyDescription = (string)dr["CURRENCYDESCRIPTION"];
            customer.TaxGroupName = (string)dr["TAXGROUPNAME"];
            customer.PriceGroupDescription = (string)dr["PRICEGROUPNAME"];
            customer.LineDiscountDescription = (string)dr["LINEDISCNAME"];
            customer.MultiLineDiscountDescription = (string)dr["MULTILINEDISCNAME"];
            customer.FinalDiscountDescription = (string)dr["ENDDISCNAME"];
        }

        public virtual List<CustomerListItem> Search(IConnectionManager entry, SearchParameter[] parameters, int maxCount)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                string wherePart = "";
                foreach (SearchParameter parameter in parameters)
                {
                    wherePart += (" and " + parameter.ParamName +
                        (parameter.ParamOperator == OperatorEnum.Equals ? " = " : " <> ") +
                        "@" + parameter.ParamName);
                }

                cmd.CommandText = "Select";
                if (maxCount != 0)
                {
                    cmd.CommandText += " Top " + maxCount.ToString();
                }
                cmd.CommandText +=
                    " ACCOUNTNUM, MASTERID, ISNULL(NAME,'') as NAME,ISNULL(INVOICEACCOUNT,'') as INVOICEACCOUNT, '' as INVOICEACCOUNTNAME , ISNULL(NAMEALIAS, '') AS NAMEALIAS, " +
                    "ISNULL(FIRSTNAME,'') as FIRSTNAME,ISNULL(MIDDLENAME,'') as MIDDLENAME,ISNULL(LASTNAME,'') as LASTNAME, " +
                    "ISNULL(NAMEPREFIX,'') as NAMEPREFIX,ISNULL(NAMESUFFIX,'') as NAMESUFFIX,ISNULL(ORGID,'') as ORGID, LOCALLYSAVED, ISNULL(DELETED, 0) AS DELETED " +
                    "from CUSTOMER " +
                       "where DATAAREAID = @dataAreaId " + wherePart + " order by ACCOUNTNUM";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                foreach (var parameter in parameters)
                {
                    MakeParam(cmd, parameter.ParamName, parameter.ParamValue, parameter.ParamType);
                }

                return Execute<CustomerListItem>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateCustomerListItemWithoutAddress);
            }
        }

        /// <summary>
        /// Searches for customers that contain a given search text, and returns the results as a List of CustomerListItem
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchString">The text to search for. Searches in customer NAME, customer ACCOUNTNUM and customer INVOICEACCOUNT fields.</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
        /// <param name="sortBy">A enum defining the sort column</param>
        /// <param name="sortBackwards">Set to true if wanting to sort backwards, else false</param>
        /// <returns>A list of found customers</returns>
        public virtual List<CustomerListItem> Search(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, CustomerSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";

                cmd.CommandText = AddCustomHandling(BaseSqlWithAddressOverRows.Replace("<ORDER>", ResolveSort(sortBy, sortBackwards)).
                    Replace("<WHERE>",
                      "AND C.DELETED = 0 AND (C.ACCOUNTNUM Like @searchString or C.NAME Like @searchString or C.INVOICEACCOUNT Like @searchString or IC.ACCOUNTNUM Like @searchString or IC.NAME Like @searchString or IC.INVOICEACCOUNT Like @searchString or C.FIRSTNAME like @searchString or C.LASTNAME like @searchString or C.NAMEALIAS like @searchString) ")
                    + "WHERE s.ROW between @rowFrom and @rowTo");

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);
                MakeParam(cmd, "rowFrom", rowFrom);
                MakeParam(cmd, "rowTo", rowTo);

                return Execute<CustomerListItem>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateCustomerListItem);
            }
        }

        /// <summary>
        /// Searches for customers that contain a given search text, and returns the results as a List of CustomerListItem
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="searchString">The text to search for. Searches in customer NAME, customer ACCOUNTNUM and customer ADDRESS fields.</param>
        /// <param name="rowFrom">The number of the first row to fetch</param>
        /// <param name="rowTo">The number of the last row to fetch</param>
        /// <param name="beginsWith">Specifies if the search text is the beginning of the text or if the text may contain the search text.</param>
        /// <param name="sortBy">A enum defining the sort column</param>
        /// <param name="sortBackwards">Set to true if wanting to sort backwards, else false</param>
        /// <returns>A list of found customers</returns>
        public virtual List<CustomerListItem> SearchWithAddress(IConnectionManager entry, string searchString, int rowFrom, int rowTo, bool beginsWith, CustomerSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = (beginsWith ? "" : "%") + searchString + "%";

                cmd.CommandText = AddCustomHandling(BaseSqlWithAddressOverRows.Replace("<ORDER>", ResolveSort(sortBy, sortBackwards)).
                    Replace("<WHERE>",
                      "AND C.DELETED = 0 AND (C.ACCOUNTNUM Like @searchString or C.NAME Like @searchString or C.INVOICEACCOUNT Like @searchString or IC.ACCOUNTNUM Like @searchString or IC.NAME Like @searchString or IC.INVOICEACCOUNT Like @searchString or C.FIRSTNAME like @searchString or C.LASTNAME like @searchString or C.NAMEALIAS like @searchString) ")
                    + "WHERE s.ROW between @rowFrom and @rowTo");

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);
                MakeParam(cmd, "rowFrom", rowFrom);
                MakeParam(cmd, "rowTo", rowTo);

                return Execute<CustomerListItem>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateCustomerListItem);
            }
        }

        public virtual List<CustomerListItem> Search(IConnectionManager entry, string displayName, string firstName, string lastName, int rowFrom, int rowTo, bool beginsWith, CustomerSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var displayNameSearch = (beginsWith ? "" : "%") + displayName + "%";
                var firstNameSearch = string.IsNullOrWhiteSpace(firstName) ? "" : firstName + "%";
                var lastNameSearch = string.IsNullOrWhiteSpace(lastName) ? "" : lastName + "%";

                cmd.CommandText = AddCustomHandling(BaseSqlWithAddressOverRows.Replace("<ORDER>", ResolveSort(sortBy, sortBackwards)).
                    Replace("<WHERE>",
                      "AND C.DELETED = 0 AND (C.NAME Like @displayNameSearch or IC.NAME Like @displayNameSearch or IC.FIRSTNAME Like @firstNameSearch or IC.LASTNAME Like @lastNameSearch or C.FIRSTNAME Like @firstNameSearch or C.LASTNAME Like @lastNameSearch) ")
                    + "WHERE s.ROW between @rowFrom and @rowTo");

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "displayNameSearch", displayNameSearch);
                MakeParam(cmd, "firstNameSearch", firstNameSearch);
                MakeParam(cmd, "lastNameSearch", lastNameSearch);
                MakeParam(cmd, "rowFrom", rowFrom);
                MakeParam(cmd, "rowTo", rowTo);

                return Execute<CustomerListItem>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateCustomerListItem);
            }
        }

        public virtual List<CustomerListItemAdvanced> AdvancedSearch(IConnectionManager entry,
                                                                        int rowFrom,
                                                                        int rowTo,
                                                                        out int totalRecordsMatching,
                                                                        CustomerSorting sortBy,
                                                                        bool sortBackwards,
                                                                        string idOrDescription = null,
                                                                        bool idOrDescriptionBeginsWith = true,
                                                                        RecordIdentifier salesTaxGroupID = null,
                                                                        RecordIdentifier priceGroupID = null,
                                                                        RecordIdentifier lineDiscountGroupID = null,
                                                                        RecordIdentifier totalDiscountGroupID = null,
                                                                        RecordIdentifier invoiceCustomerID = null,
                                                                        BlockedEnum? isBlocked = null,                                                                        
                                                                        bool? showDeleted = null,
                                                                        TaxExemptEnum? taxExempt = null)
        {
            string whereConditions = "";

            using (var cmd = entry.Connection.CreateCommand())
            using (var cmdCount = entry.Connection.CreateCommand())
            {

                if (idOrDescription != null && idOrDescription.Trim().Length > 0)
                {
                    idOrDescription = PreProcessSearchText(idOrDescription, true, idOrDescriptionBeginsWith);

                    whereConditions +=
                            " and (C.NAME Like @searchString or C.ACCOUNTNUM Like @searchString or C.NAMEALIAS Like @searchString) ";

                    MakeParam(cmd, "searchString", idOrDescription);
                    MakeParam(cmdCount, "searchString", idOrDescription);
                }

                if (salesTaxGroupID != null)
                {
                    whereConditions +=
                            " AND C.TAXGROUP = @salesTaxGroupID ";

                    MakeParam(cmd, "salesTaxGroupID", (string)salesTaxGroupID);
                    MakeParam(cmdCount, "salesTaxGroupID", (string)salesTaxGroupID);
                }

                if (invoiceCustomerID != null)
                {
                    whereConditions +=
                            " AND C.INVOICEACCOUNT = @invoiceCustomerID ";

                    MakeParam(cmd, "invoiceCustomerID", (string)invoiceCustomerID);
                    MakeParam(cmdCount, "invoiceCustomerID", (string)invoiceCustomerID);
                }

                if (priceGroupID != null)
                {
                    whereConditions +=
                            " AND C.PRICEGROUP = @priceGroupID ";

                    MakeParam(cmd, "priceGroupID", (string)priceGroupID);
                    MakeParam(cmdCount, "priceGroupID", (string)priceGroupID);
                }

                if (lineDiscountGroupID != null)
                {
                    whereConditions +=
                            " AND C.LINEDISC = @lineDiscountGroupID ";

                    MakeParam(cmd, "lineDiscountGroupID", (string)lineDiscountGroupID);
                    MakeParam(cmdCount, "lineDiscountGroupID", (string)lineDiscountGroupID);
                }

                if (totalDiscountGroupID != null)
                {
                    whereConditions +=
                            " AND C.ENDDISC = @totalDiscountGroupID ";

                    MakeParam(cmd, "totalDiscountGroupID", (string)totalDiscountGroupID);
                    MakeParam(cmdCount, "totalDiscountGroupID", (string)totalDiscountGroupID);
                }

                if (isBlocked != null)
                {
                    whereConditions +=
                            " AND C.BLOCKED = @isBlocked ";

                    MakeParam(cmd, "isBlocked", (int)isBlocked, SqlDbType.Int);
                    MakeParam(cmdCount, "isBlocked", (int)isBlocked, SqlDbType.Int);
                }

                if (taxExempt != null)
                {
                    if (taxExempt == TaxExemptEnum.No)
                    {
                        whereConditions += " AND (C.TAXEXEMPT = @taxexempt OR C.TAXEXEMPT IS NULL) ";
                    }
                    else
                    {
                        whereConditions += " AND C.TAXEXEMPT = @taxexempt ";
                    }

                    MakeParam(cmd, "taxexempt", (int)taxExempt, SqlDbType.Int);
                    MakeParam(cmdCount, "taxexempt", (int)taxExempt, SqlDbType.Int);
                }

                if (showDeleted != null && showDeleted.Value)
                {
                    whereConditions +=
                        " AND C.DELETED = @showDeleted ";

                    MakeParam(cmd, "showDeleted", 1, SqlDbType.Int);
                    MakeParam(cmdCount, "showDeleted", 1, SqlDbType.Int);
                }
                else
                {
                    whereConditions +=
                        " AND C.DELETED = @showDeleted ";

                    MakeParam(cmd, "showDeleted", 0, SqlDbType.Int);
                    MakeParam(cmdCount, "showDeleted", 0, SqlDbType.Int);
                }

                cmd.CommandText = @"select ss.* from(
                    select s.*, ROW_NUMBER() OVER(order by <sort>) AS ROW from (
                    
                    select C.ACCOUNTNUM,
                        C.MASTERID,
                        ISNULL(C.NAME,'') as NAME,
                        ISNULL(C.NONCHARGABLEACCOUNT,0) as NONCHARGABLEACCOUNT,
                        ISNULL(C.TAXGROUP,'') as TAXGROUP,
                        ISNULL(C.TAXEXEMPT, 0) AS TAXEXEMPT,
                        ISNULL(C.PRICEGROUP,'') as PRICEGROUP,
                        ISNULL(C.LINEDISC,'') as LINEDISC,
                        ISNULL(C.ENDDISC,'') as ENDDISC,
                        ISNULL(C.CREDITMAX,0) as CREDITMAX,
                        ISNULL(C.BLOCKED,0) as BLOCKED,
                        ISNULL(C.DELETED,0) as DELETED
                    from CUSTOMER C 
                    where C.DATAAREAID = @dataAreaId <whereConditions>
                    ) s
                    ) ss
                    where ss.ROW between @rowFrom and @rowTo";

                cmd.CommandText = cmd.CommandText.Replace("<sort>", AdvancedResolveSort(sortBy, sortBackwards));
                cmd.CommandText = cmd.CommandText.Replace("<whereConditions>", whereConditions);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "rowFrom", rowFrom);
                MakeParam(cmd, "rowTo", rowTo);

                // Do a count first
                cmdCount.CommandText = @"select count(*) from CUSTOMER C where C.DATAAREAID = @dataAreaId " + whereConditions;

                MakeParam(cmdCount, "dataAreaId", entry.Connection.DataAreaId);
                totalRecordsMatching = (int)entry.Connection.ExecuteScalar(cmdCount);
                List<CustomerListItemAdvanced> result =  Execute<CustomerListItemAdvanced>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateCustomerListItemAdvanced);

                foreach (CustomerListItemAdvanced customer in result)
                {
                    GetTaxInformation(entry, customer);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets a customer by a card number
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="cardNumber">Number of the card</param>
        /// <param name="usageIntent">Specifies how much extra data should be loaded</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns></returns>
        public virtual Customer GetByCardNumber(IConnectionManager entry, RecordIdentifier cardNumber, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone)
        {
            ValidateSecurity(entry);

            object result;
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "Select LINKID from RBOMSRCARDTABLE where CARDNUMBER = @cardNumber and DATAAREAID = @dataAreaID and LINKTYPE = 1";

                MakeParam(cmd, "cardNumber", (string)cardNumber);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                result = entry.Connection.ExecuteScalar(cmd);
            }

            if (result == null)
            {
                return null;
            }

            return Get(entry, (string)result, usageIntent, cacheType);
        }

        public virtual string GetCustomerName(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = "select NAME from CUSTOMER where ACCOUNTNUM = @ID and DATAAREAID = @dataAreaID ";

                MakeParam(cmd, "ID", (string)id);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                var result = entry.Connection.ExecuteScalar(cmd);

                if (result == null)
                {
                    return null;
                }

                return (string)result;
            }
        }

        public virtual Name GetCustomerLongName(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ISNULL(FIRSTNAME, '') AS FIRSTNAME, ISNULL(LASTNAME, '') AS LASTNAME, ISNULL(MIDDLENAME, '') AS MIDDLENAME,
                                   ISNULL(NAMEPREFIX, '') AS NAMEPREFIX, ISNULL(NAMESUFFIX, '') AS NAMESUFFIX FROM CUSTOMER
                                   WHERE DATAAREAID = @dataAreaID and ACCOUNTNUM = @customerID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "customerID", id);
                List<Name> result = Execute<Name>(entry, cmd, CommandType.Text, PopulateName);
                return result.Count > 0 ? result[0] : null;
            }
        }

        public virtual List<CustomerListItem> GetLocallySavedCustomers(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = AddCustomHandling(BaseSqlWithAddress.Replace("<WHERE>", "and LOCALLYSAVED = 1"));
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                return Execute<CustomerListItem>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateCustomerListItem);
            }
        }

        public virtual bool LocallySavedCustomersExist(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT Count('x') FROM CUSTOMER WHERE LOCALLYSAVED = 1 AND DATAAREAID = @dataAreaID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return ((int)entry.Connection.ExecuteScalar(cmd) > 0);
            }
        }

        /// <summary>
        /// Gets a customer by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the customer to fetch</param>
        /// <param name="usageIntent">Specifies what extra data should be loaded</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>The requested customer or null if not found</returns>
        public virtual Customer Get(IConnectionManager entry, RecordIdentifier id, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                if (usageIntent == UsageIntentEnum.Reporting)
                {
                    cmd.CommandText = AddCustomHandling(BaseSql
                        .Replace("<ADDITIONALCOLUMNS>",
                            ", ISNULL(cur.TXT,'') as CURRENCYDESCRIPTION,ISNULL(g1.NAME,'') as PRICEGROUPNAME,ISNULL(g2.NAME,'') as LINEDISCNAME,ISNULL(g3.NAME,'') as MULTILINEDISCNAME,ISNULL(g4.NAME,'') as ENDDISCNAME ")
                        .Replace("<JOINS>",
                            "left outer join CUSTOMERADDRESS A on C.ACCOUNTNUM=A.ACCOUNTNUM AND A.ADDRESSTYPE=1 AND A.ISDEFAULT=1 " +
                            "left outer join PRICEDISCGROUP g1 on c.PRICEGROUP = g1.GROUPID and g1.MODULE = 1 and g1.TYPE = 0 and c.DATAAREAID = g1.DATAAREAID " +
                            "left outer join PRICEDISCGROUP g2 on c.LINEDISC = g2.GROUPID and g2.MODULE = 1 and g2.TYPE = 1 and c.DATAAREAID = g2.DATAAREAID " +
                            "left outer join PRICEDISCGROUP g3 on c.MULTILINEDISC = g3.GROUPID and g3.MODULE = 1 and g3.TYPE = 2 and c.DATAAREAID = g3.DATAAREAID " +
                            "left outer join PRICEDISCGROUP g4 on c.ENDDISC = g4.GROUPID and g4.MODULE = 1 and g4.TYPE = 3 and c.DATAAREAID = g4.DATAAREAID " +
                            "left outer join CURRENCY cur on c.CURRENCY = cur.CURRENCYCODE and c.DATAAREAID = cur.DATAAREAID ")
                        .Replace("<WHERE>", "and c.AccountNum = @id"));

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "id", (string)id);

                    var customerAll = Get<Customer>(entry, cmd, CommandType.Text, id, entry.Settings.AddressFormat, PopulateCustomerAll, cacheType, usageIntent);
                    GetAddresses(entry, customerAll);
                    GetTaxInformation(entry, customerAll);
                    return customerAll;
                }

                cmd.CommandText = AddCustomHandling(BaseSql
                     .Replace("<ADDITIONALCOLUMNS>", "")
                     .Replace("<JOINS>",
                            "left outer join CUSTOMERADDRESS A on C.ACCOUNTNUM=A.ACCOUNTNUM AND A.ADDRESSTYPE=1 AND A.ISDEFAULT=1 ")
                     .Replace("<WHERE>", "and c.ACCOUNTNUM = @id"));

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                var customer = Get<Customer>(entry, cmd, CommandType.Text, id, entry.Settings.AddressFormat, PopulateCustomer, cacheType, usageIntent);
                GetAddresses(entry, customer);
                GetTaxInformation(entry, customer);
                return customer;
            }
        }

        public virtual List<Customer> GetAllCustomers(IConnectionManager entry, UsageIntentEnum usageIntent, bool getDeletedCustomers = true)
        {
            SearchParameter[] searchParameter = new SearchParameter[0];

            if (!getDeletedCustomers)
            {
                SearchParameter parameter = new SearchParameter("DELETED", OperatorEnum.NotEquals, 1, SqlDbType.Int);
                searchParameter = new SearchParameter[] { parameter };
            }
            if (usageIntent == UsageIntentEnum.Minimal)
            {
                List<CustomerListItem> list = Search(entry, searchParameter, 0);
                List<Customer> customers = new List<Customer>();
                foreach (CustomerListItem customerListItem in list)
                {
                    Customer customer = new Customer()
                    {
                        ID = customerListItem.ID,
                        Text = customerListItem.Text,
                        AccountNumber = customerListItem.AccountNumber,
                        Addresses = customerListItem.Addresses,
                        IdentificationNumber = customerListItem.IdentificationNumber,
                        LocallySaved = customerListItem.LocallySaved
                    };
                    customer.FirstName = customerListItem.FirstName;
                    customer.LastName = customerListItem.LastName;
                    customer.MiddleName = customerListItem.MiddleName;
                    customer.Prefix = customerListItem.Prefix;
                    customer.Suffix = customerListItem.Suffix;
                    customers.Add(customer);
                }
                return customers;
            }
            using (IDbCommand cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<Customer> customers;
                if (usageIntent == UsageIntentEnum.Reporting)
                {
                    cmd.CommandText = AddCustomHandling(BaseSql
                        .Replace("<ADDITIONALCOLUMNS>",
                            ", ISNULL(cur.TXT,'') as CURRENCYDESCRIPTION,ISNULL(g1.NAME,'') as PRICEGROUPNAME,ISNULL(g2.NAME,'') as LINEDISCNAME,ISNULL(g3.NAME,'') as MULTILINEDISCNAME,ISNULL(g4.NAME,'') as ENDDISCNAME ")
                        .Replace("<JOINS>",
                            "left outer join CUSTOMERADDRESS A on C.ACCOUNTNUM=A.ACCOUNTNUM AND A.ADDRESSTYPE=1 AND A.ISDEFAULT=1 " +
                            "left outer join PRICEDISCGROUP g1 on c.PRICEGROUP = g1.GROUPID and g1.MODULE = 1 and g1.TYPE = 0 and c.DATAAREAID = g1.DATAAREAID " +
                            "left outer join PRICEDISCGROUP g2 on c.LINEDISC = g2.GROUPID and g2.MODULE = 1 and g2.TYPE = 1 and c.DATAAREAID = g2.DATAAREAID " +
                            "left outer join PRICEDISCGROUP g3 on c.MULTILINEDISC = g3.GROUPID and g3.MODULE = 1 and g3.TYPE = 2 and c.DATAAREAID = g3.DATAAREAID " +
                            "left outer join PRICEDISCGROUP g4 on c.ENDDISC = g4.GROUPID and g4.MODULE = 1 and g4.TYPE = 3 and c.DATAAREAID = g4.DATAAREAID " +
                            "left outer join CURRENCY cur on c.CURRENCY = cur.CURRENCYCODE and c.DATAAREAID = cur.DATAAREAID ")
                        .Replace("<WHERE>", " "));

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                    customers = Execute<Customer>(entry, cmd, CommandType.Text, (dr, item) => PopulateCustomerAll(entry, dr, item, null));
                    if (customers != null)
                    {
                        foreach (Customer customer in customers)
                        {
                            GetAddresses(entry, customer);
                            GetTaxInformation(entry, customer);
                        }
                    }
                    return customers;
                }

                cmd.CommandText = AddCustomHandling(BaseSql
                     .Replace("<ADDITIONALCOLUMNS>", "")
                     .Replace("<JOINS>",
                            "left outer join CUSTOMERADDRESS A on C.ACCOUNTNUM=A.ACCOUNTNUM AND A.ADDRESSTYPE=1 AND A.ISDEFAULT=1 ")
                     .Replace("<WHERE>", " "));

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                customers = Execute<Customer>(entry, cmd, CommandType.Text, (dr, item) => PopulateCustomer(entry, dr, item, null));

                if (customers != null)
                {
                    foreach (Customer customer in customers)
                    {
                        GetAddresses(entry, customer);
                        GetTaxInformation(entry, customer);
                    }
                }
                return customers;
            }
        }

        public virtual Customer GetTemporaryInvoiceCustomer(IConnectionManager entry, RecordIdentifier id, UsageIntentEnum usageIntent, CacheType cacheType = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = AddCustomHandling(BaseSql
                    .Replace("<ADDITIONALCOLUMNS>", "")
                    .Replace("<JOINS>",
                            "left outer join CUSTOMERADDRESS A on C.ACCOUNTNUM=A.ACCOUNTNUM AND A.ADDRESSTYPE=1 AND A.ISDEFAULT=1 ")
                    .Replace("<WHERE>", "and c.ACCOUNTNUM = @id"));

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                var customer = Get<Customer>(entry, cmd, CommandType.Text, id, entry.Settings.AddressFormat, PopulateCustomer, cacheType, usageIntent);
                GetAddresses(entry, customer);
                GetTaxInformation(entry, customer);
                return customer;
            }
        }

        protected virtual void GetAddresses(IConnectionManager entry, Customer customer)
        {
            if (customer != null)
            {
                customer.Addresses = Providers.CustomerAddressData.GetListForCustomer(entry, customer.ID);
            }
        }

        public virtual void ClearCache()
        {
            taxExemptTaxGroup = null;
        }

        
        public virtual void RefreshCache(IConnectionManager entry)
        {
            ClearCache();

            GetCacheInformation(entry);
        }

        protected virtual void GetCacheInformation(IConnectionManager entry)
        {
            if (taxExemptTaxGroup == null)
            {
                Parameters parameters = Providers.ParameterData.Get(entry);
                if (!string.IsNullOrEmpty(parameters.TaxExemptTaxGroup.StringValue))
                {
                    taxExemptTaxGroup = Providers.SalesTaxGroupData.Get(entry, parameters.TaxExemptTaxGroup, CacheType.CacheTypeTransactionLifeTime);
                }
                else
                {
                    taxExemptTaxGroup = new SalesTaxGroup();
                }
            }
        }

        protected virtual void GetTaxInformation(IConnectionManager entry, CustomerListItemAdvanced customerListItem)
        {
            if (customerListItem != null && customerListItem.TaxExempt == TaxExemptEnum.Yes)
            {
                GetCacheInformation(entry);

                customerListItem.SalesTaxGroupID = taxExemptTaxGroup.ID;
                customerListItem.SalesTaxGroupName = taxExemptTaxGroup.Text;
            }
        }

        protected virtual void GetTaxInformation(IConnectionManager entry, Customer customer)
        {
            if (customer != null && customer.TaxExempt == TaxExemptEnum.Yes)
            {
                GetCacheInformation(entry);

                customer.TaxGroup = taxExemptTaxGroup.ID;
                customer.TaxGroupName = taxExemptTaxGroup.Text;
            }
        }

        public virtual List<CustomerListItem> GetList(IConnectionManager entry, string searchString, CustomerSorting sortBy, bool sortBackwards, bool beginsWith = true, RecordIdentifier excludeID = null)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                var modifiedSearchString = PreProcessSearchText(searchString, true, beginsWith);

                cmd.CommandText = AddCustomHandling(BaseSqlWithAddressOverRows.Replace("<ORDER>", ResolveSort(sortBy, sortBackwards)).
                    Replace("<WHERE>",
                      "and (C.ACCOUNTNUM Like @searchString or C.NAME Like @searchString or C.INVOICEACCOUNT Like @searchString) or (IC.ACCOUNTNUM Like @searchString or IC.NAME Like @searchString or IC.INVOICEACCOUNT Like @searchString) or C.FIRSTNAME like @searchString or C.LASTNAME like @searchString or C.NAMEALIAS like @searchString"));

                if (excludeID != null && (excludeID != RecordIdentifier.Empty || excludeID != ""))
                {
                    cmd.CommandText += " where ACCOUNTNUM <> @excludeID ";
                    MakeParam(cmd, "excludeID", excludeID);
                }

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "searchString", modifiedSearchString);

                return Execute<CustomerListItem>(entry, cmd, CommandType.Text, entry.Settings.AddressFormat, PopulateCustomerListItem);
            }
        }

        public RecordIdentifier GetOmniCustomerID(IConnectionManager entry, string userName, string password)
        {
            RecordIdentifier customerID = "";

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select CUSTOMERID
                    from OMNICUSTOMER 
                    where DATAAREAID = @dataAreaID and USERNAME = @userName and PASSWORD = @password";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "userName", userName);
                MakeParam(cmd, "password", password);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    if (dr.Read())
                    {
                        if (dr["CUSTOMERID"] != DBNull.Value)
                        {
                            customerID = (string)dr["CUSTOMERID"];
                        }
                    }
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }

            return customerID;
        }

        public List<DataEntity> GetOmniCustomers(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"Select o.CUSTOMERID, ct.NAME
                    from OMNICUSTOMER o
                    left join CUSTOMER ct on ct.DATAAREAID = o.DATAAREAID AND ct.ACCOUNTNUM = o.CUSTOMERID
                    where o.DATAAREAID = @dataAreaID";

                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                return Execute<DataEntity>(entry, cmd, CommandType.Text, "NAME", "CUSTOMERID");
            }
        }

        public bool ExistsOmniCustomer(IConnectionManager entry, RecordIdentifier customerID)
        {
            return RecordExists(entry, "OMNICUSTOMER", "CUSTOMERID", customerID);
        }

        public bool ExistsOmniUser(IConnectionManager entry, string userName)
        {
            return RecordExists(entry, "OMNICUSTOMER", "USERNAME", userName);
        }

        public void DeleteOmnicustomer(IConnectionManager entry, RecordIdentifier customerID)
        {
            DeleteRecord(entry, "OMNICUSTOMER", "CUSTOMERID", customerID, Permission.CustomerEdit);
        }

        public void SaveOmniCustomer(IConnectionManager entry, RecordIdentifier customerID, string userName, string pwd)
        {
            if (customerID == RecordIdentifier.Empty) return;

            var statement = new SqlServerStatement("OMNICUSTOMER");

            ValidateSecurity(entry, BusinessObjects.Permission.CustomerEdit);


            if (!ExistsOmniCustomer(entry, customerID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("CUSTOMERID", (string)customerID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("CUSTOMERID", (string)customerID);
            }

            statement.AddField("USERNAME", userName);
            statement.AddField("PASSWORD", pwd);

            entry.Connection.ExecuteStatement(statement);
        }

        public bool CustomerCanBeDeleted(IConnectionManager entry, RecordIdentifier id)
        {
            if (
                RecordExists(entry, "RBOLOYALTYMSRCARDTABLE", "LOYALTYCUSTID", id) ||
                RecordExists(entry, "RBOLOYALTYTRANS", "LOYALTYCUSTID", id) ||
                RecordExists(entry, "RBOSTORETABLE", "DEFAULTCUSTACCOUNT", id) ||
                RecordExists(entry, "RBOMSRCARDTABLE", "LINKID", id) ||
                RecordExists(entry, "RBOTRANSACTIONLOYALTYPOINTTRANS", "LOYALTYCUSTID", id) ||
                RecordExists(entry, "RBOTRANSACTIONTABLE", "CUSTACCOUNT", id, new[] { "ENTRYSTATUS", "ENTRYSTATUS" }, new RecordIdentifier[] { ((int)TransactionStatus.Voided).ToString(), ((int)TransactionStatus.Training).ToString() }) ||
                RecordExists(entry, "CUSTOMERLEDGERENTRIES", "CUSTOMER", id)

                )
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// Deletes a customer by a given ID
        /// </summary>
        /// <remarks>Edit customer permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the customer to be deleted</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            if (CustomerCanBeDeleted(entry, id))
            {
                //DeleteCustomer(entry, id);
                MarkAsDeleted(entry, "CUSTOMER", "ACCOUNTNUM", id, Permission.CustomerEdit);

            }
            else
            {
                var customer = Get(entry, id, UsageIntentEnum.Minimal);
                customer.Blocked = BlockedEnum.All;
                Save(entry, customer);
            }
        }

        public virtual void DeleteCustomer(IConnectionManager entry, RecordIdentifier id)
        {
            MarkAsDeleted(entry, "CUSTOMER", "ACCOUNTNUM", id, Permission.CustomerEdit);
        }

        public virtual void DeleteAllCustomers(IConnectionManager entry)
        {
            DeleteRecord(entry, "CUSTOMER", new string[0], RecordIdentifier.Empty, BusinessObjects.Permission.CustomerEdit);
            DeleteRecord(entry, "CUSTOMERADDRESS", new string[0], RecordIdentifier.Empty, BusinessObjects.Permission.CustomerEdit);
            entry.Cache.DeleteTypeFromCache(typeof(Name));
            entry.Cache.DeleteTypeFromCache(typeof(Customer));
        }

        /// <summary>
        /// Checks if a customer by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the customer to check for</param>
        /// <returns>True if the customer exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<Customer>(entry, "CUSTOMER", "ACCOUNTNUM", id);
        }

        /// <summary>
        /// Checks if any customer is using a given tax group id.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="taxgroupID">ID of the tax group</param>
        /// <returns>True if any customer uses the tax group, else false</returns>
        public virtual bool TaxGroupExists(IConnectionManager entry, RecordIdentifier taxgroupID)
        {
            return RecordExists(entry, "CUSTOMER", "TAXGROUP", taxgroupID);
        }

        /// <summary>
        /// Updates blocking status for customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customerID">Customer to be updated</param>
        /// <param name="blockingStatus">The blocking status</param>
        public virtual void SaveBlockStatus(IConnectionManager entry, RecordIdentifier customerID, BlockedEnum blockingStatus)
        {
            ValidateSecurity(entry, Permission.CustomerEdit);

            var statement = new SqlServerStatement("CUSTOMER");
            statement.StatementType = StatementType.Update;

            statement.AddCondition("ACCOUNTNUM", (string)customerID);
            statement.AddField("BLOCKED", blockingStatus, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void SaveTaxInformation(IConnectionManager entry, List<RecordIdentifier> customerIDs, TaxExemptEnum taxExempt, RecordIdentifier salesTaxGroupID)
        {
            ValidateSecurity(entry, Permission.CustomerEdit);

            var statement = new SqlServerStatement("CUSTOMER");
            statement.StatementType = StatementType.Update;

            statement.AddCondition("ACCOUNTNUM", "");

            statement.AddField("TAXEXEMPT", (int)taxExempt, SqlDbType.Int);
            statement.AddField("TAXGROUP", "");

            foreach (RecordIdentifier ID in customerIDs)
            {
                statement.UpdateCondition(0, "ACCOUNTNUM", (string)ID);
                statement.UpdateField("TAXEXEMPT", (int)taxExempt);
                statement.UpdateField("TAXGROUP", salesTaxGroupID == null ? RecordIdentifier.Empty.StringValue : (string)salesTaxGroupID);

                entry.Connection.ExecuteStatement(statement);
            }
        }

        public virtual void UpdateTaxExemptInformation(IConnectionManager entry, RecordIdentifier taxExemptTaxGroupID)
        {
            //It is valid to have no tax exempt tax group so we only check if it is null
            if (taxExemptTaxGroupID == null)
            {
                return;
            }

            ValidateSecurity(entry, Permission.CustomerEdit);

            var statement = new SqlServerStatement("CUSTOMER");
            statement.StatementType = StatementType.Update;

            statement.AddCondition("TAXEXEMPT", (int)TaxExemptEnum.Yes, SqlDbType.Int);            
            statement.AddField("TAXGROUP", (string)taxExemptTaxGroupID);
            
            entry.Connection.ExecuteStatement(statement);
            
        }

        /// <summary>
        /// Saves a customer to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <remarks>Edit customer permission is needed to execute this method</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customer">The customer to be saved</param>
        public virtual void Save(IConnectionManager entry, Customer customer)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.CustomerEdit);

            customer.Validate();

            IConnectionManager transaction;
            if (!(entry is IConnectionManagerTransaction))
            {
                // If we already have a transaction then we may not create transactions since SqlServer does not support that.
                transaction = entry.CreateTransaction();
            }
            else
            {
                transaction = entry;
            }

            try
            {
                var statement = new SqlServerStatement("CUSTOMER");
                statement.UpdateColumnOptimizer = customer;

                var isNew = false;

                if (customer.ID == RecordIdentifier.Empty)
                {
                    isNew = true;
                    customer.ID = DataProviderFactory.Instance.GenerateNumber<ICustomerData, Customer>(transaction);
                    customer.MasterID = Guid.NewGuid();
                }

                if (isNew || !Exists(transaction, customer.ID))
                {
                    statement.StatementType = StatementType.Insert;

                    statement.AddKey("DATAAREAID", transaction.Connection.DataAreaId);
                    statement.AddKey("ACCOUNTNUM", (string)customer.ID);

                    //The customer can also be created using a user defined account number (ID) and then we need to create the master ID specifically
                    if (customer.MasterID == null || customer.MasterID == Guid.Empty)
                    {
                        customer.MasterID = Guid.NewGuid();
                    }

                    statement.AddKey("MASTERID", customer.MasterID, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    statement.StatementType = StatementType.Update;

                    statement.AddCondition("DATAAREAID", transaction.Connection.DataAreaId);
                    statement.AddCondition("ACCOUNTNUM", (string)customer.ID);

                    // Restore the master id. This can occur when saving via the Integration Framework where a 3rd party system does not know anything about our master IDs
                    if (customer.MasterID == null || customer.MasterID == Guid.Empty)
                    {
                        customer.MasterID = GetMasterID(transaction, customer.ID, "CUSTOMER", "ACCOUNTNUM");
                    }

                    statement.AddCondition("MASTERID", customer.MasterID, SqlDbType.UniqueIdentifier);
                }

                statement.AddField("NAME", customer.Text);
                statement.AddField("FIRSTNAME", customer.FirstName);
                statement.AddField("MIDDLENAME", customer.MiddleName);
                statement.AddField("LASTNAME", customer.LastName);
                statement.AddField("NAMEPREFIX", customer.Prefix);
                statement.AddField("NAMESUFFIX", customer.Suffix);
                statement.AddField("INVOICEACCOUNT", customer.AccountNumber);
                statement.AddField("CURRENCY", customer.Currency);
                statement.AddField("LANGUAGEID", customer.LanguageCode);
                statement.AddField("TAXGROUP", (string)customer.TaxGroup);
                statement.AddField("PRICEGROUP", (string)customer.PriceGroupID);
                statement.AddField("LINEDISC", (string)customer.LineDiscountID);
                statement.AddField("MULTILINEDISC", (string)customer.MultiLineDiscountID);
                statement.AddField("ENDDISC", (string)customer.FinalDiscountID);
                statement.AddField("CREDITMAX", customer.MaxCredit, SqlDbType.Decimal);
                statement.AddField("ORGID", customer.IdentificationNumber);
                statement.AddField("BLOCKED", (int)customer.Blocked, SqlDbType.Int);
                statement.AddField("NONCHARGABLEACCOUNT", customer.NonChargableAccount ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("INCLTAX", customer.PricesIncludeSalesTax ? 1 : 0, SqlDbType.TinyInt);

                //statement.AddField("LOYALTYCUSTOMER", customer.IsLoyaltyCustomer ? 1 : 0, SqlDbType.TinyInt);

                statement.AddField("PHONE", customer.Telephone);
                statement.AddField("CELLULARPHONE", customer.MobilePhone);
                statement.AddField("NAMEALIAS", customer.SearchName);
                statement.AddField("CUSTGROUP", (string)customer.CustomerGroupID);

                statement.AddField("VATNUM", customer.VatNum);
                statement.AddField("EMAIL", customer.Email);
                statement.AddField("URL", customer.Url);
                statement.AddField("TAXOFFICE", customer.TaxOffice);
                statement.AddField("TAXEXEMPT", (int)customer.TaxExempt, SqlDbType.Int);

                statement.AddField("USEPURCHREQUEST", customer.UsePurchaseRequest ? 1 : 0, SqlDbType.TinyInt);
                statement.AddField("LOCALLYSAVED", customer.LocallySaved ? 1 : 0, SqlDbType.TinyInt);

                statement.AddField("GENDER", (int)customer.Gender, SqlDbType.Int);
                statement.AddField("DATEOFBIRTH", customer.DateOfBirth, SqlDbType.DateTime);

                statement.AddField("RECEIPTOPTION", (int)customer.ReceiptSettings, SqlDbType.Int);
                statement.AddField("RECEIPTEMAIL", customer.ReceiptEmailAddress);

                SaveCustom(entry, statement, customer);

                Save(transaction, customer, statement);

                if (!(entry is IConnectionManagerTransaction))
                {
                    ((IConnectionManagerTransaction)transaction).Commit();
                }
            }
            catch (Exception ex)
            {
                if (!(entry is IConnectionManagerTransaction))
                {
                    ((IConnectionManagerTransaction)transaction).Rollback();
                }

                throw ex;
            }
        }

        /// <summary>
        /// Saves the customer and the addresses assigned to the customer
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="customer">The customer to be saved</param>
        /// <returns>The id of the customer</returns>
        public virtual RecordIdentifier SaveWithAddresses(IConnectionManager entry, Customer customer)
        {
            var originalId = customer.ID;
            Save(entry, customer);

            if (originalId == customer.ID)
            {
                // Delete existing addresses
                Providers.CustomerAddressData.DeleteAll(entry, originalId);
            }

            if (customer.Addresses != null)
            {
                var savedCustomerAddresses = new List<CustomerAddress>();

                foreach (var customerAddress in customer.Addresses)
                {
                    // We have no control on the customer addresses that are passed to this method to be saved,
                    // so make sure we don't save duplicate customer addresses because we cannot differentiate them anyway
                    Func<CustomerAddress, bool> identicalWithCurrentCustomerAddress = x =>
                                                                    x.IsDefault == customerAddress.IsDefault &&
                                                                    x.TaxGroup == customerAddress.TaxGroup &&
                                                                    x.Address == customerAddress.Address;

                    if (customerAddress.Address == null || 
                        customerAddress.Address.IsEmpty || 
                        savedCustomerAddresses.Any(identicalWithCurrentCustomerAddress))
                    {
                        continue;
                    }

                    customerAddress.CustomerID = customer.ID;

                    Providers.CustomerAddressData.Save(entry, customerAddress);

                    savedCustomerAddresses.Add(customerAddress);
                }
            }

            return customer.ID;
        }

        public virtual void SetCustomerCreditLimit(IConnectionManager entry, RecordIdentifier customerID, decimal creditLimit)
        {
            if (customerID == null) return;

            ValidateSecurity(entry, Permission.CustomerEdit);

            SqlServerStatement statement = new SqlServerStatement("CUSTOMER", StatementType.Update);
            
            statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            statement.AddCondition("ACCOUNTNUM", customerID.StringValue);

            statement.AddField("CREDITMAX", creditLimit, SqlDbType.Decimal);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual CustomerPanelInformation GetCustomerPanelInformation(IConnectionManager entry, RecordIdentifier customerID)
        {
            if (RecordIdentifier.IsEmptyOrNull(customerID)) return null;

            CustomerPanelInformation info = null;

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT 
C.ACCOUNTNUM AS CUSTOMERID,
ISNULL(C.CREDITMAX, 0) AS MAXCREDIT,
(ISNULL((SELECT SUM(RemainingAmount) FROM CUSTOMERLEDGERENTRIES WHERE TYPE IN (0,1,2) AND STATUS=1 AND [CUSTOMER] = @CUSTOMERID), 0)) AS BALANCE,
T.TRANSDATE,
T.TRANSAMOUNT,
CO.COAMOUNT,
CO.COPAYMENT
FROM CUSTOMER C
OUTER APPLY(SELECT TOP 1 T.TRANSDATE, T.GROSSAMOUNT * -1 AS TRANSAMOUNT FROM RBOTRANSACTIONTABLE T WHERE T.CUSTACCOUNT = C.ACCOUNTNUM AND T.TYPE = 2 AND T.ENTRYSTATUS = 0 ORDER BY T.TRANSDATE DESC) T
OUTER APPLY(SELECT TOP 1 CO.ORDERXML.value('(/RetailTransaction/netAmountWithTax)[1]', 'nvarchar(50)') AS COAMOUNT, CO.ORDERXML.value('(/RetailTransaction/payment)[1]', 'nvarchar(50)') AS COPAYMENT FROM CUSTOMERORDERS CO WHERE CO.CUSTOMERID = C.ACCOUNTNUM AND CO.STATUS = 4 ORDER BY CO.CREATEDDATE DESC) CO
WHERE ACCOUNTNUM = @CUSTOMERID

SELECT L.CARDNUMBER, SUM(LT.REMAININGPOINTS) AS REMAININGPOINTS, ((SUM(S.QTYAMOUNTLIMIT)/SUM(S.POINTS)) * SUM(LT.REMAININGPOINTS)) AS POINTSVALUE, MAX(LS.USELIMIT) AS USELIMIT FROM RBOLOYALTYMSRCARDTABLE L
INNER JOIN RBOLOYALTYTRANS LT ON L.CARDNUMBER = LT.CARDNUMBER 
INNER JOIN RBOLOYALTYSCHEMESTABLE LS ON LS.LOYALTYSCHEMEID = L.LOYALTYSCHEMEID
CROSS APPLY (SELECT TOP 1 LOYALTYSCHEMEID, QTYAMOUNTLIMIT, POINTS, STARTINGDATE, ENDINGDATE
                       FROM RBOLOYALTYPOINTSTABLE T  
                       JOIN RBOSTORETENDERTYPETABLE SP ON T.SCHEMERELATION = SP.TENDERTYPEID AND T.DATAAREAID = SP.DATAAREAID 
                       WHERE T.LOYALTYSCHEMEID = L.LOYALTYSCHEMEID
                       AND T.TYPE = 5                      
                       AND T.QTYAMOUNTLIMIT > 0
                       AND SP.FUNCTION_ = 1 
                       AND SP.POSOPERATION = 207
					   AND STARTINGDATE <= GETDATE()
					   AND (ENDINGDATE < GETDATE() OR ENDINGDATE < STARTINGDATE)
                       ORDER BY STARTINGDATE, ENDINGDATE) S
WHERE @CUSTOMERID = LT.LOYALTYCUSTID AND LT.ENTRYTYPE = 0 AND LT.TYPE = 0 AND LT.STATUS <> 0 AND L.LINKTYPE = 1 AND L.LINKID = @CUSTOMERID AND L.LOYALTYTENDER IN (0, 1)
GROUP BY L.CARDNUMBER";

                MakeParam(cmd, "CUSTOMERID", customerID.StringValue);

                using (var reader = entry.Connection.ExecuteReader(cmd, CommandType.Text))
                {
                    if(reader.Read()) //Should be 1 record
                    {
                        info = new CustomerPanelInformation((string)reader["CUSTOMERID"]);
                        info.MaxCredit = (decimal)reader["MAXCREDIT"];
                        info.Balance = (decimal)reader["BALANCE"];

                        if(reader["TRANSDATE"] != DBNull.Value)
                        {
                            info.HasTransaction = true;
                            info.LastTransactionDate = new Date((DateTime)reader["TRANSDATE"]);
                            info.LastTransactionTotal = (decimal)reader["TRANSAMOUNT"];
                        }

                        if (reader["COAMOUNT"] != DBNull.Value)
                        {
                            info.HasCustomerOrder = true;
                            info.LastCustomerOrderTotal = Conversion.XmlStringToDecimal((string)reader["COAMOUNT"]);
                            info.LastCustomerOrderPaidDeposit = Conversion.XmlStringToDecimal((string)reader["COPAYMENT"]);
                        }
                    }

                    if(reader.NextResult())
                    {
                        CustomerPanelLoyaltyCard card = null;
                        while(reader.Read())
                        {
                            card = new CustomerPanelLoyaltyCard();
                            card.CardNumber = (string)reader["CARDNUMBER"];
                            card.RemainingPoints = (decimal)reader["REMAININGPOINTS"];
                            card.RemainingValue = (decimal)reader["POINTSVALUE"];
                            card.UsageLimit = (int)reader["USELIMIT"];
                            info.LoyaltyCards.Add(card);
                        }
                    }
                }
            }

            return info;
        }

        #region Custom overrideables
        protected virtual string AddCustomHandling(string sql)
        {
            return sql.Replace(CustomColumns, "").Replace(CustomConditions, "");
        }

        protected virtual void SaveCustom(IConnectionManager entry, SqlServerStatement statement, Customer customer)
        {
        }
        #endregion

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "CUSTOMER"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "CUSTOMER", "ACCOUNTNUM", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items</param>
        /// <returns>List of items</returns>
        public virtual List<Customer> GetCompareList(IConnectionManager entry, List<Customer> itemsToCompare)
        {
            var selectionColumns = GetSelectionColumns();

            selectionColumns.Add(new TableColumn { ColumnName = "ISDEFAULT", TableAlias = "A", IsNull = true });
            selectionColumns.Add(new TableColumn { ColumnName = "ADDRESSTYPE", TableAlias = "A" });
            selectionColumns.Add(new TableColumn { ColumnName = "COUNTY", TableAlias = "A", IsNull = true });

            var joins = new List<Join>();

            joins.Add(new Join
            {
                Condition = "C.ACCOUNTNUM = A.ACCOUNTNUM",
                JoinType = "LEFT OUTER",
                Table = "CUSTOMERADDRESS",
                TableAlias = "A"
            });
            joins.Add(new Join
            {
                Condition = "C.TAXGROUP = TG.TAXGROUP AND C.DATAAREAID = TG.DATAAREAID",
                JoinType = "LEFT OUTER",
                Table = "TAXGROUPHEADING",
                TableAlias = "TG"
            });

            DataPopulator<Customer> populator = (dr, item) =>
            {
                if (AsString(dr["STREET"]) == string.Empty &&
                    AsString(dr["ADDRESS"]) == string.Empty &&
                    AsString(dr["ZIPCODE"]) == string.Empty &&
                    AsString(dr["CITY"]) == string.Empty &&
                    AsString(dr["STATE"]) == string.Empty &&
                    AsString(dr["COUNTRY"]) == string.Empty &&
                    AsString(dr["COUNTY"]) == string.Empty &&
                    AsString(dr["ADDRESSTYPE"]) == string.Empty &&
                    ((int)dr["ADDRESSFORMAT"]) == 0)
                {
                    PopulateCustomer(entry, dr, item, null);
                }
                else
                {
                    PopulateCustomerWithAddress(entry, dr, item, null);
                }
            };

            return GetCompareListInBatches(entry, itemsToCompare, "CUSTOMER", "ACCOUNTNUM", selectionColumns, joins, populator);
        }

        private static List<TableColumn> GetSelectionColumns()
        {
            return new List<TableColumn>
            {
                new TableColumn {ColumnName = "ACCOUNTNUM", TableAlias = "C"},
                new TableColumn {ColumnName = "MASTERID", TableAlias = "C"},
                new TableColumn {ColumnName = "NAME", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "PRICEGROUP", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "LINEDISC", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "ENDDISC", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "BLOCKED", TableAlias = "C", IsNull = true, NullValue = "0"},
                new TableColumn {ColumnName = "CREDITMAX", TableAlias = "C", IsNull = true, NullValue = "0"},
                new TableColumn {ColumnName = "NONCHARGABLEACCOUNT", TableAlias = "C", IsNull = true, NullValue = "0"},
                new TableColumn {ColumnName = "FIRSTNAME", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "MIDDLENAME", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "LASTNAME", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "ADDRESS", TableAlias = "A", IsNull = true},
                new TableColumn {ColumnName = "STREET", TableAlias = "A", IsNull = true},
                new TableColumn {ColumnName = "CITY", TableAlias = "A", IsNull = true},
                new TableColumn {ColumnName = "ZIPCODE", TableAlias = "A", IsNull = true},
                new TableColumn {ColumnName = "STATE", TableAlias = "A", IsNull = true},
                new TableColumn {ColumnName = "COUNTRY", TableAlias = "A", IsNull = true},
                new TableColumn {ColumnName = "ADDRESSFORMAT", TableAlias = "A", IsNull = true},
                new TableColumn {ColumnName = "NAMEPREFIX", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "NAMESUFFIX", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "ORGID", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "NAMEALIAS", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "LOCALLYSAVED", TableAlias = "C"},
                new TableColumn {ColumnName = "CURRENCY", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "LANGUAGEID", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "INVOICEACCOUNT", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "TAXGROUP", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "MULTILINEDISC", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "CUSTGROUP", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "PHONE", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "CELLULARPHONE", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "EMAIL", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "VATNUM", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "TAXEXEMPT", TableAlias = "C", IsNull = true, NullValue = "0"},
                new TableColumn {ColumnName = "INCLTAX", TableAlias = "C", IsNull = true, NullValue = "0"},
                new TableColumn {ColumnName = "RECEIPTOPTION", TableAlias = "C", IsNull = true, NullValue = "0"},
                new TableColumn {ColumnName = "RECEIPTEMAIL", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "GENDER", TableAlias = "C", IsNull = true, NullValue = "0"},
                new TableColumn
                {
                    ColumnName = "DATEOFBIRTH",
                    TableAlias = "C",
                    IsNull = true,
                    NullValue = "'1900-01-01'"
                },
                new TableColumn {ColumnName = "URL", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "TAXOFFICE", TableAlias = "C", IsNull = true},
                new TableColumn {ColumnName = "USEPURCHREQUEST", TableAlias = "C", IsNull = true, NullValue = "0"},
                new TableColumn {ColumnName = "TAXGROUPNAME", TableAlias = "TG", IsNull = true},
                new TableColumn {ColumnName = "DELETED", TableAlias = "C", IsNull = true},
            };
        }
    }
}