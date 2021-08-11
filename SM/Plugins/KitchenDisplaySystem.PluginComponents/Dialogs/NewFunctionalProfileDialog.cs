using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class NewFunctionalProfileDialog : DialogBase
    {
        KitchenDisplayFunctionalProfile profile;
        WeakReference addPosMenuHeader;

        public NewFunctionalProfileDialog()
        {
            InitializeComponent();
            cmbButtons.SelectedData = new DataEntity("","");

            IPlugin plugin = PluginEntry.Framework.FindImplementor(this, "CanEditLayouts", null);
            addPosMenuHeader = plugin != null ? new WeakReference(plugin) : null;
            btnAdd.Visible = (addPosMenuHeader != null);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier Id
        {
            get { return profile.ID; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = (tbKitchenProfileName.Text.Length > 0) && cmbButtons.SelectedData.ID != ""; 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            profile = new KitchenDisplayFunctionalProfile();

            profile.Text = tbKitchenProfileName.Text;
            profile.ButtonsMenuId = (string)cmbButtons.SelectedData.ID;

            Providers.KitchenDisplayFunctionalProfileData.Save(PluginEntry.DataModel, profile);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenDisplayFunctionalProfile", profile.ID, profile);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbButtons_RequestData(object sender, EventArgs e)
        {
            var list = Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.KitchenDisplay);                                                 
            
            cmbButtons.SetData(list, null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var dlg = new ButtonProfileDialog();
            dlg.ShowDialog();

        }

    }
}
