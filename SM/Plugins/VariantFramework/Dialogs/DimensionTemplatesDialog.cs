using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.VariantFramework.Properties;
using LSOne.Controls;

namespace LSOne.ViewPlugins.VariantFramework.Dialogs
{
    public partial class DimensionTemplatesDialog : DialogBase
    {
        private RetailItem item;
        private List<RetailItemDimension> dimensions;
        private OrderedDictionary retailItemDimensionAttributes;
        private Dictionary<RecordIdentifier, DimensionAttribute> dimensionAttributes;
        private RecordIdentifier selectedDimensionID;
        private RecordIdentifier selectedAttributeID;
        private struct DimensionTemplates
        {
            public DimensionTemplate DimensionTemplate;
            public List<DimensionAttribute> Attributes;
            public bool Checked;
        }


        //For creating new dimension templates
        public DimensionTemplatesDialog()
        {
            InitializeComponent();
            
            retailItemDimensionAttributes = new OrderedDictionary();
            dimensionAttributes = new Dictionary<RecordIdentifier, DimensionAttribute>();
            selectedDimensionID = RecordIdentifier.Empty;
            selectedAttributeID = RecordIdentifier.Empty;

            lvDimensions.ContextMenuStrip = new ContextMenuStrip();
            lvDimensionAttribute.ContextMenuStrip = new ContextMenuStrip();

            lvDimensions.ContextMenuStrip.Opening += DimensionsContextMenuStrip_Opening;
            lvDimensionAttribute.ContextMenuStrip.Opening += AttributesContextMenuStrip_Opening;

            lvDimensions.AutoSizeColumns();
            lvDimensionAttribute.AutoSizeColumns();
        }

