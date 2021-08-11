using System.Drawing;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.DataLayer.BusinessObjects;

namespace LSOne.ViewPlugins.UserInterfaceStyles.CellExtensions
{
    internal class PictureCell : IconButtonCell
    {
        private Image image;

        public PictureCell(Image image)
            : base(new IconButton(Properties.Resources.InLineEdit16, Properties.Resources.Edit), IconButtonIconAlignmentEnum.Right | IconButtonIconAlignmentEnum.VerticalCenter, "")
        {
            this.image = image;
        }

        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }

        public override void Draw(Graphics g, Column column, Style defaultStyle, Rectangle cellBounds, Color overrideTextColor, bool hasVisualStyles, bool rowIsSelected)
        {
            base.Draw(g, column, defaultStyle, cellBounds, overrideTextColor, hasVisualStyles, rowIsSelected);

            Region oldClip = g.Clip;

            g.IntersectClip(new Rectangle(cellBounds.Left + 2,cellBounds.Top + 2,cellBounds.Width - 30,100));

            if (image != null)
            {
                g.DrawImage(image, cellBounds.Left + 2, cellBounds.Top + 2);
            }

            g.Clip = oldClip;
        }
    }
}
