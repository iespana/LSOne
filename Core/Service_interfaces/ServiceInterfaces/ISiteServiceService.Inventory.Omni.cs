using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Development;

namespace LSOne.Services.Interfaces
{
    public partial interface ISiteServiceService
	{
        /// <summary>
        /// Send an LS Commerce journal to HQ
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="omniJournal">LS Commerce journal to send</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        /// <returns>Operation result</returns>
		[LSOneUsage(CodeUsage.LSCommerce)]
        SendOmniJournalResult SendOmniJournal(IConnectionManager entry, SiteServiceProfile siteServiceProfile, OmniJournal omniJournal, bool closeConnection);

        /// <summary>
        /// Adds an image to a stock counting journal line
        /// </summary>
        /// <param name="entry">The database connection</param>
        /// <param name="siteServiceProfile">Which site service to use for this operation</param>
        /// <param name="lineType">The type of line to add the image for. Currently only <see cref="InventoryEnum.StockCounting"/>, <see cref="InventoryEnum.StoreTransfer"/> and <see cref="InventoryEnum.PurchaseOrder"/> are supported</param>
        /// <param name="templateID">The inventory stock counting template ID used to create the journal</param>
        /// <param name="omniTransactionID">The ID of the transaction in the inventory app that this line was created for</param>
        /// <param name="omniLineID">The ID of the line in the transaction that was created in the inventory app</param>
        /// <param name="image">A Base64 representation of the image</param>
        /// <param name="imageDescription">The description of the image</param>
        /// <param name="closeConnection">If true then the connection to the site service is closed as soon as the operation has finished otherwise it will stay open</param>
        [LSOneUsage(CodeUsage.LSCommerce)]
        void AddInventoryJournalLineImage(IConnectionManager entry, SiteServiceProfile siteServiceProfile, InventoryEnum lineType, string templateID, string omniTransactionID, string omniLineID, string image, string imageDescription, bool closeConnection);
	}
}
