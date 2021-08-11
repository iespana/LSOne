using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.LookupValues
{
    public interface IMsrCardLinkData : IDataProvider<MsrCardLink>
    {
        /// <summary>
        /// Gets all card links for the given link type (employee or customer)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkType">The link type</param>
        /// <returns></returns>
        List<MsrCardLink> GetList(IConnectionManager entry, MsrCardLink.LinkTypeEnum linkType);

        /// <summary>
        /// Gets a card link for a specific card link and uses the link type as a filter. 
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <param name="msrCardLinkID">The ID of the card link (the card number).</param>
        /// <param name="linkType">The link type.</param>
        /// <param name="cache">Optional parameter to specify if cache may be used.</param>
        /// <returns>The MsrCardLink that matches the given parameters.</returns>
        MsrCardLink Get(IConnectionManager entry, RecordIdentifier msrCardLinkID, MsrCardLink.LinkTypeEnum linkType, 
            CacheType cache = CacheType.CacheTypeNone);

        MsrCardLink Get(IConnectionManager entry, RecordIdentifier msrCardLinkID);
    }
}