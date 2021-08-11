using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Replenishment;

namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    public partial class InventoryTemplateGeneralPage : UserControl, ITabView
    {
        InventoryTemplate template;

        public InventoryTemplateGeneralPage()
        {
            InitializeComponent();
            cmbUnit.Items.AddRange(UnitSelectionEnumHelper.GetList().ToArray());
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryTemplateGeneralPage();
        }

        public bool DataIsModified()
        {
            return chkChangeVendorInLine.Checked != template.ChangeVendorInLine ||
                   chkChangeUomInLine.Checked != template.ChangeUomInLine ||
                   chkDisplayBarcode.Checked != template.DisplayBarcode ||
                   (UnitSelectionEnum)cmbUnit.SelectedIndex != template.UnitSelection;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            template = (InventoryTemplate)((List<object>)internalContext)[0];

            lblTypeValue.Text = TemplateEntryTypeEnumHelper.ToString(template.TemplateEntryType);
            cmbUnit.SelectedIndex = (int)template.UnitSelection;
            chkChangeVendorInLine.Checked = template.ChangeVendorInLine;
            chkChangeUomInLine.Checked = template.ChangeUomInLine;
            chkDisplayBarcode.Checked = template.DisplayBarcode;

            if (template.TemplateEntryType == TemplateEntryTypeEnum.TransferStock)
            {
                chkChangeVendorInLine.Enabled = false;
                chkChangeVendorInLine.Checked = false;
                lblChangeVendorInLine.Enabled = false;
            }
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            
        }

        public bool SaveData()
        {
            template.UnitSelection = (UnitSelectionEnum)cmbUnit.SelectedIndex;
            template.ChangeVendorInLine = chkChangeVendorInLine.Checked;
            template.ChangeUomInLine = chkChangeUomInLine.Checked;
            template.DisplayBarcode = chkDisplayBarcode.Checked;
            return true;
        }

        public void SaveUserInterface()
        {
            
        }
    }
}
