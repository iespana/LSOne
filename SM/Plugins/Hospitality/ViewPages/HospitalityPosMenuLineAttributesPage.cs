using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityPosMenuLineAttributesPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;

        public HospitalityPosMenuLineAttributesPage()
        {
            InitializeComponent();
            posMenuLineCopy = new PosMenuLine();

            cmbShape.Items.Clear();
            cmbShape.Items.AddRange(ButtonStyleUtils.GetShapeTexts());

            cmbGradientMode.Items.Clear();
            cmbGradientMode.Items.AddRange(ButtonStyleUtils.GetPartialGradientTexts());
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityPosMenuLineAttributesPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuLine = (PosMenuLine)internalContext;

            chkUseHeaderConfiguration.Checked = posMenuLine.UseHeaderAttributes;
            cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(posMenuLine.GradientMode);
            cwBackColor.SelectedColor = Color.FromArgb(posMenuLine.BackColor);
            cwBackColor2.SelectedColor = Color.FromArgb(posMenuLine.BackColor2);
            cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(posMenuLine.Shape);

            posMenuLineCopy = PluginOperations.CopyLine(posMenuLine);
        }

        public bool DataIsModified()
        {
            if (cmbGradientMode.SelectedIndex != ButtonStyleUtils.GetIndexFromGradient(posMenuLine.GradientMode) ||
                cwBackColor.SelectedColor != Color.FromArgb(posMenuLine.BackColor) ||
                cwBackColor2.SelectedColor != Color.FromArgb(posMenuLine.BackColor2) ||
                cmbShape.SelectedIndex != ButtonStyleUtils.GetIndexFromShape(posMenuLine.Shape) ||
                chkUseHeaderConfiguration.Checked != posMenuLine.UseHeaderAttributes)
            {
                posMenuLine.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {            
            posMenuLine.UseHeaderAttributes = chkUseHeaderConfiguration.Checked;
            posMenuLine.GradientMode = ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex);
            posMenuLine.BackColor = cwBackColor.SelectedColor.ToArgb();
            posMenuLine.BackColor2 = cwBackColor2.SelectedColor.ToArgb();
            posMenuLine.Shape = ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex);

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
                    // Copy from the other tabs

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

                    // General tab
                    posMenuLineCopy.Transparent  = copy.Transparent ;

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

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {
            posMenuLineCopy.UseHeaderAttributes = chkUseHeaderConfiguration.Checked;
            posMenuLineCopy.GradientMode = ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex);
            posMenuLineCopy.BackColor = cwBackColor.SelectedColor.ToArgb();
            posMenuLineCopy.BackColor2 = cwBackColor2.SelectedColor.ToArgb();
            posMenuLineCopy.Shape = ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }

        private void chkUseHeaderConfiguration_CheckedChanged(object sender, EventArgs e)
        {
            cmbGradientMode.Enabled =
                cwBackColor.Enabled =
                cwBackColor2.Enabled =
                cmbShape.Enabled = !chkUseHeaderConfiguration.Checked;

            notifyPreviewMenuButtonChanged(this, EventArgs.Empty);
        }
    }
}
