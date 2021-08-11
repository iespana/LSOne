using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Hospitality;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Hospitality;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class PrintingStationDialog : DialogBase
    {
        PrintingStation printingStation;
           

        internal PrintingStationDialog()
        {
            InitializeComponent();
          
        }

        internal PrintingStationDialog(RecordIdentifier kitchenDisplayStationId)
            : this()
        {
            PrintingStationId = kitchenDisplayStationId;
            printingStation = Providers.PrintingStationData.Get(PluginEntry.DataModel, kitchenDisplayStationId);

            tbStationName.Text = printingStation.Text;
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier PrintingStationId { get; private set; }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            CheckEnabled(e);
        }

        private void CheckEnabled(EventArgs e)
        {
            if (printingStation == null)
            {
                btnOK.Enabled = tbStationName.Text.Length > 0;
            }
            else
            {
                btnOK.Enabled = tbStationName.Text.Length > 0 && printingStation.Text != tbStationName.Text;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (printingStation == null)
            {
                printingStation = new PrintingStation();
            }
            printingStation.Text = tbStationName.Text;

            Providers.PrintingStationData.Save(PluginEntry.DataModel, printingStation);

            PrintingStationId = printingStation.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }        
    }
}
