using System;
using System.Collections.Generic;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.TouchButtons;
using LSOne.DataLayer.KDSBusinessObjects.Enums;
using LSOne.Utilities.DataTypes;
using static LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayButton;

namespace LSOne.DataLayer.KDSBusinessObjects
{
    /// <summary>
    /// This class is exactly the same as KitchenDisplayStyleProfile, but instead of UIStyle it uses PosStyle.
    /// The reason for this is that the KDS uses UIStyle objects while in LSOne we use PosStyle. We then map PosStyle over to UIStyle.
    /// </summary>
    public class LSOneKitchenDisplayStyleProfile : DataEntity
    {
        public LSOneKitchenDisplayStyleProfile()
        {
            OrderPaneStyle = new PosStyle();
            OrderStyle = new PosStyle();
            ItemDefaultStyle = new PosStyle();
            ItemOnTimeStyle = new PosStyle();
            ItemDoneStyle = new PosStyle();
            ItemModifiedStyle = new PosStyle();
            ItemVoidedStyle = new PosStyle();
            ItemRushStyle = new PosStyle();
            ItemStartedStyle = new PosStyle();
            ItemServedStyle = new PosStyle();
            DefaultFooterStyle = new PosStyle();
            DefaultHeaderStyle = new PosStyle();
            NormalItemModifierStyle = new PosStyle();
            IncreaseItemModifierStyle = new PosStyle();
            DecreaseItemModifierStyle = new PosStyle();
            CommentModifierStyle = new PosStyle();
            ItemModifierModifiedStyle = new PosStyle();
            ItemModifierVoidedStyle = new PosStyle();
            ItemModifierGlyphStyle = new PosStyle();
            DealHeaderStyle = new PosStyle();
            DealHeaderVoidedStyle = new PosStyle();
            HeavyItemModifierStyle = new PosStyle();
            LightItemModifierStyle = new PosStyle();
            OnlyItemModifierStyle = new PosStyle();
            DoneItemModifierStyle = new PosStyle();
            TransactCommentStyle = new PosStyle();
            AlertStyle = new PosStyle();
            AggregateHeaderStyle = new PosStyle();
            AggregateBodyStyle = new PosStyle();
            AggregatePaneStyle = new PosStyle();
            HistoryHeaderStyle = new PosStyle();
            HistoryBodyStyle = new PosStyle();
            HistoryPaneStyle = new PosStyle();
            HeaderPaneStyle = new PosStyle();
            ButtonDefaultStyle = new PosStyle();
            ButtonNextStyle = new PosStyle();
            ButtonPreviousStyle = new PosStyle();
            ButtonBumpStyle = new PosStyle();
            ButtonStartStyle = new PosStyle();
            ButtonNextScreenStyle = new PosStyle();
            ButtonPreviousScreenStyle = new PosStyle();
            ButtonSelectStartStyle = new PosStyle();
            ButtonSelectBumpStyle = new PosStyle();
            ButtonRecallStyle = new PosStyle();
            ButtonHomeStyle = new PosStyle();
            ButtonEndStyle = new PosStyle();
            ButtonMarkStyle = new PosStyle();
            ButtonServeStyle = new PosStyle();
            ButtonTransferStyle = new PosStyle();
            ButtonRushStyle = new PosStyle();
            TimeStyles = new List<KitchenDisplayTimeStyle>();
        }
        
        public override RecordIdentifier ID { get { return base.ID; } set { base.ID = value; } }
        
        public override string Text { get { return base.Text; } set { base.Text = value; } }
        
        public PosStyle OrderPaneStyle { get; set; }
        
        public PosStyle OrderStyle { get; set; }
        
        public PosStyle ItemDefaultStyle { get; set; }
        
        public PosStyle ItemOnTimeStyle { get; set; }
        
        public PosStyle ItemDoneStyle { get; set; }
        
        public PosStyle ItemModifiedStyle { get; set; }
        
        public PosStyle ItemVoidedStyle { get; set; }
        
        public PosStyle ItemStartedStyle { get; set; }
        
        public PosStyle ItemServedStyle { get; set; }
        
        public PosStyle ItemRushStyle { get; set; }
        
        public PosStyle DefaultFooterStyle { get; set; }
        
        public PosStyle DefaultHeaderStyle { get; set; }
        
        public PosStyle TransactCommentStyle { get; set; }
        
        public DoneChitsOverlayEnum DoneChitOverlayStyle { get; set; }
        
        public PosStyle AlertStyle { get; set; }
        
        public PosStyle NormalItemModifierStyle { get; set; }
        
        public PosStyle IncreaseItemModifierStyle { get; set; }
        
        public PosStyle DecreaseItemModifierStyle { get; set; }
        
        public PosStyle CommentModifierStyle { get; set; }
        
        public PosStyle ItemModifierModifiedStyle { get; set; }
        
