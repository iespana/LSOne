using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Administration.ViewPages
{
    public partial class AdministrationDiscountSettingsPage : ContainerControl, ITabViewV2
    {
        private DiscountCalculation discountCalculation;
        private DiscountParameters discountCalculations;

        private bool CalculationSettingsDirty;
        private bool CalculationsSettingsDirty;

        public AdministrationDiscountSettingsPage()
        {
            InitializeComponent();

            GetDataFromDatabase();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new ViewPages.AdministrationDiscountSettingsPage();
        }

        private void GetDataFromDatabase()
        {
            discountCalculation = Providers.DiscountCalculationData.Get(PluginEntry.DataModel);
            if (discountCalculation == null)
            {
                discountCalculation = new DiscountCalculation();
                CalculationSettingsDirty = true;
            }
            discountCalculations = Providers.DiscountParametersData.Get(PluginEntry.DataModel);
            if (discountCalculations == null)
            {
                discountCalculations = new DiscountParameters();
                CalculationsSettingsDirty = true;
            }
        }

        private void PopulateControls()
        {
            cmbCustomerDisc.SelectedIndex = (int)discountCalculation.CalculateCustomerDiscounts;
            cmbPeriodicDisc.SelectedIndex = (int)discountCalculation.CalculatePeriodicDiscounts;
            cmbDiscounts.SelectedIndex = (int)discountCalculation.DiscountsToApply;
            cmbClearSettings.SelectedIndex = (int)discountCalculation.ClearPeriodicDiscountCache;
            ntbClearAfter.Value = discountCalculation.ClearPeriodicDiscountAfterMinutes;
            cmbClearSettings_SelectedIndexChanged(null, new System.EventArgs());

            cmbPerodicLine.SelectedIndex = cmbPerodicLine2.SelectedIndex = (int)discountCalculations.PeriodicLine;
            cmbPerodicTotal.SelectedIndex = cmbPerodicTotal2.SelectedIndex = (int)discountCalculations.PeriodicTotal;
            cmbLineTotal.SelectedIndex = cmbLineTotal2.SelectedIndex = (int)discountCalculations.LineTotal;
        }

        #region ITabPanel Members

        public void InitializeView(RecordIdentifier context, object internalContext)
        {

        }
        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            PopulateControls();
        }

        private bool CalculationSettingsIsDirty()                     
        {
            if (cmbCustomerDisc.SelectedIndex != (int)discountCalculation.CalculateCustomerDiscounts) return true;
            if (cmbPeriodicDisc.SelectedIndex != (int)discountCalculation.CalculatePeriodicDiscounts) return true;
            if (cmbDiscounts.SelectedIndex != (int)discountCalculation.DiscountsToApply) return true;
            if (cmbClearSettings.SelectedIndex != (int)discountCalculation.ClearPeriodicDiscountCache) return true;
            if (ntbClearAfter.Enabled && ntbClearAfter.Value != discountCalculation.ClearPeriodicDiscountAfterMinutes) return true;

            return false;
        }

        private bool CalculationsSettingsIsDirty()
        {
            if (cmbPerodicLine.SelectedIndex != (int)discountCalculations.PeriodicLine) return true;
            if (cmbPerodicTotal.SelectedIndex != (int)discountCalculations.PeriodicTotal) return true;
            if (cmbLineTotal.SelectedIndex != (int) discountCalculations.LineTotal) return true;

            return false;
        }

        public bool DataIsModified()
        {
            CalculationSettingsDirty = CalculationSettingsDirty || CalculationSettingsIsDirty();
            CalculationsSettingsDirty = CalculationSettingsIsDirty() || CalculationsSettingsIsDirty();

            return CalculationSettingsDirty || CalculationsSettingsDirty;
        }

        public bool SaveData()
        {
            if (CalculationSettingsDirty)
            {
                discountCalculation.CalculateCustomerDiscounts = (CalculateCustomerDiscountEnums)cmbCustomerDisc.SelectedIndex;
                discountCalculation.CalculatePeriodicDiscounts = (CalculatePeriodicDiscountEnums)cmbPeriodicDisc.SelectedIndex;
                discountCalculation.DiscountsToApply = (LineDiscCalculationTypes)cmbDiscounts.SelectedIndex;
                discountCalculation.ClearPeriodicDiscountCache = (ClearPeriodicDiscountCacheEnum)cmbClearSettings.SelectedIndex;
                if (ntbClearAfter.Enabled)
                {
                    discountCalculation.ClearPeriodicDiscountAfterMinutes = (int)ntbClearAfter.Value;
                }

                Providers.DiscountCalculationData.Save(PluginEntry.DataModel, discountCalculation);

            }
            if (CalculationsSettingsDirty)
            {
                discountCalculations.PeriodicLine = (PeriodicLineEnum) cmbPerodicLine.SelectedIndex;
                discountCalculations.PeriodicTotal = (PeriodicTotalEnum) cmbPerodicTotal.SelectedIndex;
                discountCalculations.LineTotal = (CustomerLineTotalEnum) cmbLineTotal.SelectedIndex;

                Providers.DiscountParametersData.Save(PluginEntry.DataModel, discountCalculations);
            }
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("AdministrationDiscountAndPriceActivationPage", 1, Properties.Resources.ActivePriceDiscount, true));
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
           
        }

        public void DataUpdated(RecordIdentifier context, object internalContext)
        {
            GetDataFromDatabase();
            PopulateControls();
        }

        public void OnClose()
        {
            
        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void cmbLineTotal_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cmbLineTotal2.SelectedIndex = cmbLineTotal.SelectedIndex;
        }

        private void cmbPerodicLine_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cmbPerodicLine2.SelectedIndex = cmbPerodicLine.SelectedIndex;
        }

        private void cmbPerodicTotal_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            cmbPerodicTotal2.SelectedIndex = cmbPerodicTotal.SelectedIndex;
        }

        private void cmbClearSettings_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ntbClearAfter.Enabled = lblMinutes.Enabled = cmbClearSettings.SelectedIndex != 0;
        }
    }
}
