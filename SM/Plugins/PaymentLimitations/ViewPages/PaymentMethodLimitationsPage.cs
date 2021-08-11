using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.PaymentLimitations.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.Controls;

namespace LSOne.ViewPlugins.PaymentLimitations.ViewPages
{
    public partial class PaymentMethodLimitationsPage : UserControl, ITabView
    {
        private PaymentMethod paymentMethod;
        private List<PaymentMethodLimitation> limitations;
        private List<StorePaymentLimitation> storePaymentLimitations;

        public PaymentMethodLimitationsPage()
        {
            InitializeComponent();
            
            lvLimitations.ContextMenuStrip = new ContextMenuStrip();
            lvLimitations.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            btnsEditAddRemove.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.PaymentMethodsEdit);

            limitations = new List<PaymentMethodLimitation>();
        }        

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new PaymentMethodLimitationsPage();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {            
            paymentMethod = (PaymentMethod)internalContext;
            InitView();
        }

        public bool DataIsModified()
        {                        
            return false;
        }

        public bool SaveData()
        {            
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "PaymentMethodLimitations" && (changeHint == DataEntityChangeType.Add || changeHint == DataEntityChangeType.Delete || changeHint == DataEntityChangeType.Edit))
            {
                LoadLimitations();
            }

            if(objectName == "SetLocalCurrency")
            {
                paymentMethod.IsLocalCurrency = changeIdentifier == paymentMethod.ID;
                InitView();
            }
        }

        public void OnClose()
        {
            
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void InitView()
        {
            btnsEditAddRemove.Enabled =
            lvLimitations.Enabled =
            btnCopyFrom.Enabled = !paymentMethod.IsLocalCurrency;

            if (paymentMethod.IsLocalCurrency)
            {
                lvLimitations.ClearRows();
            }
            else
            {
                LoadLimitations();
            }
        }

        private void LoadLimitations()
        {
            limitations = Providers.PaymentLimitationsData.GetListForTender(PluginEntry.DataModel, paymentMethod.ID);

            lvLimitations.ClearRows();
            Row row;
            foreach (PaymentMethodLimitation limitation in limitations)
            {
                row = new Row();
                row.AddText((string)limitation.RestrictionCode);

                string relationText = "";
                switch (limitation.Type)
                {
                    case LimitationType.Item:
                        relationText = Properties.Resources.limitType_Item;
                        break;
                    case LimitationType.RetailGroup:
                        relationText = Properties.Resources.limitType_RetailGroup;
                        break;
                    case LimitationType.RetailDepartment:
                        relationText = Properties.Resources.limitType_RetailDepartment;
                        break;
                    case LimitationType.SpecialGroup:
                        relationText = Properties.Resources.limitType_SpecialGroup;
                        break;
                    case LimitationType.Everything:
                        relationText = Properties.Resources.limitType_Everything;
                        break;
                    default:
                        relationText = "";
                        break;
                }

                row.AddText(relationText);
                row.AddText(limitation.Description);
                row.AddText(limitation.VariantDescription);
                row.AddCell(new CheckBoxCell(limitation.Include, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.AddCell(new CheckBoxCell(limitation.TaxExempt, false, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                row.Tag = limitation.ID;

                lvLimitations.AddRow(row);
            }

            lvLimitations_SelectedIndexChanged(this, EventArgs.Empty);
            lvLimitations.AutoSizeColumns();
        }

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ExtendedMenuItem item;
            ContextMenuStrip menu;

            menu = lvLimitations.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    new EventHandler(btnsEditAddRemove_EditButtonClicked));

            item.Image = ContextButtons.GetEditButtonImage();
            item.Enabled = btnsEditAddRemove.EditButtonEnabled;
            item.Default = true;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    new EventHandler(btnsEditAddRemove_AddButtonClicked));

            item.Enabled = btnsEditAddRemove.AddButtonEnabled;

            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    new EventHandler(btnsEditAddRemove_RemoveButtonClicked));

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemove.RemoveButtonEnabled;

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.CopyLimitations,
                    400,
                    new EventHandler(btnCopyFrom_Click));

            item.Enabled = btnCopyFrom.Enabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PaymentLimitationsList", lvLimitations.ContextMenuStrip, lvLimitations);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvLimitations_DoubleClick(object sender, EventArgs e)
        {
            if (lvLimitations.Selection.Count != 0 && btnsEditAddRemove.EditButtonEnabled)
            {
                btnsEditAddRemove_EditButtonClicked(this, EventArgs.Empty);
            }
        }
        

        private void lvLimitations_SelectedIndexChanged(object sender, EventArgs e)
        {            
            btnsEditAddRemove.RemoveButtonEnabled = btnsEditAddRemove.EditButtonEnabled = (lvLimitations.Selection.Count > 0 && btnsEditAddRemove.AddButtonEnabled);
        }
        

        private void btnsEditAddRemove_AddButtonClicked(object sender, EventArgs e)
        {
            PaymentLimitationDialog dlg = new Dialogs.PaymentLimitationDialog(paymentMethod.ID, RecordIdentifier.Empty);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadLimitations();
            }
        }

        private List<PaymentMethodLimitation> GetSelectedLimitation()
        {
            List<PaymentMethodLimitation> result = new List<PaymentMethodLimitation>();
            if (lvLimitations.Selection.Count == 0)
            {
                return result;
            }

            RecordIdentifier ID = RecordIdentifier.Empty;
            if (lvLimitations.Selection.Count == 1)
            {
                ID = (RecordIdentifier)lvLimitations.Selection[0].Tag;
                result.Add(limitations.FirstOrDefault(f => f.ID == ID) ?? new PaymentMethodLimitation());
                return result;
            }

            for (int i = 0; i < lvLimitations.Selection.Count ; i++)
            {
                ID = (RecordIdentifier)lvLimitations.Selection[i].Tag;
                result.Add(limitations.FirstOrDefault(f => f.ID == ID) ?? new PaymentMethodLimitation());
            }

            return result;

        }

        private void btnsEditAddRemove_EditButtonClicked(object sender, EventArgs e)
        {
            List<PaymentMethodLimitation> limitations = GetSelectedLimitation();
            PaymentMethodLimitation selected = limitations.FirstOrDefault(f => f is PaymentMethodLimitation) ?? new PaymentMethodLimitation();

            PaymentLimitationDialog dlg = new PaymentLimitationDialog(paymentMethod.ID, selected.ID);
            dlg.ShowDialog();

        }

        private void btnsEditAddRemove_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (QuestionDialog.Show(
                Properties.Resources.DeletePaymentLimitationQuestion,
                Properties.Resources.DeletePaymentLimitation) == DialogResult.Yes)
            {
                if (storePaymentLimitations == null)
                {
                    storePaymentLimitations = Providers.StorePaymentLimitationData.GetList(PluginEntry.DataModel);
                }

                List<PaymentMethodLimitation> selectedLimitations = GetSelectedLimitation();
                foreach (PaymentMethodLimitation limitation in selectedLimitations)
                {
                    if (limitations.Count(c => c.RestrictionCode == limitation.RestrictionCode) == 1)
                    {
                        StorePaymentLimitation inUse = storePaymentLimitations.FirstOrDefault(f => f.LimitationMasterID == limitation.LimitationMasterID);
                        if (inUse != null)
                        {
                            Store store = Providers.StoreData.Get(PluginEntry.DataModel, inUse.StoreID, CacheType.CacheTypeApplicationLifeTime);
                            MessageDialog.Show(Properties.Resources.TheLimitationCannotBeDeletedAsItIsInUseOnAtLeastOneStore.Replace("{0}", store.Text));
                            LoadLimitations();
                            return;
                        }
                    }
                    Providers.PaymentLimitationsData.Delete(PluginEntry.DataModel, limitation.ID);
                    limitations.Remove(limitation);
                }

                LoadLimitations();
            }
        }

        private void btnCopyFrom_Click(object sender, EventArgs e)
        {
            CopyLimitationsDialog dlg = new CopyLimitationsDialog(paymentMethod.ID);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadLimitations();
            }
        }
    }
}
