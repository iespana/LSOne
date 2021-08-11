using LSOne.Utilities.DataTypes;
using System.Runtime.Serialization;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.Units")]
namespace LSOne.DataLayer.BusinessObjects.Units
{
    /// <summary>
    /// Data entity class for a unit conversion object. A unit conversion object maps the change for an item quantity when 
    /// changing between two units
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class UnitConversion : OptimizedUpdateDataEntity
    {
        private RecordIdentifier itemID;
        private RecordIdentifier fromUnitID;
        private RecordIdentifier toUnitID;
        private decimal factor;
        private decimal markup;

        public UnitConversion()
        {
            Initialize();
        }

        protected sealed override void Initialize()
        {
            itemID = RecordIdentifier.Empty;
            fromUnitID = RecordIdentifier.Empty;
            toUnitID = RecordIdentifier.Empty;
            factor = 0;
            markup = 0;
        }

        public override RecordIdentifier ID
        {
            get
            {                
                return new RecordIdentifier(ItemID, FromUnitID, ToUnitID);
            }
            set
            {
                base.ID = value;
            }
        }

        /// <summary>
        /// The Item ID. If this is null, the unit conversion rule is considered global
        /// </summary>
        [DataMember]
        public RecordIdentifier ItemID
        {
            get { return itemID; }
            set
            {
                if (itemID != value)
                {
                    itemID = value;
                    PropertyChanged("ITEMID", value);
                }                
            }
        }
        public string ItemName { get; set; }

        /// <summary>
        /// The Unit ID from which the conversion is made
        /// </summary>
        [DataMember]
        public RecordIdentifier FromUnitID
        {
            get { return fromUnitID; }
            set
            {
                if (fromUnitID != value)
                {
                    fromUnitID = value;
                    PropertyChanged("FROMUNIT", value);
                }
            }
        }
        public string FromUnitName { get; set; }

        /// <summary>
        /// The Unit ID to which the conversion is made
        /// </summary>
        [DataMember]
        public RecordIdentifier ToUnitID
        {
            get { return toUnitID; }
            set
            {
                if (toUnitID != value)
                {
                    toUnitID = value;
                    PropertyChanged("TOUNIT", value);
                }
            }
        }
        public string ToUnitName { get; set; }

        /// <summary>
        /// Conversion factor
        /// </summary>
        [DataMember]
        public decimal Factor
        {
            get { return factor; }
            set
            {
                if (factor != value)
                {
                    factor = value;
                    PropertyChanged("FACTOR", value);
                }
            }
        }

        public decimal Markup
        {
            get { return markup; }
            set
            {
                if (markup != value)
                {
                    markup = value;
                    PropertyChanged("MARKUP", value);
                }
            }
        }

        public decimal UnitPrice { get; set; }
    }
}