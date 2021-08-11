using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.DataProviders.Hospitality
{
    public interface IFloorLayoutData : IDataProvider<FloorLayout>
    {
        FloorLayout Get(IConnectionManager entry, RecordIdentifier id, CacheType cacheType = CacheType.CacheTypeNone);

        List<DataEntity> GetList(IConnectionManager entry);

        bool SectionRelationIsHospitalityType
        {
            get;
        }

        List<SectionRelationType> GetSectionRelationTypes(IConnectionManager entry);

        string GetSectionRelationTypeDescription(IConnectionManager entry, RecordIdentifier sectionBackOfficeID);
    }
}
