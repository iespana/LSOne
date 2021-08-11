using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions
{
    /// <summary>
    /// A class that represents a line item for a Style Group, color group or a size group
    /// </summary>
    public class StyleGroupLineItem : ItemWithDescription
    {
        string noInBarCode;
        string itemName;

        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public StyleGroupLineItem()
        {
            noInBarCode = "";
            SortingIndex = 0;
        }

        /// <summary>
        /// Returns ID of the Group
        /// </summary>
        public RecordIdentifier GroupID
        {
            get{return ID.SecondaryID;}
        }

        public string NoInBarCode
        {
            get { return noInBarCode; }
            set { noInBarCode = value; }
        }


        public string ItemName
        {
            get { return itemName; }
            internal set { itemName = value; }
        }

        public int SortingIndex { get; set; }
    }
}
