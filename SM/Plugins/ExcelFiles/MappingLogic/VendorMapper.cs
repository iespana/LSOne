using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.ExcelFiles.Enums;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal class VendorMapper : MapperBase
    {
        internal static TypeOfContactEnum MapContactType(string excelValue)
        {
            switch (excelValue)
            {
                case "Person":
                    return TypeOfContactEnum.Person;

                case "Company":
                    return TypeOfContactEnum.Company;

                default:
                    return TypeOfContactEnum.Person;
            }
        }

        internal static void Import(DataTable dt, List<ImportLogItem> importLogItems, MergeModeEnum mergeMode)
        {
            Vendor vendor;
            Contact contact;
            RecordIdentifier id;
            bool inserting;
            bool contactDirty;

            // Check that mandatory and semi mandatory columns exits
            CheckMandatoryColumns(dt, new string[] { "VENDORID", "DESCRIPTION","CURRENCY" });


            // Start importing
            foreach (DataRow row in dt.Rows)
            {
                int lineNumber = dt.GetRowNumber(row, "Line Number");
                id = row.GetStringValue("VENDORID");
                string description = row.GetStringValue("DESCRIPTION");

                if (id == "")
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, Properties.Resources.MandatoryFieldMissing.Replace("#1", "ID"),
                        lineNumber, description));

                    continue;
                }

                inserting = false;
                contact = null;
                contactDirty = false;

                try
                {

                    if (Providers.VendorData.Exists(PluginEntry.DataModel, id))
                    {
                        if (mergeMode == MergeModeEnum.InsertIfNotExists)
                        {
                            continue;
                        }

                        vendor = Providers.VendorData.GetVendor(PluginEntry.DataModel, id);
                        vendor.Dirty = false;

                        if(vendor.Deleted)
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, Properties.Resources.DeletedVendorCannotBeImported,
                                lineNumber, description));

                            continue;
                        }
                    }
                    else
                    {
                        vendor = new Vendor();
                        vendor.ID = id;
                        vendor.Dirty = true;
                        inserting = true;
                    }

                    if (mergeMode == MergeModeEnum.Merge && !inserting)
                    {
                        // We need to do merge, thus only change if something changed
                        if (row["DESCRIPTION"] != DBNull.Value)
                        {
                            if (row.GetStringValue("DESCRIPTION") != vendor.Text)
                            {
                                vendor.Text = row.GetStringValue("DESCRIPTION");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["ADDRESSLINE1"] != DBNull.Value)
                        {
                            if (row.GetStringValue("ADDRESSLINE1") != vendor.Address1)
                            {
                                vendor.Address1 = row.GetStringValue("ADDRESSLINE1");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["ADDRESSLINE2"] != DBNull.Value)
                        {
                            if (row.GetStringValue("ADDRESSLINE2") != vendor.Address2)
                            {
                                vendor.Address2 = row.GetStringValue("ADDRESSLINE2");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["CITY"] != DBNull.Value)
                        {
                            if (row.GetStringValue("CITY") != vendor.City)
                            {
                                vendor.City = row.GetStringValue("CITY");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["STATE"] != DBNull.Value)
                        {
                            if (row.GetStringValue("STATE") != vendor.State)
                            {
                                vendor.State = row.GetStringValue("STATE");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["ZIP"] != DBNull.Value)
                        {
                            if (row.GetStringValue("ZIP") != vendor.ZipCode)
                            {
                                vendor.ZipCode = row.GetStringValue("ZIP");
                                vendor.Dirty = true;
                            }
                        }


                        if (row["COUNTRY"] != DBNull.Value)
                        {
                            if (MapCountryCode(row.GetStringValue("COUNTRY")) != vendor.Country)
                            {
                                vendor.Country = MapCountryCode(row.GetStringValue("COUNTRY"));
                                vendor.Dirty = true;
                            }
                        }

                        if (row["ADDRESS FORMAT"] != DBNull.Value)
                        {
                            if (MapAddressFormat(row.GetStringValue("ADDRESS FORMAT")) != vendor.AddressFormat)
                            {
                                vendor.AddressFormat = MapAddressFormat(row.GetStringValue("ADDRESS FORMAT"));
                                vendor.Dirty = true;
                            }
                        }

                        if (row["CURRENCY"] != DBNull.Value)
                        {
                            if (row.GetStringValue("CURRENCY") != vendor.CurrencyID)
                            {
                                vendor.CurrencyID = row.GetStringValue("CURRENCY");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["PHONE"] != DBNull.Value)
                        {
                            if (row.GetStringValue("PHONE") != vendor.Phone)
                            {
                                vendor.Phone = row.GetStringValue("PHONE");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["EMAIL ADDRESS"] != DBNull.Value)
                        {
                            if (row.GetStringValue("EMAIL ADDRESS") != vendor.Email)
                            {
                                vendor.Email = row.GetStringValue("EMAIL ADDRESS");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["FAX"] != DBNull.Value)
                        {
                            if (row.GetStringValue("FAX") != vendor.Fax)
                            {
                                vendor.Fax = row.GetStringValue("FAX");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["LANGUAGE CODE"] != DBNull.Value)
                        {
                            if (MapLanguageCode(row.GetStringValue("LANGUAGE CODE")) != vendor.LanguageID)
                            {
                                vendor.LanguageID = MapLanguageCode(row.GetStringValue("LANGUAGE CODE"));
                                vendor.Dirty = true;
                            }
                        }

                        if (row["NOTES"] != DBNull.Value)
                        {
                            if (row.GetStringValue("NOTES") != vendor.LongDescription)
                            {
                                vendor.LongDescription = row.GetStringValue("NOTES");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["TAX GROUP"] != DBNull.Value)
                        {
                            if (row.GetStringValue("TAX GROUP") != vendor.TaxGroup)
                            {
                                vendor.TaxGroup = row.GetStringValue("TAX GROUP");
                                vendor.Dirty = true;
                            }
                        }

                        if (row["CONTACT TYPE"] != DBNull.Value || row["CONTACT COMPANYNAME"] != DBNull.Value
                         || row["CONTACT FIRSTNAME"] != DBNull.Value || row["CONTACT MIDDLENAME"] != DBNull.Value
                         || row["CONTACT LASTNAME"] != DBNull.Value || row["CONTACT NAMEPREFIX"] != DBNull.Value
                         || row["CONTACT NAMESUFFIX"] != DBNull.Value || row["CONTACT ADDRESSLINE1"] != DBNull.Value
                         || row["CONTACT ADDRESSLINE2"] != DBNull.Value || row["CONTACT CITY"] != DBNull.Value
                         || row["CONTACT STATE"] != DBNull.Value || row["CONTACT ZIP"] != DBNull.Value
                         || row["CONTACT COUNTRY"] != DBNull.Value || row["CONTACT ADDRESS FORMAT"] != DBNull.Value
                         || row["CONTACT PHONE"] != DBNull.Value || row["CONTACT PHONE2"] != DBNull.Value
                         || row["CONTACT FAX"] != DBNull.Value)
                        {
                            if (vendor.DefaultContactID != "")
                            {
                                contact = Providers.ContactData.Get(PluginEntry.DataModel,vendor.DefaultContactID);
                            }

                            if (contact == null)
                            {
                                contact = new Contact();
                            }

                            if (row["CONTACT TYPE"] != DBNull.Value)
                            {
                                if (MapContactType(row.GetStringValue("CONTACT TYPE")) != contact.ContactType)
                                {
                                    contact.ContactType = MapContactType(row.GetStringValue("CONTACT TYPE"));
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT COMPANYNAME"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT COMPANYNAME") != contact.CompanyName)
                                {
                                    contact.CompanyName = row.GetStringValue("CONTACT COMPANYNAME");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT NAMEPREFIX"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT NAMEPREFIX") != contact.Name.Prefix)
                                {
                                    contact.Name.Prefix = row.GetStringValue("CONTACT NAMEPREFIX");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT FIRSTNAME"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT FIRSTNAME") != contact.Name.First)
                                {
                                    contact.Name.First = row.GetStringValue("CONTACT FIRSTNAME");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT MIDDLENAME"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT MIDDLENAME") != contact.Name.Middle)
                                {
                                    contact.Name.Middle = row.GetStringValue("CONTACT MIDDLENAME");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT LASTNAME"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT LASTNAME") != contact.Name.Last)
                                {
                                    contact.Name.Last = row.GetStringValue("CONTACT LASTNAME");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT NAMESUFFIX"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT NAMESUFFIX") != contact.Name.Last)
                                {
                                    contact.Name.Suffix = row.GetStringValue("CONTACT NAMESUFFIX");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT ADDRESSLINE1"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT ADDRESSLINE1") != contact.Address.Address1)
                                {
                                    contact.Address.Address2 = row.GetStringValue("CONTACT ADDRESSLINE1");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT ADDRESSLINE2"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT ADDRESSLINE2") != contact.Address.Address2)
                                {
                                    contact.Address.Address2 = row.GetStringValue("CONTACT ADDRESSLINE2");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT CITY"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT CITY") != contact.Address.City)
                                {
                                    contact.Address.City = row.GetStringValue("CONTACT CITY");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT STATE"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT STATE") != contact.Address.State)
                                {
                                    contact.Address.State = row.GetStringValue("CONTACT STATE");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT ZIP"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT ZIP") != contact.Address.Zip)
                                {
                                    contact.Address.Zip = row.GetStringValue("CONTACT ZIP");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT COUNTRY"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT COUNTRY") != contact.Address.Country)
                                {
                                    contact.Address.Country = row.GetStringValue("CONTACT COUNTRY");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT ADDRESS FORMAT"] != DBNull.Value)
                            {
                                if (MapAddressFormat(row.GetStringValue("CONTACT ADDRESS FORMAT")) != contact.Address.AddressFormat)
                                {
                                    contact.Address.AddressFormat = MapAddressFormat(row.GetStringValue("CONTACT ADDRESS FORMAT"));
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT PHONE"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT PHONE") != contact.Phone)
                                {
                                    contact.Phone = row.GetStringValue("CONTACT PHONE");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT PHONE2"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT PHONE2") != contact.Phone2)
                                {
                                    contact.Phone2 = row.GetStringValue("CONTACT PHONE2");
                                    contactDirty = true;
                                }
                            }

                            if (row["CONTACT FAX"] != DBNull.Value)
                            {
                                if (row.GetStringValue("CONTACT FAX") != contact.Phone2)
                                {
                                    contact.Fax = row.GetStringValue("CONTACT FAX");
                                    contactDirty = true;
                                }
                            }
                        }
                        

                    }
                    else
                    {
                        // We just do dumb insert since we are either inserting new or in overide mode
                        if (!CheckMandatoryFields(dt, row, id, new string[] { "DESCRIPTION","CURRENCY" }, inserting, importLogItems, lineNumber, description))
                        {
                            continue;
                        }

                        if (row["DESCRIPTION"] != DBNull.Value)
                        {
                            vendor.Text = row.GetStringValue("DESCRIPTION");
                            vendor.Dirty = true;
                        }

                        if (row["ADDRESSLINE1"] != DBNull.Value)
                        {
                            vendor.Address1 = row.GetStringValue("ADDRESSLINE1");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.Address1 = "";
                            vendor.Dirty = true;
                        }

                        if (row["ADDRESSLINE2"] != DBNull.Value)
                        {
                            vendor.Address2 = row.GetStringValue("ADDRESSLINE2");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.Address2 = "";
                            vendor.Dirty = true;
                        }

                        if (row["CITY"] != DBNull.Value)
                        {
                            vendor.City = row.GetStringValue("CITY");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.City = "";
                            vendor.Dirty = true;
                        }

                        if (row["STATE"] != DBNull.Value)
                        {
                            vendor.State = row.GetStringValue("STATE");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.State = "";
                            vendor.Dirty = true;
                        }

                        if (row["ZIP"] != DBNull.Value)
                        {
                            vendor.ZipCode = row.GetStringValue("ZIP");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.ZipCode = "";
                            vendor.Dirty = true;
                        }

                        if (row["COUNTRY"] != DBNull.Value)
                        {
                            vendor.Country = MapCountryCode(row.GetStringValue("COUNTRY"));
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.Country = RecordIdentifier.Empty;
                            vendor.Dirty = true;
                        }

                        if (row["ADDRESS FORMAT"] != DBNull.Value)
                        {
                            vendor.AddressFormat = MapAddressFormat(row.GetStringValue("ADDRESS FORMAT"));
                            vendor.Dirty = true;
                        }

                        if (row["CURRENCY"] != DBNull.Value)
                        {
                            vendor.CurrencyID = row.GetStringValue("CURRENCY");
                            vendor.Dirty = true;
                        }

                        if (row["PHONE"] != DBNull.Value)
                        {
                            vendor.Phone = row.GetStringValue("PHONE");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.Phone = "";
                            vendor.Dirty = true;
                        }

                        if (row["EMAIL ADDRESS"] != DBNull.Value)
                        {
                            vendor.Email = row.GetStringValue("EMAIL ADDRESS");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.Email = "";
                            vendor.Dirty = true;
                        }

                        if (row["FAX"] != DBNull.Value)
                        {
                            vendor.Fax = row.GetStringValue("FAX");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.Fax = "";
                            vendor.Dirty = true;
                        }

                        if (row["LANGUAGE CODE"] != DBNull.Value)
                        {
                            vendor.LanguageID = MapLanguageCode(row.GetStringValue("LANGUAGE CODE"));
                            vendor.Dirty = true;
                        }

                        if (row["NOTES"] != DBNull.Value)
                        {
                            vendor.LongDescription = row.GetStringValue("NOTES");
                            vendor.Dirty = true;
                        }
                        else
                        {
                            vendor.LongDescription = "";
                            vendor.Dirty = true;
                        }

                        if (row["TAX GROUP"] != DBNull.Value)
                        {
                            vendor.TaxGroup = row.GetStringValue("TAX GROUP");
                            vendor.Dirty = true;
                        }

                        if (row["CONTACT TYPE"] != DBNull.Value || row["CONTACT COMPANYNAME"] != DBNull.Value
                         || row["CONTACT FIRSTNAME"] != DBNull.Value || row["CONTACT MIDDLENAME"] != DBNull.Value
                         || row["CONTACT LASTNAME"] != DBNull.Value || row["CONTACT NAMEPREFIX"] != DBNull.Value
                         || row["CONTACT NAMESUFFIX"] != DBNull.Value || row["CONTACT ADDRESSLINE1"] != DBNull.Value
                         || row["CONTACT ADDRESSLINE2"] != DBNull.Value || row["CONTACT CITY"] != DBNull.Value
                         || row["CONTACT STATE"] != DBNull.Value || row["CONTACT ZIP"] != DBNull.Value
                         || row["CONTACT COUNTRY"] != DBNull.Value || row["CONTACT ADDRESS FORMAT"] != DBNull.Value
                         || row["CONTACT PHONE"] != DBNull.Value || row["CONTACT PHONE2"] != DBNull.Value
                         || row["CONTACT FAX"] != DBNull.Value)
                        {
                            contact = new Contact();

                            if (row["CONTACT TYPE"] != DBNull.Value)
                            {
                                contact.ContactType = MapContactType(row.GetStringValue("CONTACT TYPE"));
                                contactDirty = true;
                            }

                            if (row["CONTACT COMPANYNAME"] != DBNull.Value)
                            {
                                contact.CompanyName = row.GetStringValue("CONTACT COMPANYNAME");
                                contactDirty = true;
                            }

                            if (row["CONTACT NAMEPREFIX"] != DBNull.Value)
                            {                                
                                contact.Name.Prefix = row.GetStringValue("CONTACT NAMEPREFIX");
                                contactDirty = true;
                            }

                            if (row["CONTACT FIRSTNAME"] != DBNull.Value)
                            {                                
                                contact.Name.First = row.GetStringValue("CONTACT FIRSTNAME");
                                contactDirty = true;
                            }

                            if (row["CONTACT MIDDLENAME"] != DBNull.Value)
                            {
                                contact.Name.Middle = row.GetStringValue("CONTACT MIDDLENAME");
                                contactDirty = true;
                            }

                            if (row["CONTACT LASTNAME"] != DBNull.Value)
                            { 
                                contact.Name.Last = row.GetStringValue("CONTACT LASTNAME");
                                contactDirty = true;
                            }

                            if (row["CONTACT NAMESUFFIX"] != DBNull.Value)
                            {
                                contact.Name.Suffix = row.GetStringValue("CONTACT NAMESUFFIX");
                                contactDirty = true; 
                            }

                            if (row["CONTACT ADDRESSLINE1"] != DBNull.Value)
                            {
                                contact.Address.Address2 = row.GetStringValue("CONTACT ADDRESSLINE1");
                                contactDirty = true;
                            }

                            if (row["CONTACT ADDRESSLINE2"] != DBNull.Value)
                            {
                                contact.Address.Address2 = row.GetStringValue("CONTACT ADDRESSLINE2");
                                contactDirty = true;
                            }

                            if (row["CONTACT CITY"] != DBNull.Value)
                            {
                                contact.Address.City = row.GetStringValue("CONTACT CITY");
                                contactDirty = true;
                            }

                            if (row["CONTACT STATE"] != DBNull.Value)
                            {
                                contact.Address.State = row.GetStringValue("CONTACT STATE");
                                contactDirty = true;
                            }

                            if (row["CONTACT ZIP"] != DBNull.Value)
                            {
                                contact.Address.Zip = row.GetStringValue("CONTACT ZIP");
                                contactDirty = true;
                            }

                            if (row["CONTACT COUNTRY"] != DBNull.Value)
                            {
                                contact.Address.Country = row.GetStringValue("CONTACT COUNTRY");
                                contactDirty = true;
                            }

                            if (row["CONTACT ADDRESS FORMAT"] != DBNull.Value)
                            {
                                contact.Address.AddressFormat = MapAddressFormat(row.GetStringValue("CONTACT ADDRESS FORMAT"));
                                contactDirty = true;
                            }

                            if (row["CONTACT PHONE"] != DBNull.Value)
                            {
                                contact.Phone = row.GetStringValue("CONTACT PHONE");
                                contactDirty = true;
                            }

                            if (row["CONTACT PHONE2"] != DBNull.Value)
                            {
                                contact.Phone2 = row.GetStringValue("CONTACT PHONE2");
                                contactDirty = true;
                            }

                            if (row["CONTACT FAX"] != DBNull.Value)
                            {                                
                                contact.Fax = row.GetStringValue("CONTACT FAX");
                                contactDirty = true;
                            }
                        }

                    }

                    // Post processing
                    // ------------------------------------------------------------------------------------
                    if (vendor.CurrencyID != "")
                    {
                        if (!Providers.CurrencyData.Exists(PluginEntry.DataModel, vendor.CurrencyID))
                        {
                            Currency currency = new Currency((string)vendor.CurrencyID, (string)vendor.CurrencyID);

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

                    if (vendor.TaxGroup != "")
                    {
                        if (!Providers.SalesTaxGroupData.Exists(PluginEntry.DataModel, vendor.TaxGroup))
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, id, Properties.Resources.TaxGroupNotFound,
                                lineNumber, description));
                            continue;
                        }
                    }

                    // ------------------------------------------------------------------------------------

                    // Saving
                    // ------------------------------------------------------------------------------------
                    if (contact != null && contactDirty)
                    {
                        if (contact.ID == RecordIdentifier.Empty && vendor.ID != RecordIdentifier.Empty)
                        {
                            Vendor tmp = Providers.VendorData.Get(PluginEntry.DataModel, vendor.ID);

                            if (tmp != null)
                            {
                                if (tmp.DefaultContactID != "")
                                {
                                    contact.ID = tmp.DefaultContactID;
                                }
                            }
                        }

                        contact.OwnerID = vendor.ID;
                        contact.OwnerType = ContactRelationTypeEnum.Vendor;

                        Providers.ContactData.Save(PluginEntry.DataModel, contact);

                        if (vendor.DefaultContactID != contact.ID)
                        {
                            vendor.DefaultContactID = contact.ID;
                            vendor.Dirty = true;
                        }
                    }

                    if (vendor.Dirty)
                    { 
                        Providers.VendorData.Save(PluginEntry.DataModel, vendor);

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
