using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.ViewPages
{
    public partial class ItemPeriodicDiscountsPage : UserControl, ITabView
    {
        WeakReference viewReference;
        WeakReference owner;
        RetailItem item;

        public ItemPeriodicDiscountsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            var tabControl = (TabControl) sender;
            var page =  new ViewPages.ItemPeriodicDiscountsPage(tabControl);
            var view = tabControl.Parent;
            while (view != null && !(view is ViewBase))
                view = view.Parent;
            if (view != null)
                page.viewReference = new WeakReference(view);
            return page;
        }

        public ItemPeriodicDiscountsPage()
        {
            InitializeComponent();

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            item = (RetailItem)((Dictionary<string,object>)internalContext)["Item"];

            LoadLines();
        }

        private void LoadLines()
        {
            var view = viewReference == null ? null : viewReference.Target as ViewBase;
            if (view != null)
            {
                view.ShowProgress((sender1, e1) => LoadLinesInProgress(true));
            }
            else
            {
                LoadLinesInProgress(false);
            }
        }

        private void LoadLinesInProgress(bool inProgress)
        {
            try
            {
                Row row;
                lvValues.ClearRows();

                var priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
                var discountLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

                //Load discount offers
                var lines = Providers.DiscountOfferLineData.GetDiscountOfferLinesForItem(PluginEntry.DataModel, item.ID);
                foreach (var line in lines)
                {                    
                    DiscountOffer offer = Providers.DiscountOfferData.GetOfferFromLine(PluginEntry.DataModel, line.OfferID);
                    row = new Row();
                    row.AddText(OfferTypeName(offer.OfferType));
                    row.AddText(offer.Text);                    
                    row.AddCell(new NumericCell(offer.Priority.ToString(), offer.Priority));

                    row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
                    row.AddText(line.TypeText);

                    row.AddText((string)line.ItemRelation);
                    row.AddText(line.Text);
                    row.AddText(line.VariantName);

                    decimal offerPrice = item.SalesPrice -
                                         (item.SalesPrice*(line.DiscountPercent/100.0M));
                    decimal priceIncludeTax = item.SalesPrice +
                                              Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                                                  .GetItemTax(PluginEntry.DataModel, item);

                    decimal taxAmount =
                        Services.Interfaces.Services.TaxService(PluginEntry.DataModel)
                            .GetItemTaxForAmount(PluginEntry.DataModel, item.SalesTaxItemGroupName, offerPrice);

                    decimal offerPriceWithTax = offerPrice + taxAmount;

                    decimal discountAmountWithTax = priceIncludeTax - offerPriceWithTax;

                    row.AddCell(new NumericCell(line.DiscountPercent.FormatWithLimits(discountLimiter), line.DiscountPercent));
                    row.AddCell(new NumericCell(offerPriceWithTax.FormatWithLimits(priceLimiter), offerPriceWithTax));
                    row.AddCell(new NumericCell(discountAmountWithTax.FormatWithLimits(priceLimiter), discountAmountWithTax));

                    var dict = new Dictionary<string, object>();
                    dict["Line"] = line;
                    dict["Offer"] = offer;
                    row.Tag = dict;

                    lvValues.AddRow(row);
                }

                //Load multibuy
                lines = Providers.DiscountOfferLineData.GetMultiBuyLinesForItem(PluginEntry.DataModel, item.ID);
                foreach (var line in lines)
                {
                    row = new Row();
                    var offer = Providers.DiscountOfferData.GetOfferFromLine(PluginEntry.DataModel, line.OfferID);
                    row.AddText(OfferTypeName(offer.OfferType));
                    row.AddText(offer.Text);
                    row.AddCell(new NumericCell(offer.Priority.ToString(), offer.Priority));

                    row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
                    row.AddText(line.TypeText);

                    row.AddText((string)line.ItemRelation);
                    row.AddText(line.Text);
                    row.AddText(line.VariantName);

                    row.AddText(Properties.Resources.Special);

                    row.AddText("-");
                    row.AddText("-");

                    var dict = new Dictionary<string, object>();
                    dict["Line"] = line;
                    dict["Offer"] = offer;
                    row.Tag = dict;

                    lvValues.AddRow(row);
                }

                //Load mix and match
                lines = Providers.DiscountOfferLineData.GetMixMatchLinesForItem(PluginEntry.DataModel, item.ID);
                foreach (var line in lines)
                {
                    row = new Row();
                    var offer = Providers.DiscountOfferData.GetOfferFromLine(PluginEntry.DataModel, line.OfferID);
                    row.AddText(OfferTypeName(offer.OfferType));
                    row.AddText(offer.Text);
                    row.AddCell(new NumericCell(offer.Priority.ToString(), offer.Priority));

                    row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
                    row.AddText(line.TypeText);

                    row.AddText((string)line.ItemRelation);
                    row.AddText(line.Text);
                    row.AddText(line.VariantName);

                    row.AddText(Properties.Resources.Special);

                    row.AddText("-");
                    row.AddText("-");

                    var dict = new Dictionary<string, object>();
                    dict["Line"] = line;
                    dict["Offer"] = offer;
                    row.Tag = dict;

                    lvValues.AddRow(row);
                }

                lvValues.AutoSizeColumns();
            }
            finally
            {
                if (inProgress)
                {
                    var view = viewReference.Target as ViewBase;
                    view.HideProgress();
                }

                lvValues_SelectionChanged(this, EventArgs.Empty);
            }
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
            return item.Dirty;
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
            if (objectName == "DiscountOffer")
            {
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
            var menu = lvValues.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditText + "...",
                    100,
                    btnEditValue_Click)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtonsItems.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btnRemoveValue_Click)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsContextButtonsItems.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 300));

            item = new ExtendedMenuItem(
                    Properties.Resources.ShowOffer + "...",
                    300,
                    showOffer)
            {
                Enabled = btnShowOffer.Enabled
            };

            menu.Items.Add(item);
            
            PluginEntry.Framework.ContextMenuNotify("DiscountOfferLineList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void showOffer(object sender, EventArgs e)
        {
            var lineAndOfferDictionary = (Dictionary<string, object>)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag;
            var offer = (DiscountOffer)lineAndOfferDictionary["Offer"];
            var offerType = offer.OfferType;
            var selectedID = offer.ID;
            switch (offerType)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion:
                    PluginOperations.ShowSpecificPromotionsView(selectedID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                    PluginOperations.ShowSpecificMixAndMatchView(selectedID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    PluginOperations.ShowSpecificDiscountOffersView(selectedID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                    PluginOperations.ShowSpecificMultibuyView(selectedID);
                    break;
            }
        }
        
        private void btnEditValue_Click(object sender, EventArgs e)
        {
            var dialogResult = DialogResult.None;
            var lineAndOfferDictionary = (Dictionary<string,object>)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag;
            var offer = (DiscountOffer)lineAndOfferDictionary["Offer"];
            DiscountOfferLine line;
            var offerType = offer.OfferType;
                
            switch (offerType)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                    line = (DiscountOfferLine)lineAndOfferDictionary["Line"];
                    var multiBuyDialog = new Dialogs.MultibuyLineDialog(offer.ID, line.ID);
                    dialogResult = multiBuyDialog.ShowDialog();
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                    line = (DiscountOfferLine)lineAndOfferDictionary["Line"];
                    var mixMatchDialog = new Dialogs.MixAndMatchLineDialog(offer, offer.MixAndMatchDiscountType, line.ID);
                    dialogResult = mixMatchDialog.ShowDialog();
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    line = (DiscountOfferLine)lineAndOfferDictionary["Line"];
                    var discountOfferDialog = new Dialogs.DiscountOfferLineDialog(offer, line.ID);
                    dialogResult = discountOfferDialog.ShowDialog();
                    break;
            }
                
            if (dialogResult == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "DiscountOffer", (RecordIdentifier)((DiscountOfferLine)lineAndOfferDictionary["Line"]).ID, null);
                LoadLines();
	        }
        }

        private void btnRemoveValue_Click(object sender, EventArgs e)
        {
            var lineAndOfferDictionary = (Dictionary<string, object>)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag;
            var offer = (DiscountOffer)lineAndOfferDictionary["Offer"];
            var offerType = offer.OfferType;
            var result = DialogResult.None;

            switch (offerType)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                    result = QuestionDialog.Show(Properties.Resources.DeleteMultibuyLineQuestion, Properties.Resources.DeleteMultibuyLine);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    result = QuestionDialog.Show(Properties.Resources.DeleteDiscountOfferLineQuestion, Properties.Resources.DeleteDiscountOfferLine);
                    break;
            }
            if (result == DialogResult.Yes)
            {
                Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, ((DiscountOfferLine)lineAndOfferDictionary["Line"]).ID);
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "DiscountOffer", (RecordIdentifier)((DiscountOfferLine)lineAndOfferDictionary["Line"]).ID, null);

                LoadLines();
            }
        }

        private void lvValues_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsContextButtonsItems.EditButtonEnabled)
            {
                btnEditValue_Click(this, EventArgs.Empty);
            }
        }

        private void lvValues_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtonsItems.EditButtonEnabled = btnsContextButtonsItems.RemoveButtonEnabled = btnShowOffer.Enabled = false;

            if (lvValues.Selection.Count > 0)
            {
                var offer = ((DiscountOffer)((Dictionary<string, object>)lvValues.Row(lvValues.Selection.FirstSelectedRow).Tag)["Offer"]);
                bool status = !offer.Enabled;
                var type = (PeriodicDiscOfferType)offer.OfferType;
                btnsContextButtonsItems.RemoveButtonEnabled = status;
                btnsContextButtonsItems.EditButtonEnabled = status && (type != PeriodicDiscOfferType.MixAndMatch);
                btnShowOffer.Enabled = true;
            }
        }
    }
}
