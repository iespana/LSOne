using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Controls;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.RetailItemAssemblies.Dialogs
{ 
    public partial class NewEditAssemblyDialog : DialogBase
    {
        private readonly List<DataEntity> kdsComponentOptions = new List<DataEntity>
        {
            new DataEntity((int)KitchenDisplayAssemblyComponentType.DontSend, Properties.Resources.DontSend),
            new DataEntity((int)KitchenDisplayAssemblyComponentType.SendAsItemModifiers, Properties.Resources.AsItemModifiers),
            new DataEntity((int)KitchenDisplayAssemblyComponentType.SendAsSeparateItems, Properties.Resources.AsSeparateItems)
        };

        private RecordIdentifier itemID;
        private DataEntity defaultSalesUnit;
        private decimal defaultSalesPrice;

        private bool saveRecord;
        private bool doEvents;

        private DecimalLimit priceLimit;
        private List<RetailItemAssembly> retailItemAssemblies;
        private decimal lastSalePrice;

        public RetailItemAssembly CopyFromAssembly { get; private set; }

        public RetailItemAssembly Assembly { get; private set; }

        public bool CreateAnother { get; private set; }

        public NewEditAssemblyDialog(RecordIdentifier retailItemID, bool saveRecord, List<RetailItemAssembly> localItemAssemblies, bool createAnother, decimal defaultSalesPrice, DataEntity defaultSalesUnit)
        {
            itemID = retailItemID ?? RecordIdentifier.Empty;

            InitializeComponent();

            this.retailItemAssemblies = localItemAssemblies;
            this.saveRecord = saveRecord;
            this.defaultSalesPrice = defaultSalesPrice;
            this.defaultSalesUnit = defaultSalesUnit;
            cmbStore.SelectedData = new DataEntity("", "");
            cmbCopyFrom.SelectedData = new DataEntity("", "");
            cmbSendComponentsToKds.SelectedData = new DataEntity((int)KitchenDisplayAssemblyComponentType.DontSend, Properties.Resources.DontSend);

            SetDecimalLimits();

            ntbPrice.SetValueWithLimit(defaultSalesPrice, priceLimit);

            cbCreateAnother.Checked = CreateAnother = createAnother;
            doEvents = true;
        }

        public NewEditAssemblyDialog(RetailItemAssembly assembly, bool saveRecord, DataEntity defaultSalesUnit)
        {
            Assembly = assembly;
            itemID = assembly.ItemID ?? RecordIdentifier.Empty;

            InitializeComponent();

            Text = Properties.Resources.EditRetailItemAssembly;

            this.saveRecord = saveRecord;
            this.defaultSalesUnit = defaultSalesUnit;

            cmbStore.Enabled = !(chkAllStores.Checked = (assembly.StoreID == ""));
            cmbStore.SelectedData = new DataEntity(assembly.StoreID, assembly.StoreName);

            assembly.StartingDate.ToDateControl(dtpStartingDate);

            cmbCopyFrom.Visible = lblCopyFrom.Visible = false;
            cbCreateAnother.Visible = false;
            SetDecimalLimits();

            txtDescription.Text = assembly.Text;
            ntbPrice.SetValueWithLimit(assembly.CalculatePriceFromComponents ? assembly.TotalSalesPrice : assembly.Price, priceLimit);
            ntbMargin.SetValueWithLimit(assembly.Margin, priceLimit);
            ntbTotalCost.SetValueWithLimit(assembly.TotalCost, priceLimit);
            lastSalePrice = assembly.Price;

            cmbSendComponentsToKds.SelectedData = new DataEntity(
                (int)assembly.SendAssemblyComponentsToKds, 
                PluginOperations.GetSendToKdsDisplayName(assembly.SendAssemblyComponentsToKds));

            chkShowComponentsOnPos.Checked = assembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnPOS);
            chkShowComponentsOnReceipt.Checked = assembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnReceipt);
            
            chkCalculatePriceFromComponents.Checked = assembly.CalculatePriceFromComponents;

            doEvents = true;
        }

        private void SetDecimalLimits()
        {
            priceLimit = PluginEntry.DataModel.GetDecimalSetting(DataLayer.GenericConnector.Enums.DecimalSettingEnum.Prices);

            ntbPrice.SetValueWithLimit(0m, priceLimit);
            ntbMargin.SetValueWithLimit(0m, priceLimit);
            ntbTotalCost.SetValueWithLimit(0m, priceLimit);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (txtDescription.Text == "" || (!chkAllStores.Checked && RecordIdentifier.IsEmptyOrNull(cmbStore.SelectedDataID)))
            {
                btnOK.Enabled = false;
                return;
            }

            if (Assembly != null)
            {
                if (Assembly.StoreID == cmbStore.SelectedDataID
                    && Assembly.CalculatePriceFromComponents == chkCalculatePriceFromComponents.Checked
                    && Assembly.Price == (decimal)ntbPrice.Value
                    && Assembly.Margin == (decimal)ntbMargin.Value
                    && Assembly.StartingDate.DateTime.Date == dtpStartingDate.Value.Date
                    && Assembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnPOS) == chkShowComponentsOnPos.Checked
                    && Assembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnReceipt) == chkShowComponentsOnReceipt.Checked
                    && Assembly.SendAssemblyComponentsToKds == (KitchenDisplayAssemblyComponentType)(int)cmbSendComponentsToKds.SelectedDataID
                    && Assembly.Text == txtDescription.Text)
                {
                    btnOK.Enabled = false;
                    return;
                }
            }

            btnOK.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs args)
        {
            DateTime savingTime = DateTime.Now;

            if(Assembly == null)
            {
                Assembly = new RetailItemAssembly();
                Assembly.ItemID = itemID;
            }
            else if(Assembly.StartingDate.DateTime.Date == dtpStartingDate.Value.Date)
            {
                savingTime = Assembly.StartingDate.DateTime;
            }

            Assembly.Text = txtDescription.Text;
            Assembly.SetDisplayWithComponents(ExpandAssemblyLocation.OnPOS, chkShowComponentsOnPos.Checked);
            Assembly.SetDisplayWithComponents(ExpandAssemblyLocation.OnReceipt, chkShowComponentsOnReceipt.Checked);
            Assembly.SendAssemblyComponentsToKds = (KitchenDisplayAssemblyComponentType)(int)cmbSendComponentsToKds.SelectedDataID;
            Assembly.CalculatePriceFromComponents = chkCalculatePriceFromComponents.Checked;
            Assembly.StoreID = chkAllStores.Checked ? "" : cmbStore.SelectedData.ID;
            Assembly.StoreName = chkAllStores.Checked ? "" : cmbStore.SelectedData.Text;
            Assembly.StartingDate = new Date(dtpStartingDate.Value.Date.AddHours(savingTime.Hour).AddMinutes(savingTime.Minute).AddSeconds(savingTime.Second)); 
            Assembly.Margin = (decimal)ntbMargin.Value;
            
            if(!Assembly.CalculatePriceFromComponents)
            {
                Assembly.Price = (decimal)ntbPrice.Value;
            }

            if (saveRecord)
            {
                Providers.RetailItemAssemblyData.Save(PluginEntry.DataModel, Assembly);

                if(CopyFromAssembly != null)
                {
                    List<RetailItemAssemblyComponent> assemblyComponents = Providers.RetailItemAssemblyComponentData.GetList(PluginEntry.DataModel, CopyFromAssembly.ID);

                    foreach(RetailItemAssemblyComponent component in assemblyComponents)
                    {
                        component.ID = RecordIdentifier.Empty;
                        component.AssemblyID = Assembly.ID;
                        Providers.RetailItemAssemblyComponentData.Save(PluginEntry.DataModel, component);
                    }
                }

                PluginEntry.Framework.ViewController.NotifyDataChanged(this, ViewCore.Enums.DataEntityChangeType.Add, "RetailItemAssembly", Assembly.ID, Assembly);
            }

            if (cbCreateAnother.Visible && cbCreateAnother.Checked)
            {
                if(saveRecord)
                {
                    ClearForm();
                }
                else
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void ClearForm()
        {
            Assembly = null;
            if(!chkAllStores.Checked)
            {
                cmbStore.SelectedData = new DataEntity("", "");
            }

            cmbCopyFrom.SelectedData = new DataEntity("", "");
            ntbMargin.SetValueWithLimit(0m, priceLimit);
            ntbPrice.SetValueWithLimit(defaultSalesPrice, priceLimit);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            IEnumerable<DataEntity>[] data = new IEnumerable<DataEntity>[]
            {
                retailItemAssemblies,
                Providers.RetailItemAssemblyData.GetAll(PluginEntry.DataModel)
            };

            TabbedDualDataPanel pnl = new TabbedDualDataPanel(
                cmbCopyFrom.SelectedData != null ? cmbCopyFrom.SelectedData.ID : "",
                data,
                new string[] { Properties.Resources.ThisItem, Properties.Resources.All },
                250);

            cmbCopyFrom.SetData(data, pnl);
        }

        private void cmbStore_RequestData(object sender, EventArgs e)
        {
            cmbStore.SetData(Providers.StoreData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbCopyFrom_ClearData(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = new DataEntity("", "");
        }

        private void chkAllStores_CheckedChanged(object sender, EventArgs e)
        {
            cmbStore.Enabled = !chkAllStores.Checked;

            if (chkAllStores.Checked)
            {
                cmbStore.SelectedData = new DataEntity("", "");
            }

            CheckEnabled(sender, e);
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            if(!RecordIdentifier.IsEmptyOrNull(cmbCopyFrom.SelectedDataID))
            {
                CopyFromAssembly = retailItemAssemblies.Find(x => x.ID == cmbCopyFrom.SelectedDataID);

                if(CopyFromAssembly == null)
                {
                    //Check in DB if local assembly is not found
                    CopyFromAssembly = Providers.RetailItemAssemblyData.Get(PluginEntry.DataModel, cmbCopyFrom.SelectedDataID);
                }

                doEvents = false;
                ntbMargin.SetValueWithLimit(CopyFromAssembly.Margin, priceLimit);
                ntbPrice.SetValueWithLimit(CopyFromAssembly.Price, priceLimit);
                ntbTotalCost.SetValueWithLimit(CopyFromAssembly.TotalCost, priceLimit);
                chkShowComponentsOnPos.Checked = CopyFromAssembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnPOS);
                chkShowComponentsOnReceipt.Checked = CopyFromAssembly.ShallDisplayWithComponents(ExpandAssemblyLocation.OnReceipt);
                cmbSendComponentsToKds.SelectedData = new DataEntity(
                    (int)CopyFromAssembly.SendAssemblyComponentsToKds,
                    PluginOperations.GetSendToKdsDisplayName(CopyFromAssembly.SendAssemblyComponentsToKds));

                chkCalculatePriceFromComponents.Checked = CopyFromAssembly.CalculatePriceFromComponents;

                doEvents = true;
            }
            else
            {
                CopyFromAssembly = null;
            }
        }

        private void ntbPrice_TextChanged(object sender, EventArgs e)
        {
            if (!doEvents) return;

            decimal totalCost = GetTotalCost();

            doEvents = false;

            ntbMargin.SetValueWithLimit(PluginOperations.CalculateProfitMargin((decimal)ntbPrice.Value, totalCost), priceLimit);

            if(!chkCalculatePriceFromComponents.Checked)
            {
                lastSalePrice = (decimal)ntbPrice.Value;
            }

            CheckEnabled(this, EventArgs.Empty);
            doEvents = true;
        }

        private void ntbMargin_TextChanged(object sender, EventArgs e)
        {
            if (!doEvents) return;

            doEvents = false;
            decimal totalCost = GetTotalCost();

            if (totalCost > 0)
            {
                decimal price = (totalCost * (decimal)ntbMargin.Value / (100 - (decimal)ntbMargin.Value)) + totalCost;
                ntbPrice.SetValueWithLimit(price, priceLimit);
            }

            CheckEnabled(this, EventArgs.Empty);
            doEvents = true;
        }

        private decimal GetTotalCost()
        {
            decimal totalCost = 0;

            if (CopyFromAssembly != null)
            {
                totalCost = CopyFromAssembly.TotalCost;
            }
            else if (Assembly != null)
            {
                totalCost = Assembly.TotalCost;
            }

            return totalCost;
        }

        private void cbCreateAnother_CheckedChanged(object sender, EventArgs e)
        {
            CreateAnother = cbCreateAnother.Checked;
        }

        private void chkCalculatePriceFromComponents_CheckedChanged(object sender, EventArgs e)
        {
            if(chkCalculatePriceFromComponents.Checked)
            {
                ntbPrice.SetValueWithLimit(Assembly == null ? 0 : Assembly.TotalSalesPrice, priceLimit);
                ntbPrice.Enabled = false;

                chkShowComponentsOnPos.Checked = true;
                chkShowComponentsOnPos.Enabled = false;

                chkShowComponentsOnReceipt.Checked = true;
                chkShowComponentsOnReceipt.Enabled = false;
            }
            else
            {
                ntbPrice.SetValueWithLimit(lastSalePrice, priceLimit);
                ntbPrice.Enabled = true;

                chkShowComponentsOnPos.Enabled = true;
                chkShowComponentsOnReceipt.Enabled = true;
            }

            CheckEnabled(sender, e);
        }

        private void cmbShowComponentsOnKds_RequestData(object sender, EventArgs e)
        {
            cmbSendComponentsToKds.SetData(kdsComponentOptions, null);
        }
    }
}
