using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Controls
{
    public partial class StyleControl : UserControl
    {
        public event EventHandler ValueChanged;

        private PosStyle style;
        public PosStyle Style
        {
            set
            {
                PopulateGui(value);
                style = value;
            }
            get
            {
                StyleFromGui();
                return style;
            }

        }
        private bool suspendEvents;

        public StyleControl(PosStyle style)
            :this()
        {
            PopulateGui(style);
        }

        public StyleControl()
        {
            InitializeComponent();

            DoubleBuffered = true;

            cmbShape.Items.Clear();
            cmbShape.Items.AddRange(ButtonStyleUtils.GetShapeTexts());

            cmbGradientMode.Items.Clear();
            cmbGradientMode.Items.AddRange(ButtonStyleUtils.GetPartialGradientTexts());
        }

        private void PopulateGui(PosStyle style)
        {
            suspendEvents = true;
            tbFontName.Text = style.FontName;
            ntbFontSize.Value = style.FontSize;
            chkFontBold.Checked = style.FontBold;
            chkFontItalic.Checked = style.FontItalic;
            chkStrikethrough.Checked = style.FontStrikethrough;
            cwForeColor.SelectedColor = Color.FromArgb(style.ForeColor);
            ntbFontCharset.Value = style.FontCharset;
            cwBackColor.SelectedColor = Color.FromArgb(style.BackColor);
            cwBackColor2.SelectedColor = Color.FromArgb(style.BackColor2);
            cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(style.GradientMode);
            cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(style.Shape);
            suspendEvents = false;
            UpdatePreview(null, EventArgs.Empty);
        }

        private void StyleFromGui()
        {
            style.FontName = tbFontName.Text;
            style.FontSize = (int)ntbFontSize.Value;
            style.FontBold = chkFontBold.Checked;
            style.FontItalic = chkFontItalic.Checked;
            style.FontStrikethrough = chkStrikethrough.Checked;
            style.ForeColor = cwForeColor.SelectedColor.ToArgb();
            style.FontCharset = (int)ntbFontCharset.Value;
            style.BackColor = cwBackColor.SelectedColor.ToArgb();
            style.BackColor2 = cwBackColor2.SelectedColor.ToArgb();
            style.GradientMode = ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex);
            style.Shape = ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex);
        }

        public void UpdatePreview(object sender, EventArgs e)
        {
            if (suspendEvents) return;

            btnMenuButtonPreview.Font = new Font(
                tbFontName.Text,
                (int)ntbFontSize.Value == 0 ? 1 : (float)ntbFontSize.Value,
                FontStyle,
                btnMenuButtonPreview.Font.Unit,
                Convert.ToByte(ntbFontCharset.Value));
            btnMenuButtonPreview.ForeColor = cwForeColor.SelectedColor;

            // Attributes
            btnMenuButtonPreview.GradientMode = ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex);
            btnMenuButtonPreview.ButtonColor = cwBackColor.SelectedColor;
            btnMenuButtonPreview.ButtonColor2 = cwBackColor2.SelectedColor;
            btnMenuButtonPreview.Shape = ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex);

            btnMenuButtonPreview.Text = "Abc";

            OnValueChanged();
        }

        public FontStyle FontStyle
        {
            get
            {
                var fontStyle = FontStyle.Regular;

                if (chkFontBold.Checked) { fontStyle = fontStyle | FontStyle.Bold; }
                if (chkFontItalic.Checked) { fontStyle = fontStyle | FontStyle.Italic; }
                if (chkStrikethrough.Checked) { fontStyle = fontStyle | FontStyle.Strikeout; }

                return fontStyle;
            }
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

            UpdatePreview(null, EventArgs.Empty);
        }

        public bool Changed {
            get
            {
                if (style == null) return false;
                if (IsNew) return true;

                if(style.FontName != tbFontName.Text) return true;
                if(style.FontSize != (int)ntbFontSize.Value) return true;
                if(style.FontBold != chkFontBold.Checked) return true;
                if(style.FontItalic != chkFontItalic.Checked) return true;
                if (style.FontStrikethrough != chkStrikethrough.Checked) return true;
                if (style.ForeColor != cwBackColor.SelectedColor.ToArgb()) return true;
                if(style.FontCharset != (int)ntbFontCharset.Value) return true;
                if(style.BackColor != cwBackColor.SelectedColor.ToArgb()) return true;
                if(style.BackColor2 != cwBackColor2.SelectedColor.ToArgb()) return true;
                if(style.GradientMode != ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex)) return true;
                if(style.Shape != ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex)) return true;
                return false;
            }
        }

        public bool IsNew { get; set; }

        private void OnValueChanged()
        {
            if (ValueChanged != null)
            {
                ValueChanged(this, EventArgs.Empty);
            }
        }
    }
}
