using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.Controls.Rows;
using LSOne.DataLayer.DDBusinessObjects;
using LSOne.ViewPlugins.Scheduler.Properties;
using LSRetail.DD.Common;
using ListView = LSOne.Controls.ListView;

namespace LSOne.ViewPlugins.Scheduler.Controls
{
    [DefaultEvent("LocationsSelected")]
    public partial class LocationListControl : UserControl
    {
        private JscLocation[] selectedLocations;
        private List<LocationKind> locationKinds;

        public LocationListControl()
        {
            InitializeComponent();

            locationKinds = new List<LocationKind> {LocationKind.Undefined, 
                                                    LocationKind.HeadOffice, 
                                                    LocationKind.Store, 
                                                    LocationKind.Terminal, 
                                                    LocationKind.General};
        }

        public void Clear()
        {
            lvLocations.ClearRows();
            selectedLocations = null;
        }

        public void LoadData(IEnumerable<JscLocation> locations)
        {
            lvLocations.ClearRows();

            foreach (var kind in locationKinds)
            {
                List<SubRow> subRows = new List<SubRow>();
                ColumnCollection subRowColumns = new ColumnCollection() { new Column("", 25), new Column(Properties.Resources.Name, 200, 10, 0, true), new Column(Properties.Resources.ID, 80, 10, 0, true) };
                foreach (var location in locations.Where(l => l.LocationKind == kind))
                {
                    subRows.Add(LocationSubRow(location, subRowColumns));
                }
                if (subRows.Count > 0)
                {
                    Row row = new Row();
                    var locationKindString = Utils.Utils.EnumResourceString(Resources.ResourceManager, kind);
                    CollapsableCell collapsableCell = new CollapsableCell(subRowColumns, subRows, locationKindString, true);
                    row.AddCell(collapsableCell);
                    row.Tag = kind;
                    lvLocations.AddRow(row);
                    collapsableCell.AutoSizeColumns(lvLocations);
                    collapsableCell.SetCollapsed(lvLocations, lvLocations.RowCount - 1, false);
                }
            }
            lvLocations.AutoSizeColumns();
            selectedLocations = null;
        }

        private Image LocationImage(JscLocation location)
        {
            Image result;
            switch (location.LocationKind)
            {
                case LocationKind.General:
                    result = location.Enabled ? Properties.Resources.localization_16 : Properties.Resources.localization_disabled_16;
                    break;
                case LocationKind.HeadOffice:
                    result =  location.Enabled ? Properties.Resources.HeadOfficeImage : Properties.Resources.HeadOfficeDisabledImage;
                    break;
                case LocationKind.Store:
                    result =  location.Enabled ? Properties.Resources.store_16 : Properties.Resources.store_disabled_16;
                    break;
                case LocationKind.Terminal:
                    result =  location.Enabled ? Properties.Resources.terminal_16 : Properties.Resources.terminal_disabled_16;
                    break;
                default:
                    result =  location.Enabled ? Properties.Resources.ClearImage : Properties.Resources.HeadOfficeDisabledImage;
                    break;
            }
            return result;
        }

        private SubRow LocationSubRow(JscLocation location, ColumnCollection subRowColumns)
        {

            SubRow row = new SubRow(subRowColumns);
            IconButton button = new IconButton(LocationImage(location), "", true, false);
            IconButtonCell imageCell = new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter);
            row.AddCell(imageCell);
            row.AddText(location.Text);
            row.AddText(location.ExCode);
            row.Tag = location;
            return row;
        }

        public void AddLocation(JscLocation location)
        {
            Row collapsableRow = null;
            foreach (var row in lvLocations.Rows.Where(r => r.Tag is LocationKind))
            {
                LocationKind tag = (LocationKind)row.Tag;
                if (tag == location.LocationKind)
                {
                    collapsableRow = row;
                    break;
                }
            }

            ColumnCollection subRowColumns = collapsableRow != null 
                ? ((CollapsableCell) collapsableRow[0]).Columns 
                : new ColumnCollection() { new Column("", 25), new Column(Properties.Resources.Name, 200, 100, 0, true), new Column(Properties.Resources.ID, 80, 50, 0, true) };
            List<SubRow> subRows = new List<SubRow> {LocationSubRow(location, subRowColumns)};
            if (collapsableRow == null)
            {
                Row row = new Row();
                var locationKindString = Utils.Utils.EnumResourceString(Resources.ResourceManager, location.LocationKind);
                CollapsableCell collapsableCell = new CollapsableCell(subRowColumns, subRows, locationKindString, true);
                row.AddCell(collapsableCell);
                row.Tag = location.LocationKind;
                lvLocations.AddRow(row);
                return;
            }
            ((CollapsableCell)collapsableRow[0]).AddSubRows(subRowColumns, subRows);
           
        }

