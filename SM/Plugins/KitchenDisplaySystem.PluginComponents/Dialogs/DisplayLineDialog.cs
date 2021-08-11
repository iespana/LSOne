using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using static LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayStation;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class DisplayLineDialog : DialogBase
    {
        private KitchenDisplayLine displayLine;
        KitchenDisplayProfile displayProfile;

        public RecordIdentifier ID
        {
            get { return displayLine.ID; }
        }

        internal DisplayLineDialog(RecordIdentifier displayLineID, KitchenDisplayProfile displayProfile)
        {
            InitializeComponent();
            this.displayProfile = displayProfile;

            SetData(displayLineID);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void SetData(RecordIdentifier displayLineID)
        {
            cmbLineType.Items.Clear();
            cmbLineType.Items.Add(DisplayRowTypeEnum.ChitHeader);
            cmbLineType.Items.Add(DisplayRowTypeEnum.ChitFooter);

            if (displayLineID == null)
            {
                cmbLineType.SelectedIndex = 0;

                displayLine = new KitchenDisplayLine();
                displayLine.ID = new RecordIdentifier(Guid.NewGuid());
                displayLine.DisplayProfileID = displayProfile.ID;
                displayLine.Type = (DisplayRowTypeEnum)cmbLineType.SelectedIndex;
            }
            else
            {
                displayLine = Providers.KitchenDisplayLineData.Get(PluginEntry.DataModel, displayLineID, displayProfile.ID);
                cmbLineType.Text = displayLine.GetTypeText();
                cmbLineType.SelectedItem = displayLine.Type;
            }
        }

        private bool Save()
        {
            displayLine.Type = (DisplayRowTypeEnum)cmbLineType.SelectedItem;
            Providers.KitchenDisplayLineData.Save(PluginEntry.DataModel, displayLine);
            return true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Save())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
