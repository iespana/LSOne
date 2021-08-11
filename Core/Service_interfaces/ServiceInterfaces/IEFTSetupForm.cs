using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Interface to handle fields required for setting up EFT for terminals
    /// </summary>
    public interface IEFTSetupForm 
    {
        /// <summary>
        /// IPAddress for EFT device
        /// </summary>
        string IPAddress { get; set; }
        /// <summary>
        /// Store ID for EFT
        /// </summary>
        string EFTStoreId { get; set; }
        /// <summary>
        /// Terminal ID for EFT
        /// </summary>
        string EFTTerminalID { get; set; }
        /// <summary>
        /// Custom data, implementation specific
        /// </summary>
        string CustomField1 { get; set; }
        /// <summary>
        /// Custom data, implementation specific
        /// </summary>
        string CustomField2 { get; set; }

        bool SaveOverride(IConnectionManager connectionManager, ISettings settings);

        void LoadData(IConnectionManager connectionManager, ISettings settings);

    }
}
