using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs;
using LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Views;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents
{
    public class PluginOperationsHelper
    {
        internal static void ManageKitchenServiceConfig(string host, int port)
        {
            var dlg = new KitchenServiceConfigDialog(host, port);
            dlg.ShowDialog();
        }

        public static void ShowKitchenServiceProfilesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new KitchenServiceProfilesView());
        }

        public static void ShowKitchenServiceProfileView(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new KitchenServiceProfileView(id));
        }

        public static void ShowFunctionalProfilesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new FunctionalProfilesView());
        }

        internal static void ShowFunctionalProfilesView(RecordIdentifier profileId)
        {
            PluginEntry.Framework.ViewController.Add(new FunctionalProfilesView(profileId));
        }

        internal static void ShowFunctionalProfileView(RecordIdentifier profileId)
        {
            PluginEntry.Framework.ViewController.Add(new FunctionalProfileView(profileId));
        }

        public static void ShowStyleProfilesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new StyleProfilesView());
        }

        public static RecordIdentifier AddKitchenServiceProfile()
        {
            var dlg = new NewKitchenServiceProfileDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                return dlg.ProfileID;
            }

            return RecordIdentifier.Empty;
        }

        internal static void ShowStyleProfilesView(RecordIdentifier profileId)
        {
            PluginEntry.Framework.ViewController.Add(new StyleProfilesView(profileId));
        }

        internal static void ShowStyleProfileView(RecordIdentifier profileId)
        {
            PluginEntry.Framework.ViewController.Add(new StyleProfileView(profileId));
        }

        public static void ShowStyleProfileView(object sender, EventArgs args)
        {
            if (args is PluginOperationArguments)
            {
                if (((PluginOperationArguments)args).WantsViewReturned)
                {
                    ((PluginOperationArguments)args).ResultView = new StyleProfileView(((PluginOperationArguments)args).ID);
                    return;
                }
                else
                {
                    PluginEntry.Framework.ViewController.Add(new StyleProfileView(((PluginOperationArguments)args).ID));
                }
            }
        }

        public static void ShowVisualProfilesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new VisualProfilesView());
        }

        internal static void ShowVisualProfilesView(RecordIdentifier profileId)
        {
            PluginEntry.Framework.ViewController.Add(new VisualProfilesView(profileId));
        }

        internal static void ShowVisualProfileView(RecordIdentifier profileId)
        {
            PluginEntry.Framework.ViewController.Add(new VisualProfileView(profileId));
        }

        public static void ShowDisplayProfilesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new DisplayProfilesView());
        }

        internal static void ShowDisplayProfilesView(RecordIdentifier profileId)
        {
            PluginEntry.Framework.ViewController.Add(new DisplayProfilesView(profileId));
        }

        internal static void ShowDisplayProfileView(RecordIdentifier profileId)
        {
            PluginEntry.Framework.ViewController.Add(new DisplayProfileView(profileId));
        }

        public static void ShowButtonProfilesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new ButtonProfilesView());
        }

        internal static void ShowButtonProfilesView(RecordIdentifier kitchenDisplayHeader)
        {
            PluginEntry.Framework.ViewController.Add(new ButtonProfilesView(kitchenDisplayHeader));
        }

        public static void ShowDisplayStationsView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new DisplaysView());
        }

        internal static void ShowDisplayStationsView(RecordIdentifier kitchenDisplayStationId)
        {
            PluginEntry.Framework.ViewController.Add(new DisplayView(kitchenDisplayStationId));
        }

        internal static void ShowStylesView(object sender, EventArgs args)
        {
            bool canRunOperation = PluginEntry.Framework.CanRunOperation("UIStyles");
            if (canRunOperation)
            {
                PluginEntry.Framework.RunOperation("UIStyles", null, new PluginOperationArguments(null, null));
            }
        }

        internal static void ShowKitchenPrinters(object sender, EventArgs e)
        {
            PluginEntry.Framework.ViewController.Add(new PrintersView());
        }

        internal static void ShowKitchenPrinter(RecordIdentifier kitchenPrinterId)
        {
            PluginEntry.Framework.ViewController.Add(new PrinterView(kitchenPrinterId));
        }

        public static void ShowHeaderPanesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new HeaderPanesView());
        }

        public static void ShowAggregateProfilesView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new AggregateProfilesView());
        }

        internal static void ShowAggregateProfilesView(RecordIdentifier aggregateProfileID)
        {
            PluginEntry.Framework.ViewController.Add(new AggregateProfilesView(aggregateProfileID));
        }

        public static void ShowAggregateGroupsView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new AggregateGroupsView());
        }

        internal static void ShowAggregateGroupsView(RecordIdentifier aggregateGroupID)
        {
            PluginEntry.Framework.ViewController.Add(new AggregateGroupsView(aggregateGroupID));
        }

        internal static void ShowButtonDialog(RecordIdentifier menuHeaderID)
        {
            using (ButtonDialog dlg = new ButtonDialog(menuHeaderID))
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowButtonDialog(RecordIdentifier menuHeaderID, RecordIdentifier posMenuLineID)
        {
            using (ButtonDialog dlg = new ButtonDialog(menuHeaderID, posMenuLineID))
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowHeaderPaneDialog()
        {
            using (HeaderPaneDialog dlg = new HeaderPaneDialog())
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowHeaderPaneDialog(RecordIdentifier sectionId)
        {
            using (HeaderPaneDialog dlg = new HeaderPaneDialog(sectionId))
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowHeaderPaneColumnDialog(RecordIdentifier headerPaneLineID, RecordIdentifier headerPaneID)
        {
            using (HeaderPaneColumnDialog dlg = new HeaderPaneColumnDialog(headerPaneLineID, headerPaneID))
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowHeaderPaneColumnDialog(RecordIdentifier columnId, RecordIdentifier headerPaneLineID, RecordIdentifier headerPaneID)
        {
            using (HeaderPaneColumnDialog dlg = new HeaderPaneColumnDialog(columnId, headerPaneLineID, headerPaneID))
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowAggregateProfileDialog()
        {
            using (AggregateProfileDialog dlg = new AggregateProfileDialog())
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowAggregateProfileDialog(RecordIdentifier aggregateProfileID)
        {
            using (AggregateProfileDialog dlg = new AggregateProfileDialog(aggregateProfileID))
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowAggregateGroupDialog()
        {
            using (AggregateGroupDialog dlg = new AggregateGroupDialog())
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowAggregateGroupDialog(RecordIdentifier aggregateGroupID)
        {
            using (AggregateGroupDialog dlg = new AggregateGroupDialog(aggregateGroupID))
            {
                dlg.ShowDialog();
            }
        }

        internal static void ShowAggregateGroupRelationDialog(RecordIdentifier aggregateProfileID)
        {
            using (AggregateGroupRelationDialog dlg = new AggregateGroupRelationDialog(aggregateProfileID))
            {
                dlg.ShowDialog();
            }
        }

        internal static bool DeleteKitchenServiceProfile(RecordIdentifier ksProfileId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenServiceProfiles))
            {
                if (Providers.StoreData.KdsProfileInUse(PluginEntry.DataModel, ksProfileId))
                {
                    MessageDialog.Show(Properties.Resources.CannotDeleteKdsProfileInUse);
                    return true;
                }
                
                if (QuestionDialog.Show(Properties.Resources.RemoveProfileQuestion, Properties.Resources.RemoveProfile) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayTransactionProfileData.Delete(PluginEntry.DataModel, ksProfileId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenServiceProfile", ksProfileId, null);

                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteKitchenServiceProfiles(List<RecordIdentifier> kitchenServiceProfileIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenServiceProfiles))
            {
                if (QuestionDialog.Show(Properties.Resources.RemoveProfilesQuestion, Properties.Resources.RemoveProfiles) == DialogResult.Yes)
                {
                    string profileNamesCouldNotDelete = "";

                    foreach (var ID in kitchenServiceProfileIds)
                    {
                        if (Providers.StoreData.KdsProfileInUse(PluginEntry.DataModel, ID))
                        {
                            string profileName = Providers.KitchenDisplayTransactionProfileData.Get(PluginEntry.DataModel, ID)?.Text;
                            if (profileName != null)
                            {
                                profileNamesCouldNotDelete += Environment.NewLine + " - " + profileName;
                            }
                        }
                        else 
                        {
                            Providers.KitchenDisplayTransactionProfileData.Delete(PluginEntry.DataModel, ID);
                        }
                    }

                    if (profileNamesCouldNotDelete.Length > 0)
                    {
                        MessageDialog.Show(Properties.Resources.CannotDeleteKdsProfilesInUse + profileNamesCouldNotDelete);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenServiceProfile", null, kitchenServiceProfileIds);

                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static void DeleteDisplayStation(RecordIdentifier kitchenDisplayStationId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteKitchenDisplayStationQuestion, Properties.Resources.DeleteDisplayStation) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayStationData.Delete(PluginEntry.DataModel, kitchenDisplayStationId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayStation", kitchenDisplayStationId, null);
                }
            }
        }

        internal static void DeleteDisplayStations(List<RecordIdentifier> kitchenDisplayStationIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteKitchenDisplayStationsQuestion, Properties.Resources.DeleteDisplayStations) == DialogResult.Yes)
                {
                    foreach (var ID in kitchenDisplayStationIds)
                    {
                        Providers.KitchenDisplayStationData.Delete(PluginEntry.DataModel, ID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayStation", null, kitchenDisplayStationIds);
                }
            }
        }

        internal static void DeleteItemConnections(List<RecordIdentifier> connectionIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteConnectionsQuestion, Properties.Resources.DeleteConnections) == DialogResult.Yes)
                {
                    foreach (var ID in connectionIds)
                    {
                        Providers.KitchenDisplayItemRoutingConnectionData.Delete(PluginEntry.DataModel, ID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayItemConnection", null, connectionIds);
                }
            }
        }

        internal static void DeleteItemConnection(RecordIdentifier connectionId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteConnectionQuestion, Properties.Resources.DeleteConnection) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayItemRoutingConnectionData.Delete(PluginEntry.DataModel, connectionId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayItemConnection", connectionId, null);
                }
            }
        }

        internal static void DeleteHospitalityTypeConnections(List<RecordIdentifier> connectionIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteConnectionsQuestion, Properties.Resources.DeleteConnections) == DialogResult.Yes)
                {
                    foreach (var ID in connectionIds)
                    {
                        Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.Delete(PluginEntry.DataModel, ID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayHospitalityTypeConnection", null, connectionIds);
                }
            }
        }

        internal static void DeleteHospitalityTypeConnection(RecordIdentifier connectionId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteConnectionQuestion, Properties.Resources.DeleteConnection) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayHospitalityTypeRoutingConnectionData.Delete(PluginEntry.DataModel, connectionId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayHospitalityTypeConnection", connectionId, null);
                }
            }
        }

        internal static void DeleteTerminalConnections(List<RecordIdentifier> connectionIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteConnectionsQuestion, Properties.Resources.DeleteConnections) == DialogResult.Yes)
                {
                    foreach (var ID in connectionIds)
                    {
                        Providers.KitchenDisplayTerminalRoutingConnectionData.Delete(PluginEntry.DataModel, ID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayTerminalConnection", null, connectionIds);
                }
            }
        }

        internal static void DeleteTerminalConnection(RecordIdentifier connectionId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteConnectionQuestion, Properties.Resources.DeleteConnection) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayTerminalRoutingConnectionData.Delete(PluginEntry.DataModel, connectionId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayTerminalConnection", connectionId, null);
                }
            }
        }

        internal static bool DeleteFunctionalProfile(RecordIdentifier profileId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProfileQuestion,
                    Properties.Resources.DeleteProfile) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayFunctionalProfileData.Delete(PluginEntry.DataModel, profileId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayFunctionalProfile", profileId, null);
                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteFunctionalProfiles(List<RecordIdentifier> profileIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProfilesQuestion,
                    Properties.Resources.DeleteProfiles) == DialogResult.Yes)
                {
                    foreach (var profileId in profileIds)
                    {
                        Providers.KitchenDisplayFunctionalProfileData.Delete(PluginEntry.DataModel, profileId);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayFunctionalProfile", null, profileIds);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteStyleProfile(RecordIdentifier profileId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProfileQuestion,
                    Properties.Resources.DeleteProfile) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayStyleProfileData.Delete(PluginEntry.DataModel, profileId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayStyleProfile", profileId, null);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteStyleProfiles(List<RecordIdentifier> profileIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProfilesQuestion,
                    Properties.Resources.DeleteProfiles) == DialogResult.Yes)
                {
                    foreach (var profileId in profileIds)
                    {
                        Providers.KitchenDisplayStyleProfileData.Delete(PluginEntry.DataModel, profileId);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayStyleProfile", null, profileIds);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteVisualProfile(RecordIdentifier profileId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProfileQuestion,
                    Properties.Resources.DeleteProfile) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayVisualProfileData.Delete(PluginEntry.DataModel, profileId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayVisualProfile", profileId, null);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteVisualProfiles(List<RecordIdentifier> profileIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProfilesQuestion,
                    Properties.Resources.DeleteProfiles) == DialogResult.Yes)
                {
                    foreach (var profileId in profileIds)
                    {
                        Providers.KitchenDisplayVisualProfileData.Delete(PluginEntry.DataModel, profileId);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayVisualProfile", null, profileIds);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteDisplayProfile(RecordIdentifier profileId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProfileQuestion,
                    Properties.Resources.DeleteProfile) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayProfileData.Delete(PluginEntry.DataModel, profileId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayProfile", profileId, null);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteDisplayProfiles(List<RecordIdentifier> profileIds)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProfilesQuestion,
                    Properties.Resources.DeleteProfiles) == DialogResult.Yes)
                {
                    foreach (var profileId in profileIds)
                    {
                        Providers.KitchenDisplayProfileData.Delete(PluginEntry.DataModel, profileId);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayProfile", null, profileIds);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteDisplayLine(RecordIdentifier profileId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    "Are you sure to delete this line?",
                    "Delete line") == DialogResult.Yes)
                {
                    Providers.KitchenDisplayLineData.Delete(PluginEntry.DataModel, profileId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayLine", profileId, null);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteDisplayLineColumn(RecordIdentifier profileId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    "Are you sure to delete this column?",
                    "Delete column") == DialogResult.Yes)
                {
                    Providers.KitchenDisplayLineColumnData.Delete(PluginEntry.DataModel, profileId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayLineColumn", profileId, null);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        internal static bool DeleteButtonProfile(RecordIdentifier posMenuHeaderID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteButtonProfileQuestion, Properties.Resources.DeleteButtonProfile) == DialogResult.Yes)
                {
                    Providers.PosMenuHeaderData.Delete(PluginEntry.DataModel, posMenuHeaderID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosButtonGridMenuHeader", posMenuHeaderID, null);

                    Providers.PosMenuLineData.DeleteForHeaderID(PluginEntry.DataModel, posMenuHeaderID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosButtonGridMenuLine", posMenuHeaderID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        internal static bool DeleteButtonProfiles(List<RecordIdentifier> posMenuHeaderIDs)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteButtonProfilesQuestion, Properties.Resources.DeleteButtonProfiles) == DialogResult.Yes)
                {
                    foreach (var posMenuHeaderID in posMenuHeaderIDs)
                    {
                        Providers.PosMenuHeaderData.Delete(PluginEntry.DataModel, posMenuHeaderID);
                        Providers.PosMenuLineData.DeleteForHeaderID(PluginEntry.DataModel, posMenuHeaderID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "PosButtonGridMenuHeader", null, posMenuHeaderIDs);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "PosButtonGridMenuLine", null, posMenuHeaderIDs);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        internal static bool DeleteKdsButtons(List<RecordIdentifier> IDs, RecordIdentifier posMenuHeaderId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteButtonsQuestion, Properties.Resources.DeleteButtons) == DialogResult.Yes)
                {
                    foreach (RecordIdentifier id in IDs)
                    {
                        Providers.PosMenuLineData.Delete(PluginEntry.DataModel, id);
                    }

                    DataLayer.BusinessObjects.TouchButtons.PosMenuHeader posMenuHeder = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuHeaderId);
                    posMenuHeder.Columns -= IDs.Count;
                    Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, posMenuHeder);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "PosButtonGridMenuLine", null, IDs);

                    return true;
                }
            }

            return false;
        }

        internal static bool DeleteKdsButton(RecordIdentifier posMenuLineID, RecordIdentifier buttonGridMenuId)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteButtonQuestion, Properties.Resources.DeleteButton) == DialogResult.Yes)
                {
                    Providers.PosMenuLineData.Delete(PluginEntry.DataModel, posMenuLineID);

                    DataLayer.BusinessObjects.TouchButtons.PosMenuHeader buttonGridMenu = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, buttonGridMenuId);
                    buttonGridMenu.Columns -= 1;
                    Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, buttonGridMenu);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosButtonGridMenuLine", posMenuLineID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        internal static void DeleteKitchenPrinter(RecordIdentifier kitchenPrinterId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteKitchenPrinterQuestion, Properties.Resources.DeleteKitchenPrinter) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayPrinterData.Delete(PluginEntry.DataModel, kitchenPrinterId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenPrinter", kitchenPrinterId, null);
                }
            }
        }

        internal static bool DeleteProductionSection(RecordIdentifier sectionId)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteProductionSectionQuestion,
                    Properties.Resources.DeleteProductionSection) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayProductionSectionData.Delete(PluginEntry.DataModel, sectionId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayProductionSection", sectionId, null);
                    retValue = true;
                }
            }

            return retValue;
        }

        internal static bool DeleteItemSectionRouting(RecordIdentifier routingId)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteItemSectionRoutingQuestion,
                    Properties.Resources.DeleteItemSectionRouting) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayItemSectionRoutingData.Delete(PluginEntry.DataModel, routingId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayItemSectionRouting", routingId, null);
                    retValue = true;
                }
            }

            return retValue;
        }

        internal static bool DeleteSectionStationRouting(RecordIdentifier routingId)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteSectionStationRoutingQuestion,
                    Properties.Resources.DeleteSectionStationRouting) == DialogResult.Yes)
                {
                    Providers.KitchenDisplaySectionStationRoutingData.Delete(PluginEntry.DataModel, routingId);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplaySectionStationRouting", routingId, null);
                    retValue = true;
                }
            }

            return retValue;
        }

        internal static bool DeleteHeaderPane(RecordIdentifier headerPaneID)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                List<KitchenDisplayVisualProfile> visualProfiles = Providers.KitchenDisplayVisualProfileData.GetList(PluginEntry.DataModel, headerPaneID);
                if (visualProfiles != null && visualProfiles.Count > 0)
                {
                    MessageDialog.Show(Properties.Resources.HeaderProfileDelete, MessageBoxButtons.OK);
                    retValue = false;
                }
                else if (QuestionDialog.Show(Properties.Resources.DeleteHeaderPaneQuestion, Properties.Resources.DeleteHeaderProfile) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayHeaderPaneData.Delete(PluginEntry.DataModel, headerPaneID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayHeaderPane", headerPaneID, null);
                    retValue = true;
                }
            }

            return retValue;
        }

        internal static bool DeleteHeaderPaneColumn(RecordIdentifier headerPaneColumnID)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteHeaderPaneColumnQuestion,
                    Properties.Resources.DeleteHeaderPaneColumn) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayHeaderPaneLineColumnData.Delete(PluginEntry.DataModel, headerPaneColumnID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayHeaderPaneColumn", headerPaneColumnID, null);
                    retValue = true;
                }
            }

            return retValue;
        }

        internal static bool DeleteHeaderPaneLine(RecordIdentifier headerPaneLineID)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteHeaderPaneLineQuestion,
                    Properties.Resources.DeleteHeaderPaneLine) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayHeaderPaneLineData.Delete(PluginEntry.DataModel, headerPaneLineID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayHeaderPaneLine", headerPaneLineID, null);
                    retValue = true;
                }
            }

            return retValue;
        }

        internal static bool DeleteAggregateProfile(RecordIdentifier aggregateProfileID)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteAggregateProfileQuestion,
                    Properties.Resources.DeleteAggregateProfile) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayAggregateProfileData.Delete(PluginEntry.DataModel, aggregateProfileID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayAggregateProfile", aggregateProfileID, null);
                    retValue = true;
                }
            }

            return retValue;
        }

        internal static bool DeleteAggregateGroup(RecordIdentifier aggregateGroupID)
        {
            bool retValue = false;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(
                    Properties.Resources.DeleteAggregateGroupQuestion,
                    Properties.Resources.DeleteAggregateGroup) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayAggregateProfileGroupData.Delete(PluginEntry.DataModel, aggregateGroupID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayAggregateGroup", aggregateGroupID, null);
                    retValue = true;
                }
            }

            return retValue;
        }

        internal static void DeleteAggregateGroupItems(List<RecordIdentifier> aggregateGroupItemIDs)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteAggregateGroupItemsQuestion, Properties.Resources.DeleteAggregateGroupItems) == DialogResult.Yes)
                {
                    foreach (RecordIdentifier ID in aggregateGroupItemIDs)
                    {
                        Providers.KitchenDisplayAggregateGroupItemData.Delete(PluginEntry.DataModel, ID);
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "KitchenDisplayAggregateGroupItem", null, aggregateGroupItemIDs);
                }
            }
        }

        internal static void DeleteAggregateGroupItem(RecordIdentifier aggregateGroupItemID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteAggregateGroupItemQuestion, Properties.Resources.DeleteAggregateGroupItem) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayAggregateGroupItemData.Delete(PluginEntry.DataModel, aggregateGroupItemID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayAggregateGroupItem", aggregateGroupItemID, null);
                }
            }
        }

        internal static void DeleteAggregateGroupRelation(RecordIdentifier aggregateGroupRelationID)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayProfiles))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteAggregateGroupRelationQuestion, Properties.Resources.DeleteAggregateGroupRelation) == DialogResult.Yes)
                {
                    Providers.KitchenDisplayAggregateProfileGroupData.RemoveGroupFromProfile(PluginEntry.DataModel, aggregateGroupRelationID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "KitchenDisplayAggregateGroupRelation", aggregateGroupRelationID, null);
                }
            }
        }

        internal static void CreateDisplayStation()
        {
            DialogResult result;
            RecordIdentifier selectedId;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
            {
                var dlg = new KitchenDisplayDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                if (result == DialogResult.OK)
                {
                    selectedId = dlg.KitchenDisplayStationId;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenDisplayStation", selectedId, null);
                    ShowDisplayStationsView(selectedId);
                }

                PluginEntry.Framework.ResumeSearchBarClosing();
            }
        }

        internal static void CreateKitchenPrinter()
        {
            DialogResult result;
            RecordIdentifier selectedId;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageKitchenPrinters))
            {
                var dlg = new NewKitchenPrinterDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                if (result == DialogResult.OK)
                {
                    selectedId = dlg.Id;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenPrinter", selectedId, null);
                    ShowKitchenPrinter(selectedId);
                }

                PluginEntry.Framework.ResumeSearchBarClosing();
            }
        }

        internal static List<HeaderPaneProfile> SortList(List<HeaderPaneProfile> headerPaneProfiles, HeaderPaneProfileSort sort, bool ascending)
        {
            switch (sort)
            {
                default:
                case HeaderPaneProfileSort.Description:
                    return ascending ? headerPaneProfiles.OrderBy(x => x.Name).ToList() : headerPaneProfiles.OrderByDescending(x => x.Name).ToList();
                case HeaderPaneProfileSort.NumberOfLines:
                    return ascending ? headerPaneProfiles.OrderBy(x => x.HeaderLines.Count).ToList() : headerPaneProfiles.OrderByDescending(x => x.HeaderLines.Count).ToList();
            }
        }

    }
}