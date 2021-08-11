using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkRetailItemInterface
{
    public partial interface IIntegrationFrameworkRetailItemService
    {
        /// <summary>
        /// Saves a single <see cref="RetailItem"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="retailItem">The item to save</param>
        [OperationContract]
        void Save(RetailItem retailItem);

        /// <summary>
        /// Saves a list of <see cref="RetailItem"/> objects to the database. Items that do not exist will be created.
        /// </summary>
        /// <param name="retailItems">The list of items to save</param>
        [OperationContract]
        SaveResult SaveList(List<RetailItem> retailItems);

        /// <summary>
        /// Gets a single <see cref="RetailItem"/> from the databse for the given <paramref name="itemID"/>
        /// </summary>
        /// <param name="itemID">The ID of the item to get. This can be either the item's master ID or the regular item ID</param>
        /// <returns></returns>
        [OperationContract]
        RetailItem Get(RecordIdentifier itemID);

        /// <summary>
        /// Deletes the item with the given <paramref name="itemID"/> from the database
        /// </summary>
        /// <param name="itemID">The ID of the item to delete</param>
        [OperationContract]
        void Delete(RecordIdentifier itemID);
    }
}
