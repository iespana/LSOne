#if !MONO
#endif
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Exceptions;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using LSOne.Utilities.GUI;
using System.Runtime.Serialization;
using System.Xml;

[assembly: ContractNamespace("http://www.lsretail.com/lsone/2017/12/entities", ClrNamespace = "LSOne.DataLayer.BusinessObjects")]
namespace LSOne.DataLayer.BusinessObjects
{
    [Serializable]
    [DataContract]
    public class DataEntity : IDataEntity, ICloneable
    {
        private RecordIdentifier id;
        private string text;
        private UsageIntentEnum usageIntent;
        [NonSerialized]
        private readonly CultureInfo xmlCulture;

        protected bool serializing = false;        

        [DataMember]
        public virtual RecordIdentifier ID
        {
            get { return id; }
            set
            {
                id = value;
            }
        }

        [DataMember]
        public virtual string Text
        {
            get { return text; }
            set { text = value; }
        }

        public UsageIntentEnum UsageIntent 
        {
            get {return usageIntent;}
            set { usageIntent = value; }
        }

        public override string ToString()
        {
            return Text;
        }

        protected CultureInfo XmlCulture
        {
            get { return xmlCulture; }
        }

        public DataEntity(RecordIdentifier id, string text)
        {
            this.xmlCulture = new CultureInfo("en-US");

            usageIntent = UsageIntentEnum.Normal;
            this.id = id;
            this.Text = text;
            this.xmlCulture = new CultureInfo("en-US");
        }

        public DataEntity()
        {
            this.xmlCulture = new CultureInfo("en-US");

            Initialize();
        }

        private void Initialize()
        {
            usageIntent = UsageIntentEnum.Normal;
            this.id = RecordIdentifier.Empty;
            this.text = "";
        }

        public virtual string this[int index]
        {
            get
            {
                return (index == 0) ? id.ToString() : ((index == 1) ? text : "");
            }
        }

        public virtual object this[string field]
        {
            get
            {
                var propertyInfo = GetType().GetProperty(field);
                return propertyInfo?.GetValue(this);
            }
            set { }
        }

        public void Validate()
        {
            var res = new List<ValidationResult>();
#if !MONO
            bool valid = Validator.TryValidateObject(this, new ValidationContext(this, null, null), res, true);
#else
            // Set as valid since the validation framework is not supported for Mono at the moment
            bool valid = true;
#endif

            if (!valid)
            {
                throw new BusinessObjectValidationException(res);
            }
        }

        public virtual XElement ToXML(IErrorLog errorLogger = null)
        {
            try
            {
                var xAnswer = new XElement("DataEntity",
                        new XElement("ID", (string)ID),
                        new XElement("Text", Text)
                    );

                return xAnswer;
            }
            catch (Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "DataEntity.ToXml", ex);
            }

            return null;
        }

        public virtual void ToClass(XElement xmlAnswer, IErrorLog errorLogger = null)
        {
            try
            {
                if (xmlAnswer.HasElements)
                {                    
                    IEnumerable<XElement> answerVariables = xmlAnswer.Elements();
                    foreach (XElement answerElem in answerVariables)
                    {
                        if (!answerElem.IsEmpty)
                        {
                            try
                            {
                                switch (answerElem.Name.ToString())
                                {
                                    case "ID":
                                        ID = answerElem.Value;
                                        break;
                                    case "Text":
                                        Text = answerElem.Value;
                                        break;
                                }
                            }
                            catch(Exception ex)
                            {
                                errorLogger?.LogMessage(LogMessageType.Error,answerElem.Name.ToString(),ex);
                            }
                        }
                    }                      
                }
            }
            catch(Exception ex)
            {
                errorLogger?.LogMessage(LogMessageType.Error, "DataEntity.ToClass", ex);
            }
        }

        public virtual object Clone()
        {
            var item = new DataEntity();
            Populate(item);
            return item;
        }

        protected void Populate(DataEntity item)
        {
            item.id = (RecordIdentifier)this.id.Clone();
            item.text = this.text;
            item.usageIntent = this.usageIntent;
        }

		//TODO: Move/use from LSOne.Utilities
        protected static Image FromBase64(string base64Encoded)
        {
            if (string.IsNullOrEmpty(base64Encoded))
                return null;

            try
            {
                var bytes = Convert.FromBase64String(base64Encoded);
                return ImageUtils.ByteArrayToImage(bytes);
            }
            catch { }

            return null;
        }

		//TODO: Move/use from LSOne.Utilities
		protected static string ToBase64(Image defaultItemImage)
        {
            if (defaultItemImage == null)
                return string.Empty;

            try
            {
                var bytes = ImageUtils.ImageToByteArray(defaultItemImage);
                return Convert.ToBase64String(bytes);
            }
            catch { }

            return string.Empty;
        }

        [OnSerializing]
        private void OnSerializing(StreamingContext ctx)
        {
            serializing = true;

            OnSerializing();
        }

        [OnSerialized]
        private void OnSerialized(StreamingContext ctx)
        {
            serializing = false;

            OnSerialized();
        }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext ctx)
        {
            serializing = true;

            Initialize();

            OnDeserializing();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            serializing = false;

            OnDeserialized();
        }

        /// <summary>
        /// Called just before the object is serialized
        /// </summary>
        protected virtual void OnSerializing()
        {
            
        }

        /// <summary>
        /// Called after the object has been serialized
        /// </summary>
        protected virtual void OnSerialized()
        {
            
        }

        /// <summary>
        /// Called just before the object is deserialized
        /// </summary>
        protected virtual void OnDeserializing()
        {
            
        }

        /// <summary>
        /// Called after the object has been deserialized
        /// </summary>
        protected virtual void OnDeserialized()
        {

        }
    }
}
