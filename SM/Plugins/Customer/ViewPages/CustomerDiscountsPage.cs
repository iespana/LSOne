using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Customer.ViewPages
{
    public partial class CustomerDiscountsPage : UserControl, ITabView
    {

        private LSOne.DataLayer.BusinessObjects.Customers.Customer customer;
        private static bool containsTabs;

        public CustomerDiscountsPage()
        {
            InitializeComponent();            
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CustomerDiscountsPage();
        }

        public static void TabMessage(TabControl sender, TabControl.Tab tab, int hint, object data)
        {
            tab.Visible = containsTabs;
            sender.Invalidate();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            customer = (LSOne.DataLayer.BusinessObjects.Customers.Customer)internalContext;

            if (!isRevert)
            {
                tabSheetTabs.Broadcast(this, context, internalContext);
                containsTabs = tabSheetTabs.TabCount > 0;
            }

            tabSheetTabs.SetData(isRevert, context, internalContext);
        }

        public bool DataIsModified()
        {
            return tabSheetTabs.IsModified();
        }

        public bool SaveData()
        {
            return tabSheetTabs.GetData();            
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
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
