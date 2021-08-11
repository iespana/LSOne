using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.Enums;
using LSOne.ViewCore.Dialogs;
using Style = LSOne.DataLayer.BusinessObjects.TouchButtons.Style;

namespace LSOne.ViewPlugins.Hospitality.Controls
{
    public partial class ButtonPropertiesControl : UserControl
    {
        public event EventHandler Modified;

        private RecordIdentifier posStyleID;
        private PosStyle lastStyle;
        private PosStyle useButtonProperties;
        private bool initializing;
        private bool enableStyleUse;
        private PosMenuHeader posMenuHeader;
        private bool suspendEvents;

        public ButtonPropertiesControl()
        {
            initializing = true;
            suspendEvents = false;

            InitializeComponent();

            cmbShape.Items.Clear();
            cmbShape.Items.AddRange(ButtonStyleUtils.GetShapeTexts());

            cmbGradientMode.Items.Clear();
            cmbGradientMode.Items.AddRange(ButtonStyleUtils.GetPartialGradientTexts());

            cmbTextPosition.Items.Clear();
            cmbTextPosition.Items.AddRange(ButtonStyleUtils.GetPositionTexts());

            cmbStyle.AutoSelectOnEmpty = false;

            useButtonProperties = new PosStyle(RecordIdentifier.Empty, "");

            posMenuHeader = new PosMenuHeader();

            initializing = false;
        }

        #region Properties
        [DefaultValue(false)]
        public bool AutoSelectOnEmpty
        {
            get { return cmbStyle.AutoSelectOnEmpty; }
            set { cmbStyle.AutoSelectOnEmpty = value; }
        }

        [Browsable(false)]
        public bool EnableStyleUse
        {
            get { return enableStyleUse; }
            set
            {
                enableStyleUse = value;
                FireModifiedEvent();
            }
        }

        [Browsable(false)]
        public RecordIdentifier PosStyleID
        {
            get { return posStyleID; }
            set
            { 
                posStyleID = value;
                StyleSet(false);
            }
        }

        [Browsable(false)]
        public string FontName
        {
            get { return tbFontName.Text; }
            set { tbFontName.Text = value; }
        }

        [Browsable(false)]
        public int FontSize
        {
            get { return (int)ntbFontSize.Value; }
            set { ntbFontSize.Value = value; }
        }

        [Browsable(false)]
        public bool FontBold
        {
            get { return chkFontBold.Checked; }
            set { chkFontBold.Checked = value; }
        }

        [Browsable(false)]
        public bool FontItalic
        {
            get { return chkFontItalic.Checked; }
            set { chkFontItalic.Checked = value; }
        }

        [Browsable(false)]
        public int FontCharset
        {
            get { return (int)ntbFontCharset.Value; }
            set { ntbFontCharset.Value = value; }
        }

        [Browsable(false)]
        public Color StyleForeColor
        {
            get { return cwForeColor.SelectedColor; }
            set { cwForeColor.SelectedColor = value; }
        }

        [Browsable(false)]
        public Color StyleBackColor
        {
            get { return cwBackColor.SelectedColor; }
            set { cwBackColor.SelectedColor = value; }
        }

        [Browsable(false)]
        public Color StyleBackColor2
        {
            get { return cwBackColor2.SelectedColor; }
            set { cwBackColor2.SelectedColor = value; }
        }

        [Browsable(false)]
        public GradientModeEnum GradientMode
        {
            get { return ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex); }
            set { cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(value); }
        }

