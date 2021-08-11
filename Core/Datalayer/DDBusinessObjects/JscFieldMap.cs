using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.DataLayer.DDBusinessObjects
{
    public class JscFieldMap
    {
        private JscTableMap jscTableMap;
        private JscFieldDesign fromJscFieldDesign;
        private JscFieldDesign toJscFieldDesign;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier TableMap { get; set; }
        public RecordIdentifier FromField { get; set; }
        public RecordIdentifier ToField { get; set; }
        public ConversionMethod ConversionMethod { get; set; }
        public string ConversionValue { get; set; }

        public JscTableMap JscTableMap
        {
            get { return jscTableMap; }
            set
            {
                jscTableMap = value;
                TableMap = value == null ? TableMap : value.ID;
            }
        }

        public JscFieldDesign FromJscFieldDesign
        {
            get { return fromJscFieldDesign; }
            set
            {
                fromJscFieldDesign = value;
                FromField = value == null ? FromField : value.ID;
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