using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.ColorPalette;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services
{
    public partial class TenderRestrictionDialog : TouchBaseForm
    {
        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private PosTransaction tempPosTransaction;
        private ReceiptItems receipt;

        private enum Button
        {
            Cancel,
            Continue
        }

        private TenderRestrictionDialog(IConnectionManager entry)
        {
            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);
        }
        
        public TenderRestrictionDialog(IConnectionManager entry, IRetailTransaction retailTransaction, StorePaymentMethod paymentMethod) : this(entry)
        {
            InitializeComponent();

            buttonPanel.AddButton(Properties.Resources.Continue, Button.Continue, "", TouchButtonType.OK, DockEnum.DockEnd);
            buttonPanel.AddButton(Properties.Resources.Cancel, Button.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);

            ICloneTransactions cloning = Interfaces.Services.TransactionService(dlgEntry).CreateCloneTransactions();
            tempPosTransaction = (PosTransaction)cloning.CloneTransaction(dlgEntry, (PosTransaction)retailTransaction);

            List<ISaleLineItem> toRemove = new List<ISaleLineItem>();

            Func<ISaleLineItem, bool> hasLimitation = sli => !string.IsNullOrEmpty(sli.LimitationMasterID.StringValue) && sli.LimitationMasterID != Guid.Empty;
            Func<ISaleLineItem, bool> doesNotHaveLimitation = sli => string.IsNullOrEmpty(sli.LimitationMasterID.StringValue) || sli.LimitationMasterID == Guid.Empty;

            RetailTransaction tempRetailTransaction = (RetailTransaction)tempPosTransaction;

            if (dlgSettings.FunctionalityProfile.DialogLimitationDisplayType == FunctionalityProfile.LimitationDisplayType.ItemsExcluded)
            {
                toRemove = tempRetailTransaction
                    .SaleItems
                    .Where(x => x.Voided || (hasLimitation(x) && paymentMethod.PaymentLimitations.Exists(p => p.LimitationMasterID == x.LimitationMasterID)))
                    .ToList();
            }
            else
            {
                toRemove = tempRetailTransaction
                    .SaleItems
                    .Where(x => x.Voided || doesNotHaveLimitation(x) || (hasLimitation(x) && !paymentMethod.PaymentLimitations.Exists(p => p.LimitationMasterID == x.LimitationMasterID) || x.PaymentIndex > 0))
                    .ToList();
            }

            foreach(ISaleLineItem item in tempRetailTransaction.SaleItems.Where(x => !x.Voided && x.IsAssembly && !x.IsAssemblyComponent))
            {
                if(!toRemove.Contains(item))
                {
                    toRemove.RemoveAll(x => x.IsAssemblyComponent && x.LinkedToLineId == item.LineId);
                }
            }
            
            foreach (ISaleLineItem item in toRemove)
            {
                tempRetailTransaction.SaleItems.Remove(item);
            }

            // Because we may have removed lines from the transaction, we need to recalculate totals so that the total displayed on the dialog is correct
            // However, because we may have added discounts, we cannot use CalculationService.CalculateTotals()
            // because it would recalculate discounts based on the remaining lines on the temporary transaction which would be incorrect
            // and the dialog would display incorrect discounts for each line
            // Therefore we need to just clear the total amounts and update them without going through the entire process of recalculating the transactions with all its lines
            ((RetailTransaction)tempPosTransaction).ClearTotalAmounts();
            tempRetailTransaction.SaleItems.ToList().ForEach(x => ((RetailTransaction)tempPosTransaction).UpdateTotalAmounts(x));

            if (dlgSettings.FunctionalityProfile.DialogLimitationDisplayType == FunctionalityProfile.LimitationDisplayType.ItemsExcluded)
            {
                touchDialogBanner1.BannerText = Properties.Resources.ItemsExcludedFromPayment;
                lblMessage.Text = Properties.Resources.PaymentLimitationsExcludeTheseItems;
                lblSelectOtherPayment.Text = Properties.Resources.PleaseSelectAnotherPayment;
                lblSelectOtherPayment.Visible = true;
            }
            else
            {
                touchDialogBanner1.BannerText = Properties.Resources.ItemsIncludedInPayment;
                lblMessage.Text = Properties.Resources.PaymentLimitationsIncludeTheseItems;
                lblSelectOtherPayment.Visible = false;
                pnlReceipt.Location = new Point(pnlReceipt.Location.X, pnlReceipt.Location.Y - lblSelectOtherPayment.Height);
                pnlReceipt.Height = pnlReceipt.Height + lblSelectOtherPayment.Height;
            }

            if (!DesignMode)
            {
                receipt = new ReceiptControlFactory().CreateReceiptItemsControl(pnlReceipt);
            }
        }

        private void TenderRestrictionDialog_Load(object sender, EventArgs e)
        {
            receipt.SetMode(ReceiptItemsViewMode.ItemsSelect);
            receipt.DisplayRTItems(tempPosTransaction);
            receipt.LookAndFeel.SkinName = "Light";
            receipt.GridViewItems.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
        }

        private void buttonPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            if ((Button) args.Tag == Button.Continue)
            {
                DialogResult = DialogResult.OK;
            }
            else if ((Button) args.Tag == Button.Continue)
            {
                DialogResult = DialogResult.Cancel;
            }

            Close();
        }
    }
}