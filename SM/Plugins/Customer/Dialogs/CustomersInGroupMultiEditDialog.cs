using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Customers;
using LSOne.DataLayer.BusinessObjects.Enums;
using LSOne.DataLayer.BusinessObjects.PricesAndDiscounts;
using LSOne.DataLayer.DataProviders;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.ViewCore.Dialogs;
using LSOne.ViewCore.Enums;
using LSOne.ViewCore.Interfaces;
using LSOne.ViewPlugins.Customer.Views;

namespace LSOne.ViewPlugins.Customer.Dialogs
{
    public partial class CustomersInGroupMultiEditDialog : DialogBase
    {
        private List<DataEntity> selectedLines;
        private List<DataEntity> linesToAdd;
        private List<DataEntity> linesToRemove;
        private Style redStrikeThroughStyle;
        private Style greenStyle;
        private Style redStyle;

        private RecordIdentifier groupID;

        private CustomerGroupTypeEnum groupType;
        private PriceDiscGroupEnum priceDiscGroupType;

        public List<DataEntity> CustomerList { get; set; }

        private CustomersInGroupMultiEditDialog()
        {
            InitializeComponent();

            groupType = CustomerGroupTypeEnum.PriceGroup;
            selectedLines = new List<DataEntity>();
            linesToRemove = new List<DataEntity>();
            linesToAdd = new List<DataEntity>();

            redStrikeThroughStyle = new Style(lvlEditPreview.DefaultStyle);
            redStrikeThroughStyle.Font = new Font(redStrikeThroughStyle.Font, FontStyle.Strikeout);
            redStrikeThroughStyle.TextColor = ColorPalette.RedDark;

            greenStyle = new Style(lvlEditPreview.DefaultStyle) { TextColor = ColorPalette.GreenLight };

            redStyle = new Style(lvlEditPreview.DefaultStyle) { TextColor = ColorPalette.RedDark };

            groupID = RecordIdentifier.Empty;
        }

        /// <summary>
        /// Tells the dialog what type of group the customers are being added to i.e. Customer group or Price group
        /// </summary>
        /// <param name="groupType">To what type of group is the dialog adding customers</param>
        /// <param name="priceDiscGroupType">If the group type is Price group then the dialog needs to know what type of price or discount group</param>
        /// <param name="groupID">The ID of the group</param>
        public CustomersInGroupMultiEditDialog(CustomerGroupTypeEnum groupType, PriceDiscGroupEnum? priceDiscGroupType, RecordIdentifier groupID) 
            : this()
        {
            this.groupType = groupType;
            this.groupID = groupID;
            
            this.priceDiscGroupType = priceDiscGroupType != null ? (PriceDiscGroupEnum)priceDiscGroupType : this.priceDiscGroupType;

            switch (groupType)
            {
                case CustomerGroupTypeEnum.PriceGroup:
                    selectedLines = Providers.PriceDiscountGroupData.GetCustomersInGroupList(PluginEntry.DataModel, (PriceDiscGroupEnum)priceDiscGroupType, (string)groupID, null, null).Cast<DataEntity>().ToList();
                    break;
                case CustomerGroupTypeEnum.CustomerGroup:
                    selectedLines = Providers.CustomersInGroupData.GetCustomersInGroupList(PluginEntry.DataModel, groupID, null, null).Cast<DataEntity>().ToList();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("groupType");
            }

            LoadPreviewLines();
        }

        protected override IApplicationCallbacks OnGetFramework()
        {
            return PluginEntry.Framework;
        }

        public RecordIdentifier ID
        {
            get { return groupID; }
        }

        private void CheckEnabled()
        {
            btnOK.Enabled = linesToAdd.Count > 0 || linesToRemove.Count > 0;
        }

        private void LoadPreviewLines()
        {
            lvlEditPreview.ClearRows();

            AddSelectedAndRemovedPreviewRows();

            AddNewPreviewRows();

            lvlEditPreview.AutoSizeColumns(true);
        }

        private void AddNewPreviewRows()
        {
            foreach (var line in linesToAdd)
            {
                var row = new Row();
                row.AddCell(new Cell(line.Text, greenStyle));
                row.AddCell(new Cell(Properties.Resources.Add, greenStyle));
                row.Tag = Tuple.Create(RowTypeEnum.LineToAdd, line);

                var button = new IconButton(Properties.Resources.RevertSmallImage, Properties.Resources.Undo, true);
                row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                lvlEditPreview.AddRow(row);
            }
        }

