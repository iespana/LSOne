using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.ErrorHandling;

namespace LSOne.DataLayer.BusinessObjects.Profiles
{
    public class VisualProfile : DataEntity
    {
        
        public enum HardwareTypes
        {
            /// <summary>
            /// 0
            /// </summary>
            Touch = 0,
            /// <summary>
            /// 1
            /// </summary>
            Keyboard = 1
        }

        private int opacity;
        private int receiptPaymentLinesSize;

        public VisualProfile()
            : this(RecordIdentifier.Empty,"",0,0,false,false,false,0,false, ScreenNumberEnum.MainScreen, 30)
        {

        }

        public VisualProfile(RecordIdentifier profileID, string profileName) 
            : this(profileID,profileName,0,0,false,false,false,0,false, ScreenNumberEnum.MainScreen, 30)
        {
            
        }

        public VisualProfile(
            RecordIdentifier profileID, 
            string profileName,
            ResolutionsEnum resolution,
            HardwareTypes terminalType, 
            bool hideCursor,
            bool designAllowedOnPos,
            bool opaqueBackgroundForm,
            int opacity,
            bool useFormBackgroundImage,
            ScreenNumberEnum screenNumber,
            int receiptPaymentLinesSize)
            : base (profileID,profileName)
        {
            this.Resolution = resolution;
            this.TerminalType = terminalType;
            this.HideCursor = hideCursor;
            this.DesignAllowedOnPos = designAllowedOnPos;
            this.OpaqueBackgroundForm = opaqueBackgroundForm;
            this.opacity = opacity;
            this.UseFormBackgroundImage = useFormBackgroundImage;
            this.ScreenNumber = screenNumber;
            this.ReceiptPaymentLinesSize = receiptPaymentLinesSize;
            ReceiptReturnBackgroundImageID = RecordIdentifier.Empty;
            ReceiptReturnBackgroundImageLayout = ImageLayout.Tile;
            ReceiptReturnBorderColor = ColorPalette.NegativeNumber.ToArgb();
            ConfirmButtonStyleID = RecordIdentifier.Empty;
            CancelButtonStyleID = RecordIdentifier.Empty;
            ActionButtonStyleID = RecordIdentifier.Empty;
            NormalButtonStyleID = RecordIdentifier.Empty;
            OtherButtonStyleID = RecordIdentifier.Empty;
            OverridePOSControlBorderColor = false;
            POSControlBorderColor = ColorPalette.POSControlBorderColor.ToArgb();
            OverridePOSSelectedRowColor = false;
            POSSelectedRowColor = ColorPalette.POSSelectedRowColor.ToArgb();
        }

        public ResolutionsEnum Resolution { get; set; }

        /// <summary>
        /// Keyboard or Touch?
        /// </summary>
        public HardwareTypes TerminalType { get; set; }

        /// <summary>
        /// If set to true the display cursor is hidden
        /// </summary>
        public bool HideCursor { get; set; }

        /// <summary>
        /// Is set to true if the POS design can be changed on the POS, else false.
        /// </summary>
        public bool DesignAllowedOnPos { get; set; }

        /// <summary>
        /// Show an opaque background form behind all shown dialogs and forms?
        /// </summary>
        public bool OpaqueBackgroundForm { get; set; }

        /// <summary>
        /// Show the currency symbol on column headers
        /// </summary>
        public bool ShowCurrencySymbolOnColumns{ get; set; }

        /// <summary>
        ///  Which screen should the POS be visible on.
        /// </summary>
        public ScreenNumberEnum ScreenNumber { get; set; }

        /// <summary>
        /// The opacity percentage.
        /// </summary>
        public int Opacity
        {
            get { return opacity; }
            set { opacity = Math.Max(0,Math.Min(value,100)); }
        }

        /// <summary>
        /// Whether to display the Posis girl background image on the respective forms
        /// </summary>
        public bool UseFormBackgroundImage { get; set; }

        /// <summary>
        /// True if the profile is used by a store or terminal
        /// </summary>
        public bool ProfileIsUsed { get; set; }

        /// <summary>
        /// Percent size of the payment lines on the receipt control
        /// Valid values: 30, 40, 50, 60
        /// </summary>
        public int ReceiptPaymentLinesSize
        {
            get { return receiptPaymentLinesSize; }
            set { receiptPaymentLinesSize = Math.Max(30, Math.Min(value, 60)); }
        }

        /// <summary>
        /// ID of the image to use as background in the receipt control when in return mode
        /// </summary>
        public RecordIdentifier ReceiptReturnBackgroundImageID { get; set; }

        /// <summary>
        /// Layout of the background image when the receipt control is in return mode
        /// </summary>
        public ImageLayout ReceiptReturnBackgroundImageLayout { get; set; }

        /// <summary>
        /// Color of the border of the receipt control when in return mode
        /// </summary>
        public int ReceiptReturnBorderColor { get; set; }

        /// <summary>
        /// The ID of the style that should be applied for confirm(OK, Yes) buttons in the POS
        /// </summary>
        public RecordIdentifier ConfirmButtonStyleID { get; set; }

        /// <summary>
        /// The ID of the style that should be applied for cancellation buttons(Cancel, Close, No) in the POS
        /// </summary>
        public RecordIdentifier CancelButtonStyleID { get; set; }

        /// <summary>
        /// The ID of the style that should be applied for action buttons in the POS
        /// </summary>
        public RecordIdentifier ActionButtonStyleID { get; set; }

        /// <summary>
        /// The ID of the style that should be applied for normal buttons in the POS
        /// </summary>
        public RecordIdentifier NormalButtonStyleID { get; set; }

        /// <summary>
        /// The ID of the style that should be applied for other/default buttons in the POS
        /// </summary>
        public RecordIdentifier OtherButtonStyleID { get; set; }

