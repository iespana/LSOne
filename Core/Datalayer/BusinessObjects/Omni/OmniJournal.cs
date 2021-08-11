using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Omni
{
    public class OmniJournal : DataEntity
    {

        public OmniJournal() : base()
        {
            JournalType = -1;
            StoreID = RecordIdentifier.Empty;
            TemplateID = RecordIdentifier.Empty;
            StaffID = RecordIdentifier.Empty;
            Status = OmniJournalStatus.Received;
            XmlData = "";
            TransactionId = "";
        }
        
        /// <summary>
        /// Type of journal
        /// </summary>
        public int JournalType { get; set; }

        /// <summary>
        /// Store ID which created the journal
        /// </summary>
        public RecordIdentifier StoreID { get; set; }

        /// <summary>
        /// Inventory template ID
        /// </summary>
        public RecordIdentifier TemplateID { get; set; }

        /// <summary>
        /// Staff ID which created the journal
        /// </summary>
        public RecordIdentifier StaffID { get; set; }

        /// <summary>
        /// Current status of the journal
        /// </summary>
        public OmniJournalStatus Status { get; set; }

        /// <summary>
        /// Number of times this journal tried to be processed again
        /// </summary>
        public int RetryCounter { get; set; }

        /// <summary>
        /// Data about lines to be added in the corresponding documents, in XML format.
        /// </summary>
        /// <remarks>The format of the XML must be 
        /// <![CDATA[<DOCUMENT>
        /// <DOCUMENTLINE>
        ///     <ITEMDID>value</ITEMD>
        ///     <UNITID>value</UNITID>
        ///     <LINENUMBER>value</LINENUMBER>
        ///     <QUANTITY>value</QUANTITY>
        ///     <AREAID>value</AREAID>
        ///     <OMNILINEID>value</OMNILINEID>
        /// </DOCUMENTLINE>
        /// </DOCUMENT>]]>. If AREAID is null, the element should not exist in the XML.</remarks>
        public string XmlData { get; set; }

        /// <summary>
        /// The ID of the transaction in the inventory app that created this journal
        /// </summary>
        public string TransactionId { get; set; }
    }
}
