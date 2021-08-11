namespace LSOne.Services.Interfaces.Enums
{
    public enum EMailTruncateSetting
    {
        /// <summary>
        /// Delete each e-mail after sending
        /// </summary>
        Each,

        /// <summary>
        /// Delete all day-old e-mails after sending
        /// </summary>
        Daily,

        /// <summary>
        /// Delete all week-old e-mails after sending
        /// </summary>
        Weekly,

        /// <summary>
        /// Delete all month-old e-mails after sending
        /// </summary>
        Monthly,

        /// <summary>
        /// Never delete e-mails, only mark as sent
        /// </summary>
        Never
    }
}
