using System;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.GenericConnector.DataEntities
{
    public class Country : IDataEntity
    {
        RecordIdentifier id;
        string text;

        public Country(RecordIdentifier id, string text)
        {
            this.id = id;
            this.text = text;
        }

        public Country()
        {
            id = RecordIdentifier.Empty;
            text = "";
        }

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
