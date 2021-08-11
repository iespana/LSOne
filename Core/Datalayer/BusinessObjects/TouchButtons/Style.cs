using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.Enums;
using LSOne.Utilities.GUI;

namespace LSOne.DataLayer.BusinessObjects.TouchButtons
{
    public class Style : DataEntity, IStyle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Style" /> class.
        /// </summary>
        public Style()
        {
            Text = "";            
            FontName = "Tahoma";
            FontSize = 14;
            FontBold = false;
            ForeColor = ColorPalette.Black;
            BackColor = ColorPalette.White;
            FontItalic = false;
            FontCharset = 0;            
            BackColor2 = ColorPalette.White;
            GradientMode = GradientModeEnum.None;
            Shape = ShapeEnum.RoundRectangle;
            TextPosition = Position.Center;
        }                
        
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public bool FontBold { get; set; }
        public Color ForeColor { get; set; }
        public Color BackColor { get; set; }
        public bool FontItalic { get; set; }
        public int FontCharset { get; set; }
        public Color BackColor2 { get; set; }
        public GradientModeEnum GradientMode { get; set; }
        public ShapeEnum Shape { get; set; }
        public Position TextPosition { get; set; }

        public int ForeColorArgb 
        {
            get
            {
                return ForeColor.ToArgb();
            }
            internal set
            {
                ForeColor = Color.FromArgb(value);
            }
        }

        public int BackColorArgb
        {
            get
            {
                return BackColor.ToArgb();
            }
            internal set
            {
                BackColor = Color.FromArgb(value);
            }
        }

        public int BackColor2Argb
        {
            get
            {
                return BackColor2.ToArgb();
            }
            internal set
            {
                BackColor2 = Color.FromArgb(value);
            }
        }

        private Font font;

        /// <summary>
        /// Creates a font using the properties of the Style
        /// </summary>
        /// <returns>A font</returns>
        public Font CreateFont()
        {
            if (FontName == "")
            {
                FontName = "Tahoma";
            }

            var style = FontStyle.Regular;

            if (FontBold) { style = style | FontStyle.Bold; }
            if (FontItalic) { style = style | FontStyle.Italic; }

            return new Font(
                FontName,
                FontSize == 0 ? 1 : FontSize,
                style,
                GraphicsUnit.Point,
                Convert.ToByte(FontCharset));                      
        }

        public Brush CreateBackgroundBrush(Point point, Point point2)
        {
            return CreateBackgroundBrush(new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y));
        }

        /// <summary>
        /// Creates a brush using the properties of the Style 
        /// </summary>
        /// <param name="rect">The rectangle to be painted with the brush</param>
        /// <returns>A brush</returns>
        public Brush CreateBackgroundBrush(Rectangle rect)
        {            
            switch (GradientMode)
	        {		        
                case GradientModeEnum.Horizontal:                    
                case GradientModeEnum.Vertical:
                case GradientModeEnum.BackwardDiagonal:                    
                case GradientModeEnum.ForwardDiagonal:
                    return new LinearGradientBrush(rect, BackColor, BackColor2, LinearGradientMode);                           
                default:
                    return new SolidBrush(BackColor);
	        }           
        }

        public Font Font
        {
            get { return font ?? CreateFont(); }
            set
            {
                if(value != null)
                {
                    font = value;
                }
            }
        }

        public LinearGradientMode LinearGradientMode
        {
            get 
            {
                switch (GradientMode)
                {
                    case GradientModeEnum.Horizontal:
                        return LinearGradientMode.Horizontal;
                    case GradientModeEnum.Vertical:
                        return LinearGradientMode.Vertical;
                    case GradientModeEnum.BackwardDiagonal:
                        return LinearGradientMode.BackwardDiagonal;
                    case GradientModeEnum.ForwardDiagonal:
                        return LinearGradientMode.ForwardDiagonal;
                }

                return LinearGradientMode.BackwardDiagonal;
            }
        }
    }
}
