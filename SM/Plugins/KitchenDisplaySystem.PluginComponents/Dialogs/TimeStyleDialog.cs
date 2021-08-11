using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.KDSBusinessObjects;

using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class TimeStyleDialog : DialogBase
    {
        private RecordIdentifier styleProfileId;
        private RecordIdentifier timeStyleId;
        private KitchenDisplayTimeStyle timeStyle;
        private RecordIdentifier originalTimeStyleId;

        public TimeStyleDialog(RecordIdentifier styleProfileId)
        {
            InitializeComponent();
            this.styleProfileId = styleProfileId;

            cmbOrderStyle.SelectedData = new DataEntity(RecordIdentifier.Empty,"");
            cmbOrderStyle.Tag = PluginEntry.ChitGuid;
            btnsOrderStyle.SetBuddyControl(cmbOrderStyle);
        }

        public TimeStyleDialog(RecordIdentifier styleProfileId, RecordIdentifier timeStyleId)
            : this(styleProfileId)
        {
            this.timeStyleId = timeStyleId;
            timeStyle = Providers.KitchenDisplayTimeStyleData.Get(PluginEntry.DataModel, timeStyleId);
            originalTimeStyleId = timeStyle.ID;
            ntbSecondsUntilActive.Value = timeStyle.SecondsPassed;
            cmbOrderStyle.SelectedData = timeStyle.UiStyle;
            btnsOrderStyle.SetBuddyControl(cmbOrderStyle);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier TimeStyleId
        {
            get { return timeStyleId; }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (timeStyle == null)
            {
                btnOK.Enabled = cmbOrderStyle.SelectedData.ID != RecordIdentifier.Empty && ntbSecondsUntilActive.Value != 0;
            }
            else
            {
                btnOK.Enabled = (cmbOrderStyle.SelectedData.ID != timeStyle.StyleId || ntbSecondsUntilActive.Value != timeStyle.SecondsPassed)
                                && ntbSecondsUntilActive.Value != 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (timeStyle == null)
            {
                timeStyle = new KitchenDisplayTimeStyle();
                timeStyle.KdStyleProfileId = (Guid)styleProfileId;

                // Set the original Id as the new ID because we are adding a new one and there is no original ID
                originalTimeStyleId = new RecordIdentifier(styleProfileId,(int) ntbSecondsUntilActive.Value);
            }

            timeStyle.SecondsPassed = (int)ntbSecondsUntilActive.Value;
            timeStyle.StyleId = cmbOrderStyle.SelectedData.ID;

            Providers.KitchenDisplayTimeStyleData.Save(PluginEntry.DataModel, timeStyle, originalTimeStyleId);

            timeStyleId = timeStyle.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cmbOrderStyle_RequestData(object sender, EventArgs e)
        {
            cmbOrderStyle.SetData(Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME"), null);
        }

    }
}
