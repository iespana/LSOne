using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Inventory.Containers
{
    /// <summary>
    /// Contains information returned by creating a new stock counting journal
    /// </summary>
    [DataContract]
    public class CreateStockCountingContainer
    {
        /// <summary>
        /// The result of the operation
        /// </summary>
        [DataMember]
        public CreateStockCountingResult Result { get; set; }

        /// <summary>
        /// The ID generated when creating a new stock counting ID
        /// </summary>
        [DataMember]
        public RecordIdentifier CreatedStockCountingID { get; set; }

        /// <summary>
        /// Indicates if an item was not found in the local database when creating journal from a filter/template
        /// </summary>
        [DataMember]
        public bool ItemNotFoundLocally { get; set; }

        public CreateStockCountingContainer()
        {
            //Default constructor for serialization
        }

        public CreateStockCountingContainer(CreateStockCountingResult result, RecordIdentifier createdStockCountingID)
        {
            Result = result;
            CreatedStockCountingID = createdStockCountingID;
            ItemNotFoundLocally = false;
        }

        public CreateStockCountingContainer(CreateStockCountingResult result, RecordIdentifier createdStockCountingID, bool itemNotFoundLocally)
        {
            Result = result;
            CreatedStockCountingID = createdStockCountingID;
            ItemNotFoundLocally = itemNotFoundLocally;
        }
    }
}
