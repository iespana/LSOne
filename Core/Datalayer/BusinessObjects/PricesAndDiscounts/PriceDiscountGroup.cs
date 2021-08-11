using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.PricesAndDiscounts")]
namespace LSOne.DataLayer.BusinessObjects.PricesAndDiscounts
{
    /// <summary>
    /// These groups can be used either for a customer discount (line, multi line or total discount) 
    /// or for a price group that can be attached to a customer or a store
    /// </summary>
    /// 
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class PriceDiscountGroup : OptimizedUpdateDataEntity
    {
        // ID consists of Module, Type, GroupID

        /// <summary>
        /// The constructor for the object. Sets IncludeTax as a default to false
        /// </summary>
        public PriceDiscountGroup()
            : base()
        {
            Initialize();
        }


        /// <summary>
        /// The description of the retail group
        /// </summary>
        [DataMember]
        [StringLength(60)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    PropertyChanged("NAME", value);
                    base.Text = value;
                }
            }
        }

        protected override void Initialize()
        {
            IncludeTax = false;
            GroupID = RecordIdentifier.Empty;
        }

        /// <summary>
        /// The primary key for the group. The key consists of: Module, Type and GroupID
        /// </summary>
        [RecordIdentifierValidation(20)]
        [IgnoreDataMember]
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier((int)Module,
                    new RecordIdentifier((int)Type, GroupID));
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
        /// IS OBSOLETE - EVEN IF SET THIS IS NOT USED BY THE POS
        /// CONFIGURATIONS ON THE STORE DECIDE IF THE PRICE INCLUDING TAX IS USED OR NOT
        /// </summary>
        [IgnoreDataMember]
        public bool IncludeTax { get; set; }

        /// <summary>
        /// Sets if the group consists of items or customers
        /// </summary>
        [DataMember(IsRequired = true)]
        public PriceDiscountModuleEnum Module { get; set; }

        /// <summary>
        /// What type of discount group is this; Line discount group, Price group and etc <see cref="PriceDiscGroupEnum"/>
        /// </summary>
        [DataMember(IsRequired = true)]

        public PriceDiscGroupEnum Type { get; set; }

        /// <summary>
        /// The ID of the group
        /// </summary>
        [DataMember(IsRequired = true)]

        public RecordIdentifier GroupID { get; set; }
    }
}