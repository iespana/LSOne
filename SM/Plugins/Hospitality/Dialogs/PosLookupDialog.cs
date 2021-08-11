using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class PosLookupDialog : DialogBase
    {
        RecordIdentifier posLookupID;
        PosLookup posLookup;
        bool editingExisting;
        bool suspendEvents = false;
        //bool initialFocus;

        /// <summary>
        /// Opens the dialog to create a new restaurant menu type
        /// </summary>
        public PosLookupDialog()
        {
            InitializeComponent();

            //initialFocus = false;
            posLookupID = RecordIdentifier.Empty;
            editingExisting = false;

        }

        /// <summary>
        /// Opens the dialog for editing an existing restaurant menu type
        /// </summary>
        /// <param name="restaurantID">The id of the restaurant</param>
        /// <param name="menuOrder">The id of the menu order</param>
        public PosLookupDialog(RecordIdentifier posLookupID)
        {
            InitializeComponent();

            this.posLookupID = posLookupID;

            posLookup = Providers.PosLookupData.Get(PluginEntry.DataModel, posLookupID);

            editingExisting = true;
            suspendEvents = true;

            tbDescription.Text = posLookup.Text;
            cmbDynamicMenuID.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posLookup.DynamicMenuID) ?? new DataEntity("", "");
            cmbDynamicMenu2ID.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posLookup.DynamicMenu2ID) ?? new DataEntity("", "");
            cmbGrid1MenuID.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posLookup.Grid1MenuID) ?? new DataEntity("", "");
            cmbGrid2MenuID.SelectedData = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, posLookup.Grid2MenuID) ?? new DataEntity("", "");

            suspendEvents = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            btnAddDynamicMenu.Visible =
                btnAddDynamicMenu2.Visible =
                btnAddGrid1Menu.Visible =
                btnAddGrid2Menu.Visible = PluginEntry.DataModel.HasPermission(Permission.EditPosMenus);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier PosLookupID
        {
            get { return posLookupID; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suspendEvents)
                return;

            errorProvider1.Clear();

            if (editingExisting)
            {
                btnOK.Enabled = tbDescription.Text.Length > 0 && IsModified();
            }
            else
            {
                btnOK.Enabled = tbDescription.Text.Length > 0;
            }
        }


        private bool IsModified()
        {
            if (tbDescription.Text != posLookup.Text) return true;
            if (cmbDynamicMenuID.SelectedData != null && cmbDynamicMenuID.SelectedData.ID != posLookup.DynamicMenuID) return true;
            if (cmbDynamicMenu2ID.SelectedData != null && cmbDynamicMenu2ID.SelectedData.ID != posLookup.DynamicMenu2ID) return true;
            if (cmbGrid1MenuID.SelectedData != null && cmbGrid1MenuID.SelectedData.ID != posLookup.Grid1MenuID) return true;
            if (cmbGrid2MenuID.SelectedData != null && cmbGrid2MenuID.SelectedData.ID != posLookup.Grid2MenuID) return true;

            return false;
        }
        

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (editingExisting)
            {
                posLookup.Text = tbDescription.Text;
                posLookup.DynamicMenuID = cmbDynamicMenuID.SelectedData != null ? cmbDynamicMenuID.SelectedData.ID : "";
                posLookup.DynamicMenu2ID = cmbDynamicMenu2ID.SelectedData != null ? cmbDynamicMenu2ID.SelectedData.ID : "";
                posLookup.Grid1MenuID = cmbGrid1MenuID.SelectedData != null ? cmbGrid1MenuID.SelectedData.ID : "";
                posLookup.Grid2MenuID = cmbGrid2MenuID.SelectedData != null ? cmbGrid2MenuID.SelectedData.ID : "";

                Providers.PosLookupData.Save(PluginEntry.DataModel, posLookup);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "PosLookup", posLookup.ID, null);

                posLookupID = posLookup.ID;
            }
            else
            {
                posLookup = new PosLookup();

                posLookup.Text = tbDescription.Text;
                posLookup.DynamicMenuID = cmbDynamicMenuID.SelectedData != null ? cmbDynamicMenuID.SelectedData.ID : "";
                posLookup.DynamicMenu2ID = cmbDynamicMenu2ID.SelectedData != null ? cmbDynamicMenu2ID.SelectedData.ID : "";
                posLookup.Grid1MenuID = cmbGrid1MenuID.SelectedData != null ? cmbGrid1MenuID.SelectedData.ID : "";
                posLookup.Grid2MenuID = cmbGrid2MenuID.SelectedData != null ? cmbGrid2MenuID.SelectedData.ID : "";

                Providers.PosLookupData.Save(PluginEntry.DataModel, posLookup);
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "PosLookup", posLookup.ID, null);

                posLookupID = posLookup.ID;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void addMenuHandler(object sender, EventArgs args)
        {
            PluginOperations.NewPosMenu();
        }

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {

        }

        private void cmbDynamicMenuID_RequestData(object sender, EventArgs e)
        {
            cmbDynamicMenuID.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.Hospitality), null);
        }

        private void cmbDynamicMenu2ID_RequestData(object sender, EventArgs e)
        {
            cmbDynamicMenu2ID.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.Hospitality), null);
        }

        private void cmbGrid1MenuID_RequestData(object sender, EventArgs e)
        {
            cmbGrid1MenuID.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.Hospitality), null);
        }

        private void cmbGrid2MenuID_RequestData(object sender, EventArgs e)
        {
            cmbGrid2MenuID.SetData(Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, MenuTypeEnum.Hospitality), null);
        }

        
    }
}
