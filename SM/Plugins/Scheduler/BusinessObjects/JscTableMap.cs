using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
    public class JscTableMap
    {
        private JscTableDesign fromJscTableDesign;
        private JscTableDesign toJscTableDesign;
        
        public RecordIdentifier ID { get; set; }
        public RecordIdentifier FromTable { get; set; }
        public RecordIdentifier ToTable { get; set; }
        public string Description { get; set; }
        public List<JscFieldMap> JscFieldMaps { get; set; }
        
        public JscTableDesign FromJscTableDesign
        {
            get { return fromJscTableDesign; }
            set
            {
                fromJscTableDesign = value;
                FromTable = value == null ? FromTable : value.ID;
            }
        }

        public JscTableDesign ToJscTableDesign
        {
            get { return toJscTableDesign; }
            set
            {
                toJscTableDesign = value;
                ToTable = value == null ? ToTable : value.ID;
            }
        }
    }
}