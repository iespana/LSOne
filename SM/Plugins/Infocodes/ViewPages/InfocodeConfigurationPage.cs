//using LSRetail.StoreController.Controls;

using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    public partial class InfocodeConfigurationPage : UserControl, ITabView
    {
        private Infocode infocode;

        public InfocodeConfigurationPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.InfocodeConfigurationPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (chkMultipleSelection.Checked != infocode.MultipleSelection) return true;
            if (ntbMinSelection.Value != infocode.MinSelection) return true;
            if (ntbMaxSelection.Value != infocode.MaxSelection) return true;

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            if (internalContext is Infocode)
            {
                infocode = (Infocode)internalContext;

                chkMultipleSelection.Checked = infocode.MultipleSelection;
                ntbMinSelection.Value = infocode.MinSelection;
                ntbMaxSelection.Value = infocode.MaxSelection;
                
            }
            else
                infocode = new Infocode();
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
  
        }

        public bool SaveData()
        {
            infocode.MultipleSelection = chkMultipleSelection.Checked;
            infocode.MinSelection = (int)ntbMinSelection.Value;
            infocode.MaxSelection = (int) ntbMaxSelection.Value;
            

            return true;
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
