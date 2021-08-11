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
    public partial class HospitalityPosMenuLineGlyphsPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;

        public HospitalityPosMenuLineGlyphsPage()
        {
            InitializeComponent();

            //InitialValues();

            posMenuLineCopy = new PosMenuLine();

            #region SettingEvents
            for (int i = 0; i < grpGlyph1.Controls.Count; i++)
            {
                if (grpGlyph1.Controls[i] is ComboBox)
                {
                    ((ComboBox)grpGlyph1.Controls[i]).SelectedIndexChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph1.Controls[i] is TextBox)
                {
                    ((TextBox)grpGlyph1.Controls[i]).TextChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph1.Controls[i] is NumericTextBox)
                {
                    ((NumericTextBox)grpGlyph1.Controls[i]).TextChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph1.Controls[i] is ColorWell)
                {
                    ((ColorWell)grpGlyph1.Controls[i]).SelectedColorChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
            }

            for (int i = 0; i < grpGlyph2.Controls.Count; i++)
            {
                if (grpGlyph2.Controls[i] is ComboBox)
                {
                    ((ComboBox)grpGlyph2.Controls[i]).SelectedIndexChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph2.Controls[i] is TextBox)
                {
                    ((TextBox)grpGlyph2.Controls[i]).TextChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph2.Controls[i] is NumericTextBox)
                {
                    ((NumericTextBox)grpGlyph2.Controls[i]).TextChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph2.Controls[i] is ColorWell)
                {
                    ((ColorWell)grpGlyph2.Controls[i]).SelectedColorChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
            }

            for (int i = 0; i < grpGlyph3.Controls.Count; i++)
            {
                if (grpGlyph3.Controls[i] is ComboBox)
                {
                    ((ComboBox)grpGlyph3.Controls[i]).SelectedIndexChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph3.Controls[i] is TextBox)
                {
                    ((TextBox)grpGlyph3.Controls[i]).TextChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph3.Controls[i] is NumericTextBox)
                {
                    ((NumericTextBox)grpGlyph3.Controls[i]).TextChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph3.Controls[i] is ColorWell)
                {
                    ((ColorWell)grpGlyph3.Controls[i]).SelectedColorChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
            }

            for (int i = 0; i < grpGlyph4.Controls.Count; i++)
            {
                if (grpGlyph4.Controls[i] is ComboBox)
                {
                    ((ComboBox)grpGlyph4.Controls[i]).SelectedIndexChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph4.Controls[i] is TextBox)
                {
                    ((TextBox)grpGlyph4.Controls[i]).TextChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph4.Controls[i] is NumericTextBox)
                {
                    ((NumericTextBox)grpGlyph4.Controls[i]).TextChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
                else if (grpGlyph4.Controls[i] is ColorWell)
                {
                    ((ColorWell)grpGlyph4.Controls[i]).SelectedColorChanged += new EventHandler(notifyPreviewMenuButtonChanged);
                }
            }
            #endregion
        }

        private void InitialValues()
        {
            int defaultOffset = 1;
            int defaultSize = 5;

            ntbGlyphOffset.Value = defaultOffset;
            ntbGlyph2Offset.Value = defaultOffset;
            ntbGlyph3Offset.Value = defaultOffset;
            ntbGlyph4Offset.Value = defaultOffset;

            ntbGlyphTextFontSize.Value = defaultSize;
            ntbGlyphText2FontSize.Value = defaultSize;
            ntbGlyphText3FontSize.Value = defaultSize;
            ntbGlyphText4FontSize.Value = defaultSize;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityPosMenuLineGlyphsPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuLine = (PosMenuLine)internalContext;
            
            // Glyph 1
            cmbGlyph.SelectedIndex = (int)posMenuLine.Glyph;
            tbGlyphText.Text = posMenuLine.GlyphText;
            tbGlyphTextFont.Text = posMenuLine.GlyphTextFont;
            ntbGlyphTextFontSize.Value = posMenuLine.GlyphTextFontSize;
            cwGlyphTextForeColor.SelectedColor = Color.FromArgb(posMenuLine.GlyphTextForeColor);
            ntbGlyphOffset.Value = posMenuLine.GlyphOffSet;

            // Glyph 2
            cmbGlyph2.SelectedIndex = (int)posMenuLine.Glyph2;
            tbGlyphText2.Text = posMenuLine.GlyphText2;
            tbGlyphText2Font.Text = posMenuLine.GlyphText2Font;
            ntbGlyphText2FontSize.Value = posMenuLine.GlyphText2FontSize;
            cwGlyphText2ForeColor.SelectedColor = Color.FromArgb(posMenuLine.GlyphText2ForeColor);
            ntbGlyph2Offset.Value = posMenuLine.Glyph2OffSet;

            // Glyph 3
            cmbGlyph3.SelectedIndex = (int)posMenuLine.Glyph3;
            tbGlyphText3.Text = posMenuLine.GlyphText3;
            tbGlyphText3Font.Text = posMenuLine.GlyphText3Font;
            ntbGlyphText3FontSize.Value = posMenuLine.GlyphText3FontSize;
            cwGlyphText3ForeColor.SelectedColor = Color.FromArgb(posMenuLine.GlyphText3ForeColor);
            ntbGlyph3Offset.Value = posMenuLine.Glyph3OffSet;

            // Glyph 4
            cmbGlyph4.SelectedIndex = (int)posMenuLine.Glyph4;
            tbGlyphText4.Text = posMenuLine.GlyphText4;
            tbGlyphText4Font.Text = posMenuLine.GlyphText4Font;
            ntbGlyphText4FontSize.Value = posMenuLine.GlyphText4FontSize;
            cwGlyphText4ForeColor.SelectedColor = Color.FromArgb(posMenuLine.GlyphText4ForeColor);
            ntbGlyph4Offset.Value = posMenuLine.Glyph4OffSet;

            posMenuLineCopy = PluginOperations.CopyLine(posMenuLine);
        }

        public bool DataIsModified()
        {
            // Glyph 1
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
            // Glyph 1
            posMenuLine.Glyph = (PosMenuLine.GlyphEnum)cmbGlyph.SelectedIndex;
            posMenuLine.GlyphText = tbGlyphText.Text;
            posMenuLine.GlyphTextFont = tbGlyphTextFont.Text;
            posMenuLine.GlyphTextFontSize = (int)ntbGlyphTextFontSize.Value;
            posMenuLine.GlyphTextForeColor = cwGlyphTextForeColor.SelectedColor.ToArgb();
            posMenuLine.GlyphOffSet = (int)ntbGlyphOffset.Value;

            // Glyph 2
            posMenuLine.Glyph2 = (PosMenuLine.GlyphEnum)cmbGlyph2.SelectedIndex;
            posMenuLine.GlyphText2 = tbGlyphText2.Text;
            posMenuLine.GlyphText2Font = tbGlyphText2Font.Text;
            posMenuLine.GlyphText2FontSize = (int)ntbGlyphText2FontSize.Value;
            posMenuLine.GlyphText2ForeColor = cwGlyphText2ForeColor.SelectedColor.ToArgb();
            posMenuLine.Glyph2OffSet = (int)ntbGlyph2Offset.Value;

            // Glyph 3
            posMenuLine.Glyph3 = (PosMenuLine.GlyphEnum)cmbGlyph3.SelectedIndex;
            posMenuLine.GlyphText3 = tbGlyphText3.Text;
            posMenuLine.GlyphText3Font = tbGlyphText3Font.Text;
            posMenuLine.GlyphText3FontSize = (int)ntbGlyphText3FontSize.Value;
            posMenuLine.GlyphText3ForeColor = cwGlyphText3ForeColor.SelectedColor.ToArgb();
            posMenuLine.Glyph3OffSet = (int)ntbGlyph3Offset.Value;

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

                    // General tab
                    posMenuLineCopy.Transparent  = copy.Transparent ;

                    // Attributes tab
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

        private void btnEditGlyphFont_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbGlyphTextFont.Text))
            {
                fontDialog.Font = new Font(tbGlyphTextFont.Text, (float)ntbGlyphTextFontSize.Value);
            }
            
            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                tbGlyphTextFont.Text = fontDialog.Font.Name;
                ntbGlyphTextFontSize.Value = fontDialog.Font.Size;
            }
        }

        private void btnEditGlyphFont2_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbGlyphText2Font.Text))
            {
                fontDialog.Font = new Font(tbGlyphText2Font.Text, (float)ntbGlyphText2FontSize.Value);
            }

            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                tbGlyphText2Font.Text = fontDialog.Font.Name;
                ntbGlyphText2FontSize.Value = fontDialog.Font.Size;
            }
        }

        private void btnEditGlyphFont3_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbGlyphText3Font.Text))
            {
                fontDialog.Font = new Font(tbGlyphText3Font.Text, (float)ntbGlyphText3FontSize.Value);
            }

            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                tbGlyphText3Font.Text = fontDialog.Font.Name;
                ntbGlyphText3FontSize.Value = fontDialog.Font.Size;
            }
        }

        private void btnEditGlyphFont4_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbGlyphText4Font.Text))
            {
                fontDialog.Font = new Font(tbGlyphText4Font.Text, (float)ntbGlyphText4FontSize.Value);
            }

            if (fontDialog.ShowDialog(this) == DialogResult.OK)
            {
                tbGlyphText4Font.Text = fontDialog.Font.Name;
                ntbGlyphText4FontSize.Value = fontDialog.Font.Size;
            }
        }

        private void cmbGlyph_SelectedIndexChanged(object sender, EventArgs e)
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

        private void cmbGlyph2_SelectedIndexChanged(object sender, EventArgs e)
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

        private void cmbGlyph3_SelectedIndexChanged(object sender, EventArgs e)
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
            // Glyph 1
            posMenuLineCopy.Glyph = (PosMenuLine.GlyphEnum)cmbGlyph.SelectedIndex;
            posMenuLineCopy.GlyphText = tbGlyphText.Text;
            posMenuLineCopy.GlyphTextFont = tbGlyphTextFont.Text;
            posMenuLineCopy.GlyphTextFontSize = (int)ntbGlyphTextFontSize.Value;
            posMenuLineCopy.GlyphTextForeColor = cwGlyphTextForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.GlyphOffSet = (int)ntbGlyphOffset.Value;

            // Glyph 2
            posMenuLineCopy.Glyph2 = (PosMenuLine.GlyphEnum)cmbGlyph2.SelectedIndex;
            posMenuLineCopy.GlyphText2 = tbGlyphText2.Text;
            posMenuLineCopy.GlyphText2Font = tbGlyphText2Font.Text;
            posMenuLineCopy.GlyphText2FontSize = (int)ntbGlyphText2FontSize.Value;
            posMenuLineCopy.GlyphText2ForeColor = cwGlyphText2ForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.Glyph2OffSet = (int)ntbGlyph2Offset.Value;

            // Glyph 3
            posMenuLineCopy.Glyph3 = (PosMenuLine.GlyphEnum)cmbGlyph3.SelectedIndex;
            posMenuLineCopy.GlyphText3 = tbGlyphText3.Text;
            posMenuLineCopy.GlyphText3Font = tbGlyphText3Font.Text;
            posMenuLineCopy.GlyphText3FontSize = (int)ntbGlyphText3FontSize.Value;
            posMenuLineCopy.GlyphText3ForeColor = cwGlyphText3ForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.Glyph3OffSet = (int)ntbGlyph3Offset.Value;

            // Glyph 4
            posMenuLineCopy.Glyph4 = (PosMenuLine.GlyphEnum)cmbGlyph4.SelectedIndex;
            posMenuLineCopy.GlyphText4 = tbGlyphText4.Text;
            posMenuLineCopy.GlyphText4Font = tbGlyphText4Font.Text;
            posMenuLineCopy.GlyphText4FontSize = (int)ntbGlyphText4FontSize.Value;
            posMenuLineCopy.GlyphText4ForeColor = cwGlyphText4ForeColor.SelectedColor.ToArgb();
            posMenuLineCopy.Glyph4OffSet = (int)ntbGlyph4Offset.Value;

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }
    }
}
