using LSOne.DataLayer.BusinessObjects.Properties;

namespace LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions
{
    /// <summary>
    /// A dimension group defines which of the tree dimension types (color, size, style) are active on the item.
    /// </summary>
    public class DimensionGroup : DataEntity
    {

        public enum PosDisplaySettingEnum
        {
            Name,
            ID,
            NameAndID
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DimensionGroup"/> class.
        /// </summary>
        public DimensionGroup()
            : base()
        {

        }

        public PosDisplaySettingEnum PosDisplaySetting
        {
            get; 
            set; 
        }

        public string PosDisplaySettingText
        {
            get
            {
                switch (PosDisplaySetting)
                {
                        case PosDisplaySettingEnum.ID:
                        return Resources.PosDisplaySettingTextID;
                        case PosDisplaySettingEnum.Name:
                        return Resources.Name;
                        case PosDisplaySettingEnum.NameAndID:
                        return Resources.PosDisplaySettingTextNameAndID;
                }
                return null;
            }
        }


        /// <summary>
        /// If true the dimension includes a color
        /// </summary>
        public bool ColorActive
        {
            get;
            set;
        }

        /// <summary>
        /// If true the dimension includes a size
        /// </summary>
        public bool SizeActive
        {
            get;
            set;
        }

        /// <summary>
        /// If the true the dimension includes a style
        /// </summary>
        public bool StyleActive
        {
            get;
            set;
        }

        /// <summary>
        /// If true the dimension includes a serial number
        /// </summary>
        public bool SerialActive
        {
            get;
            set;
        }

        /// <summary>
        /// If true the serial number doesn't have to have a value
        /// </summary>
        public bool SerialAllowBlank
        {
            get;
            set;
        }

        
    }
}
