using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ProviderConfig;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.SiteService.Utilities;
using LSOne.Utilities.IO.JSON;
using LSOne.Utilities.IO.JSON.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using static LSOne.Utilities.DataTypes.RecordIdentifier;
using AggregateGroupItem = LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem.AggregateGroupItem;

namespace LSOne.SiteService.Plugins.KDSLSOneWebServicePlugin
{
    public partial class KDSLSOneWebServicePlugin
    {        
        public Ping_Result Ping(Ping request)
        {
            return new Ping_Result(true);
        }

        public GetKitchenServiceConfigXML_Result GetKitchenServiceConfigXML(GetKitchenServiceConfigXML request)
        {
            KDSConfigurationXML configXML = new KDSConfigurationXML();
            configXML.KitchenServiceConfiguration = new KitchenServiceConfiguration[] { new KitchenServiceConfiguration() };

            GetKitchenServiceConfigXML_Result result = new GetKitchenServiceConfigXML_Result(configXML);
            return result;
        }

        public GetKitchenServiceConfig_Result GetKitchenServiceConfig(GetKitchenServiceConfig request)
        {
            KitchenServiceConfiguration kdsConfig = new KitchenServiceConfiguration();
            JObject kdsConfigJson = new JObject
            {
                ["kdsDatabaseType"] = kdsConfig.kdsDatabaseType
            };

            GetKitchenServiceConfig_Result result = new GetKitchenServiceConfig_Result(kdsConfigJson.ToString());
            return result;
        }

        public GetKDSDisplayStations_Result GetKDSDisplayStations(GetKDSDisplayStations request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<KitchenDisplayStation> displayStations = Providers.KitchenDisplayStationData.GetList(entry);
                    GetKDSDisplayStations_Result result = new GetKDSDisplayStations_Result(JsonConvert.SerializeObject(displayStations));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSDisplayStations_Result();
        }

        public GetUIStyles_Result GetUIStyles(GetUIStyles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<UIStyle> styles = PosStyle.ToKDSStyle(Providers.PosStyleData.GetList(entry));
                    GetUIStyles_Result result = new GetUIStyles_Result(JsonConvert.SerializeObject(styles));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetUIStyles_Result();
        }

