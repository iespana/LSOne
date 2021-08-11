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
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class PromotionLineMultiEditDialog : DialogBase
    {
        private enum RowTypeEnum
        {
            SelectedLine,
            LineToAdd,
            LineToRemove
        }

        private RecordIdentifier offerID;        
        private Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> selectedLines;
        private Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> linesToAdd;
        private Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>> linesToRemove;
        private Style redStrikeThroughStyle;
        private Style greenStyle;
        private Style redStyle;
        private MultiEditEnums.ItemGroupEnum currentItemGroup;

        public PromotionLineMultiEditDialog(RecordIdentifier offerID)
        {
            InitializeComponent();            
           
            // The dictionaries are initialized so that managing the key-value pairs is easier, i.e we don't have to always check for
            // existing values. This makes managing the different lists in the dictionaries easier
            selectedLines = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>();
            selectedLines.Add(MultiEditEnums.ItemGroupEnum.Item, Providers.DiscountOfferLineData.GetSimplePromotionLines(PluginEntry.DataModel, offerID, DiscountOfferLine.DiscountOfferTypeEnum.Item));
            selectedLines.Add(MultiEditEnums.ItemGroupEnum.RetailDepartment, Providers.DiscountOfferLineData.GetSimplePromotionLines(PluginEntry.DataModel, offerID, DiscountOfferLine.DiscountOfferTypeEnum.RetailDepartment));
            selectedLines.Add(MultiEditEnums.ItemGroupEnum.RetailGroup, Providers.DiscountOfferLineData.GetSimplePromotionLines(PluginEntry.DataModel, offerID, DiscountOfferLine.DiscountOfferTypeEnum.RetailGroup));
            selectedLines.Add(MultiEditEnums.ItemGroupEnum.SpecialGroup, Providers.DiscountOfferLineData.GetSimplePromotionLines(PluginEntry.DataModel, offerID, DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup));

            linesToAdd = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>();
            linesToAdd.Add(MultiEditEnums.ItemGroupEnum.Item, new List<MasterIDEntity>());
            linesToAdd.Add(MultiEditEnums.ItemGroupEnum.RetailDepartment, new List<MasterIDEntity>());
            linesToAdd.Add(MultiEditEnums.ItemGroupEnum.RetailGroup, new List<MasterIDEntity>());
            linesToAdd.Add(MultiEditEnums.ItemGroupEnum.SpecialGroup, new List<MasterIDEntity>());

            linesToRemove = new Dictionary<MultiEditEnums.ItemGroupEnum, List<MasterIDEntity>>();
            linesToRemove.Add(MultiEditEnums.ItemGroupEnum.Item, new List<MasterIDEntity>());
            linesToRemove.Add(MultiEditEnums.ItemGroupEnum.RetailDepartment, new List<MasterIDEntity>());
            linesToRemove.Add(MultiEditEnums.ItemGroupEnum.RetailGroup, new List<MasterIDEntity>());
            linesToRemove.Add(MultiEditEnums.ItemGroupEnum.SpecialGroup, new List<MasterIDEntity>());

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
            DiscountOffer offer = Providers.DiscountOfferData.Get(PluginEntry.DataModel, offerID, DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion);

            // Go through the lines to add
            foreach (MasterIDEntity line in linesToAdd[MultiEditEnums.ItemGroupEnum.Item])
            {
                CreateAndSavePromotionLine(line, MultiEditEnums.ItemGroupEnum.Item, offer);
            }

            foreach (MasterIDEntity line in linesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment])
            {
                CreateAndSavePromotionLine(line, MultiEditEnums.ItemGroupEnum.RetailDepartment, offer);
            }

            foreach (MasterIDEntity line in linesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup])
            {
                CreateAndSavePromotionLine(line, MultiEditEnums.ItemGroupEnum.RetailGroup, offer);
            }

            foreach (MasterIDEntity line in linesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup])
            {
                CreateAndSavePromotionLine(line, MultiEditEnums.ItemGroupEnum.SpecialGroup, offer);
            }
            
            // Go through the lines to delete
            foreach (MasterIDEntity line in linesToRemove[MultiEditEnums.ItemGroupEnum.Item])
            {
                Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.Item));
                Providers.DiscountOfferLineData.DeletePromotionByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.Item));
            }

            foreach (MasterIDEntity line in linesToRemove[MultiEditEnums.ItemGroupEnum.RetailDepartment])
            {
                Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.RetailDepartment));
                Providers.DiscountOfferLineData.DeletePromotionByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.RetailDepartment));
            }

            foreach (MasterIDEntity line in linesToRemove[MultiEditEnums.ItemGroupEnum.RetailGroup])
            {
                Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.RetailGroup));
                Providers.DiscountOfferLineData.DeletePromotionByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.RetailGroup));
            }

            foreach (MasterIDEntity line in linesToRemove[MultiEditEnums.ItemGroupEnum.SpecialGroup])
            {
                Providers.DiscountOfferLineData.DeleteByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.SpecialGroup));
                Providers.DiscountOfferLineData.DeletePromotionByRelation(PluginEntry.DataModel, offerID, line.ID, MapSelectionToDiscountOfferType(MultiEditEnums.ItemGroupEnum.SpecialGroup));
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

            switch ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex)
            {
                case MultiEditEnums.ItemGroupEnum.Item:
                    cmbRelation.SelectionList = new List<IDataEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.Item]);
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
            List<MasterIDEntity> selectionList = new List<MasterIDEntity>();
            List<MasterIDEntity> addList = new List<MasterIDEntity>();
            List<MasterIDEntity> removeList = new List<MasterIDEntity>();
            bool largerSize = false;
            switch ((MultiEditEnums.ItemGroupEnum)cmbType.SelectedIndex)
            {                
                case MultiEditEnums.ItemGroupEnum.Item:
                    selectionList = new List<MasterIDEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.Item]);
                    addList = new List<MasterIDEntity>(linesToAdd[MultiEditEnums.ItemGroupEnum.Item]);
                    removeList = new List<MasterIDEntity>(linesToRemove[MultiEditEnums.ItemGroupEnum.Item]);
                    largerSize = true;
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailGroup:
                    selectionList = new List<MasterIDEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    addList = new List<MasterIDEntity>(linesToAdd[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    removeList = new List<MasterIDEntity>(linesToRemove[MultiEditEnums.ItemGroupEnum.RetailGroup]);
                    break;
                case MultiEditEnums.ItemGroupEnum.RetailDepartment:
                    selectionList = new List<MasterIDEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    addList = new List<MasterIDEntity>(linesToAdd[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    removeList = new List<MasterIDEntity>(linesToRemove[MultiEditEnums.ItemGroupEnum.RetailDepartment]);
                    break;
                case MultiEditEnums.ItemGroupEnum.SpecialGroup:
                    selectionList = new List<MasterIDEntity>(selectedLines[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    addList = new List<MasterIDEntity>(linesToAdd[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    removeList = new List<MasterIDEntity>(linesToRemove[MultiEditEnums.ItemGroupEnum.SpecialGroup]);
                    break;
            }

            e.ControlToEmbed = new MultiSearchPanel(PluginEntry.DataModel,
                                                                selectionList.Cast<DataEntity>().ToList(),
                                                                addList.Cast<DataEntity>().ToList(),
                                                                removeList.Cast<DataEntity>().ToList(),
                                                                TypeToSearchTypeEnum(), 
                                                                false,
                                                                largerSize);
            cmbRelation.ShowDropDownOnTyping = true;
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
        
        private void CreateAndSavePromotionLine(MasterIDEntity line, MultiEditEnums.ItemGroupEnum type, DiscountOffer offer)
        {
            PromotionOfferLine offerLine = new PromotionOfferLine();
            offerLine.OfferID = offerID;
            offerLine.TargetMasterID = line.ID;
            offerLine.ItemRelation = line.ReadadbleID;
            offerLine.Type = MapSelectionToDiscountOfferType(type);
            offerLine.Text = line.Text;            

            if (offer.DiscountPercent == 0.0M && offer.DiscountAmount != 0.0M)
            {
                
            }

            offerLine.OfferPrice = 0.0M;
            offerLine.OfferPriceIncludeTax = 0.0M;
            
            offerLine.DiscountPercent = offer.DiscountPercent;

            if (type == MultiEditEnums.ItemGroupEnum.Item)
            {                
                RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, line.ID);

                if (retailItem != null)
                {
                    decimal standardPrice = retailItem.SalesPrice;
                    decimal standardPriceWithTax = standardPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, retailItem);
                    offerLine.DiscountAmount = standardPrice * (offer.DiscountPercent / 100.0M);
                    decimal discountAmounWithTax = standardPriceWithTax * (offer.DiscountPercent / 100.0M);
                    offerLine.DiscountamountIncludeTax = discountAmounWithTax;
                }
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
            foreach (MasterIDEntity line in linesToAdd[itemGroup])
            {
                Row row = new Row();
                row.AddCell(new Cell(GetItemGroupDescription(itemGroup), greenStyle));
                row.AddCell(new Cell(line.Text, greenStyle));
                row.AddCell(new Cell(line.ExtendedText, greenStyle));
                row.AddCell(new Cell(Properties.Resources.Add, greenStyle));
                row.Tag = Tuple.Create(itemGroup, RowTypeEnum.LineToAdd, line);

                IconButton button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                lvlEditPreview.AddRow(row);
            }   
        }

        private void AddSelectedAndRemovedPreviewRows(MultiEditEnums.ItemGroupEnum itemGroup)
        {
            //Copy selected and removed to a new list. This is needed so we don't modify the existing lists in memory
            List<MasterIDEntity> selected = selectedLines[itemGroup];
            List<MasterIDEntity> removed = linesToRemove[itemGroup];

            List<DataEntity> selectedAndRemoved = new List<DataEntity>();

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
                    Row row = new Row();
                    row.AddCell(new Cell(GetItemGroupDescription(itemGroup), redStrikeThroughStyle));
                    row.AddCell(new Cell(line.Text, redStrikeThroughStyle));
                    row.AddCell(new Cell(line.ExtendedText, redStrikeThroughStyle));
                    row.AddCell(new Cell(Properties.Resources.Delete, redStyle));
                    row.Tag = Tuple.Create(itemGroup, RowTypeEnum.LineToRemove, line);

                    IconButton button = new IconButton(Properties.Resources.revert_16, Properties.Resources.Undo, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
                else if (!linesToAdd[itemGroup].Exists(p => p.ID == line.ID))
                {
                    Row row = new Row();
                    row.AddText(GetItemGroupDescription(itemGroup));
                    row.AddText(line.Text);
                    row.AddText(line.ExtendedText);
                    row.AddText(Properties.Resources.EditTypeNone);
                    row.Tag = Tuple.Create(itemGroup, RowTypeEnum.SelectedLine, line);

                    IconButton button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

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
            var tuple = (Tuple<MultiEditEnums.ItemGroupEnum, RowTypeEnum, MasterIDEntity>)lvlEditPreview.Row(args.RowNumber).Tag;

            // Row type
            switch (tuple.Item2)
            {
                case RowTypeEnum.SelectedLine:                   
                    if (!linesToAdd[tuple.Item1].Exists(p => p.ID == tuple.Item3.ID))
                    {
                        selectedLines[tuple.Item1].Remove(tuple.Item3);
                        linesToRemove[tuple.Item1].Add(tuple.Item3);
                    }
                    break;
                case RowTypeEnum.LineToAdd:
                    linesToAdd[tuple.Item1].Remove(tuple.Item3);
                    selectedLines[tuple.Item1].Remove(tuple.Item3);
                    break;
                case RowTypeEnum.LineToRemove:
                    // Undo a remove
                    linesToRemove[tuple.Item1].Remove(tuple.Item3);
                    selectedLines[tuple.Item1].Add(tuple.Item3);                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LoadPreviewLines();
            CheckEnabled();
        }
    }
}
