using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// 
    /// </summary>
    public class ItemTranslation : DataEntity
    {
        public RecordIdentifier ItemID { get; set; }
        public RecordIdentifier LanguageID { get; set; }
        public string Description { get; set; }
    }
}
