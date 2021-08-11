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
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.PeriodicDiscounts.Dialogs;
using LSOne.ViewPlugins.PeriodicDiscounts.Properties;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Views
{
    public partial class PeriodicDiscountPrioritiesView : ViewBase
    {
        private RecordIdentifier selectedID = "";
        private List<DiscountOffer> allOffersPriorityAsc;

        public PeriodicDiscountPrioritiesView(RecordIdentifier selectedID)
            : this()
        {
            this.selectedID = selectedID.ToString();
        }

        public PeriodicDiscountPrioritiesView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;



            lvPeriodicDiscounts.ContextMenuStrip = new ContextMenuStrip();
            lvPeriodicDiscounts.ContextMenuStrip.Opening += lvItems_Opening;
            

            ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.ManageDiscounts);

            HeaderText = Properties.Resources.PeriodicDiscounts;            
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //contexts.Add(new AuditDescriptor("PeriodicDiscounts", RecordIdentifier.Empty, Properties.Resources.ValidationPeriods, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PeriodicDiscounts;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {           
            LoadItems();            
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PeriodicDiscount":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadItems();
                    break;

                case "DiscountOffer":
                    if (changeHint == DataEntityChangeType.Edit)
                    {
                        selectedID = changeIdentifier;
                    }

                    LoadItems();
                    break;
            }
        }
      

        private void EditOfferHandler(object sender, EventArgs e)
        {
            DiscountOffer offer = (DiscountOffer)lvPeriodicDiscounts.Row(lvPeriodicDiscounts.Selection.FirstSelectedRow).Tag;

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

        private void EditPriority(object sender, EventArgs e)
        {
            EditPriorityDialog dlg = new EditPriorityDialog(selectedID);
            dlg.ShowDialog();
        }


        void lvItems_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvPeriodicDiscounts.ContextMenuStrip;

            menu.Items.Clear();
            
            item = new ExtendedMenuItem(
                    Properties.Resources.EditText,
                    100,
                    EditPriority);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnEdit.Enabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 400);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.EditDiscount,
                    500,
                    EditOfferHandler);
            item.Enabled = btnEdit.Enabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem("-", 550);
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Resources.MovePriorityUp,
                600,
                btnMoveUp_Click);
                    item.Enabled = btnMoveUp.Enabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Resources.MovePriorityDown,
                700,
                btnMoveDown_Click);
                    item.Enabled = btnMoveDown.Enabled;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PeriodicDiscounts", lvPeriodicDiscounts.ContextMenuStrip, lvPeriodicDiscounts);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadItems()
        {
            lvPeriodicDiscounts.ClearRows();

            // Order by priority to make the up and down buttons easier to implement
            allOffersPriorityAsc = Providers.DiscountOfferData.GetPeriodicDiscounts(PluginEntry.DataModel,
                                                                          true,
                                                                          DiscountOfferSorting.Priority,
                                                                          false);


            foreach (DiscountOffer offer in allOffersPriorityAsc)
            {
                Row row = new Row();

                row.AddText((string) offer.ID);
                row.AddText(offer.Text);

                if (allOffersPriorityAsc.Any(p => p.ID != offer.ID && p.Priority == offer.Priority))
                {
                    //row.AddCell(new ExtendedCell(offer.Priority.ToString(), Properties.Resources.Warning16));
                    row.AddCell(new IconToolTipCell(offer.Priority.ToString(), Properties.Resources.Warning16,
                                                    Properties.Resources.PriorityAlreadyInUse));
                }
                else
                {
                    row.AddCell(new NumericCell(offer.Priority.ToString(), offer.Priority));
                }

                switch (offer.OfferType)
                {
                    case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                        row.AddText(Properties.Resources.Multibuy);
                        break;
                    case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                        row.AddText(Properties.Resources.MixAndMatch);
                        break;
                    case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                        row.AddText(Properties.Resources.DiscountOffer);
                        break;
                    case DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion:
                        row.AddText(Properties.Resources.PromotionOffer);
                        break;
                    case DiscountOffer.PeriodicDiscountOfferTypeEnum.All:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                row.AddText(offer.TriggeringText);

                row.AddText(offer.Triggering == DiscountOffer.TriggeringEnum.Manual ? offer.BarCode : "");

                row.Tag = offer;

                lvPeriodicDiscounts.AddRow(row);

                if (offer.ID == selectedID)
                {
                    lvPeriodicDiscounts.Selection.Set(lvPeriodicDiscounts.RowCount - 1);
                }
            }

            lvPeriodicDiscounts.AutoSizeColumns();
            
            lvPeriodicDiscounts.ShowRowOnScreen = true;
            
        
        }


        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType() + ".Related")
            {
                arguments.Add(new ContextBarItem(Properties.Resources.DiscountOffers, null, PluginOperations.ShowDiscountOffersView), 100);
                arguments.Add(new ContextBarItem(Properties.Resources.Multibuy, null, PluginOperations.ShowMultibuyView), 200);
                arguments.Add(new ContextBarItem(Properties.Resources.MixAndMatch, null, PluginOperations.ShowMixAndMatchView), 300);
            }
        }

        private void lvPeriodicDiscounts_SelectionChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = btnEditDiscount.Enabled = btnMoveUp.Enabled = btnMoveDown.Enabled = lvPeriodicDiscounts.Selection.Count > 0;
            
            if (lvPeriodicDiscounts.Selection.Count > 0)
            {
                btnMoveUp.Enabled = ((DiscountOffer)lvPeriodicDiscounts.Row(lvPeriodicDiscounts.Selection.FirstSelectedRow).Tag).Priority > 0;
                selectedID = ((DiscountOffer)lvPeriodicDiscounts.Row(lvPeriodicDiscounts.Selection.FirstSelectedRow).Tag).ID;
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            DiscountOffer currentOffer = allOffersPriorityAsc.First(p => p.ID == selectedID);
            int currentPriorityIndex = allOffersPriorityAsc.IndexOf(currentOffer);

            int priority = currentPriorityIndex == 0 ? currentOffer.Priority : allOffersPriorityAsc[currentPriorityIndex - 1].Priority;

            // Calculate the next available priority
            while (allOffersPriorityAsc.Exists(p => p.Priority == priority) && priority >= 0)
            {
                priority--;
            }

            if (priority < 0)
            {
                priority = 0;
            }

            if (allOffersPriorityAsc.Exists(p => p.Priority == priority))
            {
                return;
            }

            if (priority != currentOffer.Priority)
            {
                currentOffer.Priority = priority;
                Providers.DiscountOfferData.Save(PluginEntry.DataModel, currentOffer);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PeriodicDiscount", currentOffer.ID, currentOffer.OfferType);
                LoadData(false);
            }
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            DiscountOffer currentOffer = (DiscountOffer)lvPeriodicDiscounts.Row(lvPeriodicDiscounts.Selection.FirstSelectedRow).Tag;            
            int currentPriorityIndex = allOffersPriorityAsc.IndexOf(currentOffer);

            int priority = currentPriorityIndex == allOffersPriorityAsc.Count - 1 ? currentOffer.Priority : allOffersPriorityAsc[currentPriorityIndex + 1].Priority;
            

            // Calculate the next available priority
            while (allOffersPriorityAsc.Exists(p => p.Priority == priority))
            {
                priority++;
            }

            if (priority != currentOffer.Priority)
            {
                currentOffer.Priority = priority;
                Providers.DiscountOfferData.Save(PluginEntry.DataModel, currentOffer);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PeriodicDiscount", currentOffer.ID, currentOffer.OfferType);

                LoadData(false);
            }
        }

        private void lvPeriodicDiscounts_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnEdit.Enabled)
            {
                EditPriority(this, EventArgs.Empty);
            }
        }

        private void btnEditDiscount_Click(object sender, EventArgs e)
        {
            EditOfferHandler(sender, e);
        }
    }
}
