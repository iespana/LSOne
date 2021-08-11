using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Customers;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.ColorPalette;

namespace LSOne.ViewPlugins.CustomerLoyalty.Dialogs
{
	public partial class LoyaltyCardDialog : DialogBase
	{
		private RecordIdentifier dataObjectId;
        private LoyaltyMSRCard dataObject;
        private bool editMode;
        private LoyaltyCustomerParams loyaltyParams;
	    private SiteServiceProfile siteServiceProfile;

		public LoyaltyCardDialog(RecordIdentifier dataObjectId,LoyaltyCustomerParams loyaltyParams, SiteServiceProfile siteServiceProfile)
			:this()
		{
            LoyaltySchemes loyaltyScheme = null;
            int transactionCount;

		    this.siteServiceProfile = siteServiceProfile;

            this.loyaltyParams = loyaltyParams;
			this.dataObjectId = dataObjectId;

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            dataObject = service.GetLoyaltyMSRCard(PluginEntry.DataModel, siteServiceProfile, dataObjectId, false);

            if (dataObject != null)
            {
                loyaltyScheme = service.GetLoyaltyScheme(PluginEntry.DataModel, siteServiceProfile, dataObject.SchemeID, false);
            }

            transactionCount = service.GetCustomerLoyaltyMSRCardTransCount(PluginEntry.DataModel, siteServiceProfile, dataObject.CardNumber, false);

            service.Disconnect(PluginEntry.DataModel);

			if (dataObject != null)
			{
				Text = Properties.Resources.EditLoyaltyCard;
                Header = Properties.Resources.EditLoyaltyCardDescription;

				editMode = true;
				ntbStartingPts.Value = (double)dataObject.StartingPoints;

				dcbScheme.SelectedData = new DataEntity((string)dataObject.SchemeID, loyaltyScheme == null ? (string)dataObject.SchemeID : loyaltyScheme.Text);

				Customer loyaltyCust = Providers.CustomerData.Get(PluginEntry.DataModel, dataObject.CustomerID,UsageIntentEnum.Minimal);
				dcbCustomer.SelectedData = new DataEntity((string)dataObject.CustomerID, loyaltyCust == null ? (string)dataObject.CustomerID : loyaltyCust.Text);

                switch ((int)dataObject.TenderType)
                {
                    case 0:
                        cmbCardType.SelectedIndex = 0;
                        break;
                    case 2:
                        cmbCardType.SelectedIndex = 1;
                        break;
                    case 3:
                        cmbCardType.SelectedIndex = 2;
                        break;
                }
				//cmbCardType.SelectedIndex = (int)dataObject.TenderType;

                laCardsQty.Text = Properties.Resources.CardNumber + ":";
                ntbCardsQty.Text = (string)dataObject.ID;

				ntbCardsQty.ReadOnly = true;
				ntbCardsQty.BackColor = ColorPalette.BackgroundColor;
				ntbCardsQty.ForeColor = System.Drawing.Color.FromArgb(109,109,132);
			}
			else
			{
				dcbScheme.SelectedData = new DataEntity("", "");
				dcbCustomer.SelectedData = new DataEntity("", "");

                cmbCardType.Items.RemoveAt(cmbCardType.Items.Count - 1); // Removing the "Blocked item" when creating new cards
			}

            if (transactionCount > 0)
			{
				ntbStartingPts.Enabled = false;
				dcbCustomer.Enabled = false;
			}

		}

        public LoyaltyCardDialog(LoyaltyCustomerParams loyaltyParams, SiteServiceProfile siteServiceProfile)
            : this()
        {
            this.loyaltyParams = loyaltyParams;
            this.siteServiceProfile = siteServiceProfile;
        }

		public LoyaltyCardDialog()
		{
			InitializeComponent();

		    ntbStartingPts.DecimalLetters = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices).Max;

			cmbCardType.Items.Clear();
			var names = LoyaltyMSRCard.TenderTypeEnumNames();

			for (int i = 0; i < names.Length; i++)
			{
				cmbCardType.Items.Add(names[i]);
			}

			ntbCardsQty.Value = 1;
			ntbStartingPts.Value = 0;

			btnAddScheme.Enabled =  PluginEntry.DataModel.HasPermission(Permission.SchemesEdit);
            btnEditScheme.Enabled = (dcbScheme.SelectedData != null);
		}

