using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSRetail.StoreController.Controls;
using LSRetail.StoreController.SharedCore;
using LSRetail.StoreController.SharedCore.Enums;
using LSRetail.Utilities.DataTypes;
using LSRetail.StoreController.SharedDatabase.DataEntities;
using LSRetail.StoreController.SharedCore.EventArguments;
using LSRetail.StoreController.BusinessObjects;
using LSRetail.StoreController.BusinessObjects.BarCodes;
using LSRetail.StoreController.DataProviders.BarCodes;
using LSRetail.StoreController.Controls.DataControls;

namespace LSRetail.StoreController.BarCodes.Views
{
    public partial class BarCodeSetupDataView : ViewBase
    {
        private RecordIdentifier barCodeSetupID;
        private BarCodeSetup barCodeSetup;
        private bool initialInitialize = false;

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

            //this.ReadOnly = !PluginEntry.Connection.CheckPermission(Permission.VisualProfileEdit);

            btnAddBarCodeMask.Visible = PluginEntry.DataModel.HasPermission(Permission.ManageBarCodesMasks);
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

        public override Utilities.DataTypes.RecordIdentifier ID
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
            HeaderIcon = Properties.Resources.BarcodeEditImage;

            barCodeSetup = Providers.BarCodeSetupData.Get(PluginEntry.DataModel,barCodeSetupID);

            tbID.Text = (string)barCodeSetup.ID;
            tbDescription.Text = barCodeSetup.Text;

            cmbBarCodeMask.SelectedData = new Datalayer.DataEntities.BarCodeMask(barCodeSetup.BarCodeMask);
            ntbMinimumLength.Value = (double)barCodeSetup.MinimumLength;
            ntbMaximumLength.Value = (double)barCodeSetup.MaximumLength;

            // This one has to be set last.
            cmbBarCodeType.SelectedIndex = BarCodeTypeToIndex(barCodeSetup.BarCodeType);
        }

        protected override bool DataIsModified()
        {
            if (tbDescription.Text != barCodeSetup.Text) return true;
            if (cmbBarCodeType.SelectedIndex != BarCodeTypeToIndex(barCodeSetup.BarCodeType)) return true;
            if (((Datalayer.DataEntities.BarCodeMask)cmbBarCodeMask.SelectedData).Mask != barCodeSetup.BarCodeMask) return true;
            if (ntbMinimumLength.Value != (double)barCodeSetup.MinimumLength) return true;
            if (ntbMaximumLength.Value != (double)barCodeSetup.MaximumLength) return true;

            return false;
        }

        protected override bool SaveData()
        {
            barCodeSetup.Text = tbDescription.Text;
            barCodeSetup.BarCodeType = IndexToBarCodeType(cmbBarCodeType.SelectedIndex);
            barCodeSetup.BarCodeMask = ((Datalayer.DataEntities.BarCodeMask)cmbBarCodeMask.SelectedData).Mask;
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

        //private void FormatData(object sender, LSRetail.StoreController.Controls.DropDownFormatDataArgs e)
        //{
        //    e.TextToDisplay = ((Datalayer.DataEntities.BarCodeMask)((DualDataComboBox)sender).SelectedData).Mask;
        //}

        private void RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new Datalayer.DataEntities.BarCodeMask("");
        }

        private void cmbBarCodeMask_RequestData(object sender, EventArgs e)
        {
            cmbBarCodeMask.SetHeaders(new string[]{
                Properties.Resources.Mask,
                Properties.Resources.Symbology,
                Properties.Resources.Description,
                Properties.Resources.Type},
                new int[] { 1, 2, 3, 4 });

            cmbBarCodeMask.SetWidth(400);

            cmbBarCodeMask.SetData(Datalayer.BarCodeMaskData.GetBarCodeMasks(PluginEntry.DataModel,5,false).Cast<DataEntity>(), null, -1);
        }

        protected override void OnSetupContextBarItems(LSRetail.StoreController.SharedCore.EventArguments.ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == base.GetType().ToString() + ".Related")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.ManageBarCodesMasks))
                {
                    arguments.Add(new ContextBarItem(
                        Properties.Resources.ManageBarCodeMasks,
                        Properties.Resources.BarcodeImage,
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

        private void cmbFontName_FormatData(object sender, DropDownFormatDataArgs e)
        {
            e.TextToDisplay = (string)((DualDataComboBox)sender).SelectedData.ID;
        }

        private void cmbFontName_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void cmbFontName_RequestData(object sender, EventArgs e)
        {
            Datalayer.DataEntities.BarCodeTypes.BarCodeType barCodeType;

            barCodeType = Datalayer.DataEntities.BarCodeTypes.BarCodeType.GetBarCodeType(IndexToBarCodeType(cmbBarCodeType.SelectedIndex));
        }

        private void btnAddBarCodeSetup_Click(object sender, EventArgs e)
        {
            Dialogs.BarCodeMaskDialog dlg = new Dialogs.BarCodeMaskDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, "BarCodeMaskSetup", dlg.BarCodeMaskID, null);
            }
        }


    }
}
