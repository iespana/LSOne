using LSOne.Utilities.DataTypes;


namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    public class KitchenDisplayProductionSection : DataEntity
    {
        /// <summary>
        /// Code name for the production section
        /// </summary>
        public RecordIdentifier Code { get; set; }

        /// <summary>
        /// Description of the production section
        /// </summary>
        public string Description { get; set; }

        public KitchenDisplayProductionSection()
        {
            ID = RecordIdentifier.Empty;
            Code = RecordIdentifier.Empty;
            Description = string.Empty;
        }
    }
}