using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSRetail.StoreController.BusinessObjects.Scheduler.Common
{
    public class ConfigData
    {
        private int tcprouterport = -1;
        private int tcpsrouterport = -1;
      
        /// <summary>
        /// TCPS Router Port
        /// </summary>
        public int RouterPortTCPS
        {
            get { return tcpsrouterport; }
            set { tcpsrouterport = value; }
        }
        /// <summary>
        /// TCP Router Port
        /// </summary>
        public int RouterPortTCP
        {
            get { return tcprouterport; }
            set { tcprouterport = value; }
        }
    }
}
