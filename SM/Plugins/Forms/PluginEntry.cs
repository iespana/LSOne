using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Forms
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;


        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.FormDesigner; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "CanViewForm")
            {
                return PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsView);
            }
            if (message == "CanAddForm" || message == "CanEditForm" || message == "CanAddFormType" || message == "CanEditFormType")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.FormsEdit);
            }
            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            if (message == "ViewForm")
            {
                PluginOperations.ShowNewFormSheet((RecordIdentifier)((object[])parameters)[0], (int)((object[])parameters)[1]);
            }
            if (message == "AddFormForProfileLine")
            {
                if ((RecordIdentifier)parameters != RecordIdentifier.Empty)
                {
                    PluginOperations.NewFormForProfileLine((RecordIdentifier) parameters);
                }
                else
                {
                    PluginOperations.NewFormForProfileLine();
                }
            }
            if (message == "EditFormForProfileLine")
            {
                PluginOperations.EditFormForProfileLine((RecordIdentifier) ((object[]) parameters)[0],
                                                        (RecordIdentifier) ((object[]) parameters)[1]);
            }
            if (message == "AddFormTypeForProfileLine")
            {
                PluginOperations.NewFormType((RecordIdentifier) parameters);
            }
            if (message == "EditFormTypeForProfileLine")
            {
                PluginOperations.EditFormType((RecordIdentifier) ((object[]) parameters)[0],
                                            (RecordIdentifier) ((object[]) parameters)[1]);
            }
            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            frameworkCallbacks.AddOperationCategoryConstructionHandler(new OperationCategoryEventHandler(PluginOperations.AddOperationCategoryHandler));
            frameworkCallbacks.AddOperationItemConstructionHandler(new OperationItemEventHandler(PluginOperations.AddOperationItemHandler));
            frameworkCallbacks.AddOperationButttonConstructionHandler(new OperationbuttonEventHandler(PluginOperations.AddOperationButtonHandler));

            frameworkCallbacks.AddRibbonPageConstructionHandler(new RibbonPageEventHandler(PluginOperations.AddRibbonPages));
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(new RibbonPageCategoryEventHandler(PluginOperations.AddRibbonPageCategories));
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(new RibbonPageCategoryItemEventHandler(PluginOperations.AddRibbonPageCategoryItems));

            // We want to be able to add to sheet contexts from other plugins
            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.FormLayouts, PluginOperations.ShowFormsSheet, LSOne.DataLayer.BusinessObjects.Permission.FormsView);
        }

        #endregion
    }
}
