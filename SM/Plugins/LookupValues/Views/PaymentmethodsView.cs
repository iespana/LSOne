using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.LookupValues;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class PaymentmethodsView : ViewBase
    {
        RecordIdentifier selectedID;

        public PaymentmethodsView()
        {
            selectedID = RecordIdentifier.Empty;

            InitializeComponent();

            imageList1.Images.Add(Properties.Resources.PaymentMethodImage);

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Audit |
                ViewAttributes.Close | 
                ViewAttributes.Help;


            lvPaymentMethods.ContextMenuStrip = new ContextMenuStrip();
            lvPaymentMethods.ContextMenuStrip.Opening += lvPaymentMethods_Opening;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> descriptors)
        {
            descriptors.Add(new AuditDescriptor("PaymentMethods", RecordIdentifier.Empty, Properties.Resources.PaymentTypes, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.PaymentTypes;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            List<PaymentMethod> paymentMethods;
            ListViewItem item;

            paymentMethods = Providers.PaymentMethodData.GetList(PluginEntry.DataModel).OrderBy(o => o.Text).ToList();

            lvPaymentMethods.Items.Clear();

            foreach (PaymentMethod paymentMethod in paymentMethods)
            {
                item = new ListViewItem((string)paymentMethod.ID);
                item.SubItems.Add((string)paymentMethod.Text);
                item.SubItems.Add(paymentMethod.IsLocalCurrency ? Properties.Resources.Yes : Properties.Resources.No);
                item.Tag = paymentMethod;
                item.ImageIndex = 0;

                lvPaymentMethods.Add(item);

                if (selectedID == paymentMethod.ID)
                {
                    item.Selected = true;
                }
            }

            lvPaymentMethods.BestFitColumns();

            HeaderText = Properties.Resources.PaymentTypes;

            lvCardTypes_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected override bool DataIsModified()
        {
            return false;
        }

        protected override bool SaveData()
        {
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "PaymentMethod":
                    LoadData(false);
                    break;
            }
        }

        private void lvCardTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = lvPaymentMethods.SelectedItems.Count == 1 ? ((PaymentMethod)lvPaymentMethods.SelectedItems[0].Tag).ID : RecordIdentifier.Empty;
            btnsContextButtons.EditButtonEnabled = selectedID != RecordIdentifier.Empty && PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsEdit);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled && !((PaymentMethod)lvPaymentMethods.SelectedItems[0].Tag).IsLocalCurrency;

            if(IsLoaded)
            {
                PluginEntry.Framework.ViewController.RebuildViewContextBar(this);
            }
        }

        void lvPaymentMethods_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvPaymentMethods.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnEdit_Click);
            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsContextButtons.EditButtonEnabled;
            item.Default = true;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click);
            item.Image = ContextButtons.GetAddButtonImage();
            item.Enabled = btnsContextButtons.AddButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click);
            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsContextButtons.RemoveButtonEnabled;
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.SetAsLocalCurrency,
                    400,
                    SetAsLocalCurrency);
            item.Enabled = selectedID != RecordIdentifier.Empty && !((PaymentMethod)lvPaymentMethods.SelectedItems[0].Tag).IsLocalCurrency;
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PaymentMethodList", lvPaymentMethods.ContextMenuStrip, lvPaymentMethods);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.ShowPaymentMethodSheet(selectedID);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RecordIdentifier id;

            id = PluginOperations.NewPaymentMethod();

            if (id != RecordIdentifier.Empty)
            {
                selectedID = (string)id;
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeletePaymentMethod(selectedID);
        }

        private void lvCardTypes_DoubleClick(object sender, EventArgs e)
        {
            if (lvPaymentMethods.SelectedItems.Count != 0)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsEdit))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), btnAdd_Click), 10);
                    arguments.Add(new ContextBarItem(Properties.Resources.SetAsLocalCurrency, "SetLocalCurrency", selectedID != RecordIdentifier.Empty && !((PaymentMethod)lvPaymentMethods.SelectedItems[0].Tag).IsLocalCurrency, SetAsLocalCurrency), 20);
                }
            }
        }

        private void SetAsLocalCurrency(object sender, EventArgs args)
        {
            PluginOperations.SetPaymentMethodAsLocalCurrency(selectedID);
        }

        protected override void OnClose()
        {
            lvPaymentMethods.SmallImageList = null;
            base.OnClose();
        }
    }
}
