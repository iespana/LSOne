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
    public partial class GroupSubcodeGeneralPage : UserControl, ITabView
    {
        private InfocodeSubcode infocodeSubcode;

        private WeakReference subcodeEditor;
        IPlugin plugin;

        public GroupSubcodeGeneralPage()
        {
            InitializeComponent();

            plugin = PluginEntry.Framework.FindImplementor(this, "ViewSubcode", null);
            subcodeEditor = plugin != null ? new WeakReference(plugin) : null;
            btnInfocode.Visible = (subcodeEditor != null);
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.GroupSubcodeGeneralPage();
        }

        #region ITabView Members

        public bool DataIsModified()
        {
            if (tbDescription.Text != infocodeSubcode.Text) return true;
            if (cmbInfocode.SelectedData.ID != infocodeSubcode.InfocodeId) return true;
            if (tbInfocodePrompt.Text != infocodeSubcode.InfocodePrompt) return true;
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            throw new NotImplementedException();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            infocodeSubcode = (InfocodeSubcode)internalContext;

            tbID.Text = infocodeSubcode.SubcodeId.ToString();
            tbDescription.Text = infocodeSubcode.Text;
            tbInfocodePrompt.Text = infocodeSubcode.InfocodePrompt;

            cmbInfocode.SelectedData = new DataEntity(new RecordIdentifier(infocodeSubcode.TriggerCode), infocodeSubcode.TriggerCode.PrimaryID.ToString()); 
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            throw new NotImplementedException();
        }

        public bool SaveData()
        {
            this.infocodeSubcode.Text = tbDescription.Text;

            this.infocodeSubcode.TriggerFunction = TriggerFunctions.Infocode;
            this.infocodeSubcode.TriggerCode = (string)cmbInfocode.SelectedData.Text;
            this.infocodeSubcode.InfocodePrompt = tbInfocodePrompt.Text;
            return true;
        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbInfocode_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                //e.TextToDisplay = ((DataEntity)e.Data).ID.ToString() + " - " + ((DataEntity)e.Data).Text;
                e.TextToDisplay = ((DataEntity)e.Data).Text;
            }
        }

        private void cmbInfocode_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", ""); //TODO Properties.Resources.NoSelection);
            cmbInfocode_SelectedDataChanged(this, EventArgs.Empty);
        }

        private void cmbInfocode_RequestData(object sender, EventArgs e)
        {
            cmbInfocode.SetData(Providers.InfocodeData.GetInfocodes(PluginEntry.DataModel, new UsageCategoriesEnum[] { UsageCategoriesEnum.None }, RefTableEnum.All), null);
        }

        private void cmbInfocode_SelectedDataChanged(object sender, EventArgs e)
        {
             //TODO
        }

        private void btnInfocode_Click(object sender, EventArgs e)
        {
            if (subcodeEditor.IsAlive)
            {
                ((IPlugin)subcodeEditor.Target).Message(this, "ViewSubcode", cmbInfocode.SelectedData.ID);
            }
        }
    }
}
