using System;
using System.Collections.Generic;
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
    public partial class HospitalityPosMenuLineGeneralPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;

        public HospitalityPosMenuLineGeneralPage()
        {
            InitializeComponent();
            posMenuLineCopy = new PosMenuLine();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityPosMenuLineGeneralPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuLine = (PosMenuLine)internalContext;

            chkBackgroundHidden.Checked = posMenuLine.BackgroundHidden;
            chkTransparent.Checked = posMenuLine.Transparent;
            ntbColumnSpan.Value = posMenuLine.ColumnSpan;
            ntbRowSpan.Value = posMenuLine.RowSpan;

            posMenuLineCopy = PluginOperations.CopyLine(posMenuLine);
        }

        public bool DataIsModified()
        {

            if (chkBackgroundHidden.Checked != posMenuLine.BackgroundHidden ||
                chkTransparent.Checked != posMenuLine.Transparent ||
                ntbColumnSpan.Value != (double)posMenuLine.ColumnSpan ||
                ntbRowSpan.Value != (double)posMenuLine.RowSpan)
            {
                posMenuLine.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            posMenuLine.BackgroundHidden = chkBackgroundHidden.Checked;
            posMenuLine.Transparent = chkTransparent.Checked;
            posMenuLine.ColumnSpan = (int)ntbColumnSpan.Value;
            posMenuLine.RowSpan = (int)ntbRowSpan.Value;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PreviewMenuButton":
                    PosMenuLine copy = (PosMenuLine)param;

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

                    // Glyphs tab
                    // Glyph 1
                    posMenuLineCopy.Glyph = copy.Glyph ;
                    posMenuLineCopy.GlyphText = copy.GlyphText ;
                    posMenuLineCopy.GlyphTextFont = copy.GlyphTextFont ;
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

        private void chkTransparent_CheckedChanged(object sender, EventArgs e)
        {            
            posMenuLineCopy.Transparent = chkTransparent.Checked;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }

        private void ntbColumnSpan_TextChanged(object sender, EventArgs e)
        {
            if (ntbColumnSpan.Value < 1)
            {
                ntbColumnSpan.Value = 1;
            }
        }

        private void ntbRowSpan_TextChanged(object sender, EventArgs e)
        {
            if (ntbRowSpan.Value < 1)
            {
                ntbRowSpan.Value = 1;
            }
        }
    }
}
