using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.USConfigurations
{
    internal class PluginOperations
    {
        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            if (args.ContextName == "LSOne.ViewPlugins.Store.Views.StoreView")
            {
                if ( PluginEntry.DataModel.HasPermission(Permission.ManagePriceSettings ) )
                {
                    args.Add(new TabControl.Tab(Properties.Resources.PriceTaxCalculations, ViewPages.StorePriceTaxCalculationsPage.CreateInstance), 350);
                }
            }
        }
    }
}
