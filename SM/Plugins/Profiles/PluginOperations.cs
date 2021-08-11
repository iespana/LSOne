using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Profiles.Dialogs;
using LSOne.ViewPlugins.Profiles.Properties;
using Microsoft.Win32;
using LSOne.DataLayer.BusinessObjects.UserManagement;

namespace LSOne.ViewPlugins.Profiles
{
    internal class PluginOperations
    {
        public static void ShowVisualProfilesSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.VisualProfilesView());
        }

        public static void ShowUserProfilesSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.UserProfilesView());
        }

        public static void ShowVisualProfilesSheet(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.VisualProfilesView(id));
        }

        public static void ShowCSVImportProfilesSheet(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CSVImportProfilesView(id));
        }

        public static void ShowHardwareProfilesSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.HardwareProfilesView());
        }

        public static void ShowHardwareProfilesSheet(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.HardwareProfilesView(id));
        }

        public static void ShowFunctionalityProfilesSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.FunctionalityProfilesView());
        }

        public static void ShowFunctionalityProfilesSheet(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.FunctionalityProfilesView(id));
        }

        public static void ShowCSVImportProfileSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CSVImportProfilesView());
        }

        public static void ShowFormProfileSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.FormProfilesView());
        }

        public static void ShowStyleProfileSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.StyleProfilesView());
        }

        public static void ShowContextViewSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ContextsView());
        }

        public static void ShowStylesProfileSheet(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.StyleProfilesView(id));
        }

        public static void ShowStylesProfilesSheet()
        {
            PluginEntry.Framework.ViewController.Add(new Views.StyleProfilesView());
        }

        

        public static bool DeleteFunctionalityProfile(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteFuncProfileQuestion, Properties.Resources.DeleteFuncProfile) == DialogResult.Yes)
                {
                    Providers.FunctionalityProfileData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "FunctionalityProfile", (string)id, null);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteStyleProfile(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteStyleProfileQuestion, Properties.Resources.DeleteStyleProfile) == DialogResult.Yes)
                {
                    Providers.StyleProfileData.Delete(PluginEntry.DataModel, id);
                    Providers.PosStyleProfileLineData.DeleteLines(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "StyleProfiles", (string)id, null);

                    return true;
                }
            }

            return false;
        }

        public static void DeleteWindowsPrinterConfiguration(RecordIdentifier ID)
        {
            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.HardwareProfileEdit) || PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageStationPrinting))
            {
                if (QuestionDialog.Show(Resources.DeleteWindowsPrinterConfigurationQuestion, Resources.DeleteWindowsPrinterConfiguration) == DialogResult.Yes)
                {
                    Providers.WindowsPrinterConfigurationData.Delete(PluginEntry.DataModel, ID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "WindowsPrinterConfiguration", ID, null);
                }
            }
        }

        public static bool DeleteStyleProfileLine(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteStyleProfileLineQuestion, Properties.Resources.DeleteStyleProfileLine) == DialogResult.Yes)
                {
                    Providers.PosStyleProfileLineData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "ContextStyle", id, null);

                    return true;
                }
            }

            return false;
        }
        

        public static bool DeleteHardwareProfile(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.HardwareProfileEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteHardwareProfileQuestion, Properties.Resources.DeleteHardwareProfile) == DialogResult.Yes)
                {
                    Providers.HardwareProfileData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "HardwareProfile", (string)id, null);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteVisualProfile(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteProfileQuestion, Properties.Resources.DeleteProfile) == DialogResult.Yes)
                {
                    Providers.VisualProfileData.Delete(PluginEntry.DataModel, (string)id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "VisualProfile", (string)id, null);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteFormProfileLine(RecordIdentifier profileId, RecordIdentifier formTypeId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.FormProfileEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteFormProfileLineQuestion, Properties.Resources.DeleteFormProfileLine) == DialogResult.Yes)
                {
                    Providers.FormProfileLineData.DeleteProfileLine(PluginEntry.DataModel, profileId, formTypeId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "deleteReceiptLine", (Guid)profileId, null);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteFormProfile(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.FormProfileEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteFormProfileQuestion, Properties.Resources.DeleteFormProfile) == DialogResult.Yes)
                {
                    List<FormProfileLine> profileLines = Providers.FormProfileLineData.Get(PluginEntry.DataModel, id);
                    foreach (FormProfileLine line in profileLines)
                    {
                        Providers.FormProfileLineData.DeleteProfileLine(PluginEntry.DataModel, id, line.ReceiptTypeID);
                    }
                    Providers.FormProfileData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "FormProfile", (Guid)id, null);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteCSVImportProfile(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
            {
                if (QuestionDialog.Show(
                        Properties.Resources.DeleteCSVImportProfileQuestion, 
                        Properties.Resources.DeleteCSVImportProfile) 
                    == DialogResult.Yes)
                {
                    Providers.ImportProfileData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "CSVImportProfile", id, null);

                    return true;
                }
            }

            return false;
        }

        public static bool DeleteCSVImportProfileLine(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
            {
                if (QuestionDialog.Show(
                        Properties.Resources.DeleteCSVImportProfileLineQuestion,
                        Properties.Resources.DeleteCSVImportProfileLine)
                    == DialogResult.Yes)
                {
                    Providers.ImportProfileLineData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "CSVImportProfile", id, null);

                    return true;
                }
            }

            return false;
        }

        public static bool SetDefaultCSVImportProfile(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
            {
                if (QuestionDialog.Show(
                        Properties.Resources.SetDefaultCSVImportProfileQuestion,
                        Properties.Resources.SetDefaultCSVImportProfile)
                    == DialogResult.Yes)
                {
                    if (Providers.ImportProfileLineData.GetSelectList(PluginEntry.DataModel, id).Count == 0)
                    {
                        MessageDialog.Show(Resources.CSVMappingNotSet+" "+Resources.AddMappingToProfile);
                        return true;
                    }
                    else
                    {
                        Providers.ImportProfileData.SetAsDefault(PluginEntry.DataModel, id);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "CSVImportProfile", id, null);
                        return false;
                    }
                }
            }

            return false;
        }

        public static void ShowVisualProfileSheet(object sender, string profileID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.VisualProfileView(profileID));
        }

        public static void ShowUserProfileSheet(object sender, RecordIdentifier profileID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.UserProfileView(profileID));
        }

        public static void ShowUserProfileSheet(RecordIdentifier profileID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.UserProfileView(profileID));
        }

        public static bool ShowEditUserProfileDialog(List<User> users)
        {
            DialogResult result = new EditUserProfileDialog(users).ShowDialog();
            if (result == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "User", RecordIdentifier.Empty, null);
                return true;
            }
            return false;
        }

        public static void ShowCSVImportProfileSheet(object sender, RecordIdentifier profileID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CSVImportProfileView(profileID));
        }

        public static void ShowHardwareProfileSheet(RecordIdentifier profileID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.HardwareProfileView(profileID));
        }

        public static void ShowStyleProfileSheet(RecordIdentifier profileID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.StyleProfileView(profileID));
        }

        public static void ShowStyleProfileSheet(object sender, RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.StyleProfileView(id));
        }

        public static void ShowFunctionalityProfileSheet(object sender, RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.FunctionalityProfileView(id));
        }

        /// <summary>
        /// Shows the new functionality profile dialog without showing the functionality profile view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void AddFunctionalityProfile(object sender, PluginOperationArguments args)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileEdit))
            {
                Dialogs.NewFunctionalityProfileDialog dlg = new Dialogs.NewFunctionalityProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "FunctionalityProfile", selectedID, null);                    
                }
            }            
        }

        /// <summary>
        /// Shows the new hardware profile dialog without showing the hardware profile view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void AddHardwareProfile(object sender, PluginOperationArguments args)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.HardwareProfileEdit))
            {
                Dialogs.NewHardwareProfileDialog dlg = new Dialogs.NewHardwareProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "HardwareProfile", selectedID, null);                    
                }
            }
        }

        /// <summary>
        /// Shows the new visual profile dialog without showing the visual profile view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void AddVisualProfile(object sender, PluginOperationArguments args)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.VisualProfileEdit))
            {
                Dialogs.NewVisualProfileDialog dlg = new Dialogs.NewVisualProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "VisualProfile", selectedID, null);                    
                }
            }
        }

        public static RecordIdentifier NewFunctionalityProfile()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileEdit))
            {
                Dialogs.NewFunctionalityProfileDialog dlg = new Dialogs.NewFunctionalityProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "FunctionalityProfile", selectedID, null);

                    PluginOperations.ShowFunctionalityProfileSheet(null, selectedID);
                }
            }

            return selectedID;
        }

        public static PosContext NewPosContext()
        {
            return EditPosContext(RecordIdentifier.Empty);            
        }

        public static PosContext EditPosContext(RecordIdentifier id)
        {
            PosContext selectedContext = null;

            if (PluginEntry.DataModel.HasPermission(Permission.StyleProfileEdit))
            {
                Dialogs.NewContext dlg = new Dialogs.NewContext(id);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedContext = dlg.Context;
                    dlg.Close();
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosContext", selectedContext.ID, null);
                }
            }

            return selectedContext;
        }

        public static bool DeletePosContext(List<RecordIdentifier> IDs)
        {
            if (IDs.Count == 0)
            {
                return false;
            }

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ContextEdit))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteContextQuestion, Properties.Resources.Delete) == DialogResult.Yes)
                {                    
                    foreach (RecordIdentifier id in IDs)
                    {                        
                        Providers.PosContextData.Delete(PluginEntry.DataModel, id);                        
                    }                    

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "PosContext", null, IDs);

                    return true;
                }
            }

            return false;
        }

        

        public static RecordIdentifier NewHardwareProfile()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.HardwareProfileEdit))
            {
                Dialogs.NewHardwareProfileDialog dlg = new Dialogs.NewHardwareProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "HardwareProfile", selectedID, null);

                    PluginOperations.ShowHardwareProfileSheet(selectedID);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewUserProfile()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles))
            {
                NewUserProfileDialog dlg = new NewUserProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "UserProfile", selectedID, null);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewStyleProfile()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.StyleProfileEdit))
            {
                Dialogs.NewStyleProfileDialog dlg = new Dialogs.NewStyleProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "StyleProfiles", selectedID, null);
                    PluginOperations.ShowStyleProfileSheet(selectedID);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewFormProfile()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.FormProfileEdit))
            {
                FormProfileDialog dlg = new FormProfileDialog();

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "FormProfile", selectedID, null);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier EditFormProfile(RecordIdentifier profileID)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.FormProfileEdit))
            {
                FormProfileDialog dlg = new FormProfileDialog(profileID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "FormProfile", selectedID, null);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewContextStyle(RecordIdentifier profileID)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.StyleProfileEdit))
            {
                Dialogs.NewStyleProfileLine dlg = new Dialogs.NewStyleProfileLine(profileID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    selectedID = dlg.ProfileLineID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "ContextStyle", dlg.ProfileLineID, null);
                }
            }
            return selectedID;
        }

        public static RecordIdentifier NewContextStyle(RecordIdentifier profileID, RecordIdentifier profileLineID)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.StyleProfileEdit))
            {
                Dialogs.NewStyleProfileLine dlg = new Dialogs.NewStyleProfileLine(profileID, profileLineID);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //go back into StyleProfileView
                    selectedID = dlg.ProfileLineID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "ContextStyle", dlg.ProfileLineID, null);
                }
            }
            return selectedID;
        }     

        public static RecordIdentifier NewVisualProfile()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileEdit))
            {
                Dialogs.NewVisualProfileDialog dlg = new Dialogs.NewVisualProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "VisualProfile", selectedID, null);

                    PluginOperations.ShowVisualProfileSheet(null, (string)selectedID);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewCSVImportProfile()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
            {
                NewCSVImportProfileDialog dlg = new NewCSVImportProfileDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "CSVImportProfile", selectedID, null);

                    PluginOperations.ShowCSVImportProfileSheet(null, selectedID);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewCSVImportProfileLine(RecordIdentifier parentImportProfileId)
        {
            RecordIdentifier selectedId = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
            {
                NewCSVImportProfileLineDialog dlg = new NewCSVImportProfileLineDialog(parentImportProfileId, selectedId);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedId = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "CSVImportProfile", selectedId, null);
                }
            }

            return selectedId;
        }

        public static RecordIdentifier EditCSVImportProfileLine(RecordIdentifier profileLineId)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
            {
                NewCSVImportProfileLineDialog dlg = new NewCSVImportProfileLineDialog(RecordIdentifier.Empty, profileLineId);
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    profileLineId = dlg.ProfileID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "CSVImportProfile", profileLineId, null);
                }
            }

            return profileLineId;
        }

        public static RecordIdentifier CreateProfileLine(RecordIdentifier profileID)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.FormProfileEdit))
            {
                FormProfileLineDialog dlg = new FormProfileLineDialog(profileID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    selectedID = new RecordIdentifier(dlg.ProfileID, dlg.FormTypeID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "FormProfile", selectedID, null);
                }
            }
            return selectedID;
        }

        public static RecordIdentifier EditProfileLine(RecordIdentifier profileID, RecordIdentifier formTypeID)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;
            if (PluginEntry.DataModel.HasPermission(Permission.FormProfileEdit))
            {
                FormProfileLineDialog dlg = new FormProfileLineDialog(profileID, formTypeID);

                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    selectedID = new RecordIdentifier(dlg.ProfileID, dlg.FormTypeID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "FormProfile", selectedID, null);
                }
            }
            return selectedID;
        }

        public static void ShowWindowsPrinterConfigurationDialog(RecordIdentifier configID)
        {
            if(PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.HardwareProfileEdit) || PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.ManageStationPrinting))
            {
                WindowsPrinterConfigurationDialog dlg = new WindowsPrinterConfigurationDialog(configID);

                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "WindowsPrinterConfiguration", dlg.PrinterConfigurationID, null);
                }
            }
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.StoreSetup, "Store setup", null), 75);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Store setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileView) ||
                    PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileView) ||
                    PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileView) ||
                    PluginEntry.DataModel.HasPermission(Permission.HardwareProfileView) || 
                    PluginEntry.DataModel.HasPermission(Permission.StyleProfileView) ||
                    PluginEntry.DataModel.HasPermission(Permission.FormProfileView))
                {
                    args.Add(new Item(Properties.Resources.Profiles, "Profiles", null), 500);
                   
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Store setup" && args.ItemKey == "Profiles")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileView))
                {
                    args.Add(new ItemButton(Properties.Resources.VisualProfiles, Properties.Resources.VisualProfilesDescription, new EventHandler(ShowVisualProfilesSheet)), 300);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileView))
                {
                    args.Add(new ItemButton(Properties.Resources.FunctionalityProfiles, Properties.Resources.FunctionalityProfilesDescription, new EventHandler(ShowFunctionalityProfilesSheet)), 400);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.HardwareProfileView))
                {
                    args.Add(new ItemButton(Properties.Resources.HardwareProfiles, Properties.Resources.HardwareProfilesDescription, new EventHandler(ShowHardwareProfilesSheet)), 500);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.FormProfileView))
                {
                    args.Add(new ItemButton(Properties.Resources.FormProfiles, Properties.Resources.FormProfileDescription, new EventHandler(ShowFormProfileSheet)), 900);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
                {
                    args.Add(new ItemButton(Properties.Resources.CSVImportProfiles, Properties.Resources.CSVImportProfilesDescription, new EventHandler(ShowCSVImportProfileSheet)), 1000);
                }

                if(PluginEntry.DataModel.HasPermission(Permission.ManageUserProfiles))
                {
                    args.Add(new ItemButton(Resources.UserProfiles, Resources.UserProfilesTooltipDescription, new EventHandler(ShowUserProfilesSheet)), 1100);
                }
            }
        }

        internal static List<string> GetRegistryStrings(string oposdevice)
        {
            List<string> registryStrings = new List<string>();

            using (RegistryKey Key = Registry.LocalMachine.OpenSubKey("Software\\OLEforRetail\\ServiceOPOS\\" + oposdevice + "\\", false))
            {
                if (Key != null)
                {
                    registryStrings = Key.GetSubKeyNames().ToList();
                }
                else
                {
                    using (RegistryKey Key2 = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\OLEforRetail\\ServiceOPOS\\" + oposdevice + "\\", false))
                    {
                        if (Key2 != null)
                        {
                            registryStrings = Key2.GetSubKeyNames().ToList();
                        }
                    }
                }
            }

            return registryStrings;
        }

        internal static void SetRegistryStrings(string oposdevice, ComboBox combobox)
        {
            combobox.Items.Clear();
            combobox.Items.AddRange(GetRegistryStrings(oposdevice).ToArray());
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Resources.Sites, "Sites"), 700);
            args.Add(new Page(Resources.Sites, "Inventory"), 400);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sites")
            {
                args.Add(new PageCategory(Resources.Profiles, "Profiles"), 200);
                args.Add(new PageCategory(Resources.Forms, "Forms"), 500);
            }

            if (args.PageKey == "Inventory")
            {
                args.Add(new PageCategory(Resources.Stock, "Stock"), 300);
            }

            if (args.PageKey == "Tools")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
                {
                    args.Add(new PageCategory(Resources.Import, "Import"), 200);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sites" && args.CategoryKey == "Profiles")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.VisualProfileView))
                {
                    args.Add(new CategoryItem(
                        Resources.VisualProfileRibbon,
                        Resources.VisualProfiles,
                        Resources.VisualProfilesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        ShowVisualProfilesSheet,
                        "VisualProfiles"), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileView))
                {
                    args.Add(new CategoryItem(
                        Resources.FunctionalityProfilesRibbon,
                        Resources.FunctionalityProfiles,
                        Resources.FunctionalityProfilesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        ShowFunctionalityProfilesSheet,
                        "FunctionalityProfiles"), 20);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.HardwareProfileView))
                {
                    args.Add(new CategoryItem(
                        Resources.HardwareProfilesRibbon,
                        Resources.HardwareProfiles,
                        Resources.HardwareProfilesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        ShowHardwareProfilesSheet,
                        "HardwareProfiles"), 30);
                }
            }

            if (args.PageKey == "Sites" && args.CategoryKey == "Forms")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.FormProfileView))
                {
                    args.Add(new CategoryItem(
                        Resources.FormProfiles,
                        Resources.FormProfiles,
                        Resources.FormProfilesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                         ShowFormProfileSheet,
                        "FormProfiles"), 20);
                }
            }

            if (args.PageKey == "Tools" && args.CategoryKey == "Import")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageImportProfiles))
                {
                    args.Add(new CategoryItem(
                        Resources.CSVImportProfiles,
                        Resources.CSVImportProfiles,
                        Resources.CSVImportProfilesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        Resources.CSV_import_profiles_16,
                        null,
                        ShowCSVImportProfileSheet,
                        "ImportProfiles"), 30);
                }
            }

            if (args.PageKey == "Users" && args.CategoryKey == "Users")
            {
                args.Add(new CategoryItem(
                            Resources.UserProfiles,
                            Resources.UserProfiles,
                            Resources.UserProfilesTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            new EventHandler(ShowUserProfilesSheet),
                            "ViewUserProfiles"), 30);
            }
        }

        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            if (args.Key == "RibbonPrintingHosts" && (PluginEntry.DataModel.HasPermission(Permission.HardwareProfileView) || PluginEntry.DataModel.HasPermission(Permission.HardwareProfileEdit)))
            {
                args.AddMenu(new ExtendedMenuItem(
                            Resources.WindowsPrinters,
                            null,
                            20,
                            ShowWindowsPrinterConfigurationsView));
            }
        }

        public static void ShowWindowsPrinterConfigurationsView(object sender, EventArgs e)
        {
            PluginEntry.Framework.ViewController.Add(new Views.WindowsPrinterConfigurationsView());
        }

        public static void ShowWindowsPrinterConfigurationsView(RecordIdentifier configurationID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.WindowsPrinterConfigurationsView(configurationID));
        }

        public static void ShowWindowsPrinterConfigurationsView(object sender, PluginOperationArguments args)
        {
            if (args != PluginOperationArguments.Empty)
            {
                ShowWindowsPrinterConfigurationsView(args.ID);
            }
            else
            {
                ShowWindowsPrinterConfigurationsView(RecordIdentifier.Empty);
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.SerialNumbers.Views.SerialNumbersView.Related")
            {
                arguments.Add(new ContextBarItem(Resources.Related_CSVProfiles, "CSVProfiles", true, ShowCSVProfiles), 1);
            }

            if (arguments.CategoryKey == "LSOne.ViewPlugins.User.Views.User.Related" ||
                arguments.CategoryKey == "LSOne.ViewPlugins.User.Views.UsersView.Related")
            {
                arguments.Add(new ContextBarItem(Resources.UserProfiles, "UserProfiles", true, ShowUserProfilesSheet), 20);
            }
        }

        private static void ShowCSVProfiles(object sender, ContextBarClickEventArguments args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CSVImportProfilesView());
        }
    }
}