		public RecordIdentifier CardID
		{
			get { return dataObjectId; }
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (dataObject == null)
			{
				dataObject = new LoyaltyMSRCard();
			}

		    int qty = editMode ? 1 : (int)ntbCardsQty.Value;

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

			for (int i = 0; i < qty; i++)
			{
                if (!editMode)
                {
                    dataObject.ID = RecordIdentifier.Empty;
                }
                dataObject.SchemeID = dcbScheme.SelectedData == null ? null : dcbScheme.SelectedData.ID;
                dataObject.LinkType = LoyaltyMSRCard.LinkTypeEnum.Customer;

                if (cmbCardType.SelectedIndex <= 0)
                {
                    dataObject.TenderType = 0;
                }
                else if (cmbCardType.SelectedIndex == 1)
                {
                    dataObject.TenderType = (LoyaltyMSRCard.TenderTypeEnum)2;
                }
                else if (cmbCardType.SelectedIndex == 2)
                {
                    dataObject.TenderType = (LoyaltyMSRCard.TenderTypeEnum)3;
                }
                //dataObject.TenderType = cmbCardType.SelectedIndex >= 0 ? (LoyaltyMSRCard.TenderTypeEnum)cmbCardType.SelectedIndex : 0;
                dataObject.StartingPoints = (decimal)ntbStartingPts.Value;

                RecordIdentifier newLinkID = dcbCustomer.SelectedData == null ? null : dcbCustomer.SelectedData.ID;

                dataObject.ID = service.SaveLoyaltyMSRCard(PluginEntry.DataModel, siteServiceProfile, dataObject, false);

			    if (dataObject.LinkID != newLinkID)
                {
                    
                    LoyaltyCustomer.ErrorCodes valid = LoyaltyCustomer.ErrorCodes.UnknownError;
                    string comment = String.Empty;

                    service.UpdateLoyaltyCardCustomerID(
                        PluginEntry.DataModel,
                        siteServiceProfile,
                        ref valid,
                        ref comment,
                        dataObject.ID,
                        newLinkID,
                        false);
                    if (valid != 0)
                    {
                        ntbStartingPts.Value = (double)dataObject.StartingPoints;
                        LoyaltySchemes loyScheme = Providers.LoyaltySchemesData.Get(PluginEntry.DataModel, dataObject.SchemeID);
                        dcbScheme.SelectedData = new DataEntity((string)dataObject.SchemeID, loyScheme == null ? (string)dataObject.SchemeID : loyScheme.Text);

                        LoyaltyCustomer loyCust = Providers.LoyaltyCustomerData.Get(PluginEntry.DataModel, dataObject.CustomerID);
                        dcbCustomer.SelectedData = new DataEntity((string)dataObject.CustomerID, loyCust == null ? (string)dataObject.CustomerID : loyCust.Text);

                        switch ((int)dataObject.TenderType)
                        {
                            case 0:
                                cmbCardType.SelectedIndex = 0;
                                break;
                            case 2:
                                cmbCardType.SelectedIndex = 1;
                                break;
                            case 3:
                                cmbCardType.SelectedIndex = 2;
                                break;
                        }
                        //cmbCardType.SelectedIndex = (int)(int)dataObject.TenderType;
                        MessageDialog.Show(comment);
                        return;
                    }
                }

			    
			    service.Disconnect(PluginEntry.DataModel);
			}

            service.Disconnect(PluginEntry.DataModel);

			DialogResult = DialogResult.OK;
			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		// Check for the Ok btn being enabled
		private void CheckEnabled(object sender, EventArgs e)
		{
			var enabled = (ntbCardsQty.Text.Length > 0 || editMode) && (cmbCardType.SelectedIndex >= 0);
			enabled = enabled && (dcbScheme.SelectedData != null) && ((string)dcbScheme.SelectedData.ID != "");
            btnOK.Enabled = enabled;
		}

		private void DualDataComboBox_RequestClear(object sender, EventArgs e)
		{
			((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
			CheckEnabled(sender, e);
		}

		private void dcbScheme_RequestData(object sender, EventArgs e)
		{
		    List<LoyaltySchemes> schemes;

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            schemes =  service.GetLoyaltySchemes(PluginEntry.DataModel, siteServiceProfile, true);

            dcbScheme.SetData(schemes, null);
		}

		private void btnAddScheme_Click(object sender, EventArgs e)
		{
			var dlg = new LoyaltySchemeDialog( siteServiceProfile);

			if (dlg.ShowDialog() == DialogResult.OK)
			{
				var loyaltyScheme = dlg.LoyaltyScheme;
				dcbScheme.SelectedData = new DataEntity(loyaltyScheme.ID, loyaltyScheme.Description);

				dcbScheme_SelectedDataChanged(this, EventArgs.Empty);
			}
		}

		private void btnEditScheme_Click(object sender, EventArgs e)
		{
			if (dcbScheme.SelectedData != null)
			{
				var dlg = new LoyaltySchemeDialog(dcbScheme.SelectedData.ID, siteServiceProfile);

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					var loyaltyScheme = dlg.LoyaltyScheme;
					dcbScheme.SelectedData = new DataEntity(loyaltyScheme.ID, loyaltyScheme.Description);
					dcbScheme_SelectedDataChanged(this, EventArgs.Empty);
				}
			}
		}

		private void dcbScheme_SelectedDataChanged(object sender, EventArgs e)
		{
			CheckEnabled(sender, e);
		    if (dcbScheme.SelectedData != null && dcbScheme.SelectedData.ID != string.Empty)
		    {
		        btnEditScheme.Enabled = true;
		    }
		    else
		    {
		        btnEditScheme.Enabled = false;
		    }
		}

		private void dcbCustomer_SelectedDataChanged(object sender, EventArgs e)
		{
			CheckEnabled(sender, e);
		}

		private void cmbCardType_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckEnabled(sender, e);
		}

		private void dcbCustomer_DropDown(object sender, DropDownEventArgs e)
		{
			e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,false, e.DisplayText, SearchTypeEnum.Customers);
		}

		private void ntbStartingPts_TextChanged(object sender, EventArgs e)
		{
			CheckEnabled(sender, e);
		}
	}
}
