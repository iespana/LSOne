using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    public interface IPosOperationData : IDataProviderBase<PosOperation>
    {
        /// <summary>
        /// Returns a list of all pos user operations ordered by the specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of PosOperation objects conataining all user pos operation records, ordered by the chosen field</returns>
        List<PosOperation> GetUserOperations(IConnectionManager entry, PosOperationSorting sortBy = PosOperationSorting.OperationName, 
            bool sortBackwards = false);

        List<PosOperation> GetList(IConnectionManager entry, CacheType cache = CacheType.CacheTypeNone);

        /// <summary>
        /// Returns a list of operations having the given <see cref="MenuTypeEnum"/>, ordered by the specified field. 
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="type">One of the <see cref="MenuTypeEnum"/></param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns></returns>
        List<PosOperation> GetList(IConnectionManager entry, OperationTypeEnum type, PosOperationSorting sortBy = PosOperationSorting.OperationName, bool sortBackwards = false);

        bool OperationsExists(IConnectionManager entry);

        PosOperation Get(IConnectionManager entry, RecordIdentifier posOperationID,
            CacheType cache = CacheType.CacheTypeNone);

        List<DataEntity> GetPaymentOperations(IConnectionManager entry);
        List<PosOperation> GetAuditableList(IConnectionManager entry);
    }
}