using System.Collections.Generic;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.DDBusinessObjects
{
    public class JscTableDesign
    {
        private JscDatabaseDesign jscDatabaseDesign;
        
        public RecordIdentifier ID { get; set; }
        public RecordIdentifier DatabaseDesign { get; set; }
        public string TableName { get; set; }
        public bool Enabled { get; set; }
        public List<JscSubJob> JscSubJobs { get; set; }
        public List<JscTableMap> JscTableMaps { get; set; }
        public List<JscTableMap> JscTableMaps1 { get; set; }

        public JscDatabaseDesign JscDatabaseDesign
        {
            get { return jscDatabaseDesign; }
            set
            {
                jscDatabaseDesign = value;
                DatabaseDesign = value == null ? DatabaseDesign : value.ID;
            }
        }
    }
}