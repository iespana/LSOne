using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IPosColorData : IDataProvider<PosColor>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry, string sort);

        /// <summary>
        /// Gets a list of DataEntity objects containing ColorCode and Description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of DataEntity objects that contains ColorCode and Description</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of PosColor objects containing all rows from the POSCOLOR table
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of pos colors</returns>
        List<PosColor> GetAllColors(IConnectionManager entry);

        /// <summary>
        /// Gets a specific pos color record from the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorCode">The code of the color to get</param>
        /// <returns>A PosColor object containing the pos color record with the given color code</returns>
        PosColor GetColor(IConnectionManager entry, RecordIdentifier colorCode);

        /// <summary>
        /// Checks if a given pos color is in use.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorID">The id of the color to look for</param>
        /// <returns>True if the color is in use, false otherwise</returns>
        bool ColorIsInUse(IConnectionManager entry, RecordIdentifier colorID);
    }
}