using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;

namespace LSRetail.SiteService.IntegrationFrameworkTaxInterface
{
    public partial interface IIntegrationFrameworkTaxService
    {
        /// <summary>
        /// Gets a tax code with a given ID
        /// </summary>
        /// <param name="taxCodeID">ID of the tax code</param>
        /// <returns></returns>
        [OperationContract]
        TaxCode GetTaxCode(RecordIdentifier taxCodeID);

        /// <summary>
        /// Gets a list of all tax codes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<TaxCode> GetTaxCodes();

        /// <summary>
        /// Saves a single <see cref="TaxCode"/> to the database. If it does not exist it will be created.
        /// </summary>
        /// <param name="taxcode">The tax code to save</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult SaveTaxCode(TaxCode taxcode);

        /// <summary>
        /// Saves a list of <see cref="TaxCode"/> objects to the database. Tax codes that do not exist will be created.
        /// </summary>
        /// <param name="taxCodes">The list of item sales tax groups to save</param>
        /// <returns></returns>
        [OperationContract]
        SaveResult SaveTaxCodeList(List<TaxCode> taxCodes);

        /// <summary>
        /// Deletes a tax code by a given ID
        /// </summary>
        /// <param name="taxCodeID">The ID of the tax code to delete</param>
        [OperationContract]
        void DeleteTaxCode(RecordIdentifier taxCodeID);
    }
}
