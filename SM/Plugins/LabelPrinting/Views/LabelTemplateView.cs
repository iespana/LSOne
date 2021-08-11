using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Labels;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Labels;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewPlugins.LabelPrinting.Properties;

namespace LSOne.ViewPlugins.LabelPrinting.Views
{
    public partial class LabelTemplateView : ViewBase
    {
        RecordIdentifier labelTemplateID;
        LabelTemplate labelTemplate;

        private TabControl.Tab templateTab;

        public LabelTemplateView(RecordIdentifier groupID)
            : this()
        {
            labelTemplateID = groupID;

            labelTemplate = Providers.LabelTemplateData.Get(PluginEntry.DataModel, labelTemplateID);
        }

        public LabelTemplateView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.ContextBar |
                ViewAttributes.Close |
                ViewAttributes.Help;

            labelTemplate = new LabelTemplate();

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.LabelTemplatesEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("LabelTemplate", labelTemplateID, LabelPrinting.Properties.Resources.LabelTemplate, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return LabelPrinting.Properties.Resources.LabelTemplate;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        {
                return labelTemplateID;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                // Do any possible re-load on rever logic here.
                templateTab = new TabControl.Tab(LabelPrinting.Properties.Resources.Template, 
                    ViewPages.LabelTemplatePage.CreateInstance);

                tabSheetTabs.AddTab(templateTab);

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, labelTemplateID, labelTemplate);
            }

            tbID.Text = (string)labelTemplate.ID;
            tbName.Text = labelTemplate.Text;

            cmbContext.Enabled = false;
            cmbContext.Items.Clear();
            cmbContext.Items.AddRange(LabelTemplate.ContextStrings);
            cmbContext.SelectedItem = labelTemplate.ContextString;
                                        
            HeaderText = Resources.LabelTemplate;
            //HeaderIcon = Properties.Resources.RetailItemImage16;

            tabSheetTabs.SetData(isRevert, labelTemplateID, labelTemplate);
        }

        protected override bool DataIsModified()
        {
            if (tabSheetTabs.IsModified()) return true;
            if (tbID.Text != labelTemplate.ID) return true;
            //if (cmbContext.SelectedText != labelTemplate.Context) return true;
            if (tbName.Text != labelTemplate.Text) return true;            

            return false;
        }

        protected override bool SaveData()
        {
            labelTemplate.ID = tbID.Text;
            labelTemplate.Text = tbName.Text;

            tabSheetTabs.GetData();

            Providers.LabelTemplateData.Save(PluginEntry.DataModel, labelTemplate);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "LabelTemplate", labelTemplate.ID, null);

            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "LabelTemplate":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == labelTemplateID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    if (changeHint == DataEntityChangeType.Add)
                    {
                        LoadData(false);
                    }
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteLabelTemplate(labelTemplateID);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Delete, "LabelTemplate", labelTemplateID, null);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }
    }
}
