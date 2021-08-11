using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Infocodes
{
    public class InfocodeSpecificData : SqlServerDataProviderBase, IInfocodeSpecificData
    {
        private static string BaseSelectString
        {
            get
            {
                return
                @"SELECT   
                  i.INFOCODEID,i.REFRELATION,
                  ISNULL(i.REFRELATION2,'') AS REFRELATION2,
                  ISNULL(i.REFRELATION3,'') AS REFRELATION3,
                  ISNULL(i.SEQUENCE,0) AS SEQUENCE,
                  ISNULL(i.TRIGGERING,0) AS TRIGGERING,
                  ISNULL(i.UNITOFMEASURE,'') AS UNITOFMEASURE,
                  ISNULL(i.SALESTYPEFILTER,'') AS SALESTYPEFILTER,
                  ISNULL(i.USAGECATEGORY,0) AS USAGECATEGORY,
                  ISNULL(j.DESCRIPTION,'') AS INFOCODEDESCRIPTION,
                  ISNULL(i.REFTABLEID,0) AS REFTABLEID,
                  ISNULL(j.INPUTTYPE,0) AS INFOCODEINPUTTYPE,
                  ISNULL(i.INPUTREQUIRED,0) AS INPUTREQUIRED, 
                  REFRELATIONDESCRIPTION = 
	                  CASE i.REFTABLEID
		                  WHEN 0 THEN ''
		                  WHEN 1 THEN ISNULL (it.ITEMNAME, '')
		                  WHEN 2 THEN ISNULL (ct.NAME, '')
		                  WHEN 3 THEN ISNULL (stt.NAME, '')
                          WHEN 4 THEN ISNULL (sttct.NAME, '')
                          WHEN 5 THEN ISNULL (ieat.NAME, '')
                          WHEN 6 THEN ISNULL (iid.NAME, '')
                          WHEN 7 THEN ISNULL (iirg.NAME, '')
                          WHEN 8 THEN ISNULL (pfp.NAME, '')
                          WHEN 9 THEN ISNULL (pfp.NAME, '')
                          ELSE ''
	                  END
                  FROM RBOINFOCODETABLESPECIFIC i 
                  LEFT OUTER JOIN RBOINFOCODETABLE j ON i.INFOCODEID = j.INFOCODEID
                  LEFT JOIN RETAILITEM it ON it.ITEMID = i.REFRELATION
                  LEFT JOIN CUSTOMER ct ON ct.ACCOUNTNUM = i.REFRELATION
                  LEFT JOIN RBOSTORETENDERTYPETABLE stt ON stt.STOREID = i.REFRELATION AND stt.TENDERTYPEID = i.REFRELATION2
                  LEFT JOIN RBOSTORETENDERTYPECARDTABLE sttct ON sttct.STOREID = i.REFRELATION AND sttct.TENDERTYPEID = i.REFRELATION2 AND sttct.CARDTYPEID = i.REFRELATION3
                  LEFT JOIN RBOINCOMEEXPENSEACCOUNTTABLE ieat ON ieat.ACCOUNTNUM = i.REFRELATION AND ieat.STOREID = i.REFRELATION2
                  LEFT JOIN RETAILDEPARTMENT iid ON iid.DEPARTMENTID = i.REFRELATION
                  LEFT JOIN RETAILGROUP iirg ON iirg.GROUPID = i.REFRELATION 
                  LEFT JOIN POSFUNCTIONALITYPROFILE pfp ON pfp.PROFILEID = i.REFRELATION ";
            }
        }

        private static string ResolveSort(InfocodeSpecificSorting sort, bool backwards)
        {
            switch (sort)
            {
                case InfocodeSpecificSorting.InfocodeID:
                    return backwards ? "INFOCODEID DESC" : "INFOCODEID ASC";

                case InfocodeSpecificSorting.InfocodeDescription:
                    return backwards ? "INFOCODEDESCRIPTION DESC" : "INFOCODEDESCRIPTION ASC";
            }

            return "";
        }

        private static void PopulateInfocodeSpecific(IDataReader dr, InfocodeSpecific infocodeSpecific)
        {
            infocodeSpecific.InfocodeId = (string)dr["INFOCODEID"];
            infocodeSpecific.RefRelation = (string)dr["REFRELATION"];
            infocodeSpecific.RefRelation2 = (string)dr["REFRELATION2"];
            infocodeSpecific.RefRelation3 = (string)dr["REFRELATION3"];
            infocodeSpecific.Sequence = (int)dr["SEQUENCE"];
            infocodeSpecific.Triggering = (TriggeringEnum)((byte)dr["TRIGGERING"]);
            infocodeSpecific.UnitOfMeasure = (string)dr["UNITOFMEASURE"];
            infocodeSpecific.SalesTypeFilter = (string)dr["SALESTYPEFILTER"];
            infocodeSpecific.UsageCategory = (UsageCategoriesEnum)((int)dr["USAGECATEGORY"]);
            infocodeSpecific.InfocodeDescription = (string)dr["INFOCODEDESCRIPTION"];
            infocodeSpecific.InfocodeInputType = (InputTypesEnum)((int)dr["INFOCODEINPUTTYPE"]);
            infocodeSpecific.RefTableId = (RefTableEnum)dr["REFTABLEID"];
            infocodeSpecific.InputRequired = ((Byte)dr["INPUTREQUIRED"] != 0);
            infocodeSpecific.RefRelationDescription = (string)dr["REFRELATIONDESCRIPTION"];
        }

        /// <summary>
        /// Gets the infocodeSpeficic with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeSpecificID">The ID of the infocodeSpecific to get(InfocodeID,RefRelation,RefRelation2,RefRelation3)</param>
        /// <returns>The infocodeSpecific with the given ID</returns>
        public virtual InfocodeSpecific Get(IConnectionManager entry, RecordIdentifier infocodeSpecificID)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where i.DATAAREAID = @dataAreaId and i.INFOCODEID = @infocodeId and i.REFRELATION = @refRelation";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "infocodeId", (string) infocodeSpecificID[0]);
                MakeParam(cmd, "refRelation", (string) infocodeSpecificID[1]);

                if (infocodeSpecificID[2] != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " and i.REFRELATION2 = @refRelation2";
                    MakeParam(cmd, "refRelation2", (string) infocodeSpecificID[2]);
                }

                if (infocodeSpecificID[3] != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " and i.REFRELATION3 = @refRelation3";
                    MakeParam(cmd, "refRelation3", (string) infocodeSpecificID[3]);
                }

                var result = Execute<InfocodeSpecific>(entry, cmd, CommandType.Text, PopulateInfocodeSpecific);
                return result.Count > 0 ? result[0] : null;
            }
        }       

        /// <summary>
        /// Gets all infocodeSpecific records for a specific usage category ordered by a specified field
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="refRelationID">The ID of the RefRelation to get fo(RefRelation,RefRelation2,RefRelation3)</param>
        /// <param name="usageCategory">The usage category to filter by</param>
        /// <param name="refTable">The table that we are referencing. This determines what our infocode is attached to</param>
        /// <param name="sortBy">A sort enum that defines how the result should be sorted</param>
        /// <param name="sortBackwards">Determines in what order the result is sorted</param>
        /// <returns>A list of all infocodeSpecific for a specific usage category</returns>
        public List<InfocodeSpecific> GetListForRefRelation(
            IConnectionManager entry, 
            RecordIdentifier refRelationID, 
            UsageCategoriesEnum usageCategory, 
            RefTableEnum refTable,
            InfocodeSpecificSorting sortBy,
            bool sortBackwards)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where i.DATAAREAID = @dataAreaId and i.REFRELATION = @refRelation and i.USAGECATEGORY = @usageCategory and i.REFTABLEID = @refTable";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "refRelation", (string) refRelationID[0]);
                MakeParam(cmd, "usageCategory", (int) usageCategory);
                MakeParam(cmd, "refTable", (int) refTable, SqlDbType.Int);

                if (refRelationID[1] != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " and i.REFRELATION2 = @refRelation2";
                    MakeParam(cmd, "refRelation2", (string) refRelationID[1]);
                }

                if (refRelationID[2] != RecordIdentifier.Empty)
                {
                    cmd.CommandText += " and i.REFRELATION3 = @refRelation3";
                    MakeParam(cmd, "refRelation3", (string) refRelationID[2]);
                }

                cmd.CommandText += " order by " + ResolveSort(sortBy, sortBackwards);

                return Execute<InfocodeSpecific>(entry, cmd, CommandType.Text, PopulateInfocodeSpecific);
            }
        }

        /// <summary>
        /// Checks if an infocodeSpecific with the given ID exists
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeSpecificID">The ID fo the infocodeSpecific to look for(InfocodeID, RefRelation, RefRelation2, RefRelation3)</param>
        /// <returns>True if the record exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier infocodeSpecificID)
        {
            return RecordExists(entry, "RBOINFOCODETABLESPECIFIC", new[] { "INFOCODEID", "REFRELATION", "REFRELATION2", "REFRELATION3", "REFTABLEID" }, infocodeSpecificID);
        }

        /// <summary>
        /// Deletes an infocodeSpecific record with the given ID
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeSpecificID">The id of the infocodeSpecific to delete</param>
        public virtual void Delete(IConnectionManager entry, RecordIdentifier infocodeSpecificID)
        {
            if (infocodeSpecificID[3] != RecordIdentifier.Empty)
            {
                DeleteRecord(entry, "RBOINFOCODETABLESPECIFIC", new[] { "INFOCODEID", "REFRELATION", "REFRELATION2", "REFRELATION3" }, infocodeSpecificID, BusinessObjects.Permission.InfocodeEdit);
            }
            else if (infocodeSpecificID[2] != RecordIdentifier.Empty)
            {
                DeleteRecord(entry, "RBOINFOCODETABLESPECIFIC", new[] { "INFOCODEID", "REFRELATION", "REFRELATION2" }, infocodeSpecificID, BusinessObjects.Permission.InfocodeEdit);
            }
            else
            {
                DeleteRecord(entry, "RBOINFOCODETABLESPECIFIC", new[] { "INFOCODEID", "REFRELATION" }, infocodeSpecificID, BusinessObjects.Permission.InfocodeEdit);
            }
        }

        /// <summary>
        /// Saves an InfocodeSpecific object to the databse. If the record does not exist then a Insert is done, else it executes a update statement.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="infocodeSpecific">The InfocodeSpecific to be saved</param>
        public virtual void Save(IConnectionManager entry, InfocodeSpecific infocodeSpecific)
        {
            var statement = new SqlServerStatement("RBOINFOCODETABLESPECIFIC");

            ValidateSecurity(entry, BusinessObjects.Permission.InfocodeEdit);

            if (!Exists(entry, infocodeSpecific.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("INFOCODEID", (string)infocodeSpecific.ID.PrimaryID);
                statement.AddKey("REFRELATION", (string)infocodeSpecific.RefRelation);
                statement.AddKey("REFRELATION2", (string)infocodeSpecific.RefRelation2);
                statement.AddKey("REFRELATION3", (string)infocodeSpecific.RefRelation3);
                statement.AddKey("REFTABLEID", (int)infocodeSpecific.RefTableId, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("INFOCODEID", (string)infocodeSpecific.ID.PrimaryID);
                statement.AddCondition("REFRELATION", (string)infocodeSpecific.RefRelation);
                statement.AddCondition("REFRELATION2", (string)infocodeSpecific.RefRelation2);
                statement.AddCondition("REFRELATION3", (string)infocodeSpecific.RefRelation3);
                statement.AddCondition("REFTABLEID", (int)infocodeSpecific.RefTableId, SqlDbType.Int);
            }

            statement.AddField("TRIGGERING", (int)infocodeSpecific.Triggering, SqlDbType.TinyInt);
            statement.AddField("UNITOFMEASURE", infocodeSpecific.UnitOfMeasure.ToString());
            statement.AddField("SALESTYPEFILTER", infocodeSpecific.SalesTypeFilter.ToString());
            statement.AddField("USAGECATEGORY", (int)infocodeSpecific.UsageCategory, SqlDbType.Int);
            statement.AddField("SEQUENCE", infocodeSpecific.Sequence, SqlDbType.Int);
            statement.AddField("INPUTREQUIRED", infocodeSpecific.InputRequired ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
