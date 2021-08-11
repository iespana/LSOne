using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.GUI;
using ListView = LSOne.Controls.ListView;

namespace LSOne.ViewPlugins.UserInterfaceStyles.CellExtensions
{
    public class TokenCell : DropDownCell
    {
        BaseStyle.LineTokenEnum token;

        public TokenCell(BaseStyle.LineTokenEnum token)
            : base(GetTokenText(token))
        {
            this.token = token;
            Touch = true;
        }

        public BaseStyle.LineTokenEnum Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
                text = GetTokenText(token);
            }
        }

        internal static string GetTokenText(BaseStyle.LineTokenEnum token)
        {
            switch (token)
            {
                case BaseStyle.LineTokenEnum.Circle:
                    return Properties.Resources.Circle;

                case BaseStyle.LineTokenEnum.Rectangle:
                    return Properties.Resources.Rectangle;

                case BaseStyle.LineTokenEnum.TriangleDown:
                    return Properties.Resources.TriangleDown;

                case BaseStyle.LineTokenEnum.TriangleUp:
                    return Properties.Resources.TriangleUp;

                case BaseStyle.LineTokenEnum.TriangleLeft:
                    return Properties.Resources.TriangleLeft;

                case BaseStyle.LineTokenEnum.TriangleRight:
                    return Properties.Resources.TriangleRight;

                default:
                    return Properties.Resources.NoToken;
                
            }
        }


        protected override void DrawImage(Graphics g,  ref Rectangle cellBounds)
        {
            Size imageSize = new Size(12, 12);

            if (token != BaseStyle.LineTokenEnum.NoToken)
            {
                Point imageLocation = new Point(cellBounds.Left + 2, cellBounds.Top + 2);

                if (imageVerticalAlignment == Column.VerticalAlignmentEnum.Center)
                {
                    imageLocation.Y = cellBounds.Top + (cellBounds.Height / 2) - (imageSize.Height / 2);
                }
                else if (imageVerticalAlignment == Column.VerticalAlignmentEnum.Bottom)
                {
                    imageLocation.Y = cellBounds.Bottom - imageSize.Height - 2;
                }

                Rectangle tokenBounds = new Rectangle(imageLocation, imageSize);

                using (Brush tokenBrush = new SolidBrush(Color.Black))
                {
                    if ((int)token < 5)
                    {
                        // Triangle of some kind
                        PathHelper.DrawTriangle(g, tokenBrush, tokenBounds, (PathHelper.TrianagleEnum)token);
                    }
                    else if (token == BaseStyle.LineTokenEnum.Circle)
                    {
                        SmoothingMode oldMode = g.SmoothingMode;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        g.FillEllipse(tokenBrush, tokenBounds);
                        g.SmoothingMode = oldMode;
                    }
                    else if (token == BaseStyle.LineTokenEnum.Rectangle)
                    {
                        g.FillRectangle(tokenBrush, tokenBounds);
                    }
                }

                cellBounds.X += (imageSize.Width + 4);
                cellBounds.Width -= (imageSize.Width + 4);
            }
        }

        protected override void OnStripItemClicked(object sender, System.Windows.Forms.ToolStripItemClickedEventArgs e)
        {
            var owner = (ListView)((WeakReference)((ContextMenuStrip)sender).Tag).Target;

            Token = (BaseStyle.LineTokenEnum)e.ClickedItem.Tag;

            TriggerCellAction(owner);
        }
    }
}
