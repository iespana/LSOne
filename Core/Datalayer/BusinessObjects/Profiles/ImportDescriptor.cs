using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.IO;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.BusinessObjects.Inventory;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public delegate void ImportFileDelegate(IConnectionManager entry, List<ImportFileItem> files);
    public class ImportDescriptor
    {
        public ImportFileDelegate Importer { get; private set; }
        public string Mask { get; private set; }
        public string Label { get; private set; }

        public ImportDescriptor(ImportFileDelegate importer, string mask, string label)
        {
            Importer = importer;
            Mask = mask;
            Label = label;
        }
    }

    public class ImportFileItem
    {
        public FolderItem FolderItem { get; }

        public RecordIdentifier RecordIdentifier { get; }

        public InventoryAdjustment InventoryAdjustment { get; }

        public ImportFileItem(FolderItem folderItem, RecordIdentifier recordIdentifier, InventoryAdjustment inventoryAdjustment)
        {
            FolderItem = folderItem;
            RecordIdentifier = recordIdentifier;
            InventoryAdjustment = inventoryAdjustment;
        }
    }
}
