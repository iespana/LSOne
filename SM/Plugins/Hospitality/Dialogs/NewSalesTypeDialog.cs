using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class NewSalesTypeDialog : DialogBase
    {
        DataEntity emptySalesType;
        RecordIdentifier salesTypeID;
        //bool initialFocus;

        public NewSalesTypeDialog()
        {
            InitializeComponent();

            //initialFocus = false;
            salesTypeID = RecordIdentifier.Empty;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            emptySalesType = new DataEntity(RecordIdentifier.Empty, "");

        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier SalesTypeID
        {
            get { return salesTypeID; }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = (tbDescription.Text.Length > 0);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SalesType salesType;

            salesType = new SalesType();
            salesType.Text = tbDescription.Text;

            Providers.SalesTypeData.Save(PluginEntry.DataModel, salesType);

            salesTypeID = salesType.ID;

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
