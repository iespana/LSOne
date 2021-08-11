using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Images;

namespace LSOne.ViewPlugins.TouchButtons.ViewPages
{
    public partial class PosButtonGridMenuLineAttributesPage : UserControl, ITabView
    {

        private PosMenuLine posMenuLine;
        private PosMenuLine posMenuLineCopy;
        private PosMenuHeader posMenuHeader;
        private PosStyle posStyle;
        private bool doNotify;

        public PosButtonGridMenuLineAttributesPage()
        {
            InitializeComponent();
            posMenuLineCopy = new PosMenuLine();
            posMenuHeader = new PosMenuHeader();

            doNotify = true;   
       
            cmbShape.Items.Clear();
            cmbShape.Items.AddRange(ButtonStyleUtils.GetShapeTexts());

            cmbGradientMode.Items.Clear();
            cmbGradientMode.Items.AddRange(ButtonStyleUtils.GetPartialGradientTexts());
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.PosButtonGridMenuLineAttributesPage();
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

                //posStyle is the current style selected - needs to be updated if a new style has been selected by the user
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
                
                chkUseHeaderConfiguration.Checked = posMenuLine.StyleID != RecordIdentifier.Empty ? false : posMenuLine.UseHeaderAttributes;

                doNotify = false;

                //If there is a style on the button then use those properties
                if (posMenuLine.StyleID != RecordIdentifier.Empty)
                {
                    cwBackColor.SelectedColor = Color.FromArgb(posStyle.BackColor);
                    cwBackColor2.SelectedColor = Color.FromArgb(posStyle.BackColor2);
                    cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(posStyle.GradientMode);
                    cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(posStyle.Shape);
                }
                else
                {
                    //If the use header configuration is not checked then use the menu line configuration
                    if (!chkUseHeaderConfiguration.Checked)
                    {
                        cwBackColor.SelectedColor = Color.FromArgb(posMenuLine.BackColor);
                        cwBackColor2.SelectedColor = Color.FromArgb(posMenuLine.BackColor2);
                        cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(posMenuLine.GradientMode);
                        cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(posMenuLine.Shape);
                    }
                    //If the button should use the header configuration then either use the menu header or the style
                    else
                    {
                        if (posMenuHeader.StyleID != RecordIdentifier.Empty)
                        {
                            cwBackColor.SelectedColor = Color.FromArgb(posStyle.BackColor);
                            cwBackColor2.SelectedColor = Color.FromArgb(posStyle.BackColor2);
                            cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(posStyle.GradientMode);
                            cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(posStyle.Shape);
                        }
                        else
                        {
                            cwBackColor.SelectedColor = Color.FromArgb(posMenuHeader.BackColor);
                            cwBackColor2.SelectedColor = Color.FromArgb(posMenuHeader.BackColor2);
                            cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(posMenuHeader.GradientMode);
                            cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(posMenuHeader.Shape);
                        }
                    }
                }

                posMenuLineCopy = PosMenuLine.Clone(posMenuLine);

                if (posMenuLine.StyleID != RecordIdentifier.Empty)
                {
                    ButtonPropertiesEnabled(false, false, notifyOfChange);
                }
                else
                {
                    ButtonPropertiesEnabled(!chkUseHeaderConfiguration.Checked, true, false);
                }
            }
            finally
            {                
                doNotify = true;
            }
        }

        public bool DataIsModified()
        {
            posMenuLine.StyleID = posMenuLine.StyleID == "" ? RecordIdentifier.Empty : posMenuLine.StyleID;

            if ((posMenuLine.StyleID == RecordIdentifier.Empty && cmbGradientMode.SelectedIndex != ButtonStyleUtils.GetIndexFromGradient(posMenuLine.GradientMode)) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && cwBackColor.SelectedColor != Color.FromArgb(posMenuLine.BackColor)) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && cwBackColor2.SelectedColor != Color.FromArgb(posMenuLine.BackColor2)) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && cmbShape.SelectedIndex != ButtonStyleUtils.GetIndexFromShape(posMenuLine.Shape)) ||
                (posMenuLine.StyleID == RecordIdentifier.Empty && chkUseHeaderConfiguration.Checked != posMenuLine.UseHeaderAttributes))
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
                posMenuLine.UseHeaderAttributes = chkUseHeaderConfiguration.Checked;
                //Only save the line attributes if Use header is false
                if (posMenuLine.UseHeaderAttributes == false)
                {
                    posMenuLine.GradientMode = ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex);
                    posMenuLine.BackColor = cwBackColor.SelectedColor.ToArgb();
                    posMenuLine.BackColor2 = cwBackColor2.SelectedColor.ToArgb();
                    posMenuLine.Shape = ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex);
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

                    // Copy from the view
                    posMenuLineCopy.Text = copy.Text;
                    posMenuLineCopy.StyleID = copy.StyleID;

                    if (posStyle == null || posStyle.ID != posMenuLineCopy.StyleID)
                    {
                        posStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, posMenuLineCopy.StyleID);
                    }                        
            
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
                    posMenuLineCopy.Transparent  = copy.Transparent;

                    // Glyphs tab
                    // Glyph 1
                    posMenuLineCopy.Glyph = copy.Glyph;
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

        private void notifyPreviewMenuButtonChanged(object sender, EventArgs args)
        {
            if (!doNotify) { return; }

            if (posMenuLineCopy.StyleID == RecordIdentifier.Empty)
            {
                posMenuLineCopy.UseHeaderAttributes = chkUseHeaderConfiguration.Checked;
                posMenuLineCopy.GradientMode = ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex);
                posMenuLineCopy.BackColor = cwBackColor.SelectedColor.ToArgb();
                posMenuLineCopy.BackColor2 = cwBackColor2.SelectedColor.ToArgb();
                posMenuLineCopy.Shape = ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex);
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
                    doNotify = false;

                    cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(posMenuLineCopy.GradientMode);
                    cwBackColor.SelectedColor = Color.FromArgb(posMenuLineCopy.BackColor);
                    cwBackColor2.SelectedColor = Color.FromArgb(posMenuLineCopy.BackColor2);
                    cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(posMenuLineCopy.Shape);
                }
                finally
                {
                    doNotify = true;
                }
            }
            ButtonPropertiesEnabled(!chkUseHeaderConfiguration.Checked, true, true);
        }

        private void ButtonPropertiesEnabled(bool enabled, bool useHeaderEnabled, bool notify)
        {
            cmbGradientMode.Enabled =
                cwBackColor.Enabled =
                cwBackColor2.Enabled =                
                cmbShape.Enabled =                 
                lblShape.Enabled =
                lblGradientMode.Enabled = 
                lblBackcolor2.Enabled =
                lblBackcolor.Enabled = enabled;

            chkUseHeaderConfiguration.Enabled =
                lblUserConfiguration.Enabled = useHeaderEnabled;

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
