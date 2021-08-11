using System.Drawing;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public class Connector 
    {
        public Shape End1 { get; set; }
        public Shape End2 { get; set; }

        public int LineWidth { get; set; }

        public void Draw(System.Windows.Forms.PaintEventArgs e)
        {
            Point point1 = new Point(End1.Left + End1.Width / 2, End1.Top + End1.Height / 2);
            Point point2 = new Point(End2.Left + End2.Width / 2, End2.Top + End2.Height / 2);
            using (Pen pen = new Pen(Color.Black))
            {
                e.Graphics.DrawLine(pen, point1, point2);
            }
        }

    }
}
