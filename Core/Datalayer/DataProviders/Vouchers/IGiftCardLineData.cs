using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Vouchers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Vouchers
{
    public interface IGiftCardLineData : IDataProviderBase<GiftCardLine>
    {
        /// <summary>
        /// Gets a list of gift card lines for a given gift card.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="giftCardID">ID of the gift card to get gift card lines for</param>
        /// <returns>List of gift card lines for the given ID</returns>
        List<GiftCardLine> GetList(IConnectionManager entry, RecordIdentifier giftCardID);

        void Save(IConnectionManager entry, GiftCardLine item);
    }
}