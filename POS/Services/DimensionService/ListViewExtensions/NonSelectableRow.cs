using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Controls;
using LSOne.Controls.Rows;

namespace LSOne.Services.ListViewExtensions
{
    internal class NonSelectableRow : Row
    {
        public override void Draw(Graphics g, ListView grid, int rowNumber, Row row, int firstColumn, int topIndent, int width, Style defaultStyle, Color rowBackColor,
            bool hasVisualStyles, ref int columnCursor, int rowCursor)
        {
            Region oldClip = g.Clip;
            var overrideTextColor = Color.Empty;

            var rowBounds = new Rectangle(1, topIndent, width, row.Height);

            g.SetClip(rowBounds, CombineMode.Intersect);

            var rowIsSelected = grid.Selection.RowIsSelected(rowNumber) && grid.Enabled;

            using (var brush = new SolidBrush(BackColor == Color.Empty ? rowBackColor : BackColor))
            {
                g.FillRectangle(brush, rowBounds);
            }

            var leftIndent = rowBounds.Left;

            for (int i = LockLeft ? 0 : firstColumn; i < grid.Columns.Count; i++)
            {
                var column = grid.Columns[i];

                if (Cells.Count >= i + 1)
                {
                    if (Cells[i] != null)
                    {
                        var cellRect = new Rectangle(leftIndent, topIndent, column.Width, row.Height);

                        Cells[i].Draw(g, column, defaultStyle, cellRect, overrideTextColor, hasVisualStyles, rowIsSelected);

                        if (ActionCellCount > 1 && rowCursor == rowNumber && i == columnCursor && grid.Focused)
                        {
                            if (Cells[i].WantsActionKey)
                            {
                                Cells[i].DrawFocusRectangle(grid, g, cellRect);
                            }
                            else
                            {
                                columnCursor++;
                            }
                        }
                    }
                    else
                    {
                        grid.DefaultCell.Draw(g, column, defaultStyle, new Rectangle(leftIndent, topIndent, column.Width, row.Height), overrideTextColor, hasVisualStyles, rowIsSelected);
                    }
                }
                else
                {
                    grid.DefaultCell.Draw(g, column, defaultStyle, new Rectangle(leftIndent, topIndent, column.Width, row.Height), overrideTextColor, hasVisualStyles, rowIsSelected);
                }

                leftIndent += column.Width;
            }

            if (!rowIsSelected && grid.RowLines)
            {
                using (var rowLinePen = new Pen(grid.RowLineColor))
                {
                    g.DrawLine(rowLinePen, rowBounds.Left + 3, rowBounds.Bottom - 1, rowBounds.Right - 3, rowBounds.Bottom - 1);
                }
            }

            g.Clip = oldClip;
        }                
    }
}
