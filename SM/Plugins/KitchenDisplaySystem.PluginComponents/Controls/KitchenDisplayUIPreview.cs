using LSOne.DataLayer.KDSBusinessObjects;
using LSOne.Utilities.ColorPalette;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using static LSOne.DataLayer.KDSBusinessObjects.KitchenDisplayVisualProfile;


namespace LSOne.ViewPlugins.KitchenDisplaySystem.PluginComponents.Controls
{
    public partial class KitchenDisplayUIPreview : UserControl
    {
        public KitchenDisplayUIPreview()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        public void AdjustPreview(KitchenDisplayVisualProfile visualProfile)
        {
            SuspendLayout();
            Controls.Remove(pnlOrder);
            Controls.Remove(pnlHeader);
            Controls.Remove(pnlButton);
            Controls.Remove(pnlAggregate);
            Controls.Remove(pnlHistory);

            SetSinglePane(pnlOrder, 0, visualProfile.HeaderPaneHeight, 1, 1 - visualProfile.HeaderPaneHeight, true, ColorPalette.White);
            SetSinglePane(pnlHeader, 0, 0, 1, visualProfile.HeaderPaneHeight, visualProfile.HeaderPaneHeight > 0, ColorPalette.GrayLight);
            SetSinglePane(pnlButton, 0, 0, 0, 0, visualProfile.ButtonPaneVisible, ColorPalette.GrayLight);
            SetSinglePane(pnlAggregate, 0, 0, 0, 0, visualProfile.AggregatePaneVisible, ColorPalette.GrayLight);
            SetSinglePane(pnlHistory, 0, 0, 0, 0, visualProfile.HistoryPaneVisible, ColorPalette.GrayLight);

            if(visualProfile.ButtonPaneVisible)
            {
                AdjustForButtonPanel(visualProfile);
            }
            if(visualProfile.AggregatePaneVisible)
            {
                AdjustForAggregatePanel(visualProfile);
            }
            if(visualProfile.HistoryPaneVisible)
            {
                AdjustForHistoryPanel(visualProfile);
            }

            Controls.Add(pnlOrder);
            Controls.Add(pnlHeader);
            Controls.Add(pnlButton);
            Controls.Add(pnlAggregate);
            Controls.Add(pnlHistory);
            ResumeLayout();
        }

        private void AdjustForButtonPanel(KitchenDisplayVisualProfile visualProfile)
        {
            switch (visualProfile.ButtonPanePosition)
            {
                case ButtonPositionEnum.Right:
                    pnlButton.Width = (int)Math.Round(visualProfile.ButtonPaneWidth * Width);
                    pnlButton.Height = Height;
                    pnlOrder.Width -= pnlButton.Width;
                    pnlButton.X = pnlOrder.Width;
                    break;
                case ButtonPositionEnum.Left:
                    pnlButton.Width = (int)Math.Round(visualProfile.ButtonPaneWidth * Width);
                    pnlButton.Height = Height;
                    pnlOrder.Width -= pnlButton.Width;
                    pnlOrder.X = pnlButton.Width;
                    break;
                case ButtonPositionEnum.Top:
                    pnlButton.Width = Width;
                    pnlButton.Height = (int)Math.Round(visualProfile.ButtonPaneHeight * Height);
                    pnlOrder.Height -= pnlButton.Height;
                    pnlOrder.MoveY(pnlButton.Height);
                    pnlHeader.Y = pnlButton.Height;
                    break;
                case ButtonPositionEnum.Bottom:
                    pnlButton.Width = Width;
                    pnlButton.Height = (int)Math.Round(visualProfile.ButtonPaneHeight * Height);
                    pnlOrder.Height -= pnlButton.Height;
                    pnlButton.Y = pnlOrder.Height + pnlHeader.Height;
                    break;
            }
        }

