using LSOne.Controls;
using LSOne.Controls.SupportClasses;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Ledger;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.Delegates;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.Panels;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System.Drawing;
using System.Windows.Forms;
using System;
using LSOne.DataLayer.TransactionObjects;

namespace LSOne.Services.WinFormsTouch
{
    public partial class EditCustomerDialog : TouchBaseForm
    {
        public enum Buttons
        {
            Details,
            Address,
            Other,
            Ledger,
            SearchLedger,
            ClearLedgerSearch,
            Loyalty,
            AddLoyaltyCard,
            UseLoyaltyCard,
            Save,
            Close
        }

        public Customer Customer { get; private set; }

        private ShowKeyboardInputHandler keyboardHandler;

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;
        private IPosTransaction transaction;

        private CustomerDetailsPanel customerDetailsPanel;
        private CustomerOtherPanel customerOtherPanel;
        private CustomerLedgerPanel customerLedgerPanel;
        private CustomerLoyaltyPanel customerLoyaltyPanel;
        private Control activePanel;

        private bool expandLedger;
        private bool expandLoyalty;

        private bool clearLedgerSearchActive;

        public EditCustomerDialog(IConnectionManager entry, Customer customer, ShowKeyboardInputHandler keyboardHandler, IPosTransaction transaction)
        {
            InitializeComponent();

            touchDialogBanner1.Location = new Point(1, 1);
            touchDialogBanner1.Width = Width - 2;

            this.Customer = customer;
            this.keyboardHandler = keyboardHandler;
            this.transaction = transaction;

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            AddButtons(false, false);
            LoadDefaultPanels();
            ShowDetailsPanel();
        }

