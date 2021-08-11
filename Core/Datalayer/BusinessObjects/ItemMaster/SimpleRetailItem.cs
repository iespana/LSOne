using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.Utilities.Helpers;
using LSOne.Utilities.Validation;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects.ItemMaster")]
namespace LSOne.DataLayer.BusinessObjects.ItemMaster
{
    /// <summary>
    /// A class that represents a minor subset of a retail item. This class is used in some queries that return a list of retail items.
    /// 
    /// When using the features of <see cref="OptimizedUpdateDataEntity"/> the following properties must be populated:
    /// <list type="bullet">
    /// <item>
    /// <description>ID</description>
    /// </item>
    /// </list>
    /// </summary>
    [DataContract]
    [KnownType(typeof(RecordIdentifier))]
    public class SimpleRetailItem :  OptimizedUpdateDataEntity
    {
        protected string nameAlias;
        protected ItemTypeEnum itemType;
        protected RecordIdentifier retailGroupMasterID;
        protected RecordIdentifier salesTaxItemGroupID;
        protected string validationPeriodID;
        protected decimal salesPrice;
        protected decimal salesPriceIncludingTax;
        protected string variantName;
        protected RecordIdentifier headerItemID;


        /// <summary>
        /// Default constructor for the ItemListItem
        /// </summary>
        public SimpleRetailItem()
            : base()
        {
            InitializeBase();            
        }

        protected override void InitializeBase()
        {
            nameAlias = "";
            itemType = ItemTypeEnum.Item;
            validationPeriodID = "";
            salesTaxItemGroupID = RecordIdentifier.Empty;
            variantName = "";
            headerItemID = Guid.Empty;
            retailGroupMasterID = Guid.Empty;
            validationPeriodID = "";
            salesPrice = 0M;
            salesPriceIncludingTax = 0M;
        }

        public SimpleRetailItem(RetailItem retailItem) : base()
        {
            this.ID = retailItem.ID;
            this.MasterID = retailItem.MasterID;
            this.HeaderItemID = retailItem.HeaderItemID;
            this.Text = retailItem.Text;
            this.VariantName = retailItem.VariantName;
            this.ItemType = retailItem.ItemType;
            this.NameAlias = retailItem.NameAlias;
            this.SalesPrice = retailItem.SalesPrice;
            this.SalesPriceIncludingTax = retailItem.SalesPriceIncludingTax;
            this.SalesTaxItemGroupID = retailItem.SalesTaxItemGroupID;
            this.Deleted = retailItem.Deleted;
            this.RetailGroupMasterID = retailItem.RetailGroupMasterID;
            this.RetailDepartmentMasterID = retailItem.RetailDepartmentMasterID;
            this.RetailDivisionMasterID = retailItem.RetailDivisionMasterID;
        }

		[DataMember]
        [RecordIdentifierValidation(Utilities.DataTypes.RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier MasterID
        {
            get;
            set;
        }

        [DataMember]
        [RecordIdentifierValidation(Utilities.DataTypes.RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier HeaderItemID
        {
            get { return headerItemID; }
            set
            {
                if (headerItemID != value)
                {
                    PropertyChanged("HEADERITEMID", value);
                    headerItemID = value;
                }
            }
        }

        /// <summary>
        /// The name alias of the retail item.
        /// </summary>
        [DataMember]
        [StringLength(60)]
        public string NameAlias
        {
            get
            {
                return nameAlias;
            }
            set
            {
                if (nameAlias != value)
                {
                    PropertyChanged("NAMEALIAS", value);
                    nameAlias = value;
                }
            }
        }

        /// <summary>
        /// Type of the retail item, N/A, Item, BOM, Service.
        /// </summary>
        [DataMember]
        public ItemTypeEnum ItemType
        {
            get{ return itemType; }
            set
            {
                if (itemType != value)
                {
                    PropertyChanged("ITEMTYPE", value);
                    itemType = value;
                }
            }
        }

        /// <summary>
        /// The ID of the retail sub-group the item belongs to
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]        
        public RecordIdentifier RetailDivisionMasterID
        {
            get;
            set;
        }

        /// <summary>
        /// The ID of the retail group the item belongs to
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier RetailGroupMasterID
        {
            get { return retailGroupMasterID; }
            set { retailGroupMasterID = value; }
        }

        /// <summary>
        /// The ID of the retail department the item belongs to
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(RecordIdentifier.RecordIdentifierType.Guid)]
        public RecordIdentifier RetailDepartmentMasterID
        {
            get;
            set;
        }

        /// <summary>
        /// The ID of the tax group the item belongs to
        /// </summary>
        [DataMember]
        [RecordIdentifierValidation(20)]
        public RecordIdentifier SalesTaxItemGroupID
        {
            get { return salesTaxItemGroupID; }
            set
            {
                if (salesTaxItemGroupID != value)
                {
                    PropertyChanged("SALESTAXITEMGROUPID", value);
                    salesTaxItemGroupID = value;
                }
            }
        }

        [DataMember]
        public string ValidationPeriodID
        {
            get { return validationPeriodID; }
            set
            {
                if (validationPeriodID != value)
                {
                    PropertyChanged("VALIDATIONPERIODID", value);
                    validationPeriodID = value;
                }
            }
        }

        /// <summary>
        /// Item price without modifications
        /// </summary>
        [DataMember]
        public decimal SalesPrice
        {
            get { return salesPrice; }
            set
            {
                if (salesPrice != value)
                {
                    PropertyChanged("SALESPRICE", value);
                    salesPrice = value;
                }
            }
        }
        
        /// <summary>
        /// Item price without modifications
        /// </summary>
        [DataMember]
        public decimal SalesPriceIncludingTax
        {
            get { return salesPriceIncludingTax; }
            set
            {
                if (salesPriceIncludingTax != value)
                {
                    PropertyChanged("SALESPRICEINCLTAX", value);
                    salesPriceIncludingTax = value;
                }
            }
        }

        [DataMember]
        [StringLength(60)]
        public string VariantName
        {
            get { return variantName; }
            set
            {
                if (variantName != value)
                {
                    PropertyChanged("VARIANTNAME", value);
                    variantName = value;
                }
            }
        }

        [DataMember]
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets the <see cref="ItemTypeEnum">item type</see> description of the current <see cref="SimpleRetailItem">retail item</see> 
        /// </summary>
        public string ItemTypeDescription
        {
            get
            {
                return ItemTypeHelper.ItemTypeToString(ItemType);
            }
        }
    }
}