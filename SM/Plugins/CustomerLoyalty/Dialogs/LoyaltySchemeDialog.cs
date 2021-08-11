using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Loyalty;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Loyalty;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CustomerLoyalty.Dialogs
{
	public partial class LoyaltySchemeDialog : DialogBase
	{
        private LoyaltySchemes dataObject;
	    private SiteServiceProfile siteServiceProfile;
		
        public LoyaltySchemes LoyaltyScheme { get { return dataObject; } }

        public LoyaltySchemeDialog(RecordIdentifier dataObjectId, SiteServiceProfile siteServiceProfile)
            : this( siteServiceProfile)
		{

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            dataObject = service.GetLoyaltyScheme(PluginEntry.DataModel, siteServiceProfile, dataObjectId, true);

			if (dataObject != null)
			{
				tbDescription.Text = dataObject.Description;
				cmbExpTimeUnit.SelectedIndex = (int)dataObject.ExpirationTimeUnit;
				ntbExpTimeValue.Text = dataObject.ExpireTimeValue.ToString();
				ntbPtsUseLimit.Text = dataObject.UseLimit.ToString();
				cmbDefMultType.SelectedIndex = (int)dataObject.CalculationType;
			    cmbCopyFrom.Enabled = false;
			}
		}

        public LoyaltySchemeDialog( SiteServiceProfile siteServiceProfile)
		{
			InitializeComponent();

            this.siteServiceProfile = siteServiceProfile;

            cmbCopyFrom.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            cmbCopyFrom.Enabled = true;


			var names = LoyaltySchemes.ExpirationTimeUnitEnumNames();
			foreach (string name in names)
			{
			    cmbExpTimeUnit.Items.Add(name);
			}

			names = LoyaltySchemes.CalculationTypeBaseNames();
			foreach (string name in names)
			{
			    cmbDefMultType.Items.Add(name);
			}
			CheckEnabled(this, EventArgs.Empty);
		}

		protected override IApplicationCallbacks OnGetFramework()
		{
			return PluginEntry.Framework;
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			if (dataObject == null)
			{
				dataObject = new LoyaltySchemes();
			}

			dataObject.Description = tbDescription.Text;
			dataObject.ExpirationTimeUnit = cmbExpTimeUnit.SelectedIndex > 0 ? (TimeUnitEnum)cmbExpTimeUnit.SelectedIndex : 0;
			dataObject.ExpireTimeValue = (int)ntbExpTimeValue.Value;
			dataObject.UseLimit = (int)ntbPtsUseLimit.Value;
			dataObject.CalculationType = cmbDefMultType.SelectedIndex > 0 ? (CalculationTypeBase)cmbDefMultType.SelectedIndex : 0;

            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            dataObject = service.SaveLoyaltyScheme(PluginEntry.DataModel, siteServiceProfile, dataObject, cmbCopyFrom.SelectedData.ID, true);

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
			if (dataObject == null) // Creating new
			{
				btnOK.Enabled = tbDescription.Text.Length > 0 
                    && cmbExpTimeUnit.SelectedIndex >= 0
                    && ntbExpTimeValue.Value > 0
					&& cmbDefMultType.SelectedIndex >= 0;
			}
			else // Editing existing
			{
				btnOK.Enabled = ((tbDescription.Text.Length > 0 && tbDescription.Text != dataObject.Description)
								|| ((int)dataObject.ExpirationTimeUnit != (cmbExpTimeUnit.SelectedIndex))
								|| (dataObject.ExpireTimeValue.ToString() != ntbExpTimeValue.Text)
								|| (dataObject.UseLimit.ToString() != ntbPtsUseLimit.Text)
								|| ((cmbDefMultType.SelectedIndex >= 0) && ((int)dataObject.CalculationType != (cmbDefMultType.SelectedIndex)))
                                ) && ntbExpTimeValue.Value > 0;
			}
		}

		private void cmbDefMultType_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckEnabled(this, EventArgs.Empty);
		}

		private void ntbPtsUseLimit_TextChanged(object sender, EventArgs e)
		{
			CheckEnabled(this, EventArgs.Empty);
		}

		private void ntbExpTimeValue_TextChanged(object sender, EventArgs e)
		{
			CheckEnabled(this, EventArgs.Empty);
		}

		private void cmbExpTimeUnit_SelectedIndexChanged(object sender, EventArgs e)
		{
			CheckEnabled(this, EventArgs.Empty);
		}

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            List<LoyaltySchemes> schemes;
          
            var service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
            schemes = service.GetLoyaltySchemes(PluginEntry.DataModel, siteServiceProfile, true);


            cmbCopyFrom.SetData(schemes, null);
        }

        private void cmbCopyFrom_RequestClear(object sender, EventArgs e)
        {
            cmbCopyFrom.SelectedData.ID = RecordIdentifier.Empty;
        }
	}
}
