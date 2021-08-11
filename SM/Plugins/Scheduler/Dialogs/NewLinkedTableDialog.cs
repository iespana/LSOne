using System;
using System.Windows.Forms;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.DataLayer.DDDataProviders;
using LSOne.ViewCore.Dialogs;

namespace LSOne.ViewPlugins.Scheduler.Dialogs
{
    public partial class NewLinkedTableDialog : DialogBase
    {
        private JscTableDesign fromTable;

        private class Item
        {
            public JscTableDesign TableDesign { get; set; }

            public override string ToString()
            {
                return TableDesign.TableName;
            }
        }

        public NewLinkedTableDialog()
        {
            InitializeComponent();
        }


        private new DialogResult ShowDialog(IWin32Window owner)
        {
            return base.ShowDialog(owner);
        }

        public DialogResult ShowDialog(IWin32Window owner,  JscTableDesign fromTable)
        {
            this.fromTable = fromTable;

            PopulateComboBox();
            UpdateActions();
            Header = string.Format(Properties.Resources.LinkedTableHeaderMsg, fromTable.TableName);
            return ShowDialog(owner);
        }

        private void PopulateComboBox()
        {
            Cursor.Current = Cursors.WaitCursor;
            cmbTableDesigns.BeginUpdate();
            cmbTableDesigns.Text = null;
            cmbTableDesigns.Items.Clear();
            foreach (var tableDesign in DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().GetTableDesigns(PluginEntry.DataModel, fromTable.DatabaseDesign, false))
            {
                if (tableDesign.TableName != fromTable.TableName)
                {
                    cmbTableDesigns.Items.Add(new Item { TableDesign = tableDesign });
                }
            }
            cmbTableDesigns.EndUpdate();
        }

        private void UpdateActions()
        {
            btnOK.Enabled =
                cmbTableDesigns.SelectedItem != null &&
                StringComparer.InvariantCultureIgnoreCase.Compare(((Item)cmbTableDesigns.SelectedItem).TableDesign.TableName, cmbTableDesigns.Text) == 0;
        }


        public JscTableDesign SelectedTableDesign
        {
            get
            {
                if (cmbTableDesigns.SelectedItem != null)
                    return ((Item)cmbTableDesigns.SelectedItem).TableDesign;
                return null;
            }
        }

        private void cmbTableDesigns_SelectedValueChanged(object sender, EventArgs e)
        {
            errorProvider.SetError(cmbTableDesigns, string.Empty);
            UpdateActions();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().ExistsLinkedTable(PluginEntry.DataModel, fromTable.ID, SelectedTableDesign.ID))
            {
                errorProvider.SetError(cmbTableDesigns, Properties.Resources.LinkedTableAlreadyExistsMsg);
                cmbTableDesigns.Focus();
                return;
            }

            if (!DataProviderFactory.Instance.Get<IDesignData, JscTableDesign>().HasFields(PluginEntry.DataModel, SelectedTableDesign.ID))
            {
                errorProvider.SetError(cmbTableDesigns, Properties.Resources.LinkedTableFieldsMissingMsg);
                cmbTableDesigns.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

    }
}
