using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSOne.DataLayer.BusinessObjects
{
    public class StatementHeader : DataEntity
    {
        public StatementHeader()
            : base()
        {
            StoreID = "";
        }

        public DateTime StartingTime { get; set; }
        public DateTime EndingTime { get; set; }
        public RecordIdentifier StoreID { get; set; }

        public override string Text
        {
            get
            {
                return StartingTime.ToShortDateString() + " " + StartingTime.ToShortTimeString() + " - " + EndingTime.ToShortDateString() + " " + EndingTime.ToShortTimeString(); 
            }
        }
    }
}
