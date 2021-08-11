using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Controls;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.BusinessObjects.Images;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    public partial class PosButtonGridMenuLineImagePage : UserControl, ITabView
    {
        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;
        private DataLayer.BusinessObjects.Images.Image currentImage;
        private bool doNotify;
        private Font imageFont;

        public PosButtonGridMenuLineImagePage()
        {
            InitializeComponent();

            posMenuLineCopy = new PosMenuLine();

            doNotify = true;

            cmbImagePosition.Items.Clear();
            cmbImagePosition.Items.AddRange(ButtonStyleUtils.GetImagePositionTexts());
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new PosButtonGridMenuLineImagePage();
        }

        public bool DataIsModified()
        {
            if(cmbImagePosition.SelectedIndex != ButtonStyleUtils.GetIndexFromImagePosition(posMenuLine.ImagePosition) ||
                chkUseFontImage.Checked != posMenuLine.UseImageFont ||
                tbImageText.Text != posMenuLine.ImageFontText ||
                cwImageFontColor.SelectedColor.ToArgb() != posMenuLine.ImageFontColor ||
                imageFont.Name != posMenuLine.ImageFontName ||
                (int)imageFont.SizeInPoints != posMenuLine.ImageFontSize ||
                imageFont.GdiCharSet != posMenuLine.ImageFontCharset ||
                imageFont.Bold != posMenuLine.ImageFontBold ||
                imageFont.Italic != posMenuLine.ImageFontItalic ||
                imageFont.Underline != posMenuLine.ImageFontUnderline ||
                imageFont.Strikeout != posMenuLine.ImageFontStrikethrough ||
                (currentImage == null && !RecordIdentifier.IsEmptyOrNull(posMenuLine.PictureID) ||
                (currentImage != null && currentImage.ID != posMenuLine.PictureID)))
            {
                return true;
            }

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuLine = (PosMenuLine)internalContext;

            doNotify = false;

            currentImage = Providers.ImageData.Get(PluginEntry.DataModel, posMenuLine.PictureID);
            tbImageFile.Text = currentImage == null ? "" : currentImage.Text;
            cmbImagePosition.SelectedIndex = ButtonStyleUtils.GetIndexFromImagePosition(posMenuLine.ImagePosition);
            tbImageText.Text = posMenuLine.ImageFontText;
            tbImageFontName.Text = posMenuLine.ImageFontName == "" ? posMenuLine.FontName : posMenuLine.ImageFontName;
            tbImageFontSize.Text = posMenuLine.ImageFontSize == 0 ? posMenuLine.FontSize.ToString() : posMenuLine.ImageFontSize.ToString();
            cwImageFontColor.SelectedColor = posMenuLine.ImageFontColor == 0 ? Color.FromArgb(posMenuLine.ForeColor) : Color.FromArgb(posMenuLine.ImageFontColor);
            chkUseFontImage.Checked = posMenuLine.UseImageFont;

            FontStyle style = FontStyle.Regular;

            if (posMenuLine.ImageFontBold) { style = style | FontStyle.Bold; }
            if (posMenuLine.ImageFontItalic) { style = style | FontStyle.Italic; }
            if (posMenuLine.ImageFontStrikethrough) { style = style | FontStyle.Strikeout; }
            if (posMenuLine.ImageFontUnderline) { style = style | FontStyle.Underline; }

            imageFont = new Font(posMenuLine.ImageFontName == "" ? posMenuLine.FontName : posMenuLine.ImageFontName, 
                                 posMenuLine.ImageFontSize == 0 ? posMenuLine.FontSize : posMenuLine.ImageFontSize, 
                                 style, GraphicsUnit.Point, Convert.ToByte(posMenuLine.ImageFontCharset));

            chkUseFontImage_CheckedChanged(null, EventArgs.Empty);

            posMenuLineCopy = PosMenuLine.Clone(posMenuLine);
            doNotify = true;
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "ButtonGridPreviewMenuButton":
                    PosMenuLine copy = (PosMenuLine)param;

                    // Copy from the view
                    posMenuLineCopy.Text = copy.Text;
                    posMenuLineCopy.StyleID = copy.StyleID;
                    posMenuLineCopy.UseHeaderFont = copy.UseHeaderFont;
                    posMenuLineCopy.FontName = copy.FontName;
                    posMenuLineCopy.FontSize = copy.FontSize;
                    posMenuLineCopy.FontBold = copy.FontBold;
                    posMenuLineCopy.FontItalic = copy.FontItalic;
                    posMenuLineCopy.FontStrikethrough = copy.FontStrikethrough;
                    posMenuLineCopy.FontUnderline = copy.FontUnderline;
                    posMenuLineCopy.ForeColor = copy.ForeColor;
                    posMenuLineCopy.FontCharset = copy.FontCharset;
                    posMenuLineCopy.TextPosition = copy.TextPosition;

                    // General tab
                    posMenuLineCopy.Transparent = copy.Transparent;

                    // Glyphs tab
                    // Glyph 1
                    posMenuLineCopy.Glyph = copy.Glyph;
                    posMenuLineCopy.GlyphText = copy.GlyphText;
                    posMenuLineCopy.GlyphTextFont = copy.GlyphTextFont;
                    posMenuLineCopy.GlyphTextFontSize = copy.GlyphTextFontSize;
                    posMenuLineCopy.GlyphTextForeColor = copy.GlyphTextForeColor;
                    posMenuLineCopy.GlyphOffSet = copy.GlyphOffSet;

                    // Glyph 2
                    posMenuLineCopy.Glyph2 = copy.Glyph2;
                    posMenuLineCopy.GlyphText2 = copy.GlyphText2;
                    posMenuLineCopy.GlyphText2Font = copy.GlyphText2Font;
                    posMenuLineCopy.GlyphText2FontSize = copy.GlyphText2FontSize;
                    posMenuLineCopy.GlyphText2ForeColor = copy.GlyphText2ForeColor;
                    posMenuLineCopy.Glyph2OffSet = copy.Glyph2OffSet;

                    // Glyph 3
                    posMenuLineCopy.Glyph3 = copy.Glyph3;
                    posMenuLineCopy.GlyphText3 = copy.GlyphText3;
                    posMenuLineCopy.GlyphText3Font = copy.GlyphText3Font;
                    posMenuLineCopy.GlyphText3FontSize = copy.GlyphText3FontSize;
                    posMenuLineCopy.GlyphText3ForeColor = copy.GlyphText3ForeColor;
                    posMenuLineCopy.Glyph3OffSet = copy.Glyph3OffSet;

                    // Glyph 4
                    posMenuLineCopy.Glyph4 = copy.Glyph4;
                    posMenuLineCopy.GlyphText4 = copy.GlyphText4;
                    posMenuLineCopy.GlyphText4Font = copy.GlyphText4Font;
                    posMenuLineCopy.GlyphText4FontSize = copy.GlyphText4FontSize;
                    posMenuLineCopy.GlyphText4ForeColor = copy.GlyphText4ForeColor;
                    posMenuLineCopy.Glyph4OffSet = copy.Glyph4OffSet;

                    // Attributes tab                   
                    posMenuLineCopy.UseHeaderAttributes = copy.UseHeaderAttributes;
                    posMenuLineCopy.GradientMode = copy.GradientMode;
                    posMenuLineCopy.BackColor = copy.BackColor;
                    posMenuLineCopy.BackColor2 = copy.BackColor2;
                    posMenuLineCopy.Shape = copy.Shape;

                    break;
            }
        }

        public bool SaveData()
        {
            PopulateLine(posMenuLine);
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void btnEditImage_Click(object sender, EventArgs e)
        {
            DataLayer.BusinessObjects.Images.Image img = PluginOperations.ShowImageBankSelectDialog(ImageTypeEnum.Button);

            if (img != null)
            {
                tbImageFile.Text = img.Text;
                currentImage = img;
                notifyPreviewMenuButtonChanged(this, EventArgs.Empty);
            }
        }

        private void btnDeleteImage_Click(object sender, EventArgs e)
        {
            tbImageFile.Text = "";
            currentImage = null;

            notifyPreviewMenuButtonChanged(this, EventArgs.Empty);
        }

        private void btnEditImageFont_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbImageFontName.Text))
            {
                fontDialog1.ShowEffects = true;
                fontDialog1.Color = cwImageFontColor.SelectedColor;
                fontDialog1.Font = imageFont;
            }

            if (fontDialog1.ShowDialog(this) == DialogResult.OK)
            {
                doNotify = false;

                FontStyle style = FontStyle.Regular;

                if (fontDialog1.Font.Bold) { style = style | FontStyle.Bold; }
                if (fontDialog1.Font.Italic) { style = style | FontStyle.Italic; }
                if (fontDialog1.Font.Strikeout) { style = style | FontStyle.Strikeout; }
                if (fontDialog1.Font.Underline) { style = style | FontStyle.Underline; }

                imageFont = new Font(fontDialog1.Font.Name, (int)fontDialog1.Font.SizeInPoints, style, GraphicsUnit.Point, fontDialog1.Font.GdiCharSet);

                tbImageFontName.Text = fontDialog1.Font.Name;
                tbImageFontSize.Text = Convert.ToInt32(fontDialog1.Font.SizeInPoints).ToString();
                cwImageFontColor.SelectedColor = fontDialog1.Color;

                doNotify = true;
                notifyPreviewMenuButtonChanged(this, EventArgs.Empty);
            }
        }

        private void cmbImagePosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            notifyPreviewMenuButtonChanged(sender, e);
        }

        private void chkUseFontImage_CheckedChanged(object sender, EventArgs e)
        {
            btnDeleteImage.Enabled =
            btnEditImage.Enabled = !chkUseFontImage.Checked;

            tbImageText.Enabled =
            btnEditImageFont.Enabled =
            cwImageFontColor.Enabled = chkUseFontImage.Checked;

            notifyPreviewMenuButtonChanged(sender, e);
        }

        private void tbImageText_TextChanged(object sender, EventArgs e)
        {
            notifyPreviewMenuButtonChanged(sender, e);
        }

        private void cwImageFontColor_SelectedColorChanged(object sender, EventArgs e)
        {
            notifyPreviewMenuButtonChanged(sender, e);
        }

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {
            if (!doNotify) return;

            PopulateLine(posMenuLineCopy);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }

        private void PopulateLine(PosMenuLine line)
        {
            line.UseImageFont = chkUseFontImage.Checked;
            line.ImageFontText = tbImageText.Text;
            line.ImageFontName = imageFont.Name;
            line.ImageFontColor = cwImageFontColor.SelectedColor.ToArgb();
            line.ImageFontBold = imageFont.Bold;
            line.ImageFontItalic = imageFont.Italic;
            line.ImageFontUnderline = imageFont.Underline;
            line.ImageFontStrikethrough = imageFont.Strikeout;
            line.ImageFontCharset = imageFont.GdiCharSet;
            line.ImageFontSize = Convert.ToInt32(imageFont.SizeInPoints);
            line.PictureID = currentImage == null ? RecordIdentifier.Empty : currentImage.ID;
            line.ImagePosition = ButtonStyleUtils.GetImagePositionFromIndex(cmbImagePosition.SelectedIndex);
        }
    }
}
