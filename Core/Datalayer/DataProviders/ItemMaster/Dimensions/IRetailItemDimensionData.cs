using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.DataProviders.ItemMaster.Dimensions
{
    public interface IRetailItemDimensionData : IDataProviderBase<RetailItemDimension>
    {        
        RetailItemDimension Get(IConnectionManager entry, RecordIdentifier templateID);

        List<RetailItemDimension> GetListForRetailItem(IConnectionManager entry, RecordIdentifier retailItemMasterID);

        /// <summary>
        /// Returns a list of all retail item dimensions that are currently being used on variant items belonging to the given item master ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="retailItemMasterID">The ID of the master item</param>
        /// <returns></returns>
        List<RetailItemDimension> GetListInUseByRetailItem(IConnectionManager entry, RecordIdentifier retailItemMasterID);
        
        void Delete(IConnectionManager entry, RecordIdentifier dimensionID);

        void Save(IConnectionManager entry, RetailItemDimension dimension);
    }
}
