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
    public partial class NewTouchButtonDialog : DialogBase
    {
        RecordIdentifier layoutID;
        DataEntity emptyItem;
        PosMenuHeader noSelection;

        public NewTouchButtonDialog()
        {
            layoutID = RecordIdentifier.Empty;

            InitializeComponent();

            btnAdd1.Enabled =
                btnAdd2.Enabled =
                btnAdd3.Enabled =
                btnAdd4.Enabled =
                btnAdd5.Enabled = PluginEntry.DataModel.HasPermission(Permission.ManageTouchButtonLayout);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            emptyItem = new DataEntity(RecordIdentifier.Empty, Properties.Resources.DoNotCopy);
            noSelection = new PosMenuHeader();
            noSelection.ID = RecordIdentifier.Empty;
            noSelection.Text = "";

            cmbCopyFrom.SelectedData = emptyItem;
            cmbButtonGrid1Menu.SelectedData = noSelection;
            cmbButtonGrid2Menu.SelectedData = noSelection;
            cmbButtonGrid3Menu.SelectedData = noSelection;
            cmbButtonGrid4Menu.SelectedData = noSelection;
            cmbButtonGrid5Menu.SelectedData = noSelection;
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
            TouchLayout layout;

            layout = new TouchLayout();

            layout.ID = RecordIdentifier.Empty;
            layout.Text = tbDescription.Text;

            if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
            {
                layoutID = Providers.TouchLayoutData.CreateNewAndCopyFrom(PluginEntry.DataModel, layout, cmbCopyFrom.SelectedData.ID);
            }
            else
            {
                layout.ButtonGrid1 = cmbButtonGrid1Menu.SelectedData.ID;
                layout.ButtonGrid2 = cmbButtonGrid2Menu.SelectedData.ID;
                layout.ButtonGrid3 = cmbButtonGrid3Menu.SelectedData.ID;
                layout.ButtonGrid4 = cmbButtonGrid4Menu.SelectedData.ID;
                layout.ButtonGrid5 = cmbButtonGrid5Menu.SelectedData.ID;

                Providers.TouchLayoutData.Save(PluginEntry.DataModel, layout);

                layoutID = layout.ID;
            }            

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
            btnOK.Enabled = tbDescription.Text.Length > 0;
        }

        private void cmbCopyFrom_FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            List<DataEntity> items = Providers.TouchLayoutData.GetList(PluginEntry.DataModel, "NAME");

            items.Insert(0, emptyItem);

            cmbCopyFrom.SetData(items,null);
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

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            cmbButtonGrid1Menu.Enabled =
                cmbButtonGrid2Menu.Enabled =
                cmbButtonGrid3Menu.Enabled =
                cmbButtonGrid4Menu.Enabled =
                cmbButtonGrid5Menu.Enabled = cmbCopyFrom.SelectedData.ID == RecordIdentifier.Empty;
            
            btnAdd1.Enabled =
                btnAdd2.Enabled =
                btnAdd3.Enabled =
                btnAdd4.Enabled =
                btnAdd5.Enabled = cmbCopyFrom.SelectedData.ID == RecordIdentifier.Empty;
        }
    }
}
