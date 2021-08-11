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
    public interface IKitchenDisplayLineData : IDataProvider<KitchenDisplayLine>
    {
        /// <summary>
        /// Gets all Chit or LineDisplay lines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the display profile which the lines belong to</param>
        /// <returns>A list of kitchen display lines</returns>
        List<KitchenDisplayLine> GetList(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Gets a list of all displaylines
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all displaylines</returns>
        List<KitchenDisplayLine> GetList(IConnectionManager entry);

        /// <summary>
        /// Gets a Chit or LineDisplay line with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the line to get</param>
        /// <param name="displayProfileID">The ID of the display profile which the line belongs to</param>
        /// <returns>A Kitchen printer with the given ID</returns>
        KitchenDisplayLine Get(IConnectionManager entry, RecordIdentifier id, RecordIdentifier displayProfileID);
        short MaxLineNo(IConnectionManager entry, KitchenDisplayLine displayLine);
    }
}
