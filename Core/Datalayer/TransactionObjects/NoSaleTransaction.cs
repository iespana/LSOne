using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Transactions.Line;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.TransactionObjects
{
    /// <summary>
    /// Used when only a drawer is open and no sale is made.
    /// </summary>
    [Serializable]
    public class NoSaleTransaction : PosTransaction, ICloneable
    {
        protected List<InfoCodeLineItem> infoCodeLines;
        public List<InfoCodeLineItem> InfoCodeLines
        {
            set { infoCodeLines = value; }
            get { return infoCodeLines; }
        }

        public NoSaleTransaction()
        {
            infoCodeLines = new List<InfoCodeLineItem>();
        }

        public override void Save()
        {
        }

        public override TypeOfTransaction TransactionType()
        {
            return TypeOfTransaction.OpenDrawer;
        }

        public override object Clone()
        {
            NoSaleTransaction transaciton = new NoSaleTransaction();
            Populate(transaciton);
            return transaciton;
        }

        public override System.Xml.Linq.XElement ToXML(IErrorLog errorLogger = null)
        {
            return base.ToXML(errorLogger);
        }

        public override void ToClass(System.Xml.Linq.XElement xmlTrans, IErrorLog errorLogger)
        {
            if (xmlTrans.HasElements)
            {
                base.ToClass(xmlTrans, errorLogger);
            }     
        }
        
        /// <summary>
        /// Adds an infocode line to the collection of infocode lines that belongs to this transaction
        /// </summary>
        /// <param name="infoCodeLineItem">Infocode to be added to the transaction</param>
        public void Add(InfoCodeLineItem infoCodeLineItem)
        {
            infoCodeLineItem.LineId = this.infoCodeLines.Count + 1;
            this.infoCodeLines.Add(infoCodeLineItem);
        }

        /// <summary>
        /// Looks through existing infocodes and check if the infocode already exists on the transaction.
        /// </summary>
        /// <param name="infoCodeId"></param>
        /// <returns></returns>
        public Boolean InfoCodeNeeded(string infoCodeId)
        {
            foreach (InfoCodeLineItem infoCodeLineItem in this.infoCodeLines)
            {
                if (infoCodeLineItem.InfocodeId == infoCodeId)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
