using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Profiles
{
    public interface ISiteServiceProfileData : IDataProvider<SiteServiceProfile>, ISequenceable
    {
        List<DataEntity> GetList(IConnectionManager entry);
        List<SiteServiceProfile> GetSelectList(IConnectionManager entry);
        List<DataEntity> GetList(IConnectionManager entry, string sort);
        List<SiteServiceProfile> GetSiteServiceProfileList(IConnectionManager entry, string sort);

        /// <summary>
        /// Retrieves the StoreServer address and port for a given terminal and populates the ref values given.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalId">The id of the terminal to get the store server profile for</param>
        /// <param name="storeId">The ID of the store to get the store server profile for</param>
        /// <param name="storeServerAddress">The address of the store server</param>
        /// <param name="storeServerPort">The port for the store server</param>
        void GetStoreServerAddressAndPort(IConnectionManager entry, string terminalId, string storeId, ref string storeServerAddress, ref ushort storeServerPort);

        /// <summary>
        /// Retrieves the gift card option for the given terminal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalId">The ID for the terminal</param>
        /// <param name="storeId">The ID for the store</param>
        /// <returns>The issue gift card option for the given terminal</returns>
        SiteServiceProfile.IssueGiftCardOptionEnum GetTerminalGiftCardOption(IConnectionManager entry, RecordIdentifier terminalId, RecordIdentifier storeId);

        /// <summary>
        /// Retrieves the Store server profile for the given terminal
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="terminalId">The ID of the terminal to get the profile for</param>
        /// <param name="storeId">The ID of the store to get the profile for</param>
        /// <param name="cacheType">The type of cache to be used</param>
        /// <returns>The store server profile for the given terminal</returns>
        SiteServiceProfile GetTerminalProfile(IConnectionManager entry, RecordIdentifier terminalId, RecordIdentifier storeId, CacheType cacheType = CacheType.CacheTypeNone);

        SiteServiceProfile GetStoreProfile(IConnectionManager entry, RecordIdentifier storeId, 
            CacheType cacheType = CacheType.CacheTypeNone);

        SiteServiceProfile Get(IConnectionManager entry, RecordIdentifier id,
            CacheType cacheType = CacheType.CacheTypeNone);

        /// <summary>
        /// Save the SSL thumbnail of a site service profile
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="profile">Profile to save</param>
        void SaveSSLThumbnail(IConnectionManager entry, SiteServiceProfile profile);
    }
}