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
    public partial class CustomerPeriodicDiscountsPage : UserControl, ITabView
    {
        private RecordIdentifier customerID;
        private RecordIdentifier selectedID;
        private List<DiscountOffer> allOffers; 

        public CustomerPeriodicDiscountsPage(TabControl owner, TabControl.Tab ownerTab)
            : this()
        {
      
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {            
            return new CustomerPeriodicDiscountsPage((TabControl)sender, tab);
        }
        
        public static void FlowMessage(TabControl sender, TabControl.Tab tab, int hint, object data)
        {
            PriceDiscGroupEnum displayType = (PriceDiscGroupEnum)hint;

            tab.Visible = displayType == PriceDiscGroupEnum.LineDiscountGroup;
            sender.Invalidate();
        }

        public CustomerPeriodicDiscountsPage()
        {
            InitializeComponent();

            selectedID = "";

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;
        }       

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            customerID = context;
            allOffers = Providers.DiscountOfferData.GetPeriodicDiscounts(PluginEntry.DataModel, true, DiscountOfferSorting.OfferNumber, false);

            LoadLines();
        }

        private void LoadLines()
        {
            List<DiscountOffer> offers = Providers.DiscountOfferData.GetForCustomer(PluginEntry.DataModel, DiscountOfferFilter.AllExceptPromotions, customerID);

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
                    IconButtonNumericCell ibtnCell = new IconButtonNumericCell(iconButton,
                                                                 IconButtonCell.IconButtonIconAlignmentEnum.Left,
                                                                 offer.Priority.ToString(), offer.Priority);

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
                Properties.Resources.EditText,
                100,
                EditOfferHandler);
            item.Enabled = btnEdit.Enabled;
            item.Image = ContextButtons.GetEditButtonImage();
            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 200));

            item = new ExtendedMenuItem(
                Properties.Resources.ViewPriorities,
                300,
                ViewPrioritiesHandler);
            item.Enabled = true;

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

            btnEdit.Enabled = lvValues.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.CustomerEdit);
        }

        private void ViewPrioritiesHandler(object sender, EventArgs args)
        {
            PluginOperations.ShowPeriodicDiscountPrioritiesView(selectedID);
        }

        private void lvValues_CellAction(object sender, CellEventArgs args)
        {
            RecordIdentifier selectedDiscountID = ((DiscountOffer)lvValues.Row(args.RowNumber).Tag).ID;

            // The only cell that can fire this event is the icon button cell tied to the priority column
            PluginOperations.ShowPeriodicDiscountPrioritiesView(selectedDiscountID);
        }
    }
}
