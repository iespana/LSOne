using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Forms;
using LSOne.DataLayer.DataProviders.Forms;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Forms
{
    public class FormTypeData : SqlServerDataProviderBase, IFormTypeData
    {
        private static string ResolveSort(FormTypeSorting sort, bool backwards)
        {
            switch (sort)
            {
                case FormTypeSorting.Description:
                    return string.Format("DESCRIPTION {0}", (backwards ? "DESC" : "ASC"));
                case FormTypeSorting.Type:
                    return string.Format("SYSTEMTYPE {0}", (backwards ? "DESC" : "ASC"));
            }
            return "";
        }

        private static void PopulateFormType(IDataReader dr, FormType formType)
        {
            formType.ID = (Guid)dr["ID"];
            formType.Text = (string)dr["DESCRIPTION"];
            formType.SystemType = Convert.ToInt32(dr["SYSTEMTYPE"]);
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList(entry, FormTypeSorting.Description, false);
        }

        /// <summary>
        /// Gets a list of all form types
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">Sorting column</param>
        /// <param name="sortBackwards"></param>
        /// <returns>A list of all form types</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, FormTypeSorting sort, bool sortBackwards)
        {
            return GetList<DataEntity>(entry, "POSFORMTYPE", "DESCRIPTION", "ID", ResolveSort(sort, sortBackwards));
        }

        public virtual List<FormType> GetFormTypes(IConnectionManager entry, FormTypeSorting sortBy, bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "SELECT ID, ISNULL(DESCRIPTION,'') as DESCRIPTION," +
                    "ISNULL (SYSTEMTYPE, 0) AS SYSTEMTYPE " +
                    "from POSFORMTYPE " +
                    "where DATAAREAID = @dataAreaId " +
                    "ORDER BY " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<FormType>(entry, cmd, CommandType.Text, PopulateFormType);
            }
        }

        /// <summary>
        /// Gets a list of form types that are not used by a profile
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="profileId">Profile id to filter form types</param>
        /// <returns>List of form types not used by the profile</returns>
        public virtual List<FormType> GetUnusedFormTypes(IConnectionManager entry, RecordIdentifier profileId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = @"SELECT p.ID, ISNULL(p.DESCRIPTION, '') AS DESCRIPTION, ISNULL(p.SYSTEMTYPE, 0) AS SYSTEMTYPE 
                                    FROM POSFORMTYPE p
                                    WHERE NOT EXISTS(
                                            SELECT NULL FROM POSFORMPROFILELINES l 
                                            WHERE l.FORMTYPEID = p.ID 
                                            AND l.PROFILEID = @profileID 
                                            AND l.DATAAREAID = @dataAreaID) 
                                    AND p.DATAAREAID = @dataAreaID ORDER BY DESCRIPTION ASC";

                MakeParam(cmd, "profileID", profileId);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);

                return Execute<FormType>(entry, cmd, CommandType.Text, PopulateFormType);
            }
        }

        /// <summary>
        /// Gets the formType with id sent as parameter
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="id">id of FormType to get</param>
        /// /// <param name="cache">Cachetype</param>
        /// <returns>A formType </returns>
        public virtual FormType Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                       "select ID, ISNULL(DESCRIPTION,'') as DESCRIPTION," +
                       "ISNULL (SYSTEMTYPE,0) AS SYSTEMTYPE " +
                       "from POSFORMTYPE " +
                       "where DATAAREAID = @dataAreaId and ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (Guid)id);

                return Get<FormType>(entry, cmd, id, PopulateFormType, cache, UsageIntentEnum.Normal);
            }
        }

        public virtual void Save(IConnectionManager entry, FormType formType)
        {
            var statement = new SqlServerStatement("POSFORMTYPE");

            ValidateSecurity(entry, BusinessObjects.Permission.FormProfileEdit);
            //formType.Validate();

            bool isNew = false;
            if (formType.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                formType.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, formType.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)formType.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)formType.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("DESCRIPTION", formType.Text);
            statement.AddField("SYSTEMTYPE", formType.SystemType, SqlDbType.TinyInt);

            Save(entry, formType, statement);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<FormType>(entry, "POSFORMTYPE", "ID", (Guid)id);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<FormType>(entry, "POSFORMTYPE", "ID", (Guid)id, BusinessObjects.Permission.FormProfileEdit);
        }

        #region ISequenceable Members

        public RecordIdentifier SequenceID
        {
            get { return "POSFORMTYPE"; }
        }

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSFORMTYPE", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
