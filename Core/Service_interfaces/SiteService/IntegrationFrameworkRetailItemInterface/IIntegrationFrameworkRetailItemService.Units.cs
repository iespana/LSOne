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
        /// Saves a single <see cref="Unit"/> to the database. If it does not exists it will be created.
        /// </summary>
        /// <param name="unit">The unit  to save</param>
        [OperationContract]
        void SaveUnit(Unit unit);

        /// <summary>
        /// Saves a list of <see cref="Unit"/> objects to the database. Unit s that do not exist will be created.
        /// </summary>
        /// <param name="units">The list of unit s to save</param>
        [OperationContract]
        SaveResult SaveUnitList(List<Unit> units);

        /// <summary>
        /// Gets a single <see cref="Unit"/> from the databse for the given <paramref name="unitId"/>
        /// </summary>
        /// <param name="unitId">The ID of the unit  to get. </param>
        /// <returns></returns>
        [OperationContract]
        Unit GetUnit(RecordIdentifier unitId);

        /// <summary>
        /// Deletes the unit  with the given <paramref name="unitId"/> from the database
        /// </summary>
        /// <param name="unitId">The ID of the unit  to delete</param>
        [OperationContract]
        void DeleteUnit(RecordIdentifier unitId);
    }
}
