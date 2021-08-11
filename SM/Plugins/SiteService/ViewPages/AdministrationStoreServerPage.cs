using System;
using System.Collections.Generic;

using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

using ServiceType = LSOne.DataLayer.GenericConnector.Enums.ServiceType;

namespace LSOne.ViewPlugins.SiteService.ViewPages
{
	public partial class AdministrationStoreServerPage : ContainerControl, ITabViewV2
	{
		private Parameters parameters;

		private List<SiteServiceProfile> profiles = null;

		public AdministrationStoreServerPage()
		{
			InitializeComponent();
		}

		public static ITabView CreateInstance(object sender, TabControl.Tab tab)
		{
			return new AdministrationStoreServerPage();
		}

		#region ITabPanel Members

		public void InitializeView(RecordIdentifier context, object internalContext)
		{
			// no customizations required
		}

		public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
		{
			parameters = (Parameters) internalContext;

			profiles = Providers.SiteServiceProfileData.GetSelectList(PluginEntry.DataModel);

			SiteServiceProfile profile = profiles.Find(p => p.ID == parameters.SiteServiceProfile);
			if (profile != null)
			{
				tbHost.Text = profile.SiteServiceAddress;
				ntbPort.Value = (ushort) profile.SiteServicePortNumber;
				cmbTransactionServiceProfile.SelectedData = new DataEntity(profile.ID, profile.Text);
			}
			else
			{
				cmbTransactionServiceProfile.SelectedData = new DataEntity("", "");
			}
			

		}


		public bool DataIsModified()
		{
			if (cmbTransactionServiceProfile.SelectedData != null && cmbTransactionServiceProfile.SelectedData.ID  != parameters.SiteServiceProfile)
			{
				parameters.Dirty = true;
				return true;
			}

			return false;
		}

		public bool SaveData()
		{
			if (parameters.Dirty)
			{
				parameters.SiteServiceProfile = cmbTransactionServiceProfile.SelectedData.ID;

				PluginEntry.DataModel.SiteServiceAddress = tbHost.Text;
				PluginEntry.DataModel.SiteServicePortNumber = (UInt16) ntbPort.Value;

				if (PluginEntry.DataModel.ServiceIsLoaded(ServiceType.SiteServiceService))
				{
					Services.Interfaces.Services.SiteServiceService(PluginEntry.DataModel)
							.SetAddress(PluginEntry.DataModel.SiteServiceAddress,
										PluginEntry.DataModel.SiteServicePortNumber);
				}
			}

			return true;
		}


		public void GetAuditDescriptors(List<AuditDescriptor> contexts)
		{
			// no customizations required
		}

		public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier,
								  object param)
		{
			// no customizations required
		}

		public void DataUpdated(RecordIdentifier context, object internalContext)
		{
			LoadData(false, RecordIdentifier.Empty, internalContext);
		}

		public void OnClose()
		{

		}

		public void SaveUserInterface()
		{
			// no customizations required
		}

		#endregion

		private void tbHost_TextChanged(object sender, EventArgs e)
		{
			btnTestConnection.Enabled = tbHost.Text.Length > 0 && ntbPort.Value != 0;
		}

		private void ntbPort_TextChanged(object sender, EventArgs e)
		{
			btnTestConnection.Enabled = tbHost.Text.Length > 0 && ntbPort.Value != 0;
		}

		private void btnTestConnection_Click(object sender, EventArgs e)
		{
			PluginEntry.Framework.ViewController.CurrentView.ShowProgress(TestConnection,
																		  Properties.Resources.ConnectingToStoreServer);
		}

		private void TestConnection(object sender, EventArgs e)
		{
			ISiteServiceService service =
				(ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
			ConnectionEnum result = service.TestConnection(PluginEntry.DataModel, tbHost.Text, (UInt16) ntbPort.Value);

			PluginEntry.Framework.ViewController.CurrentView.HideProgress();


			string message = "";

			switch (result)
			{
				case ConnectionEnum.Success:
					message = Properties.Resources.ConnectionToStoreServerWorked;
					break;
				case ConnectionEnum.ExternalConnectionFailed:
					message = Properties.Resources.CouldNotConnectToExternalService;
					break;
				case ConnectionEnum.ConnectionFailed:
					message = Properties.Resources.CouldNotConnectToStoreServer;
					break;
				case ConnectionEnum.DatabaseConnectionFailed:
					message = Properties.Resources.CouldNotConnectToDataBase; 
					break;									
				case ConnectionEnum.ClientTimeNotSynchronized:
					message = Properties.Resources.ClientTimeNotSynchronizedMessage;
					break;
			}

			MessageDialog.Show(message);
		}

		private void cmbTransactionServiceProfile_RequestData(object sender, EventArgs e)
		{
			List<DataEntity> items = Providers.SiteServiceProfileData.GetList(PluginEntry.DataModel);

			cmbTransactionServiceProfile.SetData(items,
				null, true);
		}

		private void cmbTransactionServiceProfile_SelectedDataChanged(object sender, EventArgs e)
		{
			if (profiles == null)
				return;

			SiteServiceProfile profile = profiles.Find(p => p.ID == cmbTransactionServiceProfile.SelectedData.ID);

			if (profile != null)
			{
				tbHost.Text = profile.SiteServiceAddress;
				ntbPort.Value = profile.SiteServicePortNumber;
			}
			else
			{
				tbHost.Text = string.Empty;
				ntbPort.Text = "0";
				
			}
			btnTestConnection.Enabled = profile != null;


		}

		private void btnEditTransactionProfiles_Click(object sender, EventArgs e)
		{
			PluginOperations.ShowTransactionServiceProfilesSheet(cmbTransactionServiceProfile.SelectedData.ID);
		}

		private void cmbTransactionServiceProfile_RequestClear(object sender, EventArgs e)
		{
			cmbTransactionServiceProfile.SelectedData = new DataEntity("", "");
		}


	}
}
