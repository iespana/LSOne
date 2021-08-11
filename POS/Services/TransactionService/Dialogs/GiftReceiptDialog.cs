using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.TransactionObjects;
using LSOne.POS.Processes.EventArguments;
using LSOne.POS.Processes.WinControls;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    public partial class GiftReceiptDialog : TouchBaseForm
    {
        private PosTransaction posTransaction;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private ReceiptItems receipt;

        private enum Buttons
        {
            SelectLine,
            SelectAll,
            ClearSelection,
            Print,
            Cancel
        }

        public LinkedList<int> LineNumbersToPrint { get; private set; }

        public GiftReceiptDialog(IConnectionManager entry)
        {
            InitializeComponent();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            if (!DesignMode)
            {
                LineNumbersToPrint = new LinkedList<int>();
                receipt = new ReceiptControlFactory().CreateReceiptItemsControl(pnlReceipt);
                receipt.ReceiptItemClick += Receipt_ReceiptItemClick;
            }
        }

        public GiftReceiptDialog(IConnectionManager entry, PosTransaction transaction)
            : this(entry)
        {
            posTransaction = transaction;
            InitializeButtons();
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            receipt.SetMode(ReceiptItemsViewMode.ItemsSelect);
            receipt.LookAndFeel.SkinName = "Light2";
            receipt.GridViewItems.OptionsView.ShowHorizontalLines = DevExpress.Utils.DefaultBoolean.True;
            if (posTransaction is RetailTransaction)
            {
                receipt.DisplayRTItems(posTransaction);
            }
            else if (posTransaction is CustomerPaymentTransaction)
            {
                receipt.DisplayCPTDeposits(posTransaction);
            }

            SetClearSearchEnabledState();
        }

        private void Print()
        {
            // Populate the return list if some items are selected. Otherwise report with an error....
            string value = "";

            for (int i = 0; i < receipt.ItemTable.Rows.Count; i++)
            {
                DataRow itemRow = receipt.ItemTable.Rows[i];
                value = itemRow["Selected"].ToString() ?? "0";

                if (value == "True")
                {
                    LineNumbersToPrint.AddLast(Convert.ToInt16(itemRow["LineId"].ToString()));
                }
            }

            // Check if any items are selected...
            if (LineNumbersToPrint.Count == 0)
            {
                LSOne.Services.Interfaces.Services.DialogService(dlgEntry).ShowMessage(Properties.Resources.NoItemsAreSelected, MessageBoxButtons.OK, MessageDialogType.Generic);  // No items are selected                
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void InitializeButtons()
        {
            touchScrollButtonPanel1.Clear();

            touchScrollButtonPanel1.AddButton(Properties.Resources.SelectLine, Buttons.SelectLine, "");
            touchScrollButtonPanel1.AddButton(Properties.Resources.SelectAll, Buttons.SelectAll, "");
            touchScrollButtonPanel1.AddButton(Properties.Resources.ClearSelection, Buttons.ClearSelection, Conversion.ToStr((int)Buttons.ClearSelection));

            touchScrollButtonPanel1.AddButton(Properties.Resources.Print, Buttons.Print, "", TouchButtonType.OK, DockEnum.DockEnd);
            touchScrollButtonPanel1.AddButton(Properties.Resources.Cancel, Buttons.Cancel, "", TouchButtonType.Cancel, DockEnum.DockEnd);
        }

        private void touchScrollButtonPanel1_Click(object sender, ScrollButtonEventArguments args)
        {
            if(args.Tag is Buttons)
            {
                switch ((Buttons)args.Tag)
                {
                    case Buttons.SelectLine:
                        receipt.ToggleSelect();
                        SetClearSearchEnabledState();
                        break;
                    case Buttons.SelectAll:
                        receipt.SelectAll();
                        SetClearSearchEnabledState();
                        break;
                    case Buttons.ClearSelection:
                        receipt.Deselect_All(this, EventArgs.Empty);
                        SetClearSearchEnabledState();
                        break;
                    case Buttons.Print:
                        Print();
                        break;
                    case Buttons.Cancel:
                        DialogResult = DialogResult.Cancel;
                        Close();
                        break;
                }
            }
        }

        private void Receipt_ReceiptItemClick(object sender, ReceiptItemClickArgs e)
        {
            SetClearSearchEnabledState();
        }

        private void SetClearSearchEnabledState()
        {
            bool hasSelectedItems = false;

            for (int i = 0; i < receipt.ItemTable.Rows.Count; i++)
            {
                if (receipt.ItemTable.Rows[i]["Selected"].ToString() == "True")
                {
                    hasSelectedItems = true;
                    break;
                }
            }

            touchScrollButtonPanel1.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearSelection), hasSelectedItems);
        }
    }
}