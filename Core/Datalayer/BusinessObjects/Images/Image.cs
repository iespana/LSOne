using System.Xml.Linq;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;
using System;

namespace LSOne.DataLayer.BusinessObjects.Images
{
    public class Image : DataEntity
    {
        public Image() : base()
        {
            Guid = Guid.Empty;
        }

        public System.Drawing.Image Picture { get; set; }
        public ImageTypeEnum ImageType { get; set; }
        public RecordIdentifier BackgroundStyle { get; set; }
        //External property
        public bool IsImageUsed { get; set; }
        //External property - taken from background style
        public int BackColor { get; set; }
        public Guid Guid { get; set; } 

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            var elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "pictureID":
                                ID = current.Value;
                                break;
                            case "pictureDescription":
                                Text = current.Value;
                                break;
                            case "imageType":
                                ImageType = (ImageTypeEnum)Convert.ToInt32(current.Value);
                                break;
                            case "backgroundStyleID":
                                BackgroundStyle = current.Value;
                                break;
                            case "picture":
                                Picture = FromBase64(current.Value);
                                break;
                            case "guid":
                                Guid = Guid.Parse(current.Value);
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
            var xImage = new XElement("image",
                new XElement("pictureID", ID),
                new XElement("pictureDescription", Text),
                new XElement("imageType", (int)ImageType),
                new XElement("backgroundStyleID", BackgroundStyle),
                new XElement("picture", ToBase64(Picture)),
                new XElement("guid", Guid));

            return xImage;
        }
    }
}
