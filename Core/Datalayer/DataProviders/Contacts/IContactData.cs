using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Contacts
{
    public interface IContactData : IDataProvider<Contact>, ISequenceable
    {
        List<Contact> GetList(IConnectionManager entry,ContactRelationTypeEnum ownerType, RecordIdentifier ownerID, ContactSorting sortBy, bool sortBackwards);

        /// <summary>
        /// Deletes a contact with a given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="contactID">The ID of the contact to delete</param>
        /// <param name="ownerType">Owner type of the contact to be deleted</param>
        void Delete(IConnectionManager entry, RecordIdentifier contactID, ContactRelationTypeEnum ownerType);

        Contact Get(IConnectionManager entry, RecordIdentifier contactID);
    }
}