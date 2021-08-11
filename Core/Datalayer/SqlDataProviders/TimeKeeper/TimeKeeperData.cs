using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.BusinessObjects.TimeKeeper;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.StoreManagement;
using LSOne.DataLayer.DataProviders.Terminals;
using LSOne.DataLayer.DataProviders.TimeKeeper;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.SqlConnector.QueryHelpers;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.TimeKeeper
{
    public class TimeKeeperData : SqlServerDataProviderBase, ITimeKeeperData
    {
        private static void PopulateTimeKept(IDataReader dr, TimeKept timeKept)
        {
            timeKept.ID = (Guid)dr["GUID"];
            timeKept.UserID = (Guid)dr["USERGUID"];
            timeKept.DateKept = (DateTime)dr["TIMETOKEEP"];
            timeKept.TypeToKeep = (KeepType)(short) dr["TIMETYPE"];
            timeKept.Text = (string)dr["COMMENT"];
            timeKept.ClosesEntry = dr["CLOSESENTRY"] != DBNull.Value? (Guid)dr["CLOSESENTRY"] :Guid.Empty;

        }
      
        private static void PopulateTimeInterval(IDataReader dr, TimeInterval interval)
        {
            interval.login = (string)dr["login"];
            if (dr["clockin"] != DBNull.Value)
            {
                interval.clockIn = (DateTime) dr["clockin"];
            }
            if (dr["clockout"] != DBNull.Value)
            {
                interval.clockOut = (DateTime)dr["clockout"];
            }
            if (dr["clockoutcomment"] != DBNull.Value)
            {
                interval.clockOutComment = (string)dr["clockoutcomment"];
            }
            if (dr["clockincomment"] != DBNull.Value)
            {
                interval.clockInComment = (string)dr["clockincomment"];
            }
            

        }
        private static List<TableColumn> selectionColumns = new List<TableColumn>
        {
            new TableColumn {ColumnName = "GUID", TableAlias = "A"},
            new TableColumn {ColumnName = "USERGUID", TableAlias = "A"},
            new TableColumn {ColumnName = "TIMETOKEEP", TableAlias = "A"},
            new TableColumn {ColumnName = "TIMETYPE", TableAlias = "A"},
            new TableColumn {ColumnName = "ISNULL(COMMENT, '')", ColumnAlias = "COMMENT"},
            new TableColumn {ColumnName = "CLOSESENTRY", TableAlias = "A"},
         
        };
        public void Save(IConnectionManager entry, TimeKept timeKept)
        {
            var statement = new SqlServerStatement("USERKEPTTIMES");

            ValidateSecurity(entry, BusinessObjects.Permission.TerminalEdit);

            bool isNew = false;
            if (timeKept.ID == RecordIdentifier.Empty)
            {
                isNew = true;
                timeKept.ID = Guid.NewGuid();
            }

            if (isNew || !Exists(entry, timeKept.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("GUID", (Guid)timeKept.ID,SqlDbType.UniqueIdentifier);
                
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("GUID", (Guid)timeKept.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("USERGUID", (Guid)timeKept.UserID,SqlDbType.UniqueIdentifier);
            statement.AddField("TIMETOKEEP", timeKept.DateKept,SqlDbType.DateTime);
            statement.AddField("TIMETYPE", timeKept.TypeToKeep,SqlDbType.TinyInt);
            if (!string.IsNullOrEmpty(timeKept.Text))
            {
                statement.AddField("COMMENT", timeKept.Text);
            }
            if (timeKept.ClosesEntry != null && timeKept.ClosesEntry != Guid.Empty)
            {

                statement.AddField("CLOSESENTRY", (Guid)timeKept.ClosesEntry, SqlDbType.UniqueIdentifier);
            }

        entry.Connection.ExecuteStatement(statement);

        }

        public TimeKept GetLastTimeKept(IConnectionManager entry, RecordIdentifier user)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.USERGUID = @userID " });

                MakeParam(cmd, "userID", (Guid)user, SqlDbType.UniqueIdentifier);
               
                

                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("USERKEPTTIMES", "A",1),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  string.Empty,
                  QueryPartGenerator.ConditionGenerator(conditions),
                  "ORDER BY TIMETOKEEP DESC"
                  );

                var groups = Execute<TimeKept>(entry, cmd, CommandType.Text, PopulateTimeKept);

                return (groups.Count > 0) ? groups[0] : null;
            }
        }

        public void Delete(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(IConnectionManager entry, RecordIdentifier ID)
        {
            throw new System.NotImplementedException();
        }

        public List<TimeKept> GetListForUser(IConnectionManager entry, RecordIdentifier userID)
        {
            throw new System.NotImplementedException();
        }

        public TimeKept Get(IConnectionManager entry, RecordIdentifier timeId)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);
                List<Condition> conditions = new List<Condition>();

                conditions.Add(new Condition { Operator = "AND", ConditionValue = "A.GUID = @timeId " });

                MakeParam(cmd, "timeId", (Guid)timeId, SqlDbType.UniqueIdentifier);



                cmd.CommandText = string.Format(
                  QueryTemplates.BaseQuery("USERKEPTTIMES", "A", 1),
                  QueryPartGenerator.InternalColumnGenerator(selectionColumns),
                  string.Empty,
                  QueryPartGenerator.ConditionGenerator(conditions),
                  ""
                  );

                var groups = Execute<TimeKept>(entry, cmd, CommandType.Text, PopulateTimeKept);

                return (groups.Count > 0) ? groups[0] : null;
            }
        }

        public List<TimeInterval> GetReport(IConnectionManager entry, RecordIdentifier userGuid)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);



                cmd.CommandText = @"select u.login,  times.* from users u 
join(
SELECT TOP 1000
      a.[UserGUID] userguid
      , a. [TIMETOKEEP] clockout
      , b. [TIMETOKEEP] clockin
      , a.[COMMENT] as clockoutcomment
      , b.[COMMENT] as clockincomment


  FROM[USERKEPTTIMES] a
  join
  [USERKEPTTIMES] b on a.closesentry = b.guid


  where a.[CLOSESENTRY] is not null and a.userGuid = @userGuid
  union
  select a.userguid, a.[TIMETOKEEP], null, a.[COMMENT], null
  from[USERKEPTTIMES] a where a.closesentry is null and a.timetype = 1 and a.userGuid = @userGuid
  union
  select a.userguid, null, a.[TIMETOKEEP], null, a.[COMMENT]
  from[USERKEPTTIMES] a where a.timetype = 0 and a.guid not in (select distinct[CLOSESENTRY] from[USERKEPTTIMES] where closesentry is not null) and a.userGuid = @userGuid
  ) times
  on times.userguid = u.GUID";
                MakeParam(cmd, "userGuid", (Guid)userGuid, SqlDbType.UniqueIdentifier);
                var groups = Execute<TimeInterval>(entry, cmd, CommandType.Text, PopulateTimeInterval);

                return groups;
            }
        } 
    }
}