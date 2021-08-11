using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.LookupValues;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PaymentLimitations.Dialogs
{
    public partial class PaymentLimitationDialog : DialogBase
    {
        private RecordIdentifier paymentID;
        private PaymentMethodLimitation orgLimitation;
        private bool editing;
        private bool checkIfExists;
        private List<PaymentMethodLimitationRestrictionCode> limitationRestrictionCodes;

        public PaymentLimitationDialog(RecordIdentifier paymentID, RecordIdentifier ID) : this(paymentID)
        {
            if (ID != RecordIdentifier.Empty)
            {
                editing = true;
                checkIfExists = false;
                chkCreateAnother.Visible = false;

                orgLimitation = Providers.PaymentLimitationsData.Get(PluginEntry.DataModel, ID);

                cmbLimitationCode.SelectedData = new DataEntity(orgLimitation.LimitationMasterID, (string)orgLimitation.RestrictionCode);
                cmbLimitationCode.SelectedData = limitationRestrictionCodes.FirstOrDefault(p => p.ID == orgLimitation.RestrictionCode);
                chkIncluded.Checked = orgLimitation.Include;
                chkTaxExempt.Checked = orgLimitation.TaxExempt;

                switch (orgLimitation.Type)
                {
                    case LimitationType.Item:
                    case LimitationType.RetailDepartment:
                    case LimitationType.RetailGroup:
                    case LimitationType.SpecialGroup:
                        cmbType.SelectedIndex = (int) orgLimitation.Type;
                        SetRelation();
                        break;

                    case LimitationType.Everything:
                        cmbType.SelectedIndex = cmbType.Items.Count-1;
                        break;
                }

                CheckEnabled(null, new EventArgs());                
            }
        }

        public PaymentLimitationDialog(RecordIdentifier paymentID)
        {
            InitializeComponent();

            this.paymentID = paymentID;
            orgLimitation = new PaymentMethodLimitation();            
            limitationRestrictionCodes = Providers.PaymentLimitationsData.GetRestrictionCodesForTender(PluginEntry.DataModel, paymentID);
            editing = false;
            checkIfExists = true;
            chkCreateAnother.Checked = false;
            cmbRelation.Enabled = false;            
        }

        private void SetRelation()
        {
            LimitationType currentType = GetSelectedType();
            switch (currentType)
            {
                case LimitationType.Item:
                    RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, orgLimitation.RelationMasterID, CacheType.CacheTypeApplicationLifeTime);
                    cmbRelation.SelectedData = new DataEntity(item.ID, item.Text);
                    cmbRelation.Enabled = true;
                    cmbVariant.Enabled = item.IsVariantItem || item.IsHeaderItem;
                    if (item.IsVariantItem)
                    {
                        cmbVariant.SelectedData = new DataEntity(item.MasterID, item.VariantName);
                    }
                    break;
                case LimitationType.RetailGroup:
                case LimitationType.RetailDepartment:
                case LimitationType.SpecialGroup:
                    cmbRelation.SelectedData = new DataEntity(orgLimitation.RelationReadableID, orgLimitation.Description);
                    cmbRelation.Enabled = true;
                    break;
                case LimitationType.Everything:
                    cmbRelation.Enabled = false;
                    cmbVariant.Enabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private LimitationType GetSelectedType()
        {
            return cmbType.SelectedIndex == cmbType.Items.Count - 1 ? LimitationType.Everything : (LimitationType) cmbType.SelectedIndex;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            PaymentMethodLimitation limitation = new PaymentMethodLimitation();

            limitation.ID = orgLimitation.ID;
            limitation.RestrictionCode = ((DataEntity) cmbLimitationCode.SelectedData).Text;

            if (limitationRestrictionCodes.Count(c => c.ID == limitation.RestrictionCode) > 0)
            {
                limitation.LimitationMasterID = limitationRestrictionCodes.FirstOrDefault(f => f.ID == limitation.RestrictionCode).LimitationMasterID;
            }
            else
            {
                limitation.LimitationMasterID = Providers.PaymentLimitationsData.GetLimitationMasterID(PluginEntry.DataModel, limitation.RestrictionCode);
                limitation.LimitationMasterID = limitation.LimitationMasterID == RecordIdentifier.Empty ? Guid.NewGuid() : limitation.LimitationMasterID;

                limitationRestrictionCodes.Add(new PaymentMethodLimitationRestrictionCode()
                {
                    ID = limitation.RestrictionCode,
                    LimitationMasterID = limitation.LimitationMasterID,
                    TaxExempt = chkTaxExempt.Checked,
                    TenderID = limitation.TenderID
                });
            }

            limitation.TenderID = paymentID;
            limitation.Include = chkIncluded.Checked;
            limitation.Type = GetSelectedType();
            limitation.RelationMasterID = GetRelation();
            limitation.TaxExempt = chkTaxExempt.Checked;

            if (checkIfExists && (Providers.PaymentLimitationsData.Exists(PluginEntry.DataModel, limitation.TenderID, limitation.RestrictionCode, limitation.Type, limitation.RelationMasterID)))
            {
                errorProvider1.SetError(cmbRelation, Properties.Resources.PaymentLimitationExists);
                return;
            }
            
            Providers.PaymentLimitationsData.Save(PluginEntry.DataModel, limitation);

            // Set tax exempt status for all limitations using the same limitation code. This is so that we don't get
            // multiple limitations for the same code using different tax exempt status.
            Providers.PaymentLimitationsData.SetTaxExemptStatus(PluginEntry.DataModel, limitation.LimitationMasterID, limitation.RestrictionCode, limitation.TaxExempt);

            PluginEntry.Framework.ViewController.NotifyDataChanged(null, orgLimitation.Empty() ? DataEntityChangeType.Add : DataEntityChangeType.Edit, "PaymentMethodLimitations", limitation.ID, limitation);

            if (chkCreateAnother.Checked)
            {
                cmbRelation.SelectedData = new DataEntity();
                cmbVariant.SelectedData = new DataEntity();
                cmbRelation.Focus();
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private RecordIdentifier GetRelation()
        {
            LimitationType type = GetSelectedType();
            switch (type)
            {
                case LimitationType.Item:
                    RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedDataID, CacheType.CacheTypeApplicationLifeTime);
                    if (item.IsHeaderItem)
                    {
                        item = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbVariant.SelectedDataID, CacheType.CacheTypeApplicationLifeTime);
                    }
                    return item.MasterID;
                case LimitationType.RetailGroup:
                    RetailGroup itemGroup = Providers.RetailGroupData.Get(PluginEntry.DataModel, cmbRelation.SelectedDataID);
                    return itemGroup.MasterID;
                    
                case LimitationType.RetailDepartment:
                    RetailDepartment itemdepartment = Providers.RetailDepartmentData.Get(PluginEntry.DataModel, cmbRelation.SelectedDataID);
                    return itemdepartment.MasterID;

                case LimitationType.SpecialGroup:
                    SpecialGroup specialGroup = Providers.SpecialGroupData.GetSpecialGroup(PluginEntry.DataModel, cmbRelation.SelectedDataID);
                    return specialGroup.MasterID;

                case LimitationType.Everything:
                    return Guid.Empty;
                    
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CheckEnabled(object sender, EventArgs args)
        {
            errorProvider1.Clear();

            LimitationType type = GetSelectedType();

            if (cmbLimitationCode.SelectedData == null || cmbLimitationCode.SelectedDataID == RecordIdentifier.Empty)
            {
                btnOK.Enabled = false;
                return;
            }

            if (type != LimitationType.Everything && (cmbRelation.SelectedData == null || cmbRelation.SelectedDataID == RecordIdentifier.Empty))
            {
                btnOK.Enabled = false;
                return;
            }

            if (cmbVariant.Enabled && (cmbVariant.SelectedData == null || cmbVariant.SelectedDataID == RecordIdentifier.Empty))
            {
                btnOK.Enabled = false;
                return;
            }

            if (orgLimitation.RestrictionCode == ((DataEntity)cmbLimitationCode.SelectedData).Text &&
                orgLimitation.Include == chkIncluded.Checked &&
                orgLimitation.RelationMasterID == cmbRelation.SelectedDataID &&
                orgLimitation.Type == GetSelectedType() &&
                orgLimitation.TaxExempt == chkTaxExempt.Checked)
            {
                btnOK.Enabled = false;
                return;
            }

            btnOK.Enabled = true;
        }

        private void DoCheckIfExists(bool selectionHasChanged)
        {
            if (!editing)
            {
                checkIfExists = true;
                return;
            }

            checkIfExists = !checkIfExists && (editing && selectionHasChanged);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LimitationType currentSelection = GetSelectedType();

            DoCheckIfExists(orgLimitation.Type != currentSelection);

            if (currentSelection == LimitationType.Everything)
            {
                cmbRelation.SelectedData = new DataEntity();
                cmbVariant.SelectedData = new DataEntity();
                cmbRelation.Enabled = false;
                cmbVariant.Enabled = false;
            }
            else if (currentSelection == LimitationType.Item)
            {
                cmbRelation.SelectedData = new DataEntity();
                cmbVariant.SelectedData = new DataEntity();
                cmbRelation.Enabled = true;
                cmbVariant.Enabled = true;
            }
            else
            {
                cmbRelation.SelectedData = new DataEntity();
                cmbVariant.SelectedData = new DataEntity();
                cmbRelation.Enabled = true;
                cmbVariant.Enabled = false;
            }
            CheckEnabled(sender, e);
        }

        private void cmbRelation_DropDown(object sender, DropDownEventArgs e)
        {
            LimitationType currentType = GetSelectedType();

            if (currentType == LimitationType.Everything)
            {
                return;
            }

            RecordIdentifier initialSearchText;
            bool textInitallyHighlighted;
            if (e.DisplayText != "")
            {
                initialSearchText = e.DisplayText;
                textInitallyHighlighted = false;
            }
            else
            {
                initialSearchText = cmbRelation.SelectedData.Text;
                textInitallyHighlighted = true;
            }
            
            switch (currentType)
            {
                case LimitationType.Item:
                    e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailItems, textInitallyHighlighted, null, true);
                    break;
                case LimitationType.RetailGroup:
                    e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailGroups, textInitallyHighlighted, null, true);
                    break;
                case LimitationType.RetailDepartment:
                    e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.RetailDepartments, textInitallyHighlighted, null, true);
                    break;
                case LimitationType.SpecialGroup:
                    e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel, false, initialSearchText, SearchTypeEnum.SpecialGroups, textInitallyHighlighted, null, true);
                    break;
                case LimitationType.Everything:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void cmbVariant_DropDown(object sender, DropDownEventArgs e)
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
                initialSearchText = ((DataEntity)cmbVariant.SelectedData).Text;
                textInitallyHighlighted = true;
            }

            RetailItem retailItem = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedDataID, CacheType.CacheTypeApplicationLifeTime);
            if (retailItem != null)
            {
                e.ControlToEmbed = new SingleSearchPanel(PluginEntry.DataModel,
                    retailItem.ItemType == ItemTypeEnum.MasterItem ?
                        retailItem.MasterID :
                        retailItem.HeaderItemID,
                        true, initialSearchText, SearchTypeEnum.RetailItemVariantMasterID, excludedItemIDs, textInitallyHighlighted, true);
            }
        }

        private void cmbRelation_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbRelation.SelectedData != null && !string.IsNullOrEmpty(cmbRelation.SelectedDataID.StringValue))
            {
                if (GetSelectedType() == LimitationType.Item)
                {
                    RetailItem item = Providers.RetailItemData.Get(PluginEntry.DataModel, cmbRelation.SelectedDataID, CacheType.CacheTypeApplicationLifeTime);
                    cmbRelation.SelectedData = new DataEntity(item.MasterID, item.Text);
                    cmbVariant.Enabled = (item.ItemType == ItemTypeEnum.MasterItem);
                }
                else
                {
                    cmbVariant.SelectedData = new DataEntity();
                    cmbVariant.Enabled = false;
                }
                DoCheckIfExists(orgLimitation.RelationMasterID != cmbRelation.SelectedDataID);
            }
            CheckEnabled(sender, e);
        }

        private void GenericLeave(object sender, EventArgs e)
        {
            CheckEnabled(sender, e);
        }

        private void cmbVariant_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbVariant.Enabled = true && cmbVariant.SelectedData != null && !string.IsNullOrEmpty(cmbVariant.SelectedDataID.StringValue))
            {
                DoCheckIfExists(orgLimitation.RelationReadableID != cmbVariant.SelectedDataID);
                
                CheckEnabled(sender, e);
            }
        }

        private void tbLimitationCode_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            CheckEnabled(sender, e);
        }

        private void cmbLimitationCode_RequestData(object sender, EventArgs e)
        {            
            cmbLimitationCode.SetData(limitationRestrictionCodes.OrderBy(o => o.Text), null);
        }

        private void PaymentLimitationDialog_Load(object sender, EventArgs e)
        {
            if (limitationRestrictionCodes.Count() == 1)
            {
                cmbLimitationCode.SelectedData = limitationRestrictionCodes[0];
                cmbType.Focus();
            }

            CmbLimitationCode_SelectedDataChanged(this, EventArgs.Empty);
        }

        private void CmbLimitationCode_SelectedDataChanged(object sender, EventArgs e)
        {
            btnsEditAdd.EditButtonEnabled = cmbLimitationCode.SelectedData != null && cmbLimitationCode.SelectedDataID != RecordIdentifier.Empty;

            if(cmbLimitationCode.SelectedDataID != null && cmbLimitationCode.SelectedDataID != RecordIdentifier.Empty)
            {
                PaymentMethodLimitationRestrictionCode selectedCode = (PaymentMethodLimitationRestrictionCode)cmbLimitationCode.SelectedData;
                chkTaxExempt.Checked = selectedCode.TaxExempt;
            }
            else
            {
                chkTaxExempt.Checked = false;
            }
        }

        private void BtnsEditAdd_AddButtonClicked(object sender, EventArgs e)
        {
            LimitationCodeDialog dlg = new LimitationCodeDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PaymentMethodLimitationRestrictionCode newRestrictionCode = new PaymentMethodLimitationRestrictionCode();
                newRestrictionCode.ID = dlg.LimitationCode;
                newRestrictionCode.TenderID = paymentID;
                newRestrictionCode.LimitationMasterID = orgLimitation.Empty() ? Guid.NewGuid() : orgLimitation.LimitationMasterID;
                limitationRestrictionCodes.Add(newRestrictionCode);
                cmbLimitationCode.SelectedData = newRestrictionCode;
                chkTaxExempt.Checked = dlg.TaxExempt;
                CheckEnabled(sender, e);
            }
        }

        private void BtnsEditAdd_EditButtonClicked(object sender, EventArgs e)
        {
            PaymentMethodLimitationRestrictionCode selectedCode = (PaymentMethodLimitationRestrictionCode)cmbLimitationCode.SelectedData;

            using (LimitationCodeDialog dlg = new LimitationCodeDialog(selectedCode.ID, chkTaxExempt.Checked))
            {
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    chkTaxExempt.Checked = dlg.TaxExempt;
                }
            }
        }
    }
}