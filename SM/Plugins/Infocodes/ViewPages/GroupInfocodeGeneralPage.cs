using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Infocodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Infocodes.ViewPages
{
    public partial class GroupInfocodeGeneralPage : UserControl, ITabView
    {
        private Infocode infocode;

        public GroupInfocodeGeneralPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.GroupInfocodeGeneralPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (tbDescription.Text != infocode.Text) return true;
            if (tbExplanatoryHeaderText.Text != infocode.ExplanatoryHeaderText) return true;
            if (ntbMinSelection.Value != infocode.MinSelection) return true;
            if (ntbMaxSelection.Value != infocode.MaxSelection) return true;
            if (chkLinkItemLinesToTriggerLine.Checked != infocode.LinkItemLinesToTriggerLine) return true;
            if (cmbTriggering.SelectedIndex != (int)infocode.Triggering) return true;
            if (cmbLinkedInfocode.SelectedData.ID != infocode.LinkedInfocodeId) return true;
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
            ntbMinSelection.Value = infocode.MinSelection;
            ntbMaxSelection.Value = infocode.MaxSelection;
            chkLinkItemLinesToTriggerLine.Checked = infocode.LinkItemLinesToTriggerLine;
            cmbLinkedInfocode.SelectedData = new DataEntity(new RecordIdentifier(infocode.LinkedInfocodeId), infocode.LinkedInfocodeId.PrimaryID.ToString());

            if (infocode.UsageCategory == UsageCategoriesEnum.CrossSelling)
            {
                lblLinkedInfocode.Visible = false;
                cmbLinkedInfocode.Visible = false;
                //btnEditLinkedInfocode.Visible = false;
            }
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
            infocode.MinSelection = Convert.ToInt32(ntbMinSelection.Value);
            infocode.MaxSelection = Convert.ToInt32(ntbMaxSelection.Value);
            infocode.LinkItemLinesToTriggerLine = chkLinkItemLinesToTriggerLine.Checked;
            infocode.LinkedInfocodeId = cmbLinkedInfocode.SelectedData.Text;
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
            if (((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                e.TextToDisplay = ((DataEntity)e.Data).ID.ToString() + " - " + ((DataEntity)e.Data).Text;
            }
        }

        private void cmbLinkedInfocode_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", ""); //TODO Properties.Resources.NoSelection);
            cmbLinkedInfocode_SelectedDataChanged(this, EventArgs.Empty);
        }

        private void cmbLinkedInfocode_RequestData(object sender, EventArgs e)
        {
            //cmbLinkedInfocode.SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new UsageCategories[] { UsageCategories.None }, true), null);
        }

        private void cmbLinkedInfocode_SelectedDataChanged(object sender, EventArgs e)
        {
            //TODO
        }
    }
}
