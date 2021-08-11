using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface ISalesTypeData : IDataProvider<SalesType>, ISequenceable
    {
        /// <summary>
        /// Gets a list of data entities containing ID and name for each sales type, ordered by a chosen field
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all sales types, ordered by a chosen field</returns>
        List<DataEntity> GetList(IConnectionManager entry, SalesTypeSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Gets a list of data entities containing IDs and names for all sales types, ordered by the sales type description
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of data entities contaning IDs and names of sales types, ordered by the sales type description</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a data entity for Sales type wich contains the Code (ID) field and description field for the given sales type id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="code">The code (ID) field for the sales type</param>
        /// <returns>A SalesType object with the givn Code (ID), or null if the sales type was not found</returns>
        SalesType GetSalesTypeIdDescription(IConnectionManager entry, RecordIdentifier code);

        SalesType Get(IConnectionManager entry, RecordIdentifier id);
    }
}