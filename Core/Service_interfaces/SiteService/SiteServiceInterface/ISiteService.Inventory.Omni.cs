using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.Development;
using System.ServiceModel;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public partial interface ISiteService
    {
        /// <summary>
        /// Send an LS Commerce journal to HQ
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="omniJournal">LS Commerce journal to send</param>
        /// <returns>Operation result</returns>
        [OperationContract]
        [LSOneUsage(CodeUsage.LSCommerce)]
        SendOmniJournalResult SendOmniJournal(LogonInfo logonInfo, OmniJournal omniJournal);

        /// <summary>
        /// Adds an image to the appropriate inventory journal line
        /// </summary>
        /// <param name="logonInfo">The login information for the database</param>
        /// <param name="lineType">The type of line to add the image for. Currently only <see cref="InventoryEnum.StockCounting"/>, <see cref="InventoryEnum.StoreTransfer"/> and <see cref="InventoryEnum.PurchaseOrder"/> are supported</param>
        /// <param name="templateID">The inventory stock counting template ID used to create the journal</param>
        /// <param name="omniTransactionID">The ID of the transaction in the inventory app that this line was created for</param>
        /// <param name="omniLineID">The ID of the line in the transaction that was created in the inventory app</param>
        /// <param name="image">A Base64 representation of the image</param>
        /// <param name="imageDescription">The description of the image</param>
        [OperationContract]
        [LSOneUsage(CodeUsage.LSCommerce)]
        void AddInventoryJournalLineImage(LogonInfo logonInfo, InventoryEnum lineType, string templateID, string omniTransactionID, string omniLineID, string image, string imageDescription);
    }
}
