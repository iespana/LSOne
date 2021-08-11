using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.BarCodes.Properties;

namespace LSOne.ViewPlugins.BarCodes.DialogPages
{
    public partial class NewRetailItemBarCodePage : System.Windows.Forms.UserControl, IDialogTabViewWithRequiredFields, IMessageTabExtension
    {
        

        public event EventHandler RequiredInputValidate;
        bool hasBarcodePermission;
        RetailItem retailItem;
        private BarCodeSetup barCodeSetup;
        private bool barCodeValid;

        public NewRetailItemBarCodePage()
        {
            InitializeComponent();
            hasBarcodePermission = PluginEntry.DataModel.HasPermission(Permission.ManageItemBarcodes);
            cmbBarCodeSetup.SelectedData = new DataEntity("", "");
            cmbBarCodeSetup.Enabled = hasBarcodePermission;
            tbBarCode.Enabled = hasBarcodePermission;
            linkBarCode.Enabled = hasBarcodePermission;
            lblBarCodeSetup.Enabled = hasBarcodePermission;
            lblDefaultBarCode.Enabled = hasBarcodePermission;
            barCodeValid = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            cmbBarCodeSetup.Select();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new NewRetailItemBarCodePage();
        }

        public bool DataIsModified()
        {
            // This will never be called since we are on dialog
            return false;
        }

        public void GetAuditDescriptors(List<ViewCore.AuditDescriptor> contexts)
        {
            // This will never be called since we are on dialog
        }

        public void LoadData(bool isRevert, Utilities.DataTypes.RecordIdentifier context, object internalContext)
        {
            retailItem = (RetailItem) internalContext;
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(ViewCore.Enums.DataEntityChangeType changeHint, string objectName, Utilities.DataTypes.RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveSecondaryRecords()
        {
            // Here would be the right place to save to Barcode table since at this point we have ID in the main record

            if (tbBarCode.Text.Trim() != "")
            {
                BarCode barCode = new BarCode();
                barCode.ItemID = retailItem.ID;
                barCode.ItemBarCode = tbBarCode.Text;
                barCode.UnitID = "";

                barCode.BarCodeSetupID = cmbBarCodeSetup.SelectedData.ID;
                barCode.ShowForItem = true;
                barCode.UseForInput = false;
                barCode.UseForPrinting = false;
                barCode.Quantity = 0;

                Providers.BarCodeData.Save(PluginEntry.DataModel, barCode);
            }
        }

        public void SaveUserInterface()
        {
            
        }

        void IDialogTabViewWithRequiredFields.RequiredFieldsAreValid(FieldValidationArguments args)
        {
            if (barCodeValid)
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.Valid;
            }
            else
            {
                args.Result = FieldValidationArguments.FieldValidationEnum.OtherInvalid;
                args.ResultDescription = Resources.BarcodeNotCorrectlySetup;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if(RequiredInputValidate != null)
            {
                RequiredInputValidate(this, EventArgs.Empty);
            }
        }

        private void cmbBarCodeSetup_RequestData(object sender, EventArgs e)
        {
            cmbBarCodeSetup.SetData(Providers.BarCodeSetupData.GetList(PluginEntry.DataModel),
                null);
        }

        private void cmbBarCodeSetup_SelectedDataChanged(object sender, EventArgs e)
        {
            barCodeSetup = Providers.BarCodeSetupData.Get(PluginEntry.DataModel, cmbBarCodeSetup.SelectedData.ID);
            barCodeValid = validateBarCode();
            CheckEnabled(this, EventArgs.Empty);
        }

        private void cmbBarCodeSetup_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
            barCodeSetup = null;
            tbBarCode.Clear();
            barCodeValid = validateBarCode();
            CheckEnabled(this, EventArgs.Empty);
        }

        private void tbBarCode_TextChanged(object sender, EventArgs e)
        {
            barCodeValid = validateBarCode();
            CheckEnabled(this, EventArgs.Empty);
        }

        private bool validateBarCode()
        {
            if (barCodeSetup != null && barCodeSetup.BarCodeMask.Length != barCodeSetup.BarCodeMask.Count(c => c == 'X'))
            {
                if (barCodeSetup.MinimumLength > 0)
                {
                    if (tbBarCode.Text.Length < barCodeSetup.MinimumLength)
                    {
                        errorProvider1.SetError(linkBarCode, Properties.Resources.BarCodeMinLengthError.Replace("#1", barCodeSetup.MinimumLength.ToString()));
                        return false;
                    }
                    
                }
                if (barCodeSetup.MaximumLength > 0)
                {
                    if (tbBarCode.Text.Length > barCodeSetup.MaximumLength)
                    {
                        errorProvider1.SetError(linkBarCode, Properties.Resources.BarCodeMaxLengthError.Replace("#1", barCodeSetup.MaximumLength.ToString()));
                        return false;
                    }
                }
            }
            else if (tbBarCode.Text != "" && barCodeSetup == null)
            {
                errorProvider1.SetError(linkBarCode, Resources.PleaseSelectBarCodeSetup);
                return false;
            }

            if(barCodeSetup != null && tbBarCode.Text != "")
            {
                if(Providers.BarCodeData.Exists(PluginEntry.DataModel, tbBarCode.Text))
                {
                    errorProvider1.SetError(linkBarCode, Resources.BarCodeExists);
                    return false;
                }
            }

            errorProvider1.Clear();
            return true;
        }

        public object OnViewPageMessage(object sender, string message, object param, ref bool handled)
        {
            switch (message)
            {
                case "CreateAnother":
                    tbBarCode.Text = "";
                    barCodeValid = true;
                    errorProvider1.Clear();
                    return null;
            }
            return null;
        }
    }
}
