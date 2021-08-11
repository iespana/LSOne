using System.Collections.Generic;
using System.Data;
using System.Drawing;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.PricesAndDiscounts
{
    /// <summary>
    /// Data provider class for mix and match line groups
    /// </summary>
    public class MixAndMatchLineGroupData : SqlServerDataProviderBase, IMixAndMatchLineGroupData
    {
        /// <summary>
        /// Gets a list of mix of match group lines for a given offer.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <returns>A list of mix and match line groups</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry, RecordIdentifier offerID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =
                    "Select  LINEGROUP, OFFERID, ISNULL(DESCRIPTION,'') AS DESCRIPTION from POSMMLINEGROUPS where DATAAREAID = @dataAreaId and OFFERID = @offerID order by LINEGROUP";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string) offerID);

                return Execute<DataEntity>(entry, cmd, CommandType.Text, "DESCRIPTION", "LINEGROUP");
            }
        }

        private static void PopulateMixAndMatchLineGroup(IDataReader dr, MixAndMatchLineGroup group)
        {
            group.OfferID = (string)dr["OFFERID"];
            group.LineGroup = (string)dr["LINEGROUP"];
            group.NumberOfItemsNeeded = (int)dr["NOOFITEMSNEEDED"];
            group.Color = Color.FromArgb((int)dr["COLOR"]);
            group.Text = (string)dr["DESCRIPTION"];
        }

        /// <summary>
        /// Gets a single mix and match line group by a given ID
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineGroupID">The id of the line group to fetch. Note that this is a double key: OFFERID, LINEGROUP</param>
        /// <returns>The line group entity or null if not found</returns>
        public virtual MixAndMatchLineGroup Get(IConnectionManager entry, RecordIdentifier lineGroupID)
        {
            ValidateSecurity(entry);

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select g.OFFERID,LINEGROUP,ISNULL(g.NOOFITEMSNEEDED,0) as NOOFITEMSNEEDED,ISNULL(g.COLOR,-1) as COLOR, ISNULL(g.DESCRIPTION,'') AS DESCRIPTION  " +
                    "from POSMMLINEGROUPS g " +
                    "where g.DATAAREAID = @dataAreaId and g.OFFERID = @offerID and LINEGROUP = @lineGroup";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string)lineGroupID.PrimaryID);
                MakeParam(cmd, "lineGroup", (string)lineGroupID.SecondaryID);

                var result = Execute<MixAndMatchLineGroup>(entry, cmd, CommandType.Text, PopulateMixAndMatchLineGroup);
                return (result.Count > 0) ? result[0] : null;
            }
        }

        /// <summary>
        /// Gets all mix and match groups for a a given offer.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="offerID">The id of the offer</param>
        /// <param name="sortColumn">The number of the column to sort by. 0 = LINEGROUP, 1 = NOOFITEMSNEEDED</param>
        /// <param name="backwardsSort">True if the sort should be backwards</param>
        /// <returns>List of all mix and match groups found for a given offer.</returns>
        public virtual List<MixAndMatchLineGroup> GetGroups(IConnectionManager entry, RecordIdentifier offerID, int sortColumn, bool backwardsSort)
        {
            string sort = "";

            ValidateSecurity(entry);

            var columns = new[] { "g.LINEGROUP", "g.NOOFITEMSNEEDED" };

            if (sortColumn < columns.Length)
            {
                sort = " order by " + columns[sortColumn] + (backwardsSort ? " DESC" : " ASC");
            }

            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "Select g.OFFERID,LINEGROUP,ISNULL(g.NOOFITEMSNEEDED,0) as NOOFITEMSNEEDED,ISNULL(g.COLOR,0) as COLOR, ISNULL(g.DESCRIPTION,'') AS DESCRIPTION " +
                    "from POSMMLINEGROUPS g " +
                    "where g.DATAAREAID = @dataAreaId and g.OFFERID = @offerID" + sort;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "offerID", (string)offerID);

                return Execute<MixAndMatchLineGroup>(entry, cmd, CommandType.Text, PopulateMixAndMatchLineGroup);
            }
        }

        /// <summary>
        /// Checks if a mix and match group may be deleted. This will return false if there is some line using the group.
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineGroupID">ID of the line group, note this is double key, OFFERID, LINEGROUP</param>
        /// <returns>True if it is safe to delete, else false</returns>
        public virtual bool CanDelete(IConnectionManager entry, RecordIdentifier lineGroupID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT 'x' " +
                    "FROM PERIODICDISCOUNTLINE " +
                    "WHERE OFFERID = @offerID AND LINEGROUP = @lineGroup AND DELETED = 0";

                MakeParam(cmd, "offerID", (string)lineGroupID.PrimaryID);
                MakeParam(cmd, "lineGroup", (string)lineGroupID.SecondaryID);

                ValidateSecurity(entry);

                IDataReader dr = null;
                try
                {
                    dr = entry.Connection.ExecuteReader(cmd, CommandType.Text);

                    return !dr.Read();
                }
                finally
                {
                    if (dr != null)
                    {
                        dr.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if a record with the given ID exists
        /// </summary>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineGroupID">ID of the line group, note this is double key, OFFERID, LINEGROUP</param>
        /// <returns>True if the record exists, else false</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier lineGroupID)
        {
            return RecordExists(entry, "POSMMLINEGROUPS", new[] { "OFFERID", "LINEGROUP" }, lineGroupID);
        }

        private static bool LineGroupExists(IConnectionManager entry, RecordIdentifier lineGroupID)
        {
            return RecordExists(entry, "POSMMLINEGROUPS", "LINEGROUP", lineGroupID);
        }

        /// <summary>
        /// Deletes a mix and match line group from the database
        /// </summary>
        /// /// <remarks>Note that Manage discounts permission is needed for this operation</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="lineGroupID">ID of the line group, note this is double key, OFFERID, LINEGROUP</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier lineGroupID)
        {
            if (CanDelete(entry, lineGroupID))
            {
                DeleteRecord(entry, "POSMMLINEGROUPS", new[] { "OFFERID", "LINEGROUP" }, lineGroupID, BusinessObjects.Permission.ManageDiscounts);
            }
        }

        /// <summary>
        /// Saves a mix and match line group into the database. This function can handle both update and inserts.
        /// </summary>
        /// <remarks>Note that Manage discounts permission is needed for this operation</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="group">The mix and match group to save</param>
        public virtual void Save(IConnectionManager entry, MixAndMatchLineGroup group)
        {
            var statement = new SqlServerStatement("POSMMLINEGROUPS");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageDiscounts);

            //if (group.ID.SecondaryID == RecordIdentifier.Empty)
            bool isNew = false;
            if (group.LineGroup == RecordIdentifier.Empty)
            {
                isNew = true;
                group.LineGroup = DataProviderFactory.Instance.GenerateNumber<IMixAndMatchLineGroupData, MixAndMatchLineGroup>(entry);
            }

            if (isNew || !Exists(entry, group.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("OFFERID", (string)group.ID.PrimaryID);
                statement.AddKey("LINEGROUP", (string)group.ID.SecondaryID);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("OFFERID", (string)group.ID.PrimaryID);
                statement.AddCondition("LINEGROUP", (string)group.ID.SecondaryID);
            }

            statement.AddField("NOOFITEMSNEEDED", group.NumberOfItemsNeeded, SqlDbType.Int);
            statement.AddField("COLOR", group.Color.ToArgb(), SqlDbType.Int);
            statement.AddField("DESCRIPTION", group.Text);

            entry.Connection.ExecuteStatement(statement);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return LineGroupExists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "POSMMLINEGROUPS"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSMMLINEGROUPS", "LINEGROUP", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
