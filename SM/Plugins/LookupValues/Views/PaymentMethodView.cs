using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class PaymentMethodView : ViewBase
    {
        RecordIdentifier paymentMethodID;
        PaymentMethod paymentMethod;

        public PaymentMethodView(RecordIdentifier paymentMethodID) 
            : this()
        {
            this.paymentMethodID = paymentMethodID;
        }

        public PaymentMethodView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Close |
                ViewAttributes.Audit |
                ViewAttributes.Help;

            this.ReadOnly = !PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> descriptors)
        {
            descriptors.Add(new AuditDescriptor("PaymentMethod", paymentMethodID, Properties.Resources.PaymentType, true));
            descriptors.Add(new AuditDescriptor("PaymentMethodLimitations", paymentMethodID, Properties.Resources.PaymentLimitations, false));
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PaymentType;
            }
        }

        protected override string SecondaryRevertText
        {
            get
            {
                return Properties.Resources.CannotRevertLimitations;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return paymentMethodID;
	        }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.PaymentType + ": " + (string)tbDescription.Text;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (!isRevert)
            {
                paymentMethod = Providers.PaymentMethodData.Get(PluginEntry.DataModel, paymentMethodID);

                AddParentViewDescriptor(new ParentViewDescriptor(
                        RecordIdentifier.Empty,
                        Properties.Resources.PaymentTypes,
                        Properties.Resources.PaymentMethodImage,
                        new ShowParentViewHandler(PluginOperations.ShowPaymentMethodsSheet)));

                // Allow other plugins to extend this tab panel
                tabSheetTabs.Broadcast(this, paymentMethodID, paymentMethod);
            }

            tbID.Text = (string)paymentMethod.ID;
            tbDescription.Text = paymentMethod.Text;
            cmbDefaultFunction.SelectedIndex = (int)paymentMethod.DefaultFunction;
            chkLocalCurrency.Checked = paymentMethod.IsLocalCurrency;
          
            HeaderText = Description;

            tabSheetTabs.SetData(isRevert, paymentMethodID, paymentMethod);

        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != paymentMethod.Text) return true;
            if (cmbDefaultFunction.SelectedIndex != (int)paymentMethod.DefaultFunction) return true;

            return false;
        }

        protected override bool SaveData()
        {
            paymentMethod.Text = tbDescription.Text;
            paymentMethod.DefaultFunction = (PaymentMethodDefaultFunctionEnum)cmbDefaultFunction.SelectedIndex;

            Providers.PaymentMethodData.Save(PluginEntry.DataModel, paymentMethod);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PaymentMethod", paymentMethod.ID, paymentMethod);

            return true;
        }

        protected override void OnDelete()
        {
            PluginOperations.DeletePaymentMethod(paymentMethodID);
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PaymentMethod":
                    if ((changeHint & DataEntityChangeType.Delete) == DataEntityChangeType.Delete && changeIdentifier == paymentMethodID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
                case "SetLocalCurrency":
                    paymentMethod.IsLocalCurrency = changeIdentifier == paymentMethod.ID;
                    chkLocalCurrency.Checked = paymentMethod.IsLocalCurrency;
                    PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
                    break;
            }

            tabSheetTabs.BroadcastChangeInformation(changeHint, objectName, changeIdentifier, param);
        }

        protected override void OnClose()
        {
            tabSheetTabs.SendCloseMessage();

            base.OnClose();
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsEdit))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.SetAsLocalCurrency, "SetLocalCurrency", !paymentMethod.IsLocalCurrency, SetAsLocalCurrency), 20000);
                }
            }
        }

        private void SetAsLocalCurrency(object sender, EventArgs args)
        {
            PluginOperations.SetPaymentMethodAsLocalCurrency(paymentMethod.ID);
        }
    }
}
