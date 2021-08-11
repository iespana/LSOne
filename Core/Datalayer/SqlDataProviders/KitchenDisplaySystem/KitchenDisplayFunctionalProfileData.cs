using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.KitchenDisplaySystem;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
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
    public class KitchenDisplayFunctionalProfileData : SqlServerDataProviderBase, IKitchenDisplayFunctionalProfileData
    {
        private static string BaseSelectString
        {
            get
            {
                return @"   select 
                             kfp.ID
                            ,kfp.NAME
                            ,kfp.BUMPPOSSIBLE
                            ,kfp.BUTTONSMENUID
                            ,kfp.RECALLEDORDERSAPPEAR
                            ,ISNULL(pmh.DESCRIPTION,'') as BUTTONSMENUDESCRIPTION
                            ,kfp.SOUNDONNEWORDER
                            ,ISNULL(kfp.DONEORDERSAPPEAR,0) as DONEORDERSAPPEAR
                            ,ISNULL(kfp.TIMEUNTILFORCEBUMPFROMSTATION,0) as TIMEUNTILFORCEBUMPFROMSTATION
                            from KITCHENDISPLAYFUNCTIONALPROFILE kfp left outer join POSMENUHEADER pmh on kfp.BUTTONSMENUID = MENUID ";
            }
        }

        private static void PopulateProfile(IDataReader dr, KitchenDisplayFunctionalProfile kitchenDisplayFunctionalProfile)
        {
            kitchenDisplayFunctionalProfile.ID = (Guid)dr["ID"];
            kitchenDisplayFunctionalProfile.Text = (string)dr["NAME"];
            kitchenDisplayFunctionalProfile.BumpPossible = (BumpPossibleEnum)dr["BUMPPOSSIBLE"];
            kitchenDisplayFunctionalProfile.ButtonsMenuId = (string)dr["BUTTONSMENUID"];
            kitchenDisplayFunctionalProfile.ButtonsMenuDescription = (string)dr["BUTTONSMENUDESCRIPTION"];
            kitchenDisplayFunctionalProfile.RecalledOrdersAppear = (RecalledOrdersAppearEnum)dr["RECALLEDORDERSAPPEAR"];
            kitchenDisplayFunctionalProfile.DoneOrdersAppear = (DoneOrdersAppearEnum)dr["DONEORDERSAPPEAR"];
            kitchenDisplayFunctionalProfile.SoundOnNewOrder = AsBool(dr["SOUNDONNEWORDER"]);
            kitchenDisplayFunctionalProfile.TimeUntilForceBumpFromStation = AsInt(dr["TIMEUNTILFORCEBUMPFROMSTATION"]);
        }

        public virtual KitchenDisplayFunctionalProfile Get(IConnectionManager entry, RecordIdentifier profileId)
        {
            
                using (var cmd = entry.Connection.CreateCommand())
                {
                    cmd.CommandText =
                        BaseSelectString +
                        "where kfp.ID = @profileId and  kfp.DATAAREAID = @dataAreaId";

                    MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                    MakeParam(cmd, "profileId", (Guid)profileId, SqlDbType.UniqueIdentifier);

                    List<KitchenDisplayFunctionalProfile> results;
                    results = Execute<KitchenDisplayFunctionalProfile>(entry, cmd, CommandType.Text, PopulateProfile);

                    if (results.Count == 1)
                    {
                        KitchenDisplayFunctionalProfile profile = results[0];

                        var listOfButtons = Providers.PosMenuLineData.GetList(entry, profile.ButtonsMenuId);
                        foreach (var button in listOfButtons)
                        {
                            var kitchenDisplayButton = new KitchenDisplayButton
                            {
                                ButtonAction = (KitchenDisplayButton.ButtonActionEnum)(int)button.Operation,
                                ButtonKey = button.KeyMapping,
                                ButtonText = button.Text,
                                ChitCellNoToBump = button.ChitCellNoToBump
                            };

                            profile.Buttons.Add(kitchenDisplayButton);
                        }

                        return profile;
                    }
                    return null;
                }
        }

        public virtual List<KitchenDisplayFunctionalProfile> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kfp.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<KitchenDisplayFunctionalProfile>(entry, cmd, CommandType.Text, PopulateProfile);
            }
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier profileId)
        {
            DeleteRecord(entry, "KITCHENDISPLAYFUNCTIONALPROFILE", "ID", profileId, BusinessObjects.Permission.ManageKitchenDisplayProfiles);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier profileId)
        {
            return RecordExists(entry, "KITCHENDISPLAYFUNCTIONALPROFILE", "ID", profileId);
        }

        public virtual void Save(IConnectionManager entry, KitchenDisplayFunctionalProfile kitchenDisplayFunctionalProfile)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYFUNCTIONALPROFILE");
            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayProfiles);

            var isNew = false;
            if (kitchenDisplayFunctionalProfile.ID.IsEmpty)
            {
                kitchenDisplayFunctionalProfile.ID = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, kitchenDisplayFunctionalProfile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)kitchenDisplayFunctionalProfile.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)kitchenDisplayFunctionalProfile.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("NAME", kitchenDisplayFunctionalProfile.Text);
            statement.AddField("BUMPPOSSIBLE", kitchenDisplayFunctionalProfile.BumpPossible, SqlDbType.Int);
            statement.AddField("BUTTONSMENUID", kitchenDisplayFunctionalProfile.ButtonsMenuId);
            statement.AddField("RECALLEDORDERSAPPEAR", kitchenDisplayFunctionalProfile.RecalledOrdersAppear, SqlDbType.Int);
            statement.AddField("DONEORDERSAPPEAR", kitchenDisplayFunctionalProfile.DoneOrdersAppear, SqlDbType.Int);
            statement.AddField("SOUNDONNEWORDER", kitchenDisplayFunctionalProfile.SoundOnNewOrder, SqlDbType.TinyInt);
            statement.AddField("TIMEUNTILFORCEBUMPFROMSTATION", kitchenDisplayFunctionalProfile.TimeUntilForceBumpFromStation, SqlDbType.Int);

            entry.Connection.ExecuteStatement(statement);
        }
    }
}
