using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Exceptions;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CustomerLoyalty.Dialogs
{
	public partial class LoyaltySchemeRuleDialog : DialogBase
	{
		private RecordIdentifier objectId;
        private RecordIdentifier lineId;
		private int type = 0;
        private DialogResult dialogResult = DialogResult.Cancel;
	    private LoyaltySchemes scheme;

		private DecimalLimit qtyLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
        private DecimalLimit priceLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);

	    private SiteServiceProfile siteServiceProfile;

        public LoyaltyPoints LoyalityPoints { get; set; }

		// When editing
        public LoyaltySchemeRuleDialog(RecordIdentifier objectId, RecordIdentifier lineId, LoyaltyCustomerParams loyaltyParams, SiteServiceProfile siteServiceProfile)
            : this(objectId, loyaltyParams, siteServiceProfile)
		{
			this.objectId = objectId;
			this.lineId = lineId;

            var service =(ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            LoyalityPoints = service.GetLoyaltySchemeRule(PluginEntry.DataModel, siteServiceProfile, new RecordIdentifier(objectId, lineId), true);

			//cmbType.SelectedIndex = (int)LoyalityPoints.Type;
			switch (LoyalityPoints.Type)
			{
				case LoyaltyPointTypeBase.Item:
			        cmbType.SelectedIndex = (int) LoyalityPoints.Type;
			        break;
				case LoyaltyPointTypeBase.RetailGroup:
                    cmbType.SelectedIndex = (int) LoyalityPoints.Type;
			        break;
				case LoyaltyPointTypeBase.RetailDepartment:
                    cmbType.SelectedIndex = (int) LoyalityPoints.Type;
			        break;
				case LoyaltyPointTypeBase.Promotion:
				case LoyaltyPointTypeBase.Discount:
				case LoyaltyPointTypeBase.Tender:
                    // We need to have the Tender number 3 at the moment because the two above currently do not work and are hidden
                    cmbType.SelectedIndex = 3;
			        break;
				case LoyaltyPointTypeBase.SpecialGroup:
					
					break;
				default:
					cmbRelation.SelectedData = new DataEntity(null, null);
					cmbRelation_SelectedDataChanged(null, EventArgs.Empty);
					break;
			}

            cmbRelation.SelectedData = new DataEntity(LoyalityPoints.SchemeRelation, LoyalityPoints.SchemeRelationName);
            cmbRelation_SelectedDataChanged(null, EventArgs.Empty);

			dtpStartingDate.Checked = LoyalityPoints.StartingDate != Date.Empty;
			dtpStartingDate.Value = LoyalityPoints.StartingDate.DateTime;

			dtpEndingDate.Checked = LoyalityPoints.EndingDate != Date.Empty;
			dtpEndingDate.Value = LoyalityPoints.EndingDate.DateTime;
            
			cmbMultType.SelectedIndex = (int)LoyalityPoints.BaseCalculationOn;
            ntbQtyAmt.Value = (double)LoyalityPoints.QtyAmountLimit;
            ntbLoyPts.Value = Math.Abs((double) LoyalityPoints.Points);
			CheckEnabled(this, EventArgs.Empty);
            CheckErrorProvider();
		}

		// When adding
        public LoyaltySchemeRuleDialog(RecordIdentifier objectId, LoyaltyCustomerParams loyaltyParams, SiteServiceProfile siteServiceProfile)
		{
			InitializeComponent();

			this.objectId = objectId;

            this.siteServiceProfile = siteServiceProfile;

			var names = LoyaltyPoints.LoyaltyPointTypeBaseNames();
			cmbType.Items.Clear();
			foreach (string name in names)
			{
			    cmbType.Items.Add(name);
			}

		    names = LoyaltyPoints.CalculationTypeBaseNames();
			cmbMultType.Items.Clear();
			foreach (string name in names)
			{
			    cmbMultType.Items.Add(name);
			}

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            scheme = service.GetLoyaltyScheme(PluginEntry.DataModel, siteServiceProfile, objectId, true);

		    SetDefaults();

			qtyLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);
			priceLimit = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
			RefreshQtyAmtDecimalPlaces();
			ntbLoyPts.DecimalLetters = priceLimit.Max;

			CheckEnabled(this, EventArgs.Empty);
		}

		private void RefreshQtyAmtDecimalPlaces()
		{
		    decimal orgQtyAmtValue = (decimal)ntbQtyAmt.Value;
			switch (cmbMultType.SelectedIndex)
			{
				case (int)CalculationTypeBase.Amounts:
					ntbQtyAmt.DecimalLetters = priceLimit.Max;
					break;
				case (int)CalculationTypeBase.Quantity:
					ntbQtyAmt.DecimalLetters = qtyLimit.Max;
					break;
				default:
					ntbQtyAmt.DecimalLetters = qtyLimit.Max > priceLimit.Max ? qtyLimit.Max : priceLimit.Max;
					break;
			}
		    ntbQtyAmt.Value = (double) orgQtyAmtValue;
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void SetDefaults()
		{
			lineId = RecordIdentifier.Empty;
			LoyalityPoints = null;
			type = 0;
			cmbRelation.SelectedData = new DataEntity("", "");
			dtpStartingDate.Checked = false;
			dtpEndingDate.Checked = false;
			ntbQtyAmt.Value = 0;
            cmbMultType.SelectedIndex = (int)scheme.CalculationType;
			ntbLoyPts.Value = 0;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (Save())
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private bool Save()
		{
			if (LoyalityPoints == null)
			{
				LoyalityPoints = new LoyaltyPoints {SchemeID = objectId};
			}

			switch (type)
			{
				case (int)LoyaltyPointTypeBase.Item:
				case (int)LoyaltyPointTypeBase.RetailGroup:
				case (int)LoyaltyPointTypeBase.RetailDepartment:
				case (int)LoyaltyPointTypeBase.Promotion:
				case (int)LoyaltyPointTypeBase.Discount:
				case (int)LoyaltyPointTypeBase.Tender:
				case (int)LoyaltyPointTypeBase.SpecialGroup:
                    LoyalityPoints.Type = (LoyaltyPointTypeBase)type;
					LoyalityPoints.SchemeRelation = cmbRelation.SelectedData.ID;
					LoyalityPoints.Text = cmbRelation.SelectedData.Text;
					break;

				default:
					LoyalityPoints.Type = LoyaltyPointTypeBase.Item;
					LoyalityPoints.SchemeRelation = RecordIdentifier.Empty;
					LoyalityPoints.Text = String.Empty;
					break;
			}

			LoyalityPoints.StartingDate = new Date(!dtpStartingDate.Checked, dtpStartingDate.Value).GetDateWithoutTime();
			LoyalityPoints.EndingDate = new Date(!dtpEndingDate.Checked, dtpEndingDate.Value).GetDateWithoutTime();
			LoyalityPoints.QtyAmountLimit = (decimal)ntbQtyAmt.Value;
			LoyalityPoints.BaseCalculationOn = cmbMultType.SelectedIndex > 0 ? (CalculationTypeBase)cmbMultType.SelectedIndex : 0;
			LoyalityPoints.Points = (decimal)ntbLoyPts.Value;

			if ((LoyalityPoints.StartingDate > LoyalityPoints.EndingDate) && (LoyalityPoints.EndingDate != Date.Empty))
			{
				MessageDialog.Show(Properties.Resources.StartingDateCannotBeBigger);
				return false;
			}

		  
            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            service.SaveLoyaltySchemeRule(PluginEntry.DataModel, siteServiceProfile, LoyalityPoints, true);

			return true;
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (dialogResult == DialogResult.OK)
			{
				DialogResult = DialogResult.OK;
			}
			else
			{
				DialogResult = DialogResult.Cancel;
			}

			Close();
		}

		// Check for the Ok btn being enabled
		private void CheckEnabled(object sender, EventArgs e)
		{
            errorProvider1.Clear();

            bool enabled = true;
			enabled = enabled && (objectId != null) && ((string)objectId != "");
			enabled = enabled && (type >= 0);
			switch (type)
			{
				case (int)LoyaltyPointTypeBase.Item:
				case (int)LoyaltyPointTypeBase.RetailGroup:
				case (int)LoyaltyPointTypeBase.RetailDepartment:
				case (int)LoyaltyPointTypeBase.Promotion:
				case (int)LoyaltyPointTypeBase.Discount:
				case (int)LoyaltyPointTypeBase.Tender:
				case (int)LoyaltyPointTypeBase.SpecialGroup:
					enabled = enabled && ((string)cmbRelation.SelectedData.ID != "");
					break;
			} //switch(cmbType)
			enabled = enabled && dtpStartingDate.Checked;
			enabled = enabled && (ntbQtyAmt.Value != 0);
			enabled = enabled && (cmbMultType.SelectedIndex >= 0);
			enabled = enabled && (ntbLoyPts.Value != 0);

            if (LoyalityPoints != null)
            {
                enabled = enabled && 
                    ((int) LoyalityPoints.Type != cmbType.SelectedIndex ||
                    LoyalityPoints.StartingDate != new Date(!dtpStartingDate.Checked, dtpStartingDate.Value).GetDateWithoutTime() ||
                    LoyalityPoints.EndingDate != new Date(!dtpEndingDate.Checked, dtpEndingDate.Value).GetDateWithoutTime() ||
                    LoyalityPoints.QtyAmountLimit != (decimal)ntbQtyAmt.Value ||
                    (int)LoyalityPoints.BaseCalculationOn != ((cmbMultType.SelectedIndex > 0) ? cmbMultType.SelectedIndex : 0) ||
                    LoyalityPoints.Points != (decimal)ntbLoyPts.Value ||
                    LoyalityPoints.SchemeRelation != cmbRelation.SelectedData.ID
                    );
            }

			btnOK.Enabled = enabled;
			btnCreateAnother.Enabled = enabled;
		}

		private void cmbRelation_RequestData(object sender, EventArgs e)
		{
			switch (type)
			{
				case (int)LoyaltyPointTypeBase.RetailGroup:
					cmbRelation.SetData(Providers.RetailGroupData.GetList(PluginEntry.DataModel), null);
					break;

				case (int)LoyaltyPointTypeBase.RetailDepartment:
					cmbRelation.SetData(Providers.RetailDepartmentData.GetList(PluginEntry.DataModel), null);
					break;

				case (int)LoyaltyPointTypeBase.Promotion:
					cmbRelation.SetData(Providers.DiscountOfferData.GetOffers(PluginEntry.DataModel, DiscountOffer.PeriodicDiscountOfferTypeEnum.Promotion, 0, false), null);
					break;

				case (int)LoyaltyPointTypeBase.Discount:
					cmbRelation.SetData(Providers.DiscountOfferData.GetOffers(PluginEntry.DataModel, DiscountOffer.PeriodicDiscountOfferTypeEnum.Offer, 0, false), null);
					break;

				case (int)LoyaltyPointTypeBase.Tender:
					cmbRelation.SetData(Providers.PaymentMethodData.GetList(PluginEntry.DataModel), null);
					break;

				case (int)LoyaltyPointTypeBase.SpecialGroup:
					cmbRelation.SetData(Providers.SpecialGroupData.GetList(PluginEntry.DataModel), null);
					break;
			}
		}

		private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
		{
			if (cmbRelation.SelectedData.ID != "")
			{
				switch (type)
				{
					case (int)LoyaltyPointTypeBase.Item:
						RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedData.ID);
						if (retailItem == null)
						{
							throw new DataIntegrityException(typeof(RetailItem), cmbRelation.SelectedData.ID);
						}
						break;
					case (int)LoyaltyPointTypeBase.RetailGroup:
					case (int)LoyaltyPointTypeBase.RetailDepartment:
					case (int)LoyaltyPointTypeBase.Promotion:
					case (int)LoyaltyPointTypeBase.Discount:
					case (int)LoyaltyPointTypeBase.Tender:
					case (int)LoyaltyPointTypeBase.SpecialGroup:
						break;
					default:
						break;
				} //switch
			}
			CheckEnabled(this, EventArgs.Empty);
		}

		private void btnCreateAnother_Click(object sender, EventArgs e)
		{
			if (Save())
			{
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, 
                    "LoyaltySchemeRule", LoyalityPoints.ID, LoyalityPoints);
				dialogResult = DialogResult.OK;
				SetDefaults();
				cmbType.Focus();
			}
		}

		private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
		{
			cmbRelation.SelectedData = new DataEntity("", "");

            if (cmbType.SelectedIndex < 3)
		    {
                type = cmbType.SelectedIndex;
		        cmbMultType.Enabled = true;
		    }
            else if (cmbType.SelectedIndex == 3)
            {
                type = 5;
                cmbMultType.SelectedIndex = 0;
                cmbMultType.Enabled = false;
            }
            
			cmbRelation.Enabled = type >= 0;
			cmbRelation_SelectedDataChanged(this, EventArgs.Empty);
			cmbRelation.ShowDropDownOnTyping = true;
			CheckEnabled(this, EventArgs.Empty);
		}

		private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
		{
			if (type == (int)LoyaltyPointTypeBase.Item)
			{
                e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, e.DisplayText, SearchTypeEnum.RetailItems);
			}
		}

		private void dtpStartingDate_ValueChanged(object sender, EventArgs e)
		{
			CheckEnabled(this, EventArgs.Empty);
		}

		private void dtpEndingDate_ValueChanged(object sender, EventArgs e)
		{
			CheckEnabled(this, EventArgs.Empty);
		}

		private void cmbMultType_SelectedIndexChanged(object sender, EventArgs e)
		{
			RefreshQtyAmtDecimalPlaces();
			CheckEnabled(this, EventArgs.Empty);
		}

		private void ntbQtyAmt_TextChanged(object sender, EventArgs e)
		{
			CheckEnabled(this, EventArgs.Empty);
		}

		private void ntbLoyPts_TextChanged(object sender, EventArgs e)
		{
			CheckEnabled(this, EventArgs.Empty);
		    CheckErrorProvider();
		}

        private void CheckErrorProvider()
        {
            if ((LoyaltyPointTypeBase)type == LoyaltyPointTypeBase.Tender)
            {
                errorProvider1.SetError(ntbLoyPts, Properties.Resources.PayLoyaltyTenderInfo);
            }
        }
	}
}
