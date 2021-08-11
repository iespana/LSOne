using System;
using LSOne.DataLayer.BusinessObjects;
using LSOne.Utilities.DataTypes;

namespace LSOne.Services.Datalayer.BusinessObjects.Dimensions
{
    /// <summary>
    /// A dimension combination that can be assigned to the item. A combination can have one or more of the following dimensions; color, style or size.
    /// </summary>
    public class OldDimensionCombination : DataEntity
    {
        RecordIdentifier itemID;
        RecordIdentifier sizeID;
        RecordIdentifier styleID;
        RecordIdentifier colorID;

        string colorName;
        string sizeName;
        string styleName;

        /// <summary>
        /// The constructor for the class. All variables are set to default values
        /// </summary>
        public OldDimensionCombination()
            : base()
        {
            itemID = "";
            sizeID = "";
            styleID = "";
            colorID = "";

            colorName = "";
            sizeName = "";
            styleName = "";
        }

        /// <summary>
        /// The unique ID of the dimension
        /// </summary>
        public override RecordIdentifier ID
        {
            get
            {
                return new RecordIdentifier(itemID,
                    new RecordIdentifier(sizeID,
                    new RecordIdentifier(colorID,styleID)));
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The ID of the color group that the combination is created from
        /// </summary>
        public RecordIdentifier ColorID
        {
            get { return colorID; }
            set { colorID = value; }
        }

        /// <summary>
        /// The ID of the style group that the combination is created from
        /// </summary>
        public RecordIdentifier StyleID
        {
            get { return styleID; }
            set { styleID = value; }
        }

        /// <summary>
        /// The ID of the size group that the combination is created from
        /// </summary>
        public RecordIdentifier SizeID
        {
            get { return sizeID; }
            set { sizeID = value; }
        }

        /// <summary>
        /// The item that has this dimension on it
        /// </summary>
        public RecordIdentifier ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        /// <summary>
        /// The description of the color group
        /// </summary>
        public string ColorName
        {
            get { return colorName; }
            set { colorName = value; }
        }

        /// <summary>
        /// The description of the size group
        /// </summary>
        public string SizeName
        {
            get { return sizeName; }
            set { sizeName = value; }
        }

        /// <summary>
        /// The description of the style group
        /// </summary>
        public string StyleName
        {
            get { return styleName; }
            set { styleName = value; }
        }


        public int ColorSortingIndex { get; set; }
        public int SizeSortingIndex { get; set; }
        public int StyleSortingIndex { get; set; }

        public override string this[int index]
        {
            get
            {
                switch (index)
                {
                    case 1:
                        return (sizeID == "") ? "" : sizeID.ToString() + " - " + sizeName;

                    case 2:
                        return (colorID == "") ? "" : colorID.ToString() + " - " + colorName;

                    case 3:
                        return (styleID == "") ? "" : styleID.ToString() + " - " + styleName;

                    default:
                        return "";
                }

                
            }
        }
    }
}