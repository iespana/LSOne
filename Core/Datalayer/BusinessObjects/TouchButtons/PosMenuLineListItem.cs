using System.Windows.Forms;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    /// <summary>
    /// A class used to display pos menu lines in a list view. It only contains the fields that are useful to the user when viewing a list of pos menu lines.
    /// Detailed information about the lines such as button color, gradient etc are omitted.
    /// </summary>
    public class PosMenuLineListItem : DataEntity
    {

        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(MenuID, Sequence);
            }
            set
            {
                base.ID = value;
            }
        }  

        public PosMenuLineListItem()
        {
            MenuID = "";
            Sequence = "";
            KeyNo = 0;
            OperationName = "";
            OperationLookupType = LookupTypeEnum.None;
            Parameter = "";
            ItemName = "";
            PaymentTypeName = "";
            PosMenuName = "";
            InfocodeName = "";
            PosMenuAndButtonGridPosMenuName = "";
            SuspensionTypeName = "";
            BlankOperationName = "";
            BlankOperationParameter = "";
            IncomeExpenseAccountName = "";
            StorePaymentAndAmountPaymentTypeName = "";
            HospitalityOperationName = "";
            ManuallyTriggeredPeriodicDiscountName = "";
            ParameterItemID = "";
        }

        /// <summary>
        /// The POS menu that this line belongs to
        /// </summary>
        public RecordIdentifier MenuID { get; set; }

        /// <summary>
        /// The sequence, this is part of the primary key
        /// </summary>
        public RecordIdentifier Sequence { get; set; }

        /// <summary>
        /// The placement of this line
        /// </summary>
        public int KeyNo { get; set; }

        /// <summary>
        ///  The Id of the operation that is to run
        /// </summary>
        public RecordIdentifier Operation { get; set; }

        /// <summary>
        /// The name of the POS operation that this line should run
        /// </summary>
        public string OperationName { get; set; }

        /// <summary>
        /// The lookup type for the POS operation that this line runs
        /// </summary>
        public LookupTypeEnum OperationLookupType { get; set; }

        /// <summary>
        /// The parameter value for the POS operation
        /// </summary>
        public string Parameter { get; set; }

        /// <summary>
        /// The name of the retail item used for lookup type RetailItems
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// The name of the payment type used for lookup type StorePaymentTypes
        /// </summary>
        public string PaymentTypeName { get; set; }

        /// <summary>
        /// The name of the pos menu used for lookup type PosMenu
        /// </summary>
        public string PosMenuName { get; set; }

        /// <summary>
        /// The name of the infocode used for lookup type TaxGroupInfocode
        /// </summary>
        public string InfocodeName { get; set; }

        /// <summary>
        /// The name of the pos menu used for lookup type PosMenuAndButtonGrid
        /// </summary>
        public string PosMenuAndButtonGridPosMenuName { get; set; }

        /// <summary>
        /// The name of the suspension type used for lookup type SuspendTransactionTypes
        /// </summary>
        public string SuspensionTypeName { get; set; }

        /// <summary>
        /// The name of the blank operation used for lookup type BlankOperations
        /// </summary>
        public string BlankOperationName { get; set; }

        /// <summary>
        /// The parameter for the blank operation
        /// </summary>
        public string BlankOperationParameter { get; set; }

        /// <summary>
        /// The name of the income/expense account used for lookup types IncomeAccount and ExpenseAccount
        /// </summary>
        public string IncomeExpenseAccountName { get; set; }

        /// <summary>
        /// The name of the payment type used for lookup type StorePaymentAndAmount
        /// </summary>
        public string StorePaymentAndAmountPaymentTypeName { get; set; }

        /// <summary>
        /// Contains the name of the hospitality operation name
        /// </summary>
        public string HospitalityOperationName { get; set; }

        /// <summary>
        /// The name of the periodic discount used for lookup type ManuallyTriggerPeriodicDiscount
        /// </summary>
        public string ManuallyTriggeredPeriodicDiscountName { get; set; }

        public Keys KeyMapping { get; set; }
        public RecordIdentifier StyleID { get; set; }

        public string ParameterItemID { get; set; }
    }
}
