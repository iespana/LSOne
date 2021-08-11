using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.RetailItems.Properties;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{ 
    public partial class RetailDivisionDialog : DialogBase
    {
        public RetailDivision retailDivision;
        private RecordIdentifier retailDivisionID;

        public RetailDivisionDialog()
        {
            InitializeComponent();
        }

        public RetailDivisionDialog(RecordIdentifier retailDivisionID)
            : this()
        {
            this.retailDivisionID = retailDivisionID;
            retailDivision = Providers.RetailDivisionData.Get(PluginEntry.DataModel, retailDivisionID);
            tbDescription.Text = retailDivision.Text;

            Text = Resources.EditRetailDivision;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier RetailDivisionId
        {
            get { return retailDivisionID; }
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            if (retailDivision == null)
            {
                retailDivision = new RetailDivision { Text = tbDescription.Text };
            }
            else
            {
                retailDivision.Text = tbDescription.Text;
            }

            Providers.RetailDivisionData.Save(PluginEntry.DataModel, retailDivision);

            retailDivisionID = retailDivision.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = tbDescription.Text.Length > 0 && (retailDivision == null || retailDivision.Text != tbDescription.Text);
        }
    }
}
