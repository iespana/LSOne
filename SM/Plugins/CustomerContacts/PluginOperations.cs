using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.CustomerContacts
{
    internal class PluginOperations
    {
        internal static void ConstructTabs(object sender, TabPanelConstructionArguments args)
        {
            switch (args.ContextName)
            {
                case "LSOne.ViewPlugins.Customer.Views.CustomerView":
                    args.Add(
                    new TabControl.Tab(
                        Properties.Resources.Contacts, 
                        ViewPages.CustomerContactsPage.CreateInstance
                        ),
                    120);
                    break;

              
            }
        }

    }
}
