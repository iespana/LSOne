namespace LSOne.DataLayer.BusinessObjects.StoreManagement
{
    /// <summary>
    /// Store logo size enum used by WinPrinter when printing receipts.
    /// </summary>
    public enum StoreLogoSizeType
    {
        /// <summary>
        /// Normal size.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 2x size.
        /// </summary>
        Double = 2
    }

    /// <summary>
    /// Helper class for operations on <see cref="StoreLogoSizeType">store logo size</see> enumeration.
    /// </summary>
    public static class StoreLogoSizeTypeHelper
    {
        /// <summary>
        /// Gets the <see cref="StoreLogoSizeType">store logo size</see> localized description.
        /// </summary>
        public static string StoreLogoSizeTypeToString(StoreLogoSizeType logoSizeType)
        {
            switch (logoSizeType)
            {
                case StoreLogoSizeType.Double:
                    return Properties.Resources.DoubleStoreLogoSize;
                case StoreLogoSizeType.Normal:
                    return Properties.Resources.NormalStoreLogoSize;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Converts from localized description to <see cref="StoreLogoSizeType">store logo size</see> enumeration.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static StoreLogoSizeType StringToStoreLogoSizeType(string value)
        {
            if (value == Properties.Resources.DoubleStoreLogoSize)
                return StoreLogoSizeType.Double;
            if (value == Properties.Resources.NormalStoreLogoSize)
                return StoreLogoSizeType.Normal;

            return StoreLogoSizeType.Normal;
        }
    }
}