        private void AdjustForAggregatePanel(KitchenDisplayVisualProfile visualProfile)
        {
            switch (visualProfile.AggregatePanePosition)
            {
                case AggregatePositionEnum.Right:
                    pnlAggregate.Width = (int)Math.Round(visualProfile.AggregatePaneWidth * Width);
                    pnlOrder.Width -= pnlAggregate.Width;
                    pnlAggregate.Height = pnlOrder.Height;
                    pnlAggregate.X = pnlOrder.Width;
                    pnlAggregate.Y = pnlOrder.Y;

                    if(visualProfile.ButtonPaneVisible && visualProfile.ButtonPanePosition == ButtonPositionEnum.Left)
                    {
                        pnlAggregate.MoveX(pnlButton.Width);
                    }
                    break;
                case AggregatePositionEnum.Left:
                    pnlAggregate.Width = (int)Math.Round(visualProfile.AggregatePaneWidth * Width);
                    pnlOrder.Width -= pnlAggregate.Width;
                    pnlAggregate.Height = pnlOrder.Height;
                    pnlOrder.MoveX(pnlAggregate.Width);
                    pnlAggregate.MoveY(pnlOrder.Y);

                    if (visualProfile.ButtonPaneVisible && visualProfile.ButtonPanePosition == ButtonPositionEnum.Left)
                    {
                        pnlAggregate.X = pnlButton.Width;
                    }
                    break;
                case AggregatePositionEnum.Top:
                    pnlAggregate.Width = Width;
                    pnlAggregate.Height = (int)Math.Round(visualProfile.AggregatePaneHeight * Height);
                    pnlOrder.Height -= pnlAggregate.Height;
                    pnlAggregate.X = 0;
                    pnlOrder.MoveY(pnlAggregate.Height);
                    pnlHeader.MoveY(pnlAggregate.Height);

                    if(visualProfile.ButtonPaneVisible && visualProfile.ButtonPanePosition == ButtonPositionEnum.Top)
                    {
                        pnlAggregate.MoveY(pnlButton.Height);
                    }
                    break;
                case AggregatePositionEnum.Bottom:
                    pnlAggregate.Width = Width;
                    pnlAggregate.Height = (int)Math.Round(visualProfile.AggregatePaneHeight * Height);
                    pnlOrder.Height -= pnlAggregate.Height;
                    pnlAggregate.X = pnlOrder.X;
                    pnlAggregate.Y = pnlOrder.Height + pnlHeader.Height;

                    if (visualProfile.ButtonPaneVisible && visualProfile.ButtonPanePosition == ButtonPositionEnum.Top)
                    {
                        pnlAggregate.MoveY(pnlButton.Height);
                    }
                    break;
            }
        }

        private void AdjustForHistoryPanel(KitchenDisplayVisualProfile visualProfile)
        {
            pnlHistory.Width = (int)Math.Round(visualProfile.HistoryPaneWidth * Width);
            pnlHistory.Height = pnlOrder.Height;
            pnlOrder.Width -= pnlHistory.Width;
            pnlHistory.Y = pnlOrder.Y;

            switch (visualProfile.HistoryPanePosition)
            {
                case HistoryPositionEnum.Right:
                    pnlHistory.X = pnlOrder.Width;
                    pnlHistory.Y = pnlOrder.Y;

                    if(visualProfile.AggregatePaneVisible && visualProfile.AggregatePanePosition == AggregatePositionEnum.Left)
                    {
                        pnlHistory.MoveX(pnlAggregate.Width);
                    }
                    if(visualProfile.ButtonPaneVisible && visualProfile.ButtonPanePosition == ButtonPositionEnum.Left)
                    {
                        pnlHistory.MoveX(pnlButton.Width);
                    }
                    break;
                case HistoryPositionEnum.Left:
                    pnlOrder.MoveX(pnlHistory.Width);

                    if(visualProfile.AggregatePaneVisible && visualProfile.AggregatePanePosition == AggregatePositionEnum.Left)
                    {
                        pnlHistory.MoveX(pnlAggregate.Width);
                    }
                    if(visualProfile.ButtonPaneVisible && visualProfile.ButtonPanePosition == ButtonPositionEnum.Left)
                    {
                        pnlHistory.MoveX(pnlButton.Width);
                    }
                    break;
            }
        }

