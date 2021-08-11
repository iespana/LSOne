using System;
using System.Data;
using System.IO;
using System.Xml.Serialization;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;

namespace LSOne.DataLayer.SqlDDDataProviders
{
    public class InfoData : SqlServerDataProvider, IInfoData
    {
        private const string SchedulerSettingsInfoName = "SchedulerSettings";

        private static void PopulateJobInfo(IDataReader dr, JscInfo info)
        {
            info.Id = (Guid) dr["Id"];
            info.Name = (string) dr["Name"];
            info.Xml = (string) dr["XML"];
        }

        public JscInfo GetInfo(IConnectionManager entry, string name)
        {
            JscInfo info;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =

                    @" SELECT 
                        Id,
                        Name,
                        Xml
                        FROM JscInfos
                        WHERE Name = @settingsName";

                MakeParam(cmd, "settingsName", SchedulerSettingsInfoName);

                var records = Execute<JscInfo>(entry, cmd, CommandType.Text, PopulateJobInfo);

                info = (records.Count > 0) ? records[0] : null;
                return info;
            }
        }

        public SchedulerSettings GetSchedulerSettings(IConnectionManager entry)
        {
            JscInfo info;

            using (var cmd = entry.Connection.CreateCommand())
            {
                ValidateSecurity(entry);

                cmd.CommandText =

                    @" SELECT 
                        Id,
                        Name,
                        Xml
                        FROM JscInfos
                        WHERE Name = @settingsName";

                MakeParam(cmd, "settingsName", SchedulerSettingsInfoName);

                var records = Execute<JscInfo>(entry, cmd, CommandType.Text, PopulateJobInfo);

                info = (records.Count > 0) ? records[0] : null;
                if (info == null)
                {
                    return null;
                }

                SchedulerSettings schedulerSettings = null;
                XmlSerializer ser = new XmlSerializer(typeof (SchedulerSettings));
                using (StringReader reader = new StringReader(info.Xml))
                {
                    schedulerSettings = (SchedulerSettings) ser.Deserialize(reader);
                }

                return schedulerSettings;
            }
        }

        public void Save(IConnectionManager entry, SchedulerSettings schedulerSettings)
        {
            if (schedulerSettings == null)
            {
                throw new ArgumentNullException("schedulerSettings");
            }

            schedulerSettings.Validate();

            var statement = new SqlServerStatement("JscInfos");

            var x = GetSchedulerSettings(entry);
            if (x == null)
            {
                statement.StatementType = StatementType.Insert;
                statement.AddKey("Id", Guid.NewGuid(), SqlDbType.UniqueIdentifier);
                statement.AddField("Name", SchedulerSettingsInfoName, SqlDbType.VarChar);
            }
            else
            {
                statement.StatementType = StatementType.Update;
                statement.AddCondition("Name", SchedulerSettingsInfoName, SqlDbType.VarChar);
            }
            string xmlString;

            //SqlXml xmlSchedulerSettings = null;
            XmlSerializer ser = new XmlSerializer(typeof (SchedulerSettings));
            using (StringWriter writer = new StringWriter())
            {
                ser.Serialize(writer, schedulerSettings);
                xmlString = writer.ToString();
                //xmlSchedulerSettings = new SqlXml(new XmlTextReader(xmlString
                //                                                    , XmlNodeType.Document, null));
            }

            statement.AddField("XML", xmlString, SqlDbType.Xml);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