        public PosStyle ItemModifierVoidedStyle { get; set; }
        
        public PosStyle ItemModifierGlyphStyle { get; set; }
        
        public PosStyle DealHeaderStyle { get; set; }
        
        public PosStyle DealHeaderVoidedStyle { get; set; }
        
        public PosStyle HeavyItemModifierStyle { get; set; }
        
        public PosStyle LightItemModifierStyle { get; set; }
        
        public PosStyle OnlyItemModifierStyle { get; set; }
        
        public PosStyle DoneItemModifierStyle { get; set; }
        
        public PosStyle AggregateHeaderStyle { get; set; }
        
        public PosStyle AggregateBodyStyle { get; set; }
        
        public PosStyle AggregatePaneStyle { get; set; }
        
        public PosStyle HistoryHeaderStyle { get; set; }
        
        public PosStyle HistoryBodyStyle { get; set; }
        
        public PosStyle HistoryPaneStyle { get; set; }
        
        public PosStyle HeaderPaneStyle { get; set; }

        public PosStyle ButtonDefaultStyle { get; set; }

        public PosStyle ButtonNextStyle { get; set; }

        public PosStyle ButtonPreviousStyle { get; set; }

        public PosStyle ButtonBumpStyle { get; set; }

        public PosStyle ButtonStartStyle { get; set; }

        public PosStyle ButtonNextScreenStyle { get; set; }

        public PosStyle ButtonPreviousScreenStyle { get; set; }

        public PosStyle ButtonSelectStartStyle { get; set; }

        public PosStyle ButtonSelectBumpStyle { get; set; }

        public PosStyle ButtonRecallStyle { get; set; }

        public PosStyle ButtonHomeStyle { get; set; }

        public PosStyle ButtonEndStyle { get; set; }

        public PosStyle ButtonMarkStyle { get; set; }

        public PosStyle ButtonServeStyle { get; set; }

        public PosStyle ButtonTransferStyle { get; set; }

        public PosStyle ButtonRushStyle { get; set; }

        public List<KitchenDisplayTimeStyle> TimeStyles { get; set; }


        public string DoneChitOverlayStyleText(DoneChitsOverlayEnum doneChitOverlay)
        {
            switch (doneChitOverlay)
            {
                case DoneChitsOverlayEnum.BezierCurve:
                    return "Bezier curve";
                case DoneChitsOverlayEnum.SolidGreen:
                    return "Solid green color";
                default:
                    return "";
            }
        }

        public bool PercentOfCookingTimeInUse()
        {
            //Are the timestyles applied by absoulute time (overdue) or by percent of cooking time
            //It's enough to check on the first one because all of them are set to the same value
            if ((TimeStyles != null) && (TimeStyles.Count > 0))
            {
                return TimeStyles[0].UsePercentOfCookingTime;
            }
            return false;
        }

