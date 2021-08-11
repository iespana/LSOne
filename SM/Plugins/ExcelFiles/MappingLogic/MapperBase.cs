using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Contacts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewPlugins.ExcelFiles.Exceptions;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal abstract class MapperBase
    {
        protected static void CheckMandatoryColumns(DataTable dt, string[] fieldNames)
        {
            int count = fieldNames.GetUpperBound(0) + 1;

            for (int i = 0; i < count; i++)
            {
                if (!dt.Columns.Contains(fieldNames[i]))
                {
                    throw new ColumnMissingException(fieldNames[i]);
                }
            }
        }

        protected static bool CheckMandatoryFields(DataTable dt, DataRow row, RecordIdentifier itemID, string[] fieldNames, bool inserting, List<ImportLogItem> importLogItems, 
            int? lineNumber = null, string itemDescription = "")
        {
            if (inserting)
            {
                int count = fieldNames.GetUpperBound(0) + 1;

                for (int i = 0; i < count; i++)
                {
                    if (row[fieldNames[i]] == DBNull.Value)
                    {
                        // Mandatory field missing
                        if (lineNumber.HasValue)
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", fieldNames[i]),
                                lineNumber, itemDescription));
                        }
                        else
                        {
                            importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.MandatoryFieldMissing.Replace("#1", fieldNames[i])));
                        }
                        return false;
                    }
                }
            }

            return true;
        }

        protected static Address.AddressFormatEnum MapAddressFormat(string excelValue)
        {
            switch (excelValue)
            {
                case "Generic with state":
                    return Address.AddressFormatEnum.GenericWithState;

                case "Generic without state":
                    return Address.AddressFormatEnum.GenericWithoutState;

                case "US":
                    return Address.AddressFormatEnum.US;

                case "Canadian":
                    return Address.AddressFormatEnum.Canadian;

                case "Indian":
                    return Address.AddressFormatEnum.Indian;

                case "UK":
                    return Address.AddressFormatEnum.UK;

                default:
                    return Address.AddressFormatEnum.GenericWithState;
            }
        }

        protected static string MapLanguageCode(string excelValue)
        {
            if (excelValue == "")
            {
                return "";
            }
            else
            {
                return excelValue;
            }
        }

        protected static string MapCountryCode(string excelValue)
        {
            if (excelValue == "")
            {
                return "";
            }
            else
            {
                return Providers.CountryData.GetIDFromName(PluginEntry.DataModel, excelValue);
            }
        }
    }
}
