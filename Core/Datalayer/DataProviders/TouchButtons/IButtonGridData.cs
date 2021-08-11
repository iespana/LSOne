using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.DataProviders.TouchButtons
{
    public interface IButtonGridData : IDataProvider<ButtonGrid>, ISequenceable
    {
        /// <summary>
        /// Gets a list of DataEntity that contains button grid ID and button grid Description. The list is sorted by button grid description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>Gets a list of DataEntity that contains button grid and button grid Description</returns>
        List<DataEntity> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a list of all button grids
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns></returns>
        List<ButtonGrid> GetButtonGrids(IConnectionManager entry);
    }
}