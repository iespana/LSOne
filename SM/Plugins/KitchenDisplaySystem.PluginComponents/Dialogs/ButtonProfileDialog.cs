using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class ButtonProfileDialog : DialogBase
    {
        private PosMenuHeader buttonGridMenu;

        public ButtonProfileDialog(RecordIdentifier buttonGridMenuId) : this()
        {
            buttonGridMenu = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, buttonGridMenuId);
            tbDescription.Text = buttonGridMenu.Text;
        }

        public ButtonProfileDialog()
        {
            InitializeComponent();
        }

        public RecordIdentifier Id
        {
            get { return buttonGridMenu.ID; }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text.Trim().Length > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (buttonGridMenu == null)
            {
                buttonGridMenu = new PosMenuHeader();
            }

            buttonGridMenu.Text = tbDescription.Text;
            buttonGridMenu.Rows = 1;
            buttonGridMenu.MenuType = MenuTypeEnum.KitchenDisplay;

            Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, buttonGridMenu);

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