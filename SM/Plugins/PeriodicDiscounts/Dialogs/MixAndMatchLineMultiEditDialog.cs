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
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class MixAndMatchLineMultiEditDialog : DialogBase
    {
        private enum RowTypeEnum
        {
            SelectedLine,
            LineToAdd,
            LineToRemove
        }

        private RecordIdentifier offerID;
        private RecordIdentifier currentLineGroupID;
        private Dictionary<RecordIdentifier, LineGroupLines> lineGroupLines; 
        private Style redStrikeThroughStyle;
        private Style greenStyle;
        private Style redStyle;
        private MultiEditEnums.ItemGroupEnum currentItemGroup;
        private MixAndMatchLineGroup emptyLineGroup;
        private List<MixAndMatchLineGroup> mmLineGroups;
        private LineGroupLines currentLines;

        public MixAndMatchLineMultiEditDialog(RecordIdentifier offerID)
        {
            InitializeComponent();

            currentLineGroupID = RecordIdentifier.Empty;

            lineGroupLines = new Dictionary<RecordIdentifier, LineGroupLines>();
            mmLineGroups = Providers.MixAndMatchLineGroupData.GetGroups(PluginEntry.DataModel, offerID, 0, false);            

            foreach (MixAndMatchLineGroup mixAndMatchLineGroup in mmLineGroups)
            {
                lineGroupLines.Add(mixAndMatchLineGroup.ID, new LineGroupLines(mixAndMatchLineGroup.LineGroup, offerID, mixAndMatchLineGroup.Color));
            }

            currentLines = null;

            redStrikeThroughStyle = new Style(lvlEditPreview.DefaultStyle);
            redStrikeThroughStyle.Font = new Font(redStrikeThroughStyle.Font, FontStyle.Strikeout);
            redStrikeThroughStyle.TextColor = ColorPalette.RedDark;

            greenStyle = new Style(lvlEditPreview.DefaultStyle);
            greenStyle.TextColor = ColorPalette.GreenDark;

            redStyle = new Style(lvlEditPreview.DefaultStyle);
            redStyle.TextColor = ColorPalette.RedDark;

            this.offerID = offerID;            
            cmbType.SelectedIndex = 0;
            
            currentItemGroup = MultiEditEnums.ItemGroupEnum.Item;

            emptyLineGroup = new MixAndMatchLineGroup();
            emptyLineGroup.OfferID = RecordIdentifier.Empty;
            emptyLineGroup.LineGroup = RecordIdentifier.Empty;
            emptyLineGroup.Text = Properties.Resources.All;
            cmbLineGroups.SelectedData = emptyLineGroup;

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
            bool enabled = false;

            foreach (MixAndMatchLineGroup mixAndMatchLineGroup in mmLineGroups)
            {
                enabled = enabled || lineGroupLines[mixAndMatchLineGroup.ID].IsModified();
            }
                          
            btnOK.Enabled = enabled;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DiscountOffer offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch);

            foreach (MixAndMatchLineGroup mixAndMatchLineGroup in mmLineGroups)
            {
                LineGroupLines linesToEdit = lineGroupLines[mixAndMatchLineGroup.ID];

                // Go through the lines to add
                foreach (MasterIDEntity line in linesToEdit.LinesToAdd[MultiEditEnums.ItemGroupEnum.Item])
                {
                    CreateAndSaveDiscountOfferLine(line, MultiEditEnums.ItemGroupEnum.Item, mixAndMatchLineGroup.LineGroup, offer);
                }

                foreach (MasterIDEntity line in linesToEdit.LinesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment])
                {
                    CreateAndSaveDiscountOfferLine(line, MultiEditEnums.ItemGroupEnum.RetailDepartment, mixAndMatchLineGroup.LineGroup, offer);
                }

                foreach (MasterIDEntity line in linesToEdit.LinesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup])
                {
                    CreateAndSaveDiscountOfferLine(line, MultiEditEnums.ItemGroupEnum.RetailGroup, mixAndMatchLineGroup.LineGroup, offer);
                }

                foreach (MasterIDEntity line in linesToEdit.LinesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup])
                {
                    CreateAndSaveDiscountOfferLine(line, MultiEditEnums.ItemGroupEnum.SpecialGroup, mixAndMatchLineGroup.LineGroup, offer);
                }

                // Go through the lines to delete
                foreach (MasterIDEntity line in linesToEdit.LinesToRemove[MultiEditEnums.ItemGroupEnum.Item])
                {
                    Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel,
                                                           offerID,
                                                           line.ID,
                                                           MapSelectionToDiscountOfferType(
                                                               MultiEditEnums.ItemGroupEnum.Item));
                }

                foreach (MasterIDEntity line in linesToEdit.LinesToRemove[MultiEditEnums.ItemGroupEnum.RetailDepartment])
                {
                    Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel,
                                                           offerID,
                                                           line.ID,
                                                           MapSelectionToDiscountOfferType(
                                                               MultiEditEnums.ItemGroupEnum.RetailDepartment));
                }

                foreach (MasterIDEntity line in linesToEdit.LinesToRemove[MultiEditEnums.ItemGroupEnum.RetailGroup])
                {
                    Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel,
                                                           offerID,
                                                           line.ID,
                                                           MapSelectionToDiscountOfferType(
                                                               MultiEditEnums.ItemGroupEnum.RetailGroup));
                }

                foreach (MasterIDEntity line in linesToEdit.LinesToRemove[MultiEditEnums.ItemGroupEnum.SpecialGroup])
                {
                    Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel,
                                                           offerID,
                                                           line.ID,
                                                           MapSelectionToDiscountOfferType( MultiEditEnums.ItemGroupEnum.SpecialGroup));
                }
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex != currentItemGroup)
            {
                cmbRelation.SelectedData = new DataEntity("", "");
                currentItemGroup = (MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex;
            }            

            if (currentLines == null)
            {
                return;
            }
            
            switch ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex)
            {
                case MultiEditEnums.ItemGroupEnum.Item:
                    cmbRelation.SelectionList = new List<IDataEntity>(currentLines.SelectedLines[MultiEditEnums.ItemGroupEnum.Item]);
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailGroup:
                    cmbRelation.SelectionList = new List<IDataEntity>(currentLines.SelectedLines[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailDepartment:
                    cmbRelation.SelectionList = new List<IDataEntity>(currentLines.SelectedLines[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    break;
                case MultiEditEnums.ItemGroupEnum.SpecialGroup:
                    cmbRelation.SelectionList = new List<IDataEntity>(currentLines.SelectedLines[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    break;
            }
        }

        private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
        {
            List<MasterIDEntity> selectionList = new List<MasterIDEntity>();
            List<MasterIDEntity> addList = new List<MasterIDEntity>();
            List<MasterIDEntity> removeList = new List<MasterIDEntity>();
            bool largerSize = false;            

            switch ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex)
            {                
                case MultiEditEnums.ItemGroupEnum.Item:
                    selectionList = new List<MasterIDEntity>(currentLines.SelectedLines[MultiEditEnums.ItemGroupEnum.Item]);
                    addList = new List<MasterIDEntity>(currentLines.LinesToAdd[MultiEditEnums.ItemGroupEnum.Item]);
                    removeList = new List<MasterIDEntity>(currentLines.LinesToRemove[MultiEditEnums.ItemGroupEnum.Item]);
                    largerSize = true;
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailGroup:
                    selectionList = new List<MasterIDEntity>(currentLines.SelectedLines[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    addList = new List<MasterIDEntity>(currentLines.LinesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    removeList = new List<MasterIDEntity>(currentLines.LinesToRemove[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailDepartment:
                    selectionList = new List<MasterIDEntity>(currentLines.SelectedLines[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    addList = new List<MasterIDEntity>(currentLines.LinesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    removeList = new List<MasterIDEntity>(currentLines.LinesToRemove[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    break;
                case MultiEditEnums.ItemGroupEnum.SpecialGroup:
                    selectionList = new List<MasterIDEntity>(currentLines.SelectedLines[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    addList = new List<MasterIDEntity>(currentLines.LinesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    removeList = new List<MasterIDEntity>(currentLines.LinesToRemove[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    break;
            }

            e.ControlToEmbed = new MultiSearchPanel(PluginEntry.DataModel,
                                                                selectionList.Cast<DataEntity>().ToList(),
                                                                addList.Cast<DataEntity>().ToList(),
                                                                removeList.Cast<DataEntity>().ToList(),
                                                                TypeToSearchTypeEnum(),
                                                                GetExcludedIDs((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex),
                                                                false,
                                                                largerSize);
            cmbRelation.ShowDropDownOnTyping = true;
        }

        private List<RecordIdentifier> GetExcludedIDs(MultiEditEnums.ItemGroupEnum groupType)
        {
            List<RecordIdentifier> excludedIDs = new List<RecordIdentifier>();
            RecordIdentifier lineGroupID;

            foreach (MixAndMatchLineGroup mixAndMatchLineGroup in mmLineGroups)
            {
                lineGroupID = mixAndMatchLineGroup.ID;

                if (lineGroupID == currentLineGroupID)
                {
                    continue;
                }

                LineGroupLines linesToExclude = lineGroupLines[lineGroupID];


                foreach (MasterIDEntity line in linesToExclude.SelectedLines[groupType])
                {
                    excludedIDs.Add(line.ID);
                }
            }

            return excludedIDs;
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
                    return SearchTypeEnum.RetailItemsMasterID;
            }
        }

        private void cmbConnection_SelectedDataChanged(object sender, EventArgs e)
        {            
            int prevAddedCount = currentLines.GetCurrentAddedLinesCount();

            currentLines.SelectedLines[(MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex] = cmbRelation.SelectionList.Cast<MasterIDEntity>().ToList();
            currentLines.LinesToAdd[(MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex] = cmbRelation.AddList.Cast<MasterIDEntity>().ToList();
            currentLines.LinesToRemove[(MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex] = cmbRelation.RemoveList.Cast<MasterIDEntity>().ToList();

            int curAddedCount = currentLines.GetCurrentAddedLinesCount();

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
        
        private void CreateAndSaveDiscountOfferLine(MasterIDEntity line, MultiEditEnums.ItemGroupEnum type, RecordIdentifier lineGroupID, DiscountOffer offer)
        {
            DiscountOfferLine offerLine = new DiscountOfferLine();
            offerLine.OfferID = offerID;
            offerLine.TargetMasterID = line.ID;
            offerLine.ItemRelation = line.ReadadbleID;
            offerLine.Type = MapSelectionToDiscountOfferType(type);
            offerLine.Text = line.Text;
            offerLine.LineGroup = lineGroupID;

            // By default we create a deal price discount line if the mix and match offer itself is set to line specific
            if (offer.MixAndMatchDiscountType == DiscountOffer.MixAndMatchDiscountTypeEnum.LineSpecific && type == MultiEditEnums.ItemGroupEnum.Item)
            {
                RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, line.ID);

                if (retailItem == null)
                {
                    throw new DataIntegrityException(typeof(RetailItem), line.ID);
                }

                decimal standardPrice = retailItem.SalesPrice;
                decimal standardPriceWithTax = standardPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, retailItem);

                offerLine.DiscountType = DiscountOfferLine.MixAndMatchDiscountTypeEnum.DealPrice;
                offerLine.DiscountPercent = standardPriceWithTax;
            }

            Providers.DiscountOfferLineData.Save(PluginEntry.DataModel, offerLine);
        }

        private void LoadPreviewLines()
        {            
            lvlEditPreview.ClearRows();

            bool showingAllLines = currentLineGroupID.IsEmpty;

            if (showingAllLines)
            {
                LineGroupLines linesToShow;

                foreach (MixAndMatchLineGroup mixAndMatchLineGroup in mmLineGroups)
                {
                    linesToShow = lineGroupLines[mixAndMatchLineGroup.ID];

                    AddSelectedAndRemovedPreviewRows(linesToShow, MultiEditEnums.ItemGroupEnum.Item);
                    AddSelectedAndRemovedPreviewRows(linesToShow, MultiEditEnums.ItemGroupEnum.RetailDepartment);
                    AddSelectedAndRemovedPreviewRows(linesToShow, MultiEditEnums.ItemGroupEnum.RetailGroup);
                    AddSelectedAndRemovedPreviewRows(linesToShow, MultiEditEnums.ItemGroupEnum.SpecialGroup);

                    AddNewPreviewRows(linesToShow, MultiEditEnums.ItemGroupEnum.Item);
                    AddNewPreviewRows(linesToShow, MultiEditEnums.ItemGroupEnum.RetailDepartment);
                    AddNewPreviewRows(linesToShow, MultiEditEnums.ItemGroupEnum.RetailGroup);
                    AddNewPreviewRows(linesToShow, MultiEditEnums.ItemGroupEnum.SpecialGroup);  
                }
            }
            else
            {
                if (currentLines == null)
                {
                    return;
                }

                AddSelectedAndRemovedPreviewRows(currentLines, MultiEditEnums.ItemGroupEnum.Item);
                AddSelectedAndRemovedPreviewRows(currentLines, MultiEditEnums.ItemGroupEnum.RetailDepartment);
                AddSelectedAndRemovedPreviewRows(currentLines, MultiEditEnums.ItemGroupEnum.RetailGroup);
                AddSelectedAndRemovedPreviewRows(currentLines, MultiEditEnums.ItemGroupEnum.SpecialGroup);

                AddNewPreviewRows(currentLines, MultiEditEnums.ItemGroupEnum.Item);
                AddNewPreviewRows(currentLines, MultiEditEnums.ItemGroupEnum.RetailDepartment);
                AddNewPreviewRows(currentLines, MultiEditEnums.ItemGroupEnum.RetailGroup);
                AddNewPreviewRows(currentLines, MultiEditEnums.ItemGroupEnum.SpecialGroup);   
            }
                      
            lvlEditPreview.AutoSizeColumns(true);
        }


        private void AddNewPreviewRows(LineGroupLines linesToDisplay, MultiEditEnums.ItemGroupEnum itemGroup)
        {
            foreach (MasterIDEntity line in linesToDisplay.LinesToAdd[itemGroup])
            {
                Row row = new Row();
                row.AddCell(new ColorBoxCell(5, linesToDisplay.LineGroupColor, Color.Black));
                row.AddCell(new Cell(GetItemGroupDescription(itemGroup), greenStyle));
                row.AddCell(new Cell(line.Text, greenStyle));
                row.AddCell(new Cell(line.ExtendedText, greenStyle));
                row.AddCell(new Cell(Properties.Resources.Add, greenStyle));
                row.Tag = Tuple.Create(itemGroup, RowTypeEnum.LineToAdd, line, linesToDisplay.LineGroupID);


                IconButton button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                lvlEditPreview.AddRow(row);
            }   
        }

        private void AddSelectedAndRemovedPreviewRows(LineGroupLines linesToDisplay, MultiEditEnums.ItemGroupEnum itemGroup)
        {
            //Copy selected and removed to a new list. This is needed so we don't modify the existing lists in memory
            List<MasterIDEntity> selected =  linesToDisplay.SelectedLines[itemGroup];
            List<MasterIDEntity> removed = linesToDisplay.LinesToRemove[itemGroup];

            List<MasterIDEntity> selectedAndRemoved = new List<MasterIDEntity>();

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
                if (linesToDisplay.LinesToRemove[itemGroup].Exists(p => p.ID == line.ID))
                {
                    Row row = new Row();
                    row.AddCell(new ColorBoxCell(5, linesToDisplay.LineGroupColor, Color.Black));
                    row.AddCell(new Cell(GetItemGroupDescription(itemGroup), redStrikeThroughStyle));
                    row.AddCell(new Cell(line.Text, redStrikeThroughStyle));
                    row.AddCell(new Cell(line.ExtendedText, redStrikeThroughStyle));
                    row.AddCell(new Cell(Properties.Resources.Delete, redStyle));
                    row.Tag = Tuple.Create(itemGroup, RowTypeEnum.LineToRemove, line, linesToDisplay.LineGroupID);

                    IconButton button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
                else if (!linesToDisplay.LinesToAdd[itemGroup].Exists(p => p.ID == line.ID))
                {
                    Row row = new Row();
                    row.AddCell(new ColorBoxCell(5, linesToDisplay.LineGroupColor, Color.Black));
                    row.AddText(GetItemGroupDescription(itemGroup));
                    row.AddText(line.Text);
                    row.AddText(line.ExtendedText);
                    row.AddText(Properties.Resources.EditTypeNone);
                    row.Tag = Tuple.Create(itemGroup, RowTypeEnum.SelectedLine, line, linesToDisplay.LineGroupID);

                    IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
            }            
        }

        private int CompareDataEntities(MasterIDEntity dataEntity, MasterIDEntity entity)
        {
            return dataEntity.Text.CompareTo(entity.Text);
        }

        private void lvlEditPreview_CellAction(object sender, CellEventArgs args)
        {
            var tuple = (Tuple<MultiEditEnums.ItemGroupEnum, RowTypeEnum, MasterIDEntity, RecordIdentifier>)lvlEditPreview.Row(args.RowNumber).Tag;
            LineGroupLines linesToEdit = lineGroupLines[tuple.Item4];

            // Row type
            switch (tuple.Item2)
            {
                case RowTypeEnum.SelectedLine:                   
                    if (!linesToEdit.LinesToAdd[tuple.Item1].Exists(p => p.ID == tuple.Item3.ID))
                    {
                        linesToEdit.SelectedLines[tuple.Item1].Remove(tuple.Item3);
                        linesToEdit.LinesToRemove[tuple.Item1].Add(tuple.Item3);
                    }
                    break;
                case RowTypeEnum.LineToAdd:
                    linesToEdit.LinesToAdd[tuple.Item1].Remove(tuple.Item3);
                    linesToEdit.SelectedLines[tuple.Item1].Remove(tuple.Item3);
                    break;
                case RowTypeEnum.LineToRemove:
                    // Undo a remove
                    linesToEdit.LinesToRemove[tuple.Item1].Remove(tuple.Item3);
                    linesToEdit.SelectedLines[tuple.Item1].Add(tuple.Item3);                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LoadPreviewLines();
            CheckEnabled();
        }

        private void cmbLineGroups_RequestData(object sender, EventArgs e)
        {
            List<MixAndMatchLineGroup> lineGroups = Providers.MixAndMatchLineGroupData.GetGroups(PluginEntry.DataModel, offerID, 0, false);            
            lineGroups.Insert(0, emptyLineGroup);
            cmbLineGroups.SetData(lineGroups, null);
        }

        private void cmbLineGroups_SelectedDataChanged(object sender, EventArgs e)
        {
            currentLineGroupID = cmbLineGroups.SelectedData.ID;

            cmbRelation.Enabled = cmbType.Enabled = !currentLineGroupID.IsEmpty;
            cmbRelation.SelectedData = new DataEntity("", "");

            if (!currentLineGroupID.IsEmpty)
            {
                currentLines = lineGroupLines[cmbLineGroups.SelectedData.ID];
            }

            LoadPreviewLines();
        }
    }
}
