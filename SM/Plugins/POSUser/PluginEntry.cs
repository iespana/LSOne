using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.POSUser
{
    public class PluginEntry : IPlugin
    {
        internal static IConnectionManager DataModel = null;
        internal static IApplicationCallbacks Framework = null;

        internal static int POSUserImageIndex;

        
        #region IPlugin Members

        public string Description
        {
            get { return Properties.Resources.POSUserManagement; }
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
            ImageList iml;
            DataModel = dataModel;
            Framework = frameworkCallbacks;

            // Register Icons that this plugin uses to the framework
            // -------------------------------------------------
            iml = frameworkCallbacks.GetImageList();

            iml.Images.Add(Properties.Resources.POSUser);
            POSUserImageIndex = iml.Images.Count - 1;

            TabControl.TabPanelConstructionHandler += PluginOperations.ConstructTabs;

            frameworkCallbacks.AddOperationTrigger(OperationTriggers.TriggerHandler);
        }

        public void Dispose()
        {

        }

        public void GetOperations(IOperationList operations)
        {

        }

        #endregion
    }
}