        //For creating dimension templates from item dimensions and reating new dimension templates
        public DimensionTemplatesDialog(RecordIdentifier itemID)
            :this()
        {
            Text = Resources.SaveAsTemplate;
            Header = Resources.SaveDimensionAsTemplate;
            item = Providers.RetailItemData.Get(PluginEntry.DataModel, itemID);
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (DictionaryEntry retailItemDimensionAttribute in retailItemDimensionAttributes)
            {
                RecordIdentifier dimensionTemplateID;
                if (((DimensionTemplates)retailItemDimensionAttribute.Value).Checked)
                {
                    int attributeSequence = 0;

                    dimensionTemplateID = Providers.DimensionTemplateData.SaveAndReturnTemplateID(PluginEntry.DataModel, ((DimensionTemplates)retailItemDimensionAttribute.Value).DimensionTemplate);

                    foreach (DimensionAttribute attribute in ((DimensionTemplates) retailItemDimensionAttribute.Value).Attributes)
                    {
                        attributeSequence += 10;
                        attribute.Sequence = attributeSequence;
                        if (RecordIdentifier.IsEmptyOrNull(attribute.DimensionID))
                        {
                            attribute.DimensionID = dimensionTemplateID;
                        }
                        Providers.DimensionAttributeData.Save(PluginEntry.DataModel, attribute);
                    }
                }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            dimensions = Providers.RetailItemDimensionData.GetListForRetailItem(PluginEntry.DataModel, item.MasterID);
            foreach (RetailItemDimension dimension in dimensions)
            {
                DimensionTemplates dimensionTemplates = new DimensionTemplates();
                dimensionTemplates.DimensionTemplate = dimension;
                dimensionTemplates.Attributes = Providers.DimensionAttributeData.GetListForRetailItemDimension(PluginEntry.DataModel, dimension.ID);
                dimensionTemplates.Checked = false;
                retailItemDimensionAttributes.Add(dimension.ID, dimensionTemplates);
            }
            LoadDimensionListViews();
            lvDimensions.Selection.Set(0);
        }

        private void LoadDimensionListViews()
        {
            lvDimensions.ClearRows();
            Row row;
            CheckBoxCell cell;
            foreach (DictionaryEntry retailItemDimensionAttribute in retailItemDimensionAttributes)
            {
                cell = new CheckBoxCell();
                cell.CheckBoxAlignment = CheckBoxCell.CheckBoxAlignmentEnum.Center;
                row = new Row();
                row.AddCell(cell);

                row.AddText(((DimensionTemplates)retailItemDimensionAttribute.Value).DimensionTemplate.Text);
                row.Tag = retailItemDimensionAttribute.Key;
                lvDimensions.AddRow(row);
                if (((DimensionTemplates)retailItemDimensionAttribute.Value).DimensionTemplate.ID == selectedDimensionID)
                {
                    lvDimensions.Selection.Set(lvDimensions.RowCount - 1);
                }
            }
            lvDimensions.AutoSizeColumns();
        }

        private void LoadAttributeListView()
        {
            lvDimensionAttribute.ClearRows();
            if (lvDimensions.Selection.Count == 1)
            {
                List<DimensionAttribute> attributes = ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes;
                Row row;
                foreach (var attribute in attributes)
                {
                    row = new Row();
                    row.AddText(attribute.Text);
                    row.AddText(attribute.Code);
                    row.Tag = attribute.ID;
                    lvDimensionAttribute.AddRow(row);
                    if (!dimensionAttributes.ContainsKey(attribute.ID))
                    {
                        dimensionAttributes.Add(attribute.ID, attribute);
                    }
                }
                lvDimensionAttribute.AutoSizeColumns();
                lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void btnsEditAddRemoveDimension_AddButtonClicked(object sender, EventArgs e)
        {
            NewAndEditDimensionTemplateDialog dlg = new NewAndEditDimensionTemplateDialog();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DimensionTemplate dimensionTemplate = dlg.DimensionTemplate;
                if (dimensionTemplate != null)
                {
                    if (dimensionTemplate.ID == RecordIdentifier.Empty)
                    {
                        dimensionTemplate.ID = Guid.NewGuid();
                    }
                    DimensionTemplates dimensionTemplates = new DimensionTemplates();
                    dimensionTemplates.DimensionTemplate = dimensionTemplate;
                    dimensionTemplates.Attributes = new List<DimensionAttribute>();
                    dimensionTemplates.Checked = true;
                    retailItemDimensionAttributes.Add(dimensionTemplate.ID, dimensionTemplates);
                    Row row = new Row();

                    CheckBoxCell cell = new CheckBoxCell();
                    cell.CheckBoxAlignment = CheckBoxCell.CheckBoxAlignmentEnum.Center;
                    cell.CheckState = CheckState.Checked;
                    row.AddCell(cell);

                    row.AddText(dimensionTemplate.Text);
                    row.Tag = dimensionTemplate.ID;
                    lvDimensions.AddRow(row);
                    lvDimensions.AutoSizeColumns();
                    lvDimensions.Selection.Set(lvDimensions.RowCount - 1);
                    lvDimensions_SelectionChanged(this, EventArgs.Empty);
                }
                CheckEnabled();
            }
        }

        private void btnsEditAddRemoveDimensionAttribute_AddButtonClicked(object sender, EventArgs e)
        {
            List<DimensionAttribute> dimensionAttributes = ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes;
            Dialogs.AddNewDimensionAttributeDialog dlg = new AddNewDimensionAttributeDialog(dimensionAttributes);
            dlg.CreateWithoutClosing += CreateAttributeWithoutClosing;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                AddNewDimensionAttribute(dlg);
            }
            dlg.CreateWithoutClosing -= CreateAttributeWithoutClosing;
            CheckEnabled();
        }

        private void CreateAttributeWithoutClosing(object sender, EventArgs e)
        {
            AddNewDimensionAttribute((AddNewDimensionAttributeDialog)sender);
        }

        private void AddNewDimensionAttribute(AddNewDimensionAttributeDialog dlg)
        {
            DimensionAttribute dimensionAttribute = dlg.DimensionAttribute;
            if (dimensionAttribute != null)
            {
                Row row = new Row();
                row.AddText(dimensionAttribute.Text);
                row.AddText(dimensionAttribute.Code);
                dimensionAttribute.ID = Guid.NewGuid();
                dimensionAttribute.DimensionID = selectedDimensionID;
                row.Tag = dimensionAttribute.ID;
                ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes.Add(dimensionAttribute);
                if (lvDimensionAttribute.RowCount != 0)
                {
                    int latestAttributeSequence = dimensionAttributes[(RecordIdentifier) lvDimensionAttribute.Row(lvDimensionAttribute.RowCount - 1).Tag].Sequence;
                    dimensionAttribute.Sequence = latestAttributeSequence + 10;
                }
                else
                {
                    dimensionAttribute.Sequence = 10;
                }
                lvDimensionAttribute.AddRow(row);
                dimensionAttributes.Add(dimensionAttribute.ID, dimensionAttribute);
                lvDimensionAttribute.AutoSizeColumns();
                lvDimensionAttribute.Selection.Set(lvDimensionAttribute.RowCount - 1);
                lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void lvDimensionAttribute_SelectionChanged(object sender, EventArgs e)
        {
            if (lvDimensionAttribute.Selection.FirstSelectedRow < 0)
            {
                return;
            }
            RecordIdentifier currentAttribute = (RecordIdentifier)lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow).Tag;
            if (currentAttribute != selectedAttributeID)
            {
                selectedAttributeID = currentAttribute;
            }
            btnsEditAddRemoveDimensionAttribute.EditButtonEnabled = (lvDimensionAttribute.Selection.Count == 1);
            btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled = (lvDimensionAttribute.Selection.Count == 1) && (PluginEntry.DataModel.HasPermission(LSOne.DataLayer.BusinessObjects.Permission.ItemsEdit));
            btnMoveUpDimensionAttribute.Enabled = (lvDimensionAttribute.Selection.Count == 1) && (lvDimensionAttribute.RowCount > 1) && (lvDimensionAttribute.Selection.FirstSelectedRow != 0);
            btnMoveDownDimensionAttribute.Enabled = (lvDimensionAttribute.Selection.Count == 1) && (lvDimensionAttribute.RowCount > 1) && (lvDimensionAttribute.Selection.FirstSelectedRow != lvDimensionAttribute.RowCount - 1);
        }

        private void lvDimensions_SelectionChanged(object sender, EventArgs e)
        {
            if (lvDimensions.Selection.FirstSelectedRow < 0)
            {
                return;
            }
            RecordIdentifier currentDimension = (RecordIdentifier) lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag;

            if (currentDimension != selectedDimensionID)
            {
                selectedDimensionID = currentDimension;
                LoadAttributeListView();
            }
            btnsEditAddRemoveDimension.EditButtonEnabled = (lvDimensions.Selection.Count == 1);
            btnsEditAddRemoveDimension.RemoveButtonEnabled = (lvDimensions.Selection.Count == 1);
            btnsEditAddRemoveDimensionAttribute.AddButtonEnabled = (lvDimensions.Selection.Count == 1);
            btnMoveUpDimensionAttribute.Enabled = false;
            btnMoveDownDimensionAttribute.Enabled = false;
            btnsEditAddRemoveDimensionAttribute.EditButtonEnabled = false;
            btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled = false;
        }
        

        private void btnMoveUpDimensionAttribute_Click(object sender, EventArgs e)
        {
            DimensionAttribute tempAttribute = ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes[lvDimensionAttribute.Selection.FirstSelectedRow];
            ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes.RemoveAt(lvDimensionAttribute.Selection.FirstSelectedRow);
            ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes.Insert(lvDimensionAttribute.Selection.FirstSelectedRow - 1, tempAttribute);

            lvDimensionAttribute.SwapRows(lvDimensionAttribute.Selection.FirstSelectedRow, lvDimensionAttribute.Selection.FirstSelectedRow - 1);
            
            lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
        }

        private void btnMoveDownDimensionAttribute_Click(object sender, EventArgs e)
        {
            DimensionAttribute tempAttribute = ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes[lvDimensionAttribute.Selection.FirstSelectedRow];
            ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes.RemoveAt(lvDimensionAttribute.Selection.FirstSelectedRow);
            ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes.Insert(lvDimensionAttribute.Selection.FirstSelectedRow + 1, tempAttribute);

            lvDimensionAttribute.SwapRows(lvDimensionAttribute.Selection.FirstSelectedRow, lvDimensionAttribute.Selection.FirstSelectedRow + 1);
            lvDimensionAttribute_SelectionChanged(this, EventArgs.Empty);
            if (!btnMoveDownDimensionAttribute.Enabled)
            {
                btnMoveUpDimensionAttribute.Focus();
            }
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = ValidateFields();
        }

        private bool ValidateFields()
        {
            bool valid = false;
            foreach (DictionaryEntry retailItemDimensionAttribute in retailItemDimensionAttributes)
            {
                if (((DimensionTemplates) retailItemDimensionAttribute.Value).Checked &&
                    ((DimensionTemplates) retailItemDimensionAttribute.Value).Attributes.Count > 0)
                {
                    valid = true;
                }
                if (((DimensionTemplates)retailItemDimensionAttribute.Value).Checked &&
                    ((DimensionTemplates)retailItemDimensionAttribute.Value).Attributes.Count <= 0)
                {
                    valid = false;
                }
            }
            return valid;
        }

        private void btnsEditAddRemoveDimensionAttribute_RemoveButtonClicked(object sender, EventArgs e)
        {
            bool deleteAttribute = false;

            if (QuestionDialog.Show(Resources.DeleteAttributeWarning, Resources.DeleteAttribute) == DialogResult.Yes)
            {
                PluginOperations.DeleteAttributesAndVariantItems(selectedAttributeID);

                deleteAttribute = true;
            }

            if (deleteAttribute)
            {
                int selectedRow = lvDimensionAttribute.Selection.FirstSelectedRow;
                lvDimensionAttribute.RemoveRow(selectedRow);

                if (selectedRow < lvDimensionAttribute.RowCount)
                {
                    lvDimensionAttribute.Selection.Set(selectedRow);
                }
                else if (lvDimensionAttribute.RowCount > 0)
                {
                    lvDimensionAttribute.Selection.Set(lvDimensionAttribute.RowCount - 1);
                }

                ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes.RemoveAt(selectedRow);

                CheckEnabled();
            }
        }

        private void btnsEditAddRemoveDimension_EditButtonClicked(object sender, EventArgs e)
        {
            DimensionTemplate description = ((DimensionTemplates)retailItemDimensionAttributes[(RecordIdentifier)lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow).Tag]).DimensionTemplate;

            NewAndEditDimensionTemplateDialog dlg = new NewAndEditDimensionTemplateDialog(description);

            if (dlg.ShowDialog() == DialogResult.OK)
            {

                lvDimensions.Row(lvDimensions.Selection.FirstSelectedRow)[1].Text = description.Text;
                lvDimensions.Invalidate();
                lvDimensions.AutoSizeColumns();
                lvDimensionAttribute.AutoSizeColumns();
            }
        }

        private void btnsEditAddRemoveDimensionAttribute_EditButtonClicked(object sender, EventArgs e)
        {
            DimensionAttribute attribute = (((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes)[lvDimensionAttribute.Selection.FirstSelectedRow];
            List<DimensionAttribute> dimensionAttributes = ((DimensionTemplates)retailItemDimensionAttributes[selectedDimensionID]).Attributes;

            Dialogs.AddNewDimensionAttributeDialog dlg = new Dialogs.AddNewDimensionAttributeDialog(attribute, dimensionAttributes);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow)[0].Text = attribute.Text;
                lvDimensionAttribute.Row(lvDimensionAttribute.Selection.FirstSelectedRow)[1].Text = attribute.Code;
                lvDimensionAttribute.Invalidate();
                lvDimensionAttribute.AutoSizeColumns();
            }
        }

        private void DimensionsContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvDimensions.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnsEditAddRemoveDimension_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemoveDimension.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.AddDimension,
                    200,
                    btnsEditAddRemoveDimension_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsEditAddRemoveDimension.AddButtonEnabled
            };

