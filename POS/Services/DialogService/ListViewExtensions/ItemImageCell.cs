using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.Services.Properties;
using LSOne.Services.SupportClasses;
using LSOne.Utilities.GUI;

namespace LSOne.Services.ListViewExtensions
{
    internal class ItemImageCell : Cell
    {
        private Image image;
        private Image resizedImage;
        private Image tickMarkImage;
        private string itemName;
        private string itemVariantName;
        private string priceText;
        private bool focused;
        private bool mouseDown;        
        private ImageCellDictionaryManager imageCellManager;
        private SimpleRetailItem retailItem;
        string ellipsis = "…";

        public ItemImageCell(Image image, string itemName, string itemVariantName, SimpleRetailItem retailItem, ImageCellDictionaryManager imageCellManager, Image tickMarkImage)
        {
            this.image = image;
            this.itemName = itemName;
            this.itemVariantName = itemVariantName;
            this.imageCellManager = imageCellManager;
            this.retailItem = retailItem;
            this.tickMarkImage = tickMarkImage;
            priceText = "";
            focused = false;
            mouseDown = false;
        }

        public ItemImageCell()
        {
            
        }

        public string PriceText
        {
            get { return priceText; }
            set
            {
                priceText = value;                                
            }
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public bool Focused
        {
            get
            {
                return focused;
            }
            set
            {
                focused = value;
            }
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

        public bool ShowNoImage { get; set; }


        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public SimpleRetailItem RetailItem => retailItem;


        public void ClearImage()
        {
            if (image != null)
            {
                // We don't want do dispose the no image object since that is a shared image resource and disposing it would cause it to become
                // unusable elsewhere
                if (!ShowNoImage)
                {
                    image.Dispose();
                    resizedImage?.Dispose();
                }

                image = null;
                resizedImage = null;
            }
        }

        public override void Draw(Graphics g, Column column, Style defaultStyle, Rectangle cellBounds, Color overrideTextColor, bool hasVisualStyles, bool rowIsSelected)
        {
            Rectangle newBounds = new Rectangle(cellBounds.X + 1, cellBounds.Y + 1, cellBounds.Width - 2, cellBounds.Height - 2);

            Font itemNameFont = new Font(defaultStyle.Font.FontFamily, 12);
            Font itemVariantFont = new Font(defaultStyle.Font.FontFamily, 9.75f);
            Font itemPriceFont = new Font(defaultStyle.Font.FontFamily, 9.75f);
            Font noImageTextFont = new Font(defaultStyle.Font.FontFamily, 9.75f);
            Brush fontBrush = new SolidBrush(Utilities.ColorPalette.ColorPalette.POSTextColor);

            int imgWidth = newBounds.Width;
            int imgHeight = newBounds.Height - 53;

            int textAreaTotalHeight = 47;
            int variantTextAreaHeight = 22;
            int priceTextAreaHeight = 22;
            int noImageTextAreaHeight = 16;

            float itemNameHeight = g.MeasureString(itemName, itemNameFont).Height;

            if (focused)
            {
                Brush focusBrush = new SolidBrush(Utilities.ColorPalette.ColorPalette.POSSelectedRowColor);
                Rectangle focusRectangle = new Rectangle(cellBounds.X, cellBounds.Y, cellBounds.Width, cellBounds.Height - 1);

                g.FillRectangle(focusBrush, focusRectangle);

                focusRectangle.Inflate(-1, -1);
                Pen focusPen = new Pen(Utilities.ColorPalette.ColorPalette.POSFocusedBorderColor, 2);
                g.DrawRectangle(focusPen, focusRectangle);

                focusPen.Dispose();
                focusBrush.Dispose();
            }

            // In some instances the image variable becomes corrupted for some reason but the resized image is still usable
            if ((image != null && image.PixelFormat != PixelFormat.DontCare) || (resizedImage != null && resizedImage.PixelFormat != PixelFormat.DontCare))
            {
                if (resizedImage == null)
                {
                    resizedImage = ImageUtils.ResizeImage(image, imgWidth < imgHeight ? imgWidth : imgHeight, true);                    
                }

                Rectangle resizedImageBounds = new Rectangle(newBounds.X + ((newBounds.Width / 2) - (resizedImage.Width / 2)), newBounds.Y + 2, resizedImage.Width, resizedImage.Height);

                // Adjust the image position so that it is centered in the image area
                resizedImageBounds.Y += (imgHeight - resizedImageBounds.Height)/ 2;

                g.DrawImage(resizedImage, resizedImageBounds);           

                if (ShowNoImage)
                {
                    RectangleF noImageTextBounds = new RectangleF(newBounds.X, newBounds.Y + (resizedImageBounds.Height - noImageTextAreaHeight), newBounds.Width, noImageTextAreaHeight);
                    StringFormat noImageTextFormat = new StringFormat();
                    noImageTextFormat.Alignment = StringAlignment.Center;
                    g.DrawString(Resources.NoImage, noImageTextFont, fontBrush, noImageTextBounds, noImageTextFormat);
                }       
            }
            else
            {
                if (!imageCellManager.RequestExistsForCell(this))
                {
                    imageCellManager.AddRequest(this);                    
                }
                
                Font ellipsisFont = new Font(defaultStyle.Font.FontFamily, 48f, FontStyle.Bold);
                
                RectangleF ellipsisBounds = new RectangleF(newBounds.X, newBounds.Y, newBounds.Width, imgHeight);
                StringFormat ellipsisFormat = new StringFormat();
                ellipsisFormat.Alignment = StringAlignment.Center;
                ellipsisFormat.LineAlignment = StringAlignment.Center;

                g.DrawString(ellipsis, ellipsisFont, fontBrush, ellipsisBounds, ellipsisFormat);

                ellipsisFont.Dispose();
            }

            // Draw item name            
            bool extendedHeight = !string.IsNullOrEmpty(priceText) || !string.IsNullOrEmpty(itemVariantName);
            RectangleF nameBounds = new RectangleF(newBounds.X, newBounds.Y + (newBounds.Height - (!extendedHeight ? ((newBounds.Height - imgHeight) / 2f + itemNameHeight / 2) : textAreaTotalHeight)), newBounds.Width, priceTextAreaHeight);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.Trimming = StringTrimming.EllipsisCharacter;
            format.FormatFlags = format.FormatFlags | StringFormatFlags.NoWrap;
            g.DrawString(itemName, itemNameFont, fontBrush, nameBounds, format);

            if (!string.IsNullOrEmpty(itemVariantName))
            {
                // Draw item variant name
                RectangleF variantBounds = new RectangleF(newBounds.X + newBounds.Width / 4, newBounds.Y + (newBounds.Height - variantTextAreaHeight), newBounds.Width / 2, variantTextAreaHeight);
                format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.Trimming = StringTrimming.EllipsisCharacter;
                format.FormatFlags = format.FormatFlags | StringFormatFlags.NoWrap;
                g.DrawString(itemVariantName, itemVariantFont, fontBrush, variantBounds, format);
            }

            if (imageCellManager.ShowPrice)
            {
                // Draw item price
                RectangleF priceBounds = new RectangleF(newBounds.X, newBounds.Y + (newBounds.Height - priceTextAreaHeight), newBounds.Width, priceTextAreaHeight);
                format = new StringFormat();
                format.Alignment = StringAlignment.Far;
                g.DrawString(priceText == "" ? ellipsis : priceText, itemPriceFont, fontBrush, priceBounds, format);
            }

            itemNameFont.Dispose();
            itemPriceFont.Dispose();
            noImageTextFont.Dispose();
            fontBrush.Dispose();
        }        

        protected override bool MouseDown(ListView ownerListView, Rectangle cellBounds, Point location, ref bool captureMouse, int columnNumber, int rowNumber, ref bool cancelEventAction)
        {
            if (cellBounds.Contains(location))
            {
                mouseDown = true;
                captureMouse = true;
                return true;
            }
            else
            {
                mouseDown = false;
            }

            return false;
        }

        protected override void MouseUp(ListView ownerListView, int rowNumber, Rectangle cellBounds, Point location, ref bool triggerAction, ref int actionIndex)
        {
            if (cellBounds.Contains(location))
            {
                // toggle focused state
                if (mouseDown)
                {
                    focused = true;
                    triggerAction = true;
                }           
                
            }            
        }

        public override bool DontSuppressDoubleClick => true;
        protected override bool ActionSupressesSelection => false;
    }
}
