using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface IReprintInfo : IDataEntity
    {
        RecordIdentifier LineID { get; set; }
        Date ReprintDate { get; set; }
        RecordIdentifier Staff { get; set; }
        RecordIdentifier Store { get; set; }
        RecordIdentifier Terminal { get; set; }
        ReprintTypeEnum ReprintType { get; set; }
    }
}
