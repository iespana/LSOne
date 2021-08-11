using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    public class LSOneKitchenDisplayTerminalRoutingConnection : KitchenDisplayTerminalRoutingConnection
    {
        /// <summary>
        /// Tells if this routing line should be included or excluded
        /// </summary>
        public IncludeEnum IncludeExclude;

        public enum IncludeEnum
        {
            Include,
            Exclude
        }

        /// <summary>
        /// The terminal ID for the terminal routing
        /// </summary>
        public RecordIdentifier TerminalID { get; set; }

        /// <summary>
        /// The store ID for the terminal routing
        /// </summary>
        public RecordIdentifier StoreID { get; set; }

        /// <summary>
        /// The name of the terminal that the routing applies to
        /// </summary>
        public string TerminalName { get; set; }

        /// <summary>
        /// The name of the store that the routing applies to
        /// </summary>
        public string StoreName { get; set; }

        public static string IncludeEnumToString(IncludeEnum includeEnum)
        {
            switch (includeEnum)
            {
                case IncludeEnum.Include:
                    return Properties.Resources.Include;
                case IncludeEnum.Exclude:
                    return Properties.Resources.Exclude;
                default:
                    return string.Empty;
            }
        }
    }
}
