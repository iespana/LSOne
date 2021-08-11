using LSOne.DataLayer.BusinessObjects.TouchButtons;
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
using static LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayButton;

namespace LSOne.DataLayer.SqlDataProviders.KitchenDisplaySystem
{
    public class KitchenDisplayStyleProfileData : SqlServerDataProviderBase, IKitchenDisplayStyleProfileData
    {
        private static string BaseSelectString
        {
            get
            {
                return @" SELECT 
                             kds.ID
                            ,kds.NAME
                            ,kds.ORDERPANESTYLEID
                            ,kds.ORDERSTYLEID
                            ,kds.ITEMDEFAULTSTYLEID
                            ,kds.ITEMONTIMESTYLEID
                            ,kds.ITEMDONESTYLEID
                            ,kds.ITEMMODIFIEDSTYLEID
                            ,kds.ITEMVOIDEDSTYLEID
                            ,kds.ITEMRUSHSTYLEID
                            ,kds.DEFAULTFOOTERSTYLEID
                            ,kds.DEFAULTHEADERSTYLEID
                            ,kds.TRANSACTCOMMENTSTYLEID
                            ,kds.NORMALITEMMODIFIERSTYLEID
                            ,kds.INCREASEITEMMODIFIERSTYLEID
                            ,kds.DECREASEITEMMODIFIERSTYLEID
                            ,kds.COMMENTMODIFIERSTYLEID
                            ,kds.ITEMMODIFIERMODIFIEDSTYLEID
                            ,kds.ITEMMODIFIERVOIDEDSTYLEID
                            ,kds.ITEMMODIFIERGLYPHSTYLEID
                            ,kds.DEALHEADERSTYLEID
                            ,kds.DEALHEADERVOIDEDSTYLEID
                            ,kds.HEAVYITEMMODIFIERSTYLEID
                            ,kds.LIGHTITEMMODIFIERSTYLEID
                            ,kds.ONLYITEMMODIFIERSTYLEID
                            ,kds.DONEITEMMODIFIERUISTYLEID
                            ,kds.ALERTSTYLEID
                            ,kds.DONECHIT_OVERLAYTYPE
                            ,kds.ITEMSTARTEDSTYLEID
                            ,kds.ITEMSERVEDSTYLEID
                            ,kds.AGGREGATEHEADERSTYLEID
                            ,kds.AGGREGATEBODYSTYLEID
                            ,kds.AGGREGATEPANESTYLEID
                            ,kds.HISTORYHEADERSTYLEID
                            ,kds.HISTORYBODYSTYLEID
                            ,kds.HISTORYPANESTYLEID
                            ,kds.BUTTONDEFAULTSTYLEID
                            ,kds.BUTTONNEXTSTYLEID
                            ,kds.BUTTONPREVIOUSSTYLEID
                            ,kds.BUTTONBUMPSTYLEID
                            ,kds.BUTTONSTARTSTYLEID
                            ,kds.BUTTONNEXTSCREENSTYLEID
                            ,kds.BUTTONPREVIOUSSCREENSTYLEID
                            ,kds.BUTTONSELECTSTARTSTYLEID
                            ,kds.BUTTONSELECTBUMPSTYLEID
                            ,kds.BUTTONRECALLSTYLEID
                            ,kds.BUTTONHOMESTYLEID
                            ,kds.BUTTONENDSTYLEID
                            ,kds.BUTTONMARKSTYLEID
                            ,kds.BUTTONSERVESTYLEID
                            ,kds.BUTTONTRANSFERSTYLEID
                            ,kds.BUTTONRUSHSTYLEID
                          FROM KITCHENDISPLAYSTYLEPROFILE kds ";
            }
        }

