namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum ReceiptSettingsEnum
    {
        /// <summary>
        /// The receipt should be printed on the POS
        /// </summary>
        Printed = 0,

        /// <summary>
        /// The receipt should be sent as an email to the customer
        /// </summary>
        Email = 1,

        /// <summary>
        /// The receipt should both be printed on the POS as well as sent as an email to the customer
        /// </summary>
        PrintAndEmail = 2,

        /// <summary>
        /// The email settings will be ignored and only Printing will be done. This is set when the POS cannot connect to the site service
        /// </summary>
        Ignore = 99
    }

    /// <summary>
    /// The options that can be set in the Site service profile on the behaviour of sending receipts through emails
    /// </summary>
    public enum ReceiptEmailOptionsEnum
    {
        /// <summary>
        /// Emails are never sent
        /// </summary>
        Never = 0,
        /// <summary>
        /// The user is always prompted if the receipt should be sent as email
        /// </summary>
        Always = 1,
        /// <summary>
        /// The user is prompted if a customer is on the sale
        /// </summary>
        OnlyToCustomers = 2,
        /// <summary>
        /// The user is never prompted but can be sent through an operation on the POS
        /// </summary>
        OnRequest = 3
    }

    public enum ReceiptEmailParameterEnum
    {
        LastReceipt = 1,
        SearchReceipt = 2,
        CurrentReceipt = 3
    }
  
}
