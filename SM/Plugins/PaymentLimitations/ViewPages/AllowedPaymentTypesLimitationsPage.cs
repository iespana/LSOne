using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.PaymentLimitations.ViewPages
{
    public partial class AllowedPaymentTypesLimitationsPage : UserControl, ITabView
    {
        RecordIdentifier storeID;
        StorePaymentMethod paymentMethod;
        PaymentMethod basePaymentMethod;
        List<PaymentMethodLimitation> limitationCodes;
        bool modified;

        public AllowedPaymentTypesLimitationsPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new AllowedPaymentTypesLimitationsPage();
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            storeID = context;
            paymentMethod = (StorePaymentMethod)internalContext;
            basePaymentMethod = Providers.PaymentMethodData.Get(PluginEntry.DataModel, paymentMethod.PaymentTypeID);

            chkForceRefundToThisPaymentType.Checked = paymentMethod.ForceRefundToThisPaymentType;

            if(!basePaymentMethod.IsLocalCurrency)
            {
                LoadItems();
            }
        }

        private void LoadItems()
        {
            limitationCodes = Providers.PaymentLimitationsData.GetListForTender(PluginEntry.DataModel, paymentMethod.PaymentTypeID).GroupBy(x => x.LimitationMasterID).Select(x => x.First()).ToList();

            Row row;
            CheckBoxCell cell;

            foreach (PaymentMethodLimitation code in limitationCodes)
            {
                row = new Row();

                row.AddText(code.RestrictionCode.StringValue);

                cell = new CheckBoxCell(code.TaxExempt);
                cell.Enabled = false;
                cell.CheckBoxAlignment = CheckBoxCell.CheckBoxAlignmentEnum.Center;
                row.AddCell(cell);

                cell = new CheckBoxCell(paymentMethod.PaymentLimitations.FirstOrDefault(f => f.LimitationMasterID == code.LimitationMasterID) != null);
                cell.Enabled = true;
                cell.CheckBoxAlignment = CheckBoxCell.CheckBoxAlignmentEnum.Center;
                row.AddCell(cell);

                row.Tag = code;

                lvLimitations.AddRow(row);
            }

            lvLimitations.AutoSizeColumns();
        }

        #region ITabPanel Members

        public bool DataIsModified()
        {
            if (chkForceRefundToThisPaymentType.Checked != paymentMethod.ForceRefundToThisPaymentType) return true;

            return modified;
        }

        public bool SaveData()
        {
            paymentMethod.ForceRefundToThisPaymentType = chkForceRefundToThisPaymentType.Checked;

            paymentMethod.PaymentLimitations.Clear();
            foreach (Row row in lvLimitations.Rows)
            {
                if (((CheckBoxCell)row[2]).Checked)
                {
                    StorePaymentLimitation limitation = new StorePaymentLimitation();
                    limitation.StoreID = paymentMethod.StoreID;
                    limitation.TenderTypeID = paymentMethod.PaymentTypeID;
                    limitation.RestrictionCode = ((PaymentMethodLimitation)row.Tag).RestrictionCode;

                    PaymentMethodLimitation code = limitationCodes.FirstOrDefault(f => f.RestrictionCode == limitation.RestrictionCode);
                    if (code != null)
                    {
                        limitation.LimitationMasterID = code.LimitationMasterID;
                    }
                    paymentMethod.PaymentLimitations.Add(limitation);
                }
            }
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (objectName == "SetLocalCurrency")
            {
                basePaymentMethod.IsLocalCurrency = changeIdentifier == paymentMethod.ID;

                if(basePaymentMethod.IsLocalCurrency)
                {
                    lvLimitations.ClearRows();
                }
            }
        }

        public void OnClose()
        {
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void LvLimitations_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            modified = true;
        }
    }
}
