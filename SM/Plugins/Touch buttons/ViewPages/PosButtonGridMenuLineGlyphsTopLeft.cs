using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    public partial class PosButtonGridMenuLineGlyphsTopLeftPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;

        public PosButtonGridMenuLineGlyphsTopLeftPage()
        {
            InitializeComponent();

            //InitialValues();

            posMenuLineCopy = new PosMenuLine();            
   
            cmbGlyph4.SelectedIndexChanged += new EventHandler(notifyPreviewMenuButtonChanged);            
            
        }

        private void InitialValues()
        {
            int defaultOffset = 1;
            int defaultSize = 5;
                        
            ntbGlyph4Offset.Value = defaultOffset;
                        
            ntbGlyphText4FontSize.Value = defaultSize;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PosButtonGridMenuLineGlyphsTopLeftPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            errorProvider1.Clear();

            posMenuLine = (PosMenuLine)internalContext;            

            // Glyph 4
            cmbGlyph4.SelectedIndex = (int)posMenuLine.Glyph4;
            tbGlyphText4.Text = posMenuLine.GlyphText4;
            tbGlyphText4Font.Text = posMenuLine.GlyphText4Font;
            ntbGlyphText4FontSize.Value = posMenuLine.GlyphText4FontSize;
            cwGlyphText4ForeColor.SelectedColor = Color.FromArgb(posMenuLine.GlyphText4ForeColor);
            ntbGlyph4Offset.Value = posMenuLine.Glyph4OffSet;

            CheckFont(posMenuLine.GlyphText4Font, posMenuLine.GlyphText4FontSize, btnEditGlyphFont4, errorProvider1, tbGlyphText4Font);

            posMenuLineCopy = PosMenuLine.Clone(posMenuLine);
        }

        private void CheckFont(string glyphTextFont, int glyphTextFontSize, ContextButton btnEditGlyphFont, ErrorProvider errorProvider, TextBox tbGlyphTextFont)
        {
            errorProvider.Clear();

            Font glyphFont = null;
            try
            {
                if (!String.IsNullOrEmpty(glyphTextFont))
                {
                    glyphFont = new Font(glyphTextFont, glyphTextFontSize);
                }
            }
            catch (Exception)
            {
                glyphFont = new Font("Tahoma", glyphTextFontSize);
                errorProvider.SetError(btnEditGlyphFont, Properties.Resources.FontNotApplicable);
            }

            if (glyphFont != null)
            {
                tbGlyphTextFont.Text = glyphFont.FontFamily.Name;
            }
        }

        public bool DataIsModified()
        {
            // Glyph 4
            if (posMenuLine.Glyph4 != (PosMenuLine.GlyphEnum)cmbGlyph4.SelectedIndex ||
                posMenuLine.GlyphText4 != tbGlyphText4.Text ||
                posMenuLine.GlyphText4Font != tbGlyphText4Font.Text ||
                posMenuLine.GlyphText4FontSize != (int)ntbGlyphText4FontSize.Value ||
                posMenuLine.GlyphText4ForeColor != cwGlyphText4ForeColor.SelectedColor.ToArgb() ||
                posMenuLine.Glyph4OffSet != (int)ntbGlyph4Offset.Value)
            {
                posMenuLine.Dirty = true;
                return true;
            }
            
            return false;
        }

        public bool SaveData()
        {
            // Glyph 4
            posMenuLine.Glyph4 = (PosMenuLine.GlyphEnum)cmbGlyph4.SelectedIndex;
            posMenuLine.GlyphText4 = tbGlyphText4.Text;
            posMenuLine.GlyphText4Font = tbGlyphText4Font.Text;
            posMenuLine.GlyphText4FontSize = (int)ntbGlyphText4FontSize.Value;
            posMenuLine.GlyphText4ForeColor = cwGlyphText4ForeColor.SelectedColor.ToArgb();
            posMenuLine.Glyph4OffSet = (int)ntbGlyph4Offset.Value;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            //throw new NotImplementedException();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "ButtonGridPreviewMenuButton":
                    PosMenuLine copy = (PosMenuLine)param;

                    // Get changes from the view to update our own copy
                    posMenuLineCopy.Text = copy.Text;

                    // Get changes from other tabs to update our own copy
                    // Font tab
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
                    posMenuLineCopy.Transparent  = copy.Transparent ;

                    // Attributes tab
                    posMenuLineCopy.UseHeaderAttributes = copy.UseHeaderAttributes;
                    posMenuLineCopy.GradientMode = copy.GradientMode;
                    posMenuLineCopy.BackColor = copy.BackColor;
                    posMenuLineCopy.BackColor2 = copy.BackColor2;
                    posMenuLineCopy.Shape = copy.Shape;

                    //Glyphs tabs
                    posMenuLineCopy.Glyph = copy.Glyph;
                    posMenuLineCopy.GlyphText = copy.GlyphText;
                    posMenuLineCopy.GlyphTextFont = copy.GlyphTextFont;
                    posMenuLineCopy.GlyphTextFontSize = copy.GlyphTextFontSize;
                    posMenuLineCopy.GlyphTextForeColor = copy.GlyphTextForeColor;
                    posMenuLineCopy.GlyphOffSet = copy.GlyphOffSet;

                    posMenuLineCopy.Glyph2 = copy.Glyph2;
                    posMenuLineCopy.GlyphText2 = copy.GlyphText2;
                    posMenuLineCopy.GlyphText2Font = copy.GlyphText2Font;
                    posMenuLineCopy.GlyphText2FontSize = copy.GlyphText2FontSize;
                    posMenuLineCopy.GlyphText2ForeColor = copy.GlyphText2ForeColor;
                    posMenuLineCopy.Glyph2OffSet = copy.Glyph2OffSet;

                    posMenuLineCopy.Glyph3 = copy.Glyph3;
                    posMenuLineCopy.GlyphText3 = copy.GlyphText3;
                    posMenuLineCopy.GlyphText3Font = copy.GlyphText3Font;
                    posMenuLineCopy.GlyphText3FontSize = copy.GlyphText3FontSize;
                    posMenuLineCopy.GlyphText3ForeColor = copy.GlyphText3ForeColor;
                    posMenuLineCopy.Glyph3OffSet = copy.Glyph3OffSet;

                    //Image tab
                    posMenuLineCopy.PictureID = copy.PictureID;
                    posMenuLineCopy.ImagePosition = copy.ImagePosition;
                    posMenuLineCopy.UseImageFont = copy.UseImageFont;
                    posMenuLineCopy.ImageFontText = copy.ImageFontText;
                    posMenuLineCopy.ImageFontName = copy.ImageFontName;
                    posMenuLineCopy.ImageFontColor = copy.ImageFontColor;
                    posMenuLineCopy.ImageFontBold = copy.ImageFontBold;
                    posMenuLineCopy.ImageFontItalic = copy.ImageFontItalic;
                    posMenuLineCopy.ImageFontUnderline = copy.ImageFontUnderline;
                    posMenuLineCopy.ImageFontStrikethrough = copy.ImageFontStrikethrough;
                    posMenuLineCopy.ImageFontCharset = copy.ImageFontCharset;
                    posMenuLineCopy.ImageFontSize = copy.ImageFontSize;
                    break;
            }
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion
        

        private void btnEditGlyphFont4_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbGlyphText4Font.Text))
            {
                fontDialog.Font = new Font(tbGlyphText4Font.Text, (float)ntbGlyphText4FontSize.Value);
            }

            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                CheckFont(fontDialog.Font.Name, (int)fontDialog.Font.Size, btnEditGlyphFont4, errorProvider1, tbGlyphText4Font);                            
                ntbGlyphText4FontSize.Value = fontDialog.Font.Size;
            }
        }        

        private void cmbGlyph4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((PosMenuLine.GlyphEnum)cmbGlyph4.SelectedIndex)
            {
                case PosMenuLine.GlyphEnum.None:
                    tbGlyphText4.Enabled = tbGlyphText4Font.Enabled = btnEditGlyphFont4.Enabled = ntbGlyphText4FontSize.Enabled = cwGlyphText4ForeColor.Enabled = ntbGlyph4Offset.Enabled = false;
                    break;

                case PosMenuLine.GlyphEnum.Text:
                    tbGlyphText4.Enabled = btnEditGlyphFont4.Enabled = true;
                    ntbGlyphText4FontSize.Enabled = cwGlyphText4ForeColor.Enabled = ntbGlyph4Offset.Enabled = true;
                    break;

                default:
                    tbGlyphText4.Enabled = btnEditGlyphFont4.Enabled = false;
                    ntbGlyphText4FontSize.Enabled = cwGlyphText4ForeColor.Enabled = ntbGlyph4Offset.Enabled = true;
                    break;
            }
        }

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {   
            // Glyph 4
            posMenuLineCopy.Glyph4 = (PosMenuLine.GlyphEnum)cmbGlyph4.SelectedIndex;
            posMenuLineCopy.GlyphText4 = tbGlyphText4.Text;
            posMenuLineCopy.GlyphText4Font = tbGlyphText4Font.Text;
            posMenuLineCopy.GlyphText4FontSize = (int)ntbGlyphText4FontSize.Value;
            posMenuLineCopy.GlyphText4ForeColor = cwGlyphText4ForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.Glyph4OffSet = (int)ntbGlyph4Offset.Value;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }
    }
}
