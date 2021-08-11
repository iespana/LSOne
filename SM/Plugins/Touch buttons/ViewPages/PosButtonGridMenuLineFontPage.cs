using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    public partial class PosButtonGridMenuLineFontPage : UserControl, ITabView
    {
        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;
        private PosMenuHeader posMenuHeader;
        private PosStyle posStyle;
        private bool doNotification;

        public PosButtonGridMenuLineFontPage()
        {
            InitializeComponent();
            posMenuLineCopy = new PosMenuLine();
            posMenuHeader = new PosMenuHeader();
            doNotification = true;

            cmbTextPosition.Items.Clear();
            cmbTextPosition.Items.AddRange(ButtonStyleUtils.GetPositionTexts());
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PosButtonGridMenuLineFontPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuLine = (PosMenuLine)internalContext;
            
            if (posMenuHeader.ID == RecordIdentifier.Empty)
            {
                posMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuLine.MenuID);
            }

            LoadData((PosMenuLine)internalContext, true);
        }

        public void LoadData(PosMenuLine posMenuLine, bool notifyOfChange)
        {
            try
            {
                PosStyle buttonStyle = new PosStyle(RecordIdentifier.Empty, "");

                if (posStyle == null || posStyle.ID != posMenuLine.StyleID)
                {
                    posStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, posMenuLine.StyleID);
                }

                //If neither the button or the header has a style then clear buttonStyle 
                if (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuHeader.StyleID == RecordIdentifier.Empty)
                {
                    buttonStyle = new PosStyle(RecordIdentifier.Empty, "");
                }
                //if the button has a style then it should be used
                else if (posMenuLine.StyleID != RecordIdentifier.Empty)
                {
                    buttonStyle = posStyle; // Providers.PosStyleData.Get(PluginEntry.DataModel, posMenuLine.StyleID);
                }
                //if the button doesn't have a style but the header has one then that should be used
                else if (posMenuHeader.StyleID != RecordIdentifier.Empty)
                {
                    buttonStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, posMenuHeader.StyleID);
                }      

                posMenuLine.StyleID = posMenuLine.StyleID == "" ? RecordIdentifier.Empty : posMenuLine.StyleID;
                posMenuHeader.StyleID = posMenuHeader.StyleID == "" ? RecordIdentifier.Empty : posMenuHeader.StyleID;

                chkUseHeaderConfiguration.Checked = posMenuLine.StyleID != RecordIdentifier.Empty ? false : posMenuLine.UseHeaderFont;

                doNotification = false;

                //If there is a style on the button then use those properties
                if (posMenuLine.StyleID != RecordIdentifier.Empty)
                {
                    tbFontName.Text = buttonStyle.FontName;
                    ntbFontSize.Value = buttonStyle.FontSize;
                    chkFontBold.Checked = buttonStyle.FontBold;
                    chkFontItalic.Checked = buttonStyle.FontItalic;
                    chkFontStrikeThrough.Checked = false;
                    chkFontUnderline.Checked = false;
                    cwForeColor.SelectedColor = Color.FromArgb(buttonStyle.ForeColor);
                    ntbFontCharset.Value = buttonStyle.FontCharset;
                    cmbTextPosition.SelectedIndex = ButtonStyleUtils.GetIndexFromPosition(buttonStyle.TextPosition);
                }
                else
                {
                    //If the use header configuration is not checked then use the menu line configuration
                    if (!chkUseHeaderConfiguration.Checked)
                    {
                        tbFontName.Text = posMenuLine.FontName;
                        ntbFontSize.Value = posMenuLine.FontSize;
                        chkFontBold.Checked = posMenuLine.FontBold;
                        chkFontItalic.Checked = posMenuLine.FontItalic;
                        chkFontStrikeThrough.Checked = posMenuLine.FontStrikethrough;
                        chkFontUnderline.Checked = posMenuLine.FontUnderline;
                        cwForeColor.SelectedColor = Color.FromArgb(posMenuLine.ForeColor);
                        ntbFontCharset.Value = posMenuLine.FontCharset;
                        cmbTextPosition.SelectedIndex = ButtonStyleUtils.GetIndexFromPosition(posMenuLine.TextPosition);
                    }
                    //If the button should use the header configuration then either use the menu header or the style
                    else
                    {
                        if (posMenuHeader.StyleID != RecordIdentifier.Empty)
                        {
                            tbFontName.Text = buttonStyle.FontName;
                            ntbFontSize.Value = buttonStyle.FontSize;
                            chkFontBold.Checked = buttonStyle.FontBold;
                            chkFontItalic.Checked = buttonStyle.FontItalic;
                            chkFontStrikeThrough.Checked = false;
                            chkFontUnderline.Checked = false;
                            cwForeColor.SelectedColor = Color.FromArgb(buttonStyle.ForeColor);
                            ntbFontCharset.Value = buttonStyle.FontCharset;
                            cmbTextPosition.SelectedIndex = ButtonStyleUtils.GetIndexFromPosition(buttonStyle.TextPosition);
                        }
                        else
                        {
                            tbFontName.Text = posMenuHeader.FontName;
                            ntbFontSize.Value = posMenuHeader.FontSize;
                            chkFontBold.Checked = posMenuHeader.FontBold;
                            chkFontItalic.Checked = posMenuHeader.FontItalic;
                            chkFontStrikeThrough.Checked = false;
                            chkFontUnderline.Checked = false;
                            cwForeColor.SelectedColor = Color.FromArgb(posMenuHeader.ForeColor);
                            ntbFontCharset.Value = posMenuHeader.FontCharset;
                            cmbTextPosition.SelectedIndex = ButtonStyleUtils.GetIndexFromPosition(posMenuHeader.TextPosition);
                        }
                    }
                }

                posMenuLineCopy = PosMenuLine.Clone(posMenuLine);

                if (posMenuLine.StyleID != RecordIdentifier.Empty)
                {
                    FontPropertiesEnabled(false, false, notifyOfChange);
                }
                else
                {
                    FontPropertiesEnabled(!chkUseHeaderConfiguration.Checked, true, false);
                }
            }
            finally
            {
                doNotification = true;
            }
        }

        public bool DataIsModified()
        {
            posMenuLine.StyleID = posMenuLine.StyleID == "" ? RecordIdentifier.Empty : posMenuLine.StyleID;

            if ((posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.FontName != tbFontName.Text) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.FontSize != (int)ntbFontSize.Value) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.FontBold != chkFontBold.Checked) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.FontItalic != chkFontItalic.Checked) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.FontStrikethrough != chkFontStrikeThrough.Checked) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.FontUnderline != chkFontUnderline.Checked) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.ForeColor != cwForeColor.SelectedColor.ToArgb()) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.FontCharset != (int)ntbFontCharset.Value) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.UseHeaderFont != chkUseHeaderConfiguration.Checked) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && posMenuLine.TextPosition != ButtonStyleUtils.GetPositionFromIndex(cmbTextPosition.SelectedIndex)))
            {
                posMenuLine.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            posStyle.ID = posStyle.ID == "" ? RecordIdentifier.Empty : posStyle.ID;

            if (posStyle.ID == RecordIdentifier.Empty)
            {
                posMenuLine.UseHeaderFont = chkUseHeaderConfiguration.Checked;
                //Only save the line attributes if Use header is false
                if (posMenuLine.UseHeaderFont == false)
                {
                    posMenuLine.FontName = tbFontName.Text;
                    posMenuLine.FontSize = (int)ntbFontSize.Value;
                    posMenuLine.FontBold = chkFontBold.Checked;
                    posMenuLine.FontItalic = chkFontItalic.Checked;
                    posMenuLine.FontStrikethrough = chkFontStrikeThrough.Checked;
                    posMenuLine.FontUnderline = chkFontUnderline.Checked;
                    posMenuLine.ForeColor = cwForeColor.SelectedColor.ToArgb();
                    posMenuLine.FontCharset = (int)ntbFontCharset.Value;
                    posMenuLine.TextPosition = ButtonStyleUtils.GetPositionFromIndex(cmbTextPosition.SelectedIndex);
                }
            }

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
                    posMenuLineCopy.StyleID = copy.StyleID;

                    if (posStyle == null || posStyle.ID != posMenuLineCopy.StyleID)
                    {
                        posStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, posMenuLineCopy.StyleID);
                    }

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

                    LoadData(copy, false);

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
                try
                {
                    doNotification = false;

                    tbFontName.Text = fontDialog1.Font.Name;
                    ntbFontSize.Value = fontDialog1.Font.Size;
                    chkFontBold.Checked = fontDialog1.Font.Bold;
                    chkFontItalic.Checked = fontDialog1.Font.Italic;
                    chkFontStrikeThrough.Checked = fontDialog1.Font.Strikeout;
                    chkFontUnderline.Checked = fontDialog1.Font.Underline;
                    cwForeColor.SelectedColor = fontDialog1.Color;
                    ntbFontCharset.Value = Convert.ToInt16(fontDialog1.Font.GdiCharSet);
                }
                finally
                {
                    doNotification = true;
                    notifyPreviewMenuButtonChanged(this, EventArgs.Empty);
                }
            }
        }

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {
            if (!doNotification) { return; }

            if (posMenuLineCopy.StyleID == RecordIdentifier.Empty)
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
                posMenuLineCopy.TextPosition = ButtonStyleUtils.GetPositionFromIndex(cmbTextPosition.SelectedIndex);
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "ButtonGridPreviewMenuButton", RecordIdentifier.Empty, posMenuLineCopy);
        }

        private void chkUseHeaderConfiguration_CheckedChanged(object sender, EventArgs e)
        {
            //If the use header configuration is being taken off then we need to set the values back to what the posMenuLineCopy represents
            //before sending out the notification otherwise the posStyle/posMenuHeader values will overwrite either the org or changed values which will then be lost
            if (chkUseHeaderConfiguration.Checked == false)
            {
                try
                {
                    doNotification = false;
                    tbFontName.Text = posMenuLineCopy.FontName;
                    ntbFontSize.Value = posMenuLineCopy.FontSize;
                    chkFontBold.Checked = posMenuLineCopy.FontBold;
                    chkFontItalic.Checked = posMenuLineCopy.FontItalic;
                    chkFontStrikeThrough.Checked = posMenuLineCopy.FontStrikethrough;
                    chkFontUnderline.Checked = posMenuLineCopy.FontUnderline;
                    cwForeColor.SelectedColor = Color.FromArgb(posMenuLineCopy.ForeColor);
                    ntbFontCharset.Value = posMenuLineCopy.FontCharset;
                    cmbTextPosition.SelectedIndex = ButtonStyleUtils.GetIndexFromPosition(posMenuLineCopy.TextPosition);
                }
                finally
                {
                    doNotification = true;
                }
            }
            FontPropertiesEnabled(!chkUseHeaderConfiguration.Checked, true, true);
        }

        private void FontPropertiesEnabled(bool enabled, bool useHeaderEnabled, bool notify)
        {
            btnEditFont.Enabled =
                ntbFontSize.Enabled =
                chkFontBold.Enabled =
                chkFontItalic.Enabled =
                chkFontStrikeThrough.Enabled =
                chkFontUnderline.Enabled =
                cwForeColor.Enabled =
                tbFontName.Enabled =                
                ntbFontCharset.Enabled =
                lblFontBold.Enabled = 
                lblFontCharset.Enabled = 
                lblFontColor.Enabled = 
                lblFontItalic.Enabled = 
                lblFontName.Enabled = 
                lblFontSize.Enabled = 
                lblFontStrikethrough.Enabled = 
                lblFontUnderline.Enabled = 
                lblTextPosition.Enabled =
                cmbTextPosition.Enabled = enabled;

            chkUseHeaderConfiguration.Enabled  =
                lblHeadConfiguration.Enabled = useHeaderEnabled;

            if (notify)
            {
                notifyPreviewMenuButtonChanged(this, EventArgs.Empty);
            }
            else if (!notify && posMenuLine.StyleID == RecordIdentifier.Empty)
            {
                notifyPreviewMenuButtonChanged(this, EventArgs.Empty);
            }
        }
    }
}
