using System;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class NewTableMapDialog : DialogBase
    {
        public NewTableMapDialog()
        {
            InitializeComponent();
        }


        private void NewTableMapDialog_Load(object sender, EventArgs e)
        {
            tvDesignFrom.Focus();
        }

        private void NewTableMapDialog_Shown(object sender, EventArgs e)
        {
            tvDesignFrom.LoadData();
            tvDesignTo.LoadData();
            UpdateActions();
        }

        private void tvDesignFrom_DatabaseDesignSelected(object sender, Controls.DatabaseDesignEventArgs e)
        {
            UpdateActions();
        }

        private void tvDesignFrom_TableDesignSelected(object sender, Controls.TableDesignEventArgs e)
        {
            UpdateActions();
        }

        private void tvDesignTo_DatabaseDesignSelected(object sender, Controls.DatabaseDesignEventArgs e)
        {
            UpdateActions();
        }

        private void tvDesignTo_TableDesignSelected(object sender, Controls.TableDesignEventArgs e)
        {
            UpdateActions();
        }

        private void UpdateActions()
        {
            bool hasTableDesignFrom = tvDesignFrom.SelectedTableDesign != null;
            bool hasTableDesignTo = tvDesignTo.SelectedTableDesign != null;
            bool hasDatabaseDesignTo = tvDesignTo.SelectedDatabaseDesign != null;

            btnOK.Enabled = hasTableDesignFrom && hasTableDesignTo;
            btnSearch.Enabled = hasTableDesignFrom && (hasDatabaseDesignTo || hasTableDesignTo);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var designFrom = tvDesignFrom.SelectedTableDesign;
            if (designFrom == null)
                return;

            if (tvDesignTo.FindTableDesign(designFrom.TableName))
            {
                tvDesignFrom.Focus();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            TableMap = new JscTableMap();
            TableMap.FromJscTableDesign = tvDesignFrom.SelectedTableDesign;
            TableMap.ToJscTableDesign = tvDesignTo.SelectedTableDesign;
            DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().Save(PluginEntry.DataModel, TableMap);

            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        public JscTableMap TableMap { get; private set; }


    }
}
