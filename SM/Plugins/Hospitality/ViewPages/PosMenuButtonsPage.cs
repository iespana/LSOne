using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using LSOne.Controls;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.DataProviders;
using LSOne.Controls.Rows;
using LSOne.ViewCore.Dialogs;
using TabControl = LSOne.ViewCore.Controls.TabControl;

namespace LSOne.ViewPlugins.Hospitality.ViewPages
{
    public partial class PosMenuButtonsPage : UserControl, ITabView
    {
        private RecordIdentifier posMenuHeaderID;
        private RecordIdentifier selectedPosMenuLineId = "";
        List<PosMenuLineListItem> posMenuLines;
        private int currItemIndex;

        public PosMenuButtonsPage()
        {
            InitializeComponent();

            lvPosMenuLines.ContextMenuStrip = new ContextMenuStrip();
            lvPosMenuLines.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new PosMenuButtonsPage();
        }

        public bool DataIsModified()
        {
            return false;
        }

        public void GetAuditDescriptors(List<AuditDescriptor> contexts)
        {
            
        }

        public void LoadData(bool isRevert, RecordIdentifier context, object internalContext)
        {
            posMenuHeaderID = context;
            LoadLines();
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "PosMenuLine")
            {
                LoadLines();
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvPosMenuLines.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Properties.Resources.Edit,
                    100,
                    btnsEditAddRemovePosMenuLine_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemovePosMenuLine.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                   Properties.Resources.Add,
                   200,
                   btnsEditAddRemovePosMenuLine_AddButtonClicked);

            item.Enabled = btnsEditAddRemovePosMenuLine.AddButtonEnabled;
            item.Image = ContextButtons.GetAddButtonImage();

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Properties.Resources.Delete,
                    300,
                    btnsEditAddRemovePosMenuLine_RemoveButtonClicked);

            item.Image = ContextButtons.GetRemoveButtonImage();
            item.Enabled = btnsEditAddRemovePosMenuLine.RemoveButtonEnabled;

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("PosMenuLineList", lvPosMenuLines.ContextMenuStrip, lvPosMenuLines);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void LoadLines()
        {
            var oldSelectedId = selectedPosMenuLineId;

            lvPosMenuLines.ClearRows();

            posMenuLines = Providers.PosMenuLineData.GetListItems(PluginEntry.DataModel, posMenuHeaderID);

            foreach (PosMenuLineListItem line in posMenuLines)
            {
                var row = new Row();
                row.AddText((line.KeyNo.ToString()));
                row.AddText(line.Text);
                row.AddText(line.HospitalityOperationName);
                row.AddText(line.Parameter);

                row.Tag = line;

                lvPosMenuLines.AddRow(row);

                if (oldSelectedId == line.ID)
                {
                    lvPosMenuLines.Selection.Set(lvPosMenuLines.RowCount - 1);
                }
            }

            lvPosMenuLines_SelectedIndexChanged(this, EventArgs.Empty);
        }

