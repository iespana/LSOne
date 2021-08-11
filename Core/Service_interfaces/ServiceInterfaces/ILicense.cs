using System;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.Services.Interfaces
{
    /// <summary>
    /// Implemented by the service License.cs
    /// Used to enable the possibility to bypass the standard way of providing the license for a given 
    /// ID that identifies the current hardware.
    /// </summary>
    public interface ILicense : IService
    {
        void GetPassword(IConnectionManager entry, string volumeNumber, ref string password, ref DateTime expireDate, ref string companyName, ref LicenseDeliveryMode deliveryMode);
    }
}
