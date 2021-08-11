using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// Data entity class that contains information about a special group and a retail item and 
    /// whether the item is in the group
    /// </summary>
    public class OldSpecialGroupItem : DataEntity
    {
        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public OldSpecialGroupItem()
        {
            GroupId = "";
            Dirty = false;
            ItemId = "";
            GroupName = "";
        }

        /// <summary>
        /// The unique ID of the special group
        /// </summary>
        public RecordIdentifier GroupId { get; set; }

        /// <summary>
        /// The description of the special group
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// ID of the retail item.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// If true the group has items in it
        /// </summary>
        public bool ItemIsInGroup { get; set; }

        /// <summary>
        /// Used by the userinterface to determine if the item has changed after it was loaded.
        /// </summary>
        public bool Dirty { get; set; }
    }
}
