using System;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Replenishment
{
    public class InventoryTemplateSection : DataEntity
    {
        public override RecordIdentifier  ID
        {
	        get 
	        { 
		         return new RecordIdentifier(SectionID, TemplateID);
	        }
	        set 
	        {
	            if (!serializing)
	            {
	                throw new NotImplementedException();
	            }
	        }
        }

        [RecordIdentifierValidation(20, Depth = 1)]
        public RecordIdentifier SectionID { get; set; }

        [RecordIdentifierValidation(20, Depth = 1)]
        public RecordIdentifier TemplateID { get; set; }

        public int SortRank { get; set; }
    }
}
