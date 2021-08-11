using System;
using System.Collections.Generic;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Terminals;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.Terminals;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;

namespace LSOne.ViewPlugins.Terminals.Dialogs
{
    public partial class NewTerminalGroupConnection : DialogBase
    {
        private RecordIdentifier groupId;
        private TerminalGroup terminalGroup;
        private List<DataEntity> entities;
        DataEntitySelectionList selectionList = null;
        private List<DataEntity> selectedItems;

        public NewTerminalGroupConnection()
        {
            InitializeComponent();
            terminalGroup = new TerminalGroup();
           
        }

        public NewTerminalGroupConnection(RecordIdentifier groupId)
            : this()
        {
            this.groupId = groupId;
            terminalGroup = Providers.TerminalGroupData.Get(PluginEntry.DataModel, groupId);

            txtTerminalGroup.Text = terminalGroup.Text;

            entities = Providers.TerminalGroupConnectionData.GetTerminalsForDropDown(PluginEntry.DataModel, groupId);
            selectionList = new DataEntitySelectionList(entities);
            cmbTerminal.DropDown += cmbTerminal_DropDown;
            cmbTerminal.SelectedData = selectionList;

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var terminalGroupConnection = new TerminalGroupConnection();

            terminalGroupConnection.TerminalGroupId = groupId;

            foreach (var item in selectedItems)
            {
                terminalGroupConnection.TerminalId = (string)item.ID;
                terminalGroupConnection.StoreId = (string)item.ID.SecondaryID;
                Providers.TerminalGroupConnectionData.Save(PluginEntry.DataModel, terminalGroupConnection);
            }
          

            Close();
            PluginEntry.Framework.ViewController.NotifyDataChanged(null, DataEntityChangeType.Edit, "TerminalGroupConnection", groupId, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmbTerminal_DropDown(object sender, DropDownEventArgs e)
        {
            DataEntitySelectionList list = ((DualDataComboBox)sender).SelectedData as DataEntitySelectionList;
            if (list != null)
            {
                e.ControlToEmbed = new CheckBoxSelectionListPanel(list);
            }
        }

        private void cmbTerminal_SelectedDataChanged(object sender, EventArgs e)
        {
            if (((DualDataComboBox)sender).SelectedData != null)
            {
                selectionList = (DataEntitySelectionList)((DualDataComboBox)sender).SelectedData;

                selectedItems = selectionList.GetSelectedItems();

                var names = new List<string>();
                var ids = new List<string>();

                foreach (DataEntity item in selectedItems)
                {
                    ids.Add((string)item.ID);
                    names.Add(item.Text);
                }

               
            }

            CheckEnabled();
        }

        private void CheckEnabled()
        {
            btnOk.Enabled = (txtTerminalGroup.Text.Length > 0 && cmbTerminal.SelectedData.ID != "");
        }
    }
}

