using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Integrations;
using LSOne.DataLayer.DataProviders.Integrations;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;

namespace LSOne.DataLayer.SqlDataProviders.Integrations
{
    /// <summary>
    /// Data provider class for integration logs
    /// </summary>
    public class IntegrationLogData : SqlServerDataProviderBase, IIntegrationLogData
    {
        private static string SearchString
        {
            get { return @"select ss.* from(
                        Select s.*, ROW_NUMBER() OVER(order by STAMP DESC) AS ROW from 
						(
							select distinct log.ID, 
							ISNULL(log.STAMP,'') as STAMP,
							ISNULL(log.ERRORID,'') as ERRORID,
							ISNULL(log.LOGTYPE,'') as LOGTYPE,
							ISNULL(log.METHOD,'') as METHOD, 
							ISNULL(log.MESSAGE,'') as MESSAGE 
							from RBOINTEGRATIONLOG log
							where log.DATAAREAID = @dataAreaId
                        and (log.STAMP >= @dateFrom AND log.STAMP <= @dateTo)
                        ) s 
                        ) ss
                        where ss.ROW between  @rowFrom  and @rowTo"; }
        }

        private static void PopulateIntegrationLog(IDataReader dr, IntegrationLog integrationLog)
        {
            integrationLog.ID = (long)dr["ID"];
            integrationLog.Stamp = (DateTime)dr["STAMP"];
            integrationLog.ErrorID = (int)dr["ERRORID"];
            try
            {
                integrationLog.LogType = (IntegrationLog.LogTypes) Enum.Parse(typeof (IntegrationLog.LogTypes), (string)dr["LOGTYPE"]);
            }
            catch {}
            integrationLog.Text = (string)dr["METHOD"];
            integrationLog.Message = (string)dr["MESSAGE"];
        }

        /// <summary>
        /// Saves a given integration mapping to the database
        /// </summary>
        /// <remarks>Requires the 'Edit integration mapping' permission</remarks>
        /// <param name="entry">Entry into the database</param>
        /// <param name="integrationLog">A new integration log entry</param>
        public virtual void Save(IConnectionManager entry, IntegrationLog integrationLog)
        {
            var statement = new SqlServerStatement("RBOINTEGRATIONLOG");

            statement.StatementType = StatementType.Insert;

            statement.AddField("STAMP", integrationLog.Stamp, SqlDbType.DateTime);
            statement.AddField("ERRORID", integrationLog.ErrorID, SqlDbType.Int);
            statement.AddField("LOGTYPE", integrationLog.LogType.ToString());
            statement.AddField("METHOD", integrationLog.Method);
            statement.AddField("MESSAGE", integrationLog.Message ?? string.Empty);
            statement.AddField("DATAAREAID", entry.Connection.DataAreaId);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Clear(IConnectionManager entry, DateTime stampDate)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = string.Format("DELETE FROM RBOINTEGRATIONLOG WHERE STAMP<'{0}'",
                                                stampDate.ToString("yyyy-MM-dd HH:mm:ss"));
                entry.Connection.ExecuteNonQuery(cmd, true, CommandType.Text);
            }
        }

        public List<IntegrationLog> Search(IConnectionManager entry, 
            int rowFrom, int rowTo, 
            DateTime dateFrom, DateTime dateTo)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = SearchString;

                if (dateFrom.Year < 1900)
                    dateFrom = new DateTime(1901, 1, 1);
                if (dateTo.Year < 1900)
                    dateTo = new DateTime(3000, 1, 1);

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "dateFrom", dateFrom, SqlDbType.DateTime);
                MakeParam(cmd, "dateTo", dateTo, SqlDbType.DateTime);
                MakeParam(cmd, "rowFrom", rowFrom, SqlDbType.Int);
                MakeParam(cmd, "rowTo", rowTo, SqlDbType.Int);

                return Execute<IntegrationLog>(entry, cmd, CommandType.Text, PopulateIntegrationLog);
            }
        }

        public virtual int Count(IConnectionManager entry)
        {
            return Count(entry, "RBOINTEGRATIONLOG");
        }
    }
}
