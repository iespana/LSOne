using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.BarCodes;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.BarCodes;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewPlugins.BarCodes.Datalayer;
using LSOne.ViewPlugins.BarCodes.Datalayer.DataEntities;

namespace LSOne.ViewPlugins.BarCodes.Views
{
    public partial class BarCodeSetupDataView : ViewBase
    {
        private RecordIdentifier barCodeSetupID;
        private BarCodeSetup barCodeSetup;
        private bool initialInitialize = false;
        private List<DataEntity> barcodeMasks;
        private List<BarcodeMask> masks;

        public BarCodeSetupDataView(RecordIdentifier barCodeSetupID)
            : this()
        {
            this.barCodeSetupID = barCodeSetupID;
        }

        private BarCodeSetupDataView()
        {
            InitializeComponent();

            Attributes = ViewAttributes.Revert |
                ViewAttributes.Save |
                ViewAttributes.Delete |
                ViewAttributes.Audit |
                ViewAttributes.Close |
                ViewAttributes.ContextBar |
                ViewAttributes.Help;

            barcodeMasks = new List<DataEntity>();
            PopulateBarcodeMasks();
            btnsEditAdd.AddButtonEnabled = btnsEditAdd.EditButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            contexts.Add(new AuditDescriptor("BarCodeSetup", barCodeSetupID, Properties.Resources.BarCodeSetup, true));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.BarCodeSetup;
            }
        }

        public override RecordIdentifier ID
        {
            get
            {
                return barCodeSetupID;
            }
        }

        public string Description
        {
            get
            {
                return Properties.Resources.BarCodeSetupID + ": " + (string)barCodeSetupID;
            }
        }

        protected override void LoadData(bool isRevert)
        {
            if (isRevert)
            {
                
            }

            if(initialInitialize == false)
            {
                AddParentViewDescriptor(new ParentViewDescriptor(
                        RecordIdentifier.Empty,
                        Properties.Resources.BarCodeSetup,
                        Properties.Resources.BarcodeImage,
                        new ShowParentViewHandler(PluginOperations.ShowBarCodeSetup)));

                initialInitialize = true;
            }

            HeaderText = Description;
            //HeaderIcon = Properties.Resources.BarcodeEditImage;

            barCodeSetup = Providers.BarCodeSetupData.Get(PluginEntry.DataModel,barCodeSetupID);

            tbID.Text = (string)barCodeSetup.ID;
            tbDescription.Text = barCodeSetup.Text;

            cmbBarCodeMask.SelectedData = new DataEntity(new RecordIdentifier(barCodeSetup.BarCodeMask, barCodeSetup.BarCodeMaskID), barCodeSetup.BarCodeMaskDescription);

            tbBarcodeMask.Text = barCodeSetup.BarCodeMask;
            ntbMinimumLength.Value = (double)barCodeSetup.MinimumLength;
            ntbMaximumLength.Value = (double)barCodeSetup.MaximumLength;

            // This one has to be set last.
            cmbBarCodeType.SelectedIndex = BarCodeTypeToIndex(barCodeSetup.BarCodeType);
            cmbBarCodeMask_SelectedDataChanged(this, EventArgs.Empty);
        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != barCodeSetup.Text) return true;
            if (cmbBarCodeType.SelectedIndex != BarCodeTypeToIndex(barCodeSetup.BarCodeType)) return true;
            if (cmbBarCodeMask.SelectedData.ID != barCodeSetup.BarCodeMask) return true;
            if (ntbMinimumLength.Value != (double)barCodeSetup.MinimumLength) return true;
            if (ntbMaximumLength.Value != (double)barCodeSetup.MaximumLength) return true;

