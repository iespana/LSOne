using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Sequencable
{
    public class NumberSequence : IDataEntity
    {
        RecordIdentifier id;
        string text;

        public NumberSequence() 
            : base()            
        {
            id = "";
            text = "";
            Highest = 0;
            NextRecord = 0;
            Format = "";
            EmbedStoreID = true;
            EmbedTerminalID = true;
            CanBeDeleted = false;
            StoreID = "HO";
        }
        
        public int Highest { get; set; }
        public int NextRecord { get; set; }
        public string Format { get; set; }
        public bool EmbedStoreID { get; set; }
        public bool EmbedTerminalID { get; set; }

        /// <summary>
        /// If true this number sequence record can be deleted using remove button in the number sequences view
        /// </summary>
        public bool CanBeDeleted { get; set; }

        /// <summary>
        /// Default value is HO
        /// </summary>
        public RecordIdentifier StoreID { get; set; }

        #region IDataEntity Members

        public virtual RecordIdentifier ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        public virtual string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }

        public override string ToString()
        {
            return text;
        }

        public virtual string this[int index]
        {
            get { return (index == 0) ? id.ToString() : ((index == 1) ? text : ""); }
        }

        public virtual object this[string field]
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }
        #endregion


        public UsageIntentEnum UsageIntent
        {
            get
            {
                return UsageIntentEnum.Normal;
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void ToClass(System.Xml.Linq.XElement xmlAnswer, IErrorLog errorLogger = null)
        {
            throw new NotImplementedException();
        }

        public System.Xml.Linq.XElement ToXML(IErrorLog errorLogger = null)
        {
            throw new NotImplementedException();
        }
    }
}