        public void UpdateButtonStatus()
        {
            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.UseLoyaltyCard), customerLoyaltyPanel.loyaltyCardSelected);
        }

        //This is used to initialize panels that have data which could be saved and needs validation
        private void LoadDefaultPanels()
        {
            if(customerDetailsPanel == null)
            {
                customerDetailsPanel = new CustomerDetailsPanel(dlgEntry, Customer);
                customerDetailsPanel.Location = new Point(0, 0);
            }

        }

        private void AddButtons(bool expandLedger, bool expandLoyalty)
        {
            btnPanel.Clear();

            this.expandLedger = expandLedger;
            this.expandLoyalty = expandLoyalty;

            btnPanel.AddButton(Properties.Resources.EditCustomerDetailsButtonText, Buttons.Details, Conversion.ToStr((int)Buttons.Details));
            btnPanel.AddButton(Properties.Resources.Other, Buttons.Other, Conversion.ToStr((int)Buttons.Other));

            if(dlgEntry.HasPermission(Permission.CustomerTransactions))
            {
                btnPanel.AddButton(Properties.Resources.Ledger, Buttons.Ledger, Conversion.ToStr((int)Buttons.Ledger), TouchButtonType.Action, DockEnum.DockNone, expandLedger ? Properties.Resources.white_line_arrow_up_16 : Properties.Resources.white_line_arrow_down_16, ImageAlignment.Left);

                if(expandLedger)
                {
                    btnPanel.AddButton(Properties.Resources.Search, Buttons.SearchLedger, Conversion.ToStr((int)Buttons.SearchLedger));
                    btnPanel.AddButton(Properties.Resources.ClearSearch, Buttons.ClearLedgerSearch, Conversion.ToStr((int)Buttons.ClearLedgerSearch));
                    btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearLedgerSearch), clearLedgerSearchActive);
                }
            }

            if(dlgEntry.HasPermission(Permission.AddCustomerToLoyaltyCard) || dlgEntry.HasPermission(Permission.LoyaltyRequest))
            {
                btnPanel.AddButton(Properties.Resources.Loyalty, Buttons.Loyalty, Conversion.ToStr((int)Buttons.Loyalty), TouchButtonType.Action, DockEnum.DockNone, expandLoyalty ? Properties.Resources.white_line_arrow_up_16 : Properties.Resources.white_line_arrow_down_16, ImageAlignment.Left);

                if(expandLoyalty)
                {
                    if(dlgEntry.HasPermission(Permission.AddCustomerToLoyaltyCard))
                    {
                        btnPanel.AddButton(Properties.Resources.AddCard, Buttons.AddLoyaltyCard, Conversion.ToStr((int)Buttons.AddLoyaltyCard));

                        if(dlgSettings.TrainingMode)
                        {
                            btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.AddLoyaltyCard), false);
                        }
                    }

                    if (dlgEntry.HasPermission(Permission.LoyaltyRequest))
                    {
                        btnPanel.AddButton(Properties.Resources.UseCard, Buttons.UseLoyaltyCard, Conversion.ToStr((int)Buttons.UseLoyaltyCard));
                        btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.UseLoyaltyCard), false);
                    }
                }
            }

            btnPanel.AddButton(Properties.Resources.Save, Buttons.Save, Conversion.ToStr((int)Buttons.Save), TouchButtonType.OK, DockEnum.DockEnd);
            btnPanel.AddButton(Properties.Resources.Close, Buttons.Close, Conversion.ToStr((int)Buttons.Close), TouchButtonType.Cancel, DockEnum.DockEnd);
        }

        private void ShowKeyboard(bool showKeyboard)
        {
            if(showKeyboard && !touchKeyboard.Visible)
            {
                touchKeyboard.Visible = true;
                pnlControls.Height -= touchKeyboard.Height + 5;
                btnPanel.Height -= touchKeyboard.Height + 5;

            }
            else if(!showKeyboard && touchKeyboard.Visible)
            {
                touchKeyboard.Visible = false;
                pnlControls.Height += touchKeyboard.Height + 5;
                btnPanel.Height += touchKeyboard.Height + 5;
            }
        }

        private void btnPanel_Click(object sender, ScrollButtonEventArguments args)
        {
            //Set keyboard visibility before adding the panel to the view for proper resizing
            switch((int)args.Tag)
            {
                case (int)Buttons.Details:
                    ShowKeyboard(true);
                    AddButtons(false, false);
                    ShowDetailsPanel();
                    break;
                case (int)Buttons.Address:
                    break;
                case (int)Buttons.Other:
                    ShowKeyboard(true);
                    AddButtons(false, false);
                    ShowOtherPanel();
                    break;
                case (int)Buttons.Ledger:
                    ShowKeyboard(false);
                    AddButtons(!expandLedger, false);
                    ShowLedgerPanel();
                    break;
                case (int)Buttons.SearchLedger:
                    CustomerLedgerFilterDialog dlg = new CustomerLedgerFilterDialog(dlgEntry);
                    if(dlg.ShowDialog() == DialogResult.OK)
                    {
                        customerLedgerPanel.LoadData(dlg.LedgerFilter);
                        clearLedgerSearchActive = true;
                        btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearLedgerSearch), clearLedgerSearchActive);
                    }
                    break;
                case (int)Buttons.ClearLedgerSearch:
                    customerLedgerPanel.LoadData(new CustomerLedgerFilter());
                    clearLedgerSearchActive = false;
                    btnPanel.SetButtonEnabled(Conversion.ToStr((int)Buttons.ClearLedgerSearch), clearLedgerSearchActive);
                    break;
                case (int)Buttons.Loyalty:
                    ShowKeyboard(false);
                    AddButtons(false, !expandLoyalty);
                    ShowLoyaltyPanel();
                    break;
                case (int)Buttons.AddLoyaltyCard:
                    AddLoyaltyCard();
                    break;
                case (int)Buttons.UseLoyaltyCard:
                    customerLoyaltyPanel.UseLoyaltyCard(transaction);
                    break;
                case (int)Buttons.Save:
                    Save();
                    break;
                case (int)Buttons.Close:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }

        private void touchKeyboard_ObtainCultureName(object sender, Controls.EventArguments.CultureEventArguments args)
        {
            if (dlgSettings.UserProfile.KeyboardCode != "")
            {
                args.CultureName = dlgSettings.UserProfile.KeyboardCode;
                args.LayoutName = dlgSettings.UserProfile.KeyboardLayoutName;
            }
            else
            {
                args.CultureName = dlgSettings.Store.KeyboardCode;
                args.LayoutName = dlgSettings.Store.KeyboardLayoutName;
            }
        }

        private void ShowDetailsPanel()
        {
            if(activePanel == null || activePanel != customerDetailsPanel)
            {
                if(customerDetailsPanel == null)
                {
                    customerDetailsPanel = new CustomerDetailsPanel(dlgEntry, Customer);
                    customerDetailsPanel.Location = new Point(0, 0);
                }

                SetPanel(customerDetailsPanel);
            }
        }

        private void ShowOtherPanel()
        {
            if (activePanel == null || activePanel != customerOtherPanel)
            {
                if (customerOtherPanel == null)
                {
                    customerOtherPanel = new CustomerOtherPanel(dlgEntry, Customer);
                    customerOtherPanel.Location = new Point(0, 0);
                }

                SetPanel(customerOtherPanel);
            }
        }

        private void ShowLedgerPanel()
        {
            if (activePanel == null || activePanel != customerLedgerPanel)
            {
                if (customerLedgerPanel == null)
                {
                    customerLedgerPanel = new CustomerLedgerPanel(dlgEntry, Customer);
                    customerLedgerPanel.Location = new Point(0, 0);
                }

                SetPanel(customerLedgerPanel);
            }
        }

        private void ShowLoyaltyPanel()
        {
            if(activePanel == null || activePanel != customerLoyaltyPanel)
            {
                if(customerLoyaltyPanel == null)
                {
                    customerLoyaltyPanel = new CustomerLoyaltyPanel(dlgEntry, Customer, this);
                    customerLoyaltyPanel.Location = new Point(0, 0);
                }

                SetPanel(customerLoyaltyPanel);
            }
        }

        private void SetPanel(Control control)
        {
            control.Size = new Size(pnlControls.Width, pnlControls.Height);
            pnlControls.Controls.Clear();
            pnlControls.Controls.Add(control);
            activePanel = control;
            control.Select();
            control.Invalidate();
        }

        private void Save()
        {
            bool detailsValid = true;
            bool otherValid = true;

            if(customerDetailsPanel != null)
            {
                detailsValid = customerDetailsPanel.ValidateData();
                customerDetailsPanel.CustomerErrorProviderVisible = !detailsValid;
            }

            if (customerOtherPanel != null)
            {
                otherValid = customerOtherPanel.ValidateData();
                customerOtherPanel.CustomerErrorProviderVisible = !otherValid;
            }

            if (detailsValid && otherValid)
            {
                if(customerDetailsPanel != null)
                {
                    Customer.FirstName = customerDetailsPanel.FirstName;
                    Customer.LastName = customerDetailsPanel.LastName;
                    Customer.Prefix = customerDetailsPanel.Title;
                    Customer.Text = customerDetailsPanel.DisplayName;
                    Customer.Email = customerDetailsPanel.Email;
                    Customer.ReceiptEmailAddress = customerDetailsPanel.ReceiptEmailAddress;
                    Customer.Telephone = customerDetailsPanel.Phone;
                    Customer.SearchName = customerDetailsPanel.SearchAlias;
                    Customer.ReceiptSettings = customerDetailsPanel.ReceiptOption;
                }

                if(customerOtherPanel != null)
                {
                    Customer.NonChargableAccount = customerOtherPanel.IsCashCustomer;
                    Customer.Gender = customerOtherPanel.Gender;
                    Customer.Blocked = customerOtherPanel.Blocked;
                    Customer.DateOfBirth = customerOtherPanel.DateOfBirth;
                    Customer.TaxGroup = customerOtherPanel.SalesTaxGroupID;
                    Customer.AccountNumber = customerOtherPanel.InvoiceCustomerID;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                if (!detailsValid)
                    ShowDetailsPanel();

                if (!otherValid)
                    ShowOtherPanel();
            }
        }

        private void AddLoyaltyCard()
        {
            Interfaces.Services.LoyaltyService(dlgEntry).AddCustomerToLoyaltyCard(dlgEntry, transaction, Customer);
            customerLoyaltyPanel.LoadData();
        }
    }
}
