using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.Settings;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LabelPrinting
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;
        internal static string PrinterItems;
        internal static string LabelItems;
        internal static string PrinterCustomers;
        internal static string LabelCustomers;

        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.LabelPrinting; }
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
            frameworkCallbacks.ViewController.AddContextBarCategoryConstructionHandler(PluginOperations.TaskBarCategoryCallback);
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);

            frameworkCallbacks.AddOperationCategoryConstructionHandler(new OperationCategoryEventHandler(PluginOperations.AddOperationCategoryHandler));
            frameworkCallbacks.AddOperationItemConstructionHandler(new OperationItemEventHandler(PluginOperations.AddOperationItemHandler));
            frameworkCallbacks.AddOperationButttonConstructionHandler(new OperationbuttonEventHandler(PluginOperations.AddOperationButtonHandler));

            frameworkCallbacks.AddRibbonPageConstructionHandler(new RibbonPageEventHandler(PluginOperations.AddRibbonPages));
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(new RibbonPageCategoryEventHandler(PluginOperations.AddRibbonPageCategories));
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(new RibbonPageCategoryItemEventHandler(PluginOperations.AddRibbonPageCategoryItems));

            frameworkCallbacks.AddMenuConstructionConstructionHandler(PluginOperations.ConstructMenus);

            frameworkCallbacks.AddSettingsHandler(SettingsHandler);
        }

        private void SettingsHandler(SettingsBase stream, bool isWriting)
        {
            if (isWriting)
            {
                stream.Write("Client.Application.Labels.Items.Printer", PrinterItems);
                stream.Write("Client.Application.Labels.Items.LabelForm", LabelItems);
                stream.Write("Client.Application.Labels.Customers.Printer", PrinterCustomers);
                stream.Write("Client.Application.Labels.Customers.LabelForm", LabelCustomers);
            }
            else
            {
                PrinterItems = stream.Read("Client.Application.Labels.Items.Printer", "");
                LabelItems = stream.Read("Client.Application.Labels.Items.LabelForm", "");
                PrinterCustomers = stream.Read("Client.Application.Labels.Customers.Printer", "");
                LabelCustomers = stream.Read("Client.Application.Labels.Customers.LabelForm", "");
            }
        }

        public void Dispose()
        {
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.ItemLabelTemplates, PluginOperations.ShowItemLabelTemplatesView, LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit);
            operations.AddOperation(Properties.Resources.CustomerLableTemplates, PluginOperations.ShowCustomerLabelTemplatesView, LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit);
        }
        #endregion
    }
}
