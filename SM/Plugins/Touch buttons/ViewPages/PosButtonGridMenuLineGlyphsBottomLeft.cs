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
    public partial class PosButtonGridMenuLineGlyphsBottomLeftPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;

        public PosButtonGridMenuLineGlyphsBottomLeftPage()
        {
            InitializeComponent();            

            posMenuLineCopy = new PosMenuLine();            
   
            cmbGlyph3.SelectedIndexChanged += new EventHandler(notifyPreviewMenuButtonChanged);            
            
        }

        private void InitialValues()
        {
            int defaultOffset = 1;
            int defaultSize = 5;
                        
            ntbGlyph3Offset.Value = defaultOffset;
                        
            ntbGlyphText3FontSize.Value = defaultSize;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PosButtonGridMenuLineGlyphsBottomLeftPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            errorProvider1.Clear();

            posMenuLine = (PosMenuLine)internalContext;            

            // Glyph 3
            cmbGlyph3.SelectedIndex = (int)posMenuLine.Glyph3;
            tbGlyphText3.Text = posMenuLine.GlyphText3;
            tbGlyphText3Font.Text = posMenuLine.GlyphText3Font;
            ntbGlyphText3FontSize.Value = posMenuLine.GlyphText3FontSize;
            cwGlyphText3ForeColor.SelectedColor = Color.FromArgb(posMenuLine.GlyphText3ForeColor);
            ntbGlyph3Offset.Value = posMenuLine.Glyph3OffSet;

            CheckFont(posMenuLine.GlyphText3Font, posMenuLine.GlyphText3FontSize, btnEditGlyphFont3, errorProvider1, tbGlyphText3Font);                                                

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
            // Glyph 3
            if (posMenuLine.Glyph3 != (PosMenuLine.GlyphEnum)cmbGlyph3.SelectedIndex ||
                posMenuLine.GlyphText3 != tbGlyphText3.Text ||
                posMenuLine.GlyphText3Font != tbGlyphText3Font.Text ||
                posMenuLine.GlyphText3FontSize != (int)ntbGlyphText3FontSize.Value ||
                posMenuLine.GlyphText3ForeColor != cwGlyphText3ForeColor.SelectedColor.ToArgb() ||
                posMenuLine.Glyph3OffSet != (int)ntbGlyph3Offset.Value)
            {
                posMenuLine.Dirty = true;
                return true;
            }
            
            return false;
        }

        public bool SaveData()
        {
            // Glyph 3
            posMenuLine.Glyph3 = (PosMenuLine.GlyphEnum)cmbGlyph3.SelectedIndex;
            posMenuLine.GlyphText3 = tbGlyphText3.Text;
            posMenuLine.GlyphText3Font = tbGlyphText3Font.Text;
            posMenuLine.GlyphText3FontSize = (int)ntbGlyphText3FontSize.Value;
            posMenuLine.GlyphText3ForeColor = cwGlyphText3ForeColor.SelectedColor.ToArgb();
            posMenuLine.Glyph3OffSet = (int)ntbGlyph3Offset.Value;

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
            if (!String.IsNullOrEmpty(tbGlyphText3Font.Text))
            {
                fontDialog.Font = new Font(tbGlyphText3Font.Text, (float)ntbGlyphText3FontSize.Value);
            }

            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                CheckFont(fontDialog.Font.Name, (int)fontDialog.Font.Size, btnEditGlyphFont3, errorProvider1, tbGlyphText3Font);                                                        
            }
        }        

        private void cmbGlyph4_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((PosMenuLine.GlyphEnum)cmbGlyph3.SelectedIndex)
            {
                case PosMenuLine.GlyphEnum.None:
                    tbGlyphText3.Enabled = tbGlyphText3Font.Enabled = btnEditGlyphFont3.Enabled = ntbGlyphText3FontSize.Enabled = cwGlyphText3ForeColor.Enabled = ntbGlyph3Offset.Enabled = false;
                    break;

                case PosMenuLine.GlyphEnum.Text:
                    tbGlyphText3.Enabled = btnEditGlyphFont3.Enabled = true;
                    ntbGlyphText3FontSize.Enabled = cwGlyphText3ForeColor.Enabled = ntbGlyph3Offset.Enabled = true;
                    break;

                default:
                    tbGlyphText3.Enabled = btnEditGlyphFont3.Enabled = false;
                    ntbGlyphText3FontSize.Enabled = cwGlyphText3ForeColor.Enabled = ntbGlyph3Offset.Enabled = true;
                    break;
            }
        }

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {   
            // Glyph 3
            posMenuLineCopy.Glyph3 = (PosMenuLine.GlyphEnum)cmbGlyph3.SelectedIndex;
            posMenuLineCopy.GlyphText3 = tbGlyphText3.Text;
            posMenuLineCopy.GlyphText3Font = tbGlyphText3Font.Text;
            posMenuLineCopy.GlyphText3FontSize = (int)ntbGlyphText3FontSize.Value;
            posMenuLineCopy.GlyphText3ForeColor = cwGlyphText3ForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.Glyph3OffSet = (int)ntbGlyph3Offset.Value;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }
    }
}
