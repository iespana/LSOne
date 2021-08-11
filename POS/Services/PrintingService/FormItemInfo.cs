using System;
using System.Data;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.Peripherals.OPOS;

namespace LSOne.Services
{
    public partial class FormItemInfo
    {
        private int length;
        private FormFontStyles formFontStyle; // Integer values form ReportFontStyles enum in ReportDesigner

        public int SizeFactor { get; private set; }

        public string Variable { get; set; }

        public bool IsVariable { get; set; }

        public int LineIndex { get; set; }

        public int CharIndex { get; set; }

        public string ValueString { get; set; }

        public valign VertAlign { get; set; }

        public char Fill { get; set; }

        public int Length
        {
            get
            {
                // if font is bold then letters occupy more space and therefore have more length
                return length / SizeFactor;
            }
            set
            {
                length = value;
            }
        }

        public string Prefix { get; set; }

        public FormItemInfo(DataRow formItem)
        {
            SizeFactor = 1;
            try
            {
                this.CharIndex = Convert.ToInt16(formItem["nr"]);
                this.ValueString = formItem["value"].ToString();
                if (formItem["valign"].ToString() == "right")
                    this.VertAlign = valign.right;
                else if (formItem["valign"].ToString() == "left")
                    this.VertAlign = valign.left;
                else if (formItem["valign"].ToString() == "centre")
                    this.VertAlign = valign.center;
                this.Fill = Convert.ToChar(string.Concat(formItem["fill"]));
                this.Variable = formItem["variable"].ToString();
                this.IsVariable = formItem["variable"].ToString() != "";
                this.Prefix = formItem["prefix"].ToString();
                
                formFontStyle = (FormFontStyles)Convert.ToInt32(formItem["FontStyle"].ToString());
                switch (formFontStyle)
                {
                    case FormFontStyles.Regular:
                    case FormFontStyles.Italic:
                    case FormFontStyles.Underline:
                    case FormFontStyles.Bold:
                    case FormFontStyles.DoubleUnderline:
                    case FormFontStyles.ReverseVideo:
                    case FormFontStyles.Shaded:
                        SizeFactor = 1;
                        break;
                    case FormFontStyles.DoubleHigh:
                    case FormFontStyles.DoubleHighAndWide:
                    case FormFontStyles.DoubleWide:
                        SizeFactor = 2;
                        break;
                }

                this.length = (Convert.ToInt32(formItem["length"]) + this.Prefix.Length) * this.SizeFactor;
            }
            catch (Exception)
            {
            }
        }

        public static string RegularFont { get { return OPOSConstants.RegularFont; } }
        public static string BoldFont { get { return OPOSConstants.BoldFont; } }
        public static string ItalicFont { get { return OPOSConstants.ItalicFont; } }
        public static string SingleUnderLineFont { get { return OPOSConstants.SingleUnderLineFont; } }
        public static string DoubleUnderLineFont { get { return OPOSConstants.DoubleUnderLineFont; } }
        public static string DoubleWideFont { get { return OPOSConstants.DoubleWideFont; } }
        public static string DoubleHighFont { get { return OPOSConstants.DoubleHighFont; } }
        public static string DoubleHighAndWideFont { get { return OPOSConstants.DoubleHighAndWideFont; } }
        public static string ReverseVideoFont { get { return OPOSConstants.ReverseVideoFont; } }
        public static string ShadedFont { get { return OPOSConstants.ShadedFont; } }
        public static string EndFontSequence { get { return OPOSConstants.EndFontSequence; } }

        internal string ApplyFont(string data)
        {
            return PrintingService.esc + GetStartFontSequence() + data + PrintingService.esc + EndFontSequence;
        }

        private string GetStartFontSequence()
        {
            switch (formFontStyle)
            {
                case FormFontStyles.DoubleWide:
                    return DoubleWideFont;
                case FormFontStyles.Italic:
                    return ItalicFont; 
                case FormFontStyles.Underline:
                    return SingleUnderLineFont;
                case FormFontStyles.Bold:
                    return BoldFont;
                case FormFontStyles.DoubleUnderline:
                    return DoubleUnderLineFont;
                case FormFontStyles.DoubleHigh:
                    return DoubleHighFont;
                case FormFontStyles.DoubleHighAndWide:
                    return DoubleHighAndWideFont;
                case FormFontStyles.ReverseVideo:
                    return ReverseVideoFont;
                case FormFontStyles.Shaded:
                    return ShadedFont;
            }
            return RegularFont;
        }

        public FormItemInfo()
        {
            SizeFactor = 1;
        }
    }
}