        private static void PopulateProfileIds(IDataReader dr, LSOneKitchenDisplayStyleProfile kitchenDisplayStyleProfile)
        {
            kitchenDisplayStyleProfile.ID = (Guid)dr["ID"];
            kitchenDisplayStyleProfile.Text = (string)dr["NAME"];
            kitchenDisplayStyleProfile.OrderPaneStyle.ID = (string)dr["ORDERPANESTYLEID"];
            kitchenDisplayStyleProfile.OrderStyle.ID = (string)dr["ORDERSTYLEID"];
            kitchenDisplayStyleProfile.ItemDefaultStyle.ID = (string)dr["ITEMDEFAULTSTYLEID"];
            kitchenDisplayStyleProfile.ItemOnTimeStyle.ID = (string)dr["ITEMONTIMESTYLEID"];
            kitchenDisplayStyleProfile.ItemDoneStyle.ID = (string)dr["ITEMDONESTYLEID"];
            kitchenDisplayStyleProfile.ItemModifiedStyle.ID = (string)dr["ITEMMODIFIEDSTYLEID"];
            kitchenDisplayStyleProfile.ItemVoidedStyle.ID = (string)dr["ITEMVOIDEDSTYLEID"];
            kitchenDisplayStyleProfile.ItemRushStyle.ID = (string)dr["ITEMRUSHSTYLEID"];
            kitchenDisplayStyleProfile.DefaultFooterStyle.ID = (string)dr["DEFAULTFOOTERSTYLEID"];
            kitchenDisplayStyleProfile.DefaultHeaderStyle.ID = (string)dr["DEFAULTHEADERSTYLEID"];
            kitchenDisplayStyleProfile.TransactCommentStyle.ID = (string)dr["TRANSACTCOMMENTSTYLEID"];
            kitchenDisplayStyleProfile.NormalItemModifierStyle.ID = (string)dr["NORMALITEMMODIFIERSTYLEID"];
            kitchenDisplayStyleProfile.IncreaseItemModifierStyle.ID = (string)dr["INCREASEITEMMODIFIERSTYLEID"];
            kitchenDisplayStyleProfile.DecreaseItemModifierStyle.ID = (string)dr["DECREASEITEMMODIFIERSTYLEID"];
            kitchenDisplayStyleProfile.CommentModifierStyle.ID = (string)dr["COMMENTMODIFIERSTYLEID"];
            kitchenDisplayStyleProfile.ItemModifierModifiedStyle.ID = (string)dr["ITEMMODIFIERMODIFIEDSTYLEID"];
            kitchenDisplayStyleProfile.ItemModifierVoidedStyle.ID = (string)dr["ITEMMODIFIERVOIDEDSTYLEID"];
            kitchenDisplayStyleProfile.ItemModifierGlyphStyle.ID = (string)dr["ITEMMODIFIERGLYPHSTYLEID"];
            kitchenDisplayStyleProfile.DealHeaderStyle.ID = (string)dr["DEALHEADERSTYLEID"];
            kitchenDisplayStyleProfile.DealHeaderVoidedStyle.ID = (string)dr["DEALHEADERVOIDEDSTYLEID"];
            kitchenDisplayStyleProfile.HeavyItemModifierStyle.ID = (string)dr["HEAVYITEMMODIFIERSTYLEID"];
            kitchenDisplayStyleProfile.LightItemModifierStyle.ID = (string)dr["LIGHTITEMMODIFIERSTYLEID"];
            kitchenDisplayStyleProfile.OnlyItemModifierStyle.ID = (string)dr["ONLYITEMMODIFIERSTYLEID"];
            kitchenDisplayStyleProfile.DoneItemModifierStyle.ID = (string)dr["DONEITEMMODIFIERUISTYLEID"];
            kitchenDisplayStyleProfile.AlertStyle.ID = (string)dr["ALERTSTYLEID"];
            kitchenDisplayStyleProfile.DoneChitOverlayStyle = (KDSBusinessObjects.Enums.DoneChitsOverlayEnum)dr["DONECHIT_OVERLAYTYPE"];
            kitchenDisplayStyleProfile.ItemStartedStyle.ID = (string)dr["ITEMSTARTEDSTYLEID"];
            kitchenDisplayStyleProfile.ItemServedStyle.ID = (string)dr["ITEMSERVEDSTYLEID"];
            kitchenDisplayStyleProfile.AggregateHeaderStyle.ID = (string)dr["AGGREGATEHEADERSTYLEID"];
            kitchenDisplayStyleProfile.AggregateBodyStyle.ID = (string)dr["AGGREGATEBODYSTYLEID"];
            kitchenDisplayStyleProfile.AggregatePaneStyle.ID = (string)dr["AGGREGATEPANESTYLEID"];
            kitchenDisplayStyleProfile.HistoryHeaderStyle.ID = (string)dr["HISTORYHEADERSTYLEID"];
            kitchenDisplayStyleProfile.HistoryBodyStyle.ID = (string)dr["HISTORYBODYSTYLEID"];
            kitchenDisplayStyleProfile.HistoryPaneStyle.ID = (string)dr["HISTORYPANESTYLEID"];

            // Button styles
            kitchenDisplayStyleProfile.ButtonDefaultStyle.ID = (string)dr["BUTTONDEFAULTSTYLEID"];
            kitchenDisplayStyleProfile.ButtonNextStyle.ID = (string)dr["BUTTONNEXTSTYLEID"];
            kitchenDisplayStyleProfile.ButtonPreviousStyle.ID = (string)dr["BUTTONPREVIOUSSTYLEID"];
            kitchenDisplayStyleProfile.ButtonBumpStyle.ID = (string)dr["BUTTONBUMPSTYLEID"];
            kitchenDisplayStyleProfile.ButtonStartStyle.ID = (string)dr["BUTTONSTARTSTYLEID"];
            kitchenDisplayStyleProfile.ButtonNextScreenStyle.ID = (string)dr["BUTTONNEXTSCREENSTYLEID"];
            kitchenDisplayStyleProfile.ButtonPreviousScreenStyle.ID = (string)dr["BUTTONPREVIOUSSCREENSTYLEID"];
            kitchenDisplayStyleProfile.ButtonSelectStartStyle.ID = (string)dr["BUTTONSELECTSTARTSTYLEID"];
            kitchenDisplayStyleProfile.ButtonSelectBumpStyle.ID = (string)dr["BUTTONSELECTBUMPSTYLEID"];
            kitchenDisplayStyleProfile.ButtonRecallStyle.ID = (string)dr["BUTTONRECALLSTYLEID"];
            kitchenDisplayStyleProfile.ButtonHomeStyle.ID = (string)dr["BUTTONHOMESTYLEID"];
            kitchenDisplayStyleProfile.ButtonEndStyle.ID = (string)dr["BUTTONENDSTYLEID"];
            kitchenDisplayStyleProfile.ButtonMarkStyle.ID = (string)dr["BUTTONMARKSTYLEID"];
            kitchenDisplayStyleProfile.ButtonServeStyle.ID = (string)dr["BUTTONSERVESTYLEID"];
            kitchenDisplayStyleProfile.ButtonTransferStyle.ID = (string)dr["BUTTONTRANSFERSTYLEID"];
            kitchenDisplayStyleProfile.ButtonRushStyle.ID = (string)dr["BUTTONRUSHSTYLEID"];
        }

