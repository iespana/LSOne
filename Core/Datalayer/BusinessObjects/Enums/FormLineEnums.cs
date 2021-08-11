using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum FormLineAppendTypeEnum
    {
        Append = 0,
        AppendLine = 1
    }

    public enum FormLineFontTypeEnum
    {
        Normal,
        Bold,
        Wide,
        WideHigh,
        High,
        Italic
    }

    public enum FormLineTypeEnum
    {
        Text,
        Logo,
        Barcode
    }

    public enum LogoAlignEnum
    {
        Left,
        Center,
        Right
    }
}
