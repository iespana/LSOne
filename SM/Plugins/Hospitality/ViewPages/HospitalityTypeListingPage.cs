using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class HospitalityTypeListingPage : UserControl, ITabView
    {
        private HospitalityType hospitalityType;

        public HospitalityTypeListingPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.HospitalityTypeListingPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            hospitalityType = (HospitalityType)internalContext;

            chkViewTransDate.Checked = hospitalityType.ViewTransDate;
            chkViewTransTime.Checked = hospitalityType.ViewTransTime;
            chkViewListTotals.Checked = hospitalityType.ViewListTotals;
            chkViewRestaurant.Checked = hospitalityType.ViewRestaurant;
            chkViewCountdown.Checked = hospitalityType.ViewCountDown;
            chkViewProgressStatus.Checked = hospitalityType.ViewProgressStatus;
            // TODO: add direct edit operation id
            chkAllowNewEntries.Checked = hospitalityType.AllowNewEntries;
            cmbOrderBy.SelectedIndex = (int)hospitalityType.OrderBy;

        }

        public bool DataIsModified()
        {
            if (chkViewTransDate.Checked != hospitalityType.ViewTransDate) return true;
            if (chkViewTransTime.Checked != hospitalityType.ViewTransTime) return true;
            if (chkViewListTotals.Checked != hospitalityType.ViewListTotals) return true;
            if (chkViewRestaurant.Checked != hospitalityType.ViewRestaurant) return true;
            if (chkViewCountdown.Checked != hospitalityType.ViewCountDown) return true;
            if (chkViewProgressStatus.Checked != hospitalityType.ViewProgressStatus) return true;
            // TODO: add direct edit operation id
            if (chkAllowNewEntries.Checked != hospitalityType.AllowNewEntries) return true;
            if (cmbOrderBy.SelectedIndex != (int)hospitalityType.OrderBy) return true;
            
            return false;
        }

        public bool SaveData()
        {
            hospitalityType.ViewTransDate = chkViewTransDate.Checked;
            hospitalityType.ViewTransTime = chkViewTransTime.Checked;
            hospitalityType.ViewListTotals = chkViewListTotals.Checked;
            hospitalityType.ViewRestaurant = chkViewRestaurant.Checked;
            hospitalityType.ViewCountDown = chkViewCountdown.Checked;
            hospitalityType.ViewProgressStatus = chkViewProgressStatus.Checked;
            // TODO: add direct edit operation id
            hospitalityType.AllowNewEntries = chkAllowNewEntries.Checked;
            hospitalityType.OrderBy = (HospitalityType.OrderByEnum)cmbOrderBy.SelectedIndex;

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

        private void DataFormatterHandler(object sender, DropDownFormatDataArgs e)
        {
            DataEntity item = ((DataEntity)e.Data);
            e.TextToDisplay = (item.ID == RecordIdentifier.Empty ? "" : item.ID.ToString() + " - ") + item.Text;
        }
    }
}
