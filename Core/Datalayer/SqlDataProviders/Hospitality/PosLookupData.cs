using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Hospitality
{
    public class PosLookupData : SqlServerDataProviderBase, IPosLookupData
    {
        private static string BaseSelectString
        {
            get
            {
                return "select " +
                    "LOOKUPID, " +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION, " +
                    "ISNULL(DYNAMICMENUID,'') as DYNAMICMENUID, " +
                    "ISNULL(DYNAMICMENU2ID,'') as DYNAMICMENU2ID, " +
                    "ISNULL(GRID1MENUID,'') as GRID1MENUID, " +
                    "ISNULL(GRID2MENUID,'') as GRID2MENUID " +
                    "from POSLOOKUP ";
            }
        }

        private static string ResolveSort(PosLookupSorting sort, bool backwards)
        {
            switch (sort)
            {
                case PosLookupSorting.LookupID:
                    return backwards ? "LOOKUPID DESC" : "LOOKUPID ASC";

                case PosLookupSorting.Description:
                    return backwards ? "DESCRIPTION DESC" : "DESCRIPTION ASC";
            }

            return "";
        }

        private static void PopulatePosLookup(IDataReader dr, PosLookup posLookup)
        {
            posLookup.ID = (string)dr["LOOKUPID"];
            posLookup.Text = (string)dr["DESCRIPTION"];
            posLookup.DynamicMenuID = (string)dr["DYNAMICMENUID"];
            posLookup.DynamicMenu2ID = (string)dr["DYNAMICMENU2ID"];
            posLookup.Grid1MenuID = (string)dr["GRID1MENUID"];
            posLookup.Grid2MenuID = (string)dr["GRID2MENUID"];
        }

        /// <summary>
        /// Gets a list of all pos lookups
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of all pos lookup recrods</returns>
        public virtual List<PosLookup> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosLookup>(entry, cmd, CommandType.Text, PopulatePosLookup);
            }
        }

        /// <summary>
        /// Gets a list of all pos lookups ordered by the specific field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted </param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of PosLookup objects conataining all pos lookup records, ordered by the chosen field</returns>
        public virtual List<PosLookup> GetList(IConnectionManager entry, PosLookupSorting sortBy, bool sortBackwards)
        {
            if (sortBy != PosLookupSorting.LookupID && sortBy != PosLookupSorting.Description)
            {
                throw new NotSupportedException();
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId " +
                    "order by " + ResolveSort(sortBy, sortBackwards);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosLookup>(entry, cmd, CommandType.Text, PopulatePosLookup);
            }
        }

        /// <summary>
        /// Gets a pos lookup with the given ID
        /// </summary>
        /// <param name="entry">The entyr into the database</param>
        /// <param name="posLookupID">The id of the pos lookup to get</param>
        /// <returns>The pos lookup with the given ID, or null if it is not found</returns>
        public virtual PosLookup Get(IConnectionManager entry, RecordIdentifier posLookupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where DATAAREAID = @dataAreaId and LOOKUPID = @lookupId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "lookupId", (string) posLookupID);

                var result = Execute<PosLookup>(entry, cmd, CommandType.Text, PopulatePosLookup);

                return result.Count > 0 ? result[0] : null;
            }
        }

        /// <summary>
        /// Checks if a pos lookup with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posLookupID">The ID of the pos lookup to check for</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier posLookupID)
        {
            return RecordExists(entry, "POSLOOKUP", "LOOKUPID", posLookupID);
        }

        /// <summary>
        /// Deletes the pos lookup with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="posLookupID">The ID of the pos lookup to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier posLookupID)
        {
            DeleteRecord(entry, "POSLOOKUP", "LOOKUPID", posLookupID, BusinessObjects.Permission.ManageMenuGroups);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="posLookup"></param>
        public virtual void Save(IConnectionManager entry, PosLookup posLookup)
        {
            var statement = new SqlServerStatement("POSLOOKUP");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageMenuGroups);

            bool isNew = false;
            if (posLookup.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                posLookup.ID = DataProviderFactory.Instance.GenerateNumber<IPosLookupData,PosLookup>(entry);
            }

            if (isNew || !Exists(entry, posLookup.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("LOOKUPID", (string)posLookup.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("LOOKUPID", (string)posLookup.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
            }

            statement.AddField("DESCRIPTION", posLookup.Text);
            statement.AddField("DYNAMICMENUID", (string)posLookup.DynamicMenuID);
            statement.AddField("DYNAMICMENU2ID", (string)posLookup.DynamicMenu2ID);
            statement.AddField("GRID1MENUID", (string)posLookup.Grid1MenuID);
            statement.AddField("GRID2MENUID", (string)posLookup.Grid2MenuID);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "POSLOOKUP"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSLOOKUP", "LOOKUPID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
