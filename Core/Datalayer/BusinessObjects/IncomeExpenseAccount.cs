using System;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects
{
    public class IncomeExpenseAccount : DataEntity
    {
        private RecordIdentifier storeID;
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(AccountNum, StoreID);
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }

        public override string Text
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }

        public IncomeExpenseAccount()
            : base()
        {
            AccountNum = "";
            Name = "";
            StoreID = "";
            NameAlias = "";
            AccountType = AccountTypeEnum.IncomeAccount;
            LedgerAccount = "";
            MessageLine1 = "";
            MessageLine2 = "";
            SlipText1 = "";
            SlipText2 = "";
            TaxCodeID = "";
            ModifiedDate = new DateTime();
            ModifiedTime = 0;
            ModiefiedBy = "";
            ModifiedTransactionID = "";


        }

        public RecordIdentifier AccountNum { get; set; }
        public string Name { get; set; }
        public RecordIdentifier StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }
        public string NameAlias { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public RecordIdentifier LedgerAccount { get; set; }
        public string MessageLine1 { get; set; }
        public string MessageLine2 { get; set; }
        public string SlipText1 { get; set; }
        public string SlipText2 { get; set; }
        public RecordIdentifier TaxCodeID { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedTime { get; set; }
        public string ModiefiedBy { get; set; }
        public RecordIdentifier ModifiedTransactionID { get; set; }

        /// <summary>
        /// The amount accrued to the account. Is only used for reports and is NOT saved to the database.
        /// </summary>
        public decimal Amount { get; set; }

        public enum AccountTypeEnum
        {
            IncomeAccount = 0,
            ExpenseAccount = 1,
            All =  2
        }

        public string AccountTypeText
        {
            get
            {
                switch (AccountType)
                {
                    case AccountTypeEnum.ExpenseAccount:
                        return Properties.Resources.Expense;

                    case AccountTypeEnum.IncomeAccount:
                        return Properties.Resources.Income;

                    default:
                        return "";
                }
            }
        }

    }
}
