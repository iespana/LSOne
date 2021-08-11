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
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class DiscountOfferLineMultiEditDialog : DialogBase
    {   
        private RecordIdentifier offerID;        
        private Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> selectedLines;
        private Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> linesToAdd;
        private Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> linesToRemove;
        private Style redStrikeThroughStyle;
        private Style greenStyle;
        private Style redStyle;
        private MultiEditEnums.ItemGroupEnum currentItemGroup;
        private DiscountOffer.PeriodicDiscountOfferTypeEnum offerType;

        public DiscountOfferLineMultiEditDialog(RecordIdentifier offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum offerType)
        {
            InitializeComponent();            
           
            // The dictionaries are initialized so that managing the key-value pairs is easier, i.e we don't have to always check for
            // existing values. This makes managing the different lists in the dictionaries easier
            selectedLines = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>
                {
                    {
                        MultiEditEnums.ItemGroupEnum.Item,
                        Providers.DiscountOfferLineData.GetSimpleLines(PluginEntry.DataModel, offerID,
                                                             DiscountOfferLine.DiscountOfferTypeEnum.Item)
                    },
                    {
                        MultiEditEnums.ItemGroupEnum.RetailDepartment,
                        Providers.DiscountOfferLineData.GetSimpleLines(PluginEntry.DataModel, offerID,
                                                             DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment)
                    },
                    {
                        MultiEditEnums.ItemGroupEnum.RetailGroup,
                        Providers.DiscountOfferLineData.GetSimpleLines(PluginEntry.DataModel, offerID,
                                                             DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup)
                    },
                    {
                        MultiEditEnums.ItemGroupEnum.SpecialGroup,
                        Providers.DiscountOfferLineData.GetSimpleLines(PluginEntry.DataModel, offerID,
                                                             DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup)
                    }
                };

            linesToAdd = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>
                {
                    {MultiEditEnums.ItemGroupEnum.Item, new List<MasterIDEntity>()},
                    {MultiEditEnums.ItemGroupEnum.RetailDepartment, new List<MasterIDEntity>()},
                    {MultiEditEnums.ItemGroupEnum.RetailGroup, new List<MasterIDEntity>()},
                    {MultiEditEnums.ItemGroupEnum.SpecialGroup, new List<MasterIDEntity>()}
                };

            linesToRemove = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>
                {
                    {MultiEditEnums.ItemGroupEnum.Item, new List<MasterIDEntity>()},
                    {MultiEditEnums.ItemGroupEnum.RetailDepartment, new List<MasterIDEntity>()},
                    {MultiEditEnums.ItemGroupEnum.RetailGroup, new List<MasterIDEntity>()},
                    {MultiEditEnums.ItemGroupEnum.SpecialGroup, new List<MasterIDEntity>()}
                };

            redStrikeThroughStyle = new Style(lvlEditPreview.DefaultStyle);
            redStrikeThroughStyle.Font = new Font(redStrikeThroughStyle.Font, FontStyle.Strikeout);
            redStrikeThroughStyle.TextColor = ColorPalette.RedDark;

            greenStyle = new Style(lvlEditPreview.DefaultStyle) {TextColor = ColorPalette.GreenDark };

            redStyle = new Style(lvlEditPreview.DefaultStyle) {TextColor = ColorPalette.RedDark };

            this.offerID = offerID;
            this.offerType = offerType;
            cmbType.SelectedIndex = 0;
            
            currentItemGroup = MultiEditEnums.ItemGroupEnum.Item;
            cmbRelation.Select();
            LoadPreviewLines();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier ID
        {
            get { return offerID; }
        }

        private void CheckEnabled()
        {
            bool added = linesToAdd[MultiEditEnums.ItemGroupEnum.Item].Count > 0 ||
                         linesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment].Count > 0 ||
                         linesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup].Count > 0 ||
                         linesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup].Count > 0;

            bool removed = linesToRemove[MultiEditEnums.ItemGroupEnum.Item].Count > 0 ||
                           linesToRemove[MultiEditEnums.ItemGroupEnum.RetailDepartment].Count > 0 ||
                           linesToRemove[MultiEditEnums.ItemGroupEnum.RetailGroup].Count > 0 ||
                           linesToRemove[MultiEditEnums.ItemGroupEnum.SpecialGroup].Count > 0;
                          
            btnOK.Enabled = added || removed;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            var offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer);

            // Go through the lines to add
            foreach (var line in linesToAdd[MultiEditEnums.ItemGroupEnum.Item])
            {
                CreateAndSaveDiscountOfferLine(line, MultiEditEnums.ItemGroupEnum.Item, offer);
            }

            foreach (var line in linesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment])
            {
                CreateAndSaveDiscountOfferLine(line, MultiEditEnums.ItemGroupEnum.RetailDepartment, offer);
            }

            foreach (var line in linesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup])
            {
                CreateAndSaveDiscountOfferLine(line, MultiEditEnums.ItemGroupEnum.RetailGroup, offer);
            }

            foreach (var line in linesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup])
            {
                CreateAndSaveDiscountOfferLine(line, MultiEditEnums.ItemGroupEnum.SpecialGroup, offer);
            }
            
            // Go through the lines to delete
            foreach (var line in linesToRemove[MultiEditEnums.ItemGroupEnum.Item])
            {
                Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.Item));
            }

            foreach (var line in linesToRemove[MultiEditEnums.ItemGroupEnum.RetailDepartment])
            {
                Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.RetailDepartment));
            }

            foreach (var line in linesToRemove[MultiEditEnums.ItemGroupEnum.RetailGroup])
            {
                Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.RetailGroup));
            }

            foreach (var line in linesToRemove[MultiEditEnums.ItemGroupEnum.SpecialGroup])
            {
                Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.SpecialGroup));
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbStationSelectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex != currentItemGroup)
            {
                cmbRelation.SelectedData = new DataEntity("", "");
                currentItemGroup = (MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex;
            }

            cmbRelation.ShowDropDownOnTyping = false;

            switch ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex)
            {
                case MultiEditEnums.ItemGroupEnum.Item:
                    cmbRelation.SelectionList = new List<IDataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.Item]);
                    cmbRelation.ShowDropDownOnTyping = true;
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailGroup:
                    cmbRelation.SelectionList = new List<IDataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailDepartment:
                    cmbRelation.SelectionList = new List<IDataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    break;
                case MultiEditEnums.ItemGroupEnum.SpecialGroup:
                    cmbRelation.SelectionList = new List<IDataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    break;
            }
        }

        private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
        {
            var selectionList = new List<DataEntity>();
            var addList = new List<DataEntity>();
            var removeList = new List<DataEntity>();
            bool largerSize = false;
            switch ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex)
            {                
                case MultiEditEnums.ItemGroupEnum.Item:
                    selectionList = new List<DataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.Item]);
                    addList = new List<DataEntity>(linesToAdd[MultiEditEnums.ItemGroupEnum.Item]);
                    removeList = new List<DataEntity>(linesToRemove[MultiEditEnums.ItemGroupEnum.Item]);
                    largerSize = true;
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailGroup:
                    selectionList = new List<DataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    addList = new List<DataEntity>(linesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    removeList = new List<DataEntity>(linesToRemove[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailDepartment:
                    selectionList = new List<DataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    addList = new List<DataEntity>(linesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    removeList = new List<DataEntity>(linesToRemove[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    break;
                case MultiEditEnums.ItemGroupEnum.SpecialGroup:
                    selectionList = new List<DataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    addList = new List<DataEntity>(linesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    removeList = new List<DataEntity>(linesToRemove[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    break;
            }

            string initialSearchText;
            if (!string.IsNullOrEmpty(e.DisplayText))
            {
                initialSearchText = e.DisplayText;
            }
            else
            {
                initialSearchText = cmbRelation.SelectedData != null ? ((DataEntity)cmbRelation.SelectedData).Text : "";
            }

            e.ControlToEmbed = new MultiSearchPanel(PluginEntry.DataModel,
                                                                selectionList,
                                                                addList,
                                                                removeList,
                                                                TypeToSearchTypeEnum(),
                                                                false,
                                                                largerSize,
                                                                true,
                                                                initialSearchText);
        }

        private SearchTypeEnum TypeToSearchTypeEnum()
        {
            switch ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex)
            {
                case MultiEditEnums.ItemGroupEnum.Item:
                    return SearchTypeEnum.RetailItemsMasterID;
                    
                case MultiEditEnums.ItemGroupEnum.RetailGroup:
                    return SearchTypeEnum.RetailGroupsMasterID;
                    
                case MultiEditEnums.ItemGroupEnum.RetailDepartment:
                    return SearchTypeEnum.RetailDepartmentsMasterID;
                    
                case MultiEditEnums.ItemGroupEnum.SpecialGroup:
                    return SearchTypeEnum.SpecialGroupsMasterID;
                    
                default:
                    return SearchTypeEnum.RetailItems;
            }
        }

        private void cmbConnection_SelectedDataChanged(object sender, EventArgs e)
        {
            int prevAddedCount = linesToAdd[MultiEditEnums.ItemGroupEnum.Item].Count +
                                 linesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment].Count +
                                 linesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup].Count +
                                 linesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup].Count;

            selectedLines[(MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex] = cmbRelation.SelectionList.Cast<MasterIDEntity>().ToList();
            linesToAdd[(MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex] = cmbRelation.AddList.Cast<MasterIDEntity>().ToList();
            linesToRemove[(MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex] = cmbRelation.RemoveList.Cast<MasterIDEntity>().ToList();

            int curAddedCount = linesToAdd[MultiEditEnums.ItemGroupEnum.Item].Count +
                                linesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment].Count +
                                linesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup].Count +
                                linesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup].Count;

            LoadPreviewLines();
            CheckEnabled();

            if (curAddedCount > prevAddedCount && !lvlEditPreview.RowIsOnScreen(lvlEditPreview.RowCount - 1))
            {
                lvlEditPreview.ScrollRowIntoView(lvlEditPreview.RowCount - 1);
            }
        }
        
        private DiscountOfferLine.DiscountOfferTypeEnum MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum selection)        
        {
            switch (selection)
            {
                case MultiEditEnums.ItemGroupEnum.Item:
                    return DiscountOfferLine.DiscountOfferTypeEnum.Item;
                case MultiEditEnums.ItemGroupEnum.RetailGroup:
                    return DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup;
                case MultiEditEnums.ItemGroupEnum.RetailDepartment:
                    return DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment;
                case MultiEditEnums.ItemGroupEnum.SpecialGroup:
                    return DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup;
                default:
                    return DiscountOfferLine.DiscountOfferTypeEnum.Item;                    
            }
        }

        private string GetItemGroupDescription(MultiEditEnums.ItemGroupEnum itemGroup)
        {
            switch (itemGroup)
            {
                case MultiEditEnums.ItemGroupEnum.Item:
                    return Properties.Resources.Item;
                    
                case MultiEditEnums.ItemGroupEnum.RetailGroup:
                    return Properties.Resources.RetailGroup;
                    
                case MultiEditEnums.ItemGroupEnum.RetailDepartment:
                    return Properties.Resources.RetailDepartment;
                    
                case MultiEditEnums.ItemGroupEnum.SpecialGroup:
                    return Properties.Resources.SpecialGroup;
                    
                default:
                    return "";
            }
        }
        
        private void CreateAndSaveDiscountOfferLine(MasterIDEntity line, MultiEditEnums.ItemGroupEnum type, DiscountOffer offer)
        {
            var offerLine = new DiscountOfferLine
                {
                    OfferID = offerID,
                    ItemRelation = line.ReadadbleID,
                    TargetMasterID = line.ID,
                    Type = MapSelectionToDiscountOfferType(type),
                    Text = line.Text
                };

            switch (offerType)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    offerLine.DiscountPercent = offer.DiscountPercent;
                    break;
            }            

            Providers.DiscountOfferLineData.Save(PluginEntry.DataModel, offerLine);
        }

        private void LoadPreviewLines()
        {            
            lvlEditPreview.ClearRows();

            AddSelectedAndRemovedPreviewRows(MultiEditEnums.ItemGroupEnum.Item);
            AddSelectedAndRemovedPreviewRows(MultiEditEnums.ItemGroupEnum.RetailDepartment);
            AddSelectedAndRemovedPreviewRows(MultiEditEnums.ItemGroupEnum.RetailGroup);
            AddSelectedAndRemovedPreviewRows(MultiEditEnums.ItemGroupEnum.SpecialGroup);

            AddNewPreviewRows(MultiEditEnums.ItemGroupEnum.Item);
            AddNewPreviewRows(MultiEditEnums.ItemGroupEnum.RetailDepartment);
            AddNewPreviewRows(MultiEditEnums.ItemGroupEnum.RetailGroup);
            AddNewPreviewRows(MultiEditEnums.ItemGroupEnum.SpecialGroup);              

            lvlEditPreview.AutoSizeColumns(true);
        }

        private void AddNewPreviewRows(MultiEditEnums.ItemGroupEnum itemGroup)
        {
            foreach (var line in linesToAdd[itemGroup])
            {
                var row = new Row();
                row.AddCell(new Cell(GetItemGroupDescription(itemGroup), greenStyle));
                row.AddCell(new Cell(line.Text, greenStyle));
                row.AddCell(new Cell(line.ExtendedText, greenStyle));
                row.AddCell(new Cell(Properties.Resources.Add, greenStyle));
                row.Tag = (ItemGroup: itemGroup, RowType: RowTypeEnum.LineToAdd, groupLine: line);

                var button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false));

                lvlEditPreview.AddRow(row);
            }   
        }

        private void AddSelectedAndRemovedPreviewRows(MultiEditEnums.ItemGroupEnum itemGroup)
        {
            //Copy selected and removed to a new list. This is needed so we don't modify the existing lists in memory
            var selected = selectedLines[itemGroup];
            var removed = linesToRemove[itemGroup];

            var selectedAndRemoved = new List<MasterIDEntity>();

            foreach (MasterIDEntity line in selected)
            {
                selectedAndRemoved.Add(line);
            }

            foreach (MasterIDEntity line in removed)
            {
                selectedAndRemoved.Add(line);
            }

            selectedAndRemoved.Sort(CompareDataEntities);

            foreach (MasterIDEntity line in selectedAndRemoved)
            {
                if (linesToRemove[itemGroup].Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddCell(new Cell(GetItemGroupDescription(itemGroup), redStrikeThroughStyle));
                    row.AddCell(new Cell(line.Text, redStrikeThroughStyle));
                    row.AddCell(new Cell(line.ExtendedText, redStrikeThroughStyle));
                    row.AddCell(new Cell(Properties.Resources.Delete, redStyle));
                    row.Tag = (ItemGroup: itemGroup, RowType: RowTypeEnum.LineToRemove, groupLine: line);

                    var button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Left, "", false));

                    lvlEditPreview.AddRow(row);
                }
                else if (!linesToAdd[itemGroup].Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddText(GetItemGroupDescription(itemGroup));
                    row.AddText(line.Text);
                    row.AddText(line.ExtendedText);
                    row.AddText(Properties.Resources.EditTypeNone);
                    row.Tag = (ItemGroup: itemGroup, RowType: RowTypeEnum.SelectedLine, groupLine: line);

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

        private void lvlEditPreview_CellAction(object sender, CellEventArgs args)
        {
            var previewItem = ((MultiEditEnums.ItemGroupEnum ItemGroup, RowTypeEnum RowType, MasterIDEntity groupLine))lvlEditPreview.Row(args.RowNumber).Tag;

            switch (previewItem.RowType)
            {
                case RowTypeEnum.SelectedLine:
                    if (!linesToAdd[previewItem.ItemGroup].Exists(p => p.ID == previewItem.groupLine.ID))
                    {
                        selectedLines[previewItem.ItemGroup].Remove(previewItem.groupLine);
                        linesToRemove[previewItem.ItemGroup].Add(previewItem.groupLine);
                    }
                    break;
                case RowTypeEnum.LineToAdd:
                    linesToAdd[previewItem.ItemGroup].Remove(previewItem.groupLine);
                    selectedLines[previewItem.ItemGroup].Remove(previewItem.groupLine);
                    break;
                case RowTypeEnum.LineToRemove:
                    linesToRemove[previewItem.ItemGroup].Remove(previewItem.groupLine);
                    selectedLines[previewItem.ItemGroup].Add(previewItem.groupLine);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LoadPreviewLines();
            CheckEnabled();
        }
    }
}