        private void AddSelectedAndRemovedPreviewRows()
        {
            //Copy selected and removed to a new list. This is needed so we don't modify the existing lists in memory
            var selected = selectedLines;
            var removed = linesToRemove;

            var selectedAndRemoved = new List<DataEntity>();

            foreach (DataEntity line in selected)
            {
                selectedAndRemoved.Add(line);
            }

            foreach (DataEntity line in removed)
            {
                selectedAndRemoved.Add(line);
            }

            selectedAndRemoved.Sort(CompareDataEntities);

            foreach (DataEntity line in selectedAndRemoved)
            {
                if (linesToRemove.Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddCell(new Cell(line.Text, redStrikeThroughStyle));
                    row.AddCell(new Cell(Properties.Resources.Delete, redStyle));
                    row.Tag = Tuple.Create(RowTypeEnum.LineToRemove, line);

                    var button = new IconButton(Properties.Resources.RevertSmallImage, Properties.Resources.Undo, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
                else if (!linesToAdd.Exists(p => p.ID == line.ID))
                {
                    var row = new Row();
                    row.AddText(line.Text);
                    row.AddText(Properties.Resources.EditTypeNone);
                    row.Tag = Tuple.Create(selectedLines.Exists(p => p.ID == line.ID) ? RowTypeEnum.SelectedLine : RowTypeEnum.LineToAdd, line);

                    var button = new IconButton(PluginEntry.Framework.GetImage(ImageEnum.EmbeddedListDelete), Properties.Resources.Delete, true);
                    row.AddCell(new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.Right, "", false));

                    lvlEditPreview.AddRow(row);
                }
            }
        }

        private int CompareDataEntities(DataEntity dataEntity, DataEntity entity)
        {
            return dataEntity.Text.CompareTo(entity.Text);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lvlEditPreview_CellAction(object sender, Controls.EventArguments.CellEventArgs args)
        {
            var tuple = (Tuple<RowTypeEnum, DataEntity>)lvlEditPreview.Row(args.RowNumber).Tag;

            // Row type
            switch (tuple.Item1)
            {
                case RowTypeEnum.SelectedLine:
                    if (!linesToAdd.Exists(p => p.ID == tuple.Item2.ID))
                    {
                        selectedLines.Remove((DataEntity)tuple.Item2);
                        linesToRemove.Add((DataEntity)tuple.Item2);
                    }
                    break;
                case RowTypeEnum.LineToAdd:
                    linesToAdd.Remove((DataEntity)tuple.Item2);
                    selectedLines.Remove((DataEntity)tuple.Item2);
                    break;
                case RowTypeEnum.LineToRemove:
                    // Undo a remove
                    linesToRemove.Remove((DataEntity)tuple.Item2);
                    selectedLines.Add((DataEntity)tuple.Item2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            LoadPreviewLines();
            CheckEnabled();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            switch (groupType)
            {
                case CustomerGroupTypeEnum.PriceGroup:
                    UpdatePriceDiscountGroupInformation();
                    break;
                case CustomerGroupTypeEnum.CustomerGroup:
                    UpdateCustomerGroupInformation();
                    break;
            }

            PluginEntry.Framework.ViewController.NotifyDataChanged(this, DataEntityChangeType.Add, NotifyContexts.CustomersInGroupPage, groupID, null);

            DialogResult = DialogResult.OK;
            Close();
        }

        private void UpdatePriceDiscountGroupInformation()
        {
            foreach (DataEntity added in linesToAdd)
            {
                PriceDiscountGroup group = new PriceDiscountGroup();
                group.ID = new RecordIdentifier(added.ID, groupID);
                Providers.PriceDiscountGroupData.Save(PluginEntry.DataModel, group);
            }

            foreach (DataEntity removed in linesToRemove)
            {
                PriceDiscountGroup group = new PriceDiscountGroup();
                group.ID = new RecordIdentifier(removed.ID, groupID);
                Providers.PriceDiscountGroupData.Delete(PluginEntry.DataModel, group);
            }
        }

        private void UpdateCustomerGroupInformation()
        {
            foreach (DataEntity added in linesToAdd)
            {
                PluginOperations.AddCustomerToGroup(added.ID, groupID);
            }

            foreach (DataEntity removed in linesToRemove)
            {
                PluginOperations.RemoveCustomerFromGroup(removed.ID, groupID);
            }
        }

        private void cmbCustomers_DropDown(object sender, DropDownEventArgs e)
        {
            e.ControlToEmbed = new MultiSearchPanel(PluginEntry.DataModel,
                                                                selectedLines,
                                                                linesToAdd,
                                                                linesToRemove,
                                                                SearchTypeEnum.Customers, false);
            cmbCustomers.ShowDropDownOnTyping = true;
        }

        private void cmbCustomers_SelectedDataChanged(object sender, EventArgs e)
        {
            int prevAddedCount = linesToAdd.Count;

            selectedLines = cmbCustomers.SelectionList.Cast<DataEntity>().ToList();
            linesToAdd = cmbCustomers.AddList.Cast<DataEntity>().ToList();
            linesToRemove = cmbCustomers.RemoveList.Cast<DataEntity>().ToList();

            int curAddedCount = linesToAdd.Count;

            LoadPreviewLines();
            CheckEnabled();

            if (curAddedCount > prevAddedCount && !lvlEditPreview.RowIsOnScreen(lvlEditPreview.RowCount-1))
            {
                lvlEditPreview.ScrollRowIntoView(lvlEditPreview.RowCount - 1);
            }
        }
    }
}
