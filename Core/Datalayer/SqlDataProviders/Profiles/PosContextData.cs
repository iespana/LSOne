using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Profiles
{
    /// <summary>
    /// A data provider for the <see cref="PosContext"/> business object
    /// </summary>
    public class PosContextData : SqlServerDataProviderBase, IPosContextData
    {
        private static string BaseSql
        {
            get
            {
                return " SELECT DISTINCT C.ID, ISNULL(C.NAME,'') AS NAME," +
                       " ISNULL(C.MENUREQUIRED,0) AS MENUREQUIRED, " +
                       " ISNULL(S.CONTEXTID, '') CONTEXTID " +
                       " FROM POSCONTEXT C " +
                       " LEFT JOIN POSSTYLEPROFILELINE S ON C.ID = S.CONTEXTID ";
            }
        }

        /// <summary>
        /// Gets a list of all Contexts
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <returns>A list of all Contexts</returns>
        public virtual List<PosContext> GetList(IConnectionManager entry)
        {
            return GetList(entry, "NAME");
        }

        /// <summary>
        /// Gets a list of all Contexts
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="sort">How to sort list</param>
        /// <param name="cache">The selected cache</param>
        /// <returns>A list of all Contexts sorted in a particular order</returns>
        public virtual List<PosContext> GetList(IConnectionManager entry, string sort, CacheType cache = CacheType.CacheTypeNone)
        {            
            return GetList(entry, RecordIdentifier.Empty, sort, cache);
        }

        /// <summary>
        /// Gets a list of all Contexts associated with id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">Get contexts associated with this id. If empty then all contexts are returned</param>
        /// <param name="sort">How to sort the list</param>
        /// <param name="cache">The selected cache</param>
        /// <returns>A list of all Contexts associated with a particular id</returns>
        public virtual List<PosContext> GetList(IConnectionManager entry, RecordIdentifier id, string sort = "", CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSql +
                       "WHERE C.DATAAREAID = @dataAreaId ";
                
                if (id != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " AND C.ID = @id ORDER BY ID";
                    MakeParam(cmd, "id", id);
                }

                if (sort != "")
                {
                    cmd.CommandText += " ORDER BY " + sort;
                }
                
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosContext>(entry, cmd, CommandType.Text, PopulateProfile);
            }
        }
                
        private static void PopulateProfile(IDataReader dr, PosContext profile)
        {
            profile.ID = (string)dr["ID"];
            profile.Text = (string)dr["NAME"];
            profile.MenuRequired = ((byte)dr["MENUREQUIRED"] != 0 );
            profile.UsedInStyleProfile = ((string)dr["CONTEXTID"] != "");
        }

        /// <summary>
        /// Gets a context with a particular id
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">get context with this id</param>
        /// <param name="cache">cachetype</param>
        /// <returns>The <see cref="PosContext"/> found</returns>
        public virtual PosContext Get(IConnectionManager entry, RecordIdentifier id, CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText = BaseSql +                       
                       "WHERE C.DATAAREAID = @dataAreaId AND C.ID = @id";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "id", (string)id);

                return Get<PosContext>(entry, cmd, id, PopulateProfile, cache, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Returns true if the Context ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID to check</param>
        /// <returns>True if the ID exists otherwise false is returned</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists<PosContext>(entry, "POSCONTEXT", "ID", id);
        }

        /// <summary>
        /// Deletes a specific Context
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="id">The ID to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord<PosContext>(entry, "POSCONTEXT", "ID", id, BusinessObjects.Permission.ContextEdit);
        }        

        /// <summary>
        /// Saves an instance of <see cref="PosContext"/> to the database
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// /// <param name="context">The <see cref="PosContext"/> that is to be saved</param>
        public virtual void Save(IConnectionManager entry, PosContext context)
        {
            var statement = new SqlServerStatement("POSCONTEXT");

            ValidateSecurity(entry, BusinessObjects.Permission.ContextEdit);
            context.Validate();

            bool isNew = false;
            if (context.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                context.ID = DataProviderFactory.Instance.GenerateNumber<IPosContextData, PosContext>(entry);
            }

            if (isNew || !Exists(entry, context.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)context.ID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)context.ID);
            }

            statement.AddField("NAME", context.Text);
            statement.AddField("MENUREQUIRED", context.MenuRequired ? 1 : 0, SqlDbType.TinyInt);

            Save(entry, context, statement);
        }

        #region ISequenceable Members

        /// <summary>
        /// Returns true if the sequence exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The sequence ID to check</param>
        /// <returns>True if the sequence exists otherwise false is returned</returns>
        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        /// <summary>
        /// The SequenceID name used for <see cref="PosContext"/>
        /// </summary>
        public RecordIdentifier SequenceID
        {
            get { return "POSCONTEXT"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSCONTEXT", "ID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
