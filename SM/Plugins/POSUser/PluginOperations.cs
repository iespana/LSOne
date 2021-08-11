using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.POSUser
{
    internal class PluginOperations
    {
        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            switch (args.ContextName)
            {
                case "LSOne.ViewPlugins.User.Views.User":
                    args.Add(
                    new TabControl.Tab(
                        Properties.Resources.Settings,
                        ViewPages.UserPOSSettingsPage.CreateInstance
                        ),
                    500);
                    break;


            }
        }
    }
}
