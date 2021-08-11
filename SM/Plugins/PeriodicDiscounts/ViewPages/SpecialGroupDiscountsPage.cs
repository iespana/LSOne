using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.PeriodicDiscounts.ViewPages
{
    public partial class SpecialGroupDiscountsPage : UserControl, ITabView
    {
        WeakReference owner;
        RecordIdentifier groupID;

        public SpecialGroupDiscountsPage(TabControl owner)
            : this()
        {
            this.owner = new WeakReference(owner);
        }

        public SpecialGroupDiscountsPage()
        {
            InitializeComponent();

            lvValues.ContextMenuStrip = new ContextMenuStrip();
            lvValues.ContextMenuStrip.Opening += lvValues_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new SpecialGroupDiscountsPage((TabControl)sender);
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            groupID = context;
            LoadLines();
        }

        public void LoadLines()
        {
            DecimalLimit discountLimiter;
            ListViewItem listItem;

            lvValues.Items.Clear();            
            discountLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

            //Promotion offers
            List<DiscountOffer> offers = Providers.DiscountOfferData.GetOffersAndPromotionsFromRelation(PluginEntry.DataModel, groupID, DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup);

            foreach (DiscountOffer offer in offers)
            {
                var line = offer.OfferType == DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion
                               ? Providers.DiscountOfferLineData.GetPromotion(PluginEntry.DataModel,
                                                            offer.ID,
                                                            groupID,
                                                            DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup)
                               : Providers.DiscountOfferLineData.Get(PluginEntry.DataModel,
                                                           offer.ID,
                                                           groupID,
                                                           DiscountOfferLine.DiscountOfferTypeEnum.SpecialGroup);

                listItem = new ListViewItem(offer.OfferTypeText); //Offer type
                listItem.SubItems.Add(offer.Text);
                listItem.SubItems.Add(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
                listItem.SubItems.Add(offer.ValidationPeriodDescription == "" ? " - " : offer.ValidationPeriodDescription);
                listItem.SubItems.Add(offer.PriceGroupName == "" ? " - " : offer.PriceGroupName);

                string offerTypeText;

                if (offer.OfferType == DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch)
                {
                    offerTypeText = Properties.Resources.Special;
                }
                else if (offer.DiscountType == DiscountOffer.PeriodicDiscountDiscountTypeEnum.UnitPrice)
                {
                    offerTypeText = Properties.Resources.UnitPrice;
                }
                else
                {
                    offerTypeText = line.DiscountPercent.FormatWithLimits(discountLimiter);
                }

                listItem.SubItems.Add(offerTypeText);

                var dict = new Dictionary<string, object>();
                dict["Line"] = line;
                dict["Offer"] = offer;
                listItem.Tag = dict;
                listItem.ImageIndex = -1;

                // Replace zeros with a dash
                for (int i = 1; i < listItem.SubItems.Count; i++)
                {
                    if (listItem.SubItems[i].Text == "0")
                    {
                        listItem.SubItems[i].Text = "-";
                    }
                }

                lvValues.Add(listItem);
            }

            lvValues_SelectedIndexChanged(this, EventArgs.Empty);

            lvValues.BestFitColumns();
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
            contexts.Add(new AuditDescriptor("PromotionLine", RecordIdentifier.Empty, Properties.Resources.PromotionLines, false));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PromotionOffer" || objectName == "DiscountOffer")
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
                    btnEdit_Click)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsContextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btnRemove_Click)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsContextButtons.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            menu.Items.Add(new ExtendedMenuItem("-", 300));

            item = new ExtendedMenuItem(
                    Properties.Resources.ShowOffer + "...",
                    300,
                    ShowOffer)
            {
                Enabled = lvValues.SelectedItems.Count > 0
            };

            menu.Items.Add(item);
            
            PluginEntry.Framework.ContextMenuNotify("DiscountOfferLineList", lvValues.ContextMenuStrip, lvValues);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void ShowOffer(object sender, EventArgs e)
        {
            var lineAndOfferDictionary = (Dictionary<string, object>)lvValues.SelectedItems[0].Tag;
            var offer = (DiscountOffer)lineAndOfferDictionary["Offer"];
            var offerType = offer.OfferType;
            var selectedID = offer.ID;
            switch (offerType)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion:
                    PluginOperations.ShowSpecificPromotionsView(selectedID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    PluginOperations.ShowSpecificDiscountOffersView(selectedID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MultiBuy:
                    PluginOperations.ShowSpecificMultibuyView(selectedID);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                    PluginOperations.ShowSpecificMixAndMatchView(selectedID);
                    break;
            }
        }

        private void lvValues_DoubleClick(object sender, EventArgs e)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(sender, e);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            var lineAndOfferDictionary = (Dictionary<string, object>)lvValues.SelectedItems[0].Tag;
            var offer = (DiscountOffer)lineAndOfferDictionary["Offer"];
            var offerType = offer.OfferType;
            var result = DialogResult.None;
            switch (offerType)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion:
                    result = QuestionDialog.Show(Properties.Resources.DeletePromotionLineQuestion, Properties.Resources.DeletePromotionLine);
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    result = QuestionDialog.Show(Properties.Resources.DeleteDiscountOfferLineQuestion, Properties.Resources.DeleteDiscountOfferLine);
                    break;
            }
            if (result == DialogResult.Yes)
            {
                if (offerType == DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion)
                {
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, ((PromotionOfferLine)lineAndOfferDictionary["Line"]).ID);
                    Providers.DiscountOfferLineData.DeletePromotion(PluginEntry.DataModel, ((PromotionOfferLine)lineAndOfferDictionary["Line"]).ID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PromotionOffer", ((PromotionOfferLine)lineAndOfferDictionary["Line"]).ID, null);
                }
                else
                {
                    Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, ((DiscountOfferLine)lineAndOfferDictionary["Line"]).ID);
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "DiscountOffer", ((DiscountOfferLine)lineAndOfferDictionary["Line"]).ID, null);
                }
                LoadLines();
            }
        }

        private void lvValues_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvValues.Visible = true;
            btnsContextButtons.Visible = true;
            btnShowOffer.Enabled =  btnsContextButtons.EditButtonEnabled = btnsContextButtons.RemoveButtonEnabled = lvValues.SelectedItems.Count > 0 && (lvValues.SelectedItems[0].SubItems[2].Text == Properties.Resources.Disabled);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var dialogResult = DialogResult.None;
            var lineAndOfferDictionary = (Dictionary<string, object>)lvValues.SelectedItems[0].Tag;

            RecordIdentifier offerID;
            RecordIdentifier lineID;
            DiscountOfferLine line;

            var offer = (DiscountOffer)lineAndOfferDictionary["Offer"];
            var offerType = offer.OfferType;

            switch (offerType)
            {
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion:
                    offerID = ((PromotionOfferLine)lineAndOfferDictionary["Line"]).OfferID;
                    lineID = ((PromotionOfferLine)lineAndOfferDictionary["Line"]).ID;
                    Dialogs.PromotionLineDialog editDialog = Dialogs.PromotionLineDialog.CreateForEditing(offerID, lineID);
                    dialogResult = editDialog.ShowDialog();
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.MixMatch:
                    line = (DiscountOfferLine)lineAndOfferDictionary["Line"];
                    Dialogs.MixAndMatchLineDialog mixMatchDialog = new Dialogs.MixAndMatchLineDialog(offer, offer.MixAndMatchDiscountType, line.ID);
                    dialogResult = mixMatchDialog.ShowDialog();
                    break;
                case DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer:
                    line = (DiscountOfferLine)lineAndOfferDictionary["Line"];
                    Dialogs.DiscountOfferLineDialog discountOfferDialog = new Dialogs.DiscountOfferLineDialog(offer, line.ID);
                    dialogResult = discountOfferDialog.ShowDialog();
                    break;
            }

            if (dialogResult == DialogResult.OK)
            {
                if (offerType == DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion)
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PromotionOffer", ((PromotionOfferLine)lineAndOfferDictionary["Line"]).ID, null);
                }
                else
                {
                    PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "DiscountOffer", ((DiscountOfferLine)lineAndOfferDictionary["Line"]).ID, null);
                }
                LoadLines();
            }
        }
    }
}