        public void RemoveLocation(JscLocation location)
        {
            Row rowToRemove = lvLocations.Rows.FirstOrDefault(r => r.Tag is JscLocation && r.Tag == location);
            lvLocations.RemoveRow(lvLocations.Rows.IndexOf(rowToRemove));
        }


        public void UpdateLocation(JscLocation location)
        {
            Row rowToUpdate = lvLocations.Rows.FirstOrDefault(r => r.Tag is JscLocation && r.Tag == location);
            SetRow(rowToUpdate, location);
        }

        private void SetRow(Row row, JscLocation location)
        {
            IconButton button = new IconButton(LocationImage(location), "");
            IconButtonCell imageCell = new IconButtonCell(button, IconButtonCell.IconButtonIconAlignmentEnum.HorizontalCenter);
            row.Clear();
            row.AddCell(imageCell);
            row.AddText(location.Text);
            row.AddText(location.ExCode);
            row.Tag = location;
        }

        [Browsable(false)]
        public JscLocation SelectedLocation
        {
            get
            {
                return SelectedLocations.Length > 0 ? SelectedLocations[0] : null;
            }
        }

        [Browsable(false)]
        public Guid? SelectedLocationId
        {
            get
            {
                if (SelectedLocations.Length > 0)
                    return (Guid) SelectedLocations[0].ID;
                return null;
            }
            set
            {
                if (value != null)
                {
                    lvLocations.Selection.Clear();
                    foreach (var row in lvLocations.Rows.Where(r => r.Tag is JscLocation && ((JscLocation)r.Tag).ID == value.Value))
                    {
                        int rowIndex = lvLocations.Rows.IndexOf(row);
                        lvLocations.Selection.AddRows(rowIndex, rowIndex);
                    }
                }
            }
        }


        [Browsable(false)]
        public JscLocation[] SelectedLocations
        {
            get
            {
                if (selectedLocations == null)
                {
                    List<JscLocation> selectedList = new List<JscLocation>();
                    for (int i = 0; i < lvLocations.Selection.Count; i++)
                    {
                        var tag = lvLocations.Rows[lvLocations.Selection.GetRowIndex(i)].Tag;
                        if (tag is JscLocation)
                            selectedList.Add((JscLocation)lvLocations.Rows[lvLocations.Selection.GetRowIndex(i)].Tag);
                    }
                    selectedLocations = selectedList.ToArray();
                }
                return selectedLocations;
            }
        }

        public event EventHandler<EventArgs> LocationsSelected;

        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool MultiSelect
        {
            get { return lvLocations.SelectionModel == ListView.SelectionModelEnum.FullRowMultiSelection; }
            set 
            { 
                lvLocations.SelectionModel = value 
                                    ? ListView.SelectionModelEnum.FullRowMultiSelection 
                                    : ListView.SelectionModelEnum.FullRowSingleSelection;
            }
        }

        [Browsable(true)]
        [DefaultValue("")]
        [Category("Behavior")]
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                return lvLocations.ContextMenuStrip;
            }
            set
            {
                lvLocations.ContextMenuStrip = value;
            }
        }

        private void lvLocations_SelectionChanged(object sender, EventArgs e)
        {
            selectedLocations = null;
            if (LocationsSelected != null)
            {
                LocationsSelected(this, EventArgs.Empty);
            }
        }

        private void lvLocations_RowDoubleClick(object sender, LSOne.Controls.EventArguments.RowEventArgs args)
        {
            OnDoubleClick(EventArgs.Empty);
        }
    }

}
