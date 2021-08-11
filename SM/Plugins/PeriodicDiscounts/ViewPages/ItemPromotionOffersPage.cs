using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.PeriodicDiscounts.ViewPages
{
	public partial class ItemPromotionOffersPage : UserControl, ITabView
	{
		WeakReference owner;
		RecordIdentifier itemId;
		public ItemPromotionOffersPage(TabControl owner)
			: this()
		{
			this.owner = new WeakReference(owner);
		}

		public ItemPromotionOffersPage()
		{
			InitializeComponent();

			lvValues.ContextMenuStrip = new ContextMenuStrip();
			lvValues.ContextMenuStrip.Opening += lvValues_Opening;
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new ItemPromotionOffersPage((TabControl)sender);
		}

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			itemId = context;
			LoadLines();
		}

		public void LoadLines()
		{
			DecimalLimit priceLimiter;
			DecimalLimit discountLimiter;
			decimal priceIncludeTax;
			decimal discountAmount;
			decimal discountAmountIncludeTax;

			RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemId);

			List<PromotionOfferLine> lines = Providers.DiscountOfferLineData.GetPromotionsForItem(
					PluginEntry.DataModel,
					itemId,
					RecordIdentifier.Empty,
					RecordIdentifier.Empty,
					false,
					false);

			lvValues.ClearRows();

			priceLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
			discountLimiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);

			Row row;
			foreach (PromotionOfferLine line in lines)
			{
				row = new Row();

				DiscountOffer offer = Providers.DiscountOfferData.GetOfferFromLine(PluginEntry.DataModel, line.OfferID);
				row.AddText(offer.Text);
				row.AddText(offer.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled);
				row.AddText(line.TypeText);

				row.AddText(item.VariantName);
				row.AddCell(new NumericCell(line.OfferPrice.FormatWithLimits(priceLimiter), line.OfferPrice));
				row.AddCell(new NumericCell(line.OfferPriceIncludeTax.FormatWithLimits(priceLimiter), line.OfferPriceIncludeTax));
				row.AddCell(new NumericCell(line.DiscountPercent.FormatWithLimits(discountLimiter), line.DiscountPercent));

				if (line.Type == DiscountOfferLine.DiscountOfferTypeEnum.Variant || line.Type == DiscountOfferLine.DiscountOfferTypeEnum.Item)
				{
					row.AddCell(new NumericCell(line.DiscountAmount.FormatWithLimits(priceLimiter), line.DiscountAmount));
					row.AddCell(new NumericCell(line.DiscountamountIncludeTax.FormatWithLimits(priceLimiter), line.DiscountamountIncludeTax));
				}
				else
				{
					discountAmount = item.SalesPrice * (line.DiscountPercent / 100.0M);
					priceIncludeTax = item.SalesPrice + Services.Interfaces.Services.TaxService(PluginEntry.DataModel).GetItemTax(PluginEntry.DataModel, item);
					discountAmountIncludeTax = priceIncludeTax * (line.DiscountPercent / 100.0M);
					row.AddCell(new NumericCell(discountAmount.FormatWithLimits(priceLimiter), discountAmount));
					row.AddCell(new NumericCell(discountAmountIncludeTax.FormatWithLimits(priceLimiter), discountAmountIncludeTax));
				}
				row.Tag = line;

				lvValues.AddRow(row);
			}
			lvValues_SelectionChanged(this, EventArgs.Empty);
			lvValues.AutoSizeColumns();
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
			if (objectName == "PromotionOffer")
			{
				LoadLines();
			}
		}

		public void OnClose()
		{
			//no customization needed
		}

		public void SaveUserInterface()
		{
			//no customization needed
		}

		void lvValues_Opening(object sender, CancelEventArgs e)
		{
			ContextMenuStrip menu = lvValues.ContextMenuStrip;

			menu.Items.Clear();

			// We can optionally add our own items right here
			ExtendedMenuItem item = new ExtendedMenuItem(
					Properties.Resources.EditText + "...",
					100,
					new EventHandler(btnEdit_Click))
			{
				Image = ContextButtons.GetEditButtonImage(),
				Enabled = btnsContextButtonsItems.EditButtonEnabled,
				Default = true
			};

			menu.Items.Add(item);


			item = new ExtendedMenuItem(
					Properties.Resources.Delete + "...",
					300,
					new EventHandler(btnRemove_Click))
			{
				Image = ContextButtons.GetRemoveButtonImage(),
				Enabled = btnsContextButtonsItems.RemoveButtonEnabled
			};

			menu.Items.Add(item);

			menu.Items.Add(new ExtendedMenuItem("-", 300));

			item = new ExtendedMenuItem(
					Properties.Resources.ShowOffer + "...",
					300,
					new EventHandler(showOffer))
			{
				Enabled = btnShowOffer.Enabled
			};

			menu.Items.Add(item);

			PluginEntry.Framework.ContextMenuNotify("DiscountOfferLineList", lvValues.ContextMenuStrip, lvValues);

			e.Cancel = (menu.Items.Count == 0);
		}

		private void showOffer(object sender, EventArgs e)
		{
			RecordIdentifier selectedID = ((PromotionOfferLine)lvValues.Selection[0].Tag).OfferID;
			PluginOperations.ShowSpecificPromotionsView(selectedID);
		}

		private void btnEdit_Click(object sender, EventArgs e)
		{
			RecordIdentifier offerID = ((PromotionOfferLine)lvValues.Selection[0].Tag).OfferID;
			RecordIdentifier lineID = ((PromotionOfferLine)lvValues.Selection[0].Tag).ID;

			Dialogs.PromotionLineDialog editDialog = Dialogs.PromotionLineDialog.CreateForEditing(offerID, lineID);
			DialogResult result = editDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PromotionOffer", ((PromotionOfferLine)lvValues.Selection[0].Tag).ID, null);
				LoadLines();
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			if (QuestionDialog.Show(
			   Properties.Resources.DeletePromotionLineQuestion,
			   Properties.Resources.DeletePromotionLine) == DialogResult.Yes)
			{
				Providers.DiscountOfferLineData.Delete(PluginEntry.DataModel, ((PromotionOfferLine)lvValues.Selection[0].Tag).ID);
				Providers.DiscountOfferLineData.DeletePromotion(PluginEntry.DataModel, ((PromotionOfferLine)lvValues.Selection[0].Tag).ID);
				PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Delete, "PromotionOffer", ((PromotionOfferLine)lvValues.Selection[0].Tag).ID, null);

				LoadLines();
			}
		}

		private void lvValues_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
		{
			if (btnsContextButtonsItems.EditButtonEnabled)
			{
				btnEdit_Click(this, EventArgs.Empty);
			}
		}

		private void lvValues_SelectionChanged(object sender, EventArgs e)
		{
			btnsContextButtonsItems.EditButtonEnabled = btnsContextButtonsItems.RemoveButtonEnabled = btnShowOffer.Enabled = false;
			if (lvValues.Selection.Count > 0)
			{
				btnsContextButtonsItems.EditButtonEnabled = btnsContextButtonsItems.RemoveButtonEnabled = !((PromotionOfferLine)lvValues.Selection[0].Tag).InActiveDiscount;
				btnShowOffer.Enabled = true;
			}
		}
	}
}
