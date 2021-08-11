using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using LSOne.ViewCore.Interfaces;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Enums;
using TabControl = LSOne.ViewCore.Controls.TabControl;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.Controls;
using LSOne.Controls.Rows;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewPlugins.Replenishment.Dialogs;

namespace LSOne.ViewPlugins.Replenishment.ViewPages
{
    public partial class InventoryTemplateStockCountingPage : UserControl, ITabView
    {
        InventoryTemplate template;
        InventoryArea area;

        public InventoryTemplateStockCountingPage()
        {
            InitializeComponent();

            lvAreas.ContextMenuStrip = new ContextMenuStrip();
            lvAreas.ContextMenuStrip.Opening += lvAreas_Opening;
        }

        public static ITabView CreateInstance(object sender, TabControl.Tab tab)
        {
            return new InventoryTemplateStockCountingPage();
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
            template = (InventoryTemplate)((List<object>)internalContext)[0];
            LoadAreas();
        }

        public void OnClose()
        {
            
        }

        public void OnDataChanged(DataEntityChangeType changeHint, string objectName, RecordIdentifier changeIdentifier, object param)
        {
            if(objectName == "InventoryTemplateArea" && changeIdentifier == template.ID)
            {
                LoadAreas();
            }
        }

        public bool SaveData()
        {
            return true;
        }

        public void SaveUserInterface()
        {
            
        }

        private void LoadAreas()
        {
            if(template.AreaID != Guid.Empty)
            {
                try
                {
                    area = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .GetInventoryArea(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), template.AreaID, true);

                    if(area != null)
                    {
                        lvAreas.ClearRows();

                        foreach (InventoryAreaLine line in area.InventoryAreaLines)
                        {
                            Row row = new Row();
                            row.AddText(line.Text);
                            row.Tag = line;
                            lvAreas.AddRow(row);
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }
            }

            lvAreas.AutoSizeColumns(true);
            lvAreas_SelectionChanged(this, EventArgs.Empty);
            SetAreaText();
        }

        private void lvAreas_Opening(object sender, CancelEventArgs e)
        {
            var menu = lvAreas.ContextMenuStrip;

            menu.Items.Clear();

            var item = new ExtendedMenuItem(
                Properties.Resources.Edit,
                100,
                contextButtons_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = contextButtons.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Add,
                200,
                contextButtons_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = contextButtons.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                Properties.Resources.Delete,
                300,
                contextButtons_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = contextButtons.RemoveButtonEnabled
            };

            menu.Items.Add(item);

            PluginEntry.Framework.ContextMenuNotify("InventoryTemplateAreaList", lvAreas.ContextMenuStrip, lvAreas);

            e.Cancel = (menu.Items.Count == 0);
        }

        private void btnCopyFrom_Click(object sender, EventArgs e)
        {
            InventoryTemplateCopyAreaDialog dlg = new InventoryTemplateCopyAreaDialog(template);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                LoadAreas();
            }
        }

        private void contextButtons_AddButtonClicked(object sender, EventArgs e)
        {
            InventoryTemplateAreaDialog dlg = new InventoryTemplateAreaDialog(template);
            dlg.ShowDialog();
        }

        private void contextButtons_EditButtonClicked(object sender, EventArgs e)
        {
            InventoryAreaLine areaLine = (InventoryAreaLine)lvAreas.Selection[0].Tag;
            InventoryTemplateAreaDialog dlg = new InventoryTemplateAreaDialog(template, areaLine);
            dlg.ShowDialog();
        }

        private void contextButtons_RemoveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                InventoryAreaLine areaLine = (InventoryAreaLine)lvAreas.Selection[0].Tag;

                if (Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .AreaInUse(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), areaLine.ID, true))
                {
                    MessageDialog.Show(Properties.Resources.AreaLineInUse,
                    Properties.Resources.DeleteAreaQuestionCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (QuestionDialog.Show(Properties.Resources.DeleteAreaQuestion, Properties.Resources.DeleteAreaQuestionCaption) == DialogResult.Yes)
                {
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .DeleteInventoryAreaLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), areaLine.ID, true);
                    LoadAreas();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void lvAreas_SelectionChanged(object sender, EventArgs e)
        {
            contextButtons.EditButtonEnabled =
                contextButtons.RemoveButtonEnabled = lvAreas.Selection.Count == 1;
        }

        private void lvAreas_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            contextButtons_EditButtonClicked(sender, args);
        }

        private void btnEditArea_Click(object sender, EventArgs e)
        {
            EditAreaDialog dlg = new EditAreaDialog(area);

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                SetAreaText();
            }
        }

        private void SetAreaText()
        {
            if(area != null)
            {
                txtArea.Text = area.Text;
                btnEditArea.Enabled = true;
            }
            else
            {
                txtArea.Text = "";
                btnEditArea.Enabled = false;
            }
        }
    }
}