        /// <summary>
        /// If true then the value in POSCONTROLBORDERCOLOR is used for the border color of POS controls
        /// </summary>
        public bool OverridePOSControlBorderColor { get; set; }

        /// <summary>
        /// The color to use for control borders
        /// </summary>
        public int POSControlBorderColor { get; set; }

        /// <summary>
        /// If true then the value in POSSELECTEDROWCOLOR is used for the selected row color of POS list views
        /// </summary>
        public bool OverridePOSSelectedRowColor{ get; set; }

        /// <summary>
        /// The color to use for the selected row in POS list views
        /// </summary>
        public int POSSelectedRowColor { get; set; }

        public override void ToClass(XElement element, IErrorLog errorLogger = null)
        {
            IEnumerable<XElement> elements = element.Elements();
            foreach (XElement current in elements)
            {
                if (!current.IsEmpty)
                {
                    try
                    {
                        switch (current.Name.ToString())
                        {
                            case "visualProfileID":
                                ID = current.Value;
                                break;
                            case "visualProfileName":
                                Text = current.Value;
                                break;
                            case "resolution":
                                Resolution = (ResolutionsEnum) Convert.ToInt32(current.Value);
                                break;
                            case "terminalType":
                                TerminalType = (HardwareTypes) Convert.ToInt32(current.Value);
                                break;
                            case "hideCursor":
                                HideCursor = current.Value != "false";
                                break;
                            case "designAllowedOnPos":
                                DesignAllowedOnPos = current.Value != "false";
                                break;
                            case "opaqueBackgroundForm":
                                OpaqueBackgroundForm = current.Value != "false";
                                break;
                            case "showCurrencySymbolOnColumns":
                                ShowCurrencySymbolOnColumns = current.Value != "false";
                                break;
                            case "screenNumber":
                                ScreenNumber = (ScreenNumberEnum)Convert.ToInt32(current.Value);
                                break;
                            case "opacity":
                                Opacity = Convert.ToInt32(current.Value);
                                break;
                            case "useFormBackgroundImage":
                                UseFormBackgroundImage = current.Value != "false";
                                break;
                            case "profileIsUsed":
                                ProfileIsUsed = current.Value != "false";
                                break;
                            case "receiptPaymentLinesSize":
                                ReceiptPaymentLinesSize = Convert.ToInt32(current.Value);
                                break;
                            case "receiptReturnBackgroundImageID":
                                ReceiptReturnBackgroundImageID = current.Value;
                                break;
                            case "receiptReturnBackgroundImageLayout":
                                ReceiptReturnBackgroundImageLayout = (ImageLayout)Convert.ToInt32(current.Value);
                                break;
                            case "receiptReturnBorderColor":
                                ReceiptReturnBorderColor = Convert.ToInt32(current.Value);
                                break;
                            case "confirmButtonStyleID":
                                ConfirmButtonStyleID = current.Value;
                                break;
                            case "cancelButtonStyleID":
                                CancelButtonStyleID = current.Value;
                                break;
                            case "actionButtonStyleID":
                                ActionButtonStyleID = current.Value;
                                break;
                            case "normalButtonStyleID":
                                NormalButtonStyleID = current.Value;
                                break;
                            case "otherButtonStyleID":
                                OtherButtonStyleID = current.Value;
                                break;
                            case "overridePOSControlBorderColor":
                                OverridePOSControlBorderColor = current.Value != "false";
                                break;
                            case "posControlBorderColor":
                                POSControlBorderColor = Convert.ToInt32(current.Value);
                                break;
                            case "overridePOSSelectedRowColor":
                                OverridePOSSelectedRowColor = current.Value != "false";
                                break;
                            case "posSelectedRowColor":
                                POSSelectedRowColor = Convert.ToInt32(current.Value);
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
            XElement xml = new XElement("visualProfile",
                    new XElement("visualProfileID", ID),
                    new XElement("visualProfileName", Text),
                    new XElement("resolution", (int)Resolution),
                    new XElement("terminalType", (int)TerminalType),
                    new XElement("hideCursor", HideCursor),
                    new XElement("designAllowedOnPos", DesignAllowedOnPos),
                    new XElement("opaqueBackgroundForm", OpaqueBackgroundForm),
                    new XElement("showCurrencySymbolOnColumns", ShowCurrencySymbolOnColumns),
                    new XElement("screenNumber", (int)ScreenNumber),
                    new XElement("opacity", Opacity),
                    new XElement("useFormBackgroundImage", UseFormBackgroundImage),
                    new XElement("profileIsUsed", ProfileIsUsed),
                    new XElement("receiptPaymentLinesSize", ReceiptPaymentLinesSize),
                    new XElement("receiptReturnBackgroundImageID", ReceiptReturnBackgroundImageID),
                    new XElement("receiptReturnBackgroundImageLayout", (int)ReceiptReturnBackgroundImageLayout),
                    new XElement("receiptReturnBorderColor", ReceiptReturnBorderColor),
                    new XElement("confirmButtonStyleID", (string)ConfirmButtonStyleID),
                    new XElement("cancelButtonStyleID", (string)CancelButtonStyleID),
                    new XElement("actionButtonStyleID", (string)ActionButtonStyleID),
                    new XElement("normalButtonStyleID", (string)NormalButtonStyleID),
                    new XElement("otherButtonStyleID", (string)OtherButtonStyleID),
                    new XElement("overridePOSControlBorderColor", OverridePOSControlBorderColor),
                    new XElement("posControlBorderColor", POSControlBorderColor),
                    new XElement("overridePOSSelectedRowColor", OverridePOSSelectedRowColor),
                    new XElement("posSelectedRowColor", POSSelectedRowColor)
                    ); 
            return xml;
        }     
    }
}
