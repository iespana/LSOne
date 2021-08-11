using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Dialogs
{
    public partial class ButtonDialog : DialogBase
    {
        private RecordIdentifier menuHeaderID;
        private PosMenuLine kitchenDisplayMenuLine;
        private Dictionary<string, DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum> textToEnum = new Dictionary<string, DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum>();

        public ButtonDialog(RecordIdentifier menuHeaderID, RecordIdentifier kitchenDisplayMenuLineID)
            : this()
        {
            this.menuHeaderID = menuHeaderID;
            kitchenDisplayMenuLine = Providers.PosMenuLineData.Get(PluginEntry.DataModel, kitchenDisplayMenuLineID);

            cmbOperation.SelectedItem = DataLayer.KDSBusinessObjects.KitchenDisplayButton.GetButtonText((DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum)(int)kitchenDisplayMenuLine.Operation);
            tbDescription.Text = kitchenDisplayMenuLine.Text;
            tbKeyMapping.Text = kitchenDisplayMenuLine.KeyMapping.ToString();
            tbKeyMapping.Tag = kitchenDisplayMenuLine.KeyMapping;

            cbCreateAnother.Visible = false;
        }

        public ButtonDialog(RecordIdentifier menuHeaderID)
            : this()
        {
            this.menuHeaderID = menuHeaderID;
            cmbOperation.SelectedIndex = 0;
            btnClear_Click(null, null);
        }

        public ButtonDialog()
        {
            InitializeComponent();

            var sortedList = new SortedList<string, string>();
            var values = Enum.GetValues(typeof(DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum));
            foreach (DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum btn in values)
            {
                if ((btn != DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum.NoOperation) &&
                    (btn != DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum.Serve)) // Temporarily remove the Serve action since we do not support it yet.
                {
                    var text = DataLayer.KDSBusinessObjects.KitchenDisplayButton.GetButtonText(btn);
                    textToEnum[text] = btn;
                    while (sortedList.ContainsKey(text))
                        text += "_2"; // Make sure we have a unique key
                    sortedList[text] = text;
                }
            }

            cmbOperation.Items.Clear();
            var noOpText = DataLayer.KDSBusinessObjects.KitchenDisplayButton.GetButtonText(DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum.NoOperation);
            textToEnum[noOpText] = DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum.NoOperation;
            cmbOperation.Items.Add(noOpText);
            foreach (var btnText in sortedList.Values)
                cmbOperation.Items.Add(btnText);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier Id
        {
            get
            {
                return kitchenDisplayMenuLine.ID;
            }
        }

        private DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum SelectedOperation
        {
            get
            {
                if (textToEnum.ContainsKey(cmbOperation.SelectedItem.ToString()))
                    return textToEnum[cmbOperation.SelectedItem.ToString()];

                return DataLayer.KDSBusinessObjects.KitchenDisplayButton.ButtonActionEnum.NoOperation;
            }
        }

        private void CheckEnabled(object sender, EventArgs e)
        {
            btnOK.Enabled = tbDescription.Text.Trim().Length > 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if(kitchenDisplayMenuLine == null)
            {
                kitchenDisplayMenuLine = new PosMenuLine();

                PosMenuHeader menuHeader = Providers.PosMenuHeaderData.Get(PluginEntry.DataModel, menuHeaderID);
                menuHeader.Columns += 1;
                Providers.PosMenuHeaderData.Save(PluginEntry.DataModel, menuHeader);
            }

            kitchenDisplayMenuLine.Text = tbDescription.Text;
            kitchenDisplayMenuLine.MenuID = menuHeaderID;
            kitchenDisplayMenuLine.Operation = (int)SelectedOperation;
            kitchenDisplayMenuLine.KeyMapping = (Keys)tbKeyMapping.Tag;

            Providers.PosMenuLineData.Save(PluginEntry.DataModel, kitchenDisplayMenuLine);
            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Edit, "PosButtonGridMenuLine", kitchenDisplayMenuLine.ID, null);

            if (cbCreateAnother.Checked)
            {
                kitchenDisplayMenuLine = null;
            }
            else
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

        private void cmbOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbDescription.Text = (string)cmbOperation.SelectedItem;
        }

        private void tbKeyMapping_Enter(object sender, EventArgs e)
        {
            tbKeyMapping.BackColor = Color.White;
        }

        private void tbKeyMapping_Leave(object sender, EventArgs e)
        {
            tbKeyMapping.BackColor = Color.LightGray;
        }

        private void tbKeyMapping_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            tbKeyMapping.Tag = e.KeyCode;

            if (e.Alt)
            {
                tbKeyMapping.Text = Properties.Resources.Alt;
            }
            else if (e.Shift)
            {
                tbKeyMapping.Text = Properties.Resources.Shift;
            }
            else if (e.Control)
            {
                tbKeyMapping.Text = Properties.Resources.Control;
            }
            else
            {
                tbKeyMapping.Text = e.KeyCode.ToString();
            }

            cmbOperation.Focus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbKeyMapping.Text = "";
            tbKeyMapping.Tag = Keys.None;
        }
    }
}