using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EFT;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.EFT
{
    public interface IEFTMappingData : IDataProvider<EFTMapping>, ISequenceable
    {
        List<EFTMapping> GetList(IConnectionManager entry, bool includeDisabled = false);

        /// <summary>
        /// Gets a mapping for the specified scheme name (only if ENABLED = 1)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemeName">The name of the scheme to search for</param>
        /// <returns>The mapping with the given scheme name</returns>
        EFTMapping GetForScheme(IConnectionManager entry, string schemeName);

        /// <summary>
        /// Gets a mapping for the specified scheme. If no such mapping is found, but a mapping for a '*' scheme
        /// is found, then copy that scheme, save it with the new name and return that mapping
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemeName">The name of the scheme to search for</param>
        /// <returns>The mapping with the given scheme name</returns>
        EFTMapping GetForSchemeWithFallback(IConnectionManager entry, string schemeName);

        /// <summary>
        /// Gets a mapping where the scheme name is contained in the specified card name
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="cardName">Cardname to compare to schemes</param>
        /// <returns>The mapping that matches, or null</returns>
        EFTMapping GetWhereSchemeIsInCardName(IConnectionManager entry, string cardName);

        /// <summary>
        /// Saves an Infocode object to the database. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="mapping">The Infocode to be saved</param>
        /// <param name="validateSecurity">If true, then edit permissions will be validated</param>
        void Save(IConnectionManager entry, EFTMapping mapping, bool validateSecurity);

        EFTMapping Get(IConnectionManager entry, RecordIdentifier mappingID);
    }
}