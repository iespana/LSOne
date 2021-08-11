using System;
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
    public partial class InfocodePrintingPage : UserControl, ITabView
    {
        private Infocode infocode;

        public InfocodePrintingPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.InfocodePrintingPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (chkPrintInputNameOnReceipt.Checked != infocode.PrintInputNameOnReceipt) return true;
            if (chkPrintInputOnReceipt.Checked != infocode.PrintInputOnReceipt) return true;
            if (chkPrintPromptOnReceipt.Checked != infocode.PrintPromptOnReceipt) return true;
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            throw new NotImplementedException();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            infocode = (Infocode)internalContext;
            chkPrintInputNameOnReceipt.Checked = infocode.PrintInputNameOnReceipt;
            chkPrintInputOnReceipt.Checked = infocode.PrintInputOnReceipt;
            chkPrintPromptOnReceipt.Checked = infocode.PrintPromptOnReceipt;
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            throw new NotImplementedException();
        }

        public bool SaveData()
        {
            infocode.PrintInputNameOnReceipt = chkPrintInputNameOnReceipt.Checked;
            infocode.PrintInputOnReceipt = chkPrintInputOnReceipt.Checked;
            infocode.PrintPromptOnReceipt = chkPrintPromptOnReceipt.Checked;
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
