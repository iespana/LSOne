//style class here

using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.UserInterfaceStyles.Dialogs
{
    public partial class FontStyleDialog : DialogBase
    {
        private FontStyle fontStyle;
        //bool lockEvents;

        public FontStyleDialog()
        {
            fontStyle = FontStyle.Regular;

            InitializeComponent();
        }

        public FontStyleDialog(FontStyle fontStyle)
            :this()
        {
            this.fontStyle = fontStyle;

            //lockEvents = true;

            if ((fontStyle & FontStyle.Bold) == FontStyle.Bold)
            {
                chkBold.Checked = true;
            }

            if ((fontStyle & FontStyle.Italic) == FontStyle.Italic)
            {
                chkItalic.Checked = true;
            }

            if ((fontStyle & FontStyle.Underline) == FontStyle.Underline)
            {
                chkUnderline.Checked = true;
            }

            if ((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout)
            {
                chkStrikeout.Checked = true;
            }

            // lockEvents = false;            
        }
           

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            fontStyle = FontStyle.Regular;

            if (chkBold.Checked)
            {
                fontStyle = fontStyle | FontStyle.Bold;
            }

            if (chkItalic.Checked)
            {
                fontStyle = fontStyle | FontStyle.Italic;
            }

            if (chkUnderline.Checked)
            {
                fontStyle = fontStyle | FontStyle.Underline;
            }

            if (chkStrikeout.Checked)
            {
                fontStyle = fontStyle | FontStyle.Strikeout;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            btnOK.Enabled =
                (((fontStyle & FontStyle.Bold) == FontStyle.Bold) != chkBold.Checked) ||
                (((fontStyle & FontStyle.Italic) == FontStyle.Italic) != chkItalic.Checked) ||
                (((fontStyle & FontStyle.Underline) == FontStyle.Underline) != chkUnderline.Checked) ||
                (((fontStyle & FontStyle.Strikeout) == FontStyle.Strikeout) != chkStrikeout.Checked);

        }

        private void AttributeChanged(object sender, EventArgs e)
        {
            CheckEnabled(this, EventArgs.Empty);
        }

        public FontStyle FontStyle
        {
            get
            {
                return fontStyle;
            }
        }
    }
}

