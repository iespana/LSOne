using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Replenishment;

namespace LSOne.ViewPlugins.LSCommerce.ViewPages
{
    public partial class InventoryTemplateGeneralPage : UserControl, ITabView
    {
        InventoryTemplate template;

        public InventoryTemplateGeneralPage()
        {
            InitializeComponent();

            cmbEnteringType.Items.AddRange(EnteringTypeEnumHelper.GetList().ToArray());
            cmbQuantityMethod.Items.AddRange(QuantityMethodEnumHelper.GetList().ToArray());
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryTemplateGeneralPage();
        }

        public bool DataIsModified()
        {
            return chkUseBarcodeUom.Checked != template.UseBarcodeUom ||
                   chkAllowAddNewLine.Checked != template.AllowAddNewLine ||
                   chkAllowImageImport.Checked != template.AllowImageImport ||
                   cmbEnteringType.SelectedIndex != (int)template.EnteringType ||
                   cmbQuantityMethod.SelectedIndex != (int)template.QuantityMethod ||
                   ntDefaultQty.Value != (double)template.DefaultQuantity;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            template = (InventoryTemplate)((List<object>)internalContext)[0];

            cmbEnteringType.SelectedIndex = (int)template.EnteringType;
            cmbQuantityMethod.SelectedIndex = (int)template.QuantityMethod;
            ntDefaultQty.Value = (double)template.DefaultQuantity;
            chkUseBarcodeUom.Checked = template.UseBarcodeUom;
            chkAllowAddNewLine.Checked = template.AllowAddNewLine;
            chkAllowImageImport.Checked = template.AllowImageImport;
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            template.EnteringType = (EnteringTypeEnum)cmbEnteringType.SelectedIndex;
            template.QuantityMethod = (QuantityMethodEnum)cmbQuantityMethod.SelectedIndex;
            template.DefaultQuantity = (decimal)ntDefaultQty.Value;
            template.UseBarcodeUom = chkUseBarcodeUom.Checked;
            template.AllowAddNewLine = chkAllowAddNewLine.Checked;
            template.AllowImageImport = chkAllowImageImport.Checked;
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void cmbQuantityMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ntDefaultQty.Enabled = cmbQuantityMethod.SelectedIndex > 0;

            if (!ntDefaultQty.Enabled)
            {
                ntDefaultQty.Value = 1;
            }
        }
    }
}
