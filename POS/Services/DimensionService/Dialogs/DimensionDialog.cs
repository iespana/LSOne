using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.EventArguments;
using LSOne.Controls.Rows;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.DataLayer.DataProviders;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Services.Interfaces.Constants;
using LSOne.Services.Interfaces.SupportInterfaces;
using LSOne.Services.ListViewExtensions;
using LSOne.Services.SupportClasses;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LSOne.Services.Dialogs
{
    public partial class DimensionDialog : TouchBaseForm
    {
        /// <summary>
        /// Used to preserve the states of the dimension header rows
        /// </summary>
        private class DimensionVisibilityState
        {
            public bool Expanded;
            public bool Collapsable;
        }

        private bool noDimensions = false;
        private RecordIdentifier masterItemID;
        private List<RetailItemDimension> dimensions;
        private Dictionary<RecordIdentifier, List<DimensionAttribute>> retailItemAttributeRelations;
        private Dictionary<RecordIdentifier, List<DimensionAttribute>> dimensionAttributesFilter;
        private Dictionary<RecordIdentifier, DimensionVisibilityState> dimensionVisibilityState;
        private Dictionary<RecordIdentifier, RecordIdentifier> selectedDimensionAttribute;
        private Dictionary<RecordIdentifier, List<DimensionAttribute>> allDimensionAttributes;

        private Cell clickedCell;        
        private RecordIdentifier selectedItemID;
        private int clickedRowNumber;
        private int collapsableRowThreshold = 8 * 5; // Number of attribute rows needed to enable the "collapsable" functionality. The reason for multiplication is that there
                                                     // are 8 attributes per row. This makes it easier to figure out.

        private IConnectionManager dlgEntry;
        private ISettings dlgSettings;

        public DimensionDialog(IConnectionManager entry, ISaleLineItem saleLineItem) : this(entry, saleLineItem.MasterID, saleLineItem.Description) { }

        public DimensionDialog(IConnectionManager entry, RecordIdentifier masterItemID, string masterItemName)
        {
            InitializeComponent();

            this.SuspendLayout();
            touchDialogBanner1.BannerText = masterItemName;
            lvDimensions.HeaderHeight = 0;
            lvDimensions.ForeColor = ColorPalette.POSTextColor;
            lvDimensions.BorderColor = ColorPalette.POSDialogBackgroundColor;
            lvDimensions.RowLines = false;
            lvDimensions.VerticalScrollbar = false;
            lvDimensions.DefaultStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lvDimensions.DefaultStyle.TextColor = ColorPalette.POSTextColor;
            this.ResumeLayout();

            dlgEntry = entry;
            dlgSettings = (ISettings)entry.Settings.GetApplicationSettings(ApplicationSettingsConstants.POSApplication);

            this.masterItemID = masterItemID;

            retailItemAttributeRelations = new Dictionary<RecordIdentifier, List<DimensionAttribute>>();
            dimensionAttributesFilter = new Dictionary<RecordIdentifier, List<DimensionAttribute>>();
            dimensionVisibilityState = new Dictionary<RecordIdentifier, DimensionVisibilityState>();
            selectedDimensionAttribute = new Dictionary<RecordIdentifier, RecordIdentifier>();
            allDimensionAttributes = new Dictionary<RecordIdentifier, List<DimensionAttribute>>();
        }

        public RecordIdentifier SelectedItemID => selectedItemID;

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);            

            LoadDimensions();

            lvDimensions.Focus();
        }

        private void LoadDimensions()
        {
            Exception exception;
            Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => LoadDimensionData(), Properties.Resources.PleaseWait, Properties.Resources.GettingItemInformation, out exception);

            if (exception != null)
            {
                throw exception;
            }

            if (noDimensions)
            {
                lblNoVariantsAvailable.Visible = true;
                btnOK.Visible = false;
                lvDimensions.Visible = false;
            }
            else
            {
                //Figure out if we need a scrollbar
                int totalHeight = 0;

                foreach(var dim in dimensions)
                {
                    totalHeight += 41; //Dimension text row;
                    int rowCount = (allDimensionAttributes[dim.ID].Count + 7) / 8;
                    totalHeight += rowCount * 50;
                }

                if(totalHeight > lvDimensions.Height)
                {
                    lvDimensions.VerticalScrollbar = true;
                    lvDimensions.VerticalScrollbarYOffset = 1;
                    lvDimensions.Width -= 3;
                    Width += 15;
                }
            }
            
            LoadRows();
        }

        private void LoadDimensionData()
        {
            dimensions = Providers.RetailItemDimensionData.GetListInUseByRetailItem(dlgEntry, masterItemID);
            List<DimensionAttribute> attributesInUse = Providers.DimensionAttributeData.GetAttributesInUseByItem(dlgEntry, masterItemID);

            if (dimensions.Count == 0)
            {
                noDimensions = true;
                return;

            }
            // Construct a dictionary for dimension-attribute relations
            foreach (RetailItemDimension dimension in dimensions)
            {
                allDimensionAttributes.Add(dimension.ID, attributesInUse.Where(p => p.DimensionID == dimension.ID).ToList());
            }

            retailItemAttributeRelations = Providers.DimensionAttributeData.GetRetailItemDimensionAttributeRelations(dlgEntry, new RecordIdentifier(masterItemID), false);
        }

        private void LoadRows()
        {
            int selectedRow = lvDimensions.Selection.FirstSelectedRow;
            lvDimensions.ClearRows();
            Row row;

            for (int i = 0; i < dimensions.Count; i++)
            {
                if (i > 0)
                {
                    //Add an empty row to have more space between dimensions and less between text and attributes
                    lvDimensions.AddRow(new NonSelectableRow() { Height = 3, BackColor = ColorPalette.POSDialogBackgroundColor });
                }

                RetailItemDimension dimension = dimensions[i];
                row = new CollapsableRowTouch();
                row.AddText(dimension.Text);
                row.BackColor = ColorPalette.POSDialogBackgroundColor;
                row.Height = 34;
                row.Tag = dimension.ID;

                if (dimensionVisibilityState.ContainsKey(dimension.ID))
                {
                    ((CollapsableRowTouch) row).Expaned = dimensionVisibilityState[dimension.ID].Expanded;
                    ((CollapsableRowTouch) row).Collapsable = dimensionVisibilityState[dimension.ID].Collapsable;
                }

                lvDimensions.AddRow(row);

                if (lvDimensions.RowCount - 1 == selectedRow)
                {
                    lvDimensions.Selection.Set(selectedRow);
                }

                if (i == dimensions.Count - 1)
                {
                    ((CollapsableRowTouch) row).IsLastRow = true;
                }

                if (dimensionVisibilityState.ContainsKey(dimension.ID) && !dimensionVisibilityState[dimension.ID].Expanded)
                {
                    continue;
                }

                List<DimensionAttributeExtended> attributes = new List<DimensionAttributeExtended>();

                if (!dimensionVisibilityState.ContainsKey(dimension.ID))
                {
                    ((CollapsableRowTouch) row).Collapsable = attributes.Count > collapsableRowThreshold;
                }

                RecordIdentifier selectedAttributeID = null;

                // Checking for selected attributes
                if (selectedDimensionAttribute.ContainsKey(dimension.ID))
                {
                    selectedAttributeID = allDimensionAttributes[dimension.ID].Single(p => p.ID == selectedDimensionAttribute[dimension.ID]).ID;
                }

                foreach (DimensionAttribute attribute in allDimensionAttributes[dimension.ID])
                {
                    DimensionAttributeExtended extendedAttribute = new DimensionAttributeExtended(attribute, true, selectedAttributeID == attribute.ID);

                    if(dimensionAttributesFilter.Count > 0 && !dimensionAttributesFilter[dimension.ID].Any(x => x.ID == attribute.ID))
                    {
                        extendedAttribute.Selectable = false;
                    }

                    attributes.Add(extendedAttribute);
                }

                List<DimensionAttributeExtended> attributeBuffer = new List<DimensionAttributeExtended>();

                bool firstRow = true;
                for (int j = 0; j < attributes.Count; j++)
                {
                    attributeBuffer.Add(attributes[j]);

                    if ((j + 1) % 8 == 0 || (j + 1) == attributes.Count && attributes.Count < 8)
                    {
                        row = new NonSelectableRow();
                        row.AddCell(new DimensionAttributeCell(dimension.ID, attributeBuffer, firstRow));
                        row.Height = 50;
                        row.Tag = RecordIdentifier.Empty;

                        if (lvDimensions.RowCount - 1 == selectedRow)
                        {
                            lvDimensions.Selection.Set(selectedRow);
                        }

                        lvDimensions.AddRow(row);

                        if(firstRow)
                        {
                            firstRow = false;
                        }

                        attributeBuffer.Clear();
                    }
                }

                // Dump the rest of the buffer into a row. This happens when we have less than 8 items left from the attributes to add
                if (attributeBuffer.Count > 0)
                {
                    row = new NonSelectableRow();
                    row.AddCell(new DimensionAttributeCell(dimension.ID, attributeBuffer, firstRow));
                    row.Height = 50;
                    row.Tag = RecordIdentifier.Empty;

                    if (lvDimensions.RowCount - 1 == selectedRow)
                    {
                        lvDimensions.Selection.Set(selectedRow);
                    }

                    lvDimensions.AddRow(row);
                }
            }

            lvDimensions.AutoSizeColumns();

            if (!lvDimensions.RowIsOnScreen(selectedRow))
            {
                lvDimensions.ScrollRowIntoView(selectedRow);
            }
        }

        private void lvDimensions_CellAction(object sender, CellEventArgs args)
        {
            DimensionAttributeCell cell = (DimensionAttributeCell)args.Cell;
            if (selectedDimensionAttribute.ContainsKey(cell.DimensionID))
            {
                if (cell.SelectedAttributeID == null)
                {
                    // De-select attribute
                    selectedDimensionAttribute.Remove(cell.DimensionID);
                }
                else
                {
                    selectedDimensionAttribute[cell.DimensionID] = cell.SelectedAttributeID;
                }
            }
            else
            {
                selectedDimensionAttribute.Add(cell.DimensionID, cell.SelectedAttributeID);
            }
            Exception ex;
            Interfaces.Services.DialogService(dlgEntry).ShowSpinnerDialog(() => GenerateDimensionAttributeFilter(), "", "", out ex);

            if (ex != null)
            {
                throw ex;
            }

            LoadRows();
            CheckForSelectedItem();
        }

        private void GenerateDimensionAttributeFilter()
        {
            dimensionAttributesFilter.Clear();

            if (selectedDimensionAttribute.Count == 0)
            {
                return;
            }

            foreach (KeyValuePair<RecordIdentifier, List<DimensionAttribute>> dimensionAttributesPair in allDimensionAttributes)
            {
                dimensionAttributesFilter.Add(dimensionAttributesPair.Key, new List<DimensionAttribute>());

                // For the current dimension we need to find the attributes that are available for all currently selected attributes
                List<RecordIdentifier> selectedAttributeIDs = new List<RecordIdentifier>(selectedDimensionAttribute.Values);

                foreach (KeyValuePair<RecordIdentifier, List<DimensionAttribute>> retailItemAttributeRelation in retailItemAttributeRelations)
                {
                    // Checking if this relation contains an attribute that belongs to the dimension we are currently looking at
                    if (retailItemAttributeRelation.Value.Exists(p => dimensionAttributesPair.Value.Exists(p2 => p2.ID == p.ID)))
                    {
                        // For a relation to be valid it must contain all of the selected attributes
                        bool allAttributesFound = true;

                        // Here we examine each attribute that belongs to the variant item and check if contains all of the currently selected
                        // attribute IDs
                        foreach (var selectedAttribute in selectedDimensionAttribute)
                        {
                            if (selectedAttribute.Key != dimensionAttributesPair.Key && !retailItemAttributeRelation.Value.Exists(p => p.ID == selectedAttribute.Value))
                            {
                                allAttributesFound = false;
                                break;
                            }
                        }

                        if (allAttributesFound)
                        {
                            DimensionAttribute validAttribute = retailItemAttributeRelation.Value.Single(p => p.DimensionID == dimensionAttributesPair.Key);

                            if (!dimensionAttributesFilter[dimensionAttributesPair.Key].Exists(p => p.ID == validAttribute.ID))
                            {
                                dimensionAttributesFilter[dimensionAttributesPair.Key].Add(validAttribute);
                            }
                        }
                    }
                }
            }
        }

        private void lvDimensions_RowClick(object sender, RowEventArgs args)
        {
            if (args.Row is CollapsableRowTouch && ((CollapsableRowTouch)args.Row).Collapsable)
            {
                RecordIdentifier dimensionID = (RecordIdentifier)args.Row.Tag;


                if (!dimensionVisibilityState.ContainsKey(dimensionID))
                {
                    DimensionVisibilityState state = new DimensionVisibilityState();
                    state.Collapsable = ((CollapsableRowTouch) args.Row).Collapsable;
                    state.Expanded = !state.Collapsable;
                    dimensionVisibilityState.Add(dimensionID, state);
                }
                else
                {
                    if (dimensionVisibilityState[dimensionID].Collapsable)
                    {
                        dimensionVisibilityState[dimensionID].Expanded = !dimensionVisibilityState[dimensionID].Expanded;
                    }
                }

                LoadRows();
            }
        }

        /// <summary>
        /// Checks if the user has finished selecting a valid combination
        /// </summary>
        private void CheckForSelectedItem()
        {
            // The user has selected an attribute from every dimension
            if (dimensions.Count == selectedDimensionAttribute.Count)
            {
                selectedItemID = Providers.RetailItemData.GetMasterIDFromAttributes(dlgEntry, selectedDimensionAttribute.Values.ToList());
                btnOK.Enabled = true;
            }
            else
            {
                btnOK.Enabled = false;
            }
        }

        private void lvDimensions_MouseDown(object sender, MouseEventArgs e)
        {
            int rowNumber = -1;
            int columnNumber = -1;
            Rectangle cellBounds = Rectangle.Empty;
            bool isInContentArea = false;

            lvDimensions.CellFromPoint(e.Location, ref columnNumber, ref rowNumber, ref cellBounds, ref isInContentArea);

            if (rowNumber >= 0 && columnNumber > -1)
            {
                Cell cell = lvDimensions.Rows[rowNumber][(uint) columnNumber];

                if (cell != null && (cell is DimensionAttributeCell))
                {
                    ((DimensionAttributeCell)cell).ManualMouseDown(e.Location);
                    clickedCell = cell;
                    clickedRowNumber = rowNumber;
                }
            }
                                  
        }

        private void lvDimensions_MouseUp(object sender, MouseEventArgs e)
        {
            if (clickedCell != null)
            {
                if (((DimensionAttributeCell) clickedCell).ManualMouseUp(e.Location))
                {
                    lvDimensions.OnCellAction(clickedCell);
                    lvDimensions.InvalidateContent();
                    lvDimensions.Selection.Set(clickedRowNumber);
                    clickedCell = null;
                }
            }
        }

        private void lblNoVariantsAvailable_Paint(object sender, PaintEventArgs e)
        {
            Pen borderPen = new Pen(ColorPalette.POSControlBorderColor);
            e.Graphics.DrawRectangle(borderPen, new Rectangle(e.ClipRectangle.Location, new Size(e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1)));
            borderPen.Dispose();
        }
    }
}
