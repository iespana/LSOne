using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    /// <summary>
    /// Data entity class for a store in a price group.
    /// </summary>
    public class StoreInPriceGroup : DataEntity
    {
        public override RecordIdentifier ID
        {
            get
            {
                return StoreID;
            }
            set
            {
                StoreID = value;
            }
        }

        public RecordIdentifier StoreID { get; set; }
        public int Level { set; get; }
        public RecordIdentifier PriceGroupID { get; set; }
        public string PriceGroupName { get; set; }
        public bool IncludeTaxForPriceGroup { get; set; }
        public bool Dirty { get; set; }
    }
}
 