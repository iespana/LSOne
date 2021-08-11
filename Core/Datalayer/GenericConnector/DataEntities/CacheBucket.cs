using System;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.GenericConnector.DataEntities
{
    public class CacheBucket : IDataEntity
    {
        RecordIdentifier id;
        string text;
        UsageIntentEnum usageIntent;

        public CacheBucket()
            : base()
	    {

	    }   

        public CacheBucket (RecordIdentifier id, string text,object bucketData)
            : base()
	    {
            ID = id;
            Text = text;
            BucketData = bucketData;
	    }   

        public RecordIdentifier ID
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

        public string Text
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

        public string this[int index]
        {
            get { return text; }
        }

        public object this[string field]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object BucketData { get; set; }


        public UsageIntentEnum UsageIntent
        {
            get
            {
                return usageIntent;
            }
            set
            {
                usageIntent = value;
            }
        }

        public override string ToString()
        {
            return Text;
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
