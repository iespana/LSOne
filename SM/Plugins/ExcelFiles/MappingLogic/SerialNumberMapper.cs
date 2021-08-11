using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.FileImport;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.ExcelFiles.MappingLogic
{
    internal class SerialNumberMapper : MapperBase
    {
        private const string ItemIDColumn = "ITEMID";
        private const string SerialNumberColumn = "SERIALNUMBER";
        private const string TypeColumn = "TYPE";

        internal static void Import(DataTable dt, List<ImportLogItem> importLogItems)
        {
            string[] mandatoryColumns = new string[]
            {
                    ItemIDColumn, SerialNumberColumn
            };

            CheckMandatoryColumns(dt, mandatoryColumns);

            //start importing
            foreach (DataRow row in dt.Rows)
            {
                int lineNumber = dt.GetRowNumber(row, "Line Number");
                string itemID = row.GetStringValue(ItemIDColumn);
                string serialNumber = row.GetStringValue(SerialNumberColumn);
                TypeOfSerial serialType = TypeOfSerial.SerialNumber;
                int type = 0;
                if (dt.Columns.Contains(TypeColumn))
                {
                    type = row.GetIntegerValue(TypeColumn);
                }
                if (Enum.IsDefined(typeof(TypeOfSerial), type))
                {
                    serialType = (TypeOfSerial)type;
                }

                RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID, true, DataLayer.GenericConnector.Enums.CacheType.CacheTypeNone);
                if (item == null)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.CouldNotFindItem.Replace("#1", itemID), lineNumber, string.Empty));
                    continue;
                }

                string description = item.Text;

                if (item.Deleted)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.ItemDeleted.Replace("#1", itemID), lineNumber, description));
                    continue;
                }
                if (string.IsNullOrWhiteSpace(serialNumber))
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.EmptySerialNumber, lineNumber, description));
                    continue;
                }
                try
                {
                    bool insert = true;
                    if (Providers.SerialNumberData.GetByItemAndSerialNumber(PluginEntry.DataModel, item.MasterID, serialNumber) != null)
                    {
                        insert = false;
                    }


                    SerialNumber sn = new SerialNumber() { ItemMasterID = item.MasterID, SerialNo = serialNumber, SerialType = serialType };
                    bool wasSaved = Providers.SerialNumberData.Save(PluginEntry.DataModel, sn, true);
                    if (!wasSaved)
                    {
                        importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, Properties.Resources.ErrorSavingSerialNumber,lineNumber, description));
                        continue;
                    }
                    else
                    {
                        importLogItems.Add(new ImportLogItem(insert ? ImportAction.Inserted : ImportAction.Updated, dt.TableName, itemID, ""));
                    }
                }
                catch (Exception ex)
                {
                    importLogItems.Add(new ImportLogItem(ImportAction.Skipped, dt.TableName, itemID, ex.Message, lineNumber));
                    continue;
                }
            }
        }
    }
}
