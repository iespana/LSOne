using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.DataTypes;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using static LSOne.Utilities.DataTypes.RecordIdentifier;

namespace LSOne.DataLayer.BusinessObjects.KitchenDisplaySystem
{
    public class LSOneHeaderPaneLineColumn : HeaderPaneLineColumn
    {
        public override RecordIdentifier ID
        {
            get 
            {
                return new RecordIdentifier(ColumnNumber, LineId); 
            } 
        }

        public PosStyle Style { get; set; }


        public LSOneHeaderPaneLineColumn() : base()
        {
            Style = new PosStyle();
        }

        public string ColumnTypeAsString()
        {
            switch (ColumnType)
            {
                case HdrPnColumnTypeEnum.Time:
                    return Properties.Resources.Time;
                case HdrPnColumnTypeEnum.StationName:
                    return Properties.Resources.StationID;
                case HdrPnColumnTypeEnum.PageNo:
                    return Properties.Resources.PageNo;
                case HdrPnColumnTypeEnum.KPI_APTD:
                    return Properties.Resources.APTD;
                case HdrPnColumnTypeEnum.KPI_APTH:
                    return Properties.Resources.APTH;
                case HdrPnColumnTypeEnum.AggregateGroups:
                    return Properties.Resources.AggregateGroups;
                case HdrPnColumnTypeEnum.Text:
                    return Properties.Resources.Caption;
                default:
                    return string.Empty;
            }
        }

        public string ColumnAlignmentAsString()
        {
            switch (ColumnAlignment)
            {
                case HdrPnColumnAlignmentEnum.Left:
                    return Properties.Resources.Left;
                case HdrPnColumnAlignmentEnum.Center:
                    return Properties.Resources.Center;
                case HdrPnColumnAlignmentEnum.Right:
                    return Properties.Resources.Right;
                default:
                    return string.Empty;
            }
        }

        public static HeaderPaneLineColumn ToKDSHeaderPaneLineColumn(LSOneHeaderPaneLineColumn headerColumn)
        {
            HeaderPaneLineColumn kdsHeaderColumn = new HeaderPaneLineColumn();

            kdsHeaderColumn.ColumnNumber = headerColumn.ColumnNumber;
            kdsHeaderColumn.LineId = headerColumn.LineId;
            kdsHeaderColumn.HeaderProfileId = headerColumn.HeaderProfileId;
            kdsHeaderColumn.HeaderProfileId.SerializationDBType = SqlDbType.NVarChar;
            kdsHeaderColumn.HeaderProfileId.SerializationType = RecordIdentifierType.String;
            kdsHeaderColumn.ColumnType = headerColumn.ColumnType;
            kdsHeaderColumn.Text = headerColumn.Text;
            kdsHeaderColumn.ColumnAlignment = headerColumn.ColumnAlignment;
            kdsHeaderColumn.ColorStyle = PosStyle.ToBaseStyle(headerColumn.Style);
            kdsHeaderColumn.FontStyle = (FontStyle)kdsHeaderColumn.ColorStyle.FontStyle;

            return kdsHeaderColumn;
        }

        public static List<HeaderPaneLineColumn> ToKDSHeaderPaneLineColumn(List<LSOneHeaderPaneLineColumn> headerColumns)
        {
            return headerColumns.ConvertAll(h => ToKDSHeaderPaneLineColumn(h));
        }
    }
}