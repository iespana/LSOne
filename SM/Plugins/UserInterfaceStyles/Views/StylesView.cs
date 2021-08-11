using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.BusinessObjects.UserInterface;
using LSOne.DataLayer.BusinessObjects.UserInterface.ListItems;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.DataProviders.UserInterface;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using Style = LSOne.Controls.Style;
using LSOne.ViewPlugins.UserInterfaceStyles.Properties;
using LSOne.DataLayer.GenericConnector;
using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.Enums;
using LSOne.DataLayer.BusinessObjects.Enums;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using LSOne.ViewCore.EventArguments;
using System.Resources;
using LSOne.DataLayer.DatabaseUtil;
using LSOne.DataLayer.DatabaseUtil.ScriptInformation;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Views
{
    /// <summary>
    /// Displays a list of all the <see cref="PosStyle" />s available in the system.
    /// </summary>
    public partial class StylesView : ViewBase
    {
        private RecordIdentifier selectedStyleID;
        private bool backwardsSort;
        private Setting searchBarSetting;
        private static Guid BarSettingID = new Guid("c7dcca46-7d03-4c13-82f1-333e3fd584fa");
        List<PosStyle> styleList = null;

        /// <summary>
        /// The constructor for the view that displays all the <see cref="PosStyle" />s available in the system.
        /// </summary>
        /// <param name="selectedId">The ID of the <see cref="PosStyle" /> that should be selected when the view is opened</param>
        public StylesView(RecordIdentifier selectedId)
            : this()
        {
            this.selectedStyleID = selectedId;
        }

        /// <summary>
        /// The constructor for the view that displays all the <see cref="PosStyle" />s available in the system.
        /// </summary>
        public StylesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            HeaderText = Properties.Resources.Styles;

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup);

            ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup);

            lvStyles.ContextMenuStrip = new ContextMenuStrip();
            lvStyles.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

            backwardsSort = false;

            searchBar.BuddyControl = lvStyles;
            searchBar.FocusFirstInput();
        }

        /// <summary>
        /// Provides audit descriptors for the view. This is never called if the ViewAttributes.Audit flag is not set.
        /// </summary>
        /// <param name="contexts">Add the applicable audit descriptors to this list</param>
        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //contexts.Add(new AuditDescriptor("NameOfAuditDescriptor", dataObjectId, description, true));
        }

        /// <summary>
        /// Sets the top context bar header. This context bar header can include Save, Close, Delete and Revert but you can also add to it.
        /// </summary>
        /// <value>The text to be displayed on the header of the Context box</value>
        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.Styles;
            }
        }

        /// <summary>
        /// Overload this to return the ID of the current view. This should return RecordIdentifier.Empty for single instance views. For multi instance
        /// view return logical context identifier, f.x. for a store that would be the storeId
        /// </summary>        
        /// <returns>The logical context ID if applicable</returns>
        public override RecordIdentifier ID
        {
            get
            {
                return RecordIdentifier.Empty;
            }
        }

        /// <summary>
        /// The event called by the Site Manager framework during a data change broadcast.
        /// </summary>
        /// <param name="changeAction">Enum that tells you the type of change</param>
        /// <param name="objectName">Tells you what changed , f.x. "Store"</param>
        /// <param name="changeIdentifier">The ID of the changed object</param>
        /// <param name="param">Extra information</param>
        public override void OnDataChanged(DataEntityChangeType changeAction, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if ((objectName == "UIStyle" || objectName == "Style") && (changeAction == DataEntityChangeType.Add
                                              || changeAction == DataEntityChangeType.Delete
                                              || changeAction == DataEntityChangeType.Edit
                                              || changeAction == DataEntityChangeType.MultiDelete))
            {
                if(changeAction == DataEntityChangeType.Edit || changeAction == DataEntityChangeType.Add)
                {
                    selectedStyleID = changeIdentifier;
                }

                LoadData(false);
            }
        }

        /// <summary>
        /// Loads and displays the data to be available in the view. The function is called by the framework when the view is being loaded.
        /// </summary>
        /// <param name="isRevert">Tells if we are loading from a revert call</param>
        /// <remarks>Overload this function in each view to load the relevant data</remarks>
        protected override void LoadData(bool isRevert)
        {
            ShowProgress((sender1, e1) => LoadObjects(backwardsSort), GetLocalizedSearchingText());
        }

        private void LoadObjects(bool backwards)
        {
            Dictionary<Guid, ContextStyleDescriptor> contexts = PluginEntry.Framework.GetPluginContextStyleDescriptors();

            lvStyles.ClearRows();
            lvStyles.Columns.Clear();

            string sort = backwards == false ? "NAME ASC" : "NAME DESC";

            List<SearchParameterResult> results = searchBar.SearchParameterResults;

            string description = string.Empty;
            bool beginsWith = true;
            GradientModeEnum? selectedGradient = null;
            ShapeEnum? selectedShape = null;
            StyleType? selectedType = null;

            foreach (SearchParameterResult result in results)
            {
                switch (result.ParameterKey)
                {
                    case "Type":
                        switch (result.ComboSelectedIndex)
                        {
                            case 1:
                                selectedType = StyleType.Normal;
                                break;
                            case 2:
                                selectedType = StyleType.DualDisplayLine;
                                break;
                        }
                        break;
                    case "Gradient":
                        int index = result.ComboSelectedIndex;
                        selectedGradient = index == 0 ? (GradientModeEnum?)null : (GradientModeEnum)(index - 1);
                        break;
                    case "Description":
                        description = result.StringValue;
                        beginsWith = (result.SearchModification == SearchParameterResult.SearchModificationEnum.BeginsWith);
                        break;
                    case "Shape":
                        int shapeIndex = result.ComboSelectedIndex;
                        selectedShape = shapeIndex == 0 ? (ShapeEnum?)null : (ShapeEnum)(shapeIndex - 1);
                        break;
                }
            }


            if (PluginEntry.Framework.IsAtleastSiteManagerBasic)
            {
                styleList = Providers.PosStyleData.GetListByFilters(PluginEntry.DataModel, description, beginsWith, selectedType, selectedShape, selectedGradient, sort);
            }

            Style fontStyle = null;

            if (styleList != null)
            {
                lvStyles.Columns.Add(new Column(Properties.Resources.Description, true));
                lvStyles.Columns.Add(new Column(Properties.Resources.Font, true));
                lvStyles.Columns.Add(new Column(Properties.Resources.BackGroundColor, true));
                lvStyles.Columns.Add(new Column(Properties.Resources.BackGroundColor + " 2", true));
                lvStyles.Columns.Add(new Column(Properties.Resources.Gradient, true));
                lvStyles.Columns.Add(new Column(Properties.Resources.Shape, true));
                lvStyles.Columns.Add(new Column(Properties.Resources.IsSystemStyle, true));
            }

            if (styleList != null)
            {
                foreach (PosStyle posStyle in styleList)
                {
                    #region Set Font style
                    FontStyle style = FontStyle.Regular;

                    if (posStyle.FontBold == true) { style |= FontStyle.Bold; }
                    if (posStyle.FontItalic == true) { style |= FontStyle.Italic; }
                    if (posStyle.FontStrikethrough == true) { style |= FontStyle.Strikeout; }

                    fontStyle = new Style(lvStyles.DefaultStyle);
                    fontStyle.Font = new System.Drawing.Font(posStyle.FontName, posStyle.FontSize, style, fontStyle.Font.Unit, Convert.ToByte(posStyle.FontCharset));
                    fontStyle.TextColor = Color.FromArgb(posStyle.ForeColor);

                    #endregion

                    #region Create and add row
                    Row row = new Row();
                    row.BackColor = ColorPalette.GrayLight;
                    row.AddText(posStyle.IsSystemStyle ? posStyle.StyleTypeString :posStyle.Text);
                    row.AddCell(new Cell(Properties.Resources.FontPreview, fontStyle));
                    row.AddCell(new ColorBoxCell(5, Color.FromArgb(posStyle.BackColor), Color.Black));
                    row.AddCell(new ColorBoxCell(5, Color.FromArgb(posStyle.BackColor2), Color.Black));
                    row.AddText(ButtonStyleUtils.GetGradientText(posStyle.GradientMode));
                    row.AddText(ButtonStyleUtils.GetShapeText(posStyle.Shape));
                    row.AddCell(new CheckBoxCell(posStyle.IsSystemStyle, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));

                    row.Tag = posStyle.ID;
                    lvStyles.AddRow(row);

                    #endregion

                    if (selectedStyleID == posStyle.ID)
                    {
                        lvStyles.Selection.Set(lvStyles.RowCount - 1);
                        lvStyles.ScrollRowIntoView(lvStyles.RowCount - 1);
                    }
                }
            }

            lvStyles_SelectionChanged(this, EventArgs.Empty);
            lvStyles.AutoSizeColumns();
            HideProgress();
        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.EditStyle(selectedStyleID);
        }

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.EditStyle(RecordIdentifier.Empty);
        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            List<RecordIdentifier> IDs = GetSelectedIDs();
            PluginOperations.DeleteStyle(IDs);
        }

        private List<RecordIdentifier> GetSelectedIDs()
        {
            var IDs = new List<RecordIdentifier>();
            for (int i = 0; i < lvStyles.Selection.Count; i++)
            {
                if (lvStyles.Selection[i] == null)
                {
                    continue;
                }
                IDs.Add((RecordIdentifier)((Row)lvStyles.Selection[i]).Tag);
            }

            return IDs;
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvStyles.ContextMenuStrip;
            menu.Items.Clear();

            // Each item is a line in the right click menu. 
            // Usually there is not much that needs to be changed here

            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemove_EditButtonClicked);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true; // The default item has a bold font
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;
            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvStyles_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (args.ColumnNumber == 0)
            {
                backwardsSort = !backwardsSort;
                LoadData(false);
            }
        }

        private void lvStyles_SelectionChanged(object sender, EventArgs e)
        {
            bool cannotEdit = false;
            bool canDelete = true;

            if (lvStyles.Selection.Count > 0 && lvStyles.Selection[0] != null)
            {
                selectedStyleID = (RecordIdentifier)lvStyles.Selection[0].Tag;
            }

            if (lvStyles.Selection.Count == 1 && lvStyles.Selection[0] != null)
            {
                var id = (RecordIdentifier)((Row)lvStyles.Selection[0]).Tag;
                PosStyle selectedStyle = styleList.FirstOrDefault(s => s.ID == id);
                if (selectedStyle != null && selectedStyle.IsSystemStyle)
                {
                    canDelete = false;
                }
            }
            else if (lvStyles.Selection.Count > 1)
            {
                selectedStyleID = "";
            }



            btnsEditAddRemove.EditButtonEnabled = (lvStyles.Selection.Count == 1) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup) && !cannotEdit;
            btnsEditAddRemove.RemoveButtonEnabled = (lvStyles.Selection.Count >= 1) && PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup) && GetSelectedIDs().Count > 0 && canDelete;
        }

        private void lvStyles_RowDoubleClick(object sender, RowEventArgs args)
        {

            // If the edit button is enabled when run the edit button operation. No need to change anything here
            if (btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void searchBar_SetupConditions(object sender, EventArgs e)
        {
            List<object> typeList = new List<object>();
            typeList.Add(Resources.AllTypes);
            typeList.Add(Resources.NormalStyles);
            typeList.Add(Resources.SystemStyles);

            List<object> shapeList = new List<object>();
            shapeList.Add(Resources.AllShapes);
            shapeList.Add(Resources.RoundRectangle);
            shapeList.Add(Resources.Triangle);
            shapeList.Add(Resources.Hexagon);
            shapeList.Add(Resources.Ellipse);
            shapeList.Add(Resources.Windows3D);
            shapeList.Add(Resources.Rectangle);

            List<object> gradientList = new List<object>();
            gradientList.Add(Resources.AllGradients);
            gradientList.Add(Resources.None);
            gradientList.Add(Resources.Horizontal);
            gradientList.Add(Resources.Vertical);
            gradientList.Add(Resources.ForwardDiagonal);
            gradientList.Add(Resources.BackwardDiagonal);

            searchBar.AddCondition(new ConditionType(Resources.Description, "Description", ConditionType.ConditionTypeEnum.Text));
            searchBar.AddCondition(new ConditionType(Resources.Type, "Type", ConditionType.ConditionTypeEnum.ComboBox, typeList, 0, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.Gradient, "Gradient", ConditionType.ConditionTypeEnum.ComboBox, gradientList, 0, 0, false));
            searchBar.AddCondition(new ConditionType(Resources.Shape, "Shape", ConditionType.ConditionTypeEnum.ComboBox, shapeList, 0, 0, false));

            searchBar.DefaultNumberOfSections = 2;
            searchBar_LoadDefault(this, EventArgs.Empty);
        }

        private void searchBar_LoadDefault(object sender, EventArgs e)
        {
            searchBarSetting = PluginEntry.DataModel.Settings.GetLongSetting(PluginEntry.DataModel, BarSettingID, SettingType.UISetting, "");

            if (searchBarSetting != null && searchBarSetting.LongUserSetting != "")
            {
                searchBar.LoadFromSetupString(searchBarSetting.LongUserSetting);
            }
        }

        private void searchBar_SaveAsDefault(object sender, EventArgs e)
        {
            string layoutString = searchBar.LayoutStringWithData;

            if (searchBarSetting.LongUserSetting != layoutString)
            {
                searchBarSetting.LongUserSetting = layoutString;
                searchBarSetting.UserSettingExists = true;

                PluginEntry.DataModel.Settings.SaveSetting(PluginEntry.DataModel, BarSettingID, SettingsLevel.User, searchBarSetting);
            }
            searchBar.GetLocalizedSavingText();
        }

        private void searchBar_UnknownControlAdd(object sender, UnknownControlCreateArguments args)
        {

        }

        private void searchBar_SearchClicked(object sender, EventArgs e)
        {
            LoadData(false);
        }

        private void searchBar_UnknownControlGetSelection(object sender, UnknownControlSelectionArguments args)
        {

        }

        private void searchBar_UnknownControlHasSelection(object sender, UnknownControlSelectionArguments args)
        {

        }

        private void searchBar_UnknownControlRemove(object sender, UnknownControlArguments args)
        {

        }

        private void searchBar_UnknownControlSetSelection(object sender, UnknownControlSelectionArguments args)
        {

        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {

            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.Related_ResetSystemStyles, "ResetSystemStyles", true, ResetSystemStyles), 5000000);
            }
        }

        private void ResetSystemStyles(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
               Resources.ResetSystemStyleQuestion,
               Resources.ResetSystemStyleHeader) == DialogResult.Yes)
            {
                List<PosStyle> styleList = Providers.PosStyleData.GetListByFilters(PluginEntry.DataModel, string.Empty, true, StyleType.DualDisplayLine, null, null, "NAME ASC");
                styleList.ForEach(s => Providers.PosStyleData.Delete(PluginEntry.DataModel, s.ID));

                List<ScriptInfo> scriptInfo = DatabaseUtility.GetSQLScriptInfo(DataLayer.DatabaseUtil.Enums.RunScripts.SystemData);
                foreach (ScriptInfo systemStyleScript in scriptInfo.Where(w => w.ResourceName.Contains(ScriptInfo.SystemStylesConst)).OrderBy(o => o.ResourceName))
                {
                    DatabaseUtility.RunSpecificScript(PluginEntry.DataModel, systemStyleScript, null);
                }

                LoadData(false);
            }
        }

    }
}
