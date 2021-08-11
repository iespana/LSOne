using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;
using LSOne.ViewCore.Interfaces;
using LSRetail.PrintingStationClient;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.ViewPages
{
    internal partial class KitchenPrinterGeneralPage : UserControl, ITabView
    {
        KitchenDisplayPrinter printer;

        public KitchenPrinterGeneralPage()
        {
            InitializeComponent();
            btnEditHosts.Visible = PluginEntry.Framework.CanRunOperation("EditPrintingStations");

            cmbStationType.Items.Clear();
            foreach (var mode in Enum.GetValues(typeof(KitchenDisplayStation.StationTypeEnum)))
            {
                cmbStationType.Items.Add(
                    KitchenDisplayStation.GetStationTypeText((KitchenDisplayStation.StationTypeEnum)mode));
            }
            cmbLayout.Items.Clear();
            foreach (var mode in Enum.GetValues(typeof(KitchenDisplayStation.ChitLayoutEnum)))
            {
                cmbLayout.Items.Add(
                    KitchenDisplayStation.GetLayoutText((KitchenDisplayStation.ChitLayoutEnum)mode));
            }
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new KitchenPrinterGeneralPage();
        }

        #region ITabPanel Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            printer = (KitchenDisplayPrinter)internalContext;

            tbDescription.Text = printer.Text;
            cmbPrintingStationHost.SelectedData = new DataEntity(printer.HostId, printer.HostDescription);
            cmbPrinterName.SelectedData = new DataEntity("", printer.PrinterName);
            cmbVisualProfile.SelectedData = new DataEntity(printer.VisualProfileId, printer.VisualProfileDescription);
            cbCutPaper.Checked = printer.CutPaper;
            tbOposMaxChars.Text = Convert.ToString(printer.OPosMaxChars);
            tbOposFontSizeV.Text = Convert.ToString(printer.OPosFontSizeV);
            tbOposFontSizeH.Text = Convert.ToString(printer.OPosFontSizeH);
            if (printer.OPosLineSpacing <= 30) printer.OPosLineSpacing = 30;
            tbOposLineSpacing.Text = Convert.ToString(printer.OPosLineSpacing);
            cmbStationType.SelectedIndex = (int)printer.StationType;
            cmbLayout.SelectedIndex = (int)printer.LayoutType;
            topSpaceTextBox.Text = Convert.ToString(printer.TopSpaces);
            bottomSpaceTextBox.Text = Convert.ToString(printer.BottomSpaces);
            cbPrintToLogFile.Checked = printer.DirectOutputToLog;
        }

        public bool DataIsModified()
        {
            if (Convert.ToInt32(tbOposLineSpacing.Text) < 30)
            {
                tbOposLineSpacing.Text = "30";
            }
            if (Convert.ToInt32(tbOposLineSpacing.Text) > 255)
            {
                tbOposLineSpacing.Text = "255";
            }
            if (tbDescription.Text != printer.Text) return true;
            if (cmbPrintingStationHost.SelectedData.ID != printer.HostId) return true;
            if (cmbPrinterName.Text != printer.PrinterName) return true;
            if (cmbVisualProfile.SelectedData.ID != printer.VisualProfileId) return true;
            if (cbCutPaper.Checked != printer.CutPaper) return true;
            if (tbOposMaxChars.Text != Convert.ToString(printer.OPosMaxChars)) return true;
            if (tbOposFontSizeV.Text != Convert.ToString(printer.OPosFontSizeV)) return true;
            if (tbOposFontSizeH.Text != Convert.ToString(printer.OPosFontSizeH)) return true;
            if (tbOposLineSpacing.Text != Convert.ToString(printer.OPosLineSpacing)) return true;
            if (cmbStationType.SelectedIndex != (int)printer.StationType) return true;
            if (cmbLayout.SelectedIndex != (int)printer.LayoutType) return true;
            if (topSpaceTextBox.Text != Convert.ToString(printer.TopSpaces)) return true;
            if (bottomSpaceTextBox.Text != Convert.ToString(printer.BottomSpaces)) return true;
            if (cbPrintToLogFile.Checked != printer.DirectOutputToLog) return true;

            return false;
        }

        public bool SaveData()
        {
            printer.Text = tbDescription.Text;
            printer.HostId = (string)cmbPrintingStationHost.SelectedDataID;
            printer.PrinterName = cmbPrinterName.Text;
            printer.VisualProfileId = (Guid)cmbVisualProfile.SelectedDataID;
            printer.CutPaper = cbCutPaper.Checked;
            printer.OPosMaxChars = Convert.ToInt16(tbOposMaxChars.Text);
            printer.OPosFontSizeV = Convert.ToInt16(tbOposFontSizeV.Text);
            printer.OPosFontSizeH = Convert.ToInt16(tbOposFontSizeH.Text);
            printer.OPosLineSpacing = Convert.ToInt16(tbOposLineSpacing.Text);
            if (printer.OPosLineSpacing < 30) printer.OPosLineSpacing = 30;
            if (printer.OPosLineSpacing > 255) printer.OPosLineSpacing = 255;
            printer.StationType = (KitchenDisplayStation.StationTypeEnum)cmbStationType.SelectedIndex;
            printer.LayoutType = (KitchenDisplayStation.ChitLayoutEnum)cmbLayout.SelectedIndex;
            printer.TopSpaces = Convert.ToInt16(topSpaceTextBox.Text);
            printer.BottomSpaces = Convert.ToInt16(bottomSpaceTextBox.Text);
            printer.DirectOutputToLog = cbPrintToLogFile.Checked;
            return true;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {

        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {

        }

        public void OnClose()
        {

        }

        public void SaveUserInterface()
        {
        }

        #endregion

        private void btnEditHosts_Click(object sender, System.EventArgs e)
        {
            PluginEntry.Framework.RunOperation("EditPrintingStations", this, new PluginOperationArguments(cmbPrintingStationHost.SelectedDataID, null));
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

        private void cmbVisualProfile_RequestData(object sender, EventArgs e)
        {
            cmbVisualProfile.SetData(Providers.KitchenDisplayVisualProfileData.GetList(PluginEntry.DataModel), null);
        }

        private void btnEditVisualProfile_Click(object sender, EventArgs e)
        {
            PluginOperationsHelper.ShowVisualProfilesView(cmbVisualProfile.SelectedDataID);
        }

        private void cmbStationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stationType = (KitchenDisplayStation.StationTypeEnum)cmbStationType.SelectedIndex;
            switch (stationType)
            {
                case KitchenDisplayStation.StationTypeEnum.ExpediterStation:
                    break;
                case KitchenDisplayStation.StationTypeEnum.CustomerFacingStation:
                    break;
                case KitchenDisplayStation.StationTypeEnum.PrepStation:
                    break;
            }
        }

        private void cmbLayoutType_SelectedIndexChanged(object sender, EventArgs e)
        {
            var layoutType = (KitchenDisplayStation.ChitLayoutEnum)cmbLayout.SelectedIndex;
            switch (layoutType)
            {
                case KitchenDisplayStation.ChitLayoutEnum.Layout1:
                    break;
                case KitchenDisplayStation.ChitLayoutEnum.Layout2:
                    break;
                case KitchenDisplayStation.ChitLayoutEnum.Layout3:
                    break;
            }
        }

        private void btnTestPrint_Click(object sender, EventArgs e)
        {
            var host = Providers.StationPrintingHostData.Get(PluginEntry.DataModel, cmbPrintingStationHost.SelectedDataID);
            PrintingStationCli stPrintClient = new PrintingStationCli(host.Address, host.Port);

            string orderString = "\u001b|1vC\u001b|1hCTestprinter\r\nfriday, 19. august 2016 15:56\r\nEmployee:                                         Jimmy\r\nHospType:                                       Dine-In\r\nTable#:                                          15 / 0\r\nTransNo:                                             23\r\nRecall:                                          Recall\r\nStatus:                                      TestStatus\r\n\r\n\r\n1 Drink \r\n\r\n\r\n\r\n\r\n\r\n\r\n";
            MessageBox.Show(string.Format("Send test to:\nPrinterName {0}\nHostAddress {1}\nHostPort {2}\n\nPress OK to continue", cmbPrinterName.Text, host.Address, host.Port));

            if (!stPrintClient.StationPrintEx(cmbPrinterName.Text, orderString))
                    MessageBox.Show("Test print failed");
            else
                MessageBox.Show("Test print successful");
            stPrintClient.Close();
        }
    }
}
