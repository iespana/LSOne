using System;
using System.Windows.Forms;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;
using LSOne.DataLayer.BusinessObjects.TouchButtons;

namespace LSOne.ViewPlugins.TouchButtons
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        /*private void ConstructMenus(object sender, MenuConstructionArguments args)
        {
            switch (args.Key)
            {
                case "Edit":

                    args.AddSeparator(500);

                    if (DataModel.HasPermission(Permission.TouchButtonLayoutView))
                    {
                        args.Add(new ExtendedMenuItem(Properties.Resources.Profiles, 510, "Profiles"), 510);
                    }
                    break;

                case "Profiles":
                    if (DataModel.HasPermission(Permission.TouchButtonLayoutView))
                    {
                        args.AddMenu(new ExtendedMenuItem(
                            Properties.Resources.TouchButtonLayouts + "...",
                            Properties.Resources.TouchLayoutImage,
                            1200,
                            new EventHandler(PluginOperations.ShowTouchButtonLayoutsSheet)));
                    }
                    break;
            }
        }*/

       
        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.TouchbuttonSetup; }
        }


        public bool ImplementsFeature(object sender, string message, object parameters)
        {
            if (message == "CanEditLayouts")
            {
                return DataModel.HasPermission(Permission.ManageTouchButtonLayout);
            }
            if (message == "NewPosMenuDialog")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageTouchButtonLayout);
            }
            if (message == "EditPosMenuDialog")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageTouchButtonLayout);
            }
            if (message == "NewStyleMenuDialog")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup);
            }
            if (message == "EditStyleMenuDialog")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup);
            }
            if (message == "CanEditStyles")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageStyleSetup);
            }
            if(message == "CanManagePosMenuHeaders")
            {
                return DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ManageTouchButtonLayout);
            }

            
            return false;
        }

        public object Message(object sender, string message, object parameters)
        {
            switch (message)
            {
                case "EditLayout":
                    PluginOperations.ShowTouchButtonLayoutsSheet(this, EventArgs.Empty);
                    return null;             
                case "AddPosMenuHeader":
                    Dialogs.NewButtonMenuDialog addMenuDialog = new Dialogs.NewButtonMenuDialog();
                    if (addMenuDialog.ShowDialog() == DialogResult.OK)
                    {
                        return addMenuDialog.PosMenuHeaderID;
                    }
                    return RecordIdentifier.Empty;
                case "AddPosMenuHeaderWithOperation":
                    return PluginOperations.NewPosButtonGridMenuWithOperation((int)((object[])parameters)[0], (int)((object[])parameters)[1], (int)((object[])parameters)[2]);
                case "EditPosMenuHeader":
                    PluginOperations.ShowPosButtonGridView((RecordIdentifier)parameters);
                    return RecordIdentifier.Empty;
                case "EditStyleSetup":
                    return PluginOperations.NewStyle((RecordIdentifier)parameters);
                case "NewStyleSetup":
                    return PluginOperations.NewStyle(RecordIdentifier.Empty);
                case "ManagePosMenuHeaders":
                    PluginOperations.ShowPosButtonGridMenusListView((RecordIdentifier)((object[])parameters)[0], (DeviceTypeEnum)((int)((object[])parameters)[1]));
                    break;
            }
            return null;
        }

        public void Init(IConnectionManager dataModel, IApplicationCallbacks frameworkCallbacks)
        {
            DataModel = dataModel;
            Framework = frameworkCallbacks;


            // We want to be able to register items to the main application menu
            //frameworkCallbacks.AddMenuConstructionConstructionHandler(new MenuConstructionEventHandler(ConstructMenus));

            frameworkCallbacks.AddOperationCategoryConstructionHandler(PluginOperations.AddOperationCategoryHandler);
            frameworkCallbacks.AddOperationItemConstructionHandler(PluginOperations.AddOperationItemHandler);
            frameworkCallbacks.AddOperationButttonConstructionHandler(PluginOperations.AddOperationButtonHandler);

            frameworkCallbacks.AddRibbonPageConstructionHandler(PluginOperations.AddRibbonPages);
            frameworkCallbacks.AddRibbonPageCategoryConstructionHandler(PluginOperations.AddRibbonPageCategories);
            frameworkCallbacks.AddRibbonPageCategoryItemConstructionHandler(PluginOperations.AddRibbonPageCategoryItems);

            frameworkCallbacks.ViewController.AddContextBarItemConstructionHandler(PluginOperations.TaskBarItemCallback);
        }

        public void Dispose()
        {
            
        }

        public void GetOperations(IOperationList operations)
        {
            operations.AddOperation(Properties.Resources.TouchButtonLayouts, PluginOperations.ShowTouchButtonLayoutsSheet, Permission.ManageTouchButtonLayout);
            operations.AddOperation(Properties.Resources.POSButtonGridMenus, PluginOperations.ShowPosButtonGridMenusListView, Permission.ManageTouchButtonLayout);

            operations.AddOperation("", "AddLayout", false, false, PluginOperations.AddTouchButtonLayout, Permission.ManageTouchButtonLayout);
        }

        #endregion
    }
}