        public GetKDSStyleProfiles_Result GetKDSStyleProfiles(GetKDSStyleProfiles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<LSOneKitchenDisplayStyleProfile> styleProfiles = Providers.KitchenDisplayStyleProfileData.GetList(entry);
                    JArray styleProfilesJson = new JArray();

                    foreach(LSOneKitchenDisplayStyleProfile profile in styleProfiles)
                    {
                        JObject profileJson = new JObject()
                        {
                            ["ID"] = JToken.FromObject(profile.ID),
                            ["Text"] = profile.Text,
                            ["DoneChitOverlayStyle"] = (int)profile.DoneChitOverlayStyle,
                            ["OrderPaneStyle"] = (string)profile.OrderPaneStyle.ID,
                            ["OrderStyle"] = (string)profile.OrderStyle.ID,
                            ["ItemDefaultStyle"] = (string)profile.ItemDefaultStyle.ID,
                            ["ItemOnTimeStyle"] = (string)profile.ItemOnTimeStyle.ID,
                            ["ItemDoneStyle"] = (string)profile.ItemDoneStyle.ID,
                            ["ItemModifiedStyle"] = (string)profile.ItemModifiedStyle.ID,
                            ["ItemVoidedStyle"] = (string)profile.ItemVoidedStyle.ID,
                            ["ItemRushStyle"] = (string)profile.ItemRushStyle.ID,
                            ["ItemStartedStyle"] = (string)profile.ItemStartedStyle.ID,
                            ["ItemServedStyle"] = (string)profile.ItemServedStyle.ID,
                            ["DefaultFooterStyle"] = (string)profile.DefaultFooterStyle.ID,
                            ["DefaultHeaderStyle"] = (string)profile.DefaultHeaderStyle.ID,
                            ["NormalItemModifierStyle"] = (string)profile.NormalItemModifierStyle.ID,
                            ["IncreaseItemModifierStyle"] = (string)profile.IncreaseItemModifierStyle.ID,
                            ["DecreaseItemModifierStyle"] = (string)profile.DecreaseItemModifierStyle.ID,
                            ["CommentModifierStyle"] = (string)profile.CommentModifierStyle.ID,
                            ["ItemModifierModifiedStyle"] = (string)profile.ItemModifierModifiedStyle.ID,
                            ["ItemModifierVoidedStyle"] = (string)profile.ItemModifierVoidedStyle.ID,
                            ["ItemModifierGlyphStyle"] = (string)profile.ItemModifierGlyphStyle.ID,
                            ["DealHeaderStyle"] = (string)profile.DealHeaderStyle.ID,
                            ["DealHeaderVoidedStyle"] = (string)profile.DealHeaderVoidedStyle.ID,
                            ["HeavyItemModifierStyle"] = (string)profile.HeavyItemModifierStyle.ID,
                            ["LightItemModifierStyle"] = (string)profile.LightItemModifierStyle.ID,
                            ["OnlyItemModifierStyle"] = (string)profile.OnlyItemModifierStyle.ID,
                            ["DoneItemModifierStyle"] = (string)profile.DoneItemModifierStyle.ID,
                            ["TransactCommentStyle"] = (string)profile.TransactCommentStyle.ID,
                            ["AlertStyle"] = (string)profile.AlertStyle.ID,
                            ["AggregateHeaderStyle"] = (string)profile.AggregateHeaderStyle.ID,
                            ["AggregateBodyStyle"] = (string)profile.AggregateBodyStyle.ID,
                            ["AggregatePaneStyle"] = (string)profile.AggregatePaneStyle.ID,
                            ["HistoryHeaderStyle"] = (string)profile.HistoryHeaderStyle.ID,
                            ["HistoryBodyStyle"] = (string)profile.HistoryBodyStyle.ID,
                            ["HistoryPaneStyle"] = (string)profile.HistoryPaneStyle.ID,
                            ["HeaderPaneStyle"] = (string)profile.HeaderPaneStyle.ID,
                            ["TimeStyles"] = JArray.FromObject(Providers.KitchenDisplayTimeStyleData.GetList(entry, profile.ID)),
                            ["ButtonStyleProfileID"] = LSOneKitchenDisplayStyleProfile.ToKDSObject(profile).ButtonStyleProfileID
                        };

                        styleProfilesJson.Add(profileJson);
                    }

                    GetKDSStyleProfiles_Result result = new GetKDSStyleProfiles_Result(styleProfilesJson.ToString().Replace("StyleData", "Style"));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSStyleProfiles_Result();
        }

        public GetKDSVisualProfiles_Result GetKDSVisualProfiles(GetKDSVisualProfiles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<KitchenDisplayVisualProfile> visualProfiles = Providers.KitchenDisplayVisualProfileData.GetList(entry);
                    GetKDSVisualProfiles_Result result = new GetKDSVisualProfiles_Result(JsonConvert.SerializeObject(visualProfiles));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSVisualProfiles_Result();
        }

        public GetKDSFunctionalProfiles_Result GetKDSFunctionalProfiles(GetKDSFunctionalProfiles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<KitchenDisplayFunctionalProfile> functionalProfiles = Providers.KitchenDisplayFunctionalProfileData.GetList(entry);
                    GetKDSFunctionalProfiles_Result result = new GetKDSFunctionalProfiles_Result(JsonConvert.SerializeObject(functionalProfiles));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSFunctionalProfiles_Result();
        }

        public GetKDSDisplayProfiles_Result GetKDSDisplayProfiles(GetKDSDisplayProfiles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<KitchenDisplayProfile> displayProfiles = Providers.KitchenDisplayProfileData.GetList(entry);
                    GetKDSDisplayProfiles_Result result = new GetKDSDisplayProfiles_Result(JsonConvert.SerializeObject(displayProfiles));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSDisplayProfiles_Result();
        }

        public GetKDSDisplayProfileLines_Result GetKDSDisplayProfileLines(GetKDSDisplayProfileLines request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<KitchenDisplayLine> displayLines = Providers.KitchenDisplayLineData.GetList(entry);
                    GetKDSDisplayProfileLines_Result result = new GetKDSDisplayProfileLines_Result(JsonConvert.SerializeObject(displayLines));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSDisplayProfileLines_Result();
        }

        public GetKDSDisplayProfileLineColumns_Result GetKDSDisplayProfileLineColumns(GetKDSDisplayProfileLineColumns request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<KitchenDisplayLineColumn> lineColumns = Providers.KitchenDisplayLineColumnData.GetList(entry);
                    GetKDSDisplayProfileLineColumns_Result result = new GetKDSDisplayProfileLineColumns_Result(JsonConvert.SerializeObject(lineColumns));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSDisplayProfileLineColumns_Result();
        }

