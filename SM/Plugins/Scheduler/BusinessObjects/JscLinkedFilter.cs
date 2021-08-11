using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{

    public class JscLinkedFilter
    {
        private JscLinkedTable jscLinkedTable;
        private JscFieldDesign toJscFieldDesign;
        private JscFieldDesign linkedJscFieldDesign;
        
        public RecordIdentifier ID { get; set; }
        public RecordIdentifier LinkedTable { get; set; }
        public RecordIdentifier LinkedField { get; set; }
        public LinkedFilterType LinkType { get; set; }
        public RecordIdentifier ToField { get; set; }
        public string Filter { get; set; }

        public JscLinkedTable JscLinkedTable
        {
            get { return jscLinkedTable; }
            set
            {
                jscLinkedTable = value;
                LinkedTable = value == null ? LinkedTable : value.ID;
            }
        }

        public JscFieldDesign LinkedJscFieldDesign
        {
            get { return linkedJscFieldDesign; }
            set
            {
                linkedJscFieldDesign = value;
                LinkedField = value == null ? LinkedField : value.ID;
            }
        }

        public JscFieldDesign ToJscFieldDesign
        {
            get { return toJscFieldDesign; }
            set
            {
                toJscFieldDesign = value;
                ToField = value == null ? ToField : value.ID;
            }
        }
    }
}