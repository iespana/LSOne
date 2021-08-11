using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Replenishment;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LSOne.ViewPlugins.Replenishment.Dialogs
{
    public partial class InventoryTemplateCopyAreaDialog : DialogBase
    {
        private InventoryTemplate inventoryTemplate;

        public InventoryTemplateCopyAreaDialog(InventoryTemplate inventoryTemplate)
        {
            InitializeComponent();
            this.inventoryTemplate = inventoryTemplate;
            cmbCopyFrom.SelectedData = new DataEntity("", "");
            lvAreas.AutoSizeColumns(true);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void cmbCopyFrom_RequestData(object sender, EventArgs e)
        {
            try
            {
                List<InventoryArea> areas = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                            .GetInventoryAreasList(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), true);

                if(inventoryTemplate.AreaID != Guid.Empty)
                {
                    areas.RemoveAll(x => (Guid)x.ID == inventoryTemplate.AreaID);
                }

                cmbCopyFrom.SetData(areas, null);
            }
            catch(Exception ex)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void cmbCopyFrom_SelectedDataChanged(object sender, EventArgs e)
        {
            lvAreas.ClearRows();
            btnOK.Enabled = false;

            if(cmbCopyFrom.SelectedData.ID != "")
            {
                try
                {
                    List<InventoryAreaLine> areaLines = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                                            .GetInventoryAreaLinesByArea(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), cmbCopyFrom.SelectedData.ID, true);

                    foreach (InventoryAreaLine line in areaLines)
                    {
                        Row row = new Row();
                        row.AddCell(new CheckBoxCell(false, true, CheckBoxCell.CheckBoxAlignmentEnum.Center));
                        row.AddText(line.Text);
                        row.Tag = line;
                        lvAreas.AddRow(row);
                    }
                }
                catch(Exception ex)
                {
                    MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
                }
            }

            lvAreas.AutoSizeColumns(true);
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            lvAreas.Rows.ForEach(cell => ((CheckBoxCell)cell[0]).Checked = true);
            lvAreas.Invalidate();
            btnOK.Enabled = true;
        }

        private void btnSelectNone_Click(object sender, EventArgs e)
        {
            lvAreas.Rows.ForEach(cell => ((CheckBoxCell)cell[0]).Checked = false);
            lvAreas.Invalidate();
            btnOK.Enabled = false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Save()
        {
            try
            {
                if (inventoryTemplate.AreaID == Guid.Empty)
                {
                    InventoryArea newArea = new InventoryArea();
                    newArea.Text = inventoryTemplate.Text + " - " + Properties.Resources.Area;

                    newArea.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .SaveInventoryArea(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), newArea, false);

                //Save the new area header on the template
                inventoryTemplate.AreaID = (Guid)newArea.ID;
                    Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                        .SaveInventoryTemplate(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), inventoryTemplate, false);
                }

                foreach(Row row in lvAreas.Rows)
                {
                    if(((CheckBoxCell)row[0]).Checked)
                    {
                        InventoryAreaLine lineToCopy = (InventoryAreaLine)row.Tag;
                        InventoryAreaLine newLine = new InventoryAreaLine();

                        newLine.AreaID = inventoryTemplate.AreaID;
                        newLine.Text = lineToCopy.Text;
                        newLine.ID = Services.Interfaces.Services.InventoryService(PluginEntry.DataModel)
                            .SaveInventoryAreaLine(PluginEntry.DataModel, PluginOperations.GetSiteServiceProfile(), newLine, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.Show(Properties.Resources.CouldNotConnectToStoreServer + "\r\n" + ex.Message);
            }
        }

        private void lvAreas_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            if(args.Cell is CheckBoxCell)
            {
                btnOK.Enabled = lvAreas.Rows.Any(cell => ((CheckBoxCell)cell[0]).Checked);
            }
        }
    }
}
