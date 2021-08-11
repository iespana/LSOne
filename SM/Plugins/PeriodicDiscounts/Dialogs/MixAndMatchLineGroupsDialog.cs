using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.DataProviders.PricesAndDiscounts;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;

namespace LSOne.ViewPlugins.PeriodicDiscounts.Dialogs
{
    public partial class MixAndMatchLineGroupsDialog : DialogBase
    {
        RecordIdentifier offerID;
        bool changed;
        RecordIdentifier selectedID;

        public MixAndMatchLineGroupsDialog(RecordIdentifier offerID)
            : this()
        {
            this.offerID = offerID;
        }

        public MixAndMatchLineGroupsDialog()
        {
            offerID = RecordIdentifier.Empty;
            changed = false;
            selectedID = "";

            InitializeComponent();

            lvGroups.ContextMenuStrip = new ContextMenuStrip();
            lvGroups.ContextMenuStrip.Opening += lvGroups_Opening;
        }


        private void LoadItems(bool doBestFit)
        {
            lvGroups.ClearRows();

            List<MixAndMatchLineGroup> groups = Providers.MixAndMatchLineGroupData.GetGroups(PluginEntry.DataModel, offerID, 0, false);

            IconButton btn = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete),Properties.Resources.Delete);

            foreach (MixAndMatchLineGroup group in groups)
            {
                Row row = new Row();

                row.AddText(group.Text);
                row.AddText(group.NumberOfItemsNeeded.ToString());
                row.AddCell(new ColorBoxCell(3,group.Color,Color.Black,0,"", false));
                row.AddCell(new IconButtonCell(btn,IconButtonCell.IconButtonIconAlignmentEnum.VerticalCenter | IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter,"",true));
                row.Tag = group.ID;

                lvGroups.AddRow(row);

                if (selectedID == group.ID)
                {
                    lvGroups.Selection.Set(lvGroups.RowCount - 1);
                
                }
            }

            lvGroups_SelectionChanged(this, EventArgs.Empty);

            if (doBestFit)
            {
                lvGroups.ApplyRelativeColumnSize();
            }
        }

       

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            LoadItems(true);
        }

        

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier OfferID
        {
            get { return offerID; }
        }

        public bool Changed
        {
            get
            {
                return changed;
            }
        }


        

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MixAndMatchLineGroupDialog dlg = new MixAndMatchLineGroupDialog(offerID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                changed = true;
                LoadItems( true);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            MixAndMatchLineGroupDialog dlg = new MixAndMatchLineGroupDialog(offerID,((RecordIdentifier)lvGroups.Selection[0].Tag).SecondaryID);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                changed = true;
                LoadItems( true);
            }
        }

        


        void lvGroups_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvGroups.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            ExtendedMenuItem item = new ExtendedMenuItem(
                    Properties.Resources.Add + "...",
                    300,
                    btnAdd_Click)
                    {
                        Image = ContextButtons.GetAddButtonImage(),
                        Enabled = btnsContextButtons.AddButtonEnabled
                    };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.EditText + "...",
                    100,
                    btnEdit_Click)
                    {
                        Image = ContextButtons.GetEditButtonImage(),
                        Enabled = btnsContextButtons.EditButtonEnabled,
                        Default = true
                    };

            menu.Items.Add(item);


            item = new ExtendedMenuItem(
                    Properties.Resources.Delete + "...",
                    300,
                    btnRemove_Click)
                    {
                        Image = ContextButtons.GetRemoveButtonImage(),
                        Enabled = btnsContextButtons.RemoveButtonEnabled
                    };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("MixAndMatchGroupLineList", lvGroups.ContextMenuStrip, lvGroups);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void lvGroups_RowDoubleClick(object sender, RowEventArgs args)
        {
            if (btnsContextButtons.EditButtonEnabled)
            {
                btnEdit_Click(this, EventArgs.Empty);
            }
        }


        private void lvGroups_SelectionChanged(object sender, EventArgs e)
        {
            btnsContextButtons.RemoveButtonEnabled = lvGroups.Selection.Count > 0;
            btnsContextButtons.EditButtonEnabled = lvGroups.Selection.Count == 1;

            selectedID = (lvGroups.Selection.Count > 0) ? (RecordIdentifier)lvGroups.Selection[0].Tag : "";
        }

        private void lvGroups_CellAction(object sender, CellEventArgs args)
        {
            if (QuestionDialog.Show(
                   Properties.Resources.DeleteLineGroupQuestion,
                   Properties.Resources.DeleteLineGroup) == DialogResult.Yes)
            {
                DeleteRow((RecordIdentifier)lvGroups.Row(args.RowNumber).Tag);
            }
        }

        private void DeleteRow(RecordIdentifier id)
        {
            if (Providers.MixAndMatchLineGroupData.CanDelete(PluginEntry.DataModel, id))
            {
                Providers.MixAndMatchLineGroupData.Delete(PluginEntry.DataModel, id);

                changed = true;
                LoadItems(true);
            }
            else
            {
                MessageDialog.Show(Properties.Resources.CouldNotDeleteLineGroup);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lvGroups.Selection.Count == 1)
            {
                if (QuestionDialog.Show(
                   Properties.Resources.DeleteLineGroupQuestion,
                   Properties.Resources.DeleteLineGroup) == DialogResult.Yes)
                {
                    DeleteRow((RecordIdentifier)lvGroups.Selection[0].Tag);
                }
            }
            else
            {
                if (QuestionDialog.Show(
                   Properties.Resources.DeleteLineGroupsQuestion,
                   Properties.Resources.DeleteLineGroups) == DialogResult.Yes)
                {
                    List<RecordIdentifier> idList = new List<RecordIdentifier>();

                    for (int i = 0; i < lvGroups.Selection.Count; i++)
                    {
                        idList.Add((RecordIdentifier)lvGroups.Selection[i].Tag);
                    }

                    foreach (RecordIdentifier id in idList)
                    {
                        DeleteRow(id);
                    }
                }
            }
        }



        
    }
}
