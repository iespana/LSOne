using System;

namespace LSOne.Services.Interfaces.EventArguments
{
    public class PreRefreshStatusStripArgs : EventArgs
    {
        /// <summary>
        /// Terminal status to display on the POS status bar instead of the default terminal status
        /// </summary>
        public string TerminalStatus { get; set; }

        /// <summary>
        /// Operator status to display on the POS status bar instead of the default operator status
        /// </summary>
        public string OperatorStatus { get; set; }

        /// <summary>
        /// Indicates if the terminal status has been overridden
        /// </summary>
        public bool OverrideTerminalStatus
        {
            get
            {
                return TerminalStatus != "";
            }
        }

        /// <summary>
        /// Indicates if the operator status has been overridden
        /// </summary>
        public bool OverrideOperatorStatus
        {
            get
            {
                return OperatorStatus != "";
            }
        }

        /// <summary>
        /// Represents the base arguments class allowing to override the terminal and operator status bars on the POS
        /// </summary>
        public PreRefreshStatusStripArgs()
        {
            TerminalStatus = "";
            OperatorStatus = "";
        }
    }
}
