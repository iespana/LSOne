using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.BusinessObjects.Companies;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.Inventory;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.DataLayer.BusinessObjects.Units;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Inventory;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.SerialNumbers.Properties;
using LSOne.DataLayer.BusinessObjects.SerialNumbers;

namespace LSOne.ViewPlugins.SerialNumbers.Dialogs
{

    public partial class SerialNumberDialog : DialogBase
    {

        private RecordIdentifier selectedSerialNumber = RecordIdentifier.Empty;
        private RecordIdentifier itemRecId;
        private RetailItem retailItem;
        private BarCode barCode;

        private string itemId = "";
        private string itemText = "";
        private bool lockEvent = false;

        private bool edit = false;
        private SerialNumber editSerialNumber;

        private SerialNumberDialog()
        {
            InitializeComponent();
            cmbRelation.SelectedData = new DataEntity("", "");
            tbBarcode.Tag = ControlTypeEnums.BarcodeSearch;
        }

        public SerialNumberDialog(RecordIdentifier selectedSerialNumber)
            : this()
        {
            this.selectedSerialNumber = selectedSerialNumber;
            btnOK.Enabled = false;

            cmbType.Items.Add(new { Text = SerialNumber.GetTypeOfSerialString(TypeOfSerial.SerialNumber), Value = (int)TypeOfSerial.SerialNumber });
            cmbType.Items.Add(new { Text = SerialNumber.GetTypeOfSerialString(TypeOfSerial.RFIDTag), Value = (int)TypeOfSerial.RFIDTag });
            cmbType.DisplayMember = "Text";
            cmbType.ValueMember = "Value";

            if (selectedSerialNumber == RecordIdentifier.Empty)
            {
                edit = false;
                cmbType.SelectedIndex = 0;
            }
            else
            {
                editSerialNumber = Providers.SerialNumberData.Get(PluginEntry.DataModel, selectedSerialNumber);
                RecordIdentifier itemID = Providers.RetailItemData.GetItemIDFromMasterID(PluginEntry.DataModel, editSerialNumber.ItemMasterID);
                if (editSerialNumber.SerialType == TypeOfSerial.SerialNumber)
                {
                    cmbType.SelectedIndex = 0;
                }
                else
                {
                    cmbType.SelectedIndex = 1;
                }
                tbBarcode.Enabled = false;
                cmbRelation.Enabled = false;
                cbCreateAnother.Checked = false;
                cbCreateAnother.Visible = false;
                cmbVariantNumber.Enabled = false;
                edit = true;
                tbSerialNumber.Text = editSerialNumber.SerialNo;

                cmbRelation.Text = editSerialNumber.ItemDescription;
                cmbVariantNumber.Text = editSerialNumber.ItemVariant;
                BarCode barcode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, itemID);
                if (barcode != null)
                {
                    tbBarcode.Text = barcode.ItemBarCode.ToString();
                }
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
        {
            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbRelation.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailItems, textInitallyHighlighted, null, true);
        }

        private void cmbRelation_FormatData(object sender, DropDownFormatDataArgs e)
        {
            if (((DataEntity)e.Data).ID == "")
            {
                e.TextToDisplay = "";
            }
            else
            {
                itemRecId = ((DataEntity)e.Data).ID;
                itemId = (string)((DataEntity)e.Data).ID;
                itemText = ((DataEntity)e.Data).Text;
                e.TextToDisplay = itemText;
            }

            CheckEnabled();
        }

        private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
        {

            if (cmbRelation.SelectedData.ID != "")
            {
                lblVariantNumber.Enabled = cmbVariantNumber.Enabled = false;

                cmbVariantNumber.SelectedData = new DataEntity();

                retailItem = PluginOperations.GetRetailItem(cmbRelation.SelectedData.ID);
                if (retailItem == null)
                {
                    return;
                }

                itemText = retailItem.Text;

                if (sender is DualDataComboBox || e == null)
                {
                    BarCode barCode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, retailItem.ID);
                    if (barCode != null)
                    {
                        tbBarcode.Text = (string)barCode.ItemBarCode;
                    }
                    else
                    {
                        tbBarcode.Text = "";
                    }
                }
                else if (sender is TextBox)
                {
                    cmbRelation.SelectedData.ID = retailItem.ID;
                    cmbRelation.Text = retailItem.Text;
                    if (retailItem.ItemType == ItemTypeEnum.Item && retailItem.VariantName != "")
                    {
                        cmbVariantNumber.SelectedData.ID = retailItem.ID;
                        cmbVariantNumber.Text = retailItem.VariantName;
                        lblVariantNumber.Enabled = cmbVariantNumber.Enabled = true;
                        cmbVariantNumber_SelectedDataChanged(sender, e);
                    }
                }

                if (retailItem.ItemType == ItemTypeEnum.MasterItem)
                {
                    lblVariantNumber.Enabled = cmbVariantNumber.Enabled = true;
                }
            }

