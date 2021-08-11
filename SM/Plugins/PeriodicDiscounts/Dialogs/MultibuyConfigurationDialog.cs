using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class MultibuyConfigurationDialog : DialogBase
    {
        Dictionary<decimal, MultibuyDiscountLine> currentLines;
        MultibuyDiscountLine currentRecord;

        public MultibuyConfigurationDialog(MultibuyDiscountLine line,DiscountOffer.PeriodicDiscountDiscountTypeEnum discountType, Dictionary<decimal, MultibuyDiscountLine> currentLines)
            : this(discountType,currentLines)
        {
            DecimalLimit limiter;

            currentRecord = line;

            if (discountType == DiscountOffer.PeriodicDiscountDiscountTypeEnum.DiscountPercent)
            {
                limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);
            }
            else
            {
                limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            }

            ntbPriceDiscount.Text = line.PriceOrDiscountPercent.FormatWithLimits(limiter);

            limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            ntbMinQty.Text = ((decimal)line.MinQuantity).FormatWithLimits(limiter);

        }

        public MultibuyConfigurationDialog(DiscountOffer.PeriodicDiscountDiscountTypeEnum discountType, Dictionary<decimal, MultibuyDiscountLine> currentLines)
            : this()
        {
            DecimalLimit limiter;

            this.currentLines = currentLines;

            if (discountType == DiscountOffer.PeriodicDiscountDiscountTypeEnum.DiscountPercent)
            {
                lblPriceDiscount.Text = Properties.Resources.DiscountPercent + ":";
                Header = Properties.Resources.MBConfigHeaderPercent;

                limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.DiscountPercent);
            }
            else
            {
                limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Prices);
            }

            ntbPriceDiscount.DecimalLetters = limiter.Max;

            limiter = PluginEntry.DataModel.GetDecimalSetting(DecimalSettingEnum.Quantity);

            ntbMinQty.DecimalLetters = limiter.Max;
        }

        public MultibuyConfigurationDialog()
            : base()
        {
            currentRecord = null;

            InitializeComponent();

        }

        public MultibuyDiscountLine DiscountLine
        {
            get
            {
                return currentRecord;
            }
        }

 
        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }



        private void btnOK_Click(object sender, EventArgs e)
        {
            bool needsToCheck;

            if (currentRecord != null)
            {
                needsToCheck = (decimal)ntbMinQty.Value != currentRecord.MinQuantity;
            }
            else
            {
                needsToCheck = true;
            }

            if (needsToCheck && currentLines.ContainsKey((decimal)ntbMinQty.Value))
            {
                errorProvider1.SetError(ntbMinQty,Properties.Resources.ThatMinimumQuantityExists);
                return;
            }

            if (currentRecord == null)
            {
                currentRecord = new MultibuyDiscountLine();
            }

            currentRecord.MinQuantity = (decimal)ntbMinQty.Value;
            currentRecord.PriceOrDiscountPercent = (decimal)ntbPriceDiscount.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            

            DialogResult = DialogResult.Cancel;
            Close();
        }

        

        private void CheckEnabled(object sender, EventArgs e)
        {
            errorProvider1.Clear();

            if (currentRecord != null)
            {
                btnOK.Enabled = (decimal)ntbMinQty.Value != currentRecord.MinQuantity ||
                                (decimal)ntbPriceDiscount.Value != currentRecord.PriceOrDiscountPercent;
            }
            else
            {
                btnOK.Enabled = true;
            }
        }

   
    }
}
