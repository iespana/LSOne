using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSRetail.DD.Common;

namespace LSOne.ViewPlugins.Scheduler.BusinessObjects
{
   

    public class JscFieldDesign
    {
        private JscTableDesign jscTableDesign;

        public RecordIdentifier ID { get; set; }
        public RecordIdentifier TableDesign { get; set; }
        public string FieldName { get; set; }
        public int Sequence { get; set; }
        public DDDataType DataType { get; set; }
        public int? Length { get; set; }
        public short FieldClass { get; set; }
        public string FieldOption { get; set; }
        public bool Enabled { get; set; }
        public List<JscSubJobFromTableFilter> JscSubJobFromTableFilters { get; set; }
        public List<JscLinkedFilter> JscLinkedFilters { get; set; }
        public List<JscLinkedFilter> JscLinkedFilters1 { get; set; }

        public JscTableDesign JscTableDesign
        {
            get { return jscTableDesign; }
            set
            {
                jscTableDesign = value;

                TableDesign = value == null ? TableDesign : value.ID;
            }
        }
    }
}