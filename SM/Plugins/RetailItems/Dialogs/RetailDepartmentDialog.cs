using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.ItemMaster;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItems.Dialogs
{ 
    public partial class RetailDepartmentDialog : DialogBase
    {
        RetailDepartment retailDepartment;        
        public RecordIdentifier RetailDepartmentID { get; private set; }

        private bool searchAlias;

        public RetailDepartmentDialog()
        {
            InitializeComponent();            
        }

        public RetailDepartmentDialog(RecordIdentifier retailDepartmentID, bool searchAlias)
            : this()
        {
            RetailDepartmentID = retailDepartmentID;
            retailDepartment = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, retailDepartmentID);
            tbDescription.Text = retailDepartment.Text;

            cmbRetailDivision.SelectedData = new DataEntity(retailDepartment.RetailDivisionID, retailDepartment.RetailDivisionName);
            cmbRetailDivision.Text = retailDepartment.RetailDivisionName;

            Text = Properties.Resources.EditRetailDepartment;

            this.searchAlias = searchAlias;
            if (searchAlias)
            {
                cmbRetailDivision.Visible = lblRetailDivision.Visible = btnAddRetailDivision.Visible = false;
                tbSearchAlias.Visible = lblSearchName.Visible = true;
                tbSearchAlias.Text = retailDepartment.NameAlias;
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public MasterIDEntity SelectedEntity
        {
            get { return new MasterIDEntity() { Text = retailDepartment.Text, ID = retailDepartment.MasterID, ReadadbleID = retailDepartment.ID }; }
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            if (retailDepartment == null)
            {
                retailDepartment = new RetailDepartment();
            }
            
            retailDepartment.Text = tbDescription.Text;
            if (cmbRetailDivision.SelectedData != null)
            {
                retailDepartment.RetailDivisionID = cmbRetailDivision.SelectedData.ID;
            }

            if (searchAlias)
            {
                retailDepartment.NameAlias = tbSearchAlias.Text;
            }
            var division = Providers.RetailDivisionData.Get(PluginEntry.DataModel, retailDepartment.RetailDivisionID);
            if (division != null)
            {
                retailDepartment.RetailDivisionMasterID = division.MasterID;
            }

            Providers.RetailDepartmentData.Save(PluginEntry.DataModel, retailDepartment);

            RetailDepartmentID = retailDepartment.ID;            
            
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

            btnOK.Enabled = tbDescription.Text.Length > 0 && ((retailDepartment == null || tbDescription.Text != retailDepartment.Text) ||
                                                              (cmbRetailDivision.SelectedData != null && cmbRetailDivision.SelectedData.ID != retailDepartment.RetailDivisionID));
        }

        private void btnAddRetailDivision_Click(object sender, EventArgs e)
        {
            var newDivision = PluginOperations.NewRetailDivision(sender, e);
            if (newDivision != null)
            {
                cmbRetailDivision.SelectedData = newDivision;
                CheckEnabled(sender,e);
            }
        }

        private void cmbRetailDivision_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbRetailDivision_RequestData(object sender, EventArgs e)
        {
            cmbRetailDivision.SetData(Providers.RetailDivisionData.GetList(PluginEntry.DataModel,RetailDivisionSorting.RetailDivisionName), null);
        }

      
    }
}
