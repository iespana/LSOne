using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.ViewPlugins.CustomerOrders
{
    public class PluginKeys
    {
        /// <summary>
        /// Key = Retail
        /// </summary>
        internal static string RetailKey = "Retail";

        /// <summary>
        /// Key = Retail
        /// </summary>
        internal static string CustomersKey = "Customers";

        /// <summary>
        /// Key = Customer orders
        /// </summary>
        internal static string CustomerOrdersKey = "Customer orders";

        /// <summary>
        /// Key = Customer order settings
        /// </summary>
        internal static string SettingsKey = "Customer order settings";

        /// <summary>
        /// Key = Quotes
        /// </summary>
        internal static string QuotesKey = "Quotes";

        /// <summary>
        /// Key = Configuration
        /// </summary>
        internal static string ConfigurationKey = "Configuration";

        /// <summary>
        /// Key = Description
        /// </summary>
        internal static string SearchKey_Description = "Description";

        /// <summary>
        /// Key = Type
        /// </summary>
        internal static string SearchKey_Type = "Type";

    }

    public class PluginPriority
    {
        internal static int CustomerOrdersPriority = 10;
        internal static int QuotesPriority = 20;
        internal static int SettingsPriority = 30;

        internal static int NumberSequenceRelatedKey = 10;
    }
}

