using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects.Fiscal
{
    public class FiscalLogEntity : DataEntity
    {
        public FiscalLogEntity()
            : base()
        {
            ID = new RecordIdentifier();
            EntryDate = DateTime.MinValue;
            PrintString = "";
            Operation = POSOperations.NoOperation;
        }

        public DateTime EntryDate { get; set; }
        public string PrintString { get; set; }
        public POSOperations Operation { get; set; }

        public override string ToString()
        {
            string outputString = "".PadLeft(40, '=') + Environment.NewLine;

            if (Operation == POSOperations.NoOperation)
            {
                outputString += PrintString + Environment.NewLine;
            }
            else
            {
                outputString += Properties.Resources.Date.PadRight(10, '.') + ":\t" + EntryDate.ToString("d") + Environment.NewLine;
                outputString += Properties.Resources.Time.PadRight(10, '.') + ":\t" + EntryDate.ToString("t") + Environment.NewLine;
                outputString += Properties.Resources.Operation.PadRight(10, '.') + ":\t" + PrintString + "(" + Conversion.ToStr((int) Operation) + ")" + Environment.NewLine;
            }
            return outputString;
        }

        /// <summary>
        /// Returns a string similar to 
        /// ========================================
        /// Date......:	8/14/2019
        /// Time......:	5:38 PM
        /// Operation.:	(Item sale - 100)
        /// </summary>
        /// <param name="operationsNames">A dictionary with defined system operations info</param>
        /// <returns></returns>
        public string ToStringWithOperationNames(Dictionary<string, string> operationsNames)
        {
            string outputString = "".PadLeft(40, '=') + Environment.NewLine;

            if (Operation == POSOperations.NoOperation)
            {
                outputString += PrintString + Environment.NewLine;
            }
            else
            {
                var operationId = Conversion.ToStr((int)Operation);
                var operationNameAndSeparator = operationsNames.ContainsKey(operationId) ? operationsNames[operationId] + " - " : string.Empty;
                outputString += Properties.Resources.Date.PadRight(10, '.') + ":\t" + EntryDate.ToString("d") + Environment.NewLine;
                outputString += Properties.Resources.Time.PadRight(10, '.') + ":\t" + EntryDate.ToString("t") + Environment.NewLine;
                outputString += Properties.Resources.Operation.PadRight(10, '.') + ":\t" + PrintString + "(" + operationNameAndSeparator + operationId + ")" + Environment.NewLine;
            }
            return outputString;
        }
    }
}
