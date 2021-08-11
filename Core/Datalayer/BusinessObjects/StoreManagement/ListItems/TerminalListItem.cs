using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.StoreManagement.ListItems
{
    public class TerminalListItem : DataEntity
    {

        public enum SortEnum
        {
            STORENAME, 
            ID,
            NAME
        }

        public TerminalListItem(RecordIdentifier terminalID, string description)
            : base(terminalID, description)
        {
            this.StoreID = "";
            this.StoreName = "";
        }

        public TerminalListItem()
            : base()
        {
            this.StoreID = "";
            this.StoreName = "";
        }

        public string Name
        {
            get { return Text; }
            set { Text = value; }
        }
        /// <summary>
        /// This field contains the identification number of the store that the POS terminal is located in.
        /// </summary>
        public RecordIdentifier StoreID { get; set; }
        /// <summary>
        /// This field contains the name of the store that the POS terminal is located in.
        /// </summary>
        public string StoreName { get; set; }

        public string DisplayName
        {
            get; set;
        }

    }
}
