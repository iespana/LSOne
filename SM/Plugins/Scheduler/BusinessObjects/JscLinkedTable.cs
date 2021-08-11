using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{

    public class JscLinkedTable
    {
        private JscTableDesign fromJscTableDesign;
        private JscTableDesign toJscTableDesign;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier FromTable { get; set; }
        public RecordIdentifier ToTable { get; set; }
        public List<JscLinkedFilter> JscLinkedFilters { get; set; }

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