        private void SetSinglePane(KitchenDisplayPanePreview pane, decimal paneX, decimal paneY, decimal paneWidth, decimal paneHeight, bool paneVisible, Color color)
        {
            pane.Location = new Point((int) Math.Round(paneX*Width), (int) Math.Round(paneY*Height));
            pane.Width = (int) Math.Round(paneWidth*Width);
            pane.Height = (int) Math.Round(paneHeight*Height);
            pane.Visible = paneVisible;
            pane.BackColor = color;
        }

        private void GetSinglePaneData(KitchenDisplayPanePreview pane, out decimal paneX, out decimal paneY, out decimal paneWidth, out decimal paneHeight)
        {
            paneX = (decimal) pane.Location.X/Width;
            paneY = (decimal) pane.Location.Y/Height;
            paneWidth = (decimal) pane.Width/Width;
            paneHeight = (decimal) pane.Height/Height;
        }

        public void PopulateCoordinates(KitchenDisplayVisualProfile visualProfile)
        {
            decimal orderPaneX;
            decimal orderPaneY;
            decimal orderPaneWidth;
            decimal orderPaneHeight;
            GetSinglePaneData(pnlOrder, out orderPaneX, out orderPaneY, out orderPaneWidth, out orderPaneHeight);
            visualProfile.OrderPaneX = orderPaneX;
            visualProfile.OrderPaneY = orderPaneY;
            visualProfile.OrderPaneWidth = orderPaneWidth;
            visualProfile.OrderPaneHeight = orderPaneHeight;

            decimal headerPaneX;
            decimal headerPaneY;
            decimal headerPaneWidth;
            decimal headerPaneHeight;
            GetSinglePaneData(pnlHeader, out headerPaneX, out headerPaneY, out headerPaneWidth, out headerPaneHeight);
            visualProfile.HeaderPaneX = headerPaneX;
            visualProfile.HeaderPaneY = headerPaneY;
            visualProfile.HeaderPaneWidth = headerPaneWidth;
            visualProfile.HeaderPaneHeight = headerPaneHeight;

            decimal aggregatePaneX;
            decimal aggregatePaneY;
            decimal aggregatePaneWidth;
            decimal aggregatePaneHeight;
            GetSinglePaneData(pnlAggregate, out aggregatePaneX, out aggregatePaneY, out aggregatePaneWidth, out aggregatePaneHeight);
            visualProfile.AggregatePaneX = aggregatePaneX;
            visualProfile.AggregatePaneY = aggregatePaneY;
            visualProfile.AggregatePaneWidth = aggregatePaneWidth;
            visualProfile.AggregatePaneHeight = aggregatePaneHeight;

            decimal buttonPaneX;
            decimal buttonPaneY;
            decimal buttonPaneWidth;
            decimal buttonPaneHeight;
            GetSinglePaneData(pnlButton, out buttonPaneX, out buttonPaneY, out buttonPaneWidth, out buttonPaneHeight);
            visualProfile.ButtonPaneX = buttonPaneX;
            visualProfile.ButtonPaneY = buttonPaneY;
            visualProfile.ButtonPaneWidth = buttonPaneWidth;
            visualProfile.ButtonPaneHeight = buttonPaneHeight;

            decimal historyPaneX;
            decimal historyPaneY;
            decimal historyPaneWidth;
            decimal historyPaneHeight;
            GetSinglePaneData(pnlHistory, out historyPaneX, out historyPaneY, out historyPaneWidth, out historyPaneHeight);
            visualProfile.HistoryPaneX = historyPaneX;
            visualProfile.HistoryPaneY = historyPaneY;
            visualProfile.HistoryPaneWidth = historyPaneWidth;
            visualProfile.HistoryPaneHeight = historyPaneHeight;
        }
    }
}