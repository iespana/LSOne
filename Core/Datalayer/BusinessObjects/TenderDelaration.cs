using System;
using System.Collections.Generic;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.BusinessObjects
{
    public class Tenderdeclaration : DataEntity
    {


        public override RecordIdentifier ID
        {
            get
            {
                return CountedTime;
            }
            set
            {
                if (!serializing)
                {
                    throw new NotImplementedException();
                }
            }
        }


        public string TerminalID { get; set; }
        public string StoreID { get; set; }
        public DateTime CountedTime { get; set; }
        public string StatementID { get; set; }
        public List<TenderdeclarationLine> TenderDeclarationLines { get; set; }
        
        public Tenderdeclaration()
        {
            TenderDeclarationLines = new List<TenderdeclarationLine>();
            TerminalID = "";
            StoreID = "";
            StatementID = "";
        }

    }
}
