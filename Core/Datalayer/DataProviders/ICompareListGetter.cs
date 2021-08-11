using LSOne.DataLayer.GenericConnector.Interfaces;
using System.Collections.Generic;

namespace LSOne.DataLayer.DataProviders
{
    public interface ICompareListGetter<BusinessObject> where BusinessObject : class, new()
    {
        /// <summary>
        /// Gets all the items from the database matching the list of <paramref name="itemsToCompare"/>
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="itemsToCompare">List of items you want to get from the database matching items </param>
        /// <returns>List of items</returns>
        List<BusinessObject> GetCompareList(IConnectionManager entry, List<BusinessObject> itemsToCompare);
    }
}