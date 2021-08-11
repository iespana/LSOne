using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.ColorUtilities;
using LSOne.Utilities.GUI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LSOne.Services.SupportClasses
{
    public enum DockEnum
    {
        DockNone,
        DockEnd
    }

    public enum ImageAlignment
    {
        Center,
        Left
    }

    internal class TouchKey
    {
        internal Rectangle Bounds { get; set; }
        internal string Text { get; set; }
        internal object Tag { get; set; }
        internal string Key { get; set; }
        internal DockEnum Dock { get; set; }
        internal bool Enabled { get; set; }
        internal IStyle Style { get; set; }
        internal int CalculatedOptimalWidth { get; set; }
        internal Image Image { get; set; }
        internal ImageAlignment ImageAlignment { get; set; }
        internal bool Clickable { get; set; }

        public TouchKey()
        {
            Clickable = true;
            Text = "";
            Tag = null;
            Dock = DockEnum.DockNone;
            Enabled = true;
            Bounds = Rectangle.Empty;
        }
        

        public TouchKey(string text, object tag) : this()
        {
            Text = text;
            Tag = tag;
        }

        public TouchKey(string text, object tag, DockEnum dock) : this(text, tag)
        {
            Dock = dock;
        }


        public virtual bool HitTest(Point location)
        {
            return Bounds.Contains(location);
        }

        public void SetBounds(Rectangle bounds)
        {
            this.Bounds = bounds;
        }

        public bool Pressed { get; set; }

        public void CalculateOptimalWidth(Graphics g, TextFormatFlags formatFlags)
        {
            CalculatedOptimalWidth = TextRenderer.MeasureText(g, Text, Style.Font).Width;
        }

        public virtual void Draw(Graphics g, bool enabled, TextFormatFlags formatFlags, bool horizontal)
        {
            Bitmap glyph;
            LinearGradientBrush gradientBrush;
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(Bounds);

            if (!enabled)
            {
                Brush brush = new SolidBrush(ColorContrast.Disable(Style.BackColor));
                g.FillPath(brush, path);
                brush.Dispose();
            }
            else
            {
                if (Pressed)
                {
                    gradientBrush = new LinearGradientBrush(
                        new Point(Bounds.Left, Bounds.Top),
                        new Point(Bounds.Left, Bounds.Bottom),
                        ColorContrast.DarkenBy(Style.BackColor, 10),
                        ColorContrast.DarkenBy(Style.BackColor2, 10));

                    g.FillPath(gradientBrush, path);
                    gradientBrush.Dispose();
                }
                else
                {
                    gradientBrush = new LinearGradientBrush(
                       new Point(Bounds.Left, Bounds.Top),
                       new Point(Bounds.Left, Bounds.Bottom),
                       Style.BackColor,
                       Style.BackColor2);

                    g.FillPath(gradientBrush, path);
                    gradientBrush.Dispose();
                }
            }

            Pen borderPen = new Pen(ColorPalette.POSControlBorderColor);
            g.DrawRectangle(borderPen, Bounds.X, Bounds.Y, Bounds.Width - 1, Bounds.Height - 1);
            borderPen.Dispose();
            
            if (Image != null)
            {
                if (ImageAlignment == ImageAlignment.Center)
                {
                    g.DrawImage(enabled ? Image : (Image)BitmapHelper.AdjustBrightnessContrast((Bitmap)Image, 3.0f, 2.0f, 0.3f),
                        new Rectangle(Bounds.X + Bounds.Width / 2 - Image.Width / 2, Bounds.Y + Bounds.Height / 2 - Image.Height / 2, Image.Width, Image.Height));
                }
                else if (ImageAlignment == ImageAlignment.Left)
                {
                    if (Text == "")
                    {
                        g.DrawImage(enabled ? Image : (Image)BitmapHelper.AdjustBrightnessContrast((Bitmap)Image, 3.0f, 2.0f, 0.3f),
                                Bounds.X + 6, 1 + Bounds.Y + (Bounds.Height / 2) - (this.Image.Height / 2) + 1);
                    }
                    else
                    {
                        // Both image and text should be centered with 7px between them
                        Rectangle imageBounds = new Rectangle(0, Bounds.Y + Bounds.Height / 2 - Image.Height / 2, Image.Width, Image.Height);
                        Size textSize = TextRenderer.MeasureText(g, Text, Style.Font);
                        imageBounds.X = Bounds.X + Bounds.Width / 2 - textSize.Width / 2 - imageBounds.Width;
                        Rectangle textBounds = new Rectangle(Bounds.X + Bounds.Width / 2 - textSize.Width / 2 + imageBounds.Width / 2, Bounds.Y, textSize.Width, Bounds.Height - 2);
                        textBounds.X -= 3;
                        imageBounds.X += 3;

                        g.DrawImage(enabled ? Image : (Image)BitmapHelper.AdjustBrightnessContrast((Bitmap)Image, 3.0f, 2.0f, 0.3f), imageBounds);
                        TextRenderer.DrawText(g, Text, Style.Font, textBounds, enabled ? Style.ForeColor : ColorPalette.POSGhostTextColor, formatFlags);
                    }
                }
                else
                {
                    Rectangle topHalf = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height / 2);
                    Rectangle imageBounds = new Rectangle(topHalf.X + topHalf.Width / 2 - Image.Width / 2, Math.Max(topHalf.Y, topHalf.Y + topHalf.Height / 2 - Image.Height / 2) + 2, Image.Width, Math.Min(topHalf.Height, Image.Height));

                    g.DrawImage(
                       enabled ? Image : (Image)BitmapHelper.AdjustBrightnessContrast((Bitmap)Image, 3.0f, 2.0f, 0.3f),
                       imageBounds);

                    if (Text != "")
                    {
                        Rectangle bottomHalf = new Rectangle(Bounds.X, Bounds.Y + Bounds.Height / 2, Bounds.Width, Bounds.Height / 2);
                        bottomHalf.Inflate(-2, -2);
                        TextRenderer.DrawText(g, Text, Style.Font, bottomHalf, enabled ? Style.ForeColor : ColorPalette.POSGhostTextColor, formatFlags);
                    }
                }
            }
            else
            {
                if (Text != "")
                {
                    Bounds.Inflate(-2, -2);
                    TextRenderer.DrawText(g, Text, Style.Font, Bounds, enabled ? Style.ForeColor : ColorPalette.POSGhostTextColor, formatFlags);
                    Bounds.Inflate(2, 2);
                }
            }

            path.Dispose();
        }
    }
}