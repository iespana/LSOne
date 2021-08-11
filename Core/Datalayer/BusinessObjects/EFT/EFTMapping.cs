using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.EFT
{
    public class EFTMapping : DataEntity
    {
        public EFTMapping() : base()
        {
            Enabled = true;
        }

        // Accessors
        public RecordIdentifier MappingID { get { return ID; } set { ID = value; } }
        public string SchemeName { get { return Text; } set { Text = value; } }

        public RecordIdentifier TenderTypeID { get; set; }
        public string TenderTypeName { get; set; }
        public RecordIdentifier CardTypeID { get; set; }
        public string CardTypeName { get; set; }
        public bool Enabled { get; set; }
        public DateTime Created { get; set; }
        public short LookupOrder { get; set; }
    }
}