            return false;
        }

        protected override bool SaveData()
        {
            barCodeSetup.Text = tbDescription.Text;
            barCodeSetup.BarCodeType = IndexToBarCodeType(cmbBarCodeType.SelectedIndex);
            barCodeSetup.BarCodeMask = (string)cmbBarCodeMask.SelectedData.ID;
            barCodeSetup.BarCodeMaskDescription = cmbBarCodeMask.SelectedData.Text;
            barCodeSetup.MinimumLength = (int)ntbMinimumLength.Value;
            barCodeSetup.MaximumLength = (int)ntbMaximumLength.Value;

            Providers.BarCodeSetupData.Save(PluginEntry.DataModel,barCodeSetup);

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "BarCodeSetup", barCodeSetup.ID, null);
            
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "BarCodeSetup":
                    if (changeHint == DataEntityChangeType.Delete && changeIdentifier == barCodeSetupID)
                    {
                        PluginEntry.Framework.ViewController.DiscardCurrentView(this);
                    }
                    break;
            }
        }

        protected override void OnDelete()
        {
            PluginOperations.DeleteBarCodeSetup(barCodeSetupID);
        }

        private int BarCodeTypeToIndex(int barCodeType)
        {
            switch (barCodeType)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    return barCodeType;

                case 9:
                    return 9;//101;

                case 10:
                    return 10;// 102;

 
            }

            return 0;
        }

        private int IndexToBarCodeType(int index)
        {
            switch (index)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    return index;

                case 9:
                    return 9;//101;

                case 10:
                    return 10;//102
            }

            return 0;
        }

        private void PopulateBarcodeMasks()
        {
            masks = Providers.BarcodeMaskData.GetBarCodeMasks(PluginEntry.DataModel, 1, false);
            foreach (var mask in masks)
            {
                barcodeMasks.Add(new DataEntity(new RecordIdentifier(mask.Mask, mask.ID), mask.Text));
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks))
                {
                    arguments.Add(new ContextBarItem(
                        Properties.Resources.ManageBarCodeMasks,
                        new ContextbarClickEventHandler(PluginOperations.ShowBarCodeMaskSetup)), 300);
                }
            }
        }

        private void cmbBarCodeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Datalayer.DataEntities.BarCodeTypes.BarCodeType barCodeType;

            barCodeType = Datalayer.DataEntities.BarCodeTypes.BarCodeType.GetBarCodeType(IndexToBarCodeType(cmbBarCodeType.SelectedIndex));

            if (barCodeType.MaximumLength == 0)
            {
                ntbMaximumLength.Enabled = true;
            }
            else
            {
                ntbMaximumLength.Value = barCodeType.MaximumLength;
                ntbMaximumLength.Enabled = false;
            }

            if (barCodeType.MinimumLength == 0)
            {
                ntbMinimumLength.Enabled = true;
            }
            else
            {
                ntbMinimumLength.Value = barCodeType.MinimumLength;
                ntbMinimumLength.Enabled = false;
            }
        }

        private void btnAddBarCodeSetup_Click(object sender, EventArgs e)
        {
            Dialogs.BarCodeMaskDialog dlg = new Dialogs.BarCodeMaskDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                cmbBarCodeMask.SelectedData = new DataEntity(new RecordIdentifier(dlg.BarcodeMask.Mask, dlg.BarCodeMaskID), dlg.BarcodeMask.Text);
                tbBarcodeMask.Text = dlg.BarcodeMask.Mask;
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "BarCodeSetup", dlg.BarCodeMaskID, null);
                barcodeMasks.Clear();
                PopulateBarcodeMasks();
                cmbBarCodeMask_SelectedDataChanged(sender, e);
            }
        }

        private void cmbBarCodeMask_DropDown(object sender, DropDownEventArgs e)
        {
            var dataPanel = new DualDataPanel("", barcodeMasks, null, true, false, false, 22, false);
            e.ControlToEmbed = dataPanel;
        }

        private void cmbBarCodeMask_SelectedDataChanged(object sender, EventArgs e)
        {
            tbBarcodeMask.Text = cmbBarCodeMask.SelectedData.ID.StringValue;
            btnsEditAdd.EditButtonEnabled = !RecordIdentifier.IsEmptyOrNull(cmbBarCodeMask.SelectedData.ID) && PluginEntry.DataModel.HasPermission(Permission.ManageBarcodesMasks);
        }

        private void cmbBarCodeMask_FormatData(object sender, DropDownFormatDataArgs e)
        {
            cmbBarCodeMask.Text = ((DataEntity)e.Data).Text;
        }

        private void btnsEditAdd_EditButtonClicked(object sender, EventArgs e)
        {
            var mask = barcodeMasks.Find(x => x.ID.SecondaryID == cmbBarCodeMask.SelectedData.ID.SecondaryID);
            var dlg = new Dialogs.BarCodeMaskDialog(mask.ID.SecondaryID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                cmbBarCodeMask.SelectedData = new DataEntity(new RecordIdentifier(dlg.BarcodeMask.Mask, dlg.BarCodeMaskID), dlg.BarcodeMask.Text);
                tbBarcodeMask.Text = dlg.BarcodeMask.Mask;
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "BarCodeSetup", dlg.BarCodeMaskID, null);
                barcodeMasks.Clear();
                PopulateBarcodeMasks();
            }
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
