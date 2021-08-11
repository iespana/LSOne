using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.Hospitality.Dialogs;
using Microsoft.Win32;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality
{
    internal class PluginOperations
    {        
        private static WeakReference StoreViewer
        {
            get
            {
                IPlugin plugin = PluginEntry.Framework.FindImplementor(null, "ViewStores", null);
                return plugin != null ? new WeakReference(plugin) : null;
            }
        }

        public static void ShowHospitalitySetupSheet(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.HospitalitySetupView());
        }

        public static void ShowSalesTypesListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SalesTypesView());
        }

        public static void ShowHospitalityTypesLisetView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.HospitalityTypesView());
        }

        public static void ShowRestaurantSetupSheet(object sender, EventArgs args)
        {
            if (StoreViewer.IsAlive)
            {
                ((IPlugin)StoreViewer.Target).Message(null, "ViewStores", null);
            }
        }

        #region PosLookup
        public static void ShowPosLookupListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.PosLookupsView());
        }

        public static void ShowPosLookup(RecordIdentifier posLookupID)
        {
            Dialogs.PosLookupDialog dlg = new Dialogs.PosLookupDialog(posLookupID);
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static void NewPosLookup(object sender, EventArgs args)
        {
            NewPosLookup();
        }

        public static RecordIdentifier NewPosLookup()
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups))
            {
                Dialogs.PosLookupDialog dlg = new Dialogs.PosLookupDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedID = dlg.PosLookupID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosLookup", dlg.PosLookupID, null);
                }
            }

            return selectedID;
        }

        public static bool DeletePosLookup(RecordIdentifier posLookupID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletePosLookupQuestion, Properties.Resources.DeletePosLookup) == DialogResult.Yes)
                {
                    Providers.PosLookupData.Delete(PluginEntry.DataModel, posLookupID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosLookup", posLookupID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }
        #endregion

        #region PosMenus
        // POS Menu Headers

        public static void ShowPosMenusListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ButtonMenusView());
        }

        public static void ShowPosMenusListView(RecordIdentifier posMenuHeaderID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ButtonMenusView(posMenuHeaderID));
        }

        public static void ShowPosMenu(RecordIdentifier posMenuHeaderID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ButtonMenuView(posMenuHeaderID));
        }        

        public static void NewPosMenu(object sender, EventArgs args)
        {
            RecordIdentifier newId = NewPosMenu();

            if (newId == RecordIdentifier.Empty)
                return;

            ShowPosMenu(newId);
        }

        public static RecordIdentifier NewPosMenu()
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus))
            {
                Dialogs.NewButtonMenuDialog dlg = new Dialogs.NewButtonMenuDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedID = dlg.PosMenuHeaderID;

                    PosMenuHeader posMenuHeader = dlg.MenuHeader;

                    if (!RecordIdentifier.IsEmptyOrNull(dlg.CopyFromPosMenuHeaderID))
                    {
                        List<PosMenuLine> linesToCopy = Providers.PosMenuLineData.GetList(PluginEntry.DataModel, dlg.CopyFromPosMenuHeaderID);

                        foreach (PosMenuLine line in linesToCopy)
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

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosMenuHeader", dlg.PosMenuHeaderID, null);
                }
            }

            return selectedID;
        }

        public static bool DeletePosMenu(RecordIdentifier posMenuHeaderID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletePosMenuQuestion, Properties.Resources.DeletePosMenu) == DialogResult.Yes)
                {
                    Providers.PosMenuHeaderData.Delete(PluginEntry.DataModel, posMenuHeaderID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosMenuHeader", posMenuHeaderID, null);

                    Providers.PosMenuLineData.DeleteForHeaderID(PluginEntry.DataModel, posMenuHeaderID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosMenuLine", posMenuHeaderID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        public static RecordIdentifier ShowEditStyleDialog(RecordIdentifier styleID)
        {
            IPlugin touchButtons = PluginEntry.Framework.FindImplementor(null, "CanEditStyles", null);
            if (touchButtons != null)
            {
                return (RecordIdentifier)touchButtons.Message(null, "EditStyleSetup", styleID);
            }
            return RecordIdentifier.Empty;
        }

        // POS Menu Lines
        public static void ShowPosMenuLine(RecordIdentifier posMenuLineID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.ButtonView(posMenuLineID));
        }

        public static RecordIdentifier NewPosMenuLine(RecordIdentifier posMenuHeaderID)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus))
            {
                Dialogs.NewButtonDialog dlg = new Dialogs.NewButtonDialog(posMenuHeaderID);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedID = dlg.PosMenuLineID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosMenuLine", dlg.PosMenuLineID, null);

                    ShowPosMenuLine(selectedID);
                }
            }

            return selectedID;
        }

       
        public static bool DeletePosMenuLine(RecordIdentifier posMenuLineID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus))
            {
                if (QuestionDialog.Show(Properties.Resources.DeletePosMenuLineQuestion, Properties.Resources.DeletePosMenuLine) == DialogResult.Yes)
                {
                    Providers.PosMenuLineData.Delete(PluginEntry.DataModel, posMenuLineID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "PosMenuLine", posMenuLineID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        public static PosMenuLine CopyLine(PosMenuLine lineToCopy)
        {
            PosMenuLine newLine = new PosMenuLine();

            newLine.MenuID = lineToCopy.MenuID;
            newLine.Sequence = lineToCopy.Sequence;
            newLine.KeyNo = lineToCopy.KeyNo;
            newLine.Text = lineToCopy.Text;
            newLine.Operation = lineToCopy.Operation;
            newLine.Parameter = lineToCopy.Parameter;
            newLine.ParameterType = lineToCopy.ParameterType;
            newLine.FontName = lineToCopy.FontName;
            newLine.FontSize = lineToCopy.FontSize;
            newLine.FontBold = lineToCopy.FontBold;
            newLine.ForeColor = lineToCopy.ForeColor; 
            newLine.BackColor = lineToCopy.BackColor; 
            newLine.FontItalic = lineToCopy.FontItalic; 
            newLine.FontCharset = lineToCopy.FontCharset; 
            newLine.Disabled = lineToCopy.Disabled;
            //newLine.Picture = lineToCopy.Picture; 
            //newLine.PictureFile = lineToCopy.PictureFile; 
            newLine.PictureID = lineToCopy.PictureID;
            newLine.HideDescrOnPicture = lineToCopy.HideDescrOnPicture; 
            newLine.FontStrikethrough = lineToCopy.FontStrikethrough; 
            newLine.FontUnderline = lineToCopy.FontUnderline; 
            newLine.ColumnSpan = lineToCopy.ColumnSpan; 
            newLine.RowSpan = lineToCopy.RowSpan; 
            newLine.NavOperation = lineToCopy.NavOperation; 
            newLine.Hidden = lineToCopy.Hidden; 
            newLine.ShadeWhenDisabled = lineToCopy.ShadeWhenDisabled; 
            newLine.BackgroundHidden = lineToCopy.BackgroundHidden;
            newLine.Transparent = lineToCopy.Transparent;
            newLine.Glyph = lineToCopy.Glyph;
            newLine.Glyph2 = lineToCopy.Glyph2; 
            newLine.Glyph3 = lineToCopy.Glyph3;
            newLine.Glyph4 = lineToCopy.Glyph4; 
            newLine.GlyphText = lineToCopy.GlyphText;
            newLine.GlyphText2 = lineToCopy.GlyphText2; 
            newLine.GlyphText3 = lineToCopy.GlyphText3; 
            newLine.GlyphText4 = lineToCopy.GlyphText4; 
            newLine.GlyphTextFont = lineToCopy.GlyphTextFont;
            newLine.GlyphText2Font = lineToCopy.GlyphText2Font; 
            newLine.GlyphText3Font = lineToCopy.GlyphText3Font; 
            newLine.GlyphText4Font = lineToCopy.GlyphText4Font; 
            newLine.GlyphTextFontSize = lineToCopy.GlyphTextFontSize; 
            newLine.GlyphText2FontSize = lineToCopy.GlyphText2FontSize; 
            newLine.GlyphText3FontSize = lineToCopy.GlyphText3FontSize; 
            newLine.GlyphText4FontSize = lineToCopy.GlyphText4FontSize; 
            newLine.GlyphTextForeColor = lineToCopy.GlyphTextForeColor; 
            newLine.GlyphText2ForeColor = lineToCopy.GlyphText2ForeColor; 
            newLine.GlyphText3ForeColor = lineToCopy.GlyphText3ForeColor; 
            newLine.GlyphText4ForeColor = lineToCopy.GlyphText4ForeColor; 
            newLine.GlyphOffSet  = lineToCopy.GlyphOffSet; 
            newLine.Glyph2OffSet = lineToCopy.Glyph2OffSet; 
            newLine.Glyph3OffSet = lineToCopy.Glyph3OffSet; 
            newLine.Glyph4OffSet = lineToCopy.Glyph4OffSet; 
            newLine.BackColor2  = lineToCopy.BackColor2; 
            newLine.GradientMode  = lineToCopy.GradientMode;
            newLine.Shape = lineToCopy.Shape;
            newLine.UseHeaderAttributes = lineToCopy.UseHeaderAttributes;
            newLine.UseHeaderFont = lineToCopy.UseHeaderFont;

            return newLine;
        }

        #endregion

        #region StationPrinting

        public static void ShowPrintingStationsListView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RestaurantStationsView(PrintingStation.StationTypeEnum.WindowsPrinter)); 
        }

        public static void ShowPrintingStation(RecordIdentifier printingStationID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.RestaurantStationView(printingStationID));            
        }

        internal static void CreatePrintingStation()
        {
            DialogResult result;
            RecordIdentifier selectedId = RecordIdentifier.Empty;
            
            if (PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting))
            {
                var dlg = new Dialogs.PrintingStationDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);      

                if (result == DialogResult.OK)
                {
                    selectedId = dlg.PrintingStationId;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "RestaurantStation", selectedId, null);

                    ShowPrintingStation(selectedId);
                }

                PluginEntry.Framework.ResumeSearchBarClosing();
            }

        }

        public static bool DeleteRestaurantStation(RecordIdentifier restaurantStationID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting))
            {
                List<StationSelection> stationList = Providers.StationSelectionData.GetListForStation(PluginEntry.DataModel,
                                                                                            restaurantStationID);
                if (stationList.Any())
                {
                    MessageDialog.Show(Properties.Resources.StationIsInUseCannotBeDeleted.Replace("#1", stationList.FirstOrDefault().StationDescription));
                    return false;
                }

                if (QuestionDialog.Show(Properties.Resources.DeleteRestaurantStationQuestion, Properties.Resources.DeleteRestaurantStation) == DialogResult.Yes)
                {
                    Providers.PrintingStationData.Delete(PluginEntry.DataModel, restaurantStationID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RestaurantStation", restaurantStationID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        
        public static void ShowStationPrintingView(object sender, EventArgs args)
        {
            PluginEntry.Framework.ViewController.Add(new Views.StationPrintingView());
        }

        public static RecordIdentifier NewStationSelection(object sender, EventArgs args)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting))
            {
                Dialogs.StationSelectionDialog dlg = new Dialogs.StationSelectionDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedID = dlg.StationSelectionID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "StationSelection", dlg.StationSelectionID, null);
                }
            }

            return selectedID;
        }

        public static void ShowStationSelection(RecordIdentifier stationSelectionID)
        {
            Dialogs.StationSelectionDialog dlg = new Dialogs.StationSelectionDialog(stationSelectionID);
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static bool DeleteStationSelection(RecordIdentifier stationSelectionID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteStationSelectionQuestion, Properties.Resources.DeleteStationSelection) == DialogResult.Yes)
                {
                    Providers.StationSelectionData.Delete(PluginEntry.DataModel, stationSelectionID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "StationSelection", stationSelectionID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }
       

        #endregion

        #region RestaurantMenuTypes
        /// <summary>
        /// Opens the restaurant menu type dialog for the type with the given id
        /// </summary>
        /// <param name="restaurantMenuTypeID">The id of the restaurant menu type to show</param>
        public static void ShowRestaurantMenuType(RecordIdentifier restaurantMenuTypeID)
        {
            Dialogs.RestaurantMenuTypeDialog dlg = new Dialogs.RestaurantMenuTypeDialog(restaurantMenuTypeID[0], restaurantMenuTypeID[1]);
            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        /// <summary>
        /// Opens the new restaurant menu type dialog
        /// </summary>
        /// <returns>The RecordIdentifier for the new restaurant menu type, or a RecordIdentifier.Empty if no type was created</returns>
        public static RecordIdentifier NewRestaurantMenuType(RecordIdentifier restaurantID)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageRestaurantMenuTypes))
            {
                Dialogs.RestaurantMenuTypeDialog dlg = new Dialogs.RestaurantMenuTypeDialog(restaurantID);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedID = dlg.RestaurantMenuTypeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "RestaurantMenuType", dlg.RestaurantMenuTypeID, null);
                }
            }

            return selectedID;
        }

        /// <summary>
        /// Deletes the restaurant menu type with the given id
        /// </summary>
        /// <param name="restaurantMenuTypeID">The id of the restaurant menu type</param>
        /// <returns>True if the record was deleted, false otherwise</returns>
        public static bool DeleteRestaurantMenuType(RecordIdentifier restaurantMenuTypeID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageRestaurantMenuTypes))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteRestaurantMenuTypeQuestion, Properties.Resources.DeleteRestaurantMenuType) == DialogResult.Yes)
                {
                    Providers.RestaurantMenuTypeData.Delete(PluginEntry.DataModel, restaurantMenuTypeID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RestaurantMenuType", restaurantMenuTypeID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        #endregion

        #region DiningTableLayouts

        public static DiningTableLayout NewDiningTableLayout(RecordIdentifier restaurantID, RecordIdentifier sequence, RecordIdentifier salesType)
        {
            DialogResult result;
            DataEntity selectedEntity = null;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts))
            {                
                Dialogs.NewDiningTableLayoutDialog dlg = new Dialogs.NewDiningTableLayoutDialog(restaurantID, sequence, salesType);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    selectedEntity = dlg.DiningTableLayoutEntity;
                    RecordIdentifier selectedID = dlg.DiningTableLayoutID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "DiningTableLayout", dlg.DiningTableLayoutID, null);

                    PluginOperations.ShowDiningTableLayout(selectedID);
                }
            }

            DiningTableLayout diningTableLayout = null;

            if (selectedEntity != null)
            {
                diningTableLayout = new DiningTableLayout
                {
                    ID = selectedEntity.ID,
                    Text = selectedEntity.Text
                };
            }
            

            return diningTableLayout;
        }
        /// <summary>
        /// Shows the DiningTableLayoutVew for the given dining table layout
        /// </summary>
        /// <param name="diningTableLayoutID">A RecordIdentifier containing (RestaurantID, Sequence, SalesType, layoutID)</param>
        public static void ShowDiningTableLayout(RecordIdentifier diningTableLayoutID)
        {
            PluginEntry.Framework.ViewController.Add(new Views.DiningTableLayoutView(diningTableLayoutID));
        }

        /// <summary>
        /// Deletes the dining table layout with the given ID. Also deletes all dining table layout screens and restaurant dining tables for that same ID
        /// </summary>
        /// <param name="diningTableLayoutID">The ID of the dining table layout</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool DeleteDiningTableLayout(RecordIdentifier diningTableLayoutID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteDiningTableLayoutQuestion, Properties.Resources.DeleteDiningTableLayout) == DialogResult.Yes)
                {
                    // Delete the dining table layout
                    Providers.DiningTableLayoutData.Delete(PluginEntry.DataModel, diningTableLayoutID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "DiningTableLayout", diningTableLayoutID, null);

                    // Delete the dining table layout screens
                    Providers.DiningTableLayoutScreenData.DeleteForDiningTableLayout(PluginEntry.DataModel, diningTableLayoutID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "DiningTableLayoutScreen", diningTableLayoutID, null);

                    // Delete the restaurant dining tables
                    Providers.RestaurantDiningTableData.DeleteForDiningTableLayout(PluginEntry.DataModel, diningTableLayoutID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RestaurantDiningTable", diningTableLayoutID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        /// <summary>
        /// Shows a dialog for adding restaurant dining tables for a given dining table layout id
        /// </summary>
        /// <param name="diningTableLayoutID">The dining table layout id to add dining tables for (restaurantId, sequence, salestype, diningTableLayoutId)</param>
        /// <param name="maximumNumberOfTables">The current maximum number of tables allowed (rows * columns)</param>
        public static void AddRestaurantDiningTables(RecordIdentifier diningTableLayoutID, int maximumNumberOfTables)
        {
            if (!PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts))
            {
                return;
            }

            Dialogs.AddRestaurantDiningTablesDialog dlg = new Dialogs.AddRestaurantDiningTablesDialog(diningTableLayoutID, maximumNumberOfTables);

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "RestaurantDiningTable", RecordIdentifier.Empty, null);
            }
        }

        /// <summary>
        /// Shows the DiningTabeLayout screens dialog for the given dining table layout id
        /// </summary>
        /// <param name="diningTableLayoutID">The id of the dining table layout id</param>
        public static void ShowDiningTableLayoutScreens(RecordIdentifier diningTableLayoutID)
        {
            if (!PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts))
            {
                return;
            }

            Dialogs.DiningTableLayoutScreensDialog dlg = new Dialogs.DiningTableLayoutScreensDialog(diningTableLayoutID);

            dlg.ShowDialog(PluginEntry.Framework.MainWindow);

        }

        /// <summary>
        /// Shows the DiningTableLayout screens dialog for the given dining table layout id with the given screenNo selected
        /// </summary>
        /// <param name="diningTableLayoutID">The id of the dining table layout id</param>
        /// <param name="screenNo">The screen no that should be selected when the dialog is diplayed</param>
        public static void ShowDiningTableLayoutScreens(RecordIdentifier diningTableLayoutID, RecordIdentifier screenNo)
        {
            if (!PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts))
            {
                return;
            }

            Dialogs.DiningTableLayoutScreensDialog dlg = new Dialogs.DiningTableLayoutScreensDialog(diningTableLayoutID, screenNo);

            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        /// <summary>
        /// Shows the EditRestaurantDiningTable dialog for the given iD
        /// </summary>
        /// <param name="restaurantDiningTableID">The id of the restaurant dining table to edit</param>
        public static void ShowRestaurantDiningTable(RecordIdentifier restaurantDiningTableID)
        {
            if (!PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts))
            {
                return;
            }

            Dialogs.EditRestaurantDiningTableDialog dlg = new Dialogs.EditRestaurantDiningTableDialog(restaurantDiningTableID);

            dlg.ShowDialog(PluginEntry.Framework.MainWindow);
        }

        public static bool DeleteRestaurantDiningTable(RecordIdentifier restaurantDiningTableID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteRestaurantDiningTableQuestion, Properties.Resources.DeleteRestaurantDiningTable) == DialogResult.Yes)
                {
                    Providers.RestaurantDiningTableData.Delete(PluginEntry.DataModel, restaurantDiningTableID);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RestaurantDiningTable", restaurantDiningTableID, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }
        #endregion

        #region HospitalityTypes

        /// <summary>
        /// Opens the new hospitality type dialog, and shows the hospitality type view if the user created a new type.
        /// </summary>
        /// <param name="sender">The caller of the operation</param>
        /// <param name="args">Arguments for the operation</param>
        public static void NewHospitalityType(object sender, EventArgs args)
        {
            RecordIdentifier newId = NewHospitalityType();

            if (newId == RecordIdentifier.Empty)
                return;

            PluginOperations.ShowHospitalityType(newId);
        }

        public static RecordIdentifier NewHospitalityType(Store store = null)
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes))
            {
                Dialogs.NewHospitalityTypeDialog dlg = new Dialogs.NewHospitalityTypeDialog(store);

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog();

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.HospitalityTypeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "HospitalityType", dlg.HospitalityTypeID, null);                    
                }

            }

            return selectedID;
        }

        /// <summary>
        /// Opens the hospitality type card for the specific hospitality type
        /// </summary>
        /// <param name="id">The id of the hospitality type to view</param>
        public static void ShowHospitalityType(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.HospitalityTypeView(id));
        }


        /// <summary>
        /// Deletes the hospitality type with the given ID. Also delets all dining table layouts, dining table layout screens and restaurant dining tables.
        /// </summary>
        /// <param name="hospitalityTypeID"></param>
        /// <returns></returns>
        public static bool DeleteHospitalityType(RecordIdentifier hospitalityTypeID)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteHospitalityTypeQuestion, Properties.Resources.DeleteHospitalityType) == DialogResult.Yes)
                {
                    // Delete the hospitality type
                    Providers.HospitalityTypeData.Delete(PluginEntry.DataModel, hospitalityTypeID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "HospitalityType", hospitalityTypeID, null);

                    // Delete the dining table layouts
                    Providers.DiningTableLayoutData.DeleteForHospitalityType(PluginEntry.DataModel, hospitalityTypeID[0], hospitalityTypeID[2]);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "DiningTableLayout", RecordIdentifier.Empty, null);

                    // Delete the dining table layout screens
                    Providers.DiningTableLayoutScreenData.DeleteForHospitalityType(PluginEntry.DataModel, hospitalityTypeID[0], hospitalityTypeID[2]);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "DiningTableLayoutScreen", RecordIdentifier.Empty, null);

                    // Delete the restaurant dining tables
                    Providers.RestaurantDiningTableData.DeleteForHospitalityType(PluginEntry.DataModel, hospitalityTypeID[0], hospitalityTypeID[2]);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "RestaurantDiningTable", RecordIdentifier.Empty, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }
        #endregion

        #region SalesTypes
        /// <summary>
        /// Opens the sales type card for the specific sales type
        /// </summary>
        /// <param name="id">The id of the sales type to view</param>
        public static void ShowSalesType(RecordIdentifier id)
        {
            PluginEntry.Framework.ViewController.Add(new Views.SalesTypeView(id));
        }

        /// <summary>
        /// Opens the new sales type dialog
        /// </summary>
        /// <param name="sender">The caller of the operation</param>
        /// <param name="args">Arguments for the operation</param>
        public static void NewSalesType(object sender, EventArgs args)
        {
            RecordIdentifier newId = NewSalesType();

            if (newId == RecordIdentifier.Empty)
                return;

            PluginOperations.ShowSalesType(newId);
        }

        /// <summary>
        /// Deletes a given sales type
        /// </summary>
        /// <param name="salesTypeId">The sales type to delete</param>
        /// <returns>True if the sales type was deleted, false otherwise</returns>
        public static bool DeleteSalesType(RecordIdentifier salesTypeId)
        {
            bool retValue = false;

            PluginEntry.Framework.SuspendSearchBarClosing();

            if (PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes))
            {
                if (QuestionDialog.Show(Properties.Resources.DeleteSalesTypeQuestion, Properties.Resources.DeleteSalesType) == DialogResult.Yes)
                {
                    Providers.SalesTypeData.Delete(PluginEntry.DataModel, salesTypeId);

                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "SalesType", salesTypeId, null);

                    retValue = true;
                }
            }

            PluginEntry.Framework.ResumeSearchBarClosing();

            return retValue;
        }

        /// <summary>
        /// Displays the new sales type dialog and returns the id of the new sales type.
        /// </summary>
        /// <returns>The RecordIdentifier containing the id of the new sales type</returns>
        public static RecordIdentifier NewSalesType()
        {
            DialogResult result;
            RecordIdentifier selectedID = RecordIdentifier.Empty;

            if (PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes))
            {
                Dialogs.NewSalesTypeDialog dlg = new Dialogs.NewSalesTypeDialog();

                PluginEntry.Framework.SuspendSearchBarClosing();

                result = dlg.ShowDialog(PluginEntry.Framework.MainWindow);

                PluginEntry.Framework.ResumeSearchBarClosing();

                if (result == DialogResult.OK)
                {
                    // We put null in sender so that we also get our own change hint sent.
                    selectedID = dlg.SalesTypeID;
                    PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "SalesType", dlg.SalesTypeID, null);                    
                }
            }

            return selectedID;
        }
        #endregion

        public static void TaskBarItemCallback(object sender, ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == "LSOne.ViewPlugins.Store.Views.StoreView.Hospitality")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.SalesTypes, ShowSalesTypesListView), 200);
            }

            if (arguments.CategoryKey == "LSOne.ViewPlugins.TouchButtons.Views.PosButtonGridMenusView.Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.HospitalityPosMenus, ShowPosMenusListView), 70);
            }

        }

        internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
        {
            args.Add(new Category(Properties.Resources.GeneralSetup, "General setup", null), 100);
            args.Add(new Category(Properties.Resources.StoreSetup, "Store setup", null), 75);
        }

        internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
        {
            if (args.CategoryKey == "General setup")
            {
                if (                    PluginEntry.DataModel.HasPermission(Permission.ManageHospitalitySetup) ||
                    PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes) ||
                    PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes) ||
                    PluginEntry.DataModel.HasPermission(Permission.ManageDiningTableLayouts) ||
                    PluginEntry.DataModel.HasPermission(Permission.ManageRestaurantMenuTypes) ||
                    PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting) ||
                    PluginEntry.DataModel.HasPermission(Permission.ViewPosMenus) ||
                    PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups))
                {
                    args.Add(new Item(Properties.Resources.Hospitality, "Hospitality", null), 490);
                }
            }
        }

        internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
        {
            if (args.CategoryKey == "General setup" && args.ItemKey == "Hospitality")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalitySetup))
                {
                    args.Add(new ItemButton(Properties.Resources.HospitalitySetup,
                        Properties.Resources.HospitalitySetupDescription,
                        ShowHospitalitySetupSheet), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes))
                {
                    args.Add(new ItemButton(Properties.Resources.HospitalityTypes,
                        Properties.Resources.HospitalityTypesDescription,
                        ShowHospitalityTypesLisetView), 20);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.StoreView) && PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes))
                {
                    args.Add(new ItemButton(Properties.Resources.Restaurants,
                        Properties.Resources.RestaurantsDescription,
                        ShowRestaurantSetupSheet), 30);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting))
                {
                    args.Add(new ItemButton(Properties.Resources.PrintingStations,
                        Properties.Resources.PrintingStationsDescription,
                        ShowPrintingStationsListView), 40);

                    args.Add(new ItemButton(Properties.Resources.StationRouting,
                        Properties.Resources.StationRoutingDescription,
                        ShowStationPrintingView), 50);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ViewPosMenus))
                {
                    args.Add(new ItemButton(Properties.Resources.HospitalityPosMenus,
                        Properties.Resources.PosMenusDescription,
                        ShowPosMenusListView), 70);
                
                    args.Add(new ItemButton(Properties.Resources.HospitalityMenuGroups,
                        Properties.Resources.PosLookupsDescription,
                        ShowPosLookupListView), 80);
                }
            }

            if (PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes))
            {
                // Add a button handler to the Retail items button collection
                if (args.CategoryKey == "Retail" && args.ItemKey == "RetailItems")
                {
                    args.Add(new ItemButton(Properties.Resources.SalesTypes,
                        Properties.Resources.SalesTypesDescription,
                        ShowSalesTypesListView), 400);
                }
            }

            
        }

        internal static void AddRibbonPages(object sender, RibbonPageArguments args)
        {
            args.Add(new Page(Properties.Resources.Hospitality, "Hospitality"), 800);
            args.Add(new Page(Properties.Resources.Setup, "Setup"), 900);
        }

        internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
        {
            if (args.PageKey == "Hospitality")
            {
                args.Add(new PageCategory(Properties.Resources.Sites, "Sites"), 100);
                args.Add(new PageCategory(Properties.Resources.Setup, "Setup"), 200);
                args.Add(new PageCategory(Properties.Resources.KitchenPrinting, "KitchenPrinting"), 300);
                args.Add(new PageCategory(Properties.Resources.PosMenus, "PosMenus"), 400);
            }
            if (args.PageKey == "Setup")
            {
                args.Add(new PageCategory(Properties.Resources.SalesTypes, "SalesTypes"), 600);
            }
        }

        internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
        {
            if (args.PageKey == "Hospitality" && args.CategoryKey == "Sites")
            {
                if (PluginEntry.Framework.FindImplementor(null, "ViewStores", null) != null && PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.Restaurants,
                        Properties.Resources.Restaurants,
                        Properties.Resources.RestaurantsTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        Properties.Resources.store_32,
                        ShowRestaurantSetupSheet,
                        "Restaurants"), 10);
                }
            }

            if (args.PageKey == "Hospitality" && args.CategoryKey == "Setup")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.HospitalityTypes,
                            Properties.Resources.HospitalityTypes,
                            Properties.Resources.HospitalityTypesTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            null,
                            ShowHospitalityTypesLisetView,
                            "HospitalityTypes"), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalitySetup))
                {
                    args.Add(new CategoryItem(
                            Properties.Resources.HospitalitySetup,
                            Properties.Resources.HospitalitySetup,
                            Properties.Resources.HospitalitySetupTooltipDescription,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            null,
                            ShowHospitalitySetupSheet,
                            "HospitalitySetup"), 20);
                }
         
                if (PluginEntry.DataModel.HasPermission(Permission.ManageSalesTypes))
                {
                    args.Add(new CategoryItem(
                        Properties.Resources.SalesTypes,
                        Properties.Resources.SalesTypes,
                        Properties.Resources.SalesTypesTooltipDescription,
                        CategoryItem.CategoryItemFlags.Button,
                        null,
                        ShowSalesTypesListView,
                        "SalesTypesList"), 30);
                }
            }

            if (args.PageKey == "Hospitality" && args.CategoryKey == "KitchenPrinting")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting))
                {
                    args.Add(new CategoryItem(
                           Properties.Resources.StationPrinting,
                           Properties.Resources.StationPrinting,
                           Properties.Resources.StationPrintingTooltipDescription,
                           CategoryItem.CategoryItemFlags.DropDown,
                           null,
                           Properties.Resources.print_32,
                           null,
                           "Stations"), 10);
                }
            }

            if (args.PageKey == "Hospitality" && args.CategoryKey == "PosMenus")
            {

                if ((PluginEntry.DataModel.HasPermission(Permission.ViewPosMenus)) || (PluginEntry.DataModel.HasPermission(Permission.EditPosMenus)))
                {
                    args.Add(new CategoryItem(
                          Properties.Resources.PosMenuButtons,
                          Properties.Resources.HospitalityPosMenus,
                          Properties.Resources.HospitalityPosMenusTooltip,
                          CategoryItem.CategoryItemFlags.Button,
                          null,
                          null,
                          ShowPosMenusListView,
                          "HospPosMenu"), 10);
                }

                if (PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups))
                { 
                    args.Add(new CategoryItem(
                            Properties.Resources.ButtonMenuGroups,
                            Properties.Resources.HospitalityMenuGroups,
                            Properties.Resources.HospitalityMenuGroupsTooltip,
                            CategoryItem.CategoryItemFlags.Button,
                            null,
                            null,
                            ShowPosLookupListView,
                            "HospMenuGroup"), 20);
                }
            }
        }

       internal static void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            switch (args.Key)
            {
                case "RibbonSetup":
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalitySetup))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.HospitalitySetup + "...",
                            null,
                            10,
                            ShowHospitalitySetupSheet));
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.HospitalityTypes + "...",
                            null,
                            20,
                            ShowHospitalityTypesLisetView));
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManageStationPrinting))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.RestaurantStations + "...",
                            null,
                            40,
                            ShowPrintingStationsListView));

                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.StationRouting + "...",
                            null,
                            50,
                            PluginOperations.ShowStationPrintingView));
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ViewPosMenus))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.HospitalityPosMenus + "...",
                            null,
                            70,
                            PluginOperations.ShowPosMenusListView));
                    }

                    if (PluginEntry.DataModel.HasPermission(Permission.ManageMenuGroups))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.HospitalityMenuGroups + "...",
                            null,
                            80,
                            PluginOperations.ShowPosLookupListView));
                    }
                    break;
                case "RibbonStations":
                    
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.StationRouting,
                            null,
                            10,
                            PluginOperations.ShowStationPrintingView));
                    
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.RestaurantStations,
                            null,
                            20,
                            PluginOperations.ShowPrintingStationsListView));

                    break;
            }
        }

        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            switch (args.ContextName)
            {
                case "LSOne.ViewPlugins.Store.Views.StoreView":
                    args.Add(new TabControl.Tab(Properties.Resources.HospitalityTypes, ViewPages.HospitalityRestaurantHospitalityTypesPage.CreateInstance), 400);
                    args.Add(new TabControl.Tab(Properties.Resources.RestaurantMenuTypes, ViewPages.HospitalityRestaurantMenuTypesPage.CreateInstance), 450);
                    break;
                case "LSOne.ViewPlugins.Terminals.Views.TerminalView":
                    if (PluginEntry.DataModel.HasPermission(Permission.ManageHospitalityTypes))
                    {
                        args.Add(
                            new TabControl.Tab(Properties.Resources.Hospitality,
                                ViewPages.HospitalityTerminalHospitalityPage.CreateInstance), 30);
                    }
                    break;
                case "LSOne.ViewPlugins.Profiles.Views.FunctionalityProfileView":
                    args.Add(new TabControl.Tab(Properties.Resources.Hospitality, ViewPages.HospitalityFunctionalityProfileHospitalityPage.CreateInstance), 30);
                    break;
                case "LSOne.ViewPlugins.RetailItems.Views.ItemView":
                    if(PluginEntry.DataModel.HasPermission(Permission.ManageKitchenDisplayStations))
                    {
                        args.Add(new TabControl.Tab(Properties.Resources.Hospitality, ItemTabKey.Hospitality.ToString(), ViewPages.HospitalityRetailItemPage.CreateInstance), 1000);
                    }
                    break;

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
    }
}