        public static KitchenDisplayStyleProfile ToKDSObject(LSOneKitchenDisplayStyleProfile profile)
        {
            KitchenDisplayStyleProfile kdsProfile = new KitchenDisplayStyleProfile();
            kdsProfile.ID = profile.ID;
            kdsProfile.Text = profile.Text;
            kdsProfile.DoneChitOverlayStyle = profile.DoneChitOverlayStyle;
            kdsProfile.TimeStyles = profile.TimeStyles;

            kdsProfile.OrderPaneUiStyle = PosStyle.ToUIStyle(profile.OrderPaneStyle);
            kdsProfile.OrderUiStyle = PosStyle.ToUIStyle(profile.OrderStyle);
            kdsProfile.ItemDefaultUiStyle = PosStyle.ToUIStyle(profile.ItemDefaultStyle);
            kdsProfile.ItemOnTimeUiStyle = PosStyle.ToUIStyle(profile.ItemOnTimeStyle);
            kdsProfile.ItemDoneUiStyle = PosStyle.ToUIStyle(profile.ItemDoneStyle);
            kdsProfile.ItemModifiedUiStyle = PosStyle.ToUIStyle(profile.ItemModifiedStyle);
            kdsProfile.ItemVoidedUiStyle = PosStyle.ToUIStyle(profile.ItemVoidedStyle);
            kdsProfile.ItemRushUiStyle = PosStyle.ToUIStyle(profile.ItemRushStyle);
            kdsProfile.ItemStartedUiStyle = PosStyle.ToUIStyle(profile.ItemStartedStyle);
            kdsProfile.ItemServedUiStyle = PosStyle.ToUIStyle(profile.ItemServedStyle);
            kdsProfile.DefaultFooterUiStyle = PosStyle.ToUIStyle(profile.DefaultFooterStyle);
            kdsProfile.DefaultHeaderUiStyle = PosStyle.ToUIStyle(profile.DefaultHeaderStyle);
            kdsProfile.NormalItemModifierUiStyle = PosStyle.ToUIStyle(profile.NormalItemModifierStyle);
            kdsProfile.IncreaseItemModifierUiStyle = PosStyle.ToUIStyle(profile.IncreaseItemModifierStyle);
            kdsProfile.DecreaseItemModifierUiStyle = PosStyle.ToUIStyle(profile.DecreaseItemModifierStyle);
            kdsProfile.CommentModifierUiStyle = PosStyle.ToUIStyle(profile.CommentModifierStyle);
            kdsProfile.ItemModifierModifiedUiStyle = PosStyle.ToUIStyle(profile.ItemModifiedStyle);
            kdsProfile.ItemModifierVoidedUiStyle = PosStyle.ToUIStyle(profile.ItemModifierVoidedStyle);
            kdsProfile.ItemModifierGlyphUiStyle = PosStyle.ToUIStyle(profile.ItemModifierGlyphStyle);
            kdsProfile.DealHeaderUiStyle = PosStyle.ToUIStyle(profile.DealHeaderStyle);
            kdsProfile.DealHeaderVoidedUiStyle = PosStyle.ToUIStyle(profile.DealHeaderVoidedStyle);
            kdsProfile.HeavyItemModifierUiStyle = PosStyle.ToUIStyle(profile.HeavyItemModifierStyle);
            kdsProfile.LightItemModifierUiStyle = PosStyle.ToUIStyle(profile.LightItemModifierStyle);
            kdsProfile.OnlyItemModifierUiStyle = PosStyle.ToUIStyle(profile.OnlyItemModifierStyle);
            kdsProfile.DoneItemModifierUiStyle = PosStyle.ToUIStyle(profile.DoneItemModifierStyle);
            kdsProfile.TransactCommentHeaderUiStyle = PosStyle.ToUIStyle(profile.TransactCommentStyle);
            kdsProfile.AlertUiStyle = PosStyle.ToUIStyle(profile.AlertStyle);
            kdsProfile.AggregateHeaderUiStyle = PosStyle.ToUIStyle(profile.AggregateHeaderStyle);
            kdsProfile.AggregateBodyUiStyle = PosStyle.ToUIStyle(profile.AggregateBodyStyle);
            kdsProfile.AggregatePaneUiStyle = PosStyle.ToUIStyle(profile.AggregatePaneStyle);
            kdsProfile.HistoryHeaderUiStyle = PosStyle.ToUIStyle(profile.HistoryHeaderStyle);
            kdsProfile.HistoryBodyUiStyle = PosStyle.ToUIStyle(profile.HistoryBodyStyle);
            kdsProfile.HistoryPaneUiStyle = PosStyle.ToUIStyle(profile.HistoryPaneStyle);
            kdsProfile.HeaderPaneUiStyle = PosStyle.ToUIStyle(profile.HeaderPaneStyle);

            // Button styles

            List<KdsButtonOperationStyle> operationStyles = new List<KdsButtonOperationStyle>();
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Next, profile.ButtonNextStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Previous, profile.ButtonPreviousStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Bump, profile.ButtonBumpStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Start, profile.ButtonStartStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.NextScreen, profile.ButtonNextScreenStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.PreviousScreen, profile.ButtonPreviousScreenStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.SelectStart, profile.ButtonSelectStartStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.SelectBump, profile.ButtonSelectBumpStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.RecallLastBump, profile.ButtonRecallStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Home, profile.ButtonHomeStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.End, profile.ButtonEndStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Mark, profile.ButtonMarkStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Serve, profile.ButtonServeStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Transfer, profile.ButtonTransferStyle));
            operationStyles.Add(CreateOperationStyle(ButtonActionEnum.Rush, profile.ButtonRushStyle));

            kdsProfile.ButtonStyleProfile.ProfileID = kdsProfile.ButtonStyleProfileID = (string)profile.ID;
            kdsProfile.ButtonStyleProfile.Description = profile.Text;
            kdsProfile.ButtonStyleProfile.StyleID = (string)profile.ButtonDefaultStyle.ID;
            kdsProfile.ButtonStyleProfile.Style = PosStyle.ToUIStyle(profile.ButtonDefaultStyle);
            kdsProfile.ButtonStyleProfile.OperationStyles = operationStyles;

            return kdsProfile;
        }

        public static List<KitchenDisplayStyleProfile> ToKDSObject(List<LSOneKitchenDisplayStyleProfile> profiles)
        {
            return profiles.ConvertAll(profile => ToKDSObject(profile));
        }

        private static KdsButtonOperationStyle CreateOperationStyle(ButtonActionEnum operation, PosStyle style)
        {
            return new KdsButtonOperationStyle()
            {
                Operation = operation,
                StyleID = (string)style.ID,
                Style = PosStyle.ToUIStyle(style),
                BackColorEnabled = style.ID.StringValue != string.Empty,
                ForeColorEnabled = style.ID.StringValue != string.Empty
            };
        }
    }
}