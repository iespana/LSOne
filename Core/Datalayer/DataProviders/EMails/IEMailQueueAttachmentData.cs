using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.EMails;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.EMails
{
    public interface IEMailQueueAttachmentData : IDataProviderBase<EMailQueueAttachment>
    {
        /// <summary>
        /// Gets attachments for a specific email
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="emailid">Email id that attachments are attached to</param>
        /// <returns>An instance of <see cref="EMailQueueAttachment"/></returns>
        List<EMailQueueAttachment> GetList(IConnectionManager entry, RecordIdentifier emailid);

        void Save(IConnectionManager entry, EMailQueueAttachment emailQueueAttachment);
    }
}