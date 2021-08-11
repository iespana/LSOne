using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.PeriodicDiscounts.ViewPages
{
    public partial class PriceDiscountGroupPeriodicDiscountsPage : UserControl, ITabView
    {
        private PriceDiscGroupEnum displayType;
        private RecordIdentifier discountGroupID;
        private RecordIdentifier selectedID;
        private List<DiscountOffer> allOffers; 

        public PriceDiscountGroupPeriodicDiscountsPage(TabControl owner, TabControl.Tab ownerTab)
            : this()
        {
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {            
            return new PriceDiscountGroupPeriodicDiscountsPage((TabControl)sender, tab);
        }
        
        public static void TabMessage(TabControl sender, TabControl.Tab tab, int hint, object data)
        {
            PriceDiscGroupEnum displayType = (PriceDiscGroupEnum)hint;

            tab.Visible = displayType == PriceDiscGroupEnum.LineDiscountGroup;
            sender.Invalidate();
        }

        public PriceDiscountGroupPeriodicDiscountsPage()
        {
            InitializeComponent();

            lvValues.Columns[0].Tag = DiscountOfferSorting.OfferNumber;
            lvValues.Columns[1].Tag = DiscountOfferSorting.Description;
            lvValues.Columns[2].Tag = DiscountOfferSorting.OfferType;
            lvValues.Columns[3].Tag = DiscountOfferSorting.Status;
            lvValues.Columns[4].Tag = DiscountOfferSorting.Priority;
            lvValues.Columns[5].Tag = DiscountOfferSorting.DiscountValidationPeriod;
            lvValues.Columns[6].Tag = DiscountOfferSorting.StartingDate;
            lvValues.Columns[7].Tag = DiscountOfferSorting.EndingDate;
            lvValues.SetSortColumn(0, true);

            selectedID = "";

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;
        }       

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            displayType = (PriceDiscGroupEnum)((int)internalContext);
            discountGroupID = context[2];
            allOffers = Providers.DiscountOfferData.GetPeriodicDiscounts(PluginEntry.DataModel, true, DiscountOfferSorting.OfferNumber, false);

            if (displayType == PriceDiscGroupEnum.LineDiscountGroup)
            {
                LoadLines();
            }
        }

        private void LoadLines()
        {
            List<DiscountOffer> offers = Providers.DiscountOfferData.GetOffersForLineDiscountGroup(PluginEntry.DataModel, discountGroupID, (DiscountOfferSorting)lvValues.SortColumn.Tag, !lvValues.SortedAscending);

            lvValues.ClearRows();

            foreach (DiscountOffer offer in offers)
            {
                Row row = new Row();
                row.AddText((string)offer.ID);
                row.AddText(offer.Text);
                row.AddText(OfferTypeName(offer.OfferType));
                row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);

                if (allOffers.Any(p => p.ID != offer.ID && p.Priority == offer.Priority))
                {
                    IconButton iconButton = new IconButton(Properties.Resources.Warning16, Properties.Resources.ViewConflictingPriorities);
                    IconButtonCell ibtnCell = new IconButtonCell(iconButton,
                                                                 IconButtonCell.IconButtonIconAlignmentEnum.Left,
                                                                 offer.Priority.ToString());

                    row.AddCell(ibtnCell);
                }
                else
                {
                    row.AddCell(new NumericCell(offer.Priority.ToString(), offer.Priority));
                }

                row.AddText(offer.ValidationPeriodDescription);

                row.AddCell(new DateTimeCell(offer.StartingDate.ToShortDateString(), offer.StartingDate.DateTime));
                row.AddCell(new DateTimeCell(offer.EndingDate.ToShortDateString(), offer.EndingDate.DateTime));
                row.AddText(offer.PriceGroupName);

                row.Tag = offer;
                
                lvValues.AddRow(row);

                if (offer.ID == selectedID)
                {
                    lvValues.Selection.Set(lvValues.RowCount - 1);
                }
            }

            lvValues.AutoSizeColumns();            
        }

        private string OfferTypeName(DiscountOffer.PeriodicDiscountOfferTypeEnum type)
        {
            string result = "";

            switch (type)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                    result = Properties.Resources.Multibuy;
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                    result = Properties.Resources.MixMatch;
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    result = Properties.Resources.DiscountOffer;
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion:
                    result = Properties.Resources.PromotionOffer;
                    break;
            }
            return result;
        }


        public bool DataIsModified()
        {
            return false;
        }

        public bool SaveData()
        {            

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("MultibuyDiscountLine", 0, Properties.Resources.MultibuyConfigurations, false));
            contexts.Add(new AuditDescriptor("DiscountOfferLine", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy, Properties.Resources.MultibuyLines, false));
            contexts.Add(new AuditDescriptor("DiscountOfferLine", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer, Properties.Resources.DiscountOfferLines, false));
            contexts.Add(new AuditDescriptor("POSMMLINEGROUPS", RecordIdentifier.Empty, Properties.Resources.MixAndMatchLineGroups, false));
            contexts.Add(new AuditDescriptor("DiscountOfferLine", (int)DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch, Properties.Resources.MixAndMatchLines, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PeriodicDiscount")
            {
                selectedID = changeIdentifier;
                LoadLines();
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        void lvValues_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            ExtendedMenuItem item = new ExtendedMenuItem(
                Properties.Resources.EditDiscount,
                100,
                EditOfferHandler);
            item.Enabled = btnEdit.Enabled;
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 200));

            item = new ExtendedMenuItem(
                Properties.Resources.ViewPriorities,
                300,
                ViewPrioritiesHandler);
            item.Enabled = btnEdit.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("DiscountGroupPeriodicDiscountList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void EditOfferHandler(object sender, EventArgs e)
        {
            DiscountOffer offer = (DiscountOffer)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag;

            DiscountOffer.PeriodicDiscountOfferTypeEnum offerType = offer.OfferType;
            RecordIdentifier offerID = offer.ID;

            switch (offerType)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                    PluginOperations.ShowSpecificMultibuyView(offerID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                    PluginOperations.ShowSpecificMixAndMatchView(offerID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    PluginOperations.ShowSpecificDiscountOffersView(offerID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion:
                    PluginOperations.ShowSpecificPromotionsView(offerID);
                    break;
            }
        }


        private void lvValues_HeaderClicked(object sender, ColumnEventArgs args)
        {
            if (lvValues.SortColumn == args.Column)
            {
                lvValues.SetSortColumn(args.Column, !lvValues.SortedAscending);
                LoadLines();
            }
            else
            {
                lvValues.SetSortColumn(args.Column, true);
                LoadLines();
            }
        }

        private void lvValues_RowDoubleClick(object sender, RowEventArgs args)
        {
            EditOfferHandler(this, EventArgs.Empty);
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            if (lvValues.Selection.Count > 0)
            {
                selectedID = ((DiscountOffer)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag).ID;
            }

            btnEdit.Enabled = lvValues.Selection.Count > 0;
        }

        private void ViewPrioritiesHandler(object sender, EventArgs args)
        {
            PluginOperations.ShowPeriodicDiscountPrioritiesView(selectedID);
        }

        private void lvValues_CellAction(object sender, CellEventArgs args)
        {
            RecordIdentifier selectedID = ((DiscountOffer)lvValues.Row(args.RowNumber).Tag).ID;

            // The only cell that can fire this event is the icon button cell tied to the priority column
            PluginOperations.ShowPeriodicDiscountPrioritiesView(selectedID);
        }
    }
}
