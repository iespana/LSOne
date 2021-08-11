using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.Profiles;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Profiles.ViewPages.Hardware
{
    public partial class HardwareProfileFiscalPrinterPage : UserControl, ITabView
    {
        private HardwareProfile profile;
        List<string> OPOSPrinterList;
        List<string> windowsPrinterList;

        public HardwareProfileFiscalPrinterPage()
        {
            InitializeComponent();
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new HardwareProfileFiscalPrinterPage();
        }

        protected override void OnLoad(EventArgs e)
        {
            OPOSPrinterList = PluginOperations.GetRegistryStrings("POSPrinter");
            windowsPrinterList = PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        }

        #region ITabView Members

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            profile = (HardwareProfile)internalContext;

            cmbPrinter.SelectedIndex = profile.FiscalPrinter;
            cmbDeviceName.Text = profile.FiscalPrinterConnection;
            tbDescription.Text = profile.FiscalPrinterDescription;
        }

        public bool DataIsModified()
        {
            if (cmbPrinter.SelectedIndex != profile.FiscalPrinter) return true;
            if (cmbDeviceName.Text != profile.FiscalPrinterConnection) return true;
            if (tbDescription.Text != profile.FiscalPrinterDescription) return true;

            return false;
        }

        public bool SaveData()
        {
            profile.FiscalPrinter = cmbPrinter.SelectedIndex;
            profile.FiscalPrinterConnection = cmbDeviceName.Text;
            profile.FiscalPrinterDescription = tbDescription.Text;

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

        private void cmbPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((HardwareProfile.PrinterHardwareTypes)cmbPrinter.SelectedIndex)
            {
                case HardwareProfile.PrinterHardwareTypes.Windows:
                    cmbDeviceName.Items.Clear();
                    cmbDeviceName.Items.AddRange(windowsPrinterList.ToArray());
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = true;
                    break;
                case HardwareProfile.PrinterHardwareTypes.OPOS:
                    cmbDeviceName.Items.Clear();
                    cmbDeviceName.Items.AddRange(OPOSPrinterList.ToArray());
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = true;
                    break;
                case HardwareProfile.PrinterHardwareTypes.None:
                    cmbDeviceName.Text = "";
                    cmbDeviceName.Enabled = false;
                    break;
            }
        }
    }
}
