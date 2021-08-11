using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace LSOne.DataLayer.DataProviders.ProviderConfig
{
    public class Configuration
    {
        private static object syncRoot = new object();
        private static Configuration configuration;

        public bool RegisterDefaultProviders { get; set; }
        public List<Provider> Providers { get; set; }

        public bool IsEmpty
        {
            get
            {
                return Providers == null || Providers.Count == 0;
            }
        }

        public Configuration()
        {
            RegisterDefaultProviders = true;
        }

        [Browsable(false)]
        public static Configuration Instance
        {
            get
            {
                if (null == configuration)
                {
                    lock (syncRoot)
                    {
                        if (null == configuration)
                        {
                            try
                            {
                                // Attempt to load from the LSOneDataLayer node in app.config
                                configuration = ConfigurationManager.GetSection("LSOne")
                                    as Configuration;
                            }
                            catch (Exception)
                            {
                            }
                        }

                        if (configuration == null)
                            configuration = new Configuration();
                    }
                }

                return configuration;
            }
        }

        public void Register()
        {
            if (Providers == null)
                return;
            foreach (var provider in Providers)
            {
                if (!string.IsNullOrEmpty(provider.Assembly))
                {
                    DataProviderFactory.Instance.RegisterViaRegistrar(provider.Assembly);
                }
                else if (!string.IsNullOrEmpty(provider.Interface) && !string.IsNullOrEmpty(provider.Implementation))
                {
                    DataProviderFactory.Instance.RegisterDelimitedType(provider.Interface, provider.Implementation);
                }
            }
        }
    }
}
