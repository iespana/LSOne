using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Omni;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.LSCommerce.ViewPages
{
	internal partial class FunctionalityProfilePage : UserControl, ITabView
	{
		private OmniFunctionalityProfile omniProfile;

		public FunctionalityProfilePage()
		{
			InitializeComponent();

			btnAddSuspensionType.Visible = PluginEntry.Framework.CanRunOperation("AddSuspensionType");
			btnEditPrintingStation.Visible = PluginEntry.Framework.CanRunOperation("EditRestaurantStations");
			btnEditImageItemLookupGroup.Visible = PluginEntry.Framework.CanRunOperation("EditSpecialGroups");

			cmbEnteringType.Items.AddRange(EnteringTypeEnumHelper.GetList().ToArray());
			cmbQuantityMethod.Items.AddRange(QuantityMethodEnumHelper.GetList().ToArray());
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new FunctionalityProfilePage();
		}

		#region ITabPanel Members

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			omniProfile = ((FunctionalityProfile) internalContext).OmniProfile;

			cmbMainMenu.SelectedData = RecordIdentifier.IsEmptyOrNull(omniProfile.MainMenu)
				? new DataEntity(string.Empty, string.Empty)
				: Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, omniProfile.MainMenu);

			cmbEnteringType.SelectedIndex = (int)omniProfile.EnteringType;
			cmbQuantityMethod.SelectedIndex = (int)omniProfile.QuantityMethod;
			ntDefaultQty.Value = (double)omniProfile.DefaultQuantity;

			cmbSuspensionTypes.SelectedData = RecordIdentifier.IsEmptyOrNull(omniProfile.SuspensionType)
				? new DataEntity(string.Empty, string.Empty)
				: Providers.SuspendedTransactionTypeData.Get(PluginEntry.DataModel, omniProfile.SuspensionType);

			cmbPrintingStation.SelectedData = RecordIdentifier.IsEmptyOrNull(omniProfile.PrintingStation)
				? new DataEntity(string.Empty, string.Empty)
				: Providers.PrintingStationData.Get(PluginEntry.DataModel, omniProfile.PrintingStation);

			cmbItemImageLookupGroup.SelectedData = RecordIdentifier.IsEmptyOrNull(omniProfile.ItemImageLookupGroup)
				? new MasterIDEntity()
				: Providers.SpecialGroupData.GetMasterIDEntity(PluginEntry.DataModel, omniProfile.ItemImageLookupGroup);
			chkAllowOfflineTrans.Checked = omniProfile.AllowOfflineTransaction;
			chkShowLSCommerceInventory.Checked = omniProfile.ShowMobileInventory;
		}

		public bool DataIsModified()
		{
			return omniProfile.MainMenu != cmbMainMenu.SelectedDataID
				   || (int)omniProfile.EnteringType != cmbEnteringType.SelectedIndex
				   || (int)omniProfile.QuantityMethod != cmbQuantityMethod.SelectedIndex
				   || (double)omniProfile.DefaultQuantity != ntDefaultQty.Value
				   || omniProfile.SuspensionType != cmbSuspensionTypes.SelectedDataID
				   || omniProfile.PrintingStation != cmbPrintingStation.SelectedDataID
				   || omniProfile.ItemImageLookupGroup != cmbItemImageLookupGroup.SelectedDataID
				   || omniProfile.AllowOfflineTransaction != chkAllowOfflineTrans.Checked
				   || omniProfile.ShowMobileInventory != chkShowLSCommerceInventory.Checked;
		}

		public bool SaveData()
		{
			omniProfile.MainMenu = cmbMainMenu.SelectedDataID ?? RecordIdentifier.Empty;
			omniProfile.EnteringType = (EnteringTypeEnum)cmbEnteringType.SelectedIndex;
			omniProfile.QuantityMethod = (QuantityMethodEnum)cmbQuantityMethod.SelectedIndex;
			omniProfile.DefaultQuantity = (decimal)ntDefaultQty.Value;

			omniProfile.SuspensionType = cmbSuspensionTypes.SelectedDataID;
			omniProfile.PrintingStation = cmbPrintingStation.SelectedDataID;
			omniProfile.ItemImageLookupGroup = cmbItemImageLookupGroup.SelectedDataID;
			omniProfile.AllowOfflineTransaction = chkAllowOfflineTrans.Checked;
			omniProfile.ShowMobileInventory = chkShowLSCommerceInventory.Checked;

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
		
		private void cmbSuspensionTypes_RequestData(object sender, EventArgs e)
		{
			cmbSuspensionTypes.SetData(Providers.SuspendedTransactionTypeData.GetList(PluginEntry.DataModel), null);
		}

		private void btnAddSuspensionType_Click(object sender, EventArgs e)
		{
			PluginEntry.Framework.RunOperation("AddSuspensionType", this, PluginOperationArguments.Empty);
		}

		private void cmbPrintingStation_RequestData(object sender, EventArgs e)
		{
			cmbPrintingStation.SetData(Providers.PrintingStationData.GetList(PluginEntry.DataModel), null);
		}
		private void btnEditPrintingStation_Click(object sender, EventArgs e)
		{
			PluginEntry.Framework.RunOperation("EditRestaurantStations", this, PluginOperationArguments.Empty);
		}

		private void cmbItemImageLookupGroup_RequestData(object sender, EventArgs e)
		{
			cmbItemImageLookupGroup.SetData(Providers.SpecialGroupData.GetMasterIDList(PluginEntry.DataModel), null);
		}

		private void cmbItemImageLookupGroup_RequestClear(object sender, EventArgs e)
		{
			cmbItemImageLookupGroup.SelectedData = new MasterIDEntity();
		}

		private void btnEditImageItemLookupGroup_Click(object sender, EventArgs e)
		{
			RecordIdentifier specialGroupID = ((MasterIDEntity) cmbItemImageLookupGroup.SelectedData).ReadadbleID;
			PluginOperationArguments args = new PluginOperationArguments(specialGroupID, null);
			PluginEntry.Framework.RunOperation("EditSpecialGroups", this, args);
		}

		private void cmbMainMenu_RequestData(object sender, EventArgs e)
		{
			var items = Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, new PosMenuHeaderFilter { MainMenu = true, DeviceType = (int)DeviceTypeEnum.MobileInventory });
			cmbMainMenu.SetData(items, null);
		}

		private void btnEditMainMenu_Click(object sender, EventArgs e)
		{
			IPlugin touchButtons = PluginEntry.Framework.FindImplementor(null, "CanManagePosMenuHeaders", null);
			if (touchButtons != null)
			{
				touchButtons.Message(null, "ManagePosMenuHeaders",
											new object[] { cmbMainMenu.SelectedDataID ?? RecordIdentifier.Empty, DeviceTypeEnum.MobileInventory });
			}
		}
	}
}
