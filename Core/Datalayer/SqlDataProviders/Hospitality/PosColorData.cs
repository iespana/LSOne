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
    public class PosColorData : SqlServerDataProviderBase, IPosColorData
    {
        /// <summary>
        /// Populates all fields of a PosColor object
        /// </summary>
        /// <param name="dr">The SqlDatareader to read the data from</param>
        /// <param name="posColor">The PosColor object to populate</param>
        private static void PopulatePosColor(IDataReader dr, PosColor posColor)
        {
            posColor.ColorCode = (string)dr["COLORCODE"];
            posColor.ID = posColor.ColorCode;
            posColor.Text = (string)dr["DESCRIPTION"];
            posColor.Text = posColor.Text;
            posColor.Color = (int)dr["COLOR"];
            posColor.Bold = ((byte)dr["BOLD"] == 1);
        }

        public virtual List<DataEntity> GetList(IConnectionManager entry, string sort)
        {
            return GetList<DataEntity>(entry, "POSCOLOR", "DESCRIPTION", "COLORCODE", sort);
        }

        /// <summary>
        /// Gets a list of DataEntity objects containing ColorCode and Description.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of DataEntity objects that contains ColorCode and Description</returns>
        public virtual List<DataEntity> GetList(IConnectionManager entry)
        {
            return GetList<DataEntity>(entry, "POSCOLOR", "DESCRIPTION", "COLORCODE", "COLORCODE");
        }

        /// <summary>
        /// Gets a list of PosColor objects containing all rows from the POSCOLOR table
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <returns>A list of pos colors</returns>
        public virtual List<PosColor> GetAllColors(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            { 
                cmd.CommandText =
                    "select COLORCODE, " +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION, ISNULL(COLOR,0) as COLOR, ISNULL(BOLD,0) as BOLD " +
                    "from POSCOLOR " +
                    "where DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<PosColor>(entry, cmd, CommandType.Text, PopulatePosColor);
            }
        }

        /// <summary>
        /// Gets a specific pos color record from the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorCode">The code of the color to get</param>
        /// <returns>A PosColor object containing the pos color record with the given color code</returns>
        public virtual PosColor GetColor(IConnectionManager entry, RecordIdentifier colorCode)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    "select COLORCODE, " +
                    "ISNULL(DESCRIPTION,'') as DESCRIPTION, ISNULL(COLOR,0) as COLOR, ISNULL(BOLD,0) as BOLD " +
                    "from POSCOLOR " +
                    "where DATAAREAID = @dataAreaId and COLORCODE = @colorCode";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "colorCode", (string)colorCode);

                return Execute<PosColor>(entry, cmd, CommandType.Text, PopulatePosColor)[0];
            }
        }

        /// <summary>
        /// Checks if a color exists with a given color code
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorID">The id of the color code</param>
        /// <returns>True if the color exists, false otherwise</returns>
        public virtual bool Exists(IConnectionManager entry, RecordIdentifier colorID)
        {
            return RecordExists(entry, "POSCOLOR", "COLORCODE", colorID);
        }

        /// <summary>
        /// Checks if a given pos color is in use.
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="colorID">The id of the color to look for</param>
        /// <returns>True if the color is in use, false otherwise</returns>
        public virtual bool ColorIsInUse(IConnectionManager entry, RecordIdentifier colorID)
        {
            var hospitalitySetup = Providers.HospitalitySetupData.Get(entry);

            // Check each forecolor in hospitalitysetup
            bool result =
                (string)colorID == hospitalitySetup.TableFreeColorF ||
                (string)colorID == hospitalitySetup.TableNotAvailColorF ||
                (string)colorID == hospitalitySetup.TableLockedColorF ||
                (string)colorID == hospitalitySetup.OrderNotPrintedColorF ||
                (string)colorID == hospitalitySetup.OrderPrintedColorF ||
                (string)colorID == hospitalitySetup.OrderStartedColorF ||
                (string)colorID == hospitalitySetup.OrderFinishedColorF ||
                (string)colorID == hospitalitySetup.OrderConfirmedColorF;
                
            return result;
        }

        /// <summary>
        /// Saves a given color into the database
        /// </summary>
        /// <param name="entry">The entry into the database</param>
        /// <param name="color">The color to save</param>
        public virtual void Save(IConnectionManager entry, PosColor color)
        {
            var statement = new SqlServerStatement("POSCOLOR");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageHospitalitySetup);

            bool isNew = false;
            if (color.ID.IsEmpty)
            {
                isNew = true;
                color.ID = DataProviderFactory.Instance.GenerateNumber<IPosColorData, PosColor>(entry);
            }

            if (isNew || !Exists(entry, color.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("COLORCODE", (string)color.ID);

            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("COLORCODE", (string)color.ID);
            }

            statement.AddField("DESCRIPTION", color.Text);
            statement.AddField("COLOR", color.Color, SqlDbType.Int);
            statement.AddField("BOLD", color.Bold ? 1 : 0, SqlDbType.TinyInt);

            entry.Connection.ExecuteStatement(statement);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier colorCode)
        {
            DeleteRecord(entry, "POSCOLOR", "COLORCODE", colorCode, BusinessObjects.Permission.ManageHospitalitySetup);
        }

        #region ISequenceable Members

        public virtual bool SequenceExists(IConnectionManager entry, RecordIdentifier id)
        {
            return Exists(entry, id);
        }

        public RecordIdentifier SequenceID
        {
            get { return "HOSPITALITY_COLOR"; }
        }

        public List<RecordIdentifier> GetExistingSequences(IConnectionManager entry, string sequenceFormat, int startingRecord, int numberOfRecords)
        {
            return GetExistingRecords(entry, "POSCOLOR", "COLORCODE", sequenceFormat, startingRecord, numberOfRecords);
        }

        #endregion
    }
}
