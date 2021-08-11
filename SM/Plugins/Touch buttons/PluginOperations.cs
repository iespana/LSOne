using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.TouchButtons.Dialogs;
using LSOne.DataLayer.BusinessObjects.Images;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.TouchButtons.Properties;

namespace LSOne.ViewPlugins.TouchButtons
{
    internal class PluginOperations
    {
        public static void ShowTouchButtonLayoutsSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.TouchButtonListView());
        }

        public static void ShowTouchButtonSheet(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.TouchButtonView(id));
        }

        /// <summary>
        /// Shows the new touch button layout dialog without showing the touch button layout view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void AddTouchButtonLayout(object sender, PluginOperationArguments args)
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout))
            {
                Dialogs.NewTouchButtonDialog dlg = new Dialogs.NewTouchButtonDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.LayoutID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "TouchButtonLayout", selectedID, null);                    
                }
            }            
        }

        public static RecordIdentifier NewTouchButtonLayout()
        {
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout))
            {
                Dialogs.NewTouchButtonDialog dlg = new Dialogs.NewTouchButtonDialog();

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.LayoutID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "TouchButtonLayout", selectedID, null);

                    PluginOperations.ShowTouchButtonSheet(selectedID);
                }
            }

            return selectedID;
        }

        public static bool DeleteTouchButtonLayout(RecordIdentifier id)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteTouchLayoutQuestion, Properties.Resources.Delete) == DialogResult.Yes)
                {
                    Providers.TouchLayoutData.Delete(PluginEntry.DataModel, id);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "TouchButtonLayout", (string)id, null);

                    return true;
                }
            }

            return false;
        }

        public static Image ShowImageBankSelectDialog(ImageTypeEnum defaultImageType)
        {
            IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "ShowImageBankSelectDialog", null);

            if (plugin != null)
            {
                return (Image)plugin.Message(null, "ShowImageBankSelectDialog", defaultImageType);
            }

            return null;
        }

        public static bool DeleteTouchButtonLayouts(List<RecordIdentifier> IDs)
        {
            if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteTouchLayoutsQuestion, Properties.Resources.Delete) == DialogResult.Yes)
                {
                    foreach (RecordIdentifier id in IDs)
                    {
                        Providers.TouchLayoutData.Delete(PluginEntry.DataModel, id);                        
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "TouchButtonLayout", null, IDs);

                    return true;
                }
            }

            return false;
        }

        public static void ShowPosButtonGridMenusListView(object sender, EventArgs args)
        {
            if (PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.MobileInvButtonGridMenus)
            {
                PluginEntry.Framework.ViewController.CloseCurrentView();
            }
            PluginEntry.Framework.ViewController.Add(new Views.ButtonMenusView());
        }

        public static void ShowPosButtonGridMenusListView(RecordIdentifier posMenuHeaderID)
        {
            if (PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.MobileInvButtonGridMenus)
            {
                PluginEntry.Framework.ViewController.CloseCurrentView();
            }

            PluginEntry.Framework.ViewController.Add(new Views.ButtonMenusView(posMenuHeaderID));
        }

        public static void ShowPosButtonGridMenusListView(RecordIdentifier posMenuHeaderID, DeviceTypeEnum deviceType)
        {
            if (PluginEntry.Framework.ViewController.CurrentView.HeaderText == Resources.POSButtonGridMenus)
            {
                PluginEntry.Framework.ViewController.CloseCurrentView();
            }
            PluginEntry.Framework.ViewController.Add(new Views.ButtonMenusView(posMenuHeaderID, deviceType));
        }

        /// <summary>
        /// Shows the PosMenuHeader view for POS button grids
        /// </summary>
        /// <param name="posMenuHeaderID">The ID for the pos menu header to edit</param>        
        public static void ShowPosButtonGridView(RecordIdentifier posMenuHeaderID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ButtonMenuView(posMenuHeaderID));
        }

        /// <summary>
        /// Creates a new Button menu with device type = POS
        /// </summary>
        /// <returns></returns>
        public static RecordIdentifier NewPosButtonGridMenu()
        {
            return NewPosButtonGridMenu(DeviceTypeEnum.POS);
        }
        /// <summary>
        /// Creates a new Button menu with the given device type
        /// </summary>
        /// <returns></returns>
        public static RecordIdentifier NewPosButtonGridMenu(DeviceTypeEnum deviceType)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus))
            {
                Dialogs.NewButtonMenuDialog dlg = new Dialogs.NewButtonMenuDialog(deviceType);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedID = dlg.PosMenuHeaderID;

                    PosMenuHeader posMenuHeader = dlg.MenuHeader;

                    if(!RecordIdentifier.IsEmptyOrNull(dlg.CopyFromPosMenuHeaderID))
                    {
                        List<PosMenuLine> linesToCopy = Providers.PosMenuLineData.GetList(PluginEntry.DataModel, dlg.CopyFromPosMenuHeaderID);

                        foreach(PosMenuLine line in linesToCopy)
                        {
                            line.MenuID = posMenuHeader.ID;
                            line.Sequence = RecordIdentifier.Empty;
                            Providers.PosMenuLineData.Save(PluginEntry.DataModel, line);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < posMenuHeader.Columns * posMenuHeader.Rows; i++)
                        {
                            PosMenuLine newLine = new PosMenuLine();

                            newLine.Sequence = RecordIdentifier.Empty;
                            newLine.MenuID = selectedID;
                            newLine.Text = "";
                            newLine.Operation = RecordIdentifier.Empty;
                            newLine.UseHeaderAttributes = true;
                            newLine.UseHeaderFont = true;
                            newLine.Operation = posMenuHeader.DefaultOperation;

                            if (posMenuHeader.StyleID == RecordIdentifier.Empty)
                            {
                                newLine.FontName = posMenuHeader.FontName;
                                newLine.FontSize = posMenuHeader.FontSize;
                                newLine.FontBold = posMenuHeader.FontBold;
                                newLine.ForeColor = posMenuHeader.ForeColor;
                                newLine.BackColor = posMenuHeader.BackColor;
                                newLine.FontItalic = posMenuHeader.FontItalic;
                                newLine.FontCharset = posMenuHeader.FontCharset;
                                newLine.BackColor2 = posMenuHeader.BackColor2;
                                newLine.GradientMode = posMenuHeader.GradientMode;
                                newLine.Shape = posMenuHeader.Shape;
                            }

                            Providers.PosMenuLineData.Save(PluginEntry.DataModel, newLine);
                        }
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosButtonGridMenuHeader", dlg.PosMenuHeaderID, null);
                }
            }

            return selectedID;
        }

        public static RecordIdentifier NewPosButtonGridMenuWithOperation(int defaultOperation, int columns, int rows)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus))
            {
                Dialogs.NewButtonMenuDialog dlg = new Dialogs.NewButtonMenuDialog(defaultOperation, columns, rows);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedID = dlg.PosMenuHeaderID;

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosButtonGridMenuHeader", dlg.PosMenuHeaderID, null);
                }
            }

            return selectedID;
        }

        public static bool DeletePosButtonGridMenu(List<RecordIdentifier> posMenuHeaderIDs)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus))
            {
                string text = posMenuHeaderIDs.Count == 1 ? Properties.Resources.DeletePosButtonGridMenuQuestion : Properties.Resources.DeletePosButtonGridMenusQuestion;
                string caption = posMenuHeaderIDs.Count == 1 ? Properties.Resources.DeletePosButtonGridMenu : Properties.Resources.DeletePosButtonGridMenus;

                if (QuestionDialog.Show(text, caption) == DialogResult.Yes)
                {
                    foreach (var posMenuHeaderID in posMenuHeaderIDs)
                    {
                        Providers.PosMenuHeaderData.Delete(PluginEntry.DataModel, posMenuHeaderID);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosButtonGridMenuHeader", posMenuHeaderID, null);

                        Providers.PosMenuLineData.DeleteForHeaderID(PluginEntry.DataModel, posMenuHeaderID);
                        PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosButtonGridMenuLine", posMenuHeaderID, null);
                    }
                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        // POS Menu Lines
        public static void ShowPosButtonGridMenuLine(RecordIdentifier posMenuLineID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ButtonView(posMenuLineID));
        }

        public static void ShowPosButtonGridMenuLine(RecordIdentifier posMenuLineID, IEnumerable<IDataEntity> recordBrowsingContext)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ButtonView(posMenuLineID, recordBrowsingContext));      
        }

        public static RecordIdentifier NewPosButtonGridMenuLine(RecordIdentifier posMenuHeaderID, MenuTypeEnum type)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus))
            {
                Dialogs.NewButtonDialog dlg = new Dialogs.NewButtonDialog(posMenuHeaderID);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedID = dlg.PosMenuLineID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosButtonGridMenuLine", dlg.PosMenuLineID, null);

                    if (type != MenuTypeEnum.KitchenDisplay)
                    {
                        if (dlg.CreateMultipleLines)
                        {
                            ShowPosButtonGridMenuLine(selectedID, dlg.CreatedLines);
                        }
                        else
                        {
                            ShowPosButtonGridMenuLine(selectedID);
                        }     
                    }                  
                }
            }

            return selectedID;
        }

        public static bool DeletePosButtonGridMenuLines(RecordIdentifier posMenuLineID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletePosButtonGridMenuLineQuestion, Properties.Resources.DeletePosButtonGridMenuLine) == DialogResult.Yes)
                {
                    Providers.PosMenuLineData.Delete(PluginEntry.DataModel, posMenuLineID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "PosButtonGridMenuLine", posMenuLineID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        public static bool DeletePosButtonGridMenuLine(RecordIdentifier posMenuLineID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.EditPosMenus))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletePosButtonGridMenuLineQuestion, Properties.Resources.DeletePosButtonGridMenuLine) == DialogResult.Yes)
                {
                    Providers.PosMenuLineData.Delete(PluginEntry.DataModel, posMenuLineID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosButtonGridMenuLine", posMenuLineID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        /// <summary>
        /// Shows a dialog to edit which pos menus are linked to button grids 1 - 5 for the give touch layout
        /// </summary>
        /// <param name="touchLayoutID">The id of the touch layout to edit</param>
        public static void EditTouchLayoutButtonGrids(RecordIdentifier touchLayoutID)
        {
            EditTouchLayoutButtonGridsDialog dlg = new EditTouchLayoutButtonGridsDialog(touchLayoutID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "TouchButtonLayout", dlg.LayoutID, null);
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
                if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout) || PluginEntry.DataModel.HasPermission(Permission.ManageStyleSetup))
                {
                    args.Add(new Item(Properties.Resources.LookAndFeel, "Look and feel", null), 750);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "Store setup" && args.ItemKey == "Look and feel")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout))
                {
                    args.Add(new ItemButton(Properties.Resources.TouchButtonLayouts, Properties.Resources.TouchButtonLayoutsDescription, ShowTouchButtonLayoutsSheet), 100);
                    args.Add(new ItemButton(Properties.Resources.POSButtonGridMenus, Properties.Resources.POSButtonGridMenusDescription, ShowPosButtonGridMenusListView), 200);

                }   
            }
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
                    args.Add(new PageCategory(Properties.Resources.LookAndFeel, "Look and feel"), 300);
                }
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Sites" && args.CategoryKey == "Look and feel")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.TouchButtonLayouts,
                        Properties.Resources.TouchButtonLayouts,
                        Properties.Resources.TouchButtonLayoutsTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        Properties.Resources.touch_button_layouts_32,
                        ShowTouchButtonLayoutsSheet,
                        "TouchButtonLayouts"), 10);

                    args.Add(new CategoryItem(
                        Properties.Resources.ButtonMenus,
                        Properties.Resources.ButtonMenus,
                        Properties.Resources.POSButtonGridMenusTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        Properties.Resources.POS_button_grid_menu_16,
                        ShowPosButtonGridMenusListView,
                        "POSButtonGridMenus"), 20);
                }
            }
        }

        public static RecordIdentifier NewStyle(RecordIdentifier styleID)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup))
            {
                Dialogs.NewStyle dlg = new Dialogs.NewStyle(styleID);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    return dlg.Style.ID;
                }
            }
            return RecordIdentifier.Empty;
        }

        public static bool DeleteStyle(List<RecordIdentifier> IDs)
        {
            if (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup))
            {
                //Check all the ID to make sure they are not being used.
                foreach (RecordIdentifier id in IDs)
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

                if (QuestionDialog.Show(Properties.Resources.DeleteStyle, Properties.Resources.Delete) == DialogResult.Yes)
                {
                    foreach (RecordIdentifier id in IDs)
                    {
                        Providers.PosStyleData.Delete(PluginEntry.DataModel, id);                        
                    }

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiDelete, "StyleView", null, IDs);

                    return true;
                }
            }

            return false;
        }

        internal static void ShowImageBank(object sender, ContextBarClickEventArguments args)
        {
            PluginOperationArguments operationArgs = new PluginOperationArguments(RecordIdentifier.Empty, null, true);

            PluginEntry.Framework.RunOperation("ShowImageBank", null, operationArgs);

            PluginEntry.Framework.ViewController.Add(operationArgs.ResultView);
        }

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSRetail.SiteManager.Plugins.Hospitality.Views.PosMenusView.Related")
            {
                arguments.Add(
                    new ContextBarItem(Properties.Resources.POSButtonGridMenus, ShowPosButtonGridMenusListView), 200);
            }
        }

        public static void ImportLayouts()
        {
            PluginEntry.Framework.ViewController.CurrentView.ShowProgress(Import, Properties.Resources.Importing + "...");
        }
        
        private static void Import(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog { CheckFileExists = true, Multiselect = false, Filter = Properties.Resources.TouchButtonLayout + " (*.layout)|*.layout" };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DateTime importTime = DateTime.Now;
                XDocument doc = XDocument.Load(dlg.FileName);
                XElement root = doc.Root;

                XElement layoutsElement = root.Element("Layouts");
                IEnumerable<XElement> layoutElements = layoutsElement.Elements();
                List<TouchLayout> layouts = new List<TouchLayout>();
                foreach (var xLayout in layoutElements)
                {
                    TouchLayout tmp = new TouchLayout();
                    tmp.ToClass(xLayout);
                    layouts.Add(tmp);
                }

                XElement headersElement = root.Element("MenuHeaders");
                IEnumerable<XElement> headerElements = headersElement.Elements();
                List<PosMenuHeader> headers = new List<PosMenuHeader>();
                foreach (var xLayout in headerElements)
                {
                    PosMenuHeader tmp = new PosMenuHeader();
                    tmp.ToClass(xLayout);
                    headers.Add(tmp);
                }

                XElement linesElement = root.Element("MenuLines");
                IEnumerable<XElement> lineElements = linesElement.Elements();
                List<PosMenuLine> lines = new List<PosMenuLine>();
                foreach (var xLayout in lineElements)
                {
                    PosMenuLine tmp = new PosMenuLine();
                    tmp.ToClass(xLayout);
                    lines.Add(tmp);
                }

                XElement stylesElement = root.Element("Styles");
                IEnumerable<XElement> styleElements = stylesElement.Elements();
                List<PosStyle> styles = new List<PosStyle>();
                foreach (var xLayout in styleElements)
                {
                    PosStyle tmp = new PosStyle();
                    tmp.ToClass(xLayout);
                    styles.Add(tmp);
                }

                XElement picturesElement = root.Element("Pictures");
                IEnumerable<XElement> pictureElements = picturesElement != null ? picturesElement.Elements() : Enumerable.Empty<XElement>();
                List<Image> images = new List<Image>();
                foreach (var xLayout in pictureElements)
                {
                    Image tmp = new Image();
                    tmp.ToClass(xLayout);
                    {
                        if (tmp.Picture == null)
                        {
                            foreach (var line in lines)
                            {
                                if (line.PictureID == tmp.ID)
                                {
                                    line.PictureID = RecordIdentifier.Empty;
                                }
                            }
                        }
                        else
                        {
                            images.Add(tmp);
                        }
                    }
                }

                bool layoutConflicts = layouts.Any(l => Providers.TouchLayoutData.GuidExists(PluginEntry.DataModel, l.Guid));
                bool headerConflicts = headers.Any(h => Providers.PosMenuHeaderData.GuidExists(PluginEntry.DataModel, h.Guid));
                bool styleConflicts = styles.Any(s => Providers.PosStyleData.GuidExists(PluginEntry.DataModel, s.Guid));
                bool imageConflicts = images.Any(i => Providers.ImageData.GuidExists(PluginEntry.DataModel, i.Guid));

                bool overrideConflicts = false;

                if (layoutConflicts || headerConflicts || styleConflicts || imageConflicts)
                {
                    List<MessageButtonValue> buttons = new List<MessageButtonValue>();
                    buttons.Add(new MessageButtonValue { Text = Properties.Resources.Overwrite, DialogResult = DialogResult.Yes, Options = MessageButtonValue.ButtonOptions.IsDefault });
                    buttons.Add(new MessageButtonValue { Text = Properties.Resources.Add, DialogResult = DialogResult.No, Options = MessageButtonValue.ButtonOptions.None });
                    buttons.Add(MessageButtonValue.CancelButton);

                    string msg = ResolveDialogMessage(layoutConflicts, headerConflicts, styleConflicts);

                    DialogResult result = MessageDialog.Show(msg, Properties.Resources.ImportConflicts, "", MessageBoxIcon.Question, new MessageDialogSetup(buttons));

                    if (result == DialogResult.Cancel)
                    {
                        PluginEntry.Framework.ViewController.CurrentView.HideProgress();
                        return;
                    }
                    overrideConflicts = result == DialogResult.Yes;
                }

                var changeType = overrideConflicts ? DataEntityChangeType.Edit : DataEntityChangeType.Add;
                SaveStyles(styles, headers, lines, overrideConflicts, importTime);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, changeType, "StyleView", styles.Count > 0 ? styles[0].ID : "", null);
                SaveImages(images, lines, overrideConflicts);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, changeType, "ImageBank", layouts.Count > 0 ? layouts[0].ID : "", null);
                SaveHeaders(headers, lines, layouts, overrideConflicts, importTime);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, changeType, "PosButtonGridMenuHeader", headers.Count > 0 ? headers[0].ID : "", null);
                SaveMenuLines(lines);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, changeType, "PosButtonGridMenuLine", lines.Count > 0 ? lines[0].ID : "", null);
                SaveLayouts(layouts, overrideConflicts, importTime);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, changeType, "TouchButtonLayout", layouts.Count > 0 ? layouts[0].ID : "", null);

                PluginEntry.Framework.ViewController.CurrentView.HideProgress();
            }
            else
            {
                PluginEntry.Framework.ViewController.CurrentView.HideProgress();
            }
        }

        private static string ResolveDialogMessage(bool layoutConflicts, bool headerConflicts, bool styleConflicts)
        {
            int conflicts = 0;
            conflicts += layoutConflicts ? 4 : 0;
            conflicts += headerConflicts ? 2 : 0;
            conflicts += styleConflicts ? 1 : 0;

            string result = "";
            switch (conflicts)
            {
                case 1:
                    result = Properties.Resources.StylesConflict;
                    break;
                case 2:
                    result = Properties.Resources.MenusConflict;
                    break;
                case 3:
                    result = Properties.Resources.MenusAndStylesConflict;
                    break;
                case 4:
                    result = Properties.Resources.LayoutsConflict;
                    break;
                case 5:
                    result = Properties.Resources.LayoutsAndStylesConflict;
                    break;
                case 6:
                    result = Properties.Resources.LayoutsAndMenusConflict;
                    break;
                case 7:
                    result = Properties.Resources.LayoutsMenusAndStylesConflict;
                    break;
            }
            return result;
        }

        private static void SaveLayouts(IEnumerable<TouchLayout> layouts, bool overrideConflicts, DateTime importTime)
        {
            foreach (var touchLayout in layouts)
            {
                var layoutInDatabase = Providers.TouchLayoutData.GetByGuid(PluginEntry.DataModel, touchLayout.Guid);
                if (layoutInDatabase != null)
                {
                    if (overrideConflicts)
                    {
                        touchLayout.ID = layoutInDatabase.ID;
                        touchLayout.ImportDateTime = importTime;
                        touchLayout.Text = layoutInDatabase.Text;
                        Providers.TouchLayoutData.Save(PluginEntry.DataModel, touchLayout);
                    }
                    else
                    {
                        layoutInDatabase.Guid = Guid.Empty;
                        Providers.TouchLayoutData.Save(PluginEntry.DataModel, layoutInDatabase);
                        touchLayout.ID = RecordIdentifier.Empty;
                        touchLayout.ImportDateTime = importTime;
                        bool useDatabaseName = layoutInDatabase.Text.StartsWith(touchLayout.Text);
                        touchLayout.Text = useDatabaseName ? UpdateName(layoutInDatabase.Text) : touchLayout.Text;
                        Providers.TouchLayoutData.Save(PluginEntry.DataModel, touchLayout);
                    }
                }
                else
                {
                    if (Providers.TouchLayoutData.Exists(PluginEntry.DataModel, touchLayout.ID))
                    {
                        touchLayout.ID = RecordIdentifier.Empty;
                        touchLayout.ImportDateTime = importTime;
                        Providers.TouchLayoutData.Save(PluginEntry.DataModel, touchLayout);
                    }
                    else
                    {
                        touchLayout.ImportDateTime = importTime;
                        Providers.TouchLayoutData.Save(PluginEntry.DataModel, touchLayout);
                    }
                }
            }
        }

        private static void SaveMenuLines(IEnumerable<PosMenuLine> lines)
        {
            foreach (var posMenuLine in lines)
            {
                //SaveHeaders guarantees that lines in db have been deleted if
                //the user wants to overwrite and importing lines have the correct 
                //ID if the user wants to add new instances
                Providers.PosMenuLineData.Save(PluginEntry.DataModel, posMenuLine);
            }
        }

        private static void SaveHeaders(IEnumerable<PosMenuHeader> headers, List<PosMenuLine> lines, List<TouchLayout> layouts, bool overrideConflicts, DateTime importTime)
        {
            foreach (var header in headers)
            {
                var headerInDatabase = Providers.PosMenuHeaderData.GetByGuid(PluginEntry.DataModel, header.Guid);
                if (headerInDatabase != null)
                {
                    if (overrideConflicts)
                    {
                        UpdateMenuIdForLines(lines, header.ID, headerInDatabase.ID);
                        UpdateGridIdForLayouts(layouts, header.ID, headerInDatabase.ID);
                        UpdateSubMenuParameter(lines, header.ID, headerInDatabase.ID);
                        header.ID = headerInDatabase.ID;
                        header.ImportDateTime = importTime;
                        header.Text = headerInDatabase.Text;
                        Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, header);
                        Providers.PosMenuLineData.DeleteForHeaderID(PluginEntry.DataModel, header.ID);
                    }
                    else
                    {
                        headerInDatabase.Guid = Guid.Empty;
                        Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, headerInDatabase);
                        var oldHeaderID = header.ID;
                        header.ID = RecordIdentifier.Empty;
                        header.ImportDateTime = importTime;

                        bool useDatabaseName = headerInDatabase.Text.StartsWith(header.Text);
                        header.Text = useDatabaseName ? UpdateName(headerInDatabase.Text) : header.Text;
                        Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, header);
                        UpdateMenuIdForLines(lines, oldHeaderID, header.ID);
                        UpdateGridIdForLayouts(layouts, oldHeaderID, header.ID);
                        UpdateSubMenuParameter(lines, oldHeaderID, header.ID);
                    }
                }
                else
                {
                    if (Providers.PosMenuHeaderData.Exists(PluginEntry.DataModel, header.ID))
                    {
                        var oldHeaderID = header.ID;
                        header.ID = RecordIdentifier.Empty;
                        header.ImportDateTime = importTime;
                        Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, header);
                        UpdateMenuIdForLines(lines, oldHeaderID, header.ID);
                        UpdateGridIdForLayouts(layouts, oldHeaderID, header.ID);
                        UpdateSubMenuParameter(lines, oldHeaderID, header.ID);
                    }
                    else
                    {
                        header.ImportDateTime = importTime;
                        Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, header);
                        if (overrideConflicts)
                        {
                            Providers.PosMenuLineData.DeleteForHeaderID(PluginEntry.DataModel, header.ID);
                        }
                    }
                }
            }
        }

        private static void SaveImages(List<Image> images, List<PosMenuLine> lines, bool overrideConflicts)
        {
            foreach(Image img in images)
            {
                Image dbImage = Providers.ImageData.GetByGuid(PluginEntry.DataModel, img.Guid);

                if(dbImage != null)
                {
                    if(overrideConflicts)
                    {
                        UpdatePictureIdForMenuLines(lines, img.ID, dbImage.ID);
                        img.ID = dbImage.ID;
                        img.Text = dbImage.Text;
                        Providers.ImageData.Save(PluginEntry.DataModel, img);
                    }
                    else
                    {
                        img.Guid = Guid.Empty;
                        RecordIdentifier oldImageID = img.ID;
                        img.ID = RecordIdentifier.Empty;
                        Providers.ImageData.Save(PluginEntry.DataModel, img);
                        UpdatePictureIdForMenuLines(lines, oldImageID, img.ID);
                    }
                }
                else
                {
                    if(Providers.ImageData.Exists(PluginEntry.DataModel, img.ID))
                    {
                        RecordIdentifier oldImageID = img.ID;
                        img.ID = RecordIdentifier.Empty;
                        Providers.ImageData.Save(PluginEntry.DataModel, img);
                        UpdatePictureIdForMenuLines(lines, oldImageID, img.ID);
                    }
                    else
                    {
                        Providers.ImageData.Save(PluginEntry.DataModel, img);
                    }
                }
            }
        }

        private static void UpdatePictureIdForMenuLines(List<PosMenuLine> lines, RecordIdentifier oldImageID, RecordIdentifier newImageID)
        {
            foreach (PosMenuLine line in lines.Where(l => l.PictureID == oldImageID))
            {
                line.PictureID = newImageID;
            }
        }

        private static void UpdateSubMenuParameter(List<PosMenuLine> lines, RecordIdentifier oldHeaderID, RecordIdentifier newHeaderID)
        {
            foreach (var line in lines.Where(l => l.Operation == 1500 && l.Parameter.Substring(0, l.Parameter.IndexOf(';')) == oldHeaderID))
            {
                line.Parameter = newHeaderID + line.Parameter.Substring(line.Parameter.IndexOf(';'));
            }
            foreach (var line in lines.Where(l => (l.Operation == 400 || l.Operation == 401) && l.Parameter == oldHeaderID))
            {
                line.Parameter = (string)newHeaderID;
            }
        }

        private static void UpdateGridIdForLayouts(List<TouchLayout> layouts, RecordIdentifier oldHeaderID, RecordIdentifier newHeaderID)
        {
            foreach (var touchLayout in layouts.Where(l => LayoutUsesHeader(l, oldHeaderID)))
            {
                if (touchLayout.ButtonGrid1 == oldHeaderID) touchLayout.ButtonGrid1 = newHeaderID;
                if (touchLayout.ButtonGrid2 == oldHeaderID) touchLayout.ButtonGrid2 = newHeaderID;
                if (touchLayout.ButtonGrid3 == oldHeaderID) touchLayout.ButtonGrid3 = newHeaderID;
                if (touchLayout.ButtonGrid4 == oldHeaderID) touchLayout.ButtonGrid4 = newHeaderID;
                if (touchLayout.ButtonGrid5 == oldHeaderID) touchLayout.ButtonGrid5 = newHeaderID;
            }
        }

        private static void UpdateMenuIdForLines(List<PosMenuLine> lines, RecordIdentifier oldHeaderID, RecordIdentifier newHeaderID)
        {
            foreach (var line in lines.Where(l => l.MenuID == oldHeaderID))
            {
                line.MenuID = newHeaderID;
            }
        }

        private static bool LayoutUsesHeader(TouchLayout l, RecordIdentifier headerID)
        {
            bool result = l.ButtonGrid1 == headerID;
            result |= l.ButtonGrid2 == headerID;
            result |= l.ButtonGrid3 == headerID;
            result |= l.ButtonGrid4 == headerID;
            result |= l.ButtonGrid5 == headerID;
            return result;
        }

        private static void SaveStyles(IEnumerable<PosStyle> styles, List<PosMenuHeader> headers, List<PosMenuLine> lines, bool overrideConflicts, DateTime importTime)
        {
            foreach (var style in styles)
            {
                var styleInDatabase = Providers.PosStyleData.GetByGuid(PluginEntry.DataModel, style.Guid);
                if (styleInDatabase != null)
                {
                    if (overrideConflicts)
                    {
                        UpdateStyleIdOnHeaders(headers, style.ID, styleInDatabase.ID);
                        UpdateStyleIdOnLines(lines, style.ID, styleInDatabase.ID);

                        style.ID = styleInDatabase.ID;
                        style.ImportDateTime = importTime;
                        style.Text = styleInDatabase.Text;
                        Providers.PosStyleData.Save(PluginEntry.DataModel, style);
                    }
                    else
                    {
                        styleInDatabase.Guid = Guid.Empty;
                        Providers.PosStyleData.Save(PluginEntry.DataModel, styleInDatabase);
                        var oldStyleID = style.ID;
                        style.ID = RecordIdentifier.Empty;
                        style.ImportDateTime = importTime;
                        bool useDatabaseName = styleInDatabase.Text.StartsWith(style.Text);
                        style.Text = useDatabaseName ? UpdateName(styleInDatabase.Text) : style.Text;
                        Providers.PosStyleData.Save(PluginEntry.DataModel, style);
                        UpdateStyleIdOnHeaders(headers, oldStyleID, style.ID);
                        UpdateStyleIdOnLines(lines, oldStyleID, style.ID);
                    }
                }
                else
                {
                    if (Providers.PosStyleData.Exists(PluginEntry.DataModel, style.ID))
                    {
                        var oldStyleID = style.ID;
                        style.ID = RecordIdentifier.Empty;
                        style.ImportDateTime = importTime;
                        Providers.PosStyleData.Save(PluginEntry.DataModel, style);
                        UpdateStyleIdOnHeaders(headers, oldStyleID, style.ID);
                        UpdateStyleIdOnLines(lines, oldStyleID, style.ID);
                    }
                    else
                    {
                        style.ImportDateTime = importTime;
                        Providers.PosStyleData.Save(PluginEntry.DataModel, style);
                    }
                }
            }
        }

        private static string UpdateName(string name)
        {
            string result;
            string copy = Properties.Resources.Copy;
            var comparison = StringComparison.Ordinal;
            if (name.IndexOf(copy, comparison) == -1)
            {
                result = name + " - " + copy;
            }
            else if (name.IndexOf(" (", name.IndexOf(copy, comparison), comparison) == -1)
            {
                result = name + " (2)";
            }
            else
            {
                int oldCopyNumber = Convert.ToInt32(name.Substring(name.IndexOf(" (", name.IndexOf(copy, comparison), comparison) + 2, 1));
                int copyNumber = oldCopyNumber + 1;
                result = name.Replace("(" + oldCopyNumber + ")", "(" + copyNumber + ")");
            }
            return result;
        }

        private static void UpdateStyleIdOnLines(List<PosMenuLine> lines, RecordIdentifier oldStyleID, RecordIdentifier newStyleID)
        {
            foreach (var posMenuLine in lines.Where(l => l.StyleID == oldStyleID))
            {
                posMenuLine.StyleID = newStyleID;
            }
        }

        private static void UpdateStyleIdOnHeaders(List<PosMenuHeader> headers, RecordIdentifier oldStyleID, RecordIdentifier newStyleID)
        {
            foreach (var posMenuHeader in headers.Where(h => h.StyleID == oldStyleID))
            {
                posMenuHeader.StyleID = newStyleID;
            }
        }

        public static void ExportLayouts(List<RecordIdentifier> layoutIds)
        {
            var dlg = new SaveFileDialog
                {
                    Filter = Properties.Resources.TouchButtonLayout + " (*.layout)|*.layout",
                    DefaultExt = ".layout"
                };

            var dlgRes = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

            if (dlgRes != DialogResult.Cancel)
            {
                var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                XElement rootNode = new XElement("Root");
                doc.Add(rootNode);
                var layoutElement = new XElement("Layouts");
                doc.Element("Root").Add(layoutElement);
                var headerElement = new XElement("MenuHeaders");
                doc.Element("Root").Add(headerElement);
                var linesElement = new XElement("MenuLines");
                doc.Element("Root").Add(linesElement);
                var styleElement = new XElement("Styles");
                doc.Element("Root").Add(styleElement);
                var pictureElement = new XElement("Pictures");
                doc.Element("Root").Add(pictureElement);

                List<PosMenuHeader> headers = new List<PosMenuHeader>();
                foreach (var layoutId in layoutIds)
                {
                    TouchLayout layout = Providers.TouchLayoutData.Get(PluginEntry.DataModel, layoutId);
                    var layoutXml = layout.ToXML();
                    doc.Element("Root").Element("Layouts").Add(layoutXml);

                    AddHeaderToList(headers, layout.ButtonGrid1);
                    AddHeaderToList(headers, layout.ButtonGrid2);
                    AddHeaderToList(headers, layout.ButtonGrid3);
                    AddHeaderToList(headers, layout.ButtonGrid4);
                    AddHeaderToList(headers, layout.ButtonGrid5);
                }


                List<PosMenuLine> menuLines = new List<PosMenuLine>();
                List<PosMenuLine> linesWithSubMenu = new List<PosMenuLine>();
                List<PosStyle> styles = new List<PosStyle>();
                List<Image> images = new List<Image>();

                foreach (var posMenuHeader in headers)
                {
                    var headerXml = posMenuHeader.ToXML();
                    doc.Element("Root").Element("MenuHeaders").Add(headerXml);

                    var headerLines = Providers.PosMenuLineData.GetList(PluginEntry.DataModel, posMenuHeader.ID);
                    menuLines.AddRange(headerLines);

                    foreach (var posMenuLine in headerLines.Where(l => l.Operation == 1500 || l.Operation == 400 || l.Operation == 401))
                    {
                        linesWithSubMenu.Add(posMenuLine);
                    }

                    if (posMenuHeader.StyleID != "")
                    {
                        var style = Providers.PosStyleData.Get(PluginEntry.DataModel, posMenuHeader.StyleID);
                        if (styles.All(s => s.ID != style.ID))
                        {
                            styles.Add(style);
                        }
                    }
                }

                //Get all sub menus from operation "Open menu", "Sub menu" and "Popup menu"
                //and lines and style on those menus
                List<PosMenuHeader> subMenus = new List<PosMenuHeader>();
                while(linesWithSubMenu.Count > 0)
                {
                    var current = linesWithSubMenu[0];
                    var subMenuID = current.Operation == 1500
                                        ? current.Parameter.Substring(0, current.Parameter.IndexOf(';'))
                                        : current.Parameter;
                    var menuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, subMenuID);
                    if (menuHeader != null && subMenus.All(h => h.ID != menuHeader.ID) && headers.All(h => h.ID != menuHeader.ID))
                    {
                        subMenus.Add(menuHeader);
                        var headerLines = Providers.PosMenuLineData.GetList(PluginEntry.DataModel, menuHeader.ID);
                        menuLines.AddRange(headerLines);

                        foreach (var posMenuLine in headerLines.Where(l => l.Operation == 1500 || l.Operation == 400 || l.Operation == 401))
                        {
                            linesWithSubMenu.Add(posMenuLine);
                        }

                        if (menuHeader.StyleID != "")
                        {
                            var style = Providers.PosStyleData.Get(PluginEntry.DataModel, menuHeader.StyleID);
                            if (styles.All(s => s.ID != style.ID))
                            {
                                styles.Add(style);
                            }
                        }
                    }
                    linesWithSubMenu.RemoveAt(0);
                }

                foreach (var posMenuHeader in subMenus)
                {
                    var headerXml = posMenuHeader.ToXML();
                    doc.Element("Root").Element("MenuHeaders").Add(headerXml);
                }

                foreach (var menuLine in menuLines)
                {
                    var lineXml = menuLine.ToXML();
                    doc.Element("Root").Element("MenuLines").Add(lineXml);
                    
                    if (menuLine.StyleID != "")
                    {
                        var style = Providers.PosStyleData.Get(PluginEntry.DataModel, menuLine.StyleID);
                        if (styles.All(s => s.ID != style.ID))
                        {
                            styles.Add(style);
                        }
                    }

                    if(menuLine.PictureID != "")
                    {
                        var picture = Providers.ImageData.Get(PluginEntry.DataModel, menuLine.PictureID);
                        if(images.All(i => i.ID != picture.ID))
                        {
                            images.Add(picture);
                        }
                    }
                }

                foreach (var posStyle in styles)
                {
                    var styleXml = posStyle.ToXML();
                    doc.Element("Root").Element("Styles").Add(styleXml);
                }

                foreach(var image in images)
                {
                    var imageXml = image.ToXML();
                    doc.Element("Root").Element("Pictures").Add(imageXml);
                }

                doc.Save(dlg.FileName);
            }
        }

        private static void AddHeaderToList(List<PosMenuHeader> headers, RecordIdentifier headerID)
        {
            if (headerID == "") return;
            if (headers.All(h => h.ID != headerID))
            {
                var header = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, headerID);
                if (header != null)
                {
                    headers.Add(header);
                }
            }
        }
    }
}
