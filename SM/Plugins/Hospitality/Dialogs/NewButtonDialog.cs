using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class NewButtonDialog : DialogBase
    {
        RecordIdentifier posMenuHeaderID;
        RecordIdentifier posMenuLineID;

        public NewButtonDialog(RecordIdentifier posMenuHeaderID)
        {
            InitializeComponent();

            this.posMenuHeaderID = posMenuHeaderID;
            posMenuLineID = RecordIdentifier.Empty;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);           
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier PosMenuLineID
        {
            get { return posMenuLineID; }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text.Length > 0;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = tbDescription.Text.Length > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PosMenuLine posMenuLine = new PosMenuLine();
            PosMenuHeader posMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posMenuHeaderID);

            posMenuLine.Sequence = RecordIdentifier.Empty;
            posMenuLine.MenuID = posMenuHeaderID;
            posMenuLine.Text = tbDescription.Text;

            posMenuLine.FontName = posMenuHeader.FontName;
            posMenuLine.FontSize = posMenuHeader.FontSize;
            posMenuLine.FontBold = posMenuHeader.FontBold;
            posMenuLine.ForeColor = posMenuHeader.ForeColor;
            posMenuLine.BackColor = posMenuHeader.BackColor;
            posMenuLine.FontItalic = posMenuHeader.FontItalic;
            posMenuLine.FontCharset = posMenuHeader.FontCharset;
            posMenuLine.BackColor2 = posMenuHeader.BackColor2;
            posMenuLine.GradientMode = posMenuHeader.GradientMode;
            posMenuLine.Shape = posMenuHeader.Shape;
            posMenuLine.UseHeaderAttributes = true;
            posMenuLine.UseHeaderFont = true;

            Providers.PosMenuLineData.Save(PluginEntry.DataModel, posMenuLine);

            posMenuLineID = posMenuLine.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
