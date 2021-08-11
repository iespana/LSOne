using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Dialogs.Interfaces;
using LSOne.ViewCore.Dialogs;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.DataLayer.BusinessObjects.StoreManagement.Validity;
using LSOne.Utilities.DataTypes;

namespace LSOne.ViewPlugins.RMSMigration.Dialogs.WizardPages
{

    public partial class DataIntegrityTestPanel : UserControl, IWizardPage
    {
        WizardBase parent;

        public DataIntegrityTestPanel(WizardBase parent)
            : this()
        {
            this.parent = parent;
            Errors = new List<string>();
        }

        private DataIntegrityTestPanel()
        {
            InitializeComponent();
        }


        #region Properties

        public List<string> Errors { get; set; }

        #endregion

        #region IWizardPage Members
        public bool HasFinish
        {
            get { return false; }
        }

        public bool HasForward
        {
            get { return true; }
        }

        public Control PanelControl
        {
            get { return this; }
        }

        public void Display()
        {
            CheckLSOneDataIntegrity();
        }

        public bool NextButtonClick(ref bool canUseFromForwardStack)
        {
            canUseFromForwardStack = false;
            return true;
        }

        public IWizardPage RequestNextPage()
        {
            return new DatabaseConnectionPanel(parent);
        }

        public void ResetControls()
        {

        }

        #endregion

        private void DataIntegrityTestPanel_Load(object sender, EventArgs e)
        {
            
        }

        private void ShowErrors()
        {
            if (Errors == null || Errors.Count == 0)
            {
                return;
            }

            lblErrorInfo.Visible = true;
            int i = 0;
            Errors.ForEach(error =>
            {
                Label lblError = new Label();
                lblError.Text = error;
                lblError.ForeColor = System.Drawing.Color.Black;

                lblError.BorderStyle = BorderStyle.FixedSingle;
                lblError.BackColor = System.Drawing.Color.White;
                lblError.Top = 130 + (90 * (i++));
                lblError.Left = 130;
                lblError.Width = 400;
                lblError.Height = 80;
                lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                this.Controls.Add(lblError);
            });

        }

        private void CheckLSOneDataIntegrity()
        {
            IConnectionManager connection = parent.Connection;
            bool hasCompanyCurrency = Providers.CompanyInfoData.HasCompanyCurrency(connection);
            bool hasBarcodeSetup = Providers.BarCodeSetupData.GetList(connection).Count > 0;

            if (!hasCompanyCurrency)
            {
                Errors.Add(Properties.Resources.CompanyCurrencyIsMissing + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.Company));
                ShowErrors();
            }
            else
            {
                bool hasError = false;
                List<StoreValidity> storeValidities = Providers.StoreData.CheckStoreValidity(connection);

                if (storeValidities.Count == 0)
                {
                    Errors.Add(Properties.Resources.NoStoreExists + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.Store));
                    hasError = true;
                }
                else
                {
                    foreach (StoreValidity storeValidity in storeValidities)
                    {
                        if (!storeValidity.TerminalExists)
                        {
                            Errors.Add(Properties.Resources.StoreNamedHasNoTerminal.Replace("#1", storeValidity.Text) + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.Store));
                            hasError = true;
                            break;
                        }
                        else if (!storeValidity.ButtonLayoutExists)
                        {
                            Errors.Add(Properties.Resources.StoreNamedHasNoTouchButtons.Replace("#1", storeValidity.Text) + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.Store));
                            hasError = true;
                            break;
                        }
                        else if (!storeValidity.FunctionalityProfileExist)
                        {
                            Errors.Add(Properties.Resources.StoreNamedHasNoFunctionalityProfile.Replace("#1", storeValidity.Text) + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.Store));
                            hasError = true;
                            break;
                        }
                    }
                }

                var defaultStoreSalesTaxGroup = Providers.StoreData.GetDefaultStoreSalesTaxGroup(connection);
                if (defaultStoreSalesTaxGroup.StringValue == string.Empty)
                {
                    Errors.Add(Properties.Resources.StoreMissingSalesTaxGroup + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.Store));
                    hasError = true;
                }

                if (!hasError)
                {
                    List<TerminalValidity> terminalValidities = Providers.TerminalData.CheckTerminalValidity(connection);

                    foreach (TerminalValidity terminalValidity in terminalValidities)
                    {
                        if (!terminalValidity.VisualProfileExists)
                        {
                            Errors.Add(Properties.Resources.TerminalNamedHasNoVisualProfile.Replace("#1", terminalValidity.Text) + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.Terminal));
                            hasError = true;
                            break;
                        }

                        if (!terminalValidity.HardwareProfileExists)
                        {
                            Errors.Add(Properties.Resources.TerminalNamedHasNoHardwareProfile.Replace("#1", terminalValidity.Text) + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.Terminal));
                            hasError = true;
                            break;
                        }
                    }
                }

                if (!hasBarcodeSetup)
                {
                    Errors.Add(Properties.Resources.NoSetupBarCodeExists + " " + Properties.Resources.FinishConfigureInformation.Replace("#1", Properties.Resources.BarCodeSetup));
                    ShowErrors();
                    return;
                }

                if (!hasError)
                {
                    lblNoErrors.Visible = true;
                    parent.NextEnabled = true;
                }
                else
                {
                    ShowErrors();
                }
            }
        }
    }
}