            CheckEnabled();
        }


        private void cmbVariantNumber_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbVariantNumber.SelectedData.ID != "")
            {
                retailItem = PluginOperations.GetRetailItem(cmbVariantNumber.SelectedData.ID);
                if (retailItem == null)
                {
                    return;
                }

                UpdateBarCode(retailItem);
            }

            CheckEnabled();
            if (!btnOK.Enabled)
            {
                tbSerialNumber.Focus();
            }
        }

        private void UpdateBarCode(RetailItem retailItem)
        {
            barCode = Providers.BarCodeData.GetBarCodeForItem(PluginEntry.DataModel, retailItem.ID);
            if (barCode != null)
            {
                tbBarcode.Text = (string)barCode.ItemBarCode;
            }
            else
            {
                tbBarcode.Text = "";
            }
        }

        private void CheckEnabled()
        {
            errorProvider1.Clear();
            bool enabled = true;

            if (!edit)
            {
                enabled = cmbRelation.Text != "";
                enabled = enabled && (
                    (
                        cmbVariantNumber.Enabled
                        && !RecordIdentifier.IsEmptyOrNull(cmbVariantNumber.SelectedData.ID.PrimaryID)
                        && cmbVariantNumber.SelectedData.ID.PrimaryID != ""
                    )
                    || (
                        !cmbVariantNumber.Enabled
                        && RecordIdentifier.IsEmptyOrNull(cmbVariantNumber.SelectedData.ID)
                        )
                    );
            }
            enabled = enabled && !string.IsNullOrWhiteSpace(tbSerialNumber.Text);

            btnOK.Enabled = enabled;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SerialNumber serialNumber = new SerialNumber();

            if (edit)
            {
                serialNumber = editSerialNumber;
                SerialNumber contextSerialNumber = Providers.SerialNumberData.Get(PluginEntry.DataModel, serialNumber.ID);
                if (contextSerialNumber.Reserved || contextSerialNumber.UsedDate.HasValue)
                {
                    MessageDialog.Show(Resources.SerialNumberReserved);
                    return;
                }
            }
            else
            {
                serialNumber.ID = selectedSerialNumber;
                RecordIdentifier target;
                if (cmbVariantNumber.SelectedData.ID != RecordIdentifier.Empty)
                {
                    if (cmbVariantNumber.SelectedData is MasterIDEntity)
                    {
                        target = ((MasterIDEntity)cmbVariantNumber.SelectedData).ReadadbleID;
                    }
                    else
                    {
                        target = (cmbVariantNumber.SelectedData as DataEntity).ID;
                    }

                }
                else
                {
                    target = itemId;
                }
                serialNumber.ItemMasterID = Providers.RetailItemData.Get(PluginEntry.DataModel, target).MasterID;
            }

            SerialNumber sn = Providers.SerialNumberData.GetByItemAndSerialNumber(PluginEntry.DataModel, serialNumber.ItemMasterID, tbSerialNumber.Text);

            if ((!edit && sn != null) || (edit && sn != null && sn.ID != serialNumber.ID))
            {
                errorProvider1.SetError(tbSerialNumber, Resources.SerialNumberAlreadyExists);
                return;
            }

            serialNumber.SerialNo = tbSerialNumber.Text;
            serialNumber.SerialType = cmbType.SelectedIndex == 0 ? TypeOfSerial.SerialNumber : TypeOfSerial.RFIDTag;

            Providers.SerialNumberData.Save(PluginEntry.DataModel, serialNumber, false);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "SerialNumbers", serialNumber.ID, serialNumber);

            if (cbCreateAnother.Checked)
            {
                SetDefaults();
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbVariantNumber_DropDown(object sender, DropDownEventArgs e)
        {
            List<RecordIdentifier> excludedItemIDs = new List<RecordIdentifier>();

            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = ((DataEntity)cmbVariantNumber.SelectedData).Text;
                textInitallyHighlighted = true;
            }
            e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                retailItem.ItemType == ItemTypeEnum.MasterItem ?
                retailItem.MasterID :
                retailItem.HeaderItemID,
                true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs, textInitallyHighlighted, true);
        }

        private void tbBarcode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CheckBarCode();
            }
        }

        private void tbBarcode_Leave(object sender, EventArgs e)
        {
            if (!lockEvent)
            {
                CheckBarCode();
            }
            lockEvent = false;
        }

        private void CheckBarCode()
        {
            if (string.IsNullOrWhiteSpace(tbBarcode.Text))
            {
                return;
            }

            RecordIdentifier ItemID = RecordIdentifier.Empty;
            IBarcodeService barcodeService = (IBarcodeService)PluginEntry.DataModel.Service(ServiceType.BarcodeService);
            if (barcodeService != null)
            {
                BarCode barCode = barcodeService.ProcessBarcode(PluginEntry.DataModel,
                    BarCode.BarcodeEntryType.ManuallyEntered, tbBarcode.Text);
                if (barCode != null && barCode.InternalType == BarcodeInternalType.Item)
                {
                    ItemID = barCode.ItemID;
                }
                else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, tbBarcode.Text))
                {
                    ItemID = tbBarcode.Text;
                }
                else if (cmbRelation.SelectedData.ID == RecordIdentifier.Empty)
                {
                    cmbRelation.SelectedData = new DataEntity("", "");
                    cmbVariantNumber.SelectedData = new DataEntity("", "");
                    lblVariantNumber.Enabled = cmbVariantNumber.Enabled = false;
                    return;
                }
            }
            else if (Providers.RetailItemData.Exists(PluginEntry.DataModel, tbBarcode.Text))
            {
                ItemID = tbBarcode.Text;
            }

            if (ItemID != RecordIdentifier.Empty)
            {
                cmbRelation.SelectedData = new DataEntity { ID = ItemID };
                cmbRelation_SelectedDataChanged(tbBarcode, EventArgs.Empty);
            }

            lockEvent = true;

            VariantWantsFocus();

            lockEvent = false;
        }

        private void SetDefaults()
        {
            tbBarcode.Text = "";
            cmbRelation.SelectedData = new DataEntity("", "");
            cmbVariantNumber.SelectedData = new DataEntity("", "");

            lblVariantNumber.Enabled = cmbVariantNumber.Enabled = false;

            tbSerialNumber.Text = "";

            btnOK.Enabled = false;
            cmbType.SelectedIndex = 0;

            tbBarcode.Focus();
        }

        private void tbBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyData == Keys.Tab) || (e.KeyData == (Keys.Tab | Keys.Shift)) || e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                if (e.KeyData == (Keys.Tab | Keys.Shift))
                {
                    lockEvent = true;
                    cbCreateAnother.Select();
                }
                else
                {
                    cmbRelation.Select();
                }
            }
        }

        private void SerialNumberDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && ActiveControl is TextBox && (ActiveControl as TextBox).Tag != null && (ControlTypeEnums)(ActiveControl as TextBox).Tag == ControlTypeEnums.BarcodeSearch)
            {
                if (!string.IsNullOrEmpty(tbBarcode.Text))
                {
                    tbBarcode_KeyDown(sender, e);
                }
            }
        }

        private void cmbRelation_Leave(object sender, EventArgs e)
        {
            if (retailItem != null)
            {
                if (!lockEvent)
                {
                    if (retailItem.ItemType == ItemTypeEnum.MasterItem)
                    {
                        cmbVariantNumber.Focus();
                    }
                    else
                    {
                        tbSerialNumber.Focus();
                    }
                }
            }
            lockEvent = false;
        }

        private void VariantWantsFocus()
        {
            if (
                cmbVariantNumber.Enabled
                && (cmbVariantNumber.SelectedDataID == null || string.IsNullOrEmpty((string)cmbVariantNumber.SelectedDataID))
                && retailItem.ItemType == ItemTypeEnum.MasterItem
               )
            {
                cmbVariantNumber.Focus();
                return;
            }
        }

        private void tbSerialNumber_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckEnabled();
        }

        private void tbSerialNumber_TextChanged_1(object sender, EventArgs e)
        {
            CheckEnabled();
        }
    }
}
