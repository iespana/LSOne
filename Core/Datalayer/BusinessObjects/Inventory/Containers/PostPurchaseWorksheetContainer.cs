using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Inventory.Containers
{
    /// <summary>
    /// Contains information returned by posting a purchase worksheet
    /// </summary>
    [DataContract]
    public class PostPurchaseWorksheetContainer
    {
        /// <summary>
        /// The result of the operation
        /// </summary>
        [DataMember]
        public PostPurchaseWorksheetResult Result { get; set; }
         
        /// <summary>
        /// List of created purchase order IDs after posting the worksheet
        /// </summary>
        [DataMember]
        public List<RecordIdentifier> CreatedPurchaseOrderIDs { get; set; }

        public PostPurchaseWorksheetContainer(PostPurchaseWorksheetResult result, List<RecordIdentifier> createdPurchaseOrderIDs)
        {
            Result = result;
            CreatedPurchaseOrderIDs = createdPurchaseOrderIDs;
        }

        public PostPurchaseWorksheetContainer(PostPurchaseWorksheetResult result) : this(result, new List<RecordIdentifier>())
        {

        }

        public PostPurchaseWorksheetContainer() { } //Default constructor for serialization
    }
}
