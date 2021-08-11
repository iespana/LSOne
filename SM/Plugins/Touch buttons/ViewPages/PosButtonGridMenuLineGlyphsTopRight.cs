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
    public partial class PosButtonGridMenuLineGlyphsTopRightPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;

        public PosButtonGridMenuLineGlyphsTopRightPage()
        {
            InitializeComponent();

            //InitialValues();

            posMenuLineCopy = new PosMenuLine();            
   
            cmbGlyph.SelectedIndexChanged += new EventHandler(notifyPreviewMenuButtonChanged);            
            
        }

        private void InitialValues()
        {
            int defaultOffset = 1;
            int defaultSize = 5;
                        
            ntbGlyphOffset.Value = defaultOffset;
                        
            ntbGlyphTextFontSize.Value = defaultSize;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PosButtonGridMenuLineGlyphsTopRightPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuLine = (PosMenuLine)internalContext;
            
            // Glyph
            cmbGlyph.SelectedIndex = (int)posMenuLine.Glyph;
            tbGlyphText.Text = posMenuLine.GlyphText;
            tbGlyphTextFont.Text = posMenuLine.GlyphTextFont;
            ntbGlyphTextFontSize.Value = posMenuLine.GlyphTextFontSize;
            cwGlyphTextForeColor.SelectedColor = Color.FromArgb(posMenuLine.GlyphTextForeColor);
            ntbGlyphOffset.Value = posMenuLine.GlyphOffSet;

            CheckFont(posMenuLine.GlyphTextFont, posMenuLine.GlyphTextFontSize, btnEditGlyphFont, errorProvider1, tbGlyphTextFont);

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
            // Glyph
            if (posMenuLine.Glyph != (PosMenuLine.GlyphEnum)cmbGlyph.SelectedIndex ||
                posMenuLine.GlyphText != tbGlyphText.Text ||
                posMenuLine.GlyphTextFont != tbGlyphTextFont.Text ||
                posMenuLine.GlyphTextFontSize != (int)ntbGlyphTextFontSize.Value ||
                posMenuLine.GlyphTextForeColor != cwGlyphTextForeColor.SelectedColor.ToArgb() ||
                posMenuLine.GlyphOffSet != (int)ntbGlyphOffset.Value)
            {
                posMenuLine.Dirty = true;
                return true;
            }
            
            return false;
        }

        public bool SaveData()
        {
            // Glyph
            posMenuLine.Glyph = (PosMenuLine.GlyphEnum)cmbGlyph.SelectedIndex;
            posMenuLine.GlyphText = tbGlyphText.Text;
            posMenuLine.GlyphTextFont = tbGlyphTextFont.Text;
            posMenuLine.GlyphTextFontSize = (int)ntbGlyphTextFontSize.Value;
            posMenuLine.GlyphTextForeColor = cwGlyphTextForeColor.SelectedColor.ToArgb();
            posMenuLine.GlyphOffSet = (int)ntbGlyphOffset.Value;

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
            if (!String.IsNullOrEmpty(tbGlyphTextFont.Text))
            {
                fontDialog.Font = new Font(tbGlyphTextFont.Text, (float)ntbGlyphTextFontSize.Value);
            }

            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                CheckFont(fontDialog.Font.Name, (int)fontDialog.Font.Size, btnEditGlyphFont, errorProvider1, tbGlyphTextFont);                
                ntbGlyphTextFontSize.Value = fontDialog.Font.Size;
            }
        }        

        private void cmbGlyph4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((PosMenuLine.GlyphEnum)cmbGlyph.SelectedIndex)
            {
                case PosMenuLine.GlyphEnum.None:
                    tbGlyphText.Enabled = tbGlyphTextFont.Enabled = btnEditGlyphFont.Enabled = ntbGlyphTextFontSize.Enabled = cwGlyphTextForeColor.Enabled = ntbGlyphOffset.Enabled = false;
                    break;

                case PosMenuLine.GlyphEnum.Text:
                    tbGlyphText.Enabled = btnEditGlyphFont.Enabled = true;
                    ntbGlyphTextFontSize.Enabled = cwGlyphTextForeColor.Enabled = ntbGlyphOffset.Enabled = true;
                    break;

                default:
                    tbGlyphText.Enabled = btnEditGlyphFont.Enabled = false;
                    ntbGlyphTextFontSize.Enabled = cwGlyphTextForeColor.Enabled = ntbGlyphOffset.Enabled = true;
                    break;
            }
        }

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {   
            // Glyph
            posMenuLineCopy.Glyph = (PosMenuLine.GlyphEnum)cmbGlyph.SelectedIndex;
            posMenuLineCopy.GlyphText = tbGlyphText.Text;
            posMenuLineCopy.GlyphTextFont = tbGlyphTextFont.Text;
            posMenuLineCopy.GlyphTextFontSize = (int)ntbGlyphTextFontSize.Value;
            posMenuLineCopy.GlyphTextForeColor = cwGlyphTextForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.GlyphOffSet = (int)ntbGlyphOffset.Value;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }
    }
}
