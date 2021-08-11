using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.Hospitality.ListItems;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityTerminalHospitalityPage : UserControl, ITabView
    {
        private Terminal terminal;

        public HospitalityTerminalHospitalityPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HospitalityTerminalHospitalityPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            terminal = (Terminal)internalContext;
            chkSwitchUserWhenEnteringPOS.Checked = terminal.SwitchUserWhenEnteringPOS;
            LoadItems();
        }

        public bool DataIsModified()
        {
            // Compare the numbers of checked items vs. the number of sales types attached to this terminal
            return ConstructSalesTypeFilter() != terminal.SalesTypeFilter
                   || terminal.SwitchUserWhenEnteringPOS != chkSwitchUserWhenEnteringPOS.Checked;
        }

        public bool SaveData()
        {
            terminal.SalesTypeFilter = ConstructSalesTypeFilter();
            terminal.SwitchUserWhenEnteringPOS = chkSwitchUserWhenEnteringPOS.Checked;
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

        private void LoadItems()
        {
            // Populate the flow layout panel
            List<HospitalityTypeListItem> hospitalityTypes = Providers.HospitalityTypeData.GetHostpitalityTypesForRestaurant(PluginEntry.DataModel, terminal.StoreID);
            string[] salesTypes = terminal.SalesTypeFilter.Split(new[]{'|'});
           
            foreach (HospitalityTypeListItem hospitalityType in hospitalityTypes)
            {
                pnlHospitalityTypes.Controls.Add(new CheckBox(){Text = hospitalityType.SalesType + " - " + hospitalityType.Text, 
                                                                Tag = hospitalityType.SalesType,
                                                                AutoSize = true,
                                                                Checked = salesTypes.Contains<string>((string)hospitalityType.SalesType)});

            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (Control control in pnlHospitalityTypes.Controls)
            {
                ((CheckBox)control).Checked = true;
            }
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            foreach (Control control in pnlHospitalityTypes.Controls)
            {
                ((CheckBox)control).Checked = false;
            }
        }

        /// <summary>
        /// Goes through each checkbox and constructs a salestype filter string
        /// </summary>
        /// <returns>A sales type filter string with the selected hospitality types</returns>
        private string ConstructSalesTypeFilter()
        {
            string salesTypeFilter = "";

            foreach (Control control in pnlHospitalityTypes.Controls)
            {
                if (((CheckBox)control).Checked)
                {
                    salesTypeFilter = salesTypeFilter + control.Tag + "|";
                }
            }

            if (salesTypeFilter.EndsWith("|"))
            {
                salesTypeFilter = salesTypeFilter.Substring(0, salesTypeFilter.Length - 1);
            }

            return salesTypeFilter;
        }

    }
}
