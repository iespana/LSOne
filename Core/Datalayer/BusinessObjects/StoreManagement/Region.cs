using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    public class Region : DataEntity
    {
        public Region() : base()
        {

        }

        public Region(RecordIdentifier ID, string description)
        {
            this.ID = ID;
            Text = description;
        }

        public enum SortEnum
        {
            ID,
            Description
        }
    }
}
