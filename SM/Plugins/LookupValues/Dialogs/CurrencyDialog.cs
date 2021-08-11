using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Currencies;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Currencies;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.LookupValues.Dialogs
{
    public partial class CurrencyDialog : DialogBase
    {
        RecordIdentifier currencyID;
        private RecordIdentifier recordIdentifier;
        private Currency currInfoExisting; 


        public CurrencyDialog()
        {
            InitializeComponent();
            
            tbID.Enabled = true;
            ntbRoundoffSales.Value = 0.01;            
            ntbRoundoffPurch.Value = 0.01;
            ntbRoundoffAmt.Value = 0.01;
            
            cmbRoundoffTypeAmt.SelectedIndex = 0;
            cmbRoundoffTypePurch.SelectedIndex = 0;
            cmbRoundoffTypeSales.SelectedIndex = 0;            
        }

        public CurrencyDialog(RecordIdentifier recordIdentifier) :this()
        {           
            label18.Visible = false;
            tbID.ReadOnly = true;
            this.recordIdentifier = recordIdentifier;
            currInfoExisting = Providers.CurrencyData.Get(PluginEntry.DataModel, recordIdentifier);
            
            tbID.Text = recordIdentifier.ToString().Trim();
            tbDescription.Text = currInfoExisting.Text.ToString();
            tbPrefix.Text = currInfoExisting.CurrencyPrefix.ToString();
            tbSuffix.Text = currInfoExisting.CurrencySuffix.ToString();

            cmbRoundoffTypeAmt.SelectedIndex = (int)currInfoExisting.RoundOffTypeAmount;
            cmbRoundoffTypePurch.SelectedIndex = (int)currInfoExisting.RoundOffTypePurchase;
            cmbRoundoffTypeSales.SelectedIndex = (int)currInfoExisting.RoundOffTypeSales;


            ntbRoundoffSales.SetValueWithLimit(currInfoExisting.RoundOffSales, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
            ntbRoundoffPurch.SetValueWithLimit(currInfoExisting.RoundOffPurchase, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));
            ntbRoundoffAmt.SetValueWithLimit(currInfoExisting.RoundOffAmount, PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices));

        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        } 

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {       

            Currency currencyInfo;

            if (currInfoExisting == null)  //then we have a new record set
            {
                if (Providers.CurrencyData.Exists(PluginEntry.DataModel, tbID.Text.Trim()))
                {
                    errorProvider1.SetError(tbID, Properties.Resources.CurrencyExists);
                    tbID.Focus();
                    return;
                }
                currencyInfo = new Currency(tbID.Text.Trim(), tbDescription.Text.Trim());
            }
            else
            {
                currencyInfo = currInfoExisting;
                currencyInfo.Text = tbDescription.Text.Trim();
            }

            currencyInfo.RoundOffAmount = (decimal)ntbRoundoffAmt.Value;
            currencyInfo.RoundOffSales = (decimal)ntbRoundoffSales.Value;
            currencyInfo.RoundOffPurchase = (decimal)ntbRoundoffPurch.Value;

            currencyInfo.RoundOffTypeAmount = Convert.ToInt16(cmbRoundoffTypeAmt.SelectedIndex);
            currencyInfo.RoundOffTypeSales = Convert.ToInt16(cmbRoundoffTypeSales.SelectedIndex);
            currencyInfo.RoundOffTypePurchase = Convert.ToInt16(cmbRoundoffTypePurch.SelectedIndex);

            currencyInfo.CurrencyPrefix = tbPrefix.Text;
            currencyInfo.CurrencySuffix = tbSuffix.Text;
            currencyInfo.Symbol = string.IsNullOrEmpty(tbPrefix.Text) ? tbSuffix.Text : tbPrefix.Text;
                
            Providers.CurrencyData.Save(PluginEntry.DataModel, currencyInfo);

            currencyID = (string)currencyInfo.ID;  //used to notify other components on the stack
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs e)
        {

            if (currInfoExisting == null)  // then we have a new currency
            {
                btnOK.Enabled = PerformCheckRoutines();  
            }
            else        // edit an existing currency
            {
                bool txtIsNotEmpty = tbDescription.Text != "";
                bool txtHasChanged = (tbDescription.Text != currInfoExisting.Text);
                bool prefixHasChanged = (tbPrefix.Text != currInfoExisting.CurrencyPrefix);
                bool suffixHasChanged = (tbSuffix.Text != currInfoExisting.CurrencySuffix);
                bool roundOffAmtHasChanged = ((decimal)ntbRoundoffAmt.Value != currInfoExisting.RoundOffAmount);
                bool roundOffTypeAmtHasChanged = (cmbRoundoffTypeAmt.SelectedIndex != (int)currInfoExisting.RoundOffTypeAmount);               
                bool roundOffPurchHasChanged = ((decimal)ntbRoundoffPurch.Value != currInfoExisting.RoundOffPurchase);
                bool roundOffTypePurchHasChanged = (cmbRoundoffTypePurch.SelectedIndex != (int)currInfoExisting.RoundOffTypePurchase);
                bool roundOffSalesHasChanged = ((decimal)ntbRoundoffSales.Value != currInfoExisting.RoundOffSales);
                bool roundOffTypeSalesHasChanged = (cmbRoundoffTypeSales.SelectedIndex != (int)currInfoExisting.RoundOffTypeSales);

                btnOK.Enabled = false;

                if (PerformCheckRoutines())
                {

                    btnOK.Enabled = txtIsNotEmpty || txtHasChanged || prefixHasChanged || suffixHasChanged || roundOffAmtHasChanged || roundOffTypeAmtHasChanged
                           || roundOffPurchHasChanged || roundOffTypePurchHasChanged || roundOffSalesHasChanged || roundOffTypeSalesHasChanged;
                }
            }
        }

        private bool PerformCheckRoutines()
        {
            errorProvider1.Clear();
            UpdatePreviewLable();

            if (
                    tbID.Text.Length > 2
                    && tbDescription.Text != ""
                    && ntbRoundoffAmt.Value > 0
                    && ntbRoundoffPurch.Value > 0
                    && ntbRoundoffSales.Value > 0
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }        

        private void UpdatePreviewLable()
        {
            lblPreviewResult.Text = tbPrefix.Text + "10 " + tbSuffix.Text;
        }

        private bool CheckPreOrSuffix()
        {
            if (tbPrefix.Text != "")
            {
                if (tbSuffix.Text != "")
                {
                    errorProvider1.SetError(tbSuffix, Properties.Resources.NotBothPreAndSuffixAllowed);
                    lblPreviewResult.Text = "";
                    return false;
                }
                    
                return true;
            }
            else 
            {
                if (tbSuffix.Text != "")
                {
                    return true;
                }
                else
                {
                    //errorProvider1.SetError(tbPrefix, Properties.Resources.PreOrSuffixNeeded);
                    lblPreviewResult.Text = "";
                    return false;
                }
            }            
        }

        private void tbID_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckEnabled(this, EventArgs.Empty);
        }

        public RecordIdentifier CurrencyID
        {
            get
            {
                return currencyID;
            }
        }

        private void BoundariesChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
        }          

        private void tbPrefix_TextChanged(object sender, EventArgs e)
        {           
            CheckEnabled(sender, e);
        }

        private void tbSuffix_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(sender, e);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }
      
    }
}
