using System;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.Enums;

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    public enum SystemStylesEnum
    {
        SystemNumPadButtonStyle,
        SystemNumPadDigitButtonStyle,
        DualDisplayLine,
        DualDisplayLineSub,
        DualDisplayTotal,
        PosTotalsLabels,
        PosTotalsValues,
        PosTotalsBalanceValue,
        PosTotalsBalanceLabel,
        PosTotalsBalanceLabelNegative,
        PosTotalsBalanceValueNegative,
        PosReceiptSaleTotalLabel,
        PosReceiptSaleTotalLabelNegative,
        PosReceiptSaleTotalValue,
        PosReceiptSaleTotalValueNegative,
        PosReceiptLineSub,
        DualDisplayTotalLabelNegative,
        DualDisplayTotalValue,
        DualDisplayTotalValueNegative,
        DualDisplayTotalsBalanceLabel,
        DualDisplayTotalsBalanceLabelNegative,
        DualDisplayTotalsBalanceValueNegative,
        DualDisplayTotalsLabels,
        DualDisplayTotalsValues,
        DualDisplayTotalsBalanceValue,
        SystemKeyboardButtonStyle,
        PosReceiptSaleLineStyle
    }

    public class SystemStyles
    {
        public static bool IsSystemStyle(string name)
        {
            SystemStylesEnum value;
            if (Enum.TryParse(name, out value))
                return true;

            return false;
        }

        public static PosStyle GetDefaults(string name)
        {
            SystemStylesEnum value;
            if (Enum.TryParse(name, out value))
            {
                return GetDefaults(value);
            }
            return null;
        }

        public static PosStyle GetDefaults(SystemStylesEnum style)
        {
            switch (style)
            {
                case SystemStylesEnum.SystemNumPadButtonStyle:
                    return SystemNumPadButtonStyleDefaults;
                case SystemStylesEnum.SystemNumPadDigitButtonStyle:
                    return SystemNumPadDigitButtonStyleDefaults;
                case SystemStylesEnum.DualDisplayLine:
                    return DualDisplayLineDefaults;
                case SystemStylesEnum.DualDisplayLineSub:
                    return DualDisplayLineSubDefaults;
                case SystemStylesEnum.DualDisplayTotal:
                    return DualDisplayTotalDefaults;
                case SystemStylesEnum.DualDisplayTotalLabelNegative:
                    return DualDisplayTotalLabelNegativeDefaults;
                case SystemStylesEnum.DualDisplayTotalValue:
                    return DualDisplayTotalValueDefaults;
                case SystemStylesEnum.DualDisplayTotalValueNegative:
                    return DualDisplayTotalValueNegativeDefaults;
                case SystemStylesEnum.PosTotalsLabels:
                    return PosTotalsLabelsDefaults;
                case SystemStylesEnum.PosTotalsValues:
                    return PosTotalsValuesDefaults;
                case SystemStylesEnum.PosTotalsBalanceValue:
                    return PosTotalsBalanceValueDefaults;
                case SystemStylesEnum.PosTotalsBalanceLabel:
                    return PosTotalsBalanceLabelDefaults;
                case SystemStylesEnum.PosTotalsBalanceLabelNegative:
                    return PosTotalsBalanceLabelNegativeDefaults;
                case SystemStylesEnum.PosTotalsBalanceValueNegative:
                    return PosTotalsBalanceValueNegativeDefaults;
                case SystemStylesEnum.PosReceiptSaleTotalLabel:
                    return PosReceiptSaleTotalLabelDefaults;
                case SystemStylesEnum.PosReceiptSaleTotalLabelNegative:
                    return PosReceiptSaleTotalLabelNegativeDefaults;
                case SystemStylesEnum.PosReceiptSaleTotalValue:
                    return PosReceiptSaleTotalValueDefaults;
                case SystemStylesEnum.PosReceiptSaleTotalValueNegative:
                    return PosReceiptSaleTotalValueNegativeDefaults;
                case SystemStylesEnum.PosReceiptLineSub:
                    return PosReceiptLineSubDefaults;
                case SystemStylesEnum.DualDisplayTotalsLabels:
                    return DualDisplayTotalsLabelsDefaults;
                case SystemStylesEnum.DualDisplayTotalsValues:
                    return DualDisplayTotalsValuesDefaults;
                case SystemStylesEnum.DualDisplayTotalsBalanceValue:
                    return DualDisplayTotalsBalanceValueDefaults;
                case SystemStylesEnum.DualDisplayTotalsBalanceLabel:
                    return DualDisplayTotalsBalanceLabelDefaults;
                case SystemStylesEnum.DualDisplayTotalsBalanceLabelNegative:
                    return DualDisplayTotalsBalanceLabelNegativeDefaults;
                case SystemStylesEnum.DualDisplayTotalsBalanceValueNegative:
                    return DualDisplayTotalsBalanceValueNegativeDefaults;
                case SystemStylesEnum.SystemKeyboardButtonStyle:
                    return SystemKeyboardButtonStyleDefaults;
                case SystemStylesEnum.PosReceiptSaleLineStyle:
                    return PosReceiptSaleLineStyleDefaults;
            }
            return null;
        }

        public static PosStyle SystemNumPadButtonStyleDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.SystemNumPadButtonStyle.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 24,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.White.ToArgb(),
                    BackColor = ColorPalette.POSKeyboardButton.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.SystemNumPadButtonStyle
                };
            }
        }

        public static PosStyle SystemNumPadDigitButtonStyleDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.SystemNumPadDigitButtonStyle.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 24,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.White.ToArgb(),
                    BackColor = ColorPalette.POSKeyboardButton.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.SystemNumPadDigitButtonStyle
                };
            }
        }

        public static PosStyle DualDisplayLineDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayLine.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayLine
                };
            }
        }

        public static PosStyle DualDisplayLineSubDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayLineSub.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 10,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSListViewItemDetailRowTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayLineSub
                };
            }
        }

        public static PosStyle DualDisplayTotalDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotal.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 15,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotal
                };
            }
        }

        public static PosStyle PosTotalsLabelsDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosTotalsLabels.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosTotalsLabels
                };
            }
        }

        public static PosStyle PosTotalsValuesDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosTotalsValues.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosTotalsValues
                };
            }
        }

        public static PosStyle PosTotalsBalanceValueDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosTotalsBalanceValue.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 16,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosTotalsBalanceValue
                };
            }
        }

        public static PosStyle DualDisplayTotalLabelNegativeDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalLabelNegative.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 15,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.NegativeNumber.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalLabelNegative
                };
            }
        }

        public static PosStyle DualDisplayTotalValueDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalValue.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 15,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalValue
                };
            }
        }

        public static PosStyle DualDisplayTotalValueNegativeDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalValueNegative.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 15,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.NegativeNumber.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalValueNegative
                };
            }
        }

        public static PosStyle PosTotalsBalanceLabelDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosTotalsBalanceLabel.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosTotalsBalanceLabel
                };
            }
        }

        public static PosStyle PosTotalsBalanceLabelNegativeDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosTotalsBalanceLabelNegative.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.NegativeNumber.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosTotalsBalanceLabelNegative
                };
            }
        }

        public static PosStyle PosTotalsBalanceValueNegativeDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosTotalsBalanceValueNegative.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 16,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.NegativeNumber.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosTotalsBalanceValueNegative
                };
            }
        }

        public static PosStyle PosReceiptSaleTotalLabelDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosReceiptSaleTotalLabel.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 15,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosReceiptSaleTotalLabel
                };
            }
        }

        public static PosStyle PosReceiptSaleTotalLabelNegativeDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosReceiptSaleTotalLabelNegative.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 15,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.NegativeNumber.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosReceiptSaleTotalLabelNegative
                };
            }
        }

        public static PosStyle PosReceiptSaleTotalValueDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosReceiptSaleTotalValue.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 15,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosReceiptSaleTotalValue
                };
            }
        }

        public static PosStyle PosReceiptSaleTotalValueNegativeDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosReceiptSaleTotalValueNegative.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 15,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.NegativeNumber.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosReceiptSaleTotalValueNegative
                };
            }
        }

        public static PosStyle PosReceiptLineSubDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosReceiptLineSub.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 10,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSListViewItemDetailRowTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosReceiptLineSub
                };
            }
        }

        public static PosStyle DualDisplayTotalsLabelsDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalsLabels.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalsLabels
                };
            }
        }

        public static PosStyle DualDisplayTotalsValuesDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalsValues.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalsValues
                };
            }
        }

        public static PosStyle DualDisplayTotalsBalanceValueDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalsBalanceValue.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 16,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalsBalanceValue
                };
            }
        }

        public static PosStyle DualDisplayTotalsBalanceLabelDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalsBalanceLabel.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalsBalanceLabel
                };
            }
        }

        public static PosStyle DualDisplayTotalsBalanceLabelNegativeDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalsBalanceLabelNegative.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 12,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.NegativeNumber.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalsBalanceLabelNegative
                };
            }
        }

        public static PosStyle DualDisplayTotalsBalanceValueNegativeDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.DualDisplayTotalsBalanceValueNegative.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 16,
                    FontBold = true,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.NegativeNumber.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.DualDisplayTotalsBalanceValueNegative
                };
            }
        }

        public static PosStyle SystemKeyboardButtonStyleDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.SystemKeyboardButtonStyle.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 24,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.White.ToArgb(),
                    BackColor = ColorPalette.POSKeyboardButton.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.SystemKeyboardButtonStyle
                };
            }
        }

        public static PosStyle PosReceiptSaleLineStyleDefaults
        {
            get
            {
                return new PosStyle
                {
                    Text = SystemStylesEnum.PosReceiptSaleLineStyle.ToString(),
                    FontName = "Segoe UI",
                    FontSize = 10,
                    FontBold = false,
                    FontItalic = false,
                    FontCharset = 0,
                    ForeColor = ColorPalette.POSTextColor.ToArgb(),
                    BackColor = ColorPalette.White.ToArgb(),
                    BackColor2 = ColorPalette.White.ToArgb(),
                    GradientMode = GradientModeEnum.None,
                    Shape = ShapeEnum.Rectangle,
                    IsSystemStyle = true,
                    StyleType = Enums.StyleType.PosReceiptSaleLineStyle
                };
            }
        }
    }
}
