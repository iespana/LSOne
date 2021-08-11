using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Administration.Properties;
using LSOne.ViewPlugins.Administration.Views;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Administration
{
    public class PluginOperations
    {
        public static void ShowAdministrationDiscountAndPriceActivationPage(object sender, EventArgs args)
        {
            ShowAdministationOptions("DiscountPriceActivation");
            

        }
        internal static void ShowAdministationOptions(object sender,EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.AdministrationView());
        }
      

        internal static void ShowAdministationOptions(string tabKey)
        {
            PluginEntry.Framework.ViewController.Add(new Views.AdministrationView(tabKey));
        }

        internal static void ShowNumberSequences(object sender, PluginOperationArguments args)
        {
            if (args.WantsViewReturned)
            {
                args.ResultView = new Views.AdministrationView("NumberSequencesTab");
            }
        }

        internal static RecordIdentifier NewNumberSequence()
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.CurrentStoreID != RecordIdentifier.Empty)
            {
                if (QuestionDialog.Show(Properties.Resources.NewNumberSequenceWarning) == DialogResult.No)
                {
                    return RecordIdentifier.Empty;
                }
            }

            if (PluginEntry.DataModel.HasPermission(Permission.ManageNumberSequences))
            {
                Dialogs.NewNumberSequenceDialog dlg = new Dialogs.NewNumberSequenceDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.NumberSequenceID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "NumberSequence", selectedID, null);
                }
            }

            return selectedID;
        }

        public static bool DeleteNumberSequence(RecordIdentifier numberSequenceID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageNumberSequences))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteNumberSequenceQuestion, Properties.Resources.DeleteNumberSequence) == DialogResult.Yes)
                {
                    Providers.NumberSequenceData.Delete(PluginEntry.DataModel, numberSequenceID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "NumberSequence", numberSequenceID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        internal static void ManageAuditLog(object sender, EventArgs args)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuditLogs))
            {
                PluginEntry.Framework.ViewController.Add(new TruncateAuditLogs());
            }
        }

        internal static void ShowAuditLog(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new AuditLogViewer(PluginEntry.Framework.ViewController.CurrentView));
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.GeneralSetup, "General setup", null), 100);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "General setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                {
                    args.Add(new Item(Properties.Resources.System, "System",null), 2000);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "General setup" && args.ItemKey == "System")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.AdministrationMaintainSettings))
                {
                    args.Add(new ItemButton(Properties.Resources.AdministrativeOptions, Properties.Resources.AdministrativeOptionsDescription, ShowAdministationOptions), 1000);
                }
            }
        }
        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {

                args.Add(new Page(Properties.Resources.Tools, "Tools"), 1000);
        }
        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Tools")
            {
                args.Add(new PageCategory(Properties.Resources.Auditing, "AuditLog"), 400);
                args.Add(new PageCategory(Properties.Resources.Administration, "Administration"), 500); 
            }
        }

        internal static void InsertDefaultData(object sender, EventArgs args)
        {
            if(PluginEntry.Framework.ViewController.CurrentView is Views.AdministrationView)
            {
                if(PluginEntry.Framework.ViewController.CurrentView.ValidateSave())
                {
                    PluginEntry.Framework.ViewController.CurrentView.Save();
                }
            }

            Dialogs.InsertDefaultDataDialog dlg = new Dialogs.InsertDefaultDataDialog();

            dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            // Clear cache since default data packages might have bumped something in the data
            PluginEntry.DataModel.Cache.ClearEntityCache(CacheType.CacheTypeApplicationLifeTime);

            // Tell the home screen of potential change
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "Home", "RetailReports", null);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.DataChangedInDatabase, "AdminSettings", RecordIdentifier.Empty, null);
        }

        public static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Administration.Views.AdministrationView")
            {
                args.Add(new TabControl.Tab(Resources.ActivePriceDiscount,"DiscountPriceActivation", ViewPages.AdministrationDiscountAndPriceActivationPage.CreateInstance), 30);
                args.Add(new TabControl.Tab(Resources.DiscountSettings, ViewPages.AdministrationDiscountSettingsPage.CreateInstance), 40);
                args.Add(new TabControl.Tab(Resources.ScaleUnits, "ScaleUnitsTab", ViewPages.AdministrationScaleUnitsPage.CreateInstance), 50);
                
                if (PluginEntry.DataModel.HasPermission(Permission.ManageNumberSequences))
                {
                    args.Add(new TabControl.Tab(Resources.NumberSequences, "NumberSequencesTab", ViewPages.AdministrationNumberSequencesPage.CreateInstance), 60);
                }

                args.Add(new TabControl.Tab(Resources.DecimalFormats, ViewPages.AdministrationDecimalsPage.CreateInstance), 130);
                args.Add(new TabControl.Tab(Resources.BlankOperations, ViewPages.AdministrationBlankOperationsPage.CreateInstance), 140);
            }
        }
        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.TradeAgreements.Views.CustomerDiscountGroupView.Related")
            {
                var item = new ContextBarItem(Resources.ActivePriceDiscount,
                    ShowAdministrationDiscountAndPriceActivationPage);
             

                arguments.Add(item, 30);
            }
            if (arguments.CategoryKey == "LSOne.ViewPlugins.TradeAgreements.Views.ItemDiscountGroupView.Related")
            {
                var item = new ContextBarItem(Resources.ActivePriceDiscount,
                    ShowAdministrationDiscountAndPriceActivationPage);


                arguments.Add(item, 30);
            }


        }

    }
}
