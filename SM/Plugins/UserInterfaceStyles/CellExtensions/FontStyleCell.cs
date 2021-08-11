using System.Drawing;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.UserInterfaceStyles.CellExtensions
{
    public class FontStyleCell : IconButtonCell
    {
        private FontStyle fontStyle;

        public FontStyleCell(FontStyle fontStyle)
            : base(new IconButton(Properties.Resources.InLineEdit16, Properties.Resources.Edit), IconButtonIconAlignmentEnum.Right | IconButtonIconAlignmentEnum.VerticalCenter, GetStyleName(fontStyle))
        {
            this.fontStyle = fontStyle;
        }

        public FontStyle FontStyle
        {
            get
            {
                return fontStyle;
            }
            set
            {
                fontStyle = value;
                text = GetStyleName(fontStyle);
            }
        }

        static string GetStyleName(FontStyle style)
        {
            string result = "";

            if ((style & FontStyle.Bold) == FontStyle.Bold)
            {
                result = Properties.Resources.Bold;
            }

            if ((style & FontStyle.Italic) == FontStyle.Italic)
            {
                if (result != "")
                {
                    result += " | ";
                }

                result += Properties.Resources.Italic;
            }

            if ((style & FontStyle.Underline) == FontStyle.Underline)
            {
                if (result != "")
                {
                    result += " | ";
                }

                result += Properties.Resources.Underline;
            }

            if ((style & FontStyle.Strikeout) == FontStyle.Strikeout)
            {
                if (result != "")
                {
                    result += " | ";
                }

                result += Properties.Resources.Strikeout;
            }

            if (result == "")
            {
                result = Properties.Resources.Regular;
            }

            return result;
        }
    }
}
