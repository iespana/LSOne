using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.SiteService.Plugins.SiteManager
{
    public partial class SiteManagerPlugin
    {
        protected List<SimpleRetailItem> itemCache;
        protected long itemCacheTick = 0;

        /// <summary>
        /// Retrieves item information from the Site Manager item cache. If the item cache is more than an hour old it is refreshed
        /// If the item is not found in the cache it is retrieved from the database and added to the cache (if it was added since the cache was last refreshed)
        /// If the item is not found at all the function returns null
        /// </summary>
        /// <param name="itemID">The item ID of the item to be returned</param>
        /// <returns></returns>
        protected virtual SimpleRetailItem GetItemFromCache(IConnectionManager entry,RecordIdentifier itemID)
        {
            UpdateItemCache(entry);
            SimpleRetailItem item = itemCache.FirstOrDefault(f => f.ID == itemID);
            if (item == null)
            {
                RetailItem retailItem = Providers.RetailItemData.Get(entry, itemID);
                if (retailItem != null)
                {
                    item = new SimpleRetailItem(retailItem);                    
                    itemCache.Add(item);
                }                
            }

            return item;
        }

        protected virtual void UpdateItemCache(IConnectionManager entry)
        {            
            TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks - itemCacheTick);
            if (elapsedSpan.Minutes > 60)
            {
                //Update the item cache
                itemCache = Providers.RetailItemData.GetSimpleItems(entry);
                itemCacheTick = DateTime.Now.Ticks;
            }            
        }
    }
}