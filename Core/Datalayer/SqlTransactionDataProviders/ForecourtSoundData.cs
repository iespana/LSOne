using System.Collections.Generic;
using System.Data;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.DataLayer.TransactionDataProviders;
using LSOne.DataLayer.TransactionObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.SqlTransactionDataProviders
{
    public class ForecourtSoundData : SqlServerDataProviderBase, IForecourtSoundData
    {
        private static void PopulateForecourtSound(IDataReader dr, ForecourtSound sound)
        {
            sound.Text = (string) dr["SOUNDFILE"];
            sound.ID = (string) dr["ID"];
        }

        public virtual ForecourtSound Get(IConnectionManager entry, RecordIdentifier id)
        {
            ValidateSecurity(entry);
            using(var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, ISNULL(SOUNDFILE, '') AS SOUND FILE FROM POSISFUELLINGPOINTSOUNDS
                                    WHERE DATAAREAID = @dataAreaID AND ID = @ID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                MakeParam(cmd, "ID", id);
                
                var soundList = Execute<ForecourtSound>(entry, cmd, CommandType.Text, PopulateForecourtSound);
                return soundList.Count > 0 ? soundList[0] : null;
            }
        }

        public virtual List<ForecourtSound> GetList(IConnectionManager entry)
        {
            ValidateSecurity(entry);
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = @"SELECT ID, ISNULL(SOUNDFILE, '') AS SOUND FILE FROM POSISFUELLINGPOINTSOUNDS
                                    WHERE DATAAREAID = @dataAreaID";
                MakeParam(cmd, "dataAreaID", entry.Connection.DataAreaId);
                return Execute<ForecourtSound>(entry, cmd, CommandType.Text, PopulateForecourtSound);
            }
        }

        public virtual void Delete(IConnectionManager entry, ForecourtSound sound)
        {
            DeleteRecord(entry, "POSISFUELLINGPOINTSOUNDS", "ID", sound.ID, "");
        }

        public virtual bool RecordExists(IConnectionManager entry, ForecourtSound sound)
        {
            return RecordExists(entry, "POSISFUELLINGPOINTSOUNDS", "ID", sound.ID);
        }

        public virtual void Save(IConnectionManager entry, ForecourtSound sound)
        {
            ValidateSecurity(entry);
            var statement = new SqlServerStatement("POSISFUELLINGPOINTSOUNDS");
            if(RecordExists(entry, sound))
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (string)sound.ID);
            }
            else
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (string)sound.ID);
            }
        }
    }
}
