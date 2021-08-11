using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.DataProviders.KitchenDisplaySystem
{
    public interface IKitchenDisplayLineColumnData : IDataProvider<KitchenDisplayLineColumn>
    {
        /// <summary>
        /// Gets all Displayline columns
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the display profile which the lines belong to</param>
        /// <returns>A list of kitchen display lines</returns>
        List<KitchenDisplayLineColumn> GetList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Gets a list of all linecolumns
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of linecolumn objects containing all linecolumn records</returns>
        List<KitchenDisplayLineColumn> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a Displayline column with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the line to get</param>
        /// <param name="displayProfileID">The ID of the display profile which the line belongs to</param>
        /// <returns>A Kitchen printer with the given ID</returns>
        KitchenDisplayLineColumn Get(IConnectionManager entry, RecordIdentifier id);

        bool Exists(IConnectionManager entry, RecordIdentifier id, int columnNo);
        void Delete(IConnectionManager entry, RecordIdentifier id, int columnNo);
        void DeleteByDisplayLine(IConnectionManager entry, RecordIdentifier lineId);
        void SaveOrder(IConnectionManager entry, KitchenDisplayLineColumn displayLineColumn);
    }
}
