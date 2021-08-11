using System;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls
{
    public partial class KdsStyleButtons : ContextButtons
    {
        private DualDataComboBox buddyControl;
        private Guid contextGuid;

        public KdsStyleButtons()
        {
            InitializeComponent();

            AddButtonClicked += OnAddButtonClicked;
            EditButtonClicked += OnEditButtonClicked;

            Context = ButtonTypes.EditAdd;
        }

        private void OnEditButtonClicked(object sender, EventArgs eventArgs)
        {
            if (buddyControl != null && buddyControl.SelectedData != null)
            {
                var styleId = buddyControl.SelectedData.ID;
                var param = new PluginOperationArguments(styleId, null);
                PluginEntry.Framework.RunOperation("EditUIStyle", this, param);
                if (param.Result != null)
                {
                    buddyControl.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, styleId);                    
                }
            }
        }

        private void OnAddButtonClicked(object sender, EventArgs eventArgs)
        {
            var param = new PluginOperationArguments(contextGuid, null);
            PluginEntry.Framework.RunOperation("NewUIStyle", this, param);
            if (param.Result != null)
            {
                DataEntity newStyleEntity = (DataEntity)param.Result;
                buddyControl.SelectedData = newStyleEntity;
                BuddyControlOnSelectedDataChanged(this, EventArgs.Empty);
            }
        }

        public void SetBuddyControl(DualDataComboBox buddyControl)
        {
            this.contextGuid = (Guid)buddyControl.Tag;
            this.buddyControl = buddyControl;
            buddyControl.SelectedDataChanged += BuddyControlOnSelectedDataChanged;
            SetVisible();
        }

        private void BuddyControlOnSelectedDataChanged(object sender, EventArgs eventArgs)
        {
            EditButtonEnabled = !RecordIdentifier.IsEmptyOrNull(buddyControl.SelectedDataID);
        }

        private void SetVisible()
        {
            bool canEditStyle = PluginEntry.Framework.CanRunOperation("EditUIStyle");
            EditButtonEnabled = (canEditStyle) && 
                buddyControl.SelectedData != null &&
                buddyControl.SelectedDataID != RecordIdentifier.Empty;
            AddButtonEnabled = PluginEntry.Framework.CanRunOperation("NewUIStyle");
            Visible = (EditButtonEnabled || AddButtonEnabled);
        }
    }
}
