//style class here
using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.TouchButtons.Dialogs
{
    public partial class NewStyle : DialogBase
    {        
        private bool suspendEvents;
        private bool editingNewPosMenuHeader;
        private MenuTypeEnum type;

        public NewStyle(RecordIdentifier posStyleID)
            : this()
        {
            if (posStyleID != RecordIdentifier.Empty)
            {
                PosStyleID = posStyleID;
                editingNewPosMenuHeader = false;
                pnlCopyFrom.Visible = false;
                Text = Properties.Resources.EditStyle;
            }
        }

        public NewStyle(MenuTypeEnum type)
            : this()
        {
            this.type = type;
        }

        public NewStyle()
        {
            InitializeComponent();

            suspendEvents = false;
            PosStyleID = RecordIdentifier.Empty;
            editingNewPosMenuHeader = true;
            copyStyleID = RecordIdentifier.Empty;

            // No style allowed for this use
            buttonProperties.EnableStyleUse = false;
            buttonProperties.PosStyleID = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadData();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier PosStyleID { get; private set; }
        public RecordIdentifier copyStyleID;
        public PosStyle Style { get; private set; }        

        // Returns true if the user has made modifications
        private bool IsModified()
        {
            // General
            if (tbDescription.Text != Style.Text) return true;    

            // Button attributes
            if (buttonProperties.IsModified(Style))
                return true;

            return false;
        }

        // Saves the data entity into the database
        private void Save()
        {
            // General
            Style.Text = tbDescription.Text;

            // Button attributes
            buttonProperties.ToStyle(Style);

            Providers.PosStyleData.Save(PluginEntry.DataModel, Style);
        }        

        // Loads data from the data entity to controls
        private void LoadData()
        {
            if (Style == null)
            {
                if (copyStyleID == RecordIdentifier.Empty && editingNewPosMenuHeader)
                {
                    Style = new PosStyle {ID = RecordIdentifier.Empty};
                }
                else
                {
                    Style = Providers.PosStyleData.Get(PluginEntry.DataModel,
                                             copyStyleID != RecordIdentifier.Empty ? copyStyleID : PosStyleID);
                    if (copyStyleID != RecordIdentifier.Empty)
                    {
                        Style.ID = RecordIdentifier.Empty;
                        Style.IsSystemStyle = false;
                        Style.Guid = Guid.Empty;
                        Style.ImportDateTime = null;
                    }
                }
            }

            chkSystemStyle.Checked = 
            btnReset.Visible = Style.IsSystemStyle && SystemStyles.IsSystemStyle(Style.Text);
            tbDescription.Enabled = !(Style.IsSystemStyle && SystemStyles.IsSystemStyle(Style.Text));

            suspendEvents = true;

            // General
            if (string.IsNullOrEmpty(tbDescription.Text))
                tbDescription.Text = Style.Text;        

            // Button attributes
            buttonProperties.PosStyle = Style;

            SetPreviewButton();

            suspendEvents = false;
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            if (suspendEvents)
                return;

            errorProvider1.Clear();
            btnOK.Enabled = IsModified();
            SetPreviewButton();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();
            PosStyleID = Style.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == QuestionDialog.Show(Properties.Resources.ResetToDefault))
            {
                var id = Style.ID;
                Style = SystemStyles.GetDefaults(Style.Text);
                Style.ID = id;
                CheckEnabled(this, EventArgs.Empty);
                LoadData();
            }
        }

        private void SetPreviewButton()
        {
            buttonProperties.ToButton(btnMenuButtonPreview, 1, btnMenuButtonPreview.BorderColor);
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            var styleProfiles = Providers.PosStyleData.GetList(PluginEntry.DataModel, "NAME ASC");
            cmbCopyFrom.SetData(styleProfiles, null);
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            copyStyleID = cmbCopyFrom.SelectedData.ID;
            Style = null;
            LoadData();
        }

        private void cmbCopyFrom_RequestClear(object sender, EventArgs e)
        {
            cmbCopyFrom.SelectedData = null;
            copyStyleID = RecordIdentifier.Empty;
            LoadData();
        }

        private void OnButtonPropertiesModified(object sender, EventArgs e)
        {
            if (Style != null)
                CheckEnabled(sender, e);
        }
    }
}

