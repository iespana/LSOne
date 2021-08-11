using System.ServiceModel;

namespace LSOne.Services.Interfaces
{
    [ServiceContract]
    public interface ILSStationPrinting
    {
        [OperationContract]
        void StationPrint(string deviceName, string printString);
    }
}
