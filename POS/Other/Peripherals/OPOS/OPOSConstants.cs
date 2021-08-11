using System;

namespace LSOne.Peripherals.OPOS
{
    public class OPOSConstants
    {
        public static readonly char ESC = Convert.ToChar(27);

        public static string RegularFont { get { return "|1C"; } }
        public static string BoldFont { get { return "|bC"; } }
        public static string ItalicFont { get { return "|iC"; } }
        public static string SingleUnderLineFont { get { return "|1uC"; } }
        public static string DoubleUnderLineFont { get { return "|2uC"; } }
        public static string DoubleWideFont { get { return "|2C"; } }
        public static string DoubleHighFont { get { return "|3C"; } }
        public static string DoubleHighAndWideFont { get { return "|4C"; } }
        public static string ReverseVideoFont { get { return "|rvC"; } }
        public static string ShadedFont { get { return "|1sC"; } }
        public static string EndFontSequence { get { return "|N"; } }

        public static string HRightAligned { get { return "|RA"; } }
        public static string HLeftAligned { get { return "|LA"; } }
        public static string HCenterAligned { get { return "|CA"; } }




        public static string[] AllFontSequences
        {
            get
            {
                return new[]
                    {
                        RegularFont,
                        BoldFont,
                        ItalicFont,
                        SingleUnderLineFont,
                        DoubleUnderLineFont,
                        DoubleWideFont,
                        DoubleHighFont,
                        DoubleHighAndWideFont,
                        ReverseVideoFont,
                        ShadedFont,
                        EndFontSequence,
                        HRightAligned,
                        HLeftAligned,
                        HCenterAligned
                    };
            }
        }

        public static string CleanOPOSFonts(string text)
        {
            foreach (var fontSequence in AllFontSequences)
            {
                text = text.Replace(fontSequence, "");
            }
            return text.Replace(ESC.ToString(), "");
        }

        public static string CleanAlignments(string text)
        {
            text = text.Replace(HRightAligned, "");
            text = text.Replace(HLeftAligned, "");
            text = text.Replace(HCenterAligned, "");

            return text;
        }
    }
}
