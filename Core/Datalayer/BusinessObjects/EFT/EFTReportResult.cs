namespace LSOne.DataLayer.BusinessObjects.EFT
{
    public class EFTReportResult
    {
        /// <summary>
        /// Defines if the EFT plugin supports X and Z report functionality
        /// </summary>
        public bool CanPrint { get; set; }

        /// <summary>
        /// Defines if the report was printed by the payment device
        /// </summary>
        public bool PrintedOnDevice { get; set; }

        /// <summary>
        /// Report text to be printed
        /// </summary>
        public string ReceiptText { get; set; }

        public EFTReportResult()
        {
            ReceiptText = "";
        }
    }
}
