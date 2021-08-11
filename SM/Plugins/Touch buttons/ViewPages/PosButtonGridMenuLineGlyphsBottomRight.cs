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
    public partial class PosButtonGridMenuLineGlyphsBottomRightPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;

        public PosButtonGridMenuLineGlyphsBottomRightPage()
        {
            InitializeComponent();            

            posMenuLineCopy = new PosMenuLine();            
   
            cmbGlyph2.SelectedIndexChanged += new EventHandler(notifyPreviewMenuButtonChanged);            
            
        }

        private void InitialValues()
        {
            int defaultOffset = 1;
            int defaultSize = 5;
                        
            ntbGlyph2Offset.Value = defaultOffset;
                        
            ntbGlyphText2FontSize.Value = defaultSize;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PosButtonGridMenuLineGlyphsBottomRightPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            errorProvider1.Clear();

            posMenuLine = (PosMenuLine)internalContext;            

            // Glyph 2
            cmbGlyph2.SelectedIndex = (int)posMenuLine.Glyph2;
            tbGlyphText2.Text = posMenuLine.GlyphText2;
            tbGlyphText2Font.Text = posMenuLine.GlyphText2Font;
            ntbGlyphText2FontSize.Value = posMenuLine.GlyphText2FontSize;
            cwGlyphText2ForeColor.SelectedColor = Color.FromArgb(posMenuLine.GlyphText2ForeColor);
            ntbGlyph2Offset.Value = posMenuLine.Glyph2OffSet;

            CheckFont(posMenuLine.GlyphText2Font, posMenuLine.GlyphText2FontSize, btnEditGlyphFont2, errorProvider1, tbGlyphText2Font);

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
            // Glyph 2
            if (posMenuLine.Glyph2 != (PosMenuLine.GlyphEnum)cmbGlyph2.SelectedIndex ||
                posMenuLine.GlyphText2 != tbGlyphText2.Text ||
                posMenuLine.GlyphText2Font != tbGlyphText2Font.Text ||
                posMenuLine.GlyphText2FontSize != (int)ntbGlyphText2FontSize.Value ||
                posMenuLine.GlyphText2ForeColor != cwGlyphText2ForeColor.SelectedColor.ToArgb() ||
                posMenuLine.Glyph2OffSet != (int)ntbGlyph2Offset.Value)
            {
                posMenuLine.Dirty = true;
                return true;
            }
            
            return false;
        }

        public bool SaveData()
        {
            // Glyph 2
            posMenuLine.Glyph2 = (PosMenuLine.GlyphEnum)cmbGlyph2.SelectedIndex;
            posMenuLine.GlyphText2 = tbGlyphText2.Text;
            posMenuLine.GlyphText2Font = tbGlyphText2Font.Text;
            posMenuLine.GlyphText2FontSize = (int)ntbGlyphText2FontSize.Value;
            posMenuLine.GlyphText2ForeColor = cwGlyphText2ForeColor.SelectedColor.ToArgb();
            posMenuLine.Glyph2OffSet = (int)ntbGlyph2Offset.Value;

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

                    posMenuLineCopy.Glyph3 = copy.Glyph3;
                    posMenuLineCopy.GlyphText3 = copy.GlyphText3;
                    posMenuLineCopy.GlyphText3Font = copy.GlyphText3Font;
                    posMenuLineCopy.GlyphText3FontSize = copy.GlyphText3FontSize;
                    posMenuLineCopy.GlyphText3ForeColor = copy.GlyphText3ForeColor;
                    posMenuLineCopy.Glyph3OffSet = copy.Glyph3OffSet;

                    posMenuLineCopy.Glyph4 = copy.Glyph4;
                    posMenuLineCopy.GlyphText4 = copy.GlyphText4;
                    posMenuLineCopy.GlyphText4Font = copy.GlyphText4Font;
                    posMenuLineCopy.GlyphText4FontSize = copy.GlyphText4FontSize;
                    posMenuLineCopy.GlyphText4ForeColor = copy.GlyphText4ForeColor;
                    posMenuLineCopy.Glyph4OffSet = copy.Glyph4OffSet;

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
            if (!String.IsNullOrEmpty(tbGlyphText2Font.Text))
            {
                fontDialog.Font = new Font(tbGlyphText2Font.Text, (float)ntbGlyphText2FontSize.Value);
            }

            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                CheckFont(fontDialog.Font.Name, (int)fontDialog.Font.Size, btnEditGlyphFont2, errorProvider1, tbGlyphText2Font);                                        
                ntbGlyphText2FontSize.Value = fontDialog.Font.Size;
            }
        }        

        private void cmbGlyph4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((PosMenuLine.GlyphEnum)cmbGlyph2.SelectedIndex)
            {
                case PosMenuLine.GlyphEnum.None:
                    tbGlyphText2.Enabled = tbGlyphText2Font.Enabled = btnEditGlyphFont2.Enabled = ntbGlyphText2FontSize.Enabled = cwGlyphText2ForeColor.Enabled = ntbGlyph2Offset.Enabled = false;
                    break;

                case PosMenuLine.GlyphEnum.Text:
                    tbGlyphText2.Enabled = btnEditGlyphFont2.Enabled = true;
                    ntbGlyphText2FontSize.Enabled = cwGlyphText2ForeColor.Enabled = ntbGlyph2Offset.Enabled = true;
                    break;

                default:
                    tbGlyphText2.Enabled = btnEditGlyphFont2.Enabled = false;
                    ntbGlyphText2FontSize.Enabled = cwGlyphText2ForeColor.Enabled = ntbGlyph2Offset.Enabled = true;
                    break;
            }
        }

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {   
            // Glyph 2
            posMenuLineCopy.Glyph2 = (PosMenuLine.GlyphEnum)cmbGlyph2.SelectedIndex;
            posMenuLineCopy.GlyphText2 = tbGlyphText2.Text;
            posMenuLineCopy.GlyphText2Font = tbGlyphText2Font.Text;
            posMenuLineCopy.GlyphText2FontSize = (int)ntbGlyphText2FontSize.Value;
            posMenuLineCopy.GlyphText2ForeColor = cwGlyphText2ForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.Glyph2OffSet = (int)ntbGlyph2Offset.Value;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }
    }
}
