using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents
{
    /// <summary>
    /// Provides a setter to initilize the database connection for this assembly
    /// </summary>
    public class PluginEntrySetter
    {
        public static void Initialize(IConnectionManager entry, IApplicationCallbacks framework)
        {
            PluginEntry.DataModel = entry;
            PluginEntry.Framework = framework;
        }
    }
}
