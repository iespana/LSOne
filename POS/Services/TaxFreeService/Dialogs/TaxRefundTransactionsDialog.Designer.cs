
using LSOne.Utilities.ColorPalette;

namespace LSOne.Services.Dialogs
{
    partial class TaxRefundTransactionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaxRefundTransactionsDialog));
            this.tkSelection = new LSOne.Controls.TouchKeyboard();
            this.tdbSelection = new LSOne.Controls.TouchDialogBanner();
            this.tbSearch = new LSOne.Controls.ShadeTextBoxTouch();
            this.lvSelection = new LSOne.Controls.ListView();
            this.colID = new LSOne.Controls.Columns.Column();
            this.colDate = new LSOne.Controls.Columns.Column();
            this.colAmount = new LSOne.Controls.Columns.Column();
            this.touchScrollButtonPanel1 = new LSOne.Controls.TouchScrollButtonPanel();
            this.SuspendLayout();
            // 
            // tkSelection
            // 
            resources.ApplyResources(this.tkSelection, "tkSelection");
            this.tkSelection.BackColor = System.Drawing.Color.Transparent;
            this.tkSelection.BuddyControl = null;
            this.tkSelection.KeystrokeMode = true;
            this.tkSelection.Name = "tkSelection";
            this.tkSelection.TabStop = false;
            this.tkSelection.ObtainCultureName += new LSOne.Controls.CultureNameHandler(this.touchKeyboard_ObtainCultureName);
            // 
            // tdbSelection
            // 
            this.tdbSelection.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.tdbSelection, "tdbSelection");
            this.tdbSelection.DialogType = LSOne.Controls.SupportClasses.MessageDialogType.Generic;
            this.tdbSelection.Name = "tdbSelection";
            this.tdbSelection.TabStop = false;
            // 
            // tbSearch
            // 
            resources.ApplyResources(this.tbSearch, "tbSearch");
            this.tbSearch.BackColor = System.Drawing.Color.White;
            this.tbSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.tbSearch.MaxLength = 32767;
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearch_KeyPress);
            // 
            // lvSelection
            // 
            resources.ApplyResources(this.lvSelection, "lvSelection");
            this.lvSelection.ApplyVisualStyles = false;
            this.lvSelection.BackColor = System.Drawing.Color.White;
            this.lvSelection.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSelection.BuddyControl = null;
            this.lvSelection.Columns.Add(this.colID);
            this.lvSelection.Columns.Add(this.colDate);
            this.lvSelection.Columns.Add(this.colAmount);
            this.lvSelection.ContentBackColor = System.Drawing.Color.White;
            this.lvSelection.DefaultRowHeight = ((short)(50));
            this.lvSelection.EvenRowColor = System.Drawing.Color.White;
            this.lvSelection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(60)))), ((int)(((byte)(64)))));
            this.lvSelection.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(236)))), ((int)(((byte)(237)))));
            this.lvSelection.HeaderFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lvSelection.HeaderHeight = ((short)(30));
            this.lvSelection.HideVerticalScrollbarWhenDisabled = true;
            this.lvSelection.Name = "lvSelection";
            this.lvSelection.OddRowColor = System.Drawing.Color.White;
            this.lvSelection.RowLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(214)))), ((int)(((byte)(217)))));
            this.lvSelection.RowLines = true;
            this.lvSelection.SecondarySortColumn = ((short)(-1));
            this.lvSelection.SelectedRowColor = ColorPalette.POSSelectedRowColor;
            this.lvSelection.SelectionStyle = LSOne.Controls.ListView.SelectionStyleEnum.GradientRounded;
            this.lvSelection.SortSetting = "0:1";
            this.lvSelection.TouchScroll = true;
            this.lvSelection.UseFocusRectangle = false;
            this.lvSelection.VerticalScrollbarValue = 0;
            this.lvSelection.VerticalScrollbarYOffset = 0;
            this.lvSelection.ViewStyle = LSOne.Controls.ListView.ListViewStyle.Touch;
            this.lvSelection.SelectionChanged += new System.EventHandler(this.lvSelection_SelectionChanged);
            // 
            // colID
            // 
            this.colID.AutoSize = true;
            this.colID.Clickable = false;
            this.colID.DefaultStyle = null;
            resources.ApplyResources(this.colID, "colID");
            this.colID.MaximumWidth = ((short)(0));
            this.colID.MinimumWidth = ((short)(10));
            this.colID.SecondarySortColumn = ((short)(-1));
            this.colID.Tag = null;
            this.colID.Width = ((short)(100));
            // 
            // colDate
            // 
            this.colDate.AutoSize = true;
            this.colDate.Clickable = false;
            this.colDate.DefaultStyle = null;
            resources.ApplyResources(this.colDate, "colDate");
            this.colDate.MaximumWidth = ((short)(0));
            this.colDate.MinimumWidth = ((short)(10));
            this.colDate.SecondarySortColumn = ((short)(-1));
            this.colDate.Tag = null;
            this.colDate.Width = ((short)(100));
            // 
            // colAmount
            // 
            this.colAmount.AutoSize = true;
            this.colAmount.Clickable = false;
            this.colAmount.DefaultStyle = null;
            resources.ApplyResources(this.colAmount, "colAmount");
            this.colAmount.MaximumWidth = ((short)(0));
            this.colAmount.MinimumWidth = ((short)(10));
            this.colAmount.SecondarySortColumn = ((short)(-1));
            this.colAmount.Tag = null;
            this.colAmount.Width = ((short)(400));
            // 
            // touchScrollButtonPanel1
            // 
            resources.ApplyResources(this.touchScrollButtonPanel1, "touchScrollButtonPanel1");
            this.touchScrollButtonPanel1.BackColor = System.Drawing.Color.White;
            this.touchScrollButtonPanel1.ButtonHeight = 50;
            this.touchScrollButtonPanel1.Name = "touchScrollButtonPanel1";
            this.touchScrollButtonPanel1.Click += new LSOne.Controls.ScrollButtonEventHandler(this.touchScrollButtonPanel1_Click);
            // 
            // TaxRefundTransactionsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.touchScrollButtonPanel1);
            this.Controls.Add(this.lvSelection);
            this.Controls.Add(this.tdbSelection);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.tkSelection);
            this.Name = "TaxRefundTransactionsDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.TouchKeyboard tkSelection;
        private Controls.TouchDialogBanner tdbSelection;
        private LSOne.Controls.ShadeTextBoxTouch tbSearch;
        private Controls.ListView lvSelection;
        private Controls.Columns.Column colID;
        private Controls.Columns.Column colDate;
        private Controls.Columns.Column colAmount;
        private Controls.TouchScrollButtonPanel touchScrollButtonPanel1;
    }
}