using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.GUI;
using LSOne.ViewPlugins.UserInterfaceStyles.CellExtensions;

namespace LSOne.ViewPlugins.UserInterfaceStyles
{
    internal class ToolStripTokenItem : ToolStripMenuItem
    {
        BaseStyle.LineTokenEnum token;

        public ToolStripTokenItem(BaseStyle.LineTokenEnum token)
            : base()
        {
            this.Text = TokenCell.GetTokenText(token);
            this.token = token;
            this.Tag = token;

            if (token != BaseStyle.LineTokenEnum.NoToken)
            {
                Bitmap bitmap = new Bitmap(16,16);
                Graphics g = Graphics.FromImage(bitmap);

                Rectangle tokenBounds = new Rectangle(1, 1, 14, 14);

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

                g.Dispose();

                Image = bitmap;
            }
        }

       
    }
}
