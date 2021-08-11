using System.Drawing;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IApplicationService : IService
    {
        /// <summary>
        /// Returns the next receipt ID for the given receipt id number sequence. If no number sequence is given or if the number sequence given does not exist in the NUMBERSEQUENCETABLE
        /// the default sequence RECEIPTID is used.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="receiptIdNumberSequence">The Number sequence id being used to create the receipt ids</param>
        /// <returns></returns>
        string GetNextReceiptId(IConnectionManager entry, string receiptIdNumberSequence);

        /// <summary>
        /// Returns true if the given receiptID exists in RBOTRANSACTIONTABLE
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="receiptID">The value of the receiptID</param>
        /// <returns></returns>
        bool ReceiptIDExists(IConnectionManager entry, string receiptID);

        /// <summary>
        /// If there is no receipt ID on the transaction or if the receipt ID that is already on the transaction already exists
        /// a new receipt ID is generated and set on the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current settings</param>
        /// <param name="transaction">The transaction to check and update</param>
        void GenerateReceiptID(IConnectionManager entry, ISettings settings, IPosTransaction transaction);


        /// <summary>
        /// Returns the text to be displayed on the task bar icon if it returns an empty string a default LSPOS string is displayed
        /// </summary>
        string ApplicationWindowCaption
        {
            get;
        }

        /// <summary>
        /// Returns the icon to be displayed in the task bar if null is returned then the default LS Pos icon is displayed
        /// </summary>
        Icon ApplicationWindowIcon
        {
            get;
        }

        /// <summary>
        /// Returns the image to be displayed on the logon dialog in the top left corner if null is returned then the default LS Pos image is displayed.
        /// </summary>
        Image LoginWindowImage
        {
            get;
        }

        PartnerObjectBase PartnerObject
        {
            get;
        }

        /// <summary>
        /// Returns the sequence provider used for generating receipt IDs through the number sequence generator. If you wish to suppress the use of this provider
        /// in the POS return null.
        /// </summary>
        ISequenceable ReceiptSequenceProvider
        {
            get;
        }

    }
}