        private void lvPosMenuLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPosMenuLines.Selection.FirstSelectedRow == -1)
            {
                btnDown.Enabled = false;
                btnUp.Enabled = false;
                btnsEditAddRemovePosMenuLine.EditButtonEnabled = false;
                btnsEditAddRemovePosMenuLine.RemoveButtonEnabled = false;
                return;
            }

            selectedPosMenuLineId = (lvPosMenuLines.Selection.Count > 0) ? ((PosMenuLineListItem)(lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag)).ID : "";

            btnsEditAddRemovePosMenuLine.EditButtonEnabled = lvPosMenuLines.Selection.Count == 1 && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.EditPosMenus);
            btnsEditAddRemovePosMenuLine.RemoveButtonEnabled = lvPosMenuLines.Selection.Count > 0 && PluginEntry.DataModel.HasPermission(DataLayer.BusinessObjects.Permission.EditPosMenus);
            currItemIndex = ((PosMenuLineListItem)(lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag)).KeyNo;

            if (lvPosMenuLines.RowCount < 2 || lvPosMenuLines.Selection.Count == 0)
            {
                btnDown.Enabled = false;
                btnUp.Enabled = false;
                return;
            }

            if (currItemIndex == 1)
            {
                btnDown.Enabled = true;
                btnUp.Enabled = false;
            }
            else
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;
            }
            if (currItemIndex == lvPosMenuLines.RowCount)
            {
                btnDown.Enabled = false;
                btnUp.Enabled = true;
            }
            if (lvPosMenuLines.RowCount == 1 && currItemIndex == 1)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
            if (lvPosMenuLines.Selection.Count > 1 || lvPosMenuLines.Selection.Count == 0)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
        }


        private void btnsEditAddRemovePosMenuLine_AddButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.NewPosMenuLine(posMenuHeaderID);
        }

        private void btnsEditAddRemovePosMenuLine_EditButtonClicked(object sender, EventArgs e)
        {
            PluginOperations.ShowPosMenuLine(selectedPosMenuLineId);
        }

        private void btnsEditAddRemovePosMenuLine_RemoveButtonClicked(object sender, EventArgs e)
        {
            if (lvPosMenuLines.Selection.Count == 1)
            {
                var selectedPosMenusLineId = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag);

                foreach (var item in posMenuLines.Where(z => z.KeyNo > selectedPosMenusLineId.KeyNo))
                {
                    item.KeyNo--;
                    Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, item);
                }

                PluginOperations.DeletePosMenuLine(selectedPosMenuLineId);
            }
            else
            {
                var posMenuLineId = RecordIdentifier.Empty;
                if (
                   QuestionDialog.Show(Properties.Resources.DeletePosButtonGridMenuLineQuestion,
                                       Properties.Resources.DeletePosButtonGridMenuLine) == DialogResult.Yes)
                {
                    int selectedRowNumber;

                    for (int i = 0; i < lvPosMenuLines.Selection.Count; i++)
                    {
                        selectedRowNumber = lvPosMenuLines.Selection.GetSelectedItem(i);

                        posMenuLineId = ((PosMenuLineListItem)(lvPosMenuLines.Row(selectedRowNumber).Tag)).ID;
                        Providers.PosMenuLineData.Delete(PluginEntry.DataModel, posMenuLineId);

                        var selectedPosMenusLineId = ((PosMenuLineListItem)lvPosMenuLines.Row(selectedRowNumber).Tag);
                        posMenuLines.Remove(selectedPosMenusLineId);
                    }

                    var newlist = posMenuLines.OrderBy(x => x.KeyNo);
                    int startNo = 0;
                    foreach (var posMenuLineListItem in newlist)
                    {
                        startNo++;
                        posMenuLineListItem.KeyNo = startNo;
                        Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, posMenuLineListItem);
                    }
                }
            }

            LoadLines();
        }

        private void lvPosMenuLines_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            if (lvPosMenuLines.Selection.Count > 0 && btnsEditAddRemovePosMenuLine.EditButtonEnabled)
            {
                btnsEditAddRemovePosMenuLine_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lvPosMenuLines.RowCount > 0)
            {
                var rowGoingUp = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag);
                var rowGoingDown = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow - 1).Tag);
                rowGoingUp.KeyNo--;
                rowGoingDown.KeyNo++;
                selectedPosMenuLineId = rowGoingUp.ID;
                Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, rowGoingUp);
                Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, rowGoingDown);
            }

            LoadLines();
            lvPosMenuLines.Focus();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lvPosMenuLines.RowCount > 0)
            {
                var rowGoingDown = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow).Tag);
                var rowGoingUp = ((PosMenuLineListItem)lvPosMenuLines.Row(lvPosMenuLines.Selection.FirstSelectedRow + 1).Tag);
                rowGoingUp.KeyNo--;
                rowGoingDown.KeyNo++;
                selectedPosMenuLineId = rowGoingDown.ID;
                Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, rowGoingUp);
                Providers.PosMenuLineData.SaveOrder(PluginEntry.DataModel, rowGoingDown);
            }

            LoadLines();
            lvPosMenuLines.Focus();
        }
    }
}
