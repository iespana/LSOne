using LSOne.DataLayer.BusinessObjects.Enums;
using System.Runtime.Serialization;

namespace LSOne.DataLayer.BusinessObjects.Inventory.Containers
{
    /// <summary>
    /// Contains information returned by posting multiple stock counting lines
    /// </summary>
    [DataContract]
    public class PostStockCountingLinesContainer
    {
        /// <summary>S
        /// The result of the operation
        /// </summary>
        [DataMember]
        public PostStockCountingResult Result { get; set; }

        /// <summary>
        /// Indicates whether the journal still has unposted lines; if an error occurs during the process, this property is not initialized
        /// </summary>
        [DataMember]
        public bool HasUnpostedLines { get; set; }

        public PostStockCountingLinesContainer()
        {
            Result = PostStockCountingResult.Success;
            HasUnpostedLines = true;
        }
    }
}