using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.BusinessObjects.Tax;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.DataLayer.DataProviders.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders.Tax;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{ 
    public partial class NewRetailGroupDialog : DialogBase
    {
        public RetailGroup retailGroup;
        RecordIdentifier retailGroupId;
        WeakReference salesTaxItemGroupEditor;
        WeakReference dimensionGroupCreator;

        public NewRetailGroupDialog()
            : base()
        {
            IPlugin plugin;
            
            InitializeComponent();

            cmbRetailDepartment.SelectedData = new DataEntity("", "");
            cmbItemSalesTaxGroup.SelectedData = new DataEntity("", "");

            //TODO implement this broadcast connection once retail department editor is available.
            //plugin = PluginEntry.Framework.FindImplementor(this, "CanEditRetailDepartments", null);
            //retailDepartmentCreator = (plugin != null) ? new WeakReference(plugin) : null;
            //btnAddRetailDepartment.Visible = (retailDepartmentCreator != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewItemSalesTaxGroups", null);
            salesTaxItemGroupEditor = plugin != null ? new WeakReference(plugin) : null;
            btnAddItemSalesTaxGroup.Visible = (salesTaxItemGroupEditor != null);

            plugin = PluginEntry.Framework.FindImplementor(this, "CanEditDimensionGroups", null);
            dimensionGroupCreator = plugin != null ? new WeakReference(plugin) : null;
                
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier RetailGroupId
        {
            get { return retailGroupId; }
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            retailGroup = new RetailGroup();
            retailGroup.Text = tbDescription.Text;
            if (cmbRetailDepartment.SelectedData.ID != "")
            {
                retailGroup.RetailDepartmentMasterID = cmbRetailDepartment.SelectedData.ID;
                retailGroup.RetailDepartmentID = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, cmbRetailDepartment.SelectedData.ID).ID;
            }
            retailGroup.ItemSalesTaxGroupId = (string)cmbItemSalesTaxGroup.SelectedData.ID;
            
            Providers.RetailGroupData.Save(PluginEntry.DataModel, retailGroup);

            retailGroupId = retailGroup.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormatData(object sender, DropDownFormatDataArgs e)
        {

        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            btnOK.Enabled = tbDescription.Text.Length > 0;
        }

        private void ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbRetailDepartment_RequestData(object sender, EventArgs e)
        {
            cmbRetailDepartment.SetData(Providers.RetailDepartmentData.GetMasterIDList(PluginEntry.DataModel),null);
        }

        private void cmbSalesOrder_RequestData(object sender, EventArgs e)
        {
            cmbItemSalesTaxGroup.SetData(Providers.ItemSalesTaxGroupData.GetList(PluginEntry.DataModel),null);
        }
       
        private void btnAddRetailDepartment_Click(object sender, EventArgs e)
        {
            RetailDepartmentDialog dlg = new RetailDepartmentDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                cmbRetailDepartment.SelectedData = (MasterIDEntity)dlg.SelectedEntity;
            }
        }

        private void btnAddSalesOrder_Click(object sender, EventArgs e)
        {
            if (salesTaxItemGroupEditor.IsAlive)
            {
                ItemSalesTaxGroup itemSalesTaxGroup = (ItemSalesTaxGroup)((IPlugin)salesTaxItemGroupEditor.Target).Message(this, "NewSalesTaxItemGroup", null);
                if (itemSalesTaxGroup != null)
                {
                    cmbItemSalesTaxGroup.SelectedData = itemSalesTaxGroup;
                }
            }
        }

        
    }
}
