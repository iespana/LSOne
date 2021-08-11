using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityPosMenuLineFontPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;

        public HospitalityPosMenuLineFontPage()
        {
            InitializeComponent();
            posMenuLineCopy = new PosMenuLine();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityPosMenuLineFontPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuLine = (PosMenuLine)internalContext;

            chkUseHeaderConfiguration.Checked = posMenuLine.UseHeaderFont;
            tbFontName.Text = posMenuLine.FontName;
            ntbFontSize.Value = posMenuLine.FontSize;
            chkFontBold.Checked = posMenuLine.FontBold;
            chkFontItalic.Checked = posMenuLine.FontItalic;
            chkFontStrikeThrough.Checked = posMenuLine.FontStrikethrough;
            chkFontUnderline.Checked = posMenuLine.FontUnderline;
            cwForeColor.SelectedColor = Color.FromArgb(posMenuLine.ForeColor);
            ntbFontCharset.Value = posMenuLine.FontCharset;

            posMenuLineCopy = PluginOperations.CopyLine(posMenuLine);
        }

        public bool DataIsModified()
        {

            if (posMenuLine.FontName != tbFontName.Text ||
                posMenuLine.FontSize != (int)ntbFontSize.Value ||
                posMenuLine.FontBold != chkFontBold.Checked ||
                posMenuLine.FontItalic != chkFontItalic.Checked ||
                posMenuLine.FontStrikethrough != chkFontStrikeThrough.Checked ||
                posMenuLine.FontUnderline != chkFontUnderline.Checked ||
                posMenuLine.ForeColor != cwForeColor.SelectedColor.ToArgb() ||
                posMenuLine.FontCharset != (int)ntbFontCharset.Value ||
                posMenuLine.UseHeaderFont != chkUseHeaderConfiguration.Checked)
            {
                posMenuLine.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            posMenuLine.UseHeaderFont = chkUseHeaderConfiguration.Checked;
            posMenuLine.FontName = tbFontName.Text;
            posMenuLine.FontSize = (int)ntbFontSize.Value;
            posMenuLine.FontBold = chkFontBold.Checked;
            posMenuLine.FontItalic = chkFontItalic.Checked;
            posMenuLine.FontStrikethrough = chkFontStrikeThrough.Checked;
            posMenuLine.FontUnderline = chkFontUnderline.Checked;
            posMenuLine.ForeColor = cwForeColor.SelectedColor.ToArgb();
            posMenuLine.FontCharset = (int)ntbFontCharset.Value;            

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
                case "PreviewMenuButton":
                    PosMenuLine copy = (PosMenuLine)param;

                    // Get changes from other tabs to update our own copy
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

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnEditFont_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbFontName.Text))
            {
                FontStyle style = FontStyle.Regular;

                if (chkFontBold.Checked) { style = style | FontStyle.Bold; }
                if (chkFontItalic.Checked) { style = style | FontStyle.Italic; }
                if (chkFontStrikeThrough.Checked) { style = style | FontStyle.Strikeout; }
                if (chkFontUnderline.Checked) { style = style | FontStyle.Underline; }

                fontDialog1.ShowEffects = true;
                fontDialog1.Color = cwForeColor.SelectedColor;

                fontDialog1.Font = new Font(
                    tbFontName.Text,
                    (float)ntbFontSize.Value,
                    style,
                    GraphicsUnit.Point, Convert.ToByte(ntbFontCharset.Value));
            }

            if (fontDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                tbFontName.Text = fontDialog1.Font.Name;
                ntbFontSize.Value = fontDialog1.Font.Size;
                chkFontBold.Checked = fontDialog1.Font.Bold;
                chkFontItalic.Checked = fontDialog1.Font.Italic;
                chkFontStrikeThrough.Checked = fontDialog1.Font.Strikeout;
                chkFontUnderline.Checked = fontDialog1.Font.Underline;
                cwForeColor.SelectedColor = fontDialog1.Color;
                ntbFontCharset.Value = Convert.ToInt16(fontDialog1.Font.GdiCharSet);
            }
        }

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {
            posMenuLineCopy.UseHeaderFont = chkUseHeaderConfiguration.Checked;
            posMenuLineCopy.FontName = tbFontName.Text;
            posMenuLineCopy.FontSize = (int)ntbFontSize.Value;
            posMenuLineCopy.FontBold = chkFontBold.Checked;
            posMenuLineCopy.FontItalic = chkFontItalic.Checked;
            posMenuLineCopy.FontStrikethrough = chkFontStrikeThrough.Checked;
            posMenuLineCopy.FontUnderline = chkFontUnderline.Checked;
            posMenuLineCopy.ForeColor = cwForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.FontCharset = (int)ntbFontCharset.Value;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }

        private void chkUseHeaderConfiguration_CheckedChanged(object sender, EventArgs e)
        {
            btnEditFont.Enabled =
                ntbFontSize.Enabled =
                chkFontBold.Enabled =
                chkFontItalic.Enabled =
                chkFontStrikeThrough.Enabled =
                chkFontUnderline.Enabled =
                cwForeColor.Enabled =
                ntbFontCharset.Enabled = !chkUseHeaderConfiguration.Checked;

            notifyPreviewMenuButtonChanged(this, EventArgs.Empty);
        }
    }
}
