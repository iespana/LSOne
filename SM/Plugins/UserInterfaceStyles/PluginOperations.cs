using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserInterface;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.DataProviders.UserInterface;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.UserInterfaceStyles
{
    internal class PluginOperations
    {
        internal static void EditStyle(object sender, PluginOperationArguments args)
        {
            args.Result = EditStyle(args.ID);
        }

        internal static void NewStyle(object sender, PluginOperationArguments args)
        {
            args.Result = NewStyle((Guid)args.ID);
        }

        public static void ShowStylesView(object sender, EventArgs args)
        {
            Views.StylesView view;

            if (args is PluginOperationArguments)
            {
                view = RecordIdentifier.IsEmptyOrNull((args as PluginOperationArguments).ID) ? new Views.StylesView() : new Views.StylesView((args as PluginOperationArguments).ID);

                if (((PluginOperationArguments)args).WantsViewReturned)
                {
                    ((PluginOperationArguments)args).ResultView = view;
                    return;
                }
            }
            else
            {
                view = new Views.StylesView();
            }

            PluginEntry.Framework.ViewController.Add(view);
        }

        public static DataEntity NewStyle(Guid contextGuid = default(Guid))
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup))
            {
                Dialogs.NewStyleDialog dlg;
                if (contextGuid != default(Guid))
                {
                    dlg = new Dialogs.NewStyleDialog(contextGuid);
                }
                else
                {
                    dlg = new Dialogs.NewStyleDialog();                    
                }

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    if (dlg.PosStyle != null)
                    {
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "Style", dlg.PosStyle.ID, null);
                        return (DataEntity)dlg.PosStyle;
                    }
                }
            }
            return null;
        }

        public static RecordIdentifier EditStyle(RecordIdentifier styleID)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup))
            {
                IPlugin touchButtons = PluginEntry.Framework.FindImplementor(null, "CanEditStyles", null);
                if (touchButtons != null)
                {
                    RecordIdentifier editedStyleID = (RecordIdentifier)touchButtons.Message(null, "EditStyleSetup", styleID);

                    if (editedStyleID != RecordIdentifier.Empty)
                    {
                        if (editedStyleID.DataType == RecordIdentifier.RecordIdentifierType.Guid)
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "UIStyle", editedStyleID, null);
                        }
                        else
                        {
                            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "Style", editedStyleID, null);
                        }
                    }
                }
            }
            return RecordIdentifier.Empty;
        }

        public static bool DeleteStyle(List<RecordIdentifier> IDs)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup))
            {
                //Check all the ID to make sure they are not being used.
                // We only are about tat for te old legay pos styles
                foreach (RecordIdentifier id in IDs)
                {
                    // If te data type of the ID is Guid then we dint care to see if its in use since those will only be marked as deleted
                    if (id.DataType != RecordIdentifier.RecordIdentifierType.Guid)
                    {

                        List<PosMenuLine> btnUseStyle = Providers.PosMenuLineData.AreUsingStyle(PluginEntry.DataModel, id);
                        if (btnUseStyle != null && btnUseStyle.Count > 0)
                        {
                            PosMenuLine menuLine = btnUseStyle.FirstOrDefault();
                            if (menuLine != null)
                            {
                                PosMenuHeader header = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, menuLine.MenuID);
                                PosStyle posStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, id);

                                //sometimes the buttons don't have a description so then we need to tell the user in what header the button belongs to
                                if (menuLine.Text == "")
                                {
                                    MessageDialog.Show(Properties.Resources.StyleInUseOnButtonWithNoDescription.Replace("#1", posStyle.Text).Replace("#2", header.Text));
                                    return false;
                                }

                                MessageDialog.Show(Properties.Resources.StyleInUseOnButton.Replace("#1", posStyle.Text).Replace("#2", menuLine.Text).Replace("#3", header.Text));
                                return false;
                            }
                        }
                        else
                        {
                            List<PosMenuHeader> headerUseStyle = Providers.PosMenuHeaderData.AreUsingStyle(PluginEntry.DataModel, id);
                            if (headerUseStyle != null && headerUseStyle.Count > 0)
                            {
                                PosMenuHeader header = headerUseStyle.FirstOrDefault();
                                if (header != null)
                                {
                                    PosStyle posStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, id);
                                    MessageDialog.Show(Properties.Resources.StyleInUseOnHeader.Replace("#1", posStyle.Text).Replace("#2", header.Text.Trim() != "" ? header.Text : (string)header.ID));
                                    return false;
                                }
                            }
                        }
                    }
                }

                if (QuestionDialog.Show(Properties.Resources.DeleteStyle, Properties.Resources.Delete) == DialogResult.Yes)
                {
                    foreach (RecordIdentifier id in IDs)
                    {
                        if (id.DataType != RecordIdentifier.RecordIdentifierType.Guid)
                        {
                            Providers.PosStyleData.Delete(PluginEntry.DataModel, id);
                        }
                        else
                        {
                            Providers.UIStyleData.Delete(PluginEntry.DataModel, id);
                        }
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "UIStyle", null, IDs);

                    return true;
                }
            }

            return false;
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Sites, "Sites"), 700);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Sites")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout) ||
                    PluginEntry.DataModel.HasPermission(Permission.ManageStyleSetup))
                {
                    args.Add(new PageCategory(Properties.Resources.LookAndFeel, "Look and feel"), 400);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sites" && args.CategoryKey == "Look and feel")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.Styles,
                        Properties.Resources.Styles,
                        Properties.Resources.StylesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        Properties.Resources.styles_16,
                        ShowStylesView,
                        "Styles"), 30);
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
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup))
                {
                    args.Add(new Item(Properties.Resources.LookAndFeel, "Look and feel", null), 750);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Store setup" && args.ItemKey == "Look and feel")
            {
                if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageUIStyleSetup))
                {
                    args.Add(new ItemButton(Properties.Resources.Styles, Properties.Resources.StylesDescription, ShowStylesView), 300);
                }
            }
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey =="LSRetail.SiteManager.Plugins.Hospitality.Views.KitchenDisplayProfileView.Related")
            {
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Styles, ShowStylesView), 251);
                }
            }

            if (arguments.CategoryKey == "LSOne.ViewPlugins.Profiles.Views.VisualProfileView.Related")
            {
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Styles, ShowStylesView), 100);
                }
            }
        }
    }
}