        public GetKDSPosMenuHeaders_Result GetKDSPosMenuHeaders(GetKDSPosMenuHeaders request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<DataLayer.BusinessObjects.TouchButtons.PosMenuHeader> menuHeaders = Providers.PosMenuHeaderData.GetList(entry);
                    GetKDSPosMenuHeaders_Result result = new GetKDSPosMenuHeaders_Result(JsonConvert.SerializeObject(menuHeaders));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSPosMenuHeaders_Result();
        }

        public GetKDSPosMenuLines_Result GetKDSPosMenuLines(GetKDSPosMenuLines request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<DataLayer.BusinessObjects.TouchButtons.PosMenuLine> menuLines = Providers.PosMenuLineData.GetList(entry);
                    GetKDSPosMenuLines_Result result = new GetKDSPosMenuLines_Result(JsonConvert.SerializeObject(menuLines));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSPosMenuLines_Result();
        }

        public GetKDSTimeStyles_Result GetKDSTimeStyles(GetKDSTimeStyles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<KitchenDisplayTimeStyle> timeStyles = Providers.KitchenDisplayTimeStyleData.GetList(entry);
                    GetKDSTimeStyles_Result result = new GetKDSTimeStyles_Result(JsonConvert.SerializeObject(timeStyles).Replace("StyleData", "Style"));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSTimeStyles_Result();
        }

        public GetKDSAggregateProfiles_Result GetKDSAggregateProfiles(GetKDSAggregateProfiles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<AggregateProfile> aggregateProfiles = Providers.KitchenDisplayAggregateProfileData.GetList(entry);
                    GetKDSAggregateProfiles_Result result = new GetKDSAggregateProfiles_Result(JsonConvert.SerializeObject(aggregateProfiles));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSAggregateProfiles_Result();
        }

        public GetKDSAggregateProfileGroups_Result GetKDSAggregateProfileGroups(GetKDSAggregateProfileGroups request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<AggregateProfileGroup> aggregateProfileGroups = Providers.KitchenDisplayAggregateProfileGroupData.GetForKDS(entry);
                    GetKDSAggregateProfileGroups_Result result = new GetKDSAggregateProfileGroups_Result(JsonConvert.SerializeObject(aggregateProfileGroups));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSAggregateProfileGroups_Result();
        }

        public GetKDSAggregateGroupItems_Result GetKDSAggregateGroupItems(GetKDSAggregateGroupItems request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<AggregateGroupItem> aggregateGroupItems = Providers.KitchenDisplayAggregateGroupItemData.GetForKds(entry);
                    GetKDSAggregateGroupItems_Result result = new GetKDSAggregateGroupItems_Result(JsonConvert.SerializeObject(aggregateGroupItems));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSAggregateGroupItems_Result();
        }

        public GetKDSHeaderProfiles_Result GetKDSHeaderProfiles(GetKDSHeaderProfiles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<HeaderPaneProfile> headerPanes = Providers.KitchenDisplayHeaderPaneData.GetList(entry);
                    GetKDSHeaderProfiles_Result result = new GetKDSHeaderProfiles_Result(JsonConvert.SerializeObject(headerPanes));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSHeaderProfiles_Result();
        }

        public GetKDSHeaderProfileLines_Result GetKDSHeaderProfileLines(GetKDSHeaderProfileLines request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<HeaderPaneLine> headerPaneLines = Providers.KitchenDisplayHeaderPaneLineData.GetList(entry);
                    GetKDSHeaderProfileLines_Result result = new GetKDSHeaderProfileLines_Result(JsonConvert.SerializeObject(headerPaneLines));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSHeaderProfileLines_Result();
        }

        public GetKDSHeaderProfileLineColumns_Result GetKDSHeaderProfileLineColumns(GetKDSHeaderProfileLineColumns request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<LSOneHeaderPaneLineColumn> headerPaneLineColumns = Providers.KitchenDisplayHeaderPaneLineColumnData.GetList(entry);
                    List<HeaderPaneLineColumn> kdsHeaderPaneLineColumns = LSOneHeaderPaneLineColumn.ToKDSHeaderPaneLineColumn(headerPaneLineColumns);
                    GetKDSHeaderProfileLineColumns_Result result = new GetKDSHeaderProfileLineColumns_Result(JsonConvert.SerializeObject(kdsHeaderPaneLineColumns));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSHeaderProfileLineColumns_Result();
        }

