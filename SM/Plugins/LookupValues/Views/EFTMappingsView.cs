using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.EventArguments;

namespace LSOne.ViewPlugins.LookupValues.Views
{
    public partial class EFTMappingsView : ViewBase
    {
        RecordIdentifier selectedID;

        public EFTMappingsView()
        {
            selectedID = RecordIdentifier.Empty;

            InitializeComponent();

            Attributes = ViewAttributes.ContextBar |
                ViewAttributes.Audit |
                ViewAttributes.Close | 
                ViewAttributes.Help;

            lvMappings.ContextMenuStrip = new ContextMenuStrip();
            lvMappings.ContextMenuStrip.Opening += lvMappings_Opening;

            btnsContextButtons.AddButtonEnabled = PluginEntry.DataModel.HasPermission(Permission.EFTMappingEdit);
        }

        public override void GetAuditDescriptors(List<AuditDescriptor> descriptors)
        {
            descriptors.Add(new AuditDescriptor("EFTMappings", RecordIdentifier.Empty, Properties.Resources.EFTMappings, false));
        }

        protected override string LogicalContextName
        {
            get
            {
                return Properties.Resources.EFTMappings;
            }
        }

        public override RecordIdentifier ID
        {
	        get 
	        { 
                return RecordIdentifier.Empty;
	        }
        }

        protected override void LoadData(bool isRevert)
        {
            var mappings = Providers.EFTMappingData.GetList(PluginEntry.DataModel, true);

            lvMappings.Items.Clear();

            foreach (var mapping in mappings)
            {
                var item = new ListViewItem((string)mapping.MappingID);
                item.SubItems.Add(mapping.SchemeName);
                item.SubItems.Add(mapping.TenderTypeName);
                item.SubItems.Add(mapping.CardTypeName);
                item.SubItems.Add(mapping.Enabled ? Properties.Resources.Yes : Properties.Resources.No);
                item.SubItems.Add(mapping.LookupOrder.ToString());
                item.Tag = mapping.ID;

                lvMappings.Add(item);

                if (selectedID == (RecordIdentifier)item.Tag)
                {
                    item.Selected = true;
                }
            }

            lvMappings.BestFitColumns();

            HeaderText = Properties.Resources.EFTMappings;
            //HeaderIcon = Properties.Resources.PaymentMethodImage;

            lvMappings_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected override bool DataIsModified()
        {
            // Here our sheet is supposed to figure out if something needs to be saved and return
            // true if something needs to be saved, else false.
            return false;
        }

        protected override bool SaveData()
        {
            // Here we would let our sheet save our data.

            // We return true since saving was successful, if we would return false then
            // the viewstack will prevent other sheet from getting shown.
            return true;
        }

        public override void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            switch (objectName)
            {
                case "EFTMapping":
                    LoadData(false);
                    break;
            }

        }

        private void lvMappings_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedID = (lvMappings.SelectedItems.Count > 0) ? (RecordIdentifier)lvMappings.SelectedItems[0].Tag : RecordIdentifier.Empty;
            btnsContextButtons.EditButtonEnabled = (lvMappings.SelectedItems.Count != 0) && PluginEntry.DataModel.HasPermission(Permission.EFTMappingEdit);
            btnsContextButtons.RemoveButtonEnabled = btnsContextButtons.EditButtonEnabled;
        }

        void lvMappings_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvMappings.ContextMenuStrip;
            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.EditCmd,
                    100,
                    btnEdit_Click)
                {
                    Image = ContextButtons.GetEditButtonImage(),
                    Enabled = btnsContextButtons.EditButtonEnabled,
                    Default = true
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Add,
                    200,
                    btnAdd_Click)
                {
                    Image = ContextButtons.GetAddButtonImage(),
                    Enabled = btnsContextButtons.AddButtonEnabled
                };
            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnRemove_Click)
                {
                    Image = ContextButtons.GetRemoveButtonImage(),
                    Enabled = btnsContextButtons.RemoveButtonEnabled
                };
            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("EFTMappingList", lvMappings.ContextMenuStrip, lvMappings);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            PluginOperations.EditEFTMapping((RecordIdentifier)lvMappings.SelectedItems[0].Tag);
            lvMappings.ShowSelectedItem();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PluginOperations.NewEFTMapping();
            lvMappings.ShowSelectedItem();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            PluginOperations.DeleteEFTMapping(selectedID);
        }

        private void lvMapping_DoubleClick(object sender, EventArgs e)
        {
            if ((lvMappings.SelectedItems.Count != 0) && (PluginEntry.DataModel.HasPermission(Permission.EFTMappingEdit)))
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }

        protected override void OnSetupContextBarItems(ContextBarItemConstructionArguments arguments)
        {
            if (arguments.CategoryKey == GetType().ToString() + ".View")
            {
                if (PluginEntry.DataModel.HasPermission(Permission.EFTMappingEdit))
                {
                    arguments.Add(new ContextBarItem(Properties.Resources.Add, ContextButtons.GetAddButtonImage(), btnAdd_Click), 10);
                }
            }
        }
        
        protected override void OnClose()
        {
            lvMappings.SmallImageList = null;

            base.OnClose();
        }
    }
}
