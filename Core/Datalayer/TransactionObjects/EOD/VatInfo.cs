using LSOne.DataLayer.BusinessObjects.Enums;

namespace LSOne.DataLayer.TransactionObjects.EOD
{
    /// <summary>
    /// Declares the class VatInfo. A linked list of that type is created in the constructor.
    /// A call occurs from ReportData.cs that runs a stored procedure in LSPOSNET to collect the necessary data.
    /// </summary>
    public class VatInfo
    {
        /// <summary>
        /// The tax groups that are existing. When calculating the tax amounts, the results are grouped by these tax groups.
        /// </summary>
        public string vatType = "";

        /// <summary>
        /// Currently not in use. Supposed to carry the percentage symbol (%).
        /// </summary>
        public string vatRate = "";

        /// <summary>
        /// vatValue is not retrieved from the database but it is calculated (in class ReportData.cs) from the 
        /// vat amount divided by the netamount. The result is then rounded.
        /// </summary>
        public decimal vatValue = decimal.Zero;

        /// <summary>
        /// <b>SUM((((S.PRICE * S.QTY) - S.TAXAMOUNT) * -1 ) - S.WHOLEDISCAMOUNTWITHTAX)</b>
        /// <br></br>'S' is the table RBOTRANSACTIONSALESTRANS.
        /// </summary>
        public decimal netAmount = decimal.Zero;

        /// <summary>
        /// <b>SUM(S.TAXAMOUNT * -1))</b> 
        /// <br></br>'S' is the table RBOTRANSACTIONSALESTRANS.
        /// </summary>
        public decimal vatAmount = decimal.Zero;

        /// <summary>
        /// <b>SUM(((S.PRICE * S.QTY) * -1 ) - S.WHOLEDISCAMOUNTWITHTAX</b> 
        /// <br></br>'S' is the table RBOTRANSACTIONSALESTRANS.
        /// </summary>
        public decimal grossAmount = decimal.Zero;

        public string vatGroupName = "";

        public XZDisplayAmounts SaleReturn;
    }
}
