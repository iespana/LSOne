using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.StoreManagement;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.USConfigurations.ViewPages
{
    public partial class StorePriceTaxCalculationsPage : UserControl, ITabView
    {
        Store store;

        public StorePriceTaxCalculationsPage()
        {
            InitializeComponent();         
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.StorePriceTaxCalculationsPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            store = (Store)internalContext;

            chkKeyedPriceInclTax.Checked = store.KeyedInPriceIncludesTax;
            chkDisplayAmountsWithTax.Checked = store.DisplayAmountsWithTax;
            chkUseTaxRounding.Checked = store.UseTaxRounding;
            chkDisplayBalanceWithTax.Checked = store.DisplayBalanceWithTax;
            cmbCalculateDiscountsFrom.SelectedIndex = (int)store.CalculateDiscountsFrom;
            cmbUseTaxGroupFrom.SelectedIndex = (int)store.UseTaxGroupFrom;

        }

        public bool DataIsModified()
        {
            if (chkKeyedPriceInclTax.Checked != store.KeyedInPriceIncludesTax) return true;
            if (chkDisplayAmountsWithTax.Checked != store.DisplayAmountsWithTax) return true;
            if (chkUseTaxRounding.Checked != store.UseTaxRounding) return true;
            if (chkDisplayBalanceWithTax.Checked != store.DisplayBalanceWithTax) return true;
            if (cmbCalculateDiscountsFrom.SelectedIndex != (int)store.CalculateDiscountsFrom) return true;
            if (cmbUseTaxGroupFrom.SelectedIndex != (int)store.UseTaxGroupFrom) return true;

            return false;
        }

        public bool SaveData()
        {
            store.KeyedInPriceIncludesTax = chkKeyedPriceInclTax.Checked;
            store.CalculateDiscountsFrom = (Store.CalculateDiscountsFromEnum)cmbCalculateDiscountsFrom.SelectedIndex;
            store.DisplayAmountsWithTax = chkDisplayAmountsWithTax.Checked;
            store.DisplayBalanceWithTax = chkDisplayBalanceWithTax.Checked;
            store.UseTaxRounding = chkUseTaxRounding.Checked;
            store.UseTaxGroupFrom = (UseTaxGroupFromEnum)cmbUseTaxGroupFrom.SelectedIndex;

            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if (changeHint == DataEntityChangeType.VariableChanged)
            {
                if (objectName == "PriceTaxSettingsUpdate")
                {
                    if (param != null && (store.StorePriceSetting != (Store.StorePriceSettingsEnum)param))
                    {
                        store.StorePriceSetting = (Store.StorePriceSettingsEnum)param;
                        CheckPriceAndDiscountSettings(false);
                    }
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

        private void cmbCalculateDiscountsFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkDisplayAmountsWithTax.Checked = (cmbCalculateDiscountsFrom.SelectedIndex == 0);

            CheckPriceAndDiscountSettings(true);            
        }

        private void CheckPriceAndDiscountSettings(bool doNotify)
        {
            errorProvider.Clear();

            if (cmbCalculateDiscountsFrom.SelectedIndex == (int)Store.CalculateDiscountsFromEnum.Price &&
                store.StorePriceSetting != Store.StorePriceSettingsEnum.PricesExcludeTax)
            {
                errorProvider.SetError(cmbCalculateDiscountsFrom, Properties.Resources.PriceSettingsAndCalculations);                
            }

            else if (cmbCalculateDiscountsFrom.SelectedIndex == (int)Store.CalculateDiscountsFromEnum.PriceWithTax &&
                store.StorePriceSetting != Store.StorePriceSettingsEnum.PricesIncludeTax)
            {
                errorProvider.SetError(cmbCalculateDiscountsFrom, Properties.Resources.PriceSettingsAndCalculations);                
            }
            

            if (doNotify)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.VariableChanged, "CalculateDiscountsUpdate", RecordIdentifier.Empty, cmbCalculateDiscountsFrom.SelectedIndex);
            }
        }     

    }
}
