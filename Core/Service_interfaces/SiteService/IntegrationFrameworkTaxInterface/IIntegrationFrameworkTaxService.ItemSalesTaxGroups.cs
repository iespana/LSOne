using System.Collections.Generic;
using System.ServiceModel;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSRetail.SiteService.IntegrationFrameworkTaxInterface
{
    public partial interface IIntegrationFrameworkTaxService
    {

        /// <summary>
        /// Gets a item sales tax group by a given ID
        /// </summary>
        /// <param name="itemSalesTaxGroupID">The ID of the item sales tax group to get</param>
        /// <returns></returns>
        [OperationContract]
        ItemSalesTaxGroup GetItemSalesTaxGroup(RecordIdentifier itemSalesTaxGroupID);

        /// <summary>
        /// Gets a list of all item sales tax groups
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ItemSalesTaxGroup> GetItemSalesTaxGroups();

        /// <summary>
        /// Saves a single <see cref="ItemSalesTaxGroup"/> to the database. If it does not exist it will be created.
        /// </summary>
        /// <param name="itemSalesTaxGroup">The item sales tax group to save</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult SaveItemSalesTaxGroup(ItemSalesTaxGroup itemSalesTaxGroup);

        /// <summary>
        /// Saves a list of <see cref="ItemSalesTaxGroup"/> objects to the database. Item sales tax groups that do not exist will be created.
        /// </summary>
        /// <param name="itemSalesTaxGroups">The list of item sales tax groups to save</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult SaveItemSalesTaxGroupList(List<ItemSalesTaxGroup> itemSalesTaxGroups);

        /// <summary>
        /// Deletes an item sales tax group by a given ID
        /// </summary>
        /// <param name="itemSalesTaxGroupID">The ID of the item sales tax group to delete</param>
        [OperationContract]
        void DeleteItemSalesTaxGroup(RecordIdentifier itemSalesTaxGroupID);

        /// <summary>
        /// Adds a tax code to an item sales tax group.
        /// </summary>
        /// <param name="item">Contains IDs of the tax code and the item sales tax group that the tax code should be added to</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult AddTaxCodeToItemSalesTaxGroup(TaxCodeInItemSalesTaxGroup item);

        /// <summary>
        /// Adds a list of tax codes to item sales tax groups.
        /// </summary>
        /// <param name="items">A list containing IDs of the tax codes and item sales tax groups that they should be added to.</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult AddTaxCodeToItemSalesTaxGroupList(List<TaxCodeInItemSalesTaxGroup> items);

        /// <summary>
        /// Removes a tax code from an item sales tax group
        /// </summary>
        /// <param name="item">Contains the IDs of the tax code and the item sales tax group that the tax code should be removed from</param>
        [OperationContract]
        void RemoveTaxCodeFromItemSalesTaxGroup(TaxCodeInItemSalesTaxGroup item);
    }
}
