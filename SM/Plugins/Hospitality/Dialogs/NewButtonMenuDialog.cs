using System;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.TouchButtons;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using LSOne.Controls;
using System.Collections.Generic;
using System.Linq;

namespace LSOne.ViewPlugins.Hospitality.Dialogs
{
    public partial class NewButtonMenuDialog : DialogBase
    {
        private RecordIdentifier posMenuHeaderID;
        private PosMenuHeader posMenuHeader;
        private PosOperation noOperation;
        private PosMenuHeader noCopyHeader;
        private int defaultOperation;

        public NewButtonMenuDialog()
        {
            InitializeComponent();

            posMenuHeaderID = RecordIdentifier.Empty;

            noOperation = new PosOperation();
            noOperation.ID = RecordIdentifier.Empty;
            noOperation.Text = Properties.Resources.NoOperation;

            noCopyHeader = new PosMenuHeader();
            noCopyHeader.ID = RecordIdentifier.Empty;
            noCopyHeader.Text = Properties.Resources.DoNotCopyMenuHeader;

            cmbOperation.SelectedData = noOperation;
            cmbCopyFrom.SelectedData = noCopyHeader;
            cmbStyle.SelectedData = new DataEntity(RecordIdentifier.Empty, "");
            cmbAppliesTo.SelectedIndex = 0;
            defaultOperation = -1;
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

        public RecordIdentifier PosMenuHeaderID
        {
            get { return posMenuHeaderID; }
        }

        public RecordIdentifier CopyFromPosMenuHeaderID { get; private set; }

        public PosMenuHeader MenuHeader
        {
            get
            {
                return posMenuHeader;
            }
        }

        public RecordIdentifier SelectedOperationID
        {
            get
            {
                return cmbOperation.SelectedData.ID;
            }
        }

        private void Save()
        {
            if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
            {
                posMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, cmbCopyFrom.SelectedData.ID);
                posMenuHeader.ID = RecordIdentifier.Empty;
                CopyFromPosMenuHeaderID = cmbCopyFrom.SelectedData.ID;
            }

            posMenuHeader.Text = tbDescription.Text;
            posMenuHeader.Columns = (int)ntbColumns.Value;
            posMenuHeader.Rows = (int)ntbRows.Value;
            posMenuHeader.MainMenu = chkMainMenu.Checked;

            posMenuHeader.StyleID = RecordIdentifier.IsEmptyOrNull(cmbStyle.SelectedData.ID) ? RecordIdentifier.Empty : cmbStyle.SelectedData.ID;
            posMenuHeader.DefaultOperation = cmbOperation.SelectedData.ID;
            posMenuHeader.AppliesTo = (PosMenuHeader.AppliesToEnum)cmbAppliesTo.SelectedIndex;
            posMenuHeader.UseNavOperation = chkUseNavOperation.Checked;

            Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, posMenuHeader);
        }

        // Loads data from the data entity to controls
        private void LoadData()
        {
            posMenuHeader = new PosMenuHeader();
            posMenuHeader.ID = RecordIdentifier.Empty;
            posMenuHeader.MenuType = MenuTypeEnum.Hospitality;

            if (defaultOperation != -1)
            {
                cmbOperation.SelectedData = Providers.PosOperationData.Get(PluginEntry.DataModel, defaultOperation);
                cmbOperation.Enabled = false;
            }

            if (cmbOperation.SelectedData == null)
            {
                cmbOperation.SelectedData = noOperation;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = IsModified();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();
            posMenuHeaderID = posMenuHeader.ID;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool IsModified()
        {
            //The description cannot be empty
            if (string.IsNullOrWhiteSpace(tbDescription.Text)) return false;

            // Attributes
            if ((int)ntbColumns.Value > 0) return true;
            if ((int)ntbRows.Value > 0) return true;

            return false;
        }

        private void cmbOperation_RequestData(object sender, EventArgs e)
        {
            var operationList = Providers.PosOperationData.GetUserOperations(PluginEntry.DataModel).OrderBy(x => x.Text).ToList();

            operationList.Insert(0, noOperation);

            cmbOperation.SetData(operationList, null);
        }

        private void cmbOperation_RequestClear(object sender, EventArgs e)
        {
            ((DualDataComboBox)sender).SelectedData = noOperation;
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            if (cmbCopyFrom.SelectedData.ID != RecordIdentifier.Empty)
            {
                PosMenuHeader copyFromMenuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, cmbCopyFrom.SelectedData.ID);

                chkMainMenu.Checked = copyFromMenuHeader.MainMenu;
                ntbColumns.Value = copyFromMenuHeader.Columns;
                ntbRows.Value = copyFromMenuHeader.Rows;
                cmbOperation.SelectedData = Providers.PosOperationData.Get(PluginEntry.DataModel, copyFromMenuHeader.DefaultOperation) ?? noOperation;
                cmbStyle.SelectedData = Providers.StyleProfileData.Get(PluginEntry.DataModel, copyFromMenuHeader.StyleID) ?? new DataEntity();

                chkMainMenu.Enabled =
                    ntbColumns.Enabled =
                    ntbRows.Enabled =
                    cmbOperation.Enabled = false;
            }
            else
            {
                chkMainMenu.Enabled =
                    ntbColumns.Enabled =
                    ntbRows.Enabled =
                    cmbOperation.Enabled = true;
            }
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            PosMenuHeaderFilter filter = new PosMenuHeaderFilter();
            filter.MenuType = (int)MenuTypeEnum.Hospitality;
            filter.SortBy = PosMenuHeaderSorting.MenuDescription;
            List<PosMenuHeader> posMenus = Providers.PosMenuHeaderData.GetList(PluginEntry.DataModel, filter);

            posMenus.Insert(0, noCopyHeader);

            ((DualDataComboBox)sender).SetData(posMenus, null);
        }

        private void cmbCopyFrom_RequestClear(object sender, EventArgs e)
        {
            cmbCopyFrom.SelectedData = noCopyHeader;
        }

        private void cmbStyle_RequestData(object sender, EventArgs e)
        {
            cmbStyle.SetData(Providers.PosStyleData.GetList(PluginEntry.DataModel), null);
        }

        private void cmbStyle_RequestClear(object sender, EventArgs e)
        {
            cmbStyle.SelectedData = new DataEntity("", "");
        }

        private void btnEditStyle_Click(object sender, EventArgs e)
        {
            RecordIdentifier styleID = PluginOperations.ShowEditStyleDialog(cmbStyle.SelectedData.ID);

            if(styleID != RecordIdentifier.Empty)
            {
                cmbStyle.SelectedData = Providers.PosStyleData.Get(PluginEntry.DataModel, styleID);
            }
        }

        private void cmbStyle_SelectedDataChanged(object sender, EventArgs e)
        {
            btnEditStyle.Enabled = cmbStyle.SelectedData.ID.StringValue != "";
        }
    }
}
