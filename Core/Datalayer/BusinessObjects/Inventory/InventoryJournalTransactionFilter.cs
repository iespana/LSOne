using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Expressions;
using System.Collections.Generic;

namespace LSOne.DataLayer.BusinessObjects.Inventory
{
    public class InventoryJournalTransactionFilter
    {
        public int RowFrom { get; set; }
        public int RowTo { get; set; }
        public InventoryJournalTransactionSorting Sort { get; set; }
        public bool SortBackwards { get; set; }
        public RecordIdentifier JournalTransactionID { get; set; }
        public List<string> IdOrDescriptions { get; set; }
        public bool IdOrDescriptionStartsWith { get; set; }
        public List<string> Variants { get; set; }
        public bool VariantStartsWith { get; set; }

        public bool CountedSet { get; set; }
        public double Counted { get; set; }
        public DoubleValueOperator CountedComparison { get; set; }

        public bool InventoryOnHandSet { get; set; }
        public double InventoryOnHand { get; set; }
        public DoubleValueOperator InventoryOnHandComparison { get; set; }

        public bool DifferenceSet { get; set; }
        public double Difference { get; set; }
        public DoubleValueOperator DifferenceComparison { get; set; }

        public bool DifferencePercentageSet { get; set; }
        public double DifferencePercentage { get; set; }
        public DoubleValueOperator DifferencePercentageComparison { get; set; }

        public bool PostedSet { get; set; }
        public bool Posted { get; set; }

        public Date DateFrom { get; set; }
        public Date DateTo { get; set; }

        public RecordIdentifier RetailGroupID { get; set; }
        public RecordIdentifier RetailDepartmentID { get; set; }
        public RecordIdentifier StaffID { get; set; }
        public RecordIdentifier AreaID { get; set; }

        public InventoryJournalTransactionFilter()
        {
            Sort = InventoryJournalTransactionSorting.ItemName;
            IdOrDescriptions = new List<string>();
            Variants = new List<string>();
            IdOrDescriptionStartsWith = true;
            VariantStartsWith = true;
            JournalTransactionID = null;
            DateFrom = Date.Empty;
            DateTo = Date.Empty;
            RetailGroupID = null;
            RetailDepartmentID = null;
            StaffID = null;
            AreaID = null;
        }
    }
}
