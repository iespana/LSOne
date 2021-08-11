using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    public class LSOneKitchenDisplayHospitalityTypeRoutingConnection : KitchenDisplayHospitalityTypeRoutingConnection
    {
        /// <summary>
        /// Tells if this routing line should be included or excluded
        /// </summary>
        public IncludeEnum IncludeExclude;

        /// <summary>
        /// Restaurant id of the connected hospitality type
        /// </summary>
        public RecordIdentifier Restaurant;

        /// <summary>
        /// Sales type id of the connected hospitality type
        /// </summary>
        public RecordIdentifier SalesType;

        public enum IncludeEnum
        {
            Include,
            Exclude
        }

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
