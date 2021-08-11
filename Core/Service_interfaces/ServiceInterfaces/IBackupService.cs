using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Method to process the original barcode to populate the different barcode properties.
    /// </summary>
    public interface IBackupService : IService
    {

        void NewBackup();

    }
}
