using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{
    public partial class NewTranslationDialog : DialogBase
    {
        private ItemTranslation itemTranslation;
        private RecordIdentifier itemID;

        public NewTranslationDialog(RecordIdentifier itemTranslationID, RecordIdentifier itemID)
            : this(itemID)
        {
            cmbLanguage.Enabled = false;
            itemTranslation = Providers.ItemTranslationData.Get(PluginEntry.DataModel, itemTranslationID);
            tbDescription.Text = itemTranslation.Description;
            cmbLanguage.Text = (string)itemTranslation.LanguageID;
            btnOK.Enabled = false;

        }

        public NewTranslationDialog(RecordIdentifier itemID)
        {
            InitializeComponent();
            cmbLanguage.SelectedIndex = 0;
            this.itemID = itemID;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (itemTranslation == null)
            {
                itemTranslation = new ItemTranslation();
            }

            itemTranslation.Description = tbDescription.Text;
            itemTranslation.LanguageID = cmbLanguage.Text;
            itemTranslation.ItemID = itemID;

            Providers.ItemTranslationData.Save(PluginEntry.DataModel, itemTranslation);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckChanged(object sender, EventArgs e)
        {
            //errorProvider1.Clear();

            bool descriptionNotEmpty = (tbDescription.Text != "");
            btnOK.Enabled = descriptionNotEmpty;
        }

        public RecordIdentifier GetSelectedId()
        {
            return itemTranslation.ID;
        }

    }
}
