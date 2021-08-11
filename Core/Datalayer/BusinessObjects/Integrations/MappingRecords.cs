using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Integrations
{
    public class MappingRecords
    {
        public RecordIdentifier Id { get; set; }

        public RecordIdentifier MappingEntity { get; set; }

        public RecordIdentifier InternalId { get; set; }

        public string ExternalId { get; set; }

        public RecordIdentifier ExternalSystem { get; set; }

    }
}
