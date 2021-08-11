using System.Drawing;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class Box : Shape
    {
        public override void Draw(System.Windows.Forms.PaintEventArgs e)
        {
            Rectangle paintRectangle = DisplayRectangle;

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

        }


    }
}
