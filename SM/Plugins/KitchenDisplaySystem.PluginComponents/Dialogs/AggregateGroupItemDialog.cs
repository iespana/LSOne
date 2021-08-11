using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class AggregateGroupItemDialog : DialogBase
    {
        private readonly RecordIdentifier aggregateGroupID;
        private readonly bool editLines;
        private AggregateGroupItem.TypeEnum selectedRelationType;

        private Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>> itemsAlreadyInGroup;
        private Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>> selectedLines;
        private Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>> linesToAdd;
        private Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>> linesToRemove;

        private Style redStrikeThroughStyle;
        private Style greenStyle;
        private Style redStyle;

        public AggregateGroupItemDialog(RecordIdentifier aggregateGroupID, bool editLines)
        {
            InitializeComponent();

            this.aggregateGroupID = aggregateGroupID;
            this.editLines = editLines;
            var emptyItemLists = new Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>>
            {
                { AggregateGroupItem.TypeEnum.Item, new List<DataEntity>() },
                { AggregateGroupItem.TypeEnum.RetailGroup, new List<DataEntity>() },
                { AggregateGroupItem.TypeEnum.SpecialGroup, new List<DataEntity>() }
            };

            itemsAlreadyInGroup = new Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>>
            {
                { AggregateGroupItem.TypeEnum.Item,
                  Providers.KitchenDisplayAggregateGroupItemData.ItemsConnected(PluginEntry.DataModel, aggregateGroupID) },
                { AggregateGroupItem.TypeEnum.RetailGroup,
                  Providers.KitchenDisplayAggregateGroupItemData.RetailGroupsConnected(PluginEntry.DataModel, aggregateGroupID) },
                { AggregateGroupItem.TypeEnum.SpecialGroup,
                  Providers.KitchenDisplayAggregateGroupItemData.SpecialGroupsConnected(PluginEntry.DataModel, aggregateGroupID) }
            };

            selectedLines = editLines 
                ? new Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>>(itemsAlreadyInGroup) 
                : new Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>> {
                    { AggregateGroupItem.TypeEnum.Item, new List<DataEntity>() },
                    { AggregateGroupItem.TypeEnum.RetailGroup, new List<DataEntity>() },
                    { AggregateGroupItem.TypeEnum.SpecialGroup, new List<DataEntity>() }
                };

            linesToAdd = new Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>> {
                { AggregateGroupItem.TypeEnum.Item, new List<DataEntity>() },
                { AggregateGroupItem.TypeEnum.RetailGroup, new List<DataEntity>() },
                { AggregateGroupItem.TypeEnum.SpecialGroup, new List<DataEntity>() }
            };

            linesToRemove = new Dictionary<AggregateGroupItem.TypeEnum, List<DataEntity>> {
                { AggregateGroupItem.TypeEnum.Item, new List<DataEntity>() },
                { AggregateGroupItem.TypeEnum.RetailGroup, new List<DataEntity>() },
                { AggregateGroupItem.TypeEnum.SpecialGroup, new List<DataEntity>() }
            };

            cmbType.SelectedIndex = 0;
            cmbConnection.DropDown += cmbConnection_DropDown;

            greenStyle = new Style(lvlEditPreview.DefaultStyle) { TextColor = ColorPalette.GreenDark };
            redStyle = new Style(lvlEditPreview.DefaultStyle) { TextColor = ColorPalette.RedDark };
            redStrikeThroughStyle = new Style(lvlEditPreview.DefaultStyle);
            redStrikeThroughStyle.Font = new Font(redStrikeThroughStyle.Font, FontStyle.Strikeout);
            redStrikeThroughStyle.TextColor = ColorPalette.RedDark;

            Header = editLines ? Properties.Resources.AddOrRemoveItemsFromGroup : Properties.Resources.SelectItemsToAddTotheGroup;
            LoadPreviewLines();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled()
        {
            bool added = linesToAdd[AggregateGroupItem.TypeEnum.Item].Count > 0 ||
                         linesToAdd[AggregateGroupItem.TypeEnum.RetailGroup].Count > 0 ||
                         linesToAdd[AggregateGroupItem.TypeEnum.SpecialGroup].Count > 0;

            bool removed = linesToRemove[AggregateGroupItem.TypeEnum.Item].Count > 0 ||
                           linesToRemove[AggregateGroupItem.TypeEnum.RetailGroup].Count > 0 ||
                           linesToRemove[AggregateGroupItem.TypeEnum.SpecialGroup].Count > 0;

            btnOK.Enabled = added || removed;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (var relationType in Enum.GetValues(typeof(AggregateGroupItem.TypeEnum)).Cast<AggregateGroupItem.TypeEnum>())
            {
                foreach (var item in linesToAdd[relationType])
                {
                    CreateAndSaveAggregateGroupItem(item, relationType);
                }

                foreach (var item in linesToRemove[relationType])
                {
                    Providers.KitchenDisplayAggregateGroupItemData.Delete(PluginEntry.DataModel, new RecordIdentifier(item.ID, aggregateGroupID));
                }
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.MultiAdd, "KitchenDisplayAggregateGroupItem", aggregateGroupID, null);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CreateAndSaveAggregateGroupItem(DataEntity item, AggregateGroupItem.TypeEnum relationType)
        {
            AggregateGroupItem aggregateGroupItem = new AggregateGroupItem();
            aggregateGroupItem.ItemID = (string)item.ID;
            aggregateGroupItem.ItemDescription = item.Text;
            aggregateGroupItem.GroupID = aggregateGroupID;
            aggregateGroupItem.Type = relationType;

            Providers.KitchenDisplayAggregateGroupItemData.Save(PluginEntry.DataModel, aggregateGroupItem);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((AggregateGroupItem.TypeEnum)cmbType.SelectedIndex != this.selectedRelationType)
            {
                cmbConnection.SelectedData = new DataEntity("", "");
                this.selectedRelationType = (AggregateGroupItem.TypeEnum)cmbType.SelectedIndex;
            }

            AggregateGroupItem.TypeEnum selectedRelationType = (AggregateGroupItem.TypeEnum)cmbType.SelectedIndex;

            cmbConnection.ShowDropDownOnTyping = 
                (selectedRelationType == AggregateGroupItem.TypeEnum.Item) 
                ? true 
                : false;

            cmbConnection.SelectionList = new List<IDataEntity>(selectedLines[selectedRelationType]);
        }

        private void cmbConnection_DropDown(object sender, DropDownEventArgs e)
        {

            var selectionList = new List<DataEntity>(selectedLines[selectedRelationType]);
            var addList = new List<DataEntity>(linesToAdd[selectedRelationType]);
            var removeList = new List<DataEntity>(linesToRemove[selectedRelationType]);

            string initialSearchText;
            if (!string.IsNullOrEmpty(e.DisplayText))
            {
                initialSearchText = e.DisplayText;
            }
            else
            {
                initialSearchText = cmbConnection.SelectedData != null ? ((DataEntity)cmbConnection.SelectedData).Text : "";
            }

            e.ControlToEmbed = new MultiSearchPanel(PluginEntry.DataModel,
                                                    selectionList,
                                                    addList,
                                                    removeList,
                                                    TypeToSearchTypeEnum(),
                                                    editLines 
                                                        ? new List<RecordIdentifier>() 
                                                        : itemsAlreadyInGroup[selectedRelationType].Select(item => item.ID).ToList(),
                                                    false,
                                                    selectedRelationType == AggregateGroupItem.TypeEnum.Item,
                                                    true,
                                                    initialSearchText);
        }

        private SearchTypeEnum TypeToSearchTypeEnum()
        {
            switch ((AggregateGroupItem.TypeEnum)cmbType.SelectedIndex)
            {
                default:
                case AggregateGroupItem.TypeEnum.Item:
                    return SearchTypeEnum.RetailItems;
                case AggregateGroupItem.TypeEnum.RetailGroup:
                    return SearchTypeEnum.RetailGroups;
                case AggregateGroupItem.TypeEnum.SpecialGroup:
                    return SearchTypeEnum.SpecialGroups;
            }
        }

        private void cmbConnection_SelectedDataChanged(object sender, EventArgs e)
        {
            int prevAddedCount = linesToAdd[AggregateGroupItem.TypeEnum.Item].Count +
                                 linesToAdd[AggregateGroupItem.TypeEnum.RetailGroup].Count +
                                 linesToAdd[AggregateGroupItem.TypeEnum.SpecialGroup].Count;

            selectedLines[selectedRelationType] = cmbConnection.SelectionList.Cast<DataEntity>().ToList();
            linesToAdd[selectedRelationType] = cmbConnection.AddList.Cast<DataEntity>().ToList();
            linesToRemove[selectedRelationType] = cmbConnection.RemoveList.Cast<DataEntity>().ToList();

            int curAddedCount = linesToAdd[AggregateGroupItem.TypeEnum.Item].Count +
                                linesToAdd[AggregateGroupItem.TypeEnum.RetailGroup].Count +
                                linesToAdd[AggregateGroupItem.TypeEnum.SpecialGroup].Count;

            LoadPreviewLines();
            CheckEnabled();

            if (curAddedCount > prevAddedCount && !lvlEditPreview.RowIsOnScreen(lvlEditPreview.RowCount - 1))
            {
                lvlEditPreview.ScrollRowIntoView(lvlEditPreview.RowCount - 1);
            }
        }

        private void LoadPreviewLines()
        {
            lvlEditPreview.ClearRows();

            foreach (var relationType in Enum.GetValues(typeof(AggregateGroupItem.TypeEnum)).Cast<AggregateGroupItem.TypeEnum>())
            {
                AddSelectedAndRemovedPreviewRows(relationType);
                AddNewPreviewRows(relationType);
            }

            lvlEditPreview.AutoSizeColumns(true);
        }

        private void AddNewPreviewRows(AggregateGroupItem.TypeEnum relationType)
        {
            foreach (var line in linesToAdd[relationType])
            {
                var row = new Row();
                row.AddCell(new Cell(GetRelationTypeDescription(relationType), greenStyle));
                row.AddCell(new Cell(line.Text, greenStyle));
                row.AddCell(new Cell(Properties.Resources.Add, greenStyle));
                row.Tag = (RelationType: relationType, RowType: RowTypeEnum.LineToAdd, groupLine: line);

                var button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false));

                lvlEditPreview.AddRow(row);
            }
        }

        private void AddSelectedAndRemovedPreviewRows(AggregateGroupItem.TypeEnum relationType)
        {
            //Copy selected and removed to a new list. This is needed so we don't modify the existing lists in memory
            var selected = selectedLines[relationType];
            var removed = linesToRemove[relationType];

            var selectedAndRemoved = new List<DataEntity>();

            foreach (var line in selected)
            {
                selectedAndRemoved.Add(line);
            }

            foreach (var line in removed)
            {
                selectedAndRemoved.Add(line);
            }

            selectedAndRemoved.Sort(CompareDataEntities);

            foreach (var line in selectedAndRemoved)
            {
                if (linesToRemove[relationType].Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddCell(new Cell(GetRelationTypeDescription(relationType), redStrikeThroughStyle));
                    row.AddCell(new Cell(line.Text, redStrikeThroughStyle));
                    row.AddCell(new Cell(Properties.Resources.Delete, redStyle));
                    row.Tag = (RelationType: relationType, RowType: RowTypeEnum.LineToRemove, groupLine: line);

                    var button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false));

                    lvlEditPreview.AddRow(row);
                }
                else if (!linesToAdd[relationType].Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddText(GetRelationTypeDescription(relationType));
                    row.AddText(line.Text);
                    row.AddText(Properties.Resources.EditTypeNone);
                    row.Tag = (RelationType: relationType, RowType: RowTypeEnum.SelectedLine, groupLine: line);

                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false));

                    lvlEditPreview.AddRow(row);
                }
            }
        }

        private int CompareDataEntities(DataEntity dataEntity, DataEntity entity)
        {
            return dataEntity.Text.CompareTo(entity.Text);
        }

        private string GetRelationTypeDescription(AggregateGroupItem.TypeEnum relationType)
        {
            switch (relationType)
            {
                case AggregateGroupItem.TypeEnum.Item:
                    return Properties.Resources.Item;

                case AggregateGroupItem.TypeEnum.RetailGroup:
                    return Properties.Resources.RetailGroup;

                case AggregateGroupItem.TypeEnum.SpecialGroup:
                    return Properties.Resources.SpecialGroup;

                default:
                    return "";
            }
        }

        private void lvlEditPreview_CellAction(object sender, CellEventArgs args)
        {
            var previewItem = ((AggregateGroupItem.TypeEnum RelationType, RowTypeEnum RowType, DataEntity groupLine))lvlEditPreview.Row(args.RowNumber).Tag;

            switch (previewItem.RowType)
            {
                case RowTypeEnum.SelectedLine:
                    if (!linesToAdd[previewItem.RelationType].Exists(p => p.ID == previewItem.groupLine.ID))
                    {
                        selectedLines[previewItem.RelationType].Remove(previewItem.groupLine);
                        linesToRemove[previewItem.RelationType].Add(previewItem.groupLine);
                    }
                    break;
                case RowTypeEnum.LineToAdd:
                    linesToAdd[previewItem.RelationType].Remove(previewItem.groupLine);
                    selectedLines[previewItem.RelationType].Remove(previewItem.groupLine);
                    break;
                case RowTypeEnum.LineToRemove:
                    linesToRemove[previewItem.RelationType].Remove(previewItem.groupLine);
                    selectedLines[previewItem.RelationType].Add(previewItem.groupLine);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LoadPreviewLines();
            CheckEnabled();
        }
    }
}