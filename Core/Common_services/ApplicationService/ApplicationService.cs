using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.Services.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.ErrorHandling;
using LSOne.Triggers;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services
{
    /// <summary>
    /// Class that implements the ApplicationService interface
    /// </summary>
    public partial class ApplicationService : IApplicationService
    {
        #region IApplication Members

        IErrorLog errorLog;

        /// <summary>
        /// Access to the error log functionality
        /// </summary>
        public virtual IErrorLog ErrorLog
        {
            set
            {
                errorLog = value;
            }
        }

        /// <summary>
        /// Sets the database connection for the service
        /// </summary>
        /// <param name="entry">The entry to the database</param>
        public void Init(IConnectionManager entry)
        {

            #pragma warning disable 0612, 0618
            DLLEntry.DataModel = entry;
            #pragma warning restore 0612, 0618
        }

        /// <summary>
        /// If there is no receipt ID on the transaction or if the receipt ID that is already on the transaction already exists
        /// a new receipt ID is generated and set on the transaction
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="settings">The current settings</param>
        /// <param name="transaction">The transaction to check and update</param>
        public virtual void GenerateReceiptID(IConnectionManager entry, ISettings settings, IPosTransaction transaction)
        {
            if (string.IsNullOrEmpty(transaction.ReceiptId) || ReceiptIDExists(entry, transaction.ReceiptId))
            {
                transaction.ReceiptId = GetNextReceiptId(entry, (string)settings.Terminal.ReceiptIDNumberSequence);
            }
        }

        /// <summary>
        /// Returns the next receipt ID for the given receipt id number sequence. If no number sequence is given or if the number sequence given does not exist in the NUMBERSEQUENCETABLE
        /// the default sequence RECEIPTID is used.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="receiptIdNumberSequence">The Number sequence id being used to create the receipt ids</param>
        /// <returns></returns>
        public virtual string GetNextReceiptId(IConnectionManager entry, string receiptIdNumberSequence)
        {
            try
            {
                var sequence = TransactionProviders.ReceiptSequence;

                if (!string.IsNullOrEmpty(receiptIdNumberSequence))
                {
                    sequence.ReceiptNumberSequenceID = Providers.NumberSequenceData.Exists(entry, receiptIdNumberSequence)
                        ? receiptIdNumberSequence
                        : TransactionProviders.ReceiptSequence.DefaultReceiptSequence;
                }

                return sequence.CreateNewID(entry);
            }
            catch (Exception ex)
            {
                entry.ErrorLogger.LogMessage(LogMessageType.Error, this.ToString(), ex);
                throw;
            }
        }

        /// <summary>
        /// Returns the text to be displayed on the task bar icon if it returns an empty string a default LSPOS string is displayed
        /// </summary>
        public virtual string ApplicationWindowCaption
        {
            get { return ""; }
        }

        /// <summary>
        /// Returns the icon to be displayed in the task bar if null is returned then the default LS Pos icon is displayed
        /// </summary>
        public System.Drawing.Icon ApplicationWindowIcon
        {
            get { return null; }
            //get { return new Icon(@"Images\TestIcon.ico"); }
        }

        /// <summary>
        /// Returns the image to be displayed on the logon dialog in the top left corner if null is returned then the default LS Pos image is displayed.
        /// </summary>

        [Obsolete("This property cannot be used anymore. No alternative added", true)]
        public System.Drawing.Image LoginWindowImage
        {
            get { return null; }
            //get { return Image.FromFile(@"Images\Penguins.jpg"); }
        }

        /// <summary>
        /// Returns the partner object that has been created and can be attached to a transaction. The POS calls this property when 
        /// it needs a new instance of the partner object
        /// </summary>
        public PartnerObjectBase PartnerObject
        {
            get { return null; }
            //get { return new MyPartnerObject(); }
        }

        /// <summary>
        /// Returns true if the given receiptID exists in RBOTRANSACTIONTABLE
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="receiptID">The value of the receiptID</param>
        /// <returns></returns>
        public virtual bool ReceiptIDExists(IConnectionManager entry, string receiptID)
        {
            if (string.IsNullOrEmpty(receiptID))
            {
                return false;
            }

            return TransactionProviders.ReceiptSequence.SequenceExists(entry, receiptID);
        }

        /// <summary>
        /// Returns the sequence provider for the receipt ID
        /// </summary>
        public virtual ISequenceable ReceiptSequenceProvider => TransactionProviders.ReceiptSequence;

        #endregion
    }
}
