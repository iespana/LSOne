using System;
using System.Linq;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

namespace LSOne.DataLayer.BusinessObjects.Loyalty
{
    /// <summary>
    /// The business object that holds information about loyalty points
    /// </summary>
    public class LoyaltyPoints : DataEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoyaltyPoints" /> class.
        /// </summary>
        public LoyaltyPoints()
        {
            SchemeID = RecordIdentifier.Empty;
            RuleID = RecordIdentifier.Empty;
            Type = LoyaltyPointTypeBase.Tender;
            SchemeRelation = RecordIdentifier.Empty;
            QtyAmountLimit = decimal.Zero;
            Points = decimal.Zero;
            BaseCalculationOn = CalculationTypeBase.Quantity;
            StartingDate = Date.Empty;
            EndingDate = Date.Empty;
        }

        public static string[] LoyaltyPointTypeBaseNames()
        {
            string[] result = Enum.GetNames(typeof(LoyaltyPointTypeBase));
            result[0] = Properties.Resources.LoyaltyPointTypeBase_Item;
            result[1] = Properties.Resources.LoyaltyPointTypeBase_RetailGr;
            result[2] = Properties.Resources.LoyaltyPointTypeBase_RetailDep;
            result[3] = Properties.Resources.LoyaltyPointTypeBase_Promotion;
            result[4] = Properties.Resources.LoyaltyPointTypeBase_Discount;
            result[5] = Properties.Resources.LoyaltyPointTypeBase_Tender;
            result[6] = Properties.Resources.LoyaltyPointTypeBase_SpecialGr;

            //return result;
            
            // Only the first 3 items + Tender work currently
            var brokenList = result.Take(3).ToList();
            brokenList.Add(Properties.Resources.LoyaltyPointTypeBase_Tender);
            return brokenList.ToArray();
        }

        public static string AsString(LoyaltyPointTypeBase Value)
        {
            switch (Value)
            {
                case LoyaltyPointTypeBase.Item: return Properties.Resources.LoyaltyPointTypeBase_Item;
                case LoyaltyPointTypeBase.RetailGroup: return Properties.Resources.LoyaltyPointTypeBase_RetailGr;
                case LoyaltyPointTypeBase.RetailDepartment: return Properties.Resources.LoyaltyPointTypeBase_RetailDep;
                case LoyaltyPointTypeBase.Promotion: return Properties.Resources.LoyaltyPointTypeBase_Promotion;
                case LoyaltyPointTypeBase.Discount: return Properties.Resources.LoyaltyPointTypeBase_Discount;
                case LoyaltyPointTypeBase.Tender: return Properties.Resources.LoyaltyPointTypeBase_Tender;
                case LoyaltyPointTypeBase.SpecialGroup: return Properties.Resources.LoyaltyPointTypeBase_SpecialGr;
                default: return Enum.GetName(typeof(LoyaltyPointTypeBase), Value);
            }
        }

        public static string[] CalculationTypeBaseNames()
        {
            string[] result = Enum.GetNames(typeof(CalculationTypeBase));
            result[0] = Properties.Resources.CalculationTypeBase_Amounts;
            result[1] = Properties.Resources.CalculationTypeBase_Quantity;
            return result;
        }

        public static string AsString(CalculationTypeBase Value)
        {
            switch (Value)
            {
                case CalculationTypeBase.Amounts: return Properties.Resources.CalculationTypeBase_Amounts;
                case CalculationTypeBase.Quantity: return Properties.Resources.CalculationTypeBase_Quantity;
                default: return Enum.GetName(typeof(CalculationTypeBase), Value);
            }
        }

        [DataMember]
        [RecordIdentifierValidation(20, Depth = 2)]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(SchemeID, RuleID);
            }
            set 
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        /// <summary>
        /// Gets or sets the loyalty rule ID.
        /// </summary>
        /// <value>The rule ID.</value>
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier RuleID { get; set; }

        /// <summary>
        /// Gets or sets the scheme ID.
        /// </summary>
        /// <value>The scheme ID.</value>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SchemeID { get; set; }

        /// <summary>
        /// Gets or sets the type of point calculation
        /// </summary>
        /// <value>The type.</value>
        [DataMember]
        public LoyaltyPointTypeBase Type { get; set; }

        /// <summary>
        /// Gets the type of point calculation as string
        /// </summary>
        /// <value>The type as string.</value>
        public string TypeAsString { get { return AsString(Type); } }

        /// <summary>
        /// The ID that triggers the calculation. Depends on what is selected in <see cref="Type"/> what the ID is.
        /// </summary>
        /// <value>The scheme relation.</value>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SchemeRelation { get; set; }

        /// <summary>
        /// The Name or Description of SchemeRelation entity.
        /// </summary>
        /// <value>The scheme relation name.</value>
        public string SchemeRelationName { get; set; }

        /// <summary>
        /// The minimum qty and/or amount needed for the calculation of loyalty points
        /// </summary>
        /// <value>The qty amount limit.</value>
        [DataMember]
        public decimal QtyAmountLimit { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
        [DataMember]
        public decimal Points { get; set; }

        /// <summary>
        /// Determines what the calculation is based on i.e. amount or qty
        /// </summary>
        /// <value>The base calculation on.</value>
        [DataMember]
        public CalculationTypeBase BaseCalculationOn { get; set; }

        /// <summary>
        /// returns the calculation type as string
        /// </summary>
        /// <value>The calculation type as string.</value>
        public string BaseCalculationOnAsString { get { return AsString(BaseCalculationOn); } }

        /// <summary>
        /// Gets or sets the starting date.
        /// </summary>
        /// <value>The starting date.</value>
        [DataMember]
        public Date StartingDate { get; set; }

        /// <summary>
        /// Gets or sets the ending date.
        /// </summary>
        /// <value>The ending date.</value>
        [DataMember]
        public Date EndingDate { get; set; }
    }
}
