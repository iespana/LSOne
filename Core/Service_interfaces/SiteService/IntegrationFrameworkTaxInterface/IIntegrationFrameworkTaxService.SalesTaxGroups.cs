using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkTaxInterface
{
    public partial interface IIntegrationFrameworkTaxService
    {

        /// <summary>
        /// Gets a sales tax group by a given ID
        /// </summary>
        /// <param name="SalesTaxGroupID">The ID of the sales tax group to get</param>
        /// <returns></returns>
        [OperationContract]
        SalesTaxGroup GetSalesTaxGroup(RecordIdentifier salesTaxGroupID);

        /// <summary>
        /// Gets a list of all sales tax groups
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SalesTaxGroup> GetSalesTaxGroups();

        /// <summary>
        /// Saves a single <see cref="SalesTaxGroup"/> to the database. If it does not exist it will be created.
        /// </summary>
        /// <param name="salesTaxGroup">The sales tax group to save</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult SaveSalesTaxGroup(SalesTaxGroup salesTaxGroup);

        /// <summary>
        /// Saves a list of <see cref="SalesTaxGroup"/> objects to the database. Sales tax groups that do not exist will be created.
        /// </summary>
        /// <param name="salesTaxGroups">The list of sales tax groups to save</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult SaveSalesTaxGroupList(List<SalesTaxGroup> salesTaxGroups);

        /// <summary>
        /// Deletes an sales tax group by a given ID
        /// </summary>
        /// <param name="SalesTaxGroupID">The ID of the sales tax group to delete</param>
        [OperationContract]
        void DeleteSalesTaxGroup(RecordIdentifier salesTaxGroupID);

        /// <summary>
        /// Adds a tax code to an sales tax group.
        /// </summary>
        /// <param name="taxCode">Contains IDs of the tax code and the sales tax group that the tax code should be added to</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult AddTaxCodeToSalesTaxGroup(TaxCodeInSalesTaxGroup taxCode);

        /// <summary>
        /// Adds a list of tax codes to sales tax groups.
        /// </summary>
        /// <param name="taxCodess">A list containing IDs of the tax codes and sales tax groups that they should be added to.</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult AddTaxCodeToSalesTaxGroupList(List<TaxCodeInSalesTaxGroup> taxCodes);

        /// <summary>
        /// Removes a tax code from an sales tax group
        /// </summary>
        /// <param name="taxCode">Contains the IDs of the tax code and the sales tax group that the tax code should be removed from</param>
        [OperationContract]
        void RemoveTaxCodeFromSalesTaxGroup(TaxCodeInSalesTaxGroup taxCode);
    }
}
