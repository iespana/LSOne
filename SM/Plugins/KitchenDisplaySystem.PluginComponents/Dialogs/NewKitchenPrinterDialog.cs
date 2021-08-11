using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSRetail.PrintingStationClient;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class NewKitchenPrinterDialog : DialogBase
    {
        KitchenDisplayPrinter printer;

        public NewKitchenPrinterDialog()
        {
            InitializeComponent();
            cmbPrinterName.SelectedData = new DataEntity("", "");
            cmbVisualProfile.SelectedData = new DataEntity(Guid.Empty, "");
            btnEditHosts.Visible = PluginEntry.Framework.CanRunOperation("EditPrintingStations");
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier Id
        {
            get { return printer.ID; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = (cmbPrintingStationHost.SelectedData != null)
                && (cmbVisualProfile.SelectedDataID != Guid.Empty) 
                && (cmbPrinterName.Text != "")
                && (tbDescription.Text != ""); 

            errorProvider1.Clear();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            printer = new KitchenDisplayPrinter();

            printer.Text = tbDescription.Text;
            printer.HostId = (string)cmbPrintingStationHost.SelectedDataID;
            printer.PrinterName = cmbPrinterName.Text;
            printer.VisualProfileId = (Guid)cmbVisualProfile.SelectedData.ID;

            Providers.KitchenDisplayPrinterData.Save(PluginEntry.DataModel, printer);
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Add, "KitchenPrinter", printer.ID, printer);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbPrintingStationHost_RequestData(object sender, EventArgs e)
        {
            cmbPrintingStationHost.SetData(Providers.StationPrintingHostData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbPrinterName_RequestData(object sender, EventArgs e)
        {
            var host = Providers.StationPrintingHostData.Get(PluginEntry.DataModel, cmbPrintingStationHost.SelectedDataID);
            PrintingStationCli cli = new PrintingStationCli(host.Address, host.Port);
            string[] list = cli.GetPrinterList();

            if (list.Length == 0)
            {
                string err = string.Format(Properties.Resources.ErrorCannotLoadDeviceList, host.Address);
                MessageDialog.Show(string.Format("{0} [{1}]", err, cli.GetLastPrintMessage()));
                cmbPrinterName.Text = string.Empty;
                cmbPrinterName.Clear();
                return;
            }

            List<DataEntity> devlist = new List<DataEntity>();
            foreach (string dev in list)
            {
                devlist.Add(new DataEntity("", dev));
            }
            cmbPrinterName.SetData(devlist, null);
        }

        private void cmbPrintingStationHost_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbPrintingStationHost.SelectedData != null)
            {
                cmbPrinterName.Enabled = true;
            }
            CheckEnabled(sender, e);
        }

        private void btnEditHosts_Click(object sender, EventArgs e)
        {
            PluginEntry.Framework.RunOperation("EditPrintingStations", this, new PluginOperationArguments(cmbPrintingStationHost.SelectedDataID, null) );
        }

        private void cmbVisualProfile_RequestData(object sender, EventArgs e)
        {
            cmbVisualProfile.SetData(Providers.KitchenDisplayVisualProfileData.GetList(PluginEntry.DataModel), null);
        }
    }
}
