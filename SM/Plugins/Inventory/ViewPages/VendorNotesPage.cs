using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Inventory.ViewPages
{
    public partial class VendorNotesPage : UserControl, ITabView
    {
        Vendor vendor;

        public VendorNotesPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.VendorNotesPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            vendor = (Vendor)internalContext;

            tbNotes.Text = vendor.LongDescription;
        }

        public bool DataIsModified()
        {
            if (tbNotes.Text != vendor.LongDescription)
            {
                vendor.Dirty = true;
                return true;
            }

            return false;
        }

        public bool SaveData()
        {
            vendor.LongDescription = tbNotes.Text;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

    }
}
