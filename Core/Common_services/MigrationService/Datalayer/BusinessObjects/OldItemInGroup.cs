using LSOne.DataLayer.BusinessObjects;

namespace LSOne.Services.Datalayer.BusinessObjects
{
    /// <summary>
    /// When searching for items that are in a specific group or are not in a specific group this class is used as a result of those search criteria
    /// </summary>
    public class OldItemInGroup : DataEntity
    {
        /// <summary>
        /// Group description
        /// </summary>
        public string Group { get; set; }
    }
}
