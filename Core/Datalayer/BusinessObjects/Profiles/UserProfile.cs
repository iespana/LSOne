using LSOne.Utilities.DataTypes;
using System;
using LSOne.Utilities.ErrorHandling;
using System.Xml.Linq;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class UserProfile : DataEntity
    {
        public UserProfile(RecordIdentifier id, string text) : this()
        {
            ID = id;
            Text = text;
        }

        public UserProfile() : base()
        {
            MaxLineDiscountAmount = 0.0M;
            MaxTotalDiscountAmount = 0.0M;
            MaxLineDiscountPercentage = 0.0M;
            MaxTotalDiscountPercentage = 0.0M;
            MaxLineReturnAmount = 0.0M;
            MaxTotalReturnAmount = 0.0M;
            KeyboardCode = "";
            KeyboardLayoutName = "";
            LanguageCode = "";
            VisualProfileID = "";
            StoreID = "";
            LayoutID = "";
            VisualProfileName = "";
            StoreName = "";
            LayoutName = "";
        }

        public decimal MaxLineDiscountAmount { get; set; }
        public decimal MaxLineDiscountPercentage { get; set; }
        public decimal MaxTotalDiscountAmount { get; set; }
        public decimal MaxTotalDiscountPercentage { get; set; }
        public decimal MaxLineReturnAmount { get; set; }
        public decimal MaxTotalReturnAmount { get; set; }
        public string KeyboardCode { get; set; }
        public string KeyboardLayoutName { get; set; }
        public string LanguageCode { get; set; }
        public RecordIdentifier VisualProfileID { get; set; }
        public RecordIdentifier StoreID { get; set; }
        public RecordIdentifier LayoutID { get; set; }
        public string VisualProfileName { get; set; }
        public string StoreName { get; set; }
        public string LayoutName { get; set; }
        public bool ProfileIsUsed { get; set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var userProfileElements = element.Elements();
            foreach (XElement current in userProfileElements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "description":
                                Text = current.Value;
                                break;
                            case "profileID":
                                ID = current.Value;
                                break;
                            case "maxLineDiscountAmount":
                                MaxLineDiscountAmount = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "maxLineDiscountPercentage":
                                MaxLineDiscountPercentage = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "maxTotalDiscountAmount":
                                MaxTotalDiscountAmount = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "maxTotalDiscountPercentage":
                                MaxTotalDiscountPercentage = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "maxLineReturnAmount":
                                MaxLineReturnAmount = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "maxTotalReturnAmount":
                                MaxTotalReturnAmount = Convert.ToDecimal(current.Value, XmlCulture);
                                break;
                            case "keyboardCode":
                                KeyboardCode = current.Value;
                                break;
                            case "languageCode":
                                LanguageCode = current.Value;
                                break;
                            case "visualProfileID":
                                VisualProfileID = current.Value;
                                break;
                            case "storeID":
                                StoreID = current.Value;
                                break;
                            case "layoutID":
                                LayoutID = current.Value;
                                break;
                            case "keyboardLayoutName":
                                KeyboardLayoutName = current.Value;
                                break;
                            case "visualProfileName":
                                VisualProfileName = current.Value;
                                break;
                            case "storeName":
                                StoreName = current.Value;
                                break;
                            case "layoutName":
                                LayoutName = current.Value;
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (errorLogger != null)
                        {
                            errorLogger.LogMessage(LogMessageType.Error, current.Name.ToString(), ex);
                        }
                    }
                }
            }
        }

        public override XElement ToXML(IErrorLog errorLogger = null)
        {
            XElement xml = new XElement("userProfile",
                    new XElement("profileID", (string)ID),
                    new XElement("description", Text),
                    new XElement("maxLineDiscountAmount", MaxLineDiscountAmount.ToString(XmlCulture)),
                    new XElement("maxLineDiscountPercentage", MaxLineDiscountPercentage.ToString(XmlCulture)),
                    new XElement("maxTotalDiscountAmount", MaxTotalDiscountAmount.ToString(XmlCulture)),
                    new XElement("maxTotalDiscountPercentage", MaxTotalDiscountPercentage.ToString(XmlCulture)),
                    new XElement("maxLineReturnAmount", MaxLineReturnAmount.ToString(XmlCulture)),
                    new XElement("maxTotalReturnAmount", MaxTotalReturnAmount.ToString(XmlCulture)),
                    new XElement("keyboardCode", KeyboardCode),
                    new XElement("languageCode", LanguageCode),
                    new XElement("visualProfileID", VisualProfileID),
                    new XElement("storeID", StoreID),
                    new XElement("layoutID", LayoutID),
                    new XElement("keyboardLayoutName", KeyboardLayoutName), 
                    new XElement("visualProfileName", VisualProfileName), 
                    new XElement("storeName", StoreName), 
                    new XElement("layoutName", LayoutName));
            return xml;
        }
    }
}
