using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.Utilities.DataTypes;
using LSRetail.SiteService.IntegrationFrameworkBaseInterface.DTO;
using System.Collections.Generic;
using System.ServiceModel;

namespace LSRetail.SiteService.IntegrationFrameworkRetailItemInterface
{
    public partial interface IIntegrationFrameworkRetailItemService
    {
        /// <summary>
        /// Saves a single <see cref="UnitConversion"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="unitConversion">The unit conversion to save</param>
        [OperationContract]
        void SaveUnitConversion(UnitConversion unitConversion);

        /// <summary>
        /// Saves a list of <see cref="UnitConversion"/> objects to the database. Unit conversions that do not exist will be created.
        /// </summary>
        /// <param name="unitConversions">The list of unit conversions to save</param>
        [OperationContract]
        SaveResult SaveUnitConversionList(List<UnitConversion> unitConversions);

        /// <summary>
        /// Gets a single <see cref="UnitConversion"/> from the databse for the given <paramref name="unitConversionId"/>
        /// </summary>
        /// <param name="unitConversionId">The ID of the unit conversion to get. </param>
        /// <returns></returns>
        [OperationContract]
        UnitConversion GetUnitConversion(RecordIdentifier unitConversionId);

        /// <summary>
        /// Deletes the unit conversion with the given <paramref name="unitConversionId"/> from the database
        /// </summary>
        /// <param name="unitConversionId">The ID of the unit conversion to delete</param>
        [OperationContract]
        void DeleteUnitConversion(RecordIdentifier unitConversionId);
    }
}
