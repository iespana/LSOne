using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.Controls.Rows;
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.ListViewExtensions
{
    public class ItemDetailRow : Row
    {
        private string detailText;
        private const int textLeftPadding = 20;
        private bool heightCalculated;
        private bool wordWrap;

        public ItemDetailRow(string detailText, bool wordWrap = false)
        {
            this.detailText = detailText;
            this.wordWrap = wordWrap;

            Height = 22;      
        }

        public override void Draw(Graphics g, LSOne.Controls.ListView grid, int rowNumber, Row row, int firstColumn, int topIndent, int width, LSOne.Controls.Style defaultStyle, System.Drawing.Color rowBackColor, bool hasVisualStyles, ref int columnCursor, int rowCursor)
        {
            Region oldClip = g.Clip;            

            var rowBounds = new Rectangle(1, topIndent, width, row.Height);
            g.SetClip(rowBounds, CombineMode.Intersect);

            using (var brush = new SolidBrush(BackColor == Color.Empty ? rowBackColor : BackColor))            
            {
                g.FillRectangle(brush, rowBounds);
            }

            Rectangle textBounds = new Rectangle(rowBounds.X + textLeftPadding, rowBounds.Y, rowBounds.Width - textLeftPadding, rowBounds.Height);

            // Detail font should be a little smaller than the actual row font            
            Font detailFont = new Font(defaultStyle.Font.FontFamily, defaultStyle.Font.Size - 2, FontStyle.Bold);

            TextFormatFlags formatFlags = TextFormatFlags.LeftAndRightPadding;

            if(wordWrap)
            {
                formatFlags |= TextFormatFlags.WordBreak;

                if(!heightCalculated)
                {
                    Size calculatedSize = TextRenderer.MeasureText(detailText, detailFont, textBounds.Size, formatFlags);
                    Height = (short)calculatedSize.Height;
                    heightCalculated = true;
                    textBounds.Height = Height;
                    row.Height = Height;
                }
            }

            TextRenderer.DrawText(g, detailText, detailFont, textBounds, ColorPalette.POSListViewItemDetailRowTextColor, formatFlags);            
            
            g.Clip = oldClip;
        }
    }
}
