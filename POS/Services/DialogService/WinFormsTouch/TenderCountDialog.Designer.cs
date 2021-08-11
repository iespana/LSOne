using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.WinFormsTouch
{
    partial class TenderCountDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TenderCountDialog));
            this.touchDialogBanner1 = new LSOne.Controls.TouchDialogBanner();
            this.pnlButtons = new LSOne.Controls.TouchScrollButtonPanel();
            this.touchNumPad = new LSOne.Controls.TouchNumPad();
            this.lvCurrency = new LSOne.Controls.ListView();
            this.clmCurrencyType = new LSOne.Controls.Columns.Column();
            this.clmCurrencyCode = new LSOne.Controls.Columns.Column();
            this.clmCurrencyAmount = new LSOne.Controls.Columns.Column();
            this.clmCurrencyValue = new LSOne.Controls.Columns.Column();
            this.clmCashDenomination = new LSOne.Controls.Columns.Column();
            this.clmCashQuantity = new LSOne.Controls.Columns.Column();
            this.clmCashAmount = new LSOne.Controls.Columns.Column();
            this.clmTender = new LSOne.Controls.Columns.Column();
            this.clmSubtotal = new LSOne.Controls.Columns.Column();
            this.lvSubtotal = new LSOne.Controls.ListView();
            this.lvCash = new LSOne.Controls.ListView();
            this.lvOther = new LSOne.Controls.ListView();
            this.clmOtherType = new LSOne.Controls.Columns.Column();
            this.clmAmount = new LSOne.Controls.Columns.Column();
            this.SuspendLayout();
            // 
            // touchDialogBanner1
            // 
            resources.ApplyResources(this.touchDialogBanner1, "touchDialogBanner1");
            this.touchDialogBanner1.BackColor = System.Drawing.Color.White;
            this.touchDialogBanner1.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.touchDialogBanner1.Name = "touchDialogBanner1";
            this.touchDialogBanner1.TabStop = false;
            // 
            // pnlButtons
            // 
            resources.ApplyResources(this.pnlButtons, "pnlButtons");
            this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
            this.pnlButtons.ButtonHeight = 50;
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Click += new LSOne.Controls.ScrollButtonEventHandler(this.pnlButtons_Click);
            // 
            // touchNumPad
            // 
            resources.ApplyResources(this.touchNumPad, "touchNumPad");
            this.touchNumPad.BackColor = System.Drawing.Color.Transparent;
            this.touchNumPad.KeystrokeMode = true;
            this.touchNumPad.MultiplyButtonIsZeroZero = true;
            this.touchNumPad.Name = "touchNumPad";
            this.touchNumPad.TabStop = false;
            this.touchNumPad.EnterPressed += new System.EventHandler(this.touchNumPad_EnterPressed);
            this.touchNumPad.ClearPressed += new System.EventHandler(this.touchNumPad_ClearPressed);
            // 
            // lvCurrency
            // 
            resources.ApplyResources(this.lvCurrency, "lvCurrency");
            this.lvCurrency.ApplyVisualStyles = false;
            this.lvCurrency.BackColor = System.Drawing.Color.White;
            this.lvCurrency.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvCurrency.BuddyControl = null;
            this.lvCurrency.Columns.Add(this.clmCurrencyType);
            this.lvCurrency.Columns.Add(this.clmCurrencyCode);
            this.lvCurrency.Columns.Add(this.clmCurrencyAmount);
            this.lvCurrency.Columns.Add(this.clmCurrencyValue);
            this.lvCurrency.ContentBackColor = System.Drawing.Color.White;
            this.lvCurrency.DefaultRowHeight = ((short)(50));
            this.lvCurrency.EvenRowColor = System.Drawing.Color.White;
            this.lvCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvCurrency.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvCurrency.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvCurrency.HeaderHeight = ((short)(30));
            this.lvCurrency.HideHorizontalScrollbarWhenDisabled = true;
            this.lvCurrency.HideVerticalScrollbarWhenDisabled = true;
            this.lvCurrency.Name = "lvCurrency";
            this.lvCurrency.OddRowColor = System.Drawing.Color.White;
            this.lvCurrency.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvCurrency.RowLines = true;
            this.lvCurrency.SecondarySortColumn = ((short)(-1));
            this.lvCurrency.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvCurrency.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCurrency.SortSetting = "0:1";
            this.lvCurrency.TouchScroll = true;
            this.lvCurrency.UseFocusRectangle = false;
            this.lvCurrency.VerticalScrollbarValue = 0;
            this.lvCurrency.VerticalScrollbarYOffset = 0;
            this.lvCurrency.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvCurrency.SelectionChanged += new System.EventHandler(this.lvCurrency_SelectionChanged);
            this.lvCurrency.CellAction += new LSOne.Controls.CellActionDelegate(this.lvCurrency_CellAction);
            // 
            // clmCurrencyType
            // 
            this.clmCurrencyType.AutoSize = true;
            this.clmCurrencyType.Clickable = false;
            this.clmCurrencyType.DefaultStyle = null;
            resources.ApplyResources(this.clmCurrencyType, "clmCurrencyType");
            this.clmCurrencyType.MaximumWidth = ((short)(0));
            this.clmCurrencyType.MinimumWidth = ((short)(10));
            this.clmCurrencyType.RelativeSize = 30;
            this.clmCurrencyType.SecondarySortColumn = ((short)(-1));
            this.clmCurrencyType.Tag = null;
            this.clmCurrencyType.Width = ((short)(154));
            // 
            // clmCurrencyCode
            // 
            this.clmCurrencyCode.AutoSize = true;
            this.clmCurrencyCode.Clickable = false;
            this.clmCurrencyCode.DefaultStyle = null;
            resources.ApplyResources(this.clmCurrencyCode, "clmCurrencyCode");
            this.clmCurrencyCode.MaximumWidth = ((short)(0));
            this.clmCurrencyCode.MinimumWidth = ((short)(10));
            this.clmCurrencyCode.RelativeSize = 15;
            this.clmCurrencyCode.SecondarySortColumn = ((short)(-1));
            this.clmCurrencyCode.Tag = null;
            this.clmCurrencyCode.Width = ((short)(77));
            // 
            // clmCurrencyAmount
            // 
            this.clmCurrencyAmount.AutoSize = true;
            this.clmCurrencyAmount.Clickable = false;
            this.clmCurrencyAmount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmCurrencyAmount.DefaultStyle = null;
            resources.ApplyResources(this.clmCurrencyAmount, "clmCurrencyAmount");
            this.clmCurrencyAmount.MaximumWidth = ((short)(0));
            this.clmCurrencyAmount.MinimumWidth = ((short)(10));
            this.clmCurrencyAmount.RelativeSize = 30;
            this.clmCurrencyAmount.SecondarySortColumn = ((short)(-1));
            this.clmCurrencyAmount.Tag = null;
            this.clmCurrencyAmount.Width = ((short)(154));
            // 
            // clmCurrencyValue
            // 
            this.clmCurrencyValue.AutoSize = true;
            this.clmCurrencyValue.Clickable = false;
            this.clmCurrencyValue.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmCurrencyValue.DefaultStyle = null;
            resources.ApplyResources(this.clmCurrencyValue, "clmCurrencyValue");
            this.clmCurrencyValue.MaximumWidth = ((short)(0));
            this.clmCurrencyValue.MinimumWidth = ((short)(10));
            this.clmCurrencyValue.RelativeSize = 25;
            this.clmCurrencyValue.SecondarySortColumn = ((short)(-1));
            this.clmCurrencyValue.Tag = null;
            this.clmCurrencyValue.Width = ((short)(129));
            // 
            // clmCashDenomination
            // 
            this.clmCashDenomination.AutoSize = true;
            this.clmCashDenomination.Clickable = false;
            this.clmCashDenomination.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmCashDenomination.DefaultStyle = null;
            resources.ApplyResources(this.clmCashDenomination, "clmCashDenomination");
            this.clmCashDenomination.MaximumWidth = ((short)(0));
            this.clmCashDenomination.MinimumWidth = ((short)(10));
            this.clmCashDenomination.RelativeSize = 34;
            this.clmCashDenomination.SecondarySortColumn = ((short)(-1));
            this.clmCashDenomination.Tag = null;
            this.clmCashDenomination.Width = ((short)(172));
            // 
            // clmCashQuantity
            // 
            this.clmCashQuantity.AutoSize = true;
            this.clmCashQuantity.Clickable = false;
            this.clmCashQuantity.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmCashQuantity.DefaultStyle = null;
            resources.ApplyResources(this.clmCashQuantity, "clmCashQuantity");
            this.clmCashQuantity.MaximumWidth = ((short)(0));
            this.clmCashQuantity.MinimumWidth = ((short)(10));
            this.clmCashQuantity.RelativeSize = 33;
            this.clmCashQuantity.SecondarySortColumn = ((short)(-1));
            this.clmCashQuantity.Tag = null;
            this.clmCashQuantity.Width = ((short)(171));
            // 
            // clmCashAmount
            // 
            this.clmCashAmount.AutoSize = true;
            this.clmCashAmount.Clickable = false;
            this.clmCashAmount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmCashAmount.DefaultStyle = null;
            resources.ApplyResources(this.clmCashAmount, "clmCashAmount");
            this.clmCashAmount.MaximumWidth = ((short)(0));
            this.clmCashAmount.MinimumWidth = ((short)(10));
            this.clmCashAmount.RelativeSize = 33;
            this.clmCashAmount.SecondarySortColumn = ((short)(-1));
            this.clmCashAmount.Tag = null;
            this.clmCashAmount.Width = ((short)(171));
            // 
            // clmTender
            // 
            this.clmTender.AutoSize = true;
            this.clmTender.Clickable = false;
            this.clmTender.DefaultStyle = null;
            resources.ApplyResources(this.clmTender, "clmTender");
            this.clmTender.MaximumWidth = ((short)(0));
            this.clmTender.MinimumWidth = ((short)(10));
            this.clmTender.SecondarySortColumn = ((short)(-1));
            this.clmTender.Tag = null;
            this.clmTender.Width = ((short)(135));
            // 
            // clmSubtotal
            // 
            this.clmSubtotal.AutoSize = true;
            this.clmSubtotal.Clickable = false;
            this.clmSubtotal.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmSubtotal.DefaultStyle = null;
            resources.ApplyResources(this.clmSubtotal, "clmSubtotal");
            this.clmSubtotal.MaximumWidth = ((short)(0));
            this.clmSubtotal.MinimumWidth = ((short)(10));
            this.clmSubtotal.SecondarySortColumn = ((short)(-1));
            this.clmSubtotal.Tag = null;
            this.clmSubtotal.Width = ((short)(175));
            // 
            // lvSubtotal
            // 
            resources.ApplyResources(this.lvSubtotal, "lvSubtotal");
            this.lvSubtotal.ApplyVisualStyles = false;
            this.lvSubtotal.BackColor = System.Drawing.Color.White;
            this.lvSubtotal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSubtotal.BuddyControl = null;
            this.lvSubtotal.Columns.Add(this.clmTender);
            this.lvSubtotal.Columns.Add(this.clmSubtotal);
            this.lvSubtotal.ContentBackColor = System.Drawing.Color.White;
            this.lvSubtotal.DefaultRowHeight = ((short)(50));
            this.lvSubtotal.EvenRowColor = System.Drawing.Color.White;
            this.lvSubtotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvSubtotal.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvSubtotal.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvSubtotal.HeaderHeight = ((short)(30));
            this.lvSubtotal.HideHorizontalScrollbarWhenDisabled = true;
            this.lvSubtotal.HideVerticalScrollbarWhenDisabled = true;
            this.lvSubtotal.Name = "lvSubtotal";
            this.lvSubtotal.OddRowColor = System.Drawing.Color.White;
            this.lvSubtotal.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSubtotal.RowLines = true;
            this.lvSubtotal.SecondarySortColumn = ((short)(-1));
            this.lvSubtotal.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvSubtotal.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSubtotal.SortSetting = "0:1";
            this.lvSubtotal.TouchScroll = true;
            this.lvSubtotal.UseFocusRectangle = false;
            this.lvSubtotal.VerticalScrollbarValue = 0;
            this.lvSubtotal.VerticalScrollbarYOffset = 0;
            this.lvSubtotal.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            // 
            // lvCash
            // 
            resources.ApplyResources(this.lvCash, "lvCash");
            this.lvCash.ApplyVisualStyles = false;
            this.lvCash.BackColor = System.Drawing.Color.White;
            this.lvCash.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvCash.BuddyControl = null;
            this.lvCash.Columns.Add(this.clmCashDenomination);
            this.lvCash.Columns.Add(this.clmCashQuantity);
            this.lvCash.Columns.Add(this.clmCashAmount);
            this.lvCash.ContentBackColor = System.Drawing.Color.White;
            this.lvCash.DefaultRowHeight = ((short)(50));
            this.lvCash.EvenRowColor = System.Drawing.Color.White;
            this.lvCash.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvCash.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvCash.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvCash.HeaderHeight = ((short)(30));
            this.lvCash.HideHorizontalScrollbarWhenDisabled = true;
            this.lvCash.HideVerticalScrollbarWhenDisabled = true;
            this.lvCash.Name = "lvCash";
            this.lvCash.OddRowColor = System.Drawing.Color.White;
            this.lvCash.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvCash.RowLines = true;
            this.lvCash.SecondarySortColumn = ((short)(-1));
            this.lvCash.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvCash.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvCash.SortSetting = "0:1";
            this.lvCash.TouchScroll = true;
            this.lvCash.UseFocusRectangle = false;
            this.lvCash.VerticalScrollbarValue = 0;
            this.lvCash.VerticalScrollbarYOffset = 0;
            this.lvCash.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvCash.SelectionChanged += new System.EventHandler(this.lvCash_SelectionChanged);
            this.lvCash.CellAction += new LSOne.Controls.CellActionDelegate(this.lvCash_CellAction);
            // 
            // lvOther
            // 
            resources.ApplyResources(this.lvOther, "lvOther");
            this.lvOther.ApplyVisualStyles = false;
            this.lvOther.BackColor = System.Drawing.Color.White;
            this.lvOther.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvOther.BuddyControl = null;
            this.lvOther.Columns.Add(this.clmOtherType);
            this.lvOther.Columns.Add(this.clmAmount);
            this.lvOther.ContentBackColor = System.Drawing.Color.White;
            this.lvOther.DefaultRowHeight = ((short)(50));
            this.lvOther.EvenRowColor = System.Drawing.Color.White;
            this.lvOther.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvOther.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvOther.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvOther.HeaderHeight = ((short)(30));
            this.lvOther.HideHorizontalScrollbarWhenDisabled = true;
            this.lvOther.HideVerticalScrollbarWhenDisabled = true;
            this.lvOther.Name = "lvOther";
            this.lvOther.OddRowColor = System.Drawing.Color.White;
            this.lvOther.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvOther.RowLines = true;
            this.lvOther.SecondarySortColumn = ((short)(-1));
            this.lvOther.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvOther.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvOther.SortSetting = "0:1";
            this.lvOther.TouchScroll = true;
            this.lvOther.UseFocusRectangle = false;
            this.lvOther.VerticalScrollbarValue = 0;
            this.lvOther.VerticalScrollbarYOffset = 0;
            this.lvOther.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvOther.SelectionChanged += new System.EventHandler(this.lvOther_SelectionChanged);
            this.lvOther.CellAction += new LSOne.Controls.CellActionDelegate(this.lvOther_CellAction);
            // 
            // clmOtherType
            // 
            this.clmOtherType.AutoSize = true;
            this.clmOtherType.Clickable = false;
            this.clmOtherType.DefaultStyle = null;
            resources.ApplyResources(this.clmOtherType, "clmOtherType");
            this.clmOtherType.MaximumWidth = ((short)(0));
            this.clmOtherType.MinimumWidth = ((short)(10));
            this.clmOtherType.RelativeSize = 30;
            this.clmOtherType.SecondarySortColumn = ((short)(-1));
            this.clmOtherType.Tag = null;
            this.clmOtherType.Width = ((short)(155));
            // 
            // clmAmount
            // 
            this.clmAmount.AutoSize = true;
            this.clmAmount.Clickable = false;
            this.clmAmount.DefaultHorizontalAlignment = LSOne.Controls.Columns.Column.HorizontalAlignmentEnum.Right;
            this.clmAmount.DefaultStyle = null;
            resources.ApplyResources(this.clmAmount, "clmAmount");
            this.clmAmount.MaximumWidth = ((short)(0));
            this.clmAmount.MinimumWidth = ((short)(10));
            this.clmAmount.RelativeSize = 70;
            this.clmAmount.SecondarySortColumn = ((short)(-1));
            this.clmAmount.Tag = null;
            this.clmAmount.Width = ((short)(360));
            // 
            // TenderCountDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvCash);
            this.Controls.Add(this.lvSubtotal);
            this.Controls.Add(this.touchNumPad);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.touchDialogBanner1);
            this.Controls.Add(this.lvCurrency);
            this.Controls.Add(this.lvOther);
            this.Name = "TenderCountDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.TenderCountDialog_Load);
            this.Shown += new System.EventHandler(this.TenderCountDialog_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private LSOne.Controls.TouchDialogBanner touchDialogBanner1;
        private LSOne.Controls.TouchScrollButtonPanel pnlButtons;
        private LSOne.Controls.TouchNumPad touchNumPad;
        private LSOne.Controls.ListView lvCurrency;
        private LSOne.Controls.Columns.Column clmTender;
        private LSOne.Controls.Columns.Column clmSubtotal;
        private LSOne.Controls.ListView lvSubtotal;
        private LSOne.Controls.Columns.Column clmCashDenomination;
        private LSOne.Controls.Columns.Column clmCashQuantity;
        private LSOne.Controls.Columns.Column clmCashAmount;
        private LSOne.Controls.ListView lvCash;
        private LSOne.Controls.ListView lvOther;
        private LSOne.Controls.Columns.Column clmOtherType;
        private LSOne.Controls.Columns.Column clmAmount;
        private LSOne.Controls.Columns.Column clmCurrencyType;
        private LSOne.Controls.Columns.Column clmCurrencyCode;
        private LSOne.Controls.Columns.Column clmCurrencyAmount;
        private LSOne.Controls.Columns.Column clmCurrencyValue;
    }
}