using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.Services.Interfaces
{
    public interface IAddressLookupService : IService
    {
        Address LookupAddress(Address existingAddress);
    }
}
