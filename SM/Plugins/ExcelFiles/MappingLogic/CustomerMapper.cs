using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.ExcelFiles.Enums;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal class CustomerMapper : MapperBase
    {
        internal static ReceiptSettingsEnum MapReceiptOption(string excelValue)
        {
            switch (excelValue)
            {
                case "Not set":
                    return ReceiptSettingsEnum.Printed;

                case "Print on POS":
                    return ReceiptSettingsEnum.Printed;

                case "Send in email":
                    return ReceiptSettingsEnum.Email;

                case "Both":
                    return ReceiptSettingsEnum.PrintAndEmail;

                default:
                    return ReceiptSettingsEnum.Printed;
            }
        }

        internal static BlockedEnum MapBlocking(string excelValue)
        {
            switch (excelValue)
            {
                case "Not blocked":
                    return BlockedEnum.Nothing;

                case "Limited to invoices":
                    return BlockedEnum.Invoice;

                case "Blocked":
                    return BlockedEnum.All;

                default:
                    return BlockedEnum.Nothing;
            }
        }

        internal static TaxExemptEnum MapTaxExempt(string excelValue)
        {
            switch (excelValue)
            {
                case "True":
                case "1":
                    return TaxExemptEnum.Yes;
                case "0":
                case "False":
                    return TaxExemptEnum.No;

                default:
                    return TaxExemptEnum.No;
            }
        }


        internal static void Import(DataTable dt, List<ImportLogItem> importLogItems, MergeModeEnum mergeMode)
        {

            // Check that mandatory and semi mandatory columns exits
            CheckMandatoryColumns(dt, new string[] { "ID", "NAME" });


            // Start importing
            foreach (DataRow row in dt.Rows)
            {
                int lineNumber = dt.GetRowNumber(row, "Line Number");
                string description = row.GetStringValue("NAME");
                var id = row.GetStringValue("ID");

                if (id == "")
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, Properties.Resources.MandatoryFieldMissing.Replace("#1", "ID"),
                        lineNumber, description));

                    continue;
                }

                var inserting = false;

                try
                {
                    Customer customer;
                    if (Providers.CustomerData.Exists(PluginEntry.DataModel, id))
                    {
                        if (mergeMode == MergeModeEnum.InsertIfNotExists)
                        {
                            continue;
                        }

                        customer = Providers.CustomerData.Get(PluginEntry.DataModel, id, UsageIntentEnum.Normal);
                        customer.Dirty = false;
                    }
                    else
                    {
                        customer = new Customer();
                        customer.ID = id;
                        customer.Dirty = true;
                        inserting = true;
                    }
                    // TODO: Support more than default address
                    CustomerAddress defaultAddress = customer.Addresses.FirstOrDefault(x => x.IsDefault && x.Address.AddressType == Address.AddressTypes.Shipping) ?? new CustomerAddress();
                    if (!customer.Addresses.Contains(defaultAddress))
                    {
                        defaultAddress.ID = Guid.NewGuid();
                        defaultAddress.IsDefault = !customer.Addresses.Any(x => x.IsDefault);

                        customer.Addresses.Add(defaultAddress);
                    }

                    defaultAddress.CustomerID = customer.ID;

                    if (mergeMode == MergeModeEnum.Merge && !inserting)
                    {
                        // We need to do merge, thus only change if something changed
                        if (row["MIDDLENAME"] != DBNull.Value)
                        {
                            if (row.GetStringValue("MIDDLENAME") != customer.Text)
                            {
                                customer.MiddleName = row.GetStringValue("MIDDLENAME");
                                customer.Dirty = true;
                            }
                        }

                        if (row["LASTNAME"] != DBNull.Value)
                        {
                            if (row.GetStringValue("LASTNAME") != customer.Text)
                            {
                                customer.LastName = row.GetStringValue("LASTNAME");
                                customer.Dirty = true;
                            }
                        }

                        if (row["NAMEPREFIX"] != DBNull.Value)
                        {
                            if (row.GetStringValue("NAMEPREFIX") != customer.Text)
                            {
                                customer.Prefix = row.GetStringValue("NAMEPREFIX");
                                customer.Dirty = true;
                            }
                        }

                        if (row["NAMESUFFIX"] != DBNull.Value)
                        {
                            if (row.GetStringValue("NAMESUFFIX") != customer.Text)
                            {
                                customer.Suffix = row.GetStringValue("NAMESUFFIX");
                                customer.Dirty = true;
                            }
                        }

                        if (row["NAME"] != DBNull.Value)
                        {
                            if (customer.MiddleName == "" &&
                                customer.LastName == "" &&
                                customer.Prefix == "" &&
                                customer.Suffix == "")
                            {
                                // If all the other ones are empty, we only want to populate the Text property of the customer
                                if (row.GetStringValue("NAME") != customer.Text)
                                {
                                    customer.Text = row.GetStringValue("NAME");
                                    customer.Dirty = true;
                                }
                            }
                            else
                            {
                                if (row.GetStringValue("NAME") != customer.FirstName)
                                {
                                    customer.FirstName = row.GetStringValue("NAME").Left(31); // The first name field only holds 31 letters
                                    customer.Dirty = true;
                                }

                                // We only want to save the text property of the customer if the Name object says it has changed
                                if (customer.GetFormattedName( PluginEntry.DataModel.Settings.NameFormatter) != customer.Text)
                                {
                                    customer.Text = customer.GetFormattedName(  PluginEntry.DataModel.Settings.NameFormatter);
                                    customer.Dirty = true;
                                }
                            }
                        }

                        if (row["ADDRESSLINE1"] != DBNull.Value)
                        {
                            if (row.GetStringValue("ADDRESSLINE1") != defaultAddress.Address.Address1)
                            {
                                defaultAddress.Address.Address1 = row.GetStringValue("ADDRESSLINE1");
                                customer.Dirty = true;
                            }
                        }

                        if (row["ADDRESSLINE2"] != DBNull.Value)
                        {
                            if (row.GetStringValue("ADDRESSLINE2") != defaultAddress.Address.Address2)
                            {
                                defaultAddress.Address.Address2 = row.GetStringValue("ADDRESSLINE2");
                                customer.Dirty = true;
                            }
                        }

                        if (row["CITY"] != DBNull.Value)
                        {
                            if (row.GetStringValue("CITY") != defaultAddress.Address.City)
                            {
                                defaultAddress.Address.City = row.GetStringValue("CITY");
                                customer.Dirty = true;
                            }
                        }

                        if (row["STATE"] != DBNull.Value)
                        {
                            if (row.GetStringValue("STATE") != defaultAddress.Address.State)
                            {
                                defaultAddress.Address.State = row.GetStringValue("STATE");
                                customer.Dirty = true;
                            }
                        }

                        if (row["ZIP"] != DBNull.Value)
                        {
                            if (row.GetStringValue("ZIP") != defaultAddress.Address.Zip)
                            {
                                defaultAddress.Address.Zip = row.GetStringValue("ZIP");
                                customer.Dirty = true;
                            }
                        }

                        
                        if (row["COUNTRY"] != DBNull.Value)
                        {
                            if (MapCountryCode(row.GetStringValue("COUNTRY")) != defaultAddress.Address.Country)
                            {
                                defaultAddress.Address.Country = MapCountryCode(row.GetStringValue("COUNTRY"));
                                customer.Dirty = true;
                            }
                        }

                        if (row["ADDRESS FORMAT"] != DBNull.Value)
                        {
                            if (MapAddressFormat(row.GetStringValue("ADDRESS FORMAT")) != defaultAddress.Address.AddressFormat)
                            {
                                defaultAddress.Address.AddressFormat = MapAddressFormat(row.GetStringValue("ADDRESS FORMAT"));
                                customer.Dirty = true;
                            }
                        }

                        if (row["SEARCH ALIAS"] != DBNull.Value)
                        {
                            if (row.GetStringValue("SEARCH ALIAS") != customer.SearchName)
                            {
                                customer.SearchName = row.GetStringValue("SEARCH ALIAS");
                                customer.Dirty = true;
                            }
                        }

                        if (row["PHONE"] != DBNull.Value)
                        {
                            if (row.GetStringValue("PHONE") != customer.Telephone)
                            {
                                customer.Telephone = row.GetStringValue("PHONE");
                                customer.Dirty = true;
                            }
                        }

                        if (row["MOBILE PHONE"] != DBNull.Value)
                        {
                            if (row.GetStringValue("MOBILE PHONE") != customer.MobilePhone)
                            {
                                customer.MobilePhone = row.GetStringValue("MOBILE PHONE");
                                customer.Dirty = true;
                            }
                        }

                        if (row["EMAIL ADDRESS"] != DBNull.Value)
                        {
                            if (row.GetStringValue("EMAIL ADDRESS") != customer.Email)
                            {
                                customer.Email = row.GetStringValue("EMAIL ADDRESS");
                                customer.Dirty = true;
                            }
                        }

                        if (row["RECEIPT EMAIL"] != DBNull.Value)
                        {
                            if (row.GetStringValue("RECEIPT EMAIL") != customer.ReceiptEmailAddress)
                            {
                                customer.ReceiptEmailAddress = row.GetStringValue("RECEIPT EMAIL");
                                customer.Dirty = true;
                            }
                        }


                        if (row["ACCOUNT NUMBER"] != DBNull.Value)
                        {
                            if (row.GetStringValue("ACCOUNT NUMBER") != customer.AccountNumber)
                            {
                                customer.AccountNumber = row.GetStringValue("ACCOUNT NUMBER");
                                customer.Dirty = true;
                            }
                        }

                        if (row["IDENTIFICATION NUMBER"] != DBNull.Value)
                        {
                            if (row.GetStringValue("IDENTIFICATION NUMBER") != customer.IdentificationNumber)
                            {
                                customer.IdentificationNumber = row.GetStringValue("IDENTIFICATION NUMBER");
                                customer.Dirty = true;
                            }
                        }

                        if (row["RECEIPT OPTION"] != DBNull.Value)
                        {
                            if (MapReceiptOption(row.GetStringValue("RECEIPT OPTION")) != customer.ReceiptSettings)
                            {
                                customer.ReceiptSettings = MapReceiptOption(row.GetStringValue("RECEIPT OPTION"));
                                customer.Dirty = true;
                            }
                        }

                        if (row["LANGUAGE CODE"] != DBNull.Value)
                        {
                            if (MapLanguageCode(row.GetStringValue("LANGUAGE CODE")) != customer.LanguageCode)
                            {
                                customer.LanguageCode = MapLanguageCode(row.GetStringValue("LANGUAGE CODE"));
                                customer.Dirty = true;
                            }
                        }

                        if (row["CURRENCY"] != DBNull.Value)
                        {
                            if (row.GetStringValue("CURRENCY") != customer.Currency)
                            {
                                customer.Currency = row.GetStringValue("CURRENCY");
                                customer.Dirty = true;
                            }
                        }

                        if (row["VAT NUMBER"] != DBNull.Value)
                        {
                            if (row.GetStringValue("VAT NUMBER") != customer.VatNum)
                            {
                                customer.VatNum = row.GetStringValue("VAT NUMBER");
                                customer.Dirty = true;
                            }   
                        }

                        if (row["SALES TAX GROUP"] != DBNull.Value)
                        {
                            if (row.GetStringValue("SALES TAX GROUP") != customer.TaxGroup)
                            {
                                customer.TaxGroup = row.GetStringValue("SALES TAX GROUP");
                                customer.Dirty = true;
                            }   
                        }

                        if (row["MAXIMUM WITHDRAWAL"] != DBNull.Value)
                        {
                            if (row.GetDecimalValue("MAXIMUM WITHDRAWAL") != customer.MaxCredit)
                            {
                                customer.MaxCredit = row.GetDecimalValue("MAXIMUM WITHDRAWAL");
                                customer.Dirty = true;
                            }   
                        }

                        if (row["BLOCKING"] != DBNull.Value)
                        {
                            if (MapBlocking(row.GetStringValue("BLOCKING")) != customer.Blocked)
                            {
                                customer.Blocked = MapBlocking(row.GetStringValue("BLOCKING"));
                                customer.Dirty = true;
                            }
                        }

                        if (row["CASH CUSTOMER"] != DBNull.Value)
                        {
                            if ((row.GetIntegerValue("CASH CUSTOMER") == 1) != customer.NonChargableAccount)
                            {
                                customer.NonChargableAccount = (row.GetIntegerValue("CASH CUSTOMER") == 1);
                                customer.Dirty = true;
                            }   
                        }

                        if (row["TAX EXEMPT"] != DBNull.Value)
                        {
                            if (MapTaxExempt(row.GetStringValue("TAX EXEMPT")) != customer.TaxExempt)
                            {
                                customer.TaxExempt = MapTaxExempt(row.GetStringValue("TAX EXEMPT"));
                                customer.Dirty = true;
                            }
                        }

                    }
                    else
                    {
                        // We just do dumb insert since we are either inserting new or in overide mode
                        if (!CheckMandatoryFields(dt, row, id, new string[] { "NAME" }, inserting, importLogItems, lineNumber, description))
                        {
                            continue;
                        }


                        if (row["MIDDLENAME"] != DBNull.Value)
                        {
                            if (row.GetStringValue("MIDDLENAME") != customer.Text)
                            {
                                customer.MiddleName = row.GetStringValue("MIDDLENAME");
                                customer.Dirty = true;
                            }
                        }

                        if (row["LASTNAME"] != DBNull.Value)
                        {
                            if (row.GetStringValue("LASTNAME") != customer.Text)
                            {
                                customer.LastName = row.GetStringValue("LASTNAME");
                                customer.Dirty = true;
                            }
                        }

                        if (row["NAMEPREFIX"] != DBNull.Value)
                        {
                            if (row.GetStringValue("NAMEPREFIX") != customer.Text)
                            {
                                customer.Prefix = row.GetStringValue("NAMEPREFIX");
                                customer.Dirty = true;
                            }
                        }

                        if (row["NAMESUFFIX"] != DBNull.Value)
                        {
                            if (row.GetStringValue("NAMESUFFIX") != customer.Text)
                            {
                                customer.Suffix = row.GetStringValue("NAMESUFFIX");
                                customer.Dirty = true;
                            }
                        }

                        if (row["NAME"] != DBNull.Value)
                        {
                            if (customer.MiddleName == "" &&
                                customer.LastName == "" &&
                                customer.Prefix == "" &&
                                customer.Suffix == "")
                            {
                                // If all the other ones are empty, we only want to populate the Text property of the customer
                                customer.Text = row.GetStringValue("NAME").Left(31); // The first name field only holds 31 letters
                                customer.Dirty = true;
                            }
                            else
                            {
                                customer.FirstName = row.GetStringValue("NAME");
                                customer.Text = customer.GetFormattedName(PluginEntry.DataModel.Settings.NameFormatter);
                                customer.Dirty = true;
                            }
                        }



                        if (row["ADDRESSLINE1"] != DBNull.Value)
                        {
                            defaultAddress.Address.Address1 = row.GetStringValue("ADDRESSLINE1");
                            customer.Dirty = true;
                        }

                        if (row["ADDRESSLINE2"] != DBNull.Value)
                        {
                            defaultAddress.Address.Address2 = row.GetStringValue("ADDRESSLINE2");
                            customer.Dirty = true;
                        }

                        if (row["CITY"] != DBNull.Value)
                        {
                            defaultAddress.Address.City = row.GetStringValue("CITY");
                            customer.Dirty = true;
                        }

                        if (row["STATE"] != DBNull.Value)
                        {
                            defaultAddress.Address.State = row.GetStringValue("STATE");
                            customer.Dirty = true;
                        }

                        if (row["ZIP"] != DBNull.Value)
                        {
                            defaultAddress.Address.Zip = row.GetStringValue("ZIP");
                            customer.Dirty = true;
                        }

                        if (row["COUNTRY"] != DBNull.Value)
                        {
                            defaultAddress.Address.Country = MapCountryCode(row.GetStringValue("COUNTRY"));
                            customer.Dirty = true;
                        }

                        if (row["ADDRESS FORMAT"] != DBNull.Value)
                        {
                            defaultAddress.Address.AddressFormat = MapAddressFormat(row.GetStringValue("ADDRESS FORMAT"));
                            customer.Dirty = true;
                        }

                        if (row["SEARCH ALIAS"] != DBNull.Value)
                        {
                            customer.SearchName = row.GetStringValue("SEARCH ALIAS");
                            customer.Dirty = true;
                        }

                        if (row["PHONE"] != DBNull.Value)
                        {
                            customer.Telephone = row.GetStringValue("PHONE");
                            customer.Dirty = true;
                        }

                        if (row["MOBILE PHONE"] != DBNull.Value)
                        {
                            customer.MobilePhone = row.GetStringValue("MOBILE PHONE");
                            customer.Dirty = true;
                        }

                        if (row["EMAIL ADDRESS"] != DBNull.Value)
                        {
                            customer.Email = row.GetStringValue("EMAIL ADDRESS");
                            customer.Dirty = true;
                        }

                        if (row["RECEIPT EMAIL"] != DBNull.Value)
                        {
                            customer.ReceiptEmailAddress = row.GetStringValue("RECEIPT EMAIL");
                            customer.Dirty = true;
                        }

                        if (row["ACCOUNT NUMBER"] != DBNull.Value)
                        {
                            customer.AccountNumber = row.GetStringValue("ACCOUNT NUMBER");
                            customer.Dirty = true;
                        }

                        if (row["IDENTIFICATION NUMBER"] != DBNull.Value)
                        {
                            customer.IdentificationNumber = row.GetStringValue("IDENTIFICATION NUMBER");
                            customer.Dirty = true;
                        }

                        if (row["RECEIPT OPTION"] != DBNull.Value)
                        {
                            customer.ReceiptSettings = MapReceiptOption(row.GetStringValue("RECEIPT OPTION"));
                            customer.Dirty = true;
                        }

                        if (row["LANGUAGE CODE"] != DBNull.Value)
                        {
                            customer.LanguageCode = MapLanguageCode(row.GetStringValue("LANGUAGE CODE"));
                            customer.Dirty = true;
                        }

                        if (row["CURRENCY"] != DBNull.Value)
                        {
                            customer.Currency = row.GetStringValue("CURRENCY");
                            customer.Dirty = true;
                        }

                        if (row["VAT NUMBER"] != DBNull.Value)
                        {
                            customer.VatNum = row.GetStringValue("VAT NUMBER");
                            customer.Dirty = true;
                        }

                        if (row["SALES TAX GROUP"] != DBNull.Value)
                        {
                            customer.TaxGroup = row.GetStringValue("SALES TAX GROUP");
                            customer.Dirty = true;
                        }

                        if (row["MAXIMUM WITHDRAWAL"] != DBNull.Value)
                        {                          
                            customer.MaxCredit = row.GetDecimalValue("MAXIMUM WITHDRAWAL");
                            customer.Dirty = true;     
                        }

                        if (row["BLOCKING"] != DBNull.Value)
                        {
                            customer.Blocked = MapBlocking(row.GetStringValue("BLOCKING"));
                            customer.Dirty = true;
                        }

                        if (row["CASH CUSTOMER"] != DBNull.Value)
                        {
                            customer.NonChargableAccount = (row.GetIntegerValue("CASH CUSTOMER") == 1);
                            customer.Dirty = true;
                        }

                        if (row["TAX EXEMPT"] != DBNull.Value)
                        {
                            customer.TaxExempt = MapTaxExempt(row.GetStringValue("TAX EXEMPT"));
                            customer.Dirty = true;
                        }

                    }

                    // Post processing
                    // ------------------------------------------------------------------------------------
                    if (customer.Currency != "")
                    {
                        if (!Providers.CurrencyData.Exists(PluginEntry.DataModel, customer.Currency))
                        {
                            Currency currency = new Currency(customer.Currency, customer.Currency);

                            try
                            {
                                Providers.CurrencyData.Save(PluginEntry.DataModel, currency);
                            }
                            catch (Exception ex)
                            {
                                importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, ex.Message, lineNumber, description));
                                continue;
                            }
                        }
                    }

                    if (customer.TaxGroup != "")
                    {
                        if (!Providers.SalesTaxGroupData.Exists(PluginEntry.DataModel, customer.TaxGroup))
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, Properties.Resources.TaxGroupNotFound,
                                lineNumber, description));
                            continue;
                        }
                    }

                    // ------------------------------------------------------------------------------------

                    // Saving
                    // ------------------------------------------------------------------------------------
                    if (customer.Dirty)
                    {    
                        Providers.CustomerData.Save(PluginEntry.DataModel, customer);
                        Providers.CustomerAddressData.Save(PluginEntry.DataModel,defaultAddress);    
                        importLogItems.Add(new ImportLogItem(inserting ? ImportAction.Inserted : ImportAction.Updated, dt.TableName, id, "",
                            lineNumber, description));
                    }
                    
                }
                catch (Exception ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, ex.Message));
                    continue;
                }
            }
        }
    }
}
