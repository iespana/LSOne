using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Services.Interfaces;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.Controls.Rows;
using LSOne.Utilities.DataTypes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.POS.Processes.Common;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.DataLayer.BusinessObjects.Card;
using LSOne.Controls.SupportClasses;
using LSOne.Services.Interfaces.Constants;

namespace LSOne.Services.Panels
{
    public partial class CustomerLoyaltyPanel : UserControl
    {
        private IConnectionManager entry;
        private Customer customer;
        private List<LoyaltyMSRCard> loyaltyCards;
        private LoyaltyMSRCard loyaltyTotals = new LoyaltyMSRCard();
        private SiteServiceProfile siteServiceProfile;
        private WinFormsTouch.EditCustomerDialog parent;
        public CustomerLoyaltyPanel(IConnectionManager entry, Customer customer, WinFormsTouch.EditCustomerDialog parent)
        {
            InitializeComponent();

            this.parent = parent;
            this.entry = entry;
            this.customer = customer;

            Parameters paramsData = Providers.ParameterData.Get(entry);
            siteServiceProfile = (entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication) as ISettings).SiteServiceProfile;
            LoadData();
        }

        public bool loyaltyCardSelected;

        public void LoadData()
        {
            List<LoyaltyPoints> tenderRules = new List<LoyaltyPoints>();

            var service = (ISiteServiceService)entry.Service(ServiceType.SiteServiceService);
            try
            {
                loyaltyCards = service.GetCustomerMSRCards(entry, siteServiceProfile, new List<DataEntity>() { new DataEntity(customer.ID, "") }, null, null, null, null, LoyaltyMSRCardInequality.GreaterThan, -1, -1, -1, LoyaltyMSRCardSorting.CardNumber, false, false);

                foreach (LoyaltyMSRCard card in loyaltyCards)
                {
                    if (tenderRules.Count(c => c.SchemeID == card.SchemeID) == 0)
                    {
                        LoyaltyPoints rule = service.GetPointsExchangeRate(entry, siteServiceProfile, card.SchemeID, false);
                        if (rule != null)
                        {
                            rule.Points = rule.Points < decimal.Zero ? rule.Points * -1 : rule.Points;
                            tenderRules.Add(rule);
                        }
                    }
                }
                service.Disconnect(entry);
            }
            catch (Exception exception)
            {                
                Interfaces.Services.DialogService(entry).ShowMessage(exception is ClientTimeNotSynchronizedException ? exception.Message : Properties.Resources.CouldNotConnectToSiteService, MessageBoxButtons.OK, MessageDialogType.ErrorWarning);
                return;
            }

            lvLoyalty.ClearRows();
            loyaltyTotals.PointStatus = 0;
            loyaltyTotals.IssuedPoints = 0;
            loyaltyTotals.UsedPoints = 0;
            loyaltyTotals.ExpiredPoints = 0;

            var qtyLimit = entry.GetDecimalSetting(DecimalSettingEnum.Quantity);
            var priceLimit = entry.GetDecimalSetting(DecimalSettingEnum.Prices);
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
                            currentValue = (rule.QtyAmountLimit / rule.Points) * card.PointStatus;
                        }
                    }

                    row.AddText(currentValue == decimal.Zero ? "" : currentValue.FormatWithLimits(priceLimit, true));

                    row.AddText(card.IssuedPoints == decimal.Zero ? "" : card.IssuedPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.UsedPoints == decimal.Zero ? "" : card.UsedPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.ExpiredPoints == decimal.Zero ? "" : card.ExpiredPoints.FormatWithLimits(qtyLimit));
                    row.AddText(card.PointStatus == decimal.Zero ? "" : card.PointStatus.FormatWithLimits(qtyLimit));
                    row.Tag = card.ID;

                    lvLoyalty.AddRow(row);

                    loyaltyTotals.PointStatus += card.PointStatus;
                    loyaltyTotals.IssuedPoints += card.IssuedPoints;
                    loyaltyTotals.UsedPoints += card.UsedPoints;
                    loyaltyTotals.ExpiredPoints += card.ExpiredPoints;
                }
            }

            lblIssued.Text = loyaltyTotals.IssuedPoints.FormatWithLimits(qtyLimit);
            lblUsed.Text = loyaltyTotals.UsedPoints.FormatWithLimits(qtyLimit);
            lblExpired.Text = loyaltyTotals.ExpiredPoints.FormatWithLimits(qtyLimit);
            lblBalance.Text = loyaltyTotals.PointStatus.FormatWithLimits(qtyLimit);

            lvLoyalty.AutoSizeColumns();
        }

        public void UseLoyaltyCard(IPosTransaction transaction)
        {
            if (lvLoyalty.Selection.Count == 0)
                return;

            if (transaction == null || !(transaction is RetailTransaction))
            {
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.ThisOperationIsInvalidForThisTypeOfTransaction);
                return;
            }

            CardInfo cardInfo = new CardInfo { CardNumber = ((RecordIdentifier)lvLoyalty.Selection[0].Tag).ToString(), CardType = DataLayer.BusinessObjects.Enums.CardTypesEnum.LoyaltyCard };

            if (Interfaces.Services.LoyaltyService(entry).AddLoyaltyCardToTransaction(entry, cardInfo, (RetailTransaction)transaction))
            {
                POSFormsManager.ShowPOSStatusPanelText(Properties.Resources.LoyaltyCardAddedToSale);
                Interfaces.Services.DialogService(entry).ShowMessage(Properties.Resources.LoyaltyCardAddedToSale, MessageBoxButtons.OK, MessageDialogType.Attention);
            }
        }

        private void lvLoyalty_SelectionChanged(object sender, EventArgs e)
        {
            loyaltyCardSelected = lvLoyalty.Selection.Count > 0 ? true : false;
            parent.UpdateButtonStatus();
        }
    }
}
