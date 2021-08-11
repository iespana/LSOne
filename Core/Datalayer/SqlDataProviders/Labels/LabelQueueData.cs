using System;
using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.DataProviders.Labels;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Labels
{
	/// <summary>
    /// A Data provider that retrieves the data for the business object <see cref="LabelQueue"/>
	/// </summary>
    public class LabelQueueData : SqlServerDataProviderBase, ILabelQueueData
	{
		private static string BaseSelectString
		{
			get
			{
			    return
			        @"SELECT LABELQUEUEID
                  , ISNULL(DATAAREAID, '') DATAAREAID
                  , PRINTER
                  , NUMBEROFLABELS
                  , LABELTEMPLATEID
                  , ENTITYID
                  , BATCH
                  , PRINTED
                  , MESSAGE";
			}
		}
        private static void Populate(IDataReader dr, LabelQueue rec)
		{
            rec.ID = (int)dr["LABELQUEUEID"];
            rec.DataAreaID = (string)dr["DATAAREAID"];
            rec.NumberOfLabels = (int)dr["NUMBEROFLABELS"];
            rec.Text = (string)dr["PRINTER"];
            rec.LabelTemplateID = (string)dr["LABELTEMPLATEID"];
            rec.EntityID = (string)dr["ENTITYID"];
            rec.Batch = (string)dr["BATCH"];
            rec.Message = (string)dr["MESSAGE"];

            object result = dr["PRINTED"];
            if (result == null || result == DBNull.Value)
            {
                rec.Printed = DateTime.MinValue;
            }
            else
            {
                rec.Printed = (DateTime) result;
            }
		}

	    /// <summary>
	    /// Gets the specified entry.
	    /// </summary>
	    /// <param name="entry">The entry into the database</param>
	    /// <param name="batch">Batch to get, or null to get all</param>
	    /// <returns>An instance of <see cref="LabelQueue"/></returns>
	    public virtual List<LabelQueue> GetList(IConnectionManager entry, string batch)
		{
			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					BaseSelectString +
                    @" FROM RBOLABELQUEUE 
                    where DATAAREAID = @dataAreaId";
                if (!string.IsNullOrEmpty(batch))
                {
                    cmd.CommandText += " AND BATCH = @batch";
                    MakeParam(cmd, "batch", batch);
                }
			    cmd.CommandText += " AND PRINTED IS NULL";
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<LabelQueue>(entry, cmd, CommandType.Text, Populate);
			}
		}

        public virtual void Save(IConnectionManager entry, LabelQueue labelQueue)
		{
            var statement = entry.Connection.CreateStatement("RBOLABELQUEUE");

            if (labelQueue.ID.IsEmpty || ((int)labelQueue.ID == 0))
			{
				statement.StatementType = StatementType.Insert;
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
            }
			else
			{
				statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("LABELQUEUEID", (int)labelQueue.ID, SqlDbType.Int);
			}

            statement.AddField("PRINTER", labelQueue.Printer);
            statement.AddField("NUMBEROFLABELS", labelQueue.NumberOfLabels, SqlDbType.Int);
            statement.AddField("LABELTEMPLATEID", (string)labelQueue.LabelTemplateID);
            statement.AddField("ENTITYID", (string)labelQueue.EntityID);
            statement.AddField("BATCH", labelQueue.Batch);
            statement.AddField("MESSAGE", labelQueue.Message);
            if (labelQueue.Printed.Year > 2000)
                statement.AddField("PRINTED", labelQueue.Printed, SqlDbType.DateTime);

            entry.Connection.ExecuteStatement(statement);
		}

	    public virtual void SetPrinted(IConnectionManager entry, RecordIdentifier labelQueueID, string message)
	    {
	        using (var cmd = entry.Connection.CreateCommand())
	        {
	            cmd.CommandText = "UPDATE RBOLABELQUEUE SET PRINTED = @printed, MESSAGE = @message WHERE LABELQUEUEID = @id";

                MakeParam(cmd, "id", labelQueueID);
                MakeParam(cmd, "printed", DateTime.Now, SqlDbType.DateTime);
                MakeParam(cmd, "message", string.IsNullOrEmpty(message) ? "" : message);
                
                Execute<LabelQueue>(entry, cmd, CommandType.Text, Populate);
            }
	    }
	}
}

