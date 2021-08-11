using System;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class EditPosMenuHeaderDialog : DialogBase
    {
        private RecordIdentifier posMenuHeaderID;
        private PosMenuHeader posMenuHeader;
        private bool suspendEvents;

        public EditPosMenuHeaderDialog(RecordIdentifier posMenuHeaderID)
        {            
            InitializeComponent();

            suspendEvents = false;
            this.posMenuHeaderID = posMenuHeaderID;
            pnlBottom.ReadOnly =
                !PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);

            cmbShape.Items.Clear();
            cmbShape.Items.AddRange(ButtonStyleUtils.GetShapeTexts());

            cmbGradientMode.Items.Clear();
            cmbGradientMode.Items.AddRange(ButtonStyleUtils.GetPartialGradientTexts());
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadData();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier PosMenuHeaderID
        {
            get { return posMenuHeaderID; }
        }

        // Returns true if the user has made modifications
        private bool IsModified()
        {
            // General
            if (tbDescription.Text != posMenuHeader.Text) return true;
            if (cmbAppliesTo.SelectedIndex != (int)posMenuHeader.AppliesTo) return true;
            if (chkUseNavOperation.Checked != posMenuHeader.UseNavOperation) return true;

            // Attributes
            if ((int)ntbColumns.Value != posMenuHeader.Columns) return true;
            if ((int)ntbRows.Value != posMenuHeader.Rows) return true;
            //if (cwMenuColor.SelectedColor.ToArgb() != posMenuHeader.MenuColor) return true;

            // Button attributes
            if (tbFontName.Text != posMenuHeader.FontName) return true;
            if ((int)ntbFontSize.Value != posMenuHeader.FontSize) return true;
            if (chkFontBold.Checked != posMenuHeader.FontBold) return true;
            if (chkFontItalic.Checked != posMenuHeader.FontItalic) return true;
            if (cwForeColor.SelectedColor != Color.FromArgb(posMenuHeader.ForeColor)) return true;
            if (ntbFontCharset.Value != posMenuHeader.FontCharset) return true;
            if (cwBackColor.SelectedColor != Color.FromArgb(posMenuHeader.BackColor)) return true;
            if (cwBackColor2.SelectedColor != Color.FromArgb(posMenuHeader.BackColor2)) return true;
            if (cmbGradientMode.SelectedIndex != ButtonStyleUtils.GetIndexFromGradient(posMenuHeader.GradientMode)) return true;
            if (cmbShape.SelectedIndex != ButtonStyleUtils.GetIndexFromShape(posMenuHeader.Shape)) return true;

            return false;
        }

        // Saves the data entity into the database
        private void Save()
        {
            // General
            posMenuHeader.Text = tbDescription.Text;
            posMenuHeader.AppliesTo = (PosMenuHeader.AppliesToEnum)cmbAppliesTo.SelectedIndex;
            posMenuHeader.UseNavOperation = chkUseNavOperation.Checked;

            // Attributes
            posMenuHeader.Columns = (int)ntbColumns.Value;
            posMenuHeader.Rows = (int)ntbRows.Value;
            //posMenuHeader.MenuColor = cwMenuColor.SelectedColor.ToArgb();

            // Button attributes
            posMenuHeader.FontName = tbFontName.Text;
            posMenuHeader.FontSize = (int)ntbFontSize.Value;
            posMenuHeader.FontBold = chkFontBold.Checked;
            posMenuHeader.FontItalic = chkFontItalic.Checked;
            posMenuHeader.ForeColor = cwForeColor.SelectedColor.ToArgb();
            posMenuHeader.FontCharset = (int)ntbFontCharset.Value;
            posMenuHeader.BackColor = cwBackColor.SelectedColor.ToArgb();
            posMenuHeader.BackColor2 = cwBackColor2.SelectedColor.ToArgb();
            posMenuHeader.GradientMode = ButtonStyleUtils.GetGradientFromIndex(cmbGradientMode.SelectedIndex);
            posMenuHeader.Shape = ButtonStyleUtils.GetShapeFromIndex(cmbShape.SelectedIndex);

            Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, posMenuHeader);
        }

        // Loads data from the data entity to controls
        private void LoadData()
        {
            posMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuHeaderID);

            suspendEvents = true;

            // General
            tbDescription.Text = posMenuHeader.Text;
            cmbAppliesTo.SelectedIndex = (int)posMenuHeader.AppliesTo;
            chkUseNavOperation.Checked = posMenuHeader.UseNavOperation;

            // Attributes
            ntbColumns.Value = posMenuHeader.Columns;
            ntbRows.Value = posMenuHeader.Rows;
            //cwMenuColor.SelectedColor = Color.FromArgb(posMenuHeader.MenuColor);

            // Button attributes
            tbFontName.Text = posMenuHeader.FontName;
            ntbFontSize.Value = posMenuHeader.FontSize;
            chkFontBold.Checked = posMenuHeader.FontBold;
            chkFontItalic.Checked = posMenuHeader.FontItalic;
            cwForeColor.SelectedColor = Color.FromArgb(posMenuHeader.ForeColor);
            ntbFontCharset.Value = posMenuHeader.FontCharset;
            cwBackColor.SelectedColor = Color.FromArgb(posMenuHeader.BackColor);
            cwBackColor2.SelectedColor = Color.FromArgb(posMenuHeader.BackColor2);
            cmbGradientMode.SelectedIndex = ButtonStyleUtils.GetIndexFromGradient(posMenuHeader.GradientMode);
            cmbShape.SelectedIndex = ButtonStyleUtils.GetIndexFromShape(posMenuHeader.Shape);

            suspendEvents = false;

        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suspendEvents)
                return;

            btnOK.Enabled = IsModified() && !pnlBottom.ReadOnly;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();
            posMenuHeaderID = posMenuHeader.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnEditFont_Click(object sender, EventArgs e)
        {

            if (!String.IsNullOrEmpty(tbFontName.Text))
            {
                FontStyle style = FontStyle.Regular;

                if (chkFontBold.Checked) { style = style | FontStyle.Bold; }
                if (chkFontItalic.Checked) { style = style | FontStyle.Italic; }

                fontDialog1.ShowEffects = false;
     
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
                ntbFontCharset.Value = Convert.ToInt16(fontDialog1.Font.GdiCharSet);
            }

            
        }
    }
}
