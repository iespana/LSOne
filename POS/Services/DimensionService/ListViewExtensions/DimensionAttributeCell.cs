using LSOne.Controls;
using LSOne.Controls.Cells;
using LSOne.Controls.Columns;
using LSOne.DataLayer.BusinessObjects.ItemMaster.Dimensions;
using LSOne.Services.SupportClasses;
using LSOne.Utilities.ColorPalette;
using LSOne.Utilities.DataTypes;
using LSOne.Utilities.GUI;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ListView = LSOne.Controls.ListView;

namespace LSOne.Services.ListViewExtensions
{
    public class DimensionAttributeCell : Cell
    {
        private int buttonHeight = 50;
        private int buttonWidth = 124;
        private List<TouchKey> keys;
        private bool mouseDown;
        private int mouseDownKeyHash;
        private RecordIdentifier dimensionID;        

        private List<DimensionAttributeExtended> attributes;
        private Font buttonFont;

        private BaseStyle normalButtonStyle;
        private BaseStyle selectedButtonStyle;
        private BaseStyle disabledButtonStyle;

        private bool firstRow;

        private DimensionAttributeCell(RecordIdentifier dimensionID)
        {
            keys = new List<TouchKey>();
            mouseDown = false;
            mouseDownKeyHash = 0;            
            attributes = new List<DimensionAttributeExtended>();
            this.dimensionID = dimensionID;
            buttonFont = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);

            normalButtonStyle = new BaseStyle();
            normalButtonStyle.Font = buttonFont;
            normalButtonStyle.ForeColor = ColorPalette.POSTextColor;
            normalButtonStyle.BackColor = ColorPalette.NormalButton;
            normalButtonStyle.BackColor2 = ColorPalette.NormalButton;

            selectedButtonStyle = new BaseStyle();
            selectedButtonStyle.Font = buttonFont;
            selectedButtonStyle.ForeColor = ColorPalette.POSWhite;
            selectedButtonStyle.BackColor = ColorPalette.POSConfirmButtonColor;
            selectedButtonStyle.BackColor2 = ColorPalette.POSConfirmButtonColor;

            disabledButtonStyle = new BaseStyle();
            disabledButtonStyle.Font = buttonFont;
            disabledButtonStyle.ForeColor = ColorPalette.POSGhostTextColor;
            disabledButtonStyle.BackColor = ColorPalette.POSWhite;
            disabledButtonStyle.BackColor2 = ColorPalette.POSWhite;
        }

        /// <summary>
        /// Creates a new instance of the cell with the given attributes visible
        /// </summary>
        /// <param name="dimensionID"></param>
        /// <param name="attributes"></param>
        public DimensionAttributeCell(RecordIdentifier dimensionID, List<DimensionAttributeExtended> attributes, bool firstRow = true) : this(dimensionID)
        {
            this.attributes = attributes;

            foreach (DimensionAttributeExtended attribute in attributes)
            {
                TouchKey key = new TouchKey(attribute.Text, attribute.ID);
                
                key.Style = attribute.Selectable ? (attribute.Selected ? selectedButtonStyle : normalButtonStyle) : disabledButtonStyle;
                key.Clickable = attribute.Selectable;
                keys.Add(key);

                if(attribute.Selected)
                {
                    SelectedAttributeID = attribute.ID;
                }
            }

            this.firstRow = firstRow;
        }

        /// <summary>
        /// The ID of the attribute currently selected
        /// </summary>
        public RecordIdentifier SelectedAttributeID { get; set; }

        public RecordIdentifier DimensionID => dimensionID;

        public override void Draw(Graphics g, Column column, Style defaultStyle, Rectangle cellBounds, Color overrideTextColor, bool hasVisualStyles, bool rowIsSelected)
        {
            Region oldClip = g.Clip;

            for (int i = 0; i < keys.Count; i++)
            {
                int heightOffset = firstRow ? 0 : 1;
                Rectangle buttonBounds = new Rectangle(7 + i * buttonWidth - i, cellBounds.Top - heightOffset, buttonWidth, buttonHeight + heightOffset);

                keys[i].SetBounds(buttonBounds);                
                keys[i].Draw(g, true, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter | TextFormatFlags.WordEllipsis, true);
            }

            g.Clip = oldClip;
        }

        protected override bool MouseDown(ListView ownerListView, Rectangle cellBounds, Point location, ref bool captureMouse, int columnNumber,
            int rowNumber, ref bool cancelEventAction)
        {
            // We have to return false to enable touch-scrolling on the list view. Actual mouse down handling is done manually
            return false;
        }

        public void ManualMouseDown(Point location)
        {
            if (!mouseDown)
            {
                foreach (TouchKey key in keys)
                {
                    if (key.HitTest(location))
                    {
                        mouseDownKeyHash = key.GetHashCode();
                        break;
                    }
                }
            }            
            mouseDown = true;
        }

        public bool ManualMouseUp(Point location)
        {
            // Check if the mouse has been moved
            if (mouseDown)
            {
                mouseDown = false;

                foreach (TouchKey key in keys)
                {
                    if (key.HitTest(location) && key.GetHashCode() == mouseDownKeyHash)
                    {
                        if(!key.Clickable)
                        {
                            return false;
                        }

                        mouseDownKeyHash = 0;

                        if(SelectedAttributeID != null && (RecordIdentifier)key.Tag == SelectedAttributeID)
                        {
                            SelectedAttributeID = null;
                        }
                        else
                        {
                            SelectedAttributeID = (RecordIdentifier)key.Tag;
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}