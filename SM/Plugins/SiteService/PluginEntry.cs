using System.Windows.Forms;

using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SiteService.Dialogs;

using TabControl = LSOne.ViewCore.Controls.TabControl;


namespace LSOne.ViewPlugins.SiteService
{
	public class PluginEntry : IPlugin
	{
		internal static IConnectionManager DataModel = null;
		internal static IApplicationCallbacks Framework = null;
		internal static bool CloudAdmin= false;

		internal const string HashSalt = "One retail POS software to excel in all your retail challenges";
		internal const string HashAlgorithm = "SHA512";
		/// <summary>
		/// Encryption key for the Site Service configurations.
		/// </summary>
		internal const string ConfigurationCryptoKey = "DFB8EFC8-1584-49DD-A9B1-EC01A3FBFF9F";
		/// <summary>
		/// Delimiter for the administrative password that includes the timestamp as well.
		/// </summary>
		internal const string AdministrativePasswordDelimiter = "###";

		#region IPlugin Members

		public string Description
		{
			get { return Properties.Resources.SiteServicePluginName; }
		}

		public bool ImplementsFeature(object sender, string message, object parameters)
		{
			return false;
		}

		public object Message(object sender, string message, object parameters)
		{
			return null;
		}

		public static  void InitializeSiteServiceProfile()
		{
			if (DataModel.IsCloud)
			{
				var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

				if (!service.ClientRegistered())
				{
					var dlg = new GetCredentialsDialog();
					if (dlg.ShowDialog() == DialogResult.OK)
					{
						service.RegisterClient(PluginEntry.DataModel, dlg.UserName, dlg.Password, string.Empty);
					}
					else
					{
						MessageDialog.Show(Properties.Resources.SiteServiceAuthenticationCanceled);
						return;
					}

				}
			}
		}

		public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
		{
			DataModel = dataModel;
			Framework = frameworkCallbacks;

		   
			if (DataModel.IsCloud )
			{
				return;
			}           
		
			// We want to add tabs on tab panels in other plugins
			TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

			frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
			frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
			frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

			frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
			frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
			frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

			frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);
		}

		public void Dispose()
		{
			
		}

		public void GetOperations(IOperationList operations)
		{
			operations.AddOperation(Properties.Resources.StoreServerProfiles, PluginOperations.ShowTransactionServiceProfilesSheet, Permission.TransactionServiceProfileView);
		}

		#endregion
	}
}
