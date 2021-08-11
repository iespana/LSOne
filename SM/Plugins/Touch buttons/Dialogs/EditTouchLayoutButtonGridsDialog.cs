using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    public partial class EditTouchLayoutButtonGridsDialog : DialogBase
    {
        RecordIdentifier layoutID;        
        PosMenuHeader noSelection;
        TouchLayout touchLayout;

        public EditTouchLayoutButtonGridsDialog(RecordIdentifier touchLayoutID)
        {
            layoutID = touchLayoutID;

            InitializeComponent();
        
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            touchLayout = Providers.TouchLayoutData.Get(PluginEntry.DataModel, layoutID);

            tbID.Text = (string)touchLayout.ID;
            tbDescription.Text = touchLayout.Text;

            noSelection = new PosMenuHeader();
            noSelection.ID = RecordIdentifier.Empty;
            noSelection.Text = "";

            var menuProvider = Providers.PosMenuHeaderData;
            cmbButtonGrid1Menu.SelectedData = menuProvider.Get(PluginEntry.DataModel, touchLayout.ButtonGrid1) ?? noSelection;
            cmbButtonGrid2Menu.SelectedData = menuProvider.Get(PluginEntry.DataModel, touchLayout.ButtonGrid2) ?? noSelection;
            cmbButtonGrid3Menu.SelectedData = menuProvider.Get(PluginEntry.DataModel, touchLayout.ButtonGrid3) ?? noSelection;
            cmbButtonGrid4Menu.SelectedData = menuProvider.Get(PluginEntry.DataModel, touchLayout.ButtonGrid4) ?? noSelection;
            cmbButtonGrid5Menu.SelectedData = menuProvider.Get(PluginEntry.DataModel, touchLayout.ButtonGrid5) ?? noSelection; 
        }

        public RecordIdentifier LayoutID
        {
            get { return layoutID; }
        }


        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            touchLayout.ButtonGrid1 = cmbButtonGrid1Menu.SelectedData.ID;
            touchLayout.ButtonGrid2 = cmbButtonGrid2Menu.SelectedData.ID;
            touchLayout.ButtonGrid3 = cmbButtonGrid3Menu.SelectedData.ID;
            touchLayout.ButtonGrid4 = cmbButtonGrid4Menu.SelectedData.ID;
            touchLayout.ButtonGrid5 = cmbButtonGrid5Menu.SelectedData.ID;

            Providers.TouchLayoutData.Save(PluginEntry.DataModel, touchLayout);

            layoutID = touchLayout.ID;

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

            btnOK.Enabled =
                cmbButtonGrid1Menu.SelectedData.ID != touchLayout.ButtonGrid1 ||
                cmbButtonGrid2Menu.SelectedData.ID != touchLayout.ButtonGrid2 ||
                cmbButtonGrid3Menu.SelectedData.ID != touchLayout.ButtonGrid3 ||
                cmbButtonGrid4Menu.SelectedData.ID != touchLayout.ButtonGrid4 ||
                cmbButtonGrid5Menu.SelectedData.ID != touchLayout.ButtonGrid5;
        }

        private void cmbButtonGridMenu_RequestData(object sender, EventArgs e)
        {

            PosMenuHeaderFilter filter = new PosMenuHeaderFilter();
            filter.MenuType = (int)MenuTypeEnum.POSButtonGrid;
            filter.SortBy = PosMenuHeaderSorting.MenuDescription;
            List<PosMenuHeader> posMenus = Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, filter);

            posMenus.Insert(0, noSelection);

            ((DualDataComboBox)sender).SetData(posMenus, null);
        }

        private void btnAddButtonGridMenu_Click(object sender, EventArgs e)
        {
            PluginOperations.NewPosButtonGridMenu();
        }

        private void cmbButtonGridMenu_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = noSelection;
        }
    }
}
