using LSOne.DataLayer.BusinessObjects.TimeKeeper;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.SupportClasses;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Method to process the original barcode to populate the different barcode properties.
    /// </summary>
    public interface ITimeKeeperService : IService
    {

        void KeepTime();

    }
}
