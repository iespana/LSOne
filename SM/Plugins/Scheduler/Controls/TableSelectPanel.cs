using System;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.DDBusinessObjects;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    public partial class TableSelectPanel : UserControl, IControlClosable
    {
        private JscTableDesign preSelectedTableDesign;

        private bool hasBeenPainted;

        #pragma warning disable 0067 // We suppress this warning until we actually implement the RequestClear on all forms but its needed for the interface to have it in until then.
        public event EventHandler RequestClear;
        public event EventHandler RequestNoChange;
#pragma warning restore 0067

        private TableSelectPanel()
        {
            InitializeComponent();
            databaseDesignTree.LoadData();
        }

        public TableSelectPanel(JscTableDesign selectedTableDesign)
            : this()
        {
            preSelectedTableDesign = selectedTableDesign;
        }

        public TableSelectPanel( string filter)
            : this()
        {
            tbFilter.Text = filter;
        }

        public bool SelectNoneAllowed { get; set; }
        public bool NoChangeAllowed { get; set; }

        private void TableSelectPanel_Load(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void UpdateActions()
        {
            btnOK.Enabled = databaseDesignTree.SelectedItem != null && databaseDesignTree.SelectedItem.ItemType == DatabaseDesignTree.ItemType.TableDesign;
        }

        private void databaseDesignTree_SelectedItemChanged(object sender, EventArgs e)
        {
            UpdateActions();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (databaseDesignTree.SelectedItem.ItemType == DatabaseDesignTree.ItemType.TableDesign)
            {
                var winForm = this.FindForm();
                var dropDownForm = (IDropDownForm)winForm;
                dropDownForm.SelectedData = databaseDesignTree.SelectedItem.Object as JscTableDesign;
                dropDownForm.Close();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            var winForm = this.FindForm();
            var dropDownForm = (IDropDownForm)winForm;
            dropDownForm.Close();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            databaseDesignTree.FilterTableName = tbFilter.Text;
        }

        private void TableSelectPanel_Paint(object sender, PaintEventArgs e)
        {
            if (!hasBeenPainted)
            {
                if (preSelectedTableDesign != null)
                {
                    databaseDesignTree.Focus();
                    databaseDesignTree.SelectItem((Guid)preSelectedTableDesign.DatabaseDesign, (Guid)preSelectedTableDesign.ID);

                }
                hasBeenPainted = true;
            }
        }

        private void databaseDesignTree_ItemDoubleClicked(object sender, EventArgs e)
        {
            if (btnOK.Enabled)
            {
                btnOK_Click(btnOK, e);
            }
        }



        void IControlClosable.OnClose()
        {
        }

        Control IControlClosable.EmbeddedControl
        {
            get { return this; }
        }

        private void databaseDesignTree_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPress(e);
        }

        private void HandleKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && btnOK.Enabled)
            {
                btnOK_Click(btnOK, EventArgs.Empty);
            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPress(e);
        }

        private void btnOK_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPress(e);
        }

        private void TableSelectPanel_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPress(e);
        }
    }
}
