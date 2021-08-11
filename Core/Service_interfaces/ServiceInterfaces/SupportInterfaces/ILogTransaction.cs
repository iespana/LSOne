using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;

namespace LSOne.Services.Interfaces.SupportInterfaces
{
    public interface ILogTransaction : IPosTransaction
    {
        /// <summary>
        /// Contains all log lines to be saved
        /// </summary>
        List<LogLineItem> LogLines { get; set; }

        /// <summary>
        /// Adds a log text to the transaction
        /// </summary>
        /// <param name="logText">The text to log</param>
        void AddLine(string logText);
    }
}
