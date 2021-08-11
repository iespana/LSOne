using LSOne.DataLayer.KDSBusinessObjects;


namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    public class LSOneKitchenDisplayItemRoutingConnection : KitchenDisplayItemRoutingConnection
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