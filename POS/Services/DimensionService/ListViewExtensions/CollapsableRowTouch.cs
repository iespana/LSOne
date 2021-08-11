using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSOne.Controls;
using LSOne.Controls.Rows;

namespace LSOne.Services.ListViewExtensions
{
    internal class CollapsableRowTouch : Row
    {
        private const int arrowBaseHeight = 3;
        private const int arrowBaseWidth = 2;
        private int arrowScale = 3;

        public CollapsableRowTouch()
        {
            Expaned = true;
            Collapsable = false;
            IsLastRow = false;
        }

        /// <summary>
        /// Gets or sets wether this row is expanded or collapsed
        /// </summary>
        public bool Expaned { get; set; }

        /// <summary>
        /// Gets or sets wether this is the last collapsable row in a list view
        /// </summary>
        public bool IsLastRow { get; set; }

        /// <summary>
        /// Gets or sets wether this row is collaspable. If true then an arrow is drawn on the right-most side and the arrow will flip up/down
        /// based on the <see cref="Expanded"/> property
        /// </summary>
        public bool Collapsable { get; set; }

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

            if (Collapsable)
            {
                // Draw an arrow to indicate wether this row is collapsed or not
                GraphicsPath arrowPath = new GraphicsPath();

                PointF arrowBottom = new PointF(rowBounds.Right - 6 - (arrowBaseWidth*arrowScale), rowBounds.Top + rowBounds.Height/2f + (arrowBaseHeight*arrowScale)/2f);

                if (Expaned)
                {
                    // Arrow: /\
                    arrowPath.AddLine(arrowBottom.X - (arrowBaseWidth*arrowScale), arrowBottom.Y, arrowBottom.X, arrowBottom.Y - (arrowBaseHeight*arrowScale));
                    arrowPath.AddLine(arrowBottom.X, arrowBottom.Y - (arrowBaseHeight*arrowScale), arrowBottom.X + (arrowBaseWidth*arrowScale), arrowBottom.Y);
                }
                else
                {
                    // Arrow: \/
                    arrowPath.AddLine(arrowBottom.X - (arrowBaseWidth*arrowScale), arrowBottom.Y - (arrowBaseHeight*arrowScale), arrowBottom.X, arrowBottom.Y);
                    arrowPath.AddLine(arrowBottom.X, arrowBottom.Y, arrowBottom.X + (arrowBaseWidth*arrowScale), arrowBottom.Y - (arrowBaseHeight*arrowScale));

                    if (!IsLastRow)
                    {                       
                        using (var bottomPen = new Pen(Color.White))
                        {
                            g.DrawLine(bottomPen, rowBounds.Left + 3, rowBounds.Bottom - 1, rowBounds.Right - 3, rowBounds.Bottom - 1);
                            
                        }
                    }
                }

                Pen arrowPen = new Pen(Color.White, 2);

                SmoothingMode oldMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawPath(arrowPen, arrowPath);
                g.SmoothingMode = oldMode;

                arrowPen.Dispose();
                arrowPath.Dispose();
            }

            g.Clip = oldClip;
        }
    }
}
