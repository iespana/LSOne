using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.Controls.Themes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.CustomerLoyalty.Dialogs;
using ContainerControl = LSOne.Controls.ContainerControl;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.CustomerLoyalty.ViewPages
{
	public partial class CustomerLoyaltyPage : ContainerControl, ITabView
	{
        private RecordIdentifier selectedCardID;
		private Customer customer;
		private List<LoyaltyMSRCard> loyaltyCards;
		private LoyaltyMSRCard loyaltyTotals = new LoyaltyMSRCard();
	    private SiteServiceProfile siteServiceProfile;

		public CustomerLoyaltyPage()
		{
            selectedCardID = RecordIdentifier.Empty;

			InitializeComponent();

            var paramsData = Providers.ParameterData.Get(PluginEntry.DataModel);
            siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, paramsData.SiteServiceProfile);

            gridControl1.Columns[0].Tag = LoyaltyMSRCardSorting.CardNumber;
			gridControl1.Columns[1].Tag = LoyaltyMSRCardSorting.Type;
			gridControl1.SetSortColumn(gridControl1.Columns[0], true);

            gridControl1.ContextMenuStrip = new ContextMenuStrip();
            gridControl1.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

			LoyaltyCustomerParams loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel);
            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.CustomerEdit);

            gridControl1.ApplyTheme(new LSOneMultiSelectTheme());
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new CustomerLoyaltyPage();
		}

		#region ITabPanel Members

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			customer = (Customer)internalContext;
			LoadDataInner();
		}

		private void LoadDataInner()
		{
            List<LoyaltyPoints> tenderRules = new List<LoyaltyPoints>();
		
			var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
			try
			{
                loyaltyCards = service.GetCustomerMSRCards(PluginEntry.DataModel, siteServiceProfile, new List<DataEntity>(){ new DataEntity(customer.ID, "")},null,null, null, null, LoyaltyMSRCardInequality.GreaterThan, -1, -1, -1, (LoyaltyMSRCardSorting)gridControl1.SortColumn.Tag, !gridControl1.SortedAscending, false);
					
				foreach (LoyaltyMSRCard card in loyaltyCards)
				{
                    if (tenderRules.Count(c => c.SchemeID == card.SchemeID) == 0)
                    {
                        LoyaltyPoints rule = service.GetPointsExchangeRate(PluginEntry.DataModel, siteServiceProfile, card.SchemeID, false);
                        if (rule != null)
                        {
                            rule.Points = rule.Points < decimal.Zero ? rule.Points*-1 : rule.Points;
                            tenderRules.Add(rule);
                        }
                    }
				}
                service.Disconnect(PluginEntry.DataModel);
            }
            catch (Exception)
            {
                MessageDialog.Show(Properties.Resources.ErrorConnectingToSiteService, MessageBoxIcon.Error);
                return;
            }

			gridControl1.ClearRows();
			loyaltyTotals.PointStatus = 0;
			loyaltyTotals.IssuedPoints = 0;
			loyaltyTotals.UsedPoints = 0;
			loyaltyTotals.ExpiredPoints = 0;

			var qtyLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
			var priceLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
			if (loyaltyCards != null)
			{
				foreach (var card in loyaltyCards)
				{
					Row row = new Row();
					row.AddText(card.CardNumber);
					row.AddText(card.TenderTypeAsString);
					row.AddText(card.SchemeDescription);

				    decimal currentValue = decimal.Zero;
				        
				    if (card.PointStatus != decimal.Zero)
				    {
				        LoyaltyPoints rule = tenderRules.FirstOrDefault(f => f.SchemeID == card.SchemeID);
				        if (rule != null)
				        {
				            currentValue = (rule.QtyAmountLimit/rule.Points)*card.PointStatus;
				        }
				    }
				    
                    row.AddText(currentValue == decimal.Zero ? "" : currentValue.FormatWithLimits(priceLimit, true));

                    row.AddText(card.IssuedPoints == decimal.Zero ? "" : card.IssuedPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.UsedPoints == decimal.Zero ? "" : card.UsedPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.ExpiredPoints == decimal.Zero ? "" : card.ExpiredPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.PointStatus == decimal.Zero ? "" : card.PointStatus.FormatWithLimits(qtyLimit));
                    row.Tag = card.ID;

					gridControl1.AddRow(row);

                    if (selectedCardID == card.ID)
                    {
                        gridControl1.Selection.Set(gridControl1.RowCount - 1);
                    }

					loyaltyTotals.PointStatus += card.PointStatus;
					loyaltyTotals.IssuedPoints += card.IssuedPoints;
					loyaltyTotals.UsedPoints += card.UsedPoints;
					loyaltyTotals.ExpiredPoints += card.ExpiredPoints;
				}
			}

			tbIssued.Text = loyaltyTotals.IssuedPoints.FormatWithLimits(qtyLimit);
			tbUsed.Text = loyaltyTotals.UsedPoints.FormatWithLimits(qtyLimit);
			tbExpired.Text = loyaltyTotals.ExpiredPoints.FormatWithLimits(qtyLimit);
			tbPointStatus.Text = loyaltyTotals.PointStatus.FormatWithLimits(qtyLimit);

			gridControl1.AutoSizeColumns();
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
		}

		public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
		{
		}

		public void OnClose()
		{
		}

        public void SaveUserInterface()
        {
        }

		#endregion

		private void gridControl1_HeaderClicked(object sender, ColumnEventArgs args)
		{
			gridControl1.SetSortColumn(args.ColumnNumber, !gridControl1.SortedAscending);
			LoadDataInner();
		}

        private void btnsContextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            var dlg = new SelectLoyaltyCardDialog(customer.ID, siteServiceProfile);

            if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
            {
                LoadDataInner();
            }
        }

        private void btnsContextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            bool objectSelected = (gridControl1.Selection.Count != 0) && (gridControl1.Selection.FirstSelectedRow >= 0);
            if (objectSelected)
            {
                var loyaltyParams = Providers.LoyaltyCustomerParamsData.Get(PluginEntry.DataModel);

                var selectedObjectId = (RecordIdentifier)gridControl1.Row(gridControl1.Selection.FirstSelectedRow).Tag;
                var dlg = new LoyaltyCardDialog(selectedObjectId, loyaltyParams, siteServiceProfile);
                if (dlg.ShowDialog(PluginEntry.Framework.MainWindow) == DialogResult.OK)
                {
                    LoadDataInner();
                }
            }
        }

        private void gridControl1_SelectionChanged(object sender, EventArgs e)
        {
            if (gridControl1.Selection.Count > 0)
            {
                selectedCardID = (RecordIdentifier)gridControl1.Row(gridControl1.Selection.FirstSelectedRow).Tag;
            }

            btnsContextButtons.EditButtonEnabled = (gridControl1.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(Permission.CustomerEdit));
        }

        private void gridControl1_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnsContextButtons_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = gridControl1.ContextMenuStrip;
            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit + "...",
                    100,
                    btnsContextButtons_EditButtonClicked)
            {
                Enabled = btnsContextButtons.EditButtonEnabled,
                Image = ContextButtons.GetEditButtonImage(),
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    200,
                    btnsContextButtons_AddButtonClicked)
            {
                Enabled = btnsContextButtons.AddButtonEnabled,
                Image = ContextButtons.GetAddButtonImage()
            };

            menu.Items.Add(item);

            e.Cancel = (menu.Items.Count == 0);
        }
	}
}