        [Browsable(false)]
        public ShapeEnum Shape
        {
            get { return ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex); }
            set { cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(value); }
        }

        [Browsable(false)]
        public Position TextPosition
        {
            get { return ButtonStyleUtils.GetPositionFromIndex(cmbTextPosition.SelectedIndex); }
            set { cmbTextPosition.SelectedIndex = ButtonStyleUtils.GetIndexFromPosition(value); }
        }
        #endregion

        #region Style events

        private void StyleSet(bool fromUI)
        {
            posStyleID = (posStyleID == null || posStyleID == "") ? RecordIdentifier.Empty : posStyleID;
            if (posStyleID != RecordIdentifier.Empty)
            {
                cmbStyle.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, posStyleID);
            }
            else
            {
                if (fromUI && lastStyle != null
                    && (DialogResult.Yes == MessageDialog.Show(Properties.Resources.CopyCurrentStyle, Properties.Resources.CopyCurrentStyleHeader, MessageBoxButtons.YesNo, MessageBoxIcon.Question)))
                {
                    // Set useButtonProperties to the same values as the current style (easy modification from style)
                    lastStyle.CopyStyleTo(useButtonProperties);
                }
                cmbStyle.SelectedData = useButtonProperties;
            }

            if (cmbStyle.SelectedData.ID != RecordIdentifier.Empty)
            {
                var posStyle = Providers.PosStyleData.Get(PluginEntry.DataModel, cmbStyle.SelectedData.ID);
                if (posStyle.ID == RecordIdentifier.Empty)
                {
                    cmbStyle.SelectedData = useButtonProperties;
                    btnEditStyle.Enabled = false;
                }
                else
                {
                    cmbStyle.SelectedData = posStyle;
                    btnEditStyle.Enabled = true;
                }
            }
            else
            {
                cmbStyle.SelectedData = useButtonProperties;
                btnEditStyle.Enabled = false;
            }

            lastStyle = (PosStyle)cmbStyle.SelectedData;

            lblStyle.Visible = EnableStyleUse;
            styleFlowLayout.Visible = EnableStyleUse;
            
            SetFromStyle();

            EnableDisableControls();
        }

        private void cmbStyle_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = useButtonProperties;            
            
            EnableDisableControls();
            lastStyle = null;
        }

        private void cmbStyle_RequestData(object sender, EventArgs e)
        {
            var styleList = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME ASC");
            cmbStyle.SetData(styleList, null);
        }

        private void cmbStyle_SelectedDataChanged(object sender, EventArgs e)
        {
            posStyleID = cmbStyle.SelectedData.ID;
            posStyleID = posStyleID == "" ? RecordIdentifier.Empty : posStyleID;

            StyleSet(true);
            FireModifiedEvent();

            lastStyle = (PosStyle) cmbStyle.SelectedData;
        }

        private void btnEditStyle_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowEditStyleDialog(cmbStyle.SelectedData.ID);
            cmbStyle_SelectedDataChanged(this, e);
            FireModifiedEvent();
        }
        #endregion

        private void EnableDisableControls()
        {
            bool enabled = (cmbStyle.SelectedData.ID == RecordIdentifier.Empty);
            tbFontName.Enabled = enabled;
            btnEditFont.Enabled = enabled;
            cwBackColor.Enabled = enabled;
            cwBackColor2.Enabled = enabled;
            cwForeColor.Enabled = enabled;
            cmbGradientMode.Enabled = enabled;
            cmbShape.Enabled = enabled;
            cmbTextPosition.Enabled = enabled;
        }

        #region PosStyle helpers
        public PosStyle PosStyle
        {
            get
            {
                var style = new PosStyle();
                ToStyle(style);
                return style;
            }
            set
            {
                if (value != null)
                {
                    FontName = value.FontName;
                    FontSize = value.FontSize;
                    FontBold = value.FontBold;
                    FontItalic = value.FontItalic;
                    StyleForeColor = Color.FromArgb(value.ForeColor);
                    FontCharset = value.FontCharset;
                    StyleBackColor = Color.FromArgb(value.BackColor);
                    StyleBackColor2 = Color.FromArgb(value.BackColor2);
                    GradientMode = value.GradientMode;
                    Shape = value.Shape;
                    TextPosition = value.TextPosition;
                }
            }
        }

        public void ToStyle(PosStyle style)
        {
            style.FontName = FontName;
            style.FontSize = FontSize;
            style.FontBold = FontBold;
            style.FontItalic = FontItalic;
            style.ForeColor = StyleForeColor.ToArgb();
            style.FontCharset = FontCharset;
            style.BackColor = StyleBackColor.ToArgb();
            style.BackColor2 = StyleBackColor2.ToArgb();
            style.GradientMode = GradientMode;
            style.Shape = Shape;
            style.TextPosition = TextPosition;
        }

        public bool IsModified(PosStyle style)
        {
            if (tbFontName.Text != style.FontName) return true;
            if ((int)ntbFontSize.Value != style.FontSize) return true;
            if (chkFontBold.Checked != style.FontBold) return true;
            if (chkFontItalic.Checked != style.FontItalic) return true;
            if (cwForeColor.SelectedColor != Color.FromArgb(style.ForeColor)) return true;
            if ((int)ntbFontCharset.Value != style.FontCharset) return true;
            if (cwBackColor.SelectedColor != Color.FromArgb(style.BackColor)) return true;
            if (cwBackColor2.SelectedColor != Color.FromArgb(style.BackColor2)) return true;
            if (cmbGradientMode.SelectedIndex != ButtonStyleUtils.GetIndexFromGradient(style.GradientMode)) return true;
            if (cmbShape.SelectedIndex != ButtonStyleUtils.GetIndexFromShape(style.Shape)) return true;
            if (cmbStyle.SelectedData.ID != posStyleID) return true;
            if (cmbTextPosition.SelectedIndex != ButtonStyleUtils.GetIndexFromPosition(style.TextPosition)) return true;

            return false;
        }
        #endregion

        #region Style helpers
        public Style Style
        {
            get
            {
                var style = new Style();
                ToStyle(style);
                return style;
            }
            set
            {
                if (value != null)
                {
                    if (posStyleID == RecordIdentifier.Empty)
                    {
                        FontName = value.FontName;
                        FontSize = value.FontSize;
                        FontBold = value.FontBold;
                        FontItalic = value.FontItalic;
                        StyleForeColor = value.ForeColor;
                        FontCharset = value.FontCharset;
                        StyleBackColor = value.BackColor;
                        StyleBackColor2 = value.BackColor2;
                        GradientMode = value.GradientMode;
                        Shape = value.Shape;
                        TextPosition = value.TextPosition;
                    }
                }
            }
        }

        public void ToStyle(Style style)
        {
            style.FontName = FontName;
            style.FontSize = FontSize;
            style.FontBold = FontBold;
            style.FontItalic = FontItalic;
            style.ForeColor = StyleForeColor;
            style.FontCharset = FontCharset;
            style.BackColor = StyleBackColor;
            style.BackColor2 = StyleBackColor2;
            style.GradientMode = GradientMode;
            style.Shape = Shape;
            style.TextPosition = TextPosition;
        }

        public bool IsModified(Style style)
        {
            if (tbFontName.Text != style.FontName) return true;
            if ((int) ntbFontSize.Value != style.FontSize) return true;
            if (chkFontBold.Checked != style.FontBold) return true;
            if (chkFontItalic.Checked != style.FontItalic) return true;
            if (cwForeColor.SelectedColor != style.ForeColor) return true;
            if ((int)ntbFontCharset.Value != style.FontCharset) return true;
            if (cwBackColor.SelectedColor != style.BackColor) return true;
            if (cwBackColor2.SelectedColor != style.BackColor2) return true;
            if (cmbGradientMode.SelectedIndex != ButtonStyleUtils.GetIndexFromGradient(style.GradientMode)) return true;
            if (cmbShape.SelectedIndex != ButtonStyleUtils.GetIndexFromShape(style.Shape)) return true;
            if (cmbTextPosition.SelectedIndex != ButtonStyleUtils.GetIndexFromPosition(style.TextPosition)) return true;

            return false;
        }
        #endregion

        #region PosMenuHeader helpers
        public PosMenuHeader PosMenuHeader
        {
            get
            {
                var posMenuHeader = new PosMenuHeader();
                ToPosMenuHeader(posMenuHeader);
                return posMenuHeader;
            }
            set
            {
                if (value != null)
                {
                    posMenuHeader = value;
                    PosStyleID = value.StyleID ?? RecordIdentifier.Empty;
                    if (posStyleID == RecordIdentifier.Empty)
                    {
                        FontName = value.FontName;
                        FontSize = value.FontSize;
                        FontBold = value.FontBold;
                        FontItalic = value.FontItalic;
                        StyleForeColor = Color.FromArgb(value.ForeColor);
                        FontCharset = value.FontCharset;
                        StyleBackColor = Color.FromArgb(value.BackColor);
                        StyleBackColor2 = Color.FromArgb(value.BackColor2);
                        GradientMode = value.GradientMode;
                        Shape = value.Shape;
                        TextPosition = value.TextPosition;
                    }
                }
            }
        }

        private void SetFromStyle()
        {
            var posStyle = (posStyleID == null || posStyleID == RecordIdentifier.Empty)
                               ? useButtonProperties
                               : Providers.PosStyleData.Get(PluginEntry.DataModel, posStyleID);            

            //if the style is cleared then the original posMenuHeader values should be used and displayed

            try
            {
                suspendEvents = true;

                FontName = posStyle.ID != RecordIdentifier.Empty ? posStyle.FontName : posMenuHeader.FontName;
                FontSize = posStyle.ID != RecordIdentifier.Empty ? posStyle.FontSize : posMenuHeader.FontSize;
                FontBold = posStyle.ID != RecordIdentifier.Empty ? posStyle.FontBold : posMenuHeader.FontBold;
                FontItalic = posStyle.ID != RecordIdentifier.Empty ? posStyle.FontItalic : posMenuHeader.FontItalic;
                StyleForeColor = posStyle.ID != RecordIdentifier.Empty ? Color.FromArgb(posStyle.ForeColor) : Color.FromArgb(posMenuHeader.ForeColor);
                FontCharset = posStyle.ID != RecordIdentifier.Empty ? posStyle.FontCharset : posMenuHeader.FontCharset;
                StyleBackColor = posStyle.ID != RecordIdentifier.Empty ? Color.FromArgb(posStyle.BackColor) : Color.FromArgb(posMenuHeader.BackColor);
                StyleBackColor2 = posStyle.ID != RecordIdentifier.Empty ? Color.FromArgb(posStyle.BackColor2) : Color.FromArgb(posMenuHeader.BackColor2);
                GradientMode = posStyle.ID != RecordIdentifier.Empty ? posStyle.GradientMode : posMenuHeader.GradientMode;
                Shape = posStyle.ID != RecordIdentifier.Empty ? posStyle.Shape : posMenuHeader.Shape;
                TextPosition = posStyle.ID != RecordIdentifier.Empty ? posStyle.TextPosition : posMenuHeader.TextPosition;
            }
            finally
            {
                suspendEvents = false;
                FireModifiedEvent();
            }
        }

        public void ToPosMenuHeader(PosMenuHeader posMenuHeader)
        {
            posMenuHeader.FontName = FontName;
            posMenuHeader.FontSize = FontSize;
            posMenuHeader.FontBold = FontBold;
            posMenuHeader.FontItalic = FontItalic;
            posMenuHeader.ForeColor = StyleForeColor.ToArgb();
            posMenuHeader.FontCharset = FontCharset;
            posMenuHeader.BackColor = StyleBackColor.ToArgb();
            posMenuHeader.BackColor2 = StyleBackColor2.ToArgb();
            posMenuHeader.GradientMode = GradientMode;
            posMenuHeader.Shape = Shape;
            posMenuHeader.TextPosition = TextPosition;
        }

        public bool IsModified(PosMenuHeader menuHeader)
        {
            if (tbFontName.Text != menuHeader.FontName) return true;
            if ((int)ntbFontSize.Value != menuHeader.FontSize) return true;
            if (chkFontBold.Checked != menuHeader.FontBold) return true;
            if (chkFontItalic.Checked != menuHeader.FontItalic) return true;
            if (cwForeColor.SelectedColor != Color.FromArgb(menuHeader.ForeColor)) return true;
            if ((int)ntbFontCharset.Value != menuHeader.FontCharset) return true;
            if (cwBackColor.SelectedColor != Color.FromArgb(menuHeader.BackColor)) return true;
            if (cwBackColor2.SelectedColor != Color.FromArgb(menuHeader.BackColor2)) return true;
            if (cmbGradientMode.SelectedIndex != ButtonStyleUtils.GetIndexFromGradient(menuHeader.GradientMode)) return true;
            if (cmbShape.SelectedIndex != ButtonStyleUtils.GetIndexFromShape(menuHeader.Shape)) return true;
            if (cmbStyle.SelectedData.ID != menuHeader.StyleID) return true;
            if (cmbTextPosition.SelectedIndex != ButtonStyleUtils.GetIndexFromPosition(menuHeader.TextPosition)) return true;

            return false;
        }
        #endregion

        public FontStyle FontStyle
        {
            get
            {
                var style = FontStyle.Regular;

                if (chkFontBold.Checked) { style = style | FontStyle.Bold; }
                if (chkFontItalic.Checked) { style = style | FontStyle.Italic; }

                return style;
            }
        }

        public Font StyleFont
        {
            get
            {
                return new Font(
                    tbFontName.Text,
                    (float) ntbFontSize.Value,
                    FontStyle,
                    GraphicsUnit.Point, Convert.ToByte(ntbFontCharset.Value));
            }
        }

        public void ToButton(MenuButton button, int borderWidth, Color borderColor)
        {            
            button.Font = new Font(
                tbFontName.Text,
                (int)ntbFontSize.Value == 0 ? 1 : (float)ntbFontSize.Value,
                FontStyle,
                button.Font.Unit,
                Convert.ToByte(ntbFontCharset.Value));
            button.ForeColor = cwForeColor.SelectedColor;

            // Attributes
            button.GradientMode = ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex);
            button.ButtonColor = cwBackColor.SelectedColor;
            button.ButtonColor2 = cwBackColor2.SelectedColor;
            button.Shape = ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex);
            button.BorderColor = borderColor;
            button.BorderWidth = borderWidth;
            button.TextAlignment = ButtonStyleUtils.GetContentAlignmentFromPosition(ButtonStyleUtils.GetPositionFromIndex(cmbTextPosition.SelectedIndex));

            button.Text = "Abc";
        }

        private void btnEditFont_Click(object sender, EventArgs e)
        {
            var style = FontStyle.Regular;
            if (!String.IsNullOrEmpty(tbFontName.Text))
            {
                if (chkFontBold.Checked) { style = style | FontStyle.Bold; }
                if (chkFontItalic.Checked) { style = style | FontStyle.Italic; }
            }

            var fontDlg = new FontDialog
            {
                ShowEffects = false,
                Font = new Font(
                    tbFontName.Text,
                    (float)ntbFontSize.Value,
                    style,
                    GraphicsUnit.Point, Convert.ToByte(ntbFontCharset.Value))
            };

            if (fontDlg.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    suspendEvents = true;

                    tbFontName.Text = fontDlg.Font.Name;
                    ntbFontSize.Value = fontDlg.Font.Size;
                    chkFontBold.Checked = fontDlg.Font.Bold;
                    chkFontItalic.Checked = fontDlg.Font.Italic;
                    ntbFontCharset.Value = Convert.ToInt16(fontDlg.Font.GdiCharSet);
                }
                finally 
                {
                    suspendEvents = false;
                }
            }

            FireModifiedEvent();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (!initializing)
                FireModifiedEvent();
        }

        private void FireModifiedEvent()
        {
            if (suspendEvents) { return;  }

            if (Modified != null)
                Modified(this, EventArgs.Empty);
        }
    }
}
