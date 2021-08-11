using System.Drawing;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.UserInterfaceStyles.CellExtensions
{
    internal class FontCell : IconButtonCell
    {
        private Font font;

        public FontCell(Font font)
            : base(new IconButton(Properties.Resources.InLineEdit16, Properties.Resources.Edit), IconButtonIconAlignmentEnum.Right | IconButtonIconAlignmentEnum.VerticalCenter, GetFontName(font))
        {
            this.font = font;
        }

        public Font Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;

                if (font == null)
                {
                    this.Text = "";
                }
                else
                {
                    this.Text = GetFontName(font);
                }
            }
        }

        private static string GetFontName(Font font)
        {
            bool hasStyles = false;

            if (font == null)
            {
                return "";
            }
            else
            {
                string fontName = font.Name + "; " + font.Size.ToString("0.00");

                if (font.Style != FontStyle.Regular)
                {
                    fontName += "; ";

                    if (font.Bold)
                    {
                        fontName += Properties.Resources.Bold;
                        hasStyles = true;
                    }

                    if (font.Italic)
                    {
                        if (hasStyles)
                        {
                            fontName += " | ";
                        }

                        fontName += Properties.Resources.Italic;
                        hasStyles = true;
                    }

                    if (font.Underline)
                    {
                        if (hasStyles)
                        {
                            fontName += " | ";
                        }

                        fontName += Properties.Resources.Underline;
                        hasStyles = true;
                    }

                    if (font.Strikeout)
                    {
                        if (hasStyles)
                        {
                            fontName += " | ";
                        }

                        fontName += Properties.Resources.Strikeout;
                        hasStyles = true;
                    }
                }

                return fontName;
            }
        }
    }
}
