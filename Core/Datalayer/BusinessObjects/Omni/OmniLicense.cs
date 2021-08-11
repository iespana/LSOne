using System.ComponentModel.DataAnnotations;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Omni
{
    /// <summary>
    /// A business object that holds information about the loyalty Customer.   
    /// </summary>
    public class OmniLicense : DataEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OmniLicense" /> class.
        /// </summary>
        public OmniLicense()
        {
            AppID = "";
            DeviceID = "";
            TerminalID = "";
            StoreID = "";
            Licensekey = "";
        }

        [StringLength(20)]
        public RecordIdentifier AppID { get; set; }

        [StringLength(80)]
        public RecordIdentifier DeviceID { get; set; }

        [StringLength(20)]
        public RecordIdentifier TerminalID { get; set; }

        [StringLength(20)]
        public RecordIdentifier StoreID { get; set; }

        [StringLength(80)]
        public RecordIdentifier Licensekey { get; set; }
    }
}
