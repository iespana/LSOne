using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Services.Interfaces.SupportInterfaces;

namespace LSOne.DataLayer.TransactionObjects
{
    public class LogTransaction : PosTransaction, ILogTransaction
    {
        private List<LogLineItem> logLines;

        public LogTransaction()
        {
            logLines = new List<LogLineItem>();
        }

        public override object Clone()
        {
            LogTransaction transaciton = new LogTransaction();
            Populate(transaciton);
            transaciton.LogLines = logLines;            

            return transaciton;
        }


        public override void Save()
        {
            
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.Log;
        }

        public List<LogLineItem> LogLines
        {
            get { return logLines; }
            set { logLines = value; }
        }

        public void AddLine(string logText)
        {
            logLines.Add(new LogLineItem() {LineId = logLines.Count + 1, LogText = logText});
        }
    }
}