        public GetKDSButtonStyleProfiles_Result GetKDSButtonStyleProfiles(GetKDSButtonStyleProfiles request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<KdsButtonStyleProfile> buttonStyleProfiles = Providers.KitchenDisplayStyleProfileData.GetButtonStyleList(entry);
                    GetKDSButtonStyleProfiles_Result result = new GetKDSButtonStyleProfiles_Result(JsonConvert.SerializeObject(buttonStyleProfiles));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetKDSButtonStyleProfiles_Result();
        }

        public GetItemRoutingConfig_Result GetItemRoutingConfig(GetItemRoutingConfig request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<LSOneKitchenDisplayItemRoutingConnection> itemRoutings = Providers.KitchenDisplayItemRoutingConnectionData.GetForKds(entry);
                    GetItemRoutingConfig_Result result = new GetItemRoutingConfig_Result(JsonConvert.SerializeObject(itemRoutings));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetItemRoutingConfig_Result();
        }

        public GetHospitalityTypeRoutingConfig_Result GetHospitalityTypeRoutingConfig(GetHospitalityTypeRoutingConfig request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<LSOneKitchenDisplayHospitalityTypeRoutingConnection> hospitalityTypeRoutings = Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.GetForKds(entry);
                    GetHospitalityTypeRoutingConfig_Result result = new GetHospitalityTypeRoutingConfig_Result(JsonConvert.SerializeObject(hospitalityTypeRoutings));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetHospitalityTypeRoutingConfig_Result();
        }

        public GetTerminalRoutingConfig_Result GetTerminalRoutingConfig(GetTerminalRoutingConfig request)
        {
            try
            {
                IConnectionManager entry = GetConnectionManager();
                try
                {
                    List<LSOneKitchenDisplayTerminalRoutingConnection> terminalRoutings = Providers.KitchenDisplayTerminalRoutingConnectionData.GetForKds(entry);
                    GetTerminalRoutingConfig_Result result = new GetTerminalRoutingConfig_Result(JsonConvert.SerializeObject(terminalRoutings));
                    return result;
                }
                finally
                {
                    ReturnConnection(entry, out entry);
                }
            }
            catch (Exception e)
            {
                ThrowChannelError(e);
            }

            return new GetTerminalRoutingConfig_Result();
        }

        public GetHWSPrintersInfo_Result GetHWSPrintersInfo(GetHWSPrintersInfo request)
        {
            return new GetHWSPrintersInfo_Result("[]");
        }

        public GetHWSMappingsFromDStoPrinters_Result GetHWSMappingsFromDStoPrinters(GetHWSMappingsFromDStoPrinters request)
        {
            return new GetHWSMappingsFromDStoPrinters_Result("{}");
        }

        public GetKitchenDisplayPrinters_Result GetKitchenDisplayPrinters(GetKitchenDisplayPrinters request)
        {
            return new GetKitchenDisplayPrinters_Result("[]");
        }

        public GetStationPrintingHosts_Result GetStationPrintingHosts(GetStationPrintingHosts request)
        {
            return new GetStationPrintingHosts_Result("[]");
        }

        public GetUnsentKOTsJSON_Result GetUnsentKOTsJSON(GetUnsentKOTsJSON request)
        {
            return new GetUnsentKOTsJSON_Result("[]");
        }

        public GetUnsentKOTsXML_Result GetUnsentKOTsXML(GetUnsentKOTsXML request)
        {
            Orders orders = new Orders();
            return new GetUnsentKOTsXML_Result(orders);
        }

        public LogKitchenServiceEvent_Result LogKitchenServiceEvent(LogKitchenServiceEvent request)
        {
            Utils.Log(this, request.jsonLogFromKDS, LogLevel.Trace);
            Utils.Log(this, request.Message, LogLevel.Debug);
            return new LogKitchenServiceEvent_Result();
        }

        public LogKitchenServiceMsgToWSLog_Result LogKitchenServiceMsgToWSLog(LogKitchenServiceMsgToWSLog request)
        {
            Utils.Log(this, JsonConvert.SerializeObject(request), LogLevel.Trace);
            Utils.Log(this, request.message, LogLevel.Debug);
            return new LogKitchenServiceMsgToWSLog_Result();
        }
    }
}