        public LSOneKitchenDisplayStyleProfile Get(IConnectionManager entry, RecordIdentifier profileId,
                                                     bool includeDetails = false)
        {
            
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kds.ID = @profileId and  kds.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);
                MakeParam(cmd, "profileId", (Guid)profileId, SqlDbType.UniqueIdentifier);

                var results = Execute<LSOneKitchenDisplayStyleProfile>(entry, cmd, CommandType.Text,
                                                                                        PopulateProfileIds);
                if (results.Count == 1)
                {
                    var profile = results[0];
                    AddStyles(entry, profile);
                    return profile;
                }
                else
                {
                    return null;
                }
            }
            
        }

        public virtual List<LSOneKitchenDisplayStyleProfile> GetList(IConnectionManager entry)
        {
            using (var cmd = entry.Connection.CreateCommand())
            {
                cmd.CommandText =
                    BaseSelectString +
                    "where kds.DATAAREAID = @dataAreaId";

                MakeParam(cmd, "dataAreaId", entry.Connection.DataAreaId);

                return Execute<LSOneKitchenDisplayStyleProfile>(entry, cmd, CommandType.Text, PopulateProfileIds);
            }
        }

        public virtual List<KdsButtonStyleProfile> GetButtonStyleList(IConnectionManager entry)
        {
            List<LSOneKitchenDisplayStyleProfile> styleProfiles = GetList(entry);
            return styleProfiles.ConvertAll(i => LSOneKitchenDisplayStyleProfile.ToKDSObject(i).ButtonStyleProfile);
        }

        public virtual void Delete(IConnectionManager entry, RecordIdentifier profileId)
        {
            DeleteRecord(entry, "KITCHENDISPLAYSTYLEPROFILE", "ID", profileId, BusinessObjects.Permission.ManageKitchenDisplayProfiles);
        }

        public virtual bool Exists(IConnectionManager entry, RecordIdentifier profileId)
        {
            return RecordExists(entry, "KITCHENDISPLAYSTYLEPROFILE", "ID", profileId);
        }

        public virtual void Save(IConnectionManager entry, LSOneKitchenDisplayStyleProfile kitchenDisplayStyleProfile)
        {
            var statement = new SqlServerStatement("KITCHENDISPLAYSTYLEPROFILE");
            ValidateSecurity(entry, BusinessObjects.Permission.ManageKitchenDisplayProfiles);

            var isNew = false;
            if (kitchenDisplayStyleProfile.ID.IsEmpty)
            {
                kitchenDisplayStyleProfile.ID = Guid.NewGuid();
                isNew = true;
            }

            if (isNew || !Exists(entry, kitchenDisplayStyleProfile.ID))
            {
                statement.StatementType = StatementType.Insert;

                statement.AddKey("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddKey("ID", (Guid)kitchenDisplayStyleProfile.ID, SqlDbType.UniqueIdentifier);
            }
            else
            {
                statement.StatementType = StatementType.Update;

                statement.AddCondition("DATAAREAID", entry.Connection.DataAreaId);
                statement.AddCondition("ID", (Guid)kitchenDisplayStyleProfile.ID, SqlDbType.UniqueIdentifier);
            }

            statement.AddField("NAME", kitchenDisplayStyleProfile.Text);

            statement.AddField("ORDERPANESTYLEID", (string)kitchenDisplayStyleProfile.OrderPaneStyle.ID);
            statement.AddField("ORDERSTYLEID", (string)kitchenDisplayStyleProfile.OrderStyle.ID);
            statement.AddField("ITEMDEFAULTSTYLEID", (string)kitchenDisplayStyleProfile.ItemDefaultStyle.ID);
            statement.AddField("ITEMONTIMESTYLEID", (string)kitchenDisplayStyleProfile.ItemOnTimeStyle.ID);
            statement.AddField("ITEMDONESTYLEID", (string)kitchenDisplayStyleProfile.ItemDoneStyle.ID);
            statement.AddField("ITEMMODIFIEDSTYLEID", (string)kitchenDisplayStyleProfile.ItemModifiedStyle.ID);
            statement.AddField("ITEMVOIDEDSTYLEID", (string)kitchenDisplayStyleProfile.ItemVoidedStyle.ID);
            statement.AddField("ITEMRUSHSTYLEID", (string)kitchenDisplayStyleProfile.ItemRushStyle.ID);
            statement.AddField("DEFAULTFOOTERSTYLEID", (string)kitchenDisplayStyleProfile.DefaultFooterStyle.ID);
            statement.AddField("DEFAULTHEADERSTYLEID", (string)kitchenDisplayStyleProfile.DefaultHeaderStyle.ID);
            statement.AddField("TRANSACTCOMMENTSTYLEID", (string)kitchenDisplayStyleProfile.TransactCommentStyle.ID);
            statement.AddField("NORMALITEMMODIFIERSTYLEID", (string)kitchenDisplayStyleProfile.NormalItemModifierStyle.ID);
            statement.AddField("INCREASEITEMMODIFIERSTYLEID", (string)kitchenDisplayStyleProfile.IncreaseItemModifierStyle.ID);
            statement.AddField("DECREASEITEMMODIFIERSTYLEID", (string)kitchenDisplayStyleProfile.DecreaseItemModifierStyle.ID);
            statement.AddField("COMMENTMODIFIERSTYLEID", (string)kitchenDisplayStyleProfile.CommentModifierStyle.ID);
            statement.AddField("ITEMMODIFIERMODIFIEDSTYLEID", (string)kitchenDisplayStyleProfile.ItemModifierModifiedStyle.ID);
            statement.AddField("ITEMMODIFIERVOIDEDSTYLEID", (string)kitchenDisplayStyleProfile.ItemModifierVoidedStyle.ID);
            statement.AddField("ITEMMODIFIERGLYPHSTYLEID", (string)kitchenDisplayStyleProfile.ItemModifierGlyphStyle.ID);
            statement.AddField("DEALHEADERSTYLEID", (string)kitchenDisplayStyleProfile.DealHeaderStyle.ID);
            statement.AddField("DEALHEADERVOIDEDSTYLEID", (string)kitchenDisplayStyleProfile.DealHeaderVoidedStyle.ID);
            statement.AddField("HEAVYITEMMODIFIERSTYLEID", (string)kitchenDisplayStyleProfile.HeavyItemModifierStyle.ID);
            statement.AddField("LIGHTITEMMODIFIERSTYLEID", (string)kitchenDisplayStyleProfile.LightItemModifierStyle.ID);
            statement.AddField("ONLYITEMMODIFIERSTYLEID", (string)kitchenDisplayStyleProfile.OnlyItemModifierStyle.ID);
            statement.AddField("DONEITEMMODIFIERUISTYLEID", (string)kitchenDisplayStyleProfile.DoneItemModifierStyle.ID);
            statement.AddField("ALERTSTYLEID", (string)kitchenDisplayStyleProfile.AlertStyle.ID);
            statement.AddField("DONECHIT_OVERLAYTYPE", kitchenDisplayStyleProfile.DoneChitOverlayStyle, SqlDbType.Int);
            statement.AddField("ITEMSTARTEDSTYLEID", (string)kitchenDisplayStyleProfile.ItemStartedStyle.ID);
            statement.AddField("ITEMSERVEDSTYLEID", (string)kitchenDisplayStyleProfile.ItemServedStyle.ID);
            statement.AddField("AGGREGATEHEADERSTYLEID", (string)kitchenDisplayStyleProfile.AggregateHeaderStyle.ID);
            statement.AddField("AGGREGATEBODYSTYLEID", (string)kitchenDisplayStyleProfile.AggregateBodyStyle.ID);
            statement.AddField("AGGREGATEPANESTYLEID", (string)kitchenDisplayStyleProfile.AggregatePaneStyle.ID);
            statement.AddField("HISTORYHEADERSTYLEID", (string)kitchenDisplayStyleProfile.HistoryHeaderStyle.ID);
            statement.AddField("HISTORYBODYSTYLEID", (string)kitchenDisplayStyleProfile.HistoryBodyStyle.ID);
            statement.AddField("HISTORYPANESTYLEID", (string)kitchenDisplayStyleProfile.HistoryPaneStyle.ID);

            statement.AddField("BUTTONDEFAULTSTYLEID", (string)kitchenDisplayStyleProfile.ButtonDefaultStyle.ID);
            statement.AddField("BUTTONNEXTSTYLEID", (string)kitchenDisplayStyleProfile.ButtonNextStyle.ID);
            statement.AddField("BUTTONPREVIOUSSTYLEID", (string)kitchenDisplayStyleProfile.ButtonPreviousStyle.ID);
            statement.AddField("BUTTONBUMPSTYLEID", (string)kitchenDisplayStyleProfile.ButtonBumpStyle.ID);
            statement.AddField("BUTTONSTARTSTYLEID", (string)kitchenDisplayStyleProfile.ButtonStartStyle.ID);
            statement.AddField("BUTTONNEXTSCREENSTYLEID", (string)kitchenDisplayStyleProfile.ButtonNextScreenStyle.ID);
            statement.AddField("BUTTONPREVIOUSSCREENSTYLEID", (string)kitchenDisplayStyleProfile.ButtonPreviousScreenStyle.ID);
            statement.AddField("BUTTONSELECTSTARTSTYLEID", (string)kitchenDisplayStyleProfile.ButtonSelectStartStyle.ID);
            statement.AddField("BUTTONSELECTBUMPSTYLEID", (string)kitchenDisplayStyleProfile.ButtonSelectBumpStyle.ID);
            statement.AddField("BUTTONRECALLSTYLEID", (string)kitchenDisplayStyleProfile.ButtonRecallStyle.ID);
            statement.AddField("BUTTONHOMESTYLEID", (string)kitchenDisplayStyleProfile.ButtonHomeStyle.ID);
            statement.AddField("BUTTONENDSTYLEID", (string)kitchenDisplayStyleProfile.ButtonEndStyle.ID);
            statement.AddField("BUTTONMARKSTYLEID", (string)kitchenDisplayStyleProfile.ButtonMarkStyle.ID);
            statement.AddField("BUTTONSERVESTYLEID", (string)kitchenDisplayStyleProfile.ButtonServeStyle.ID);
            statement.AddField("BUTTONTRANSFERSTYLEID", (string)kitchenDisplayStyleProfile.ButtonTransferStyle.ID);
            statement.AddField("BUTTONRUSHSTYLEID", (string)kitchenDisplayStyleProfile.ButtonRushStyle.ID);

            entry.Connection.ExecuteStatement(statement);
        }

        public void AddStyles(IConnectionManager entry, LSOneKitchenDisplayStyleProfile profile)
        {
            profile.OrderPaneStyle = Providers.PosStyleData.Get(entry, profile.OrderPaneStyle.ID) ?? new PosStyle();
            profile.OrderStyle = Providers.PosStyleData.Get(entry, profile.OrderStyle.ID) ?? new PosStyle();
            profile.ItemDefaultStyle = Providers.PosStyleData.Get(entry, profile.ItemDefaultStyle.ID) ?? new PosStyle();
            profile.ItemOnTimeStyle = Providers.PosStyleData.Get(entry, profile.ItemOnTimeStyle.ID) ?? new PosStyle();
            profile.ItemDoneStyle = Providers.PosStyleData.Get(entry, profile.ItemDoneStyle.ID) ?? new PosStyle();
            profile.ItemModifiedStyle = Providers.PosStyleData.Get(entry,profile.ItemModifiedStyle.ID) ?? new PosStyle();
            profile.ItemVoidedStyle = Providers.PosStyleData.Get(entry, profile.ItemVoidedStyle.ID) ?? new PosStyle();
            profile.ItemStartedStyle = Providers.PosStyleData.Get(entry, profile.ItemStartedStyle.ID) ?? new PosStyle();
            profile.ItemServedStyle = Providers.PosStyleData.Get(entry, profile.ItemServedStyle.ID) ?? new PosStyle();
            profile.ItemRushStyle = Providers.PosStyleData.Get(entry, profile.ItemRushStyle.ID) ?? new PosStyle();
            profile.DefaultFooterStyle = Providers.PosStyleData.Get(entry, profile.DefaultFooterStyle.ID) ?? new PosStyle();
            profile.DefaultHeaderStyle = Providers.PosStyleData.Get(entry, profile.DefaultHeaderStyle.ID) ?? new PosStyle();
            profile.TransactCommentStyle = Providers.PosStyleData.Get(entry, profile.TransactCommentStyle.ID) ?? new PosStyle();
            profile.NormalItemModifierStyle = Providers.PosStyleData.Get(entry, profile.NormalItemModifierStyle.ID) ?? new PosStyle();
            profile.IncreaseItemModifierStyle = Providers.PosStyleData.Get(entry, profile.IncreaseItemModifierStyle.ID) ?? new PosStyle();
            profile.DecreaseItemModifierStyle = Providers.PosStyleData.Get(entry, profile.DecreaseItemModifierStyle.ID) ?? new PosStyle();
            profile.CommentModifierStyle = Providers.PosStyleData.Get(entry, profile.CommentModifierStyle.ID) ?? new PosStyle();
            profile.ItemModifierModifiedStyle = Providers.PosStyleData.Get(entry, profile.ItemModifierModifiedStyle.ID) ?? new PosStyle();
            profile.ItemModifierVoidedStyle = Providers.PosStyleData.Get(entry, profile.ItemModifierVoidedStyle.ID) ?? new PosStyle();
            profile.ItemModifierGlyphStyle = Providers.PosStyleData.Get(entry, profile.ItemModifierGlyphStyle.ID) ?? new PosStyle();
            profile.DealHeaderStyle = Providers.PosStyleData.Get(entry, profile.DealHeaderStyle.ID) ?? new PosStyle();
            profile.DealHeaderVoidedStyle = Providers.PosStyleData.Get(entry, profile.DealHeaderVoidedStyle.ID) ?? new PosStyle();
            profile.HeavyItemModifierStyle = Providers.PosStyleData.Get(entry, profile.HeavyItemModifierStyle.ID) ?? new PosStyle();
            profile.LightItemModifierStyle = Providers.PosStyleData.Get(entry, profile.LightItemModifierStyle.ID) ?? new PosStyle();
            profile.OnlyItemModifierStyle = Providers.PosStyleData.Get(entry, profile.OnlyItemModifierStyle.ID) ?? new PosStyle();
            profile.DoneItemModifierStyle = Providers.PosStyleData.Get(entry, profile.DoneItemModifierStyle.ID) ?? new PosStyle();
            profile.AlertStyle = Providers.PosStyleData.Get(entry, profile.AlertStyle.ID) ?? new PosStyle();
            profile.AggregateHeaderStyle = Providers.PosStyleData.Get(entry, profile.AggregateHeaderStyle.ID) ?? new PosStyle();
            profile.AggregateBodyStyle = Providers.PosStyleData.Get(entry, profile.AggregateBodyStyle.ID) ?? new PosStyle();
            profile.AggregatePaneStyle = Providers.PosStyleData.Get(entry, profile.AggregatePaneStyle.ID) ?? new PosStyle();
            profile.HistoryHeaderStyle = Providers.PosStyleData.Get(entry, profile.HistoryHeaderStyle.ID) ?? new PosStyle();
            profile.HistoryBodyStyle = Providers.PosStyleData.Get(entry, profile.HistoryBodyStyle.ID) ?? new PosStyle();
            profile.HistoryPaneStyle = Providers.PosStyleData.Get(entry, profile.HistoryPaneStyle.ID) ?? new PosStyle();

            profile.TimeStyles = Providers.KitchenDisplayTimeStyleData.GetList(entry, profile.ID);

            profile.ButtonDefaultStyle = Providers.PosStyleData.Get(entry, profile.ButtonDefaultStyle.ID) ?? new PosStyle();
            profile.ButtonNextStyle = Providers.PosStyleData.Get(entry, profile.ButtonNextStyle.ID) ?? new PosStyle();
            profile.ButtonPreviousStyle = Providers.PosStyleData.Get(entry, profile.ButtonPreviousStyle.ID) ?? new PosStyle();
            profile.ButtonBumpStyle = Providers.PosStyleData.Get(entry, profile.ButtonBumpStyle.ID) ?? new PosStyle();
            profile.ButtonStartStyle = Providers.PosStyleData.Get(entry, profile.ButtonStartStyle.ID) ?? new PosStyle();
            profile.ButtonNextScreenStyle = Providers.PosStyleData.Get(entry, profile.ButtonNextScreenStyle.ID) ?? new PosStyle();
            profile.ButtonPreviousScreenStyle = Providers.PosStyleData.Get(entry, profile.ButtonPreviousScreenStyle.ID) ?? new PosStyle();
            profile.ButtonSelectStartStyle = Providers.PosStyleData.Get(entry, profile.ButtonSelectStartStyle.ID) ?? new PosStyle();
            profile.ButtonSelectBumpStyle = Providers.PosStyleData.Get(entry, profile.ButtonSelectBumpStyle.ID) ?? new PosStyle();
            profile.ButtonRecallStyle = Providers.PosStyleData.Get(entry, profile.ButtonRecallStyle.ID) ?? new PosStyle();
            profile.ButtonHomeStyle = Providers.PosStyleData.Get(entry, profile.ButtonHomeStyle.ID) ?? new PosStyle();
            profile.ButtonEndStyle = Providers.PosStyleData.Get(entry, profile.ButtonEndStyle.ID) ?? new PosStyle();
            profile.ButtonMarkStyle = Providers.PosStyleData.Get(entry, profile.ButtonMarkStyle.ID) ?? new PosStyle();
            profile.ButtonServeStyle = Providers.PosStyleData.Get(entry, profile.ButtonServeStyle.ID) ?? new PosStyle();
            profile.ButtonTransferStyle = Providers.PosStyleData.Get(entry, profile.ButtonTransferStyle.ID) ?? new PosStyle();
            profile.ButtonRushStyle = Providers.PosStyleData.Get(entry, profile.ButtonRushStyle.ID) ?? new PosStyle();
        }
    }
}