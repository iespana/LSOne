using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;

using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Ribbon;
using LSOne.ViewPlugins.SiteService.Dialogs;
using LSOne.ViewPlugins.SiteService.Properties;
using LSOne.ViewPlugins.SiteService.ViewPages;
using LSOne.ViewPlugins.SiteService.Views;

using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.SiteService
{
	internal class PluginOperations
	{
		private static SiteServiceProfile siteServiceProfile;

		public static SiteServiceProfile GetSiteServiceProfile()
		{
			if (siteServiceProfile == null)
			{
				Parameters parameters = Providers.ParameterData.Get(PluginEntry.DataModel);
				if (parameters != null)
				{
					siteServiceProfile = Providers.SiteServiceProfileData.Get(PluginEntry.DataModel, parameters.SiteServiceProfile);
				}
			}

			return siteServiceProfile;
		}

		public static RecordIdentifier NewTransactionServicesProfile()
		{
			RecordIdentifier selectedID = RecordIdentifier.Empty;

			if (PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit))
			{
				NewTransactionServiceProfileDialog dlg = new NewTransactionServiceProfileDialog();

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					// We put null in sender so that we also get our own change hint sent.
					selectedID = dlg.ProfileID;
					PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "TransactionServiceProfile", selectedID, null);

					PluginOperations.ShowTransactionServiceProfileSheet(null, selectedID);
				}
			}

			return selectedID;
		}

		public static void EditIntegrationFrameworkToken(AccessToken token)
		{
			if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuthenticationTokens))
			{
				AccessTokenDialog dlg = new AccessTokenDialog(token);

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					Providers.AccessTokenData.Save(PluginEntry.DataModel, dlg.Token);
					UpdateIntegrationFrameworkTokenCache(dlg.Token, MessageAction.Update);
					PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "IntegrationFrameworkTokens", null, null);
				}
			}
		}

		public static void NewIntegrationFrameworkToken()
		{
			if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuthenticationTokens))
			{
				AccessTokenDialog dlg = new AccessTokenDialog();

				if (dlg.ShowDialog() == DialogResult.OK)
				{
					Providers.AccessTokenData.Save(PluginEntry.DataModel, dlg.Token);
					UpdateIntegrationFrameworkTokenCache(dlg.Token, MessageAction.Add);
					PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "IntegrationFrameworkTokens", null, null);
				}
			}
		}

		public static bool DeleteIntegrationFrameworkToken(RecordIdentifier token)
		{
			if (PluginEntry.DataModel.HasPermission(Permission.SecurityManageAuthenticationTokens))
			{
				if (QuestionDialog.Show(Properties.Resources.DeleteIFToken, Properties.Resources.DeleteToken) == DialogResult.Yes)
				{
					Providers.AccessTokenData.Delete(PluginEntry.DataModel, token);

					UpdateIntegrationFrameworkTokenCache(token.StringValue, MessageAction.Delete);

					PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "IntegrationFrameworkTokens", null, null);

					return true;
				}
			}

			return false;
		}

		public static void UpdateIntegrationFrameworkTokenCache(object tokenData, MessageAction action)
		{
			try
			{
				ISiteServiceService service = (ISiteServiceService)PluginEntry.DataModel.Service(ServiceType.SiteServiceService);
				service.NotifyPlugin(PluginEntry.DataModel, GetSiteServiceProfile(), new MessageEventArgs("IntegrationFrameworkBasePlugin", "UpdateToken", tokenData, action));
			}
			catch
			{
				//Nothing
			}
		}

		public static void ShowTransactionServiceProfileSheet(object sender, RecordIdentifier profileID)
		{
			PluginEntry.Framework.ViewController.Add(new SiteServiceProfileView(profileID));
		}

		public static void ShowTransactionServiceProfilesSheet(object sender, EventArgs args)
		{
			PluginEntry.Framework.ViewController.Add(new SiteServiceProfilesView());
		}

		public static void ShowTransactionServiceProfilesSheet(RecordIdentifier id)
		{
			PluginEntry.Framework.ViewController.Add(new SiteServiceProfilesView(id));
		}

		public static bool DeleteTransactionServiceProfile(RecordIdentifier id)
		{
			if (PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit))
			{
				if (QuestionDialog.Show(Properties.Resources.DeleteSSProfileQuestion, Properties.Resources.DeleteSSProfile) == DialogResult.Yes)
				{
					Providers.SiteServiceProfileData.Delete(PluginEntry.DataModel, id);

					PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "TransactionServiceProfile", (string)id, null);

					return true;
				}
			}

			return false;
		}

		public static void ShowSiteServiceAdministrationTab(object sender, EventArgs args)
		{
			IPlugin plugin;

			plugin = PluginEntry.Framework.FindImplementor(null, "CanViewAdministrationTab", null);

			if (plugin != null)
			{
				plugin.Message(null, "ViewSiteServiceTab", null);
			}
		}

		public static void ShowIntegrationFrameworkTokensView(object sender, EventArgs args)
		{
			PluginEntry.Framework.ViewController.Add(new AccessTokensView());
		}

		internal static void AddOperationCategoryHandler(object sender, OperationCategoryArguments args)
		{
			args.Add(new Category(Properties.Resources.StoreSetup, "Store setup", null), 75);
		}

		internal static void AddOperationItemHandler(object sender, OperationItemArguments args)
		{
			if (args.CategoryKey == "Store setup")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileView)||
					PluginEntry.DataModel.HasPermission(Permission.VisualProfileView) ||
					PluginEntry.DataModel.HasPermission(Permission.FunctionalProfileView) ||
					PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileView) ||
					PluginEntry.DataModel.HasPermission(Permission.HardwareProfileView) || 
					PluginEntry.DataModel.HasPermission(Permission.StyleProfileView) ||
					PluginEntry.DataModel.HasPermission(Permission.FormProfileView))
				{
					args.Add(new Item(Properties.Resources.Profiles, "Profiles", null), 500);
				}
			}
		}

		internal static void AddOperationButtonHandler(object sender, OperationButtonArguments args)
		{
			if (args.CategoryKey == "Store setup" && args.ItemKey == "Profiles")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileView))
				{
					args.Add(new ItemButton(Properties.Resources.StoreServerProfiles, Properties.Resources.StoreServerProfilesDescription, ShowTransactionServiceProfilesSheet), 700);
				}
			}
		}

		internal static void AddRibbonPages(object sender, RibbonPageArguments args)
		{
			args.Add(new Page(Properties.Resources.Sites, "Sites"), 700);
		}

		internal static void AddRibbonPageCategories(object sender, RibbonPageCategoryArguments args)
		{
			if (args.PageKey == "Sites")
			{
				args.Add(new PageCategory(Properties.Resources.SiteService, "SiteService"), 300);
			}
		}

		internal static void AddRibbonPageCategoryItems(object sender, RibbonPageCategoryItemArguments args)
		{
			if (args.PageKey == "Sites" && args.CategoryKey == "SiteService")
			{
				if (PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileView))
				{
					args.Add(new CategoryItem(
						   Properties.Resources.SiteService,
						   Properties.Resources.StoreServerProfiles,
						   Properties.Resources.SiteServiceTooltip,
						   CategoryItem.CategoryItemFlags.DropDown,
						   null,
						   Properties.Resources.site_service_32,
						   null,
						   "SiteServiceMenu"), 10);
				}    
			}
		}

		internal static void ConstructMenus(object sender, MenuConstructionArguments args)
		{
			ExtendedMenuItem item;

			switch (args.Key)
			{
				// We dont need to check permissions here since thats been done on the parent menu allready
				case "RibbonSiteServiceMenu":
					item = new ExtendedMenuItem(
						Properties.Resources.StoreServerProfile,
						10,
						ShowTransactionServiceProfilesSheet);

					args.AddMenu(item);

					item = new ExtendedMenuItem(
						Properties.Resources.LocalSetting,
						20,
						ShowSiteServiceAdministrationTab);
					
					args.AddMenu(item);

					item = new ExtendedMenuItem(
						Properties.Resources.IntegrationFrameworkTokens,
						30,
						ShowIntegrationFrameworkTokensView);

					args.AddMenu(item);

					break;
			}
		}

		internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
		{
			if (args.ContextName == "LSOne.ViewPlugins.Store.Views.StoreView" && PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit))
			{
				args.Add(new TabControl.Tab(Properties.Resources.SiteService, StoreSspSettingPage.CreateInstance), 260);
			}

			if (args.ContextName == "LSOne.ViewPlugins.Terminals.Views.TerminalView" && PluginEntry.DataModel.HasPermission(Permission.TransactionServiceProfileEdit))
			{
				args.Add(
					new TabControl.Tab(Properties.Resources.SiteService, TerminalSspSettingPage.CreateInstance), 100);

			}

			if (args.ContextName == "LSOne.ViewPlugins.Administration.Views.AdministrationView")
			{
				args.Add(new TabControl.Tab(Properties.Resources.SiteService, "SiteServiceTab", AdministrationStoreServerPage.CreateInstance), 120);
			}

			if (args.ContextName == "LSOne.ViewPlugins.SiteService.Views.SiteServiceProfileView")
			{
				args.Add(new TabControl.Tab(Properties.Resources.IntegrationFramework, "IFTab", IntegrationFrameworkSettingsPage.CreateInstance), 50);
				args.Add(new TabControl.Tab(Properties.Resources.GiftCard, GiftCardPage.CreateInstance), 100);
				args.Add(new TabControl.Tab(Properties.Resources.CreditMemo, CreditMemoPage.CreateInstance), 200);
				args.Add(new TabControl.Tab(Properties.Resources.CentralSuspension, CentralSuspensionPage.CreateInstance), 300);
				args.Add(new TabControl.Tab(Properties.Resources.Inventory, InventoryPage.CreateInstance), 400);
				args.Add(new TabControl.Tab(Properties.Resources.Customer, CustomerPage.CreateInstance), 500);
				args.Add(new TabControl.Tab(Properties.Resources.CentralReturns, CentralReturnsPage.CreateInstance), 600);
				args.Add(new TabControl.Tab(Properties.Resources.EmailReceipts, EmailReceiptPage.CreateInstance), 700);

			}
		}

		public static void AddShortHandItem(RecordIdentifier profileID)
		{
			ShorthandDialog dialog = new ShorthandDialog(profileID);
			dialog.ShowDialog();
		}

		public static void EditShortHandItem(ShorthandItem shorthandItem)
		{
			ShorthandDialog dialog = new ShorthandDialog(shorthandItem);
			dialog.ShowDialog();
		}

		public static void DeleteShortHandItem(RecordIdentifier shortHandItemMasterID)
		{
			if (QuestionDialog.Show(Properties.Resources.AreYouSureYouWantToDeleteTheShortHandLine) == DialogResult.Yes)
			{
				Providers.ShortHandItemData.Delete(PluginEntry.DataModel, shortHandItemMasterID);
				PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "ShortHandItem", null, null);
			}
		}


		/// <summary>
		/// Executes Site Service administrative tasks with error handling.
		/// </summary>
		/// <param name="mainFunction">Site Service administrative tasks to execute.</param>
		/// <param name="successFunction">Logic to execute if administrative tasks executed successful.</param>
		/// <param name="failFunction">Logic to execute if administrative tasks execution failed.</param>
		public static void AdministrateSiteService(Action mainFunction, Action successFunction, Action failFunction)
		{
			if (mainFunction == null) return;

			bool executionIsSuccessful = false;
			try
			{
				mainFunction();
				executionIsSuccessful = true;
			}
			catch (Win32Exception w32Exception)
			{
				MessageDialog.Show(string.Format(Resources.SiteServiceError, w32Exception.Message));
			}
			catch (InvalidOperationException opException)
			{
				MessageDialog.Show(string.Format(Resources.SiteServiceError, opException.Message));
			}
			catch (FaultException faultException)
			{
				MessageDialog.Show(faultException.Message);
			}
			catch (Exception exception)
			{
				MessageDialog.Show(exception.Message);
				
			}
			finally
			{
				if (executionIsSuccessful)
				{
					successFunction?.Invoke();
				}
				else
				{
					failFunction?.Invoke();
				}
			}
		}

		/// <summary>
		/// Validates the Site Service password and returns the received token (non-empty string) if validation was successful.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		/// <exception cref="FaultException">Provided administrative password was not provided or is incorrect.</exception>
		public static string ValidateSiteServiceAdministrativePassword(string host, ushort port, string password)
		{
			var service = (ISiteServiceService) PluginEntry.DataModel.Service(ServiceType.SiteServiceService);

			return service.AdministrativeLogin(PluginEntry.DataModel, host, port, password);
		}
	}
}
