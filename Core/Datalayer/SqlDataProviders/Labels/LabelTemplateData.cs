using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Labels;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlDataProviders.Labels
{
	/// <summary>
    /// A Data provider that retrieves the data for the business object <see cref="LabelTemplate"/>
	/// </summary>
    public class LabelTemplateData : SqlServerDataProviderBase, ILabelTemplateData
	{
		private static string BaseSelectString
		{
			get
			{
			    return
                    @"SELECT LABELID
                  , ISNULL(DATAAREAID, '') DATAAREAID
                  , CONTEXT
                  , NAME
                  , ISNULL(DESCRIPTION, '') DESCRIPTION
                  , TEMPLATE
                  , ISNULL(CODEPAGE, '') CODEPAGE
                  , SAMPLEIMAGE";
			}
		}

        private static void Populate(IDataReader dr, LabelTemplate rec)
		{
            rec.ID = (string)dr["LABELID"];
            rec.DataAreaID = (string)dr["DATAAREAID"];
            rec.Context = (LabelTemplate.ContextEnum)dr["CONTEXT"];
            rec.Text = (string)dr["NAME"];
            rec.Description = (string)dr["DESCRIPTION"];
            rec.Template = (string)dr["TEMPLATE"];
            rec.CodePage = (string)dr["CODEPAGE"];

            object result = dr["SAMPLEIMAGE"];
            if (result == null || result == DBNull.Value)
            {
                rec.SampleImage = null;
            }
            else
            {
                var stream = new MemoryStream((Byte[])result);

                rec.SampleImage = Image.FromStream(stream);
            }
		}

		/// <summary>
		/// Gets the specified entry.
		/// </summary>
		/// <param name="entry">The entry into the database</param>
        /// <param name="labelTemplateID">Label template id</param>
        /// <returns>An instance of <see cref="LabelTemplate"/></returns>
        public virtual LabelTemplate Get(IConnectionManager entry, RecordIdentifier labelTemplateID)
		{
            List<LabelTemplate> result;

			using (var cmd = entry.Connection.CreateCommand())
			{
				cmd.CommandText =
					BaseSelectString +
                    @" FROM RBOLABELTEMPLATES 
                    where LABELID = @labelid and DATAAREAID = @dataAreaId";

                MakeParam(cmd, "labelid", (string)labelTemplateID);
				MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                result = Execute<LabelTemplate>(entry, cmd, CommandType.Text, Populate);
			}

			return result.Count > 0 ? result[0] : null;
		}

        /// <summary>
        /// Retrieves a list of all templates applicable for a given context
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual List<LabelTemplate> GetList(IConnectionManager entry, LabelTemplate.ContextEnum context)
        {
            return GetList(entry, context, "NAME ASC");
        }

        public virtual List<LabelTemplate> GetList(IConnectionManager entry, LabelTemplate.ContextEnum context, string sorting)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString;

                cmd.CommandText = cmd.CommandText + @" FROM RBOLABELTEMPLATES ";
                cmd.CommandText = cmd.CommandText + "where DATAAREAID = @dataAreaId and CONTEXT = @context ORDER BY " + sorting;

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "context", (int) context, SqlDbType.Int);

                return Execute<LabelTemplate>(entry, cmd, CommandType.Text, Populate);
            }
        }

        public virtual bool Exists(IConnectionManager entry, LabelTemplate.ContextEnum context, string labelName)
        {
            return RecordExists(entry, "RBOLABELTEMPLATES", new[] {"CONTEXT", "NAME"},
                                new RecordIdentifier((int)context, labelName));
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier labelTemplateID)
		{
            return RecordExists(entry, "RBOLABELTEMPLATES", "LABELID", labelTemplateID);
		}

        public virtual void Save(IConnectionManager entry, LabelTemplate labelTemplate)
		{
			bool isNew = false;
            var statement = entry.Connection.CreateStatement("RBOLABELTEMPLATES");

            if (labelTemplate.ID.IsEmpty)
            {
				isNew = true;
			}

			if (isNew || !Exists(entry, labelTemplate.ID))
			{
				statement.StatementType = StatementType.Insert;
				statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                labelTemplate.ID = DataProviderFactory.Instance.GenerateNumber<ILabelTemplateData, LabelTemplate>(entry);
                statement.AddKey("LABELID", (string)labelTemplate.ID);
            }
			else
			{
				statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("LABELID", (string)labelTemplate.ID);
			}

            statement.AddField("NAME", labelTemplate.Text);
            statement.AddField("CONTEXT", (int)labelTemplate.Context, SqlDbType.Int);
            statement.AddField("DESCRIPTION", labelTemplate.Description);
            statement.AddField("TEMPLATE", labelTemplate.Template);
            statement.AddField("CODEPAGE", labelTemplate.CodePage);

            if (labelTemplate.SampleImage != null)
            {
                using (var ms = new MemoryStream())
                {
                    labelTemplate.SampleImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    var data = ms.ToArray();
                    statement.AddField("SAMPLEIMAGE", data, SqlDbType.VarBinary);
                }
            }
            else
            {
                statement.AddField("SAMPLEIMAGE", DBNull.Value, SqlDbType.VarBinary);
            }

            entry.Connection.ExecuteStatement(statement);
		}

        public virtual void Delete(IConnectionManager entry, RecordIdentifier labelTemplateID)
        {
            DeleteRecord(entry, "RBOLABELTEMPLATES", "LABELID", labelTemplateID, BusinessObjects.Permission.LabelTemplatesEdit);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "LABELTEMPLATES"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "RBOLABELTEMPLATES", "LABELID", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}

