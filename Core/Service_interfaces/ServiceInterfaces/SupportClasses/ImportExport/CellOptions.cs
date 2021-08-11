using System;
using System.Drawing;

namespace LSOne.Services.Interfaces.SupportClasses.ImportExport
{
    public class CellOptions
    {
        public CellOptions()
        {
        }

        public bool HorizontalAlignmentSet { get; private set; }
        private CellHorizontalAlignment horizontalAlignment;

        public CellHorizontalAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set
            {
                HorizontalAlignmentSet = true;
                horizontalAlignment = value;
            }
        }

        public bool VerticalAlignmentSet { get; private set; }
        private CellVerticalAlignment verticalAlignment;

        public CellVerticalAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set
            {
                VerticalAlignmentSet = true;
                verticalAlignment = value;
            }
        }

        public bool FillPatternSet { get; private set; }
        private CellFillPattern fillPattern;

        public CellFillPattern FillPattern
        {
            get { return fillPattern; }
            set
            {
                FillPatternSet = true;
                fillPattern = value;
            }
        }

        public bool RotationSet { get; private set; }
        private int rotation;

        public int Rotation
        {
            get { return rotation; }
            set
            {
                RotationSet = true;
                rotation = value;
            }
        }

        public bool Merge { get; set; }

        public bool ForeColorSet { get; private set; }
        private Color foreColor;

        public Color ForeColor
        {
            get { return foreColor; }
            set
            {
                ForeColorSet = true;
                foreColor = value;
            }
        }

        public bool BackColorSet { get; private set; }
        private Color backColor;

        public Color BackColor
        {
            get { return backColor; }
            set
            {
                BackColorSet = true;
                backColor = value;
            }
        }

        public bool FormatSet { get; private set; }
        private string format;

        public string Format
        {
            get { return format; }
            set
            {
                FormatSet = true;
                format = value;
            }
        }

        public bool FontNameSet { get; private set; }
        private string fontName;

        public string FontName
        {
            get { return fontName; }
            set
            {
                FontNameSet = true;
                fontName = value;
            }
        }

        public bool FontSizeSet { get; private set; }
        private short fontSize;

        public short FontSize
        {
            get { return fontSize; }
            set
            {
                FontSizeSet = true;
                fontSize = value;
            }
        }

        public bool FormatEnumSet { get; private set; }
        private CellFormatEnum formatEnum;

        public CellFormatEnum FormatEnum
        {
            get { return formatEnum; }
            set
            {
                FormatEnumSet = true;
                formatEnum = value;
            }
        }

        public bool FontColorSet { get; private set; }
        private Color fontColor;

        public Color FontColor
        {
            get { return fontColor; }
            set
            {
                FontColorSet = true;
                fontColor = value;
            }
        }

        public bool HyperLinkSet { get; private set; }
        private string hyperLink;

        public string HyperLink
        {
            get { return hyperLink; }
            set
            {
                HyperLinkSet = true;
                hyperLink = value;
            }
        }
    }

    public enum CellHorizontalAlignment
    {
        // Summary:
        //     Aligns data depending on the data type (text, number, etc.)
        General = 0,
        //
        // Summary:
        //     Left alignment.
        Left = 1,
        //
        // Summary:
        //     Center alignment.
        Center = 2,
        //
        // Summary:
        //     Right alignment.
        Right = 3,
        //
        // Summary:
        //     Fill alignment repeats cell data to fill the whole ExcelCell.
        Fill = 4,
        //
        // Summary:
        //     Justify alignment.
        Justify = 5,
        //
        // Summary:
        //     Centered across selection. Multiple cells can be selected but only one should
        //     have value for this alignment to have effect.
        CenterAcross = 6,
        //
        // Summary:
        //     Distributed alignment.
        Distributed = 7,
    }

    public enum CellVerticalAlignment
    {
        // Summary:
        //     Top alignment.
        Top = 0,
        //
        // Summary:
        //     Center alignment.
        Center = 1,
        //
        // Summary:
        //     Bottom alignment.
        Bottom = 2,
        //
        // Summary:
        //     Justify alignment.
        Justify = 3,
        //
        // Summary:
        //     Distributed alignment.
        Distributed = 4,
    }

    public enum CellFillPattern
    {
        // Summary:
        //     No fill pattern.
        None = 0,
        //
        // Summary:
        //     "Solid" fill pattern using foreground color.
        Solid = 1,
        //
        // Summary:
        //     "50% Gray" is Microsoft Excel pattern name, but any color can be used instead
        //     of black as a foreground color.("Gray50" for .xls; "mediumGray" for .xlsx)
        Gray50 = 2,
        //
        // Summary:
        //     "75% Gray" is Microsoft Excel pattern name, but any color can be used instead
        //     of black as a foreground color. ("Gray75" for .xls; "darkGray" for .xlsx)
        Gray75 = 3,
        //
        // Summary:
        //     "25% Gray" is Microsoft Excel pattern name, but any color can be used instead
        //     of black as a foreground color. ("Gray25" for .xls; "lightGray" for .xlsx)
        Gray25 = 4,
        //
        // Summary:
        //     "Horizontal Stripe" pattern. ("HorizontalStripe" for .xls; "darkHorizontal"
        //     for .xlsx)
        HorizontalStripe = 5,
        //
        // Summary:
        //     "Vertical Stripe" pattern. ("VerticalStripe" for .xls; "darkVertical" for
        //     .xlsx)
        VerticalStripe = 6,
        //
        // Summary:
        //     "Reverse Diagonal Stripe" pattern. ("ReverseDiagonalStripe" for .xls; "darkDown"
        //     for .xlsx)
        ReverseDiagonalStripe = 7,
        //
        // Summary:
        //     "Diagonal Stripe" pattern. ("DiagonalStripe" for .xls; "darkUp" for .xlsx)
        DiagonalStripe = 8,
        //
        // Summary:
        //     "Diagonal Crosshatch" pattern. ("DiagonalCrosshatch" for .xls; "darkGrid"
        //     for .xlsx)
        DiagonalCrosshatch = 9,
        //
        // Summary:
        //     "Thick Diagonal Crosshatch" pattern. ("ThickDiagonalCrosshatch" for .xls;
        //     "darkTrellis" for .xlsx)
        ThickDiagonalCrosshatch = 10,
        //
        // Summary:
        //     "Thin Horizontal Stripe" pattern. ("ThinHorizontalStripe" for .xls; "lightHorizontal"
        //     for .xlsx)
        ThinHorizontalStripe = 11,
        //
        // Summary:
        //     "Thin Vertical Stripe" pattern. ("ThinVerticalStripe" for .xls; "lightVertical"
        //     for .xlsx)
        ThinVerticalStripe = 12,
        //
        // Summary:
        //     "Thin Reverse Diagonal Stripe" pattern. ("ThinReverseDiagonalStripe" for
        //     .xls; "lightDown" for .xlsx)
        ThinReverseDiagonalStripe = 13,
        //
        // Summary:
        //     "Thin Diagonal Stripe" pattern. ("ThinDiagonalStripe" for .xls; "lightUp"
        //     for .xlsx)
        ThinDiagonalStripe = 14,
        //
        // Summary:
        //     "Thin Horizontal Crosshatch" pattern. ("ThinHorizontalCrosshatch" for .xls;
        //     "lightGrid" for .xlsx)
        ThinHorizontalCrosshatch = 15,
        //
        // Summary:
        //     "Thin Diagonal Crosshatch" pattern. ("ThinDiagonalCrosshatch" for .xls; "lightTrellis"
        //     for .xlsx)
        ThinDiagonalCrosshatch = 16,
        //
        // Summary:
        //     "12% Gray" is Microsoft Excel pattern name, but any color can be used instead
        //     of black as a foreground color. ("Gray12" for .xls; "gray125" for .xlsx)
        Gray12 = 17,
        //
        // Summary:
        //     "6% Gray" is Microsoft Excel pattern name, but any color can be used instead
        //     of black as a foreground color. ("Gray6" for .xls; "gray0625" for .xlsx)
        Gray6 = 18,
        //
        // Summary:
        //     Angle of the linear gradient - horizontal. Gradient fill from color1 to color2
        //     (Only .xlsx).
        GradientHorizontal = 19,
        //
        // Summary:
        //     Angle of the linear gradient - horizontal. Gradient fill from color2 to color1
        //     (Only .xlsx).
        GradientHorizontalRevers = 20,
        //
        // Summary:
        //     Angle of the linear gradient - horizontal. Gradient fill from color1 to color2
        //     to Color1 (Only .xlsx).
        GradientHorizontalCenter = 21,
        //
        // Summary:
        //     Angle of the linear gradient - vertical. Gradient fill from color1 to color2
        //     (Only .xlsx).
        GradientVertical = 22,
        //
        // Summary:
        //     Angle of the linear gradient - vertical. Gradient fill from color2 to color1
        //     (Only .xlsx).
        GradientVerticalRevers = 23,
        //
        // Summary:
        //     Angle of the linear gradient - vertical. Gradient fill from color1 to color2
        //     to color1 (Only .xlsx).
        GradientVarticalCenter = 24,
        //
        // Summary:
        //     Angle of the linear gradient - diagonal up. Gradient fill from color1 to
        //     color2 (Only .xlsx).
        GradientDiagonalUp = 25,
        //
        // Summary:
        //     Angle of the linear gradient - diagonal up. Gradient fill from color2 to
        //     color1 (Only .xlsx).
        GradientDiagonalUpRevers = 26,
        //
        // Summary:
        //     Angle of the linear gradient - diagonal up. Gradient fill from color1 to
        //     color2 to color1 (Only .xlsx).
        GradientDiagonalUpCenter = 27,
        //
        // Summary:
        //     Angle of the linear gradient - diagonal down. Gradient fill from color1 to
        //     color2 (Only .xlsx).
        GradientDiagonalDown = 28,
        //
        // Summary:
        //     Angle of the linear gradient - diagonal down. Gradient fill from color2 to
        //     color1 (Only .xlsx).
        GradientDiagonalDownRevers = 29,
        //
        // Summary:
        //     Angle of the linear gradient - diagonal up. Gradient fill from color1 to
        //     color2 to color1 (Only .xlsx).
        GradientDiagonalDownCenter = 30,
        //
        // Summary:
        //     Path gradient type means the that the boundary of transition from one color
        //     to the next is a rectangle.  Rectangle placed in the top-left corner.
        GradientFromCornerTopLeft = 31,
        //
        // Summary:
        //     Path gradient type means the that the boundary of transition from one color
        //     to the next is a rectangle.  Rectangle placed in the top-right corner.
        GradientFromCornerTopRight = 32,
        //
        // Summary:
        //     Path gradient type means the that the boundary of transition from one color
        //     to the next is a rectangle.  Rectangle placed in the top-right corner.
        GradientFromCornerBottomLeft = 33,
        //
        // Summary:
        //     Path gradient type means the that the boundary of transition from one color
        //     to the next is a rectangle.  Rectangle placed in the top-right corner.
        GradientFromCornerBottomRigth = 34,
        //
        // Summary:
        //     Path gradient type means the that the boundary of transition from one color
        //     to the next is a rectangle.  Rectangle placed in the center corner.
        GradientFromCenter = 35,
    }

    [Flags]
    public enum CellFormatEnum
    {
        Bold = 0x01,
        Italic = 0x02,
        Strikeout = 0x04,
        UnderlineSingle = 0x08,
        UnderlineDouble = 0x10,
        UnderlineSingleAccounting = 0x20,
        UnderlineDoubleAccounting = 040,
        Superscript = 0x80,
        Subscript = 0x100
    }
}
