using System.Drawing;
using System.Drawing.Drawing2D;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class InfoPanel : Box
    {
        private const int titleHeight = 22;

        public InfoPanel()
        {
            BackColor = Color.FromArgb(247, 245, 241);
            BorderColor = Color.FromArgb(140, 116, 100);
        }


        public override void Draw(System.Windows.Forms.PaintEventArgs e)
        {
            Rectangle paintRectangle = this.DisplayRectangle;

            // Background
            using (SolidBrush brush = new SolidBrush(BackColor))
            {
                e.Graphics.FillRectangle(brush, paintRectangle);
            }

            if (BorderWidth > 0)
            {
                using (Pen pen = new Pen(BorderColor, BorderWidth))
                {
                    DrawRectangle(e.Graphics, pen, paintRectangle);
                }
            }

            // Title bar
            Rectangle titleRectangle = GetTitleRectangle();
            using (LinearGradientBrush brush = new LinearGradientBrush(new Point(0, 0), new Point(0, titleHeight), Color.FromArgb(251, 250, 248), Color.FromArgb(242, 239, 231)))
            {
                e.Graphics.FillRectangle(brush, titleRectangle);
            }
            using (Pen pen = new Pen(BorderColor, BorderWidth))
            {
                e.Graphics.DrawLine(pen, new Point(0, titleHeight), new Point(paintRectangle.Width, titleHeight));
            }


        }

        private Rectangle GetTitleRectangle()
        {
            return new Rectangle(this.DisplayRectangle.X + 1, this.DisplayRectangle.Y + 1, this.DisplayRectangle.Width - 2, titleHeight - 2);
        }


        protected override void DrawText(Graphics g, Rectangle rectangle)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                Rectangle titleRectangle = GetTitleRectangle();
                PointF textLocation = GetTextLocation(g, titleRectangle, TextAlignment, this.Text);
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    g.DrawString(Text, Font, brush, textLocation);
                }
            }
        }


        protected override bool CanMove(Point point)
        {
            return point.Y <= titleHeight;
        }

    }
}
