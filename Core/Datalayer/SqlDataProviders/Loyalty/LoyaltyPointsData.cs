using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Loyalty
{
    public class LoyaltyPointsData : SqlServerDataProviderBase, ILoyaltyPointsData
    {
        private static string BaseSelectString
        {
            get
            {
                return
                    "SELECT ISNULL(t.LOYALTYSCHEMEID, '') LOYALTYSCHEMEID"
                    + ", ISNULL(t.LOYALTYRULEID, '00000000-0000-0000-0000-000000000000') LOYALTYRULEID"
                    + ", ISNULL(t.TYPE, 0) TYPE"
                    + ", CASE T.TYPE"
                    + "     WHEN 0 THEN 0  " // Retail item
                    + "     WHEN 6 THEN 1  " // Special group
                    + "     WHEN 1 THEN 2  " // Retail group 
                    + "     WHEN 2 THEN 3  " // Retail department 
                    + "     WHEN 5 THEN 4  " // Tender	
                    + "  END AS TOCHECKPRIORITY "
                    + ", ISNULL(t.SCHEMERELATION, '') SCHEMERELATION"
                    + ", COALESCE(inv.ITEMNAME,gr.NAME,dep.NAME,disc.DESCRIPTION,paym.NAME,specgr.NAME,'') as RELATIONNAME"
                    + ", ISNULL(t.QTYAMOUNTLIMIT, 0) QTYAMOUNTLIMIT"
                    + ", ISNULL(t.POINTS, 0) POINTS"
                    + ", ISNULL(t.CUSTOMERGROUP_EDIT, '') CUSTOMERGROUP_EDIT"
                    + ", ISNULL(t.CONTACTSEGMENT_EDIT, '') CONTACTSEGMENT_EDIT"
                    + ", ISNULL(t.BASECALCULATIONON, 0) BASECALCULATIONON"
                    + ", ISNULL(t.STARTINGDATE, '01.01.1900') STARTINGDATE"
                    + ", ISNULL(t.ENDINGDATE, '01.01.1900') ENDINGDATE"

                    + ", t.[MODIFIEDDATE] MODIFIEDDATE"
                    + ", t.[MODIFIEDTIME] MODIFIEDTIME"
                    + ", ISNULL(t.[MODIFIEDBY], '00000000-0000-0000-0000-000000000000') MODIFIEDBY"
                    + ", t.[CREATEDDATE] CREATEDDATE"
                    + ", t.[CREATEDTIME] CREATEDTIME"
                    + ", ISNULL(t.[CREATEDBY], '00000000-0000-0000-0000-000000000000') CREATEDBY"

                    + " FROM RBOLOYALTYPOINTSTABLE t "
                    + " LEFT OUTER JOIN RETAILITEM INV ON T.TYPE = 0 AND INV.ITEMID = T.SCHEMERELATION "
                    + " LEFT OUTER JOIN RETAILGROUP GR ON T.TYPE = 1 AND GR.GROUPID = T.SCHEMERELATION "
                    + " LEFT OUTER JOIN RETAILDEPARTMENT DEP ON T.TYPE = 2 AND DEP.DEPARTMENTID = T.SCHEMERELATION "
                    + " left outer join PERIODICDISCOUNT disc on t.TYPE >= 3 and t.TYPE <= 4 and disc.OFFERID = t.SCHEMERELATION "
                    + " left outer join RBOTENDERTYPETABLE paym on t.TYPE = 5 and paym.TENDERTYPEID = t.SCHEMERELATION and paym.DATAAREAID = t.DATAAREAID "
                    + " left outer join SPECIALGROUP specgr on t.TYPE = 6 and specgr.GROUPID = t.SCHEMERELATION ";
            }
        }

        private static void PopulateLoyaltyPoints(IDataReader dr, LoyaltyPoints points)
        {
            points.SchemeID = (string)dr["LOYALTYSCHEMEID"];
            points.RuleID = (Guid)dr["LOYALTYRULEID"];
            points.Type = (LoyaltyPointTypeBase)dr["TYPE"];
            points.SchemeRelation = (string)dr["SCHEMERELATION"];
            points.SchemeRelationName = (string)dr["RELATIONNAME"];
            points.QtyAmountLimit = (decimal)dr["QTYAMOUNTLIMIT"];
            points.Points = (decimal)dr["POINTS"];
            points.BaseCalculationOn = (CalculationTypeBase)dr["BASECALCULATIONON"];
            points.StartingDate = Date.FromAxaptaDate(dr["STARTINGDATE"]);
            points.EndingDate = Date.FromAxaptaDate(dr["ENDINGDATE"]);
        }

        /// <summary>
        /// Gets the specified rule line.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="loyaltyPointsLineID">The loyalty points line's full ID that is ShemesID + RuleID.</param>
        /// <returns>An instance of <see cref="LoyaltyPoints"/></returns>
        public virtual LoyaltyPoints Get(IConnectionManager entry, RecordIdentifier loyaltyPointsLineID)
        {
            List<LoyaltyPoints> result;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "WHERE t.LOYALTYSCHEMEID = @schemesID AND t.LOYALTYRULEID = @ruleID AND t.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "schemesID", (string)loyaltyPointsLineID.PrimaryID);
                MakeParam(cmd, "ruleID", (Guid)loyaltyPointsLineID.SecondaryID, SqlDbType.UniqueIdentifier);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltyPoints>(entry, cmd, CommandType.Text, PopulateLoyaltyPoints);
            }

            return result.Count > 0 ? result[0] : null;
        }

        /// <summary>
        /// Gets a list of rules for a given scheme.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemeID">The scheme ID.</param>
        /// <returns>A list of instances of <see cref="LoyaltyPoints"/></returns>
        public virtual List<LoyaltyPoints> GetList(IConnectionManager entry, RecordIdentifier schemeID)
        {
            List<LoyaltyPoints> result;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "WHERE t.LOYALTYSCHEMEID = @schemesID AND t.DATAAREAID = @dataAreaId " +
                    "ORDER BY TOCHECKPRIORITY ASC";

                MakeParam(cmd, "schemesID", (string)schemeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LoyaltyPoints>(entry, cmd, CommandType.Text, PopulateLoyaltyPoints);
            }

            return result;
        }

        /// <summary>
        /// returns exchange rate - how much money for one loyalty point
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="schemeID">The scheme ID</param>
        /// <returns></returns>
        public virtual LoyaltyPoints GetPointsExchangeRate(IConnectionManager entry, RecordIdentifier schemeID)
        {
            if ((schemeID == null) || ((string)schemeID == ""))
            {
                return null;
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText =
                    @" SELECT LOYALTYSCHEMEID, QTYAMOUNTLIMIT, POINTS, STARTINGDATE, ENDINGDATE
                       FROM RBOLOYALTYPOINTSTABLE t  
                       JOIN RBOSTORETENDERTYPETABLE SP ON T.SCHEMERELATION = SP.TENDERTYPEID AND T.DATAAREAID = SP.DATAAREAID 
                       WHERE t.LOYALTYSCHEMEID = @schemesID 
                       AND t.DATAAREAID = @dataAreaId 
                       AND (t.TYPE = 5)                        
                       AND (t.QTYAMOUNTLIMIT > 0)
                       AND SP.FUNCTION_ = 1 
                       AND SP.POSOPERATION = 207
                       ORDER BY STARTINGDATE, ENDINGDATE";

                MakeParam(cmd, "schemesID", (string)schemeID);
                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                List<LoyaltyPoints> result = Execute<LoyaltyPoints>(entry, cmd, CommandType.Text, PopulateTenderRules);
                if ((result != null) && (result.Count > 0))
                {
                    LoyaltyPoints tenderrule =
                        result.FirstOrDefault(
                            f =>
                            f.StartingDate.DateTime.Date <= DateTime.Now.Date &&
                            (f.EndingDate == Date.Empty || f.EndingDate.DateTime.Date > DateTime.Now.Date));

                    return tenderrule;
                }
            }

            return null;
        }

        private static void PopulateTenderRules(IDataReader dr, LoyaltyPoints points)
        {
            points.SchemeID = (string)dr["LOYALTYSCHEMEID"];
            points.QtyAmountLimit = (decimal)dr["QTYAMOUNTLIMIT"];
            points.Points = (decimal)dr["POINTS"];
            points.StartingDate = Date.FromAxaptaDate(dr["STARTINGDATE"]);
            points.EndingDate = Date.FromAxaptaDate(dr["ENDINGDATE"]);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier loyaltyPointsLineID)
        {
            return RecordExists(entry, "RBOLOYALTYPOINTSTABLE", new string[] { "LOYALTYSCHEMEID", "LOYALTYRULEID" }, loyaltyPointsLineID);
        }

        /// <summary>
        /// Delete a rule given by its full ID
        /// </summary>
        /// <param name="entry">Entry into datebase</param>
        /// <param name="loyaltyPointsLineID">ID of the loyalty points rule line that is ShemesID + RuleID</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier loyaltyPointsLineID)
        {
            DeleteRecord(entry, "RBOLOYALTYPOINTSTABLE",
                new string[] { "LOYALTYSCHEMEID", "LOYALTYRULEID" },
                loyaltyPointsLineID, BusinessObjects.Permission.SchemesEdit);
        }

        public virtual void Save(IConnectionManager entry, LoyaltyPoints loyaltyPoint)
        {
            ValidateSecurity(entry, BusinessObjects.Permission.SchemesEdit);

            if (loyaltyPoint.SchemeID == RecordIdentifier.Empty)
            {
                throw new Exception(LSOne.DataLayer.BusinessObjects.Properties.Resources.SchemeCannotBeEmpty);
            }

            loyaltyPoint.Validate();

            bool isNew = false;
            SqlServerStatement statement = new SqlServerStatement("RBOLOYALTYPOINTSTABLE");
            if ((loyaltyPoint.RuleID == null) || loyaltyPoint.RuleID.IsEmpty || (Guid)loyaltyPoint.RuleID == Guid.Empty)
            {
                isNew = true;
                loyaltyPoint.RuleID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, loyaltyPoint.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("LOYALTYSCHEMEID", (string)loyaltyPoint.SchemeID);
                statement.AddKey("LOYALTYRULEID", (Guid)loyaltyPoint.RuleID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("LOYALTYSCHEMEID", (string)loyaltyPoint.SchemeID);
                statement.AddCondition("LOYALTYRULEID", (Guid)loyaltyPoint.RuleID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("TYPE", loyaltyPoint.Type, SqlDbType.Int);
            statement.AddField("SCHEMERELATION", loyaltyPoint.SchemeRelation.ToString());
            statement.AddField("QTYAMOUNTLIMIT", loyaltyPoint.QtyAmountLimit, SqlDbType.Decimal);
            statement.AddField("POINTS", loyaltyPoint.Points, SqlDbType.Decimal);
            statement.AddField("BASECALCULATIONON", loyaltyPoint.BaseCalculationOn, SqlDbType.Int);
            statement.AddField("STARTINGDATE", loyaltyPoint.StartingDate.ToAxaptaSQLDate().Date, SqlDbType.Date);
            statement.AddField("ENDINGDATE", loyaltyPoint.EndingDate.ToAxaptaSQLDate().Date, SqlDbType.Date);

            RecordIdentifier userID = entry.CurrentUser == null ? RecordIdentifier.Empty : entry.CurrentUser.ID;
            statement.AddField("MODIFIEDDATE", DateTime.Now, SqlDbType.DateTime);
            statement.AddField("MODIFIEDTIME", DateTime.Now.TimeOfDay.TotalSeconds, SqlDbType.Int);
            if (userID == RecordIdentifier.Empty)
            {
                statement.AddField("MODIFIEDBY", DBNull.Value, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.AddField("MODIFIEDBY", (Guid)userID, SqlDbType.UniqueIdentifier);
            }

            if (isNew)
            {
                statement.AddField("CREATEDDATE", DateTime.Now, SqlDbType.DateTime);
                statement.AddField("CREATEDTIME", DateTime.Now.TimeOfDay.TotalSeconds, SqlDbType.Int);
                if (userID == RecordIdentifier.Empty)
                {
                    statement.AddField("CREATEDBY", DBNull.Value, SqlDbType.UniqueIdentifier);
                }
                else
                {
                    statement.AddField("CREATEDBY", (Guid)userID, SqlDbType.UniqueIdentifier);
                }
            }

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void CopyRules(IConnectionManager entry, RecordIdentifier copyFrom, RecordIdentifier copyTo)
        {
            copyFrom = copyFrom == "" ? RecordIdentifier.Empty : copyFrom;
            if (copyFrom == RecordIdentifier.Empty)
            {
                return;
            }

            List<LoyaltyPoints> copyFromList = GetList(entry, copyFrom);

            foreach (LoyaltyPoints copy in copyFromList)
            {
                copy.RuleID = RecordIdentifier.Empty;
                copy.SchemeID = copyTo;
                Save(entry, copy);
            }
        }
    }
}
