using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using System.Collections.Generic;
using LSOne.ViewCore;

namespace LSOne.ViewPlugins.Infocodes
{
    internal class PluginOperations
    {
        public static void ShowInfocodeSetupSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.InfocodesView());

        }

        public static void ShowInfocodeSetupSheet(RecordIdentifier selectedInfocodeID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.InfocodesView(selectedInfocodeID));
        }

        public static void ShowCrossSellingGroupInfocodeSetupSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CrossAndModifierInfocodesView( UsageCategoriesEnum.CrossSelling));
        }

        public static void ShowItemModifierGroupInfocodeSetupSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.CrossAndModifierInfocodesView( UsageCategoriesEnum.ItemModifier));
        }

        public static void ShowModifierGroupInfocodeSetupSheet(object sender, EventArgs args)
        {
            //PluginEntry.Framework.ViewController.Add(new Views._ModifierGroupInfocodesView());
        }

        public static void ShowInfocodes(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.InfocodesView(id));
        }

        public static void ShowInfocode(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.InfocodeView(id));
        }

        public static void ShowGroupInfocode(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.GroupInfocodeView(id));
        }

        public static void ShowSubcode(RecordIdentifier subCode)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SubcodeView(subCode));
        }

        public static void ShowGroupInfocode(RecordIdentifier id, UsageCategoriesEnum usageCategory)
        {
            PluginEntry.Framework.ViewController.Add(new Views.GroupInfocodeView(id,usageCategory));
        }
       
        #region InfocodesView
        public static RecordIdentifier NewInfocode(UsageCategoriesEnum usageCategory)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit))
            {
                Dialogs.NewInfocodeDialog dlg = new Dialogs.NewInfocodeDialog(usageCategory);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.InfocodeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Infocode", selectedID, null);

                    PluginOperations.ShowInfocode(selectedID);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewInfocodeGroup(UsageCategoriesEnum usageCategory = UsageCategoriesEnum.None)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit))
            {
                Dialogs.NewGroupInfocodeDialog dlg = new Dialogs.NewGroupInfocodeDialog(usageCategory);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.InfocodeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "GroupInfocode", selectedID, null);

                    PluginOperations.ShowGroupInfocode(selectedID, usageCategory);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewSubcode(RecordIdentifier id)
        {
            DialogResult result;
            RecordIdentifier selectedID = new RecordIdentifier();

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit))
            {
                Dialogs.NewSubcodeDialog dlg = new Dialogs.NewSubcodeDialog(id);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.SubCode;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Subcode", selectedID, null);

                    PluginOperations.ShowSubcode(selectedID);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewGroupToInfocodeListConnection(RecordIdentifier groupTrigger)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit))
            {
                Dialogs.NewInfocodeSpecificGroupConnectionDialog dlg = new Dialogs.NewInfocodeSpecificGroupConnectionDialog(groupTrigger);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    
                    selectedID = dlg.SubcodeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "GroupSubcode", selectedID, null);
                }
            }

            return selectedID;
        }
        #endregion

        #region InfocodeSpecific
        public static InfocodeSpecific NewInfocodeSpecific(RecordIdentifier refRelationID, RefTableEnum tableRefID, UsageCategoriesEnum usageCategory, RecordIdentifier infocodeID, bool newRecord)
        {
            return NewInfocodeSpecific(refRelationID, "", tableRefID, usageCategory, infocodeID, newRecord);
        }

        public static InfocodeSpecific NewInfocodeSpecific(
            RecordIdentifier refRelationID, 
            RecordIdentifier refRelation2ID, 
            RefTableEnum tableRefID, 
            UsageCategoriesEnum usageCategory, 
            RecordIdentifier infocodeID, 
            bool newRecord)
        {
            return NewInfocodeSpecific(refRelationID, refRelation2ID, "", tableRefID, usageCategory, infocodeID, newRecord);
        }

        public static InfocodeSpecific NewInfocodeSpecific(
            RecordIdentifier refRelationID, 
            RecordIdentifier refRelation2ID, 
            RecordIdentifier refRelation3ID, 
            RefTableEnum tableRefID, 
            UsageCategoriesEnum usageCategory, 
            RecordIdentifier infocodeID, 
            bool newRecord)
        {
            DialogResult result;
            //RecordIdentifier selectedID = RecordIdentifier.Empty;
            InfocodeSpecific createdInfoCode = null;
            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit))
            {
                Dialogs.InfocodeSpecificDialog dlg = new Dialogs.InfocodeSpecificDialog(refRelationID, refRelation2ID, refRelation3ID, tableRefID, usageCategory, infocodeID, newRecord);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    createdInfoCode = dlg.InfocodeSpecific;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "InfocodeSpecific", dlg.InfocodeSpecificID, null);
                }
            }

            return createdInfoCode;
        }

        public static bool DeleteInfocodeSpecific(RecordIdentifier infocodeSpecificID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();
          
            InfocodeSpecific infocodeSpecific = Providers.InfocodeSpecificData.Get(PluginEntry.DataModel, infocodeSpecificID);
            string deleteQuestion = Properties.Resources.DeleteInfocodeSpecificQuestion.Replace("#1", infocodeSpecific.InfocodeDescription);
            deleteQuestion = deleteQuestion.Replace("#2", infocodeSpecific.RefRelationDescription);

            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeEdit))
            {
                if (QuestionDialog.Show(deleteQuestion, Properties.Resources.DeleteInfocodeSpecific) == DialogResult.Yes)
                {
                    Providers.InfocodeSpecificData.Delete(PluginEntry.DataModel, infocodeSpecificID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "InfocodeSpecific", infocodeSpecificID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }
        #endregion

        #region PluginEntry.Init

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.Store.Views.StoreTenderView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Infocodes, new ContextbarClickEventHandler(ShowInfocodeSetupSheet)), 251);
                }
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Profiles.Views.FunctionalityProfileView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Infocodes, new ContextbarClickEventHandler(ShowInfocodeSetupSheet)), 251);
                }
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.ItemView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {

                    arguments.Add(new ContextBarItem(Properties.Resources.Infocodes, new ContextbarClickEventHandler(ShowInfocodeSetupSheet)), 251);

                    
                }
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.RetailItems.Views.RetailGroupView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Infocodes, ShowInfocodeSetupSheet), 251);
                }
            }
            else if (arguments.CategoryKey == "LSOne.ViewPlugins.Customer.Views.CustomerView.Related")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Infocodes, ShowInfocodeSetupSheet), 251);
                }
            }
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Store.Views.StoreTenderView")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(
                        new TabControl.Tab(Properties.Resources.Infocodes,
                            ViewPages.StoreTenderInfocodePage.CreateInstance
                            ),
                        500);
                }
            }
            else if (args.ContextName == "LSOne.ViewPlugins.Store.Dialogs.CardTypeDialog")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(
                        new TabControl.Tab(Properties.Resources.Infocodes,
                            ViewPages.StoreTenderCardTypeInfocodePage.CreateInstance
                            ),
                        500);
                }
            }
            else if (args.ContextName == "LSOne.ViewPlugins.Profiles.Views.FunctionalityProfileView")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(
                        new TabControl.Tab(Properties.Resources.Infocodes,
                            ViewPages.FuncProfileInfocodeTabPage.CreateInstance
                            ),
                        120);
                }
            }            
            else if (args.ContextName == "LSOne.ViewPlugins.RetailItems.Views.ItemView")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(
                        new TabControl.Tab(Properties.Resources.Infocodes, ItemTabKey.Infocodes.ToString(),
                            ViewPages.InfocodeSharedPage.CreateInstance
                            ){ Enabled = ((RetailItem)args.InternalContext).ItemType != ItemTypeEnum.MasterItem && !args.IsMultiEdit},
                        500);

                    args.Add(
                        new TabControl.Tab(Properties.Resources.CrossSelling, ItemTabKey.CrossSelling.ToString(),
                            ViewPages.RetailItemICGroupPage.CreateCSInstance
                            ){ Enabled = ((RetailItem)args.InternalContext).ItemType != ItemTypeEnum.MasterItem && !args.IsMultiEdit },
                        510);

                    args.Add(
                        new TabControl.Tab(Properties.Resources.ItemModifiers, ItemTabKey.ItemModifiers.ToString(),
                            ViewPages.RetailItemICGroupPage.CreateIMInstance
                            ){ Enabled = ((RetailItem)args.InternalContext).ItemType != ItemTypeEnum.MasterItem && !args.IsMultiEdit },
                        520);
                }
            }
            else if (args.ContextName == "LSOne.ViewPlugins.RetailItems.Views.RetailGroupView")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(
                        new TabControl.Tab(Properties.Resources.Infocodes,
                            ViewPages.InfocodeSharedPage.CreateInstance
                            ),
                        120);

                    args.Add(
                        new TabControl.Tab(Properties.Resources.CrossSelling,
                            ViewPages.RetailItemICGroupPage.CreateCSInstance
                            ),
                        130);

                    args.Add(
                        new TabControl.Tab(Properties.Resources.ItemModifiers,
                            ViewPages.RetailItemICGroupPage.CreateIMInstance
                            ),
                        140);
                }
            }
            else if (args.ContextName == "LSOne.ViewPlugins.Customer.Views.CustomerView")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(
                        new TabControl.Tab(Properties.Resources.Infocodes,
                            ViewPages.InfocodeSharedPage.CreateInstance
                            ),
                        120);
                }
            }            
        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
            {
                args.Add(new Category(Properties.Resources.Retail, "Retail", null), 50);
            }
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "Retail")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(new Item(Properties.Resources.Infocodes, "Infocodes", null), 350); //Skrá 350Ðargs.Add(new PageCategory(Properties.Resources.Customers, "Customers"), 200)?
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {            
            if (args.CategoryKey == "Retail" && args.ItemKey == "Infocodes")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(new ItemButton(Properties.Resources.Infocodes, Properties.Resources.InfocodesDescription, new EventHandler(ShowInfocodeSetupSheet)), 10);
                    args.Add(new ItemButton(Properties.Resources.CrossSellInfocodeGroups, Properties.Resources.CrossSellInfocodeGroupsDescription, new EventHandler(ShowCrossSellingGroupInfocodeSetupSheet)), 20);
                    args.Add(new ItemButton(Properties.Resources.ItemModifierGroups, Properties.Resources.ItemModifierGroupsDescription, new EventHandler(ShowItemModifierGroupInfocodeSetupSheet)), 30);
                }
   
            }
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
           args.Add(new Page(Properties.Resources.Setup, "Setup"), 900);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Setup")
            {
                args.Add(new PageCategory(Properties.Resources.Infocodes, "Infocodes"), 400);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Setup" && args.CategoryKey == "Infocodes")
            {
                if (PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.InfocodeView))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.Infocodes,
                        Properties.Resources.Infocodes,
                        Properties.Resources.InfocodesTooltipDescription,
                        CategoryItem.CategoryItemFlags.DropDown,
                        null,
                        Properties.Resources.info_codes_32,
                        null,
                        "ShowInfoCodes"), 10);
                }


            }
        }
        internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            ExtendedMenuItem item;

            switch (args.Key)
            {
                // We dont need to check permissions here since thats been done on the parent menu allready
                case "RibbonShowInfoCodes":
                    item = new ExtendedMenuItem(
                        Properties.Resources.Infocodes,
                        null,
                        10,
                        ShowInfocodeSetupSheet);

                    args.AddMenu(item);

                    item = new ExtendedMenuItem(
                        Properties.Resources.CrossSellGroups,
                        null,
                        20,
                        ShowCrossSellingGroupInfocodeSetupSheet);

                    args.AddMenu(item);

                    item = new ExtendedMenuItem(
                        Properties.Resources.ItemModifierGroups,
                        null,
                        20,
                        ShowItemModifierGroupInfocodeSetupSheet);

                    args.AddMenu(item);

                    break;
            }
        }
        #endregion
    }
}
