using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DataProviders.Customers
{
    public interface IGroupCategoryData : IDataProviderBase<GroupCategory>, ISequenceable
    {
        /// <summary>
        /// Returns a list of all categories
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of categories</returns>
        List<GroupCategory> GetList(IConnectionManager entry);

        /// <summary>
        /// Checks if a category by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the customer to check for</param>
        /// <returns>True if the customer exists, else false</returns>
        bool Exists(IConnectionManager entry, RecordIdentifier id);

        /// <summary>
        /// Saves a category to the database. If the category already exists it is updated
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="category">The category to be saved/updated</param>
        void Save(IConnectionManager entry, GroupCategory category);

        /// <summary>
        /// Deletes a category with a given ID
        /// </summary>
        /// <remarks>Requires the 'Edit categories' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="categoryID">The ID of the category to delete</param>
        void Delete(IConnectionManager entry, RecordIdentifier categoryID);

        bool CategoryUsedInGroups(IConnectionManager entry, RecordIdentifier categoryID);
    }
}