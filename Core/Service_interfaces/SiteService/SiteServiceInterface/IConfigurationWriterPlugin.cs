using System.Collections.Generic;
using LSOne.Utilities.IO;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public interface IConfigurationWriterPlugin
    {
        FolderItem CustomConfigFile { get; }

        /// <summary>
        /// Is sent after configurations are loaded, plugins can check if they have all the configurations that they want
        /// and if not then add a new one with a default value.
        /// 
        /// This is called before the Load method.
        /// </summary>
        /// <param name="configurations">The configuration dictionary as it was read from the settings file</param>
        /// <returns>True if a configuration was added or modified, else false</returns>
        bool VerifyConfigurations(Dictionary<string, string> configurations);

        void WriteConfigurations(Dictionary<string, string> configurations, ISiteServiceSettings settings);

        string ConfigurationName { get; }
    }
}
