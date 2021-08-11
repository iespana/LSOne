using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Utilities.DataTypes;
using System;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ITenderLineItem : ILineItem
    {
        /// <summary>
        /// If true then this is a deposit line that has already been paid and does not need to be saved again
        /// </summary>
        bool PaidDeposit { get; set; }


        /// <summary>
        /// Has the points that were calculated for this specific line item
        /// </summary>
        ILoyaltyItem LoyaltyPoints { get; set; }

        /// <summary>
        /// The transaction this line belongs to
        /// </summary>
        IPosTransaction Transaction { get; set; }

        /// <summary>
        /// The id for the Tendertype
        /// </summary>
        string TenderTypeId { get; set; }

        /// <summary>
        /// Payment amount
        /// </summary>
        decimal Amount { get; set; }

        /// <summary>
        /// The amount in a foreign currency.
        /// </summary>
        decimal ForeignCurrencyAmount { get; set; }

        /// <summary>
        /// The amount in the company currency as calculated from the entry in CompanyInfo table.
        /// </summary>
        decimal CompanyCurrencyAmount { get; set; }

        /// <summary>
        /// The exchange rate between any paid amount and the company currency.
        /// </summary>
        decimal ExchrateMST { get; set; }

        /// <summary>
        /// The exchange rate used to calculate the amount.
        /// </summary>
        decimal ExchangeRate { get; set; }

        /// <summary>
        /// The code to identify the currency.
        /// </summary>
        string CurrencyCode { get; set; }

        /// <summary>
        /// Should the drawer be opened?
        /// </summary>
        bool OpenDrawer { get; set; }

        /// <summary>
        /// If 0 > change - amount >= minimumChangeAmount, then
        /// </summary>     
        string ChangeTenderID { get; set; }

        /// <summary>
        /// changeTenderID will be used as changeID, else tenderTypeID will be used 
        /// </summary>
        decimal MinimumChangeAmount { get; set; }

        string AboveMinimumTenderId { get; set; }

        /// <summary>
        /// Is the tender item a change back tender line - needed for Return Transactions
        /// </summary>                                     
        bool ChangeBack { get; set; }

        /// <summary>
        /// Infocode comment on the tender line
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// The type of tender line this is. Used for rebuilding 
        /// </summary>
        TenderTypeEnum TypeOfTender { get; }

        /// <summary>
        /// The unique ID of this tender line. This is not the same as <see cref="ILineItem.LineId"/> since this is ID is not tied to the position of this 
        /// line within the transaction. This is not saved to the database and is only used at runtime to uniquely identify the line
        /// </summary>
        Guid ID { get; set; }
    }
}
