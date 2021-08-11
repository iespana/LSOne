using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    public partial class CrossAndModifierInfocodeGeneralPage : UserControl, ITabView
    {
        private Infocode infocode;

        public CrossAndModifierInfocodeGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new CrossAndModifierInfocodeGeneralPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (tbDescription.Text != infocode.Text) return true;
            if (tbExplanatoryHeaderText.Text != infocode.ExplanatoryHeaderText) return true;
            if ((int)infocode.Triggering != cmbTriggering.SelectedIndex) return true;
            if (chkMultipleSelection.Checked != infocode.MultipleSelection) return true;
            if (ntbMinSelection.Value != infocode.MinSelection) return true;
            if (ntbMaxSelection.Value != infocode.MaxSelection) return true;
            if (chkCreateTransInfoCodeEntry.Checked != infocode.CreateInfocodeTransEntries) return true;
            if (chkLinkItemLinesToTriggerLine.Checked != infocode.LinkItemLinesToTriggerLine) return true;
            if (cmbLinkedInfocode.SelectedData.ID != infocode.LinkedInfocodeId) return true;
            if ((int)infocode.OkPressedAction != cmbOkPressedAction.SelectedIndex) return true;

            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            throw new NotImplementedException();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            infocode = (Infocode)internalContext;
            
            tbID.Text = infocode.ID.ToString();
            tbDescription.Text = infocode.Text;
            tbExplanatoryHeaderText.Text = infocode.ExplanatoryHeaderText;
            cmbTriggering.SelectedIndex = (int)infocode.Triggering;
            chkMultipleSelection.Checked = infocode.MultipleSelection;
            ntbMinSelection.Value = infocode.MinSelection;
            ntbMaxSelection.Value = infocode.MaxSelection;
            chkCreateTransInfoCodeEntry.Checked = infocode.CreateInfocodeTransEntries;
            chkLinkItemLinesToTriggerLine.Checked = infocode.LinkItemLinesToTriggerLine;
            Infocode linkedInfocode = Providers.InfocodeData.Get(PluginEntry.DataModel, infocode.LinkedInfocodeId);
            cmbLinkedInfocode.SelectedData = (linkedInfocode == null) ? new DataEntity("","") : new DataEntity(linkedInfocode.ID, linkedInfocode.Text);
            cmbLinkedInfocode.Enabled = (linkedInfocode != null) && (linkedInfocode.ID != RecordIdentifier.Empty);
            chkLinkedInfocode.Checked = (linkedInfocode != null) && (linkedInfocode.ID != RecordIdentifier.Empty);
            cmbOkPressedAction.SelectedIndex = (int)infocode.OkPressedAction;
         }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            throw new NotImplementedException();
        }

        public bool SaveData()
        {
            infocode.Text = tbDescription.Text;
            
            infocode.ExplanatoryHeaderText = tbExplanatoryHeaderText.Text;
            infocode.Triggering = (TriggeringEnum)cmbTriggering.SelectedIndex;
            infocode.MultipleSelection = chkMultipleSelection.Checked;
            infocode.MinSelection = (int)ntbMinSelection.Value;
            infocode.MaxSelection = (int)ntbMaxSelection.Value;
            infocode.CreateInfocodeTransEntries = chkCreateTransInfoCodeEntry.Checked;
            infocode.LinkItemLinesToTriggerLine = chkLinkItemLinesToTriggerLine.Checked;
            infocode.LinkedInfocodeId = cmbLinkedInfocode.SelectedData.ID;
            infocode.OkPressedAction = (OKPressedActions)cmbOkPressedAction.SelectedIndex;
            return true;
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbLinkedInfocode_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == RecordIdentifier.Empty)
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((DataEntity)e.Data).ID + " - " + ((DataEntity)e.Data).Text;
            }
        }

        private void cmbLinkedInfocode_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", ""); //TODO Properties.Resources.NoSelection);
            cmbLinkedInfocode_SelectedDataChanged(this, EventArgs.Empty);
        }

        private void cmbLinkedInfocode_RequestData(object sender, EventArgs e)
        {
            cmbLinkedInfocode.SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new[] { UsageCategoriesEnum.CrossSelling } , RefTableEnum.All), null);
        }

        private void cmbLinkedInfocode_SelectedDataChanged(object sender, EventArgs e)
        {
            //TODO
        }

        private void chkLinkedInfocode_CheckedChanged(object sender, EventArgs e)
        {
            cmbLinkedInfocode.Enabled = chkLinkedInfocode.Checked;
            if (chkLinkedInfocode.Checked == false)
                cmbLinkedInfocode.SelectedData = new DataEntity();
        }
    }
}
