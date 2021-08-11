using System.ComponentModel.DataAnnotations;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Validation;
#if !MONO
#endif

namespace LSOne.DataLayer.BusinessObjects.Transactions
{
    public class SuspendedTransactionType : DataEntity
    {

        public SuspendedTransactionType()
            : base()
        {           
            EndofDayCode = SuspendedTransactionsStatementPostingEnum.StoreDefault;

        }

        [RecordIdentifierValidation(40)]
        public override RecordIdentifier ID
        {
            get
            {
                return base.ID;
            }
            set
            {
                base.ID = value;
            }
        }

       [StringLength(60)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }       
        
        public SuspendedTransactionsStatementPostingEnum EndofDayCode{get;set;}  

        public string EndOfDayCodeText
        {
            get
            {
                switch (EndofDayCode)
                {
                    case SuspendedTransactionsStatementPostingEnum.StoreDefault:
                        return Properties.Resources.StoreDefault;

                    case SuspendedTransactionsStatementPostingEnum.TerminalDefault:
                        return Properties.Resources.TerminalDefault;

                    case SuspendedTransactionsStatementPostingEnum.Yes:
                        return Properties.Resources.Yes;

                    case SuspendedTransactionsStatementPostingEnum.No:
                        return Properties.Resources.No;

                    default:
                        return "";
                }
            }
        }
    }
}
