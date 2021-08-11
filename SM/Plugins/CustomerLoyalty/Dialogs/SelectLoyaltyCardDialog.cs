using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CustomerLoyalty.Dialogs
{
	public partial class SelectLoyaltyCardDialog : DialogBase
	{
        private RecordIdentifier customerID;
	    private SiteServiceProfile siteServiceProfile;

        public SelectLoyaltyCardDialog(RecordIdentifier customerID, SiteServiceProfile siteServiceProfile)
            : this(siteServiceProfile)
        {
            this.customerID = customerID;
        }

        public SelectLoyaltyCardDialog(SiteServiceProfile siteServiceProfile)
		{
			InitializeComponent();
         
            this.siteServiceProfile = siteServiceProfile;
		}
		
		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
            var valid = LoyaltyCustomer.ErrorCodes.UnknownError;
            string comment = String.Empty;

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            service.UpdateLoyaltyCardCustomerID(
                PluginEntry.DataModel,
                siteServiceProfile,
                ref valid,
                ref comment,
                cmbLoyaltyCard.SelectedData.ID,
                customerID
                );

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
			bool enabled = true;
			btnOK.Enabled = enabled;
		}

		private void DualDataComboBox_RequestClear(object sender, EventArgs e)
		{
			((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
			CheckEnabled(sender, e);
		}

        private void dcbScheme_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (cmbLoyaltyCard.SelectedData == null || e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbLoyaltyCard.SelectedData).ID;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.LoyaltyCards, textInitallyHighlighted);
        }

        private void cmbLoyaltyCard_SelectedDataChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = cmbLoyaltyCard.SelectedData.ID != "" && cmbLoyaltyCard.SelectedData.ID != RecordIdentifier.Empty;
        }
	}
}
