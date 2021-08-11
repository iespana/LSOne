using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserInterface;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.SqlConnector;
using LSOne.DataLayer.SqlConnector.DataProviders;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayTimeStyleData : SqlServerDataProviderBase, IKitchenDisplayTimeStyleData
    {
        public static string navCompName;
        public static bool usePercentCookingTime;
        public static int defaultCookingTime;

        private static string BaseSelectString
        {
            get
            {
                return @"   select 
                            KDSTYLEPROFILEID
                            ,LINEID
                            ,SECONDSPASSED
                            ,ISNULL(PERCENTCOOKTIME, 1) AS PERCENTCOOKTIME
                            ,ISNULL(STYLEID, '') as STYLEID
                            from KITCHENDISPLAYTIMESTYLE ";
            }
        }

        private static void PopulateKitchenDisplayTimeStyle(IDataReader dr, KitchenDisplayTimeStyle kitchenDisplayTimeStyle)
        {
            kitchenDisplayTimeStyle.LineId = (int)dr["LINEID"];
            kitchenDisplayTimeStyle.KdStyleProfileId = (Guid)dr["KDSTYLEPROFILEID"];
            kitchenDisplayTimeStyle.SecondsPassed = (int)dr["SECONDSPASSED"];
            kitchenDisplayTimeStyle.StyleId = (string)dr["STYLEID"];
            kitchenDisplayTimeStyle.StyleId.SerializationData = (string)dr["STYLEID"];
            kitchenDisplayTimeStyle.PercentOfCookingTime = (decimal)dr["PERCENTCOOKTIME"];
            kitchenDisplayTimeStyle.UsePercentOfCookingTime = usePercentCookingTime;
            kitchenDisplayTimeStyle.DefaultCookingTime = defaultCookingTime;
        }

        public virtual KitchenDisplayTimeStyle Get(IConnectionManager entry, RecordIdentifier id)
        {
            var cmd = entry.Connection.CreateCommand();

            cmd.CommandText =
                BaseSelectString +
                "where KDSTYLEPROFILEID = @kdsStyleProfileId and LINEID = @lineId and DATAAREAID = @dataAreaId";

            MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
            MakeParam(cmd, "kdsStyleProfileId", (Guid)id.PrimaryID, SqlDbType.UniqueIdentifier);
            MakeParam(cmd, "lineId", (int)id.SecondaryID, SqlDbType.Int);

            var timeStyles = Execute<KitchenDisplayTimeStyle>(entry, cmd, CommandType.Text, PopulateKitchenDisplayTimeStyle);

            if (timeStyles.Count == 1)
            {
                var timeStyle = timeStyles[0];
                AddStyle(entry, timeStyle);
                return timeStyle;
            }
            else
            {
                return null;
            }
        }

        public virtual List<KitchenDisplayTimeStyle> GetList(IConnectionManager entry, RecordIdentifier kdStyleProfileId)
        {
            List<KitchenDisplayTimeStyle> timeStyles = new List<KitchenDisplayTimeStyle>();
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where KDSTYLEPROFILEID = @kdStyleProfileId and  DATAAREAID = @dataAreaId ";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "kdStyleProfileId", (Guid)kdStyleProfileId);

                var timeStylesTmp = Execute<KitchenDisplayTimeStyle>(entry, cmd, CommandType.Text, PopulateKitchenDisplayTimeStyle);

                foreach (KDSBusinessObjects.KitchenDisplayTimeStyle tmpTimeStyle in timeStylesTmp)
                {
                    if ((!usePercentCookingTime) && (tmpTimeStyle.SecondsPassed == 0))
                    {
                        //We skip the stylelines where SecondsPassed is 0 and 'Overdue time' setting is in use
                        continue;
                    }
                    AddStyle(entry, tmpTimeStyle);
                    timeStyles.Add(tmpTimeStyle);
                }
            }

            return timeStyles;
        }

        public virtual List<KitchenDisplayTimeStyle> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText = BaseSelectString;
                var timeStyles = Execute<KitchenDisplayTimeStyle>(entry, cmd, CommandType.Text, PopulateKitchenDisplayTimeStyle);

                foreach (KitchenDisplayTimeStyle timeStyle in timeStyles)
                {
                    AddStyle(entry, timeStyle);
                }

                return timeStyles;
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier id)
        {
            DeleteRecord(entry, "KITCHENDISPLAYTIMESTYLE", new[] { "KDSTYLEPROFILEID", "LINEID" }, id, BusinessObjects.Permission.ManageKitchenDisplayProfiles);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier id)
        {
            return RecordExists(entry, "KITCHENDISPLAYTIMESTYLE", new[] { "KDSTYLEPROFILEID", "LINEID" }, id);
        }

        [Obsolete("Use overridden Save method")]
        public virtual void Save(IConnectionManager entry, KDSBusinessObjects.KitchenDisplayTimeStyle newKitchenDisplayTimeStyle)
        {
            throw new NotImplementedException();
        }

        public virtual void Save(IConnectionManager entry, KDSBusinessObjects.KitchenDisplayTimeStyle newKitchenDisplayTimeStyle, RecordIdentifier oldKitchenDisplayTimeStyleId)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYTIMESTYLE");

            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayProfiles);

            if (!Exists(entry, oldKitchenDisplayTimeStyleId))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("KDSTYLEPROFILEID", (Guid)oldKitchenDisplayTimeStyleId.PrimaryID, SqlDbType.UniqueIdentifier);
                statement.AddKey("LINEID", (int)oldKitchenDisplayTimeStyleId.SecondaryID, SqlDbType.Int);
                statement.AddField("SECONDSPASSED", newKitchenDisplayTimeStyle.SecondsPassed, SqlDbType.Int);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("KDSTYLEPROFILEID", (Guid)oldKitchenDisplayTimeStyleId.PrimaryID, SqlDbType.UniqueIdentifier);
                statement.AddCondition("LINEID", (int)oldKitchenDisplayTimeStyleId.SecondaryID, SqlDbType.Int);

                statement.AddField("SECONDSPASSED", newKitchenDisplayTimeStyle.SecondsPassed, SqlDbType.Int);
                statement.AddField("PERCENTCOOKTIME", newKitchenDisplayTimeStyle.SecondsPassed, SqlDbType.Decimal);
            }

            statement.AddField("STYLEID", (string)newKitchenDisplayTimeStyle.StyleId);

            entry.Connection.ExecuteStatement(statement);
        }

        private void AddStyle(IConnectionManager entry, KitchenDisplayTimeStyle timeStyle)
        {
            timeStyle.UiStyle = PosStyle.ToUIStyle(Providers.PosStyleData.Get(entry, timeStyle.StyleId));
        }
    }
}
