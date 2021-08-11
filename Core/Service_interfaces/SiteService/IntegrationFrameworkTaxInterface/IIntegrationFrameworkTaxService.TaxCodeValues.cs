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
        /// Saves a given tax code value. If an existing entry exists for the given combination of <see cref="TaxCodeValue.TaxCode"/>, <see cref="TaxCodeValue.FromDate"/> and <see cref="TaxCodeValue.ToDate"/>
        /// it will either be overwritten or an error will be throw based on <paramref name="overwriteExistingRange"/>
        /// </summary>
        /// <param name="taxCodeValue">The tax code value to save</param>
        /// <param name="overwriteExistingRange">If true any existing tax code value will be overwritten. Otherwise an error is returned in <see cref="SaveResult"/>"/></param>
        /// <returns></returns>
        [OperationContract]
        SaveResult SaveTaxCodeValue(TaxCodeValue taxCodeValue, bool overwriteExistingRange);

        /// <summary>
        /// Saves a list of tax code values.  If an existing entry exists for the given combination of <see cref="TaxCodeValue.TaxCode"/>, <see cref="TaxCodeValue.FromDate"/> and <see cref="TaxCodeValue.ToDate"/>
        /// it will either be overwritten or an error will be throw based on <paramref name="overwriteExistingRange"/>
        /// </summary>
        /// <param name="taxCodeValues"></param>
        /// <param name="overwriteExistingRange">If true any existing tax code value will be overwritten. Otherwise an error is returned in <see cref="SaveResult"/></param>
        /// <returns></returns>
        [OperationContract]
        SaveResult SaveTaxCodeValueList(List<TaxCodeValue> taxCodeValues, bool overwriteExistingRange);

        /// <summary>
        /// Deletes a tax code value with a given ID
        /// </summary>        
        /// <param name="taxCodeValueID">ID of the tax code value</param>
        [OperationContract]
        void DeleteTaxCodeValue(RecordIdentifier taxCodeValueID);
    }
}
