using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.CustomerLoyalty
{
	public class PluginEntry : IPlugin
	{
		internal static IConnectionManager DataModel = null;
		internal static IApplicationCallbacks Framework = null;

		#region IPlugin Members

		public string Description
		{
			get { return Properties.Resources.Loyalty; }
		}

		public bool ImplementsFeature(object sender, string message, object parameters)
		{
			return false;
		}

		public object Message(object sender, string message, object parameters)
		{
			return null;
		}

		public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
		{
			DataModel = dataModel;
			Framework = frameworkCallbacks;

			// We want to be able to add to sheet contexts from other plugins
			frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

			// We want to add tabs on tab panels in other plugins
			TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;
			frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
			frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
			frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

			frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
			frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
			frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);
		}

		public void Dispose()
		{
		}

		public void GetOperations(IOperationList operations)
		{
			operations.AddOperation(Properties.Resources.LoyaltyCardNew, PluginOperations.NewCard, Permission.CardsEdit);
			operations.AddOperation(Properties.Resources.LoyaltyCardsView, PluginOperations.ShowLoyaltyCardsView, Permission.CardsView);
			operations.AddOperation(Properties.Resources.LoyaltySchemesView, PluginOperations.ShowLoyaltySchemesView, Permission.SchemesView);
			operations.AddOperation(Properties.Resources.LoyaltyTransView, PluginOperations.ShowLoyaltyTransView);
		}

		#endregion
	}
}
