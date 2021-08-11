using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Customers
{
    public class GroupCategoryData : SqlServerDataProviderBase, ISequenceable, IGroupCategoryData
    {
        /// <summary>
        /// Returns a list of all categories
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of categories</returns>
        public virtual List<GroupCategory> GetList(IConnectionManager entry)
        {
            return GetList<GroupCategory>(entry, "CUSTGROUPCATEGORY", "DESCRIPTION", "ID", "DESCRIPTION");
        }

        /// <summary>
        /// Checks if a category by a given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">ID of the customer to check for</param>
        /// <returns>True if the customer exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<GroupCategory>(entry, "CUSTGROUPCATEGORY", "ID", id);
        }

        /// <summary>
        /// Saves a category to the database. If the category already exists it is updated
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="category">The category to be saved/updated</param>
        public virtual void Save(IConnectionManager entry, GroupCategory category)
        {
            var statement = new SqlServerStatement("CUSTGROUPCATEGORY");

            ValidateSecurity(entry, LSOne.DataLayer.BusinessObjects.Permission.CategoriesEdit);

            bool isNew = false;

            if (category.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                category.ID = DataProviderFactory.Instance.GenerateNumber<IGroupCategoryData, GroupCategory>(entry);
            }

            if (isNew || !Exists(entry, category.ID))
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)category.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)category.ID);
            }

            statement.AddField("DESCRIPTION", category.Text);

            entry.Connection.ExecuteStatement(statement);
        }

        /// <summary>
        /// Deletes a category with a given ID
        /// </summary>
        /// <remarks>Requires the 'Edit categories' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="categoryID">The ID of the category to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier categoryID)
        {
            DeleteRecord(entry, "CUSTGROUPCATEGORY", "ID", categoryID, LSOne.DataLayer.BusinessObjects.Permission.CategoriesEdit);
        }

        public virtual bool CategoryUsedInGroups(IConnectionManager entry, RecordIdentifier categoryID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT ISNULL(COUNT(ID), 0) IDS " +
                    "FROM CUSTGROUP " +
                    "WHERE CATEGORY = @CATEGORY " +
                    "AND DATAAREAID = @DATAAREAID ";

                MakeParam(cmd, "DATAAREAID", entry.Connection.DataAreaId);
                MakeParam(cmd, "CATEGORY", (string)categoryID);

                return (int)entry.Connection.ExecuteScalar(cmd) > 0;
            }
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "CUSTGROUPCATEGORY"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "CUSTGROUPCATEGORY", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion

    }
}
