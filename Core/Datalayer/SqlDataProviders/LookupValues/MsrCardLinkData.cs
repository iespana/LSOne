using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.LookupValues
{
    public class MsrCardLinkData : SqlServerDataProviderBase, IMsrCardLinkData
    {
        private static string BaseSelectString
        {
            get
            {
                return
                    "select r.CARDNUMBER as CARDNUMBER, " +
                    "ISNULL(r.LINKTYPE,0) as LINKTYPE, " +
                    "ISNULL(r.LINKID,'') as LINKID, " +
                    "ISNULL(s.NAME,'') as POSUSERNAME,  " +
                    "ISNULL(u.LOGIN,'') as POSUSERLOGIN, " +
                    "ISNULL(c.NAME,'') as CUSTOMERNAME " +
                    "from RBOMSRCARDTABLE r " +
                    "left outer join RBOSTAFFTABLE s on s.STAFFID = r.LINKID and s.DATAAREAID = r.DATAAREAID " +
                    "left outer join USERS u on u.STAFFID = r.LINKID and u.DATAAREAID = r.DATAAREAID " +
                    "left outer join CUSTOMER c on c.ACCOUNTNUM = r.LINKID and c.DATAAREAID = r.DATAAREAID ";
            }
        }

        private static void PopulateMsrCardLink(IDataReader dr, MsrCardLink msrCardLink)
        {
            msrCardLink.ID = (string)dr["CARDNUMBER"];
            msrCardLink.LinkID = (string)dr["LINKID"];
            msrCardLink.LinkType = (MsrCardLink.LinkTypeEnum)((int)dr["LINKTYPE"]);

            switch (msrCardLink.LinkType)
            {
                case MsrCardLink.LinkTypeEnum.Customer:
                    msrCardLink.Text = (string)dr["CUSTOMERNAME"];
                    break;

                case MsrCardLink.LinkTypeEnum.POSUser:
                    msrCardLink.Text = (string)dr["POSUSERLOGIN"] + " - " + dr["POSUSERNAME"];
                    break;
            }
        }

        /// <summary>
        /// Gets a card link with the given ID (card number)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="msrCardLinkID">The ID of the card link (the card number)</param>
        public virtual MsrCardLink Get(IConnectionManager entry, RecordIdentifier msrCardLinkID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where r.CARDNUMBER = @cardNumber and r.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "cardNumber", (string)msrCardLinkID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                var result = Execute<MsrCardLink>(entry, cmd, CommandType.Text, PopulateMsrCardLink);
                return result.Count > 0 ? result[0] : null;
            }
        }
        
        /// <summary>
        /// Gets all card links for the given link type (employee or customer)
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="linkType">The link type</param>
        /// <returns></returns>
        public virtual List<MsrCardLink> GetList(IConnectionManager entry, MsrCardLink.LinkTypeEnum linkType)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where r.LINKTYPE = @linkType and r.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "linkType", (int)linkType, SqlDbType.Int);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<MsrCardLink>(entry, cmd, CommandType.Text, PopulateMsrCardLink);
            }
        }

        /// <summary>
        /// Gets a card link for a specific card link and uses the link type as a filter. 
        /// </summary>
        /// <param name="entry">Entry into the database.</param>
        /// <param name="msrCardLinkID">The ID of the card link (the card number).</param>
        /// <param name="linkType">The link type.</param>
        /// <param name="cache">Optional parameter to specify if cache may be used.</param>
        /// <returns>The MsrCardLink that matches the given parameters.</returns>
        public MsrCardLink Get(IConnectionManager entry, RecordIdentifier msrCardLinkID, MsrCardLink.LinkTypeEnum linkType, 
            CacheType cache = CacheType.CacheTypeNone)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    @"WHERE r.LINKTYPE = @linkType 
                    and r.DATAAREAID = @dataAreaID
                    and r.CARDNUMBER = @cardID";
                MakeParam(cmd, "linkType", (int)linkType, SqlDbType.Int);
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "cardID", msrCardLinkID);
                return Get<MsrCardLink>(entry, cmd, msrCardLinkID, PopulateMsrCardLink, cache, UsageIntentEnum.Normal);
            }
        }

        /// <summary>
        /// Checkd if a card link exists for the gifen id
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The id to check for</param>
        /// <returns></returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "RBOMSRCARDTABLE", "CARDNUMBER", id);
        }

        /// <summary>
        /// Deletes a card link with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="id">The ID of the card link to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "RBOMSRCARDTABLE", "CARDNUMBER", id, BusinessObjects.Permission.ManageMsrCardLinks);
        }

        /// <summary>
        /// Saves a card link into the database. A new record is created if the ID does not exist, otherwise an existing record is updated.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="msrCardLink"></param>
        public virtual void Save(IConnectionManager entry, MsrCardLink msrCardLink)
        {
            var statement = new SqlServerStatement("RBOMSRCARDTABLE");

            if (!Exists(entry, msrCardLink.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("CARDNUMBER", (string)msrCardLink.ID);
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);                
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("CARDNUMBER", (string)msrCardLink.ID);
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);                
            }

            statement.AddField("LINKTYPE", (int)msrCardLink.LinkType, SqlDbType.Int);
            statement.AddField("LINKID", (string)msrCardLink.LinkID);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
