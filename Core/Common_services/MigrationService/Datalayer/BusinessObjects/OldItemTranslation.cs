using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// 
    /// </summary>
    public class OldItemTranslation : DataEntity
    {
        public RecordIdentifier ItemID { get; set; }
        public RecordIdentifier LanguageID { get; set; }
        public string Description { get; set; }
    }
}