            menu.Items.Add(item);
        }

        private void AttributesContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip menu = lvDimensionAttribute.ContextMenuStrip;

            menu.Items.Clear();

            // We can optionally add our own items right here
            var item = new ExtendedMenuItem(
                    Resources.Edit,
                    100,
                    btnsEditAddRemoveDimensionAttribute_EditButtonClicked)
            {
                Image = ContextButtons.GetEditButtonImage(),
                Enabled = btnsEditAddRemoveDimensionAttribute.EditButtonEnabled,
                Default = true
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.AddAttribute,
                    200,
                    btnsEditAddRemoveDimensionAttribute_AddButtonClicked)
            {
                Image = ContextButtons.GetAddButtonImage(),
                Enabled = btnsEditAddRemoveDimensionAttribute.AddButtonEnabled
            };

            menu.Items.Add(item);

            item = new ExtendedMenuItem(
                    Resources.RemoveAttribute,
                    300,
                    btnsEditAddRemoveDimensionAttribute_RemoveButtonClicked)
            {
                Image = ContextButtons.GetRemoveButtonImage(),
                Enabled = btnsEditAddRemoveDimensionAttribute.RemoveButtonEnabled
            };

            menu.Items.Add(item);
        }

        private void lvDimensionAttribute_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemoveDimensionAttribute.EditButtonEnabled)
            {
                btnsEditAddRemoveDimensionAttribute_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvDimensions_RowDoubleClick(object sender, Controls.EventArguments.RowEventArgs args)
        {
            if (btnsEditAddRemoveDimension.EditButtonEnabled)
            {
                btnsEditAddRemoveDimension_EditButtonClicked(this, EventArgs.Empty);
            }
        }

        private void lvDimensions_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            if (((CheckBoxCell)args.Cell).Checked)
            {
                DimensionTemplates templateState = (DimensionTemplates)retailItemDimensionAttributes[(lvDimensions.Row(args.RowNumber).Tag)];
                templateState.Checked = true;
                retailItemDimensionAttributes[((RecordIdentifier)lvDimensions.Row(args.RowNumber).Tag)] = templateState;
            }
            else
            {
                DimensionTemplates templateState = (DimensionTemplates)retailItemDimensionAttributes[(lvDimensions.Row(args.RowNumber).Tag)];
                templateState.Checked = false;
                retailItemDimensionAttributes[((RecordIdentifier)lvDimensions.Row(args.RowNumber).Tag)] = templateState;
            }
            CheckEnabled();
        }
    }
}
