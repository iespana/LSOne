using LSOne.DataLayer.BusinessObjects.IntegrationFramework;
using System.Collections.Generic;

namespace LSRetail.SiteService.SiteServiceInterface
{
    public interface ISiteServicePlugin
    {
        bool Exclude { get;  }
        void Load(Dictionary<string,string> configurations);
        void Unload();
        void ReloadConfigurations(Dictionary<string, string> configurations);
        string ConfigurationKey { get; }
        void OnNotifyPlugin(object sender, MessageEventArgs e);
    }
}
