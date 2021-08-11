using LSOne.Utilities.DataTypes;
using LSOne.Utilities.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.FileImport
{
    [DataContract]
    public enum ImportAction
    {
        [EnumMember]
        Skipped,
        [EnumMember]
        Inserted,
        [EnumMember]
        Updated
    }

    [DataContract]
    public class ImportLogItem
    {
        public ImportLogItem(ImportAction action, string sheetName, RecordIdentifier id, string reason, int? lineNumber = null, string itemDescription = "", int count = 1)
        {
            Action = action;
            SheetName = sheetName;
            ID = id;
            Reason = reason;
            LineNumber = lineNumber;
            ItemDescription = itemDescription;
            Count = count;
        }

        [DataMember]
        public ImportAction Action { get; set; }
        [DataMember]
        public RecordIdentifier ID { get; set; }
        [DataMember]
        public string SheetName { get; set; }
        [DataMember]
        public string Reason { get; set; }
        [DataMember]
        public int? LineNumber { get; set; }
        [DataMember]
        public string ItemDescription { get; set; }
        [DataMember]
        public int Count { get; set; }
    }

    public class FileImportLogItem
    {
        public string ID { get; set; }
        public List<ImportLogItem> ImportLogItems { get; set; }
        public FolderItem File { get; set; }
        public string ImportType { get; set; }
        public string ImportProfile { get; set; }
    }
}
